using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glados.Core.Interfaces
{
    public interface IAzureDatabase
    {
        MobileServiceClient GetMobileServiceClient(string database);

        // Table Service for push Notification
        MobileServiceClient GetMobileServiceClient2(string database);
    }
}
