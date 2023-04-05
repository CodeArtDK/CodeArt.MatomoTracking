using System;
using System.Collections.Generic;
using System.Text;

namespace CodeArt.MatomoTracking
{
    /// <summary>
    /// Matomo options
    /// </summary>
    public class MatomoOptions
    {
        /// <summary>
        /// The url to the Matomo server
        /// </summary>
        public string MatomoHostname { get; set; }


        /// <summary>
        /// The site ID on the matomo server
        /// </summary>
        public string SiteId { get; set; }


        /// <summary>
        /// The auth token needed for certain matomo settings. Ensure this auth token has write access if you use it for tracking.
        /// </summary>
        public string AuthToken { get; set; }


        /// <summary>
        /// Enable this to also track bots (disabled by default)
        /// </summary>
        public bool TrackBots { get; set; } = false;

        
        /// <summary>
        /// Turn on verbose logging of all tracking requests
        /// </summary>
        public bool VerboseLogging { get; set; } = false;


    }
}
