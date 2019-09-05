//
//  QRScanUtil.h
//  Unity-iPhone
//
//  Created by unity on 2019/7/24.
//

#import <Foundation/Foundation.h>
#import <AVFoundation/AVFoundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface QRScanUtil : NSObject

+ (CGRect)screenBounds;

+ (AVCaptureVideoOrientation)videoOrientationFromCurrentDeviceOrientation;

@end

NS_ASSUME_NONNULL_END
