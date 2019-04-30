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
#import <UserNotifications/UserNotifications.h>
#include "UIWidgetsMessageManager.h"

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

#pragma mark wechat

- (BOOL)application:(UIApplication*)app openURL:(NSURL*)url options:(NSDictionary<NSString*, id>*)options
{
    return [WXApi handleOpenURL:url delegate:self];
}

- (void)onResp:(BaseResp *)resp {
    if ([resp isKindOfClass:[SendAuthResp class]]) {
        SendAuthResp *sendAuthResp = (SendAuthResp *) resp;
        [[WechatPlugin instance]sendCodeEvent:sendAuthResp.code stateId:sendAuthResp.state];
    }
}
@end
