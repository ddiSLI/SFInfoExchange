using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace SFInfoExchange
{
    class Program
    {
        static void Main(string[] args)
        {
            string exchangeResults = "NA";
            //args: args[0], TransDateStart(YYYY/MM/DD);
            //      args[1], TransDateEnd(YYYY/MM/DD);
            //      args[2], NeedCreateNextTransPlan(YES/NO);
            string argsString = "";

            try
            {
                TransPanel transPanel = new TransPanel();
                string startDT = null;
                string endDT = null;
                string needCreateNextTransPlan = null;

                if (args.Length==1 && Commons.IsDateTime(args[0]))
                {
                    startDT = args[0];
                    argsString = "args[0], " + args[0] + "; ";
                }
                else if (args.Length == 2 && Commons.IsDateTime(args[0])) 
                {
                    startDT = args[0];
                    if (Commons.IsDateTime(args[1]))
                    {
                        endDT = args[1];
                    }
                    else
                    {
                        endDT = args[0];
                    }
                    argsString = "args[0], " + args[0] + "; ";
                    argsString += "args[1], " + args[1] + "; ";
                }
                else if (args.Length == 3 && Commons.IsDateTime(args[0]))
                {
                    startDT = args[0];
                    if (Commons.IsDateTime(args[1]) &&  Convert.ToDateTime(args[0]) <= Convert.ToDateTime(args[1]))
                    {
                        endDT = args[1];
                    }
                    else
                    {
                        endDT = null;
                        Console.WriteLine("endDate must greater than startDate!");

                        return;
                    }

                    if (args[2].ToUpper().Trim() == "NO")
                    {
                        needCreateNextTransPlan = "NO";
                    }
                    else if (args[2].ToUpper().Trim() == "YES")
                    {
                        needCreateNextTransPlan = "YES";
                    }

                    argsString = "args[0], " + args[0] + "; ";
                    argsString += "args[1], " + args[1] + "; ";
                    argsString += "args[2], " + args[2] + "; ";
                }

                exchangeResults = transPanel.MainPanel(startDT, endDT, needCreateNextTransPlan);

                //Process the info of exchangeResults
                //
            }
            catch (Exception ex)
            {
                Messaging errProcess = new Messaging(DateTime.Now.ToLongTimeString(), "Sapphire sync to SalesForce");
                string errMsg = "SFInfoExchange.Main() met an issue: ";
                errMsg += Environment.NewLine;
                errMsg += ex.Message;
                errMsg += Environment.NewLine;
                errMsg += Environment.NewLine;
                errMsg += "args[].length()=" + args.Length.ToString();
                errMsg += Environment.NewLine;
                errMsg += argsString;
                errProcess.MsgHandler(msgType: "SYS-ERROR", errMsg);
            }

        }
    }
}
