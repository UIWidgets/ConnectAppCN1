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
- (void)showPicker:(UIImagePickerControllerSourceType)type
{
    _picker = [[UIImagePickerController alloc] init];
    _picker.delegate = self;
    _picker.sourceType = type;
    _picker.allowsEditing = NO;
    UIViewController *vc = UnityGetGLViewController();
    [vc presentViewController:_picker animated:YES completion:nil];
    [self buildAlertController];
    
}
- (void)buildAlertController
{
    if ([self isCameraAuthorization]) {
        return;
    }
    UIViewController *vc = _picker;
    NSString *appName = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleDisplayName"];
    NSString *message = [NSString stringWithFormat:@"%@没有获得照相机的使用权限，请在设置中开启", appName];
    UIAlertController *alert = [UIAlertController alertControllerWithTitle:nil message:message preferredStyle:UIAlertControllerStyleAlert];
    [alert addAction:[UIAlertAction actionWithTitle:@"取消" style:UIAlertActionStyleCancel handler:^(UIAlertAction *action) {
        [vc dismissViewControllerAnimated:YES completion:nil];
    }]];
    [alert addAction:[UIAlertAction actionWithTitle:@"开启" style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
        [vc dismissViewControllerAnimated:YES completion:nil];
        NSURL *url = [NSURL URLWithString:UIApplicationOpenSettingsURLString];
        if ([[UIApplication sharedApplication] canOpenURL:url]) {
            [[UIApplication sharedApplication] openURL:url];
        }
    }]];
    [vc presentViewController:alert animated:YES completion:nil];
}
- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary<UIImagePickerControllerInfoKey,id> *)info{
    
    NSString *type = [info objectForKey:UIImagePickerControllerMediaType];
    //当选择的类型是图片
    if ([type isEqualToString:@"public.image"])
    {
        NSString *key = UIImagePickerControllerOriginalImage;
        //获取图片
        UIImage *image = [info objectForKey:key];
        ImageCropController *crop = [[ImageCropController alloc]init];
        crop.image = image;
        __weak __typeof(self) wSelf = self;
        crop.block = ^(UIImage * _Nonnull resizeImage) {
            __strong __typeof(wSelf) sSelf = wSelf;
            NSData *data = UIImageJPEGRepresentation(resizeImage, 0.5f);
            NSString *encodedImageStr = [data base64EncodedStringWithOptions:NSDataBase64Encoding64CharacterLineLength];
            NSData *json = [NSJSONSerialization dataWithJSONObject:@{@"image":encodedImageStr} options:NSJSONWritingPrettyPrinted error: nil];
            NSString *jsonString = [[NSString alloc] initWithData:json encoding:NSUTF8StringEncoding];
            UIWidgetsMethodMessage(@"pickImage", @"success", @[jsonString]);
            [sSelf pickImageDissmiss];
        };
        crop.cancelBlock = ^{
            __strong __typeof(wSelf) sSelf = wSelf;
            UIWidgetsMethodMessage(@"pickImage", @"cancel", @[]);
            [sSelf pickImageDissmiss];
        };
        [picker pushViewController:crop animated:YES];
    }
}

- (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker{
    [self pickImageDissmiss];
}
- (void)pickImageDissmiss{
    [_picker dismissViewControllerAnimated:YES completion:^{
        
    }];
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

