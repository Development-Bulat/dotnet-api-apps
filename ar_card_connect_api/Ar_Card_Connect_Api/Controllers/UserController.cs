using Ar_Card_Connect_Api.Interfaces;
using Ar_Card_Connect_Api.JWT;
using Ar_Card_Connect_Api.Models;
using Ar_Card_Connect_Api.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Ar_Card_Connect_Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegUserAsync([FromBody] Reg reg)
    {
        return await _userService.RegUserAsync(reg);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginUserAsync([FromBody] Auth auth)
    {
        return await _userService.AuthUserAsync(auth);
    }
    
    [CustomAttributes.RoleAuthorize("Admin", "User")]
    [HttpPost]
    [Route("createCard")]
    public async Task<IActionResult> CreateCardAsync([FromForm] CreateCard card)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized(new { status = false, message = "Не авторизован" });
    
        var currentUserId = Guid.Parse(userIdClaim);
        return await _userService.CreateCardAsync(card, currentUserId);
    }

    [CustomAttributes.RoleAuthorize("Admin", "User")]
    [HttpPut]
    [Route("updateCard")]
    public async Task<IActionResult> UpdateCardAsync([FromForm] UpdateCard card)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized(new { status = false, message = "Не авторизован" });
    
        var currentUserId = Guid.Parse(userIdClaim);
        return await _userService.UpdateCardAsync(card, currentUserId);
    }

    [CustomAttributes.RoleAuthorize("Admin", "User")]
    [HttpPut]
    [Route("updateProfile")]
    public async Task<IActionResult> UpdateProfileAsync([FromBody] UpdateProfile profile)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { status = false, message = "Не авторизован" });
        }

        var currentUserId = Guid.Parse(userIdClaim);
        return await _userService.UpdateProfileAsync(profile, currentUserId);
    }

    [CustomAttributes.RoleAuthorize("Admin")]
    [HttpPost]
    [Route("createProfileAdmin")]
    public async Task<IActionResult> CreateProfileAdminAsync(CreateProfileAdmin profile)
    {
        return await _userService.CreateProfileAdminAsync(profile);
    }
    
    [CustomAttributes.RoleAuthorize("Admin")]
    [HttpDelete]
    [Route("deleteProfile")]
    public async Task<IActionResult> DeleteProfileAsync(Guid profileId)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized(new { status = false, message = "Не авторизован" });
    
        var currentAdminId = Guid.Parse(userIdClaim);
        return await _userService.DeleteProfileAsync(profileId, currentAdminId);
    }
    
    [CustomAttributes.RoleAuthorize("Admin")]
    [HttpDelete]
    [Route("deleteCardAdmin")]
    public async  Task<IActionResult> DeleteCardAdminAsync(Guid cardId)
    {
        return await _userService.DeleteCardAdminAsync(cardId);
    }

    [CustomAttributes.RoleAuthorize("Admin", "User")]
    [HttpDelete]
    [Route("deleteCard")]
    public async Task<IActionResult> DeleteCardAsync(Guid cardId)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized(new { status = false, message = "Не авторизован" });

        var currentUserId = Guid.Parse(userIdClaim);
        return await _userService.DeleteCardAsync(cardId, currentUserId);
    }

    [CustomAttributes.RoleAuthorize("Admin", "User")]
    [HttpGet]
    [Route("getMyProfile")]
    public async Task<IActionResult> GetMyProfileAsync()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
    
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { status = false, message = "Не авторизован" });
        }
    
        var currentUserId = Guid.Parse(userIdClaim);
        return await _userService.GetMyProfileAsync(currentUserId);
    }
    
    [CustomAttributes.RoleAuthorize("Admin")]
    [HttpGet]
    [Route("getAllUsers")]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        return await _userService.GetAllUsersAsync();
    }

    [CustomAttributes.RoleAuthorize("Admin", "User")]
    [HttpGet]
    [Route("getMyCards")]
    public async Task<IActionResult> GetMyCardsAsync()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized(new { status = false, message = "Не авторизован" });
    
        var currentUserId = Guid.Parse(userIdClaim);
        return await _userService.GetMyCardsAsync(currentUserId);
    }
    
    [CustomAttributes.RoleAuthorize("Admin")]
    [HttpGet]
    [Route("getAllCards")]
    public async Task<IActionResult> GetAllCardsAsync()
    {
        return await _userService.GetAllCardsAsync();
    }

    [CustomAttributes.RoleAuthorize("Admin", "User")]
    [HttpPost]
    [Route("changePassword")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized(new { status = false, message = "Не авторизован" });
    
        var currentUserId = Guid.Parse(userIdClaim);
        return await _userService.ChangePasswordAsync(currentUserId, request.oldPassword, request.newPassword);
    }
    
    [CustomAttributes.RoleAuthorize("Admin")]
    [HttpPost]
    [Route("blockUser")]
    public async Task<IActionResult> BlockUserAsync(Guid userId)
    {
        return await _userService.BlockUserAsync(userId);
    }

    [CustomAttributes.RoleAuthorize("Admin")]
    [HttpPost]
    [Route("unblockUser")]
    public async Task<IActionResult> UnblockUserAsync(Guid userId)
    {
        return await _userService.UnblockUserAsync(userId);
    }

    [CustomAttributes.RoleAuthorize("Admin")]
    [HttpPost]
    [Route("blockCard")]
    public async Task<IActionResult> BlockCardAsync(Guid cardId)
    {
        return await _userService.BlockCardAsync(cardId);
    }

    [CustomAttributes.RoleAuthorize("Admin")]
    [HttpPost]
    [Route("unblockCard")]
    public async Task<IActionResult> UnblockCardAsync(Guid cardId)
    {
        return await _userService.UnblockCardAsync(cardId);
    }
    
    [HttpGet]
    [Route("getCardByMarkerId")]
    public async Task<IActionResult> GetCardByMarkerIdAsync(string markerId)
    {
        return await _userService.GetCardByMarkerIdAsync(markerId);
    }
    
}