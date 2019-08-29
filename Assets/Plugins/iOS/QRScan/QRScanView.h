//
//  QRScanView.h
//  Unity-iPhone
//
//  Created by unity on 2019/7/24.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface QRScanView : UIView

@property (nonatomic, assign) CGSize transparentArea;
@property (nonatomic, strong) NSTimer *timer;
@property (nonatomic, assign) BOOL isShowTimer;

@end

NS_ASSUME_NONNULL_END
