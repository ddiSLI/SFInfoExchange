using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using SFInfoExchange.Models;

namespace SFInfoExchange
{
    public class TransPanel
    {
        SFService sfService = new SFService();
        private static SalesForceClient CreateClient(string previousModifiedTime = null, string lastModifiedTime = null, string needCreatNextTranPlan = null)
        {
            return new SalesForceClient(previousModifiedTime, previousModifiedTime, needCreatNextTranPlan);
        }

        public string MainPanel(string previousModifiedTime = null, string lastModifiedTime = null, string needCreateNextTransPlan = null)
        {
            string transResult = "NA";

            try
            {
                //
                //Test
                //lastModifiedTime = "2/3/2022 9:50:00 AM";
                    //DateTime.Now.AddMinutes((24 * 60) * -6).ToString();
                ////

                //set running conditions; set sync date 
                var ddiClient = CreateClient(previousModifiedTime, lastModifiedTime, needCreateNextTransPlan);
                sfService.DDIClient = ddiClient;
                sfService.LogFile = ddiClient.SFTransLog;

                List<SapAccount> sapAccounts = new List<SapAccount>();
                List<SapContact> sapContacts = new List<SapContact>();
                List<SFAccountChanges> sfAccChanges = new List<SFAccountChanges>();
                List<SFContactChanges> sfConChanges = new List<SFContactChanges>();
                SapService sapService = new SapService();
                sapService.LogFile = ddiClient.SFTransLog;
                
                //Sap.doctors includes SapDoctors(SF.Account) and SapSubaccounts(SF.Contact)
                string resultGetGetSapDoctors = sapService.GetSapDoctors(previousModifiedTime, lastModifiedTime);
                if (resultGetGetSapDoctors == "SUCCESS")
                {
                    sapAccounts = sapService.SapAccounts;
                    sapContacts = sapService.SapContacts;

                    //Step-1: Transalte Sap.Doctor to SF.Account
                    List<SFAccount> sfAccounts = sapService.GetSFUpdInsertAccounts(sapAccounts);

                    //Step-2: Translate sap.SubAccount to SF.Contact
                    List<SFContact> sfContacts = sapService.GetSFUpdInsertContacts(sapContacts);

                    //Step-3: UpdInsert SF.Account
                    sfAccChanges = sfService.UpdInsertAccounts(sfAccounts);

                    //Step-4: UpInsert SF.Contact(It includes 4 sub-steps)
                    string contactSyncResult = sfService.UpInsertContactMain(sfContacts, sfAccChanges);

                    //Step-5: Set next session date 
                    if (sfService.SFAllProcessSuccessed)
                    {
                        transResult = "SUCCESS";
                        if (ddiClient.NeedCreateNextTransPlan == "YES")
                        {
                            SQLService sqlService = new SQLService(ddiClient.RunCondition);
                            sqlService.SetSyncTransPlan(ddiClient.TransDateEnd, "SUCCESS", "SF.Account and SF.Contact updated");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Messaging errProcess = new Messaging(DateTime.Now.ToLongTimeString(), "Sapphire sync to SalesForce");
                string errMsg = "TransPanel.MainPanel() met an issue:";
                errMsg += Environment.NewLine;
                errMsg += ex.Message;
                errProcess.MsgHandler(msgType: "SYS-ERROR", errMsg);
            }

            return transResult;
        }

    }
}
