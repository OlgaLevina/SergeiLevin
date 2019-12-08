using SergeiLevin0.Domain.Entities.Base;
using SergeiLevin0.Domain.Entities.Identity;
using System;
using System.Collections.Generic;

namespace SergeiLevin0.Domain.Entities
{
    public class Order: NamedEntity
    {
        public virtual User User { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
