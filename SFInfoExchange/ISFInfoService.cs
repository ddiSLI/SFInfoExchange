using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using SFInfoExchange.Models;
using Salesforce.Force;

namespace SFInfoExchange
{
    interface ISFInfoService
    {
        List<SFAccountChanges> UpdInsertAccounts(List<SFAccount> sfAccounts);
        string UpInsertContactMain(List<SFContact> sfContacts, List<SFAccountChanges> sfAccInactive);
        List<SFContactChanges> UpdInsertContacts(List<SFContact> sfContacts);
        string CleanContactByInactiveAccounts(List<SFAccountChanges> sfAccChanges);
        string CleanContactByInactiveContacts(List<SFContactChanges> sfConChanges);
        List<SFContact> GetSFContactWithAccountId(List<SFContact> sfContacts);
    }

    interface ISapService
    {
        //List<SapAccount> GetSapDoctors(string previousModifiedTime = null, string lastModifiedTime = null);
        string GetSapDoctors(string previousModifiedTime = null, string lastModifiedTime = null);
        List<SapContact> GetSapSubAccounts(string previousModifiedTime = null, string lastModifiedTime = null);
        List<SFAccount> GetSFUpdInsertAccounts(List<SapAccount> sapAccounts);
        List<SFContact> GetSFUpdInsertContacts(List<SapContact> sapContacts);
    }
        
    interface ISQLService
    {
        string[] GetLastSyncInfo();
        string SetSyncTransPlan(string syncDoneDate, string syncStatus, string syncDesc);
        string UpdateSQL(List<SFObjectValue> objValueUpd);
    }

}

