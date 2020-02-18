//
//  ViewController.h
//  wams3test
//
//  Created by zorro on 2020/1/7.
//  Copyright © 2020 zorro. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "TinyApp.h"

@interface TinyViewController : UIViewController

@property  TinyApp* tinyapp;
@end

@interface TinyView : UIView
{
    CADisplayLink* m_displayLink;
}
@property  TinyApp* tinyapp;
@end
