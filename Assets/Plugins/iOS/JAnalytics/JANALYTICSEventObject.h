/*
 *	| |    | |  \ \  / /  | |    | |   / _______|
 *	| |____| |   \ \/ /   | |____| |  / /
 *	| |____| |    \  /    | |____| |  | |   _____
 * 	| |    | |    /  \    | |    | |  | |  |____ |
 *  | |    | |   / /\ \   | |    | |  \ \______| |
 *  | |    | |  /_/  \_\  | |    | |   \_________|
 *
 * Copyright (c) 2016~ Shenzhen HXHG. All rights reserved.
 */

#import <Foundation/Foundation.h>
/**
 *
 * @abstract 事件对象共同父类
 *
 * @discussion 所有的字符串属性长度不能超过256字节（包括extra的key和value）
 *
 */
@interface JANALYTICSEventObject : NSObject

@property (nonatomic, strong, nonnull) NSDictionary<NSString *, NSString *>* extra;

@end

/**
 登录事件对象
 */
@interface JANALYTICSLoginEvent : JANALYTICSEventObject

//登录方法,非空
@property (nonatomic, copy, nonnull) NSString* method;
//登录是否成功，非空
@property (nonatomic, assign) BOOL success;

@end

/**
 注册事件对象
 */
@interface JANALYTICSRegisterEvent : JANALYTICSEventObject

//注册方法,非空
@property (nonatomic, copy, nonnull) NSString* method;
//注册是否成功，非空
@property (nonatomic, assign) BOOL success;

@end

typedef NS_ENUM(NSUInteger, JANALYTICSPurchaseCurrency) {
  //人民币
  JANALYTICSCurrencyCNY,
  //美元
  JANALYTICSCurrencyUSD
};
/**
 购买事件对象
 */
@interface JANALYTICSPurchaseEvent : JANALYTICSEventObject

//价格 非空
@property (nonatomic, assign) CGFloat price;
//购买是否成功，非空
@property (nonatomic, assign) BOOL success;
//物品ID
@property (nonatomic, copy, nonnull) NSString* goodsID;
//物品名称
@property (nonatomic, copy, nonnull) NSString* goodsName;
//物品类型
@property (nonatomic, copy, nonnull) NSString* goodsType;
//货币类型 默认CNY
@property (nonatomic, assign) JANALYTICSPurchaseCurrency currency;
//物品数量
@property (nonatomic, assign) NSInteger quantity;

@end

/**
 内容浏览事件对象
 */
@interface JANALYTICSBrowseEvent : JANALYTICSEventObject

//内容名称,非空
@property (nonatomic, copy, nonnull) NSString* name;
//内容ID
@property (nonatomic, copy, nonnull) NSString* contentID;
//内容类型
@property (nonatomic, copy, nonnull) NSString* type;
//内容时长
@property (nonatomic, assign) CGFloat duration;

@end

/**
 自定义计数事件对象
 */
@interface JANALYTICSCountEvent : JANALYTICSEventObject

//事件ID 非空
@property (nonatomic, copy, nonnull) NSString* eventID;

@end

/**
 自定义计算事件对象
 */
@interface JANALYTICSCalculateEvent : JANALYTICSEventObject

//事件ID 非空
@property (nonatomic, copy, nonnull) NSString* eventID;
//事件值 非空
@property (nonatomic, assign) CGFloat value;

@end

typedef NS_ENUM(NSUInteger, JANALYTICSSex) {
  //未知的
  JANALYTICSSexUnknown,
  //男性
  JANALYTICSSexMale,
  //女性
  JANALYTICSSexFemale,
};

typedef NS_ENUM(NSUInteger, JANALYTICSPaid) {
  //未知
  JANALYTICSPaidUnknown,
  //付费
  JANALYTICSPaidPaid,
  //未付费
  JANALYTICSPaidUnpaid,
};

/**
 用户信息
 */
@interface JANALYTICSUserInfo : NSObject

/**
 账号ID、必填非空
 */
@property (nonatomic, copy, nonnull) NSString * accountID;
/*
 * 以下为极光内置用户维度
 * 当主动设置为nil时会删除该维度
 */
//账号创建时间、时间戳
@property (nonatomic, assign) NSTimeInterval creationTime;
//不能使用枚举意外的值
@property (nonatomic, assign) JANALYTICSSex sex;
//出生年月，yyyyMMdd格式校验
@property (nonatomic, copy, nullable) NSString * birthdate;
//不能使用枚举以外的值
@property (nonatomic, assign) JANALYTICSPaid paid;
//手机号码
@property (nonatomic, copy, nullable) NSString * phone;
//email
@property (nonatomic, copy, nullable) NSString * email;
//用户名
@property (nonatomic, copy, nullable) NSString * name;
//微信id
@property (nonatomic, copy, nullable) NSString * wechatID;
//QQid
@property (nonatomic, copy, nullable) NSString * qqID;
//新浪微博id
@property (nonatomic, copy, nullable) NSString * weiboID;

//用户自定义维度 key-value value只能为NSNumber/NSString/nil
//当value为nil时将会删除对应的维度
- (void)setExtraObject:(nullable id)obj forKey:(nonnull NSString *)key;

@end

