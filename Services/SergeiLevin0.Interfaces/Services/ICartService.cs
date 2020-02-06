using SergeiLevin0.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Interfaces
{
    public interface ICartService
    {
        void AddToCart(int id);
        void DecrementFromCart(int id);
        void RemoveFromCart(int id);
        void RemoveAll();
        CartViewModel TransformFromCart();
    }
}
