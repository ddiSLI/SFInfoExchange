using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net;
using System.Text;
using System.Threading.Tasks;

using Salesforce.Common.Models.Json;
using Salesforce.Force;
using SFInfoExchange.Models;

namespace SFInfoExchange
{
    public class SFService : ISFInfoService
    {
        public SalesForceClient DDIClient { get; set; }
        public List<string> IssuedHisIds { get; set; }
        public bool SFAllProcessSuccessed { get; set; } = true;
        public string LogFile { get; set; }

        private ForceClient GetSFClient()
        {
            var auth = new Salesforce.Common.AuthenticationClient();

            try
            {
                if (DDIClient.RunCondition == "DEV")
                {
                    auth.UsernamePasswordAsync(DDIClient.ClientId, DDIClient.ClientSecret, DDIClient.Username, DDIClient.Password).Wait();
                }
                else
                {
                    auth.UsernamePasswordAsync(DDIClient.ClientId, DDIClient.ClientSecret, DDIClient.Username, DDIClient.Password, DDIClient.TokenRequestEndpointUrl).Wait();
                }
                var client = new ForceClient(auth.InstanceUrl, auth.AccessToken, auth.ApiVersion);

                // return Task.FromResult(client);
                return client;
            }
            catch (Exception ex)
            {
                string errMsg = "SFService.GetSFClient() met an issue:";
                errMsg += Environment.NewLine;
                errMsg += ex.Message;
                string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                errProcess.MsgHandler(msgType: "SYS-ERROR", errMsg);
            }

            return null;
        }

        public List<SFAccountChanges> UpdInsertAccounts(List<SFAccount> sfAccounts)
        {
            StringBuilder sbSyncResults = new StringBuilder();
            List<SFAccountChanges> sfAccChanges = new List<SFAccountChanges>();

            string actMsg = "";
            string sfQuery;

            try
            {
                var client = GetSFClient();
                string sfClientId = "";
                string sfAccountId = "";
                string sfAccountName = "";
                string sfPhone = "";
                string sfBillingEmail = "";
                string sfMainEmail = "";
                string sfFax = "";
                string sfBillingStreet = "";
                string sfBillingCity = "";
                string sfBillingState = "";
                string sfBillingCountry = "";
                string sfBillingPostalCode = "";
                string sfShippingStreet = "";
                string sfShippingCity = "";
                string sfShippingState = "";
                string sfShippingCountry = "";
                string sfShippingPostalCode = "";
                string sfComments = "";
                string sfActive = "";

                if (sfAccounts != null && sfAccounts.Count() > 0)
                {
                    foreach (var acc in sfAccounts)
                    {
                        sfAccountId = "";
                        sfPhone = "";
                        sfBillingEmail = "";
                        sfMainEmail = "";
                        sfFax = "";
                        sfBillingStreet = "";
                        sfBillingCity = "";
                        sfBillingState = "";
                        sfBillingCountry = "";
                        sfBillingPostalCode = "";
                        sfShippingStreet = "";
                        sfShippingCity = "";
                        sfShippingState = "";
                        sfShippingCountry = "";
                        sfShippingPostalCode = "";
                        sfComments = "";

                        sfAccountName = acc.Name;
                        sfClientId = acc.Client_ID__c;
                        if (string.IsNullOrEmpty(acc.Phone) == false)
                            sfPhone = acc.Phone;
                        if (string.IsNullOrEmpty(acc.Fax) == false)
                            sfFax = acc.Fax;
                        if (string.IsNullOrEmpty(acc.BillingEmail__c) == false)
                            sfBillingEmail = acc.BillingEmail__c;
                        if (string.IsNullOrEmpty(acc.Main_Email__c) == false)
                            sfMainEmail = acc.Main_Email__c;
                        if (string.IsNullOrEmpty(acc.BillingCity) == false)
                        {
                            sfBillingStreet = acc.BillingStreet;
                            sfBillingCity = acc.BillingCity;
                            sfBillingState = acc.BillingState;
                            sfBillingCountry = acc.BillingCountry;
                            sfBillingPostalCode = acc.BillingPostalCode;
                        }
                        if (string.IsNullOrEmpty(acc.ShippingCity) == false)
                        {
                            sfShippingStreet = acc.ShippingStreet;
                            sfShippingCity = acc.ShippingCity;
                            sfShippingState = acc.ShippingState;
                            sfShippingCountry = acc.ShippingCountry;
                            sfShippingPostalCode = acc.ShippingPostalCode;
                        }
                        if (string.IsNullOrEmpty(acc.Comments__c) == false)
                            sfComments = acc.Comments__c;
                        sfActive = acc.Active__c;
                        
                        var newAcc = new
                        {
                            Name = sfAccountName,
                            Client_ID__c = sfClientId,
                            Phone = sfPhone,
                            Fax = sfFax,
                            BillingEmail__c = sfBillingEmail,
                            Main_Email__c = sfMainEmail,
                            BillingStreet = sfBillingStreet,
                            BillingCity = sfBillingCity,
                            BillingState = sfBillingState,
                            BillingCountry = sfBillingCountry,
                            BillingPostalCode = sfBillingPostalCode,
                            ShippingStreet = sfShippingStreet,
                            ShippingCity = sfShippingCity,
                            ShippingState = sfShippingState,
                            ShippingCountry = sfShippingCountry,
                            ShippingPostalCode = sfShippingPostalCode,
                            Comments__c = sfComments,
                            Active__c = sfActive
                        };


                        if (acc.Client_ID__c == "10072")
                        {
                            string clintid = acc.Client_ID__c;
                        }

                        sfQuery = "Select Id, Name, Client_ID__c ";
                        sfQuery += "    ,BillingStreet,BillingCity,BillingState, BillingCountry,BillingPostalCode ";
                        sfQuery += "    ,ShippingStreet,ShippingCity,ShippingState, ShippingCountry,ShippingPostalCode ";
                        sfQuery += "    ,Phone,Fax, BillingEmail__c, Main_Email__c ";
                        sfQuery += "    ,Comments__c,Active__c ";
                        sfQuery += " From Account";
                        sfQuery += " Where (Client_ID__c='" + acc.Client_ID__c + "') ";
                        sfQuery += "        AND (IsDeleted = false) ";

                        Task<QueryResult<dynamic>> results = client.QueryAllAsync<dynamic>(sfQuery);
                        results.Wait();
                        var sfAccs = results.Result.Records;
                        if (sfAccs.Count > 0)
                        {
                            sfAccountId = sfAccs.FirstOrDefault().Id;

                            //SyncAccounts_1:Account is existing
                            //Update this account
                            var updateSucess = client.UpdateAsync("Account", sfAccountId, newAcc);
                            updateSucess.Wait();
                            if (updateSucess.Result.Success)
                            {
                                sfClientId = acc.Client_ID__c;
                                sfAccountName = acc.Name;
                            }
                            else
                            {
                                actMsg = DateTime.Now.ToShortTimeString() + "---";
                                actMsg += "Failed Update SF.Account, Client_Id__c:" + acc.Client_ID__c;
                                sbSyncResults.Append(actMsg);
                                sbSyncResults.AppendLine();
                            }
                        }
                        else
                        {
                            //SyncAccounts_2:New Account
                            //Create a new account
                            var createTask = client.CreateAsync("Account", newAcc);
                            createTask.Wait();
                            var createResult = createTask.Result;
                            if (createResult.Success)
                            {
                                sfAccountId = createResult.Id.ToString();
                                sfClientId = acc.Client_ID__c;
                                sfAccountName = acc.Name;
                            }
                            else
                            {
                                actMsg = DateTime.Now.ToShortTimeString() + "---";
                                actMsg += "Failed Create SF.Account, Client_Id__c:" + acc.Client_ID__c;
                                sbSyncResults.Append(actMsg);
                                sbSyncResults.AppendLine();
                            }
                        }

                        //Record updated Accounts
                        if (string.IsNullOrEmpty(sfAccountId) == false)
                        {
                            sfAccChanges.Add(new SFAccountChanges
                            {
                                ClientId = sfClientId,
                                AccountId = sfAccountId,
                                AccountName = sfAccountName,
                                Active = acc.Active__c
                            });
                        }
                    }
                }

                if (sbSyncResults.Length > 0)
                {
                    SFAllProcessSuccessed = false;
                    string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                    string issueMsg = "Process SFAccounts with issues: ";
                    issueMsg += Environment.NewLine;
                    issueMsg += "Log is from SFService.UpdInsertAccounts()";
                    issueMsg += Environment.NewLine;
                    issueMsg += sbSyncResults.ToString();
                    Commons.LogWrite(LogFile, issueMsg);

                    Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                    errProcess.MsgHandler(msgType: "SF-SYNC-SF", sbSyncResults.ToString());
                }
            }
            catch (Exception ex)
            {
                SFAllProcessSuccessed = false;
                string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                string errMsg = "SYS-Error--SFService.UpdInsertAccounts() met an issue:";
                errMsg += Environment.NewLine;
                errMsg += ex.Message;
                errMsg += Environment.NewLine;
                errMsg += ex.Message;
                errMsg += Environment.NewLine;
                errMsg += Environment.NewLine;
                errMsg += "SF-Data-Sync issues:";
                errMsg += Environment.NewLine;
                errMsg += sbSyncResults.ToString();
                Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                errProcess.MsgHandler(msgType: "SYS-ERROR", errMsg);
            }

            return sfAccChanges;
        }

        public List<SFContact> GetSFContactWithAccountId(List<SFContact> sfContacts)
        {
            //Using Sap.subaccount.ClientId(Contact) to get SF.Contacts with SF.AccountId
            //Record SF.Contact.AccountId that is not in SF.Account

            StringBuilder sbSyncResults = new StringBuilder();

            try
            {
                var client = GetSFClient();
                string sfQuery = "";
                string sfClientId = "";
                string sfAccountId = "";

                if (sfContacts != null && sfContacts.Count() > 0)
                {
                    //Get all unique AccountIds in the Contacts (ClientId(Sap.DoctorId)--AccountId)
                    var sfContactsGroupByClientId = sfContacts.GroupBy(con => con.ClientId__c);

                    foreach (var clientIdGroup in sfContactsGroupByClientId)
                    {
                        sfAccountId = "";    //Blank, non-existing Account

                        sfClientId = clientIdGroup.Key;
                        sfQuery = "Select Id, Name, Client_ID__c, Active__c ";
                        sfQuery += " From Account";
                        sfQuery += " Where (Client_ID__c='" + sfClientId + "') ";
                        sfQuery += "        AND (IsDeleted = false) ";
                        Task<QueryResult<dynamic>> results = client.QueryAllAsync<dynamic>(sfQuery);
                        results.Wait();
                        var sfAccs = results.Result.Records;
                        if (sfAccs.Count > 0)
                        {
                            //Set contact.AccountId
                            sfAccountId = sfAccs.FirstOrDefault().Id;
                            sfContacts.Where(con => con.ClientId__c == sfClientId).ToList().ForEach(con => con.AccountId = sfAccountId);
                        }
                        else
                        {
                            //the Contact.ClientId is not in SF.Account, record it;
                            sbSyncResults.AppendLine();
                            sbSyncResults.Append("Sapphire.ClientId:[" + sfClientId + "]; ");
                            sbSyncResults.AppendLine();
                        }
                    }
                }

                //If SF.Contact.AccountId(s) are not in SF.Account, log it and email it;
                if (sbSyncResults.Length > 0)
                {
                    string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                    string issueMsg = "Processing SFContacts with issues";
                    issueMsg += Environment.NewLine;
                    issueMsg += "! Warning: Sapphire.ClientId(s) is NOT in SF.Account !";
                    issueMsg += sbSyncResults.ToString();
                    issueMsg += Environment.NewLine;
                    issueMsg += Environment.NewLine;
                    issueMsg += "The Message is from SFService.GetSFContactWithAccountId()";
                    Commons.LogWrite(LogFile, issueMsg);

                    Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                    errProcess.MsgHandler(msgType: "SF-SYNC-SF", issueMsg);
                }

            }
            catch (Exception ex)
            {
                SFAllProcessSuccessed = false;
                string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                string errMsg = "SFService.GetContactWithAccountId() met an issue:";
                errMsg += Environment.NewLine;
                errMsg += ex.Message;
                errMsg += Environment.NewLine;
                errMsg += Environment.NewLine;
                errMsg += "SF-Data-Sync issues:";
                errMsg += Environment.NewLine;
                errMsg += sbSyncResults.ToString();
                Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                errProcess.MsgHandler(msgType: "SYS-ERROR", errMsg);
            }

            return sfContacts;
        }

        public string UpInsertContactMain(List<SFContact> sfContacts, List<SFAccountChanges> sfAccChanges)
        {
            string contactSyncResult = "NA";
            List<SFContactChanges> sfConInactive = new List<SFContactChanges>();
            StringBuilder sbSyncResults = new StringBuilder();

            try
            {
                //SyncContacts_1: Check and filter Contact.ClientId(s) availablility in SF.Account;
                //1-if Contact.ClientId is not in SF.Account, log it and email it;
                //2-Set Contact.AccountId = SF.Account.Id
                sfContacts = GetSFContactWithAccountId(sfContacts);

                //SyncContacts_2: regular update/insert
                //1-update the existing contacts;
                //2-add New Contact;
                sfConInactive = UpdInsertContacts(sfContacts);

                //SyncContacts_3: Clean SF.Contact: ContactId is Inactive
                string resultCleanContactByInactiveContacts = CleanContactByInactiveContacts(sfConInactive);

                //SyncContacts_4: Clean SF.Contact: SF.Account.AccountId is Inactive, all contacts with this AccountId are UNavailable
                string resultCleanContactByInactiveAccounts = CleanContactByInactiveAccounts(sfAccChanges);

                contactSyncResult = "SUCCESS";
            }
            catch (Exception ex)
            {
                contactSyncResult = "SYS-ERROR";
                SFAllProcessSuccessed = false;
                string errMsg = "SFService.UpdInsertContacts() met an issue:";
                errMsg += Environment.NewLine;
                errMsg += ex.Message;
                string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                errProcess.MsgHandler(msgType: "SYS-ERROR", errMsg);
            }

            return contactSyncResult;
        }

        public List<SFContactChanges> UpdInsertContacts(List<SFContact> sfContacts)
        {
            //Update and insert SF.Contacts
            List<SFContactChanges> sfConInactive = new List<SFContactChanges>();
            StringBuilder sbSyncResults = new StringBuilder();

            string actMsg = "";
            string sfQuery = "";

            try
            {
                var client = GetSFClient();

                string accActive = "true";
                string sfContactId = "";
                string sfStaffId = "";
                string sfClientId = "";
                string sfAccountId = "";
                string sfFirstName = "";
                string sfLastName = "";
                string sfPhone = "";
                string sfFax = "";
                string sfEmail = "";
                string sfMailingStreet = "";
                string sfMailingCity = "";
                string sfMailingState = "";
                string sfMailingCountry = "";
                string sfMailingPostalCode = "";
                string sfDescription = "";

                foreach (var con in sfContacts)
                {
                    sfContactId = "";
                    sfStaffId = "";
                    sfFirstName = "";
                    sfLastName = "";
                    sfPhone = "";
                    sfFax = "";
                    sfEmail = "";
                    sfMailingStreet = "";
                    sfMailingCity = "";
                    sfMailingState = "";
                    sfMailingCountry = "";
                    sfMailingPostalCode = "";
                    sfDescription = "";
                    accActive = "true";

                    accActive = con.Active__c;
                    sfAccountId = con.AccountId;
                    sfClientId = con.ClientId__c;
                    sfStaffId = con.StaffId__c;
                    if (string.IsNullOrEmpty(con.FirstName) == false)
                        sfFirstName = con.FirstName;

                    sfLastName = con.LastName;

                    if (string.IsNullOrEmpty(con.Phone) == false)
                        sfPhone = con.Phone;
                    if (string.IsNullOrEmpty(con.Fax) == false)
                        sfFax = con.Fax;
                    if (string.IsNullOrEmpty(con.Email) == false)
                        sfEmail = con.Email;
                    if (string.IsNullOrEmpty(con.MailingCity) == false)
                    {
                        sfMailingStreet = con.MailingStreet;
                        sfMailingCity = con.MailingCity;
                        sfMailingState = con.MailingState;
                        sfMailingCountry = con.MailingCountry;
                        sfMailingPostalCode = con.MailingPostalCode;
                    }
                    if (string.IsNullOrEmpty(con.Description) == false)
                        sfDescription = con.Description;

                    if (string.IsNullOrEmpty(sfAccountId))
                    {
                        //SF.Contact.AccountId missed in SF.Account
                        sbSyncResults.AppendLine();
                        sbSyncResults.Append("  ClientId:" + sfClientId + ", StaffId:" + sfStaffId);
                        sbSyncResults.AppendLine();
                    }
                    else
                    {
                        //Default this contact is New
                        bool hasThisContact = false;

                        //Select Contacts by AccountId
                        sfQuery = "Select Id, ClientId__c, StaffId__c, AccountId ";
                        sfQuery += "    ,FirstName, LastName, Phone, Fax, Email ";
                        sfQuery += "    ,MailingStreet, MailingCity, MailingState, MailingCountry, MailingPostalCode ";
                        sfQuery += "    ,Description, Active__c ";
                        sfQuery += " From Contact";
                        sfQuery += " Where (AccountId='" + sfAccountId + "') ";
                        sfQuery += "        AND (StaffId__c='" + sfStaffId + "') ";
                        sfQuery += "        AND (IsDeleted = false) ";

                        Task<QueryResult<dynamic>> results = client.QueryAllAsync<dynamic>(sfQuery);
                        results.Wait();
                        var sfCons = results.Result.Records;
                        if (sfCons.Count > 0)
                        {
                            //Contact.AccouintId and StaffId are existing; Get ContactId for updating;
                            hasThisContact = true;
                            sfContactId = sfCons.FirstOrDefault().Id;
                        } 

                        var newCon = new
                        {
                            AccountId = sfAccountId,
                            ClientId__c = sfClientId,
                            StaffId__c = sfStaffId,
                            FirstName = sfFirstName,
                            LastName = sfLastName,
                            Phone = sfPhone,
                            Fax = sfFax,
                            Email = sfEmail,
                            MailingStreet = sfMailingStreet,
                            MailingCity = sfMailingCity,
                            MailingState = sfMailingState,
                            MailingCountry = sfMailingCountry,
                            MailingPostalCode = sfMailingPostalCode,
                            Description = sfDescription,
                            Active__c = accActive
                        };

                        if (hasThisContact)
                        {
                            //The Contact is existing
                            //Update this contact
                            var updateSucess = client.UpdateAsync("Contact", sfContactId, newCon);
                            updateSucess.Wait();
                            if (updateSucess.Result.Success)
                            {
                                //Record Inactive contacts
                                if (accActive.ToUpper() == "FALSE")
                                {
                                    sfConInactive.Add(new SFContactChanges
                                    {
                                        ContactId = sfContactId,
                                        ClientId = sfClientId,
                                        AccountId = sfAccountId,
                                        Active = accActive
                                    });
                                }
                            }
                            else
                            {
                                actMsg = DateTime.Now.ToShortTimeString() + "---";
                                actMsg += "Failed UpdateContact, ClientId__c:" + con.ClientId__c;
                                sbSyncResults.Append(actMsg);
                            }
                        }
                        else
                        {
                            //Create the new contact
                            var createTask = client.CreateAsync("Contact", newCon);
                            createTask.Wait();
                            var createResult = createTask.Result;
                            if (!createResult.Success)
                            {
                                actMsg = DateTime.Now.ToShortTimeString() + "---";
                                actMsg += "Failed CreateContact, Client_Id__c:" + con.ClientId__c;
                                sbSyncResults.Append(actMsg);
                            }
                        }
                    }   //if(string.IsNullOrEmpty(sfAccountId)) else

                }

                if (sbSyncResults.Length > 0)
                {
                    SFAllProcessSuccessed = false;
                    string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                    string issueMsg = "Process SFContacts with issues";
                    issueMsg += Environment.NewLine;
                    issueMsg += "! SF.Contact.AccountId missed in SF.Account !";
                    issueMsg += Environment.NewLine;
                    issueMsg += sbSyncResults.ToString();
                    issueMsg += Environment.NewLine;
                    issueMsg += "The messge is from SFService.UpdInsertContacts()";
                    issueMsg += Environment.NewLine;
                    Commons.LogWrite(LogFile, issueMsg);

                    Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                    errProcess.MsgHandler(msgType: "SF-SYNC-SF", issueMsg);
                }

            }
            catch (Exception ex)
            {
                SFAllProcessSuccessed = false;
                string errMsg = "SFService.UpdInsertContacts() met an issue:";
                errMsg += Environment.NewLine;
                errMsg += ex.Message;
                string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                errProcess.MsgHandler(msgType: "SYS-ERROR", errMsg);
            }

            return sfConInactive;
        }

        public string CleanContactByInactiveContacts(List<SFContactChanges> sfConInactive)
        {
            //Clean SF.Contact By SF.Contacts with the ActiveStatus=Inactive

            string actionResult = "NA";
            StringBuilder sbSyncResults = new StringBuilder();
            string actMsg = "";
            string sfContactId = "";

            try
            {
                var client = GetSFClient();
                foreach (var con in sfConInactive)
                {
                    sfContactId = con.ContactId.ToString().Trim();
                    if (con.Active.ToUpper() == "FALSE" && string.IsNullOrEmpty(sfContactId) == false)
                    {
                        var deleteSucess = client.DeleteAsync("Contact", sfContactId);
                        deleteSucess.Wait();
                        if (!deleteSucess.Result)
                        {
                            actMsg = DateTime.Now.ToShortTimeString() + "---";
                            actMsg += "Failed DeleteContact, ClientId:" + con.ClientId + ", ContactId: " + sfContactId + ", StaffId: " + con.StaffId;
                            sbSyncResults.Append(actMsg);
                        }
                    }
                }

                if (sbSyncResults.Length > 0)
                {
                    SFAllProcessSuccessed = false;
                    string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                    string issueMsg = "Process SFContacts with issues: ";
                    issueMsg += Environment.NewLine;
                    issueMsg += "Log is from SFService.CleanContactByInactiveContacts()";
                    issueMsg += Environment.NewLine;
                    issueMsg += sbSyncResults.ToString();
                    Commons.LogWrite(LogFile, issueMsg);

                    Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                    errProcess.MsgHandler(msgType: "SF-SYNC-SF", sbSyncResults.ToString());
                    actionResult = "CleanContactByInactiveContacts Processed with error";
                }
                else
                {
                    actionResult = "SUCCESS";
                }

            }
            catch (Exception ex)
            {
                SFAllProcessSuccessed = false;
                string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                string errMsg = "SFService.CleanContactByInactiveContacts() met an issue:";
                errMsg += Environment.NewLine;
                errMsg += ex.Message;
                actionResult = msgId + "---Exception:" + errMsg;
                Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                errProcess.MsgHandler(msgType: "SYS-ERROR", errMsg);
            }

            return actionResult;
        }

        public string CleanContactByInactiveAccounts(List<SFAccountChanges> sfAccInactive)
        {
            //Clean SF.Contacts By SF.Accounts with the ActiveStatus=Inactive

            string actionResult = "NA";
            StringBuilder sbSyncResults = new StringBuilder();
            string actMsg = "";
            string sfQuery = "";
            string sfAccountId = "";

            try
            {
                var client = GetSFClient();
                if (sfAccInactive != null && sfAccInactive.Count() > 0)
                {
                    foreach (var acc in sfAccInactive)
                    {
                        sfAccountId = acc.AccountId;
                        if (acc.Active.ToUpper() == "FALSE" && string.IsNullOrEmpty(sfAccountId) == false)
                        {
                            sfQuery = "Select Id, AccountId, ClientId__c, StaffId__c ";
                            sfQuery += " From Contact";
                            sfQuery += " Where (AccountId='" + sfAccountId + "') ";
                            sfQuery += "        AND (IsDeleted = false) ";

                            Task<QueryResult<dynamic>> results = client.QueryAllAsync<dynamic>(sfQuery);
                            results.Wait();
                            var sfCons = results.Result.Records;
                            if (sfCons.Count > 0)
                            {
                                foreach (var con in sfCons)
                                {
                                    var deleteSucess = client.DeleteAsync("Contact", con.Id.ToString());
                                    deleteSucess.Wait();
                                    if (!deleteSucess.Result)
                                    {
                                        actMsg = DateTime.Now.ToShortTimeString() + "---";
                                        actMsg += "Failed DeleteContact, ClientId:" + acc.ClientId + ", AccountId: " + sfAccountId;
                                        sbSyncResults.Append(actMsg);
                                    }
                                }
                            }
                        }
                    }
                }

                if (sbSyncResults.Length > 0)
                {
                    SFAllProcessSuccessed = false;
                    string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                    string issueMsg = "Process SFContacts with issues: ";
                    issueMsg += Environment.NewLine;
                    issueMsg += sbSyncResults.ToString();
                    issueMsg += Environment.NewLine;
                    issueMsg += "The Message is from SFService.CleanContactByInactiveAccounts()";
                    issueMsg += Environment.NewLine;
                    Commons.LogWrite(LogFile, issueMsg);

                    Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                    errProcess.MsgHandler(msgType: "SF-SYNC-SF", issueMsg);
                    actionResult = "CleanContactByInactiveAccounts Processed with error";
                }
                else
                {
                    actionResult = "SUCCESS";
                }

            }
            catch (Exception ex)
            {
                SFAllProcessSuccessed = false;
                string errMsg = "SFService.CleanContactByInactiveAccounts() met an issue:";
                errMsg += Environment.NewLine;
                errMsg += ex.Message;
                string msgId = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                Messaging errProcess = new Messaging(msgId, "Sapphire sync to SalesForce");
                errProcess.MsgHandler(msgType: "SYS-ERROR", errMsg);
                actionResult = msgId + "---Exception:" + errMsg;
            }

            return actionResult;
        }
    }
}
