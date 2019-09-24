using System.IO;
using System.Security.Cryptography;
using System.Text;
using ConnectApp.Utils;
using UnityEngine;

namespace System {
    public static class CacheFileHelper {

        static readonly MD5 md5Hasher = MD5.Create();

        static string GetCacheFilePath(string url, string suffix) {
#if UNITY_EDITOR
            var cacheFolder = $"{Application.dataPath}/imgCache";
#else
            var cacheFolder = $"{Application.persistentDataPath}/imgCache";
#endif
            Directory.CreateDirectory(cacheFolder);
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(url));
            var fileName = new Guid(data).ToString();
            return $"{cacheFolder}/{fileName}.{suffix}";
        }


        public static void ClearCacheFiles() {
#if UNITY_EDITOR
            var cacheFolder = $"{Application.dataPath}/imgCache";
#else
            var cacheFolder = $"{Application.persistentDataPath}/imgCache";
#endif
            var folder = new DirectoryInfo(cacheFolder);

            foreach (FileInfo file in folder.GetFiles())
            {
                file.Delete(); 
            }
        }

        public static void SyncSaveCacheFile(string url, byte[] data, string suffix) {
            var filePath = GetCacheFilePath(url, suffix);
            File.WriteAllBytes(filePath, data);
            SQLiteDBManager.instance.UpdateCachedFilePath(url, filePath);
        }
        
        
        public static void SyncSaveCacheFile(string url, Texture2D texture2D) {
            var filePath = GetCacheFilePath(url, "png");
            File.WriteAllBytes(filePath, texture2D.EncodeToPNG());
            SQLiteDBManager.instance.UpdateCachedFilePath(url, filePath);
        }
    }
}