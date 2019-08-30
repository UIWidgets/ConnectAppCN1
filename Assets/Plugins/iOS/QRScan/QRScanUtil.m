//
//  QRScanUtil.m
//  Unity-iPhone
//
//  Created by unity on 2019/7/24.
//

#import "QRScanUtil.h"

@implementation QRScanUtil

+ (CGRect)screenBounds
{
    UIScreen *screen = [UIScreen mainScreen];
    CGRect screenRect;
    if (![screen respondsToSelector:@selector(fixedCoordinateSpace)] && UIInterfaceOrientationIsLandscape([UIApplication sharedApplication].statusBarOrientation)) {
        screenRect = CGRectMake(0.0f, 0.0f, screen.bounds.size.height, screen.bounds.size.width);
    } else {
        screenRect = screen.bounds;
    }
    return screenRect;
}

+ (AVCaptureVideoOrientation)videoOrientationFromCurrentDeviceOrientation
{
    UIInterfaceOrientation orientation = [[UIApplication sharedApplication] statusBarOrientation];
    if (orientation == UIInterfaceOrientationPortrait) {
        return AVCaptureVideoOrientationPortrait;
    } else if (orientation == UIInterfaceOrientationLandscapeLeft) {
        return AVCaptureVideoOrientationLandscapeLeft;
    } else if (orientation == UIInterfaceOrientationLandscapeRight) {
        return AVCaptureVideoOrientationLandscapeRight;
    } else if (orientation == UIInterfaceOrientationPortraitUpsideDown) {
        return AVCaptureVideoOrientationPortraitUpsideDown;
    }
    return AVCaptureVideoOrientationPortrait;
}

@end
