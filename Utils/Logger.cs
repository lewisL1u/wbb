using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace wbb.Utils
{
    public class Logger
    {
        private static string _errorLog;//错误日志路径
        private static string _succLog;//正确日志路径
        private static readonly object Locker = new object();//线程锁

        /// <summary>
        /// 创建日志文件
        /// </summary>
        /// <param name="str">日志文件类型：dr=导入，fb=发布不同的功能可以写入不同的日志文件中</param>
        public static void CreateLog(string str)
        {
            var errorpath = Application.StartupPath + "\\log\\errorlog";//错误日志文件夹路径
            var succpath = Application.StartupPath + "\\log\\succlog";//成功日志文件夹路径
            if (!Directory.Exists(errorpath))
            {
                Directory.CreateDirectory(errorpath);//创建错误日志文件夹
            }
            if (!Directory.Exists(succpath))
            {
                Directory.CreateDirectory(succpath);//创建正确日志文件夹
            }
            //程序启动创建一个单独的日子文件[*.log],方便查找
            _errorLog = errorpath + "\\" + str + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log";
            _succLog = succpath + "\\" + str + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log";
        }

        /// <summary>
        /// 写日志信息
        /// </summary>
        /// <param name="flag">true=正确日志，false=错误日志</param>
        /// <param name="message">日志信息</param>
        public static void WriteLogInfo(bool flag, string message)
        {
            lock (Locker) //为了线程安全，加入线程锁
            {
                if (flag)//正确日志
                {
                    using (var sw = new StreamWriter(_succLog, true, Encoding.Default))
                    {
                        if (message != "")
                        {
                            sw.WriteLine(DateTime.Now.ToLocalTime() + " : " + message);//每次写一行
                        }
                    }
                }
                else//错误日志
                {
                    using (var sw = new StreamWriter(_errorLog, true, Encoding.Default))
                    {
                        if (message != "")
                        {
                            sw.WriteLine(DateTime.Now.ToLocalTime() + " : " + message);//每次写一行
                        }
                    }
                }
            }
        }
    }
}
