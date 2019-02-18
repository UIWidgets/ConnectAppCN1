using System;
using System.Collections.Generic;

namespace ConnectApp.models
{  
    [Serializable]
    public class EventContent
    {
        public List<_EventContentBlock> blocks;
        public Dictionary<String, _EventContentEntity> entityMap;
    }
    
    [Serializable]
    public class _EventContentBlock
    {
        public string key;
        public string text;
        public string type;
        public List<_EntityRange> entityRanges;
    }
    
    [Serializable]
    public class _EventContentEntity
    {
        public _EventContentEntityData data;
        public string type;
        public string mutability;
    }
    
    [Serializable]
    public class _EventContentEntityData
    {
        public string uploadId;
        public string contentId;
        public string title;
    }

    [Serializable]
    public class _EntityRange
    {
        public int key;
        public int length;
        public int offset;
    }
    
    
    /* contentMap 解析 */

    [Serializable]
    public class ContentMap
    {
        public _originalImage originalImage;
    }
    
    [Serializable]
    public class _originalImage
    {
        public string url;
    }

}