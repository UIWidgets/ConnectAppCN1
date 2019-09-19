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
    void InitPlayer(char* url,char* cookie,float left,float top,float width,float height,bool isPop,bool needUpdate,int limitSeconds);
    void ConfigPlayer(char* url,char* cookie);
    void VideoPlay();
    void VideoPause();
    void VideoResume();
    void VideoStop();
    void VideoReplay();
    void VideoRelease();
    void VideoHidden();
    void VideoShow();
}
void InitPlayer(char* url,char* cookie,float left,float top,float width,float height,bool isPop,bool needUpdate,int limitSeconds){
    NSString* cookieStr = [NSString stringWithCString:cookie encoding:NSUTF8StringEncoding];
    AVPlayerController * avp = [AVPlayerController shareInstance];
    NSString* videoUrl = [NSString stringWithCString:url encoding:NSUTF8StringEncoding];
    [avp initPlayerWithVideoUrl:videoUrl cookie:cookieStr left:left top:top width:width height:height isPop:isPop needUpdate:needUpdate limitSeconds:limitSeconds];
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
    AVPlayerController * avp = [AVPlayerController shareInstance];
    [avp removePlayer];
}

void VideoHidden(){
    AVPlayerController * avp = [AVPlayerController shareInstance];
    [avp hidden];
}
void VideoShow(){
    AVPlayerController * avp = [AVPlayerController shareInstance];
    [avp show];
}
void ConfigPlayer(char* url,char* cookie){
    NSString* cookieStr = [NSString stringWithCString:cookie encoding:NSUTF8StringEncoding];
    NSString* videoUrl = [NSString stringWithCString:url encoding:NSUTF8StringEncoding];
    AVPlayerController * avp = [AVPlayerController shareInstance];
    [avp configPlayerWithVideUrl:videoUrl cookie:cookieStr];
}
