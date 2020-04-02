//
//  AVPlayerController.m
//  Unity-iPhone
//
//  Created by luo on 2019/8/19.
//

#import "AVPlayerController.h"
#include "UIWidgetsMessageManager.h"
#import "NSString+Cookie.h"
#import "Masonry.h"

static AVPlayerController *avp = nil;

@implementation AVPlayerController
{
    float _marginTop;
}
+(id)shareInstance{
    if(avp == nil){
        avp = [[super allocWithZone:nil]init];
    }
    return avp;
}

+(id)allocWithZone:(NSZone *)zone{
    return [AVPlayerController shareInstance];
}
- (WMPlayer *)wmPlayer{
    if (!_wmPlayer) {
        _wmPlayer = [[WMPlayer alloc]init];
    }
    return _wmPlayer;
}

- (void)initPlayerWithVideoUrl:(NSString*)videoUrl cookie:(NSString*)cookie left:(CGFloat)left top:(CGFloat)top width:(CGFloat)width height:(CGFloat)height isPop:(BOOL)isPop needUpdate:(BOOL)needUpdate limitSeconds:(int)limitSeconds{
    _marginTop = top;
    if (videoUrl.length>0) {
        self.wmPlayer.playerModel = [self setupModelWithUrl:videoUrl cookie:cookie needUpdate:needUpdate limitSeconds:limitSeconds];
    }
    self.wmPlayer.backBtnStyle = isPop?BackBtnStylePop:BackBtnStyleNone;
    self.wmPlayer.loopPlay = NO;//设置是否循环播放
    self.wmPlayer.tintColor = [UIColor colorWithRed:243.0/255 green:33.0/255 blue:148.0/255 alpha:1];//改变播放器着色
    self.wmPlayer.enableBackgroundMode = NO;//开启后台播放模式
    self.wmPlayer.delegate = self;
    self.wmPlayer.loopPlay = NO;
    [UnityGetGLView() addSubview:self.wmPlayer];
    [self.wmPlayer mas_makeConstraints:^(MASConstraintMaker *make) {
        make.leading.trailing.equalTo(self.wmPlayer.superview);
        make.top.mas_equalTo(top);
        make.height.mas_equalTo(self.wmPlayer.mas_width).multipliedBy(9.0/16);
    }];
    if (!isPop) {
        [self.wmPlayer play];
    }
    //获取设备旋转方向的通知,即使关闭了自动旋转,一样可以监测到设备的旋转方向
    [[UIDevice currentDevice] beginGeneratingDeviceOrientationNotifications];
    //旋转屏幕通知
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(onDeviceOrientationChange:)
                                                 name:UIDeviceOrientationDidChangeNotification
                                               object:nil
     ];
}

-(WMPlayerModel *)setupModelWithUrl:(NSString *)videoUrl cookie:(NSString *)cookie needUpdate:(BOOL)needUpdate limitSeconds:(int)limitSeconds{
    NSString *Cookie = [NSString stringWithFormat:@"%@; path=/; domain=.unity.cn;",cookie];
    WMPlayerModel *model = [[WMPlayerModel alloc]init];
    NSDictionary *options = @{AVURLAssetHTTPCookiesKey : @[[Cookie cookie]]};
    AVURLAsset *videoURLAsset = [AVURLAsset URLAssetWithURL:[NSURL URLWithString:videoUrl] options:options];
    model.playerItem = [AVPlayerItem playerItemWithAsset:videoURLAsset];
    model.verticalVideo = false;
    model.needUpdateLincense = needUpdate;
    model.limitSeconds = limitSeconds;
    return model;
}
- (void)wmplayer:(WMPlayer *)wmplayer clickedBuyLinceseButton:(UIButton *)shareBtn{
    UIWidgetsMethodMessage(@"player", @"BuyLincese", @[@""]);
}
- (void)wmplayer:(WMPlayer *)wmplayer clickedUpdateLinceseButton:(UIButton *)updateBtn{
    UIWidgetsMethodMessage(@"player", @"UpdateLincese", @[@""]);
}

- (void)wmplayer:(WMPlayer *)wmplayer clickedCloseButton:(UIButton *)backBtn{
    if (wmplayer.isFullscreen) {
        [[UIDevice currentDevice] setValue:@(UIInterfaceOrientationPortrait) forKey:@"orientation"];
    }else{
        UIWidgetsMethodMessage(@"player", @"PopPage", @[@""]);
        [self removePlayer];
    }
}
- (void)wmplayer:(WMPlayer *)wmplayer clickedShareButton:(UIButton *)shareBtn{
    UIWidgetsMethodMessage(@"player", @"Share", @[@""]);
}
-(void)wmplayer:(WMPlayer *)wmplayer clickedFullScreenButton:(UIButton *)fullScreenBtn{
    NSNumber *orientationUnknown = [NSNumber numberWithInt:0];
    [[UIDevice currentDevice] setValue:orientationUnknown forKey:@"orientation"];
    if (self.wmPlayer.isFullscreen) {//全屏
        //强制翻转屏幕，Home键在下边。
        [[UIDevice currentDevice] setValue:@(UIInterfaceOrientationPortrait) forKey:@"orientation"];
    }else{//非全屏
        [[UIDevice currentDevice] setValue:@(UIInterfaceOrientationLandscapeRight) forKey:@"orientation"];
    }
    //刷新
    [UIViewController attemptRotationToDeviceOrientation];
}
- (void)play{
    if (self.wmPlayer) {
        [self.wmPlayer play];
    }
}

- (void)pause{
    if (self.wmPlayer) {
        [self.wmPlayer pause];
    }
}

- (void)show{
    if (self.wmPlayer) {
        self.wmPlayer.hidden = NO;
    }
}

- (void)hidden{
    if (self.wmPlayer) {
        self.wmPlayer.hidden = YES;
    }
}

- (void)configPlayerWithVideUrl:(NSString *)url cookie:(NSString *)cookie{
    if (self.wmPlayer) {
        [self.wmPlayer setPlayerModel:[self setupModelWithUrl:url cookie:cookie needUpdate:false limitSeconds:0]];
    }
}


- (void)removePlayer{
    [self.wmPlayer resetWMPlayer];
    [self.wmPlayer pause];
    [self.wmPlayer removeFromSuperview];
    self.wmPlayer = nil;
    [[NSNotificationCenter defaultCenter]removeObserver:self];
}
/**
 *  旋转屏幕通知
 */
- (void)onDeviceOrientationChange:(NSNotification *)notification{
    //    if (self.wmPlayer.isLockScreen){
    //        return;
    //    }
    UIDeviceOrientation orientation = [UIDevice currentDevice].orientation;
    UIInterfaceOrientation interfaceOrientation = (UIInterfaceOrientation)orientation;
    switch (interfaceOrientation) {
        case UIInterfaceOrientationPortraitUpsideDown:{
        }
            break;
        case UIInterfaceOrientationPortrait:{
            [self toOrientation:UIInterfaceOrientationPortrait];
        }
            break;
        case UIInterfaceOrientationLandscapeLeft:{
            [self toOrientation:UIInterfaceOrientationLandscapeLeft];
        }
            break;
        case UIInterfaceOrientationLandscapeRight:{
            [self toOrientation:UIInterfaceOrientationLandscapeRight];
        }
            break;
        default:
            break;
    }
}

//点击进入,退出全屏,或者监测到屏幕旋转去调用的方法
-(void)toOrientation:(UIInterfaceOrientation)orientation{
    if (orientation ==UIInterfaceOrientationPortrait) {
        [self.wmPlayer mas_remakeConstraints:^(MASConstraintMaker *make) {
            make.leading.trailing.equalTo(self.wmPlayer.superview);
            make.top.mas_equalTo(_marginTop);
            make.height.mas_equalTo(self.wmPlayer.mas_width).multipliedBy(9.0/16);
        }];
        self.wmPlayer.isFullscreen = NO;
    }else{
        [self.wmPlayer mas_remakeConstraints:^(MASConstraintMaker *make) {
            make.edges.mas_equalTo(UIEdgeInsetsMake(0, 0, 0, 0));
        }];
        self.wmPlayer.isFullscreen = YES;
    }
}
@end
