using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace ConnectApp.Utils {
    public static class CacheFileHelper {

        static readonly MD5 md5Hasher = MD5.Create();

        static string _cacheFolderPath = null;

        static string cacheFolderPath {
            get {
                if (_cacheFolderPath != null) {
                    return _cacheFolderPath;
                }
#if UNITY_EDITOR
                var assetPath = Application.dataPath;
                _cacheFolderPath = $"{assetPath.Substring(0, assetPath.LastIndexOf("/"))}/Editor/imgCache";
#else
                _cacheFolderPath = $"{Application.temporaryCachePath}/imgCache";
#endif

                return _cacheFolderPath;
            }
        }

        static string GetCacheFilePath(string url, string suffix) {
            Directory.CreateDirectory(path: cacheFolderPath);
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(s: url));
            var fileName = new Guid(b: data).ToString();
            return $"{cacheFolderPath}/{fileName}.{suffix}";
        }

        public static void ClearCacheFiles() {
#if UNITY_EDITOR
            var cacheFolder = $"{Application.dataPath}/imgCache";
#else
            var cacheFolder = $"{Application.temporaryCachePath}/imgCache";
#endif
            var folder = new DirectoryInfo(path: cacheFolder);

            foreach (FileInfo file in folder.GetFiles()) {
                file.Delete(); 
            }
        }

        public static void SyncSaveCacheFile(string url, byte[] data, string suffix) {
            var filePath = GetCacheFilePath(url: url, suffix: suffix);
            File.WriteAllBytes(path: filePath, bytes: data);
            SQLiteDBManager.instance.UpdateCachedFilePath(url: url, filePath: filePath);
        }

        public static void SyncSaveCacheFile(string url, Texture2D texture2D) {
            var filePath = GetCacheFilePath(url: url, "png");
            File.WriteAllBytes(path: filePath, texture2D.EncodeToPNG());
            SQLiteDBManager.instance.UpdateCachedFilePath(url: url, filePath: filePath);
        }
    }
}