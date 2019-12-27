using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.core.Models;

namespace Xamarin.core.Services
{
    class WebServices
    {
        public HttpResponse Get(string url) {
            var request = WebRequest.Create(url);
            request.ContentType = "application/json";
            
            using (HttpWebResponse httpResponse = request.GetResponse() as HttpWebResponse) {
                return BuildResponse(httpResponse);
            }
        }

        private static HttpResponse BuildResponse(HttpWebResponse httpResponse) {
            using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream())) {
                var content = reader.ReadToEnd();
                var response = new HttpResponse();
                response.Content = content;
                response.HttpStatusCode = httpResponse.StatusCode;
                return response;
            }
        }

        public async Task<HttpResponse> GetAsync(string url) {
            var request = WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "GET";
            using (HttpWebResponse httpResponse = await request.GetResponseAsync() as HttpWebResponse )
            {
                return BuildResponse(httpResponse);
            }
        }
    }
}