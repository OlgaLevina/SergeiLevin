using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SergeiLevin0.DAL.Context;
using SergeiLevin0.Domain.DTO.Identity;
using SergeiLevin0.Domain.Entities.Identity;

namespace SergeiLevin0.ServiceHosting.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/users")]
    [Produces("application/json")]//тип данных с которыми может работать контроллер: какие данные чистает и какие возвращает - явное указание. Можем и не указывать, т.к. базовый клиент у нас сконфигурирован для работы с джейсон 
    [ApiController]
    public class UsersAPIController : ControllerBase
    {
        private readonly UserStore<User, Role, SergeiLevinContext> UserStore;
        public UsersAPIController(SergeiLevinContext db) => UserStore = new UserStore<User, Role,SergeiLevinContext>(db);
        #region Users
        [HttpGet("AllUsers")]//используется только для отработки работы системы, в рабочем проекте не нужен
        public async Task<IEnumerable<User>> GetAllUsersAsync() => await UserStore.Users.ToArrayAsync();

        [HttpPost("userId")]
        public async Task<string> GetUserIdAsync([FromBody]User user)=> await UserStore.GetUserIdAsync(user);

        [HttpPost("userName")]
        public async Task<string> GetUserNameAsync([FromBody]User user) => await UserStore.GetUserNameAsync(user);

        [HttpPost("userName/{name}")]
        public async Task SetUserNameAsync([FromBody]User user, string name)
        {
            await UserStore.SetUserNameAsync(user, name);
            await UserStore.UpdateAsync(user);
        }

        [HttpPost("normalUserName")]
        public async Task<string> GetNormalizedUserNameAsync([FromBody]User  user) => await UserStore.GetNormalizedUserNameAsync(user);

        [HttpPost("normalUserName/{name}")]
        public async Task SetNormalizedUserNameAsync([FromBody]User user, string name)
        {
            await UserStore.SetNormalizedUserNameAsync(user, name);
            await UserStore.UpdateAsync(user);
        }

        [HttpPost("user")]
        public async Task<bool> CreateAsync([FromBody]User user)
        {
            if(user is null) { throw new ArgumentNullException(nameof(user)); }//в итоговой из урочной версии исключено 
            return (await UserStore.CreateAsync(user)).Succeeded;
        }

        [HttpPut("user")]
        public async Task<bool> UpdateAsync([FromBody]User user) => (await UserStore.UpdateAsync(user)).Succeeded;

        [HttpPost("user/delete")]
        public async Task<bool> DeleteAsync([FromBody]User user) => (await UserStore.DeleteAsync(user)).Succeeded;

        [HttpGet("user/find/{id}")]
        public async Task<User> FindByIdAsync(string id) => await UserStore.FindByIdAsync(id);

        [HttpGet("user/normal/{normalizedUserName}")]
        public async Task<User> FindByNameAsync(string normalizedUserName)=> await UserStore.FindByNameAsync(normalizedUserName);

        [HttpPost("role/{roleName}")]
        public async Task AddToRoleAsync([FromBody]User user, string roleName, [FromServices] SergeiLevinContext db)
        {
            await UserStore.AddToRoleAsync(user, roleName);
            await db.SaveChangesAsync();
        }

        [HttpPost("role/delete/{roleName}")]
        public async Task RemoveFromRoleAsync([FromBody]User user, string roleName,[FromServices] SergeiLevinContext db)
        {
            await UserStore.RemoveFromRoleAsync(user, roleName);
            db.SaveChangesAsync();
        }

        [HttpPost("roles")]
        public async Task<IList<string>> GetRolesAsync([FromBody]User user) => await UserStore.GetRolesAsync(user);

        [HttpPost("inrole/{roleName}")]
        public async Task<bool> IsInRoleAsync([FromBody]User user, string roleName) => await UserStore.IsInRoleAsync(user, roleName);

        [HttpGet("usersInRole/{roleName}")]
        public async Task<IList<User>> GetUsersInRoleAsync(string roleName) => await UserStore.GetUsersInRoleAsync(roleName);

        [HttpPost("getPasswordHash")]
        public async Task<string> GetPasswordHashAsync([FromBody]User user) => await UserStore.GetPasswordHashAsync(user);

        [HttpPost("setPasswordHash")]
        public async Task<string> SetPasswordHashAsync([FromBody]PasswordHashDTO hashDto)
        {
            await UserStore.SetPasswordHashAsync(hashDto.User, hashDto.Hash);
            await UserStore.UpdateAsync(hashDto.User);
            return hashDto.User.PasswordHash;
        }

        [HttpPost("hasPassword")]
        public async Task<bool> HasPasswordAsync([FromBody]User user) => await UserStore.HasPasswordAsync(user);
        #endregion

        #region Claims
        [HttpPost("getClaims")]
        public async Task<IList<Claim>> GetClaimsAsync([FromBody]User user) => await UserStore.GetClaimsAsync(user);

        [HttpPost("addClaims")]
        public async Task AddClaimsAsync([FromBody]AddClaimDTO claimsDto, [FromServices] SergeiLevinContext db)
        {
            await UserStore.AddClaimsAsync(claimsDto.User, claimsDto.Claims);
            await db.SaveChangesAsync();
        }

        [HttpPost("replaceClaim")]
        public async Task ReplaceClaimAsync([FromBody]ReplaceClaimDTO claimsDto, [FromServices] SergeiLevinContext db)
        {
            await UserStore.ReplaceClaimAsync(claimsDto.User, claimsDto.Claim, claimsDto.NewClaim);
            await db.SaveChangesAsync();
        }

        [HttpPost("removeClaims")]
        public async Task RemoveClaimsAsync([FromBody]RemoveClaimDTO claimsDto, [FromServices] SergeiLevinContext db)
        {
            await UserStore.RemoveClaimsAsync(claimsDto.User, claimsDto.Claims);
            await db.SaveChangesAsync();
        }

        [HttpPost("getUsersForClaim")]
        public async Task<IList<User>> GetUsersForClaimAsync([FromBody]Claim claim) => await UserStore.GetUsersForClaimAsync(claim);
        #endregion

        #region TwoFactor
        [HttpPost("setTwoFactor/{enabled}")]
        public async Task SetTwoFactorEnabledAsync([FromBody]User user, bool enabled)
        {
            await UserStore.SetTwoFactorEnabledAsync(user, enabled);
            await UserStore.UpdateAsync(user);
        }

        [HttpPost("getTwoFactorEnabled")]
        public async Task<bool> GetTwoFactorEnabledAsync([FromBody]User user) => await UserStore.GetTwoFactorEnabledAsync(user);
        #endregion

        #region Email/Phone
        [HttpPost("setEmail/{email}")]
        public async Task SetEmailAsync([FromBody]User user, string email)
        {
            await UserStore.SetEmailAsync(user, email);
            await UserStore.UpdateAsync(user);
        }

        [HttpPost("getEmail")]
        public async Task<string> GetEmailAsync([FromBody]User user) => await UserStore.GetEmailAsync(user);

        [HttpPost("getEmailConfirmed")]
        public async Task<bool> GetEmailConfirmedAsync([FromBody]User user) => await UserStore.GetEmailConfirmedAsync(user);

        [HttpPost("setEmailConfirmed/{confirmed}")]
        public async Task SetEmailConfirmedAsync([FromBody]User user, bool confirmed)
        {
            await UserStore.SetEmailConfirmedAsync(user, confirmed);
            await UserStore.UpdateAsync(user);
        }

        [HttpGet("user/findByEmail/{normalizedEmail}")]
        public async Task<User> FindByEmailAsync(string normalizedEmail) => await UserStore.FindByEmailAsync(normalizedEmail);

        [HttpPost("getNormalizedEmail")]
        public async Task<string> GetNormalizedEmailAsync([FromBody]User user) => await UserStore.GetNormalizedEmailAsync(user);

        [HttpPost("setEmail/{normalizedEmail?}")]
        public async Task SetNormalizedEmailAsync([FromBody]User user, string normalizedEmail)
        {
            await UserStore.SetNormalizedEmailAsync(user, normalizedEmail);
            await UserStore.UpdateAsync(user);
        }

        [HttpPost("setPhoneNumber/{phoneNumber}")]
        public async Task SetPhoneNumberAsync([FromBody]User user, string phoneNumber)
        {
            await UserStore.SetPhoneNumberAsync(user, phoneNumber);
            await UserStore.UpdateAsync(user);
        }

        [HttpPost("getPhoneNumber")]
        public async Task<string> GetPhoneNumberAsync([FromBody]User user) => await UserStore.GetPhoneNumberAsync(user);

        [HttpPost("getPhoneNumberConfirmed")]
        public async Task<bool> GetPhoneNumberConfirmedAsync([FromBody]User user) => await UserStore.GetPhoneNumberConfirmedAsync(user);

        [HttpPost("setPhoneNumberConfirmed/{confirmed}")]
        public async Task SetPhoneNumberConfirmedAsync([FromBody]User user, bool confirmed)
        {
            await UserStore.SetPhoneNumberConfirmedAsync(user, confirmed);
            await UserStore.UpdateAsync(user);
        }
        #endregion

        #region Login
        [HttpPost("addLogin")]
        public async Task AddLoginAsync([FromBody]AddLoginDTO loginDto, [FromServices] SergeiLevinContext db)
        {
            await UserStore.AddLoginAsync(loginDto.User, loginDto.UserLoginInfo);
            await db.SaveChangesAsync();
        }

        [HttpPost("removeLogin/{loginProvider}/{providerKey}")]
        public async Task RemoveLoginAsync([FromBody]User user, string loginProvider, string providerKey, [FromServices] SergeiLevinContext db)
        {
            await UserStore.RemoveLoginAsync(user, loginProvider, providerKey);
            await db.SaveChangesAsync();
        }

        [HttpPost("getLogins")]
        public async Task<IList<UserLoginInfo>> GetLoginsAsync([FromBody]User user) => await UserStore.GetLoginsAsync(user);

        [HttpGet("user/findbylogin/{loginProvider}/{providerKey}")]
        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey) => await UserStore.FindByLoginAsync(loginProvider, providerKey);

        [HttpPost("getLockoutEndDate")]
        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user) => await UserStore.GetLockoutEndDateAsync(user);

        [HttpPost("setLockoutEndDate")]
        public async Task SetLockoutEndDateAsync(SetLockoutDTO setLockoutDto)
        {
            await UserStore.SetLockoutEndDateAsync(setLockoutDto.User, setLockoutDto.LockoutEnd);
            await UserStore.UpdateAsync(setLockoutDto.User);
        }

        [HttpPost("IncrementAccessFailedCount")]
        public async Task<int> IncrementAccessFailedCountAsync(User user)
        {
            var count= await UserStore.IncrementAccessFailedCountAsync(user);
            await UserStore.UpdateAsync(user);
            return count;
        }

        [HttpPost("ResetAccessFailedCount")]
        public async Task ResetAccessFailedCountAsync(User user)
        {
            await UserStore.ResetAccessFailedCountAsync(user);
            await UserStore.UpdateAsync(user);
        }

        [HttpPost("GetAccessFailedCount")]
        public async Task<int> GetAccessFailedCountAsync(User user) => await UserStore.GetAccessFailedCountAsync(user);

        [HttpPost("GetLockoutEnabled")]
        public async Task<bool> GetLockoutEnabledAsync(User user) => await UserStore.GetLockoutEnabledAsync(user);

        [HttpPost("SetLockoutEnabled/{enabled}")]
        public async Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            await UserStore.SetLockoutEnabledAsync(user, enabled);
            await UserStore.UpdateAsync(user);
        }
        #endregion
    }
}