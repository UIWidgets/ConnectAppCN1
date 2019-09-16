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
        make.bottom.mas_equalTo(self.mas_centerY).mas_offset(-20);
        make.leading.mas_equalTo(16);
        make.trailing.mas_equalTo(-16);
        make.height.mas_equalTo(30);
    }];
    
    [self.updateButton mas_makeConstraints:^(MASConstraintMaker *make) {
        make.top.mas_equalTo(self.mas_centerY).mas_offset(8);
        make.right.mas_equalTo(self.mas_centerX).mas_offset(-16);
        make.width.mas_equalTo(60);
        make.height.mas_equalTo(30);
    }];
    
    [self.buyButton mas_makeConstraints:^(MASConstraintMaker *make) {
        make.top.mas_equalTo(self.mas_centerY).mas_offset(8);
        make.left.mas_equalTo(self.mas_centerX).mas_offset(16);
        make.width.mas_equalTo(60);
        make.height.mas_equalTo(30);
    }];
}
- (UILabel *)titleLabel{
    if (!_titleLabel) {
        _titleLabel = [[UILabel alloc]init];
        _titleLabel.text = @"请购买最新版pro";
        _titleLabel.textAlignment = NSTextAlignmentCenter;
        _titleLabel.font = [UIFont systemFontOfSize:15.0];
        _titleLabel.textColor = [UIColor whiteColor];
    }
    return _titleLabel;
}
- (UIButton *)updateButton{
    if (!_updateButton) {
        _updateButton = [UIButton buttonWithType:UIButtonTypeCustom];
        [_updateButton setTitle:@"UPDATE" forState:UIControlStateNormal];
        [_updateButton setTitleColor:[UIColor whiteColor] forState:UIControlStateNormal];
        _updateButton.titleLabel.font = [UIFont systemFontOfSize:13];
        _updateButton.layer.cornerRadius = 5;
        _updateButton.layer.masksToBounds = YES;
        _updateButton.layer.borderColor = [UIColor whiteColor].CGColor;
        _updateButton.layer.borderWidth = 1.0;
    }
    return _updateButton;
}
- (UIButton *)buyButton{
    if (!_buyButton) {
        _buyButton = [UIButton buttonWithType:UIButtonTypeCustom];
        [_buyButton setTitle:@"BUY" forState:UIControlStateNormal];
        [_buyButton setTitleColor:[UIColor whiteColor] forState:UIControlStateNormal];
        _buyButton.titleLabel.font = [UIFont systemFontOfSize:13];
        _buyButton.layer.cornerRadius = 5;
        _buyButton.layer.masksToBounds = YES;
        _buyButton.layer.borderColor = [UIColor whiteColor].CGColor;
        _buyButton.layer.borderWidth = 1.0;
    }
    return _buyButton;
}
@end
