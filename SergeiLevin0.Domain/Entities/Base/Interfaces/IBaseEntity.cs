using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.Domain.Entities.Base.Interfaces
{
    /// <summary>
    /// сущность базовая
    /// </summary>
    public interface IBaseEntity
    {
        /// <summary>
        /// идентификатор
        /// </summary>
        int Id { get; set; }
    }
    /// <summary>
    /// именованая сущность
    /// </summary>
    public interface INamedEntity: IBaseEntity
    {
        /// <summary>
        /// имя
        /// </summary>
        string Name { get; set; }
    }
    /// <summary>
    /// упорядочиваемая сущность
    /// </summary>
    public interface IOrderedEntity: IBaseEntity
    {
        /// <summary>
        /// порядок
        /// </summary>
        int Order { get; set; }
    }
   
}
