//
//  MSDKInfoTool.h
//  MSDK
//
//  Created by Jason on 14/11/7.
//  Copyright (c) 2014年 Tencent. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface MSDKInfoTool : NSObject

+ (NSDictionary *)getDeviceInfo;
+ (NSString *)getCurrentDeviceModel;
+ (NSString *)getAPN;
+ (NSString *)idfaString;
+ (NSString*)stringWithUUID;

@end

