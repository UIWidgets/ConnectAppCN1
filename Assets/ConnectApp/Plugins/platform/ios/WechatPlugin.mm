//
//  WechatPlugin.m
//  Unity-iPhone
//
//  Created by luo on 2019/4/1.
//

#import "WechatPlugin.h"
#import "WXApi.h"
#include "UIWidgetsMessageManager.h"

@implementation WechatPlugin

+ (nonnull instancetype) instance {
    static id _shared;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        _shared = [[self alloc] init];
    });
    return _shared;
}
- (void)initialize:(nonnull NSString*) appId {
    [WXApi registerApp:appId];
}
- (void)tryWechatLogin:(nonnull NSString*) stateId {
    SendAuthReq* request = [[SendAuthReq alloc]init];
    request.scope = @"snsapi_userinfo";
    request.state = stateId;
    [WXApi sendReq:request];
}
- (void)shareTo:(int)scene
          title:(NSString*)title
    description:(NSString*)description
            url:(NSString*)url
     imageBytes:(NSData*)imageBytes{
    WXMediaMessage *message = [WXMediaMessage message];
    message.title = title;
    message.description = description;
//    message.thumbData = imageBytes;
    
    WXWebpageObject *webpage = [WXWebpageObject object];
    webpage.webpageUrl = url;
    message.mediaObject = webpage;
    
    SendMessageToWXReq *req = [[SendMessageToWXReq alloc] init];
    req.bText = NO;
    req.message = message;
    req.scene = scene;
    
    [WXApi sendReq:req];
}

-(BOOL)isInstallWechat{
    return [WXApi isWXAppInstalled];
}

- (void) sendEvent:(NSDictionary*) model {
  NSError *error = nil;
  NSData *json = [NSJSONSerialization dataWithJSONObject:model options:NSJSONWritingPrettyPrinted error: &error];
  if (json != nil && error == nil) {
    NSString *jsonString = [[NSString alloc] initWithData:json encoding:NSUTF8StringEncoding];
    UIWidgetsMethodMessage(@"wechat", @"callback", @[jsonString]);
  }
}

- (void) sendCodeEvent:(NSString*)code stateId:(nonnull NSString*)stateId {
  if (stateId == nil) {
    [self sendEvent:@{@"type": @"cancel"}];
  } else {
    [self sendEvent:@{@"type": @"code",
                      @"code": code,
                      @"id": stateId}];
  }
}


@end

extern "C" {

    void initWechat(const char * appId){
            NSString *app=[NSString stringWithUTF8String:appId];
            [[WechatPlugin instance] initialize:app];
        }
        
    void loginWechat(const char * stateId){
        NSString *state=[NSString stringWithUTF8String:stateId];
        [[WechatPlugin instance]tryWechatLogin:state];
    }
    
    void toFriends(const char * title,const char * description,const char * url,const uint8_t * imageBytes){
        NSData *data = [NSData dataWithBytesNoCopy:imageBytes length:imageBytes.length];
        [[WechatPlugin instance]shareTo:WXSceneSession title:[NSString stringWithUTF8String:title] description:[NSString stringWithUTF8String:description] url:[NSString stringWithUTF8String:url] imageBytes:data];
    }
    void toTimeline(const char * title,const char * description,const char * url,const uint8_t * imageBytes){
        NSData *data = [NSData dataWithBytesNoCopy:imageBytes length:imageBytes.length];
        [[WechatPlugin instance]shareTo:WXSceneTimeline title:[NSString stringWithUTF8String:title] description:[NSString stringWithUTF8String:description] url:[NSString stringWithUTF8String:url] imageBytes:data];
    }
    BOOL isInstallWechat(){
        return [[WechatPlugin instance]isInstallWechat];
    }
    
}
