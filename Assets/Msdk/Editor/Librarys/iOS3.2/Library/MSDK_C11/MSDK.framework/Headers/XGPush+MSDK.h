//
//  MXGPush+MSDK.h
//  XG-SDK
//
//  Created by xiangchen on 13-10-18.
//  Copyright (c) 2013年 mta. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

#import "XGPush.h"

@interface MXGPush(MSDK)

#pragma mark - 新版注册接口，推荐优先使用

/**
 注册信鸽

 @param appId MSDK的AppId
 */
+(void)startAppForMSDK:(uint32_t)appId;


/**
 注册信鸽，并指定服务器地址

 @param appId MSDK的AppId
 @param domainOrIp 服务器地址，若传入nil，则使用信鸽公共服务器。
 */
+(void)startAppForMSDK:(uint32_t)appId domainOrIp:(NSString *)domainOrIp;

#pragma mark - 老版注册接口，仅做版本兼容
/**
 注册信鸽

 @param appId MSDK的AppId
 @param appKey MSDK的AppKey，代码已经不再使用这个参数，可以传nil
 */
+(void)startAppForMSDK:(uint32_t)appId appKey:(NSString *)appKey;


/**
 注册信鸽，并指定服务器地址

 @param appId MSDK的AppId
 @param appKey MSDK的AppKey，代码已经不再使用这个参数，可以传nil
 @param domainOrIp 服务器地址，若传入nil，则使用信鸽公共服务器。
 */
+(void)startAppForMSDK:(uint32_t)appId appKey:(NSString *)appKey domainOrIp:(NSString *)domainOrIp;

@end
