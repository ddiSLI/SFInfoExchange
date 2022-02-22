using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFInfoExchange.Models
{
    public class SapAccount
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string AccountName { get; set; }
        public string ClinicName { get; set; }
        public string ClinicId { get; set; }    // DoctorId
        public string CreditHold { get; set; }  //Yes or No
        public string DoctorFirstName { get; set; }      // PracticeName__c 
        public string DoctorLastName { get; set; }
        public DateTime DoctorBirthdate { get; set; }
        public string ShippingRecipient { get; set; }
        public string ShippingStreet1 { get; set; }
        public string ShippingStreet2 { get; set; }
        public string ShippingStreet3 { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingPostalCode { get; set; }
        public string ShippingCountry { get; set; }
        public string PrimaryRecipient { get; set; }
        public string PrimaryStreet1 { get; set; }
        public string PrimaryStreet2 { get; set; }
        public string PrimaryStreet3 { get; set; }
        public string PrimaryCity { get; set; }
        public string PrimaryState { get; set; }
        public string PrimaryPostalCode { get; set; }
        public string PrimaryCountry { get; set; }
        public string BillingRecipient { get; set; }
        public string BillingStreet1 { get; set; }
        public string BillingStreet2 { get; set; }
        public string BillingStreet3 { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingPostalCode { get; set; }
        public string BillingCountry { get; set; }
        public string PrimaryPhone { get; set; }
        public string SecondaryPhone { get; set; }
        public string MobilePhone { get; set; }
        public string PrimaryFax { get; set; }
        public string SecondaryFax { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string NoInterp { get; set; }
        public string LabrixNoInterp { get; set; }
        public string SuppressName { get; set; }
        public string SendPdfThroughEmr { get; set; }
        public string Status { get; set; }      //"Active"
    }
}
