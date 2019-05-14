using System;
using System.IO;
using UnityEngine;

namespace Plugins.Editor {
    public class XClass : IDisposable {
        string filePath;

        public XClass(string fPath) {
            this.filePath = fPath;
            if (!File.Exists(this.filePath)) {
                Debug.LogError(this.filePath + "not found in path.");
                return;
            }
        }

        public void WriteBelow(string below, string text) {
            StreamReader streamReader = new StreamReader(this.filePath);
            string text_all = streamReader.ReadToEnd();
            streamReader.Close();

            int beginIndex = text_all.IndexOf(below);
            if (beginIndex == -1) {
                Debug.LogError(this.filePath + " not found sign in " + below);
                return;
            }

            int endIndex = text_all.LastIndexOf("\n", beginIndex + below.Length);

            text_all = text_all.Substring(0, endIndex) + "\n" + text + "\n" + text_all.Substring(endIndex);

            StreamWriter streamWriter = new StreamWriter(this.filePath);
            streamWriter.Write(text_all);
            streamWriter.Close();
        }

        public void Replace(string below, string newText) {
            StreamReader streamReader = new StreamReader(this.filePath);
            string text_all = streamReader.ReadToEnd();
            streamReader.Close();

            int beginIndex = text_all.IndexOf(below);
            if (beginIndex == -1) {
                Debug.LogError(this.filePath + " not found sign in " + below);
                return;
            }

            text_all = text_all.Replace(below, newText);
            StreamWriter streamWriter = new StreamWriter(this.filePath);
            streamWriter.Write(text_all);
            streamWriter.Close();
        }

        public void Dispose() {
        }
    }
}