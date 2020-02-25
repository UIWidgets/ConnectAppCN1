//
//  UIButton+CBlock.h
//  Unity-iPhone
//
//  Created by wangshuang on 2020/2/20.
//

#import <UIKit/UIKit.h>

typedef void(^ButtonBlock)(UIButton* btn);

@interface UIButton (Block)

/**
 *  button 添加点击事件
 *
 *  @param block
 */
- (void)addAction:(ButtonBlock)block;

/**
 *  button 添加事件
 *
 *  @param block
 *  @param controlEvents 点击的方式
 */
- (void)addAction:(ButtonBlock)block forControlEvents:(UIControlEvents)controlEvents;

@end
