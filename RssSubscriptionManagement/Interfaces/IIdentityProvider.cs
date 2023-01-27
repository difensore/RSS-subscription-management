using Microsoft.AspNetCore.Identity;

namespace RssSubscriptionManagement.Interfaces
{
    public interface IIdentityProvider
    {
        public Task<Dictionary<IdentityResult, IdentityUser>> CreateUserAsync(string Email, string Password);
    }
}
