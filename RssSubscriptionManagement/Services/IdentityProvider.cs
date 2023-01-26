using DAL.Models;
using Microsoft.AspNetCore.Identity;
using RssSubscriptionManagement.Interfaces;

namespace RssSubscriptionManagement.Services
{
    public class IdentityProvider:IIdentityProvider
    {        
        private readonly UserManager<IdentityUser> _userManager;
        public IdentityProvider(RssSubscriptionManagementContext context, UserManager<IdentityUser> userManager)
        {            
            _userManager = userManager;
        }
        public async Task<Dictionary<IdentityResult, IdentityUser>> CreateUserAsync(string Email, string Password)
        {
            IdentityUser user = new IdentityUser { Email = Email, UserName = Email };            
            var result = await _userManager.CreateAsync(user, Password);
            Dictionary<IdentityResult, IdentityUser> dictionary = new Dictionary<IdentityResult, IdentityUser>();
            dictionary.Add(result, user);
            return dictionary;
        }
    }
}
