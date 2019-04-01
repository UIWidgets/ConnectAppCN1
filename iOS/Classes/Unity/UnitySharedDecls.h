#pragma once

// The contents of this file are used when building both Unity library and the trampoline. Do not change it.

// Classes/Unity/UnityForwardDecls
typedef enum ScreenOrientation
{
    orientationUnknown,

    portrait,
    portraitUpsideDown,
    landscapeLeft,
    landscapeRight,

    orientationCount,
}
ScreenOrientation;

// Classes/UI/SplashScreen.mm
#ifdef __cplusplus
struct OrientationMask
{
    bool portrait;
    bool portraitUpsideDown;
    bool landscapeLeft;
    bool landscapeRight;
};
#endif
