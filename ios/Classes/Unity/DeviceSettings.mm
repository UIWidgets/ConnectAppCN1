#include <sys/types.h>
#include <sys/sysctl.h>

#include <AdSupport/ASIdentifierManager.h>

#include "DisplayManager.h"

// ad/vendor ids

static id QueryASIdentifierManager()
{
    NSBundle* bundle = [NSBundle bundleWithPath: @"/System/Library/Frameworks/AdSupport.framework"];
    if (bundle)
    {
        [bundle load];
        Class retClass = [bundle classNamed: @"ASIdentifierManager"];
        return [retClass performSelector: @selector(sharedManager)];
    }

    return nil;
}

extern "C" const char* UnityAdvertisingIdentifier()
{
    static const char* _ADID = NULL;
    static const NSString* _ADIDNSString = nil;

    // ad id can be reset during app lifetime
    id manager = QueryASIdentifierManager();
    if (manager)
    {
        NSString* adid = [[manager performSelector: @selector(advertisingIdentifier)] UUIDString];
        // Do stuff to avoid UTF8String leaks. We still leak if ADID changes, but that shouldn't happen too often.
        if (![_ADIDNSString isEqualToString: adid])
        {
            _ADIDNSString = adid;
            free((void*)_ADID);
            _ADID = AllocCString(adid);
        }
    }

    return _ADID;
}

extern "C" int UnityAdvertisingTrackingEnabled()
{
    bool _AdTrackingEnabled = false;

    // ad tracking can be changed during app lifetime
    id manager = QueryASIdentifierManager();
    if (manager)
        _AdTrackingEnabled = [manager performSelector: @selector(isAdvertisingTrackingEnabled)];

    return _AdTrackingEnabled ? 1 : 0;
}

extern "C" const char* UnityVendorIdentifier()
{
    static const char*  _VendorID           = NULL;

    if (_VendorID == NULL)
        _VendorID = AllocCString([[UIDevice currentDevice].identifierForVendor UUIDString]);

    return _VendorID;
}

// UIDevice properties

#define QUERY_UIDEVICE_PROPERTY(FUNC, PROP)                                         \
    extern "C" const char* FUNC()                                                   \
    {                                                                               \
        static const char* value = NULL;                                            \
        if (value == NULL && [UIDevice instancesRespondToSelector:@selector(PROP)]) \
            value = AllocCString([UIDevice currentDevice].PROP);                    \
        return value;                                                               \
    }

QUERY_UIDEVICE_PROPERTY(UnityDeviceName, name)
QUERY_UIDEVICE_PROPERTY(UnitySystemName, systemName)
QUERY_UIDEVICE_PROPERTY(UnitySystemVersion, systemVersion)

#undef QUERY_UIDEVICE_PROPERTY

// hw info

extern "C" const char* UnityDeviceModel()
{
    static const char* _DeviceModel = NULL;

    if (_DeviceModel == NULL)
    {
        size_t size;
        ::sysctlbyname("hw.machine", NULL, &size, NULL, 0);

        char* model = (char*)::malloc(size + 1);
        ::sysctlbyname("hw.machine", model, &size, NULL, 0);
        model[size] = 0;

#if TARGET_OS_SIMULATOR
        if (!strncmp(model, "i386", 4) || !strncmp(model, "x86_64", 6))
        {
            NSString* simModel = [[NSProcessInfo processInfo] environment][@"SIMULATOR_MODEL_IDENTIFIER"];
            if ([simModel length] > 0)
            {
                _DeviceModel = AllocCString(simModel);
                ::free(model);
                return _DeviceModel;
            }
        }
#endif

        _DeviceModel = AllocCString([NSString stringWithUTF8String: model]);
        ::free(model);
    }

    return _DeviceModel;
}

extern "C" int UnityDeviceCPUCount()
{
    static int _DeviceCPUCount = -1;

    if (_DeviceCPUCount <= 0)
    {
        // maybe would be better to use HW_AVAILCPU
        int     ctlName[]   = {CTL_HW, HW_NCPU};
        size_t  dataLen     = sizeof(_DeviceCPUCount);

        ::sysctl(ctlName, 2, &_DeviceCPUCount, &dataLen, NULL, 0);
    }
    return _DeviceCPUCount;
}

// misc
extern "C" const char* UnitySystemLanguage()
{
    static const char* _SystemLanguage = NULL;

    if (_SystemLanguage == NULL)
    {
        NSArray* lang = [[NSUserDefaults standardUserDefaults] objectForKey: @"AppleLanguages"];
        if (lang.count > 0)
            _SystemLanguage = AllocCString(lang[0]);
    }

    return _SystemLanguage;
}

extern "C" int ParseDeviceGeneration(const char* model)
{
#if PLATFORM_IOS
    if (!strcmp(model, "iPhone2,1"))
        return deviceiPhone3GS;
    else if (!strncmp(model, "iPhone3,", 8))
        return deviceiPhone4;
    else if (!strncmp(model, "iPhone4,", 8))
        return deviceiPhone4S;
    else if (!strncmp(model, "iPhone5,", 8))
    {
        int rev = atoi(model + 8);
        if (rev >= 3)
            return deviceiPhone5C;               // iPhone5,3
        else
            return deviceiPhone5;
    }
    else if (!strncmp(model, "iPhone6,", 8))
        return deviceiPhone5S;
    else if (!strncmp(model, "iPhone7,2", 9))
        return deviceiPhone6;
    else if (!strncmp(model, "iPhone7,1", 9))
        return deviceiPhone6Plus;
    else if (!strncmp(model, "iPhone8,1", 9))
        return deviceiPhone6S;
    else if (!strncmp(model, "iPhone8,2", 9))
        return deviceiPhone6SPlus;
    else if (!strncmp(model, "iPhone8,4", 9))
        return deviceiPhoneSE1Gen;
    else if (!strncmp(model, "iPhone9,1", 9) || !strncmp(model, "iPhone9,3", 9))
        return deviceiPhone7;
    else if (!strncmp(model, "iPhone9,2", 9) || !strncmp(model, "iPhone9,4", 9))
        return deviceiPhone7Plus;
    else if (!strncmp(model, "iPhone10,1", 10) || !strncmp(model, "iPhone10,4", 10))
        return deviceiPhone8;
    else if (!strncmp(model, "iPhone10,2", 10) || !strncmp(model, "iPhone10,5", 10))
        return deviceiPhone8Plus;
    else if (!strncmp(model, "iPhone10,3", 10) || !strncmp(model, "iPhone10,6", 10))
        return deviceiPhoneX;
    else if (!strncmp(model, "iPhone11,8", 10))
        return deviceiPhoneXR;
    else if (!strncmp(model, "iPhone11,2", 10))
        return deviceiPhoneXS;
    else if (!strncmp(model, "iPhone11,4", 10) || !strncmp(model, "iPhone11,6", 10))
        return deviceiPhoneXSMax;
    else if (!strcmp(model, "iPod4,1"))
        return deviceiPodTouch4Gen;
    else if (!strncmp(model, "iPod5,", 6))
        return deviceiPodTouch5Gen;
    else if (!strncmp(model, "iPod7,", 6))
        return deviceiPodTouch6Gen;
    else if (!strncmp(model, "iPad2,", 6))
    {
        int rev = atoi(model + 6);
        if (rev >= 5)
            return deviceiPadMini1Gen;                 // iPad2,5
        else
            return deviceiPad2Gen;
    }
    else if (!strncmp(model, "iPad3,", 6))
    {
        int rev = atoi(model + 6);
        if (rev >= 4)
            return deviceiPad4Gen;                 // iPad3,4
        else
            return deviceiPad3Gen;
    }
    else if (!strncmp(model, "iPad4,", 6))
    {
        int rev = atoi(model + 6);
        if (rev >= 7)
            return deviceiPadMini3Gen;
        else if (rev >= 4)
            return deviceiPadMini2Gen;     // iPad4,4
        else
            return deviceiPadAir1;
    }
    else if (!strncmp(model, "iPad5,", 6))
    {
        int rev = atoi(model + 6);
        if (rev == 1 || rev == 2)
            return deviceiPadMini4Gen;
        else if (rev >= 3)
            return deviceiPadAir2;
    }
    else if (!strncmp(model, "iPad6,", 6))
    {
        int rev = atoi(model + 6);
        if (rev == 7 || rev == 8)
            return deviceiPadPro1Gen;
        else if (rev == 3 || rev == 4)
            return deviceiPadPro10Inch1Gen;
        else if (rev == 11 || rev == 12)
            return deviceiPad5Gen;
    }
    else if (!strncmp(model, "iPad7,", 6))
    {
        int rev = atoi(model + 6);
        if (rev == 1 || rev == 2)
            return deviceiPadPro2Gen;
        else if (rev == 3 || rev == 4)
            return deviceiPadPro10Inch2Gen;
    }
    else if (!strncmp(model, "iPad8,", 6))
    {
        int rev = atoi(model + 6);
        if (rev >= 1 && rev <= 4)
            return deviceiPadPro11Inch;
        else if (rev >= 5)
            return deviceiPadPro3Gen;
    }
    // completely unknown hw - just determine form-factor
    else
    {
        if (!strncmp(model, "iPhone", 6))
            return deviceiPhoneUnknown;
        else if (!strncmp(model, "iPad", 4))
            return deviceiPadUnknown;
        else if (!strncmp(model, "iPod", 4))
            return deviceiPodTouchUnknown;
        else
            return deviceUnknown;
    }

#elif PLATFORM_TVOS
    if (!strncmp(model, "AppleTV5,", 9))
        return deviceAppleTV1Gen;
    else if (!strncmp(model, "AppleTV6,", 9))
        return deviceAppleTV2Gen;
    else
        return deviceUnknown;
#endif
}

extern "C" int UnityDeviceGeneration()
{
    static int _DeviceGeneration = deviceUnknown;

    if (_DeviceGeneration == deviceUnknown)
    {
        const char* model = UnityDeviceModel();
        _DeviceGeneration = ParseDeviceGeneration(model);
    }
    return _DeviceGeneration;
}

extern "C" int UnityDeviceSupportedOrientations()
{
    int device = UnityDeviceGeneration();
    int orientations = 0;

    orientations |= (1 << portrait);
    orientations |= (1 << landscapeLeft);
    orientations |= (1 << landscapeRight);

    if (device != deviceiPhoneX)
    {
        orientations |= (1 << portraitUpsideDown);
    }

    return orientations;
}

extern "C" int UnityDeviceIsStylusTouchSupported()
{
    int deviceGen = UnityDeviceGeneration();
    return (deviceGen == deviceiPadPro1Gen ||
        deviceGen == deviceiPadPro10Inch1Gen ||
        deviceGen == deviceiPadPro2Gen ||
        deviceGen == deviceiPadPro10Inch2Gen) ? 1 : 0;
}

extern "C" int UnityDeviceCanShowWideColor()
{
    UIScreen* mainScreen = [UIScreen mainScreen];
    UITraitCollection* traits = mainScreen.traitCollection;
    if (![traits respondsToSelector: @selector(displayGamut)])
        return false;
#if UNITY_HAS_IOSSDK_10_0 || UNITY_HAS_TVOSSDK_10_0
    return traits.displayGamut == UIDisplayGamutP3;
#else
    return false;
#endif
}

extern "C" float UnityDeviceDPI()
{
    static float _DeviceDPI = -1.0f;

    if (_DeviceDPI < 0.0f)
    {
        switch (UnityDeviceGeneration())
        {
            // iPhone
            case deviceiPhone3GS:
                _DeviceDPI = 163.0f; break;
            case deviceiPhone4:
            case deviceiPhone4S:
            case deviceiPhone5:
            case deviceiPhone5C:
            case deviceiPhone5S:
            case deviceiPhone6:
            case deviceiPhone6S:
            case deviceiPhoneSE1Gen:
            case deviceiPhone7:
            case deviceiPhone8:
            case deviceiPhoneXR:
                _DeviceDPI = 326.0f; break;
            case deviceiPhone6Plus:
            case deviceiPhone6SPlus:
            case deviceiPhone7Plus:
            case deviceiPhone8Plus:
                _DeviceDPI = 401.0f; break;
            case deviceiPhoneX:
            case deviceiPhoneXS:
            case deviceiPhoneXSMax:
                _DeviceDPI = 458.0f; break;
            // iPad
            case deviceiPad2Gen:
                _DeviceDPI = 132.0f; break;
            case deviceiPad3Gen:
            case deviceiPad4Gen:        // iPad retina
            case deviceiPadAir1:
            case deviceiPadAir2:
            case deviceiPadPro1Gen:
            case deviceiPadPro10Inch1Gen:
            case deviceiPadPro2Gen:
            case deviceiPadPro10Inch2Gen:
            case deviceiPad5Gen:
            case deviceiPadPro11Inch:
            case deviceiPadPro3Gen:
                _DeviceDPI = 264.0f; break;

            // iPad mini
            case deviceiPadMini1Gen:
                _DeviceDPI = 163.0f; break;
            case deviceiPadMini2Gen:
            case deviceiPadMini3Gen:
            case deviceiPadMini4Gen:
                _DeviceDPI = 326.0f; break;

            // iPod
            case deviceiPodTouch4Gen:
            case deviceiPodTouch5Gen:
            case deviceiPodTouch6Gen:
                _DeviceDPI = 326.0f; break;

            // unknown (new) devices
            case deviceiPhoneUnknown:
                _DeviceDPI = 326.0f; break;
            case deviceiPadUnknown:
                _DeviceDPI = 264.0f; break;
            case deviceiPodTouchUnknown:
                _DeviceDPI = 326.0f; break;
        }

        // If we didn't find DPI, set it to "unknown" value.
        if (_DeviceDPI < 0.0f)
            _DeviceDPI = 0.0f;
    }

    return _DeviceDPI;
}

// device id with fallback for pre-ios7

extern "C" const char* UnityDeviceUniqueIdentifier()
{
    static const char* _DeviceID = NULL;

    if (_DeviceID == NULL)
        _DeviceID = UnityVendorIdentifier();

    return _DeviceID;
}
