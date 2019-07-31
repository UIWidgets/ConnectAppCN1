//
//  QRScanPlugin.m
//  Unity-iPhone
//
//  Created by unity on 2019/7/24.
//

#import "QRScanPlugin.h"
#import "QRScanViewController.h"

@implementation QRScanPlugin

@end

extern "C" {
    void pushToQRScan() {
        UIViewController *vc = UnityGetGLViewController();
        [vc presentViewController:[[QRScanViewController alloc] init] animated:YES completion:nil];
    }
}
