using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Domain.ViewModels
{
    public class CartViewModel
    {
        //!!!для упрощения жизни логику расчёта полной стоимости всей корзины перенести из представления сюда - во вью-модель
        public Dictionary<ProductViewModel, int> Items { get; set; } = new Dictionary<ProductViewModel, int>();
        public int ItemsCount => Items?.Sum(item => item.Value) ?? 0;
    }
}
