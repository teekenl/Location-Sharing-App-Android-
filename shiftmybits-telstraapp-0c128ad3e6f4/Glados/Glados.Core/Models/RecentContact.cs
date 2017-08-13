using System;

namespace Glados.Core.Models
{
    public class RecentContact
    {
        public string Id { get; set; }
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
