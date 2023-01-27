using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeArt.MatomoTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.Extensions.Options;
using Moq.Protected;
using System.Net;
using RichardSzalay.MockHttp;
using CodeArt.MatomoTracking.Models;

namespace CodeArt.MatomoTracking.Tests
{
    [TestClass()]
    public class MatomoTrackerTests
    {
        private MockHttpMessageHandler mockHttp;
        private MatomoTracker matomoTracker;


        [TestInitialize]
        public void TestInitialize()
        {
            var options = new Mock<IOptions<MatomoOptions>>();
            options.SetupGet(x => x.Value).Returns(new MatomoOptions()
            {
                MatomoUrl = "http://localhost/matomo.php",
                SiteId = "1"
            });
            var services = new Mock<IServiceProvider>();
            mockHttp = new MockHttpMessageHandler();
            matomoTracker = new MatomoTracker(services.Object, options.Object, mockHttp.ToHttpClient());

        }

        [TestCleanup]
        public void TestCleanup()
        {
            mockHttp.Clear();
        }


        [TestMethod()]
        public void TrackTest()
        {
            mockHttp.Expect("http://localhost/matomo.php")
                .WithQueryString("action_name", "Test")
                .WithQueryString("idsite","1")
                .WithQueryString("rec","1")
                .WithQueryString("url","http://localhost/abc")
                .Respond("text/plain", "mocked API response");


            var t = matomoTracker.Track(a => {
                a["action_name"] = "Test";
                a["url"] = "http://localhost/abc";
            });
            t.Wait();

            mockHttp.VerifyNoOutstandingExpectation();

        }

        //Test e-commerce
        [TestMethod()]
        public void TrackTestECommerce()
        {
            mockHttp.Expect("http://localhost/matomo.php")
                .WithQueryString("action_name", "Test")
                .WithQueryString("idsite", "1")
                .WithQueryString("rec", "1")
                .WithQueryString("url", "http://localhost/abc")
                .WithQueryString("idgoal", "0")
                .With(h => h.RequestUri.Query.Contains("abc123"))
                .Respond("text/plain", "mocked API response");

            matomoTracker.Track(new EcommerceTrackingItem()
            {
                ActionName = "Test",
                Url = "http://localhost/abc",
                Items = new List<EcommerceTrackingOrderItem>() { new EcommerceTrackingOrderItem() { SKU="abc123", Name="My product", Price=100, Quantity=2, Category="Shoes" } },
                Tax = 123
            }).Wait();


            mockHttp.VerifyNoOutstandingExpectation();
        }

        [TestMethod()]
        public void TrackTest1()
        {
            mockHttp.Expect("http://localhost/matomo.php")
                .WithQueryString("action_name", "Test")
                .WithQueryString("idsite", "1")
                .WithQueryString("rec", "1")
                .WithQueryString("url", "http://localhost/abc")
                .WithQueryString("dimension1","Test")
                .WithQueryString("dimension2","Test2")
                .Respond("text/plain", "mocked API response");

            mockHttp.Expect("http://localhost/matomo.php")
                .WithQueryString("action_name", "Test")
                .WithQueryString("idsite", "1")
                .WithQueryString("rec", "1")
                .WithQueryString("url", "http://localhost/abc")
                .WithQueryString("h", "11")
                .WithQueryString("m", "22")
                .WithQueryString("s","33")
                .Respond("text/plain", "mocked API response");

            matomoTracker.Track(new PageViewTrackingItem()
            {
                ActionName = "Test",
                Url = "http://localhost/abc",
                Dimensions = new Dictionary<int, string>() { { 1, "Test" },{ 2, "Test2" } }
            }).Wait();

            matomoTracker.Track(new PageViewTrackingItem() { 
                ActionName="Test" ,
                Url = "http://localhost/abc",
                UserTime = new TimeSpan(11,22,33)
            }).Wait();
            

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [TestMethod()]
        public void TrackTest2()
        {
            mockHttp.Expect(HttpMethod.Post, "http://localhost/matomo.php")
                .WithPartialContent("dimension1=Test")
                .WithPartialContent("h=11")
                .Respond("text/plain", "mocked API response");

            matomoTracker.Track(new PageViewTrackingItem()
            {
                ActionName = "Test",
                Url = "http://localhost/abc",
                Dimensions = new Dictionary<int, string>() { { 1, "Test" }, { 2, "Test2" } }
            }, new PageViewTrackingItem()
            {
                ActionName = "Test",
                Url = "http://localhost/abc",
                UserTime = new TimeSpan(11, 22, 33)
            }).Wait();


            mockHttp.VerifyNoOutstandingExpectation();
        }

        [TestMethod()]
        public void GeneratePageViewIdTest()
        {
            var pid =matomoTracker.GeneratePageViewId();
            Assert.IsNotNull(pid);
            Assert.IsTrue(pid.Length == 6);
        }

        [TestMethod()]
        public void GenerateVisitorIDTest()
        {
            var pid = matomoTracker.GenerateVisitorID();
            Assert.IsNotNull(pid);
            Assert.IsTrue(pid.Length == 16);
        }
    }
}