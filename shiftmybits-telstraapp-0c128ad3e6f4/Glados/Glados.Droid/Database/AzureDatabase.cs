using Glados.Core.Interfaces;
using Microsoft.WindowsAzure.MobileServices;
using System.IO;
using Glados.Core.Models;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;

namespace Glados.Droid.Database
{
    public class AzureDatabase : IAzureDatabase
    {
        MobileServiceClient azureDatabase;
        MobileServiceClient azureDatabase2;

        public MobileServiceClient GetMobileServiceClient(string database)
        {
            CurrentPlatform.Init();

            azureDatabase = new MobileServiceClient("http://telstraglados.azurewebsites.net/");
            InitializeLocal(database);
            return azureDatabase;
        }

        public MobileServiceClient GetMobileServiceClient2(string database)
        {
            CurrentPlatform.Init();

            azureDatabase2 = new MobileServiceClient("http://gladosnotification.azurewebsites.net");
            InitLocalStoreAsync2(database);
            return azureDatabase2;
        }

        // Initialize Async Table for Client
        private void InitLocalStoreAsync2(string database)
        {
            if (database.Equals("notification"))
            {
                var sqliteFilename = "NotificationSQLite.db3";
                // new code to initialize the SQLite store
                string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), sqliteFilename);

                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                }

                var store = new MobileServiceSQLiteStore(path);
                store.DefineTable<NotificationItem>();

                // Uses the default conflict handler, which fails on conflict
                // To use a different conflict handler, pass a parameter to InitializeAsync. 
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=521416
                azureDatabase2.SyncContext.InitializeAsync(store);
            }
        }

        private void InitializeLocal(string database)
        {
            if (database.Equals("contact"))
            {
                var sqliteFilename = "ContactSQLite.db3";
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
                var path = Path.Combine(documentsPath, sqliteFilename);
                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                }
                var store = new MobileServiceSQLiteStore(path);
                store.DefineTable<Contact>();
                azureDatabase.SyncContext.InitializeAsync(store);
            }
            else if (database.Equals("recentContact"))
            {
                var sqliteFilename = "recentContactSQLite.db3";
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
                var path = Path.Combine(documentsPath, sqliteFilename);
                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                }
                var store = new MobileServiceSQLiteStore(path);
                store.DefineTable<RecentContact>();
                azureDatabase.SyncContext.InitializeAsync(store);
            }
            else if (database.Equals("favouriteContact"))
            {
                var sqliteFilename = "favouriteContactSQLite.db3";
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
                var path = Path.Combine(documentsPath, sqliteFilename);
                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                }
                var store = new MobileServiceSQLiteStore(path);
                store.DefineTable<FavouriteContact>();
                azureDatabase.SyncContext.InitializeAsync(store);
            }
        }
    }
}