using Microsoft.Azure.Mobile.Server;
using System;

namespace telstraGladosService.DataObjects
{
    public class RecentContact : EntityData
    {
        public string RequesterId { get; set; }
        public string RespondentId { get; set; }
        public string RequesterFirstName { get; set; }
        public string RequesterLastName { get; set; }
        public string RespondentFirstName { get; set; }
        public string RespondentLastName { get; set; }
        public string RequesterDepartment { get; set; }
        public string RespondentDepartment { get; set; }
        public DateTime RequestedAt { get; set; }
        public string RequesterFullName { get; set; }
        public string RespondentFullName { get; set; }
        public string RequestedAtString { get; set; }
    }
}