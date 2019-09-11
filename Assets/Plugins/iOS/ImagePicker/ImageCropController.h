//
//  ImageCropController.h
//  Unity-iPhone
//
//  Created by luo on 2019/7/10.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN
typedef void (^PickImageBlock)(UIImage *image);
typedef void (^CancelBlock)();

@interface ImageCropController : UIViewController

@property (nonatomic, strong) UIImage *image;
@property (nonatomic,copy) PickImageBlock block;
@property (nonatomic,copy) CancelBlock cancelBlock;

@end

NS_ASSUME_NONNULL_END
