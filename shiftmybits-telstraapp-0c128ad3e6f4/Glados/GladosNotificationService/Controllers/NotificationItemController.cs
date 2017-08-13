using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using GladosNotificationService.DataObjects;
using GladosNotificationService.Models;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.NotificationHubs;
using System.Collections.Generic;

namespace GladosNotificationService.Controllers
{
    public class NotificationItemController : TableController<NotificationItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            NotificationItemContext context = new NotificationItemContext();
            DomainManager = new EntityDomainManager<NotificationItem>(context, Request);
        }

        // GET tables/NotificationItem
        public IQueryable<NotificationItem> GetAllNotificationItem()
        {
            return Query(); 
        }

        // GET tables/NotificationItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<NotificationItem> GetNotificationItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/NotificationItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<NotificationItem> PatchNotificationItem(string id, Delta<NotificationItem> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/NotificationItem
        public async Task<IHttpActionResult> PostNotificationItem(NotificationItem item)
        {
            NotificationItem current = await InsertAsync(item);
            // Get the settings for the server project.
            HttpConfiguration config = this.Configuration;
            MobileAppSettingsDictionary settings =
                this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();

            // Get the Notification Hubs credentials for the Mobile App.
            string notificationHubName = settings.NotificationHubName;
            string notificationHubConnection = settings
                .Connections[MobileAppSettingsKeys.NotificationHubConnectionString].ConnectionString;

            // Create a new Notification Hub client.
            NotificationHubClient hub = NotificationHubClient
            .CreateClientFromConnectionString(notificationHubConnection, notificationHubName);

            // Sending the message so that all template registrations that contain "messageParam"
            // will receive the notifications. This includes APNS, GCM, WNS, and MPNS template registrations.
            Dictionary<string, string> templateParams = new Dictionary<string, string>();
            if (item.MessageType.Equals("request"))
            {
                templateParams["senderParam"] = item.SenderId;
                templateParams["messageParam"] = item.SenderId + " wants to know your status.";
                templateParams["receiveParam"] = item.RespondentId;
                templateParams["messageType"] = item.MessageType;
            }
            else
            {
                templateParams["senderParam"] = item.SenderId;
                templateParams["messageParam"] = " " + item.SenderMessage;
                templateParams["receiveParam"] = item.RespondentId;
                templateParams["messageType"] = item.MessageType;
            }

            try
            {
                // Send the push notification and log the results.
                var result = await hub.SendTemplateNotificationAsync(templateParams);

                // Write the success result to the logs.
                config.Services.GetTraceWriter().Info(result.State.ToString());
            }
            catch (System.Exception ex)
            {
                // Write the failure result to the logs.
                config.Services.GetTraceWriter()
                    .Error(ex.Message, null, "Push.SendAsync Error");
            }
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/NotificationItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteNotificationItem(string id)
        {
             return DeleteAsync(id);
        }
    }
}
