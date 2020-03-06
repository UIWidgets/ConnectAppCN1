//
//  FPSLabelPlugin.mm
//  Unity-iPhone
//
//  Created by wangshuang on 2020/2/29.
//

#import "FPSLabelPlugin.h"
#import "FPSLabelManager.h"

@implementation FPSLabelPlugin

@end

extern "C" {
    void switchFPSLabelShowStatus(bool isOpen) {
        FPSLabelManager* fpslabelManager = [FPSLabelManager instance];
        [fpslabelManager switchFPSLabelShowStatus:isOpen];
    }
}
