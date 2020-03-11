//
//  QRScanPlugin.m
//  Unity-iPhone
//
//  Created by unity on 2019/7/24.
//

#import "QRScanPlugin.h"
#import "QRScanViewController.h"
#include "UIWidgetsMessageManager.h"

@implementation QRScanPlugin

@end

extern "C" {
    void pushToQRScan() {
        UIViewController *vc = UnityGetGLViewController();
        QRScanViewController *qrScanVc = [[QRScanViewController alloc] init];
        qrScanVc.modalPresentationStyle = UIModalPresentationFullScreen;
        qrScanVc.qrCodeBlock = ^(NSString * qrCode) {
            UIWidgetsMethodMessage(@"QRScan", @"OnReceiveQRCode", @[qrCode]);
        };
        [vc presentViewController:qrScanVc animated:YES completion:nil];
    }
}
