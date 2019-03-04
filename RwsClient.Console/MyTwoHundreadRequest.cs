using System;
using System.Text;
using Medidata.RWS.NET.Standard.Core.Responses;
using Medidata.RWS.NET.Standard.Core.Requests;
using Medidata.RWS.NET.Standard.Core;
using System.Net.Http;
using Flurl;

namespace RwsClient.Console
{
    public class MyTwoHundreadRequest : RwsGetRequest
    {
        public override string UrlPath()
        {
            return Url.Combine("twohundred");
        }

        public override IRwsResponse Result(HttpResponseMessage response)
        {
            return new RwsTextResponse(response);
        }
       
    }
}