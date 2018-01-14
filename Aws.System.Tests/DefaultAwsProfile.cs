

using System;

namespace Aws.System.Tests
{
    public class DefaultAwsProfile
    {
        internal static Profile Get() => new Profile() { Path = "e:\\awsuser\\.aws\\credentials", Name = "default" };
    }
}
