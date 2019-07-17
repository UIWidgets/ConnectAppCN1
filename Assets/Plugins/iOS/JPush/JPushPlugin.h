//
//  JPushPlugin.h
//  Unity-iPhone
//
//  Created by luo on 2019/4/29.
//

#import "UnityAppController.h"

NS_ASSUME_NONNULL_BEGIN

@interface JPushPlugin : NSObject

+ (nonnull instancetype) instance;

@property (nonatomic,copy) NSString *pushJson;

@property (nonatomic,copy) NSString *schemeUrl;

@property (nonatomic,copy) NSString *universalLink;


@end

NS_ASSUME_NONNULL_END
