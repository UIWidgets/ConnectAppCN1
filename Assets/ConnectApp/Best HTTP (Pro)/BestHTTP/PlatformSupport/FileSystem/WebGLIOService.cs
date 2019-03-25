#if UNITY_WEBGL && !UNITY_EDITOR
using System;
using System.IO;

namespace BestHTTP.PlatformSupport.FileSystem
{
    public sealed class WebGLIOService : IIOService
    {
        public Stream CreateFileStream(string path, FileStreamModes mode)
        {
            throw new NotImplementedException();
        }

        public void DirectoryCreate(string path)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(string path)
        {
            throw new NotImplementedException();
        }

        public void FileDelete(string path)
        {
            throw new NotImplementedException();
        }

        public bool FileExists(string path)
        {
            throw new NotImplementedException();
        }

        public string[] GetFiles(string path)
        {
            throw new NotImplementedException();
        }
    }
}
#endif