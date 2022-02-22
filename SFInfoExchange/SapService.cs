using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using SFInfoExchange.Models;


namespace SFInfoExchange
{
    class SapService : ISapService
    {
        //public List<SFContact> SFContactsWithoutAccount { get; set; }
        public string LogFile { get; set; }

        public List<SapAccount> SapAccounts = new List<SapAccount>();
        public List<SapContact> SapContacts = new List<SapContact>();

        public string GetSapDoctors(string previousModifiedTime = null, string lastModifiedTime = null)
        {
            //Sapphire.Doctor is orgInfo, provide info for SF.Account
            //List<SapAccount> sapAccounts = new List<SapAccount>();
            string resultGetSapDoctors = "NA";

            try
            {
                //SappireRestCliente
                var sapphireClient = new SapphireRestClient();
                DateTime datetimePreviousModified = Convert.ToDateTime("01/01/1900 12:00:00 AM");
                DateTime datetimeLastModified = DateTime.Now.AddMinutes((24 * 60) * -1);

                //testing
                //lastModifiedTime = "10/21/2021";
                //

                if (!string.IsNullOrEmpty(previousModifiedTime))
                {
                    datetimePreviousModified = Convert.ToDateTime(previousModifiedTime);
                }

                if (!string.IsNullOrEmpty(lastModifiedTime))
                {
                    datetimeLastModified = Convert.ToDateTime(lastModifiedTime);
                }

                var maxPages = 1;

                for (var i = 0; i < maxPages; i++)
                {
                    //Doctor is orgInfo, SF.Account
                    var doctors = sapphireClient.GetDoctor(i);
                    //var doctors = sapphireClient.GetDoctor(clientId: null, datetimePreviousModified, datetimeLastModified);

                    if (doctors == null)
                    {
                        break;
                    }

                    foreach (var dtr in doctors)
                    {
                        SapAccount sapAcc = new SapAccount();
                        sapAcc.ClientId = dtr.DoctorId == null ? "" : dtr.DoctorId;
                        sapAcc.AccountName = dtr.ClinicName == null ? "" : dtr.ClinicName;
                        sapAcc.ClinicName = dtr.ClinicName == null ? "" : dtr.ClinicName;

                        if (dtr.ShippingAddress != null)
                        {
                            sapAcc.ShippingRecipient = dtr.ShippingAddress.Recipient == null ? "" : dtr.ShippingAddress.Recipient;
                            sapAcc.ShippingStreet1 = dtr.ShippingAddress.StreetLine1 == null ? "" : dtr.ShippingAddress.StreetLine1;
                            sapAcc.ShippingStreet2 = dtr.ShippingAddress.StreetLine2 == null ? "" : dtr.ShippingAddress.StreetLine2;
                            sapAcc.ShippingStreet3 = dtr.ShippingAddress.StreetLine3 == null ? "" : dtr.ShippingAddress.StreetLine3;
                            sapAcc.ShippingCity = dtr.ShippingAddress.City == null ? "" : dtr.ShippingAddress.City;
                            sapAcc.ShippingState = dtr.ShippingAddress.State == null ? "" : dtr.ShippingAddress.State;
                            sapAcc.ShippingCountry = dtr.ShippingAddress.Country == null ? "" : dtr.ShippingAddress.Country;
                            sapAcc.ShippingPostalCode = dtr.ShippingAddress.PostalCode == null ? "" : dtr.ShippingAddress.PostalCode;
                        }
                        if (dtr.PrimaryAddress != null)
                        {
                            sapAcc.PrimaryRecipient = dtr.PrimaryAddress.Recipient == null ? "" : dtr.PrimaryAddress.Recipient;
                            sapAcc.PrimaryStreet1 = dtr.PrimaryAddress.StreetLine1 == null ? "" : dtr.PrimaryAddress.StreetLine1;
                            sapAcc.PrimaryStreet2 = dtr.PrimaryAddress.StreetLine2 == null ? "" : dtr.PrimaryAddress.StreetLine2;
                            sapAcc.PrimaryStreet3 = dtr.PrimaryAddress.StreetLine3 == null ? "" : dtr.PrimaryAddress.StreetLine3;
                            sapAcc.PrimaryCity = dtr.PrimaryAddress.City == null ? "" : dtr.PrimaryAddress.City;
                            sapAcc.PrimaryState = dtr.PrimaryAddress.State == null ? "" : dtr.PrimaryAddress.State;
                            sapAcc.PrimaryCountry = dtr.PrimaryAddress.Country == null ? "" : dtr.PrimaryAddress.Country;
                            sapAcc.PrimaryPostalCode = dtr.PrimaryAddress.PostalCode == null ? "" : dtr.PrimaryAddress.PostalCode;
                        }
                        if (dtr.BillingAddress != null)
                        {
                            sapAcc.BillingRecipient = dtr.BillingAddress.Recipient == null ? "" : dtr.BillingAddress.Recipient;
                            sapAcc.BillingStreet1 = dtr.BillingAddress.StreetLine1 == null ? "" : dtr.BillingAddress.StreetLine1;
                            sapAcc.BillingStreet2 = dtr.BillingAddress.StreetLine2 == null ? "" : dtr.BillingAddress.StreetLine2;
                            sapAcc.BillingStreet3 = dtr.BillingAddress.StreetLine3 == null ? "" : dtr.BillingAddress.StreetLine3;
                            sapAcc.BillingCity = dtr.BillingAddress.City == null ? "" : dtr.BillingAddress.City;
                            sapAcc.BillingState = dtr.BillingAddress.State == null ? "" : dtr.BillingAddress.State;
                            sapAcc.BillingCountry = dtr.BillingAddress.Country == null ? "" : dtr.BillingAddress.Country;
                            sapAcc.BillingPostalCode = dtr.BillingAddress.PostalCode == null ? "" : dtr.BillingAddress.PostalCode;
                        }

                        if (dtr.Phone != null)
                        {
                            sapAcc.PrimaryPhone = dtr.Phone.Primary == null ? "" : dtr.Phone.Primary;
                            sapAcc.SecondaryPhone = dtr.Phone.Secondary == null ? "" : dtr.Phone.Secondary;
                        }
                        if (dtr.Fax != null)
                        {
                            sapAcc.PrimaryFax = dtr.Fax.Primary == null ? "" : dtr.Fax.Primary;
                            sapAcc.SecondaryFax = dtr.Fax.Secondary == null ? "" : dtr.Fax.Secondary;
                        }
                        if (dtr.Email != null)
                        {
                            sapAcc.PrimaryEmail = dtr.Email.Primary == null ? "" : dtr.Email.Primary;
                            sapAcc.SecondaryEmail = dtr.Email.Secondary == null ? "" : dtr.Email.Secondary;
                        }
                        sapAcc.Status = dtr.Status.Value == null ? "Inactive" : dtr.Status;

                        SapAccounts.Add(sapAcc);
                        //sapAccounts.Add(sapAcc);


                        //Collect SubAccounts Info
                        if (dtr.SubAccounts != null && dtr.SubAccounts.Count > 0)
                        {
                            foreach (var subAcc in dtr.SubAccounts)
                            {
                                SapContact sapCon = new SapContact();
                                sapCon.ClientId = subAcc.DoctorId == null ? "" : subAcc.DoctorId;
                                sapCon.ClinicName = subAcc.ClinicName == null ? "" : subAcc.ClinicName;
                                sapCon.StaffId = subAcc.StaffId == null ? "" : subAcc.StaffId;
                                sapCon.DoctorLastName = subAcc.Name.Last == null ? "" : subAcc.Name.Last;
                                sapCon.DoctorFirstName = subAcc.Name.First == null ? "" : subAcc.Name.First;
                                if (subAcc.Phone != null)
                                {
                                    sapCon.PrimaryPhone = subAcc.Phone.Primary;
                                    sapCon.SecondaryPhone = subAcc.Phone.Secondary;
                                }
                                if (subAcc.Fax != null)
                                {
                                    sapCon.PrimaryFax = subAcc.Fax.Primary;
                                    sapCon.SecondaryFax = subAcc.Fax.Secondary;
                                }
                                if (subAcc.Email != null)
                                {
                                    sapCon.PrimaryEmail = subAcc.Email.Primary;
                                    sapCon.SecondaryEmail = subAcc.Email.Secondary;
                                }

                                //if (subAcc.PrimaryAddress != null)
                                //{
                                //    sapCon.PrimaryRecipient = subAcc.PrimaryAddress.Recipient;
                                //    sapCon.PrimaryStreet1 = subAcc.PrimaryAddress.StreetLine1;
                                //    sapCon.PrimaryStreet2 = subAcc.PrimaryAddress.StreetLine2;
                                //    sapCon.PrimaryStreet3 = subAcc.PrimaryAddress.StreetLine3;
                                //    sapCon.PrimaryCity = subAcc.PrimaryAddress.City;
                                //    sapCon.PrimaryState = subAcc.PrimaryAddress.State;
                                //    sapCon.PrimaryCountry = subAcc.PrimaryAddress.Country;
                                //    sapCon.PrimaryPostalCode = subAcc.PrimaryAddress.PostalCode;
                                //}

                                if (subAcc.PrimaryAddress != null)
                                {
                                    sapCon.PrimaryRecipient = subAcc.Address.Recipient;
                                    sapCon.PrimaryStreet1 = subAcc.Address.StreetLine1;
                                    sapCon.PrimaryStreet2 = subAcc.Address.StreetLine2;
                                    sapCon.PrimaryStreet3 = subAcc.Address.StreetLine3;
                                    sapCon.PrimaryCity = subAcc.Address.City;
                                    sapCon.PrimaryState = subAcc.Address.State;
                                    sapCon.PrimaryCountry = subAcc.Address.Country;
                                    sapCon.PrimaryPostalCode = subAcc.Address.PostalCode;
                                }
                                sapCon.Status = subAcc.Status.Value == null ? "Inactive" : subAcc.Status;

                                SapContacts.Add(sapCon);
                            }
                        }
                    }
                }

                resultGetSapDoctors = "SUCCESS";
            }
            catch (Exception ex)
            {
                resultGetSapDoctors = "ERROR";
                string errMsg = "InfoBusService.GetSapDoctors() met an issue:";
                errMsg += Environment.NewLine;
                errMsg += ex.Message;
                errMsg += Environment.NewLine;
                errMsg += "The lastModifiedTime is:" + lastModifiedTime;

                string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                errProcess.MsgHandler(msgType: "SYS-ERROR", errMsg);
            }

            return resultGetSapDoctors;
        }


        public List<SapContact> GetSapSubAccounts(string previousModifiedTime = null, string lastModifiedTime = null)
        {
            //Sapphire.SubAccount is practitionerInfo, provide info for SF.Contract
            List<SapContact> sapContacts = new List<SapContact>();

            try
            {
                //SappireRestCliente
                var sapphireClient = new SapphireRestClient();

                DateTime datetimePreviousModified = Convert.ToDateTime("01/01/1900 12:00:00 AM");
                DateTime datetimeLastModified = DateTime.Now.AddMinutes((24 * 60) * -1);

                //testing
                //lastModifiedTime = "10/21/2021";
                //

                if (!string.IsNullOrEmpty(previousModifiedTime))
                {
                    datetimePreviousModified = Convert.ToDateTime(previousModifiedTime);
                }

                if (!string.IsNullOrEmpty(lastModifiedTime))
                {
                    datetimeLastModified = Convert.ToDateTime(lastModifiedTime);
                }

                // Sapphire.practitioner is Sapphire.SubAccount, SF.Contact
                var practitioners = sapphireClient.GetSubAccount(clientId: null, datetimePreviousModified, datetimeLastModified);

                if (practitioners != null && practitioners.Count() > 0)
                {
                    foreach (var prac in practitioners)
                    {
                        SapContact sapCon = new SapContact();

                        //Sap.subaccount.doctorId=Sap.doctor.doctorId=sapApi.subaccount.ParentId;
                        //sapCon.ClientId = prac.DoctorId == null ? "" : prac.DoctorId;

                        sapCon.ClientId = prac.ParentId == null ? "" : prac.ParentId;
                        sapCon.StaffId = prac.StaffId == null ? "" : prac.StaffId;
                        sapCon.DoctorLastName = prac.Name.Last == null ? "" : prac.Name.Last;
                        sapCon.DoctorFirstName = prac.Name.First == null ? "" : prac.Name.First;
                        if (prac.Phone != null)
                        {
                            sapCon.PrimaryPhone = prac.Phone.Primary;
                            sapCon.SecondaryPhone = prac.Phone.Secondary;
                        }
                        if (prac.Fax != null)
                        {
                            sapCon.PrimaryFax = prac.Fax.Primary;
                            sapCon.SecondaryFax = prac.Fax.Secondary;
                        }
                        if (prac.Email != null)
                        {
                            sapCon.PrimaryEmail = prac.Email.Primary;
                            sapCon.SecondaryEmail = prac.Email.Secondary;
                        }

                        if (prac.Address != null)
                        {
                            sapCon.PrimaryRecipient = prac.Address.Recipient;
                            sapCon.PrimaryStreet1 = prac.Address.StreetLine1;
                            sapCon.PrimaryStreet2 = prac.Address.StreetLine2;
                            sapCon.PrimaryStreet3 = prac.Address.StreetLine3;
                            sapCon.PrimaryCity = prac.Address.City;
                            sapCon.PrimaryState = prac.Address.State;
                            sapCon.PrimaryCountry = prac.Address.Country;
                            sapCon.PrimaryPostalCode = prac.Address.PostalCode;
                        }

                        //if (prac.PrimaryAddress != null)
                        //{
                        //    sapCon.PrimaryRecipient = prac.PrimaryAddress.Recipient;
                        //    sapCon.PrimaryStreet1 = prac.PrimaryAddress.StreetLine1;
                        //    sapCon.PrimaryStreet2 = prac.PrimaryAddress.StreetLine2;
                        //    sapCon.PrimaryStreet3 = prac.PrimaryAddress.StreetLine3;
                        //    sapCon.PrimaryCity = prac.PrimaryAddress.City;
                        //    sapCon.PrimaryState = prac.PrimaryAddress.State;
                        //    sapCon.PrimaryCountry = prac.PrimaryAddress.Country;
                        //    sapCon.PrimaryPostalCode = prac.PrimaryAddress.PostalCode;
                        //}

                        sapCon.Status = prac.Status.Value == null ? "Inactive" : prac.Status;

                        sapContacts.Add(sapCon);
                    }
                }
            }
            catch (Exception ex)
            {
                string errMsg = "InfoBusService.GetSapSubAccounts() met an issue:";
                errMsg += Environment.NewLine;
                errMsg += ex.Message;
                errMsg += Environment.NewLine;
                errMsg += "The lastModifiedTime is:" + lastModifiedTime;
                string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                errProcess.MsgHandler(msgType: "SYS-ERROR", errMsg);
            }

            return sapContacts;
        }

        public List<SFAccount> GetSFUpdInsertAccounts(List<SapAccount> sapAccounts)
        {
            List<SFAccount> sfAccounts = new List<SFAccount>();

            try
            {
                SFAccount sfAcc = new SFAccount();
                StringBuilder sbIssues = new StringBuilder();
                string phoneNum = "";
                string street = "";
                string sapEmail = "";
                string sapSubEmail = "";
                int indexSemicolon = -1;

                if (sapAccounts != null && sapAccounts.Count() > 0)
                {
                    foreach (var dtr in sapAccounts)
                    {
                        if (string.IsNullOrEmpty(dtr.ClientId) || string.IsNullOrEmpty(dtr.AccountName))
                        {
                            sbIssues.AppendLine();
                            sbIssues.Append("Sap.ClientId:[" + dtr.ClientId + "],  Sap.ClinicName(AccountName):[" + dtr.AccountName + "];");
                        }
                        else
                        {
                            sfAcc = new SFAccount();
                            sfAcc.Client_ID__c = dtr.ClientId;
                            sfAcc.Name = dtr.AccountName;
                            phoneNum = string.IsNullOrEmpty(dtr.PrimaryPhone) ? dtr.SecondaryPhone : dtr.PrimaryPhone;
                            sfAcc.Phone = Commons.formatPhoneNumber(phoneNum, "(###) ###-####");

                            phoneNum = string.IsNullOrEmpty(dtr.PrimaryFax) ? dtr.SecondaryFax : dtr.PrimaryFax;
                            sfAcc.Fax = Commons.formatPhoneNumber(phoneNum, "(###) ###-####");


                            sapEmail = string.IsNullOrEmpty(dtr.PrimaryEmail) ? dtr.SecondaryEmail : dtr.PrimaryEmail;
                            if (string.IsNullOrEmpty(sapEmail) == false && sapEmail.Length > 0)
                            {
                                indexSemicolon = sapEmail.IndexOf(";");
                                if (indexSemicolon > 0 && indexSemicolon <= sapEmail.Length - 1)
                                {
                                    sapEmail = sapEmail.Substring(0, sapEmail.IndexOf(";"));
                                }
                                else if (indexSemicolon == sapEmail.Length - 1)
                                {
                                    sapEmail = sapEmail.Substring(0, sapEmail.IndexOf(";"));
                                    sapSubEmail += sapEmail.Substring(sapEmail.IndexOf(";"));
                                }
                            }
                            sfAcc.BillingEmail__c = sapEmail;

                            sapEmail = string.IsNullOrEmpty(dtr.PrimaryEmail) ? dtr.SecondaryEmail : dtr.PrimaryEmail;
                            if (string.IsNullOrEmpty(sapEmail) == false && sapEmail.Length > 0)
                            {
                                indexSemicolon = sapEmail.IndexOf(";");
                                if (indexSemicolon > 0 && indexSemicolon <= sapEmail.Length - 1)
                                {
                                    sapEmail = sapEmail.Substring(0, sapEmail.IndexOf(";"));
                                }
                                else if (indexSemicolon == sapEmail.Length - 1)
                                {
                                    sapEmail = sapEmail.Substring(0, sapEmail.IndexOf(";"));
                                    sapSubEmail += sapEmail.Substring(sapEmail.IndexOf(";"));
                                }
                            }
                            sfAcc.Main_Email__c = sapEmail;

                            street = "";
                            if (!string.IsNullOrEmpty(dtr.BillingStreet1))
                                street = dtr.BillingStreet1;
                            if (!string.IsNullOrEmpty(dtr.BillingStreet2))
                                street += ", " + dtr.BillingStreet2;
                            if (!string.IsNullOrEmpty(dtr.BillingStreet3))
                                street += ", " + dtr.BillingStreet3;
                            sfAcc.BillingStreet = street;
                            sfAcc.BillingCity = dtr.BillingCity;
                            sfAcc.BillingState = dtr.BillingState;
                            sfAcc.BillingCountry = dtr.BillingCountry;
                            sfAcc.BillingPostalCode = dtr.BillingPostalCode;


                            street = "";
                            if (!string.IsNullOrEmpty(dtr.ShippingStreet1))
                                street = dtr.ShippingStreet1;
                            if (!string.IsNullOrEmpty(dtr.ShippingStreet2))
                                street += ", " + dtr.ShippingStreet2;
                            if (!string.IsNullOrEmpty(dtr.ShippingStreet3))
                                street += ", " + dtr.ShippingStreet3;
                            sfAcc.ShippingStreet = street;
                            sfAcc.ShippingCity = dtr.ShippingCity;
                            sfAcc.ShippingState = dtr.ShippingState;
                            sfAcc.ShippingCountry = dtr.ShippingCountry;
                            sfAcc.ShippingPostalCode = dtr.ShippingPostalCode;

                            sfAcc.Comments__c = "Sapphire Accounts Transfer to SF";
                            sfAcc.Comments__c += " OtherEmail(s):" + sapSubEmail;

                            sfAcc.Active__c = dtr.Status.ToUpper() == "ACTIVE" ? "true" : "false";

                            sfAccounts.Add(sfAcc);
                        }
                    }
                }

                //Issues handling---ClientId or AccountName is null
                if (string.IsNullOrEmpty(sbIssues.ToString()) == false)
                {
                    string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                    string issueMsg = "!Warning: SapphireAccounts have issues !";
                    issueMsg += Environment.NewLine;
                    issueMsg += sbIssues.ToString();
                    issueMsg += Environment.NewLine;
                    issueMsg += Environment.NewLine;
                    issueMsg += "The message is from InfoBusService.GetSFUpdInsertAccounts(List<SapAccount> sapAccounts)";
                    Commons.LogWrite(LogFile, issueMsg);

                    Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                    errProcess.MsgHandler(msgType: "SF-SYNC-SAPPHIRE", issueMsg);
                }

            }
            catch (Exception ex)
            {
                string msgTimeStamp = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                Messaging errProcess = new Messaging(msgTimeStamp, "Sapphire sync to SalesForce");
                string errMsg = "InfoBusService.GetSFUpdInsertAccounts() met an issue:";
                errMsg += Environment.NewLine;
                errMsg += ex.Message;
                errProcess.MsgHandler(msgType: "SYS-ERROR", errMsg);
            }


            return sfAccounts;

        }

        public List<SFContact> GetSFUpdInsertContacts(List<SapContact> sapContacts)
        {
            //If ClientId, StaffId, or LastName is null, log and email them;
            //Sapphire:select doctorid, CopiaStaffId from u_subaccount where doctorid = '35287' and staus='Active'

            List<SFContact> sfContacts = new List<SFContact>();
            List<SFContact> sfContactsWithoutAccount = new List<SFContact>();

            try
            {
                string mailStreet = "";
                string phoneNum = "";
                string sapEmail = "";
                string sapSubEmail = "";
                int indexSemicolon = -1;
                StringBuilder sbIssues = new StringBuilder();

                if (sapContacts != null && sapContacts.Count() > 0)
                {
                    foreach (var sapCon in sapContacts)
                    {
                        if (string.IsNullOrEmpty(sapCon.DoctorLastName) || string.IsNullOrEmpty(sapCon.StaffId))
                        {
                            sbIssues.Append(Environment.NewLine);
                            sbIssues.Append("StaffId:[" + sapCon.StaffId + "], LastName:[" + sapCon.DoctorLastName + "];");
                        }
                        else
                        {
                            SFContact sfCon = new SFContact();
                            sfCon.FirstName = sapCon.DoctorFirstName;
                            sfCon.LastName = sapCon.DoctorLastName;
                            sfCon.StaffId__c = sapCon.StaffId;

                            phoneNum = string.IsNullOrEmpty(sapCon.PrimaryPhone) ? sapCon.SecondaryPhone : sapCon.PrimaryPhone;
                            sfCon.Phone = Commons.formatPhoneNumber(phoneNum, "(###) ###-####");

                            phoneNum = string.IsNullOrEmpty(sapCon.PrimaryFax) ? sapCon.SecondaryFax : sapCon.PrimaryFax;
                            sfCon.Fax = Commons.formatPhoneNumber(phoneNum, "(###) ###-####");

                            sapEmail = string.IsNullOrEmpty(sapCon.PrimaryEmail) ? sapCon.SecondaryEmail : sapCon.PrimaryEmail;
                            if (string.IsNullOrEmpty(sapEmail) == false && sapEmail.Length > 0)
                            {
                                indexSemicolon = sapEmail.IndexOf(";");
                                if (indexSemicolon > 0 && indexSemicolon <= sapEmail.Length - 1)
                                {
                                    sapEmail = sapEmail.Substring(0, sapEmail.IndexOf(";"));
                                }
                                else if (indexSemicolon == sapEmail.Length - 1)
                                {
                                    sapEmail = sapEmail.Substring(0, sapEmail.IndexOf(";"));
                                    sapSubEmail += sapEmail.Substring(sapEmail.IndexOf(";"));
                                }
                            }
                            sfCon.Email = sapEmail;

                            mailStreet = "";
                            if (!string.IsNullOrEmpty(sapCon.PrimaryStreet1))
                                mailStreet = sapCon.PrimaryStreet1;
                            if (!string.IsNullOrEmpty(sapCon.PrimaryStreet2))
                                mailStreet += ", " + sapCon.PrimaryStreet2;
                            if (!string.IsNullOrEmpty(sapCon.PrimaryStreet3))
                                mailStreet += ", " + sapCon.PrimaryStreet3;
                            sfCon.MailingStreet = mailStreet;
                            sfCon.MailingCity = sapCon.PrimaryCity;
                            sfCon.MailingState = sapCon.PrimaryState;
                            sfCon.MailingCountry = sapCon.PrimaryCountry;
                            sfCon.MailingPostalCode = sapCon.PrimaryPostalCode;

                            sfCon.Description = "Sapphire Contacts Transfer to SF";
                            sfCon.Description += " OtherEmail(s):" + sapSubEmail;

                            sfCon.Active__c = sapCon.Status.ToUpper() == "ACTIVE" ? "true" : "false";

                            if (string.IsNullOrEmpty(sapCon.ClientId))
                            {
                                //Sapphire.ClientId never be null
                                sbIssues.Append(Environment.NewLine);
                                sbIssues.Append(Environment.NewLine);
                                sbIssues.Append("! Warning: Sapphire.ClientId is Null] !");
                                sbIssues.Append("StaffId:[" + sapCon.StaffId + "], LastName:[" + sapCon.DoctorLastName + "];");
                                sbIssues.Append(Environment.NewLine);
                                sbIssues.Append(Environment.NewLine);
                            }
                            else
                            {
                                sfCon.ClientId__c = sapCon.ClientId;
                            }

                            sfContacts.Add(sfCon);
                        }
                    }
                }

                //Issues handling---ClientId or StaffId or LastName is null
                if (string.IsNullOrEmpty(sbIssues.ToString()) == false)
                {
                    string issueMsg = "! Warning: SapphirePractitioners have issues: ";
                    issueMsg += Environment.NewLine;
                    issueMsg += sbIssues.ToString();
                    issueMsg += Environment.NewLine;
                    issueMsg += Environment.NewLine;
                    issueMsg += "The message is from InfoBusService.GetSFUpdInsertContacts(List<SapContact> sapContacts)";
                    Commons.LogWrite(LogFile, issueMsg);
                    string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                    Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                    errProcess.MsgHandler(msgType: "SF-SYNC-SAPPHIRE", issueMsg);
                }

                //sfMatchedContacts = (List<SFContact>)sfContacts.Where(c => (c.ClientId__c == sapDct.ClientId) &&
                //                                                             (c.FirstName == sapDct.DoctorFirstName) &&
                //                                                             (c.LastName == sapDct.DoctorLastName)
                //                                                             ).ToList();

            }
            catch (Exception ex)
            {
                string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                string errMsg = "InfoBusService.GetSFUpdInsertContacts() met an issue:";
                errMsg += Environment.NewLine;
                errMsg += ex.Message;
                errProcess.MsgHandler(msgType: "SYS-ERROR", errMsg);
            }


            return sfContacts;
        }
    }
}

