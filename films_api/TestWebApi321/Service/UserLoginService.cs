using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TestWebApi321.DatabaseContext;
using TestWebApi321.Hubs;
using TestWebApi321.Interfaces;
using TestWebApi321.Models;
using TestWebApi321.Requests;

namespace TestWebApi321.Service
{
    public class UserLoginService : IUsersService
    {
        private readonly ContextDb _context;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly string _uploadPath;

        public UserLoginService(ContextDb context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(_uploadPath)) Directory.CreateDirectory(_uploadPath);
        } 

        private async Task<bool> IsAdminAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.id_User == userId);
            return user != null && user.id_Role == (int)UserRoles.Admin;
        }

        public async Task<IActionResult> CreateNewUserAsync(CreateNewUser newUser, int currentUserId)
        {
            if(!await IsAdminAsync(currentUserId)) return new ForbidResult();

            var login = new Login()
            {
                User = new User()
                {
                    Description = newUser.Description,
                    Name = newUser.Name,
                    id_Role =  newUser.SelectedRoleId
                },
                Password = newUser.Password,
                Email = newUser.Email
            };

            await _context.AddAsync(login);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                status = true,
                data = new { user = new
                {
                    id_User = login.User.id_User,
                    name = login.User.Name,
                    email = login.Email,
                    description = login.User.Description,
                    id_Role = login.User.id_Role
                } },
            });
        }

        public async Task<IActionResult> GetAllUsersAsync(int currentUserId)
        {
            if (!await IsAdminAsync(currentUserId)) return new ForbidResult();
            
            var users = await _context.Logins
                .Include(l => l.User) 
                .Select(l => new {
                    id_User = l.User.id_User,
                    name = l.User.Name,
                    email = l.Email, 
                    description = l.User.Description,
                    id_Role = l.User.id_Role
                })
                .ToListAsync();

            return new OkObjectResult(new {
                data = new { users = users },
                status = true
            });
        }


        public async Task<IActionResult> UpdateUserAsync( int userId,UpdateUser updateUser, int currentUserId)
        {
            if (!await IsAdminAsync(currentUserId)) return new ForbidResult();
            var login = await _context.Logins.Include(l => l.User).FirstOrDefaultAsync(l => l.User_id == userId);
            if (login == null) return new NotFoundObjectResult(new { status = false, message = "Пользователь не найден" });
            if (login.Email != updateUser.Email)
            {
                bool emailBusy = await _context.Logins.AnyAsync(l => l.Email == updateUser.Email && l.User_id != userId);
                if (emailBusy) return new BadRequestObjectResult(new {status = false, message = "Этот email уже занят" });
                login.Email = updateUser.Email;
            }
            if (!string.IsNullOrWhiteSpace(updateUser.Password))
            {
                login.Password = updateUser.Password;
            }
            
            login.Email = updateUser.Email;
            login.User.Name = updateUser.Name;
            login.User.Description = updateUser.Description;
            login.User.id_Role = updateUser.id_Role;
            
            await _context.SaveChangesAsync();
            return new OkObjectResult(new {status = true, data = new { user = new
            {
                id_User = userId,
                updateUser.Name,
                updateUser.Description,
                updateUser.Email,
                updateUser.id_Role
            }} });
        }

        public async Task<IActionResult> DeleteUserAsync(int userId, int currentUserId)
        {
            if(!await IsAdminAsync(currentUserId)) return new ForbidResult();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.id_User == userId);
            if (user == null)
                return new NotFoundObjectResult(new { status = false, message = "Пользователь не найден" });
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return new OkObjectResult(new {status = true, message = "Пользователь успешно удален" });
        }

        public async Task<IActionResult> AuthUserAsync(AuthUser authUser)
        {
            var login = await _context.Logins.Include(l => l.User).FirstOrDefaultAsync((l => l.Email == authUser.Email && l.Password == authUser.Password));
            if (login == null)
                return new UnauthorizedObjectResult(new {status = false, message = "Неверный логин или пароль"});
            return new OkObjectResult(new {status = true, data = new
            {
                userId = login.User_id,
                roleId = login.User.id_Role
            }});
        }

        public async Task<IActionResult> RegUserAsync(RegNewUser regNewUser)
        {
            if (await _context.Logins.AnyAsync(l => l.Email == regNewUser.Email))
                return new BadRequestObjectResult(new { status = false, message = "email уже занят" });
            var login = new Login
            {
                Email = regNewUser.Email,
                Password = regNewUser.Password,
                User = new User
                {
                    Name =  regNewUser.Name,
                    Description =   regNewUser.Description,
                    id_Role = (int)UserRoles.User
                }
            };
            await  _context.Logins.AddAsync(login);
            await _context.SaveChangesAsync();
            return new OkObjectResult(new { status = true, message = "Регистрация успешна", data = new { userId = login.User.id_User, roleId = login.User.id_Role } });
        }

        public async Task<IActionResult> GetMyProfileAsync(int currentUserId)
        {
            var profile = await _context.Logins
                .Include(l => l.User) 
                .FirstOrDefaultAsync(l => l.User_id == currentUserId);

            if (profile == null) 
                return new NotFoundObjectResult(new { status = false, message = "Пользователь не найден" });

            return new OkObjectResult(new
            {
                status = true, 
                data = new
                {
                    name = profile.User.Name,
                    description = profile.User.Description,
                    email = profile.Email,
                    roleId = profile.User.id_Role 
                }
            });
        }

        public async Task<IActionResult> GetAllFilmsAsync()
        {
            var films = await  _context.Films.Select(f => new
            {
                f.id_Film,
                f.Name,
                f.Description,
                GenreName = f.Genre.Name,
                f.ReleaseDate,
                f.Rating,
                f.imageUrl
            })
                .ToListAsync();
            return new OkObjectResult(new
            {
                status = true,
                data = films
            });
        }

        public async Task<IActionResult> GetFilmByIdAsync(int filmId)
        {
            var film = await _context.Films.Where(f => f.id_Film == filmId).Select(f => new
                {
                    f.id_Film,
                    f.Name,
                    f.Description,
                    GenreName = f.Genre.Name,
                    f.id_Genre,
                    f.ReleaseDate,
                    f.Rating,
                    f.imageUrl
                })
                .FirstOrDefaultAsync();
            if (film == null)
                return new NotFoundObjectResult(new { status = false, message = "Фильм не найден" });
            return new OkObjectResult(new
                {
                    status = true,
                    data = film
                }
            );
        }

        public async Task<IActionResult> CreateFilmAsync(CreateEditFilm createEditFilm, int currentUserId)
        {
            if(!await IsAdminAsync(currentUserId)) return new ForbidResult();
            var film = new Film
            {
                Name = createEditFilm.Name,
                Description = createEditFilm.Description,
                id_Genre = createEditFilm.id_Genre,
                ReleaseDate = createEditFilm.ReleaseDate,
                Rating = createEditFilm.Rating,
                imageUrl = createEditFilm.imageUrl
            };
            await _context.Films.AddAsync(film);
            await _context.SaveChangesAsync();
            return new OkObjectResult(new
            {
                status = true,
                data = film
            });
        }

        public async Task<IActionResult> UpdateFilmAsync(int filmId, CreateEditFilm createEditFilm, int currentUserId)
        {
            if(!await IsAdminAsync(currentUserId)) return new ForbidResult();
            var film = await _context.Films.FindAsync(filmId);
            if (film == null)
            {
                return new NotFoundObjectResult(new { status = false, message = "Фильм не найден" });
            }
            film.Name = createEditFilm.Name;
            film.Description = createEditFilm.Description;
            film.id_Genre = createEditFilm.id_Genre;
            film.ReleaseDate = createEditFilm.ReleaseDate;
            film.Rating = createEditFilm.Rating;
            film.imageUrl = createEditFilm.imageUrl;
            await _context.SaveChangesAsync();
            return new OkObjectResult(new
                {
                    status = true,
                    data = film
                }
            );
        }

        public async Task<IActionResult> DeleteFilmAsync(int filmId, int currentUserId)
        {
            if(!await IsAdminAsync(currentUserId)) return new ForbidResult();
            var film = await _context.Films.FindAsync(filmId);
            if (film == null)
            {
                return new NotFoundObjectResult(new { status = false, message = "Фильм не найден" });
            }
            _context.Films.Remove(film);
            await _context.SaveChangesAsync();
            return new OkObjectResult(new
                {
                    status = true,
                    message = "Фильм удален"
                }
            );
        }

       public async Task<IActionResult> SendMessageAsync(SendMessage sendMessage, int currentUserId)
    {
    if (string.IsNullOrWhiteSpace(sendMessage.Content) && sendMessage.Image == null)
        return new BadRequestObjectResult(new { status = false, message = "Пустое сообщение" });

    string? imageUrl = null;
    if (sendMessage.Image != null)
    {
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(sendMessage.Image.FileName);
        using (var stream = new FileStream(Path.Combine(_uploadPath, fileName), FileMode.Create))
            await sendMessage.Image.CopyToAsync(stream);
        imageUrl = $"/uploads/{fileName}";
    }

    var message = new ChatMessage
    {
        Content = sendMessage.Content,
        ImageUrl = imageUrl,
        id_Sender = currentUserId,
        id_Film = sendMessage.id_Film,
        id_Recipient = sendMessage.id_Recipient,
        Timestamp = DateTime.UtcNow
    };

    await _context.ChatMessages.AddAsync(message);
    await _context.SaveChangesAsync();
    
    var info = await _context.Users
        .Where(u => u.id_User == currentUserId)
        .Select(u => new {
            SenderName = u.Name,
            FilmName = message.id_Film.HasValue ? _context.Films.Where(f => f.id_Film == message.id_Film).Select(f => f.Name).FirstOrDefault() : null,
            RecipientName = message.id_Recipient.HasValue ? _context.Users.Where(r => r.id_User == message.id_Recipient).Select(r => r.Name).FirstOrDefault() : null
        }).FirstOrDefaultAsync();

    var broadcastData = new {
        id_Message = message.id_Message,
        content = message.Content,
        imageUrl = message.ImageUrl,
        id_Sender = message.id_Sender,
        senderName = info?.SenderName,
        id_Film = message.id_Film,
        filmName = info?.FilmName,
        id_Recipient = message.id_Recipient,
        recipientName = info?.RecipientName,
        timestamp = message.Timestamp
    };
    
    string groupName = message.id_Film.HasValue 
        ? $"Film_{message.id_Film}" 
        : $"Private_{(currentUserId < message.id_Recipient ? $"{currentUserId}_{message.id_Recipient}" : $"{message.id_Recipient}_{currentUserId}")}";
    await _hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", broadcastData);
    await _hubContext.Clients.Group("Admins").SendAsync("ReceiveMessage", broadcastData);

    return new OkObjectResult(new { status = true, data = broadcastData });
    }

        public async Task<IActionResult> GetMessageAsync(int? filmId, int? recipientId, int currentUserId)
        {
            var query = _context.ChatMessages.AsQueryable();
            if (filmId.HasValue)
            {
                query = query.Where(m => m.id_Film == filmId);
            }
            else if (recipientId.HasValue)
            {
                query = query.Where(m => m.id_Film == null && 
                                         ((m.id_Sender == currentUserId && m.id_Recipient == recipientId) ||
                                          (m.id_Sender == recipientId && m.id_Recipient == currentUserId)));
            }

            var data = await query.OrderBy(m => m.Timestamp)
                .Select(m => new
                {
                    m.id_Message,
                    m.Content,
                    m.ImageUrl,
                    m.id_Sender,
                    m.id_Recipient,
                    m.id_Film,
                    m.Timestamp,
                    SenderName = m.Sender.Name
                })
                .ToListAsync();
            Console.WriteLine($"[DEBUG] Найдено строк в БД: {data.Count} для Me={currentUserId} и Him={recipientId}");
            return new OkObjectResult(new {status = true, data});
        }

       public async Task<IActionResult> DeleteMessageAsync(int messageId, int currentUserId)
{
    var message = await _context.ChatMessages.FindAsync(messageId);
    if (message == null)
        return new NotFoundObjectResult(new { status = false, message = "Сообщение не найдено" });
    
    if (message.id_Sender != currentUserId && !await IsAdminAsync(currentUserId))
        return new ForbidResult();

    _context.ChatMessages.Remove(message);
    await _context.SaveChangesAsync();

    return new OkObjectResult(new
    {
        status = true,
        message = "Сообщение удалено",
        data = new { 
            id_Message = messageId, 
            id_Film = message.id_Film, 
            id_Sender = message.id_Sender, 
            id_Recipient = message.id_Recipient 
        }
    });
}

public async Task<IActionResult> EditMessageAsync(EditMessage message, int currentUserId)
{
    var existingMessage = await _context.ChatMessages.FindAsync(message.id_Message);
    if (existingMessage == null)
        return new NotFoundObjectResult(new { status = false, message = "Сообщение не найдено" });

    if (existingMessage.id_Sender != currentUserId && !await IsAdminAsync(currentUserId))
        return new ForbidResult();

    existingMessage.Content = message.Content;

    if (message.Image != null)
    {
        if (!string.IsNullOrEmpty(existingMessage.ImageUrl))
        {
            var oldFileName = Path.GetFileName(existingMessage.ImageUrl);
            var oldFilePath = Path.Combine(_uploadPath, oldFileName);
            if (File.Exists(oldFilePath)) File.Delete(oldFilePath);
        }

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(message.Image.FileName);
        var filePath = Path.Combine(_uploadPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await message.Image.CopyToAsync(stream);
        }
        existingMessage.ImageUrl = $"/uploads/{fileName}";
    }

    await _context.SaveChangesAsync();

    return new OkObjectResult(new
    {
        status = true,
        data = new {
            id_Message = existingMessage.id_Message,
            content = existingMessage.Content,
            imageUrl = existingMessage.ImageUrl,
            id_Film = existingMessage.id_Film,
            id_Sender = existingMessage.id_Sender,
            id_Recipient = existingMessage.id_Recipient
        }
    });
}

public async Task<IActionResult> AdminGetAllMessagesAsync(int currentUserId)
{
    if (!await IsAdminAsync(currentUserId)) return new ForbidResult();

    var allMessages = await _context.ChatMessages
        .Include(m => m.Sender)
        .Include(m => m.Recipient)
        .Include(m => m.Film)
        .OrderByDescending(m => m.Timestamp)
        .Select(m => new
        {
            id_Message = m.id_Message,
            content = m.Content,
            imageUrl = m.ImageUrl,
            timestamp = m.Timestamp,
            senderName = m.Sender.Name,
            id_Sender = m.id_Sender, 
            recipientName = m.id_Recipient.HasValue ? m.Recipient.Name : null,
            id_Recipient = m.id_Recipient, 
            filmName = m.id_Film.HasValue ? m.Film.Name : null,
            id_Film = m.id_Film, 
            chatType = m.id_Film.HasValue ? "Film" : "Private"
        })
        .ToListAsync();

    return new OkObjectResult(new { status = true, data = allMessages });
}

    }
}
