//
//  TinyWasmViewController.m
//  Unity-iPhone
//
//  Created by wangshuang on 2020/2/17.
//

#import "TinyWasmViewController.h"
#import "TinyViewController.h"
#import "TinyApp.h"
#import "Masonry.h"

#define WeakSelf(type) __weak __typeof__(type) weakSelf = type;
#define StrongSelf(type) __strong __typeof__(type) strongSelf = type;

@interface TinyWasmViewController ()

@property  NSString* url;
@property  NSString* name;
@property  UIActivityIndicatorView* indicatorView;
@property  TinyViewController* tinyViewController;
@property  UIButton* backButton;

@end

@implementation TinyWasmViewController

- (instancetype)initWithUrl:(NSString *)url name:(NSString *)name {
    if (self = [super init]) {
        self.url = url;
        self.name = name;
    }
    return self;
}

- (void)viewDidLoad {
    [super viewDidLoad];
    [self.view setBackgroundColor:[UIColor blackColor]];
    self.tinyViewController = [[TinyViewController alloc] init];
    self.indicatorView = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleWhite];
    [self.view addSubview:self.indicatorView];
    self.backButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [self.backButton setImage:[UIImage imageNamed:@"arrowBack"] forState:UIControlStateNormal];
    [self.backButton addTarget:self action:@selector(backToMainScreen:) forControlEvents:UIControlEventTouchUpInside];
    [self.view addSubview:self.backButton];
    [self addUIControlConstraints];
    [self.indicatorView startAnimating];
    if (![self.url  isEqual: @""] && ![self.name  isEqual: @""]) {
        [self launchTiny:self.url wasmName:self.name];
    }
}

//添加控件的约束
- (void)addUIControlConstraints {
    [self.indicatorView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.center.mas_equalTo(self.view);
    }];
    [self.backButton mas_makeConstraints:^(MASConstraintMaker *make) {
        make.size.mas_equalTo(CGSizeMake(44, 44));
        make.left.top.mas_equalTo(0);
    }];
}

- (void)backToMainScreen:(UIButton *)sender {
    [self dismissViewControllerAnimated:YES completion:nil];
}

- (void)launchTiny:(NSString*)baseUrl wasmName:(NSString*)wasmName {
    WeakSelf(self);
    [[TinyApp instance] load:baseUrl wasmName:wasmName completionHandler:^(NSError *error) {
            StrongSelf(weakSelf);
           if (error != nil) {
               NSLog(@"fail to load:%@", error);
               dispatch_async(dispatch_get_main_queue(), ^{
                  [strongSelf.indicatorView stopAnimating];
               });
           } else {
               NSLog(@"load complete");
               dispatch_async(dispatch_get_main_queue(), ^{
                  strongSelf.tinyViewController.tinyapp = [TinyApp instance];
                   [strongSelf addChildViewController:strongSelf.tinyViewController];
                   [strongSelf.view insertSubview:strongSelf.tinyViewController.view aboveSubview:strongSelf.indicatorView];
                   [strongSelf.indicatorView stopAnimating];
               });
           }
       }];
}

- (BOOL)shouldAutorotate {
    return YES;
}

- (UIInterfaceOrientationMask)supportedInterfaceOrientations {
    return UIInterfaceOrientationMaskAll;
}

@end
