using System;
using System.Threading.Tasks;
using Medidata.RWS.NET.Standard.Core;
using Medidata.RWS.NET.Standard.Core.Requests;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace RwsClient.Console
{
    public class MyClass
    {
        private readonly ILogger _logger;

        public MyClass(ILogger<MyClass> logger)
        {
            _logger = logger;
        }

        public async Task SomeMethod()
        {
            try
            {
                var connection = new RwsConnection("tri");
                var response = await connection.SendRequestAsync(new VersionRequest()) as RwsTextResponse;
                
                if (response != null)
                    _logger.LogInformation(await response.ResponseObject.Content.ReadAsStringAsync());
                else
                   _logger.LogInformation("No Response...");


                /* //Create a connection
                var connection = new RwsConnection("innovate", "RAVE username", "RAVE password");

                //Create a request
                var datasetRequest = new SubjectDatasetRequest("MediFlex", "PROD", subject_key: "SUBJECT001", formOid: "HEM");

                //Send the request / get a response
                var response = await connection.SendRequestAsync(datasetRequest) as RwsResponse;

                    //Write the response XML string to the console
                Console.Write(await response.ResponseObject.Content.ReadAsStringAsync());  */  
            }
            catch(Exception ex)
            {
                //System.Console.Write("Something Went Wrong!: " + ex);
                _logger.LogInformation("Hello" + ex);
            }
        }
    }
}