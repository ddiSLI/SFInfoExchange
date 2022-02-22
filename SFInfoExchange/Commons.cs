using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;


namespace SFInfoExchange
{
    public static class Commons
    {
        public static bool IsDateTime(string dateString)
        {
            bool isDateTime = false;
            string[] formats = {"yyyy/dd/MM",
                    "M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                   "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                   "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                   "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                   "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm"};

            DateTime dateTime;

            if (DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out dateTime))
                isDateTime = true;

            return isDateTime;
        }

        public static string formatPhoneNumber(string phoneNum, string phoneFormat)
        {
            string countryNum = "";
            string sourcePhoneNume = phoneNum;

            if (string.IsNullOrEmpty(phoneNum))
                return "";

            if (phoneFormat == "")
            {
                // If phone format is empty, code will use default format (###) ###-####
                phoneFormat = "(###) ###-####";
            }

            // First, remove everything except of numbers
            Regex regexObj = new Regex(@"[^\d]");
            phoneNum = regexObj.Replace(phoneNum, "");

            if (phoneNum.Length <= 10)
            {
                countryNum = phoneNum.Substring(0, phoneNum.Length - 10);
                phoneNum = phoneNum.Substring(phoneNum.Length - 10);

                // Second, format numbers to phone string
                if (phoneNum.Length > 0)
                {
                    phoneNum = Convert.ToInt64(phoneNum).ToString(phoneFormat);
                }
            }
            else
            {
                phoneNum = sourcePhoneNume;
                //countryNum = "+" + countryNum;
            }

            return countryNum + phoneNum;
        }

        public static void LogWrite(string logFile, string logMessage)
        {
            //string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))

            try
            {
                using (StreamWriter w = File.AppendText(logFile))
                {
                    LogAppend(logMessage, w);
                }
            }
            catch (Exception ex)
            {
                Messaging errProcess = new Messaging(DateTime.Now.ToLongTimeString(), "Sapphire sync to SalesForce");
                errProcess.MsgHandler(msgType: "SYS-ERROR", "Commons.LogWrite() error: " + ex.Message);
            }
        }

        private static void LogAppend(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
                Messaging errProcess = new Messaging(DateTime.Now.ToLongTimeString(), "Sapphire sync to SalesForce");
                errProcess.MsgHandler(msgType: "SYS-ERROR", "Commons.LogAppend() error: " + ex.Message);
            }
        }


    }
}
