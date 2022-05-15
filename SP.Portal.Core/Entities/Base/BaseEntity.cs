using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SP.Portal.Core.Entities.Base
{
    public abstract class BaseEntity : IEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        [Key, Column("id")]
        public Guid Id { get; set; }
    }
}
