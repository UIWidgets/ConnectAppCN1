//
//  UIButton+CBlock.m
//  Unity-iPhone
//
//  Created by wangshuang on 2020/2/20.
//

#import "UIButton+CBlock.h"

#import <objc/runtime.h>

@implementation UIButton (Block)
static char ActionTag;

/**
 *  button 添加点击事件 默认点击方式UIControlEventTouchUpInside
 *
 *  @param block
 */
- (void)addAction:(ButtonBlock)block {
    objc_setAssociatedObject(self, &ActionTag, block, OBJC_ASSOCIATION_COPY_NONATOMIC);
    [self addTarget:self action:@selector(action:) forControlEvents:UIControlEventTouchUpInside];
}

/**
 *  button 添加事件
 *
 *  @param block
 *  @param controlEvents 点击的方式
 */
- (void)addAction:(ButtonBlock)block forControlEvents:(UIControlEvents)controlEvents {
    objc_setAssociatedObject(self, &ActionTag, block, OBJC_ASSOCIATION_COPY_NONATOMIC);
    [self addTarget:self action:@selector(action:) forControlEvents:controlEvents];
}

/**
 *  button 事件的响应方法
 *
 *  @param sender
 */
- (void)action:(id)sender {
    ButtonBlock blockAction = (ButtonBlock)objc_getAssociatedObject(self, &ActionTag);
    if (blockAction) {
        blockAction(self);
    }
}
@end
