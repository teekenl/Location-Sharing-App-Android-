using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Glados.Core.Models
{
    public class NotificationItem
    {
        public string Id { get; set; }

        public string SenderId { get; set; }


        public string SenderMessage { get; set; }


        public string RespondentId { get; set; }

       
        public string MessageType { get; set; }
    }
}
