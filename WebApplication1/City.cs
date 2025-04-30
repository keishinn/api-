using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebApplication1
{
    [Table("city")]
    public class City : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("population")]
        public int Population { get; set; }

    }
}