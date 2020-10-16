using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API.YouTube.Parameters
{
    public class Parameter
    {
        /// <summary>
        /// The pageToken parameter identifies a specific page in the result set that 
        /// should be returned. In an API response, the nextPageToken and prevPageToken 
        /// properties identify other pages that could be retrieved.
        /// </summary>
        public string pageToken { get; set; }
    }
}
