using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using System;

namespace Aws.System
{
    public class Security
    {
        public AWSCredentials GetAwsCredentials(Profile profile)
        {
            //https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html
            var chain = new CredentialProfileStoreChain(profile.Path);
            AWSCredentials awsCredentials;
            chain.TryGetAWSCredentials(profile.Name, out awsCredentials);
            return awsCredentials;
        }
    }

    public class Profile
    {
        public string Name { get; set; }

        public string Path { get; set; }
    }
}
