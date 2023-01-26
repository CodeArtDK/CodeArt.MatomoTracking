using CodeArt.MatomoTracking.Interfaces;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace CodeArt.MatomoTracking
{
    public interface IMatomoTracker
    {
        /// <summary>
        /// Generates a 6 character ID for use as PageViewID
        /// </summary>
        /// <returns></returns>
        string GeneratePageViewId();

        /// <summary>
        /// Generates a 16 character hexadecimal ID for use in VisitorID
        /// </summary>
        /// <returns></returns>
        string GenerateVisitorID();
        
        /// <summary>
        /// Send a tracking request with certain parameters
        /// </summary>
        /// <param name="SetParams">An action that sets the parameters needed</param>
        /// <returns></returns>
        Task Track(Action<NameValueCollection> SetParams);
        
        /// <summary>
        /// Sends a tracking request to Matomo for a tracking item.
        /// </summary>
        /// <param name="trackingItem">This could be a PageViewTrackingItem, EventTrackingItem, EcommerceTrackingItem or ContentTrackingItem</param>
        /// <returns></returns>
        Task Track(ITrackingItem trackingItem);

        /// <summary>
        /// Track multiple tracking items in one request.
        /// </summary>
        /// <param name="trackingItems">Items to track</param>
        /// <returns></returns>
        Task Track(params ITrackingItem[] trackingItems);
    }
}