//
//  JAnalyticsPlugin.m
//  Unity-iPhone
//
//  Created by luo on 2019/5/20.
//

#import "JAnalyticsPlugin.h"
#import "JANALYTICSService.h"
@implementation JAnalyticsPlugin

@end

extern "C" {
    void startLogPageView(const char * pageName){
        NSString *page=[NSString stringWithUTF8String:pageName];
        [JANALYTICSService startLogPageView:page];
    }
    void stopLogPageView(const char * pageName){
        NSString *page=[NSString stringWithUTF8String:pageName];
        [JANALYTICSService stopLogPageView:page];
    }
    void loginEvent(const char * loginType){
        NSString *method=[NSString stringWithUTF8String:loginType];
        JANALYTICSLoginEvent * event = [[JANALYTICSLoginEvent alloc] init];
        event.success = YES;
        event.method = method;
        [JANALYTICSService eventRecord:event];
    }
}
