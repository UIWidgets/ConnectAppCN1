#import "JPushUnityManager.h"
#import "JPUSHService.h"
#import "JPushEventCache.h"

#pragma mark - Utility Function

#if defined(__cplusplus)
extern "C" {
#endif
    extern void       UnitySendMessage(const char* obj, const char* method, const char* msg);
    extern NSString*  CreateNSString (const char* string);
    extern id         APNativeJSONObject(NSData *data);
    extern NSData *   APNativeJSONData(id obj);
#if defined(__cplusplus)
}
#endif

static NSString *gameObjectName = @"jpush";

@interface JPushUnityInstnce : NSObject {
@private
}
+(JPushUnityInstnce*)sharedInstance;
@end


#if defined(__cplusplus)
extern "C" {
#endif
    const char *tagCallbackName_ = "OnJPushTagOperateResult";
    const char *aliasCallbackName_ = "OnJPushAliasOperateResult";
    
    static char *MakeHeapString(const char *string) {
        if (!string){
            return NULL;
        }
        char *mem = static_cast<char*>(malloc(strlen(string) + 1));
        if (mem) {
            strcpy(mem, string);
        }
        return mem;
    }
    
    NSString *CreateNSString (const char *string) {
        return [NSString stringWithUTF8String:(string ? string : "")];
    }
    
    id APNativeJSONObject(NSData *data) {
        if (!data) {
            return nil;
        }
        
        NSError *error = nil;
        id retId = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];
        
        if (error) {
            NSLog(@"%s trans data to obj with error: %@", __func__, error);
            return nil;
        }
        
        return retId;
    }
    
    NSData *APNativeJSONData(id obj) {
        NSError *error = nil;
        NSData *data = [NSJSONSerialization dataWithJSONObject:obj options:0 error:&error];
        if (error) {
            NSLog(@"%s trans obj to data with error: %@", __func__, error);
            return nil;
        }
        return data;
    }
    
    NSString *messageAsDictionary(NSDictionary * dic) {
        NSData *data = APNativeJSONData(dic);
        return [[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
    }
    
    JPUSHTagsOperationCompletion tagsOperationCompletion = ^(NSInteger iResCode, NSSet *iTags, NSInteger seq) {
        NSMutableDictionary *dic = [[NSMutableDictionary alloc] init];
        [dic setObject:[NSNumber numberWithInteger:seq] forKey:@"sequence"];
        [dic setValue:[NSNumber numberWithUnsignedInteger:iResCode] forKey:@"code"];
        
        if (iResCode == 0) {
          dic[@"tags"] = [iTags allObjects];
        }
        
        UnitySendMessage([gameObjectName UTF8String], tagCallbackName_, messageAsDictionary(dic).UTF8String);
      
    };
    
    JPUSHAliasOperationCompletion aliasOperationCompletion = ^(NSInteger iResCode, NSString *iAlias, NSInteger seq) {
        NSMutableDictionary* dic = [[NSMutableDictionary alloc] init];
        [dic setObject:[NSNumber numberWithInteger:seq] forKey:@"sequence"];
        [dic setValue:[NSNumber numberWithUnsignedInteger:iResCode] forKey:@"code"];
        
        if (iResCode == 0) {
            [dic setObject:iAlias==nil?@"":iAlias forKey:@"alias"];
        }
        
        UnitySendMessage([gameObjectName UTF8String], aliasCallbackName_, messageAsDictionary(dic).UTF8String);
    };
    
    NSInteger integerValue(int intValue) {
        NSNumber *n = [NSNumber numberWithInt:intValue];
        return [n integerValue];
    }
    
    int intValue(NSInteger integerValue) {
        NSNumber *n = [NSNumber numberWithInteger:integerValue];
        return [n intValue];
    }
    // private - end

    void _init(char *gameObject) {
      gameObjectName = [NSString stringWithUTF8String:gameObject];
        NSNotificationCenter *msgCenter = [NSNotificationCenter defaultCenter];
        [[NSNotificationCenter defaultCenter] addObserver:[JPushUnityInstnce sharedInstance]
                          selector:@selector(networkDidRecieveMessage:)
                              name:kJPFNetworkDidReceiveMessageNotification
                            object:nil];
      
        [[NSNotificationCenter defaultCenter] addObserver:[JPushUnityInstnce sharedInstance]
                          selector:@selector(networkDidRecievePushNotification:)
                              name:@"JPushPluginReceiveNotification"
                            object:nil];
      
        [[NSNotificationCenter defaultCenter] addObserver:[JPushUnityInstnce sharedInstance]
                                               selector:@selector(networkOpenPushNotification:)
                                                   name:@"JPushPluginOpenNotification"
                                                 object:nil];
        [[JPushEventCache sharedInstance] scheduleNotificationQueue];
    }
    
    void _setDebug(bool enable) {
        if (enable) {
            [JPUSHService setDebugMode];
        } else {
            [JPUSHService setLogOFF];
        }
    }
    
    const char *_getRegistrationId() {
        NSString *registrationID = [JPUSHService registrationID];
        return MakeHeapString([registrationID UTF8String]);
    }

    // Tag & Alias - start
    
    void _setTags(int sequence, const char *tags) {
        NSString *nsTags = CreateNSString(tags);
        if (![nsTags length]) {
            return;
        }
        
        NSData *data = [nsTags dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSArray *array = dict[@"Items"];
        NSSet *set = [[NSSet alloc] initWithArray:array];

        [JPUSHService setTags:set completion:tagsOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _addTags(int sequence, char *tags) {
        NSString* tagsJsonStr = CreateNSString(tags);
        
        NSData *data = [tagsJsonStr dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSArray *tagArr = dict[@"Items"];
        NSSet *tagSet = [[NSSet alloc] initWithArray:tagArr];
        
        [JPUSHService addTags:tagSet completion:tagsOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _deleteTags(int sequence, char *tags) {
        NSString *tagsJsonStr = CreateNSString(tags);
        
        NSData *data = [tagsJsonStr dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSArray *tagArr = dict[@"Items"];
        NSSet *tagSet = [[NSSet alloc] initWithArray:tagArr];
        
        [JPUSHService deleteTags:tagSet completion:tagsOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _cleanTags(int sequence) {
        [JPUSHService cleanTags:tagsOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _getAllTags(int sequence) {
        [JPUSHService getAllTags:tagsOperationCompletion seq:(NSInteger)sequence];
    }

    void _checkTagBindState(int sequence, char *tag) {
        NSString *nsTag = CreateNSString(tag);
        [JPUSHService validTag:nsTag completion:^(NSInteger iResCode, NSSet *iTags, NSInteger seq, BOOL isBind) {
            NSMutableDictionary *dic = [[NSMutableDictionary alloc] init];
            [dic setObject:[NSNumber numberWithInteger:seq] forKey:@"sequence"];
            [dic setValue:[NSNumber numberWithUnsignedInteger:iResCode] forKey:@"code"];
            
            if (iResCode == 0) {
                [dic setObject:[iTags allObjects] forKey:@"tags"];
                [dic setObject:[NSNumber numberWithBool:isBind] forKey:@"isBind"];
            }
            
            UnitySendMessage([gameObjectName UTF8String], tagCallbackName_, messageAsDictionary(dic).UTF8String);
        } seq:(NSInteger)sequence];
    }

    void _setAlias(int sequence, const char * alias){
        NSString *nsAlias = CreateNSString(alias);
        if (![nsAlias length]) {
            return ;
        }
        
        [JPUSHService setAlias:nsAlias completion:aliasOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _getAlias(int sequence) {
        [JPUSHService getAlias:aliasOperationCompletion seq:(NSInteger)sequence];
    }
    
    void _deleteAlias(int sequence) {
        [JPUSHService deleteAlias:aliasOperationCompletion seq:(NSInteger)sequence];
    }
    
    // Tag & Alias - end

    // 角标处理 - start
    
    void _setBadge(const int badge){
        [JPUSHService setBadge:integerValue(badge)];
    }

    void _resetBadge(){
        [JPUSHService resetBadge];
    }

    void _setApplicationIconBadgeNumber(const int badge){
        [UIApplication sharedApplication].applicationIconBadgeNumber = integerValue(badge);
    }

    int _getApplicationIconBadgeNumber(){
        return intValue([UIApplication sharedApplication].applicationIconBadgeNumber);
    }
    
    // 角标处理 - end

    // 页面统计 - start
    
    void _startLogPageView(const char *pageName) {
        NSString *nsPageName = CreateNSString(pageName);
        if (![nsPageName length]) {
            return;
        }
        
        NSData *data =[nsPageName dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString *sendPageName = dict[@"pageName"];
        [JPUSHService startLogPageView:sendPageName];
    }

    void _stopLogPageView(const char *pageName) {
        NSString *nsPageName = CreateNSString(pageName);
        if (![nsPageName length]) {
            return;
        }
        
        NSData *data =[nsPageName dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString *sendPageName = dict[@"pageName"];
        [JPUSHService stopLogPageView:sendPageName];
    }

    void _beginLogPageView(const char *pageName, const int duration) {
        NSString *nsPageName = CreateNSString(pageName);
        if (![nsPageName length]) {
            return;
        }
        
        NSData *data =[nsPageName dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString *sendPageName = dict[@"pageName"];
        [JPUSHService beginLogPageView:sendPageName duration:duration];
    }
    
    // 页面统计 - end

    // 本地通知旧接口 - start

     void _setLocalNotification(int delay, int badge, char *alertBodyAndIdKey){
        NSDate *date = [NSDate dateWithTimeIntervalSinceNow:integerValue(delay)];

        NSString *nsalertBodyAndIdKey = CreateNSString(alertBodyAndIdKey);
        if (![nsalertBodyAndIdKey length]) {
            return ;
        }
        NSData       *data =[nsalertBodyAndIdKey dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString     *sendAlertBody = dict[@"alertBody"];
        NSString     *sendIdkey = dict[@"idKey"];

        [JPUSHService setLocalNotification:date alertBody:sendAlertBody badge:badge alertAction:nil identifierKey:sendIdkey userInfo:nil soundName:nil];
    }
  
  void _sendLocalNotification(char *params) {
    NSString *nsalertBodyAndIdKey = CreateNSString(params);
    if (![nsalertBodyAndIdKey length]) {
      return ;
    }
    NSData       *data =[nsalertBodyAndIdKey dataUsingEncoding:NSUTF8StringEncoding];
    NSDictionary *dict = APNativeJSONObject(data);
    
    JPushNotificationContent *content = [[JPushNotificationContent alloc] init];
    if (dict[@"title"]) {
      content.title = dict[@"title"];
    }
    
    if (dict[@"subtitle"]) {
      content.subtitle = dict[@"subtitle"];
    }
    
    if (dict[@"content"]) {
      content.body = dict[@"content"];
    }
    
    if (dict[@"badge"]) {
      content.badge = dict[@"badge"];
    }
    
    if (dict[@"action"]) {
      content.action = dict[@"action"];
    }
    
    if (dict[@"extra"]) {
      content.userInfo = dict[@"extra"];
    }
    
    if (dict[@"sound"]) {
      content.sound = dict[@"sound"];
    }
    
    JPushNotificationTrigger *trigger = [[JPushNotificationTrigger alloc] init];
    if ([[[UIDevice currentDevice] systemVersion] floatValue] >= 10.0) {
      if (dict[@"fireTime"]) {
        NSNumber *date = dict[@"fireTime"];
        NSTimeInterval currentInterval = [[NSDate date] timeIntervalSince1970];
        NSTimeInterval interval = [date doubleValue] - currentInterval;
        interval = interval>0?interval:0;
        trigger.timeInterval = interval;
        
      }
    }
    
    else {
      if (dict[@"fireTime"]) {
        NSNumber *date = dict[@"fireTime"];
        trigger.fireDate = [NSDate dateWithTimeIntervalSince1970: [date doubleValue]];
      }
    }
    JPushNotificationRequest *request = [[JPushNotificationRequest alloc] init];
    request.content = content;
    request.trigger = trigger;
    
    if (dict[@"id"]) {
      NSNumber *identify = dict[@"id"];
      request.requestIdentifier = [identify stringValue];
    }
    request.completionHandler = ^(id result) {
      NSLog(@"result");
    };
    
    [JPUSHService addNotification:request];
  }

    void _deleteLocalNotificationWithIdentifierKey(char *idKey){
        NSString *nsIdKey = CreateNSString(idKey);
        if (![nsIdKey length]) {
            return ;
        }
        NSData       *data =[nsIdKey dataUsingEncoding:NSUTF8StringEncoding];
        NSDictionary *dict = APNativeJSONObject(data);
        NSString     *sendIdkey = dict[@"idKey"];

        [JPUSHService deleteLocalNotificationWithIdentifierKey:sendIdkey];
    }

    void _clearAllLocalNotifications(){
        [JPUSHService clearAllLocalNotifications];
    }

    // 本地通知旧接口 - end
    
#if defined(__cplusplus)
}
#endif

#pragma mark - Unity interface

@implementation JPushUnityManager : NSObject
@end

#pragma mark - Unity instance

@implementation JPushUnityInstnce

static JPushUnityInstnce * _sharedService = nil;

+ (JPushUnityInstnce*)sharedInstance {
    static dispatch_once_t onceAPService;
    dispatch_once(&onceAPService, ^{
        _sharedService = [[JPushUnityInstnce alloc] init];
    });
    return _sharedService;
}

- (void)networkDidRecieveMessage:(NSNotification *)notification {
    if (notification.name == kJPFNetworkDidReceiveMessageNotification && notification.userInfo){
        NSData *data = APNativeJSONData(notification.userInfo);
        NSString *jsonStr = [[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
        UnitySendMessage([gameObjectName UTF8String], "OnReceiveMessage", jsonStr.UTF8String);
    }
}

- (void)networkDidRecievePushNotification:(NSNotification *)notification {
    if ([notification.name isEqual:@"JPushPluginReceiveNotification"] && notification.object){
        NSData *data = APNativeJSONData(notification.object);
        NSString *jsonStr = [[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
        UnitySendMessage([gameObjectName UTF8String], "OnReceiveNotification", jsonStr.UTF8String);
    }
}

- (void)networkOpenPushNotification:(NSNotification *)notification {
  if ([notification.name isEqual:@"JPushPluginOpenNotification"] && notification.object){
    NSData *data = APNativeJSONData(notification.object);
    NSString *jsonStr = [[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
    UnitySendMessage([gameObjectName UTF8String], "OnOpenNotification", jsonStr.UTF8String);
  }
}
@end
