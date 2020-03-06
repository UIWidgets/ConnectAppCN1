//
//  FPSLabelManager.h
//  Unity-iPhone
//
//  Created by wangshuang on 2020/2/29.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface FPSLabelManager : NSObject

+ (nonnull instancetype)instance;
- (void)switchFPSLabelShowStatus:(BOOL)isOpen;

@end

NS_ASSUME_NONNULL_END
