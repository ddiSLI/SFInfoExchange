using System;
using System.Text;
using System.Configuration;
using System.IO;

namespace SFInfoExchange
{
    class SendEmail
    {
        public string FileNum_Start { get; set; } = "<AUTOMAIL>";
        public string Mail_Recipient { get; set; }
        public string Mail_cc { get; set; }
        public string Mail_Attachment { get; set; }
        public string Mail_Subject { get; set; }
        public string Mail_Message { get; set; }
        public string FileNum_End { get; set; } = "</AUTOMAIL>";

        public string TextFileMailLocation { get; set; }
        public SendEmail()
        {
            //F:\APPS\Automail\Malfiles\
            TextFileMailLocation = ConfigurationManager.AppSettings["TextFileMailLocation"].ToString();
        }

        public string SendTextMail_ErrorMsg(string mailType = null)
        {
            string actionResult = "NA";
            Mail_Recipient = ConfigurationManager.AppSettings["ErrorMailRecipient"].ToString();
            string errorFile = TextFileMailLocation + "SF_Sync_Error.txt";
            string errorFileMal = TextFileMailLocation + "SF_Sync_Error.mal";
            StringBuilder sbMail = new StringBuilder();

            try
            {
                if (mailType == "SYS-NOTICE")
                {
                    Mail_Recipient = ConfigurationManager.AppSettings["NoticeMailRecipient"].ToString();
                }


                if (File.Exists(errorFile))
                    File.Delete(errorFile);

                if (File.Exists(errorFileMal))
                    File.Delete(errorFileMal);

                sbMail.Append(FileNum_Start);
                sbMail.AppendLine();

                sbMail.Append(Mail_Recipient);
                sbMail.AppendLine();

                sbMail.AppendLine();   ////sbMail.Append(Mail_cc);

                sbMail.AppendLine();   //sbMail.Append(Mail_Attachment);

                sbMail.Append(Mail_Subject);
                sbMail.AppendLine();

                sbMail.Append(Mail_Message);
                sbMail.AppendLine();

                sbMail.Append(FileNum_End);

                File.WriteAllText(errorFile, sbMail.ToString());
                //Path.ChangeExtension(errorFile, ".mal");
                File.Move(errorFile, Path.ChangeExtension(errorFile, ".mal"));
            }
            catch (Exception ex)
            {
                string msgErr = ex.Message;
            }

            return actionResult;
        }
    }
}
