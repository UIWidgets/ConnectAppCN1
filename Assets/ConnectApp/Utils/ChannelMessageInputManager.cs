using System.Collections.Generic;
using System.Text;
using ConnectApp.Models.Model;
using UnityEngine;

namespace ConnectApp.Utils {
    class ChannelMessageInputManager {
        const bool debugMode = false;
        
        readonly List<string> contentSpans = new List<string>();
        readonly List<string> mentionIds = new List<string>();
        
                
        public ChannelMessageInputManager() {
            this.contentSpans.Add("");
            this.mentionIds.Add(null);
        }

        bool addMentionBefore;

        bool tryFallback(string originalContent) {
            if (!this.isValid(originalContent)) {
                this.doFallback(originalContent);
                return true;
            }

            return false;
        }

        void doFallback(string originalContent) {
            this.Clear();
            this.contentSpans.Add(originalContent);
            this.mentionIds.Add(null);
        }

        void OnChanged(string originalContent, bool enableFallback = true) {
            //fallback
            if (enableFallback) {
                this.tryFallback(originalContent);
            }

            if (debugMode) {
                Debug.Log(this.ToLogString());
            }
        }

        bool isValid(string originalContent) {
            return originalContent == this.ToContent();
        }

        string ToLogString() {
            var raw = new StringBuilder();
            for (int i = 0; i < this.contentSpans.Count; i++) {
                raw.Append("[");
                raw.Append(this.contentSpans[i]);
                raw.Append("(");
                raw.Append(this.mentionIds[i]);
                raw.Append(")]");
            }
            return raw.ToString();
        }

        public string ToMessage(Dictionary<string, ChannelMember> membersDict, string originalContent) {
            if(this.tryFallback(originalContent)) {
                return originalContent;
            }
            
            var message = new StringBuilder();
            for (int i = 0; i < this.contentSpans.Count; i++) {
                if (this.mentionIds[i] != null && membersDict.ContainsKey(this.mentionIds[i])) {
                    var span = this.contentSpans[i].TrimEnd();
                    if (span == $"@{membersDict[this.mentionIds[i]].user.fullName}") {
                        message.Append($"<@{this.mentionIds[i]}> ");
                        continue;
                    }
                }
                
                message.Append(this.contentSpans[i]);
            }

            return message.ToString();
        }

        string ToContent() {
            var raw = new StringBuilder();
            for (int i = 0; i < this.contentSpans.Count; i++) {
                raw.Append(this.contentSpans[i]);
            }
            return raw.ToString();
        }

        public void Clear() {
            this.contentSpans.Clear();
            this.mentionIds.Clear();
        }

        public void AddMention(string mentionName, string mentionUserId, string newContent) {
            this.addMentionBefore = true;

            if (this.contentSpans.Count == 0) {
                this.doFallback(newContent);
                return;
            }
            
            var span = this.contentSpans[this.contentSpans.Count - 1];

            if (span[span.Length - 1] == '@') {
                this.contentSpans[this.contentSpans.Count - 1] = span.Substring(0, span.Length - 1);
            }
            else {
                this.doFallback(newContent);
                return;
            }
            
            this.contentSpans.Add('@' + mentionName);
            this.mentionIds.Add(mentionUserId);
            this.contentSpans.Add("");
            this.mentionIds.Add(null);

            this.OnChanged(newContent);
        }

        public void AddContent(int selectIndex, string newContent) {
            if (this.addMentionBefore) {
                this.addMentionBefore = false;
                return;
            }
            
            var curLen = 0;
            for (var i = 0; i < this.contentSpans.Count; i++) {
                var span = this.contentSpans[i];
                curLen += span.Length;
            }
            var deltaLen = newContent.Length - curLen;
            if (deltaLen <= 0) {
                this.doFallback(newContent);
                return;
            }

            var startIndex = selectIndex - deltaLen;

            var curIndex = 0;
            var curOffset = 0;
            curLen = 0;
            for(var i = 0; i < this.contentSpans.Count; i++) {
                var span = this.contentSpans[i];
                curLen += span.Length;
                if (curLen >= startIndex && (curLen != startIndex || this.mentionIds[i] == null)) {
                    curIndex = i;
                    curOffset = span.Length - (curLen - startIndex);
                    break;
                }
            }

            //protection
            if (startIndex < 0 || startIndex >= newContent.Length || startIndex + deltaLen > newContent.Length
                || curOffset < 0 || curOffset > this.contentSpans[curIndex].Length) {
                this.doFallback(newContent);
                return;
            }
            
            var deltaContent = newContent.Substring(startIndex, deltaLen);
            this.contentSpans[curIndex] = this.contentSpans[curIndex].Insert(curOffset, deltaContent);
            
            this.OnChanged(newContent);
        }

        public string DeleteContent(int selectIndex, string newContent, ref int jumpForward) {
            var curLen = 0;
            for (var i = 0; i < this.contentSpans.Count; i++) {
                var span = this.contentSpans[i];
                curLen += span.Length;
            }
            var deltaLen = curLen - newContent.Length;
            if (deltaLen <= 0) {
                this.doFallback(newContent);
                jumpForward = 0;
                return newContent;
            }

            var selectFrom = Mathf.Max(0, selectIndex);
            var selectTo = selectFrom + deltaLen;
            
            var fromIndex = -1;
            var toIndex = -1;
            var fromOffset = -1;
            var toOffset = -1;
            curLen = 0;
            var hitFrom = false;

            for (var i = 0; i < this.contentSpans.Count; i++) {
                var span = this.contentSpans[i];
                curLen += span.Length;
                if (!hitFrom && curLen >= selectFrom) {
                    fromIndex = i;
                    fromOffset = span.Length - (curLen - selectFrom);
                    hitFrom = true;
                }

                if (curLen >= selectTo) {
                    toIndex = i;
                    toOffset = span.Length - (curLen - selectTo);
                    break;
                }
            }

            if (fromIndex == -1 || fromOffset == -1 || toIndex == -1 || toOffset == -1) {
                this.doFallback(newContent);
                jumpForward = 0;
                return newContent;
            }

            var deletedAllMention = false;
            if (deltaLen == 1 && fromIndex == toIndex) {
                if (fromOffset == this.contentSpans[fromIndex].Length - 1 && this.mentionIds[fromIndex] != null && this.contentSpans[fromIndex][fromOffset] == ' ') {
                    this.contentSpans[fromIndex] = this.contentSpans[fromIndex]
                        .Substring(0, this.contentSpans[fromIndex].Length - 1);

                    if (!this.tryFallback(newContent)) {
                        jumpForward = fromOffset;
                        this.contentSpans.RemoveAt(fromIndex);
                        this.mentionIds.RemoveAt(fromIndex);
                        deletedAllMention = true;
                    }
                }
            }

            if (!deletedAllMention) {
                if (fromIndex == toIndex) {
                    this.contentSpans[fromIndex] = this.contentSpans[fromIndex].Substring(0, fromOffset) +
                                                   this.contentSpans[fromIndex].Substring(toOffset);
                }
                else {
                    this.contentSpans[fromIndex] = this.contentSpans[fromIndex].Substring(0, fromOffset);
                    this.contentSpans[toIndex] = this.contentSpans[toIndex].Substring(toOffset);
                    var deltaLens = toIndex - fromIndex - 1;
                    while (deltaLens > 0) {
                        this.contentSpans.RemoveAt(fromIndex + 1);
                        this.mentionIds.RemoveAt(fromIndex + 1);
                        deltaLens--;
                    }
                }
            }
            
            //if a span is a mention but its content doesn't start with @
            //make it a non mention
            for (int i = 0; i < this.contentSpans.Count; i++) {
                var span = this.contentSpans[i];
                if (this.mentionIds[i] != null && span.Length > 0 && span[0] != '@') {
                    this.mentionIds[i] = null;
                }
            }
            
            //remove all zero-length spans except the last span
            var idx = 0;
            while (idx < this.contentSpans.Count - 1) {
                while (this.contentSpans[idx].Length == 0) {
                    if (idx == this.contentSpans.Count - 1) {
                        break;
                    }
                    this.contentSpans.RemoveAt(idx);
                    this.mentionIds.RemoveAt(idx);
                }
                idx++;
            }
            
            //remove the last zero-length span if its previous span is not mention
            if (this.contentSpans.Count > 1 && this.contentSpans[this.contentSpans.Count - 1].Length == 0) {
                if (this.mentionIds[this.contentSpans.Count - 2] == null) {
                    this.contentSpans.RemoveAt(this.contentSpans.Count - 1);
                    this.mentionIds.RemoveAt(this.mentionIds.Count - 1);
                }
            }
            
            //keep the contentSpan.Count >= 1
            if (this.contentSpans.Count == 0) {
                this.contentSpans.Add("");
                this.mentionIds.Add(null);
            }
            
            this.OnChanged(newContent, !deletedAllMention);
            return this.ToContent();
        }
    } 
}