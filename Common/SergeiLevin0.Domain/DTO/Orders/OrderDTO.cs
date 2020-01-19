using SergeiLevin0.Domain.Entities.Base;
using System;
using System.Collections.Generic;

namespace SergeiLevin0.Domain.DTO.Orders
{
    public class OrderDTO: NamedEntity
    {
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<OrderItemDTO> OrderItems { get; set; }
    }
}
