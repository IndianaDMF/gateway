using AwsSig4Authentication;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            string uri = "";
            string resource = "";
            var jsonBody = new { };

            ServiceClient service = new ServiceClient();
            var client = service.GetClient(uri);
            var request = service.GetRequest();

            request.Method = Method.POST;
            request.Resource = resource;
            request.AddJsonBody(jsonBody);

            var response = client.Execute(request);
            Console.WriteLine(response.StatusCode);
        }
    }    

    internal class ServiceClient
    {
        public RestRequest GetRequest()
        {
            RestRequest request = new RestRequest();
            return request;
        }

        public RestClient GetClient(string uri)
        {
            var credentials = new AwsApiKey()
            {
                AccessKey = "",
                SecretKey = "",
                Region = ""
            };

            return new RestClient(uri)
            {
                Authenticator = new Sig4Authenticator(credentials)
            };
        }
    }
}
