using Amazon;
using Amazon.APIGateway;
using Amazon.Runtime;
using RestSharp;
using RestSharp.Authenticators;

namespace Aws.System
{
    public class IamAuthenticator : IAuthenticator
    {
        private readonly AWSCredentials _AWSCredentials;
        private readonly RegionEndpoint _RegionEndpoint;
        public const string AuthorizationHeader = "Authorization";
        public const string AmazonDateHeader = "X-Amz-Date";

        public IamAuthenticator(AWSCredentials awsCredentials, RegionEndpoint region)
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
            var config = new AmazonAPIGatewayConfig() { RegionEndpoint = _RegionEndpoint };            
            var request = GetGatewayApiRequest(restsharpRequest, restsharpClient);

            var signature = GatewayClient.SignRequest(request, config, _AWSCredentials.GetCredentials());
            restsharpRequest.AddHeader(AuthorizationHeader, signature.ForAuthorizationHeader);
            restsharpRequest.AddHeader(AmazonDateHeader, signature.ISO8601DateTime);
            restsharpRequest.AddHeader("Content-Type", "application/json");
        }

        private GatewayRequest GetGatewayApiRequest(IRestRequest restsharpRequest, IRestClient restsharpClient)
        {
            var gw = new GatewayRequest(GetPublicRequest(restsharpRequest, restsharpClient), Constants.AwsServiceName);
            gw.AuthenticationRegion = _RegionEndpoint.SystemName;
            gw.ServiceName = "execute-api";
            return gw;
        }        

        /// <summary>
        /// takes the request info from RestSharp and maps to
        /// a class that aws understands
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private AwsApiGatewayRequest GetPublicRequest(IRestRequest request, IRestClient client)
        {
            var publicRequest = new AwsApiGatewayRequest();
            publicRequest.HttpMethod = request.Method.ToString();
            publicRequest.ResourcePath = request.Resource;
            
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

            publicRequest.Endpoint = client.BaseUrl;
            publicRequest.UseSig4 = true;
            
            return publicRequest;
        }
    }
}
