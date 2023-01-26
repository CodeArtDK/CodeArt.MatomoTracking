using CodeArt.MatomoTracking.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeArt.MatomoTracking.Models
{
    public class EventTrackingItem : BaseTrackingItem
    {
        /// <summary>
        /// The event category. Must not be empty. (eg. Videos, Music, Games...)
        /// </summary>
        [QueryParameter("e_c")]
        public string EventCategory { get; set; } //e_c Event Category

        /// <summary>
        /// The event action. Must not be empty. (eg. Play, Pause, Duration, Add Playlist, Downloaded, Clicked...)
        /// </summary>
        [QueryParameter("e_a")]
        public string EventAction { get; set; } //e_a Event Action
        
        /// <summary>
        /// The event name. (eg. a Movie name, or Song name, or File name...)
        /// </summary>
        [QueryParameter("e_n")]
        public string EventName { get; set; }   //e_n Event Name

        /// <summary>
        /// The event value. Must be a float or integer value (numeric), not a string.
        /// </summary>
        [QueryParameter("e_v")]
        public int? EventValue { get; set; } //e_v Event Value
    }
}
