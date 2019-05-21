//
//  JPushPlugin.m
//  Unity-iPhone
//
//  Created by luo on 2019/4/29.
//

#import "JPushPlugin.h"
#include "UIWidgetsMessageManager.h"
#import "JPUSHService.h"
@implementation JPushPlugin

+ (nonnull instancetype) instance {
    static id _shared;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        _shared = [[self alloc] init];
    });
    return _shared;
}

-(void)listenCompleted{
    if ([JPushPlugin instance].pushJson==NULL||[JPushPlugin instance].pushJson.length==0) {
        return;
    }
    UIWidgetsMethodMessage(@"jpush", @"OnOpenNotification", @[[JPushPlugin instance].pushJson]);
}
- (void)setAlias:(NSString *)alias{
    [JPUSHService setAlias:alias completion:^(NSInteger iResCode, NSString *iAlias, NSInteger seq) {
    } seq:0];
}

@end

extern "C" {
    void listenCompleted(){
        [[JPushPlugin instance]listenCompleted];
    }
    
    void setChannel(const char * channel){
    }
    
    void setAlias(const char * alias){
        NSString *aliasStr=[NSString stringWithUTF8String:alias];
        [[JPushPlugin instance]setAlias:aliasStr];
    }
}
