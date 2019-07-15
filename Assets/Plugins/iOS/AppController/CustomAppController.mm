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

static NSString *gameObjectName = @"jpush";

@interface CustomAppController : UnityAppController<WXApiDelegate>
@end
IMPL_APP_CONTROLLER_SUBCLASS (CustomAppController)

@implementation CustomAppController

- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    [super application:application didFinishLaunchingWithOptions:launchOptions];

    [application setApplicationIconBadgeNumber:0];
    [WXApi registerApp: @"wx0ab79f0c7db7ca52"];
    [[JPushEventCache sharedInstance] handFinishLaunchOption:launchOptions];
    [JPUSHService setupWithOption:launchOptions appKey:@"a50eff2d99416a0495f02766" channel:@"appstore" apsForProduction:YES];
    [JPUSHService setBadge:0];
    [JPUSHService setLogOFF];
    
    JANALYTICSLaunchConfig * config = [[JANALYTICSLaunchConfig alloc] init];
    config.appKey = @"a50eff2d99416a0495f02766";
    config.channel = @"appstore";
    [JANALYTICSService setupWithConfig:config];
    [JANALYTICSService crashLogON];
    
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(networkDidRecieveMessage:)
                                                 name:kJPFNetworkDidReceiveMessageNotification
                                               object:nil];
    
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(networkDidRecievePushNotification:)
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
}

- (void)networkDidRecieveMessage:(NSNotification *)notification {
    if (notification.name == kJPFNetworkDidReceiveMessageNotification && notification.userInfo){
        NSData *data = APNativeJSONData(notification.userInfo);
        NSString *jsonStr = [[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
        UIWidgetsMethodMessage(gameObjectName, @"OnReceiveMessage", @[jsonStr]);
    }
}

- (void)networkDidRecievePushNotification:(NSNotification *)notification {
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
}


extern "C" {
    
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
    
}

@end
