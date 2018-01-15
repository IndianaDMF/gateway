using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aws.System.Tests
{
    [TestClass]
    public class GatewayTests
    {

        [TestMethod]
        public void Can_List_Api()
        {            
            var gateway = new Gateway(DefaultAwsProfile.GetRunTimeCredentials());
            gateway.Something();            
        }
    }
}
