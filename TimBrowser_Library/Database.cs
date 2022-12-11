//using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimBrowser_Library
{
    public class Database
    {
        //private readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            //_database = new SQLiteAsyncConnection(dbPath);
            //_database.CreateTableAsync<LoginCredentialsModel>();
        }

        //public Task<List<LoginCredentialsModel>> GetLoginCredentials; /*() => _database.Table<LoginCredentialsModel>().ToListAsync();*/

        //public Task<int> SaveLoginCredentials(LoginCredentialsModel model) => _database.InsertAsync(model);
    }
}
