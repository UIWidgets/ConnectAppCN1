//
//  OpenPhotoController.m
//  Unity-iPhone
//
//  Created by luo on 2019/7/9.
//

#import "PickImageController.h"
#import <Photos/Photos.h>
#import "ImageCropController.h"
#include "UIWidgetsMessageManager.h"
@interface PickImageController()

@property (nonatomic, assign) BOOL allowsEditing;
@property (nonatomic, assign) NSInteger maxSize;
@property (nonatomic, assign) BOOL isPickFinish;

@end

@implementation PickImageController

static PickImageController *controller = nil;

+ (instancetype)sharedInstance
{
    if (controller == nil) {
        static dispatch_once_t onceToken;
        dispatch_once(&onceToken, ^{
            controller = [[PickImageController alloc] init];
        });
    }
    return controller;
}

- (void)pickImageWithSource:(NSString *)source cropped:(BOOL)cropped maxSize:(NSInteger)maxSize
{
    _allowsEditing = cropped;
    _maxSize = maxSize;
    _isPickFinish = false;
    _picker = [[UIImagePickerController alloc] init];
    _picker.delegate = self;
    if (source.integerValue == 0) {
        _picker.sourceType = UIImagePickerControllerSourceTypeCamera;
    }
    if (source.integerValue == 1) {
        _picker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
    }
    _picker.mediaTypes = @[@"public.image"];
    _picker.allowsEditing = NO;
    UIViewController *vc = UnityGetGLViewController();
    [vc presentViewController:_picker animated:YES completion:nil];
    [self buildAlertController];
}

- (void)pickVideoWithSource:(NSString *)source
{
    _isPickFinish = false;
    _picker = [[UIImagePickerController alloc] init];
    _picker.delegate = self;
    if (source.integerValue == 0) {
        _picker.sourceType = UIImagePickerControllerSourceTypeCamera;
    }
    if (source.integerValue == 1) {
        _picker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
    }
    _picker.mediaTypes = @[@"public.movie"];
    _picker.allowsEditing = YES;
    _picker.videoMaximumDuration = 15;
    UIViewController *vc = UnityGetGLViewController();
    [vc presentViewController:_picker animated:YES completion:nil];
    [self buildAlertController];
}

- (void)buildAlertController
{
    if ([self isCameraAuthorization]) {
        return;
    }
    NSString *appName = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleDisplayName"];
    NSString *message = [NSString stringWithFormat:@"%@没有获得照相机的使用权限，请在设置中开启", appName];
    UIAlertController *alert = [UIAlertController alertControllerWithTitle:nil message:message preferredStyle:UIAlertControllerStyleAlert];
    [alert addAction:[UIAlertAction actionWithTitle:@"取消" style:UIAlertActionStyleCancel handler:^(UIAlertAction *action) {
        [_picker dismissViewControllerAnimated:YES completion:nil];
    }]];
    [alert addAction:[UIAlertAction actionWithTitle:@"开启" style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
        [_picker dismissViewControllerAnimated:YES completion:nil];
        NSURL *url = [NSURL URLWithString:UIApplicationOpenSettingsURLString];
        if ([[UIApplication sharedApplication] canOpenURL:url]) {
            [[UIApplication sharedApplication] openURL:url];
        }
    }]];
    [_picker presentViewController:alert animated:YES completion:nil];
}

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary<UIImagePickerControllerInfoKey,id> *)info{
    if (_isPickFinish) return;
    _isPickFinish = true;
    NSString *type = [info objectForKey:UIImagePickerControllerMediaType];
    //当选择的类型是图片
    if ([type isEqualToString:@"public.image"]) {
        
        //获取图片
        UIImage *image = [self fixOrientationWithImage:[info objectForKey:UIImagePickerControllerOriginalImage]];
        if (_allowsEditing) {
            ImageCropController *crop = [[ImageCropController alloc] init];
            crop.image = image;
            __weak __typeof(self) wSelf = self;
            crop.cropBlock = ^(UIImage * resizeImage) {
                __strong __typeof(wSelf) sSelf = wSelf;
                NSData *data = [sSelf compressWithImage:resizeImage];
                NSString *jsonString = [sSelf dataToString:data];
                UIWidgetsMethodMessage(@"pickImage", @"pickImageSuccess", @[jsonString]);
                [sSelf pickImageDissmiss];
            };
            crop.cancelBlock = ^{
                __strong __typeof(wSelf) sSelf = wSelf;
                UIWidgetsMethodMessage(@"pickImage", @"cancel", @[]);
                [sSelf pickImageDissmiss];
            };
            [picker pushViewController:crop animated:YES];
        } else {
            NSData *data = [self compressWithImage:image];
            NSString *jsonString = [self dataToString:data];
            UIWidgetsMethodMessage(@"pickImage", @"pickImageSuccess", @[jsonString]);
            [self pickImageDissmiss];
        }
    }
    if ([type isEqualToString:@"public.movie"]) {
        NSURL *videoURL = [info objectForKey:UIImagePickerControllerMediaURL];
        if (videoURL != nil) {
            NSData *data = [NSData dataWithContentsOfURL:videoURL];
            NSString *encodedVideoStr = [data base64EncodedStringWithOptions:NSDataBase64Encoding64CharacterLineLength];
            NSData *json = [NSJSONSerialization dataWithJSONObject:@{@"videoData":encodedVideoStr} options:NSJSONWritingPrettyPrinted error: nil];
            NSString *jsonString = [[NSString alloc] initWithData:json encoding:NSUTF8StringEncoding];
            UIWidgetsMethodMessage(@"pickImage", @"pickVideoSuccess", @[jsonString]);
            [self pickImageDissmiss];
        }
    }
}

- (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker{
    UIWidgetsMethodMessage(@"pickImage", @"cancel", @[]);
    [self pickImageDissmiss];
}

- (void)pickImageDissmiss{
    [_picker dismissViewControllerAnimated:YES completion:nil];
}

- (NSData *)compressWithImage:(UIImage *)image {
    if (_maxSize <= 0.0) {
        return UIImageJPEGRepresentation(image, 0.8);
    }
    
    CGFloat compression = 1;
    NSData *data = UIImageJPEGRepresentation(image, compression);
    if (data.length < _maxSize) return data;
    
    CGFloat max = 1;
    CGFloat min = 0;
    for (int i = 0; i < 6; ++i) {
        compression = (max + min) / 2;
        data = UIImageJPEGRepresentation(image, compression);
        if (data.length < _maxSize * 0.9) {
            min = compression;
        } else if (data.length > _maxSize) {
            max = compression;
        } else {
            break;
        }
    }

    if (data.length < _maxSize) return data;
    UIImage *resultImage = [UIImage imageWithData:data];

    NSUInteger lastDataLength = 0;
    while (data.length > _maxSize && data.length != lastDataLength) {
        lastDataLength = data.length;
        CGFloat ratio = (CGFloat)_maxSize / data.length;
        CGSize size = CGSizeMake((NSUInteger)(resultImage.size.width * sqrtf(ratio)),
                                 (NSUInteger)(resultImage.size.height * sqrtf(ratio))); // Use NSUInteger to prevent white blank
        UIGraphicsBeginImageContext(size);
        [resultImage drawInRect:CGRectMake(0, 0, size.width, size.height)];
        resultImage = UIGraphicsGetImageFromCurrentImageContext();
        UIGraphicsEndImageContext();
        data = UIImageJPEGRepresentation(resultImage, compression);
    }
    return data;
}

- (NSString *)dataToString:(NSData *)data {
    NSString *encodedImageStr = [data base64EncodedStringWithOptions:NSDataBase64Encoding64CharacterLineLength];
    NSData *json = [NSJSONSerialization dataWithJSONObject:@{@"image":encodedImageStr} options:NSJSONWritingPrettyPrinted error: nil];
    NSString *jsonString = [[NSString alloc] initWithData:json encoding:NSUTF8StringEncoding];
    return jsonString;
}

- (UIImage *)fixOrientationWithImage:(UIImage *)image {
    if (image.imageOrientation == UIImageOrientationUp) return image;
    
    CGAffineTransform transform = CGAffineTransformIdentity;
    switch ((NSInteger)image.imageOrientation) {
        case UIImageOrientationDown:
        case UIImageOrientationDownMirrored:
            transform = CGAffineTransformTranslate(transform, image.size.width, image.size.height);
            transform = CGAffineTransformRotate(transform, M_PI);
            break;
            
        case UIImageOrientationLeft:
        case UIImageOrientationLeftMirrored:
            transform = CGAffineTransformTranslate(transform, image.size.width, 0);
            transform = CGAffineTransformRotate(transform, M_PI_2);
            break;
            
        case UIImageOrientationRight:
        case UIImageOrientationRightMirrored:
            transform = CGAffineTransformTranslate(transform, 0, image.size.height);
            transform = CGAffineTransformRotate(transform, -M_PI_2);
            break;
    }
    
    switch ((NSInteger)image.imageOrientation) {
        case UIImageOrientationUpMirrored:
        case UIImageOrientationDownMirrored:
            transform = CGAffineTransformTranslate(transform, image.size.width, 0);
            transform = CGAffineTransformScale(transform, -1, 1);
            break;
            
        case UIImageOrientationLeftMirrored:
        case UIImageOrientationRightMirrored:
            transform = CGAffineTransformTranslate(transform, image.size.height, 0);
            transform = CGAffineTransformScale(transform, -1, 1);
            break;
    }
    
    // Now we draw the underlying CGImage into a new context, applying the transform
    // calculated above.
    CGContextRef ctx = CGBitmapContextCreate(NULL, image.size.width, image.size.height,
                                             CGImageGetBitsPerComponent(image.CGImage), 0,
                                             CGImageGetColorSpace(image.CGImage),
                                             CGImageGetBitmapInfo(image.CGImage));
    CGContextConcatCTM(ctx, transform);
    switch (image.imageOrientation) {
        case UIImageOrientationLeft:
        case UIImageOrientationLeftMirrored:
        case UIImageOrientationRight:
        case UIImageOrientationRightMirrored:
            CGContextDrawImage(ctx, CGRectMake(0, 0, image.size.height, image.size.width), image.CGImage);
            break;
            
        default:
            CGContextDrawImage(ctx, CGRectMake(0, 0, image.size.width, image.size.height), image.CGImage);
            break;
    }
    
    CGImageRef cgimg = CGBitmapContextCreateImage(ctx);
    UIImage *img = [UIImage imageWithCGImage:cgimg];
    CGContextRelease(ctx);
    CGImageRelease(cgimg);
    return img;
}

- (bool)isPhotoLibraryAuthorization{
    PHAuthorizationStatus status = [PHPhotoLibrary authorizationStatus];
    if (status == PHAuthorizationStatusRestricted ||
        status == PHAuthorizationStatusDenied) {
        //无权限
        return NO;
    }
    return YES;
}

- (bool)isCameraAuthorization{
    AVAuthorizationStatus authStatus = [AVCaptureDevice authorizationStatusForMediaType:AVMediaTypeVideo];
    if (authStatus == AVAuthorizationStatusRestricted || authStatus ==AVAuthorizationStatusDenied) {
        return NO;
    }
    return YES;
}
@end

