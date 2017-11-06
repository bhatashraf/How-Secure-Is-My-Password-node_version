using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace azure_password_strength
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the password: ");
            string inputPassword = Console.ReadLine();
            string url = "https://qnetjsengine.azurewebsites.net/api/passwordStrength?code=ARxQePNE3jaZhNejrwMHMLpQ3XMNKvWG5Oq1UpWmtMD7lDbMjQqBpw==&password=";
            url = url + inputPassword;
            var task = MakeAsyncRequest(url, "application/json");
            Console.WriteLine("{0}", task.Result);
            Console.ReadKey();

        }

        public static Task<string> MakeAsyncRequest(string url, string contentType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = contentType;
            request.Method = WebRequestMethods.Http.Get;
            request.Timeout = 20000;
            request.Proxy = null;

            Task<WebResponse> task = Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult => request.EndGetResponse(asyncResult),
                (object)null);

            return task.ContinueWith(t => ReadStreamFromResponse(t.Result));
        }

        private static string ReadStreamFromResponse(WebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(responseStream))
            {
                //Need to return this response 
                string strContent = sr.ReadToEnd();
                return strContent;
            }
        }
    }  
}



