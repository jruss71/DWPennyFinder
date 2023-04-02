using System.Collections.Generic;
using System.Threading.Tasks;
using DWPennyFinder.Models;
using SQLite;

namespace DWPennyFinder.Data
{
    public class ItemDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public ItemDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Item>().Wait();
        }

        public Task<List<Item>> GetItemsAsync()
        {
            return _database.Table<Item>().ToListAsync();
        }

        public Task<int> SaveItemAsync(Item item)
        {
            if (item.Id != 0)
            {
                return _database.UpdateAsync(item);
            }
            else
            {
                return _database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Item item)
        {
            return _database.DeleteAsync(item);
        }
    }
}
