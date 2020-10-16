using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API.YouTube.Parameters
{
    public class VideosParameters:Parameter
    {
        private string _part = "snippet";
        /// <summary>
        /// The part parameter specifies a comma-separated list of 
        /// one or more video resource properties that the API 
        /// response will include. The part names that you can 
        /// include in the parameter value are id, snippet, 
        /// contentDetails, fileDetails, liveStreamingDetails, 
        /// localizations, player, processingDetails, recordingDetails, 
        /// statistics, status, suggestions, and topicDetails. 
        /// </summary>
        public string part
        {
            get
            {
                return _part;
            }
            set
            {
                _part = value;
            }
        }
        public string chart { get; set; }
        public string debugProjectIdOverride { get; set; }
        public string hl { get; set; }
        public string id { get; set; }
        public string locale { get; set; }
        public string myRating { get; set; }
        public string onBehalfOfContentOwner { get; set; }
        public string regionCode { get; set; }
        public string videoCategoryId { get; set; }
        public string fields { get; set; }


        private uint _maxResults = 25;
        /// <summary>
        /// The maxResults parameter specifies the maximum number of items that should 
        /// be returned in the result set. Acceptable values are 0 to 50, inclusive. 
        /// The default value is 25.
        /// </summary>
        public uint maxResults { get { return _maxResults; } set { _maxResults = value; } }


        public override string ToString()
        {
            string c = "?";
            foreach (PropertyInfo p in this.GetType().GetRuntimeProperties())
            {
                if (p.GetValue(this, null) != null)
                {
                    c += string.Format("{0}={1}&", p.Name, p.GetValue(this, null));
                }
            }
            return c;
        }
    }
}
