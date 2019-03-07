using System;

namespace ConnectApp.models
{
    [Serializable]
    public class Thumbnail
    {
        public string url;
        public float width;
        public float height;
        public bool gif;
    }
}