using System;
using ConnectApp.api;
using ConnectApp.components;
using ConnectApp.models;
using ConnectApp.plugins;
using Unity.UIWidgets.Redux;

namespace ConnectApp.redux.actions
{
    public class ShareAction : BaseAction
    {
        public ShareType type;
        public string title;
        public string description;
        public string linkUrl;
        public string imageUrl;
    }

    public static partial class Actions {
        public static object shareToWechat(ShareType type, string title, string description, string linkUrl, string imageUrl)
        {
            return new ThunkAction<AppState>((dispatcher, getState) =>
            {
                return ShareApi.FetchImageBytes(imageUrl)
                    .Then(imageBtyes =>
                {
                    var encodeBytes = Convert.ToBase64String(imageBtyes);
                    CustomDialogUtils.hiddenCustomDialog();
                    if (type == ShareType.friends)
                    {
                        WechatPlugin.instance.shareToFriend(title, description, linkUrl, encodeBytes);
                    }else if (type == ShareType.moments)
                    {
                        WechatPlugin.instance.shareToTimeline(title, description, linkUrl, encodeBytes);
                    }
                        
                });
            });
        }
    }
}