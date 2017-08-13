using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using telstraGladosService.DataObjects;
using telstraGladosService.Models;

namespace telstraGladosService.Controllers
{
    public class FavouriteContactController : TableController<FavouriteContact>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            FavouriteContactContext context = new FavouriteContactContext();
            DomainManager = new EntityDomainManager<FavouriteContact>(context, Request);
        }

        // GET tables/FavouriteContact
        public IQueryable<FavouriteContact> GetAllFavouriteContact()
        {
            return Query(); 
        }

        // GET tables/FavouriteContact/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<FavouriteContact> GetFavouriteContact(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/FavouriteContact/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<FavouriteContact> PatchFavouriteContact(string id, Delta<FavouriteContact> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/FavouriteContact
        public async Task<IHttpActionResult> PostFavouriteContact(FavouriteContact item)
        {
            FavouriteContact current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/FavouriteContact/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteFavouriteContact(string id)
        {
             return DeleteAsync(id);
        }
    }
}
