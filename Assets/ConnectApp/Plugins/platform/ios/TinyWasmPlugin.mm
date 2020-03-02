//
//  TinyWasmPlugin.m
//  Unity-iPhone
//
//  Created by wangshuang on 2020/2/18.
//

#import "TinyWasmPlugin.h"
#import "TinyWasmViewPlayer.h"

@implementation TinyWasmPlugin

@end

extern "C" {
    void pushToTinyWasmScreen(char* url, char* name) {
        NSString *urlStr = [NSString stringWithUTF8String:url ? url : ""];
        NSString *nameStr = [NSString stringWithUTF8String:name ? name : ""];
        TinyWasmViewPlayer *player = [TinyWasmViewPlayer instance];
        [player loadTinyWasmWithUrl:urlStr name:nameStr];
    }
}
