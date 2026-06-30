using Ar_Card_Connect_Api.Models;
using Ar_Card_Connect_Api.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Ar_Card_Connect_Api.Interfaces;

public interface IUserService
{
    Task<IActionResult> RegUserAsync(Reg reg);
    Task<IActionResult> AuthUserAsync(Auth auth);
    Task<IActionResult> CreateCardAsync(CreateCard card, Guid currentUserId);
    Task<IActionResult> UpdateCardAsync(UpdateCard card, Guid currentUserId);
    Task<IActionResult> UpdateProfileAsync(UpdateProfile profile, Guid currentUserId);
    Task<IActionResult> CreateProfileAdminAsync(CreateProfileAdmin profile);
    Task<IActionResult> DeleteCardAsync(Guid cardId, Guid currentUserId);
    Task<IActionResult> DeleteProfileAsync(Guid profileId, Guid currentAdminId);
    Task<IActionResult> DeleteCardAdminAsync(Guid cardId);
    Task<IActionResult> GetMyProfileAsync(Guid currentUserId);
    Task<IActionResult> GetAllUsersAsync();
    Task<IActionResult> GetMyCardsAsync(Guid currentUserId);
    Task<IActionResult> GetAllCardsAsync();
    Task<IActionResult> ChangePasswordAsync(Guid currentUserId, string oldPassword, string newPassword);
    Task<IActionResult> BlockUserAsync(Guid userId);
    Task<IActionResult> UnblockUserAsync(Guid userId);
    Task<IActionResult> BlockCardAsync(Guid cardId);
    Task<IActionResult> UnblockCardAsync(Guid cardId);
    Task<IActionResult> GetCardByMarkerIdAsync(string markerId);

}