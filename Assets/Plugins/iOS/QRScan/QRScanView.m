//
//  QRScanView.m
//  Unity-iPhone
//
//  Created by unity on 2019/7/24.
//

#import "QRScanView.h"
#import "QRScanUtil.h"

static NSTimeInterval kQrLineAnimateDuration = 0.02;

@interface QRScanView ()

@property (nonatomic, strong) UIImageView *qrScanLine;
@property (nonatomic, assign) CGFloat qrScanLineY;
@property (nonatomic, strong) UILabel *qrScanLabel;

@end

@implementation QRScanView

- (void)layoutSubviews
{
    [super layoutSubviews];
    if (!_qrScanLine) {
        [self initQRScanLine];
        NSTimer *timer = [NSTimer scheduledTimerWithTimeInterval:kQrLineAnimateDuration target:self selector:@selector(show) userInfo:nil repeats:YES];
        self.timer = timer;
        [timer fire];
    }
}

- (void)initQRScanLine
{
    CGRect screenBounds = [QRScanUtil screenBounds];
    _qrScanLine = [[UIImageView alloc] initWithFrame:CGRectMake(screenBounds.size.width / 2 - self.transparentArea.width / 2, screenBounds.size.height / 2 - self.transparentArea.height / 2, self.transparentArea.width, 2)];
    _qrScanLine.image = [UIImage imageNamed:@"qrScanLine"];
    _qrScanLine.contentMode = UIViewContentModeScaleAspectFill;
    [self addSubview:_qrScanLine];
    _qrScanLineY = _qrScanLine.frame.origin.y;
    
    _qrScanLabel = [[UILabel alloc] initWithFrame:CGRectMake(0.0, screenBounds.size.height / 2 - self.transparentArea.height / 2 + self.transparentArea.height + 20.0, screenBounds.size.width, 30.0)];
    _qrScanLabel.backgroundColor = [UIColor clearColor];
    _qrScanLabel.text = @"将二维码放入框内，即可自动扫描";
    _qrScanLabel.textColor = [UIColor whiteColor];
    _qrScanLabel.textAlignment = NSTextAlignmentCenter;
    _qrScanLabel.font = [UIFont systemFontOfSize:13.0f];
    [self addSubview:_qrScanLabel];
}

#pragma mark - 定时器的方法
- (void)show
{
    [UIView animateWithDuration:kQrLineAnimateDuration animations:^{
        CGRect rect = _qrScanLine.frame;
        rect.origin.y = _qrScanLineY;
        _qrScanLine.frame = rect;
    } completion:^(BOOL finished) {
        CGFloat maxBorder = self.frame.size.height / 2 + self.transparentArea.height / 2 - 33;
        if (_qrScanLineY > maxBorder) {
            _qrScanLineY = self.frame.size.height / 2 - self.transparentArea.height / 2 - 15;
        }
        _qrScanLineY++;
    }];
}

- (void)drawRect:(CGRect)rect
{
    //整个二维码扫描界面的颜色
    CGSize screenSize = [QRScanUtil screenBounds].size;
    CGRect screenDrawRect = CGRectMake(0, 0, screenSize.width, screenSize.height);
    
    //中间清空的矩形框
    CGRect clearDrawRect = CGRectMake(screenDrawRect.size.width / 2 - self.transparentArea.width / 2,
                                      screenDrawRect.size.height / 2 - self.transparentArea.height / 2,
                                      self.transparentArea.width,
                                      self.transparentArea.height);
    
    CGContextRef ctx = UIGraphicsGetCurrentContext();
    [self addScreenFillRect:ctx rect:screenDrawRect];
    
    [self addCenterClearRect:ctx rect:clearDrawRect];
    
    [self addWhiteRect:ctx rect:clearDrawRect];
    
    [self addCornerLineWithContext:ctx rect:clearDrawRect];
}

- (void)addScreenFillRect:(CGContextRef)ctx rect:(CGRect)rect
{
    CGContextSetRGBFillColor(ctx, 40 / 255.0, 40 / 255.0, 40 / 255.0, 0.5);
    CGContextFillRect(ctx, rect);   //draw the transparent layer
}

- (void)addCenterClearRect:(CGContextRef)ctx rect:(CGRect)rect
{
    CGContextClearRect(ctx, rect);  //clear the center rect  of the layer
}

- (void)addWhiteRect:(CGContextRef)ctx rect:(CGRect)rect
{
    CGContextStrokeRect(ctx, rect);
    CGContextSetRGBStrokeColor(ctx, 255 / 255, 255 / 255, 255 / 255, 0.5);
    CGContextSetLineWidth(ctx, 0.8);
    CGContextAddRect(ctx, rect);
    CGContextStrokePath(ctx);
}

- (void)addCornerLineWithContext:(CGContextRef)ctx rect:(CGRect)rect
{
    //画四个边角
    CGContextSetLineWidth(ctx, 2);
    CGContextSetRGBStrokeColor(ctx, 33 / 255.0, 150 / 255.0, 243 / 255.0, 1);//绿色
    
    //左上角
    CGPoint pointTopLeftA[] = {
        CGPointMake(rect.origin.x + 0.7, rect.origin.y),
        CGPointMake(rect.origin.x + 0.7, rect.origin.y + 15)
    };
    CGPoint pointTopLeftB[] = {
        CGPointMake(rect.origin.x, rect.origin.y + 0.7),
        CGPointMake(rect.origin.x + 15, rect.origin.y + 0.7)
    };
    [self addLine:pointTopLeftA pointB:pointTopLeftB ctx:ctx];
    
    //左下角
    CGPoint pointBottomLeftA[] = {
        CGPointMake(rect.origin.x + 0.7, rect.origin.y + rect.size.height - 15),
        CGPointMake(rect.origin.x + 0.7, rect.origin.y + rect.size.height)
    };
    CGPoint pointBottomLeftB[] = {
        CGPointMake(rect.origin.x, rect.origin.y + rect.size.height - 0.7),
        CGPointMake(rect.origin.x + 0.7 + 15, rect.origin.y + rect.size.height - 0.7)
    };
    [self addLine:pointBottomLeftA pointB:pointBottomLeftB ctx:ctx];
    
    //右上角
    CGPoint pointTopRightA[] = {
        CGPointMake(rect.origin.x + rect.size.width - 15, rect.origin.y + 0.7),
        CGPointMake(rect.origin.x + rect.size.width, rect.origin.y + 0.7)
    };
    CGPoint pointTopRightB[] = {
        CGPointMake(rect.origin.x + rect.size.width - 0.7, rect.origin.y),
        CGPointMake(rect.origin.x + rect.size.width - 0.7, rect.origin.y + 15 + 0.7)
    };
    [self addLine:pointTopRightA pointB:pointTopRightB ctx:ctx];
    
    //右下角
    CGPoint pointBottomRightA[] = {
        CGPointMake(rect.origin.x + rect.size.width - 0.7, rect.origin.y + rect.size.height + -15),
        CGPointMake(rect.origin.x - 0.7 + rect.size.width, rect.origin.y + rect.size.height)
    };
    CGPoint pointBottomRightB[] = {
        CGPointMake(rect.origin.x + rect.size.width - 15, rect.origin.y + rect.size.height - 0.7),
        CGPointMake(rect.origin.x + rect.size.width, rect.origin.y + rect.size.height - 0.7)
    };
    [self addLine:pointBottomRightA pointB:pointBottomRightB ctx:ctx];
    
    CGContextStrokePath(ctx);
}

- (void)addLine:(CGPoint[])pointA pointB:(CGPoint[])pointB ctx:(CGContextRef)ctx
{
    CGContextAddLines(ctx, pointA, 2);
    CGContextAddLines(ctx, pointB, 2);
}

- (void)dealloc
{
    [self.timer invalidate];
}

@end
