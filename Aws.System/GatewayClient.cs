using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Util;

namespace Aws.System
{
    public class GatewayClient
    {
        public AWS4SigningResult SignRequest(IRequest request, IClientConfig config, ImmutableCredentials creds)
        {
            var requestMetrics = new RequestMetrics();
            AWS4Signer signer = new AWS4Signer(true);
            return signer.SignRequest(request, config, requestMetrics, creds.AccessKey, creds.SecretKey);
        }
    }
}
