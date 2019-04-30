//
//  JPushPlugin.m
//  Unity-iPhone
//
//  Created by luo on 2019/4/29.
//

#import "JPushPlugin.h"
#include "UIWidgetsMessageManager.h"

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

@end

extern "C" {
    void listenCompleted(){
        [[JPushPlugin instance]listenCompleted];
    }
    
    void setChannel(const char * channel){
    }
}
