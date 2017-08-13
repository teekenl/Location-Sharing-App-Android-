using Glados.Core.Interfaces;
using Glados.Core.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using MvvmCross.Platform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Glados.Core.Database
{
    public class ContactDatabaseAzure : IContactDatabase
    {
        private MobileServiceClient contactDatabase;
        private MobileServiceClient recentContactDatabase;
        private MobileServiceClient favouriteContactDatabase;
        private IMobileServiceSyncTable<Contact> azureSyncTableContact;
        private IMobileServiceSyncTable<RecentContact> azureSyncTableRecentContact;
        private IMobileServiceSyncTable<FavouriteContact> azureSyncTableFavouriteContact;
        public ContactDatabaseAzure()
        {
            contactDatabase = Mvx.Resolve<IAzureDatabase>().GetMobileServiceClient("contact");
            recentContactDatabase = Mvx.Resolve<IAzureDatabase>().GetMobileServiceClient("recentContact");
            favouriteContactDatabase = Mvx.Resolve<IAzureDatabase>().GetMobileServiceClient("favouriteContact");
            azureSyncTableContact = contactDatabase.GetSyncTable<Contact>();
            azureSyncTableRecentContact = recentContactDatabase.GetSyncTable<RecentContact>();
            azureSyncTableFavouriteContact = favouriteContactDatabase.GetSyncTable<FavouriteContact>();
        }
        public async Task<IEnumerable<Contact>> GetContacts()
        {
            await SyncAsyncContact(true);
            var contacts = await azureSyncTableContact.ToListAsync();
            return contacts;
        }

        public async Task<IEnumerable<Contact>> GetContact(string respondentId)
        {
            await SyncAsyncContact(true);
            //string firstName = fullName.Split(',')[0];
            //string lastName = fullName.Split(',')[1].Trim();
            var contact = await azureSyncTableContact.Where(x => x.Id == respondentId).ToListAsync();
            return contact;
        }

        public async Task<IEnumerable<Contact>> GetUser(string searchTerm)
        {
            await SyncAsyncContact(true);
            string fullName = searchTerm.Split('-')[0].Trim();
            string firstName = fullName.Split(',')[0];
            string lastName = fullName.Split(',')[1].Trim();
            var contact = await azureSyncTableContact.Where(x => x.FirstName == firstName && x.LastName == lastName).ToListAsync();
            return contact;
        }


        public async Task<int> DeleteContact(object id)
        {
            await SyncAsyncContact(true);
            var contact = await azureSyncTableContact.Where(x => x.Id == (string)id).ToListAsync();
            if (contact.Any())
            {
                await azureSyncTableContact.DeleteAsync(contact.FirstOrDefault());
                await SyncAsyncContact();
                return 1;
            }
            else
            {
                return 0;

            }
        }

        public async Task<int> InsertContact(Contact contact)
        {
            await SyncAsyncContact(true);
            await azureSyncTableContact.InsertAsync(contact);
            await SyncAsyncContact();
            return 1;
        }

        public async Task<bool> CheckIfExists(Contact contact)
        {
            await SyncAsyncContact(true);
            var contacts = await azureSyncTableContact.Where(x => x.Email == contact.Email).ToListAsync();
            return contacts.Any();
        }

        private async Task SyncAsyncContact(bool pullData = false)
        {
            try
            {
                await contactDatabase.SyncContext.PushAsync();

                if (pullData)
                {
                    await azureSyncTableContact.PullAsync("allContacts", azureSyncTableContact.CreateQuery()); // query ID is used for incremental sync
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public async Task<IEnumerable<RecentContact>> GetRecentContacts()
        {
            await SyncAsyncRecentContact(true);
            var recentContacts = await azureSyncTableRecentContact.ToListAsync();
            return recentContacts;
        }

        public async Task<int> DeleteRecentContact(object id)
        {
            await SyncAsyncRecentContact(true);
            var recentContact = await azureSyncTableRecentContact.Where(x => x.Id == (string)id).ToListAsync();
            if (recentContact.Any())
            {
                await azureSyncTableRecentContact.DeleteAsync(recentContact.FirstOrDefault());
                await SyncAsyncRecentContact();
                return 1;
            }
            else
            {
                return 0;

            }
        }

        public async Task<int> InsertRecentContact(Contact requester, Contact respondent)
        {
            await SyncAsyncRecentContact(true);
            RecentContact recentContact = new RecentContact()
            {
                RequesterId = requester.Id,
                RequesterFirstName = requester.FirstName,
                RequesterLastName = requester.LastName,
                RequesterDepartment = requester.Department,
                RespondentId = respondent.Id,
                RespondentFirstName = respondent.FirstName,
                RespondentLastName = respondent.LastName,
                RespondentDepartment = respondent.Department,
                RequestedAt = DateTime.Now,
                RequesterFullName = requester.FirstName + ", " + requester.LastName,
                RespondentFullName = respondent.FirstName + ", " + respondent.LastName,
                RequestedAtString = DateTime.Now.ToString()
            };
            await azureSyncTableRecentContact.InsertAsync(recentContact);
            await SyncAsyncRecentContact();
            return 1;
        }

        private async Task SyncAsyncRecentContact(bool pullData = false)
        {
            try
            {
                await recentContactDatabase.SyncContext.PushAsync();

                if (pullData)
                {
                    await azureSyncTableRecentContact.PullAsync("allRecentContacts", azureSyncTableRecentContact.CreateQuery()); // query ID is used for incremental sync
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public async Task<IEnumerable<FavouriteContact>> GetFavouriteContacts(string userId)
        {
            await SyncAsyncFavouriteContact(true);
            var contacts = await azureSyncTableFavouriteContact.Where(x => x.UserId == userId).ToListAsync();
            return contacts;
        }

        public async Task<int> InsertFavouriteContact(string requesterId, Contact contact)
        {
            await SyncAsyncFavouriteContact(true);
            FavouriteContact favContact = new FavouriteContact()
            {
                UserId = requesterId,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                fullName = contact.FirstName + ", " + contact.LastName,
                Department = contact.Department,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
            };
            await azureSyncTableFavouriteContact.InsertAsync(favContact);
            await SyncAsyncFavouriteContact();
            return 1;
        }

        private async Task SyncAsyncFavouriteContact(bool pullData = false)
        {
            try
            {
                await favouriteContactDatabase.SyncContext.PushAsync();

                if (pullData)
                {
                    await azureSyncTableFavouriteContact.PullAsync("allFavouriteContacts", azureSyncTableFavouriteContact.CreateQuery()); // query ID is used for incremental sync
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
