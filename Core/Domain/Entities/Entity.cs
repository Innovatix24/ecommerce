

using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Entity<TId>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TId Id { get; set; }
}