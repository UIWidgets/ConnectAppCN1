//
//  TinyWasmPlugin.m
//  Unity-iPhone
//
//  Created by wangshuang on 2020/2/18.
//

#import "TinyWasmPlugin.h"

@implementation TinyWasmPlugin

@end

extern "C" {
    void pushToTinyWasmScreen(char* url, char* name) {
    }
}
