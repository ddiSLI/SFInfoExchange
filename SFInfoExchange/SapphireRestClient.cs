using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SFInfoExchange
{
    public class SapphireRestClient
    {
        private readonly RestClient _sapphireClient;
        public SapphireRestClient()
        {
            //string endpointUrl="http://prod.datagen.com", string endpointPort="4001"
            string runCondition = ConfigurationManager.AppSettings["RunCondition"];
            string endpointUrl = "";
            string endpointPort = "";

            if (runCondition == "PROD")
            {
                endpointUrl = ConfigurationManager.AppSettings["SapphireClient_endpointUrl_Prod"];
                endpointPort = ConfigurationManager.AppSettings["SapphireClient_endpointPort_Prod"];
            }
            else
            {
                endpointUrl = ConfigurationManager.AppSettings["SapphireClient_endpointUrl_Dev"]; 
                endpointPort = ConfigurationManager.AppSettings["SapphireClient_endpointPort_Dev"];
            }
            
            _sapphireClient = new RestClient($"{endpointUrl}:{endpointPort}");
        }

        public IEnumerable<dynamic> GetDoctor(int page)
        {
            //Sapphire.Doctors
            //SF.Accounts info 
            var request = new RestRequest("doctor3", Method.GET);
                       
            request.AddQueryParameter("skip", (page*100).ToString());
            request.AddQueryParameter("take", "100");

            var response = _sapphireClient.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new Exception(response.Content);
            }

            return (IEnumerable<dynamic>)JsonConvert.DeserializeObject(response.Content);
        }

        public IEnumerable<dynamic> GetDoctor_doctor2(string clientId, DateTime? lastModified)
        {
            //Sapphire.Doctors
            //SF.Accounts info 
            var request = new RestRequest("doctor2", Method.GET);
                      
            if (lastModified != null)
            {
                request.AddQueryParameter("lastModified", lastModified.Value.ToString("f"));
            }

            var response = _sapphireClient.Execute(request);

            if(!response.IsSuccessful)
            {
                throw new Exception(response.Content);
            }

            return (IEnumerable<dynamic>)JsonConvert.DeserializeObject(response.Content);
        }

        public IEnumerable<dynamic> GetSubAccount(string clientId, DateTime? previousModified, DateTime? lastModified)
        {
            //Sapphire.SubAccounts
            //SF.Contacts info
            var request = new RestRequest("subaccount", Method.GET);

            if (!string.IsNullOrWhiteSpace(clientId))
            {
                request.AddQueryParameter("clientId", "12345");
            }

            //
            if (previousModified.Value.Year != 1900)
            {
                request.AddQueryParameter("previousModified", previousModified.Value.ToString("f"));
            }
            //

            if (lastModified != null)
            {
                request.AddQueryParameter("lastModified", lastModified.Value.ToString("f"));
            }

            var response = _sapphireClient.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new Exception(response.Content);
            }

            return (IEnumerable<dynamic>)JsonConvert.DeserializeObject(response.Content);
        }
    }
}
