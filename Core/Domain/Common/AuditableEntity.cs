
using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common;

public class AuditableEntity<TId> : Entity<TId>
{
    public short CreatedBy { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? UpdatedAt { get; set; }

}