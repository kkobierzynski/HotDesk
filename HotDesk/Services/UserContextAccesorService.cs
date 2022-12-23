using System.Security.Claims;

namespace HotDesk.Services
{
    public interface IUserContextAccesorService
    {
        ClaimsPrincipal User { get; }
        int? UserId { get; }
    }
    public class UserContextAccesorService : IUserContextAccesorService
    {
        private readonly IHttpContextAccessor _contextAccesor;
        public UserContextAccesorService(IHttpContextAccessor contextAccessor)
        {
            _contextAccesor = contextAccessor;
        }

        public ClaimsPrincipal User => _contextAccesor.HttpContext?.User;
        public int? UserId => User is null ? null : (int?)int.Parse(User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
    }
}
