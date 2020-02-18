#import <UIKit/UIKit.h>


 typedef NS_ENUM(NSUInteger, TinyLoadStatus) 
 {
     TinyLoadStatusLoading = 1,
     TinyLoadStatusError,
     TinyLoadStatusComplete
 };

@interface TinyApp : NSObject
@property  NSString* baseUrl;
@property  NSString* wasmName;
@property  TinyLoadStatus loadStatus;

+ (TinyApp*)instance;

- (void) load:(NSString*) baseUrl wasmName:(NSString*) wasmName completionHandler:(void (^)(NSError *error))completionHandler;
- (void) setWinHandle:(void*) handle;


@end


