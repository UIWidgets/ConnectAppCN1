using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
#if UNITY_IPHONE
using LitJson;

#endif

// @version v3.1.0
namespace JPush {
    public class JPushBinding : MonoBehaviour {
#if UNITY_ANDROID
        private static AndroidJavaObject _plugin;

        private static int notificationDefaults = -1;
        private static int notificationFlags = 16;

        private static readonly int DEFAULT_ALL = -1;
        private static readonly int DEFAULT_SOUND = 1;
        private static readonly int DEFAULT_VIBRATE = 2;
        private static readonly int DEFAULT_LIGHTS = 4;

        private static readonly int FLAG_SHOW_LIGHTS = 1;
        private static readonly int FLAG_ONGOING_EVENT = 2;
        private static readonly int FLAG_INSISTENT = 4;
        private static readonly int FLAG_ONLY_ALERT_ONCE = 8;
        private static readonly int FLAG_AUTO_CANCEL = 16;
        private static readonly int FLAG_NO_CLEAR = 32;
        private static readonly int FLAG_FOREGROUND_SERVICE = 64;

        static JPushBinding()
        {
            using (AndroidJavaClass jpushClass = new AndroidJavaClass("cn.jiguang.unity.push.JPushBridge"))
            {
                _plugin = jpushClass.CallStatic<AndroidJavaObject>("getInstance");
            }
        }

#endif

        /// <summary>
        /// 初始化 JPush。
        /// </summary>
        /// <param name="gameObject">游戏对象名。</param>
        public static void Init(string gameObject) {
#if UNITY_ANDROID
            _plugin.Call("initPush", gameObject);

#elif UNITY_IOS
            _init(gameObject);

#endif
        }

        /// <summary>
        /// 设置是否开启 Debug 模式。
        /// <para>Debug 模式将会输出更多的日志信息，建议在发布时关闭。</para>
        /// </summary>
        /// <param name="enable">true: 开启；false: 关闭。</param>
        public static void SetDebug(bool enable) {
#if UNITY_ANDROID
            _plugin.Call("setDebug", enable);

#elif UNITY_IOS
            _setDebug(enable);

#endif
        }

        /// <summary>
        /// 获取当前设备的 Registration Id。
        /// </summary>
        /// <returns>设备的 Registration Id。</returns>
        public static string GetRegistrationId() {
#if UNITY_ANDROID
            return _plugin.Call<string>("getRegistrationId");

#elif UNITY_IOS
            return _getRegistrationId();

#else
            return "";

#endif
        }

        /// <summary>
        /// 为设备设置标签（tag）。
        /// <para>注意：这个接口是覆盖逻辑，而不是增量逻辑。即新的调用会覆盖之前的设置。</para>
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        /// <param name="tags">
        ///     标签列表。
        ///     <para>每次调用至少设置一个 tag，覆盖之前的设置，不是新增。</para>
        ///     <para>有效的标签组成：字母（区分大小写）、数字、下划线、汉字、特殊字符 @!#$&*+=.|。</para>
        ///     <para>限制：每个 tag 命名长度限制为 40 字节，最多支持设置 1000 个 tag，且单次操作总长度不得超过 5000 字节（判断长度需采用 UTF-8 编码）。</para>
        /// </param>
        public static void SetTags(int sequence, List<string> tags) {
            string tagsJsonStr = JsonHelper.ToJson<string>(tags);

#if UNITY_ANDROID
            _plugin.Call("setTags", sequence, tagsJsonStr);

#elif UNITY_IOS
            _setTags(sequence, tagsJsonStr);

#endif
        }

        /// <summary>
        /// 为设备新增标签（tag）。
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        /// <param name="tags">
        ///     标签列表。
        ///     <para>每次调用至少设置一个 tag，覆盖之前的设置，不是新增。</para>
        ///     <para>有效的标签组成：字母（区分大小写）、数字、下划线、汉字、特殊字符 @!#$&*+=.|。</para>
        ///     <para>限制：每个 tag 命名长度限制为 40 字节，最多支持设置 1000 个 tag，且单次操作总长度不得超过 5000 字节（判断长度需采用 UTF-8 编码）。</para>
        /// </param>
        public static void AddTags(int sequence, List<string> tags) {
            string tagsJsonStr = JsonHelper.ToJson(tags);

#if UNITY_ANDROID
            _plugin.Call("addTags", sequence, tagsJsonStr);

#elif UNITY_IOS
            _addTags(sequence, tagsJsonStr);

#endif
        }

        /// <summary>
        /// 删除标签（tag）。
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        /// <param name="tags">
        ///     标签列表。
        ///     <para>每次调用至少设置一个 tag，覆盖之前的设置，不是新增。</para>
        ///     <para>有效的标签组成：字母（区分大小写）、数字、下划线、汉字、特殊字符 @!#$&*+=.|。</para>
        ///     <para>限制：每个 tag 命名长度限制为 40 字节，最多支持设置 1000 个 tag，且单次操作总长度不得超过 5000 字节（判断长度需采用 UTF-8 编码）。</para>
        /// </param>
        public static void DeleteTags(int sequence, List<string> tags) {
            string tagsJsonStr = JsonHelper.ToJson(tags);

#if UNITY_ANDROID
            _plugin.Call("deleteTags", sequence, tagsJsonStr);

#elif UNITY_IOS
            _deleteTags(sequence, tagsJsonStr);

#endif
        }

        /// <summary>
        /// 清空当前设备设置的标签（tag）。
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        public static void CleanTags(int sequence) {
#if UNITY_ANDROID
            _plugin.Call("cleanTags", sequence);

#elif UNITY_IOS
            _cleanTags(sequence);

#endif
        }

        /// <summary>
        /// 获取当前设备设置的所有标签（tag）。
        /// <para>需要实现 OnJPushTagOperateResult 方法获得操作结果。</para>
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        public static void GetAllTags(int sequence) {
#if UNITY_ANDROID
            _plugin.Call("getAllTags", sequence);

#elif UNITY_IOS
            _getAllTags(sequence);

#endif
        }

        /// <summary>
        /// 查询指定标签的绑定状态。
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        /// <param name="tag">待查询的标签。</param>
        public static void CheckTagBindState(int sequence, string tag) {
#if UNITY_ANDROID
            _plugin.Call("checkTagBindState", sequence, tag);

#elif UNITY_IOS
            _checkTagBindState(sequence, tag);

#endif
        }

        /// <summary>
        /// 设置别名。
        /// <para>注意：这个接口是覆盖逻辑，而不是增量逻辑。即新的调用会覆盖之前的设置。</para>
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        /// <param name="alias">
        ///     别名。
        ///     <para>有效的别名组成：字母（区分大小写）、数字、下划线、汉字、特殊字符@!#$&*+=.|。</para>
        ///     <para>限制：alias 命名长度限制为 40 字节（判断长度需采用 UTF-8 编码）。</para>
        /// </param>
        public static void SetAlias(int sequence, string alias) {
#if UNITY_ANDROID
            _plugin.Call("setAlias", sequence, alias);

#elif UNITY_IOS
            _setAlias(sequence, alias);

#endif
        }

        /// <summary>
        /// 删除别名。
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        public static void DeleteAlias(int sequence) {
#if UNITY_ANDROID
            _plugin.Call("deleteAlias", sequence);

#elif UNITY_IOS
            _deleteAlias(sequence);

#endif
        }

        /// <summary>
        /// 获取当前设备设置的别名。
        /// </summary>
        /// <param name="sequence">用户自定义的操作序列号。同操作结果一起返回，用来标识一次操作的唯一性。</param>
        public static void GetAlias(int sequence) {
#if UNITY_ANDROID
            _plugin.Call("getAlias", sequence);

#elif UNITY_IOS
            _getAlias(sequence);

#endif
        }

#if UNITY_ANDROID
        /// <summary>
        /// 停止 JPush 推送服务。 
        /// </summary>
        public static void StopPush()
        {
            _plugin.Call("stopPush");
        }

        /// <summary>
        /// 唤醒 JPush 推送服务，使用了 StopPush 必须调用此方法才能恢复。
        /// </summary>
        public static void ResumePush()
        {
            _plugin.Call("resumePush");
        }

        /// <summary>
        /// 判断当前 JPush 服务是否停止。
        /// </summary>
        /// <returns>true: 已停止；false: 未停止。</returns>
        public static bool IsPushStopped()
        {
            return _plugin.Call<bool>("isPushStopped");
        }

        /// <summary>
        /// 设置允许推送时间。
        /// </summary>
        /// <parm name="days">为 0~6 之间由","连接而成的字符串。</parm>
        /// <parm name="startHour">0~23</parm>
        /// <parm name="endHour">0~23</parm>
        public static void SetPushTime(string days, int startHour, int endHour)
        {
            _plugin.Call("setPushTime", days, startHour, endHour);
        }

        /// <summary>
        /// 设置通知静默时间。
        /// </summary>
        /// <parm name="startHour">0~23</parm>
        /// <parm name="startMinute">0~59</parm>
        /// <parm name="endHour">0~23</parm>
        /// <parm name="endMinute">0~23</parm>
        public static void SetSilenceTime(int startHour, int startMinute, int endHour, int endMinute)
        {
            _plugin.Call("setSilenceTime", startHour, startMinute, endHour, endMinute);
        }

        /// <summary>
        /// 设置保留最近通知条数。
        /// </summary>
        /// <param name="num">要保留的最近通知条数。</param>
        public static void SetLatestNotificationNumber(int num)
        {
            _plugin.Call("setLatestNotificationNumber", num);
        }

        public static void AddLocalNotification(int builderId, string content, string title, int nId,
                int broadcastTime, string extrasStr)
        {
            _plugin.Call("addLocalNotification", builderId, content, title, nId, broadcastTime, extrasStr);
        }

        public static void AddLocalNotificationByDate(int builderId, string content, string title, int nId,
                int year, int month, int day, int hour, int minute, int second, string extrasStr)
        {
            _plugin.Call("addLocalNotificationByDate", builderId, content, title, nId,
                year, month, day, hour, minute, second, extrasStr);
        }

        public static void RemoveLocalNotification(int notificationId)
        {
            _plugin.Call("removeLocalNotification", notificationId);
        }

        public static void ClearLocalNotifications()
        {
            _plugin.Call("clearLocalNotifications");
        }

        public static void ClearAllNotifications()
        {
            _plugin.Call("clearAllNotifications");
        }

        public static void ClearNotificationById(int notificationId)
        {
            _plugin.Call("clearNotificationById", notificationId);
        }

        /// <summary>
        /// 用于 Android 6.0 及以上系统申请权限。
        /// </summary>
        public static void RequestPermission()
        {
            _plugin.Call("requestPermission");
        }

        public static void SetBasicPushNotificationBuilder()
        {
            // 需要根据自己业务情况修改后再调用。
            int builderId = 1;
            int notiDefaults = notificationDefaults | DEFAULT_ALL;
            int notiFlags = notificationFlags | FLAG_AUTO_CANCEL;
            _plugin.Call("setBasicPushNotificationBuilder", builderId, notiDefaults, notiFlags, null);
        }

        public static void SetCustomPushNotificationBuilder()
        {
            // 需要根据自己业务情况修改后再调用。
            int builderId = 1;
            string layoutName = "yourNotificationLayoutName";

            // 指定最顶层状态栏小图标
            string statusBarDrawableName = "yourStatusBarDrawableName";

            // 指定下拉状态栏时显示的通知图标
            string layoutIconDrawableName = "yourLayoutIconDrawableName";

            _plugin.Call("setCustomPushNotificationBuilder", builderId, layoutName, statusBarDrawableName, layoutIconDrawableName);
        }

        public static void InitCrashHandler()
        {
            _plugin.Call("initCrashHandler");
        }

        public static void StopCrashHandler()
        {
            _plugin.Call("stopCrashHandler");
        }

        public static bool GetConnectionState()
        {
            return _plugin.Call<bool>("getConnectionState");
        }

#endif

#if UNITY_IOS

        public static void SetBadge(int badge) {
            _setBadge(badge);
        }

        public static void ResetBadge() {
            _resetBadge();
        }

        public static void SetApplicationIconBadgeNumber(int badge) {
            _setApplicationIconBadgeNumber(badge);
        }

        public static int GetApplicationIconBadgeNumber() {
            return _getApplicationIconBadgeNumber();
        }

        public static void StartLogPageView(string pageName) {
            _startLogPageView(pageName);
        }

        public static void StopLogPageView(string pageName) {
            _stopLogPageView(pageName);
        }

        public static void BeginLogPageView(string pageName, int duration) {
            _beginLogPageView(pageName, duration);
        }

        // 本地通知 -start

        public static void SendLocalNotification(string localParams) {
            _sendLocalNotification(localParams);
        }

        public static void SetLocalNotification(int delay, string content, int badge, string idKey) {
            JsonData jd = new JsonData();
            jd["alertBody"] = content;
            jd["idKey"] = idKey;
            string jsonStr = JsonMapper.ToJson(jd);
            _setLocalNotification(delay, badge, jsonStr);
        }

        public static void DeleteLocalNotificationWithIdentifierKey(string idKey) {
            JsonData jd = new JsonData();
            jd["idKey"] = idKey;
            string jsonStr = JsonMapper.ToJson(jd);
            _deleteLocalNotificationWithIdentifierKey(jsonStr);
        }

        public static void ClearAllLocalNotifications() {
            _clearAllLocalNotifications();
        }

        // 本地通知 - end

        [DllImport("__Internal")]
        static extern void _init(string gameObject);

        [DllImport("__Internal")]
        static extern void _setDebug(bool enable);

        [DllImport("__Internal")]
        static extern string _getRegistrationId();

        [DllImport("__Internal")]
        static extern void _setTags(int sequence, string tags);

        [DllImport("__Internal")]
        static extern void _addTags(int sequence, string tags);

        [DllImport("__Internal")]
        static extern void _deleteTags(int sequence, string tags);

        [DllImport("__Internal")]
        static extern void _cleanTags(int sequence);

        [DllImport("__Internal")]
        static extern void _getAllTags(int sequence);

        [DllImport("__Internal")]
        static extern void _checkTagBindState(int sequence, string tag);

        [DllImport("__Internal")]
        static extern void _setAlias(int sequence, string alias);

        [DllImport("__Internal")]
        static extern void _deleteAlias(int sequence);

        [DllImport("__Internal")]
        static extern void _getAlias(int sequence);

        [DllImport("__Internal")]
        static extern void _setBadge(int badge);

        [DllImport("__Internal")]
        static extern void _resetBadge();

        [DllImport("__Internal")]
        static extern void _setApplicationIconBadgeNumber(int badge);

        [DllImport("__Internal")]
        static extern int _getApplicationIconBadgeNumber();

        [DllImport("__Internal")]
        static extern void _startLogPageView(string pageName);

        [DllImport("__Internal")]
        static extern void _stopLogPageView(string pageName);

        [DllImport("__Internal")]
        static extern void _beginLogPageView(string pageName, int duration);

        [DllImport("__Internal")]
        public static extern void _setLocalNotification(int delay, int badge, string alertBodyAndIdKey);

        [DllImport("__Internal")]
        public static extern void _sendLocalNotification(string localParams);

        [DllImport("__Internal")]
        public static extern void _deleteLocalNotificationWithIdentifierKey(string idKey);

        [DllImport("__Internal")]
        public static extern void _clearAllLocalNotifications();

#endif
    }
}