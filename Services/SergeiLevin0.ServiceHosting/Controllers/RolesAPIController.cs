using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SergeiLevin0.DAL.Context;
using SergeiLevin0.Domain.Entities.Identity;

namespace SergeiLevin0.ServiceHosting.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/roles")]
    [Produces("application/json")]
    [ApiController]
    public class RolesAPIController : ControllerBase
    {
        private readonly RoleStore<Role, SergeiLevinContext> RoleStore;
        public RolesAPIController(SergeiLevinContext db) => RoleStore = new RoleStore<Role,SergeiLevinContext>(db);

        [HttpGet("AllRoles")]//технический метод для проработки
        public async Task<IEnumerable<Role>> GetAllRolesAsync() => await RoleStore.Roles.ToArrayAsync();

        [HttpPost]
        public async Task<bool> CreateAsync(Role role) => (await RoleStore.CreateAsync(role)).Succeeded;

        [HttpPut]
        public async Task<bool> UpdateAsync(Role role) => (await RoleStore.UpdateAsync(role)).Succeeded;

        [HttpPost("delete")]
        public async Task<bool> DeleteAsync(Role role) => (await RoleStore.DeleteAsync(role)).Succeeded;

        [HttpPost("GetRoleId")]
        public async Task<string> GetRoleIdAsync(Role role) => await RoleStore.GetRoleIdAsync(role);

        [HttpPost("GetRoleName")]
        public async Task<string> GetRoleNameAsync(Role role) => await RoleStore.GetRoleNameAsync(role);

        [HttpPost("SetRoleName/{roleName}")]
        public async Task SetRoleNameAsync(Role role, string roleName)
        {
            await RoleStore.SetRoleNameAsync(role, roleName);
            await RoleStore.UpdateAsync(role);
        }

        [HttpPost("GetNormalizedRoleName")]
        public async Task<string> GetNormalizedRoleNameAsync(Role role) => await RoleStore.GetRoleNameAsync(role);

        [HttpPost("SetNormalizedRoleName/{normalizedName}")]
        public async Task SetNormalizedRoleNameAsync(Role role, string normalizedName)
        {
            await RoleStore.SetNormalizedRoleNameAsync(role, normalizedName);
            await RoleStore.UpdateAsync(role);
        }

        [HttpGet("FindById/{roleId}")]
        public async Task<Role> FindByIdAsync(string roleId) => await RoleStore.FindByIdAsync(roleId);

        [HttpGet("FindByName/{normalizedRoleName}")]
        public async Task<Role> FindByNameAsync(string normalizedRoleName) => await RoleStore.FindByNameAsync(normalizedRoleName);
    }
}