using System;
using System.Collections.Generic;
using System.Text;

namespace CodeArt.MatomoTracking.Models
{
    public class PageViewTrackingItem : BaseTrackingItem
    {

        public int NetworkTime { get; set; } //pf_net. Network Time. How long it took to connect to server.
        public int ServerTime { get; set; } //pf_srv.
        public int TransferTime { get; set; } //pf_tfr. Transfer Time
        public int DOMProcessingTime { get; set; } //pf_dm1
        public int DOMCompletionTime { get; set; } //pf_dm2
        public int OnloadTime { get; set; } //pf_onl
    }
}
