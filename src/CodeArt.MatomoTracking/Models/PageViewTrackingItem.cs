using CodeArt.MatomoTracking.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeArt.MatomoTracking.Models
{
    public class PageViewTrackingItem : BaseTrackingItem
    {
        [QueryParameter("pf_net")]
        public int? NetworkTime { get; set; } //pf_net. Network Time. How long it took to connect to server.

        [QueryParameter("pf_srv")]
        public int? ServerTime { get; set; } //pf_srv.

        [QueryParameter("pf_tfr")]
        public int? TransferTime { get; set; } //pf_tfr. Transfer Time

        [QueryParameter("pf_dm1")]
        public int? DOMProcessingTime { get; set; } //pf_dm1

        [QueryParameter("pf_dm2")]
        public int? DOMCompletionTime { get; set; } //pf_dm2

        [QueryParameter("pf_onl")]
        public int? OnloadTime { get; set; } //pf_onl
    }
}
