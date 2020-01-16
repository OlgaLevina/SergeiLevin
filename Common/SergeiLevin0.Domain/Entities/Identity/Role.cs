using Microsoft.AspNetCore.Identity;

namespace SergeiLevin0.Domain.Entities.Identity
{
    public class Role: IdentityRole
    {
        public const string Administrator = "Administrator";
        public const string User = "User";
        public string Descriprion { get; set; }
    }
}
