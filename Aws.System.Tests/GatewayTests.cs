﻿using Amazon;
using Amazon.Runtime.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Aws.System.Tests
{
    [TestClass]
    public class GatewayTests
    {
        private readonly string uri = "https://412ebgbtud.execute-api.us-east-2.amazonaws.com";
        private readonly string resource = "/UAT/api/v1/Quote"; 
        
        [TestMethod]
        public void Can_Call_Quote_Api_Service_With_WebRequest()
        {   
            RestClient client = getClient();
            var restRequest = getQuoteRequest();
            var response = client.Execute(restRequest);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);  
        }

        [TestMethod]
        public void Can_Call_Health_Api_Service_With_WebRequest()
        {
            RestClient client = getClient();
            var restRequest = getHealthRequest();
            var response = client.Execute(restRequest);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        private RestRequest getHealthRequest()
        {
            RestRequest request = new RestRequest();
            request.Method = Method.GET;                        
            request.Resource = "/UAT/api/health/status"; 
            return request;
        }

        private RestRequest getQuoteRequest()
        {
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("X-QuoteService-Auth", "AuthFLA");            
            request.Resource = resource;
            var body = new QuoteRequest()
            {
                DateOfBirth = "2/29/1960",
                FaceAmount = 150000,
                Gender = "M",
                PolicyLength = "10",
                Smoker = false,
                State = "WA"
            };

            request.AddBody(body);

            return request;
        }

        private RestClient getClient()
        {
            return new RestClient(uri)
            {
                Authenticator = new IamAuthenticator(DefaultAwsProfile.GetRunTimeCredentials(), RegionEndpoint.USEast2)
            };
        }
    }

    public class QuoteRequest
    {
        public string DateOfBirth { get; set; }

        public int FaceAmount { get; set; }

        public string Gender { get; set; }

        public string PolicyLength { get; set; }

        public bool Smoker { get; set; }

        public string State { get; set; }
    }
}