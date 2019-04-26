//
//  JPushEventCache.m
//  Unity-iPhone
//
//  Created by oshumini on 2017/12/21.
//
#import <UserNotifications/UserNotifications.h>
#import "JPushEventCache.h"

@interface JPushEventCache()
@property(strong, nonatomic) NSMutableDictionary *eventCache;
@property(assign, nonatomic) BOOL isJPushDidLoad;
@end

@implementation JPushEventCache

+ (JPushEventCache *)sharedInstance {
  static JPushEventCache* sharedInstance = nil;
  static dispatch_once_t onceAPService;
  dispatch_once(&onceAPService, ^{
    sharedInstance = [self new];
  });
  
  return sharedInstance;
}

- (instancetype)init {
  self = [super init];
  if (self) {
    _eventCache = @{}.mutableCopy;
    _isJPushDidLoad = NO;
  }
  
  return self;
}

- (void)sendEvent:(NSDictionary *)userInfo withKey:(NSString *)key {
  if (_isJPushDidLoad) {
    [[NSNotificationCenter defaultCenter] postNotificationName:key object: userInfo];
    return;
  }
  
  if (!userInfo) {
    return;
  }
  
  if (_eventCache[key]) {
    NSMutableArray *arr = _eventCache[key];
    [arr addObject: userInfo];
  } else {
    NSMutableArray *arr = @[].mutableCopy;
    _eventCache[key] = arr;
    [arr addObject: userInfo];
  }
}

- (void)scheduleNotificationQueue {
  _isJPushDidLoad = YES;
  
  for (NSString *key in _eventCache) {
    for (NSDictionary *notification in _eventCache[key]) {
      [[NSNotificationCenter defaultCenter] postNotificationName:key object:notification];
    }
  }
  [_eventCache removeAllObjects];
}

- (void)handFinishLaunchOption:(NSDictionary *)launchOptons {
  
  JPUSHRegisterEntity * entity = [[JPUSHRegisterEntity alloc] init];
  entity.types = JPAuthorizationOptionAlert|JPAuthorizationOptionBadge|JPAuthorizationOptionSound;
  [JPUSHService registerForRemoteNotificationConfig:entity delegate:[JPushEventCache sharedInstance]];
  
  if ([[UIDevice currentDevice].systemVersion floatValue] >= 10.0) {
  } else { // iOS 9 and later
    [self sendEvent:launchOptons withKey:@"JPushPluginOpenNotification"];
  }
}


// JPUSHRegisterDelegate
// iOS 10 Support
- (void)jpushNotificationCenter:(UNUserNotificationCenter *)center willPresentNotification:(UNNotification *)notification withCompletionHandler:(void (^)(NSInteger))completionHandler {
  // Required

  NSMutableDictionary *userInfo = @{}.mutableCopy;
  if ([notification.request.trigger isKindOfClass:[UNPushNotificationTrigger class]]) {
    userInfo = [NSMutableDictionary dictionaryWithDictionary: notification.request.content.userInfo];
    [JPUSHService handleRemoteNotification:userInfo];
  } else {
    UNNotificationContent *content = notification.request.content;
    userInfo[@"content"] = content.body;
    userInfo[@"badge"] = content.badge;
    userInfo[@"extras"] = content.userInfo;
    
    userInfo[@"identifier"] = notification.request.identifier;
  }
  [[JPushEventCache sharedInstance] sendEvent: userInfo withKey: @"JPushPluginReceiveNotification"];

  
  // 需要执行这个方法，选择是否提醒用户，有 Badge、Sound、Alert 三种类型可以选择设置
  completionHandler(UNNotificationPresentationOptionAlert | UNNotificationPresentationOptionBadge | UNNotificationPresentationOptionSound);
}

// iOS 10 Support
- (void)jpushNotificationCenter:(UNUserNotificationCenter *)center didReceiveNotificationResponse:(UNNotificationResponse *)response withCompletionHandler:(void (^)())completionHandler {
  
  UNNotification *notification = response.notification;
  NSMutableDictionary *userInfo = @{}.mutableCopy;
  
  if ([notification.request.trigger isKindOfClass:[UNPushNotificationTrigger class]]) {
    userInfo = [NSMutableDictionary dictionaryWithDictionary: notification.request.content.userInfo];
    [JPUSHService handleRemoteNotification:userInfo];
  } else {
    UNNotificationContent *content = notification.request.content;
    
    userInfo[@"content"] = content.body;
    userInfo[@"badge"] = content.badge;
    userInfo[@"extras"] = content.userInfo;

    userInfo[@"identifier"] = notification.request.identifier;
  }

  [JPushEventCache.sharedInstance sendEvent:userInfo withKey:@"JPushPluginOpenNotification"];
  completionHandler();
}
@end

