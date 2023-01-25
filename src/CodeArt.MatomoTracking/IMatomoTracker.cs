using CodeArt.MatomoTracking.Interfaces;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace CodeArt.MatomoTracking
{
    public interface IMatomoTracker
    {
        string GeneratePageViewId();
        Task Track(Action<NameValueCollection> SetParams);
        Task Track(ITrackingItem trackingItem);
    }
}