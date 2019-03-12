using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Medidata.RWS.NET.Standard.Core;
using Medidata.RWS.NET.Standard.Core.Requests;
using Medidata.RWS.NET.Standard.Core.Requests.Datasets;
using Medidata.RWS.NET.Standard.Core.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Configuration;
using RwsClient.Console.Helpers;
using Microsoft.Extensions.Options;

namespace RwsClient.Console.Core
{
    public class MyClass
    {
        private readonly ILogger _logger;
        private readonly Helpers.Secret _secrets;
        public MyClass(ILogger<MyClass> logger, IOptions<Helpers.Secret> secrets)
        {
            _logger = logger;
            _secrets = secrets.Value ?? throw new ArgumentNullException(nameof(secrets));
        }

        public async Task SomeMethod()
        {
            try
            {
                //var connection = new RwsConnection(Program.Configuration["MedidataHostName"]);
                //var response = await connection.SendRequestAsync(new VersionRequest()) as RwsTextResponse;

                var connection = new RwsConnection(Program.Configuration["MedidataHostName"], 
                                    Program.Configuration["UserName"], _secrets.MedidataPassword);                

                
               // Returns response as a string
               var response = await connection.SendRequestAsync(new StudyDatasetRequest(Constant.TestStudyName, Constant.EnvDev, dataset_type: Constant.DataTypeRaw, formOid: Constant.FormOidAESAEYN)) 
                as RwsResponse;                
                
                if (response?.ResponseObject.StatusCode == HttpStatusCode.OK)
                    using(StreamWriter writer = File.CreateText("Newfile.txt"))
                    {
                        writer.Write(await response.ResponseObject.Content.ReadAsStringAsync());
                    }
                else
                   _logger.LogInformation("No Response from web service...");

                //Returns response as XDocument
                var xmlResponse = await connection.SendRequestAsync(new StudyDatasetRequest(Constant.TestStudyName, Constant.EnvDev, dataset_type: Constant.DataTypeRaw, formOid: Constant.FormOidAESAEYN)) 
                as XDocument;

                if(response?.ResponseObject.StatusCode == HttpStatusCode.OK)
                    using(XmlWriter xmlWriter = XmlWriter.Create("xmlFile.xml", null))
                    {   
                    xmlResponse.Save(xmlWriter);
                    }
                else
                    _logger.LogInformation("No xml response from web service");


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