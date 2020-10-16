using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API
{
    public class RequestException:Exception
    {
        public RequestException(HttpStatusCode code, string message, Exception e):base(message, e)
        {
            HttpResponseCode = code;
        }

        public RequestException(HttpStatusCode code)
        {
            HttpResponseCode = code;
        }

        public RequestException(string message) : base(message)
        {

        }

        public HttpStatusCode HttpResponseCode { get; private set; } 
        public Uri RequestUri { get; set; }
        public string ResponseContent { get; set; }
    }
}
