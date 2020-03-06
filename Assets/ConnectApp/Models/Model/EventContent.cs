using System;
using System.Collections.Generic;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class EventContent {
        public List<_EventContentBlock> blocks;
        public Dictionary<string, _EventContentEntity> entityMap;
    }

    [Serializable]
    public class _EventContentBlock {
        public string key;
        public string text;
        public string type;
        public List<_InlineStyleRange> inlineStyleRanges;
        public List<_EntityRange> entityRanges;
    }

    [Serializable]
    public class _EventContentEntity {
        public _EventContentEntityData data;
        public string type;
        public string mutability;
    }

    [Serializable]
    public class _EventContentEntityData {
        public string uploadId;
        public string contentId;
        public string title;
        public string url;
    }

    [Serializable]
    public class _EntityRange {
        public int key;
        public int length;
        public int offset;
    }

    [Serializable]
    public class _InlineStyleRange {
        public int key;
        public int offset;
        public int length;
        public string style;
    }

    /* contentMap 解析 */

    [Serializable]
    public class ContentMap {
        public _OriginalImage originalImage;
        public _OriginalImage thumbnail;
        public string url;
        public string downloadUrl;
        public string contentType;
        public string attachmentId;
    }

    [Serializable]
    public class _OriginalImage {
        public string url;
        public int width;
        public int height;
    }

    [Serializable]
    public class VideoSliceMap {
        public string id;
        public string origin;
        public string verifyType;
        public string verifyArg;
        public string status;
        public int trialSlicesCount;
        public int limitSeconds;
        public bool canWatch;
    }
}