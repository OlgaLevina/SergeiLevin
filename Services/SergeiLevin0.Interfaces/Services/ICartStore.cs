using SergeiLevin0.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.Interfaces.Services
{
    public interface ICartStore
    {
        Cart Cart { get; set; }
    }
}
