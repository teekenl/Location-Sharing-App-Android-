using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using GladosNotificationService.DataObjects;
using GladosNotificationService.Models;
using Owin;

namespace GladosNotificationService
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686 
            config.EnableSystemDiagnosticsTracing();

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new NotificationTableInitializer());

            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            // Database.SetInitializer<GladosNotificationContext>(null);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                // This middleware is intended to be used locally for debugging. By default, HostName will
                // only have a value when running in an App Service application.
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }
            app.UseWebApi(config);
        }
    }

    public class NotificationTableInitializer : CreateDatabaseIfNotExists<NotificationItemContext>
    {
        protected override void Seed(NotificationItemContext context)
        {
            List<NotificationItem> NotificationsList = new List<NotificationItem>
            {
                new NotificationItem { Id = Guid.NewGuid().ToString(), SenderId = "First item", SenderMessage = "test", RespondentId = "test", MessageType = "request" },
                new NotificationItem { Id = Guid.NewGuid().ToString(), SenderId = "Second item", SenderMessage = "test2", RespondentId = "test2", MessageType = "respond" }
            };

            foreach (NotificationItem notification in NotificationsList)
            {
                context.Set<NotificationItem>().Add(notification);
            }

            base.Seed(context);
        }
    }
}

