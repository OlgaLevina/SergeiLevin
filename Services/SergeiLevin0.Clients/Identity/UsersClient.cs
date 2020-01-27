using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SergeiLevin0.Clients.Base;
using SergeiLevin0.Domain.DTO.Identity;
using SergeiLevin0.Domain.Entities.Identity;
using SergeiLevin0.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SergeiLevin0.Clients.Identity
{
    public class UsersClient:BaseClient, IUsersClient
    {
        public UsersClient(IConfiguration config): base(config, "api/users") { }

        #region IUserStore
        public async Task<string> GetUserIdAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/userId", user,cancel))
            .Content.ReadAsAsync<string>(cancel).ConfigureAwait(false);
        public async Task<string> GetUserNameAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/userName", user,cancel))
            .Content.ReadAsAsync<string>(cancel).ConfigureAwait(false);
        public async Task SetUserNameAsync(User user, string userName,  CancellationToken cancel)
        {
            user.UserName = userName;
            await PostAsync($"{ServiceAddress}/userName/{userName}", user,cancel);
        }
        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/normalUserName", user,cancel))
            .Content.ReadAsAsync<string>(cancel).ConfigureAwait(false);
        public async Task SetNormalizedUserNameAsync(User user, string normalizedName,  CancellationToken cancel)
        {
            user.NormalizedUserName = normalizedName;
            await PostAsync($"{ServiceAddress}/normalUserName/{normalizedName}", user,cancel);
        }
        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/user", user,cancel))
            .Content.ReadAsAsync<bool>(cancel) ? IdentityResult.Success : IdentityResult.Failed();
        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancel) => 
            await (await PutAsync($"{ServiceAddress}/user", user,cancel))
                .Content.ReadAsAsync<bool>(cancel) ? IdentityResult.Success : IdentityResult.Failed();
        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/user/delete", user, cancel))
            .Content.ReadAsAsync<bool>(cancel) ? IdentityResult.Success : IdentityResult.Failed();
        public async Task<User> FindByIdAsync(string userId, CancellationToken cancel) => 
            await GetAsync<User>($"{ServiceAddress}/user/find/{userId}", cancel);
        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancel) => 
            await GetAsync<User>($"{ServiceAddress}/user/normal/{normalizedUserName}",cancel);
        #endregion
        #region IUserRoleStore
        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancel) => 
            await PostAsync($"{ServiceAddress}/role/{roleName}", user, cancel);
        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancel) => 
            await PostAsync($"{ServiceAddress}/role/delete/{roleName}", user, cancel);
        //????????????????????????????????????
        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancel) =>
            await (await PostAsync($"{ServiceAddress}/roles", user, cancel))
            .Content.ReadAsAsync<IList<string>>(cancel);//.ConfigureAwait(false);-почему здесь нет, а в других есть? Какого вообще назначение этого метода, когда он нужен?
        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/inrole/{roleName}", user, cancel))
            .Content.ReadAsAsync<bool>(cancel);//.ConfigureAwait(false);-???
        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancel) => 
            await GetAsync<List<User>>($"{ServiceAddress}/usersInRole/{roleName}", cancel);
        #endregion
        #region IUserPasswordStore
        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancel)
        {
            user.PasswordHash = passwordHash;
            await PostAsync($"{ServiceAddress}/setPasswordHash", 
                new PasswordHashDTO() { User = user,   Hash = passwordHash},
                cancel);
        }
        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/getPasswordHash", user, cancel))
                .Content.ReadAsAsync<string>(cancel);
        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/hasPassword", user, cancel))
                .Content.ReadAsAsync<bool>(cancel);
        #endregion
        #region IUserClaimStore

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/getClaims", user, cancel))
                .Content.ReadAsAsync<List<Claim>>(cancel);
        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel) => 
            await PostAsync($"{ServiceAddress}/addClaims", new AddClaimDTO() { User = user, Claims = claims }, cancel);
        public async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancel) => 
            await PostAsync($"{ServiceAddress}/replaceClaim", 
                new ReplaceClaimDTO(){ User = user, Claim = claim, NewClaim = newClaim },cancel);
        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel) => 
            await PostAsync($"{ServiceAddress}/removeClaims", 
                new RemoveClaimDTO() { User = user, Claims = claims }, cancel);
        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/getUsersForClaim", claim, cancel))
                .Content.ReadAsAsync<List<User>>(cancel);
        #endregion
        #region IUserTwoFactorStore
        public async Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancel)
        {
            user.TwoFactorEnabled = enabled;
            await PostAsync($"{ServiceAddress}/setTwoFactor/{enabled}", user,cancel);
        }
        public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/getTwoFactorEnabled", user, cancel))
                .Content.ReadAsAsync<bool>(cancel);
        #endregion
        #region IUserEmailStore
        public async Task SetEmailAsync(User user, string email, CancellationToken cancel)
        {
            user.Email = email;
            await PostAsync($"{ServiceAddress}/setEmail/{email}", user,cancel);
        }
        public async Task<string> GetEmailAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/getEmail", user, cancel))
                .Content.ReadAsAsync<string>(cancel);
        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/getEmailConfirmed", user, cancel))
                .Content.ReadAsAsync<bool>(cancel);
        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            user.EmailConfirmed = confirmed;
            await PostAsync($"{ServiceAddress}/setEmailConfirmed/{confirmed}", user,cancel);
        }
        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancel) => 
            await GetAsync<User>($"{ServiceAddress}/user/findByEmail/{normalizedEmail}", cancel);
        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/getNormalizedEmail", user, cancel))
                .Content.ReadAsAsync<string>(cancel);
        public async Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancel)
        {
            user.NormalizedEmail = normalizedEmail;
            await PostAsync($"{ServiceAddress}/setEmail/{normalizedEmail}", user,cancel);
        }
        #endregion
        #region IUserPhoneNumberStore
        public async Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancel)
        {
            user.PhoneNumber = phoneNumber;
            await PostAsync($"{ServiceAddress}/setPhoneNumber/{phoneNumber}", user,cancel);
        }
        public async Task<string> GetPhoneNumberAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/getPhoneNumber", user, cancel))
                .Content.ReadAsAsync<string>(cancel);
        public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/getPhoneNumberConfirmed", user, cancel))
                .Content.ReadAsAsync<bool>(cancel);
        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            user.PhoneNumberConfirmed = confirmed;
            await PostAsync($"{ServiceAddress}/setPhoneNumberConfirmed/{confirmed}", user,cancel);
        }
        #endregion
        #region IUserLoginStore
        public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancel) => 
            await PostAsync($"{ServiceAddress}/addLogin", 
                new AddLoginDTO() { User = user, UserLoginInfo = login },cancel);
        public async Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancel) => 
            await PostAsync($"{ServiceAddress}/removeLogin/{loginProvider}/{providerKey}", user, cancel);
        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/getLogins", user, cancel))
                .Content.ReadAsAsync<List<UserLoginInfo>>(cancel);
        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancel) => 
            await GetAsync<User>($"{ServiceAddress}/user/findbylogin/{loginProvider}/{providerKey}", cancel);
        #endregion
        #region IUserLockoutStore
        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/getLockoutEndDate", user, cancel))
                .Content.ReadAsAsync<DateTimeOffset?>(cancel);
        public async Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancel)
        {
            user.LockoutEnd = lockoutEnd;
            await PostAsync($"{ServiceAddress}/setLockoutEndDate", 
                new SetLockoutDTO() {  User = user, LockoutEnd = lockoutEnd},cancel);
        }
        public async Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/IncrementAccessFailedCount", user, cancel))
                .Content.ReadAsAsync<int>(cancel);
        public async Task ResetAccessFailedCountAsync(User user, CancellationToken cancel) => 
            await PostAsync($"{ServiceAddress}/ResetAccessFailedCount", user, cancel);
        public async Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/GetAccessFailedCount", user, cancel))
                .Content.ReadAsAsync<int>(cancel);
        public async Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancel) => 
            await (await PostAsync($"{ServiceAddress}/GetLockoutEnabled", user, cancel))
                .Content.ReadAsAsync<bool>(cancel);
        public async Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancel)
        {
            user.LockoutEnabled = enabled;
            await PostAsync($"{ServiceAddress}/SetLockoutEnabled/{enabled}", user,cancel);
        }
        #endregion
        //public void Dispose() - есть в методичке, почему нет в уроке
        //{
        //    Client.Dispose();
        //}

    }
}
