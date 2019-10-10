#import "UnityViewControllerBase.h"
#import "UnityAppController.h"
#import "UnityAppController+ViewHandling.h"
#import "PluginBase/UnityViewControllerListener.h"

@implementation UnityViewControllerBase
{
    BOOL _isHidden;
    BOOL _isLight;
}
- (BOOL)shouldAutorotate
{
    return YES;
}
- (void)viewWillLayoutSubviews
{
    [super viewWillLayoutSubviews];
    AppController_SendUnityViewControllerNotification(kUnityViewWillLayoutSubviews);
}

- (void)viewDidLayoutSubviews
{
    [super viewDidLayoutSubviews];
    AppController_SendUnityViewControllerNotification(kUnityViewDidLayoutSubviews);
}

- (void)viewDidDisappear:(BOOL)animated
{
    [super viewDidDisappear: animated];
    AppController_SendUnityViewControllerNotification(kUnityViewDidDisappear);
}

- (void)viewWillDisappear:(BOOL)animated
{
    [super viewWillDisappear: animated];
    AppController_SendUnityViewControllerNotification(kUnityViewWillDisappear);
}

- (void)viewDidAppear:(BOOL)animated
{
    [super viewDidAppear: animated];
    AppController_SendUnityViewControllerNotification(kUnityViewDidAppear);
}

- (void)viewWillAppear:(BOOL)animated
{
    [super viewWillAppear: animated];
    AppController_SendUnityViewControllerNotification(kUnityViewWillAppear);
}
-(void)viewDidLoad{
    [super viewDidLoad];
    _isHidden = YES;
    [[NSNotificationCenter defaultCenter]addObserver:self selector:@selector(updateApperance:) name:@"UpdateStatusBarStyle" object:nil];
}
- (BOOL)prefersStatusBarHidden
{
    return _isHidden;
}

- (UIStatusBarStyle)preferredStatusBarStyle
{
    return _isLight ? UIStatusBarStyleLightContent: UIStatusBarStyleDefault;
}

-(void)updateApperance:(NSNotification *)na{
    if ([[na.userInfo objectForKey:@"key"]isEqualToString:@"style"]) {
        _isLight = [[na.userInfo valueForKey:@"value"] boolValue];
        
    }
    if ([[na.userInfo objectForKey:@"key"]isEqualToString:@"hidden"]){
        _isHidden = [[na.userInfo valueForKey:@"value"] boolValue];
    }
    [self setNeedsStatusBarAppearanceUpdate];
    
}

@end
