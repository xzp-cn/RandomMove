using huang.common.paths;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace F3D
{
    /// <summary>
    /// 日志管理类
    /// </summary>
    public class AppLog
    {
        /// <summary>
        /// 日志文件的保存路径
        /// </summary>
        public static string StrLogPath = AppDomain.CurrentDomain.BaseDirectory + "AppLog";
        /// <summary>
        /// 日志文件的全路径
        /// </summary>
        public static string StrLogFile = AppDomain.CurrentDomain.BaseDirectory + @"AppLog\ErrorLog.txt";
        /// <summary>
        /// 是否按小时拆分日志
        /// </summary>
        public static bool IsHourSplit = true;

        /// <summary>
        /// 初始化文件路径信息
        /// </summary>
        public static string GetFilePath()
        {
            try
            {
                // 计算新的日志文件
                //string path = AppDomain.CurrentDomain.BaseDirectory + "AppLog\\";
                string path = Paths.StartupPath + "\\" + "AppLog\\";
                DateTime time = DateTime.Now;
                string strDate = time.ToShortDateString();
                strDate = strDate.Replace("\\", "-");
                strDate = strDate.Replace("/", "-");
                path += strDate;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (IsHourSplit)
                    path = path + "\\Log_" + time.Hour + ".txt";
                else
                    path = path + "\\Log.txt";
                if (!File.Exists(path))
                {
                    using (StreamWriter writer = File.CreateText(path))
                    {
                        writer.WriteLine("程序运行日志：\r\n");
                    }
                }
                return path;
            }
            catch
            {
            }
            return StrLogFile;
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exp"></param>
        public static void AddErrorLog(string message, Exception exp)
        {
            Debug.WriteLine(message);
            try
            {
                string path = GetFilePath();
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(DateTime.Now.ToString());
                    writer.WriteLine(message);
                    writer.WriteLine(exp.Message);
                    writer.WriteLine(exp.ToString());
                    if (exp.InnerException != null)
                    {
                        writer.WriteLine(exp.InnerException.Message);
                        writer.WriteLine(exp.InnerException.TargetSite);
                    }
                    writer.WriteLine("\r");
                }
                Debug.WriteLine(DateTime.Now.ToString());
                Debug.WriteLine(message);
                Debug.WriteLine(exp.ToString());
                if (exp.InnerException != null)
                {
                    Debug.WriteLine(exp.InnerException.ToString());
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="exp"></param>
        public static void AddException(Exception exp)
        {
            try
            {
                string path = GetFilePath();
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(DateTime.Now.ToString());
                    writer.WriteLine(exp.Message);
                    writer.WriteLine(exp.ToString());
                    if (exp.InnerException != null)
                    {
                        writer.WriteLine(exp.InnerException.Message);
                        writer.WriteLine(exp.InnerException.TargetSite);
                    }
                    writer.WriteLine("\r");
                }
                Debug.WriteLine(DateTime.Now.ToString());
                Debug.WriteLine(exp.ToString());
                if (exp.InnerException != null)
                {
                    Debug.WriteLine(exp.InnerException.ToString());
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 记录程序日志信息
        /// </summary>
        /// <param name="message"></param>
        public static void AddMsg(string message)
        {
            //Debug.WriteLine(message);
            //#if UNITY_EDITOR
            UnityEngine.Debug.Log(message);
            //#endif
            try
            {
                string path = GetFilePath();
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(DateTime.Now.ToString() + "  " + message);
                }
                //Debug.WriteLine(DateTime.Now.ToString());
                //Debug.WriteLine(message);
            }
            catch
            {
            }
        }
    }
}

namespace Common
{
    public enum LogLevel
    {
        DEBUG,
        INFO,
        WARN,
        ERROR,
        FATAL,
    }
    public class AppLog
    {
        class LogData
        {
            public LogLevel m_LogType = LogLevel.INFO;
            public string m_message = string.Empty;

            public LogData(LogLevel type, string message)
            {
                m_LogType = type;
                m_message = message;
            }

        }

        private static string StartupPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        private static AppLog gAppLog = null;

        private bool m_isClosed = false;
        private List<LogData> m_outList = new List<LogData>();
        private List<LogData> m_outCacheList = new List<LogData>();

        public static LogLevel Level = LogLevel.ERROR;
        public static bool IsHourSplit = true;

        public static AppLog Instance { get { if (gAppLog == null) gAppLog = new AppLog(); return gAppLog; } }

        private AppLog()
        {
            Task.Factory.StartNew(Upate);
            UnityEngine.Application.quitting += Application_quitting;
        }

        private void Application_quitting()
        {
            m_isClosed = true;
        }

        private void Upate()
        {
            while (!m_isClosed)
            {
                m_outCacheList.Clear();
                lock (m_outList)
                {
                    m_outCacheList.AddRange(m_outList);
                    m_outList.Clear();
                }
                if (m_outCacheList.Count > 0)
                {
                    DateTime time = DateTime.Now;
                    string path = GetFilePath(time);

                    try
                    {
                        using (StreamWriter writer = new StreamWriter(path, true))
                        {
                            foreach (LogData data in m_outCacheList)
                            {
                                writer.WriteLine($"[{data.m_LogType.ToString()}]  "
                                    + time.ToString() + "  " + data.m_message);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                System.Threading.Thread.Sleep(100);
            }
        }
        private void Add(LogData data)
        {
            lock (m_outList)
            {
                m_outList.Add(data);
            }
        }

        private static string GetFilePath(DateTime time)
        {
            string path = StartupPath + $"\\AppLog\\{time.Year}-{time.Month}-{time.Day}";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (IsHourSplit)
                path = path + "\\Log_" + time.Hour + ".txt";
            else
                path = path + "\\Log.txt";
            return path;
        }

        public static void AddMsg(LogLevel level, string message)
        {
            AddFormat(level, "", message);
        }
        public static void AddMsg(LogLevel level, Exception exp)
        {
            AddFormat(level, "", exp);
        }
        public static void AddFormat(LogLevel level, string targetSite, Exception exp)
        {
            AddFormat(level, targetSite, exp.Message);
        }
        public static void AddFormat(LogLevel level, string targetSite, string message)
        {
            if (level < Level) return;
            if (!string.IsNullOrEmpty(targetSite))
                targetSite = "TargetSite:" + targetSite + "  ";
            Instance.Add(new LogData(level, targetSite + message));
#if UNITY_EDITOR
            UnityEngine.Debug.Log($"[{level.ToString()}]  " + targetSite + message);
#endif
        }

    }
}
