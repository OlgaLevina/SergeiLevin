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
        private readonly RoleStore<IdentityRole> RoleStore;
        public RolesAPIController(SergeiLevinContext db) => RoleStore = new RoleStore<IdentityRole>(db);

        [HttpGet("AllRoles")]//технический метод для проработки
        public async Task<IEnumerable<Role>> GetAllRoles() => await RoleStore.Roles.ToArrayAsync();

        [HttpPost]
        public async Task<bool> CreateAsync(IdentityRole role) => (await RoleStore.CreateAsync(role)).Succeeded;

        [HttpPut]
        public async Task<bool> UpdateAsync(IdentityRole role) => (await RoleStore.UpdateAsync(role)).Succeeded;

        [HttpPost("delete")]
        public async Task<bool> DeleteAsync(IdentityRole role) => (await RoleStore.DeleteAsync(role)).Succeeded;

        [HttpPost("GetRoleId")]
        public async Task<string> GetRoleIdAsync(IdentityRole role) => await RoleStore.GetRoleIdAsync(role);

        [HttpPost("GetRoleName")]
        public async Task<string> GetRoleNameAsync(IdentityRole role) => await RoleStore.GetRoleNameAsync(role);

        [HttpPost("SetRoleName/{roleName}")]
        public Task SetRoleNameAsync(IdentityRole role, string roleName) => RoleStore.SetRoleNameAsync(role, roleName);

        [HttpPost("GetNormalizedRoleName")]
        public async Task<string> GetNormalizedRoleNameAsync(IdentityRole role) => await RoleStore.GetRoleNameAsync(role);

        [HttpPost("SetNormalizedRoleName/{normalizedName}")]
        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName) => RoleStore.SetNormalizedRoleNameAsync(role, normalizedName);

        [HttpGet("FindById/{roleId}")]
        public async Task<IdentityRole> FindByIdAsync(string roleId) => await RoleStore.FindByIdAsync(roleId);

        [HttpGet("FindByName/{normalizedRoleName}")]
        public async Task<IdentityRole> FindByNameAsync(string normalizedRoleName) => await RoleStore.FindByNameAsync(normalizedRoleName);
    }
}