#include "UnityTrampolineCompatibility.h"
#include "UnityRendering.h"

#if UNITY_CAN_USE_METAL

#include "UnityMetalSupport.h"
#include <QuartzCore/QuartzCore.h>
#include <libkern/OSAtomic.h>

#if UNITY_TRAMPOLINE_IN_USE
#include "UnityAppController.h"
#include "CVTextureCache.h"
#endif

#include "ObjCRuntime.h"
extern "C" Class MTLTextureDescriptorClass;
extern Class MTLHeapDescriptorClass;


extern "C" void UnityAddNewMetalAPIImplIfNeeded(id<MTLDevice> device)
{
    // we were adding [MTLDevice supportsTextureSampleCount:] and MTLTextureDescriptor.usage
    // but after we switched to ios 9.0 as min target this is no longer needed

    if (class_getProperty(MTLHeapDescriptorClass, "storageMode") == 0)
    {
        IMP MTLHeapDescriptor_SetStorageMode_IMP = imp_implementationWithBlock(^void(id _self, MTLStorageMode mode) {});
        class_replaceMethod(MTLHeapDescriptorClass, @selector(setStorageMode:), MTLHeapDescriptor_SetStorageMode_IMP, MTLHeapDescriptor_setStorageMode_Enc);
    }

    if (class_getProperty(MTLHeapDescriptorClass, "size") == 0)
    {
        IMP MTLHeapDescriptor_SetSize_IMP = imp_implementationWithBlock(^void(id _self, NSUInteger size) {});
        class_replaceMethod(MTLHeapDescriptorClass, @selector(setSize:), MTLHeapDescriptor_SetSize_IMP, MTLHeapDescriptor_setSize_Enc);
    }
}

extern "C" void InitRenderingMTL()
{
#if UNITY_TRAMPOLINE_IN_USE
    extern bool _supportsMSAA;
    _supportsMSAA = true;
#endif
}

static MTLPixelFormat GetColorFormatForSurface(const UnityDisplaySurfaceMTL* surface)
{
    MTLPixelFormat colorFormat = surface->srgb ? MTLPixelFormatBGRA8Unorm_sRGB : MTLPixelFormatBGRA8Unorm;
#if (PLATFORM_IOS && UNITY_HAS_IOSSDK_10_0) || (PLATFORM_TVOS && UNITY_HAS_TVOSSDK_11_0)
    if (surface->wideColor && UnityIsWideColorSupported())
        colorFormat = surface->srgb ? MTLPixelFormatBGR10_XR_sRGB : MTLPixelFormatBGR10_XR;
#elif PLATFORM_OSX && __MAC_10_12
    if (surface->wideColor)
        colorFormat = MTLPixelFormatRGBA16Float;
#endif
    return colorFormat;
}

extern "C" void CreateSystemRenderingSurfaceMTL(UnityDisplaySurfaceMTL* surface)
{
    DestroySystemRenderingSurfaceMTL(surface);

    MTLPixelFormat colorFormat = GetColorFormatForSurface(surface);
    surface->layer.presentsWithTransaction = NO;
    surface->layer.drawsAsynchronously = YES;

#if PLATFORM_OSX
    MetalUpdateDisplaySync();
#endif


#if PLATFORM_OSX && __MAC_10_12
    CGColorSpaceRef colorSpaceRef = nil;
    if (surface->wideColor)
        colorSpaceRef = CGColorSpaceCreateWithName(surface->srgb ? kCGColorSpaceExtendedLinearSRGB : kCGColorSpaceExtendedSRGB);
    else
        colorSpaceRef = CGColorSpaceCreateWithName(kCGColorSpaceSRGB);

    surface->layer.colorspace = colorSpaceRef;
    CGColorSpaceRelease(colorSpaceRef);
#endif

    surface->layer.device = surface->device;
    surface->layer.pixelFormat = colorFormat;
    surface->layer.framebufferOnly = (surface->framebufferOnly != 0);
    surface->colorFormat = colorFormat;

    MTLTextureDescriptor* txDesc = [MTLTextureDescriptorClass texture2DDescriptorWithPixelFormat: colorFormat width: surface->systemW height: surface->systemH mipmapped: NO];
#if PLATFORM_OSX
    txDesc.resourceOptions = MTLResourceCPUCacheModeDefaultCache | MTLResourceStorageModeManaged;
#endif
    txDesc.usage = MTLTextureUsageRenderTarget | MTLTextureUsageShaderRead;

    @synchronized(surface->layer)
    {
        OSAtomicCompareAndSwap32Barrier(surface->bufferChanged, 0, &surface->bufferChanged);

        for (int i = 0; i < kUnityNumOffscreenSurfaces; i++)
        {
            OSAtomicCompareAndSwap32Barrier(surface->bufferCompleted[i], -1, &surface->bufferCompleted[i]);
            // Allocating a proxy texture is cheap until it's being rendered to and the GPU driver does allocation
            surface->drawableProxyRT[i] = [surface->device newTextureWithDescriptor: txDesc];
        }

#if PLATFORM_OSX
        OSAtomicCompareAndSwap32Barrier(surface->writeCount, surface->writeCount + (kUnityNumOffscreenSurfaces - 1), &surface->writeCount);
        OSAtomicCompareAndSwap32Barrier(surface->readCount, surface->writeCount - 1, &surface->readCount);
#endif
    }
}

extern "C" void CreateRenderingSurfaceMTL(UnityDisplaySurfaceMTL* surface)
{
    DestroyRenderingSurfaceMTL(surface);

    MTLPixelFormat colorFormat = GetColorFormatForSurface(surface);

    const int w = surface->targetW, h = surface->targetH;

    if (w != surface->systemW || h != surface->systemH || surface->useCVTextureCache)
    {
#if PLATFORM_IOS || PLATFORM_TVOS
        if (surface->useCVTextureCache)
            surface->cvTextureCache = CreateCVTextureCache();

        if (surface->cvTextureCache)
        {
            surface->cvTextureCacheTexture = CreateReadableRTFromCVTextureCache(surface->cvTextureCache, surface->targetW, surface->targetH, &surface->cvPixelBuffer);
            surface->targetColorRT = GetMetalTextureFromCVTextureCache(surface->cvTextureCacheTexture);
        }
        else
#endif
        {
            MTLTextureDescriptor* txDesc = [MTLTextureDescriptorClass new];
            txDesc.textureType = MTLTextureType2D;
            txDesc.width = w;
            txDesc.height = h;
            txDesc.depth = 1;
            txDesc.pixelFormat = colorFormat;
            txDesc.arrayLength = 1;
            txDesc.mipmapLevelCount = 1;
#if PLATFORM_OSX
            txDesc.resourceOptions = MTLResourceCPUCacheModeDefaultCache | MTLResourceStorageModeManaged;
#endif
            txDesc.usage = MTLTextureUsageRenderTarget | MTLTextureUsageShaderRead;
            surface->targetColorRT = [surface->device newTextureWithDescriptor: txDesc];
        }
        surface->targetColorRT.label = @"targetColorRT";
    }

    if (surface->msaaSamples > 1)
    {
        MTLTextureDescriptor* txDesc = [MTLTextureDescriptorClass new];
        txDesc.textureType = MTLTextureType2DMultisample;
        txDesc.width = w;
        txDesc.height = h;
        txDesc.depth = 1;
        txDesc.pixelFormat = colorFormat;
        txDesc.arrayLength = 1;
        txDesc.mipmapLevelCount = 1;
        txDesc.sampleCount = surface->msaaSamples;
#if PLATFORM_OSX
        txDesc.resourceOptions = MTLResourceCPUCacheModeDefaultCache | MTLResourceStorageModePrivate;
#endif
        txDesc.usage = MTLTextureUsageRenderTarget | MTLTextureUsageShaderRead;
        if (![surface->device supportsTextureSampleCount: txDesc.sampleCount])
            txDesc.sampleCount = 4;
        surface->targetAAColorRT = [surface->device newTextureWithDescriptor: txDesc];
        surface->targetAAColorRT.label = @"targetAAColorRT";

        if (!surface->targetColorRT)
        {
            MTLTextureDescriptor* txDescResolve = [txDesc copyWithZone: nil];
            txDescResolve.textureType = MTLTextureType2D;
            txDescResolve.sampleCount = 1;
            surface->targetColorRT = [surface->device newTextureWithDescriptor: txDescResolve];
        }
    }
}

extern "C" void DestroyRenderingSurfaceMTL(UnityDisplaySurfaceMTL* surface)
{
    surface->targetColorRT = nil;
    surface->targetAAColorRT = nil;

    if (surface->cvTextureCacheTexture)
        CFRelease(surface->cvTextureCacheTexture);
    if (surface->cvPixelBuffer)
        CFRelease(surface->cvPixelBuffer);
    if (surface->cvTextureCache)
        CFRelease(surface->cvTextureCache);
    surface->cvTextureCache = 0;
}

extern "C" void CreateSharedDepthbufferMTL(UnityDisplaySurfaceMTL* surface)
{
    DestroySharedDepthbufferMTL(surface);
    if (surface->disableDepthAndStencil)
        return;

#if PLATFORM_OSX || PLATFORM_TVOS
    MTLPixelFormat pixelFormat = MTLPixelFormatDepth32Float_Stencil8;
#else
    MTLPixelFormat pixelFormat = MTLPixelFormatDepth32Float;
#endif

    MTLTextureDescriptor* depthTexDesc = [MTLTextureDescriptorClass texture2DDescriptorWithPixelFormat: pixelFormat width: surface->targetW height: surface->targetH mipmapped: NO];
#if PLATFORM_OSX
    depthTexDesc.resourceOptions = MTLResourceCPUCacheModeDefaultCache | MTLResourceStorageModePrivate;
#endif

#if (PLATFORM_IOS && UNITY_HAS_IOSSDK_10_0) || (PLATFORM_TVOS && UNITY_HAS_TVOSSDK_10_0)
    if (surface->memorylessDepth)
        depthTexDesc.storageMode = MTLStorageModeMemoryless;
#endif

    depthTexDesc.usage = MTLTextureUsageRenderTarget | MTLTextureUsageShaderRead;
    if (surface->msaaSamples > 1)
    {
        depthTexDesc.textureType = MTLTextureType2DMultisample;
        depthTexDesc.sampleCount = surface->msaaSamples;
        if (![surface->device supportsTextureSampleCount: depthTexDesc.sampleCount])
            depthTexDesc.sampleCount = 4;
    }
    surface->depthRB = [surface->device newTextureWithDescriptor: depthTexDesc];

#if PLATFORM_OSX || PLATFORM_TVOS
    surface->stencilRB = surface->depthRB;
#else
    MTLTextureDescriptor* stencilTexDesc = [MTLTextureDescriptorClass texture2DDescriptorWithPixelFormat: MTLPixelFormatStencil8 width: surface->targetW height: surface->targetH mipmapped: NO];
    stencilTexDesc.usage = MTLTextureUsageRenderTarget | MTLTextureUsageShaderRead;
    if (surface->msaaSamples > 1)
    {
        stencilTexDesc.textureType = MTLTextureType2DMultisample;
        stencilTexDesc.sampleCount = surface->msaaSamples;
        if (![surface->device supportsTextureSampleCount: stencilTexDesc.sampleCount])
            stencilTexDesc.sampleCount = 4;
    }
    surface->stencilRB = [surface->device newTextureWithDescriptor: stencilTexDesc];
#endif
}

extern "C" void DestroySharedDepthbufferMTL(UnityDisplaySurfaceMTL* surface)
{
    surface->depthRB = nil;
    surface->stencilRB = nil;
}

extern "C" void CreateUnityRenderBuffersMTL(UnityDisplaySurfaceMTL* surface)
{
    UnityRenderBufferDesc sys_desc = { surface->systemW, surface->systemH, 1, 1, 1 };
    UnityRenderBufferDesc tgt_desc = { surface->targetW, surface->targetH, 1, (unsigned int)surface->msaaSamples, 1 };

    surface->systemColorRB  = surface->drawableProxyRT[surface->writeCount % kUnityNumOffscreenSurfaces];
    if (surface->targetAAColorRT)
        surface->unityColorBuffer   = UnityCreateExternalColorSurfaceMTL(surface->unityColorBuffer, surface->targetAAColorRT, surface->targetColorRT, &tgt_desc, nil);
    else if (surface->targetColorRT)
        surface->unityColorBuffer   = UnityCreateExternalColorSurfaceMTL(surface->unityColorBuffer, surface->targetColorRT, nil, &tgt_desc, nil);
    else
        surface->unityColorBuffer   = UnityCreateExternalColorSurfaceMTL(surface->unityColorBuffer, surface->systemColorRB, nil, &tgt_desc, surface);

    if (surface->depthRB)
        surface->unityDepthBuffer   = UnityCreateExternalDepthSurfaceMTL(surface->unityDepthBuffer, surface->depthRB, surface->stencilRB, &tgt_desc);
    else
        surface->unityDepthBuffer   = UnityCreateDummySurface(surface->unityDepthBuffer, false, &tgt_desc);

    surface->systemColorBuffer = UnityCreateExternalColorSurfaceMTL(surface->systemColorBuffer, surface->systemColorRB, nil, &sys_desc, surface);
    surface->systemDepthBuffer = UnityCreateDummySurface(surface->systemDepthBuffer, false, &sys_desc);
}

extern "C" void DestroySystemRenderingSurfaceMTL(UnityDisplaySurfaceMTL* surface)
{
    // before we needed to nil surface->systemColorRB (to release drawable we get from the view)
    // but after we switched to proxy rt this is no longer needed
    // even more it is harmful when running rendering on another thread (as is default now)
    // as on render thread we do StartFrameRenderingMTL/AcquireDrawableMTL/EndFrameRenderingMTL
    // and DestroySystemRenderingSurfaceMTL comes on main thread so we might end up with race condition for no reason
}

extern "C" void DestroyUnityRenderBuffersMTL(UnityDisplaySurfaceMTL* surface)
{
    UnityDestroyExternalSurface(surface->unityColorBuffer);
    UnityDestroyExternalSurface(surface->systemColorBuffer);
    surface->unityColorBuffer = surface->systemColorBuffer = 0;

    UnityDestroyExternalSurface(surface->unityDepthBuffer);
    UnityDestroyExternalSurface(surface->systemDepthBuffer);
    surface->unityDepthBuffer = surface->systemDepthBuffer = 0;
}

extern "C" void PreparePresentMTL(UnityDisplaySurfaceMTL* surface)
{
    if (surface->targetColorRT)
        UnityBlitToBackbuffer(surface->unityColorBuffer, surface->systemColorBuffer, surface->systemDepthBuffer);
#if UNITY_TRAMPOLINE_IN_USE
    APP_CONTROLLER_RENDER_PLUGIN_METHOD(onFrameResolved);
#endif
}

extern "C" void PresentMTL(UnityDisplaySurfaceMTL* surface)
{
    if (surface->drawable)
        [UnityCurrentMTLCommandBuffer() presentDrawable: surface->drawable];
}

extern "C" MTLTextureRef AcquireDrawableMTL(UnityDisplaySurfaceMTL* surface)
{
    if (!surface)
        return nil;

    if (!surface->drawable)
        surface->drawable = [surface->layer nextDrawable];

    // on A7 SoC nextDrawable may be nil before locking the screen
    if (!surface->drawable)
        return nil;

    surface->systemColorRB = [surface->drawable texture];
    return surface->systemColorRB;
}

extern "C" void StartFrameRenderingMTL(UnityDisplaySurfaceMTL* surface)
{
    // we will acquire drawable lazily in AcquireDrawableMTL
    surface->drawable = nil;

#if PLATFORM_OSX
    // For non-Mac platforms, writeCount remains static
    bool bufferChanged = (bool)surface->bufferChanged;
    OSAtomicCompareAndSwap32Barrier(surface->bufferChanged, 0, &surface->bufferChanged);

    if (bufferChanged && surface->writeCount <= surface->readCount)
        OSAtomicAdd32Barrier(1, &surface->writeCount);
    OSAtomicCompareAndSwap32Barrier(surface->bufferCompleted[surface->writeCount % kUnityNumOffscreenSurfaces], 0, &surface->bufferCompleted[surface->writeCount % kUnityNumOffscreenSurfaces]);
#endif
    surface->systemColorRB  = surface->drawableProxyRT[surface->writeCount % kUnityNumOffscreenSurfaces];

    UnityRenderBufferDesc sys_desc = { surface->systemW, surface->systemH, 1, 1, 1};
    UnityRenderBufferDesc tgt_desc = { surface->targetW, surface->targetH, 1, (unsigned int)surface->msaaSamples, 1};

    surface->systemColorBuffer = UnityCreateExternalColorSurfaceMTL(surface->systemColorBuffer, surface->systemColorRB, nil, &sys_desc, surface);
    if (surface->targetColorRT == nil)
    {
        if (surface->targetAAColorRT)
            surface->unityColorBuffer = UnityCreateExternalColorSurfaceMTL(surface->unityColorBuffer, surface->targetAAColorRT, surface->systemColorRB, &tgt_desc, surface);
        else
            surface->unityColorBuffer = UnityCreateExternalColorSurfaceMTL(surface->unityColorBuffer, surface->systemColorRB, nil, &tgt_desc, surface);
    }
}

extern "C" void EndFrameRenderingMTL(UnityDisplaySurfaceMTL* surface)
{
    if (surface->presentCB)
    {
        // currently we expect EndFrameRenderingMTL to be called AFTER unity is done with "main" CB
        // alas internally main CB is enqueued right before commit (like is done there),
        // so we need to make sure it was committed (and niled) to make sure we present to external screens AFTER drawing is done
        assert(UnityCurrentMTLCommandBuffer() == nil);
        [surface->presentCB enqueue]; [surface->presentCB commit];
        surface->presentCB = nil;
    }

    surface->systemColorRB  = nil;
    surface->drawable       = nil;
}

extern "C" void PreparePresentNonMainScreenMTL(UnityDisplaySurfaceMTL* surface)
{
    if (surface->drawable)
    {
        surface->presentCB = [surface->drawableCommandQueue commandBuffer];
        [surface->presentCB presentDrawable: surface->drawable];
    }
}

#else

extern "C" void InitRenderingMTL()                                          {}

extern "C" void CreateSystemRenderingSurfaceMTL(UnityDisplaySurfaceMTL*)    {}
extern "C" void CreateRenderingSurfaceMTL(UnityDisplaySurfaceMTL*)          {}
extern "C" void DestroyRenderingSurfaceMTL(UnityDisplaySurfaceMTL*)         {}
extern "C" void CreateSharedDepthbufferMTL(UnityDisplaySurfaceMTL*)         {}
extern "C" void DestroySharedDepthbufferMTL(UnityDisplaySurfaceMTL*)        {}
extern "C" void CreateUnityRenderBuffersMTL(UnityDisplaySurfaceMTL*)        {}
extern "C" void DestroySystemRenderingSurfaceMTL(UnityDisplaySurfaceMTL*)   {}
extern "C" void DestroyUnityRenderBuffersMTL(UnityDisplaySurfaceMTL*)       {}
extern "C" void StartFrameRenderingMTL(UnityDisplaySurfaceMTL*)             {}
extern "C" void EndFrameRenderingMTL(UnityDisplaySurfaceMTL*)               {}
extern "C" void PreparePresentMTL(UnityDisplaySurfaceMTL*)                  {}
extern "C" void PresentMTL(UnityDisplaySurfaceMTL*)                         {}

#endif
