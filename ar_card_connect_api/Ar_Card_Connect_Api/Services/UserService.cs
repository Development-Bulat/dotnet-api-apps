using Ar_Card_Connect_Api.DataBase;
using Ar_Card_Connect_Api.Interfaces;
using Ar_Card_Connect_Api.JWT;
using Ar_Card_Connect_Api.Models;
using Ar_Card_Connect_Api.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Ar_Card_Connect_Api.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly JwtGenerator _jwtGenerator;
    
    public UserService(AppDbContext context, JwtGenerator jwtGenerator)
    {
        _context = context;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<IActionResult> RegUserAsync(Reg reg)
    {
        if (await _context.UserLogins.AnyAsync(l => l.email == reg.email))
        {
            return new BadRequestObjectResult(new { message = "Такой email уже существует!" });
        }
        var userProfile = new UserProfile
        {
            first_name = reg.first_name,
            last_name = reg.last_name,
            surname = reg.surname,
            role_id = Guid.Parse("22222222-2222-2222-2222-222222222222")
        };
        
        await _context.UserProfiles.AddAsync(userProfile);
        await _context.SaveChangesAsync(); 
        var newLogin = new UserLogin
        {
            user_id = userProfile.user_id,
            email = reg.email,
            password = reg.password,
            User = userProfile
        };
    
        await _context.UserLogins.AddAsync(newLogin);
        await _context.SaveChangesAsync();
        var token = _jwtGenerator.GenerateToken(userProfile);
        return new OkObjectResult(new { status = true, token = token });
    }

    public async Task<IActionResult> AuthUserAsync(Auth auth)
    {
        var loginEntry = await _context.UserLogins
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.email == auth.email && l.password == auth.password);
    
        if (loginEntry == null)
        {
            return new UnauthorizedObjectResult(new
            {
                status = false,
                message = "Неверный email или пароль"
            });
        }
        if (loginEntry.User != null && loginEntry.User.is_blocked)
        {
            return new UnauthorizedObjectResult(new
            {
                status = false,
                message = "Ваш аккаунт заблокирован. Обратитесь к администратору."
            });
        }
    
        var token = _jwtGenerator.GenerateToken(loginEntry.User!);
        return new OkObjectResult(new
        {
            status = true,
            token = token,
            data = new
            {
                userId = loginEntry.user_id,
                firstName = loginEntry.User!.first_name,
                lastName = loginEntry.User.last_name,
            }
        });
    }

public async Task<IActionResult> CreateCardAsync(CreateCard card, Guid currentUserId)
{
    var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.user_id == currentUserId);

    if (user == null)
    {
        return new NotFoundObjectResult(new { status = false, message = "Пользователь не найден" });
    }

    if (user.is_blocked)
        return new UnauthorizedObjectResult(new { status = false, message = "Доступ запрещен. Ваш аккаунт заблокирован." });
    List<string>? socialLinksList = null;
    if (!string.IsNullOrEmpty(card.social_links))
    {
        if (card.social_links.Trim().StartsWith("["))
        {
            try
            {
                socialLinksList = JsonSerializer.Deserialize<List<string>>(card.social_links);
            }
            catch
            {
                socialLinksList = new List<string> { card.social_links };
            }
        }
        else
        {
            socialLinksList = card.social_links
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToList();
        }
    }

    var newCard = new UserCard
    {
        userCard_id = Guid.NewGuid(),
        user_id = currentUserId,
        full_name = card.full_name,
        email = card.email,
        phone = card.phone,
        position = card.position,
        description = card.description,
        marker_id = Guid.NewGuid().ToString(),
        template = card.template ?? "gradient",
        created_at = DateTime.UtcNow,
        social_links = socialLinksList ?? new List<string>()
    };
    
    if (card.image_file != null && card.image_file.Length > 0)
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cards");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);
        
        var fileName = $"{Guid.NewGuid()}_{card.image_file.FileName}";
        var filePath = Path.Combine(uploadsFolder, fileName);
        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await card.image_file.CopyToAsync(stream);
        }
        newCard.image_url = $"/cards/{fileName}";
    }
    
    if (card.model_file != null && card.model_file.Length > 0)
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "models");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);
        
        var fileName = $"{Guid.NewGuid()}_{card.model_file.FileName}";
        var filePath = Path.Combine(uploadsFolder, fileName);
        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await card.model_file.CopyToAsync(stream);
        }
        newCard.model_url = $"/models/{fileName}";
    }

    await _context.UserCards.AddAsync(newCard);
    await _context.SaveChangesAsync();

    return new OkObjectResult(new { status = true, data = new { card = newCard.userCard_id, markerId = newCard.marker_id } });
}

public async Task<IActionResult> UpdateCardAsync(UpdateCard card, Guid currentUserId)
{
    var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.user_id == currentUserId);
    if (user == null || user.is_blocked)
        return new UnauthorizedObjectResult(new { status = false, message = "Доступ запрещен" });

    var existingCard = await _context.UserCards.FirstOrDefaultAsync(c => c.userCard_id == card.userCard_id && c.user_id == currentUserId);
    if (existingCard == null)
        return new NotFoundObjectResult(new
        {
            status = false,
            message = "Карточка не найдена или у вас нет прав на ее редактирование"
        });
    
    if (!string.IsNullOrEmpty(card.full_name))
        existingCard.full_name = card.full_name;
    
    if (!string.IsNullOrEmpty(card.phone))
        existingCard.phone = card.phone;
    
    if (!string.IsNullOrEmpty(card.email))
        existingCard.email = card.email;
    
    if (card.position != null)
        existingCard.position = card.position;
    
    if (card.description != null)
        existingCard.description = card.description;
    
    if (!string.IsNullOrEmpty(card.template))
        existingCard.template = card.template;
    
    if (!string.IsNullOrEmpty(card.social_links))
    {
        List<string>? socialLinksList = null;
        
        if (card.social_links.Trim().StartsWith("["))
        {
            try
            {
                socialLinksList = JsonSerializer.Deserialize<List<string>>(card.social_links);
            }
            catch
            {
                socialLinksList = new List<string> { card.social_links };
            }
        }
        else
        {
            socialLinksList = card.social_links
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToList();
        }
        
        existingCard.social_links = socialLinksList;
    }
    
    if (card.image_file != null && card.image_file.Length > 0)
    {
        if (!string.IsNullOrEmpty(existingCard.image_url))
        {
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingCard.image_url.TrimStart('/'));
            if (File.Exists(oldFilePath))
                File.Delete(oldFilePath);
        }

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cards");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);
        
        var fileName = $"{Guid.NewGuid()}_{card.image_file.FileName}";
        var filePath = Path.Combine(uploadsFolder, fileName);
        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await card.image_file.CopyToAsync(stream);
        }
        existingCard.image_url = $"/cards/{fileName}";
    }

    if (card.model_file != null && card.model_file.Length > 0)
    {
        if (!string.IsNullOrEmpty(existingCard.model_url))
        {
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingCard.model_url.TrimStart('/'));
            if (File.Exists(oldFilePath))
                File.Delete(oldFilePath);
        }

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "models");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);
        
        var fileName = $"{Guid.NewGuid()}_{card.model_file.FileName}";
        var filePath = Path.Combine(uploadsFolder, fileName);
        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await card.model_file.CopyToAsync(stream);
        }
        existingCard.model_url = $"/models/{fileName}";
    }

    try
    {
        await _context.SaveChangesAsync();
        return new OkObjectResult(new { status = true, message = "Визитка обновлена" });
    }
    catch (Exception e)
    {
        return new BadRequestObjectResult(new { status = false, message = "Ошибка при обновлении: " + e.Message });
    }
}

public async Task<IActionResult> UpdateProfileAsync(UpdateProfile profile, Guid currentUserId)
{
    var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.user_id == currentUserId);
    if (user == null)
        return new NotFoundObjectResult(new { status = false, message = "Профиль не найден" });
    if (user.is_blocked)
        return new UnauthorizedObjectResult(new { status = false, message = "Доступ запрещен. Ваш аккаунт заблокирован." });
    
    user.first_name = profile.first_name ?? user.first_name;
    user.last_name = profile.last_name ?? user.last_name;
    user.surname = profile.surname ?? user.surname;
    user.phone = profile.phone ?? user.phone;
    
    if (!string.IsNullOrEmpty(profile.avatar_url))
    {
        try
        {
            if (!string.IsNullOrEmpty(user.avatar_url))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.avatar_url.TrimStart('/'));
                if (File.Exists(oldFilePath))
                    File.Delete(oldFilePath);
            }
            var bytes = Convert.FromBase64String(profile.avatar_url);
            var fileName = $"{currentUserId}_{DateTime.Now.Ticks}.jpg";
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);
            var filePath = Path.Combine(uploadsFolder, fileName);
            await File.WriteAllBytesAsync(filePath, bytes);
            user.avatar_url = $"/avatars/{fileName}";
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(new { status = false, message = $"Ошибка сохранения аватара: {ex.Message}" });
        }
    }
    
    await _context.SaveChangesAsync();
    return new OkObjectResult(new { status = true, message = "Профиль обновлен" });
}

    public async Task<IActionResult> CreateProfileAdminAsync(CreateProfileAdmin profile)
    {
        if (await _context.UserLogins.AnyAsync(l => l.email == profile.email))
            return new BadRequestObjectResult(new { status = false, message = "Этот email уже занят" });
        var newLogin = new UserLogin
        {
            email = profile.email,
            password = profile.password,
            User = new UserProfile
            {
                first_name = profile.first_name,
                last_name = profile.last_name,
                surname = profile.surname,
                phone = profile.phone,
                role_id = profile.role_id,
                avatar_url = profile.avatar_url
            }
        };
        try
        {
            await _context.UserLogins.AddAsync(newLogin);
            await _context.SaveChangesAsync();
            return new OkObjectResult(new {status = true, message = "Пользователь успешно создан администратором" ,userId = newLogin.user_id});
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult(new {status = false, message = "Ошибка: " + e.Message});
        }
    }
    

    public async Task<IActionResult> DeleteCardAsync(Guid cardId, Guid currentUserId)
    {
        var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.user_id == currentUserId);
        if (user == null || user.is_blocked)
            return new UnauthorizedObjectResult(new { status = false, message = "Доступ запрещен" });
        var card = await _context.UserCards.FirstOrDefaultAsync(c =>
            c.userCard_id == cardId && c.user_id == currentUserId);
        if (card == null)
            return new NotFoundObjectResult(new
                { status = false, message = "Карточка не найдена или у вас нет прав на ее удаление" });
        _context.UserCards.Remove(card);
        await _context.SaveChangesAsync();
        return new OkObjectResult(new { status = true, message = "Карточка успешно удалена" });
    }

    public async Task<IActionResult> DeleteCardAdminAsync(Guid cardId)
    {
        var card = await _context.UserCards.FindAsync(cardId);
        if (card == null)
            return new NotFoundObjectResult(new { status = false, message = "Карточка не найдена" });
        _context.UserCards.Remove(card);
        await _context.SaveChangesAsync();
        return new OkObjectResult(new { status = true, message = "Карточка удалена администратором" });
    }

    public async Task<IActionResult> DeleteProfileAsync(Guid profileId, Guid currentAdminId)
    {
        if (profileId == currentAdminId)
        {
            return new BadRequestObjectResult(new { status = false, message = "Вы не можете удалить свой собственный аккаунт" });
        }
    
        var profile = await _context.UserProfiles.FindAsync(profileId);
        if (profile == null)
            return new NotFoundObjectResult(new { status = false, message = "Пользователь не найден" });
    
        _context.UserProfiles.Remove(profile);
        await _context.SaveChangesAsync();
        return new OkObjectResult(new { status = true, message = "Пользователь и все его данные удалены" });
    }

    public async Task<IActionResult> GetMyProfileAsync(Guid currentUserId)
    {
        var user = await _context.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.user_id == currentUserId);
    
        if (user == null)
            return new NotFoundObjectResult(new { status = false, message = "Профиль не найден" });
        if (user.is_blocked)
            return new UnauthorizedObjectResult(new { status = false, message = "Ваш аккаунт заблокирован. Обратитесь к администратору." });
        var userLogin = await _context.UserLogins
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.user_id == currentUserId);
        return new OkObjectResult(new 
        { 
            status = true, 
            data = new 
            {
                user.user_id,
                user.first_name,
                user.last_name,
                user.surname,
                user.phone,
                user.avatar_url,
                user.role_id,
                email = userLogin?.email ?? ""
            }
        });
    }

    public async Task<IActionResult> GetAllUsersAsync()
    {
        var users = await _context.UserProfiles
            .AsNoTracking()
            .Select(u => new
            {
                u.user_id,
                u.first_name,
                u.last_name,
                u.surname,
                u.phone,
                u.avatar_url,
                u.role_id,
                u.is_blocked,
                Email = _context.UserLogins
                    .Where(l => l.user_id == u.user_id)
                    .Select(l => l.email)
                    .FirstOrDefault()
            })
            .OrderBy(u => u.last_name)
            .ToListAsync();
    
        return new OkObjectResult(new { status = true, data = users });
    }

    public async Task<IActionResult> GetMyCardsAsync(Guid currentUserId)
    {
        var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.user_id == currentUserId);

        if (user == null || user.is_blocked)
            return new UnauthorizedObjectResult(new { status = false, message = "Доступ запрещен" });

        var cards = await _context.UserCards
            .AsNoTracking()
            .Where(c => c.user_id == currentUserId && !c.is_blocked)  
            .OrderByDescending(c => c.created_at)
            .ToListAsync();

        return new OkObjectResult(new { status = true, data = cards });
    }

    public async Task<IActionResult> GetAllCardsAsync()
    {
        var allCards = await _context.UserCards.AsNoTracking().ToListAsync();
        return new OkObjectResult(new {status = true, data = allCards});
    }
    
    public async Task<IActionResult> ChangePasswordAsync(Guid currentUserId, string oldPassword, string newPassword)
    {
        var user = await _context.UserProfiles
            .FirstOrDefaultAsync(u => u.user_id == currentUserId);
    
        if (user == null)
            return new NotFoundObjectResult(new { status = false, message = "Пользователь не найден" });
        if (user.is_blocked)
            return new UnauthorizedObjectResult(new { status = false, message = "Доступ запрещен. Ваш аккаунт заблокирован." });
        
        var userLogin = await _context.UserLogins
            .FirstOrDefaultAsync(l => l.user_id == currentUserId);
    
        if (userLogin == null)
            return new NotFoundObjectResult(new { status = false, message = "Данные для входа не найдены" });
        if (userLogin.password != oldPassword)
            return new BadRequestObjectResult(new { status = false, message = "Неверный текущий пароль" });
        userLogin.password = newPassword;
        await _context.SaveChangesAsync();
    
        return new OkObjectResult(new { status = true, message = "Пароль успешно изменен" });
    }
    
    public async Task<IActionResult> BlockUserAsync(Guid userId)
    {
        var user = await _context.UserProfiles.FindAsync(userId);
        if (user == null)
            return new NotFoundObjectResult(new { status = false, message = "Пользователь не найден" });
    
        user.is_blocked = true;
        await _context.SaveChangesAsync();
        return new OkObjectResult(new { status = true, message = "Пользователь заблокирован" });
    }
    
    public async Task<IActionResult> UnblockUserAsync(Guid userId)
    {
        var user = await _context.UserProfiles.FindAsync(userId);
        if (user == null)
            return new NotFoundObjectResult(new { status = false, message = "Пользователь не найден" });
    
        user.is_blocked = false;
        await _context.SaveChangesAsync();
        return new OkObjectResult(new { status = true, message = "Пользователь разблокирован" });
    }
    
    public async Task<IActionResult> BlockCardAsync(Guid cardId)
    {
        var card = await _context.UserCards.FindAsync(cardId);
        if (card == null)
            return new NotFoundObjectResult(new { status = false, message = "Карточка не найдена" });
    
        card.is_blocked = true;
        await _context.SaveChangesAsync();
        return new OkObjectResult(new { status = true, message = "Карточка заблокирована" });
    }
    
    public async Task<IActionResult> UnblockCardAsync(Guid cardId)
    {
        var card = await _context.UserCards.FindAsync(cardId);
        if (card == null)
            return new NotFoundObjectResult(new { status = false, message = "Карточка не найдена" });
    
        card.is_blocked = false;
        await _context.SaveChangesAsync();
        return new OkObjectResult(new { status = true, message = "Карточка разблокирована" });
    }
    
    public async Task<IActionResult> GetCardByMarkerIdAsync(string markerId)
    {
        var card = await _context.UserCards
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.marker_id == markerId && !c.is_blocked);
    
        if (card == null)
            return new NotFoundObjectResult(new { status = false, message = "Визитка не найдена" });
    
        return new OkObjectResult(new { status = true, data = card });
    }
}