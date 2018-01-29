using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace Aws.System
{
    internal class GatewayRequest : IRequest
    {
        private AwsApiGatewayRequest publicRequest;
        private bool useQueryString;

        public string RequestName { get; set; }
        public string ServiceName { get; set; }
        public string HttpMethod { get; set; }
        public Uri Endpoint { get; set; }
        public string ResourcePath { get; set; }
        public byte[] Content { get; set; }
        public bool Suppress404Exceptions { get; set; }
        public bool UseChunkEncoding { get; set; }
        public string CanonicalResourcePrefix { get; set; }
        public bool UseSigV4 { get; set; }
        public RegionEndpoint AlternateEndpoint { get; set; }
        public AWS4SigningResult AWS4SignerResult { get; set; }
        public string AuthenticationRegion { get; set; }
        public bool SetContentFromParameters { get; set; }
        public Stream ContentStream { get; set; }
        public long OriginalStreamPosition { get; set; }

        public ParameterCollection ParameterCollection { get; private set; }
        public AmazonWebServiceRequest OriginalRequest { get; private set; }
        public string ContentStreamHash { get; private set; }

        public IDictionary<string, string> Headers { get; private set; }
        public IDictionary<string, string> Parameters { get; private set; }
        public IDictionary<string, string> SubResources { get; private set; }

        public GatewayRequest(AwsApiGatewayRequest publicRequest, string awsServiceName)
        {
            this.publicRequest = publicRequest ?? throw new ArgumentNullException(nameof(publicRequest));
            if (string.IsNullOrEmpty(awsServiceName)) throw new ArgumentNullException(nameof(awsServiceName));

            ServiceName = awsServiceName;
            OriginalRequest = publicRequest;
            RequestName = OriginalRequest.GetType().Name;
            UseSigV4 = ((IAmazonWebServiceRequest)OriginalRequest).UseSigV4;
            Headers = publicRequest.Headers;
            Endpoint = publicRequest.Endpoint;

            ParameterCollection = new ParameterCollection();            
        }

        public void AddHeaders(string key, string value)
        {
            Headers.Add(key, value);
        }

        public bool UseQueryString
        {
            get
            {
                if (HttpMethod == "GET")
                    return true;
                return useQueryString;
            }
            set
            {
                useQueryString = value;
            }
        }        

        public void AddParameters(string key, string value)
        {
            Parameters.Add(key, value);
        }

        public void AddSubResource(string subResource)
        {
            AddSubResource(subResource, null);
        }

        public void AddSubResource(string subResource, string value)
        {
            SubResources.Add(subResource, value);
        }

        public string ComputeContentStreamHash()
        {
            if (ContentStream == null)
                return null;

            if (ContentStreamHash == null)
            {
                var seekableStream = WrapperStream.SearchWrappedStream(ContentStream, s => s.CanSeek);
                if (seekableStream != null)
                {
                    var position = seekableStream.Position;
                    byte[] payloadHashBytes = CryptoUtilFactory.CryptoInstance.ComputeSHA256Hash(seekableStream);
                    ContentStreamHash = AWSSDKUtils.ToHex(payloadHashBytes, true);
                    seekableStream.Seek(position, SeekOrigin.Begin);
                }
            }

            return ContentStreamHash;
        }

        public bool HasRequestBody()
        {
            var isPutPost = (HttpMethod == "POST" || HttpMethod == "PUT" || HttpMethod == "PATCH");
            var hasContent = this.HasRequestData();
            return (isPutPost && hasContent);
        }

        public bool IsRequestStreamRewindable()
        {
            var stream = ContentStream;            
            if (stream != null)
            {                
                stream = WrapperStream.GetNonWrapperBaseStream(stream);                
                return stream.CanSeek;
            }

            return true;
        }

        public bool MayContainRequestBody()
        {
            return
                (HttpMethod == "POST" ||
                 HttpMethod == "PUT" ||
                 HttpMethod == "PATCH");
        }
    }
}