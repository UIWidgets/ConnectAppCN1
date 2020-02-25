// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>

using System;
using System.Collections.Generic;

namespace HtmlAgilityPack {
    class NameValuePairList {
        #region Fields

        internal readonly string Text;
        List<KeyValuePair<string, string>> _allPairs;
        Dictionary<string, List<KeyValuePair<string, string>>> _pairsWithName;

        #endregion

        #region Constructors

        internal NameValuePairList() :
            this(null) { }

        internal NameValuePairList(string text) {
            this.Text = text;
            this._allPairs = new List<KeyValuePair<string, string>>();
            this._pairsWithName = new Dictionary<string, List<KeyValuePair<string, string>>>();

            this.Parse(text);
        }

        #endregion

        #region Internal Methods

        internal static string GetNameValuePairsValue(string text, string name) {
            NameValuePairList l = new NameValuePairList(text);
            return l.GetNameValuePairValue(name);
        }

        internal List<KeyValuePair<string, string>> GetNameValuePairs(string name) {
            if (name == null) {
                return this._allPairs;
            }

            return this._pairsWithName.ContainsKey(name)
                ? this._pairsWithName[name]
                : new List<KeyValuePair<string, string>>();
        }

        internal string GetNameValuePairValue(string name) {
            if (name == null) {
                throw new ArgumentNullException();
            }

            List<KeyValuePair<string, string>> al = this.GetNameValuePairs(name);
            if (al.Count == 0) {
                return string.Empty;
            }

            // return first item
            return al[0].Value.Trim();
        }

        #endregion

        #region Private Methods

        void Parse(string text) {
            this._allPairs.Clear();
            this._pairsWithName.Clear();
            if (text == null) {
                return;
            }

            string[] p = text.Split(';');
            foreach (string pv in p) {
                if (pv.Length == 0) {
                    continue;
                }

                string[] onep = pv.Split(new[] {'='}, 2);
                if (onep.Length == 0) {
                    continue;
                }

                KeyValuePair<string, string> nvp = new KeyValuePair<string, string>(onep[0].Trim().ToLower(),
                    onep.Length < 2 ? "" : onep[1]);

                this._allPairs.Add(nvp);

                // index by name
                List<KeyValuePair<string, string>> al;
                if (!this._pairsWithName.ContainsKey(nvp.Key)) {
                    al = new List<KeyValuePair<string, string>>();
                    this._pairsWithName[nvp.Key] = al;
                }
                else {
                    al = this._pairsWithName[nvp.Key];
                }

                al.Add(nvp);
            }
        }

        #endregion
    }
}