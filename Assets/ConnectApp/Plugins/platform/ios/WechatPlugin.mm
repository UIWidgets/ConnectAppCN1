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
    message.thumbData = imageBytes;
    
    WXWebpageObject *webpage = [WXWebpageObject object];
    webpage.webpageUrl = url;
    message.mediaObject = webpage;
    
    SendMessageToWXReq *req = [[SendMessageToWXReq alloc] init];
    req.bText = NO;
    req.message = message;
    req.scene = scene;
    
    [WXApi sendReq:req];
}

- (void)shareToMiniProgramWithTitle:(NSString*)title
                        description:(NSString*)description
                                url:(NSString*)url
                         imageBytes:(NSData*)imageBytes
                           userName:(NSString*)userName
                               path:(NSString*)path
                    miniProgramType:(WXMiniProgramType)miniProgramType{
    WXMiniProgramObject *object = [WXMiniProgramObject object];
    object.webpageUrl = url;
    object.userName = userName;
    object.path = path;
    object.hdImageData = imageBytes;
    object.withShareTicket = true;
    object.miniProgramType = miniProgramType;
    
    WXMediaMessage *message = [WXMediaMessage message];
    message.title = title;
    message.description = description;
    message.thumbData = imageBytes;  //兼容旧版本节点的图片，小于32KB，新版本优先
    //使用WXMiniProgramObject的hdImageData属性
    message.mediaObject = object;
    
    SendMessageToWXReq *req = [[SendMessageToWXReq alloc] init];
    req.bText = NO;
    req.message = message;
    req.scene = WXSceneSession;  //目前只支持会话
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
    
    void loginWechat(const char * stateId){
        NSString *state=[NSString stringWithUTF8String:stateId];
        [[WechatPlugin instance]tryWechatLogin:state];
    }
    
    void toFriends(const char * title,const char * description,const char * url,const char * imageStr){
        if (imageStr==NULL) {
            [[WechatPlugin instance]shareTo:WXSceneSession title:[NSString stringWithUTF8String:title] description:[NSString stringWithUTF8String:description] url:[NSString stringWithUTF8String:url] imageBytes:nil];
            return;
        }
        NSString *base64String=[NSString stringWithUTF8String:imageStr];
        NSData *data = [[NSData alloc]initWithBase64EncodedString:base64String options:0];
        [[WechatPlugin instance]shareTo:WXSceneSession title:[NSString stringWithUTF8String:title] description:[NSString stringWithUTF8String:description] url:[NSString stringWithUTF8String:url] imageBytes:data];
    }
    void toTimeline(const char * title,const char * description,const char * url,const char * imageStr){
        if (imageStr==NULL) {
            [[WechatPlugin instance]shareTo:WXSceneTimeline title:[NSString stringWithUTF8String:title] description:[NSString stringWithUTF8String:description] url:[NSString stringWithUTF8String:url] imageBytes:nil];
            return;
        }
        NSString *base64String=[NSString stringWithUTF8String:imageStr];
        NSData *data = [[NSData alloc]initWithBase64EncodedString:base64String options:0];
        [[WechatPlugin instance]shareTo:WXSceneTimeline title:[NSString stringWithUTF8String:title] description:[NSString stringWithUTF8String:description] url:[NSString stringWithUTF8String:url] imageBytes:data];
    }
    void toMiNiProgram(const char * title,const char * description,const char * url,const char * imageStr,const char * ysId,const char * path,int miniProgramType){
        if (imageStr==NULL) {
            [[WechatPlugin instance]shareToMiniProgramWithTitle:[NSString stringWithUTF8String:title] description:[NSString stringWithUTF8String:description] url:[NSString stringWithUTF8String:url] imageBytes:nil userName:[NSString stringWithUTF8String:ysId] path:[NSString stringWithUTF8String:path] miniProgramType:(WXMiniProgramType)miniProgramType];
            return;
        }
        NSString *base64String=[NSString stringWithUTF8String:imageStr];
        NSData *data = [[NSData alloc]initWithBase64EncodedString:base64String options:0];
        [[WechatPlugin instance]shareToMiniProgramWithTitle:[NSString stringWithUTF8String:title] description:[NSString stringWithUTF8String:description] url:[NSString stringWithUTF8String:url] imageBytes:data userName:[NSString stringWithUTF8String:ysId] path:[NSString stringWithUTF8String:path] miniProgramType:(WXMiniProgramType)miniProgramType];
    }
    BOOL isInstallWechat(){
        return [[WechatPlugin instance]isInstallWechat];
    }
    void openMiNi(const char * ysId,const char * path,int miniProgramType){
        NSString *userName=[NSString stringWithUTF8String:ysId];
        NSString *reqPath=[NSString stringWithUTF8String:path];
        WXLaunchMiniProgramReq *launchMiniProgramReq = [WXLaunchMiniProgramReq object];
        launchMiniProgramReq.userName = userName;  //拉起的小程序的username
        launchMiniProgramReq.path = reqPath;    //拉起小程序页面的可带参路径，不填默认拉起小程序首页
        launchMiniProgramReq.miniProgramType = (WXMiniProgramType)miniProgramType; //拉起小程序的类型
        [WXApi sendReq:launchMiniProgramReq];
    }
    
}
