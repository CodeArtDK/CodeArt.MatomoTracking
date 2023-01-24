using CodeArt.MatomoTracking.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeArt.MatomoTracking.Models
{
    public class EventTrackingItem : BaseTrackingItem
    {
        [QueryParameter("e_c")]
        public string EventCategory { get; set; } //e_c Event Category
        [QueryParameter("e_a")]
        public string EventAction { get; set; } //e_a Event Action
        [QueryParameter("e_n")]
        public string EventName { get; set; }   //e_n Event Name
        
        [QueryParameter("e_v")]
        public int? EventValue { get; set; } //e_v Event Value
    }
}
