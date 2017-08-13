using Microsoft.Azure.Mobile.Server;

namespace telstraGladosService.DataObjects
{
    public class FavouriteContact : EntityData
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string fullName { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
    }
}