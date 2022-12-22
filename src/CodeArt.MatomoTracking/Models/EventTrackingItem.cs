using System;
using System.Collections.Generic;
using System.Text;

namespace CodeArt.MatomoTracking.Models
{
    public class EventTrackingItem : BaseTrackingItem
    {
        public string EventCategory { get; set; } //e_c Event Category
        public string EventAction { get; set; } //e_a Event Action
        public string EventName { get; set; }   //e_n Event Name
        public int EventValue { get; set; } //e_v Event Value
    }
}
