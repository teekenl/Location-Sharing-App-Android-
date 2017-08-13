using Glados.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glados.Core.Interfaces
{
    public interface IContactDatabase
    {
        Task<IEnumerable<Contact>> GetContacts();
        Task<IEnumerable<Contact>> GetContact(string name);

        Task<IEnumerable<Contact>> GetUser(string name);

        Task<int> DeleteContact(object id);
        Task<int> InsertContact(Contact contact);
        Task<bool> CheckIfExists(Contact contact);

        Task<IEnumerable<RecentContact>> GetRecentContacts();
        Task<int> DeleteRecentContact(object id);
        Task<int> InsertRecentContact(Contact requester, Contact respondent);

        Task<IEnumerable<FavouriteContact>> GetFavouriteContacts(string userId);
        Task<int> InsertFavouriteContact(string requesterId, Contact contact);
    }
}
