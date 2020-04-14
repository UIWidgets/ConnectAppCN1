//
//  CustomAppController.m
//  Unity-iPhone
//
//  Created by luo on 2019/4/25.
//

#import "UnityAppController.h"
#import "WechatPlugin.h"
#include "WXApi.h"
#import "JPUSHService.h"
#import "JPushEventCache.h"
#import "JANALYTICSService.h"
#import <UserNotifications/UserNotifications.h>
#include "UIWidgetsMessageManager.h"
#import "JPushPlugin.h"
#import <AVFoundation/AVFoundation.h>
#import "UUIDUtils.h"
#import "PickImageController.h"

static NSString *gameObjectName = @"jpush";

@interface CustomAppController : UnityAppController<WXApiDelegate>
@property (nonatomic,assign) NSInteger tabIndex;
@end
IMPL_APP_CONTROLLER_SUBCLASS (CustomAppController)

@implementation CustomAppController

- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    
    [super application:application didFinishLaunchingWithOptions:launchOptions];
    [WXApi registerApp: @"wx0ab79f0c7db7ca52"];
    [[JPushEventCache sharedInstance] handFinishLaunchOption:launchOptions];
    [JPUSHService setupWithOption:launchOptions appKey:@"a50eff2d99416a0495f02766" channel:@"appstore" apsForProduction:YES];
    [JPUSHService setLogOFF];
    
    JANALYTICSLaunchConfig * config = [[JANALYTICSLaunchConfig alloc] init];
    config.appKey = @"a50eff2d99416a0495f02766";
    config.channel = @"appstore";
    [JANALYTICSService setupWithConfig:config];
    [JANALYTICSService crashLogON];
    
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(networkDidReceiveMessage:)
                                                 name:kJPFNetworkDidReceiveMessageNotification
                                               object:nil];
    
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(networkDidReceivePushNotification:)
                                                 name:@"JPushPluginReceiveNotification"
                                               object:nil];
    
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(networkOpenPushNotification:)
                                                 name:@"JPushPluginOpenNotification"
                                               object:nil];
    [[JPushEventCache sharedInstance] scheduleNotificationQueue];
    
    return YES;
}

#pragma mark- JPUSHRegisterDelegate

- (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
    // Required.
    [JPUSHService registerDeviceToken:deviceToken];
}

- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo {
    [super application:application didReceiveRemoteNotification:userInfo];
    // Required.
    [[JPushEventCache sharedInstance] sendEvent:userInfo withKey:@"JPushPluginReceiveNotification"];
    [JPUSHService handleRemoteNotification:userInfo];
}

- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult result))handler {
    [super application:application didReceiveRemoteNotification:userInfo fetchCompletionHandler:handler];
    [[JPushEventCache sharedInstance] sendEvent:userInfo withKey:@"JPushPluginReceiveNotification"];
    [JPUSHService handleRemoteNotification:userInfo];
    handler(UIBackgroundFetchResultNewData);
}

- (void)networkDidReceiveMessage:(NSNotification *)notification {
    if (notification.name == kJPFNetworkDidReceiveMessageNotification && notification.userInfo){
        NSData *data = APNativeJSONData(notification.userInfo);
        NSString *jsonStr = [[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
        UIWidgetsMethodMessage(gameObjectName, @"OnReceiveMessage", @[jsonStr]);
    }
}

- (void)networkDidReceivePushNotification:(NSNotification *)notification {
    if ([notification.name isEqual:@"JPushPluginReceiveNotification"] && notification.object){
        NSData *data = APNativeJSONData(notification.object);
        NSString *jsonStr = [[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
        UIWidgetsMethodMessage(gameObjectName, @"OnReceiveNotification", @[jsonStr]);
    }
}

- (void)networkOpenPushNotification:(NSNotification *)notification {
    if ([notification.name isEqual:@"JPushPluginOpenNotification"] && notification.object){
        NSData *data = APNativeJSONData(notification.object);
        NSString *jsonStr = [[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
        [JPushPlugin instance].pushJson = jsonStr;
        UIWidgetsMethodMessage(gameObjectName, @"OnOpenNotification", @[jsonStr]);
    }
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
#pragma mark wechat


- (BOOL)application:(UIApplication *)application continueUserActivity:(NSUserActivity *)userActivity restorationHandler:(void (^)(NSArray<id<UIUserActivityRestoring>> * _Nullable))restorationHandler{
    
    if ([userActivity.activityType isEqualToString:NSUserActivityTypeBrowsingWeb]) {
        NSURL *webpageURL = userActivity.webpageURL;
        NSString *host = webpageURL.host;
        if ([host isEqualToString:@"applink.unity.cn"]) {
            //判断域名是自己的网站，进行我们需要的处理
            [JPushPlugin instance].universalLink = [webpageURL absoluteString];
            UIWidgetsMethodMessage(gameObjectName, @"OnOpenUniversalLinks", @[[webpageURL absoluteString]]);
        }
    }
    return YES;
}

- (BOOL)application:(UIApplication*)app openURL:(NSURL*)url options:(NSDictionary<NSString*, id>*)options
{
    if ([[url scheme] isEqualToString:@"unityconnect"]) {
        [JPushPlugin instance].schemeUrl = [url absoluteString];
        UIWidgetsMethodMessage(gameObjectName, @"OnOpenUrl", @[[url absoluteString]]);
    }
    if ([JANALYTICSService handleUrl:url]) {
        return YES;
    }
    return [WXApi handleOpenURL:url delegate:self];
}

- (void)onResp:(BaseResp *)resp {
    if ([resp isKindOfClass:[SendAuthResp class]]) {
        SendAuthResp *sendAuthResp = (SendAuthResp *) resp;
        [[WechatPlugin instance]sendCodeEvent:sendAuthResp.code stateId:sendAuthResp.state];
    }
    if ([resp isKindOfClass:[WXLaunchMiniProgramResp class]]) {
        WXLaunchMiniProgramResp *miniResp = (WXLaunchMiniProgramResp *) resp;
        if (miniResp.extMsg.length!=0) {
            UIWidgetsMethodMessage(@"wechat", @"openUrl", @[miniResp.extMsg]);
        }
    }
}

+(void)saveImage:(NSString*)imagePath{
    UIImage *img = [UIImage imageWithContentsOfFile:imagePath];
    UIImageWriteToSavedPhotosAlbum(img, self,
                                   @selector(image:didFinishSavingWithError:contextInfo:), nil);
    
}
+(void)image:(UIImage *)image didFinishSavingWithError:(NSError *)error contextInfo:(void *)contextInfo{
    if (!error) {
        UIWidgetsMethodMessage(@"jpush", @"SaveImageSuccess", @[]);
    }else{
        UIWidgetsMethodMessage(@"jpush", @"SaveImageError",@[]);
    }
}
-(void)updateTabIndex:(NSInteger)index{
    self.tabIndex = index;
}
extern "C"  {
    void pauseAudioSession();
    void setStatusBarStyle(bool isLight);
    void hiddenStatusBar(bool hidden);
    bool isOpenSensor();
    const char *getDeviceID();
    void pickImage(const char *source, bool cropped, int maxSize);
    void pickVideo(const char *source);
    void saveImage(const char *path);
    bool isPhotoLibraryAuthorization ();
    bool isCameraAuthorization ();
    bool isEnableNotification();
    void playSystemSound();
    void updateShowAlert(bool isShow);
    void clearAllAlert();
    void clearBadge();
}

void pauseAudioSession(){
    AVAudioSession *session = [AVAudioSession sharedInstance];
    [session setCategory:AVAudioSessionCategoryPlayback error:nil];
    [session setActive:YES error:nil];
}
void setStatusBarStyle(bool isLight){
    AppController_SendNotificationWithArg(@"UpdateStatusBarStyle",
                                          @{@"key":@"style",@"value":@(isLight)});
}
void hiddenStatusBar(bool hidden){
    AppController_SendNotificationWithArg(@"UpdateStatusBarStyle",
                                          @{@"key":@"hidden",@"value":@(hidden)});
}
bool isOpenSensor() {
    return true;
}

const char *getDeviceID()
{
    NSString *result = [UUIDUtils getUUID];
    if (!result) {
        return NULL;
    }
    const char *s = [result UTF8String];
    char *r = (char *)malloc(strlen(s) + 1);
    strcpy(r, s);
    return r;
}

void pickImage(const char *source, bool cropped, int maxSize) {
    NSString *sourceString = [NSString stringWithUTF8String:source];
    [[PickImageController sharedInstance] pickImageWithSource:sourceString cropped:cropped maxSize:maxSize];
    
}
void pickVideo(const char *source) {
    NSString *sourceString = [NSString stringWithUTF8String:source];
    [[PickImageController sharedInstance] pickVideoWithSource:sourceString];
}

void saveImage(const char *path)//相册
{
    NSString *imageStr = [NSString stringWithUTF8String:path];
    [CustomAppController saveImage:imageStr];
}

bool isPhotoLibraryAuthorization (){
    return [[PickImageController sharedInstance] isPhotoLibraryAuthorization];
}
bool isCameraAuthorization (){
    return [[PickImageController sharedInstance] isCameraAuthorization];
    
}
bool isEnableNotification(){
    BOOL isEnable = NO;
    if ([[UIDevice currentDevice].systemVersion floatValue] >= 8.0f) { // iOS版本 >=8.0 处理逻辑
        UIUserNotificationSettings *setting = [[UIApplication sharedApplication] currentUserNotificationSettings];
        isEnable = (UIUserNotificationTypeNone == setting.types) ? NO : YES;
    }
    return isEnable;
}

void playSystemSound(){
    AudioServicesPlaySystemSound(kSystemSoundID_Vibrate);
}

void updateShowAlert(bool isShow){
    [[JPushEventCache sharedInstance]updateShowAlert:isShow];
}

void clearAllAlert(){
    [UIApplication sharedApplication].applicationIconBadgeNumber = 1;
    [UIApplication sharedApplication].applicationIconBadgeNumber = 0;
}
void clearBadge(){
    [[UIApplication sharedApplication] setApplicationIconBadgeNumber:0];
    [JPUSHService resetBadge];
}

@end
