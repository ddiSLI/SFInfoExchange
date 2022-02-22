using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFInfoExchange.Models
{
    public class SFAccount
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public string Client_ID__c { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string BillingStreet { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingPostalCode { get; set; }
        public string BillingCountry { get; set; }
        public string BillingEmail__c { get; set; }
        public string BillingComments__c { get; set; }
        public string ShippingStreet { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingPostalCode { get; set; }
        public string ShippingCountry { get; set; }
        public string Main_Email__c { get; set; }
        public string Comments__c { get; set; }
        public string APContactName__c { get; set; }
        public string Active__c { get; set; }
  
    }
}
