using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using ImageResizer;
using ImageResizer.Plugins.RemoteReader;

namespace TRock.Party.Controllers
{
    public class BackdropController : ApiController
    {
        #region Fields

        private const string ApiKey = "590b54eae4a816b5144c09f15a8f3876";

        #endregion Fields

        #region Methods

        private Task<IEnumerable<string>> GetImageList(string artistName, CancellationToken token)
        {
            var replacedSpaces = artistName.Replace(" ", "_");
            var siteUri = new Uri("http://htbackdrops.com/api/" + ApiKey + "/searchXML?keywords=" + replacedSpaces + "&limit=7");
            var client = new WebClient();

            return client
                .OpenReadTaskAsync(siteUri)
                .ContinueWith(readTask =>
                {
                    if (readTask.IsFaulted)
                    {
                        Trace.WriteLine(readTask.Exception);
                        return new string[0];
                    }

                    var doc = new XmlDocument();
                    doc.Load(readTask.Result);

                    XmlNodeList nodelist = doc.SelectNodes("/search/images/image/id");

                    if (nodelist == null || nodelist.Count == 0)
                    {
                        return new string[0];
                    }

                    return nodelist
                        .OfType<XmlNode>()
                        .Select(node => node.InnerText)
                        .Select(imageId => "http://htbackdrops.com/api/" + ApiKey + "/download/" + imageId + "/intermediate");
                }, token);
        }

        public Task<HttpResponseMessage> GetImage(string artistName, CancellationToken token)
        {
            return GetImageList(artistName, token).ContinueWith(imageTask =>
            {
                var response = new HttpResponseMessage();
                var client = new WebClient();
                var firstImage = imageTask.Result.FirstOrDefault();

                if (!string.IsNullOrEmpty(firstImage))
                {
                    var uri = new Uri(firstImage, UriKind.RelativeOrAbsolute);

                    if (!uri.IsAbsoluteUri)
                    {
                        uri = new Uri("http://2fm.rte.ie/images/default-album-art.jpg");
                    }

                    var query = Request.RequestUri.ParseQueryString();
                    var settings = new ResizeSettings(query);
                    var remote = RemoteReaderPlugin.Current.CreateSignedUrl(uri.ToString().Replace(" ", "%20"), settings);
                    var r = Request.CreateResponse(HttpStatusCode.Moved);
                    r.Headers.Location = new Uri(remote, UriKind.RelativeOrAbsolute);

                    return r;
                    //var imageStream = client.OpenRead(firstImage);
                    //response.Content = new StreamContent(imageStream);
                    //response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    //return response;
                }

                response.Content = new StreamContent(new MemoryStream());
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                return response;
            });
        }

        #endregion Methods
    }
}
