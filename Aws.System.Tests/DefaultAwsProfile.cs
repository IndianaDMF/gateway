

using Amazon.Runtime;
using System;

namespace Aws.System.Tests
{
    public class DefaultAwsProfile
    {
        internal static Profile GetProfile() => new Profile() { Path = "e:\\awsuser\\.aws\\credentials", Name = "default" };

        internal static AWSCredentials GetRunTimeCredentials()
        {
            var security = new Security();
            var profile = GetProfile();
            var credentials = security.GetAwsCredentials(profile);
            return credentials;
        }
    }
}
