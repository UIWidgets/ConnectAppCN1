//
//  ImageCropController.m
//  Unity-iPhone
//
//  Created by luo on 2019/7/10.
//

#import "ImageCropController.h"
#import "JPImageresizeView/JPImageresizerConfigure.h"
#import "JPImageresizeView/JPImageresizerView.h"

@interface ImageCropController ()
@property (weak, nonatomic) IBOutlet UIImageView *imageView;
@property (weak, nonatomic) IBOutlet UIButton *cancelButton;
@property (weak, nonatomic) IBOutlet UIButton *recoveryButton;
@property (weak, nonatomic) IBOutlet UIButton *doneButton;
@property (weak, nonatomic) IBOutlet UIButton *rotateButton;
@property (nonatomic, weak) JPImageresizerView *imageresizerView;
@end

@implementation ImageCropController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    UIEdgeInsets contentInsets = UIEdgeInsetsMake(50, 0, (40 + 30 + 30 + 10), 0);
    BOOL isX = [UIScreen mainScreen].bounds.size.height > 736.0;
    if (isX) {
        contentInsets.top += 24;
        contentInsets.bottom += 34;
    }
    JPImageresizerConfigure *configure = [JPImageresizerConfigure defaultConfigureWithResizeImage:self.image make:^(JPImageresizerConfigure *configure) {
        configure
        .jp_resizeImage(self.image)
        .jp_maskAlpha(0.5)
        .jp_strokeColor([UIColor whiteColor])
        .jp_frameType(JPClassicFrameType)
        .jp_contentInsets(contentInsets)
        .jp_bgColor([UIColor blackColor])
        .jp_isClockwiseRotation(YES)
        .jp_resizeWHScale(1.0)
        .jp_animationCurve(JPAnimationCurveEaseOut);
    }];
    self.view.backgroundColor = configure.bgColor;
    
    self.recoveryButton.enabled = NO;
    
    __weak __typeof(self) wSelf = self;
    JPImageresizerView *imageresizerView = [JPImageresizerView imageresizerViewWithConfigure:configure imageresizerIsCanRecovery:^(BOOL isCanRecovery) {
        __strong __typeof(wSelf) sSelf = wSelf;
        if (!sSelf) return;
        // 当不需要重置设置按钮不可点
        sSelf.recoveryButton.enabled = isCanRecovery;
    } imageresizerIsPrepareToScale:^(BOOL isPrepareToScale) {
        __strong __typeof(wSelf) sSelf = wSelf;
        if (!sSelf) return;
        // 当预备缩放设置按钮不可点，结束后可点击
        BOOL enabled = !isPrepareToScale;
        sSelf.rotateButton.enabled = enabled;
    }];
    [self.view insertSubview:imageresizerView atIndex:0];
    self.imageresizerView = imageresizerView;
    // 注意：iOS11以下的系统，所在的controller最好设置automaticallyAdjustsScrollViewInsets为NO，不然就会随导航栏或状态栏的变化产生偏移
    if (@available(iOS 11.0, *)) {
        
    } else {
        self.automaticallyAdjustsScrollViewInsets = NO;
    }
    
}

- (void)viewWillAppear:(BOOL)animated{
    [super viewWillAppear:animated];
    [self.navigationController setNavigationBarHidden:YES animated:true];

}

- (void)viewDidDisappear:(BOOL)animated{
    [super viewDidDisappear:animated];
    [self.navigationController setNavigationBarHidden:NO animated:true];
}

- (IBAction)cancel:(UIButton *)sender {
    if (self.cancelBlock) {
        self.cancelBlock();
    }
}

- (IBAction)recovery:(UIButton *)sender {
    [self.imageresizerView recovery];
}

- (IBAction)done:(UIButton *)sender {
    self.recoveryButton.enabled = NO;
    
    __weak __typeof(self) wSelf = self;
    
    // 1.默认以imageView的宽度为参照宽度进行裁剪
    [self.imageresizerView imageresizerWithComplete:^(UIImage *resizeImage) {
        __strong __typeof(wSelf) sSelf = wSelf;
        if (!sSelf) return;
        
        if (!resizeImage) {
            return;
        }
        sSelf.recoveryButton.enabled = YES;
        if (sSelf.cropBlock) {
            sSelf.cropBlock(resizeImage);
        }
    }];
}

- (IBAction)rotate:(id)sender {
    [self.imageresizerView rotation];
}

@end
