//
//  HttpDns.h
//  HttpDns
//
//  Created by Coolcao on 2016/11/8.
//  Copyright (c) 2016 Tencent. All rights reserved.
//
#ifndef __HttpDns_H__
#define __HttpDns_H__

#import <Foundation/Foundation.h>

@interface HttpDns : NSObject

+ (id) sharedInstance;

/**
 *  同步接口
 *  @param domain 域名
 *  @return 查询到的IP数组，超时（1s）或者未未查询到返回[0,0]数组
 */
- (NSArray*) HttpDnsGetHostByName:(NSString*) domain;

/**
 *  异步接口
 *  @param domain 域名
 *  @return 查询到的IP数组，超时（1s）或者未未查询到返回[0,0]数组
 */

- (void) HttpDnsGetHostByNameAsync:(NSString*) domain returnIps:(void (^)(NSArray* ipsArray))handler;
    
/**
 *  超时时间及开关设置
 *  @param timeout 单位: s
 *  @param enabled YES:打开 NO:关闭
 */
- (void) HttpDnsSetTimeOut:(float) timeout OpenLog:(BOOL) enabled;

@end
#endif
