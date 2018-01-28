using Amazon.APIGateway;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;
using System;
using System.Collections.Generic;

namespace Aws.System
{
    internal partial class AwsApiGatewayRequest : AmazonAPIGatewayRequest
    {
        public string ResourcePath { get; internal set; }

        public string HttpMethod { get; internal set; }

        public IDictionary<string, string> Headers { get; internal set; }

        public Uri Endpoint { get; internal set; }
        public bool UseQueryString { get; internal set; }
        public IDictionary<string, string> Parameters { get; internal set; }
        public AwsApiGatewayRequest()
        {
            this.Parameters = new Dictionary<string, string>();
            this.Headers = new Dictionary<string, string>();
        }
    }

    internal partial class AwsApiGatewayRequestMarshaller : IMarshaller<IRequest, AwsApiGatewayRequest>, IMarshaller<IRequest, AmazonWebServiceRequest>
    {
        public IRequest Marshall(AmazonWebServiceRequest input)
        {
            return Marshall((AwsApiGatewayRequest)input);
        }

        public IRequest Marshall(AwsApiGatewayRequest publicRequest)
        {
            IRequest request = new DefaultRequest(publicRequest, Constants.AwsServiceName);
            request.HttpMethod = publicRequest.HttpMethod;
            foreach (var header in publicRequest.Headers)
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
