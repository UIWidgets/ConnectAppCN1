//
//  QRScanViewController.h
//  Unity-iPhone
//
//  Created by unity on 2019/7/24.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface QRScanViewController : UIViewController

@property (nonatomic, copy) void (^qrCodeBlock)(NSString *qrCode);

@end

NS_ASSUME_NONNULL_END
