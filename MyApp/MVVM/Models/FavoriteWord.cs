using SQLite;

namespace MyApp.MVVM.Models
{
    [Table("favorite_word")]
    public class FavoriteWord
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("word")]
        public string Word { get; set; }
    }
}
