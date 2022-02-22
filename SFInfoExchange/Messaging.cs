using System;

namespace SFInfoExchange
{
    class Messaging
    {
        private string MsgId { get; set; }
        private string Category { get; set; }
        public Messaging(string msgId= null, string category=null)
        {
            //MsgId,current time;
            //Category,which program
            MsgId = msgId;
            Category = category;
        }
        
        public string MsgHandler(string msgType, string msgTxt)
        {
            string actionResult = "NA";
            SendEmail mailSending = new SendEmail();

            if (msgType == "SYS-ERROR")
            {
                mailSending.Mail_Subject = "SYS Error(s) from SF-Sync";
                mailSending.Mail_Message += "Error happened: " + MsgId;
                mailSending.Mail_Message += Environment.NewLine;
                mailSending.Mail_Message += "Category is: " + Category;
                mailSending.Mail_Message += Environment.NewLine;
                mailSending.Mail_Message += msgTxt;
                mailSending.Mail_Message += Environment.NewLine;
                mailSending.SendTextMail_ErrorMsg();

            }
            else if (msgType == "ISSUE-MAIN")
            {
                mailSending.Mail_Subject = "Salesforce Sync Issue(s)";
                mailSending.Mail_Message += "MsgId is: " + MsgId;   
                mailSending.Mail_Message += Environment.NewLine;
                mailSending.Mail_Message += msgTxt;
                mailSending.Mail_Message += Environment.NewLine;
                mailSending.SendTextMail_ErrorMsg();

                actionResult = "PROCESSED";
            }
            else if (msgType == "SF-SYNC-SAPPHIRE")
            {
                mailSending.Mail_Subject = "Sapphire Data Issues from SF-Sync";
                mailSending.Mail_Message += "Issue(s) happened: " + MsgId;
                mailSending.Mail_Message += Environment.NewLine;
                mailSending.Mail_Message += msgTxt;
                mailSending.Mail_Message += Environment.NewLine;
                mailSending.SendTextMail_ErrorMsg();

                actionResult = "PROCESSED";
            }
            else if (msgType == "SF-SYNC-SF")
            {
                mailSending.Mail_Subject = "Salesforce Data Issues from SF-Sync";
                mailSending.Mail_Message += "Issue(s) happened: " + MsgId;
                mailSending.Mail_Message += Environment.NewLine;
                mailSending.Mail_Message += msgTxt;
                mailSending.Mail_Message += Environment.NewLine;
                mailSending.SendTextMail_ErrorMsg();

                actionResult = "PROCESSED";
            }
            else if (msgType == "SYS-NOTICE")
            {
                mailSending.Mail_Subject = "Salesforce Sync met an issue:";
                mailSending.Mail_Message += "MsgId is: " + MsgId;
                mailSending.Mail_Message += Environment.NewLine;
                mailSending.Mail_Message += msgTxt;
                mailSending.Mail_Message += Environment.NewLine;
                mailSending.SendTextMail_ErrorMsg(msgType);

            }

            return actionResult;
        }

    }
}
