using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using MvvmCross.Platform;
using Glados.Core.Interfaces;
using Glados.Core.Models;
using System.Diagnostics;

namespace Glados.Core.Database
{
    public class NotificationDatabaseAzure : INotification
    {

        private MobileServiceClient todoDatabase;
        private IMobileServiceSyncTable<NotificationItem> azureSyncTableTodo;

        public NotificationDatabaseAzure()
        {
            todoDatabase = Mvx.Resolve<IAzureDatabase>().GetMobileServiceClient2("notification");
            azureSyncTableTodo = todoDatabase.GetSyncTable<NotificationItem>();
        }


        private async Task SyncAsync(bool pullData = false)
        {
            try
            {
                await todoDatabase.SyncContext.PushAsync();

                if (pullData)
                {
                    await azureSyncTableTodo.PullAsync("allNotificationItems", azureSyncTableTodo.CreateQuery()); // query ID is used for incremental sync
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public async Task<int> InsertTodo(NotificationItem contact)
        {
            await SyncAsync(true);
            await azureSyncTableTodo.InsertAsync(contact);
            await SyncAsync();
            return 1;
        }


    }
}
