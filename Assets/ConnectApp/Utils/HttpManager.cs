using System;
using System.Text;
using Unity.UIWidgets.foundation;
using UnityEngine;
using UnityEngine.Networking;

namespace ConnectApp.utils
{
    public class Method
    {
        public const string GET = "GET";
        public const string POST = "POST";

    }

    public class HttpManager
    {

        private const string COOKIE = "Cookie";

        public static UnityWebRequest initRequest(
            string url,
            string method)
        {
            var request = new UnityWebRequest(url,method);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("X-Requested-With", "XmlHttpRequest");
            request.SetRequestHeader(COOKIE,_cookieHeader());
            return request;
        }

        public static UnityWebRequest GET(string uri)
        {
            return initRequest(uri, Method.GET);
        }


        private static string _cookieHeader()
        {
            if (PlayerPrefs.GetString(COOKIE).isNotEmpty())
            {
                return PlayerPrefs.GetString(COOKIE);
            }

            return "";
        }

        public static void clearCookie()
        {
            PlayerPrefs.DeleteKey(COOKIE);
        }

        public static void updateCookie(string newCookie)
        {
            var oldCookie = PlayerPrefs.GetString(COOKIE);
            if (oldCookie.isEmpty()|| !oldCookie.Equals(newCookie))
            {
                PlayerPrefs.SetString(COOKIE, newCookie);
                PlayerPrefs.Save();   
            }

        } 

    }
}