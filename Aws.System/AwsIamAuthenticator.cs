using Amazon;
using Amazon.APIGateway;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using RestSharp;
using RestSharp.Authenticators;

namespace Aws.System
{
    public class AwsIamAuthenticator : IAuthenticator
    {
        private readonly AWSCredentials _AWSCredentials;
        private readonly RegionEndpoint _RegionEndpoint;
        public AwsIamAuthenticator(AWSCredentials awsCredentials, RegionEndpoint region)
        {
            _AWSCredentials = awsCredentials;
            _RegionEndpoint = region;
        }

        /// <summary>
        /// Invoked during the execution of <see cref="IRestClient.Execute(IRestRequest)"/>
        /// </summary>
        /// <param name="restsharpClient"></param>
        /// <param name="restsharpRequest"></param>
        public void Authenticate(IRestClient restsharpClient, IRestRequest restsharpRequest)
        {
            var gateway = new Gateway();
            var config = new AmazonAPIGatewayConfig();
            config.RegionEndpoint = _RegionEndpoint;

            var publicRequest = GetPublicRequest(restsharpRequest);
            var azRequest = new DefaultRequest(publicRequest, Constants.AwsServiceName);

            var sign = gateway.SignRequest(azRequest, config, _AWSCredentials.GetCredentials());
            restsharpRequest.AddHeader("Authorization", sign.ForAuthorizationHeader);
        }

        /// <summary>
        /// takes the request info from RestSharp and maps to
        /// a class that aws understands
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private InvokeRestRequest GetPublicRequest(IRestRequest request)
        {
            var publicRequest = new InvokeRestRequest();
            publicRequest.HttpMethod = request.Method.ToString();
            publicRequest.ResourcePath = request.Resource;

            //headers
            foreach (var parameter in request.Parameters)
            {
                switch (parameter.Type)
                {
                    case ParameterType.HttpHeader:
                        publicRequest.Headers.Add(parameter.Name, parameter.Value.ToString());
                        break;
                    case ParameterType.QueryString:
                        publicRequest.UseQueryString = true;
                        publicRequest.Parameters.Add(parameter.Name, parameter.Value.ToString());
                        break;
                }
            }

            // publicRequest.Endpoint = //not sure where i get this from
            return publicRequest;
        }
    }
}
