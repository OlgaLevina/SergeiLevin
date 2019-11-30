using Microsoft.AspNetCore.Identity;

namespace SergeiLevin0.Domain.Entities.Identity
{
    public class Role: IdentityRole
    {
        public string Descriprion { get; set; }
    }
}
