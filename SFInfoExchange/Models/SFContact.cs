using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFInfoExchange.Models
{
    public class SFContact
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public string ClientId__c { get; set; }    //OrganizationID, AccountID
        public string StaffId__c { get; set; }
        public string AssistantName { get; set; }
        public string AssistantPhone { get; set; }
        public DateTime Birthdate { get; set; }
        public string OwnerId { get; set; }
        public string Department { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Homephone { get; set; }
        public string MailingStreet { get; set; }
        public string MailingCity { get; set; }
        public string MailingState { get; set; }
        public string MailingPostalCode { get; set; }
        public string MailingCountry { get; set; }
        public string MobilePhone { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string PracticeName__c { get; set; }
        public string Active__c { get; set; }

    }
}
