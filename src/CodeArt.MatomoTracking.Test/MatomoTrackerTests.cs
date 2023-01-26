using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeArt.MatomoTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.Extensions.Options;

namespace CodeArt.MatomoTracking.Tests
{
    [TestClass()]
    public class MatomoTrackerTests
    {
        private MatomoTracker matomoTracker;
        private Mock<HttpMessageHandler> messageHandlerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            Mock<IOptions<MatomoOptions>> options = new Mock<IOptions<MatomoOptions>>();
            options.SetupGet(x => x.Value).Returns(new MatomoOptions()
            {
                MatomoUrl = "http://localhost/matomo.php",
                SiteId = "1"
            });
            Mock<IServiceProvider> services = new Mock<IServiceProvider>();
            messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            matomoTracker = new MatomoTracker(services.Object,options.Object,new HttpClient(messageHandlerMock.Object));
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }


        [TestMethod()]
        public void TrackTest()
        {
            
        }

        [TestMethod()]
        public void TrackTest1()
        {
            
        }

        [TestMethod()]
        public void TrackTest2()
        {
            
        }

        [TestMethod()]
        public void GeneratePageViewIdTest()
        {
            var pid=matomoTracker.GeneratePageViewId();
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