using SQLite; 

namespace MyDigitalKitchen.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _connection;
        private const string DatabaseFilename = "MyDigitalKitchen.db3";

        public SQLiteAsyncConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new SQLiteAsyncConnection(GetDatabasePath());
            }
            return _connection;
        }

        private string GetDatabasePath()
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(basePath, DatabaseFilename);
        }

        public async Task Init()
        {
            
            await GetConnection().CreateTableAsync<Models.Recipe>();
            
        }
    }
}