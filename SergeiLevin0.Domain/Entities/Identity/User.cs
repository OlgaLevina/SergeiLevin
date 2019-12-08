using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.Domain.Entities.Identity
{
    public class User:IdentityUser
    {
        public const string Administrator = "Administrator";
        public const string AdminPasswordDefault = "AdminPassword";
        public string Descriprion { get; set; }
    }
}
