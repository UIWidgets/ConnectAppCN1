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
    NSString *createNSString (const char *string) {
        return [NSString stringWithUTF8String:(string ? string : "")];
    }
    id serializeJSONObject(NSData *data) {
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
    void startLogPageView(const char * pageName){
        [JANALYTICSService startLogPageView:createNSString(pageName)];
    }
    void stopLogPageView(const char * pageName){
        [JANALYTICSService stopLogPageView:createNSString(pageName)];
    }
    void loginEvent(const char * loginType){
        JANALYTICSLoginEvent * event = [[JANALYTICSLoginEvent alloc] init];
        event.success = YES;
        event.method = createNSString(loginType);
        [JANALYTICSService eventRecord:event];
    }
    void countEvent(const char * eventId,const char * extra){
        JANALYTICSCountEvent * event = [[JANALYTICSCountEvent alloc] init];
        event.eventID = createNSString(eventId);
        NSString *extras = createNSString(extra);
        if ([extras length]) {
            NSData *data = [extras dataUsingEncoding:NSUTF8StringEncoding];
            NSDictionary *dict = serializeJSONObject(data);
            event.extra = dict;
        }
        [JANALYTICSService eventRecord:event];
    }
    void calculateEvent(const char * eventId,const char * value,const char * extra){
        JANALYTICSCalculateEvent * event = [[JANALYTICSCalculateEvent alloc] init];
        event.eventID = createNSString(eventId);
        event.value = createNSString(value).floatValue;
        NSString *extras = createNSString(extra);
        if ([extras length]) {
            NSData *data = [extras dataUsingEncoding:NSUTF8StringEncoding];
            NSDictionary *dict = serializeJSONObject(data);
            event.extra = dict;
        }
        [JANALYTICSService eventRecord:event];
    }
    void browseEvent(const char * eventId,const char * name,const char * type,const char * duration,const char * extra){
        JANALYTICSBrowseEvent * event = [[JANALYTICSBrowseEvent alloc] init];
        event.contentID = createNSString(eventId);
        event.name = createNSString(name);
        event.type = createNSString(type);
        event.duration = createNSString(duration).floatValue;
        NSString *extras = createNSString(extra);
        if ([extras length]) {
            NSData *data = [extras dataUsingEncoding:NSUTF8StringEncoding];
            NSDictionary *dict = serializeJSONObject(data);
            event.extra = dict;
        }
        [JANALYTICSService eventRecord:event];
    }
    
}
