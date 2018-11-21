using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DataAccess
{
    /// <summary>
    /// Http访问帮助类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// HttpWebRequest 通过Post
        /// </summary>
        /// <param name="url">URI</param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public static string PostDataGetHtml(string url, string postData)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(postData);

                Uri uri = new Uri(url);
                HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
                if (req == null)
                {
                    return "Network error:" + new ArgumentNullException("httpWebRequest").Message;
                }
                req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = data.Length;
                req.AllowAutoRedirect = true;

                Stream outStream = req.GetRequestStream();
                outStream.Write(data, 0, data.Length);
                outStream.Close();

                var res = req.GetResponse() as HttpWebResponse;
                if (res == null)
                {
                    return "Network error:" + new ArgumentNullException("HttpWebResponse").Message;
                }
                Stream inStream = res.GetResponseStream();
                var sr = new StreamReader(inStream, Encoding.UTF8);
                string htmlResult = sr.ReadToEnd();

                return htmlResult;
            }
            catch (Exception ex)
            {
                return "网络错误(Network error)：" + ex.Message;
            }
        }

        /// <summary>
        /// HttpWebRequest 通过get
        /// </summary>
        /// <param name="url">URI</param>
        /// <returns></returns>
        public static string GetDataGetHtml(string url)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "GET";
                //对发送的数据不使用缓存
                httpWebRequest.AllowWriteStreamBuffering = false;
                httpWebRequest.Timeout = 300000;
                httpWebRequest.ServicePoint.Expect100Continue = false;

                HttpWebResponse webRespon = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream webStream = webRespon.GetResponseStream();
                if (webStream == null)
                {
                    return "网络错误(Network error)：" + new ArgumentNullException("webStream");
                }
                StreamReader streamReader = new StreamReader(webStream, Encoding.UTF8);
                string responseContent = streamReader.ReadToEnd();

                webRespon.Close();
                streamReader.Close();

                return responseContent;
            }
            catch (Exception ex)
            {
                return "网络错误(Network error)：" + ex.Message;
            }
        }

        /// <summary>
        /// HttpWebRequest 通过get
        /// </summary>
        /// <param name="url">URI</param>
        /// <returns></returns>
        public static bool GetDataGetHtml(string url, string filePath, ref string mg)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "GET";
                //对发送的数据不使用缓存
                httpWebRequest.AllowWriteStreamBuffering = false;
                httpWebRequest.Timeout = 300000;
                httpWebRequest.ServicePoint.Expect100Continue = false;

                HttpWebResponse webRespon = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream webStream = webRespon.GetResponseStream();
                if (webStream == null)
                {
                    return false;
                }
                StreamReader streamReader = new StreamReader(webStream, Encoding.UTF8);
                string responseContent = streamReader.ReadToEnd();
                mg = responseContent;
                webRespon.Close();
                streamReader.Close();
                if (responseContent.ToUpper().IndexOf("NULL") > -1)
                {
                    return false;
                }
                else
                {
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    var buff = Encoding.UTF8.GetBytes(responseContent);
                    fs.Write(buff, buff.Length, 0);
                    fs.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// 将本地文件上传到指定的服务器(HttpWebRequest方法)
        /// </summary>
        /// <param name="address">文件上传到的服务器</param>
        /// <param name="fileNamePath">要上传的本地文件（全路径）</param>
        /// <param name="saveName">文件上传后的名称</param>
        /// <returns>成功返回1，失败返回0</returns> 
        public static int Upload_Request(string address, string fileNamePath, string saveName)
        {
            // 要上传的文件
            try
            {
                if (!File.Exists(fileNamePath))
                {
                    return 0;
                }
                FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
                return Upload_Request(address, fs, saveName);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 将本地文件流上传到指定的服务器(HttpWebRequest方法)
        /// </summary>
        /// <param name="address">文件上传到的服务器</param>
        /// <param name="fileStream">要上传的本地文件流</param>
        /// <param name="saveName">文件上传后的名称</param>
        /// <returns>成功返回1，失败返回0</returns> 
        public static int Upload_Request(string address, Stream fileStream, string saveName)
        {
            int returnValue = 0;
            fileStream.Position = 0;
            var r = new BinaryReader(fileStream);
            //时间戳
            string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");
            //请求头部信息
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append("file");
            sb.Append("\"; filename=\"");
            sb.Append(saveName);
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append("application/octet-stream");
            sb.Append("\r\n");
            sb.Append("\r\n");
            string strPostHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);
            try
            {
                // 根据uri创建HttpWebRequest对象
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(address));
                httpReq.Method = "POST";
                //对发送的数据不使用缓存
                httpReq.AllowWriteStreamBuffering = false;
                //设置获得响应的超时时间（300秒）
                httpReq.Timeout = 300000;
                httpReq.ServicePoint.Expect100Continue = false;
                httpReq.ContentType = "multipart/form-data; boundary=" + strBoundary;
                long length = fileStream.Length + postHeaderBytes.Length + boundaryBytes.Length;
                long fileLength = fileStream.Length;
                httpReq.ContentLength = length;
                byte[] buffer = new byte[fileLength];
                Stream postStream = httpReq.GetRequestStream();
                //发送请求头部消息
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                int size = r.Read(buffer, 0, buffer.Length);
                postStream.Write(buffer, 0, size);
                //添加尾部的时间戳
                postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                postStream.Close();
                //获取服务器端的响应
                HttpWebResponse webRespon = (HttpWebResponse)httpReq.GetResponse();
                if (webRespon.StatusCode == HttpStatusCode.OK) //如果服务器未响应，那么继续等待相应                 
                {
                    Stream s = webRespon.GetResponseStream();
                    StreamReader sr = new StreamReader(s);
                    //读取服务器端返回的消息
                    String sReturnString = sr.ReadLine();
                    s.Close();
                    sr.Close();
                    fileStream.Close();
                    if (sReturnString == "Success")
                    {
                        returnValue = 1;
                    }
                    else
                    {
                        returnValue = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = 0;
            }
            return returnValue;
        }
        /// <summary>
        /// 将本地文件上传到指定服务器上（HttpWebRequest方法），并传递相应参数
        /// </summary>
        /// <param name="url">文件上传到的服务器</param>
        /// <param name="fileKeyName">类型（此处为文件--file）</param>
        /// <param name="filePath">要上传的本地文件（全路径）</param>
        /// <param name="filename">文件上传后的名称</param>
        /// <param name="stringDict">参数集合</param>
        /// <param name="timeOut">请求时效</param>
        /// <returns></returns>
        public static string HttpPostData(string url, string fileKeyName, string filePath, string filename, NameValueCollection stringDict, int timeOut = 900000)
        {
            string responseContent;
            try
            {
                var memStream = new MemoryStream();
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                // 边界符
                var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
                // 边界符
                var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                // 最后的结束符
                var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

                // 设置属性
                webRequest.Method = "POST";
                webRequest.Timeout = timeOut;
                webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

                // 写入文件
                const string filePartHeader = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" + "Content-Type: application/octet-stream\r\n\r\n";
                var header = string.Format(filePartHeader, fileKeyName, filename);
                var headerbytes = Encoding.UTF8.GetBytes(header);

                memStream.Write(beginBoundary, 0, beginBoundary.Length);
                memStream.Write(headerbytes, 0, headerbytes.Length);

                var buffer = new byte[1024];
                int bytesRead; // =0

                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    memStream.Write(buffer, 0, bytesRead);
                }

                // 写入字符串的Key
                var stringKeyHeader = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"" + "\r\n\r\n{1}\r\n";

                foreach (byte[] formitembytes in from string key in stringDict.Keys select string.Format(stringKeyHeader, key, stringDict[key]) into formitem select Encoding.UTF8.GetBytes(formitem))
                {
                    memStream.Write(formitembytes, 0, formitembytes.Length);
                }

                // 写入最后的结束边界符
                memStream.Write(endBoundary, 0, endBoundary.Length);

                webRequest.ContentLength = memStream.Length;

                var requestStream = webRequest.GetRequestStream();

                memStream.Position = 0;
                var tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();

                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                requestStream.Close();

                var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

                using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                {
                    responseContent = httpStreamReader.ReadToEnd();
                }

                fileStream.Close();
                httpWebResponse.Close();
                webRequest.Abort();
            }
            catch (Exception ex)
            {
                responseContent = ex.Message;
            }
            return responseContent;
        }

        /// <summary>
        /// Http下载文件支持断点续传
        /// </summary>
        /// <param name="uri">下载地址</param>
        /// <param name="filefullpath">存放完整路径（含文件名）</param>
        /// <param name="size">每次多的大小</param>
        /// <returns>下载操作是否成功</returns>
        public static bool HttpDownLoadFiles(string uri, string filefullpath, int size = 1024)
        {
            try
            {
                string fileDirectory = System.IO.Path.GetDirectoryName(filefullpath);
                if (!Directory.Exists(fileDirectory))
                {
                    Directory.CreateDirectory(fileDirectory);
                }
                string fileFullPath = filefullpath;
                string fileTempFullPath = filefullpath + ".tmp";

                if (File.Exists(fileFullPath))
                {
                    return true;
                }
                else
                {
                    if (File.Exists(fileTempFullPath))
                    {
                        FileStream fs = new FileStream(fileTempFullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

                        byte[] buffer = new byte[512];
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

                        request.Timeout = 10000;
                        request.AddRange((int)fs.Length);

                        Stream ns = request.GetResponse().GetResponseStream();

                        long contentLength = request.GetResponse().ContentLength;

                        int length = ns.Read(buffer, 0, buffer.Length);

                        while (length > 0)
                        {
                            fs.Write(buffer, 0, length);

                            buffer = new byte[512];

                            length = ns.Read(buffer, 0, buffer.Length);
                        }

                        fs.Close();
                        File.Move(fileTempFullPath, fileFullPath);
                    }
                    else
                    {
                        FileStream fs = new FileStream(fileTempFullPath, FileMode.Create);

                        byte[] buffer = new byte[512];
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                        request.Timeout = 10000;
                        request.AddRange((int)fs.Length);

                        Stream ns = request.GetResponse().GetResponseStream();

                        long contentLength = request.GetResponse().ContentLength;

                        int length = ns.Read(buffer, 0, buffer.Length);

                        while (length > 0)
                        {
                            fs.Write(buffer, 0, length);

                            buffer = new byte[512];

                            length = ns.Read(buffer, 0, buffer.Length);
                        }

                        fs.Close();
                        File.Move(fileTempFullPath, fileFullPath);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Http下载文件
        /// </summary>
        /// <param name="uri">下载地址</param>
        /// <param name="filefullpath">存放完整路径（含文件名）</param>
        /// <param name="size">每次多的大小</param>
        /// <returns>下载操作是否成功</returns>
        public static bool DownLoadFiles(string uri, string filefullpath, int size = 1024)
        {
            try
            {
                if (File.Exists(filefullpath))
                {
                    try
                    {
                        File.Delete(filefullpath);
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                string fileDirectory = System.IO.Path.GetDirectoryName(filefullpath);
                if (!Directory.Exists(fileDirectory))
                {
                    Directory.CreateDirectory(fileDirectory);
                }
                FileStream fs = new FileStream(filefullpath, FileMode.Create);
                byte[] buffer = new byte[size];
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                request.Timeout = 10000;
                request.AddRange((int)fs.Length);

                Stream ns = request.GetResponse().GetResponseStream();

                long contentLength = request.GetResponse().ContentLength;

                int length = ns.Read(buffer, 0, buffer.Length);

                while (length > 0)
                {
                    fs.Write(buffer, 0, length);

                    buffer = new byte[size];

                    length = ns.Read(buffer, 0, buffer.Length);
                }
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}