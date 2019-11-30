using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.Domain.Entities.Identity
{
    public class Use:IdentityUser
    {
        public string Descriprion { get; set; }
    }
}
