using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Ar_Card_Connect_Api.DataBase; 
namespace Ar_Card_Connect_Api.JWT;

public class CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RoleAuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string[] _roleNames; 

        public RoleAuthorizeAttribute(params string[] roleNames) 
        {
            _roleNames = roleNames;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
    
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Result = new JsonResult(new { error = "Токен не передан или неверный формат" }) { StatusCode = 401 };
                return;
            }

            try
            {
                var token = authHeader.Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    context.Result = new JsonResult(new { error = "Некорректный токен" }) { StatusCode = 401 };
                    return;
                }

                var dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
                
                var userProfile = await dbContext.UserProfiles
                    .Include(u => u.Role)  
                    .FirstOrDefaultAsync(u => u.user_id.ToString() == userIdClaim);

                if (userProfile == null)
                {
                    context.Result = new JsonResult(new { error = "Пользователь не найден" }) { StatusCode = 403 };
                    return;
                }
                
                var roleName = userProfile.Role?.role_name;
                if (roleName == null || !_roleNames.Contains(roleName))
                {
                    context.Result = new JsonResult(new { error = "Доступ запрещен. Недостаточно прав." }) { StatusCode = 403 };
                    return;
                }
                
                context.HttpContext.Items["UserId"] = userIdClaim;
                await next();
            }
            catch (Exception ex)
            {
                context.Result = new JsonResult(new { error = "Ошибка валидации токена" }) { StatusCode = 401 };
            }
        }
    }
}