using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using ZendeskApi_v2;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Configuration;
using RestSharp;
using Json;
using ZendeskApi_v2.Models.Constants;
using ZendeskApi_v2.Models.Shared;
using ZendeskApi_v2.Models.Tickets;
using System.Xml;
using System.Data;
using Newtonsoft.Json;

namespace ZenDeskSync
{
    class Program
    {
        static string requestUri = "https://aspiresupport.zendesk.com";
        static string username = ConfigurationManager.AppSettings["username"];
        static string password = ConfigurationManager.AppSettings["password"];

        static void Main(string[] args)
        {
            ZDRequest("tickets");
            ZDRequest("users");
            Console.ReadLine();
        }

        public static void ZDRequest(string source)
        {
            var client = new RestClient(requestUri);
            client.Authenticator = new HttpBasicAuthenticator(username, password);
            var connection = "/api/v2/" + source + ".json";

            var request = new RestRequest(connection, Method.GET);
            client.AddDefaultHeader("Accept", "application/json");
            IRestResponse response = client.Execute(request);

            var content = response.Content;
            JObject ticket = JObject.Parse(content);

            string path = @"C:\\Users\\dhess\\desktop\\ZD" + source + ".txt";
            using (StreamWriter sw = File.AppendText(path))
            {
                foreach (var pair in ticket)
                {
                    sw.WriteLine("{0} : {1}", pair.Key, pair.Value);
                }
            }
            Console.WriteLine("File ZD{0}.txt created.", source);
        }
    }
}

