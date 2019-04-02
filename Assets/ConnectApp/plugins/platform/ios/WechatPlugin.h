//
//  WechatPlugin.h
//  Unity-iPhone
//
//  Created by luo on 2019/4/1.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface WechatPlugin : NSObject

+ (nonnull instancetype) instance;

- (void) sendCodeEvent:(nonnull NSString*)code stateId:(nonnull NSString*)stateId;

@end

NS_ASSUME_NONNULL_END
