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
    public class RecentContactController : TableController<RecentContact>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            RecentContactContext context = new RecentContactContext();
            DomainManager = new EntityDomainManager<RecentContact>(context, Request);
        }

        // GET tables/RecentContact
        public IQueryable<RecentContact> GetAllRecentContact()
        {
            return Query(); 
        }

        // GET tables/RecentContact/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<RecentContact> GetRecentContact(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/RecentContact/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<RecentContact> PatchRecentContact(string id, Delta<RecentContact> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/RecentContact
        public async Task<IHttpActionResult> PostRecentContact(RecentContact item)
        {
            RecentContact current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/RecentContact/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteRecentContact(string id)
        {
             return DeleteAsync(id);
        }
    }
}
