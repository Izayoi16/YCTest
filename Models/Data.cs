using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YCTest.Models
{
    public class YC
    {
        public class Sales
        {
            [Key]
            public Guid Id { get; set; }

            [Required]
            [MaxLength(50)]
            public string? Name { get; set; }

            [Required]
            [MaxLength(10)]//僅限國內
            public string? PhoneNumber { get; set; }

            public virtual ICollection<House>? House { get; set; }

        }
        public class House
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            [MaxLength(255)]
            public string? Title { get; set; }

            [Required]
            [MaxLength(255)]
            public string? Local { get; set; }

            [Required]
            [MaxLength(50)]
            public string? Master { get; set; }

            [Required]
            [MaxLength(10)]//僅限國內
            public string? MasterPhone { get; set; }

            [Required]
            public Guid Sales { get; set; }

            [Required]
            public decimal Price { get; set; }

            [ForeignKey("Sales")]
            public virtual Sales? AttributionSales { get; set; }

        }
        public class YCDbContext : DbContext
        {
            public YCDbContext(DbContextOptions<YCDbContext> options) : base(options) { }
            public DbSet<Sales> Sales { get; set; }
            public DbSet<House> House { get; set; }
        }
    }
}
