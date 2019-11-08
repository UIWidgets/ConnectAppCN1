//
//  UrlLauncherPlugin.m
//  Unity-iPhone
//
//  Created by unity on 2019/11/6.
//

#import "UrlLauncherPlugin.h"
#import <SafariServices/SafariServices.h>

@implementation UrlLauncherPlugin

+ (void)launchURLInVC:(NSString *)urlString {
    NSURL *url = [NSURL URLWithString:urlString];
    SFSafariViewController *safariVc = [[SFSafariViewController alloc] initWithURL:url];
    [UnityGetGLViewController() presentViewController:safariVc animated:YES completion:nil];
}

+ (void)launchURL:(NSString *)urlString {
    NSURL *url = [NSURL URLWithString:urlString];
    UIApplication *application = [UIApplication sharedApplication];
    if ([application respondsToSelector:@selector(openURL:options:completionHandler:)]) {
        [application openURL:url
                     options:@{}
           completionHandler:^(BOOL success) {}];
    } else {
        [application openURL:url];
    }
}

@end

extern "C" {
    void launch(const char *url, bool forceSafariVC, bool forceWebView) {
        NSString *sourceString = [NSString stringWithUTF8String:url];
        if (forceSafariVC) {
            if (@available(iOS 9.0, *)) {
                [UrlLauncherPlugin launchURLInVC:sourceString];
            } else {
                [UrlLauncherPlugin launchURL:sourceString];
            }
        } else {
            [UrlLauncherPlugin launchURL:sourceString];
        }
    }
    
    bool canLaunch(const char *url) {
        NSString *urlString = [NSString stringWithUTF8String:url];
        return [[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:urlString]];
    }
}
