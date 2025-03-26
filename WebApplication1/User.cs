using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace WebApplication1
{
    [Table("users")]
    public class User : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("age")]
        public string Age { get; set; }

    }
}



