using System;
using System.Collections;
using System.Collections.Generic;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using UnityEngine;
using UnityEngine.Networking;

namespace ConnectApp.Utils {
    public class CachedNetworkImage : ImageProvider<CachedNetworkImage> {
        public CachedNetworkImage(string url,
            float scale = 1.0f,
            IDictionary<string, string> headers = null) {
            this.url = url;
            this.scale = scale;
            this.headers = headers;
        }
        
        string url;
        float scale;
        IDictionary<string, string> headers;
        
        protected override IPromise<CachedNetworkImage> obtainKey(ImageConfiguration configuration) {
            return Promise<CachedNetworkImage>.Resolved(this);
        }
        
        
        protected override ImageStreamCompleter load(CachedNetworkImage key) {
            return new MultiFrameImageStreamCompleter(
                codec: this._loadAsync(key),
                scale: key.scale,
                informationCollector: information => {
                    information.AppendLine($"Image provider: {this}");
                    information.Append($"Image key: {key}");
                }
            );
        }
        
        
        IPromise<Codec> _loadAsync(CachedNetworkImage key) {
            var localPath = SQLiteDBManager.instance.GetCachedFilePath(key.url);

            var coroutine = localPath != null
                ? Window.instance.startCoroutine(this._loadFromFile(localPath))
                : Window.instance.startCoroutine(this._loadFromNetwork(key.url));
            
            return coroutine.promise.Then(obj => {
                if (obj is byte[] bytes) {
                    return CodecUtils.getCodec(bytes);
                }

                return CodecUtils.getCodec(new Image((Texture2D) obj));
            });
        }
        
        IEnumerator _loadFromFile(string file) {
            var uri = "file://" + file;

            if (uri.EndsWith(".gif")) {
                using (var www = UnityWebRequest.Get(uri)) {
                    yield return www.SendWebRequest();

                    if (www.isNetworkError || www.isHttpError) {
                        throw new Exception($"Failed to get file \"{uri}\": {www.error}");
                    }

                    var data = www.downloadHandler.data;
                    yield return data;
                }

                yield break;
            }

            using (var www = UnityWebRequestTexture.GetTexture(uri)) {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError) {
                    throw new Exception($"Failed to get file \"{uri}\": {www.error}");
                }

                var data = ((DownloadHandlerTexture) www.downloadHandler).texture;
                yield return data;
            }
        }
        
        IEnumerator _loadFromNetwork(string url) {
            var uri = new Uri(url);

            if (uri.LocalPath.EndsWith(".gif")) {
                using (var www = UnityWebRequest.Get(uri)) {
                    if (this.headers != null) {
                        foreach (var header in this.headers) {
                            www.SetRequestHeader(header.Key, header.Value);
                        }
                    }

                    yield return www.SendWebRequest();

                    if (www.isNetworkError || www.isHttpError) {
                        throw new Exception($"Failed to load from url \"{uri}\": {www.error}");
                    }

                    var data = www.downloadHandler.data;
                    
                    CacheFileHelper.SyncSaveCacheFile(url, data, "gif");
                    
                    yield return data;
                }

                yield break;
            }

            using (var www = UnityWebRequestTexture.GetTexture(uri)) {
                if (this.headers != null) {
                    foreach (var header in this.headers) {
                        www.SetRequestHeader(header.Key, header.Value);
                    }
                }

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError) {
                    throw new Exception($"Failed to load from url \"{uri}\": {www.error}");
                }

                var data = ((DownloadHandlerTexture) www.downloadHandler).texture;
                
                CacheFileHelper.SyncSaveCacheFile(url, data);
                
                yield return data;
            }
        }
        
        public bool Equals(CachedNetworkImage other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            return string.Equals(this.url, other.url) && this.scale.Equals(other.scale);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            if (obj.GetType() != this.GetType()) {
                return false;
            }

            return this.Equals((CachedNetworkImage) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((this.url != null ? this.url.GetHashCode() : 0) * 397) ^ this.scale.GetHashCode();
            }
        }

        public static bool operator ==(CachedNetworkImage left, CachedNetworkImage right) {
            return Equals(left, right);
        }

        public static bool operator !=(CachedNetworkImage left, CachedNetworkImage right) {
            return !Equals(left, right);
        }

        public override string ToString() {
            return $"runtimeType(\"{this.url}\", scale: {this.scale})";
        }
    }
}