using CodeArt.MatomoTracking.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeArt.MatomoTracking.Models
{
    public class PageViewTrackingItem : BaseTrackingItem
    {
        /// <summary>
        /// Network time. How long it took to connect to server.
        /// </summary>
        [QueryParameter("pf_net")]
        public int? NetworkTime { get; set; } //pf_net. Network Time. How long it took to connect to server.

        /// <summary>
        /// Server time. How long it took the server to generate page.
        /// </summary>
        [QueryParameter("pf_srv")]
        public int? ServerTime { get; set; } //pf_srv.

        /// <summary>
        /// Transfer time. How long it takes the browser to download the response from the server
        /// </summary>
        [QueryParameter("pf_tfr")]
        public int? TransferTime { get; set; } //pf_tfr. Transfer Time

        /// <summary>
        /// Dom processing time. How long the browser spends loading the webpage after the response was fully received until the user can starting interacting with it.
        /// </summary>
        [QueryParameter("pf_dm1")]
        public int? DOMProcessingTime { get; set; } //pf_dm1

        /// <summary>
        /// Dom completion time. How long it takes for the browser to load media and execute any Javascript code listening for the DOMContentLoaded event.
        /// </summary>
        [QueryParameter("pf_dm2")]
        public int? DOMCompletionTime { get; set; } //pf_dm2

        /// <summary>
        /// Onload time. How long it takes the browser to execute Javascript code waiting for the window.load event.
        /// </summary>
        [QueryParameter("pf_onl")]
        public int? OnloadTime { get; set; } //pf_onl
    }
}
