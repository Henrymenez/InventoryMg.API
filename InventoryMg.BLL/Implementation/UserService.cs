using AutoMapper;
using InventoryMg.BLL.DTOs.Request;
using InventoryMg.BLL.DTOs.Response;
using InventoryMg.BLL.Interfaces;
using InventoryMg.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace InventoryMg.BLL.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserProfile> _userManager;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public UserService(UserManager<UserProfile> userManager, IHttpContextAccessor httpContext, IMapper mapper)
        {
            _userManager = userManager;
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public async Task<UserProfileResponse> GetProfile()
        {
            var userId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userExist = await _userManager.FindByIdAsync(userId);

            if (userExist == null)
                throw new KeyNotFoundException($"User Id: {userId} does not exist");

            return _mapper.Map<UserProfileResponse>(userExist);
        }

        public async Task<(bool, string)> UpdateInformation(UpdateUser updateUser)
        {
            var userId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userExist = await _userManager.FindByIdAsync(userId);

            if (userExist == null)
                throw new KeyNotFoundException($"User Id: {userId} does not exist");



            userExist.FullName = updateUser.FullName;
            userExist.Phone = updateUser.Phone;
            userExist.Password = "Pass12345@";
            userExist.UserName = updateUser.UserName;
                
               
            var result = await _userManager.UpdateAsync(userExist);
         
            if (result.Succeeded)
            {
                return (true, "Profile Updated");
            }
            return (false, "Unable to update profile");


        }
    }
}
