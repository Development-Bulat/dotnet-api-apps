using Microsoft.AspNetCore.Mvc;
using Npgsql.Replication.PgOutput.Messages;
using TestWebApi321.Interfaces;
using TestWebApi321.Requests;

namespace TestWebApi321.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
        private readonly IUsersService _userService;

        public UserController(IUsersService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("getAllUsers")]
        public async Task<IActionResult> GetAllUsers(int currentUserId)
        {
            return await _userService.GetAllUsersAsync(currentUserId);
        }

        [HttpPost]
        [Route("createNewUser")]
        public async Task<IActionResult> CreateNewUser(CreateNewUser newUser, int currentUserId)
        {
            return await _userService.CreateNewUserAsync(newUser, currentUserId);
        }

        [HttpPut]
        [Route("updateUser/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUser updateUser, int currentUserId)
        {
            return await _userService.UpdateUserAsync(userId, updateUser, currentUserId);
        }

        [HttpDelete]
        [Route("deleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId, int currentUserId)
        {
            return await _userService.DeleteUserAsync(userId, currentUserId);
        }

        
        [HttpGet]
        [Route("getMyProfile/{currentUserId}")]
        public async Task<IActionResult> GetMyProfile(int currentUserId)
        {
            return await _userService.GetMyProfileAsync(currentUserId);
        }


        [HttpPost]
        [Route("regNewUser")]
        public async Task<IActionResult> RegNewUser(RegNewUser regNewUser)
        {
            return await _userService.RegUserAsync(regNewUser);
        }
        
        [HttpPost]
        [Route("authUser")]
        public async Task<IActionResult> AuthUser(AuthUser authUser)
        {
            return await _userService.AuthUserAsync(authUser);
        }

        [HttpGet]
        [Route("getAllFilms")]
        public async Task<IActionResult> GetAllFilms()
        {
            return await _userService.GetAllFilmsAsync();
        }

        [HttpGet]
        [Route("getFilmById/{filmId}")]
        public async Task<IActionResult> GetFilmById(int filmId)
        {
            return await _userService.GetFilmByIdAsync(filmId);
        }

        [HttpDelete]
        [Route("deleteFilm/{filmId}")]
        public async Task<IActionResult> DeleteFilm(int filmId, int currentUserId)
        {
            return await _userService.DeleteFilmAsync(filmId, currentUserId);
        }

        [HttpPost]
        [Route("createFilm")]
        public async Task<IActionResult> CreateFilmAsync(CreateEditFilm createEditFilm, int currentUserId)
        {
            return await _userService.CreateFilmAsync(createEditFilm, currentUserId);
        }

        [HttpPut]
        [Route("updateFilm/{filmId}")]
        public async Task<IActionResult> UpdateFilm(int filmId, CreateEditFilm createEditFilm, int currentUserId)
        {
            return await _userService.UpdateFilmAsync(filmId, createEditFilm, currentUserId);
        }

        [HttpPost]
        [Route("sendMessage")]
        public async Task<IActionResult> SendMessageAsync([FromForm] SendMessage sendMessage, int currentUserId)
        {
            return await _userService.SendMessageAsync(sendMessage, currentUserId);
        }

        [HttpPut]
        [Route("updateMessage")]
        public async Task<IActionResult> UpdateMessageAsync([FromForm] EditMessage editMessage, int currentUserId)
        {
            return await _userService.EditMessageAsync(editMessage, currentUserId);
        }

        [HttpDelete]
        [Route("deleteMessage/{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId, int currentUserId)
        {
            return await _userService.DeleteMessageAsync(messageId, currentUserId);
        }

        [HttpGet]
        [Route("getMessage")]
        public async Task<IActionResult> GetMessage([FromQuery] int? filmId, [FromQuery] int? recipientId, [FromQuery] int currentUserId)
        {
            return await _userService.GetMessageAsync(filmId, recipientId, currentUserId);
        }

        [HttpGet]
        [Route("getAllMessages")]
        public async Task<IActionResult> GetAllMessages(int currentUserId)
        {
            return await _userService.AdminGetAllMessagesAsync(currentUserId);
        }
}
