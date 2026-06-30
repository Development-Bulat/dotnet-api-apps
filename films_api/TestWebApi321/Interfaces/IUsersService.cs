using Microsoft.AspNetCore.Mvc;
using TestWebApi321.Models;
using TestWebApi321.Requests;

namespace TestWebApi321.Interfaces
{
    public interface IUsersService
    {
        Task<IActionResult> GetAllUsersAsync(int currentUserId);
        Task<IActionResult> CreateNewUserAsync(CreateNewUser newUser, int currentUserId);
        Task<IActionResult> UpdateUserAsync(int userId, UpdateUser updateUser, int currentUserId);
        Task<IActionResult> DeleteUserAsync(int userId, int currentUserId);
        Task<IActionResult> AuthUserAsync(AuthUser authUser);
        Task<IActionResult> RegUserAsync(RegNewUser regNewUser);
        Task<IActionResult> GetMyProfileAsync(int currentUserId);
        Task<IActionResult> CreateFilmAsync(CreateEditFilm createEditFilm, int currentUserId);
        Task<IActionResult> UpdateFilmAsync(int filmId, CreateEditFilm createEditFilm, int currentUserId);
        Task<IActionResult> DeleteFilmAsync(int filmId, int currentUserId);
        Task<IActionResult> GetAllFilmsAsync();
        Task<IActionResult> GetFilmByIdAsync(int filmId);
        Task<IActionResult> SendMessageAsync(SendMessage sendMessage, int currentUserId);
        Task<IActionResult> EditMessageAsync(EditMessage message, int currentUserId);
        Task<IActionResult> DeleteMessageAsync(int messageId, int currentUserId);
        Task<IActionResult> GetMessageAsync(int? filmId, int? recipientId, int currentUserId);
        Task<IActionResult> AdminGetAllMessagesAsync(int currentUserId);
    }
}
