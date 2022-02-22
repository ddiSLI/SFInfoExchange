using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Configuration;

namespace SFInfoExchange
{
    public class SalesForceClient
    {
        private const string LOGIN_ENDPOINT = "https://login.salesforce.com/services/oauth2/token";
        private const string API_ENDPOINT = "/services/data/v36.0/";

        public string RunCondition { get; set; }
        public string CnsSQL { get; set; }
        public string CnsOracle { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string TokenRequestEndpointUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthToken { get; set; }
        public string InstanceUrl { get; set; }
        public string TransDateStart { get; set; } = DateTime.Now.Date.ToString("yyyy/MM/dd");
        public string TransDateEnd { get; set; }
        public string NeedCreateNextTransPlan { get; set; }
        public string SFTransLog { get; set; }

        public SalesForceClient(string paramDateStart=null, string paramDateEnd=null, string needCreateNextTransPlan=null)
        {

            RunCondition = ConfigurationManager.AppSettings["RunCondition"];

            if (string.IsNullOrEmpty(paramDateStart))
            {
                string syncDateStart = DateTime.Now.ToShortDateString();
                
                SQLService sqlService = new SQLService(RunCondition);
                string[] syncStatus = { "Id", "LastSyncDT", "LastSyncStauts", "LastSyncDesc" };
                syncStatus = sqlService.GetLastSyncInfo();
                if (syncStatus[2] == "SUCCESS")
                {
                    //MM/DD/YYYY
                    TransDateEnd = Convert.ToDateTime(syncStatus[1]).AddDays(1).ToShortDateString();
                }
                else
                {
                    TransDateEnd = syncStatus[1];
                }
            }
            else
            {
                TransDateStart = paramDateStart;
                if (string.IsNullOrEmpty(paramDateEnd))
                {
                    TransDateEnd = paramDateStart;
                }
                else
                {
                    TransDateEnd = paramDateEnd;
                }    

            }

            if (string.IsNullOrEmpty(needCreateNextTransPlan))
            {
                NeedCreateNextTransPlan = ConfigurationManager.AppSettings["NeedCreateNextTransPlan"];
            }
            else
            {
                NeedCreateNextTransPlan = needCreateNextTransPlan;
            }
            
            if (RunCondition == "DEV")
            {
                Username = ConfigurationManager.AppSettings["username_Dev"];
                Password = ConfigurationManager.AppSettings["password_Dev"];
                Token = ConfigurationManager.AppSettings["token_Dev"];
                Password += Token;
                ClientId = ConfigurationManager.AppSettings["clientId_Dev"];
                ClientSecret = ConfigurationManager.AppSettings["clientSecret_Dev"];
                TokenRequestEndpointUrl = ConfigurationManager.AppSettings["TokenRequestEndpointUrl_Dev"];

                SFTransLog= ConfigurationManager.AppSettings["SFTransLog_Dev"];
            }
            else if (RunCondition == "SANDBOX")
            {
                Username = ConfigurationManager.AppSettings["username_Sandbox"];
                Password = ConfigurationManager.AppSettings["password_Sandbox"];
                Token = ConfigurationManager.AppSettings["token_Sandbox"];
                Password += Token;
                ClientId = ConfigurationManager.AppSettings["clientId_Sandbox"];
                ClientSecret = ConfigurationManager.AppSettings["clientSecret_Sandbox"];
                TokenRequestEndpointUrl = ConfigurationManager.AppSettings["TokenRequestEndpointUrl_Sandbox"];

                SFTransLog = ConfigurationManager.AppSettings["SFTransLog_Dev"];
            }
            else if (RunCondition == "PROD")
            {
                Username = ConfigurationManager.AppSettings["username"];
                Password = ConfigurationManager.AppSettings["password"];
                Password += Token;
                Token = ConfigurationManager.AppSettings["token"];
                ClientId = ConfigurationManager.AppSettings["clientId"];
                ClientSecret = ConfigurationManager.AppSettings["clientSecret"];
                TokenRequestEndpointUrl = ConfigurationManager.AppSettings["TokenRequestEndpointUrl_Sandbox"];

                SFTransLog = ConfigurationManager.AppSettings["SFTransLog_Prod"];
            }
        }

        static SalesForceClient()
        {
            //SF Require TLS1.1 or 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

        }

        public void Login()
        {
            String jsonResponse;
            using (var client = new HttpClient())
            {
                var request = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        {"grant_type", "password"},
                        {"client_id", ClientId},
                        {"client_secret", ClientSecret},
                        {"username", Username},
                        {"password", Password + Token}
                    }
                );
                request.Headers.Add("X-PrettyPrint", "1");
                var response = client.PostAsync(LOGIN_ENDPOINT, request).Result;
                jsonResponse = response.Content.ReadAsStringAsync().Result;
            }
            Console.WriteLine($"Response: {jsonResponse}");
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
            AuthToken = values["access_token"];
            InstanceUrl = values["instance_url"];
        }

        public string QueryEndpoints()
        {
            using (var client = new HttpClient())
            {
                string restQuery = InstanceUrl + API_ENDPOINT;
                var request = new HttpRequestMessage(HttpMethod.Get, restQuery);
                request.Headers.Add("Authorization", "Bearer " + AuthToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("X-PrettyPrint", "1");
                var response = client.SendAsync(request).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public string Describe(string sObject)
        {
            using (var client = new HttpClient())
            {
                string restQuery = InstanceUrl + API_ENDPOINT + "sobjects/" + sObject;
                var request = new HttpRequestMessage(HttpMethod.Get, restQuery);
                request.Headers.Add("Authorization", "Bearer " + AuthToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("X-PrettyPrint", "1");
                var response = client.SendAsync(request).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public string Query(string soqlQuery)
        {
            using (var client = new HttpClient())
            {
                string restRequest = InstanceUrl + API_ENDPOINT + "query/?q=" + soqlQuery;
                var request = new HttpRequestMessage(HttpMethod.Get, restRequest);
                request.Headers.Add("Authorization", "Bearer " + AuthToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("X-PrettyPrint", "1");
                var response = client.SendAsync(request).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }










    }
}
