using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using telstraGladosService.DataObjects;
using telstraGladosService.Models;
using Owin;

namespace telstraGladosService
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
            Database.SetInitializer(new telstraGladosInitializer());
            Database.SetInitializer(new ContactInitializer());
            Database.SetInitializer(new RecentContactInitializer());

            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            // Database.SetInitializer<telstraGladosContext>(null);

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

    public class telstraGladosInitializer : CreateDatabaseIfNotExists<telstraGladosContext>
    {
        protected override void Seed(telstraGladosContext context)
        {
            List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
            };

            foreach (TodoItem todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem);
            }

            base.Seed(context);
        }
    }

    public class ContactInitializer : CreateDatabaseIfNotExists<ContactContext>
    {
        protected override void Seed(ContactContext context)
        {
            Contact newContact = new Contact
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Rena",
                LastName = "Yu",
                Department = "Human Resource",
                Email = "yewlren.chong@connect.qut.edu.au",
                PhoneNumber = 0455845785
            };
            base.Seed(context);
        }
    }

    public class RecentContactInitializer : CreateDatabaseIfNotExists<RecentContactContext>
    {
        protected override void Seed(RecentContactContext context)
        {
            RecentContact testRecentContact = new RecentContact
            {
                Id = Guid.NewGuid().ToString(),
                RequesterFirstName = "Rena",
                RequesterLastName = "Yu",
                RespondentFirstName = "Yew Lren",
                RespondentLastName = "Chong",
                RequesterDepartment = "Information Technology",
                RespondentDepartment = "Marketing",
                RequestedAt = DateTime.Now,
                RequesterFullName = "Rena, Yu",
                RespondentFullName = "Yew Lren, Chong",
                RequestedAtString = DateTime.Now.ToString()
            };
            //context.Set<RecentContact>().Add(testRecentContact);
            base.Seed(context);
        }
    }

    public class FavouriteContactInitializer : CreateDatabaseIfNotExists<FavouriteContactContext>
    {
        protected override void Seed(FavouriteContactContext context)
        {
            FavouriteContact favContact = new FavouriteContact
            {
                Id = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                FirstName = "Rena",
                LastName = "Yu",
                Department = "Human Resource",
                Email = "yewlren.chong@connect.qut.edu.au",
                PhoneNumber = 0455845785
            };
            context.Set<FavouriteContact>().Add(favContact);
            base.Seed(context);
        }
    }
}

