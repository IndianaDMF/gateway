using AwsSig4Authentication;
using RestSharp;
using System;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://420.execute-api.us-east-2.amazonaws.com"; // replace with your url
            string resource = "/"; // replace with your resource path
            var jsonBody = new
            {
               // your content
            }; 

            ServiceClient service = new ServiceClient();
            var client = service.GetClient(url);
            var request = service.GetRequest();

            request.Method = Method.POST;
            request.Resource = resource;
            //request.AddHeader("xYourHeader", "");
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

        public RestClient GetClient(string baseUrl)
        {
            var credentials = new AwsApiKey()
            {
                AccessKey = "", // fill in your IAM API Token Access Key
                SecretKey = "", // fill in your IAM API Token Secret Key
                Region = "" // fill in your service region
            };

            return new RestClient(baseUrl)
            {
                Authenticator = new Sig4Authenticator(credentials)
            };
        }
    }
}
