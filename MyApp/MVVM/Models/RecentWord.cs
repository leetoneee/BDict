using SQLite;

namespace MyApp.MVVM.Models
{
    [Table("recent_word")]
    public class RecentWord
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("word")]
        public string Word { get; set; }
    }
}
