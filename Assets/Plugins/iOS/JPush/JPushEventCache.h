//
//  JPushEventCache.h
//  Unity-iPhone
//
//  Created by oshumini on 2017/12/21.
//

#import <Foundation/Foundation.h>
#import "JPUSHService.h"

@interface JPushEventCache : NSObject<JPUSHRegisterDelegate>
+ (JPushEventCache *)sharedInstance;

- (void)sendEvent:(NSDictionary *)notification withKey:(NSString *)key;
- (void)scheduleNotificationQueue;

- (void)handFinishLaunchOption:(NSDictionary *)launchOptons;
@end

