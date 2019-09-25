//
//  ImageCropController.h
//  Unity-iPhone
//
//  Created by luo on 2019/7/10.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN
typedef void (^CropBlock)(UIImage *image);
typedef void (^CancelBlock)();

@interface ImageCropController : UIViewController

@property (nonatomic, strong) UIImage *image;
@property (nonatomic, copy) CropBlock cropBlock;
@property (nonatomic, copy) CancelBlock cancelBlock;

@end

NS_ASSUME_NONNULL_END
