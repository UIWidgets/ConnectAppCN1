using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using UnityEngine;
using UnityEngine.Networking;
using ImageUtils = Unity.UIWidgets.widgets.ImageUtils;

namespace ConnectApp.Components {
    enum CachedImagePhase {
        start,
        waiting,
        fadeOut,
        fadeIn,
        completed
    }

    delegate void _CachedImageProviderResolverListener();

    class _CachedImageProviderResolver {
        public _CachedImageProviderResolver(
            _CachedNetworkImageState state,
            _CachedImageProviderResolverListener listener
        ) {
            this.state = state;
            this.listener = listener;
        }

        readonly _CachedNetworkImageState state;
        readonly _CachedImageProviderResolverListener listener;

        CachedNetworkImage widget {
            get { return this.state.widget; }
        }

        public ImageStream _imageStream;
        public ImageInfo _imageInfo;

        public void resolve(ImageProvider provider) {
            ImageStream oldImageStream = this._imageStream;
            Size size;
            if (this.widget.width != null && this.widget.height != null) {
                size = new Size((float) this.widget.width, (float) this.widget.height);
            }
            else {
                size = null;
            }

            this._imageStream =
                provider.resolve(ImageUtils.createLocalImageConfiguration(context: this.state.context, size: size));
            D.assert(this._imageStream != null);

            if (this._imageStream.key != oldImageStream?.key) {
                oldImageStream?.removeListener(listener: this._handleImageChanged);
                this._imageStream.addListener(listener: this._handleImageChanged);
            }
        }

        void _handleImageChanged(ImageInfo imageInfo, bool synchronousCall) {
            this._imageInfo = imageInfo;
            this.listener();
        }

        public void stopListening() {
            this._imageStream?.removeListener(listener: this._handleImageChanged);
        }
    }

    public class CachedNetworkImageProvider : ImageProvider<CachedNetworkImageProvider>,
        IEquatable<CachedNetworkImageProvider> {
        public CachedNetworkImageProvider(
            string url,
            float scale = 1.0f,
            IDictionary<string, string> headers = null
        ) {
            this.url = url;
            this.scale = scale;
            this.headers = headers;
        }

        readonly string url;
        readonly float scale;
        readonly IDictionary<string, string> headers;

        protected override IPromise<CachedNetworkImageProvider> obtainKey(ImageConfiguration configuration) {
            return Promise<CachedNetworkImageProvider>.Resolved(this);
        }

        protected override ImageStreamCompleter load(CachedNetworkImageProvider key) {
            return new MultiFrameImageStreamCompleter(
                this._loadAsync(key: key),
                scale: key.scale,
                information => {
                    information.AppendLine($"Image provider: {this}");
                    information.Append($"Image key: {key}");
                }
            );
        }

        IPromise<Codec> _loadAsync(CachedNetworkImageProvider key) {
            var localPath = SQLiteDBManager.instance.GetCachedFilePath(url: key.url);
            //the cached file might be deleted by the OS
            if (!File.Exists(path: localPath)) {
                localPath = null;
            }

            var coroutine = localPath != null
                ? Window.instance.startCoroutine(this._loadFromFile(file: localPath))
                : Window.instance.startCoroutine(this._loadFromNetwork(url: key.url));

            return coroutine.promise.Then(obj => {
                if (obj is byte[] bytes) {
                    return CodecUtils.getCodec(bytes: bytes);
                }

                return CodecUtils.getCodec(new Image((Texture2D) obj));
            });
        }

        IEnumerator _loadFromFile(string file) {
#if UNITY_EDITOR_WIN
            var uri = "file:///" + file;
#else
            var uri = "file://" + file;
#endif

            if (uri.EndsWith(".gif")) {
                using (var www = UnityWebRequest.Get(uri: uri)) {
                    yield return www.SendWebRequest();

                    if (www.isNetworkError || www.isHttpError) {
                        throw new Exception($"Failed to get file \"{uri}\": {www.error}");
                    }

                    var data = www.downloadHandler.data;
                    yield return data;
                }

                yield break;
            }

            using (var www = UnityWebRequestTexture.GetTexture(uri: uri)) {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError) {
                    throw new Exception($"Failed to get file \"{uri}\": {www.error}");
                }

                var data = ((DownloadHandlerTexture) www.downloadHandler).texture;
                yield return data;
            }
        }

        IEnumerator _loadFromNetwork(string url) {
            var uri = new Uri(uriString: url);

            if (uri.LocalPath.EndsWith(".gif")) {
                using (var www = UnityWebRequest.Get(uri: uri)) {
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

                    CacheFileHelper.SyncSaveCacheFile(url: url, data: data, "gif");

                    yield return data;
                }

                yield break;
            }

            using (var www = UnityWebRequestTexture.GetTexture(uri: uri)) {
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

                CacheFileHelper.SyncSaveCacheFile(url: url, texture2D: data);

                yield return data;
            }
        }

        public bool Equals(CachedNetworkImageProvider other) {
            if (ReferenceEquals(null, objB: other)) {
                return false;
            }

            if (ReferenceEquals(this, objB: other)) {
                return true;
            }

            return string.Equals(a: this.url, b: other.url) && this.scale.Equals(obj: other.scale);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, objB: obj)) {
                return false;
            }

            if (ReferenceEquals(this, objB: obj)) {
                return true;
            }

            if (obj.GetType() != this.GetType()) {
                return false;
            }

            return this.Equals((CachedNetworkImageProvider) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((this.url != null ? this.url.GetHashCode() : 0) * 397) ^ this.scale.GetHashCode();
            }
        }

        public static bool operator ==(CachedNetworkImageProvider left, CachedNetworkImageProvider right) {
            return Equals(objA: left, objB: right);
        }

        public static bool operator !=(CachedNetworkImageProvider left, CachedNetworkImageProvider right) {
            return !Equals(objA: left, objB: right);
        }

        public override string ToString() {
            return $"runtimeType(\"{this.url}\", scale: {this.scale})";
        }
    }
}