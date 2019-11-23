using SergeiLevin0.Domain.Entities.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.Domain.Entities.Base
{
    /// <summary>
    /// базовая сущность
    /// </summary>
    public abstract class BaseEntity : IBaseEntity
    {
        public int Id { get ; set ; }
    }
}
