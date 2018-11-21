using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;


namespace DataAccess
{
    public class HttpUtilManager
    {
        private static HttpUtilManager instance = new HttpUtilManager();

        public static HttpUtilManager getInstance()
        {
            return instance;
        }

        private HttpWebRequest httpGetMethod(String url, String authorization)
        {
            HttpWebRequest method = (HttpWebRequest)WebRequest.Create(url);
            WebHeaderCollection myWebHeaderCollection = method.Headers;

            myWebHeaderCollection.Add("authorization", authorization);

            method.ContentType = "application/json";
            method.Method = "GET";
            //对发送的数据不使用缓存
            method.AllowWriteStreamBuffering = false;
            method.Timeout = 20000;
            method.ServicePoint.Expect100Continue = false;
            return method;
        }

        private HttpWebRequest httpPostMethod(String url, String authorization)
        {
            Uri uri = new Uri(url);
            HttpWebRequest method = WebRequest.Create(uri) as HttpWebRequest;
            WebHeaderCollection myWebHeaderCollection = method.Headers;

            myWebHeaderCollection.Add("authorization", authorization);
            if (method == null)
            {
                return null;
            }
            method.Method = "POST";
            method.KeepAlive = true;
            method.ContentType = "application/json";
            method.AllowAutoRedirect = true;
            return method;
        }

        public String requestHttpGet(String url_prex, String url, Dictionary<String, String> paramMap, String authorization)
        {
            url = url_prex + url + "?" + StringUtil.createLinkString(paramMap);
            if (paramMap == null)
            {
                paramMap = new Dictionary<String, String>();
            }

            HttpWebRequest method = this.httpGetMethod(url, authorization);
            HttpWebResponse response = (HttpWebResponse)method.GetResponse();

            Stream webStream = response.GetResponseStream();
            if (webStream == null)
            {
                return "request error.";
            }
            StreamReader streamReader = new StreamReader(webStream, Encoding.UTF8);
            string responseContent = streamReader.ReadToEnd();
            return responseContent;
        }

        public String requestHttpPost(String url_prex, String url, Dictionary<String, String> paras, String authorization)
        {

            url = url_prex + url;
            HttpWebRequest method = this.httpPostMethod(url, authorization);

            #region 添加Post 参数
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(paras));
            method.ContentLength = data.Length;
            using (Stream reqStream = method.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion

            HttpWebResponse res = (HttpWebResponse)method.GetResponse();
            if (res == null)
            {
                return "Network error:" + new ArgumentNullException("HttpWebResponse").Message;
            }
            Stream inStream = res.GetResponseStream();
            var sr = new StreamReader(inStream, Encoding.UTF8);
            string htmlResult = sr.ReadToEnd();

            return htmlResult;
        }

        private HttpWebRequest httpDeleteMethod(String url, String authorization)
        {
            Uri uri = new Uri(url);
            HttpWebRequest method = WebRequest.Create(uri) as HttpWebRequest;
            WebHeaderCollection myWebHeaderCollection = method.Headers;

            myWebHeaderCollection.Add("authorization", authorization);
            if (method == null)
            {
                return null;
            }
            method.Method = "DELETE";
            method.KeepAlive = true;
            method.ContentType = "application/json";
            method.AllowAutoRedirect = true;

            method.MaximumAutomaticRedirections = 1;
            return method;
        }


        public String requestHttpDelete(String url_prex, String url, Dictionary<String, String> paras, String authorization)
        {
            url = url_prex + url;
            HttpWebRequest method = this.httpPostMethod(url, authorization);

            #region 添加DELETE 参数
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(paras));
            method.ContentLength = data.Length;
            using (Stream reqStream = method.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion

            HttpWebResponse res = (HttpWebResponse)method.GetResponse();
            if (res == null)
            {
                return "Network error:" + new ArgumentNullException("HttpWebResponse").Message;
            }
            Stream inStream = res.GetResponseStream();
            var sr = new StreamReader(inStream, Encoding.UTF8);
            string htmlResult = sr.ReadToEnd();

            return htmlResult;
        }
    }
}
