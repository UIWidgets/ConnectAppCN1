using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BestHTTP.Logger
{
    /// <summary>
    /// A basic logger implementation to be able to log intelligently additional informations about the plugin's internal mechanism.
    /// </summary>
    public class DefaultLogger : BestHTTP.Logger.ILogger
    {
        public Loglevels Level { get; set; }
        public string FormatVerbose { get; set; }
        public string FormatInfo { get; set; }
        public string FormatWarn { get; set; }
        public string FormatErr { get; set; }
        public string FormatEx { get; set; }

        public DefaultLogger()
        {
            FormatVerbose = "[{0}] D [{1}]: {2}";
            FormatInfo    = "[{0}] I [{1}]: {2}";
            FormatWarn    = "[{0}] W [{1}]: {2}";
            FormatErr     = "[{0}] Err [{1}]: {2}";
            FormatEx      = "[{0}] Ex [{1}]: {2} - Message: {3}  StackTrace: {4}";

            Level = UnityEngine.Debug.isDebugBuild ? Loglevels.Warning : Loglevels.Error;
        }

        public void Verbose(string division, string verb)
        {
            if (Level <= Loglevels.All)
            {
                try
                {
                    UnityEngine.Debug.Log(string.Format(FormatVerbose, GetFormattedTime(), division, verb));
                }
                catch
                { }
            }
        }

        public void Information(string division, string info)
        {
            if (Level <= Loglevels.Information)
            {
                try
                {
                    UnityEngine.Debug.Log(string.Format(FormatInfo, GetFormattedTime(), division, info));
                }
                catch
                { }
            }
        }

        public void Warning(string division, string warn)
        {
            if (Level <= Loglevels.Warning)
            {
                try
                {
                    UnityEngine.Debug.LogWarning(string.Format(FormatWarn, GetFormattedTime(), division, warn));
                }
                catch
                { }
            }
        }

        public void Error(string division, string err)
        {
            if (Level <= Loglevels.Error)
            {
                try
                {
                    UnityEngine.Debug.LogError(string.Format(FormatErr, GetFormattedTime(), division, err));
                }
                catch
                { }
            }
        }

        public void Exception(string division, string msg, Exception ex)
        {
            if (Level <= Loglevels.Exception)
            {
                try
                {
                    string exceptionMessage = string.Empty;
                    if (ex == null)
                        exceptionMessage = "null";
                    else
                    {
                        StringBuilder sb = new StringBuilder();

                        Exception exception = ex;
                        int counter = 1;
                        while (exception != null)
                        {
                            sb.AppendFormat("{0}: {1} {2}", counter++.ToString(), exception.Message, exception.StackTrace);

                            exception = exception.InnerException;

                            if (exception != null)
                                sb.AppendLine();
                        }

                        exceptionMessage = sb.ToString();
                    }

                    UnityEngine.Debug.LogError(string.Format(FormatEx,
                                                                GetFormattedTime(),
                                                                division,
                                                                msg,
                                                                exceptionMessage,
                                                                ex != null ? ex.StackTrace : "null"));
                }
                catch
                { }
            }
        }

        private string GetFormattedTime()
        {
            return DateTime.Now.Ticks.ToString();
        }
    }
}