//
//  FPSLabelManager.m
//  Unity-iPhone
//
//  Created by wangshuang on 2020/2/29.
//

#import "FPSLabelManager.h"
#import "YYFPSLabel.h"
#import "Masonry.h"

@interface FPSLabelManager ()

@property (nonatomic, strong) YYFPSLabel *fpsLabel;

@end

@implementation FPSLabelManager

+ (nonnull instancetype)instance {
    static id _shared;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        _shared = [[self alloc] init];
    });
    return _shared;
}

- (void)switchFPSLabelShowStatus:(BOOL)isOpen {
    if (isOpen) {
        self.fpsLabel = [YYFPSLabel new];
        [UnityGetGLView() addSubview:self.fpsLabel];
        [self.fpsLabel mas_makeConstraints:^(MASConstraintMaker *make) {
            make.size.mas_equalTo(CGSizeMake(60, 30));
            make.right.mas_equalTo(-2);
            if (@available(iOS 11.0, *)) {
                make.top.mas_equalTo(UnityGetGLView().mas_safeAreaLayoutGuideTop).offset(2);
            } else {
                make.top.mas_equalTo(2);
            }
        }];
    } else {
        [self.fpsLabel removeFromSuperview];
        self.fpsLabel = nil;
    }
}

@end
