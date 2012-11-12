using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ImageResizer;
using ImageResizer.Plugins.RemoteReader;

namespace TRock.Party.Controllers
{
    public class ImageResizeController : ApiController
    {
        static ImageResizeController()
        {
            RemoteReaderPlugin.Current.AllowRemoteRequest += (sender, args) =>
            {
                args.DenyRequest = false;
            };
        }

        public HttpResponseMessage GetResize(string url)
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            
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
        }
    }
}
