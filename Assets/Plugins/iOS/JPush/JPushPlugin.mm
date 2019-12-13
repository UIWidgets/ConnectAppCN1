//
//  JPushPlugin.m
//  Unity-iPhone
//
//  Created by luo on 2019/4/29.
//

#import "JPushPlugin.h"
#include "UIWidgetsMessageManager.h"
#import "JPUSHService.h"

#if defined(__cplusplus)
extern "C" {
#endif
    extern NSString*  CreateNSString (const char* string);
    extern id         APNativeJSONObject(NSData *data);
#if defined(__cplusplus)
}
#endif

@implementation JPushPlugin

+ (nonnull instancetype) instance {
    static id _shared;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        _shared = [[self alloc] init];
    });
    return _shared;
}

@end

#if defined(__cplusplus)
extern "C" {
#endif
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
    
    JPUSHTagsOperationCompletion tagsOperationCompletion = ^(NSInteger iResCode, NSSet *iTags, NSInteger seq) {
    };
    
    JPUSHAliasOperationCompletion aliasOperationCompletion = ^(NSInteger iResCode, NSString *iAlias, NSInteger seq) {
    };
    
    void listenCompleted(){
        BOOL needPush = false;
        if ([JPushPlugin instance].pushJson.length > 0||[JPushPlugin instance].schemeUrl.length > 0||[JPushPlugin instance].universalLink.length > 0) {
            needPush = true;
        }
        NSError *error = nil;
        NSData *data = [NSJSONSerialization dataWithJSONObject:@{@"push":@(needPush)} options:0 error:&error];
        NSString *jsonStr = [[NSString alloc]initWithData:data encoding:NSUTF8StringEncoding];
        UIWidgetsMethodMessage(@"jpush", @"CompletedCallback", @[jsonStr]);
        
        if ([JPushPlugin instance].pushJson.length > 0) {
            UIWidgetsMethodMessage(@"jpush", @"OnOpenNotification", @[[JPushPlugin instance].pushJson]);
        }
        if ([JPushPlugin instance].schemeUrl.length > 0) {
            UIWidgetsMethodMessage(@"jpush", @"OnOpenUrl", @[[JPushPlugin instance].schemeUrl]);
        }
        if ([JPushPlugin instance].universalLink.length > 0) {
            UIWidgetsMethodMessage(@"jpush", @"OnOpenUniversalLinks", @[[JPushPlugin instance].universalLink]);
        }
    }
    
    void setChannel(const char * channel){
    }
    
    // Tag & Alias - start
    void setTags(int sequence, const char *tags) {
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
    
    void setAlias(int sequence, const char * alias){
        NSString *nsAlias = CreateNSString(alias);
        if (![nsAlias length]) {
            return ;
        }
        [JPUSHService setAlias:nsAlias completion:aliasOperationCompletion seq:(NSInteger)sequence];
    }
    
    void deleteAlias(int sequence, const char * alias) {
        [JPUSHService deleteAlias:aliasOperationCompletion seq:(NSInteger)sequence];
    }
    
#if defined(__cplusplus)
}
#endif
