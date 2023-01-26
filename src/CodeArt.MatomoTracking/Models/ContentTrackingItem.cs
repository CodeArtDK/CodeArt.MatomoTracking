using CodeArt.MatomoTracking.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeArt.MatomoTracking.Models
{
    /// <summary>
    /// Tracks a content interaction. 
    /// </summary>
    public class ContentTrackingItem : BaseTrackingItem
    {
        /// <summary>
        /// The name of the content. For instance 'Ad Foo Bar'
        /// </summary>
        [QueryParameter("c_n")]
        public string ContentName { get; set; }

        /// <summary>
        /// The actual content piece. For instance the path to an image, video, audio, any text
        /// </summary>
        [QueryParameter("c_p")]
        public string ContentPiece { get; set; }

        /// <summary>
        /// The target of the content. For instance the URL of a landing page
        /// </summary>
        [QueryParameter("c_t")]
        public string ContentTarget { get; set; }

        /// <summary>
        /// The name of the interaction with the content. For instance a 'click'
        /// </summary>
        [QueryParameter("c_i")]
        public string InteractionName { get; set; }
    }
}
