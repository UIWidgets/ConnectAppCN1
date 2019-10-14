//
//  NSString+Cookie.h
//  NSString+Cookie
//
//  Created by Luke on 3/5/14.
//  Copyright (c) 2014 taobao. All rights reserved.
//

#import <Foundation/Foundation.h>

/**
 将单条Cookie字符串转成NSHTTPCookie对象
 Cookie的规范可以见相应的RFC文档 http://tools.ietf.org/html/rfc6265
 */
@interface NSString(Cookie)

/*!
 当前字符串为单条cookie
 */
- (NSHTTPCookie *)cookie;

@end
