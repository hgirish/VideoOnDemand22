using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Common.Extensions;
using VOD.Database.Contexts;

namespace VOD.Database.Services
{
    public class UserService : IUserService
    {
        private readonly VODContext _db;
        private readonly UserManager<VODUser> _userManager;

        public UserService(VODContext db, UserManager<VODUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddUserAsync(RegisterUserDTO user)
        {
            var dbUser = new VODUser
            {
                UserName = user.Email,
                Email = user.Email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(dbUser, user.Password);
            return result;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                var dbUser = await _userManager.FindByIdAsync(userId);
                if (dbUser == null)
                {
                    return false;
                }

                var userRoles = await _userManager.GetRolesAsync(dbUser);
                var roleRemoved = await _userManager
                    .RemoveFromRolesAsync(dbUser, userRoles);

                var deleted = await _userManager.DeleteAsync(dbUser);
                return deleted.Succeeded;
            }
            catch 
            {
                return false;  
            }
        }

        public async Task<UserDTO> GetUserAsync(string userId)
        {
            return await (_db.Users
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsAdmin = _db.UserRoles.Any(ur =>
                    ur.UserId.Equals(user.Id) &&
                    ur.RoleId.Equals(1.ToString()))
                })
                ).FirstOrDefaultAsync(u => u.Id.Equals(userId));
        }

        public async Task<VODUser> GetUserAsync(LoginUserDTO loginUser, bool includeClaims = false)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginUser.Email);

                if (user == null)
                {
                    return null;
                }

                if (loginUser.Password.IsNullOrEmptyOrWhiteSpace() &&
                    loginUser.PasswordHash.IsNullOrEmptyOrWhiteSpace())
                {
                    return null;
                }

                if (loginUser.Password.Length > 0)
                {
                    var password = _userManager.PasswordHasher
                        .VerifyHashedPassword(user, user.PasswordHash, loginUser.Password);
                    if (password == PasswordVerificationResult.Failed)
                    {
                        return null;
                    }
                }
                else
                {
                    if (!user.PasswordHash.Equals(loginUser.PasswordHash))
                    {
                        return null;
                    }
                }
                if (includeClaims)
                {
                    user.Claims = await _userManager.GetClaimsAsync(user);
                }
                return user;
            }
            catch 
            {

                throw;
            }
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            return await (_db.Users
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsAdmin = _db.UserRoles.Any(
                        ur => ur.UserId.Equals(user.Id) &&
                        ur.RoleId.Equals(1.ToString())
                        )
                })
                )
                .FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            return await _db.Users
                .OrderBy(u => u.Email)
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsAdmin = _db.UserRoles.Any(ur =>
                    ur.UserId.Equals(user.Id) &&
                    ur.RoleId.Equals(1.ToString()))
                }).ToListAsync();
        }

        public async Task<bool> UpdateUserAsync(UserDTO user)
        {
            var dbUser = await _db.Users.FirstOrDefaultAsync(u =>
            u.Id.Equals(user.Id));
            if (dbUser == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                return false;
            }
            dbUser.Email = user.Email;
            var admin = "Admin";
            var isAdmin = await _userManager.IsInRoleAsync(dbUser, admin);
            if (isAdmin && !user.IsAdmin)
            {
                await _userManager.RemoveFromRoleAsync(dbUser, admin);
            }
            else if (!isAdmin && user.IsAdmin)
            {
                await _userManager.AddToRoleAsync(dbUser, admin);
            }
            var result = await _db.SaveChangesAsync();
            return result >= 0;
        }
    }
}
