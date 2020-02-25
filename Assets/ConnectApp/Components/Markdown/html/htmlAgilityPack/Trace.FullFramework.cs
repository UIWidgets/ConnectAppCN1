using System.Diagnostics;

namespace HtmlAgilityPack {
    partial class Trace {
        partial void WriteLineIntern(string message, string category) {
            Debug.WriteLine(message, category);
        }
    }
}