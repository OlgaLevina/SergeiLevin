using Microsoft.AspNetCore.Identity;
using SergeiLevin0.Domain.Entities.Identity;

namespace SergeiLevin0.Interfaces.Services
{
    public interface IRolesClient: IRoleClaimStore<Role> { } //т.к. он наследует от одного стандартного, то можно было бы и не переопределять
}
