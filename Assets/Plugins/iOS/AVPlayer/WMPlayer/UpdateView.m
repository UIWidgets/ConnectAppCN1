//
//  UpdateView.m
//  Unity-iPhone
//
//  Created by luo on 2019/9/16.
//

#import "UpdateView.h"
#import "Masonry.h"

@implementation UpdateView

- (instancetype)init
{
    self = [super init];
    if (self) {
        self.userInteractionEnabled = YES;
        self.backgroundColor = [UIColor blackColor];
        [self addSubviews];
    }
    return self;
}

-(void)addSubviews{
    [self addSubview: self.titleLabel];
    [self addSubview: self.updateButton];
    [self addSubview: self.buyButton];
    
    [self.titleLabel mas_makeConstraints:^(MASConstraintMaker *make) {
        make.top.mas_equalTo(self.mas_centerY).offset(-100);
        make.leading.mas_equalTo(16);
        make.trailing.mas_equalTo(-16);
        make.height.mas_equalTo(30);
    }];
    [self.updateButton mas_makeConstraints:^(MASConstraintMaker *make) {
        make.top.mas_equalTo(self.titleLabel.mas_bottom).mas_offset(32);
        make.centerX.mas_equalTo(self.mas_centerX);
        make.width.mas_equalTo(193);
        make.height.mas_equalTo(40);
    }];
    
    [self.buyButton mas_makeConstraints:^(MASConstraintMaker *make) {
        make.top.mas_equalTo(self.updateButton.mas_bottom).mas_offset(16);
        make.centerX.mas_equalTo(self.mas_centerX);
        make.width.mas_equalTo(193);
        make.height.mas_equalTo(40);
    }];
}
- (UILabel *)titleLabel{
    if (!_titleLabel) {
        _titleLabel = [[UILabel alloc]init];
        _titleLabel.text = @"请升级授权以继续观看";
        _titleLabel.textAlignment = NSTextAlignmentCenter;
        _titleLabel.font = [UIFont systemFontOfSize:16.0];
        _titleLabel.textColor = [UIColor whiteColor];
    }
    return _titleLabel;
}
- (UIButton *)updateButton{
    if (!_updateButton) {
        _updateButton = [UIButton buttonWithType:UIButtonTypeCustom];
        [_updateButton setTitle:@"购买Learn Premium" forState:UIControlStateNormal];
        [_updateButton setTitleColor:[UIColor whiteColor] forState:UIControlStateNormal];
        [_updateButton setBackgroundColor:[self colorWithHexString:@"2196f3"]];
        _updateButton.titleLabel.font = [UIFont systemFontOfSize:16];
        _updateButton.layer.cornerRadius = 20;
        _updateButton.layer.masksToBounds = YES;
        _updateButton.titleEdgeInsets = UIEdgeInsetsMake(8, 16, 8, 16);

    }
    return _updateButton;
}
- (UIButton *)buyButton{
    if (!_buyButton) {
        _buyButton = [UIButton buttonWithType:UIButtonTypeCustom];
        [_buyButton setTitle:@"购买Pro版本" forState:UIControlStateNormal];
        [_buyButton setTitleColor:[UIColor whiteColor] forState:UIControlStateNormal];
        [_buyButton setBackgroundColor:[self colorWithHexString:@"2196f3"]];
        _buyButton.titleLabel.font = [UIFont systemFontOfSize:16];
        _buyButton.layer.cornerRadius = 20;
        _buyButton.layer.masksToBounds = YES;
        _buyButton.titleEdgeInsets = UIEdgeInsetsMake(8, 16, 8, 16);
    }
    return _buyButton;
}
- (CGSize)sizeWithText:(NSString *)text font:(UIFont *)font{
    NSDictionary *attrs = @{NSFontAttributeName : font};
    return [text boundingRectWithSize:CGSizeMake(MAXFLOAT, MAXFLOAT) options:NSStringDrawingUsesLineFragmentOrigin attributes:attrs context:nil].size;
    
}
// 颜色转换三：iOS中十六进制的颜色（以#开头）转换为UIColor
- (UIColor *) colorWithHexString: (NSString *)color
{
    NSString *cString = [[color stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]] uppercaseString];
    
    // String should be 6 or 8 characters
    if ([cString length] < 6) {
        return [UIColor clearColor];
    }
    
    // 判断前缀并剪切掉
    if ([cString hasPrefix:@"0X"])
        cString = [cString substringFromIndex:2];
    if ([cString hasPrefix:@"#"])
        cString = [cString substringFromIndex:1];
    if ([cString length] != 6)
        return [UIColor clearColor];
    
    // 从六位数值中找到RGB对应的位数并转换
    NSRange range;
    range.location = 0;
    range.length = 2;
    
    //R、G、B
    NSString *rString = [cString substringWithRange:range];
    
    range.location = 2;
    NSString *gString = [cString substringWithRange:range];
    
    range.location = 4;
    NSString *bString = [cString substringWithRange:range];
    
    // Scan values
    unsigned int r, g, b;
    [[NSScanner scannerWithString:rString] scanHexInt:&r];
    [[NSScanner scannerWithString:gString] scanHexInt:&g];
    [[NSScanner scannerWithString:bString] scanHexInt:&b];
    
    return [UIColor colorWithRed:((float) r / 255.0f) green:((float) g / 255.0f) blue:((float) b / 255.0f) alpha:1.0f];
}
@end
