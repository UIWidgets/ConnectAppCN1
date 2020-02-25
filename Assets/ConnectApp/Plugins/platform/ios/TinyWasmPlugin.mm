//
//  TinyWasmPlugin.m
//  Unity-iPhone
//
//  Created by wangshuang on 2020/2/18.
//

#import "TinyWasmPlugin.h"
#import "TinyWasmViewController.h"

@implementation TinyWasmPlugin

@end

extern "C" {
    void pushToTinyWasmScreen(char* url, char* name) {
        UIViewController *vc = UnityGetGLViewController();
        NSString *urlStr = [NSString stringWithUTF8String:url ? url : ""];
        NSString *nameStr = [NSString stringWithUTF8String:name ? name : ""];
        TinyWasmViewController *tinyVc = [[TinyWasmViewController alloc] initWithUrl:urlStr name:nameStr];
        [tinyVc setModalPresentationStyle:UIModalPresentationFullScreen];
        [vc presentViewController:tinyVc animated:YES completion:nil];
    }
}
