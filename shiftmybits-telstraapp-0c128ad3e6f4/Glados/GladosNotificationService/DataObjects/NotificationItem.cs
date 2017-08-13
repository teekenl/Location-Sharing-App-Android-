using Microsoft.Azure.Mobile.Server;

namespace GladosNotificationService.DataObjects
{
    public class NotificationItem : EntityData
    {
        public string SenderId { get; set; }
        public string SenderMessage { get; set; }
        public string RespondentId { get; set; }
        public string MessageType { get; set; }
    }
}