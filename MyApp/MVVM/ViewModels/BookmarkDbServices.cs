using MyApp.MVVM.Models;
using SQLite;

namespace MyApp.MVVM.ViewModels
{
    public class BookmarkDbServices
    {
        private const string DB_NAME = "bookmark_database.db3";
        private readonly SQLiteAsyncConnection _connection;

        public BookmarkDbServices()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<FavoriteWord>();
        }

        public async Task<int> GetRowCountAsync()
        {
            return await _connection.Table<FavoriteWord>().CountAsync();
        }

        public async Task<List<FavoriteWord>> GetWordsStartingWithAsync(string startingLetter)
        {
            startingLetter = startingLetter.ToUpper();

            return await _connection.Table<FavoriteWord>()
                                    .Where(x => x.Word.StartsWith(startingLetter))
                                    .OrderByDescending(x => x.Word)
                                    .ToListAsync();
        }
        public async Task<List<FavoriteWord>> GetFavoriteWords()
        {
            return await _connection.Table<FavoriteWord>().ToListAsync();
        }
        public async Task<List<FavoriteWord>> GetFavoriteWordsA2Z()
        {
            return await _connection.Table<FavoriteWord>()
                                    .OrderBy(x => x.Word)
                                    .ToListAsync();
        }
        public async Task<List<FavoriteWord>> GetFavoriteWordsZ2A()
        {
            return await _connection.Table<FavoriteWord>()
                                    .OrderByDescending(x => x.Word)
                                    .ToListAsync();
        }
        public async Task<FavoriteWord> GetById(int Id)
        {
            return await _connection.Table<FavoriteWord>().Where(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<FavoriteWord> GetByWord(string word)
        {
            return await _connection.Table<FavoriteWord>().Where(x => x.Word == word).FirstOrDefaultAsync();
        }

        public async Task Create(FavoriteWord favoriteWord)
        {
            await _connection.InsertAsync(favoriteWord);
        }

        public async Task Update(FavoriteWord favoriteWord)
        {
            await _connection.UpdateAsync(favoriteWord);
        }

        public async Task Delete(FavoriteWord favoriteWord)
        {
            await _connection.DeleteAsync(favoriteWord);
        }
        public async Task DeleteByWordAsync(string word)
        {
            var existingWord = await _connection.Table<FavoriteWord>().Where(x => x.Word == word).FirstOrDefaultAsync();
            if (existingWord != null)
            {
                await _connection.DeleteAsync(existingWord);
            }
        }
    }
}
