using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Threading.Tasks;

namespace RwsClient.Console.Core
{
    public class CheckTLS
    {
        public async Task GetXml()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
				"https://www.checktls.com/TestReceiver"
				+ "?CUSTOMERCODE="      + WebUtility.UrlEncode("me@mydomain.com")
				+ "&CUSTOMERPASS="      + WebUtility.UrlEncode("IllNeverTell")
				+ "&EMAIL="             + WebUtility.UrlEncode("test@CheckTLS.com")
				+ "&LEVEL="             + "XML_DETAIL"
			);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			if (response.StatusCode != HttpStatusCode.OK)
				System.Console.WriteLine("CheckTLS on test@CheckTLS.com" + Environment.NewLine + response.StatusCode + ": " + response.StatusDescription);
			StreamReader streamreader = new StreamReader(response.GetResponseStream());
			String responseString = streamreader.ReadToEnd();
			response.Close();
			streamreader.Close();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(responseString);
             using(XmlWriter xmlWriter = XmlWriter.Create("xmlFile.xml", null))
            {   
                xmlDoc.Save(xmlWriter);
            }
			/* XmlNode xmlNode;
			xmlNode = xmlDoc.SelectSingleNode("/CheckTLS/eMailAddress");
			System.Console.WriteLine("Target = " + xmlNode.InnerText);
			xmlNode = xmlDoc.SelectSingleNode("//ConfidenceFactor");
			System.Console.WriteLine("Score = " + xmlNode.InnerText); */
        }
    }
}