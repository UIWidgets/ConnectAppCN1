//
//  OpenPhotoController.h
//  Unity-iPhone
//
//  Created by luo on 2019/7/9.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface PickImageController : NSObject <UIImagePickerControllerDelegate,UINavigationControllerDelegate>

@property (nonatomic, strong) UIImagePickerController *picker;

- (void)pickImageWithSource:(NSString *)source cropped:(BOOL)cropped maxSize:(NSInteger)maxSize;
- (void)pickVideoWithSource:(NSString *)source;
+ (instancetype)sharedInstance;
- (bool)isPhotoLibraryAuthorization;
- (bool)isCameraAuthorization;
@end

NS_ASSUME_NONNULL_END
