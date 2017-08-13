using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glados.Core.Models;
using Glados.Core.Interfaces;
namespace Glados.Core.Interfaces
{
    public interface INotification
    {
        Task<int> InsertTodo(NotificationItem todo);

    }
}
