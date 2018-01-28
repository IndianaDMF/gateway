using Amazon.APIGateway;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;
using System;
using System.Collections.Generic;

namespace Aws.System
{
    public class Constants
    {
        public static readonly string AwsServiceName = "Amazon.APIGateway";
    }

    public class Gateway
    {
        public AWS4SigningResult SignRequest(IRequest request, IClientConfig config, ImmutableCredentials creds)
        {
            var requestMetrics = new RequestMetrics();
            AWS4Signer signer = new AWS4Signer(true);
            return signer.SignRequest(request, config, requestMetrics, creds.AccessKey, creds.SecretKey);            
        }   
    }

    public class CredentialArgumentException : ArgumentException
    {
        public CredentialArgumentException(string message) : base(message) { }
    }

    /// <summary>
    /// Container for the parameters to the InvokeRestRequest operation.
    /// </summary>
    public partial class InvokeRestRequest : AmazonAPIGatewayRequest
    {
        public string ResourcePath { get; internal set; }

        public string HttpMethod { get; internal set; }

        public IDictionary<string,string> Headers { get; internal set; }

        public Uri Endpoint { get; internal set; }
        public bool UseQueryString { get; internal set; }
        public IDictionary<string, string> Parameters { get; internal set; }
    }

    public partial class InvokeRestRequestMarshaller : IMarshaller<IRequest, InvokeRestRequest>, IMarshaller<IRequest, AmazonWebServiceRequest>
    {
        public IRequest Marshall(AmazonWebServiceRequest input)
        {
            return this.Marshall((InvokeRestRequest)input);
        }

        public IRequest Marshall(InvokeRestRequest publicRequest)
        {
            IRequest request = new DefaultRequest(publicRequest, Constants.AwsServiceName);
            request.HttpMethod = publicRequest.HttpMethod; 
            foreach(var header in publicRequest.Headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            foreach (var parameter in publicRequest.Parameters)
            {
                request.Parameters.Add(parameter.Key, parameter.Value);
            }
          
            request.ResourcePath = publicRequest.ResourcePath;
            request.UseQueryString = true;
            

            return request;
        }
    }
}
