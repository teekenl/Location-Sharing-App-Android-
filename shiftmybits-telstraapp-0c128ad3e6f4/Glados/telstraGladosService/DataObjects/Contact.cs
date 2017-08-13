using Microsoft.Azure.Mobile.Server;

namespace telstraGladosService.DataObjects
{
    public class Contact : EntityData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
    }
}