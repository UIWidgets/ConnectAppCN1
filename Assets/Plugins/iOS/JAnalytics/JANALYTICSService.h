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

#define JANALYTICS_VERSION_NUMBER 2.0.0

#import <Foundation/Foundation.h>
#import "JANALYTICSEventObject.h"

@interface JANALYTICSLaunchConfig : NSObject

/* appKey 一个JPush 应用必须的,唯一的标识. 请参考 JPush 相关说明文档来获取这个标识. */
@property (nonatomic, copy) NSString *appKey;
/* channel 发布渠道. 可选 */
@property (nonatomic, copy) NSString *channel;
/* advertisingIdentifier 广告标识符（IDFA). 可选，IDFA能帮助您更准确的统计*/
@property (nonatomic, copy) NSString *advertisingId;
/* isProduction 是否生产环境. 如果为开发状态,设置为NO; 如果为生产状态,应改为 YES.默认为NO */
@property (nonatomic, assign) BOOL isProduction;

@end

@class CLLocation;

@interface JANALYTICSService : NSObject

/*!
 * @abstract 启动SDK
 *
 * @param config SDK启动相关模型,必填
 */
+ (void)setupWithConfig:(JANALYTICSLaunchConfig *)config;

/*!
 * @abstract 开始记录页面停留
 *
 * @param pageName 页面名称
 */
+ (void)startLogPageView:(NSString *)pageName;

/*!
 * @abstract 停止记录页面停留
 *
 * @param pageName 页面
 * @discussion 停止后，默认即时上报此页面。可通过[setFrequency:]方法更改为周期性上报策略
 */
+ (void)stopLogPageView:(NSString *)pageName;

/*!
 * @abstract 地理位置上报
 *
 * @param latitude 纬度.
 * @param longitude 经度.
 *
 */
+ (void)setLatitude:(double)latitude longitude:(double)longitude;

/*!
 * @abstract 地理位置上报
 *
 * @param location 直接传递 CLLocation * 型的地理信息
 *
 * @discussion 需要链接 CoreLocation.framework 并且 #import <CoreLocation/CoreLocation.h>
 */
+ (void)setLocation:(CLLocation *)location;

/*！事件统计
 * @param event 上报的事件模型
 * @discussion 默认即时上报事件。可通过[setFrequency:]方法更改为周期性上报策略
 */
+ (void)eventRecord:(JANALYTICSEventObject *)event;

/**
 设置用户信息

 @param userInfo 用户信息模型
 @param completion 错误码和错误信息callback
 */
+ (void)identifyAccount:(JANALYTICSUserInfo *)userInfo with:(void (^)(NSInteger err, NSString * msg))completion;

/**
 解绑当前的用户信息
 */
+ (void)detachAccount:(void (^)(NSInteger err, NSString * msg))completion;

/**
 设置周期上报频率
 默认为未设置频率，即时上报
 @param frequency 周期上报频率单位秒
 频率区间：0 或者 10 < frequency < 24*60*60
 可以设置为0，即表示取消周期上报频率，改为即时上报
 e.g. 十分钟上报一次 [JANALYTICSService setFrequency:600];
 */
+ (void)setFrequency:(NSUInteger)frequency;

/*!
 * @abstract 开启Crash日志收集
 *
 * @discussion 默认是关闭状态.
 */
+ (void)crashLogON;

/*!
 * @abstract 设置是否打印sdk产生的Debug级log信息, 默认为NO(不打印log)
 *
 * SDK 默认开启的日志级别为: Info. 只显示必要的信息, 不打印调试日志.
 *
 * 请在SDK启动后调用本接口，调用本接口可打开日志级别为: Debug, 打印调试日志.
 * 请在发布产品时改为NO，避免产生不必要的IO
 */
+ (void)setDebug:(BOOL)enable;

/*！处理url，判断是否开启圈选
 * @param url 直接透传给sdk处理的url
 */
+ (BOOL)handleUrl:(NSURL *)url;

@end

