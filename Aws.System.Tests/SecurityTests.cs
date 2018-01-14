using Aws.System;
using Aws.System.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace aws_api_gateway
{
    [TestClass]
    public class SecurityTests
    {
        [TestMethod]
        public void Can_Get_Credentials_From_Credential_File()
        {
            var security = new Security();
            var profile = DefaultAwsProfile.Get();
            var credentials = security.GetAwsCredentials(profile);
            Assert.IsNotNull(credentials);
        }
    }
}
