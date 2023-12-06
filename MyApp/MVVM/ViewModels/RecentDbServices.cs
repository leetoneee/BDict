using MyApp.MVVM.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.MVVM.ViewModels
{
    public class RecentDbServices
    {
        private const string DB_NAME = "recent_database.db3";
        private const int MaxStorageLimit = 20;
        private readonly SQLiteAsyncConnection _connection;

        public RecentDbServices()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<RecentWord>();
        }

        //public async Task<List<RecentWord>> GetRecentWords()
        //{
        //    return await _connection.Table<RecentWord>().ToListAsync();
        //}

        //public async Task<RecentWord> GetById(int Id)
        //{
        //    return await _connection.Table<RecentWord>().Where(x => x.Id == Id).FirstOrDefaultAsync();
        //}

        //public async Task<RecentWord> GetByWord(string word)
        //{
        //    return await _connection.Table<RecentWord>().Where(x => x.Word == word).FirstOrDefaultAsync();
        //}

        //public async Task Create(RecentWord recentWord)
        //{
        //    await _connection.InsertAsync(recentWord);
        //}

        //public async Task Update(RecentWord recentWord)
        //{
        //    await _connection.UpdateAsync(recentWord);
        //}

        //public async Task Delete(RecentWord recentWord)
        //{
        //    await _connection.DeleteAsync(recentWord);
        //}
        //public async Task DeleteByWordAsync(string word)
        //{
        //    var existingWord = await _connection.Table<RecentWord>().Where(x => x.Word == word).FirstOrDefaultAsync();
        //    if (existingWord != null)
        //    {
        //        await _connection.DeleteAsync(existingWord);
        //    }
        //}

        public async Task<IEnumerable<RecentWord>> GetRecentWordsAsync()
        {
            return await _connection.Table<RecentWord>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<RecentWord> GetById(int wordId)
        {
            return await _connection.Table<RecentWord>().Where(x => x.Id == wordId).FirstOrDefaultAsync();
        }

        public async Task Create(RecentWord recentWord)
        {
            var existingWord = await _connection.Table<RecentWord>().Where(x => x.Word == recentWord.Word).FirstOrDefaultAsync();
            if (existingWord != null)
            {
                await _connection.DeleteAsync(existingWord);
            }
            var currentWordCount = await _connection.Table<RecentWord>().CountAsync();
            if (currentWordCount >= MaxStorageLimit)
            {
                var oldestWord = await _connection.Table<RecentWord>().OrderBy(x => x.Id).FirstOrDefaultAsync();
                if (oldestWord != null)
                {
                    await _connection.DeleteAsync(oldestWord);
                }
            }
            await _connection.InsertAsync(recentWord);
        }

        public async Task Update(RecentWord recentWord)
        {
            await _connection.UpdateAsync(recentWord);
        }
        public async Task Delete(RecentWord recentWord)
        {
            await _connection.DeleteAsync(recentWord);
        }
    }
}
