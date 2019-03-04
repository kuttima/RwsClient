using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Medidata.RWS.NET.Standard.Core;
using Medidata.RWS.NET.Standard.Core.Requests;
using Medidata.RWS.NET.Standard.Core.Requests.Datasets;
using Medidata.RWS.NET.Standard.Core.Responses;
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
                var connection = new RwsConnection("tri", "Sreelatha2", "Medidata1234");
                var response = await connection.SendRequestAsync(new 
                StudyDatasetRequest("CDB-Phase1-001", "Dev", dataset_type: "raw", formOid: "AESAEYN")) as RwsResponse;
                
                if (response != null)
                    using(StreamWriter writer = File.CreateText("Newfile.txt"))
                    {
                        writer.Write(await response.ResponseObject.Content.ReadAsStringAsync());
                    }
                    //_logger.LogInformation(await response.ResponseObject.Content.ReadAsStringAsync());
                else
                   _logger.LogInformation("No Response from web service...");


                /* //Create a connection
                var connection = new RwsConnection("innovate", "RAVE username", "RAVE password");

                //Create a request
                var datasetRequest = new SubjectDatasetRequest("MediFlex", "PROD", subject_key: "SUBJECT001", formOid: "HEM");

                //Send the request / get a response
                var response = await connection.SendRequestAsync(datasetRequest) as RwsResponse;

                    //Write the response XML string to the console
                Console.Write(await response.ResponseObject.Content.ReadAsStringAsync());  */  
            }
            catch(HttpRequestException ex)
            {
                _logger.LogInformation(ex, "HttpRequestException from MyClass");
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex, "Exception from MyClass");
            }
        }
    }
}