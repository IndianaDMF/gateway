using Amazon.APIGateway;
using Amazon.APIGateway.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aws.System
{
    public class Gateway
    {
        private readonly AWSCredentials _AWSCredentials;

        public Gateway(AWSCredentials awsCredentials)
        {
            _AWSCredentials = awsCredentials ?? throw new CredentialArgumentException("aws credentials are required");
        }

        public void Something()
        {
            var config = new AmazonAPIGatewayConfig();
            config.RegionEndpoint = Amazon.RegionEndpoint.USEast2;
            
            var client = new AmazonAPIGatewayClient(_AWSCredentials, config);            
            var request = new GetRestApisRequest();
            var blah = client.GetRestApisAsync(request);
            while (!blah.IsCompleted)
            {

            }
            var result = blah.Result;
        }
    }

    public class CredentialArgumentException : ArgumentException
    {
        public CredentialArgumentException(string message) : base(message) { }
    }
}
