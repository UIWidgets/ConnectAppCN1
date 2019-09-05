//
//  AVPlayerPlugin.m
//  Unity-iPhone
//
//  Created by luo on 2019/8/19.
//

#import "AVPlayerController.h"
#import "UnityAppController.h"
#import "NSString+Cookie.h"


@interface AVPlayerPlugin : NSObject

@end

@implementation AVPlayerPlugin

@end


extern "C"{
    void InitPlayer(char* url,char* cookie,float left,float top,float width,float height,bool isPop);
    void VideoPlay();
    void VideoPause();
    void VideoResume();
    void VideoStop();
    void VideoReplay();
    void VideoRelease();
}
void InitPlayer(char* url,char* cookie,float left,float top,float width,float height,bool isPop){
    AppController_SendNotificationWithArg(@"UpdateStatusBarStyle",
                                          @{@"key":@"supportOrientation",@"value":@(YES)});
    NSString* cookieStr = [NSString stringWithCString:cookie encoding:NSUTF8StringEncoding];
    AVPlayerController * avp = [AVPlayerController shareInstance];
    NSString* videoUrl = [NSString stringWithCString:url encoding:NSUTF8StringEncoding];
    [avp initPlayerWithVideoUrl:videoUrl cookie:cookieStr left:left top:top width:width height:height isPop:isPop];
}

void VideoPlay(){
    AVPlayerController * avp = [AVPlayerController shareInstance];
    [avp play];
}

void VideoPause(){
    AVPlayerController * avp = [AVPlayerController shareInstance];
    [avp pause];
}
void VideoRelease(){
    AppController_SendNotificationWithArg(@"UpdateStatusBarStyle",
                                          @{@"key":@"supportOrientation",@"value":@(NO)});
    AVPlayerController * avp = [AVPlayerController shareInstance];
    [avp removePlayer];
}
