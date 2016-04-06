using System.IO.Compression;
using System.Web.Mvc;

namespace WereViewApp.Filter {
    /// <summary>
    /// Attribute that can be added to controller methods to force content
    /// to be GZip encoded if the client supports it
    /// </summary> 

    public class CompressFilter : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            var request = filterContext.HttpContext.Request;

            var acceptEncoding = request.Headers["Accept-Encoding"];

            if (string.IsNullOrEmpty(acceptEncoding)) return;

            acceptEncoding = acceptEncoding.ToUpperInvariant();

            var response = filterContext.HttpContext.Response;

            if (acceptEncoding.Contains("GZIP")) {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            } else if (acceptEncoding.Contains("DEFLATE")) {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }


        }
    }
}