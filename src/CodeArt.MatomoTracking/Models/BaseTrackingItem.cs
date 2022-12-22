using CodeArt.MatomoTracking.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeArt.MatomoTracking.Models
{
    //https://developer.matomo.org/api-reference/tracking-api
    
    public abstract class BaseTrackingItem : ITrackingItem
    {
        public string Url { get; set; } //url
        public string ActionName { get; set; } //action_name
        public string VisitorID { get; set; } //_id

        //User info:
        public string UrlRef { get; set; } //urlref

        public string CampaignName { get; set; } //_rcn
        public string CampaignKeyword { get; set; } //_rck
        public string Resolution { get; set; } //res, e.g. 1280x1024
        public TimeSpan UserTime { get; set; } //h, m, s
        public bool UserSupportsCookies { get; set; } //cookie
        public string UserAgent { get; set; } //ua
        //public string UAData { get; set; } json doc
        public string Language { get; set; } //lang
        public string UserID { get; set; } //uid - Users ID string for known users
        public string CID { get; set; } //CID, 16 hex string for anonymous visitors
        public bool NewVisit { get; set; } //new_visit
        public Dictionary<int, string> Dimensions { get; set; } = new Dictionary<int, string>(); //dimension1, dimension2, etc custom dimensions set to their value

        //Optional Action info
        public string Link { get; set; } //link - External link opened by user
        public string Download { get; set; } //download - URL of a file the user has downloaded
        public string Search { get; set; } //search - Search query
        public string SearchCategory { get; set; } //search_cat
        public int SearchCount { get; set; } //search_count
        public string PageViewID { get; set; } //pv_id If this action relates to a previous page view
        public int IDGoal { get; set; }//idgoal - will trigger a goal with that ID
        public decimal Revenue { get; set; } //revenue - Only use if idgoal is set. Monetary value of action
        public int ServerTimeToGenerateActionInMS { get; set; } //gt_ms
        public string CharsetOfPageTracked { get; set; } //cs (default utf-8)
        public bool CustomAction { get; set; }//ca
    }
}
