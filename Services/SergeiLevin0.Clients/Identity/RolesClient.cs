using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SergeiLevin0.Clients.Base;
using SergeiLevin0.Domain.Entities.Identity;
using SergeiLevin0.Interfaces.Services;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SergeiLevin0.Clients.Identity
{
    public class RolesClient: BaseClient, IRolesClient
    {
        public RolesClient(IConfiguration config) : base(config, "api/roles") { }

        //public void Dispose()//-отсутствует в уроке
        //{
        //    Client.Dispose();
        //}
        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancel) => 
            await (await PostAsync(ServiceAddress, role, cancel))
                .Content.ReadAsAsync<bool>(cancel)
                ? IdentityResult.Success : IdentityResult.Failed();
        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancel) => 
            await (await PutAsync(ServiceAddress, role, cancel)) 
                .Content.ReadAsAsync<bool>(cancel)
                ? IdentityResult.Success : IdentityResult.Failed();
        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancel) => 
            await (await PostAsync($" {ServiceAddress} /delete", role, cancel))
                .Content.ReadAsAsync<bool>(cancel)
                ? IdentityResult.Success : IdentityResult.Failed();
        public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancel) => 
            await (await PostAsync($" {ServiceAddress} /GetRoleId", role, cancel))
                .Content.ReadAsAsync<string>(cancel);
        public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancel) => 
            await (await PostAsync($" {ServiceAddress} /GetRoleName", role, cancel))
                .Content.ReadAsAsync<string>(cancel);
        public async Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancel)
        {
            role.Name = roleName;
            await PostAsync($" {ServiceAddress} /SetRoleName/ {roleName} ", role,cancel);
        }
        public async Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancel) => 
            await (await PostAsync($" {ServiceAddress} /GetNormalizedRoleName", role, cancel))
                .Content.ReadAsAsync<string>(cancel);
        public async Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancel)
        {
            role.NormalizedName = normalizedName;
            await PostAsync($" {ServiceAddress} /SetNormalizedRoleName/ {normalizedName} ", role,cancel);
        }
        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancel) => 
            await GetAsync<Role>($" {ServiceAddress} /FindById/ {roleId} ", cancel);
        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancel) => 
            await GetAsync<Role>($" {ServiceAddress} /FindByName/ {normalizedRoleName}", cancel);
    }
}
