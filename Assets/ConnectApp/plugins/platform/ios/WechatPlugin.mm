//
//  WechatPlugin.m
//  Unity-iPhone
//
//  Created by luo on 2019/4/1.
//

#import "WechatPlugin.h"
#import "WXApi.h"

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
//    message.thumbData = imageBytes.data;
    
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
@end

extern "C" {
    void InitWechat(NSString * appId){
        [[WechatPlugin instance] initialize: appId];
    }
    
    void loginWechat(NSString* stateId){
        [[WechatPlugin instance]tryWechatLogin:stateId];
    }
    
    void shareToWechat(){
        
    }
    BOOL isInstallWechat(){
        return [[WechatPlugin instance]isInstallWechat];
    }
    
}
