//
//  TinyWasmViewController.m
//  Unity-iPhone
//
//  Created by wangshuang on 2020/2/17.
//

#import "TinyWasmViewPlayer.h"
#import "TinyViewController.h"
#import "TinyApp.h"
#import "Masonry.h"
#include "UIWidgetsMessageManager.h"
#import "UnityAppController.h"
#import "UIButton+CBlock.h"

#define WeakSelf(type) __weak __typeof__(type) weakSelf = type;
#define StrongSelf(type) __strong __typeof__(type) strongSelf = type;

@interface TinyWasmViewPlayer ()

@property (nonatomic, strong) UIActivityIndicatorView *indicatorView;
@property (nonatomic, strong) TinyViewController *tinyViewController;
@property (nonatomic, strong) UIAlertController *alertController;
@property (nonatomic, strong) UIButton *backButton;
@property (nonatomic, strong) UIView *tinyWasmView;
@property (nonatomic, strong) UIView *tempView;
@property (nonatomic) BOOL isOpen;

@end

@implementation TinyWasmViewPlayer

- (void)loadTinyWasmWithUrl:(NSString *) url name: (NSString *)name {
    UIView *unityView = UnityGetGLView();
    
    // init
    self.tinyWasmView = [[UIView alloc] initWithFrame:unityView.bounds];
    [self.tinyWasmView setBackgroundColor:[UIColor blackColor]];
    
    self.tempView = [[UIView alloc] initWithFrame:unityView.bounds];
    [self.tempView setBackgroundColor:[UIColor clearColor]];
    
    self.tinyViewController = [[TinyViewController alloc] init];
    self.indicatorView = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleWhite];
    
    self.backButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [self.backButton setImage:[UIImage imageNamed:@"arrowBack"] forState:UIControlStateNormal];
    [self.backButton addAction:^(UIButton *btn) {
        [self doTransitionToTempView];
        self.isOpen = NO;
    } forControlEvents:UIControlEventTouchUpInside];
    
    self.alertController = [UIAlertController alertControllerWithTitle:@"加载失败"
           message:@"请退出重试"
    preferredStyle:UIAlertControllerStyleAlert];
    [self.alertController addAction:[UIAlertAction actionWithTitle:@"确定" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        if (self.isOpen) {
            [self doTransitionToTempView];
            self.isOpen = NO;
        }
    }]];
    
    [self.tinyWasmView addSubview:self.indicatorView];
    [self.tinyWasmView addSubview:self.backButton];
    [unityView addSubview:self.tempView];
    [self doTransitionToTinyWasmView];
    [self.indicatorView startAnimating];
    
    UIWidgetsMethodMessage(@"tinyWasm", @"init", @[]);
    if (![url  isEqual: @""] && ![name  isEqual: @""]) {
        [self launchTiny:url wasmName:name];
    }
    self.isOpen = YES;
}

- (void)doTransitionToTempView {
    WeakSelf(self);
    [UIView transitionFromView:self.tinyWasmView
                        toView:self.tempView
                      duration:1
                       options:UIViewAnimationOptionTransitionFlipFromRight
                    completion:^(BOOL finished){
                        StrongSelf(weakSelf);
                        UIWidgetsMethodMessage(@"tinyWasm", @"popPage", @[]);
                        [strongSelf.tinyWasmView removeFromSuperview];
                        [strongSelf.tempView removeFromSuperview];
                        [strongSelf.tinyViewController removeFromParentViewController];
                        strongSelf.tinyWasmView = nil;
                        strongSelf.tinyViewController.tinyapp = nil;
                        strongSelf.tinyViewController = nil;
                    }];
    [UnityGetGLView() addSubview:self.tempView];
    [UnityGetGLView() sendSubviewToBack:self.tempView];
    [self.tempView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.edges.mas_equalTo(UIEdgeInsetsMake(0, 0, 0, 0));
    }];
}

- (void)doTransitionToTinyWasmView {
    WeakSelf(self);
    [UIView transitionFromView:self.tempView
                        toView:self.tinyWasmView
                      duration:1
                       options:UIViewAnimationOptionTransitionFlipFromLeft
                    completion:^(BOOL finished){
                        StrongSelf(weakSelf);
                        [strongSelf.tempView removeFromSuperview];
                    }];
    [UnityGetGLView() addSubview:self.tinyWasmView];
    [UnityGetGLView() sendSubviewToBack:self.tinyWasmView];
    [self addUIControlConstraints];
}

//添加控件的约束
- (void)addUIControlConstraints {
    [self.indicatorView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.center.mas_equalTo(self.tinyWasmView);
    }];
    [self.backButton mas_makeConstraints:^(MASConstraintMaker *make) {
        make.size.mas_equalTo(CGSizeMake(44, 44));
        make.left.mas_equalTo(0);
        if (@available(iOS 11.0, *)) {
            make.top.mas_equalTo(self.tinyWasmView.mas_safeAreaLayoutGuideTop);
        } else {
            make.top.mas_equalTo(20);
        }
    }];
    [self.tinyWasmView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.edges.mas_equalTo(UIEdgeInsetsMake(0, 0, 0, 0));
    }];
}

- (void)launchTiny:(NSString*)baseUrl wasmName:(NSString*)wasmName {
    WeakSelf(self);
    [[TinyApp instance] load:baseUrl wasmName:wasmName completionHandler:^(NSError *error) {
            StrongSelf(weakSelf);
        if (self.isOpen) {
            if (error != nil) {
//                NSLog(@"fail to load:%@", error);
                WeakSelf(strongSelf);
                dispatch_async(dispatch_get_main_queue(), ^{
                    StrongSelf(weakSelf);
                    [strongSelf.indicatorView stopAnimating];
                    if (strongSelf.alertController != nil) {
                        [UnityGetGLViewController() presentViewController:strongSelf.alertController animated:YES completion:nil];
                    }
                });
            } else {
//                NSLog(@"load complete");
                WeakSelf(strongSelf);
                dispatch_async(dispatch_get_main_queue(), ^{
                    StrongSelf(weakSelf);
                   strongSelf.tinyViewController.tinyapp = [TinyApp instance];
                    [UnityGetGLViewController() addChildViewController:strongSelf.tinyViewController];
                    [self.tinyWasmView insertSubview:strongSelf.tinyViewController.view belowSubview:strongSelf.backButton];
                    [strongSelf.tinyViewController.view mas_makeConstraints:^(MASConstraintMaker *make) {
                        make.edges.mas_equalTo(UIEdgeInsetsMake(0, 0, 0, 0));
                    }];
                    [strongSelf.indicatorView stopAnimating];
                });
            }
        }
       }];
}

@end
