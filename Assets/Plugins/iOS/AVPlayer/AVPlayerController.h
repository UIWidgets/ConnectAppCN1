//
//  AVPlayerController.h
//  Unity-iPhone
//
//  Created by luo on 2019/8/19.
//

#import <Foundation/Foundation.h>
#import <AVFoundation/AVFoundation.h>
#import "WMPlayer.h"

NS_ASSUME_NONNULL_BEGIN

@interface AVPlayerController : NSObject<WMPlayerDelegate>


+(id)shareInstance;

@property (nonatomic, strong)WMPlayer  *wmPlayer;


@property (nonatomic, copy) NSString *cookie;

- (void)initPlayerWithVideoUrl:(NSString*)videoUrl cookie:(NSString*)cookie left:(CGFloat)left top:(CGFloat)top width:(CGFloat)width height:(CGFloat)height isPop:(BOOL)isPop needUpdate:(BOOL)needUpdate limitSeconds:(int)limitSeconds;

- (void)configPlayerWithVideUrl:(NSString *)url cookie:(NSString *)cookie;

- (void)play;

- (void)pause;

- (void)removePlayer;

- (void)show;

- (void)hidden;

@end

NS_ASSUME_NONNULL_END
