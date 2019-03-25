using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BestHTTP.Logger
{
    /// <summary>
    /// Available logging levels.
    /// </summary>
    public enum Loglevels : byte
    {
        /// <summary>
        /// All message will be logged.
        /// </summary>
        All,

        /// <summary>
        /// Only Informations and above will be logged.
        /// </summary>
        Information,

        /// <summary>
        /// Only Warnings and above will be logged.
        /// </summary>
        Warning,

        /// <summary>
        /// Only Errors and above will be logged.
        /// </summary>
        Error,

        /// <summary>
        /// Only Exceptions will be logged.
        /// </summary>
        Exception,

        /// <summary>
        /// No logging will occur.
        /// </summary>
        None
    }

    public interface ILogger
    {
        /// <summary>
        /// The minimum severity to log
        /// </summary>
        Loglevels Level { get; set; }
        string FormatVerbose { get; set; }
        string FormatInfo { get; set; }
        string FormatWarn { get; set; }
        string FormatErr { get; set; }
        string FormatEx { get; set; }

        void Verbose(string division, string verb);
        void Information(string division, string info);
        void Warning(string division, string warn);
        void Error(string division, string err);
        void Exception(string division, string msg, Exception ex);
    }
}