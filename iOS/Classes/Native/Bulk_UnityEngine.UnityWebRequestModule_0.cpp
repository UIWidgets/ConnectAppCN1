#include "il2cpp-config.h"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include <cstring>
#include <string.h>
#include <stdio.h>
#include <cmath>
#include <limits>
#include <assert.h>
#include <stdint.h>

#include "il2cpp-class-internals.h"
#include "codegen/il2cpp-codegen.h"
#include "il2cpp-object-internals.h"

template <typename R, typename T1>
struct VirtFuncInvoker1
{
	typedef R (*Func)(void*, T1, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj, p1, invokeData.method);
	}
};
template <typename R>
struct VirtFuncInvoker0
{
	typedef R (*Func)(void*, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj, invokeData.method);
	}
};
template <typename R, typename T1, typename T2, typename T3>
struct VirtFuncInvoker3
{
	typedef R (*Func)(void*, T1, T2, T3, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1, T2 p2, T3 p3)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj, p1, p2, p3, invokeData.method);
	}
};
template <typename T1>
struct VirtActionInvoker1
{
	typedef void (*Action)(void*, T1, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		((Action)invokeData.methodPtr)(obj, p1, invokeData.method);
	}
};
struct InterfaceActionInvoker0
{
	typedef void (*Action)(void*, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeClass* declaringInterface, RuntimeObject* obj)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_interface_invoke_data(slot, obj, declaringInterface);
		((Action)invokeData.methodPtr)(obj, invokeData.method);
	}
};

// System.Action`1<UnityEngine.AsyncOperation>
struct Action_1_tCBF754C290FAE894631BED8FD56E9E22C4C187F9;
// System.ArgumentException
struct ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1;
// System.AsyncCallback
struct AsyncCallback_t3F3DA3BEDAEE81DD1D24125DF8EB30E85EE14DA4;
// System.Byte[]
struct ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821;
// System.Char[]
struct CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2;
// System.Collections.Generic.LinkedList`1<System.Text.RegularExpressions.CachedCodeEntry>
struct LinkedList_1_t44CA4EB2162DC04F96F29C8A68A05D05166137F7;
// System.Collections.Hashtable
struct Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9;
// System.Collections.IDictionary
struct IDictionary_t1BD5C1546718A374EA8122FBD6C6EE45331E8CE7;
// System.DelegateData
struct DelegateData_t1BF9F691B56DAE5F8C28C5E084FDE94F15F27BBE;
// System.Delegate[]
struct DelegateU5BU5D_tDFCDEE2A6322F96C0FE49AF47E9ADB8C4B294E86;
// System.Diagnostics.StackTrace[]
struct StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196;
// System.Globalization.CodePageDataItem
struct CodePageDataItem_t6E34BEE9CCCBB35C88D714664633AF6E5F5671FB;
// System.IAsyncResult
struct IAsyncResult_t8E194308510B375B42432981AE5E7488C458D598;
// System.IO.MemoryStream
struct MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C;
// System.IO.Stream/ReadWriteTask
struct ReadWriteTask_tFA17EEE8BC5C4C83EAEFCC3662A30DE351ABAA80;
// System.IntPtr[]
struct IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD;
// System.InvalidOperationException
struct InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1;
// System.Reflection.MethodInfo
struct MethodInfo_t;
// System.Runtime.Serialization.SafeSerializationManager
struct SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770;
// System.String
struct String_t;
// System.String[]
struct StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E;
// System.Text.DecoderFallback
struct DecoderFallback_t128445EB7676870485230893338EF044F6B72F60;
// System.Text.EncoderFallback
struct EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63;
// System.Text.Encoding
struct Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4;
// System.Text.RegularExpressions.ExclusiveReference
struct ExclusiveReference_t39E202CDB13A1E6EBA4CE0C7548B192CEB5C64DB;
// System.Text.RegularExpressions.Regex
struct Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF;
// System.Text.RegularExpressions.RegexCode
struct RegexCode_t12846533CAD1E4221CEDF5A4D15D4D649EA688FA;
// System.Text.RegularExpressions.RegexRunnerFactory
struct RegexRunnerFactory_t0703F390E2102623B0189DEC095DB182698E404B;
// System.Text.RegularExpressions.SharedReference
struct SharedReference_t225BA5C249F9F1D6C959F151695BDF65EF2C92A5;
// System.Text.StringBuilder
struct StringBuilder_t;
// System.Threading.SemaphoreSlim
struct SemaphoreSlim_t2E2888D1C0C8FAB80823C76F1602E4434B8FA048;
// System.Threading.Tasks.Task`1<System.Int32>
struct Task_1_t640F0CBB720BB9CD14B90B7B81624471A9F56D87;
// System.Uri
struct Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E;
// System.Uri/UriInfo
struct UriInfo_t9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E;
// System.UriParser
struct UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC;
// System.Void
struct Void_t22962CB4C05B1D89B55A6E1139F0E87A90987017;
// UnityEngine.AsyncOperation
struct AsyncOperation_t304C51ABED8AE734CC8DDDFE13013D8D5A44641D;
// UnityEngine.Networking.CertificateHandler
struct CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0;
// UnityEngine.Networking.DownloadHandler
struct DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9;
// UnityEngine.Networking.DownloadHandlerBuffer
struct DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255;
// UnityEngine.Networking.UnityWebRequest
struct UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129;
// UnityEngine.Networking.UnityWebRequestAsyncOperation
struct UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353;
// UnityEngine.Networking.UploadHandler
struct UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4;
// UnityEngine.Networking.UploadHandlerRaw
struct UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27;

extern RuntimeClass* ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1_il2cpp_TypeInfo_var;
extern RuntimeClass* ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821_il2cpp_TypeInfo_var;
extern RuntimeClass* CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2_il2cpp_TypeInfo_var;
extern RuntimeClass* Char_tBF22D9FC341BE970735250BB6FF1A4A92BBA58B9_il2cpp_TypeInfo_var;
extern RuntimeClass* Debug_t7B5FCB117E2FD63B6838BC52821B252E2BFB61C4_il2cpp_TypeInfo_var;
extern RuntimeClass* DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_il2cpp_TypeInfo_var;
extern RuntimeClass* FormatException_t2808E076CDE4650AF89F55FD78F49290D0EC5BDC_il2cpp_TypeInfo_var;
extern RuntimeClass* GC_tC1D7BD74E8F44ECCEF5CD2B5D84BFF9AAE02D01D_il2cpp_TypeInfo_var;
extern RuntimeClass* IDisposable_t7218B22548186B208D65EA5B7870503810A2D15A_il2cpp_TypeInfo_var;
extern RuntimeClass* Int64_t7A386C2FF7B0280A0F516992401DDFCF0FF7B436_il2cpp_TypeInfo_var;
extern RuntimeClass* IntPtr_t_il2cpp_TypeInfo_var;
extern RuntimeClass* InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var;
extern RuntimeClass* MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C_il2cpp_TypeInfo_var;
extern RuntimeClass* Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF_il2cpp_TypeInfo_var;
extern RuntimeClass* StringBuilder_t_il2cpp_TypeInfo_var;
extern RuntimeClass* UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_il2cpp_TypeInfo_var;
extern RuntimeClass* Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var;
extern RuntimeClass* WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_il2cpp_TypeInfo_var;
extern RuntimeClass* WebRequestUtils_tBE8F8607E3A9633419968F6AF2F706A029AE1296_il2cpp_TypeInfo_var;
extern String_t* _stringLiteral091B0CE42EB0BD96169EA00B16DD938F6D63AC95;
extern String_t* _stringLiteral1C5E5F29CEB079B561835055FFA20C2E0B53F397;
extern String_t* _stringLiteral38263C0B87E5FC0881F12EF855C8F694115D8213;
extern String_t* _stringLiteral4345CB1FA27885A8FBFE7C0C830A592CC76A552B;
extern String_t* _stringLiteral4DD76F7BD318A8B909BC0FF86CA3BE3625DA0374;
extern String_t* _stringLiteral4E5057793E1875AA08F21BE7F738453AD461E5F0;
extern String_t* _stringLiteral56F03F5F25FB2048BF4AB5FBBF7B5E3D39A3ECEB;
extern String_t* _stringLiteral61FF81C30AA3C76E78AFEA62B2E3BD1DFA49E854;
extern String_t* _stringLiteral70EE7E18113E0328AAE2B1D5D212C2735F1C00F8;
extern String_t* _stringLiteral7138A51661947B19B5088DA5A2BFEDE2876F49B9;
extern String_t* _stringLiteral81D42CE01525C0213D5284260BDB58819D046FB9;
extern String_t* _stringLiteral88DADF72F0A8F76B45A836CE12A3DC82857776DB;
extern String_t* _stringLiteral947518D877FB275850A375D795BE6A44C27AB526;
extern String_t* _stringLiteral986F2ED15C79ED805000ECCD85519810B2DB2A93;
extern String_t* _stringLiteralA288E90C6C4E12B4E76A10851EF1ABD903F1EAE7;
extern String_t* _stringLiteralA58AA001D4152D20F7F8E0809B9CD782BE38A82C;
extern String_t* _stringLiteralA91E4897CA9F429677AFC57ED00D90DE8D3C7001;
extern String_t* _stringLiteralAEA05C1AAB9D42F987C023592D1AF2F1D8403D2F;
extern String_t* _stringLiteralBEDBFCA635D617975AC8C4A6D1FBC9714BC86399;
extern String_t* _stringLiteralCA854C2B1F1CACD772359086ADF1F752E8698E5A;
extern String_t* _stringLiteralCE27CB141098FEB00714E758646BE3E99C185B71;
extern String_t* _stringLiteralDA39A3EE5E6B4B0D3255BFEF95601890AFD80709;
extern String_t* _stringLiteralDCB16D9AACB079FE42FBDE349C3319DE8033DDD1;
extern String_t* _stringLiteralEF81042E1E86ACB765718EA37393A1292452BBCC;
extern String_t* _stringLiteralF030BBBD32966CDE41037B98A8849C46B76E4BC1;
extern String_t* _stringLiteralF37BF1E2A7C84A010A6E65E2E41A03F1C044F04B;
extern String_t* _stringLiteralF6C97A7F64063CFEE7C2DC2157847204D4DBF093;
extern String_t* _stringLiteralF80B07414273FEB6D1B5EAB1E91186C7CE65DE24;
extern String_t* _stringLiteralF92E777F4341930BAD9B2422283C4680D00DBC06;
extern String_t* _stringLiteralFE5567E8D769550852182CDF69D74BB16DFF8E29;
extern const RuntimeMethod* UnityWebRequest_InternalSetCustomMethod_mE9F0C84C6DCD5412AEDD76280EEC4FB82516EF16_RuntimeMethod_var;
extern const RuntimeMethod* UnityWebRequest_InternalSetMethod_m636508AA8E6EF12B3255D8ED108BFF7EB1AB68C9_RuntimeMethod_var;
extern const RuntimeMethod* UnityWebRequest_InternalSetUrl_m2E2C837A6F32065CAAAF6EFA7D0237C9E206689A_RuntimeMethod_var;
extern const RuntimeMethod* UnityWebRequest_SetRequestHeader_m1B54D38BDACC2789FC8EE889EA72EDD7844D2309_RuntimeMethod_var;
extern const RuntimeMethod* UnityWebRequest_set_downloadHandler_mB481C2EE30F84B62396C1A837F46046F12B8FA7E_RuntimeMethod_var;
extern const RuntimeMethod* UnityWebRequest_set_method_mF2DAC86EB05D65B9BCB52056B7CBB2C1AD87EEC6_RuntimeMethod_var;
extern const RuntimeMethod* UnityWebRequest_set_uploadHandler_m7B33656584914FB3F6FB0FF73C08F671262724A1_RuntimeMethod_var;
extern const RuntimeMethod* UnityWebRequest_set_uri_mDE5DEDA2698190CD80AE3B4B7342400A32284124_RuntimeMethod_var;
extern const RuntimeMethod* UploadHandlerRaw__ctor_m9F7643CA3314C8CE46DD41FBF584C268E2546935_RuntimeMethod_var;
extern const RuntimeMethod* WebRequestUtils_MakeInitialUrl_m446CCE4EFB276BE27A9380D55B9E704D01107B83_RuntimeMethod_var;
extern const uint32_t CertificateHandler_Dispose_m9C71BAA51760FDF05AB999B6AB6E6BC71BCB8CA0_MetadataUsageId;
extern const uint32_t DownloadHandler_Dispose_m7478E72B2DBA4B55FAA25F7A1975A13BA5891D4B_MetadataUsageId;
extern const uint32_t DownloadHandler_GetTextEncoder_m601540FD9D16122709582833632A9DEEDBF07E64_MetadataUsageId;
extern const uint32_t DownloadHandler_GetText_mA51553E65D6A397E07AAAC21214C817AD72550FD_MetadataUsageId;
extern const uint32_t UnityWebRequest_Dispose_m6AFA87DA329282058723E5ACE016B0B08CFE806D_MetadataUsageId;
extern const uint32_t UnityWebRequest_Get_m9BA0FB62F38F9704A5E1E8155E0A76B8F9C352DA_MetadataUsageId;
extern const uint32_t UnityWebRequest_Get_mF4E12AA47AAF25221AD738B434B0EA8D40659B18_MetadataUsageId;
extern const uint32_t UnityWebRequest_InternalDestroy_mF5D7484808AEAE24A43B678614D257FBF885026B_MetadataUsageId;
extern const uint32_t UnityWebRequest_InternalSetCustomMethod_mE9F0C84C6DCD5412AEDD76280EEC4FB82516EF16_MetadataUsageId;
extern const uint32_t UnityWebRequest_InternalSetMethod_m636508AA8E6EF12B3255D8ED108BFF7EB1AB68C9_MetadataUsageId;
extern const uint32_t UnityWebRequest_InternalSetUrl_m2E2C837A6F32065CAAAF6EFA7D0237C9E206689A_MetadataUsageId;
extern const uint32_t UnityWebRequest_SetRequestHeader_m1B54D38BDACC2789FC8EE889EA72EDD7844D2309_MetadataUsageId;
extern const uint32_t UnityWebRequest_get_error_mC79FE2460B3F30B8F9E5385BD7D2B4C5B295D7CC_MetadataUsageId;
extern const uint32_t UnityWebRequest_set_downloadHandler_mB481C2EE30F84B62396C1A837F46046F12B8FA7E_MetadataUsageId;
extern const uint32_t UnityWebRequest_set_method_mF2DAC86EB05D65B9BCB52056B7CBB2C1AD87EEC6_MetadataUsageId;
extern const uint32_t UnityWebRequest_set_uploadHandler_m7B33656584914FB3F6FB0FF73C08F671262724A1_MetadataUsageId;
extern const uint32_t UnityWebRequest_set_uri_mDE5DEDA2698190CD80AE3B4B7342400A32284124_MetadataUsageId;
extern const uint32_t UnityWebRequest_set_url_mA698FD94C447FF7C1C429D50C2EBAEEDD473007D_MetadataUsageId;
extern const uint32_t UploadHandlerRaw__ctor_m9F7643CA3314C8CE46DD41FBF584C268E2546935_MetadataUsageId;
extern const uint32_t UploadHandler_Dispose_m9BBE8D7D2BBAAC2DE84B52BADA0B79CEA6F2DAB2_MetadataUsageId;
extern const uint32_t WWWTranscoder_Decode_m2533830DAAAE6F33AA6EE85A5BF63C96F5D631D4_MetadataUsageId;
extern const uint32_t WWWTranscoder_URLDecode_m591A567154B1B8737ECBFE065AF4FCA59217F5D8_MetadataUsageId;
extern const uint32_t WWWTranscoder__cctor_m3436CCA2D8667A6BCF6981B6573EF048BDA49F51_MetadataUsageId;
extern const uint32_t WebRequestUtils_MakeInitialUrl_m446CCE4EFB276BE27A9380D55B9E704D01107B83_MetadataUsageId;
extern const uint32_t WebRequestUtils_MakeUriString_m5693EA04230335B9611278EFC189BD58339D01E4_MetadataUsageId;
extern const uint32_t WebRequestUtils_RedirectTo_m8AC7C0BFC562550118F6FF4AE218898717E922C1_MetadataUsageId;
extern const uint32_t WebRequestUtils_URLDecode_m3F75FA29F50FB340B93815988517E9208C52EE62_MetadataUsageId;
extern const uint32_t WebRequestUtils__cctor_m31EB3E45EC49AB6B33C7A10F79F1CD4FF2BE715A_MetadataUsageId;
struct CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0;;
struct CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_com;
struct CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_com;;
struct CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_pinvoke;
struct CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_pinvoke;;
struct Delegate_t_marshaled_com;
struct Delegate_t_marshaled_pinvoke;
struct DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9;;
struct DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_com;
struct DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_com;;
struct DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_pinvoke;
struct DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_pinvoke;;
struct Exception_t_marshaled_com;
struct Exception_t_marshaled_pinvoke;
struct UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129;;
struct UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_com;
struct UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_com;;
struct UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_pinvoke;
struct UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_pinvoke;;
struct UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4;;
struct UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_com;
struct UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_com;;
struct UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_pinvoke;
struct UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_pinvoke;;

struct ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821;
struct CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2;


#ifndef U3CMODULEU3E_T2FBFFC67F8D6B1FA13284515F9BBD8C9333B5C86_H
#define U3CMODULEU3E_T2FBFFC67F8D6B1FA13284515F9BBD8C9333B5C86_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// <Module>
struct  U3CModuleU3E_t2FBFFC67F8D6B1FA13284515F9BBD8C9333B5C86 
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // U3CMODULEU3E_T2FBFFC67F8D6B1FA13284515F9BBD8C9333B5C86_H
#ifndef RUNTIMEOBJECT_H
#define RUNTIMEOBJECT_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Object

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // RUNTIMEOBJECT_H
struct Il2CppArrayBounds;
#ifndef RUNTIMEARRAY_H
#define RUNTIMEARRAY_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Array

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // RUNTIMEARRAY_H
#ifndef EXCEPTION_T_H
#define EXCEPTION_T_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Exception
struct  Exception_t  : public RuntimeObject
{
public:
	// System.String System.Exception::_className
	String_t* ____className_1;
	// System.String System.Exception::_message
	String_t* ____message_2;
	// System.Collections.IDictionary System.Exception::_data
	RuntimeObject* ____data_3;
	// System.Exception System.Exception::_innerException
	Exception_t * ____innerException_4;
	// System.String System.Exception::_helpURL
	String_t* ____helpURL_5;
	// System.Object System.Exception::_stackTrace
	RuntimeObject * ____stackTrace_6;
	// System.String System.Exception::_stackTraceString
	String_t* ____stackTraceString_7;
	// System.String System.Exception::_remoteStackTraceString
	String_t* ____remoteStackTraceString_8;
	// System.Int32 System.Exception::_remoteStackIndex
	int32_t ____remoteStackIndex_9;
	// System.Object System.Exception::_dynamicMethods
	RuntimeObject * ____dynamicMethods_10;
	// System.Int32 System.Exception::_HResult
	int32_t ____HResult_11;
	// System.String System.Exception::_source
	String_t* ____source_12;
	// System.Runtime.Serialization.SafeSerializationManager System.Exception::_safeSerializationManager
	SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 * ____safeSerializationManager_13;
	// System.Diagnostics.StackTrace[] System.Exception::captured_traces
	StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196* ___captured_traces_14;
	// System.IntPtr[] System.Exception::native_trace_ips
	IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD* ___native_trace_ips_15;

public:
	inline static int32_t get_offset_of__className_1() { return static_cast<int32_t>(offsetof(Exception_t, ____className_1)); }
	inline String_t* get__className_1() const { return ____className_1; }
	inline String_t** get_address_of__className_1() { return &____className_1; }
	inline void set__className_1(String_t* value)
	{
		____className_1 = value;
		Il2CppCodeGenWriteBarrier((&____className_1), value);
	}

	inline static int32_t get_offset_of__message_2() { return static_cast<int32_t>(offsetof(Exception_t, ____message_2)); }
	inline String_t* get__message_2() const { return ____message_2; }
	inline String_t** get_address_of__message_2() { return &____message_2; }
	inline void set__message_2(String_t* value)
	{
		____message_2 = value;
		Il2CppCodeGenWriteBarrier((&____message_2), value);
	}

	inline static int32_t get_offset_of__data_3() { return static_cast<int32_t>(offsetof(Exception_t, ____data_3)); }
	inline RuntimeObject* get__data_3() const { return ____data_3; }
	inline RuntimeObject** get_address_of__data_3() { return &____data_3; }
	inline void set__data_3(RuntimeObject* value)
	{
		____data_3 = value;
		Il2CppCodeGenWriteBarrier((&____data_3), value);
	}

	inline static int32_t get_offset_of__innerException_4() { return static_cast<int32_t>(offsetof(Exception_t, ____innerException_4)); }
	inline Exception_t * get__innerException_4() const { return ____innerException_4; }
	inline Exception_t ** get_address_of__innerException_4() { return &____innerException_4; }
	inline void set__innerException_4(Exception_t * value)
	{
		____innerException_4 = value;
		Il2CppCodeGenWriteBarrier((&____innerException_4), value);
	}

	inline static int32_t get_offset_of__helpURL_5() { return static_cast<int32_t>(offsetof(Exception_t, ____helpURL_5)); }
	inline String_t* get__helpURL_5() const { return ____helpURL_5; }
	inline String_t** get_address_of__helpURL_5() { return &____helpURL_5; }
	inline void set__helpURL_5(String_t* value)
	{
		____helpURL_5 = value;
		Il2CppCodeGenWriteBarrier((&____helpURL_5), value);
	}

	inline static int32_t get_offset_of__stackTrace_6() { return static_cast<int32_t>(offsetof(Exception_t, ____stackTrace_6)); }
	inline RuntimeObject * get__stackTrace_6() const { return ____stackTrace_6; }
	inline RuntimeObject ** get_address_of__stackTrace_6() { return &____stackTrace_6; }
	inline void set__stackTrace_6(RuntimeObject * value)
	{
		____stackTrace_6 = value;
		Il2CppCodeGenWriteBarrier((&____stackTrace_6), value);
	}

	inline static int32_t get_offset_of__stackTraceString_7() { return static_cast<int32_t>(offsetof(Exception_t, ____stackTraceString_7)); }
	inline String_t* get__stackTraceString_7() const { return ____stackTraceString_7; }
	inline String_t** get_address_of__stackTraceString_7() { return &____stackTraceString_7; }
	inline void set__stackTraceString_7(String_t* value)
	{
		____stackTraceString_7 = value;
		Il2CppCodeGenWriteBarrier((&____stackTraceString_7), value);
	}

	inline static int32_t get_offset_of__remoteStackTraceString_8() { return static_cast<int32_t>(offsetof(Exception_t, ____remoteStackTraceString_8)); }
	inline String_t* get__remoteStackTraceString_8() const { return ____remoteStackTraceString_8; }
	inline String_t** get_address_of__remoteStackTraceString_8() { return &____remoteStackTraceString_8; }
	inline void set__remoteStackTraceString_8(String_t* value)
	{
		____remoteStackTraceString_8 = value;
		Il2CppCodeGenWriteBarrier((&____remoteStackTraceString_8), value);
	}

	inline static int32_t get_offset_of__remoteStackIndex_9() { return static_cast<int32_t>(offsetof(Exception_t, ____remoteStackIndex_9)); }
	inline int32_t get__remoteStackIndex_9() const { return ____remoteStackIndex_9; }
	inline int32_t* get_address_of__remoteStackIndex_9() { return &____remoteStackIndex_9; }
	inline void set__remoteStackIndex_9(int32_t value)
	{
		____remoteStackIndex_9 = value;
	}

	inline static int32_t get_offset_of__dynamicMethods_10() { return static_cast<int32_t>(offsetof(Exception_t, ____dynamicMethods_10)); }
	inline RuntimeObject * get__dynamicMethods_10() const { return ____dynamicMethods_10; }
	inline RuntimeObject ** get_address_of__dynamicMethods_10() { return &____dynamicMethods_10; }
	inline void set__dynamicMethods_10(RuntimeObject * value)
	{
		____dynamicMethods_10 = value;
		Il2CppCodeGenWriteBarrier((&____dynamicMethods_10), value);
	}

	inline static int32_t get_offset_of__HResult_11() { return static_cast<int32_t>(offsetof(Exception_t, ____HResult_11)); }
	inline int32_t get__HResult_11() const { return ____HResult_11; }
	inline int32_t* get_address_of__HResult_11() { return &____HResult_11; }
	inline void set__HResult_11(int32_t value)
	{
		____HResult_11 = value;
	}

	inline static int32_t get_offset_of__source_12() { return static_cast<int32_t>(offsetof(Exception_t, ____source_12)); }
	inline String_t* get__source_12() const { return ____source_12; }
	inline String_t** get_address_of__source_12() { return &____source_12; }
	inline void set__source_12(String_t* value)
	{
		____source_12 = value;
		Il2CppCodeGenWriteBarrier((&____source_12), value);
	}

	inline static int32_t get_offset_of__safeSerializationManager_13() { return static_cast<int32_t>(offsetof(Exception_t, ____safeSerializationManager_13)); }
	inline SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 * get__safeSerializationManager_13() const { return ____safeSerializationManager_13; }
	inline SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 ** get_address_of__safeSerializationManager_13() { return &____safeSerializationManager_13; }
	inline void set__safeSerializationManager_13(SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 * value)
	{
		____safeSerializationManager_13 = value;
		Il2CppCodeGenWriteBarrier((&____safeSerializationManager_13), value);
	}

	inline static int32_t get_offset_of_captured_traces_14() { return static_cast<int32_t>(offsetof(Exception_t, ___captured_traces_14)); }
	inline StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196* get_captured_traces_14() const { return ___captured_traces_14; }
	inline StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196** get_address_of_captured_traces_14() { return &___captured_traces_14; }
	inline void set_captured_traces_14(StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196* value)
	{
		___captured_traces_14 = value;
		Il2CppCodeGenWriteBarrier((&___captured_traces_14), value);
	}

	inline static int32_t get_offset_of_native_trace_ips_15() { return static_cast<int32_t>(offsetof(Exception_t, ___native_trace_ips_15)); }
	inline IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD* get_native_trace_ips_15() const { return ___native_trace_ips_15; }
	inline IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD** get_address_of_native_trace_ips_15() { return &___native_trace_ips_15; }
	inline void set_native_trace_ips_15(IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD* value)
	{
		___native_trace_ips_15 = value;
		Il2CppCodeGenWriteBarrier((&___native_trace_ips_15), value);
	}
};

struct Exception_t_StaticFields
{
public:
	// System.Object System.Exception::s_EDILock
	RuntimeObject * ___s_EDILock_0;

public:
	inline static int32_t get_offset_of_s_EDILock_0() { return static_cast<int32_t>(offsetof(Exception_t_StaticFields, ___s_EDILock_0)); }
	inline RuntimeObject * get_s_EDILock_0() const { return ___s_EDILock_0; }
	inline RuntimeObject ** get_address_of_s_EDILock_0() { return &___s_EDILock_0; }
	inline void set_s_EDILock_0(RuntimeObject * value)
	{
		___s_EDILock_0 = value;
		Il2CppCodeGenWriteBarrier((&___s_EDILock_0), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of System.Exception
struct Exception_t_marshaled_pinvoke
{
	char* ____className_1;
	char* ____message_2;
	RuntimeObject* ____data_3;
	Exception_t_marshaled_pinvoke* ____innerException_4;
	char* ____helpURL_5;
	Il2CppIUnknown* ____stackTrace_6;
	char* ____stackTraceString_7;
	char* ____remoteStackTraceString_8;
	int32_t ____remoteStackIndex_9;
	Il2CppIUnknown* ____dynamicMethods_10;
	int32_t ____HResult_11;
	char* ____source_12;
	SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 * ____safeSerializationManager_13;
	StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196* ___captured_traces_14;
	intptr_t* ___native_trace_ips_15;
};
// Native definition for COM marshalling of System.Exception
struct Exception_t_marshaled_com
{
	Il2CppChar* ____className_1;
	Il2CppChar* ____message_2;
	RuntimeObject* ____data_3;
	Exception_t_marshaled_com* ____innerException_4;
	Il2CppChar* ____helpURL_5;
	Il2CppIUnknown* ____stackTrace_6;
	Il2CppChar* ____stackTraceString_7;
	Il2CppChar* ____remoteStackTraceString_8;
	int32_t ____remoteStackIndex_9;
	Il2CppIUnknown* ____dynamicMethods_10;
	int32_t ____HResult_11;
	Il2CppChar* ____source_12;
	SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770 * ____safeSerializationManager_13;
	StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196* ___captured_traces_14;
	intptr_t* ___native_trace_ips_15;
};
#endif // EXCEPTION_T_H
#ifndef MARSHALBYREFOBJECT_TC4577953D0A44D0AB8597CFA868E01C858B1C9AF_H
#define MARSHALBYREFOBJECT_TC4577953D0A44D0AB8597CFA868E01C858B1C9AF_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.MarshalByRefObject
struct  MarshalByRefObject_tC4577953D0A44D0AB8597CFA868E01C858B1C9AF  : public RuntimeObject
{
public:
	// System.Object System.MarshalByRefObject::_identity
	RuntimeObject * ____identity_0;

public:
	inline static int32_t get_offset_of__identity_0() { return static_cast<int32_t>(offsetof(MarshalByRefObject_tC4577953D0A44D0AB8597CFA868E01C858B1C9AF, ____identity_0)); }
	inline RuntimeObject * get__identity_0() const { return ____identity_0; }
	inline RuntimeObject ** get_address_of__identity_0() { return &____identity_0; }
	inline void set__identity_0(RuntimeObject * value)
	{
		____identity_0 = value;
		Il2CppCodeGenWriteBarrier((&____identity_0), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of System.MarshalByRefObject
struct MarshalByRefObject_tC4577953D0A44D0AB8597CFA868E01C858B1C9AF_marshaled_pinvoke
{
	Il2CppIUnknown* ____identity_0;
};
// Native definition for COM marshalling of System.MarshalByRefObject
struct MarshalByRefObject_tC4577953D0A44D0AB8597CFA868E01C858B1C9AF_marshaled_com
{
	Il2CppIUnknown* ____identity_0;
};
#endif // MARSHALBYREFOBJECT_TC4577953D0A44D0AB8597CFA868E01C858B1C9AF_H
#ifndef STRING_T_H
#define STRING_T_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.String
struct  String_t  : public RuntimeObject
{
public:
	// System.Int32 System.String::m_stringLength
	int32_t ___m_stringLength_0;
	// System.Char System.String::m_firstChar
	Il2CppChar ___m_firstChar_1;

public:
	inline static int32_t get_offset_of_m_stringLength_0() { return static_cast<int32_t>(offsetof(String_t, ___m_stringLength_0)); }
	inline int32_t get_m_stringLength_0() const { return ___m_stringLength_0; }
	inline int32_t* get_address_of_m_stringLength_0() { return &___m_stringLength_0; }
	inline void set_m_stringLength_0(int32_t value)
	{
		___m_stringLength_0 = value;
	}

	inline static int32_t get_offset_of_m_firstChar_1() { return static_cast<int32_t>(offsetof(String_t, ___m_firstChar_1)); }
	inline Il2CppChar get_m_firstChar_1() const { return ___m_firstChar_1; }
	inline Il2CppChar* get_address_of_m_firstChar_1() { return &___m_firstChar_1; }
	inline void set_m_firstChar_1(Il2CppChar value)
	{
		___m_firstChar_1 = value;
	}
};

struct String_t_StaticFields
{
public:
	// System.String System.String::Empty
	String_t* ___Empty_5;

public:
	inline static int32_t get_offset_of_Empty_5() { return static_cast<int32_t>(offsetof(String_t_StaticFields, ___Empty_5)); }
	inline String_t* get_Empty_5() const { return ___Empty_5; }
	inline String_t** get_address_of_Empty_5() { return &___Empty_5; }
	inline void set_Empty_5(String_t* value)
	{
		___Empty_5 = value;
		Il2CppCodeGenWriteBarrier((&___Empty_5), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // STRING_T_H
#ifndef ENCODING_T7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4_H
#define ENCODING_T7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Text.Encoding
struct  Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4  : public RuntimeObject
{
public:
	// System.Int32 System.Text.Encoding::m_codePage
	int32_t ___m_codePage_9;
	// System.Globalization.CodePageDataItem System.Text.Encoding::dataItem
	CodePageDataItem_t6E34BEE9CCCBB35C88D714664633AF6E5F5671FB * ___dataItem_10;
	// System.Boolean System.Text.Encoding::m_deserializedFromEverett
	bool ___m_deserializedFromEverett_11;
	// System.Boolean System.Text.Encoding::m_isReadOnly
	bool ___m_isReadOnly_12;
	// System.Text.EncoderFallback System.Text.Encoding::encoderFallback
	EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63 * ___encoderFallback_13;
	// System.Text.DecoderFallback System.Text.Encoding::decoderFallback
	DecoderFallback_t128445EB7676870485230893338EF044F6B72F60 * ___decoderFallback_14;

public:
	inline static int32_t get_offset_of_m_codePage_9() { return static_cast<int32_t>(offsetof(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4, ___m_codePage_9)); }
	inline int32_t get_m_codePage_9() const { return ___m_codePage_9; }
	inline int32_t* get_address_of_m_codePage_9() { return &___m_codePage_9; }
	inline void set_m_codePage_9(int32_t value)
	{
		___m_codePage_9 = value;
	}

	inline static int32_t get_offset_of_dataItem_10() { return static_cast<int32_t>(offsetof(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4, ___dataItem_10)); }
	inline CodePageDataItem_t6E34BEE9CCCBB35C88D714664633AF6E5F5671FB * get_dataItem_10() const { return ___dataItem_10; }
	inline CodePageDataItem_t6E34BEE9CCCBB35C88D714664633AF6E5F5671FB ** get_address_of_dataItem_10() { return &___dataItem_10; }
	inline void set_dataItem_10(CodePageDataItem_t6E34BEE9CCCBB35C88D714664633AF6E5F5671FB * value)
	{
		___dataItem_10 = value;
		Il2CppCodeGenWriteBarrier((&___dataItem_10), value);
	}

	inline static int32_t get_offset_of_m_deserializedFromEverett_11() { return static_cast<int32_t>(offsetof(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4, ___m_deserializedFromEverett_11)); }
	inline bool get_m_deserializedFromEverett_11() const { return ___m_deserializedFromEverett_11; }
	inline bool* get_address_of_m_deserializedFromEverett_11() { return &___m_deserializedFromEverett_11; }
	inline void set_m_deserializedFromEverett_11(bool value)
	{
		___m_deserializedFromEverett_11 = value;
	}

	inline static int32_t get_offset_of_m_isReadOnly_12() { return static_cast<int32_t>(offsetof(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4, ___m_isReadOnly_12)); }
	inline bool get_m_isReadOnly_12() const { return ___m_isReadOnly_12; }
	inline bool* get_address_of_m_isReadOnly_12() { return &___m_isReadOnly_12; }
	inline void set_m_isReadOnly_12(bool value)
	{
		___m_isReadOnly_12 = value;
	}

	inline static int32_t get_offset_of_encoderFallback_13() { return static_cast<int32_t>(offsetof(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4, ___encoderFallback_13)); }
	inline EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63 * get_encoderFallback_13() const { return ___encoderFallback_13; }
	inline EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63 ** get_address_of_encoderFallback_13() { return &___encoderFallback_13; }
	inline void set_encoderFallback_13(EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63 * value)
	{
		___encoderFallback_13 = value;
		Il2CppCodeGenWriteBarrier((&___encoderFallback_13), value);
	}

	inline static int32_t get_offset_of_decoderFallback_14() { return static_cast<int32_t>(offsetof(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4, ___decoderFallback_14)); }
	inline DecoderFallback_t128445EB7676870485230893338EF044F6B72F60 * get_decoderFallback_14() const { return ___decoderFallback_14; }
	inline DecoderFallback_t128445EB7676870485230893338EF044F6B72F60 ** get_address_of_decoderFallback_14() { return &___decoderFallback_14; }
	inline void set_decoderFallback_14(DecoderFallback_t128445EB7676870485230893338EF044F6B72F60 * value)
	{
		___decoderFallback_14 = value;
		Il2CppCodeGenWriteBarrier((&___decoderFallback_14), value);
	}
};

struct Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4_StaticFields
{
public:
	// System.Text.Encoding modreq(System.Runtime.CompilerServices.IsVolatile) System.Text.Encoding::defaultEncoding
	Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * ___defaultEncoding_0;
	// System.Text.Encoding modreq(System.Runtime.CompilerServices.IsVolatile) System.Text.Encoding::unicodeEncoding
	Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * ___unicodeEncoding_1;
	// System.Text.Encoding modreq(System.Runtime.CompilerServices.IsVolatile) System.Text.Encoding::bigEndianUnicode
	Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * ___bigEndianUnicode_2;
	// System.Text.Encoding modreq(System.Runtime.CompilerServices.IsVolatile) System.Text.Encoding::utf7Encoding
	Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * ___utf7Encoding_3;
	// System.Text.Encoding modreq(System.Runtime.CompilerServices.IsVolatile) System.Text.Encoding::utf8Encoding
	Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * ___utf8Encoding_4;
	// System.Text.Encoding modreq(System.Runtime.CompilerServices.IsVolatile) System.Text.Encoding::utf32Encoding
	Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * ___utf32Encoding_5;
	// System.Text.Encoding modreq(System.Runtime.CompilerServices.IsVolatile) System.Text.Encoding::asciiEncoding
	Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * ___asciiEncoding_6;
	// System.Text.Encoding modreq(System.Runtime.CompilerServices.IsVolatile) System.Text.Encoding::latin1Encoding
	Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * ___latin1Encoding_7;
	// System.Collections.Hashtable modreq(System.Runtime.CompilerServices.IsVolatile) System.Text.Encoding::encodings
	Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * ___encodings_8;
	// System.Object System.Text.Encoding::s_InternalSyncObject
	RuntimeObject * ___s_InternalSyncObject_15;

public:
	inline static int32_t get_offset_of_defaultEncoding_0() { return static_cast<int32_t>(offsetof(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4_StaticFields, ___defaultEncoding_0)); }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * get_defaultEncoding_0() const { return ___defaultEncoding_0; }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 ** get_address_of_defaultEncoding_0() { return &___defaultEncoding_0; }
	inline void set_defaultEncoding_0(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * value)
	{
		___defaultEncoding_0 = value;
		Il2CppCodeGenWriteBarrier((&___defaultEncoding_0), value);
	}

	inline static int32_t get_offset_of_unicodeEncoding_1() { return static_cast<int32_t>(offsetof(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4_StaticFields, ___unicodeEncoding_1)); }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * get_unicodeEncoding_1() const { return ___unicodeEncoding_1; }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 ** get_address_of_unicodeEncoding_1() { return &___unicodeEncoding_1; }
	inline void set_unicodeEncoding_1(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * value)
	{
		___unicodeEncoding_1 = value;
		Il2CppCodeGenWriteBarrier((&___unicodeEncoding_1), value);
	}

	inline static int32_t get_offset_of_bigEndianUnicode_2() { return static_cast<int32_t>(offsetof(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4_StaticFields, ___bigEndianUnicode_2)); }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * get_bigEndianUnicode_2() const { return ___bigEndianUnicode_2; }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 ** get_address_of_bigEndianUnicode_2() { return &___bigEndianUnicode_2; }
	inline void set_bigEndianUnicode_2(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * value)
	{
		___bigEndianUnicode_2 = value;
		Il2CppCodeGenWriteBarrier((&___bigEndianUnicode_2), value);
	}

	inline static int32_t get_offset_of_utf7Encoding_3() { return static_cast<int32_t>(offsetof(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4_StaticFields, ___utf7Encoding_3)); }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * get_utf7Encoding_3() const { return ___utf7Encoding_3; }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 ** get_address_of_utf7Encoding_3() { return &___utf7Encoding_3; }
	inline void set_utf7Encoding_3(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * value)
	{
		___utf7Encoding_3 = value;
		Il2CppCodeGenWriteBarrier((&___utf7Encoding_3), value);
	}

	inline static int32_t get_offset_of_utf8Encoding_4() { return static_cast<int32_t>(offsetof(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4_StaticFields, ___utf8Encoding_4)); }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * get_utf8Encoding_4() const { return ___utf8Encoding_4; }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 ** get_address_of_utf8Encoding_4() { return &___utf8Encoding_4; }
	inline void set_utf8Encoding_4(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * value)
	{
		___utf8Encoding_4 = value;
		Il2CppCodeGenWriteBarrier((&___utf8Encoding_4), value);
	}

	inline static int32_t get_offset_of_utf32Encoding_5() { return static_cast<int32_t>(offsetof(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4_StaticFields, ___utf32Encoding_5)); }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * get_utf32Encoding_5() const { return ___utf32Encoding_5; }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 ** get_address_of_utf32Encoding_5() { return &___utf32Encoding_5; }
	inline void set_utf32Encoding_5(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * value)
	{
		___utf32Encoding_5 = value;
		Il2CppCodeGenWriteBarrier((&___utf32Encoding_5), value);
	}

	inline static int32_t get_offset_of_asciiEncoding_6() { return static_cast<int32_t>(offsetof(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4_StaticFields, ___asciiEncoding_6)); }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * get_asciiEncoding_6() const { return ___asciiEncoding_6; }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 ** get_address_of_asciiEncoding_6() { return &___asciiEncoding_6; }
	inline void set_asciiEncoding_6(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * value)
	{
		___asciiEncoding_6 = value;
		Il2CppCodeGenWriteBarrier((&___asciiEncoding_6), value);
	}

	inline static int32_t get_offset_of_latin1Encoding_7() { return static_cast<int32_t>(offsetof(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4_StaticFields, ___latin1Encoding_7)); }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * get_latin1Encoding_7() const { return ___latin1Encoding_7; }
	inline Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 ** get_address_of_latin1Encoding_7() { return &___latin1Encoding_7; }
	inline void set_latin1Encoding_7(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * value)
	{
		___latin1Encoding_7 = value;
		Il2CppCodeGenWriteBarrier((&___latin1Encoding_7), value);
	}

	inline static int32_t get_offset_of_encodings_8() { return static_cast<int32_t>(offsetof(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4_StaticFields, ___encodings_8)); }
	inline Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * get_encodings_8() const { return ___encodings_8; }
	inline Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 ** get_address_of_encodings_8() { return &___encodings_8; }
	inline void set_encodings_8(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * value)
	{
		___encodings_8 = value;
		Il2CppCodeGenWriteBarrier((&___encodings_8), value);
	}

	inline static int32_t get_offset_of_s_InternalSyncObject_15() { return static_cast<int32_t>(offsetof(Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4_StaticFields, ___s_InternalSyncObject_15)); }
	inline RuntimeObject * get_s_InternalSyncObject_15() const { return ___s_InternalSyncObject_15; }
	inline RuntimeObject ** get_address_of_s_InternalSyncObject_15() { return &___s_InternalSyncObject_15; }
	inline void set_s_InternalSyncObject_15(RuntimeObject * value)
	{
		___s_InternalSyncObject_15 = value;
		Il2CppCodeGenWriteBarrier((&___s_InternalSyncObject_15), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // ENCODING_T7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4_H
#ifndef STRINGBUILDER_T_H
#define STRINGBUILDER_T_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Text.StringBuilder
struct  StringBuilder_t  : public RuntimeObject
{
public:
	// System.Char[] System.Text.StringBuilder::m_ChunkChars
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___m_ChunkChars_0;
	// System.Text.StringBuilder System.Text.StringBuilder::m_ChunkPrevious
	StringBuilder_t * ___m_ChunkPrevious_1;
	// System.Int32 System.Text.StringBuilder::m_ChunkLength
	int32_t ___m_ChunkLength_2;
	// System.Int32 System.Text.StringBuilder::m_ChunkOffset
	int32_t ___m_ChunkOffset_3;
	// System.Int32 System.Text.StringBuilder::m_MaxCapacity
	int32_t ___m_MaxCapacity_4;

public:
	inline static int32_t get_offset_of_m_ChunkChars_0() { return static_cast<int32_t>(offsetof(StringBuilder_t, ___m_ChunkChars_0)); }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* get_m_ChunkChars_0() const { return ___m_ChunkChars_0; }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2** get_address_of_m_ChunkChars_0() { return &___m_ChunkChars_0; }
	inline void set_m_ChunkChars_0(CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* value)
	{
		___m_ChunkChars_0 = value;
		Il2CppCodeGenWriteBarrier((&___m_ChunkChars_0), value);
	}

	inline static int32_t get_offset_of_m_ChunkPrevious_1() { return static_cast<int32_t>(offsetof(StringBuilder_t, ___m_ChunkPrevious_1)); }
	inline StringBuilder_t * get_m_ChunkPrevious_1() const { return ___m_ChunkPrevious_1; }
	inline StringBuilder_t ** get_address_of_m_ChunkPrevious_1() { return &___m_ChunkPrevious_1; }
	inline void set_m_ChunkPrevious_1(StringBuilder_t * value)
	{
		___m_ChunkPrevious_1 = value;
		Il2CppCodeGenWriteBarrier((&___m_ChunkPrevious_1), value);
	}

	inline static int32_t get_offset_of_m_ChunkLength_2() { return static_cast<int32_t>(offsetof(StringBuilder_t, ___m_ChunkLength_2)); }
	inline int32_t get_m_ChunkLength_2() const { return ___m_ChunkLength_2; }
	inline int32_t* get_address_of_m_ChunkLength_2() { return &___m_ChunkLength_2; }
	inline void set_m_ChunkLength_2(int32_t value)
	{
		___m_ChunkLength_2 = value;
	}

	inline static int32_t get_offset_of_m_ChunkOffset_3() { return static_cast<int32_t>(offsetof(StringBuilder_t, ___m_ChunkOffset_3)); }
	inline int32_t get_m_ChunkOffset_3() const { return ___m_ChunkOffset_3; }
	inline int32_t* get_address_of_m_ChunkOffset_3() { return &___m_ChunkOffset_3; }
	inline void set_m_ChunkOffset_3(int32_t value)
	{
		___m_ChunkOffset_3 = value;
	}

	inline static int32_t get_offset_of_m_MaxCapacity_4() { return static_cast<int32_t>(offsetof(StringBuilder_t, ___m_MaxCapacity_4)); }
	inline int32_t get_m_MaxCapacity_4() const { return ___m_MaxCapacity_4; }
	inline int32_t* get_address_of_m_MaxCapacity_4() { return &___m_MaxCapacity_4; }
	inline void set_m_MaxCapacity_4(int32_t value)
	{
		___m_MaxCapacity_4 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // STRINGBUILDER_T_H
#ifndef VALUETYPE_T4D0C27076F7C36E76190FB3328E232BCB1CD1FFF_H
#define VALUETYPE_T4D0C27076F7C36E76190FB3328E232BCB1CD1FFF_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.ValueType
struct  ValueType_t4D0C27076F7C36E76190FB3328E232BCB1CD1FFF  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of System.ValueType
struct ValueType_t4D0C27076F7C36E76190FB3328E232BCB1CD1FFF_marshaled_pinvoke
{
};
// Native definition for COM marshalling of System.ValueType
struct ValueType_t4D0C27076F7C36E76190FB3328E232BCB1CD1FFF_marshaled_com
{
};
#endif // VALUETYPE_T4D0C27076F7C36E76190FB3328E232BCB1CD1FFF_H
#ifndef WWWFORM_T8D5ED7CAC180C102E377B21A70CC6A9AD5EAAD24_H
#define WWWFORM_T8D5ED7CAC180C102E377B21A70CC6A9AD5EAAD24_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.WWWForm
struct  WWWForm_t8D5ED7CAC180C102E377B21A70CC6A9AD5EAAD24  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // WWWFORM_T8D5ED7CAC180C102E377B21A70CC6A9AD5EAAD24_H
#ifndef WWWTRANSCODER_T0B24F1F17629756E6464A925870CC39236F39C61_H
#define WWWTRANSCODER_T0B24F1F17629756E6464A925870CC39236F39C61_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.WWWTranscoder
struct  WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61  : public RuntimeObject
{
public:

public:
};

struct WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields
{
public:
	// System.Byte[] UnityEngine.WWWTranscoder::ucHexChars
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___ucHexChars_0;
	// System.Byte[] UnityEngine.WWWTranscoder::lcHexChars
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___lcHexChars_1;
	// System.Byte UnityEngine.WWWTranscoder::urlEscapeChar
	uint8_t ___urlEscapeChar_2;
	// System.Byte[] UnityEngine.WWWTranscoder::urlSpace
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___urlSpace_3;
	// System.Byte[] UnityEngine.WWWTranscoder::dataSpace
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___dataSpace_4;
	// System.Byte[] UnityEngine.WWWTranscoder::urlForbidden
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___urlForbidden_5;
	// System.Byte UnityEngine.WWWTranscoder::qpEscapeChar
	uint8_t ___qpEscapeChar_6;
	// System.Byte[] UnityEngine.WWWTranscoder::qpSpace
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___qpSpace_7;
	// System.Byte[] UnityEngine.WWWTranscoder::qpForbidden
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___qpForbidden_8;

public:
	inline static int32_t get_offset_of_ucHexChars_0() { return static_cast<int32_t>(offsetof(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields, ___ucHexChars_0)); }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* get_ucHexChars_0() const { return ___ucHexChars_0; }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821** get_address_of_ucHexChars_0() { return &___ucHexChars_0; }
	inline void set_ucHexChars_0(ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* value)
	{
		___ucHexChars_0 = value;
		Il2CppCodeGenWriteBarrier((&___ucHexChars_0), value);
	}

	inline static int32_t get_offset_of_lcHexChars_1() { return static_cast<int32_t>(offsetof(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields, ___lcHexChars_1)); }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* get_lcHexChars_1() const { return ___lcHexChars_1; }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821** get_address_of_lcHexChars_1() { return &___lcHexChars_1; }
	inline void set_lcHexChars_1(ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* value)
	{
		___lcHexChars_1 = value;
		Il2CppCodeGenWriteBarrier((&___lcHexChars_1), value);
	}

	inline static int32_t get_offset_of_urlEscapeChar_2() { return static_cast<int32_t>(offsetof(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields, ___urlEscapeChar_2)); }
	inline uint8_t get_urlEscapeChar_2() const { return ___urlEscapeChar_2; }
	inline uint8_t* get_address_of_urlEscapeChar_2() { return &___urlEscapeChar_2; }
	inline void set_urlEscapeChar_2(uint8_t value)
	{
		___urlEscapeChar_2 = value;
	}

	inline static int32_t get_offset_of_urlSpace_3() { return static_cast<int32_t>(offsetof(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields, ___urlSpace_3)); }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* get_urlSpace_3() const { return ___urlSpace_3; }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821** get_address_of_urlSpace_3() { return &___urlSpace_3; }
	inline void set_urlSpace_3(ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* value)
	{
		___urlSpace_3 = value;
		Il2CppCodeGenWriteBarrier((&___urlSpace_3), value);
	}

	inline static int32_t get_offset_of_dataSpace_4() { return static_cast<int32_t>(offsetof(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields, ___dataSpace_4)); }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* get_dataSpace_4() const { return ___dataSpace_4; }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821** get_address_of_dataSpace_4() { return &___dataSpace_4; }
	inline void set_dataSpace_4(ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* value)
	{
		___dataSpace_4 = value;
		Il2CppCodeGenWriteBarrier((&___dataSpace_4), value);
	}

	inline static int32_t get_offset_of_urlForbidden_5() { return static_cast<int32_t>(offsetof(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields, ___urlForbidden_5)); }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* get_urlForbidden_5() const { return ___urlForbidden_5; }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821** get_address_of_urlForbidden_5() { return &___urlForbidden_5; }
	inline void set_urlForbidden_5(ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* value)
	{
		___urlForbidden_5 = value;
		Il2CppCodeGenWriteBarrier((&___urlForbidden_5), value);
	}

	inline static int32_t get_offset_of_qpEscapeChar_6() { return static_cast<int32_t>(offsetof(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields, ___qpEscapeChar_6)); }
	inline uint8_t get_qpEscapeChar_6() const { return ___qpEscapeChar_6; }
	inline uint8_t* get_address_of_qpEscapeChar_6() { return &___qpEscapeChar_6; }
	inline void set_qpEscapeChar_6(uint8_t value)
	{
		___qpEscapeChar_6 = value;
	}

	inline static int32_t get_offset_of_qpSpace_7() { return static_cast<int32_t>(offsetof(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields, ___qpSpace_7)); }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* get_qpSpace_7() const { return ___qpSpace_7; }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821** get_address_of_qpSpace_7() { return &___qpSpace_7; }
	inline void set_qpSpace_7(ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* value)
	{
		___qpSpace_7 = value;
		Il2CppCodeGenWriteBarrier((&___qpSpace_7), value);
	}

	inline static int32_t get_offset_of_qpForbidden_8() { return static_cast<int32_t>(offsetof(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields, ___qpForbidden_8)); }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* get_qpForbidden_8() const { return ___qpForbidden_8; }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821** get_address_of_qpForbidden_8() { return &___qpForbidden_8; }
	inline void set_qpForbidden_8(ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* value)
	{
		___qpForbidden_8 = value;
		Il2CppCodeGenWriteBarrier((&___qpForbidden_8), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // WWWTRANSCODER_T0B24F1F17629756E6464A925870CC39236F39C61_H
#ifndef YIELDINSTRUCTION_T836035AC7BD07A3C7909F7AD2A5B42DE99D91C44_H
#define YIELDINSTRUCTION_T836035AC7BD07A3C7909F7AD2A5B42DE99D91C44_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.YieldInstruction
struct  YieldInstruction_t836035AC7BD07A3C7909F7AD2A5B42DE99D91C44  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of UnityEngine.YieldInstruction
struct YieldInstruction_t836035AC7BD07A3C7909F7AD2A5B42DE99D91C44_marshaled_pinvoke
{
};
// Native definition for COM marshalling of UnityEngine.YieldInstruction
struct YieldInstruction_t836035AC7BD07A3C7909F7AD2A5B42DE99D91C44_marshaled_com
{
};
#endif // YIELDINSTRUCTION_T836035AC7BD07A3C7909F7AD2A5B42DE99D91C44_H
#ifndef WEBREQUESTUTILS_TBE8F8607E3A9633419968F6AF2F706A029AE1296_H
#define WEBREQUESTUTILS_TBE8F8607E3A9633419968F6AF2F706A029AE1296_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngineInternal.WebRequestUtils
struct  WebRequestUtils_tBE8F8607E3A9633419968F6AF2F706A029AE1296  : public RuntimeObject
{
public:

public:
};

struct WebRequestUtils_tBE8F8607E3A9633419968F6AF2F706A029AE1296_StaticFields
{
public:
	// System.Text.RegularExpressions.Regex UnityEngineInternal.WebRequestUtils::domainRegex
	Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF * ___domainRegex_0;

public:
	inline static int32_t get_offset_of_domainRegex_0() { return static_cast<int32_t>(offsetof(WebRequestUtils_tBE8F8607E3A9633419968F6AF2F706A029AE1296_StaticFields, ___domainRegex_0)); }
	inline Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF * get_domainRegex_0() const { return ___domainRegex_0; }
	inline Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF ** get_address_of_domainRegex_0() { return &___domainRegex_0; }
	inline void set_domainRegex_0(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF * value)
	{
		___domainRegex_0 = value;
		Il2CppCodeGenWriteBarrier((&___domainRegex_0), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // WEBREQUESTUTILS_TBE8F8607E3A9633419968F6AF2F706A029AE1296_H
#ifndef BOOLEAN_TB53F6830F670160873277339AA58F15CAED4399C_H
#define BOOLEAN_TB53F6830F670160873277339AA58F15CAED4399C_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Boolean
struct  Boolean_tB53F6830F670160873277339AA58F15CAED4399C 
{
public:
	// System.Boolean System.Boolean::m_value
	bool ___m_value_0;

public:
	inline static int32_t get_offset_of_m_value_0() { return static_cast<int32_t>(offsetof(Boolean_tB53F6830F670160873277339AA58F15CAED4399C, ___m_value_0)); }
	inline bool get_m_value_0() const { return ___m_value_0; }
	inline bool* get_address_of_m_value_0() { return &___m_value_0; }
	inline void set_m_value_0(bool value)
	{
		___m_value_0 = value;
	}
};

struct Boolean_tB53F6830F670160873277339AA58F15CAED4399C_StaticFields
{
public:
	// System.String System.Boolean::TrueString
	String_t* ___TrueString_5;
	// System.String System.Boolean::FalseString
	String_t* ___FalseString_6;

public:
	inline static int32_t get_offset_of_TrueString_5() { return static_cast<int32_t>(offsetof(Boolean_tB53F6830F670160873277339AA58F15CAED4399C_StaticFields, ___TrueString_5)); }
	inline String_t* get_TrueString_5() const { return ___TrueString_5; }
	inline String_t** get_address_of_TrueString_5() { return &___TrueString_5; }
	inline void set_TrueString_5(String_t* value)
	{
		___TrueString_5 = value;
		Il2CppCodeGenWriteBarrier((&___TrueString_5), value);
	}

	inline static int32_t get_offset_of_FalseString_6() { return static_cast<int32_t>(offsetof(Boolean_tB53F6830F670160873277339AA58F15CAED4399C_StaticFields, ___FalseString_6)); }
	inline String_t* get_FalseString_6() const { return ___FalseString_6; }
	inline String_t** get_address_of_FalseString_6() { return &___FalseString_6; }
	inline void set_FalseString_6(String_t* value)
	{
		___FalseString_6 = value;
		Il2CppCodeGenWriteBarrier((&___FalseString_6), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // BOOLEAN_TB53F6830F670160873277339AA58F15CAED4399C_H
#ifndef BYTE_TF87C579059BD4633E6840EBBBEEF899C6E33EF07_H
#define BYTE_TF87C579059BD4633E6840EBBBEEF899C6E33EF07_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Byte
struct  Byte_tF87C579059BD4633E6840EBBBEEF899C6E33EF07 
{
public:
	// System.Byte System.Byte::m_value
	uint8_t ___m_value_0;

public:
	inline static int32_t get_offset_of_m_value_0() { return static_cast<int32_t>(offsetof(Byte_tF87C579059BD4633E6840EBBBEEF899C6E33EF07, ___m_value_0)); }
	inline uint8_t get_m_value_0() const { return ___m_value_0; }
	inline uint8_t* get_address_of_m_value_0() { return &___m_value_0; }
	inline void set_m_value_0(uint8_t value)
	{
		___m_value_0 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // BYTE_TF87C579059BD4633E6840EBBBEEF899C6E33EF07_H
#ifndef CHAR_TBF22D9FC341BE970735250BB6FF1A4A92BBA58B9_H
#define CHAR_TBF22D9FC341BE970735250BB6FF1A4A92BBA58B9_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Char
struct  Char_tBF22D9FC341BE970735250BB6FF1A4A92BBA58B9 
{
public:
	// System.Char System.Char::m_value
	Il2CppChar ___m_value_0;

public:
	inline static int32_t get_offset_of_m_value_0() { return static_cast<int32_t>(offsetof(Char_tBF22D9FC341BE970735250BB6FF1A4A92BBA58B9, ___m_value_0)); }
	inline Il2CppChar get_m_value_0() const { return ___m_value_0; }
	inline Il2CppChar* get_address_of_m_value_0() { return &___m_value_0; }
	inline void set_m_value_0(Il2CppChar value)
	{
		___m_value_0 = value;
	}
};

struct Char_tBF22D9FC341BE970735250BB6FF1A4A92BBA58B9_StaticFields
{
public:
	// System.Byte[] System.Char::categoryForLatin1
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___categoryForLatin1_3;

public:
	inline static int32_t get_offset_of_categoryForLatin1_3() { return static_cast<int32_t>(offsetof(Char_tBF22D9FC341BE970735250BB6FF1A4A92BBA58B9_StaticFields, ___categoryForLatin1_3)); }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* get_categoryForLatin1_3() const { return ___categoryForLatin1_3; }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821** get_address_of_categoryForLatin1_3() { return &___categoryForLatin1_3; }
	inline void set_categoryForLatin1_3(ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* value)
	{
		___categoryForLatin1_3 = value;
		Il2CppCodeGenWriteBarrier((&___categoryForLatin1_3), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // CHAR_TBF22D9FC341BE970735250BB6FF1A4A92BBA58B9_H
#ifndef ENUM_T2AF27C02B8653AE29442467390005ABC74D8F521_H
#define ENUM_T2AF27C02B8653AE29442467390005ABC74D8F521_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Enum
struct  Enum_t2AF27C02B8653AE29442467390005ABC74D8F521  : public ValueType_t4D0C27076F7C36E76190FB3328E232BCB1CD1FFF
{
public:

public:
};

struct Enum_t2AF27C02B8653AE29442467390005ABC74D8F521_StaticFields
{
public:
	// System.Char[] System.Enum::enumSeperatorCharArray
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___enumSeperatorCharArray_0;

public:
	inline static int32_t get_offset_of_enumSeperatorCharArray_0() { return static_cast<int32_t>(offsetof(Enum_t2AF27C02B8653AE29442467390005ABC74D8F521_StaticFields, ___enumSeperatorCharArray_0)); }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* get_enumSeperatorCharArray_0() const { return ___enumSeperatorCharArray_0; }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2** get_address_of_enumSeperatorCharArray_0() { return &___enumSeperatorCharArray_0; }
	inline void set_enumSeperatorCharArray_0(CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* value)
	{
		___enumSeperatorCharArray_0 = value;
		Il2CppCodeGenWriteBarrier((&___enumSeperatorCharArray_0), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of System.Enum
struct Enum_t2AF27C02B8653AE29442467390005ABC74D8F521_marshaled_pinvoke
{
};
// Native definition for COM marshalling of System.Enum
struct Enum_t2AF27C02B8653AE29442467390005ABC74D8F521_marshaled_com
{
};
#endif // ENUM_T2AF27C02B8653AE29442467390005ABC74D8F521_H
#ifndef STREAM_TFC50657DD5AAB87770987F9179D934A51D99D5E7_H
#define STREAM_TFC50657DD5AAB87770987F9179D934A51D99D5E7_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.IO.Stream
struct  Stream_tFC50657DD5AAB87770987F9179D934A51D99D5E7  : public MarshalByRefObject_tC4577953D0A44D0AB8597CFA868E01C858B1C9AF
{
public:
	// System.IO.Stream_ReadWriteTask System.IO.Stream::_activeReadWriteTask
	ReadWriteTask_tFA17EEE8BC5C4C83EAEFCC3662A30DE351ABAA80 * ____activeReadWriteTask_2;
	// System.Threading.SemaphoreSlim System.IO.Stream::_asyncActiveSemaphore
	SemaphoreSlim_t2E2888D1C0C8FAB80823C76F1602E4434B8FA048 * ____asyncActiveSemaphore_3;

public:
	inline static int32_t get_offset_of__activeReadWriteTask_2() { return static_cast<int32_t>(offsetof(Stream_tFC50657DD5AAB87770987F9179D934A51D99D5E7, ____activeReadWriteTask_2)); }
	inline ReadWriteTask_tFA17EEE8BC5C4C83EAEFCC3662A30DE351ABAA80 * get__activeReadWriteTask_2() const { return ____activeReadWriteTask_2; }
	inline ReadWriteTask_tFA17EEE8BC5C4C83EAEFCC3662A30DE351ABAA80 ** get_address_of__activeReadWriteTask_2() { return &____activeReadWriteTask_2; }
	inline void set__activeReadWriteTask_2(ReadWriteTask_tFA17EEE8BC5C4C83EAEFCC3662A30DE351ABAA80 * value)
	{
		____activeReadWriteTask_2 = value;
		Il2CppCodeGenWriteBarrier((&____activeReadWriteTask_2), value);
	}

	inline static int32_t get_offset_of__asyncActiveSemaphore_3() { return static_cast<int32_t>(offsetof(Stream_tFC50657DD5AAB87770987F9179D934A51D99D5E7, ____asyncActiveSemaphore_3)); }
	inline SemaphoreSlim_t2E2888D1C0C8FAB80823C76F1602E4434B8FA048 * get__asyncActiveSemaphore_3() const { return ____asyncActiveSemaphore_3; }
	inline SemaphoreSlim_t2E2888D1C0C8FAB80823C76F1602E4434B8FA048 ** get_address_of__asyncActiveSemaphore_3() { return &____asyncActiveSemaphore_3; }
	inline void set__asyncActiveSemaphore_3(SemaphoreSlim_t2E2888D1C0C8FAB80823C76F1602E4434B8FA048 * value)
	{
		____asyncActiveSemaphore_3 = value;
		Il2CppCodeGenWriteBarrier((&____asyncActiveSemaphore_3), value);
	}
};

struct Stream_tFC50657DD5AAB87770987F9179D934A51D99D5E7_StaticFields
{
public:
	// System.IO.Stream System.IO.Stream::Null
	Stream_tFC50657DD5AAB87770987F9179D934A51D99D5E7 * ___Null_1;

public:
	inline static int32_t get_offset_of_Null_1() { return static_cast<int32_t>(offsetof(Stream_tFC50657DD5AAB87770987F9179D934A51D99D5E7_StaticFields, ___Null_1)); }
	inline Stream_tFC50657DD5AAB87770987F9179D934A51D99D5E7 * get_Null_1() const { return ___Null_1; }
	inline Stream_tFC50657DD5AAB87770987F9179D934A51D99D5E7 ** get_address_of_Null_1() { return &___Null_1; }
	inline void set_Null_1(Stream_tFC50657DD5AAB87770987F9179D934A51D99D5E7 * value)
	{
		___Null_1 = value;
		Il2CppCodeGenWriteBarrier((&___Null_1), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // STREAM_TFC50657DD5AAB87770987F9179D934A51D99D5E7_H
#ifndef INT32_T585191389E07734F19F3156FF88FB3EF4800D102_H
#define INT32_T585191389E07734F19F3156FF88FB3EF4800D102_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Int32
struct  Int32_t585191389E07734F19F3156FF88FB3EF4800D102 
{
public:
	// System.Int32 System.Int32::m_value
	int32_t ___m_value_0;

public:
	inline static int32_t get_offset_of_m_value_0() { return static_cast<int32_t>(offsetof(Int32_t585191389E07734F19F3156FF88FB3EF4800D102, ___m_value_0)); }
	inline int32_t get_m_value_0() const { return ___m_value_0; }
	inline int32_t* get_address_of_m_value_0() { return &___m_value_0; }
	inline void set_m_value_0(int32_t value)
	{
		___m_value_0 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // INT32_T585191389E07734F19F3156FF88FB3EF4800D102_H
#ifndef INT64_T7A386C2FF7B0280A0F516992401DDFCF0FF7B436_H
#define INT64_T7A386C2FF7B0280A0F516992401DDFCF0FF7B436_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Int64
struct  Int64_t7A386C2FF7B0280A0F516992401DDFCF0FF7B436 
{
public:
	// System.Int64 System.Int64::m_value
	int64_t ___m_value_0;

public:
	inline static int32_t get_offset_of_m_value_0() { return static_cast<int32_t>(offsetof(Int64_t7A386C2FF7B0280A0F516992401DDFCF0FF7B436, ___m_value_0)); }
	inline int64_t get_m_value_0() const { return ___m_value_0; }
	inline int64_t* get_address_of_m_value_0() { return &___m_value_0; }
	inline void set_m_value_0(int64_t value)
	{
		___m_value_0 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // INT64_T7A386C2FF7B0280A0F516992401DDFCF0FF7B436_H
#ifndef INTPTR_T_H
#define INTPTR_T_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.IntPtr
struct  IntPtr_t 
{
public:
	// System.Void* System.IntPtr::m_value
	void* ___m_value_0;

public:
	inline static int32_t get_offset_of_m_value_0() { return static_cast<int32_t>(offsetof(IntPtr_t, ___m_value_0)); }
	inline void* get_m_value_0() const { return ___m_value_0; }
	inline void** get_address_of_m_value_0() { return &___m_value_0; }
	inline void set_m_value_0(void* value)
	{
		___m_value_0 = value;
	}
};

struct IntPtr_t_StaticFields
{
public:
	// System.IntPtr System.IntPtr::Zero
	intptr_t ___Zero_1;

public:
	inline static int32_t get_offset_of_Zero_1() { return static_cast<int32_t>(offsetof(IntPtr_t_StaticFields, ___Zero_1)); }
	inline intptr_t get_Zero_1() const { return ___Zero_1; }
	inline intptr_t* get_address_of_Zero_1() { return &___Zero_1; }
	inline void set_Zero_1(intptr_t value)
	{
		___Zero_1 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // INTPTR_T_H
#ifndef SYSTEMEXCEPTION_T5380468142AA850BE4A341D7AF3EAB9C78746782_H
#define SYSTEMEXCEPTION_T5380468142AA850BE4A341D7AF3EAB9C78746782_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.SystemException
struct  SystemException_t5380468142AA850BE4A341D7AF3EAB9C78746782  : public Exception_t
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // SYSTEMEXCEPTION_T5380468142AA850BE4A341D7AF3EAB9C78746782_H
#ifndef VOID_T22962CB4C05B1D89B55A6E1139F0E87A90987017_H
#define VOID_T22962CB4C05B1D89B55A6E1139F0E87A90987017_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Void
struct  Void_t22962CB4C05B1D89B55A6E1139F0E87A90987017 
{
public:
	union
	{
		struct
		{
		};
		uint8_t Void_t22962CB4C05B1D89B55A6E1139F0E87A90987017__padding[1];
	};

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // VOID_T22962CB4C05B1D89B55A6E1139F0E87A90987017_H
#ifndef ARGUMENTEXCEPTION_TEDCD16F20A09ECE461C3DA766C16EDA8864057D1_H
#define ARGUMENTEXCEPTION_TEDCD16F20A09ECE461C3DA766C16EDA8864057D1_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.ArgumentException
struct  ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1  : public SystemException_t5380468142AA850BE4A341D7AF3EAB9C78746782
{
public:
	// System.String System.ArgumentException::m_paramName
	String_t* ___m_paramName_17;

public:
	inline static int32_t get_offset_of_m_paramName_17() { return static_cast<int32_t>(offsetof(ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1, ___m_paramName_17)); }
	inline String_t* get_m_paramName_17() const { return ___m_paramName_17; }
	inline String_t** get_address_of_m_paramName_17() { return &___m_paramName_17; }
	inline void set_m_paramName_17(String_t* value)
	{
		___m_paramName_17 = value;
		Il2CppCodeGenWriteBarrier((&___m_paramName_17), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // ARGUMENTEXCEPTION_TEDCD16F20A09ECE461C3DA766C16EDA8864057D1_H
#ifndef DELEGATE_T_H
#define DELEGATE_T_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Delegate
struct  Delegate_t  : public RuntimeObject
{
public:
	// System.IntPtr System.Delegate::method_ptr
	Il2CppMethodPointer ___method_ptr_0;
	// System.IntPtr System.Delegate::invoke_impl
	intptr_t ___invoke_impl_1;
	// System.Object System.Delegate::m_target
	RuntimeObject * ___m_target_2;
	// System.IntPtr System.Delegate::method
	intptr_t ___method_3;
	// System.IntPtr System.Delegate::delegate_trampoline
	intptr_t ___delegate_trampoline_4;
	// System.IntPtr System.Delegate::extra_arg
	intptr_t ___extra_arg_5;
	// System.IntPtr System.Delegate::method_code
	intptr_t ___method_code_6;
	// System.Reflection.MethodInfo System.Delegate::method_info
	MethodInfo_t * ___method_info_7;
	// System.Reflection.MethodInfo System.Delegate::original_method_info
	MethodInfo_t * ___original_method_info_8;
	// System.DelegateData System.Delegate::data
	DelegateData_t1BF9F691B56DAE5F8C28C5E084FDE94F15F27BBE * ___data_9;
	// System.Boolean System.Delegate::method_is_virtual
	bool ___method_is_virtual_10;

public:
	inline static int32_t get_offset_of_method_ptr_0() { return static_cast<int32_t>(offsetof(Delegate_t, ___method_ptr_0)); }
	inline Il2CppMethodPointer get_method_ptr_0() const { return ___method_ptr_0; }
	inline Il2CppMethodPointer* get_address_of_method_ptr_0() { return &___method_ptr_0; }
	inline void set_method_ptr_0(Il2CppMethodPointer value)
	{
		___method_ptr_0 = value;
	}

	inline static int32_t get_offset_of_invoke_impl_1() { return static_cast<int32_t>(offsetof(Delegate_t, ___invoke_impl_1)); }
	inline intptr_t get_invoke_impl_1() const { return ___invoke_impl_1; }
	inline intptr_t* get_address_of_invoke_impl_1() { return &___invoke_impl_1; }
	inline void set_invoke_impl_1(intptr_t value)
	{
		___invoke_impl_1 = value;
	}

	inline static int32_t get_offset_of_m_target_2() { return static_cast<int32_t>(offsetof(Delegate_t, ___m_target_2)); }
	inline RuntimeObject * get_m_target_2() const { return ___m_target_2; }
	inline RuntimeObject ** get_address_of_m_target_2() { return &___m_target_2; }
	inline void set_m_target_2(RuntimeObject * value)
	{
		___m_target_2 = value;
		Il2CppCodeGenWriteBarrier((&___m_target_2), value);
	}

	inline static int32_t get_offset_of_method_3() { return static_cast<int32_t>(offsetof(Delegate_t, ___method_3)); }
	inline intptr_t get_method_3() const { return ___method_3; }
	inline intptr_t* get_address_of_method_3() { return &___method_3; }
	inline void set_method_3(intptr_t value)
	{
		___method_3 = value;
	}

	inline static int32_t get_offset_of_delegate_trampoline_4() { return static_cast<int32_t>(offsetof(Delegate_t, ___delegate_trampoline_4)); }
	inline intptr_t get_delegate_trampoline_4() const { return ___delegate_trampoline_4; }
	inline intptr_t* get_address_of_delegate_trampoline_4() { return &___delegate_trampoline_4; }
	inline void set_delegate_trampoline_4(intptr_t value)
	{
		___delegate_trampoline_4 = value;
	}

	inline static int32_t get_offset_of_extra_arg_5() { return static_cast<int32_t>(offsetof(Delegate_t, ___extra_arg_5)); }
	inline intptr_t get_extra_arg_5() const { return ___extra_arg_5; }
	inline intptr_t* get_address_of_extra_arg_5() { return &___extra_arg_5; }
	inline void set_extra_arg_5(intptr_t value)
	{
		___extra_arg_5 = value;
	}

	inline static int32_t get_offset_of_method_code_6() { return static_cast<int32_t>(offsetof(Delegate_t, ___method_code_6)); }
	inline intptr_t get_method_code_6() const { return ___method_code_6; }
	inline intptr_t* get_address_of_method_code_6() { return &___method_code_6; }
	inline void set_method_code_6(intptr_t value)
	{
		___method_code_6 = value;
	}

	inline static int32_t get_offset_of_method_info_7() { return static_cast<int32_t>(offsetof(Delegate_t, ___method_info_7)); }
	inline MethodInfo_t * get_method_info_7() const { return ___method_info_7; }
	inline MethodInfo_t ** get_address_of_method_info_7() { return &___method_info_7; }
	inline void set_method_info_7(MethodInfo_t * value)
	{
		___method_info_7 = value;
		Il2CppCodeGenWriteBarrier((&___method_info_7), value);
	}

	inline static int32_t get_offset_of_original_method_info_8() { return static_cast<int32_t>(offsetof(Delegate_t, ___original_method_info_8)); }
	inline MethodInfo_t * get_original_method_info_8() const { return ___original_method_info_8; }
	inline MethodInfo_t ** get_address_of_original_method_info_8() { return &___original_method_info_8; }
	inline void set_original_method_info_8(MethodInfo_t * value)
	{
		___original_method_info_8 = value;
		Il2CppCodeGenWriteBarrier((&___original_method_info_8), value);
	}

	inline static int32_t get_offset_of_data_9() { return static_cast<int32_t>(offsetof(Delegate_t, ___data_9)); }
	inline DelegateData_t1BF9F691B56DAE5F8C28C5E084FDE94F15F27BBE * get_data_9() const { return ___data_9; }
	inline DelegateData_t1BF9F691B56DAE5F8C28C5E084FDE94F15F27BBE ** get_address_of_data_9() { return &___data_9; }
	inline void set_data_9(DelegateData_t1BF9F691B56DAE5F8C28C5E084FDE94F15F27BBE * value)
	{
		___data_9 = value;
		Il2CppCodeGenWriteBarrier((&___data_9), value);
	}

	inline static int32_t get_offset_of_method_is_virtual_10() { return static_cast<int32_t>(offsetof(Delegate_t, ___method_is_virtual_10)); }
	inline bool get_method_is_virtual_10() const { return ___method_is_virtual_10; }
	inline bool* get_address_of_method_is_virtual_10() { return &___method_is_virtual_10; }
	inline void set_method_is_virtual_10(bool value)
	{
		___method_is_virtual_10 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of System.Delegate
struct Delegate_t_marshaled_pinvoke
{
	intptr_t ___method_ptr_0;
	intptr_t ___invoke_impl_1;
	Il2CppIUnknown* ___m_target_2;
	intptr_t ___method_3;
	intptr_t ___delegate_trampoline_4;
	intptr_t ___extra_arg_5;
	intptr_t ___method_code_6;
	MethodInfo_t * ___method_info_7;
	MethodInfo_t * ___original_method_info_8;
	DelegateData_t1BF9F691B56DAE5F8C28C5E084FDE94F15F27BBE * ___data_9;
	int32_t ___method_is_virtual_10;
};
// Native definition for COM marshalling of System.Delegate
struct Delegate_t_marshaled_com
{
	intptr_t ___method_ptr_0;
	intptr_t ___invoke_impl_1;
	Il2CppIUnknown* ___m_target_2;
	intptr_t ___method_3;
	intptr_t ___delegate_trampoline_4;
	intptr_t ___extra_arg_5;
	intptr_t ___method_code_6;
	MethodInfo_t * ___method_info_7;
	MethodInfo_t * ___original_method_info_8;
	DelegateData_t1BF9F691B56DAE5F8C28C5E084FDE94F15F27BBE * ___data_9;
	int32_t ___method_is_virtual_10;
};
#endif // DELEGATE_T_H
#ifndef FORMATEXCEPTION_T2808E076CDE4650AF89F55FD78F49290D0EC5BDC_H
#define FORMATEXCEPTION_T2808E076CDE4650AF89F55FD78F49290D0EC5BDC_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.FormatException
struct  FormatException_t2808E076CDE4650AF89F55FD78F49290D0EC5BDC  : public SystemException_t5380468142AA850BE4A341D7AF3EAB9C78746782
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // FORMATEXCEPTION_T2808E076CDE4650AF89F55FD78F49290D0EC5BDC_H
#ifndef MEMORYSTREAM_T495F44B85E6B4DDE2BB7E17DE963256A74E2298C_H
#define MEMORYSTREAM_T495F44B85E6B4DDE2BB7E17DE963256A74E2298C_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.IO.MemoryStream
struct  MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C  : public Stream_tFC50657DD5AAB87770987F9179D934A51D99D5E7
{
public:
	// System.Byte[] System.IO.MemoryStream::_buffer
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ____buffer_4;
	// System.Int32 System.IO.MemoryStream::_origin
	int32_t ____origin_5;
	// System.Int32 System.IO.MemoryStream::_position
	int32_t ____position_6;
	// System.Int32 System.IO.MemoryStream::_length
	int32_t ____length_7;
	// System.Int32 System.IO.MemoryStream::_capacity
	int32_t ____capacity_8;
	// System.Boolean System.IO.MemoryStream::_expandable
	bool ____expandable_9;
	// System.Boolean System.IO.MemoryStream::_writable
	bool ____writable_10;
	// System.Boolean System.IO.MemoryStream::_exposable
	bool ____exposable_11;
	// System.Boolean System.IO.MemoryStream::_isOpen
	bool ____isOpen_12;
	// System.Threading.Tasks.Task`1<System.Int32> System.IO.MemoryStream::_lastReadTask
	Task_1_t640F0CBB720BB9CD14B90B7B81624471A9F56D87 * ____lastReadTask_13;

public:
	inline static int32_t get_offset_of__buffer_4() { return static_cast<int32_t>(offsetof(MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C, ____buffer_4)); }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* get__buffer_4() const { return ____buffer_4; }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821** get_address_of__buffer_4() { return &____buffer_4; }
	inline void set__buffer_4(ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* value)
	{
		____buffer_4 = value;
		Il2CppCodeGenWriteBarrier((&____buffer_4), value);
	}

	inline static int32_t get_offset_of__origin_5() { return static_cast<int32_t>(offsetof(MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C, ____origin_5)); }
	inline int32_t get__origin_5() const { return ____origin_5; }
	inline int32_t* get_address_of__origin_5() { return &____origin_5; }
	inline void set__origin_5(int32_t value)
	{
		____origin_5 = value;
	}

	inline static int32_t get_offset_of__position_6() { return static_cast<int32_t>(offsetof(MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C, ____position_6)); }
	inline int32_t get__position_6() const { return ____position_6; }
	inline int32_t* get_address_of__position_6() { return &____position_6; }
	inline void set__position_6(int32_t value)
	{
		____position_6 = value;
	}

	inline static int32_t get_offset_of__length_7() { return static_cast<int32_t>(offsetof(MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C, ____length_7)); }
	inline int32_t get__length_7() const { return ____length_7; }
	inline int32_t* get_address_of__length_7() { return &____length_7; }
	inline void set__length_7(int32_t value)
	{
		____length_7 = value;
	}

	inline static int32_t get_offset_of__capacity_8() { return static_cast<int32_t>(offsetof(MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C, ____capacity_8)); }
	inline int32_t get__capacity_8() const { return ____capacity_8; }
	inline int32_t* get_address_of__capacity_8() { return &____capacity_8; }
	inline void set__capacity_8(int32_t value)
	{
		____capacity_8 = value;
	}

	inline static int32_t get_offset_of__expandable_9() { return static_cast<int32_t>(offsetof(MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C, ____expandable_9)); }
	inline bool get__expandable_9() const { return ____expandable_9; }
	inline bool* get_address_of__expandable_9() { return &____expandable_9; }
	inline void set__expandable_9(bool value)
	{
		____expandable_9 = value;
	}

	inline static int32_t get_offset_of__writable_10() { return static_cast<int32_t>(offsetof(MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C, ____writable_10)); }
	inline bool get__writable_10() const { return ____writable_10; }
	inline bool* get_address_of__writable_10() { return &____writable_10; }
	inline void set__writable_10(bool value)
	{
		____writable_10 = value;
	}

	inline static int32_t get_offset_of__exposable_11() { return static_cast<int32_t>(offsetof(MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C, ____exposable_11)); }
	inline bool get__exposable_11() const { return ____exposable_11; }
	inline bool* get_address_of__exposable_11() { return &____exposable_11; }
	inline void set__exposable_11(bool value)
	{
		____exposable_11 = value;
	}

	inline static int32_t get_offset_of__isOpen_12() { return static_cast<int32_t>(offsetof(MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C, ____isOpen_12)); }
	inline bool get__isOpen_12() const { return ____isOpen_12; }
	inline bool* get_address_of__isOpen_12() { return &____isOpen_12; }
	inline void set__isOpen_12(bool value)
	{
		____isOpen_12 = value;
	}

	inline static int32_t get_offset_of__lastReadTask_13() { return static_cast<int32_t>(offsetof(MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C, ____lastReadTask_13)); }
	inline Task_1_t640F0CBB720BB9CD14B90B7B81624471A9F56D87 * get__lastReadTask_13() const { return ____lastReadTask_13; }
	inline Task_1_t640F0CBB720BB9CD14B90B7B81624471A9F56D87 ** get_address_of__lastReadTask_13() { return &____lastReadTask_13; }
	inline void set__lastReadTask_13(Task_1_t640F0CBB720BB9CD14B90B7B81624471A9F56D87 * value)
	{
		____lastReadTask_13 = value;
		Il2CppCodeGenWriteBarrier((&____lastReadTask_13), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // MEMORYSTREAM_T495F44B85E6B4DDE2BB7E17DE963256A74E2298C_H
#ifndef INVALIDOPERATIONEXCEPTION_T0530E734D823F78310CAFAFA424CA5164D93A1F1_H
#define INVALIDOPERATIONEXCEPTION_T0530E734D823F78310CAFAFA424CA5164D93A1F1_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.InvalidOperationException
struct  InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1  : public SystemException_t5380468142AA850BE4A341D7AF3EAB9C78746782
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // INVALIDOPERATIONEXCEPTION_T0530E734D823F78310CAFAFA424CA5164D93A1F1_H
#ifndef STRINGCOMPARISON_T02BAA95468CE9E91115C604577611FDF58FEDCF0_H
#define STRINGCOMPARISON_T02BAA95468CE9E91115C604577611FDF58FEDCF0_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.StringComparison
struct  StringComparison_t02BAA95468CE9E91115C604577611FDF58FEDCF0 
{
public:
	// System.Int32 System.StringComparison::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(StringComparison_t02BAA95468CE9E91115C604577611FDF58FEDCF0, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // STRINGCOMPARISON_T02BAA95468CE9E91115C604577611FDF58FEDCF0_H
#ifndef REGEXOPTIONS_T9A6138CDA9C60924D503C584095349F008C52EA1_H
#define REGEXOPTIONS_T9A6138CDA9C60924D503C584095349F008C52EA1_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Text.RegularExpressions.RegexOptions
struct  RegexOptions_t9A6138CDA9C60924D503C584095349F008C52EA1 
{
public:
	// System.Int32 System.Text.RegularExpressions.RegexOptions::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(RegexOptions_t9A6138CDA9C60924D503C584095349F008C52EA1, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // REGEXOPTIONS_T9A6138CDA9C60924D503C584095349F008C52EA1_H
#ifndef TIMESPAN_TA8069278ACE8A74D6DF7D514A9CD4432433F64C4_H
#define TIMESPAN_TA8069278ACE8A74D6DF7D514A9CD4432433F64C4_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.TimeSpan
struct  TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4 
{
public:
	// System.Int64 System.TimeSpan::_ticks
	int64_t ____ticks_22;

public:
	inline static int32_t get_offset_of__ticks_22() { return static_cast<int32_t>(offsetof(TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4, ____ticks_22)); }
	inline int64_t get__ticks_22() const { return ____ticks_22; }
	inline int64_t* get_address_of__ticks_22() { return &____ticks_22; }
	inline void set__ticks_22(int64_t value)
	{
		____ticks_22 = value;
	}
};

struct TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4_StaticFields
{
public:
	// System.TimeSpan System.TimeSpan::Zero
	TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  ___Zero_19;
	// System.TimeSpan System.TimeSpan::MaxValue
	TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  ___MaxValue_20;
	// System.TimeSpan System.TimeSpan::MinValue
	TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  ___MinValue_21;
	// System.Boolean modreq(System.Runtime.CompilerServices.IsVolatile) System.TimeSpan::_legacyConfigChecked
	bool ____legacyConfigChecked_23;
	// System.Boolean modreq(System.Runtime.CompilerServices.IsVolatile) System.TimeSpan::_legacyMode
	bool ____legacyMode_24;

public:
	inline static int32_t get_offset_of_Zero_19() { return static_cast<int32_t>(offsetof(TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4_StaticFields, ___Zero_19)); }
	inline TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  get_Zero_19() const { return ___Zero_19; }
	inline TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4 * get_address_of_Zero_19() { return &___Zero_19; }
	inline void set_Zero_19(TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  value)
	{
		___Zero_19 = value;
	}

	inline static int32_t get_offset_of_MaxValue_20() { return static_cast<int32_t>(offsetof(TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4_StaticFields, ___MaxValue_20)); }
	inline TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  get_MaxValue_20() const { return ___MaxValue_20; }
	inline TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4 * get_address_of_MaxValue_20() { return &___MaxValue_20; }
	inline void set_MaxValue_20(TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  value)
	{
		___MaxValue_20 = value;
	}

	inline static int32_t get_offset_of_MinValue_21() { return static_cast<int32_t>(offsetof(TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4_StaticFields, ___MinValue_21)); }
	inline TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  get_MinValue_21() const { return ___MinValue_21; }
	inline TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4 * get_address_of_MinValue_21() { return &___MinValue_21; }
	inline void set_MinValue_21(TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  value)
	{
		___MinValue_21 = value;
	}

	inline static int32_t get_offset_of__legacyConfigChecked_23() { return static_cast<int32_t>(offsetof(TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4_StaticFields, ____legacyConfigChecked_23)); }
	inline bool get__legacyConfigChecked_23() const { return ____legacyConfigChecked_23; }
	inline bool* get_address_of__legacyConfigChecked_23() { return &____legacyConfigChecked_23; }
	inline void set__legacyConfigChecked_23(bool value)
	{
		____legacyConfigChecked_23 = value;
	}

	inline static int32_t get_offset_of__legacyMode_24() { return static_cast<int32_t>(offsetof(TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4_StaticFields, ____legacyMode_24)); }
	inline bool get__legacyMode_24() const { return ____legacyMode_24; }
	inline bool* get_address_of__legacyMode_24() { return &____legacyMode_24; }
	inline void set__legacyMode_24(bool value)
	{
		____legacyMode_24 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // TIMESPAN_TA8069278ACE8A74D6DF7D514A9CD4432433F64C4_H
#ifndef FLAGS_TEBE7CABEBD13F16920D6950B384EB8F988250A2A_H
#define FLAGS_TEBE7CABEBD13F16920D6950B384EB8F988250A2A_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Uri_Flags
struct  Flags_tEBE7CABEBD13F16920D6950B384EB8F988250A2A 
{
public:
	// System.UInt64 System.Uri_Flags::value__
	uint64_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(Flags_tEBE7CABEBD13F16920D6950B384EB8F988250A2A, ___value___2)); }
	inline uint64_t get_value___2() const { return ___value___2; }
	inline uint64_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(uint64_t value)
	{
		___value___2 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // FLAGS_TEBE7CABEBD13F16920D6950B384EB8F988250A2A_H
#ifndef URIFORMAT_T4355763D39FF6F0FAA2B43E3A209BA8500730992_H
#define URIFORMAT_T4355763D39FF6F0FAA2B43E3A209BA8500730992_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.UriFormat
struct  UriFormat_t4355763D39FF6F0FAA2B43E3A209BA8500730992 
{
public:
	// System.Int32 System.UriFormat::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(UriFormat_t4355763D39FF6F0FAA2B43E3A209BA8500730992, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // URIFORMAT_T4355763D39FF6F0FAA2B43E3A209BA8500730992_H
#ifndef URIIDNSCOPE_TE1574B39C7492C761EFE2FC12DDE82DE013AC9D1_H
#define URIIDNSCOPE_TE1574B39C7492C761EFE2FC12DDE82DE013AC9D1_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.UriIdnScope
struct  UriIdnScope_tE1574B39C7492C761EFE2FC12DDE82DE013AC9D1 
{
public:
	// System.Int32 System.UriIdnScope::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(UriIdnScope_tE1574B39C7492C761EFE2FC12DDE82DE013AC9D1, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // URIIDNSCOPE_TE1574B39C7492C761EFE2FC12DDE82DE013AC9D1_H
#ifndef URIKIND_T26D0760DDF148ADC939FECD934C0B9FF5C71EA08_H
#define URIKIND_T26D0760DDF148ADC939FECD934C0B9FF5C71EA08_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.UriKind
struct  UriKind_t26D0760DDF148ADC939FECD934C0B9FF5C71EA08 
{
public:
	// System.Int32 System.UriKind::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(UriKind_t26D0760DDF148ADC939FECD934C0B9FF5C71EA08, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // URIKIND_T26D0760DDF148ADC939FECD934C0B9FF5C71EA08_H
#ifndef ASYNCOPERATION_T304C51ABED8AE734CC8DDDFE13013D8D5A44641D_H
#define ASYNCOPERATION_T304C51ABED8AE734CC8DDDFE13013D8D5A44641D_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.AsyncOperation
struct  AsyncOperation_t304C51ABED8AE734CC8DDDFE13013D8D5A44641D  : public YieldInstruction_t836035AC7BD07A3C7909F7AD2A5B42DE99D91C44
{
public:
	// System.IntPtr UnityEngine.AsyncOperation::m_Ptr
	intptr_t ___m_Ptr_0;
	// System.Action`1<UnityEngine.AsyncOperation> UnityEngine.AsyncOperation::m_completeCallback
	Action_1_tCBF754C290FAE894631BED8FD56E9E22C4C187F9 * ___m_completeCallback_1;

public:
	inline static int32_t get_offset_of_m_Ptr_0() { return static_cast<int32_t>(offsetof(AsyncOperation_t304C51ABED8AE734CC8DDDFE13013D8D5A44641D, ___m_Ptr_0)); }
	inline intptr_t get_m_Ptr_0() const { return ___m_Ptr_0; }
	inline intptr_t* get_address_of_m_Ptr_0() { return &___m_Ptr_0; }
	inline void set_m_Ptr_0(intptr_t value)
	{
		___m_Ptr_0 = value;
	}

	inline static int32_t get_offset_of_m_completeCallback_1() { return static_cast<int32_t>(offsetof(AsyncOperation_t304C51ABED8AE734CC8DDDFE13013D8D5A44641D, ___m_completeCallback_1)); }
	inline Action_1_tCBF754C290FAE894631BED8FD56E9E22C4C187F9 * get_m_completeCallback_1() const { return ___m_completeCallback_1; }
	inline Action_1_tCBF754C290FAE894631BED8FD56E9E22C4C187F9 ** get_address_of_m_completeCallback_1() { return &___m_completeCallback_1; }
	inline void set_m_completeCallback_1(Action_1_tCBF754C290FAE894631BED8FD56E9E22C4C187F9 * value)
	{
		___m_completeCallback_1 = value;
		Il2CppCodeGenWriteBarrier((&___m_completeCallback_1), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of UnityEngine.AsyncOperation
struct AsyncOperation_t304C51ABED8AE734CC8DDDFE13013D8D5A44641D_marshaled_pinvoke : public YieldInstruction_t836035AC7BD07A3C7909F7AD2A5B42DE99D91C44_marshaled_pinvoke
{
	intptr_t ___m_Ptr_0;
	Il2CppMethodPointer ___m_completeCallback_1;
};
// Native definition for COM marshalling of UnityEngine.AsyncOperation
struct AsyncOperation_t304C51ABED8AE734CC8DDDFE13013D8D5A44641D_marshaled_com : public YieldInstruction_t836035AC7BD07A3C7909F7AD2A5B42DE99D91C44_marshaled_com
{
	intptr_t ___m_Ptr_0;
	Il2CppMethodPointer ___m_completeCallback_1;
};
#endif // ASYNCOPERATION_T304C51ABED8AE734CC8DDDFE13013D8D5A44641D_H
#ifndef CERTIFICATEHANDLER_TBD070BF4150A44AB482FD36EA3882C363117E8C0_H
#define CERTIFICATEHANDLER_TBD070BF4150A44AB482FD36EA3882C363117E8C0_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.Networking.CertificateHandler
struct  CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0  : public RuntimeObject
{
public:
	// System.IntPtr UnityEngine.Networking.CertificateHandler::m_Ptr
	intptr_t ___m_Ptr_0;

public:
	inline static int32_t get_offset_of_m_Ptr_0() { return static_cast<int32_t>(offsetof(CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0, ___m_Ptr_0)); }
	inline intptr_t get_m_Ptr_0() const { return ___m_Ptr_0; }
	inline intptr_t* get_address_of_m_Ptr_0() { return &___m_Ptr_0; }
	inline void set_m_Ptr_0(intptr_t value)
	{
		___m_Ptr_0 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of UnityEngine.Networking.CertificateHandler
struct CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_pinvoke
{
	intptr_t ___m_Ptr_0;
};
// Native definition for COM marshalling of UnityEngine.Networking.CertificateHandler
struct CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_com
{
	intptr_t ___m_Ptr_0;
};
#endif // CERTIFICATEHANDLER_TBD070BF4150A44AB482FD36EA3882C363117E8C0_H
#ifndef DOWNLOADHANDLER_T4A7802ADC97024B469C87FA454B6973951980EE9_H
#define DOWNLOADHANDLER_T4A7802ADC97024B469C87FA454B6973951980EE9_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.Networking.DownloadHandler
struct  DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9  : public RuntimeObject
{
public:
	// System.IntPtr UnityEngine.Networking.DownloadHandler::m_Ptr
	intptr_t ___m_Ptr_0;

public:
	inline static int32_t get_offset_of_m_Ptr_0() { return static_cast<int32_t>(offsetof(DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9, ___m_Ptr_0)); }
	inline intptr_t get_m_Ptr_0() const { return ___m_Ptr_0; }
	inline intptr_t* get_address_of_m_Ptr_0() { return &___m_Ptr_0; }
	inline void set_m_Ptr_0(intptr_t value)
	{
		___m_Ptr_0 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of UnityEngine.Networking.DownloadHandler
struct DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_pinvoke
{
	intptr_t ___m_Ptr_0;
};
// Native definition for COM marshalling of UnityEngine.Networking.DownloadHandler
struct DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_com
{
	intptr_t ___m_Ptr_0;
};
#endif // DOWNLOADHANDLER_T4A7802ADC97024B469C87FA454B6973951980EE9_H
#ifndef UNITYWEBREQUESTERROR_T0FD8E16D965B4EA8BECD6C42C6BFEA8506E4C327_H
#define UNITYWEBREQUESTERROR_T0FD8E16D965B4EA8BECD6C42C6BFEA8506E4C327_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.Networking.UnityWebRequest_UnityWebRequestError
struct  UnityWebRequestError_t0FD8E16D965B4EA8BECD6C42C6BFEA8506E4C327 
{
public:
	// System.Int32 UnityEngine.Networking.UnityWebRequest_UnityWebRequestError::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(UnityWebRequestError_t0FD8E16D965B4EA8BECD6C42C6BFEA8506E4C327, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // UNITYWEBREQUESTERROR_T0FD8E16D965B4EA8BECD6C42C6BFEA8506E4C327_H
#ifndef UNITYWEBREQUESTMETHOD_T704B7938E8655E8FEDDE169AD54B962166142118_H
#define UNITYWEBREQUESTMETHOD_T704B7938E8655E8FEDDE169AD54B962166142118_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.Networking.UnityWebRequest_UnityWebRequestMethod
struct  UnityWebRequestMethod_t704B7938E8655E8FEDDE169AD54B962166142118 
{
public:
	// System.Int32 UnityEngine.Networking.UnityWebRequest_UnityWebRequestMethod::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(UnityWebRequestMethod_t704B7938E8655E8FEDDE169AD54B962166142118, ___value___2)); }
	inline int32_t get_value___2() const { return ___value___2; }
	inline int32_t* get_address_of_value___2() { return &___value___2; }
	inline void set_value___2(int32_t value)
	{
		___value___2 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // UNITYWEBREQUESTMETHOD_T704B7938E8655E8FEDDE169AD54B962166142118_H
#ifndef UPLOADHANDLER_T24F4097D30A1E7C689D8881A27F251B4741601E4_H
#define UPLOADHANDLER_T24F4097D30A1E7C689D8881A27F251B4741601E4_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.Networking.UploadHandler
struct  UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4  : public RuntimeObject
{
public:
	// System.IntPtr UnityEngine.Networking.UploadHandler::m_Ptr
	intptr_t ___m_Ptr_0;

public:
	inline static int32_t get_offset_of_m_Ptr_0() { return static_cast<int32_t>(offsetof(UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4, ___m_Ptr_0)); }
	inline intptr_t get_m_Ptr_0() const { return ___m_Ptr_0; }
	inline intptr_t* get_address_of_m_Ptr_0() { return &___m_Ptr_0; }
	inline void set_m_Ptr_0(intptr_t value)
	{
		___m_Ptr_0 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of UnityEngine.Networking.UploadHandler
struct UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_pinvoke
{
	intptr_t ___m_Ptr_0;
};
// Native definition for COM marshalling of UnityEngine.Networking.UploadHandler
struct UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_com
{
	intptr_t ___m_Ptr_0;
};
#endif // UPLOADHANDLER_T24F4097D30A1E7C689D8881A27F251B4741601E4_H
#ifndef MULTICASTDELEGATE_T_H
#define MULTICASTDELEGATE_T_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.MulticastDelegate
struct  MulticastDelegate_t  : public Delegate_t
{
public:
	// System.Delegate[] System.MulticastDelegate::delegates
	DelegateU5BU5D_tDFCDEE2A6322F96C0FE49AF47E9ADB8C4B294E86* ___delegates_11;

public:
	inline static int32_t get_offset_of_delegates_11() { return static_cast<int32_t>(offsetof(MulticastDelegate_t, ___delegates_11)); }
	inline DelegateU5BU5D_tDFCDEE2A6322F96C0FE49AF47E9ADB8C4B294E86* get_delegates_11() const { return ___delegates_11; }
	inline DelegateU5BU5D_tDFCDEE2A6322F96C0FE49AF47E9ADB8C4B294E86** get_address_of_delegates_11() { return &___delegates_11; }
	inline void set_delegates_11(DelegateU5BU5D_tDFCDEE2A6322F96C0FE49AF47E9ADB8C4B294E86* value)
	{
		___delegates_11 = value;
		Il2CppCodeGenWriteBarrier((&___delegates_11), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of System.MulticastDelegate
struct MulticastDelegate_t_marshaled_pinvoke : public Delegate_t_marshaled_pinvoke
{
	Delegate_t_marshaled_pinvoke** ___delegates_11;
};
// Native definition for COM marshalling of System.MulticastDelegate
struct MulticastDelegate_t_marshaled_com : public Delegate_t_marshaled_com
{
	Delegate_t_marshaled_com** ___delegates_11;
};
#endif // MULTICASTDELEGATE_T_H
#ifndef REGEX_TFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF_H
#define REGEX_TFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Text.RegularExpressions.Regex
struct  Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF  : public RuntimeObject
{
public:
	// System.String System.Text.RegularExpressions.Regex::pattern
	String_t* ___pattern_0;
	// System.Text.RegularExpressions.RegexRunnerFactory System.Text.RegularExpressions.Regex::factory
	RegexRunnerFactory_t0703F390E2102623B0189DEC095DB182698E404B * ___factory_1;
	// System.Text.RegularExpressions.RegexOptions System.Text.RegularExpressions.Regex::roptions
	int32_t ___roptions_2;
	// System.TimeSpan System.Text.RegularExpressions.Regex::internalMatchTimeout
	TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  ___internalMatchTimeout_5;
	// System.Collections.Hashtable System.Text.RegularExpressions.Regex::caps
	Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * ___caps_8;
	// System.Collections.Hashtable System.Text.RegularExpressions.Regex::capnames
	Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * ___capnames_9;
	// System.String[] System.Text.RegularExpressions.Regex::capslist
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* ___capslist_10;
	// System.Int32 System.Text.RegularExpressions.Regex::capsize
	int32_t ___capsize_11;
	// System.Text.RegularExpressions.ExclusiveReference System.Text.RegularExpressions.Regex::runnerref
	ExclusiveReference_t39E202CDB13A1E6EBA4CE0C7548B192CEB5C64DB * ___runnerref_12;
	// System.Text.RegularExpressions.SharedReference System.Text.RegularExpressions.Regex::replref
	SharedReference_t225BA5C249F9F1D6C959F151695BDF65EF2C92A5 * ___replref_13;
	// System.Text.RegularExpressions.RegexCode System.Text.RegularExpressions.Regex::code
	RegexCode_t12846533CAD1E4221CEDF5A4D15D4D649EA688FA * ___code_14;
	// System.Boolean System.Text.RegularExpressions.Regex::refsInitialized
	bool ___refsInitialized_15;

public:
	inline static int32_t get_offset_of_pattern_0() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF, ___pattern_0)); }
	inline String_t* get_pattern_0() const { return ___pattern_0; }
	inline String_t** get_address_of_pattern_0() { return &___pattern_0; }
	inline void set_pattern_0(String_t* value)
	{
		___pattern_0 = value;
		Il2CppCodeGenWriteBarrier((&___pattern_0), value);
	}

	inline static int32_t get_offset_of_factory_1() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF, ___factory_1)); }
	inline RegexRunnerFactory_t0703F390E2102623B0189DEC095DB182698E404B * get_factory_1() const { return ___factory_1; }
	inline RegexRunnerFactory_t0703F390E2102623B0189DEC095DB182698E404B ** get_address_of_factory_1() { return &___factory_1; }
	inline void set_factory_1(RegexRunnerFactory_t0703F390E2102623B0189DEC095DB182698E404B * value)
	{
		___factory_1 = value;
		Il2CppCodeGenWriteBarrier((&___factory_1), value);
	}

	inline static int32_t get_offset_of_roptions_2() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF, ___roptions_2)); }
	inline int32_t get_roptions_2() const { return ___roptions_2; }
	inline int32_t* get_address_of_roptions_2() { return &___roptions_2; }
	inline void set_roptions_2(int32_t value)
	{
		___roptions_2 = value;
	}

	inline static int32_t get_offset_of_internalMatchTimeout_5() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF, ___internalMatchTimeout_5)); }
	inline TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  get_internalMatchTimeout_5() const { return ___internalMatchTimeout_5; }
	inline TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4 * get_address_of_internalMatchTimeout_5() { return &___internalMatchTimeout_5; }
	inline void set_internalMatchTimeout_5(TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  value)
	{
		___internalMatchTimeout_5 = value;
	}

	inline static int32_t get_offset_of_caps_8() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF, ___caps_8)); }
	inline Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * get_caps_8() const { return ___caps_8; }
	inline Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 ** get_address_of_caps_8() { return &___caps_8; }
	inline void set_caps_8(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * value)
	{
		___caps_8 = value;
		Il2CppCodeGenWriteBarrier((&___caps_8), value);
	}

	inline static int32_t get_offset_of_capnames_9() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF, ___capnames_9)); }
	inline Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * get_capnames_9() const { return ___capnames_9; }
	inline Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 ** get_address_of_capnames_9() { return &___capnames_9; }
	inline void set_capnames_9(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * value)
	{
		___capnames_9 = value;
		Il2CppCodeGenWriteBarrier((&___capnames_9), value);
	}

	inline static int32_t get_offset_of_capslist_10() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF, ___capslist_10)); }
	inline StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* get_capslist_10() const { return ___capslist_10; }
	inline StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E** get_address_of_capslist_10() { return &___capslist_10; }
	inline void set_capslist_10(StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* value)
	{
		___capslist_10 = value;
		Il2CppCodeGenWriteBarrier((&___capslist_10), value);
	}

	inline static int32_t get_offset_of_capsize_11() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF, ___capsize_11)); }
	inline int32_t get_capsize_11() const { return ___capsize_11; }
	inline int32_t* get_address_of_capsize_11() { return &___capsize_11; }
	inline void set_capsize_11(int32_t value)
	{
		___capsize_11 = value;
	}

	inline static int32_t get_offset_of_runnerref_12() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF, ___runnerref_12)); }
	inline ExclusiveReference_t39E202CDB13A1E6EBA4CE0C7548B192CEB5C64DB * get_runnerref_12() const { return ___runnerref_12; }
	inline ExclusiveReference_t39E202CDB13A1E6EBA4CE0C7548B192CEB5C64DB ** get_address_of_runnerref_12() { return &___runnerref_12; }
	inline void set_runnerref_12(ExclusiveReference_t39E202CDB13A1E6EBA4CE0C7548B192CEB5C64DB * value)
	{
		___runnerref_12 = value;
		Il2CppCodeGenWriteBarrier((&___runnerref_12), value);
	}

	inline static int32_t get_offset_of_replref_13() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF, ___replref_13)); }
	inline SharedReference_t225BA5C249F9F1D6C959F151695BDF65EF2C92A5 * get_replref_13() const { return ___replref_13; }
	inline SharedReference_t225BA5C249F9F1D6C959F151695BDF65EF2C92A5 ** get_address_of_replref_13() { return &___replref_13; }
	inline void set_replref_13(SharedReference_t225BA5C249F9F1D6C959F151695BDF65EF2C92A5 * value)
	{
		___replref_13 = value;
		Il2CppCodeGenWriteBarrier((&___replref_13), value);
	}

	inline static int32_t get_offset_of_code_14() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF, ___code_14)); }
	inline RegexCode_t12846533CAD1E4221CEDF5A4D15D4D649EA688FA * get_code_14() const { return ___code_14; }
	inline RegexCode_t12846533CAD1E4221CEDF5A4D15D4D649EA688FA ** get_address_of_code_14() { return &___code_14; }
	inline void set_code_14(RegexCode_t12846533CAD1E4221CEDF5A4D15D4D649EA688FA * value)
	{
		___code_14 = value;
		Il2CppCodeGenWriteBarrier((&___code_14), value);
	}

	inline static int32_t get_offset_of_refsInitialized_15() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF, ___refsInitialized_15)); }
	inline bool get_refsInitialized_15() const { return ___refsInitialized_15; }
	inline bool* get_address_of_refsInitialized_15() { return &___refsInitialized_15; }
	inline void set_refsInitialized_15(bool value)
	{
		___refsInitialized_15 = value;
	}
};

struct Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF_StaticFields
{
public:
	// System.TimeSpan System.Text.RegularExpressions.Regex::MaximumMatchTimeout
	TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  ___MaximumMatchTimeout_3;
	// System.TimeSpan System.Text.RegularExpressions.Regex::InfiniteMatchTimeout
	TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  ___InfiniteMatchTimeout_4;
	// System.TimeSpan System.Text.RegularExpressions.Regex::FallbackDefaultMatchTimeout
	TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  ___FallbackDefaultMatchTimeout_6;
	// System.TimeSpan System.Text.RegularExpressions.Regex::DefaultMatchTimeout
	TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  ___DefaultMatchTimeout_7;
	// System.Collections.Generic.LinkedList`1<System.Text.RegularExpressions.CachedCodeEntry> System.Text.RegularExpressions.Regex::livecode
	LinkedList_1_t44CA4EB2162DC04F96F29C8A68A05D05166137F7 * ___livecode_16;
	// System.Int32 System.Text.RegularExpressions.Regex::cacheSize
	int32_t ___cacheSize_17;

public:
	inline static int32_t get_offset_of_MaximumMatchTimeout_3() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF_StaticFields, ___MaximumMatchTimeout_3)); }
	inline TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  get_MaximumMatchTimeout_3() const { return ___MaximumMatchTimeout_3; }
	inline TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4 * get_address_of_MaximumMatchTimeout_3() { return &___MaximumMatchTimeout_3; }
	inline void set_MaximumMatchTimeout_3(TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  value)
	{
		___MaximumMatchTimeout_3 = value;
	}

	inline static int32_t get_offset_of_InfiniteMatchTimeout_4() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF_StaticFields, ___InfiniteMatchTimeout_4)); }
	inline TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  get_InfiniteMatchTimeout_4() const { return ___InfiniteMatchTimeout_4; }
	inline TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4 * get_address_of_InfiniteMatchTimeout_4() { return &___InfiniteMatchTimeout_4; }
	inline void set_InfiniteMatchTimeout_4(TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  value)
	{
		___InfiniteMatchTimeout_4 = value;
	}

	inline static int32_t get_offset_of_FallbackDefaultMatchTimeout_6() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF_StaticFields, ___FallbackDefaultMatchTimeout_6)); }
	inline TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  get_FallbackDefaultMatchTimeout_6() const { return ___FallbackDefaultMatchTimeout_6; }
	inline TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4 * get_address_of_FallbackDefaultMatchTimeout_6() { return &___FallbackDefaultMatchTimeout_6; }
	inline void set_FallbackDefaultMatchTimeout_6(TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  value)
	{
		___FallbackDefaultMatchTimeout_6 = value;
	}

	inline static int32_t get_offset_of_DefaultMatchTimeout_7() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF_StaticFields, ___DefaultMatchTimeout_7)); }
	inline TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  get_DefaultMatchTimeout_7() const { return ___DefaultMatchTimeout_7; }
	inline TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4 * get_address_of_DefaultMatchTimeout_7() { return &___DefaultMatchTimeout_7; }
	inline void set_DefaultMatchTimeout_7(TimeSpan_tA8069278ACE8A74D6DF7D514A9CD4432433F64C4  value)
	{
		___DefaultMatchTimeout_7 = value;
	}

	inline static int32_t get_offset_of_livecode_16() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF_StaticFields, ___livecode_16)); }
	inline LinkedList_1_t44CA4EB2162DC04F96F29C8A68A05D05166137F7 * get_livecode_16() const { return ___livecode_16; }
	inline LinkedList_1_t44CA4EB2162DC04F96F29C8A68A05D05166137F7 ** get_address_of_livecode_16() { return &___livecode_16; }
	inline void set_livecode_16(LinkedList_1_t44CA4EB2162DC04F96F29C8A68A05D05166137F7 * value)
	{
		___livecode_16 = value;
		Il2CppCodeGenWriteBarrier((&___livecode_16), value);
	}

	inline static int32_t get_offset_of_cacheSize_17() { return static_cast<int32_t>(offsetof(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF_StaticFields, ___cacheSize_17)); }
	inline int32_t get_cacheSize_17() const { return ___cacheSize_17; }
	inline int32_t* get_address_of_cacheSize_17() { return &___cacheSize_17; }
	inline void set_cacheSize_17(int32_t value)
	{
		___cacheSize_17 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // REGEX_TFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF_H
#ifndef URI_T87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_H
#define URI_T87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Uri
struct  Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E  : public RuntimeObject
{
public:
	// System.String System.Uri::m_String
	String_t* ___m_String_16;
	// System.String System.Uri::m_originalUnicodeString
	String_t* ___m_originalUnicodeString_17;
	// System.UriParser System.Uri::m_Syntax
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___m_Syntax_18;
	// System.String System.Uri::m_DnsSafeHost
	String_t* ___m_DnsSafeHost_19;
	// System.Uri_Flags System.Uri::m_Flags
	uint64_t ___m_Flags_20;
	// System.Uri_UriInfo System.Uri::m_Info
	UriInfo_t9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E * ___m_Info_21;
	// System.Boolean System.Uri::m_iriParsing
	bool ___m_iriParsing_22;

public:
	inline static int32_t get_offset_of_m_String_16() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E, ___m_String_16)); }
	inline String_t* get_m_String_16() const { return ___m_String_16; }
	inline String_t** get_address_of_m_String_16() { return &___m_String_16; }
	inline void set_m_String_16(String_t* value)
	{
		___m_String_16 = value;
		Il2CppCodeGenWriteBarrier((&___m_String_16), value);
	}

	inline static int32_t get_offset_of_m_originalUnicodeString_17() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E, ___m_originalUnicodeString_17)); }
	inline String_t* get_m_originalUnicodeString_17() const { return ___m_originalUnicodeString_17; }
	inline String_t** get_address_of_m_originalUnicodeString_17() { return &___m_originalUnicodeString_17; }
	inline void set_m_originalUnicodeString_17(String_t* value)
	{
		___m_originalUnicodeString_17 = value;
		Il2CppCodeGenWriteBarrier((&___m_originalUnicodeString_17), value);
	}

	inline static int32_t get_offset_of_m_Syntax_18() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E, ___m_Syntax_18)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_m_Syntax_18() const { return ___m_Syntax_18; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_m_Syntax_18() { return &___m_Syntax_18; }
	inline void set_m_Syntax_18(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___m_Syntax_18 = value;
		Il2CppCodeGenWriteBarrier((&___m_Syntax_18), value);
	}

	inline static int32_t get_offset_of_m_DnsSafeHost_19() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E, ___m_DnsSafeHost_19)); }
	inline String_t* get_m_DnsSafeHost_19() const { return ___m_DnsSafeHost_19; }
	inline String_t** get_address_of_m_DnsSafeHost_19() { return &___m_DnsSafeHost_19; }
	inline void set_m_DnsSafeHost_19(String_t* value)
	{
		___m_DnsSafeHost_19 = value;
		Il2CppCodeGenWriteBarrier((&___m_DnsSafeHost_19), value);
	}

	inline static int32_t get_offset_of_m_Flags_20() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E, ___m_Flags_20)); }
	inline uint64_t get_m_Flags_20() const { return ___m_Flags_20; }
	inline uint64_t* get_address_of_m_Flags_20() { return &___m_Flags_20; }
	inline void set_m_Flags_20(uint64_t value)
	{
		___m_Flags_20 = value;
	}

	inline static int32_t get_offset_of_m_Info_21() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E, ___m_Info_21)); }
	inline UriInfo_t9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E * get_m_Info_21() const { return ___m_Info_21; }
	inline UriInfo_t9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E ** get_address_of_m_Info_21() { return &___m_Info_21; }
	inline void set_m_Info_21(UriInfo_t9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E * value)
	{
		___m_Info_21 = value;
		Il2CppCodeGenWriteBarrier((&___m_Info_21), value);
	}

	inline static int32_t get_offset_of_m_iriParsing_22() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E, ___m_iriParsing_22)); }
	inline bool get_m_iriParsing_22() const { return ___m_iriParsing_22; }
	inline bool* get_address_of_m_iriParsing_22() { return &___m_iriParsing_22; }
	inline void set_m_iriParsing_22(bool value)
	{
		___m_iriParsing_22 = value;
	}
};

struct Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields
{
public:
	// System.String System.Uri::UriSchemeFile
	String_t* ___UriSchemeFile_0;
	// System.String System.Uri::UriSchemeFtp
	String_t* ___UriSchemeFtp_1;
	// System.String System.Uri::UriSchemeGopher
	String_t* ___UriSchemeGopher_2;
	// System.String System.Uri::UriSchemeHttp
	String_t* ___UriSchemeHttp_3;
	// System.String System.Uri::UriSchemeHttps
	String_t* ___UriSchemeHttps_4;
	// System.String System.Uri::UriSchemeWs
	String_t* ___UriSchemeWs_5;
	// System.String System.Uri::UriSchemeWss
	String_t* ___UriSchemeWss_6;
	// System.String System.Uri::UriSchemeMailto
	String_t* ___UriSchemeMailto_7;
	// System.String System.Uri::UriSchemeNews
	String_t* ___UriSchemeNews_8;
	// System.String System.Uri::UriSchemeNntp
	String_t* ___UriSchemeNntp_9;
	// System.String System.Uri::UriSchemeNetTcp
	String_t* ___UriSchemeNetTcp_10;
	// System.String System.Uri::UriSchemeNetPipe
	String_t* ___UriSchemeNetPipe_11;
	// System.String System.Uri::SchemeDelimiter
	String_t* ___SchemeDelimiter_12;
	// System.Boolean modreq(System.Runtime.CompilerServices.IsVolatile) System.Uri::s_ConfigInitialized
	bool ___s_ConfigInitialized_23;
	// System.Boolean modreq(System.Runtime.CompilerServices.IsVolatile) System.Uri::s_ConfigInitializing
	bool ___s_ConfigInitializing_24;
	// System.UriIdnScope modreq(System.Runtime.CompilerServices.IsVolatile) System.Uri::s_IdnScope
	int32_t ___s_IdnScope_25;
	// System.Boolean modreq(System.Runtime.CompilerServices.IsVolatile) System.Uri::s_IriParsing
	bool ___s_IriParsing_26;
	// System.Boolean System.Uri::useDotNetRelativeOrAbsolute
	bool ___useDotNetRelativeOrAbsolute_27;
	// System.Boolean System.Uri::IsWindowsFileSystem
	bool ___IsWindowsFileSystem_29;
	// System.Object System.Uri::s_initLock
	RuntimeObject * ___s_initLock_30;
	// System.Char[] System.Uri::HexLowerChars
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___HexLowerChars_34;
	// System.Char[] System.Uri::_WSchars
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ____WSchars_35;

public:
	inline static int32_t get_offset_of_UriSchemeFile_0() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___UriSchemeFile_0)); }
	inline String_t* get_UriSchemeFile_0() const { return ___UriSchemeFile_0; }
	inline String_t** get_address_of_UriSchemeFile_0() { return &___UriSchemeFile_0; }
	inline void set_UriSchemeFile_0(String_t* value)
	{
		___UriSchemeFile_0 = value;
		Il2CppCodeGenWriteBarrier((&___UriSchemeFile_0), value);
	}

	inline static int32_t get_offset_of_UriSchemeFtp_1() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___UriSchemeFtp_1)); }
	inline String_t* get_UriSchemeFtp_1() const { return ___UriSchemeFtp_1; }
	inline String_t** get_address_of_UriSchemeFtp_1() { return &___UriSchemeFtp_1; }
	inline void set_UriSchemeFtp_1(String_t* value)
	{
		___UriSchemeFtp_1 = value;
		Il2CppCodeGenWriteBarrier((&___UriSchemeFtp_1), value);
	}

	inline static int32_t get_offset_of_UriSchemeGopher_2() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___UriSchemeGopher_2)); }
	inline String_t* get_UriSchemeGopher_2() const { return ___UriSchemeGopher_2; }
	inline String_t** get_address_of_UriSchemeGopher_2() { return &___UriSchemeGopher_2; }
	inline void set_UriSchemeGopher_2(String_t* value)
	{
		___UriSchemeGopher_2 = value;
		Il2CppCodeGenWriteBarrier((&___UriSchemeGopher_2), value);
	}

	inline static int32_t get_offset_of_UriSchemeHttp_3() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___UriSchemeHttp_3)); }
	inline String_t* get_UriSchemeHttp_3() const { return ___UriSchemeHttp_3; }
	inline String_t** get_address_of_UriSchemeHttp_3() { return &___UriSchemeHttp_3; }
	inline void set_UriSchemeHttp_3(String_t* value)
	{
		___UriSchemeHttp_3 = value;
		Il2CppCodeGenWriteBarrier((&___UriSchemeHttp_3), value);
	}

	inline static int32_t get_offset_of_UriSchemeHttps_4() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___UriSchemeHttps_4)); }
	inline String_t* get_UriSchemeHttps_4() const { return ___UriSchemeHttps_4; }
	inline String_t** get_address_of_UriSchemeHttps_4() { return &___UriSchemeHttps_4; }
	inline void set_UriSchemeHttps_4(String_t* value)
	{
		___UriSchemeHttps_4 = value;
		Il2CppCodeGenWriteBarrier((&___UriSchemeHttps_4), value);
	}

	inline static int32_t get_offset_of_UriSchemeWs_5() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___UriSchemeWs_5)); }
	inline String_t* get_UriSchemeWs_5() const { return ___UriSchemeWs_5; }
	inline String_t** get_address_of_UriSchemeWs_5() { return &___UriSchemeWs_5; }
	inline void set_UriSchemeWs_5(String_t* value)
	{
		___UriSchemeWs_5 = value;
		Il2CppCodeGenWriteBarrier((&___UriSchemeWs_5), value);
	}

	inline static int32_t get_offset_of_UriSchemeWss_6() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___UriSchemeWss_6)); }
	inline String_t* get_UriSchemeWss_6() const { return ___UriSchemeWss_6; }
	inline String_t** get_address_of_UriSchemeWss_6() { return &___UriSchemeWss_6; }
	inline void set_UriSchemeWss_6(String_t* value)
	{
		___UriSchemeWss_6 = value;
		Il2CppCodeGenWriteBarrier((&___UriSchemeWss_6), value);
	}

	inline static int32_t get_offset_of_UriSchemeMailto_7() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___UriSchemeMailto_7)); }
	inline String_t* get_UriSchemeMailto_7() const { return ___UriSchemeMailto_7; }
	inline String_t** get_address_of_UriSchemeMailto_7() { return &___UriSchemeMailto_7; }
	inline void set_UriSchemeMailto_7(String_t* value)
	{
		___UriSchemeMailto_7 = value;
		Il2CppCodeGenWriteBarrier((&___UriSchemeMailto_7), value);
	}

	inline static int32_t get_offset_of_UriSchemeNews_8() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___UriSchemeNews_8)); }
	inline String_t* get_UriSchemeNews_8() const { return ___UriSchemeNews_8; }
	inline String_t** get_address_of_UriSchemeNews_8() { return &___UriSchemeNews_8; }
	inline void set_UriSchemeNews_8(String_t* value)
	{
		___UriSchemeNews_8 = value;
		Il2CppCodeGenWriteBarrier((&___UriSchemeNews_8), value);
	}

	inline static int32_t get_offset_of_UriSchemeNntp_9() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___UriSchemeNntp_9)); }
	inline String_t* get_UriSchemeNntp_9() const { return ___UriSchemeNntp_9; }
	inline String_t** get_address_of_UriSchemeNntp_9() { return &___UriSchemeNntp_9; }
	inline void set_UriSchemeNntp_9(String_t* value)
	{
		___UriSchemeNntp_9 = value;
		Il2CppCodeGenWriteBarrier((&___UriSchemeNntp_9), value);
	}

	inline static int32_t get_offset_of_UriSchemeNetTcp_10() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___UriSchemeNetTcp_10)); }
	inline String_t* get_UriSchemeNetTcp_10() const { return ___UriSchemeNetTcp_10; }
	inline String_t** get_address_of_UriSchemeNetTcp_10() { return &___UriSchemeNetTcp_10; }
	inline void set_UriSchemeNetTcp_10(String_t* value)
	{
		___UriSchemeNetTcp_10 = value;
		Il2CppCodeGenWriteBarrier((&___UriSchemeNetTcp_10), value);
	}

	inline static int32_t get_offset_of_UriSchemeNetPipe_11() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___UriSchemeNetPipe_11)); }
	inline String_t* get_UriSchemeNetPipe_11() const { return ___UriSchemeNetPipe_11; }
	inline String_t** get_address_of_UriSchemeNetPipe_11() { return &___UriSchemeNetPipe_11; }
	inline void set_UriSchemeNetPipe_11(String_t* value)
	{
		___UriSchemeNetPipe_11 = value;
		Il2CppCodeGenWriteBarrier((&___UriSchemeNetPipe_11), value);
	}

	inline static int32_t get_offset_of_SchemeDelimiter_12() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___SchemeDelimiter_12)); }
	inline String_t* get_SchemeDelimiter_12() const { return ___SchemeDelimiter_12; }
	inline String_t** get_address_of_SchemeDelimiter_12() { return &___SchemeDelimiter_12; }
	inline void set_SchemeDelimiter_12(String_t* value)
	{
		___SchemeDelimiter_12 = value;
		Il2CppCodeGenWriteBarrier((&___SchemeDelimiter_12), value);
	}

	inline static int32_t get_offset_of_s_ConfigInitialized_23() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___s_ConfigInitialized_23)); }
	inline bool get_s_ConfigInitialized_23() const { return ___s_ConfigInitialized_23; }
	inline bool* get_address_of_s_ConfigInitialized_23() { return &___s_ConfigInitialized_23; }
	inline void set_s_ConfigInitialized_23(bool value)
	{
		___s_ConfigInitialized_23 = value;
	}

	inline static int32_t get_offset_of_s_ConfigInitializing_24() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___s_ConfigInitializing_24)); }
	inline bool get_s_ConfigInitializing_24() const { return ___s_ConfigInitializing_24; }
	inline bool* get_address_of_s_ConfigInitializing_24() { return &___s_ConfigInitializing_24; }
	inline void set_s_ConfigInitializing_24(bool value)
	{
		___s_ConfigInitializing_24 = value;
	}

	inline static int32_t get_offset_of_s_IdnScope_25() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___s_IdnScope_25)); }
	inline int32_t get_s_IdnScope_25() const { return ___s_IdnScope_25; }
	inline int32_t* get_address_of_s_IdnScope_25() { return &___s_IdnScope_25; }
	inline void set_s_IdnScope_25(int32_t value)
	{
		___s_IdnScope_25 = value;
	}

	inline static int32_t get_offset_of_s_IriParsing_26() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___s_IriParsing_26)); }
	inline bool get_s_IriParsing_26() const { return ___s_IriParsing_26; }
	inline bool* get_address_of_s_IriParsing_26() { return &___s_IriParsing_26; }
	inline void set_s_IriParsing_26(bool value)
	{
		___s_IriParsing_26 = value;
	}

	inline static int32_t get_offset_of_useDotNetRelativeOrAbsolute_27() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___useDotNetRelativeOrAbsolute_27)); }
	inline bool get_useDotNetRelativeOrAbsolute_27() const { return ___useDotNetRelativeOrAbsolute_27; }
	inline bool* get_address_of_useDotNetRelativeOrAbsolute_27() { return &___useDotNetRelativeOrAbsolute_27; }
	inline void set_useDotNetRelativeOrAbsolute_27(bool value)
	{
		___useDotNetRelativeOrAbsolute_27 = value;
	}

	inline static int32_t get_offset_of_IsWindowsFileSystem_29() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___IsWindowsFileSystem_29)); }
	inline bool get_IsWindowsFileSystem_29() const { return ___IsWindowsFileSystem_29; }
	inline bool* get_address_of_IsWindowsFileSystem_29() { return &___IsWindowsFileSystem_29; }
	inline void set_IsWindowsFileSystem_29(bool value)
	{
		___IsWindowsFileSystem_29 = value;
	}

	inline static int32_t get_offset_of_s_initLock_30() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___s_initLock_30)); }
	inline RuntimeObject * get_s_initLock_30() const { return ___s_initLock_30; }
	inline RuntimeObject ** get_address_of_s_initLock_30() { return &___s_initLock_30; }
	inline void set_s_initLock_30(RuntimeObject * value)
	{
		___s_initLock_30 = value;
		Il2CppCodeGenWriteBarrier((&___s_initLock_30), value);
	}

	inline static int32_t get_offset_of_HexLowerChars_34() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ___HexLowerChars_34)); }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* get_HexLowerChars_34() const { return ___HexLowerChars_34; }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2** get_address_of_HexLowerChars_34() { return &___HexLowerChars_34; }
	inline void set_HexLowerChars_34(CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* value)
	{
		___HexLowerChars_34 = value;
		Il2CppCodeGenWriteBarrier((&___HexLowerChars_34), value);
	}

	inline static int32_t get_offset_of__WSchars_35() { return static_cast<int32_t>(offsetof(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields, ____WSchars_35)); }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* get__WSchars_35() const { return ____WSchars_35; }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2** get_address_of__WSchars_35() { return &____WSchars_35; }
	inline void set__WSchars_35(CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* value)
	{
		____WSchars_35 = value;
		Il2CppCodeGenWriteBarrier((&____WSchars_35), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // URI_T87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_H
#ifndef DOWNLOADHANDLERBUFFER_TF6A73B82C9EC807D36B904A58E1DF2DDA696B255_H
#define DOWNLOADHANDLERBUFFER_TF6A73B82C9EC807D36B904A58E1DF2DDA696B255_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.Networking.DownloadHandlerBuffer
struct  DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255  : public DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of UnityEngine.Networking.DownloadHandlerBuffer
struct DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_marshaled_pinvoke : public DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_pinvoke
{
};
// Native definition for COM marshalling of UnityEngine.Networking.DownloadHandlerBuffer
struct DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_marshaled_com : public DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_com
{
};
#endif // DOWNLOADHANDLERBUFFER_TF6A73B82C9EC807D36B904A58E1DF2DDA696B255_H
#ifndef UNITYWEBREQUEST_T9120F5A2C7D43B936B49C0B7E4CA54C822689129_H
#define UNITYWEBREQUEST_T9120F5A2C7D43B936B49C0B7E4CA54C822689129_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.Networking.UnityWebRequest
struct  UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129  : public RuntimeObject
{
public:
	// System.IntPtr UnityEngine.Networking.UnityWebRequest::m_Ptr
	intptr_t ___m_Ptr_0;
	// UnityEngine.Networking.DownloadHandler UnityEngine.Networking.UnityWebRequest::m_DownloadHandler
	DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * ___m_DownloadHandler_1;
	// UnityEngine.Networking.UploadHandler UnityEngine.Networking.UnityWebRequest::m_UploadHandler
	UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * ___m_UploadHandler_2;
	// UnityEngine.Networking.CertificateHandler UnityEngine.Networking.UnityWebRequest::m_CertificateHandler
	CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * ___m_CertificateHandler_3;
	// System.Uri UnityEngine.Networking.UnityWebRequest::m_Uri
	Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___m_Uri_4;
	// System.Boolean UnityEngine.Networking.UnityWebRequest::<disposeCertificateHandlerOnDispose>k__BackingField
	bool ___U3CdisposeCertificateHandlerOnDisposeU3Ek__BackingField_5;
	// System.Boolean UnityEngine.Networking.UnityWebRequest::<disposeDownloadHandlerOnDispose>k__BackingField
	bool ___U3CdisposeDownloadHandlerOnDisposeU3Ek__BackingField_6;
	// System.Boolean UnityEngine.Networking.UnityWebRequest::<disposeUploadHandlerOnDispose>k__BackingField
	bool ___U3CdisposeUploadHandlerOnDisposeU3Ek__BackingField_7;

public:
	inline static int32_t get_offset_of_m_Ptr_0() { return static_cast<int32_t>(offsetof(UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129, ___m_Ptr_0)); }
	inline intptr_t get_m_Ptr_0() const { return ___m_Ptr_0; }
	inline intptr_t* get_address_of_m_Ptr_0() { return &___m_Ptr_0; }
	inline void set_m_Ptr_0(intptr_t value)
	{
		___m_Ptr_0 = value;
	}

	inline static int32_t get_offset_of_m_DownloadHandler_1() { return static_cast<int32_t>(offsetof(UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129, ___m_DownloadHandler_1)); }
	inline DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * get_m_DownloadHandler_1() const { return ___m_DownloadHandler_1; }
	inline DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 ** get_address_of_m_DownloadHandler_1() { return &___m_DownloadHandler_1; }
	inline void set_m_DownloadHandler_1(DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * value)
	{
		___m_DownloadHandler_1 = value;
		Il2CppCodeGenWriteBarrier((&___m_DownloadHandler_1), value);
	}

	inline static int32_t get_offset_of_m_UploadHandler_2() { return static_cast<int32_t>(offsetof(UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129, ___m_UploadHandler_2)); }
	inline UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * get_m_UploadHandler_2() const { return ___m_UploadHandler_2; }
	inline UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 ** get_address_of_m_UploadHandler_2() { return &___m_UploadHandler_2; }
	inline void set_m_UploadHandler_2(UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * value)
	{
		___m_UploadHandler_2 = value;
		Il2CppCodeGenWriteBarrier((&___m_UploadHandler_2), value);
	}

	inline static int32_t get_offset_of_m_CertificateHandler_3() { return static_cast<int32_t>(offsetof(UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129, ___m_CertificateHandler_3)); }
	inline CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * get_m_CertificateHandler_3() const { return ___m_CertificateHandler_3; }
	inline CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 ** get_address_of_m_CertificateHandler_3() { return &___m_CertificateHandler_3; }
	inline void set_m_CertificateHandler_3(CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * value)
	{
		___m_CertificateHandler_3 = value;
		Il2CppCodeGenWriteBarrier((&___m_CertificateHandler_3), value);
	}

	inline static int32_t get_offset_of_m_Uri_4() { return static_cast<int32_t>(offsetof(UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129, ___m_Uri_4)); }
	inline Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * get_m_Uri_4() const { return ___m_Uri_4; }
	inline Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E ** get_address_of_m_Uri_4() { return &___m_Uri_4; }
	inline void set_m_Uri_4(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * value)
	{
		___m_Uri_4 = value;
		Il2CppCodeGenWriteBarrier((&___m_Uri_4), value);
	}

	inline static int32_t get_offset_of_U3CdisposeCertificateHandlerOnDisposeU3Ek__BackingField_5() { return static_cast<int32_t>(offsetof(UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129, ___U3CdisposeCertificateHandlerOnDisposeU3Ek__BackingField_5)); }
	inline bool get_U3CdisposeCertificateHandlerOnDisposeU3Ek__BackingField_5() const { return ___U3CdisposeCertificateHandlerOnDisposeU3Ek__BackingField_5; }
	inline bool* get_address_of_U3CdisposeCertificateHandlerOnDisposeU3Ek__BackingField_5() { return &___U3CdisposeCertificateHandlerOnDisposeU3Ek__BackingField_5; }
	inline void set_U3CdisposeCertificateHandlerOnDisposeU3Ek__BackingField_5(bool value)
	{
		___U3CdisposeCertificateHandlerOnDisposeU3Ek__BackingField_5 = value;
	}

	inline static int32_t get_offset_of_U3CdisposeDownloadHandlerOnDisposeU3Ek__BackingField_6() { return static_cast<int32_t>(offsetof(UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129, ___U3CdisposeDownloadHandlerOnDisposeU3Ek__BackingField_6)); }
	inline bool get_U3CdisposeDownloadHandlerOnDisposeU3Ek__BackingField_6() const { return ___U3CdisposeDownloadHandlerOnDisposeU3Ek__BackingField_6; }
	inline bool* get_address_of_U3CdisposeDownloadHandlerOnDisposeU3Ek__BackingField_6() { return &___U3CdisposeDownloadHandlerOnDisposeU3Ek__BackingField_6; }
	inline void set_U3CdisposeDownloadHandlerOnDisposeU3Ek__BackingField_6(bool value)
	{
		___U3CdisposeDownloadHandlerOnDisposeU3Ek__BackingField_6 = value;
	}

	inline static int32_t get_offset_of_U3CdisposeUploadHandlerOnDisposeU3Ek__BackingField_7() { return static_cast<int32_t>(offsetof(UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129, ___U3CdisposeUploadHandlerOnDisposeU3Ek__BackingField_7)); }
	inline bool get_U3CdisposeUploadHandlerOnDisposeU3Ek__BackingField_7() const { return ___U3CdisposeUploadHandlerOnDisposeU3Ek__BackingField_7; }
	inline bool* get_address_of_U3CdisposeUploadHandlerOnDisposeU3Ek__BackingField_7() { return &___U3CdisposeUploadHandlerOnDisposeU3Ek__BackingField_7; }
	inline void set_U3CdisposeUploadHandlerOnDisposeU3Ek__BackingField_7(bool value)
	{
		___U3CdisposeUploadHandlerOnDisposeU3Ek__BackingField_7 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of UnityEngine.Networking.UnityWebRequest
struct UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_pinvoke
{
	intptr_t ___m_Ptr_0;
	DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_pinvoke ___m_DownloadHandler_1;
	UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_pinvoke ___m_UploadHandler_2;
	CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_pinvoke ___m_CertificateHandler_3;
	Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___m_Uri_4;
	int32_t ___U3CdisposeCertificateHandlerOnDisposeU3Ek__BackingField_5;
	int32_t ___U3CdisposeDownloadHandlerOnDisposeU3Ek__BackingField_6;
	int32_t ___U3CdisposeUploadHandlerOnDisposeU3Ek__BackingField_7;
};
// Native definition for COM marshalling of UnityEngine.Networking.UnityWebRequest
struct UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_com
{
	intptr_t ___m_Ptr_0;
	DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_com* ___m_DownloadHandler_1;
	UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_com* ___m_UploadHandler_2;
	CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_com* ___m_CertificateHandler_3;
	Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___m_Uri_4;
	int32_t ___U3CdisposeCertificateHandlerOnDisposeU3Ek__BackingField_5;
	int32_t ___U3CdisposeDownloadHandlerOnDisposeU3Ek__BackingField_6;
	int32_t ___U3CdisposeUploadHandlerOnDisposeU3Ek__BackingField_7;
};
#endif // UNITYWEBREQUEST_T9120F5A2C7D43B936B49C0B7E4CA54C822689129_H
#ifndef UNITYWEBREQUESTASYNCOPERATION_T726E134F16701A2671D40BEBE22110DC57156353_H
#define UNITYWEBREQUESTASYNCOPERATION_T726E134F16701A2671D40BEBE22110DC57156353_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.Networking.UnityWebRequestAsyncOperation
struct  UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353  : public AsyncOperation_t304C51ABED8AE734CC8DDDFE13013D8D5A44641D
{
public:
	// UnityEngine.Networking.UnityWebRequest UnityEngine.Networking.UnityWebRequestAsyncOperation::<webRequest>k__BackingField
	UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * ___U3CwebRequestU3Ek__BackingField_2;

public:
	inline static int32_t get_offset_of_U3CwebRequestU3Ek__BackingField_2() { return static_cast<int32_t>(offsetof(UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353, ___U3CwebRequestU3Ek__BackingField_2)); }
	inline UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * get_U3CwebRequestU3Ek__BackingField_2() const { return ___U3CwebRequestU3Ek__BackingField_2; }
	inline UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 ** get_address_of_U3CwebRequestU3Ek__BackingField_2() { return &___U3CwebRequestU3Ek__BackingField_2; }
	inline void set_U3CwebRequestU3Ek__BackingField_2(UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * value)
	{
		___U3CwebRequestU3Ek__BackingField_2 = value;
		Il2CppCodeGenWriteBarrier((&___U3CwebRequestU3Ek__BackingField_2), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of UnityEngine.Networking.UnityWebRequestAsyncOperation
struct UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353_marshaled_pinvoke : public AsyncOperation_t304C51ABED8AE734CC8DDDFE13013D8D5A44641D_marshaled_pinvoke
{
	UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_pinvoke* ___U3CwebRequestU3Ek__BackingField_2;
};
// Native definition for COM marshalling of UnityEngine.Networking.UnityWebRequestAsyncOperation
struct UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353_marshaled_com : public AsyncOperation_t304C51ABED8AE734CC8DDDFE13013D8D5A44641D_marshaled_com
{
	UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_com* ___U3CwebRequestU3Ek__BackingField_2;
};
#endif // UNITYWEBREQUESTASYNCOPERATION_T726E134F16701A2671D40BEBE22110DC57156353_H
#ifndef UPLOADHANDLERRAW_T9E6A69B7726F134F31F6744F5EFDF611E7C54F27_H
#define UPLOADHANDLERRAW_T9E6A69B7726F134F31F6744F5EFDF611E7C54F27_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// UnityEngine.Networking.UploadHandlerRaw
struct  UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27  : public UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of UnityEngine.Networking.UploadHandlerRaw
struct UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27_marshaled_pinvoke : public UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_pinvoke
{
};
// Native definition for COM marshalling of UnityEngine.Networking.UploadHandlerRaw
struct UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27_marshaled_com : public UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_com
{
};
#endif // UPLOADHANDLERRAW_T9E6A69B7726F134F31F6744F5EFDF611E7C54F27_H
#ifndef ACTION_1_TCBF754C290FAE894631BED8FD56E9E22C4C187F9_H
#define ACTION_1_TCBF754C290FAE894631BED8FD56E9E22C4C187F9_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Action`1<UnityEngine.AsyncOperation>
struct  Action_1_tCBF754C290FAE894631BED8FD56E9E22C4C187F9  : public MulticastDelegate_t
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // ACTION_1_TCBF754C290FAE894631BED8FD56E9E22C4C187F9_H
// System.Byte[]
struct ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821  : public RuntimeArray
{
public:
	ALIGN_FIELD (8) uint8_t m_Items[1];

public:
	inline uint8_t GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline uint8_t* GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, uint8_t value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
	}
	inline uint8_t GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline uint8_t* GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, uint8_t value)
	{
		m_Items[index] = value;
	}
};
// System.Char[]
struct CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2  : public RuntimeArray
{
public:
	ALIGN_FIELD (8) Il2CppChar m_Items[1];

public:
	inline Il2CppChar GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline Il2CppChar* GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, Il2CppChar value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
	}
	inline Il2CppChar GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline Il2CppChar* GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, Il2CppChar value)
	{
		m_Items[index] = value;
	}
};

extern "C" void DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshal_pinvoke(const DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9& unmarshaled, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_pinvoke& marshaled);
extern "C" void DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshal_pinvoke_back(const DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_pinvoke& marshaled, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9& unmarshaled);
extern "C" void DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshal_pinvoke_cleanup(DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_pinvoke& marshaled);
extern "C" void UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshal_pinvoke(const UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4& unmarshaled, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_pinvoke& marshaled);
extern "C" void UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshal_pinvoke_back(const UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_pinvoke& marshaled, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4& unmarshaled);
extern "C" void UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshal_pinvoke_cleanup(UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_pinvoke& marshaled);
extern "C" void CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshal_pinvoke(const CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0& unmarshaled, CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_pinvoke& marshaled);
extern "C" void CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshal_pinvoke_back(const CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_pinvoke& marshaled, CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0& unmarshaled);
extern "C" void CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshal_pinvoke_cleanup(CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_pinvoke& marshaled);
extern "C" void DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshal_com(const DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9& unmarshaled, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_com& marshaled);
extern "C" void DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshal_com_back(const DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_com& marshaled, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9& unmarshaled);
extern "C" void DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshal_com_cleanup(DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_com& marshaled);
extern "C" void UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshal_com(const UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4& unmarshaled, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_com& marshaled);
extern "C" void UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshal_com_back(const UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_com& marshaled, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4& unmarshaled);
extern "C" void UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshal_com_cleanup(UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_com& marshaled);
extern "C" void CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshal_com(const CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0& unmarshaled, CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_com& marshaled);
extern "C" void CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshal_com_back(const CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_com& marshaled, CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0& unmarshaled);
extern "C" void CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshal_com_cleanup(CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_com& marshaled);
extern "C" void UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshal_pinvoke(const UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129& unmarshaled, UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_pinvoke& marshaled);
extern "C" void UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshal_pinvoke_back(const UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_pinvoke& marshaled, UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129& unmarshaled);
extern "C" void UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshal_pinvoke_cleanup(UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_pinvoke& marshaled);
extern "C" void UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshal_com(const UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129& unmarshaled, UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_com& marshaled);
extern "C" void UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshal_com_back(const UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_com& marshaled, UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129& unmarshaled);
extern "C" void UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshal_com_cleanup(UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_com& marshaled);


// System.Void UnityEngine.Networking.CertificateHandler::Dispose()
extern "C" IL2CPP_METHOD_ATTR void CertificateHandler_Dispose_m9C71BAA51760FDF05AB999B6AB6E6BC71BCB8CA0 (CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * __this, const RuntimeMethod* method);
// System.Void System.Object::Finalize()
extern "C" IL2CPP_METHOD_ATTR void Object_Finalize_m4015B7D3A44DE125C5FE34D7276CD4697C06F380 (RuntimeObject * __this, const RuntimeMethod* method);
// System.Boolean System.IntPtr::op_Inequality(System.IntPtr,System.IntPtr)
extern "C" IL2CPP_METHOD_ATTR bool IntPtr_op_Inequality_mB4886A806009EA825EFCC60CD2A7F6EB8E273A61 (intptr_t p0, intptr_t p1, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.CertificateHandler::Release()
extern "C" IL2CPP_METHOD_ATTR void CertificateHandler_Release_m8D680D11AF8B070587DA5C73E2AE652208BDA90A (CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * __this, const RuntimeMethod* method);
// System.Void System.Object::.ctor()
extern "C" IL2CPP_METHOD_ATTR void Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0 (RuntimeObject * __this, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.DownloadHandler::Dispose()
extern "C" IL2CPP_METHOD_ATTR void DownloadHandler_Dispose_m7478E72B2DBA4B55FAA25F7A1975A13BA5891D4B (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.DownloadHandler::Release()
extern "C" IL2CPP_METHOD_ATTR void DownloadHandler_Release_m913DA503E4183F3323A3D0121FFC978D0F220D5D (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * __this, const RuntimeMethod* method);
// System.Text.Encoding UnityEngine.Networking.DownloadHandler::GetTextEncoder()
extern "C" IL2CPP_METHOD_ATTR Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * DownloadHandler_GetTextEncoder_m601540FD9D16122709582833632A9DEEDBF07E64 (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * __this, const RuntimeMethod* method);
// System.String UnityEngine.Networking.DownloadHandler::GetContentType()
extern "C" IL2CPP_METHOD_ATTR String_t* DownloadHandler_GetContentType_mB1653D4D9CA539D1D622C32B52DF5C38548D30E8 (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * __this, const RuntimeMethod* method);
// System.Boolean System.String::IsNullOrEmpty(System.String)
extern "C" IL2CPP_METHOD_ATTR bool String_IsNullOrEmpty_m06A85A206AC2106D1982826C5665B9BD35324229 (String_t* p0, const RuntimeMethod* method);
// System.Int32 System.String::IndexOf(System.String,System.StringComparison)
extern "C" IL2CPP_METHOD_ATTR int32_t String_IndexOf_mF9EA8429E9D1B7475D5A297E67435CF34E965F28 (String_t* __this, String_t* p0, int32_t p1, const RuntimeMethod* method);
// System.Int32 System.String::IndexOf(System.Char,System.Int32)
extern "C" IL2CPP_METHOD_ATTR int32_t String_IndexOf_m66F6178DB4B2F61F4FAFD8B75787D0AB142ADD7D (String_t* __this, Il2CppChar p0, int32_t p1, const RuntimeMethod* method);
// System.String System.String::Substring(System.Int32)
extern "C" IL2CPP_METHOD_ATTR String_t* String_Substring_m2C4AFF5E79DD8BADFD2DFBCF156BF728FBB8E1AE (String_t* __this, int32_t p0, const RuntimeMethod* method);
// System.String System.String::Trim()
extern "C" IL2CPP_METHOD_ATTR String_t* String_Trim_mB52EB7876C7132358B76B7DC95DEACA20722EF4D (String_t* __this, const RuntimeMethod* method);
// System.String System.String::Trim(System.Char[])
extern "C" IL2CPP_METHOD_ATTR String_t* String_Trim_m788DE5AEFDAC40E778745C4DF4AFD45A4BC1007E (String_t* __this, CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* p0, const RuntimeMethod* method);
// System.Int32 System.String::IndexOf(System.Char)
extern "C" IL2CPP_METHOD_ATTR int32_t String_IndexOf_m2909B8CF585E1BD0C81E11ACA2F48012156FD5BD (String_t* __this, Il2CppChar p0, const RuntimeMethod* method);
// System.String System.String::Substring(System.Int32,System.Int32)
extern "C" IL2CPP_METHOD_ATTR String_t* String_Substring_mB593C0A320C683E6E47EFFC0A12B7A465E5E43BB (String_t* __this, int32_t p0, int32_t p1, const RuntimeMethod* method);
// System.Text.Encoding System.Text.Encoding::GetEncoding(System.String)
extern "C" IL2CPP_METHOD_ATTR Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * Encoding_GetEncoding_mA19D07F2E88F8FF58D42B73AFF5E22241607D54E (String_t* p0, const RuntimeMethod* method);
// System.String System.String::Format(System.String,System.Object,System.Object)
extern "C" IL2CPP_METHOD_ATTR String_t* String_Format_m19325298DBC61AAC016C16F7B3CF97A8A3DEA34A (String_t* p0, RuntimeObject * p1, RuntimeObject * p2, const RuntimeMethod* method);
// System.Void UnityEngine.Debug::LogWarning(System.Object)
extern "C" IL2CPP_METHOD_ATTR void Debug_LogWarning_m37338644DC81F640CCDFEAE35A223F0E965F0568 (RuntimeObject * p0, const RuntimeMethod* method);
// System.Text.Encoding System.Text.Encoding::get_UTF8()
extern "C" IL2CPP_METHOD_ATTR Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * Encoding_get_UTF8_m67C8652936B681E7BC7505E459E88790E0FF16D9 (const RuntimeMethod* method);
// System.Void UnityEngine.Networking.DownloadHandler::.ctor()
extern "C" IL2CPP_METHOD_ATTR void DownloadHandler__ctor_m39F80F1C9B379B0D0362DF9264DE42604BDB24E0 (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.DownloadHandlerBuffer::InternalCreateBuffer()
extern "C" IL2CPP_METHOD_ATTR void DownloadHandlerBuffer_InternalCreateBuffer_m661B598DF8BD7BF86374FD84C52C8AEA8FA7BEF6 (DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255 * __this, const RuntimeMethod* method);
// System.IntPtr UnityEngine.Networking.DownloadHandlerBuffer::Create(UnityEngine.Networking.DownloadHandlerBuffer)
extern "C" IL2CPP_METHOD_ATTR intptr_t DownloadHandlerBuffer_Create_m39E26BEA64B617123CEF559999C8352CA9FA5137 (DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255 * ___obj0, const RuntimeMethod* method);
// System.Byte[] UnityEngine.Networking.DownloadHandlerBuffer::InternalGetData()
extern "C" IL2CPP_METHOD_ATTR ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* DownloadHandlerBuffer_InternalGetData_m9266395B691394754B68543A2FF2F19566C5ABBF (DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255 * __this, const RuntimeMethod* method);
// System.Byte[] UnityEngine.Networking.DownloadHandler::InternalGetByteArray(UnityEngine.Networking.DownloadHandler)
extern "C" IL2CPP_METHOD_ATTR ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* DownloadHandler_InternalGetByteArray_mD6D13BFFBF2F56415E10FFEFDC4A68FE29D6D4FD (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * ___dh0, const RuntimeMethod* method);
// System.IntPtr UnityEngine.Networking.UnityWebRequest::Create()
extern "C" IL2CPP_METHOD_ATTR intptr_t UnityWebRequest_Create_m98363C34C71AA034B47FA64589711B6F0AEF6698 (const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::InternalSetDefaults()
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_InternalSetDefaults_m644CC3C1C737838385F0EC9523A8930E696A9309 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::set_url(System.String)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_set_url_mA698FD94C447FF7C1C429D50C2EBAEEDD473007D (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::set_method(System.String)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_set_method_mF2DAC86EB05D65B9BCB52056B7CBB2C1AD87EEC6 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::set_downloadHandler(UnityEngine.Networking.DownloadHandler)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_set_downloadHandler_mB481C2EE30F84B62396C1A837F46046F12B8FA7E (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::set_uploadHandler(UnityEngine.Networking.UploadHandler)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_set_uploadHandler_m7B33656584914FB3F6FB0FF73C08F671262724A1 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::set_uri(System.Uri)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_set_uri_mDE5DEDA2698190CD80AE3B4B7342400A32284124 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::Abort()
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_Abort_mF2C9BD010E5B32FF9F57C2EB4A9A0C8D0289CA7E (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::Release()
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_Release_mD168D309DCE6696163B3357FA21047689D1A7D74 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::set_disposeDownloadHandlerOnDispose(System.Boolean)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_set_disposeDownloadHandlerOnDispose_mA888301C47844E383DEC96D88CAD6CB8D9E7B9FA (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, bool ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::set_disposeUploadHandlerOnDispose(System.Boolean)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_set_disposeUploadHandlerOnDispose_mC176753B8AFBB40B69FAD7F1E2B2711CA5D6AA71 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, bool ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::set_disposeCertificateHandlerOnDispose(System.Boolean)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_set_disposeCertificateHandlerOnDispose_m8609E1213309D1796E00860ECA9228F6454114AE (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, bool ___value0, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::DisposeHandlers()
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_DisposeHandlers_m0E54EE2A704090B2C2F1F3C90D30A47E3BF2B5C9 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::InternalDestroy()
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_InternalDestroy_mF5D7484808AEAE24A43B678614D257FBF885026B (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// System.Void System.GC::SuppressFinalize(System.Object)
extern "C" IL2CPP_METHOD_ATTR void GC_SuppressFinalize_m037319A9B95A5BA437E806DE592802225EE5B425 (RuntimeObject * p0, const RuntimeMethod* method);
// System.Boolean UnityEngine.Networking.UnityWebRequest::get_disposeDownloadHandlerOnDispose()
extern "C" IL2CPP_METHOD_ATTR bool UnityWebRequest_get_disposeDownloadHandlerOnDispose_m3BE68E08A94D92D7076F49CB5196019E6E5E17AA (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// UnityEngine.Networking.DownloadHandler UnityEngine.Networking.UnityWebRequest::get_downloadHandler()
extern "C" IL2CPP_METHOD_ATTR DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * UnityWebRequest_get_downloadHandler_m83044026479E6B4B2739DCE9EEA8A0FAE7D9AF41 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.Networking.UnityWebRequest::get_disposeUploadHandlerOnDispose()
extern "C" IL2CPP_METHOD_ATTR bool UnityWebRequest_get_disposeUploadHandlerOnDispose_mE4A39A3A06DB4450DA49972254B4498A5F8F69DE (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// UnityEngine.Networking.UploadHandler UnityEngine.Networking.UnityWebRequest::get_uploadHandler()
extern "C" IL2CPP_METHOD_ATTR UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * UnityWebRequest_get_uploadHandler_mB23A35C2412258E44538F37AA540421C95E69A5C (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UploadHandler::Dispose()
extern "C" IL2CPP_METHOD_ATTR void UploadHandler_Dispose_m9BBE8D7D2BBAAC2DE84B52BADA0B79CEA6F2DAB2 (UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.Networking.UnityWebRequest::get_disposeCertificateHandlerOnDispose()
extern "C" IL2CPP_METHOD_ATTR bool UnityWebRequest_get_disposeCertificateHandlerOnDispose_m98EFCAC30D637479DC0DC45CFD8A15D402328F99 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// UnityEngine.Networking.CertificateHandler UnityEngine.Networking.UnityWebRequest::get_certificateHandler()
extern "C" IL2CPP_METHOD_ATTR CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * UnityWebRequest_get_certificateHandler_mD3C46D07991190373A7144A6732E390FFBE6DF00 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// UnityEngine.Networking.UnityWebRequestAsyncOperation UnityEngine.Networking.UnityWebRequest::SendWebRequest()
extern "C" IL2CPP_METHOD_ATTR UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * UnityWebRequest_SendWebRequest_mF536CB2A0A39354A54B555B66B017816C5833EBD (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// UnityEngine.Networking.UnityWebRequestAsyncOperation UnityEngine.Networking.UnityWebRequest::BeginWebRequest()
extern "C" IL2CPP_METHOD_ATTR UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * UnityWebRequest_BeginWebRequest_m1EF3612D316F7924F6E40D63DD3B0D0118C50CC0 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequestAsyncOperation::set_webRequest(UnityEngine.Networking.UnityWebRequest)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequestAsyncOperation_set_webRequest_m07869D44180E2A93042A18260FA5A2BB934AC42F (UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * __this, UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * ___value0, const RuntimeMethod* method);
// System.Boolean UnityEngine.Networking.UnityWebRequest::get_isModifiable()
extern "C" IL2CPP_METHOD_ATTR bool UnityWebRequest_get_isModifiable_mD7583537BBC7111555FF73846D120103D2563342 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// System.Void System.InvalidOperationException::.ctor(System.String)
extern "C" IL2CPP_METHOD_ATTR void InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706 (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * __this, String_t* p0, const RuntimeMethod* method);
// UnityEngine.Networking.UnityWebRequest/UnityWebRequestError UnityEngine.Networking.UnityWebRequest::SetMethod(UnityEngine.Networking.UnityWebRequest/UnityWebRequestMethod)
extern "C" IL2CPP_METHOD_ATTR int32_t UnityWebRequest_SetMethod_mEE55FF0E071E784318B8C2110E3A3688BF4661CB (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, int32_t ___methodType0, const RuntimeMethod* method);
// System.String UnityEngine.Networking.UnityWebRequest::GetWebErrorString(UnityEngine.Networking.UnityWebRequest/UnityWebRequestError)
extern "C" IL2CPP_METHOD_ATTR String_t* UnityWebRequest_GetWebErrorString_m92A1DDF2ADFFF8AEE6B1A7FAE384743C31F9E01D (int32_t ___err0, const RuntimeMethod* method);
// UnityEngine.Networking.UnityWebRequest/UnityWebRequestError UnityEngine.Networking.UnityWebRequest::SetCustomMethod(System.String)
extern "C" IL2CPP_METHOD_ATTR int32_t UnityWebRequest_SetCustomMethod_mC818FAC0FD8B91FD454C6DFBF7561EEE2D0BA4F4 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___customMethodName0, const RuntimeMethod* method);
// System.Void System.ArgumentException::.ctor(System.String)
extern "C" IL2CPP_METHOD_ATTR void ArgumentException__ctor_m9A85EF7FEFEC21DDD525A67E831D77278E5165B7 (ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 * __this, String_t* p0, const RuntimeMethod* method);
// System.String System.String::ToUpper()
extern "C" IL2CPP_METHOD_ATTR String_t* String_ToUpper_m23D019B7C5EF2C5C01F524EB8137A424B33EEFE2 (String_t* __this, const RuntimeMethod* method);
// System.Boolean System.String::op_Equality(System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR bool String_op_Equality_m139F0E4195AE2F856019E63B241F36F016997FCE (String_t* p0, String_t* p1, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::InternalSetMethod(UnityEngine.Networking.UnityWebRequest/UnityWebRequestMethod)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_InternalSetMethod_m636508AA8E6EF12B3255D8ED108BFF7EB1AB68C9 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, int32_t ___methodType0, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::InternalSetCustomMethod(System.String)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_InternalSetCustomMethod_mE9F0C84C6DCD5412AEDD76280EEC4FB82516EF16 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___customMethodName0, const RuntimeMethod* method);
// System.Boolean UnityEngine.Networking.UnityWebRequest::get_isNetworkError()
extern "C" IL2CPP_METHOD_ATTR bool UnityWebRequest_get_isNetworkError_m082AFE1A58A330AC4CBD179606B61CB39DD44588 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// System.Boolean UnityEngine.Networking.UnityWebRequest::get_isHttpError()
extern "C" IL2CPP_METHOD_ATTR bool UnityWebRequest_get_isHttpError_m8F636B70C239EC848FACC83189DE0C22CADEC1C3 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// System.Int64 UnityEngine.Networking.UnityWebRequest::get_responseCode()
extern "C" IL2CPP_METHOD_ATTR int64_t UnityWebRequest_get_responseCode_m34819872549939D1EF9EA3D4010974FBEBAF0070 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// System.String UnityEngine.Networking.UnityWebRequest::GetHTTPStatusString(System.Int64)
extern "C" IL2CPP_METHOD_ATTR String_t* UnityWebRequest_GetHTTPStatusString_m370515E94B5B3C14B4A49677A31D8494262D7EDA (int64_t ___responseCode0, const RuntimeMethod* method);
// UnityEngine.Networking.UnityWebRequest/UnityWebRequestError UnityEngine.Networking.UnityWebRequest::GetError()
extern "C" IL2CPP_METHOD_ATTR int32_t UnityWebRequest_GetError_m55BF2299E3B195AC416CCCB46C3DBD83C075018C (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method);
// System.String UnityEngineInternal.WebRequestUtils::MakeInitialUrl(System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR String_t* WebRequestUtils_MakeInitialUrl_m446CCE4EFB276BE27A9380D55B9E704D01107B83 (String_t* ___targetUrl0, String_t* ___localUrl1, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::InternalSetUrl(System.String)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_InternalSetUrl_m2E2C837A6F32065CAAAF6EFA7D0237C9E206689A (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___url0, const RuntimeMethod* method);
// System.Boolean System.Uri::get_IsAbsoluteUri()
extern "C" IL2CPP_METHOD_ATTR bool Uri_get_IsAbsoluteUri_m8C189085F1C675DBC3148AA70C38074EC075D722 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.String System.Uri::get_OriginalString()
extern "C" IL2CPP_METHOD_ATTR String_t* Uri_get_OriginalString_m56099E46276F0A52524347F1F46A2F88E948504F (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.String UnityEngineInternal.WebRequestUtils::MakeUriString(System.Uri,System.String,System.Boolean)
extern "C" IL2CPP_METHOD_ATTR String_t* WebRequestUtils_MakeUriString_m5693EA04230335B9611278EFC189BD58339D01E4 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___targetUri0, String_t* ___targetUrl1, bool ___prependProtocol2, const RuntimeMethod* method);
// UnityEngine.Networking.UnityWebRequest/UnityWebRequestError UnityEngine.Networking.UnityWebRequest::SetUrl(System.String)
extern "C" IL2CPP_METHOD_ATTR int32_t UnityWebRequest_SetUrl_mED007912E89AA114D1A3D6905586116F74C8D774 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___url0, const RuntimeMethod* method);
// UnityEngine.Networking.UnityWebRequest/UnityWebRequestError UnityEngine.Networking.UnityWebRequest::InternalSetRequestHeader(System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR int32_t UnityWebRequest_InternalSetRequestHeader_m7481D7E49B6E6078598E40B81D1A3DA9B8D2BD10 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___name0, String_t* ___value1, const RuntimeMethod* method);
// UnityEngine.Networking.UnityWebRequest/UnityWebRequestError UnityEngine.Networking.UnityWebRequest::SetUploadHandler(UnityEngine.Networking.UploadHandler)
extern "C" IL2CPP_METHOD_ATTR int32_t UnityWebRequest_SetUploadHandler_m046EF4089035441F661AED13F703024DEE030525 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * ___uh0, const RuntimeMethod* method);
// UnityEngine.Networking.UnityWebRequest/UnityWebRequestError UnityEngine.Networking.UnityWebRequest::SetDownloadHandler(UnityEngine.Networking.DownloadHandler)
extern "C" IL2CPP_METHOD_ATTR int32_t UnityWebRequest_SetDownloadHandler_mDE4E6137C34A90754C41B3A0B7B303135771EEDD (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * ___dh0, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.DownloadHandlerBuffer::.ctor()
extern "C" IL2CPP_METHOD_ATTR void DownloadHandlerBuffer__ctor_m2134187D8FB07FBAEA2CE23BFCEB13FD94261A9A (DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::.ctor(System.String,System.String,UnityEngine.Networking.DownloadHandler,UnityEngine.Networking.UploadHandler)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest__ctor_m0D2F8F3E1202EF4256D17E91B95DB6CC673FC8D6 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___url0, String_t* ___method1, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * ___downloadHandler2, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * ___uploadHandler3, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UnityWebRequest::.ctor(System.Uri,System.String,UnityEngine.Networking.DownloadHandler,UnityEngine.Networking.UploadHandler)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest__ctor_mE0CF781DACE7D8A434D8CDEF1AA1B2C2B21AB4BF (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___uri0, String_t* ___method1, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * ___downloadHandler2, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * ___uploadHandler3, const RuntimeMethod* method);
// System.Void UnityEngine.AsyncOperation::.ctor()
extern "C" IL2CPP_METHOD_ATTR void AsyncOperation__ctor_mEEE6114B72B8807F4AA6FF48FA79E4EFE480293F (AsyncOperation_t304C51ABED8AE734CC8DDDFE13013D8D5A44641D * __this, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UploadHandler::Release()
extern "C" IL2CPP_METHOD_ATTR void UploadHandler_Release_m1723A22438AF0A7BE616D512E54190D9CE0EC3C4 (UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * __this, const RuntimeMethod* method);
// System.Void UnityEngine.Networking.UploadHandler::.ctor()
extern "C" IL2CPP_METHOD_ATTR void UploadHandler__ctor_m3F76154710C5CB7099388479FA02E6555D077F6E (UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * __this, const RuntimeMethod* method);
// System.IntPtr UnityEngine.Networking.UploadHandlerRaw::Create(UnityEngine.Networking.UploadHandlerRaw,System.Byte[])
extern "C" IL2CPP_METHOD_ATTR intptr_t UploadHandlerRaw_Create_m921D80A8952FC740F358E5FD28E6D5A70622687B (UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27 * ___self0, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___data1, const RuntimeMethod* method);
// System.Text.Encoding System.Text.Encoding::get_ASCII()
extern "C" IL2CPP_METHOD_ATTR Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * Encoding_get_ASCII_m9B673AE3152AB04D07CADE6E5E142C785B5BC94E (const RuntimeMethod* method);
// System.Byte[] UnityEngine.WWWTranscoder::Decode(System.Byte[],System.Byte,System.Byte[])
extern "C" IL2CPP_METHOD_ATTR ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* WWWTranscoder_Decode_m2533830DAAAE6F33AA6EE85A5BF63C96F5D631D4 (ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___input0, uint8_t ___escapeChar1, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___space2, const RuntimeMethod* method);
// System.Void System.IO.MemoryStream::.ctor(System.Int32)
extern "C" IL2CPP_METHOD_ATTR void MemoryStream__ctor_m78689C82DED9ACE5022B7EABF28F17FF318DF2AA (MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C * __this, int32_t p0, const RuntimeMethod* method);
// System.Boolean UnityEngine.WWWTranscoder::ByteSubArrayEquals(System.Byte[],System.Int32,System.Byte[])
extern "C" IL2CPP_METHOD_ATTR bool WWWTranscoder_ByteSubArrayEquals_m268C2A9B31CCF4D81E7BEEF843DF5D477ECA9958 (ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___array0, int32_t ___index1, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___comperand2, const RuntimeMethod* method);
// System.Byte UnityEngine.WWWTranscoder::Hex2Byte(System.Byte[],System.Int32)
extern "C" IL2CPP_METHOD_ATTR uint8_t WWWTranscoder_Hex2Byte_mD417CA540CFBE045FCE32959CD3443EB9C8C7423 (ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___b0, int32_t ___offset1, const RuntimeMethod* method);
// System.Text.Encoding UnityEngine.WWWForm::get_DefaultEncoding()
extern "C" IL2CPP_METHOD_ATTR Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * WWWForm_get_DefaultEncoding_m13BB72339201269AB257B275B20A5A35B233BC3F (const RuntimeMethod* method);
// System.Char System.String::get_Chars(System.Int32)
extern "C" IL2CPP_METHOD_ATTR Il2CppChar String_get_Chars_m14308AC3B95F8C1D9F1D1055B116B37D595F1D96 (String_t* __this, int32_t p0, const RuntimeMethod* method);
// System.Void System.Uri::.ctor(System.String,System.UriKind)
extern "C" IL2CPP_METHOD_ATTR void Uri__ctor_mA02DB222F4F35380DE2700D84F58EB42497FDDE4 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, String_t* p0, int32_t p1, const RuntimeMethod* method);
// System.String System.Uri::get_AbsoluteUri()
extern "C" IL2CPP_METHOD_ATTR String_t* Uri_get_AbsoluteUri_m4326730E572E7E3874021E802813EB6F49F7F99E (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.Void System.Uri::.ctor(System.Uri,System.Uri)
extern "C" IL2CPP_METHOD_ATTR void Uri__ctor_m42192656437FBEF1EEA8724D3EF2BB67DA0ED6BF (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * p0, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * p1, const RuntimeMethod* method);
// System.Void System.Uri::.ctor(System.String)
extern "C" IL2CPP_METHOD_ATTR void Uri__ctor_mBA69907A1D799CD12ED44B611985B25FE4C626A2 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, String_t* p0, const RuntimeMethod* method);
// System.Void System.Uri::.ctor(System.Uri,System.String)
extern "C" IL2CPP_METHOD_ATTR void Uri__ctor_m41A759BF295FB902084DD289849793E01A65A14E (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * p0, String_t* p1, const RuntimeMethod* method);
// System.Boolean System.Uri::op_Equality(System.Uri,System.Uri)
extern "C" IL2CPP_METHOD_ATTR bool Uri_op_Equality_mFED3D4AFAB090B76D2088C485507F8F702ADA18F (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * p0, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * p1, const RuntimeMethod* method);
// System.Boolean System.Text.RegularExpressions.Regex::IsMatch(System.String)
extern "C" IL2CPP_METHOD_ATTR bool Regex_IsMatch_m79684C4D2CE6C5495BCCE9A32AC029E1E5950B7C (Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF * __this, String_t* p0, const RuntimeMethod* method);
// System.String System.Uri::get_Scheme()
extern "C" IL2CPP_METHOD_ATTR String_t* Uri_get_Scheme_m14A8F0018D8AACADBEF39600A59944F33EE39187 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.String System.String::Concat(System.String,System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR String_t* String_Concat_mF4626905368D6558695A823466A1AF65EADB9923 (String_t* p0, String_t* p1, String_t* p2, const RuntimeMethod* method);
// System.Boolean System.Uri::get_IsFile()
extern "C" IL2CPP_METHOD_ATTR bool Uri_get_IsFile_m06AB5A15E2A34BBC5177C6E902C5C9D7E766A213 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.Boolean System.Uri::get_IsLoopback()
extern "C" IL2CPP_METHOD_ATTR bool Uri_get_IsLoopback_mCD7E1228C8296730CBD31C713B0A81B660D99BC4 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.String System.Uri::get_AbsolutePath()
extern "C" IL2CPP_METHOD_ATTR String_t* Uri_get_AbsolutePath_mA9A825E2BBD0A43AD76EB9A9765E29E45FE32F31 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.Boolean System.String::Contains(System.String)
extern "C" IL2CPP_METHOD_ATTR bool String_Contains_m4488034AF8CB3EEA9A205EB8A1F25D438FF8704B (String_t* __this, String_t* p0, const RuntimeMethod* method);
// System.String UnityEngineInternal.WebRequestUtils::URLDecode(System.String)
extern "C" IL2CPP_METHOD_ATTR String_t* WebRequestUtils_URLDecode_m3F75FA29F50FB340B93815988517E9208C52EE62 (String_t* ___encoded0, const RuntimeMethod* method);
// System.Int32 System.String::get_Length()
extern "C" IL2CPP_METHOD_ATTR int32_t String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018 (String_t* __this, const RuntimeMethod* method);
// System.String System.String::Concat(System.Object,System.Object)
extern "C" IL2CPP_METHOD_ATTR String_t* String_Concat_mBB19C73816BDD1C3519F248E1ADC8E11A6FDB495 (RuntimeObject * p0, RuntimeObject * p1, const RuntimeMethod* method);
// System.String System.String::Concat(System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR String_t* String_Concat_mB78D0094592718DA6D5DB6C712A9C225631666BE (String_t* p0, String_t* p1, const RuntimeMethod* method);
// System.Void System.Text.StringBuilder::.ctor(System.String,System.Int32)
extern "C" IL2CPP_METHOD_ATTR void StringBuilder__ctor_m786CAFE74FE0D479747A0D474BE6EBCFDA5743EA (StringBuilder_t * __this, String_t* p0, int32_t p1, const RuntimeMethod* method);
// System.Text.StringBuilder System.Text.StringBuilder::Append(System.Char)
extern "C" IL2CPP_METHOD_ATTR StringBuilder_t * StringBuilder_Append_m05C12F58ADC2D807613A9301DF438CB3CD09B75A (StringBuilder_t * __this, Il2CppChar p0, const RuntimeMethod* method);
// System.Boolean System.String::StartsWith(System.String)
extern "C" IL2CPP_METHOD_ATTR bool String_StartsWith_m7D468FB7C801D9C2DBEEEEC86F8BA8F4EC3243C1 (String_t* __this, String_t* p0, const RuntimeMethod* method);
// System.Text.StringBuilder System.Text.StringBuilder::Append(System.String)
extern "C" IL2CPP_METHOD_ATTR StringBuilder_t * StringBuilder_Append_mDBB8CCBB7750C67BE2F2D92F47E6C0FA42793260 (StringBuilder_t * __this, String_t* p0, const RuntimeMethod* method);
// System.String System.Uri::get_PathAndQuery()
extern "C" IL2CPP_METHOD_ATTR String_t* Uri_get_PathAndQuery_mF079BA04B7A397B2729E5B5DEE72B3654A44E384 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.String System.Uri::get_Fragment()
extern "C" IL2CPP_METHOD_ATTR String_t* Uri_get_Fragment_m111666DD668AC59B9F3C3D3CEEEC7F70F6904D41 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.Byte[] UnityEngine.WWWTranscoder::URLDecode(System.Byte[])
extern "C" IL2CPP_METHOD_ATTR ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* WWWTranscoder_URLDecode_m591A567154B1B8737ECBFE065AF4FCA59217F5D8 (ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___toEncode0, const RuntimeMethod* method);
// System.Void System.Text.RegularExpressions.Regex::.ctor(System.String)
extern "C" IL2CPP_METHOD_ATTR void Regex__ctor_m2769A5BA7B7A835514F6C0E4D30FAD467C6B1B0C (Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF * __this, String_t* p0, const RuntimeMethod* method);
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Conversion methods for marshalling of: UnityEngine.Networking.CertificateHandler
extern "C" void CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshal_pinvoke(const CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0& unmarshaled, CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_pinvoke& marshaled)
{
	marshaled.___m_Ptr_0 = unmarshaled.get_m_Ptr_0();
}
extern "C" void CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshal_pinvoke_back(const CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_pinvoke& marshaled, CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0& unmarshaled)
{
	intptr_t unmarshaled_m_Ptr_temp_0;
	memset(&unmarshaled_m_Ptr_temp_0, 0, sizeof(unmarshaled_m_Ptr_temp_0));
	unmarshaled_m_Ptr_temp_0 = marshaled.___m_Ptr_0;
	unmarshaled.set_m_Ptr_0(unmarshaled_m_Ptr_temp_0);
}
// Conversion method for clean up from marshalling of: UnityEngine.Networking.CertificateHandler
extern "C" void CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshal_pinvoke_cleanup(CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_pinvoke& marshaled)
{
}
// Conversion methods for marshalling of: UnityEngine.Networking.CertificateHandler
extern "C" void CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshal_com(const CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0& unmarshaled, CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_com& marshaled)
{
	marshaled.___m_Ptr_0 = unmarshaled.get_m_Ptr_0();
}
extern "C" void CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshal_com_back(const CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_com& marshaled, CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0& unmarshaled)
{
	intptr_t unmarshaled_m_Ptr_temp_0;
	memset(&unmarshaled_m_Ptr_temp_0, 0, sizeof(unmarshaled_m_Ptr_temp_0));
	unmarshaled_m_Ptr_temp_0 = marshaled.___m_Ptr_0;
	unmarshaled.set_m_Ptr_0(unmarshaled_m_Ptr_temp_0);
}
// Conversion method for clean up from marshalling of: UnityEngine.Networking.CertificateHandler
extern "C" void CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshal_com_cleanup(CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0_marshaled_com& marshaled)
{
}
// System.Void UnityEngine.Networking.CertificateHandler::Release()
extern "C" IL2CPP_METHOD_ATTR void CertificateHandler_Release_m8D680D11AF8B070587DA5C73E2AE652208BDA90A (CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * __this, const RuntimeMethod* method)
{
	typedef void (*CertificateHandler_Release_m8D680D11AF8B070587DA5C73E2AE652208BDA90A_ftn) (CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 *);
	static CertificateHandler_Release_m8D680D11AF8B070587DA5C73E2AE652208BDA90A_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (CertificateHandler_Release_m8D680D11AF8B070587DA5C73E2AE652208BDA90A_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.CertificateHandler::Release()");
	_il2cpp_icall_func(__this);
}
// System.Void UnityEngine.Networking.CertificateHandler::Finalize()
extern "C" IL2CPP_METHOD_ATTR void CertificateHandler_Finalize_m897F6342A2C8D1AC7AA32B6B12E3C961844BF9ED (CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * __this, const RuntimeMethod* method)
{
	Exception_t * __last_unhandled_exception = 0;
	NO_UNUSED_WARNING (__last_unhandled_exception);
	Exception_t * __exception_local = 0;
	NO_UNUSED_WARNING (__exception_local);
	int32_t __leave_target = -1;
	NO_UNUSED_WARNING (__leave_target);
	{
	}

IL_0001:
	try
	{ // begin try (depth: 1)
		CertificateHandler_Dispose_m9C71BAA51760FDF05AB999B6AB6E6BC71BCB8CA0(__this, /*hidden argument*/NULL);
		IL2CPP_LEAVE(0x13, FINALLY_000c);
	} // end try (depth: 1)
	catch(Il2CppExceptionWrapper& e)
	{
		__last_unhandled_exception = (Exception_t *)e.ex;
		goto FINALLY_000c;
	}

FINALLY_000c:
	{ // begin finally (depth: 1)
		Object_Finalize_m4015B7D3A44DE125C5FE34D7276CD4697C06F380(__this, /*hidden argument*/NULL);
		IL2CPP_RESET_LEAVE(0x13);
		IL2CPP_END_FINALLY(12)
	} // end finally (depth: 1)
	IL2CPP_CLEANUP(12)
	{
		IL2CPP_JUMP_TBL(0x13, IL_0013)
		IL2CPP_RETHROW_IF_UNHANDLED(Exception_t *)
	}

IL_0013:
	{
		return;
	}
}
// System.Boolean UnityEngine.Networking.CertificateHandler::ValidateCertificate(System.Byte[])
extern "C" IL2CPP_METHOD_ATTR bool CertificateHandler_ValidateCertificate_m10584FA8D39D238AA435AB440279D3943273817D (CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * __this, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___certificateData0, const RuntimeMethod* method)
{
	bool V_0 = false;
	{
		V_0 = (bool)0;
		goto IL_0008;
	}

IL_0008:
	{
		bool L_0 = V_0;
		return L_0;
	}
}
// System.Boolean UnityEngine.Networking.CertificateHandler::ValidateCertificateNative(System.Byte[])
extern "C" IL2CPP_METHOD_ATTR bool CertificateHandler_ValidateCertificateNative_mE500FAB5B59229D61E85A5DC0E28A0F583679170 (CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * __this, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___certificateData0, const RuntimeMethod* method)
{
	bool V_0 = false;
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_0 = ___certificateData0;
		bool L_1 = VirtFuncInvoker1< bool, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* >::Invoke(5 /* System.Boolean UnityEngine.Networking.CertificateHandler::ValidateCertificate(System.Byte[]) */, __this, L_0);
		V_0 = L_1;
		goto IL_000e;
	}

IL_000e:
	{
		bool L_2 = V_0;
		return L_2;
	}
}
// System.Void UnityEngine.Networking.CertificateHandler::Dispose()
extern "C" IL2CPP_METHOD_ATTR void CertificateHandler_Dispose_m9C71BAA51760FDF05AB999B6AB6E6BC71BCB8CA0 (CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (CertificateHandler_Dispose_m9C71BAA51760FDF05AB999B6AB6E6BC71BCB8CA0_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		intptr_t L_0 = __this->get_m_Ptr_0();
		bool L_1 = IntPtr_op_Inequality_mB4886A806009EA825EFCC60CD2A7F6EB8E273A61((intptr_t)L_0, (intptr_t)(0), /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_0029;
		}
	}
	{
		CertificateHandler_Release_m8D680D11AF8B070587DA5C73E2AE652208BDA90A(__this, /*hidden argument*/NULL);
		__this->set_m_Ptr_0((intptr_t)(0));
	}

IL_0029:
	{
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Conversion methods for marshalling of: UnityEngine.Networking.DownloadHandler
extern "C" void DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshal_pinvoke(const DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9& unmarshaled, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_pinvoke& marshaled)
{
	marshaled.___m_Ptr_0 = unmarshaled.get_m_Ptr_0();
}
extern "C" void DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshal_pinvoke_back(const DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_pinvoke& marshaled, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9& unmarshaled)
{
	intptr_t unmarshaled_m_Ptr_temp_0;
	memset(&unmarshaled_m_Ptr_temp_0, 0, sizeof(unmarshaled_m_Ptr_temp_0));
	unmarshaled_m_Ptr_temp_0 = marshaled.___m_Ptr_0;
	unmarshaled.set_m_Ptr_0(unmarshaled_m_Ptr_temp_0);
}
// Conversion method for clean up from marshalling of: UnityEngine.Networking.DownloadHandler
extern "C" void DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshal_pinvoke_cleanup(DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_pinvoke& marshaled)
{
}
// Conversion methods for marshalling of: UnityEngine.Networking.DownloadHandler
extern "C" void DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshal_com(const DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9& unmarshaled, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_com& marshaled)
{
	marshaled.___m_Ptr_0 = unmarshaled.get_m_Ptr_0();
}
extern "C" void DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshal_com_back(const DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_com& marshaled, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9& unmarshaled)
{
	intptr_t unmarshaled_m_Ptr_temp_0;
	memset(&unmarshaled_m_Ptr_temp_0, 0, sizeof(unmarshaled_m_Ptr_temp_0));
	unmarshaled_m_Ptr_temp_0 = marshaled.___m_Ptr_0;
	unmarshaled.set_m_Ptr_0(unmarshaled_m_Ptr_temp_0);
}
// Conversion method for clean up from marshalling of: UnityEngine.Networking.DownloadHandler
extern "C" void DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshal_com_cleanup(DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9_marshaled_com& marshaled)
{
}
// System.Void UnityEngine.Networking.DownloadHandler::.ctor()
extern "C" IL2CPP_METHOD_ATTR void DownloadHandler__ctor_m39F80F1C9B379B0D0362DF9264DE42604BDB24E0 (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * __this, const RuntimeMethod* method)
{
	{
		Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0(__this, /*hidden argument*/NULL);
		return;
	}
}
// System.Void UnityEngine.Networking.DownloadHandler::Release()
extern "C" IL2CPP_METHOD_ATTR void DownloadHandler_Release_m913DA503E4183F3323A3D0121FFC978D0F220D5D (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * __this, const RuntimeMethod* method)
{
	typedef void (*DownloadHandler_Release_m913DA503E4183F3323A3D0121FFC978D0F220D5D_ftn) (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 *);
	static DownloadHandler_Release_m913DA503E4183F3323A3D0121FFC978D0F220D5D_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (DownloadHandler_Release_m913DA503E4183F3323A3D0121FFC978D0F220D5D_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.DownloadHandler::Release()");
	_il2cpp_icall_func(__this);
}
// System.Void UnityEngine.Networking.DownloadHandler::Finalize()
extern "C" IL2CPP_METHOD_ATTR void DownloadHandler_Finalize_mC6CBFA6D7B38827B12D64D265D5D4FB6B57D50CA (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * __this, const RuntimeMethod* method)
{
	Exception_t * __last_unhandled_exception = 0;
	NO_UNUSED_WARNING (__last_unhandled_exception);
	Exception_t * __exception_local = 0;
	NO_UNUSED_WARNING (__exception_local);
	int32_t __leave_target = -1;
	NO_UNUSED_WARNING (__leave_target);
	{
	}

IL_0001:
	try
	{ // begin try (depth: 1)
		DownloadHandler_Dispose_m7478E72B2DBA4B55FAA25F7A1975A13BA5891D4B(__this, /*hidden argument*/NULL);
		IL2CPP_LEAVE(0x13, FINALLY_000c);
	} // end try (depth: 1)
	catch(Il2CppExceptionWrapper& e)
	{
		__last_unhandled_exception = (Exception_t *)e.ex;
		goto FINALLY_000c;
	}

FINALLY_000c:
	{ // begin finally (depth: 1)
		Object_Finalize_m4015B7D3A44DE125C5FE34D7276CD4697C06F380(__this, /*hidden argument*/NULL);
		IL2CPP_RESET_LEAVE(0x13);
		IL2CPP_END_FINALLY(12)
	} // end finally (depth: 1)
	IL2CPP_CLEANUP(12)
	{
		IL2CPP_JUMP_TBL(0x13, IL_0013)
		IL2CPP_RETHROW_IF_UNHANDLED(Exception_t *)
	}

IL_0013:
	{
		return;
	}
}
// System.Void UnityEngine.Networking.DownloadHandler::Dispose()
extern "C" IL2CPP_METHOD_ATTR void DownloadHandler_Dispose_m7478E72B2DBA4B55FAA25F7A1975A13BA5891D4B (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (DownloadHandler_Dispose_m7478E72B2DBA4B55FAA25F7A1975A13BA5891D4B_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		intptr_t L_0 = __this->get_m_Ptr_0();
		bool L_1 = IntPtr_op_Inequality_mB4886A806009EA825EFCC60CD2A7F6EB8E273A61((intptr_t)L_0, (intptr_t)(0), /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_0029;
		}
	}
	{
		DownloadHandler_Release_m913DA503E4183F3323A3D0121FFC978D0F220D5D(__this, /*hidden argument*/NULL);
		__this->set_m_Ptr_0((intptr_t)(0));
	}

IL_0029:
	{
		return;
	}
}
// System.Byte[] UnityEngine.Networking.DownloadHandler::get_data()
extern "C" IL2CPP_METHOD_ATTR ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* DownloadHandler_get_data_m4AE4E3764FBE186ABA89B5F3A7F91048EE5E38FB (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * __this, const RuntimeMethod* method)
{
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* V_0 = NULL;
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_0 = VirtFuncInvoker0< ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* >::Invoke(5 /* System.Byte[] UnityEngine.Networking.DownloadHandler::GetData() */, __this);
		V_0 = L_0;
		goto IL_000d;
	}

IL_000d:
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_1 = V_0;
		return L_1;
	}
}
// System.String UnityEngine.Networking.DownloadHandler::get_text()
extern "C" IL2CPP_METHOD_ATTR String_t* DownloadHandler_get_text_m1D707E375899B4F4F0434B79AB8D57ADFE5424FF (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * __this, const RuntimeMethod* method)
{
	String_t* V_0 = NULL;
	{
		String_t* L_0 = VirtFuncInvoker0< String_t* >::Invoke(6 /* System.String UnityEngine.Networking.DownloadHandler::GetText() */, __this);
		V_0 = L_0;
		goto IL_000d;
	}

IL_000d:
	{
		String_t* L_1 = V_0;
		return L_1;
	}
}
// System.Byte[] UnityEngine.Networking.DownloadHandler::GetData()
extern "C" IL2CPP_METHOD_ATTR ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* DownloadHandler_GetData_m684807DC14346A128E64E455E8DD147C32125E04 (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * __this, const RuntimeMethod* method)
{
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* V_0 = NULL;
	{
		V_0 = (ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821*)NULL;
		goto IL_0008;
	}

IL_0008:
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_0 = V_0;
		return L_0;
	}
}
// System.String UnityEngine.Networking.DownloadHandler::GetText()
extern "C" IL2CPP_METHOD_ATTR String_t* DownloadHandler_GetText_mA51553E65D6A397E07AAAC21214C817AD72550FD (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (DownloadHandler_GetText_mA51553E65D6A397E07AAAC21214C817AD72550FD_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* V_0 = NULL;
	String_t* V_1 = NULL;
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_0 = VirtFuncInvoker0< ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* >::Invoke(5 /* System.Byte[] UnityEngine.Networking.DownloadHandler::GetData() */, __this);
		V_0 = L_0;
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_1 = V_0;
		if (!L_1)
		{
			goto IL_002e;
		}
	}
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_2 = V_0;
		NullCheck(L_2);
		if ((((int32_t)(((int32_t)((int32_t)(((RuntimeArray *)L_2)->max_length))))) <= ((int32_t)0)))
		{
			goto IL_002e;
		}
	}
	{
		Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_3 = DownloadHandler_GetTextEncoder_m601540FD9D16122709582833632A9DEEDBF07E64(__this, /*hidden argument*/NULL);
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_4 = V_0;
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_5 = V_0;
		NullCheck(L_5);
		NullCheck(L_3);
		String_t* L_6 = VirtFuncInvoker3< String_t*, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821*, int32_t, int32_t >::Invoke(35 /* System.String System.Text.Encoding::GetString(System.Byte[],System.Int32,System.Int32) */, L_3, L_4, 0, (((int32_t)((int32_t)(((RuntimeArray *)L_5)->max_length)))));
		V_1 = L_6;
		goto IL_003a;
	}

IL_002e:
	{
		V_1 = _stringLiteralDA39A3EE5E6B4B0D3255BFEF95601890AFD80709;
		goto IL_003a;
	}

IL_003a:
	{
		String_t* L_7 = V_1;
		return L_7;
	}
}
// System.Text.Encoding UnityEngine.Networking.DownloadHandler::GetTextEncoder()
extern "C" IL2CPP_METHOD_ATTR Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * DownloadHandler_GetTextEncoder_m601540FD9D16122709582833632A9DEEDBF07E64 (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (DownloadHandler_GetTextEncoder_m601540FD9D16122709582833632A9DEEDBF07E64_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	String_t* V_0 = NULL;
	int32_t V_1 = 0;
	int32_t V_2 = 0;
	String_t* V_3 = NULL;
	int32_t V_4 = 0;
	Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * V_5 = NULL;
	ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 * V_6 = NULL;
	Exception_t * __last_unhandled_exception = 0;
	NO_UNUSED_WARNING (__last_unhandled_exception);
	Exception_t * __exception_local = 0;
	NO_UNUSED_WARNING (__exception_local);
	int32_t __leave_target = -1;
	NO_UNUSED_WARNING (__leave_target);
	{
		String_t* L_0 = DownloadHandler_GetContentType_mB1653D4D9CA539D1D622C32B52DF5C38548D30E8(__this, /*hidden argument*/NULL);
		V_0 = L_0;
		String_t* L_1 = V_0;
		bool L_2 = String_IsNullOrEmpty_m06A85A206AC2106D1982826C5665B9BD35324229(L_1, /*hidden argument*/NULL);
		if (L_2)
		{
			goto IL_00b1;
		}
	}
	{
		String_t* L_3 = V_0;
		NullCheck(L_3);
		int32_t L_4 = String_IndexOf_mF9EA8429E9D1B7475D5A297E67435CF34E965F28(L_3, _stringLiteralDCB16D9AACB079FE42FBDE349C3319DE8033DDD1, 5, /*hidden argument*/NULL);
		V_1 = L_4;
		int32_t L_5 = V_1;
		if ((((int32_t)L_5) <= ((int32_t)(-1))))
		{
			goto IL_00b0;
		}
	}
	{
		String_t* L_6 = V_0;
		int32_t L_7 = V_1;
		NullCheck(L_6);
		int32_t L_8 = String_IndexOf_m66F6178DB4B2F61F4FAFD8B75787D0AB142ADD7D(L_6, ((int32_t)61), L_7, /*hidden argument*/NULL);
		V_2 = L_8;
		int32_t L_9 = V_2;
		if ((((int32_t)L_9) <= ((int32_t)(-1))))
		{
			goto IL_00af;
		}
	}
	{
		String_t* L_10 = V_0;
		int32_t L_11 = V_2;
		NullCheck(L_10);
		String_t* L_12 = String_Substring_m2C4AFF5E79DD8BADFD2DFBCF156BF728FBB8E1AE(L_10, ((int32_t)il2cpp_codegen_add((int32_t)L_11, (int32_t)1)), /*hidden argument*/NULL);
		NullCheck(L_12);
		String_t* L_13 = String_Trim_mB52EB7876C7132358B76B7DC95DEACA20722EF4D(L_12, /*hidden argument*/NULL);
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_14 = (CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2*)SZArrayNew(CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2_il2cpp_TypeInfo_var, (uint32_t)2);
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_15 = L_14;
		NullCheck(L_15);
		(L_15)->SetAt(static_cast<il2cpp_array_size_t>(0), (Il2CppChar)((int32_t)39));
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_16 = L_15;
		NullCheck(L_16);
		(L_16)->SetAt(static_cast<il2cpp_array_size_t>(1), (Il2CppChar)((int32_t)34));
		NullCheck(L_13);
		String_t* L_17 = String_Trim_m788DE5AEFDAC40E778745C4DF4AFD45A4BC1007E(L_13, L_16, /*hidden argument*/NULL);
		NullCheck(L_17);
		String_t* L_18 = String_Trim_mB52EB7876C7132358B76B7DC95DEACA20722EF4D(L_17, /*hidden argument*/NULL);
		V_3 = L_18;
		String_t* L_19 = V_3;
		NullCheck(L_19);
		int32_t L_20 = String_IndexOf_m2909B8CF585E1BD0C81E11ACA2F48012156FD5BD(L_19, ((int32_t)59), /*hidden argument*/NULL);
		V_4 = L_20;
		int32_t L_21 = V_4;
		if ((((int32_t)L_21) <= ((int32_t)(-1))))
		{
			goto IL_0080;
		}
	}
	{
		String_t* L_22 = V_3;
		int32_t L_23 = V_4;
		NullCheck(L_22);
		String_t* L_24 = String_Substring_mB593C0A320C683E6E47EFFC0A12B7A465E5E43BB(L_22, 0, L_23, /*hidden argument*/NULL);
		V_3 = L_24;
	}

IL_0080:
	try
	{ // begin try (depth: 1)
		String_t* L_25 = V_3;
		Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_26 = Encoding_GetEncoding_mA19D07F2E88F8FF58D42B73AFF5E22241607D54E(L_25, /*hidden argument*/NULL);
		V_5 = L_26;
		goto IL_00bd;
	} // end try (depth: 1)
	catch(Il2CppExceptionWrapper& e)
	{
		__exception_local = (Exception_t *)e.ex;
		if(il2cpp_codegen_class_is_assignable_from (ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1_il2cpp_TypeInfo_var, il2cpp_codegen_object_class(e.ex)))
			goto CATCH_008e;
		throw e;
	}

CATCH_008e:
	{ // begin catch(System.ArgumentException)
		V_6 = ((ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 *)__exception_local);
		String_t* L_27 = V_3;
		ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 * L_28 = V_6;
		NullCheck(L_28);
		String_t* L_29 = VirtFuncInvoker0< String_t* >::Invoke(5 /* System.String System.Exception::get_Message() */, L_28);
		String_t* L_30 = String_Format_m19325298DBC61AAC016C16F7B3CF97A8A3DEA34A(_stringLiteral88DADF72F0A8F76B45A836CE12A3DC82857776DB, L_27, L_29, /*hidden argument*/NULL);
		IL2CPP_RUNTIME_CLASS_INIT(Debug_t7B5FCB117E2FD63B6838BC52821B252E2BFB61C4_il2cpp_TypeInfo_var);
		Debug_LogWarning_m37338644DC81F640CCDFEAE35A223F0E965F0568(L_30, /*hidden argument*/NULL);
		goto IL_00ae;
	} // end catch (depth: 1)

IL_00ae:
	{
	}

IL_00af:
	{
	}

IL_00b0:
	{
	}

IL_00b1:
	{
		Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_31 = Encoding_get_UTF8_m67C8652936B681E7BC7505E459E88790E0FF16D9(/*hidden argument*/NULL);
		V_5 = L_31;
		goto IL_00bd;
	}

IL_00bd:
	{
		Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_32 = V_5;
		return L_32;
	}
}
// System.String UnityEngine.Networking.DownloadHandler::GetContentType()
extern "C" IL2CPP_METHOD_ATTR String_t* DownloadHandler_GetContentType_mB1653D4D9CA539D1D622C32B52DF5C38548D30E8 (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * __this, const RuntimeMethod* method)
{
	typedef String_t* (*DownloadHandler_GetContentType_mB1653D4D9CA539D1D622C32B52DF5C38548D30E8_ftn) (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 *);
	static DownloadHandler_GetContentType_mB1653D4D9CA539D1D622C32B52DF5C38548D30E8_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (DownloadHandler_GetContentType_mB1653D4D9CA539D1D622C32B52DF5C38548D30E8_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.DownloadHandler::GetContentType()");
	String_t* retVal = _il2cpp_icall_func(__this);
	return retVal;
}
// System.Byte[] UnityEngine.Networking.DownloadHandler::InternalGetByteArray(UnityEngine.Networking.DownloadHandler)
extern "C" IL2CPP_METHOD_ATTR ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* DownloadHandler_InternalGetByteArray_mD6D13BFFBF2F56415E10FFEFDC4A68FE29D6D4FD (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * ___dh0, const RuntimeMethod* method)
{
	typedef ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* (*DownloadHandler_InternalGetByteArray_mD6D13BFFBF2F56415E10FFEFDC4A68FE29D6D4FD_ftn) (DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 *);
	static DownloadHandler_InternalGetByteArray_mD6D13BFFBF2F56415E10FFEFDC4A68FE29D6D4FD_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (DownloadHandler_InternalGetByteArray_mD6D13BFFBF2F56415E10FFEFDC4A68FE29D6D4FD_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.DownloadHandler::InternalGetByteArray(UnityEngine.Networking.DownloadHandler)");
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* retVal = _il2cpp_icall_func(___dh0);
	return retVal;
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Conversion methods for marshalling of: UnityEngine.Networking.DownloadHandlerBuffer
extern "C" void DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_marshal_pinvoke(const DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255& unmarshaled, DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_marshaled_pinvoke& marshaled)
{
	marshaled.___m_Ptr_0 = unmarshaled.get_m_Ptr_0();
}
extern "C" void DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_marshal_pinvoke_back(const DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_marshaled_pinvoke& marshaled, DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255& unmarshaled)
{
	intptr_t unmarshaled_m_Ptr_temp_0;
	memset(&unmarshaled_m_Ptr_temp_0, 0, sizeof(unmarshaled_m_Ptr_temp_0));
	unmarshaled_m_Ptr_temp_0 = marshaled.___m_Ptr_0;
	unmarshaled.set_m_Ptr_0(unmarshaled_m_Ptr_temp_0);
}
// Conversion method for clean up from marshalling of: UnityEngine.Networking.DownloadHandlerBuffer
extern "C" void DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_marshal_pinvoke_cleanup(DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_marshaled_pinvoke& marshaled)
{
}
// Conversion methods for marshalling of: UnityEngine.Networking.DownloadHandlerBuffer
extern "C" void DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_marshal_com(const DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255& unmarshaled, DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_marshaled_com& marshaled)
{
	marshaled.___m_Ptr_0 = unmarshaled.get_m_Ptr_0();
}
extern "C" void DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_marshal_com_back(const DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_marshaled_com& marshaled, DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255& unmarshaled)
{
	intptr_t unmarshaled_m_Ptr_temp_0;
	memset(&unmarshaled_m_Ptr_temp_0, 0, sizeof(unmarshaled_m_Ptr_temp_0));
	unmarshaled_m_Ptr_temp_0 = marshaled.___m_Ptr_0;
	unmarshaled.set_m_Ptr_0(unmarshaled_m_Ptr_temp_0);
}
// Conversion method for clean up from marshalling of: UnityEngine.Networking.DownloadHandlerBuffer
extern "C" void DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_marshal_com_cleanup(DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_marshaled_com& marshaled)
{
}
// System.Void UnityEngine.Networking.DownloadHandlerBuffer::.ctor()
extern "C" IL2CPP_METHOD_ATTR void DownloadHandlerBuffer__ctor_m2134187D8FB07FBAEA2CE23BFCEB13FD94261A9A (DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255 * __this, const RuntimeMethod* method)
{
	{
		DownloadHandler__ctor_m39F80F1C9B379B0D0362DF9264DE42604BDB24E0(__this, /*hidden argument*/NULL);
		DownloadHandlerBuffer_InternalCreateBuffer_m661B598DF8BD7BF86374FD84C52C8AEA8FA7BEF6(__this, /*hidden argument*/NULL);
		return;
	}
}
// System.IntPtr UnityEngine.Networking.DownloadHandlerBuffer::Create(UnityEngine.Networking.DownloadHandlerBuffer)
extern "C" IL2CPP_METHOD_ATTR intptr_t DownloadHandlerBuffer_Create_m39E26BEA64B617123CEF559999C8352CA9FA5137 (DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255 * ___obj0, const RuntimeMethod* method)
{
	typedef intptr_t (*DownloadHandlerBuffer_Create_m39E26BEA64B617123CEF559999C8352CA9FA5137_ftn) (DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255 *);
	static DownloadHandlerBuffer_Create_m39E26BEA64B617123CEF559999C8352CA9FA5137_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (DownloadHandlerBuffer_Create_m39E26BEA64B617123CEF559999C8352CA9FA5137_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.DownloadHandlerBuffer::Create(UnityEngine.Networking.DownloadHandlerBuffer)");
	intptr_t retVal = _il2cpp_icall_func(___obj0);
	return retVal;
}
// System.Void UnityEngine.Networking.DownloadHandlerBuffer::InternalCreateBuffer()
extern "C" IL2CPP_METHOD_ATTR void DownloadHandlerBuffer_InternalCreateBuffer_m661B598DF8BD7BF86374FD84C52C8AEA8FA7BEF6 (DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255 * __this, const RuntimeMethod* method)
{
	{
		intptr_t L_0 = DownloadHandlerBuffer_Create_m39E26BEA64B617123CEF559999C8352CA9FA5137(__this, /*hidden argument*/NULL);
		((DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 *)__this)->set_m_Ptr_0((intptr_t)L_0);
		return;
	}
}
// System.Byte[] UnityEngine.Networking.DownloadHandlerBuffer::GetData()
extern "C" IL2CPP_METHOD_ATTR ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* DownloadHandlerBuffer_GetData_m5A7FFA694EA35F1CE0731803F41E50BBDB16BF14 (DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255 * __this, const RuntimeMethod* method)
{
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* V_0 = NULL;
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_0 = DownloadHandlerBuffer_InternalGetData_m9266395B691394754B68543A2FF2F19566C5ABBF(__this, /*hidden argument*/NULL);
		V_0 = L_0;
		goto IL_000d;
	}

IL_000d:
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_1 = V_0;
		return L_1;
	}
}
// System.Byte[] UnityEngine.Networking.DownloadHandlerBuffer::InternalGetData()
extern "C" IL2CPP_METHOD_ATTR ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* DownloadHandlerBuffer_InternalGetData_m9266395B691394754B68543A2FF2F19566C5ABBF (DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255 * __this, const RuntimeMethod* method)
{
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* V_0 = NULL;
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_0 = DownloadHandler_InternalGetByteArray_mD6D13BFFBF2F56415E10FFEFDC4A68FE29D6D4FD(__this, /*hidden argument*/NULL);
		V_0 = L_0;
		goto IL_000d;
	}

IL_000d:
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_1 = V_0;
		return L_1;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif






// Conversion methods for marshalling of: UnityEngine.Networking.UnityWebRequest
extern "C" void UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshal_pinvoke(const UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129& unmarshaled, UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_pinvoke& marshaled)
{
	Exception_t* ___m_Uri_4Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field 'm_Uri' of type 'UnityWebRequest': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___m_Uri_4Exception, NULL, NULL);
}
extern "C" void UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshal_pinvoke_back(const UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_pinvoke& marshaled, UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129& unmarshaled)
{
	Exception_t* ___m_Uri_4Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field 'm_Uri' of type 'UnityWebRequest': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___m_Uri_4Exception, NULL, NULL);
}
// Conversion method for clean up from marshalling of: UnityEngine.Networking.UnityWebRequest
extern "C" void UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshal_pinvoke_cleanup(UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_pinvoke& marshaled)
{
}






// Conversion methods for marshalling of: UnityEngine.Networking.UnityWebRequest
extern "C" void UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshal_com(const UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129& unmarshaled, UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_com& marshaled)
{
	Exception_t* ___m_Uri_4Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field 'm_Uri' of type 'UnityWebRequest': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___m_Uri_4Exception, NULL, NULL);
}
extern "C" void UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshal_com_back(const UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_com& marshaled, UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129& unmarshaled)
{
	Exception_t* ___m_Uri_4Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field 'm_Uri' of type 'UnityWebRequest': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___m_Uri_4Exception, NULL, NULL);
}
// Conversion method for clean up from marshalling of: UnityEngine.Networking.UnityWebRequest
extern "C" void UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshal_com_cleanup(UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_marshaled_com& marshaled)
{
}
// System.Void UnityEngine.Networking.UnityWebRequest::.ctor(System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest__ctor_m3CBA159B3514D89C931002DFD333B9768A08EBFA (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___url0, String_t* ___method1, const RuntimeMethod* method)
{
	{
		Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0(__this, /*hidden argument*/NULL);
		intptr_t L_0 = UnityWebRequest_Create_m98363C34C71AA034B47FA64589711B6F0AEF6698(/*hidden argument*/NULL);
		__this->set_m_Ptr_0((intptr_t)L_0);
		UnityWebRequest_InternalSetDefaults_m644CC3C1C737838385F0EC9523A8930E696A9309(__this, /*hidden argument*/NULL);
		String_t* L_1 = ___url0;
		UnityWebRequest_set_url_mA698FD94C447FF7C1C429D50C2EBAEEDD473007D(__this, L_1, /*hidden argument*/NULL);
		String_t* L_2 = ___method1;
		UnityWebRequest_set_method_mF2DAC86EB05D65B9BCB52056B7CBB2C1AD87EEC6(__this, L_2, /*hidden argument*/NULL);
		return;
	}
}
// System.Void UnityEngine.Networking.UnityWebRequest::.ctor(System.String,System.String,UnityEngine.Networking.DownloadHandler,UnityEngine.Networking.UploadHandler)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest__ctor_m0D2F8F3E1202EF4256D17E91B95DB6CC673FC8D6 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___url0, String_t* ___method1, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * ___downloadHandler2, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * ___uploadHandler3, const RuntimeMethod* method)
{
	{
		Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0(__this, /*hidden argument*/NULL);
		intptr_t L_0 = UnityWebRequest_Create_m98363C34C71AA034B47FA64589711B6F0AEF6698(/*hidden argument*/NULL);
		__this->set_m_Ptr_0((intptr_t)L_0);
		UnityWebRequest_InternalSetDefaults_m644CC3C1C737838385F0EC9523A8930E696A9309(__this, /*hidden argument*/NULL);
		String_t* L_1 = ___url0;
		UnityWebRequest_set_url_mA698FD94C447FF7C1C429D50C2EBAEEDD473007D(__this, L_1, /*hidden argument*/NULL);
		String_t* L_2 = ___method1;
		UnityWebRequest_set_method_mF2DAC86EB05D65B9BCB52056B7CBB2C1AD87EEC6(__this, L_2, /*hidden argument*/NULL);
		DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * L_3 = ___downloadHandler2;
		UnityWebRequest_set_downloadHandler_mB481C2EE30F84B62396C1A837F46046F12B8FA7E(__this, L_3, /*hidden argument*/NULL);
		UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * L_4 = ___uploadHandler3;
		UnityWebRequest_set_uploadHandler_m7B33656584914FB3F6FB0FF73C08F671262724A1(__this, L_4, /*hidden argument*/NULL);
		return;
	}
}
// System.Void UnityEngine.Networking.UnityWebRequest::.ctor(System.Uri,System.String,UnityEngine.Networking.DownloadHandler,UnityEngine.Networking.UploadHandler)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest__ctor_mE0CF781DACE7D8A434D8CDEF1AA1B2C2B21AB4BF (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___uri0, String_t* ___method1, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * ___downloadHandler2, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * ___uploadHandler3, const RuntimeMethod* method)
{
	{
		Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0(__this, /*hidden argument*/NULL);
		intptr_t L_0 = UnityWebRequest_Create_m98363C34C71AA034B47FA64589711B6F0AEF6698(/*hidden argument*/NULL);
		__this->set_m_Ptr_0((intptr_t)L_0);
		UnityWebRequest_InternalSetDefaults_m644CC3C1C737838385F0EC9523A8930E696A9309(__this, /*hidden argument*/NULL);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_1 = ___uri0;
		UnityWebRequest_set_uri_mDE5DEDA2698190CD80AE3B4B7342400A32284124(__this, L_1, /*hidden argument*/NULL);
		String_t* L_2 = ___method1;
		UnityWebRequest_set_method_mF2DAC86EB05D65B9BCB52056B7CBB2C1AD87EEC6(__this, L_2, /*hidden argument*/NULL);
		DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * L_3 = ___downloadHandler2;
		UnityWebRequest_set_downloadHandler_mB481C2EE30F84B62396C1A837F46046F12B8FA7E(__this, L_3, /*hidden argument*/NULL);
		UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * L_4 = ___uploadHandler3;
		UnityWebRequest_set_uploadHandler_m7B33656584914FB3F6FB0FF73C08F671262724A1(__this, L_4, /*hidden argument*/NULL);
		return;
	}
}
// System.String UnityEngine.Networking.UnityWebRequest::GetWebErrorString(UnityEngine.Networking.UnityWebRequest_UnityWebRequestError)
extern "C" IL2CPP_METHOD_ATTR String_t* UnityWebRequest_GetWebErrorString_m92A1DDF2ADFFF8AEE6B1A7FAE384743C31F9E01D (int32_t ___err0, const RuntimeMethod* method)
{
	typedef String_t* (*UnityWebRequest_GetWebErrorString_m92A1DDF2ADFFF8AEE6B1A7FAE384743C31F9E01D_ftn) (int32_t);
	static UnityWebRequest_GetWebErrorString_m92A1DDF2ADFFF8AEE6B1A7FAE384743C31F9E01D_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_GetWebErrorString_m92A1DDF2ADFFF8AEE6B1A7FAE384743C31F9E01D_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::GetWebErrorString(UnityEngine.Networking.UnityWebRequest/UnityWebRequestError)");
	String_t* retVal = _il2cpp_icall_func(___err0);
	return retVal;
}
// System.String UnityEngine.Networking.UnityWebRequest::GetHTTPStatusString(System.Int64)
extern "C" IL2CPP_METHOD_ATTR String_t* UnityWebRequest_GetHTTPStatusString_m370515E94B5B3C14B4A49677A31D8494262D7EDA (int64_t ___responseCode0, const RuntimeMethod* method)
{
	typedef String_t* (*UnityWebRequest_GetHTTPStatusString_m370515E94B5B3C14B4A49677A31D8494262D7EDA_ftn) (int64_t);
	static UnityWebRequest_GetHTTPStatusString_m370515E94B5B3C14B4A49677A31D8494262D7EDA_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_GetHTTPStatusString_m370515E94B5B3C14B4A49677A31D8494262D7EDA_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::GetHTTPStatusString(System.Int64)");
	String_t* retVal = _il2cpp_icall_func(___responseCode0);
	return retVal;
}
// System.Boolean UnityEngine.Networking.UnityWebRequest::get_disposeCertificateHandlerOnDispose()
extern "C" IL2CPP_METHOD_ATTR bool UnityWebRequest_get_disposeCertificateHandlerOnDispose_m98EFCAC30D637479DC0DC45CFD8A15D402328F99 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	bool V_0 = false;
	{
		bool L_0 = __this->get_U3CdisposeCertificateHandlerOnDisposeU3Ek__BackingField_5();
		V_0 = L_0;
		goto IL_000c;
	}

IL_000c:
	{
		bool L_1 = V_0;
		return L_1;
	}
}
// System.Void UnityEngine.Networking.UnityWebRequest::set_disposeCertificateHandlerOnDispose(System.Boolean)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_set_disposeCertificateHandlerOnDispose_m8609E1213309D1796E00860ECA9228F6454114AE (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CdisposeCertificateHandlerOnDisposeU3Ek__BackingField_5(L_0);
		return;
	}
}
// System.Boolean UnityEngine.Networking.UnityWebRequest::get_disposeDownloadHandlerOnDispose()
extern "C" IL2CPP_METHOD_ATTR bool UnityWebRequest_get_disposeDownloadHandlerOnDispose_m3BE68E08A94D92D7076F49CB5196019E6E5E17AA (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	bool V_0 = false;
	{
		bool L_0 = __this->get_U3CdisposeDownloadHandlerOnDisposeU3Ek__BackingField_6();
		V_0 = L_0;
		goto IL_000c;
	}

IL_000c:
	{
		bool L_1 = V_0;
		return L_1;
	}
}
// System.Void UnityEngine.Networking.UnityWebRequest::set_disposeDownloadHandlerOnDispose(System.Boolean)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_set_disposeDownloadHandlerOnDispose_mA888301C47844E383DEC96D88CAD6CB8D9E7B9FA (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CdisposeDownloadHandlerOnDisposeU3Ek__BackingField_6(L_0);
		return;
	}
}
// System.Boolean UnityEngine.Networking.UnityWebRequest::get_disposeUploadHandlerOnDispose()
extern "C" IL2CPP_METHOD_ATTR bool UnityWebRequest_get_disposeUploadHandlerOnDispose_mE4A39A3A06DB4450DA49972254B4498A5F8F69DE (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	bool V_0 = false;
	{
		bool L_0 = __this->get_U3CdisposeUploadHandlerOnDisposeU3Ek__BackingField_7();
		V_0 = L_0;
		goto IL_000c;
	}

IL_000c:
	{
		bool L_1 = V_0;
		return L_1;
	}
}
// System.Void UnityEngine.Networking.UnityWebRequest::set_disposeUploadHandlerOnDispose(System.Boolean)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_set_disposeUploadHandlerOnDispose_mC176753B8AFBB40B69FAD7F1E2B2711CA5D6AA71 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, bool ___value0, const RuntimeMethod* method)
{
	{
		bool L_0 = ___value0;
		__this->set_U3CdisposeUploadHandlerOnDisposeU3Ek__BackingField_7(L_0);
		return;
	}
}
// System.IntPtr UnityEngine.Networking.UnityWebRequest::Create()
extern "C" IL2CPP_METHOD_ATTR intptr_t UnityWebRequest_Create_m98363C34C71AA034B47FA64589711B6F0AEF6698 (const RuntimeMethod* method)
{
	typedef intptr_t (*UnityWebRequest_Create_m98363C34C71AA034B47FA64589711B6F0AEF6698_ftn) ();
	static UnityWebRequest_Create_m98363C34C71AA034B47FA64589711B6F0AEF6698_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_Create_m98363C34C71AA034B47FA64589711B6F0AEF6698_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::Create()");
	intptr_t retVal = _il2cpp_icall_func();
	return retVal;
}
// System.Void UnityEngine.Networking.UnityWebRequest::Release()
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_Release_mD168D309DCE6696163B3357FA21047689D1A7D74 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	typedef void (*UnityWebRequest_Release_mD168D309DCE6696163B3357FA21047689D1A7D74_ftn) (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 *);
	static UnityWebRequest_Release_mD168D309DCE6696163B3357FA21047689D1A7D74_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_Release_mD168D309DCE6696163B3357FA21047689D1A7D74_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::Release()");
	_il2cpp_icall_func(__this);
}
// System.Void UnityEngine.Networking.UnityWebRequest::InternalDestroy()
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_InternalDestroy_mF5D7484808AEAE24A43B678614D257FBF885026B (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UnityWebRequest_InternalDestroy_mF5D7484808AEAE24A43B678614D257FBF885026B_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		intptr_t L_0 = __this->get_m_Ptr_0();
		bool L_1 = IntPtr_op_Inequality_mB4886A806009EA825EFCC60CD2A7F6EB8E273A61((intptr_t)L_0, (intptr_t)(0), /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_002f;
		}
	}
	{
		UnityWebRequest_Abort_mF2C9BD010E5B32FF9F57C2EB4A9A0C8D0289CA7E(__this, /*hidden argument*/NULL);
		UnityWebRequest_Release_mD168D309DCE6696163B3357FA21047689D1A7D74(__this, /*hidden argument*/NULL);
		__this->set_m_Ptr_0((intptr_t)(0));
	}

IL_002f:
	{
		return;
	}
}
// System.Void UnityEngine.Networking.UnityWebRequest::InternalSetDefaults()
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_InternalSetDefaults_m644CC3C1C737838385F0EC9523A8930E696A9309 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	{
		UnityWebRequest_set_disposeDownloadHandlerOnDispose_mA888301C47844E383DEC96D88CAD6CB8D9E7B9FA(__this, (bool)1, /*hidden argument*/NULL);
		UnityWebRequest_set_disposeUploadHandlerOnDispose_mC176753B8AFBB40B69FAD7F1E2B2711CA5D6AA71(__this, (bool)1, /*hidden argument*/NULL);
		UnityWebRequest_set_disposeCertificateHandlerOnDispose_m8609E1213309D1796E00860ECA9228F6454114AE(__this, (bool)1, /*hidden argument*/NULL);
		return;
	}
}
// System.Void UnityEngine.Networking.UnityWebRequest::Finalize()
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_Finalize_mEBEE0B5A630F0D75CE9F23CDA91DB5048D92CF2C (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	Exception_t * __last_unhandled_exception = 0;
	NO_UNUSED_WARNING (__last_unhandled_exception);
	Exception_t * __exception_local = 0;
	NO_UNUSED_WARNING (__exception_local);
	int32_t __leave_target = -1;
	NO_UNUSED_WARNING (__leave_target);
	{
	}

IL_0001:
	try
	{ // begin try (depth: 1)
		UnityWebRequest_DisposeHandlers_m0E54EE2A704090B2C2F1F3C90D30A47E3BF2B5C9(__this, /*hidden argument*/NULL);
		UnityWebRequest_InternalDestroy_mF5D7484808AEAE24A43B678614D257FBF885026B(__this, /*hidden argument*/NULL);
		IL2CPP_LEAVE(0x19, FINALLY_0012);
	} // end try (depth: 1)
	catch(Il2CppExceptionWrapper& e)
	{
		__last_unhandled_exception = (Exception_t *)e.ex;
		goto FINALLY_0012;
	}

FINALLY_0012:
	{ // begin finally (depth: 1)
		Object_Finalize_m4015B7D3A44DE125C5FE34D7276CD4697C06F380(__this, /*hidden argument*/NULL);
		IL2CPP_RESET_LEAVE(0x19);
		IL2CPP_END_FINALLY(18)
	} // end finally (depth: 1)
	IL2CPP_CLEANUP(18)
	{
		IL2CPP_JUMP_TBL(0x19, IL_0019)
		IL2CPP_RETHROW_IF_UNHANDLED(Exception_t *)
	}

IL_0019:
	{
		return;
	}
}
// System.Void UnityEngine.Networking.UnityWebRequest::Dispose()
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_Dispose_m6AFA87DA329282058723E5ACE016B0B08CFE806D (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UnityWebRequest_Dispose_m6AFA87DA329282058723E5ACE016B0B08CFE806D_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		UnityWebRequest_DisposeHandlers_m0E54EE2A704090B2C2F1F3C90D30A47E3BF2B5C9(__this, /*hidden argument*/NULL);
		UnityWebRequest_InternalDestroy_mF5D7484808AEAE24A43B678614D257FBF885026B(__this, /*hidden argument*/NULL);
		IL2CPP_RUNTIME_CLASS_INIT(GC_tC1D7BD74E8F44ECCEF5CD2B5D84BFF9AAE02D01D_il2cpp_TypeInfo_var);
		GC_SuppressFinalize_m037319A9B95A5BA437E806DE592802225EE5B425(__this, /*hidden argument*/NULL);
		return;
	}
}
// System.Void UnityEngine.Networking.UnityWebRequest::DisposeHandlers()
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_DisposeHandlers_m0E54EE2A704090B2C2F1F3C90D30A47E3BF2B5C9 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * V_0 = NULL;
	UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * V_1 = NULL;
	CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * V_2 = NULL;
	{
		bool L_0 = UnityWebRequest_get_disposeDownloadHandlerOnDispose_m3BE68E08A94D92D7076F49CB5196019E6E5E17AA(__this, /*hidden argument*/NULL);
		if (!L_0)
		{
			goto IL_0023;
		}
	}
	{
		DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * L_1 = UnityWebRequest_get_downloadHandler_m83044026479E6B4B2739DCE9EEA8A0FAE7D9AF41(__this, /*hidden argument*/NULL);
		V_0 = L_1;
		DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * L_2 = V_0;
		if (!L_2)
		{
			goto IL_0022;
		}
	}
	{
		DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * L_3 = V_0;
		NullCheck(L_3);
		DownloadHandler_Dispose_m7478E72B2DBA4B55FAA25F7A1975A13BA5891D4B(L_3, /*hidden argument*/NULL);
	}

IL_0022:
	{
	}

IL_0023:
	{
		bool L_4 = UnityWebRequest_get_disposeUploadHandlerOnDispose_mE4A39A3A06DB4450DA49972254B4498A5F8F69DE(__this, /*hidden argument*/NULL);
		if (!L_4)
		{
			goto IL_0045;
		}
	}
	{
		UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * L_5 = UnityWebRequest_get_uploadHandler_mB23A35C2412258E44538F37AA540421C95E69A5C(__this, /*hidden argument*/NULL);
		V_1 = L_5;
		UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * L_6 = V_1;
		if (!L_6)
		{
			goto IL_0044;
		}
	}
	{
		UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * L_7 = V_1;
		NullCheck(L_7);
		UploadHandler_Dispose_m9BBE8D7D2BBAAC2DE84B52BADA0B79CEA6F2DAB2(L_7, /*hidden argument*/NULL);
	}

IL_0044:
	{
	}

IL_0045:
	{
		bool L_8 = UnityWebRequest_get_disposeCertificateHandlerOnDispose_m98EFCAC30D637479DC0DC45CFD8A15D402328F99(__this, /*hidden argument*/NULL);
		if (!L_8)
		{
			goto IL_0067;
		}
	}
	{
		CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * L_9 = UnityWebRequest_get_certificateHandler_mD3C46D07991190373A7144A6732E390FFBE6DF00(__this, /*hidden argument*/NULL);
		V_2 = L_9;
		CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * L_10 = V_2;
		if (!L_10)
		{
			goto IL_0066;
		}
	}
	{
		CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * L_11 = V_2;
		NullCheck(L_11);
		CertificateHandler_Dispose_m9C71BAA51760FDF05AB999B6AB6E6BC71BCB8CA0(L_11, /*hidden argument*/NULL);
	}

IL_0066:
	{
	}

IL_0067:
	{
		return;
	}
}
// UnityEngine.Networking.UnityWebRequestAsyncOperation UnityEngine.Networking.UnityWebRequest::BeginWebRequest()
extern "C" IL2CPP_METHOD_ATTR UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * UnityWebRequest_BeginWebRequest_m1EF3612D316F7924F6E40D63DD3B0D0118C50CC0 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	typedef UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * (*UnityWebRequest_BeginWebRequest_m1EF3612D316F7924F6E40D63DD3B0D0118C50CC0_ftn) (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 *);
	static UnityWebRequest_BeginWebRequest_m1EF3612D316F7924F6E40D63DD3B0D0118C50CC0_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_BeginWebRequest_m1EF3612D316F7924F6E40D63DD3B0D0118C50CC0_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::BeginWebRequest()");
	UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * retVal = _il2cpp_icall_func(__this);
	return retVal;
}
// UnityEngine.AsyncOperation UnityEngine.Networking.UnityWebRequest::Send()
extern "C" IL2CPP_METHOD_ATTR AsyncOperation_t304C51ABED8AE734CC8DDDFE13013D8D5A44641D * UnityWebRequest_Send_mE2BC0213F05A8E6E8384AB4BB8BA75883BDD801F (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	AsyncOperation_t304C51ABED8AE734CC8DDDFE13013D8D5A44641D * V_0 = NULL;
	{
		UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * L_0 = UnityWebRequest_SendWebRequest_mF536CB2A0A39354A54B555B66B017816C5833EBD(__this, /*hidden argument*/NULL);
		V_0 = L_0;
		goto IL_000d;
	}

IL_000d:
	{
		AsyncOperation_t304C51ABED8AE734CC8DDDFE13013D8D5A44641D * L_1 = V_0;
		return L_1;
	}
}
// UnityEngine.Networking.UnityWebRequestAsyncOperation UnityEngine.Networking.UnityWebRequest::SendWebRequest()
extern "C" IL2CPP_METHOD_ATTR UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * UnityWebRequest_SendWebRequest_mF536CB2A0A39354A54B555B66B017816C5833EBD (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * V_0 = NULL;
	UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * V_1 = NULL;
	{
		UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * L_0 = UnityWebRequest_BeginWebRequest_m1EF3612D316F7924F6E40D63DD3B0D0118C50CC0(__this, /*hidden argument*/NULL);
		V_0 = L_0;
		UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * L_1 = V_0;
		if (!L_1)
		{
			goto IL_0015;
		}
	}
	{
		UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * L_2 = V_0;
		NullCheck(L_2);
		UnityWebRequestAsyncOperation_set_webRequest_m07869D44180E2A93042A18260FA5A2BB934AC42F(L_2, __this, /*hidden argument*/NULL);
	}

IL_0015:
	{
		UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * L_3 = V_0;
		V_1 = L_3;
		goto IL_001c;
	}

IL_001c:
	{
		UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * L_4 = V_1;
		return L_4;
	}
}
// System.Void UnityEngine.Networking.UnityWebRequest::Abort()
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_Abort_mF2C9BD010E5B32FF9F57C2EB4A9A0C8D0289CA7E (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	typedef void (*UnityWebRequest_Abort_mF2C9BD010E5B32FF9F57C2EB4A9A0C8D0289CA7E_ftn) (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 *);
	static UnityWebRequest_Abort_mF2C9BD010E5B32FF9F57C2EB4A9A0C8D0289CA7E_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_Abort_mF2C9BD010E5B32FF9F57C2EB4A9A0C8D0289CA7E_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::Abort()");
	_il2cpp_icall_func(__this);
}
// UnityEngine.Networking.UnityWebRequest_UnityWebRequestError UnityEngine.Networking.UnityWebRequest::SetMethod(UnityEngine.Networking.UnityWebRequest_UnityWebRequestMethod)
extern "C" IL2CPP_METHOD_ATTR int32_t UnityWebRequest_SetMethod_mEE55FF0E071E784318B8C2110E3A3688BF4661CB (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, int32_t ___methodType0, const RuntimeMethod* method)
{
	typedef int32_t (*UnityWebRequest_SetMethod_mEE55FF0E071E784318B8C2110E3A3688BF4661CB_ftn) (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 *, int32_t);
	static UnityWebRequest_SetMethod_mEE55FF0E071E784318B8C2110E3A3688BF4661CB_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_SetMethod_mEE55FF0E071E784318B8C2110E3A3688BF4661CB_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::SetMethod(UnityEngine.Networking.UnityWebRequest/UnityWebRequestMethod)");
	int32_t retVal = _il2cpp_icall_func(__this, ___methodType0);
	return retVal;
}
// System.Void UnityEngine.Networking.UnityWebRequest::InternalSetMethod(UnityEngine.Networking.UnityWebRequest_UnityWebRequestMethod)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_InternalSetMethod_m636508AA8E6EF12B3255D8ED108BFF7EB1AB68C9 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, int32_t ___methodType0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UnityWebRequest_InternalSetMethod_m636508AA8E6EF12B3255D8ED108BFF7EB1AB68C9_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	int32_t V_0 = 0;
	{
		bool L_0 = UnityWebRequest_get_isModifiable_mD7583537BBC7111555FF73846D120103D2563342(__this, /*hidden argument*/NULL);
		if (L_0)
		{
			goto IL_0017;
		}
	}
	{
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_1 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_1, _stringLiteral70EE7E18113E0328AAE2B1D5D212C2735F1C00F8, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_1, NULL, UnityWebRequest_InternalSetMethod_m636508AA8E6EF12B3255D8ED108BFF7EB1AB68C9_RuntimeMethod_var);
	}

IL_0017:
	{
		int32_t L_2 = ___methodType0;
		int32_t L_3 = UnityWebRequest_SetMethod_mEE55FF0E071E784318B8C2110E3A3688BF4661CB(__this, L_2, /*hidden argument*/NULL);
		V_0 = L_3;
		int32_t L_4 = V_0;
		if (!L_4)
		{
			goto IL_0031;
		}
	}
	{
		int32_t L_5 = V_0;
		String_t* L_6 = UnityWebRequest_GetWebErrorString_m92A1DDF2ADFFF8AEE6B1A7FAE384743C31F9E01D(L_5, /*hidden argument*/NULL);
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_7 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_7, L_6, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_7, NULL, UnityWebRequest_InternalSetMethod_m636508AA8E6EF12B3255D8ED108BFF7EB1AB68C9_RuntimeMethod_var);
	}

IL_0031:
	{
		return;
	}
}
// UnityEngine.Networking.UnityWebRequest_UnityWebRequestError UnityEngine.Networking.UnityWebRequest::SetCustomMethod(System.String)
extern "C" IL2CPP_METHOD_ATTR int32_t UnityWebRequest_SetCustomMethod_mC818FAC0FD8B91FD454C6DFBF7561EEE2D0BA4F4 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___customMethodName0, const RuntimeMethod* method)
{
	typedef int32_t (*UnityWebRequest_SetCustomMethod_mC818FAC0FD8B91FD454C6DFBF7561EEE2D0BA4F4_ftn) (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 *, String_t*);
	static UnityWebRequest_SetCustomMethod_mC818FAC0FD8B91FD454C6DFBF7561EEE2D0BA4F4_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_SetCustomMethod_mC818FAC0FD8B91FD454C6DFBF7561EEE2D0BA4F4_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::SetCustomMethod(System.String)");
	int32_t retVal = _il2cpp_icall_func(__this, ___customMethodName0);
	return retVal;
}
// System.Void UnityEngine.Networking.UnityWebRequest::InternalSetCustomMethod(System.String)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_InternalSetCustomMethod_mE9F0C84C6DCD5412AEDD76280EEC4FB82516EF16 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___customMethodName0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UnityWebRequest_InternalSetCustomMethod_mE9F0C84C6DCD5412AEDD76280EEC4FB82516EF16_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	int32_t V_0 = 0;
	{
		bool L_0 = UnityWebRequest_get_isModifiable_mD7583537BBC7111555FF73846D120103D2563342(__this, /*hidden argument*/NULL);
		if (L_0)
		{
			goto IL_0017;
		}
	}
	{
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_1 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_1, _stringLiteral70EE7E18113E0328AAE2B1D5D212C2735F1C00F8, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_1, NULL, UnityWebRequest_InternalSetCustomMethod_mE9F0C84C6DCD5412AEDD76280EEC4FB82516EF16_RuntimeMethod_var);
	}

IL_0017:
	{
		String_t* L_2 = ___customMethodName0;
		int32_t L_3 = UnityWebRequest_SetCustomMethod_mC818FAC0FD8B91FD454C6DFBF7561EEE2D0BA4F4(__this, L_2, /*hidden argument*/NULL);
		V_0 = L_3;
		int32_t L_4 = V_0;
		if (!L_4)
		{
			goto IL_0031;
		}
	}
	{
		int32_t L_5 = V_0;
		String_t* L_6 = UnityWebRequest_GetWebErrorString_m92A1DDF2ADFFF8AEE6B1A7FAE384743C31F9E01D(L_5, /*hidden argument*/NULL);
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_7 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_7, L_6, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_7, NULL, UnityWebRequest_InternalSetCustomMethod_mE9F0C84C6DCD5412AEDD76280EEC4FB82516EF16_RuntimeMethod_var);
	}

IL_0031:
	{
		return;
	}
}
// System.Void UnityEngine.Networking.UnityWebRequest::set_method(System.String)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_set_method_mF2DAC86EB05D65B9BCB52056B7CBB2C1AD87EEC6 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___value0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UnityWebRequest_set_method_mF2DAC86EB05D65B9BCB52056B7CBB2C1AD87EEC6_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	String_t* V_0 = NULL;
	{
		String_t* L_0 = ___value0;
		bool L_1 = String_IsNullOrEmpty_m06A85A206AC2106D1982826C5665B9BD35324229(L_0, /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_0018;
		}
	}
	{
		ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 * L_2 = (ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 *)il2cpp_codegen_object_new(ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1_il2cpp_TypeInfo_var);
		ArgumentException__ctor_m9A85EF7FEFEC21DDD525A67E831D77278E5165B7(L_2, _stringLiteralF37BF1E2A7C84A010A6E65E2E41A03F1C044F04B, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_2, NULL, UnityWebRequest_set_method_mF2DAC86EB05D65B9BCB52056B7CBB2C1AD87EEC6_RuntimeMethod_var);
	}

IL_0018:
	{
		String_t* L_3 = ___value0;
		NullCheck(L_3);
		String_t* L_4 = String_ToUpper_m23D019B7C5EF2C5C01F524EB8137A424B33EEFE2(L_3, /*hidden argument*/NULL);
		V_0 = L_4;
		String_t* L_5 = V_0;
		if (!L_5)
		{
			goto IL_009a;
		}
	}
	{
		String_t* L_6 = V_0;
		bool L_7 = String_op_Equality_m139F0E4195AE2F856019E63B241F36F016997FCE(L_6, _stringLiteralF030BBBD32966CDE41037B98A8849C46B76E4BC1, /*hidden argument*/NULL);
		if (L_7)
		{
			goto IL_006a;
		}
	}
	{
		String_t* L_8 = V_0;
		bool L_9 = String_op_Equality_m139F0E4195AE2F856019E63B241F36F016997FCE(L_8, _stringLiteral61FF81C30AA3C76E78AFEA62B2E3BD1DFA49E854, /*hidden argument*/NULL);
		if (L_9)
		{
			goto IL_0076;
		}
	}
	{
		String_t* L_10 = V_0;
		bool L_11 = String_op_Equality_m139F0E4195AE2F856019E63B241F36F016997FCE(L_10, _stringLiteral091B0CE42EB0BD96169EA00B16DD938F6D63AC95, /*hidden argument*/NULL);
		if (L_11)
		{
			goto IL_0082;
		}
	}
	{
		String_t* L_12 = V_0;
		bool L_13 = String_op_Equality_m139F0E4195AE2F856019E63B241F36F016997FCE(L_12, _stringLiteral7138A51661947B19B5088DA5A2BFEDE2876F49B9, /*hidden argument*/NULL);
		if (L_13)
		{
			goto IL_008e;
		}
	}
	{
		goto IL_009a;
	}

IL_006a:
	{
		UnityWebRequest_InternalSetMethod_m636508AA8E6EF12B3255D8ED108BFF7EB1AB68C9(__this, 0, /*hidden argument*/NULL);
		goto IL_00ab;
	}

IL_0076:
	{
		UnityWebRequest_InternalSetMethod_m636508AA8E6EF12B3255D8ED108BFF7EB1AB68C9(__this, 1, /*hidden argument*/NULL);
		goto IL_00ab;
	}

IL_0082:
	{
		UnityWebRequest_InternalSetMethod_m636508AA8E6EF12B3255D8ED108BFF7EB1AB68C9(__this, 2, /*hidden argument*/NULL);
		goto IL_00ab;
	}

IL_008e:
	{
		UnityWebRequest_InternalSetMethod_m636508AA8E6EF12B3255D8ED108BFF7EB1AB68C9(__this, 3, /*hidden argument*/NULL);
		goto IL_00ab;
	}

IL_009a:
	{
		String_t* L_14 = ___value0;
		NullCheck(L_14);
		String_t* L_15 = String_ToUpper_m23D019B7C5EF2C5C01F524EB8137A424B33EEFE2(L_14, /*hidden argument*/NULL);
		UnityWebRequest_InternalSetCustomMethod_mE9F0C84C6DCD5412AEDD76280EEC4FB82516EF16(__this, L_15, /*hidden argument*/NULL);
		goto IL_00ab;
	}

IL_00ab:
	{
		return;
	}
}
// UnityEngine.Networking.UnityWebRequest_UnityWebRequestError UnityEngine.Networking.UnityWebRequest::GetError()
extern "C" IL2CPP_METHOD_ATTR int32_t UnityWebRequest_GetError_m55BF2299E3B195AC416CCCB46C3DBD83C075018C (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	typedef int32_t (*UnityWebRequest_GetError_m55BF2299E3B195AC416CCCB46C3DBD83C075018C_ftn) (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 *);
	static UnityWebRequest_GetError_m55BF2299E3B195AC416CCCB46C3DBD83C075018C_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_GetError_m55BF2299E3B195AC416CCCB46C3DBD83C075018C_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::GetError()");
	int32_t retVal = _il2cpp_icall_func(__this);
	return retVal;
}
// System.String UnityEngine.Networking.UnityWebRequest::get_error()
extern "C" IL2CPP_METHOD_ATTR String_t* UnityWebRequest_get_error_mC79FE2460B3F30B8F9E5385BD7D2B4C5B295D7CC (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UnityWebRequest_get_error_mC79FE2460B3F30B8F9E5385BD7D2B4C5B295D7CC_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	String_t* V_0 = NULL;
	String_t* V_1 = NULL;
	{
		bool L_0 = UnityWebRequest_get_isNetworkError_m082AFE1A58A330AC4CBD179606B61CB39DD44588(__this, /*hidden argument*/NULL);
		if (L_0)
		{
			goto IL_001e;
		}
	}
	{
		bool L_1 = UnityWebRequest_get_isHttpError_m8F636B70C239EC848FACC83189DE0C22CADEC1C3(__this, /*hidden argument*/NULL);
		if (L_1)
		{
			goto IL_001e;
		}
	}
	{
		V_0 = (String_t*)NULL;
		goto IL_0064;
	}

IL_001e:
	{
		bool L_2 = UnityWebRequest_get_isHttpError_m8F636B70C239EC848FACC83189DE0C22CADEC1C3(__this, /*hidden argument*/NULL);
		if (!L_2)
		{
			goto IL_0052;
		}
	}
	{
		int64_t L_3 = UnityWebRequest_get_responseCode_m34819872549939D1EF9EA3D4010974FBEBAF0070(__this, /*hidden argument*/NULL);
		String_t* L_4 = UnityWebRequest_GetHTTPStatusString_m370515E94B5B3C14B4A49677A31D8494262D7EDA(L_3, /*hidden argument*/NULL);
		V_1 = L_4;
		int64_t L_5 = UnityWebRequest_get_responseCode_m34819872549939D1EF9EA3D4010974FBEBAF0070(__this, /*hidden argument*/NULL);
		int64_t L_6 = L_5;
		RuntimeObject * L_7 = Box(Int64_t7A386C2FF7B0280A0F516992401DDFCF0FF7B436_il2cpp_TypeInfo_var, &L_6);
		String_t* L_8 = V_1;
		String_t* L_9 = String_Format_m19325298DBC61AAC016C16F7B3CF97A8A3DEA34A(_stringLiteral4DD76F7BD318A8B909BC0FF86CA3BE3625DA0374, L_7, L_8, /*hidden argument*/NULL);
		V_0 = L_9;
		goto IL_0064;
	}

IL_0052:
	{
		int32_t L_10 = UnityWebRequest_GetError_m55BF2299E3B195AC416CCCB46C3DBD83C075018C(__this, /*hidden argument*/NULL);
		String_t* L_11 = UnityWebRequest_GetWebErrorString_m92A1DDF2ADFFF8AEE6B1A7FAE384743C31F9E01D(L_10, /*hidden argument*/NULL);
		V_0 = L_11;
		goto IL_0064;
	}

IL_0064:
	{
		String_t* L_12 = V_0;
		return L_12;
	}
}
// System.Void UnityEngine.Networking.UnityWebRequest::set_url(System.String)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_set_url_mA698FD94C447FF7C1C429D50C2EBAEEDD473007D (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___value0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UnityWebRequest_set_url_mA698FD94C447FF7C1C429D50C2EBAEEDD473007D_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	String_t* V_0 = NULL;
	{
		V_0 = _stringLiteralF6C97A7F64063CFEE7C2DC2157847204D4DBF093;
		String_t* L_0 = ___value0;
		String_t* L_1 = V_0;
		IL2CPP_RUNTIME_CLASS_INIT(WebRequestUtils_tBE8F8607E3A9633419968F6AF2F706A029AE1296_il2cpp_TypeInfo_var);
		String_t* L_2 = WebRequestUtils_MakeInitialUrl_m446CCE4EFB276BE27A9380D55B9E704D01107B83(L_0, L_1, /*hidden argument*/NULL);
		UnityWebRequest_InternalSetUrl_m2E2C837A6F32065CAAAF6EFA7D0237C9E206689A(__this, L_2, /*hidden argument*/NULL);
		return;
	}
}
// System.Void UnityEngine.Networking.UnityWebRequest::set_uri(System.Uri)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_set_uri_mDE5DEDA2698190CD80AE3B4B7342400A32284124 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___value0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UnityWebRequest_set_uri_mDE5DEDA2698190CD80AE3B4B7342400A32284124_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_0 = ___value0;
		NullCheck(L_0);
		bool L_1 = Uri_get_IsAbsoluteUri_m8C189085F1C675DBC3148AA70C38074EC075D722(L_0, /*hidden argument*/NULL);
		if (L_1)
		{
			goto IL_0017;
		}
	}
	{
		ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 * L_2 = (ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 *)il2cpp_codegen_object_new(ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1_il2cpp_TypeInfo_var);
		ArgumentException__ctor_m9A85EF7FEFEC21DDD525A67E831D77278E5165B7(L_2, _stringLiteralCA854C2B1F1CACD772359086ADF1F752E8698E5A, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_2, NULL, UnityWebRequest_set_uri_mDE5DEDA2698190CD80AE3B4B7342400A32284124_RuntimeMethod_var);
	}

IL_0017:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_3 = ___value0;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_4 = ___value0;
		NullCheck(L_4);
		String_t* L_5 = Uri_get_OriginalString_m56099E46276F0A52524347F1F46A2F88E948504F(L_4, /*hidden argument*/NULL);
		IL2CPP_RUNTIME_CLASS_INIT(WebRequestUtils_tBE8F8607E3A9633419968F6AF2F706A029AE1296_il2cpp_TypeInfo_var);
		String_t* L_6 = WebRequestUtils_MakeUriString_m5693EA04230335B9611278EFC189BD58339D01E4(L_3, L_5, (bool)0, /*hidden argument*/NULL);
		UnityWebRequest_InternalSetUrl_m2E2C837A6F32065CAAAF6EFA7D0237C9E206689A(__this, L_6, /*hidden argument*/NULL);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_7 = ___value0;
		__this->set_m_Uri_4(L_7);
		return;
	}
}
// UnityEngine.Networking.UnityWebRequest_UnityWebRequestError UnityEngine.Networking.UnityWebRequest::SetUrl(System.String)
extern "C" IL2CPP_METHOD_ATTR int32_t UnityWebRequest_SetUrl_mED007912E89AA114D1A3D6905586116F74C8D774 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___url0, const RuntimeMethod* method)
{
	typedef int32_t (*UnityWebRequest_SetUrl_mED007912E89AA114D1A3D6905586116F74C8D774_ftn) (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 *, String_t*);
	static UnityWebRequest_SetUrl_mED007912E89AA114D1A3D6905586116F74C8D774_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_SetUrl_mED007912E89AA114D1A3D6905586116F74C8D774_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::SetUrl(System.String)");
	int32_t retVal = _il2cpp_icall_func(__this, ___url0);
	return retVal;
}
// System.Void UnityEngine.Networking.UnityWebRequest::InternalSetUrl(System.String)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_InternalSetUrl_m2E2C837A6F32065CAAAF6EFA7D0237C9E206689A (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___url0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UnityWebRequest_InternalSetUrl_m2E2C837A6F32065CAAAF6EFA7D0237C9E206689A_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	int32_t V_0 = 0;
	{
		bool L_0 = UnityWebRequest_get_isModifiable_mD7583537BBC7111555FF73846D120103D2563342(__this, /*hidden argument*/NULL);
		if (L_0)
		{
			goto IL_0017;
		}
	}
	{
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_1 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_1, _stringLiteral81D42CE01525C0213D5284260BDB58819D046FB9, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_1, NULL, UnityWebRequest_InternalSetUrl_m2E2C837A6F32065CAAAF6EFA7D0237C9E206689A_RuntimeMethod_var);
	}

IL_0017:
	{
		String_t* L_2 = ___url0;
		int32_t L_3 = UnityWebRequest_SetUrl_mED007912E89AA114D1A3D6905586116F74C8D774(__this, L_2, /*hidden argument*/NULL);
		V_0 = L_3;
		int32_t L_4 = V_0;
		if (!L_4)
		{
			goto IL_0031;
		}
	}
	{
		int32_t L_5 = V_0;
		String_t* L_6 = UnityWebRequest_GetWebErrorString_m92A1DDF2ADFFF8AEE6B1A7FAE384743C31F9E01D(L_5, /*hidden argument*/NULL);
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_7 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_7, L_6, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_7, NULL, UnityWebRequest_InternalSetUrl_m2E2C837A6F32065CAAAF6EFA7D0237C9E206689A_RuntimeMethod_var);
	}

IL_0031:
	{
		return;
	}
}
// System.Int64 UnityEngine.Networking.UnityWebRequest::get_responseCode()
extern "C" IL2CPP_METHOD_ATTR int64_t UnityWebRequest_get_responseCode_m34819872549939D1EF9EA3D4010974FBEBAF0070 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	typedef int64_t (*UnityWebRequest_get_responseCode_m34819872549939D1EF9EA3D4010974FBEBAF0070_ftn) (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 *);
	static UnityWebRequest_get_responseCode_m34819872549939D1EF9EA3D4010974FBEBAF0070_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_get_responseCode_m34819872549939D1EF9EA3D4010974FBEBAF0070_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::get_responseCode()");
	int64_t retVal = _il2cpp_icall_func(__this);
	return retVal;
}
// System.Boolean UnityEngine.Networking.UnityWebRequest::get_isModifiable()
extern "C" IL2CPP_METHOD_ATTR bool UnityWebRequest_get_isModifiable_mD7583537BBC7111555FF73846D120103D2563342 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	typedef bool (*UnityWebRequest_get_isModifiable_mD7583537BBC7111555FF73846D120103D2563342_ftn) (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 *);
	static UnityWebRequest_get_isModifiable_mD7583537BBC7111555FF73846D120103D2563342_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_get_isModifiable_mD7583537BBC7111555FF73846D120103D2563342_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::get_isModifiable()");
	bool retVal = _il2cpp_icall_func(__this);
	return retVal;
}
// System.Boolean UnityEngine.Networking.UnityWebRequest::get_isNetworkError()
extern "C" IL2CPP_METHOD_ATTR bool UnityWebRequest_get_isNetworkError_m082AFE1A58A330AC4CBD179606B61CB39DD44588 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	typedef bool (*UnityWebRequest_get_isNetworkError_m082AFE1A58A330AC4CBD179606B61CB39DD44588_ftn) (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 *);
	static UnityWebRequest_get_isNetworkError_m082AFE1A58A330AC4CBD179606B61CB39DD44588_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_get_isNetworkError_m082AFE1A58A330AC4CBD179606B61CB39DD44588_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::get_isNetworkError()");
	bool retVal = _il2cpp_icall_func(__this);
	return retVal;
}
// System.Boolean UnityEngine.Networking.UnityWebRequest::get_isHttpError()
extern "C" IL2CPP_METHOD_ATTR bool UnityWebRequest_get_isHttpError_m8F636B70C239EC848FACC83189DE0C22CADEC1C3 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	typedef bool (*UnityWebRequest_get_isHttpError_m8F636B70C239EC848FACC83189DE0C22CADEC1C3_ftn) (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 *);
	static UnityWebRequest_get_isHttpError_m8F636B70C239EC848FACC83189DE0C22CADEC1C3_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_get_isHttpError_m8F636B70C239EC848FACC83189DE0C22CADEC1C3_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::get_isHttpError()");
	bool retVal = _il2cpp_icall_func(__this);
	return retVal;
}
// UnityEngine.Networking.UnityWebRequest_UnityWebRequestError UnityEngine.Networking.UnityWebRequest::InternalSetRequestHeader(System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR int32_t UnityWebRequest_InternalSetRequestHeader_m7481D7E49B6E6078598E40B81D1A3DA9B8D2BD10 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___name0, String_t* ___value1, const RuntimeMethod* method)
{
	typedef int32_t (*UnityWebRequest_InternalSetRequestHeader_m7481D7E49B6E6078598E40B81D1A3DA9B8D2BD10_ftn) (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 *, String_t*, String_t*);
	static UnityWebRequest_InternalSetRequestHeader_m7481D7E49B6E6078598E40B81D1A3DA9B8D2BD10_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_InternalSetRequestHeader_m7481D7E49B6E6078598E40B81D1A3DA9B8D2BD10_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::InternalSetRequestHeader(System.String,System.String)");
	int32_t retVal = _il2cpp_icall_func(__this, ___name0, ___value1);
	return retVal;
}
// System.Void UnityEngine.Networking.UnityWebRequest::SetRequestHeader(System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_SetRequestHeader_m1B54D38BDACC2789FC8EE889EA72EDD7844D2309 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, String_t* ___name0, String_t* ___value1, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UnityWebRequest_SetRequestHeader_m1B54D38BDACC2789FC8EE889EA72EDD7844D2309_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	int32_t V_0 = 0;
	{
		String_t* L_0 = ___name0;
		bool L_1 = String_IsNullOrEmpty_m06A85A206AC2106D1982826C5665B9BD35324229(L_0, /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_0017;
		}
	}
	{
		ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 * L_2 = (ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 *)il2cpp_codegen_object_new(ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1_il2cpp_TypeInfo_var);
		ArgumentException__ctor_m9A85EF7FEFEC21DDD525A67E831D77278E5165B7(L_2, _stringLiteralBEDBFCA635D617975AC8C4A6D1FBC9714BC86399, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_2, NULL, UnityWebRequest_SetRequestHeader_m1B54D38BDACC2789FC8EE889EA72EDD7844D2309_RuntimeMethod_var);
	}

IL_0017:
	{
		String_t* L_3 = ___value1;
		if (L_3)
		{
			goto IL_0028;
		}
	}
	{
		ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 * L_4 = (ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 *)il2cpp_codegen_object_new(ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1_il2cpp_TypeInfo_var);
		ArgumentException__ctor_m9A85EF7FEFEC21DDD525A67E831D77278E5165B7(L_4, _stringLiteralA288E90C6C4E12B4E76A10851EF1ABD903F1EAE7, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_4, NULL, UnityWebRequest_SetRequestHeader_m1B54D38BDACC2789FC8EE889EA72EDD7844D2309_RuntimeMethod_var);
	}

IL_0028:
	{
		bool L_5 = UnityWebRequest_get_isModifiable_mD7583537BBC7111555FF73846D120103D2563342(__this, /*hidden argument*/NULL);
		if (L_5)
		{
			goto IL_003e;
		}
	}
	{
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_6 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_6, _stringLiteralAEA05C1AAB9D42F987C023592D1AF2F1D8403D2F, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_6, NULL, UnityWebRequest_SetRequestHeader_m1B54D38BDACC2789FC8EE889EA72EDD7844D2309_RuntimeMethod_var);
	}

IL_003e:
	{
		String_t* L_7 = ___name0;
		String_t* L_8 = ___value1;
		int32_t L_9 = UnityWebRequest_InternalSetRequestHeader_m7481D7E49B6E6078598E40B81D1A3DA9B8D2BD10(__this, L_7, L_8, /*hidden argument*/NULL);
		V_0 = L_9;
		int32_t L_10 = V_0;
		if (!L_10)
		{
			goto IL_0059;
		}
	}
	{
		int32_t L_11 = V_0;
		String_t* L_12 = UnityWebRequest_GetWebErrorString_m92A1DDF2ADFFF8AEE6B1A7FAE384743C31F9E01D(L_11, /*hidden argument*/NULL);
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_13 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_13, L_12, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_13, NULL, UnityWebRequest_SetRequestHeader_m1B54D38BDACC2789FC8EE889EA72EDD7844D2309_RuntimeMethod_var);
	}

IL_0059:
	{
		return;
	}
}
// UnityEngine.Networking.UnityWebRequest_UnityWebRequestError UnityEngine.Networking.UnityWebRequest::SetUploadHandler(UnityEngine.Networking.UploadHandler)
extern "C" IL2CPP_METHOD_ATTR int32_t UnityWebRequest_SetUploadHandler_m046EF4089035441F661AED13F703024DEE030525 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * ___uh0, const RuntimeMethod* method)
{
	typedef int32_t (*UnityWebRequest_SetUploadHandler_m046EF4089035441F661AED13F703024DEE030525_ftn) (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 *, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 *);
	static UnityWebRequest_SetUploadHandler_m046EF4089035441F661AED13F703024DEE030525_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_SetUploadHandler_m046EF4089035441F661AED13F703024DEE030525_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::SetUploadHandler(UnityEngine.Networking.UploadHandler)");
	int32_t retVal = _il2cpp_icall_func(__this, ___uh0);
	return retVal;
}
// UnityEngine.Networking.UploadHandler UnityEngine.Networking.UnityWebRequest::get_uploadHandler()
extern "C" IL2CPP_METHOD_ATTR UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * UnityWebRequest_get_uploadHandler_mB23A35C2412258E44538F37AA540421C95E69A5C (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * V_0 = NULL;
	{
		UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * L_0 = __this->get_m_UploadHandler_2();
		V_0 = L_0;
		goto IL_000d;
	}

IL_000d:
	{
		UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * L_1 = V_0;
		return L_1;
	}
}
// System.Void UnityEngine.Networking.UnityWebRequest::set_uploadHandler(UnityEngine.Networking.UploadHandler)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_set_uploadHandler_m7B33656584914FB3F6FB0FF73C08F671262724A1 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * ___value0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UnityWebRequest_set_uploadHandler_m7B33656584914FB3F6FB0FF73C08F671262724A1_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	int32_t V_0 = 0;
	{
		bool L_0 = UnityWebRequest_get_isModifiable_mD7583537BBC7111555FF73846D120103D2563342(__this, /*hidden argument*/NULL);
		if (L_0)
		{
			goto IL_0017;
		}
	}
	{
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_1 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_1, _stringLiteralF80B07414273FEB6D1B5EAB1E91186C7CE65DE24, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_1, NULL, UnityWebRequest_set_uploadHandler_m7B33656584914FB3F6FB0FF73C08F671262724A1_RuntimeMethod_var);
	}

IL_0017:
	{
		UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * L_2 = ___value0;
		int32_t L_3 = UnityWebRequest_SetUploadHandler_m046EF4089035441F661AED13F703024DEE030525(__this, L_2, /*hidden argument*/NULL);
		V_0 = L_3;
		int32_t L_4 = V_0;
		if (!L_4)
		{
			goto IL_0031;
		}
	}
	{
		int32_t L_5 = V_0;
		String_t* L_6 = UnityWebRequest_GetWebErrorString_m92A1DDF2ADFFF8AEE6B1A7FAE384743C31F9E01D(L_5, /*hidden argument*/NULL);
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_7 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_7, L_6, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_7, NULL, UnityWebRequest_set_uploadHandler_m7B33656584914FB3F6FB0FF73C08F671262724A1_RuntimeMethod_var);
	}

IL_0031:
	{
		UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * L_8 = ___value0;
		__this->set_m_UploadHandler_2(L_8);
		return;
	}
}
// UnityEngine.Networking.UnityWebRequest_UnityWebRequestError UnityEngine.Networking.UnityWebRequest::SetDownloadHandler(UnityEngine.Networking.DownloadHandler)
extern "C" IL2CPP_METHOD_ATTR int32_t UnityWebRequest_SetDownloadHandler_mDE4E6137C34A90754C41B3A0B7B303135771EEDD (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * ___dh0, const RuntimeMethod* method)
{
	typedef int32_t (*UnityWebRequest_SetDownloadHandler_mDE4E6137C34A90754C41B3A0B7B303135771EEDD_ftn) (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 *, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 *);
	static UnityWebRequest_SetDownloadHandler_mDE4E6137C34A90754C41B3A0B7B303135771EEDD_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UnityWebRequest_SetDownloadHandler_mDE4E6137C34A90754C41B3A0B7B303135771EEDD_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UnityWebRequest::SetDownloadHandler(UnityEngine.Networking.DownloadHandler)");
	int32_t retVal = _il2cpp_icall_func(__this, ___dh0);
	return retVal;
}
// UnityEngine.Networking.DownloadHandler UnityEngine.Networking.UnityWebRequest::get_downloadHandler()
extern "C" IL2CPP_METHOD_ATTR DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * UnityWebRequest_get_downloadHandler_m83044026479E6B4B2739DCE9EEA8A0FAE7D9AF41 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * V_0 = NULL;
	{
		DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * L_0 = __this->get_m_DownloadHandler_1();
		V_0 = L_0;
		goto IL_000d;
	}

IL_000d:
	{
		DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * L_1 = V_0;
		return L_1;
	}
}
// System.Void UnityEngine.Networking.UnityWebRequest::set_downloadHandler(UnityEngine.Networking.DownloadHandler)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequest_set_downloadHandler_mB481C2EE30F84B62396C1A837F46046F12B8FA7E (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * ___value0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UnityWebRequest_set_downloadHandler_mB481C2EE30F84B62396C1A837F46046F12B8FA7E_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	int32_t V_0 = 0;
	{
		bool L_0 = UnityWebRequest_get_isModifiable_mD7583537BBC7111555FF73846D120103D2563342(__this, /*hidden argument*/NULL);
		if (L_0)
		{
			goto IL_0017;
		}
	}
	{
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_1 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_1, _stringLiteralA58AA001D4152D20F7F8E0809B9CD782BE38A82C, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_1, NULL, UnityWebRequest_set_downloadHandler_mB481C2EE30F84B62396C1A837F46046F12B8FA7E_RuntimeMethod_var);
	}

IL_0017:
	{
		DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * L_2 = ___value0;
		int32_t L_3 = UnityWebRequest_SetDownloadHandler_mDE4E6137C34A90754C41B3A0B7B303135771EEDD(__this, L_2, /*hidden argument*/NULL);
		V_0 = L_3;
		int32_t L_4 = V_0;
		if (!L_4)
		{
			goto IL_0031;
		}
	}
	{
		int32_t L_5 = V_0;
		String_t* L_6 = UnityWebRequest_GetWebErrorString_m92A1DDF2ADFFF8AEE6B1A7FAE384743C31F9E01D(L_5, /*hidden argument*/NULL);
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_7 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_7, L_6, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_7, NULL, UnityWebRequest_set_downloadHandler_mB481C2EE30F84B62396C1A837F46046F12B8FA7E_RuntimeMethod_var);
	}

IL_0031:
	{
		DownloadHandler_t4A7802ADC97024B469C87FA454B6973951980EE9 * L_8 = ___value0;
		__this->set_m_DownloadHandler_1(L_8);
		return;
	}
}
// UnityEngine.Networking.CertificateHandler UnityEngine.Networking.UnityWebRequest::get_certificateHandler()
extern "C" IL2CPP_METHOD_ATTR CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * UnityWebRequest_get_certificateHandler_mD3C46D07991190373A7144A6732E390FFBE6DF00 (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * __this, const RuntimeMethod* method)
{
	CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * V_0 = NULL;
	{
		CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * L_0 = __this->get_m_CertificateHandler_3();
		V_0 = L_0;
		goto IL_000d;
	}

IL_000d:
	{
		CertificateHandler_tBD070BF4150A44AB482FD36EA3882C363117E8C0 * L_1 = V_0;
		return L_1;
	}
}
// UnityEngine.Networking.UnityWebRequest UnityEngine.Networking.UnityWebRequest::Get(System.String)
extern "C" IL2CPP_METHOD_ATTR UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * UnityWebRequest_Get_mF4E12AA47AAF25221AD738B434B0EA8D40659B18 (String_t* ___uri0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UnityWebRequest_Get_mF4E12AA47AAF25221AD738B434B0EA8D40659B18_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * V_0 = NULL;
	UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * V_1 = NULL;
	{
		String_t* L_0 = ___uri0;
		DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255 * L_1 = (DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255 *)il2cpp_codegen_object_new(DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_il2cpp_TypeInfo_var);
		DownloadHandlerBuffer__ctor_m2134187D8FB07FBAEA2CE23BFCEB13FD94261A9A(L_1, /*hidden argument*/NULL);
		UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * L_2 = (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 *)il2cpp_codegen_object_new(UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_il2cpp_TypeInfo_var);
		UnityWebRequest__ctor_m0D2F8F3E1202EF4256D17E91B95DB6CC673FC8D6(L_2, L_0, _stringLiteralF030BBBD32966CDE41037B98A8849C46B76E4BC1, L_1, (UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 *)NULL, /*hidden argument*/NULL);
		V_0 = L_2;
		UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * L_3 = V_0;
		V_1 = L_3;
		goto IL_001a;
	}

IL_001a:
	{
		UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * L_4 = V_1;
		return L_4;
	}
}
// UnityEngine.Networking.UnityWebRequest UnityEngine.Networking.UnityWebRequest::Get(System.Uri)
extern "C" IL2CPP_METHOD_ATTR UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * UnityWebRequest_Get_m9BA0FB62F38F9704A5E1E8155E0A76B8F9C352DA (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___uri0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UnityWebRequest_Get_m9BA0FB62F38F9704A5E1E8155E0A76B8F9C352DA_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * V_0 = NULL;
	UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * V_1 = NULL;
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_0 = ___uri0;
		DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255 * L_1 = (DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255 *)il2cpp_codegen_object_new(DownloadHandlerBuffer_tF6A73B82C9EC807D36B904A58E1DF2DDA696B255_il2cpp_TypeInfo_var);
		DownloadHandlerBuffer__ctor_m2134187D8FB07FBAEA2CE23BFCEB13FD94261A9A(L_1, /*hidden argument*/NULL);
		UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * L_2 = (UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 *)il2cpp_codegen_object_new(UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129_il2cpp_TypeInfo_var);
		UnityWebRequest__ctor_mE0CF781DACE7D8A434D8CDEF1AA1B2C2B21AB4BF(L_2, L_0, _stringLiteralF030BBBD32966CDE41037B98A8849C46B76E4BC1, L_1, (UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 *)NULL, /*hidden argument*/NULL);
		V_0 = L_2;
		UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * L_3 = V_0;
		V_1 = L_3;
		goto IL_001a;
	}

IL_001a:
	{
		UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * L_4 = V_1;
		return L_4;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif


// Conversion methods for marshalling of: UnityEngine.Networking.UnityWebRequestAsyncOperation
extern "C" void UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353_marshal_pinvoke(const UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353& unmarshaled, UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353_marshaled_pinvoke& marshaled)
{
	Exception_t* ___U3CwebRequestU3Ek__BackingField_2Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field '<webRequest>k__BackingField' of type 'UnityWebRequestAsyncOperation': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___U3CwebRequestU3Ek__BackingField_2Exception, NULL, NULL);
}
extern "C" void UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353_marshal_pinvoke_back(const UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353_marshaled_pinvoke& marshaled, UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353& unmarshaled)
{
	Exception_t* ___U3CwebRequestU3Ek__BackingField_2Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field '<webRequest>k__BackingField' of type 'UnityWebRequestAsyncOperation': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___U3CwebRequestU3Ek__BackingField_2Exception, NULL, NULL);
}
// Conversion method for clean up from marshalling of: UnityEngine.Networking.UnityWebRequestAsyncOperation
extern "C" void UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353_marshal_pinvoke_cleanup(UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353_marshaled_pinvoke& marshaled)
{
}


// Conversion methods for marshalling of: UnityEngine.Networking.UnityWebRequestAsyncOperation
extern "C" void UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353_marshal_com(const UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353& unmarshaled, UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353_marshaled_com& marshaled)
{
	Exception_t* ___U3CwebRequestU3Ek__BackingField_2Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field '<webRequest>k__BackingField' of type 'UnityWebRequestAsyncOperation': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___U3CwebRequestU3Ek__BackingField_2Exception, NULL, NULL);
}
extern "C" void UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353_marshal_com_back(const UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353_marshaled_com& marshaled, UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353& unmarshaled)
{
	Exception_t* ___U3CwebRequestU3Ek__BackingField_2Exception = il2cpp_codegen_get_marshal_directive_exception("Cannot marshal field '<webRequest>k__BackingField' of type 'UnityWebRequestAsyncOperation': Reference type field marshaling is not supported.");
	IL2CPP_RAISE_MANAGED_EXCEPTION(___U3CwebRequestU3Ek__BackingField_2Exception, NULL, NULL);
}
// Conversion method for clean up from marshalling of: UnityEngine.Networking.UnityWebRequestAsyncOperation
extern "C" void UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353_marshal_com_cleanup(UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353_marshaled_com& marshaled)
{
}
// System.Void UnityEngine.Networking.UnityWebRequestAsyncOperation::.ctor()
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequestAsyncOperation__ctor_mB260FD4CE600B27EB9A2ABA0BDD20FAF8449D523 (UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * __this, const RuntimeMethod* method)
{
	{
		AsyncOperation__ctor_mEEE6114B72B8807F4AA6FF48FA79E4EFE480293F(__this, /*hidden argument*/NULL);
		return;
	}
}
// System.Void UnityEngine.Networking.UnityWebRequestAsyncOperation::set_webRequest(UnityEngine.Networking.UnityWebRequest)
extern "C" IL2CPP_METHOD_ATTR void UnityWebRequestAsyncOperation_set_webRequest_m07869D44180E2A93042A18260FA5A2BB934AC42F (UnityWebRequestAsyncOperation_t726E134F16701A2671D40BEBE22110DC57156353 * __this, UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * ___value0, const RuntimeMethod* method)
{
	{
		UnityWebRequest_t9120F5A2C7D43B936B49C0B7E4CA54C822689129 * L_0 = ___value0;
		__this->set_U3CwebRequestU3Ek__BackingField_2(L_0);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Conversion methods for marshalling of: UnityEngine.Networking.UploadHandler
extern "C" void UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshal_pinvoke(const UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4& unmarshaled, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_pinvoke& marshaled)
{
	marshaled.___m_Ptr_0 = unmarshaled.get_m_Ptr_0();
}
extern "C" void UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshal_pinvoke_back(const UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_pinvoke& marshaled, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4& unmarshaled)
{
	intptr_t unmarshaled_m_Ptr_temp_0;
	memset(&unmarshaled_m_Ptr_temp_0, 0, sizeof(unmarshaled_m_Ptr_temp_0));
	unmarshaled_m_Ptr_temp_0 = marshaled.___m_Ptr_0;
	unmarshaled.set_m_Ptr_0(unmarshaled_m_Ptr_temp_0);
}
// Conversion method for clean up from marshalling of: UnityEngine.Networking.UploadHandler
extern "C" void UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshal_pinvoke_cleanup(UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_pinvoke& marshaled)
{
}
// Conversion methods for marshalling of: UnityEngine.Networking.UploadHandler
extern "C" void UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshal_com(const UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4& unmarshaled, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_com& marshaled)
{
	marshaled.___m_Ptr_0 = unmarshaled.get_m_Ptr_0();
}
extern "C" void UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshal_com_back(const UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_com& marshaled, UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4& unmarshaled)
{
	intptr_t unmarshaled_m_Ptr_temp_0;
	memset(&unmarshaled_m_Ptr_temp_0, 0, sizeof(unmarshaled_m_Ptr_temp_0));
	unmarshaled_m_Ptr_temp_0 = marshaled.___m_Ptr_0;
	unmarshaled.set_m_Ptr_0(unmarshaled_m_Ptr_temp_0);
}
// Conversion method for clean up from marshalling of: UnityEngine.Networking.UploadHandler
extern "C" void UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshal_com_cleanup(UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4_marshaled_com& marshaled)
{
}
// System.Void UnityEngine.Networking.UploadHandler::.ctor()
extern "C" IL2CPP_METHOD_ATTR void UploadHandler__ctor_m3F76154710C5CB7099388479FA02E6555D077F6E (UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * __this, const RuntimeMethod* method)
{
	{
		Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0(__this, /*hidden argument*/NULL);
		return;
	}
}
// System.Void UnityEngine.Networking.UploadHandler::Release()
extern "C" IL2CPP_METHOD_ATTR void UploadHandler_Release_m1723A22438AF0A7BE616D512E54190D9CE0EC3C4 (UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * __this, const RuntimeMethod* method)
{
	typedef void (*UploadHandler_Release_m1723A22438AF0A7BE616D512E54190D9CE0EC3C4_ftn) (UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 *);
	static UploadHandler_Release_m1723A22438AF0A7BE616D512E54190D9CE0EC3C4_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UploadHandler_Release_m1723A22438AF0A7BE616D512E54190D9CE0EC3C4_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UploadHandler::Release()");
	_il2cpp_icall_func(__this);
}
// System.Void UnityEngine.Networking.UploadHandler::Finalize()
extern "C" IL2CPP_METHOD_ATTR void UploadHandler_Finalize_m68B0CC0B647B11B53908CA4E577AEA5DBA31E4D8 (UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * __this, const RuntimeMethod* method)
{
	Exception_t * __last_unhandled_exception = 0;
	NO_UNUSED_WARNING (__last_unhandled_exception);
	Exception_t * __exception_local = 0;
	NO_UNUSED_WARNING (__exception_local);
	int32_t __leave_target = -1;
	NO_UNUSED_WARNING (__leave_target);
	{
	}

IL_0001:
	try
	{ // begin try (depth: 1)
		UploadHandler_Dispose_m9BBE8D7D2BBAAC2DE84B52BADA0B79CEA6F2DAB2(__this, /*hidden argument*/NULL);
		IL2CPP_LEAVE(0x13, FINALLY_000c);
	} // end try (depth: 1)
	catch(Il2CppExceptionWrapper& e)
	{
		__last_unhandled_exception = (Exception_t *)e.ex;
		goto FINALLY_000c;
	}

FINALLY_000c:
	{ // begin finally (depth: 1)
		Object_Finalize_m4015B7D3A44DE125C5FE34D7276CD4697C06F380(__this, /*hidden argument*/NULL);
		IL2CPP_RESET_LEAVE(0x13);
		IL2CPP_END_FINALLY(12)
	} // end finally (depth: 1)
	IL2CPP_CLEANUP(12)
	{
		IL2CPP_JUMP_TBL(0x13, IL_0013)
		IL2CPP_RETHROW_IF_UNHANDLED(Exception_t *)
	}

IL_0013:
	{
		return;
	}
}
// System.Void UnityEngine.Networking.UploadHandler::Dispose()
extern "C" IL2CPP_METHOD_ATTR void UploadHandler_Dispose_m9BBE8D7D2BBAAC2DE84B52BADA0B79CEA6F2DAB2 (UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UploadHandler_Dispose_m9BBE8D7D2BBAAC2DE84B52BADA0B79CEA6F2DAB2_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		intptr_t L_0 = __this->get_m_Ptr_0();
		bool L_1 = IntPtr_op_Inequality_mB4886A806009EA825EFCC60CD2A7F6EB8E273A61((intptr_t)L_0, (intptr_t)(0), /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_0029;
		}
	}
	{
		UploadHandler_Release_m1723A22438AF0A7BE616D512E54190D9CE0EC3C4(__this, /*hidden argument*/NULL);
		__this->set_m_Ptr_0((intptr_t)(0));
	}

IL_0029:
	{
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Conversion methods for marshalling of: UnityEngine.Networking.UploadHandlerRaw
extern "C" void UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27_marshal_pinvoke(const UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27& unmarshaled, UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27_marshaled_pinvoke& marshaled)
{
	marshaled.___m_Ptr_0 = unmarshaled.get_m_Ptr_0();
}
extern "C" void UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27_marshal_pinvoke_back(const UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27_marshaled_pinvoke& marshaled, UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27& unmarshaled)
{
	intptr_t unmarshaled_m_Ptr_temp_0;
	memset(&unmarshaled_m_Ptr_temp_0, 0, sizeof(unmarshaled_m_Ptr_temp_0));
	unmarshaled_m_Ptr_temp_0 = marshaled.___m_Ptr_0;
	unmarshaled.set_m_Ptr_0(unmarshaled_m_Ptr_temp_0);
}
// Conversion method for clean up from marshalling of: UnityEngine.Networking.UploadHandlerRaw
extern "C" void UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27_marshal_pinvoke_cleanup(UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27_marshaled_pinvoke& marshaled)
{
}
// Conversion methods for marshalling of: UnityEngine.Networking.UploadHandlerRaw
extern "C" void UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27_marshal_com(const UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27& unmarshaled, UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27_marshaled_com& marshaled)
{
	marshaled.___m_Ptr_0 = unmarshaled.get_m_Ptr_0();
}
extern "C" void UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27_marshal_com_back(const UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27_marshaled_com& marshaled, UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27& unmarshaled)
{
	intptr_t unmarshaled_m_Ptr_temp_0;
	memset(&unmarshaled_m_Ptr_temp_0, 0, sizeof(unmarshaled_m_Ptr_temp_0));
	unmarshaled_m_Ptr_temp_0 = marshaled.___m_Ptr_0;
	unmarshaled.set_m_Ptr_0(unmarshaled_m_Ptr_temp_0);
}
// Conversion method for clean up from marshalling of: UnityEngine.Networking.UploadHandlerRaw
extern "C" void UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27_marshal_com_cleanup(UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27_marshaled_com& marshaled)
{
}
// System.Void UnityEngine.Networking.UploadHandlerRaw::.ctor(System.Byte[])
extern "C" IL2CPP_METHOD_ATTR void UploadHandlerRaw__ctor_m9F7643CA3314C8CE46DD41FBF584C268E2546935 (UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27 * __this, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___data0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UploadHandlerRaw__ctor_m9F7643CA3314C8CE46DD41FBF584C268E2546935_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		UploadHandler__ctor_m3F76154710C5CB7099388479FA02E6555D077F6E(__this, /*hidden argument*/NULL);
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_0 = ___data0;
		if (!L_0)
		{
			goto IL_0020;
		}
	}
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_1 = ___data0;
		NullCheck(L_1);
		if ((((int32_t)((int32_t)(((RuntimeArray *)L_1)->max_length)))))
		{
			goto IL_0020;
		}
	}
	{
		ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 * L_2 = (ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 *)il2cpp_codegen_object_new(ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1_il2cpp_TypeInfo_var);
		ArgumentException__ctor_m9A85EF7FEFEC21DDD525A67E831D77278E5165B7(L_2, _stringLiteral4E5057793E1875AA08F21BE7F738453AD461E5F0, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_2, NULL, UploadHandlerRaw__ctor_m9F7643CA3314C8CE46DD41FBF584C268E2546935_RuntimeMethod_var);
	}

IL_0020:
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_3 = ___data0;
		intptr_t L_4 = UploadHandlerRaw_Create_m921D80A8952FC740F358E5FD28E6D5A70622687B(__this, L_3, /*hidden argument*/NULL);
		((UploadHandler_t24F4097D30A1E7C689D8881A27F251B4741601E4 *)__this)->set_m_Ptr_0((intptr_t)L_4);
		return;
	}
}
// System.IntPtr UnityEngine.Networking.UploadHandlerRaw::Create(UnityEngine.Networking.UploadHandlerRaw,System.Byte[])
extern "C" IL2CPP_METHOD_ATTR intptr_t UploadHandlerRaw_Create_m921D80A8952FC740F358E5FD28E6D5A70622687B (UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27 * ___self0, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___data1, const RuntimeMethod* method)
{
	typedef intptr_t (*UploadHandlerRaw_Create_m921D80A8952FC740F358E5FD28E6D5A70622687B_ftn) (UploadHandlerRaw_t9E6A69B7726F134F31F6744F5EFDF611E7C54F27 *, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821*);
	static UploadHandlerRaw_Create_m921D80A8952FC740F358E5FD28E6D5A70622687B_ftn _il2cpp_icall_func;
	if (!_il2cpp_icall_func)
	_il2cpp_icall_func = (UploadHandlerRaw_Create_m921D80A8952FC740F358E5FD28E6D5A70622687B_ftn)il2cpp_codegen_resolve_icall ("UnityEngine.Networking.UploadHandlerRaw::Create(UnityEngine.Networking.UploadHandlerRaw,System.Byte[])");
	intptr_t retVal = _il2cpp_icall_func(___self0, ___data1);
	return retVal;
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Text.Encoding UnityEngine.WWWForm::get_DefaultEncoding()
extern "C" IL2CPP_METHOD_ATTR Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * WWWForm_get_DefaultEncoding_m13BB72339201269AB257B275B20A5A35B233BC3F (const RuntimeMethod* method)
{
	Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * V_0 = NULL;
	{
		Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_0 = Encoding_get_ASCII_m9B673AE3152AB04D07CADE6E5E142C785B5BC94E(/*hidden argument*/NULL);
		V_0 = L_0;
		goto IL_000c;
	}

IL_000c:
	{
		Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_1 = V_0;
		return L_1;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Byte UnityEngine.WWWTranscoder::Hex2Byte(System.Byte[],System.Int32)
extern "C" IL2CPP_METHOD_ATTR uint8_t WWWTranscoder_Hex2Byte_mD417CA540CFBE045FCE32959CD3443EB9C8C7423 (ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___b0, int32_t ___offset1, const RuntimeMethod* method)
{
	uint8_t V_0 = 0x0;
	int32_t V_1 = 0;
	int32_t V_2 = 0;
	uint8_t V_3 = 0x0;
	{
		V_0 = (uint8_t)0;
		int32_t L_0 = ___offset1;
		V_1 = L_0;
		goto IL_007a;
	}

IL_000a:
	{
		uint8_t L_1 = V_0;
		V_0 = (uint8_t)(((int32_t)((uint8_t)((int32_t)il2cpp_codegen_multiply((int32_t)L_1, (int32_t)((int32_t)16))))));
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_2 = ___b0;
		int32_t L_3 = V_1;
		NullCheck(L_2);
		int32_t L_4 = L_3;
		uint8_t L_5 = (L_2)->GetAt(static_cast<il2cpp_array_size_t>(L_4));
		V_2 = L_5;
		int32_t L_6 = V_2;
		if ((((int32_t)L_6) < ((int32_t)((int32_t)48))))
		{
			goto IL_002f;
		}
	}
	{
		int32_t L_7 = V_2;
		if ((((int32_t)L_7) > ((int32_t)((int32_t)57))))
		{
			goto IL_002f;
		}
	}
	{
		int32_t L_8 = V_2;
		V_2 = ((int32_t)il2cpp_codegen_subtract((int32_t)L_8, (int32_t)((int32_t)48)));
		goto IL_005e;
	}

IL_002f:
	{
		int32_t L_9 = V_2;
		if ((((int32_t)L_9) < ((int32_t)((int32_t)65))))
		{
			goto IL_0049;
		}
	}
	{
		int32_t L_10 = V_2;
		if ((((int32_t)L_10) > ((int32_t)((int32_t)75))))
		{
			goto IL_0049;
		}
	}
	{
		int32_t L_11 = V_2;
		V_2 = ((int32_t)il2cpp_codegen_subtract((int32_t)L_11, (int32_t)((int32_t)55)));
		goto IL_005e;
	}

IL_0049:
	{
		int32_t L_12 = V_2;
		if ((((int32_t)L_12) < ((int32_t)((int32_t)97))))
		{
			goto IL_005e;
		}
	}
	{
		int32_t L_13 = V_2;
		if ((((int32_t)L_13) > ((int32_t)((int32_t)102))))
		{
			goto IL_005e;
		}
	}
	{
		int32_t L_14 = V_2;
		V_2 = ((int32_t)il2cpp_codegen_subtract((int32_t)L_14, (int32_t)((int32_t)87)));
	}

IL_005e:
	{
		int32_t L_15 = V_2;
		if ((((int32_t)L_15) <= ((int32_t)((int32_t)15))))
		{
			goto IL_006f;
		}
	}
	{
		V_3 = (uint8_t)((int32_t)63);
		goto IL_008a;
	}

IL_006f:
	{
		uint8_t L_16 = V_0;
		int32_t L_17 = V_2;
		V_0 = (uint8_t)(((int32_t)((uint8_t)((int32_t)il2cpp_codegen_add((int32_t)L_16, (int32_t)(((int32_t)((uint8_t)L_17))))))));
		int32_t L_18 = V_1;
		V_1 = ((int32_t)il2cpp_codegen_add((int32_t)L_18, (int32_t)1));
	}

IL_007a:
	{
		int32_t L_19 = V_1;
		int32_t L_20 = ___offset1;
		if ((((int32_t)L_19) < ((int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_20, (int32_t)2)))))
		{
			goto IL_000a;
		}
	}
	{
		uint8_t L_21 = V_0;
		V_3 = L_21;
		goto IL_008a;
	}

IL_008a:
	{
		uint8_t L_22 = V_3;
		return L_22;
	}
}
// System.Byte[] UnityEngine.WWWTranscoder::URLDecode(System.Byte[])
extern "C" IL2CPP_METHOD_ATTR ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* WWWTranscoder_URLDecode_m591A567154B1B8737ECBFE065AF4FCA59217F5D8 (ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___toEncode0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (WWWTranscoder_URLDecode_m591A567154B1B8737ECBFE065AF4FCA59217F5D8_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* V_0 = NULL;
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_0 = ___toEncode0;
		IL2CPP_RUNTIME_CLASS_INIT(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_il2cpp_TypeInfo_var);
		uint8_t L_1 = ((WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields*)il2cpp_codegen_static_fields_for(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_il2cpp_TypeInfo_var))->get_urlEscapeChar_2();
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_2 = ((WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields*)il2cpp_codegen_static_fields_for(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_il2cpp_TypeInfo_var))->get_urlSpace_3();
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_3 = WWWTranscoder_Decode_m2533830DAAAE6F33AA6EE85A5BF63C96F5D631D4(L_0, L_1, L_2, /*hidden argument*/NULL);
		V_0 = L_3;
		goto IL_0017;
	}

IL_0017:
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_4 = V_0;
		return L_4;
	}
}
// System.Boolean UnityEngine.WWWTranscoder::ByteSubArrayEquals(System.Byte[],System.Int32,System.Byte[])
extern "C" IL2CPP_METHOD_ATTR bool WWWTranscoder_ByteSubArrayEquals_m268C2A9B31CCF4D81E7BEEF843DF5D477ECA9958 (ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___array0, int32_t ___index1, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___comperand2, const RuntimeMethod* method)
{
	bool V_0 = false;
	int32_t V_1 = 0;
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_0 = ___array0;
		NullCheck(L_0);
		int32_t L_1 = ___index1;
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_2 = ___comperand2;
		NullCheck(L_2);
		if ((((int32_t)((int32_t)il2cpp_codegen_subtract((int32_t)(((int32_t)((int32_t)(((RuntimeArray *)L_0)->max_length)))), (int32_t)L_1))) >= ((int32_t)(((int32_t)((int32_t)(((RuntimeArray *)L_2)->max_length)))))))
		{
			goto IL_0015;
		}
	}
	{
		V_0 = (bool)0;
		goto IL_0044;
	}

IL_0015:
	{
		V_1 = 0;
		goto IL_0034;
	}

IL_001c:
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_3 = ___array0;
		int32_t L_4 = ___index1;
		int32_t L_5 = V_1;
		NullCheck(L_3);
		int32_t L_6 = ((int32_t)il2cpp_codegen_add((int32_t)L_4, (int32_t)L_5));
		uint8_t L_7 = (L_3)->GetAt(static_cast<il2cpp_array_size_t>(L_6));
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_8 = ___comperand2;
		int32_t L_9 = V_1;
		NullCheck(L_8);
		int32_t L_10 = L_9;
		uint8_t L_11 = (L_8)->GetAt(static_cast<il2cpp_array_size_t>(L_10));
		if ((((int32_t)L_7) == ((int32_t)L_11)))
		{
			goto IL_0030;
		}
	}
	{
		V_0 = (bool)0;
		goto IL_0044;
	}

IL_0030:
	{
		int32_t L_12 = V_1;
		V_1 = ((int32_t)il2cpp_codegen_add((int32_t)L_12, (int32_t)1));
	}

IL_0034:
	{
		int32_t L_13 = V_1;
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_14 = ___comperand2;
		NullCheck(L_14);
		if ((((int32_t)L_13) < ((int32_t)(((int32_t)((int32_t)(((RuntimeArray *)L_14)->max_length)))))))
		{
			goto IL_001c;
		}
	}
	{
		V_0 = (bool)1;
		goto IL_0044;
	}

IL_0044:
	{
		bool L_15 = V_0;
		return L_15;
	}
}
// System.Byte[] UnityEngine.WWWTranscoder::Decode(System.Byte[],System.Byte,System.Byte[])
extern "C" IL2CPP_METHOD_ATTR ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* WWWTranscoder_Decode_m2533830DAAAE6F33AA6EE85A5BF63C96F5D631D4 (ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___input0, uint8_t ___escapeChar1, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___space2, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (WWWTranscoder_Decode_m2533830DAAAE6F33AA6EE85A5BF63C96F5D631D4_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C * V_0 = NULL;
	int32_t V_1 = 0;
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* V_2 = NULL;
	Exception_t * __last_unhandled_exception = 0;
	NO_UNUSED_WARNING (__last_unhandled_exception);
	Exception_t * __exception_local = 0;
	NO_UNUSED_WARNING (__exception_local);
	int32_t __leave_target = -1;
	NO_UNUSED_WARNING (__leave_target);
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_0 = ___input0;
		NullCheck(L_0);
		MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C * L_1 = (MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C *)il2cpp_codegen_object_new(MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C_il2cpp_TypeInfo_var);
		MemoryStream__ctor_m78689C82DED9ACE5022B7EABF28F17FF318DF2AA(L_1, (((int32_t)((int32_t)(((RuntimeArray *)L_0)->max_length)))), /*hidden argument*/NULL);
		V_0 = L_1;
	}

IL_000a:
	try
	{ // begin try (depth: 1)
		{
			V_1 = 0;
			goto IL_0077;
		}

IL_0012:
		{
			ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_2 = ___input0;
			int32_t L_3 = V_1;
			ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_4 = ___space2;
			IL2CPP_RUNTIME_CLASS_INIT(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_il2cpp_TypeInfo_var);
			bool L_5 = WWWTranscoder_ByteSubArrayEquals_m268C2A9B31CCF4D81E7BEEF843DF5D477ECA9958(L_2, L_3, L_4, /*hidden argument*/NULL);
			if (!L_5)
			{
				goto IL_0037;
			}
		}

IL_0020:
		{
			int32_t L_6 = V_1;
			ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_7 = ___space2;
			NullCheck(L_7);
			V_1 = ((int32_t)il2cpp_codegen_add((int32_t)L_6, (int32_t)((int32_t)il2cpp_codegen_subtract((int32_t)(((int32_t)((int32_t)(((RuntimeArray *)L_7)->max_length)))), (int32_t)1))));
			MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C * L_8 = V_0;
			NullCheck(L_8);
			VirtActionInvoker1< uint8_t >::Invoke(30 /* System.Void System.IO.Stream::WriteByte(System.Byte) */, L_8, (uint8_t)((int32_t)32));
			goto IL_0072;
		}

IL_0037:
		{
			ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_9 = ___input0;
			int32_t L_10 = V_1;
			NullCheck(L_9);
			int32_t L_11 = L_10;
			uint8_t L_12 = (L_9)->GetAt(static_cast<il2cpp_array_size_t>(L_11));
			uint8_t L_13 = ___escapeChar1;
			if ((!(((uint32_t)L_12) == ((uint32_t)L_13))))
			{
				goto IL_0067;
			}
		}

IL_0040:
		{
			int32_t L_14 = V_1;
			ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_15 = ___input0;
			NullCheck(L_15);
			if ((((int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_14, (int32_t)2))) >= ((int32_t)(((int32_t)((int32_t)(((RuntimeArray *)L_15)->max_length)))))))
			{
				goto IL_0067;
			}
		}

IL_004b:
		{
			int32_t L_16 = V_1;
			V_1 = ((int32_t)il2cpp_codegen_add((int32_t)L_16, (int32_t)1));
			MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C * L_17 = V_0;
			ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_18 = ___input0;
			int32_t L_19 = V_1;
			int32_t L_20 = L_19;
			V_1 = ((int32_t)il2cpp_codegen_add((int32_t)L_20, (int32_t)1));
			IL2CPP_RUNTIME_CLASS_INIT(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_il2cpp_TypeInfo_var);
			uint8_t L_21 = WWWTranscoder_Hex2Byte_mD417CA540CFBE045FCE32959CD3443EB9C8C7423(L_18, L_20, /*hidden argument*/NULL);
			NullCheck(L_17);
			VirtActionInvoker1< uint8_t >::Invoke(30 /* System.Void System.IO.Stream::WriteByte(System.Byte) */, L_17, L_21);
			goto IL_0072;
		}

IL_0067:
		{
			MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C * L_22 = V_0;
			ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_23 = ___input0;
			int32_t L_24 = V_1;
			NullCheck(L_23);
			int32_t L_25 = L_24;
			uint8_t L_26 = (L_23)->GetAt(static_cast<il2cpp_array_size_t>(L_25));
			NullCheck(L_22);
			VirtActionInvoker1< uint8_t >::Invoke(30 /* System.Void System.IO.Stream::WriteByte(System.Byte) */, L_22, L_26);
		}

IL_0072:
		{
			int32_t L_27 = V_1;
			V_1 = ((int32_t)il2cpp_codegen_add((int32_t)L_27, (int32_t)1));
		}

IL_0077:
		{
			int32_t L_28 = V_1;
			ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_29 = ___input0;
			NullCheck(L_29);
			if ((((int32_t)L_28) < ((int32_t)(((int32_t)((int32_t)(((RuntimeArray *)L_29)->max_length)))))))
			{
				goto IL_0012;
			}
		}

IL_0080:
		{
			MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C * L_30 = V_0;
			NullCheck(L_30);
			ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_31 = VirtFuncInvoker0< ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* >::Invoke(34 /* System.Byte[] System.IO.MemoryStream::ToArray() */, L_30);
			V_2 = L_31;
			IL2CPP_LEAVE(0x99, FINALLY_008c);
		}
	} // end try (depth: 1)
	catch(Il2CppExceptionWrapper& e)
	{
		__last_unhandled_exception = (Exception_t *)e.ex;
		goto FINALLY_008c;
	}

FINALLY_008c:
	{ // begin finally (depth: 1)
		{
			MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C * L_32 = V_0;
			if (!L_32)
			{
				goto IL_0098;
			}
		}

IL_0092:
		{
			MemoryStream_t495F44B85E6B4DDE2BB7E17DE963256A74E2298C * L_33 = V_0;
			NullCheck(L_33);
			InterfaceActionInvoker0::Invoke(0 /* System.Void System.IDisposable::Dispose() */, IDisposable_t7218B22548186B208D65EA5B7870503810A2D15A_il2cpp_TypeInfo_var, L_33);
		}

IL_0098:
		{
			IL2CPP_RESET_LEAVE(0x99);
			IL2CPP_END_FINALLY(140)
		}
	} // end finally (depth: 1)
	IL2CPP_CLEANUP(140)
	{
		IL2CPP_JUMP_TBL(0x99, IL_0099)
		IL2CPP_RETHROW_IF_UNHANDLED(Exception_t *)
	}

IL_0099:
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_34 = V_2;
		return L_34;
	}
}
// System.Void UnityEngine.WWWTranscoder::.cctor()
extern "C" IL2CPP_METHOD_ATTR void WWWTranscoder__cctor_m3436CCA2D8667A6BCF6981B6573EF048BDA49F51 (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (WWWTranscoder__cctor_m3436CCA2D8667A6BCF6981B6573EF048BDA49F51_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_0 = WWWForm_get_DefaultEncoding_m13BB72339201269AB257B275B20A5A35B233BC3F(/*hidden argument*/NULL);
		NullCheck(L_0);
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_1 = VirtFuncInvoker1< ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821*, String_t* >::Invoke(17 /* System.Byte[] System.Text.Encoding::GetBytes(System.String) */, L_0, _stringLiteralCE27CB141098FEB00714E758646BE3E99C185B71);
		((WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields*)il2cpp_codegen_static_fields_for(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_il2cpp_TypeInfo_var))->set_ucHexChars_0(L_1);
		Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_2 = WWWForm_get_DefaultEncoding_m13BB72339201269AB257B275B20A5A35B233BC3F(/*hidden argument*/NULL);
		NullCheck(L_2);
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_3 = VirtFuncInvoker1< ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821*, String_t* >::Invoke(17 /* System.Byte[] System.Text.Encoding::GetBytes(System.String) */, L_2, _stringLiteralFE5567E8D769550852182CDF69D74BB16DFF8E29);
		((WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields*)il2cpp_codegen_static_fields_for(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_il2cpp_TypeInfo_var))->set_lcHexChars_1(L_3);
		((WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields*)il2cpp_codegen_static_fields_for(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_il2cpp_TypeInfo_var))->set_urlEscapeChar_2((uint8_t)((int32_t)37));
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_4 = (ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821*)SZArrayNew(ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821_il2cpp_TypeInfo_var, (uint32_t)1);
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_5 = L_4;
		NullCheck(L_5);
		(L_5)->SetAt(static_cast<il2cpp_array_size_t>(0), (uint8_t)((int32_t)43));
		((WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields*)il2cpp_codegen_static_fields_for(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_il2cpp_TypeInfo_var))->set_urlSpace_3(L_5);
		Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_6 = WWWForm_get_DefaultEncoding_m13BB72339201269AB257B275B20A5A35B233BC3F(/*hidden argument*/NULL);
		NullCheck(L_6);
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_7 = VirtFuncInvoker1< ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821*, String_t* >::Invoke(17 /* System.Byte[] System.Text.Encoding::GetBytes(System.String) */, L_6, _stringLiteral986F2ED15C79ED805000ECCD85519810B2DB2A93);
		((WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields*)il2cpp_codegen_static_fields_for(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_il2cpp_TypeInfo_var))->set_dataSpace_4(L_7);
		Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_8 = WWWForm_get_DefaultEncoding_m13BB72339201269AB257B275B20A5A35B233BC3F(/*hidden argument*/NULL);
		NullCheck(L_8);
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_9 = VirtFuncInvoker1< ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821*, String_t* >::Invoke(17 /* System.Byte[] System.Text.Encoding::GetBytes(System.String) */, L_8, _stringLiteral1C5E5F29CEB079B561835055FFA20C2E0B53F397);
		((WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields*)il2cpp_codegen_static_fields_for(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_il2cpp_TypeInfo_var))->set_urlForbidden_5(L_9);
		((WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields*)il2cpp_codegen_static_fields_for(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_il2cpp_TypeInfo_var))->set_qpEscapeChar_6((uint8_t)((int32_t)61));
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_10 = (ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821*)SZArrayNew(ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821_il2cpp_TypeInfo_var, (uint32_t)1);
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_11 = L_10;
		NullCheck(L_11);
		(L_11)->SetAt(static_cast<il2cpp_array_size_t>(0), (uint8_t)((int32_t)95));
		((WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields*)il2cpp_codegen_static_fields_for(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_il2cpp_TypeInfo_var))->set_qpSpace_7(L_11);
		Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_12 = WWWForm_get_DefaultEncoding_m13BB72339201269AB257B275B20A5A35B233BC3F(/*hidden argument*/NULL);
		NullCheck(L_12);
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_13 = VirtFuncInvoker1< ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821*, String_t* >::Invoke(17 /* System.Byte[] System.Text.Encoding::GetBytes(System.String) */, L_12, _stringLiteral38263C0B87E5FC0881F12EF855C8F694115D8213);
		((WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_StaticFields*)il2cpp_codegen_static_fields_for(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_il2cpp_TypeInfo_var))->set_qpForbidden_8(L_13);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.String UnityEngineInternal.WebRequestUtils::RedirectTo(System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR String_t* WebRequestUtils_RedirectTo_m8AC7C0BFC562550118F6FF4AE218898717E922C1 (String_t* ___baseUri0, String_t* ___redirectUri1, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (WebRequestUtils_RedirectTo_m8AC7C0BFC562550118F6FF4AE218898717E922C1_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * V_0 = NULL;
	String_t* V_1 = NULL;
	Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * V_2 = NULL;
	Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * V_3 = NULL;
	{
		String_t* L_0 = ___redirectUri1;
		NullCheck(L_0);
		Il2CppChar L_1 = String_get_Chars_m14308AC3B95F8C1D9F1D1055B116B37D595F1D96(L_0, 0, /*hidden argument*/NULL);
		if ((!(((uint32_t)L_1) == ((uint32_t)((int32_t)47)))))
		{
			goto IL_001c;
		}
	}
	{
		String_t* L_2 = ___redirectUri1;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_3 = (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)il2cpp_codegen_object_new(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		Uri__ctor_mA02DB222F4F35380DE2700D84F58EB42497FDDE4(L_3, L_2, 2, /*hidden argument*/NULL);
		V_0 = L_3;
		goto IL_0024;
	}

IL_001c:
	{
		String_t* L_4 = ___redirectUri1;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_5 = (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)il2cpp_codegen_object_new(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		Uri__ctor_mA02DB222F4F35380DE2700D84F58EB42497FDDE4(L_5, L_4, 0, /*hidden argument*/NULL);
		V_0 = L_5;
	}

IL_0024:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_6 = V_0;
		NullCheck(L_6);
		bool L_7 = Uri_get_IsAbsoluteUri_m8C189085F1C675DBC3148AA70C38074EC075D722(L_6, /*hidden argument*/NULL);
		if (!L_7)
		{
			goto IL_003b;
		}
	}
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_8 = V_0;
		NullCheck(L_8);
		String_t* L_9 = Uri_get_AbsoluteUri_m4326730E572E7E3874021E802813EB6F49F7F99E(L_8, /*hidden argument*/NULL);
		V_1 = L_9;
		goto IL_0057;
	}

IL_003b:
	{
		String_t* L_10 = ___baseUri0;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_11 = (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)il2cpp_codegen_object_new(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		Uri__ctor_mA02DB222F4F35380DE2700D84F58EB42497FDDE4(L_11, L_10, 1, /*hidden argument*/NULL);
		V_2 = L_11;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_12 = V_2;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_13 = V_0;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_14 = (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)il2cpp_codegen_object_new(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		Uri__ctor_m42192656437FBEF1EEA8724D3EF2BB67DA0ED6BF(L_14, L_12, L_13, /*hidden argument*/NULL);
		V_3 = L_14;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_15 = V_3;
		NullCheck(L_15);
		String_t* L_16 = Uri_get_AbsoluteUri_m4326730E572E7E3874021E802813EB6F49F7F99E(L_15, /*hidden argument*/NULL);
		V_1 = L_16;
		goto IL_0057;
	}

IL_0057:
	{
		String_t* L_17 = V_1;
		return L_17;
	}
}
// System.String UnityEngineInternal.WebRequestUtils::MakeInitialUrl(System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR String_t* WebRequestUtils_MakeInitialUrl_m446CCE4EFB276BE27A9380D55B9E704D01107B83 (String_t* ___targetUrl0, String_t* ___localUrl1, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (WebRequestUtils_MakeInitialUrl_m446CCE4EFB276BE27A9380D55B9E704D01107B83_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	String_t* V_0 = NULL;
	bool V_1 = false;
	Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * V_2 = NULL;
	Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * V_3 = NULL;
	FormatException_t2808E076CDE4650AF89F55FD78F49290D0EC5BDC * V_4 = NULL;
	FormatException_t2808E076CDE4650AF89F55FD78F49290D0EC5BDC * V_5 = NULL;
	Exception_t * __last_unhandled_exception = 0;
	NO_UNUSED_WARNING (__last_unhandled_exception);
	Exception_t * __exception_local = 0;
	NO_UNUSED_WARNING (__exception_local);
	int32_t __leave_target = -1;
	NO_UNUSED_WARNING (__leave_target);
	{
		String_t* L_0 = ___targetUrl0;
		bool L_1 = String_IsNullOrEmpty_m06A85A206AC2106D1982826C5665B9BD35324229(L_0, /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_0017;
		}
	}
	{
		V_0 = _stringLiteralDA39A3EE5E6B4B0D3255BFEF95601890AFD80709;
		goto IL_00d7;
	}

IL_0017:
	{
		V_1 = (bool)0;
		String_t* L_2 = ___localUrl1;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_3 = (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)il2cpp_codegen_object_new(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		Uri__ctor_mBA69907A1D799CD12ED44B611985B25FE4C626A2(L_3, L_2, /*hidden argument*/NULL);
		V_2 = L_3;
		V_3 = (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)NULL;
		String_t* L_4 = ___targetUrl0;
		NullCheck(L_4);
		Il2CppChar L_5 = String_get_Chars_m14308AC3B95F8C1D9F1D1055B116B37D595F1D96(L_4, 0, /*hidden argument*/NULL);
		if ((!(((uint32_t)L_5) == ((uint32_t)((int32_t)47)))))
		{
			goto IL_003c;
		}
	}
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_6 = V_2;
		String_t* L_7 = ___targetUrl0;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_8 = (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)il2cpp_codegen_object_new(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		Uri__ctor_m41A759BF295FB902084DD289849793E01A65A14E(L_8, L_6, L_7, /*hidden argument*/NULL);
		V_3 = L_8;
		V_1 = (bool)1;
	}

IL_003c:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_9 = V_3;
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		bool L_10 = Uri_op_Equality_mFED3D4AFAB090B76D2088C485507F8F702ADA18F(L_9, (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)NULL, /*hidden argument*/NULL);
		if (!L_10)
		{
			goto IL_006f;
		}
	}
	{
		IL2CPP_RUNTIME_CLASS_INIT(WebRequestUtils_tBE8F8607E3A9633419968F6AF2F706A029AE1296_il2cpp_TypeInfo_var);
		Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF * L_11 = ((WebRequestUtils_tBE8F8607E3A9633419968F6AF2F706A029AE1296_StaticFields*)il2cpp_codegen_static_fields_for(WebRequestUtils_tBE8F8607E3A9633419968F6AF2F706A029AE1296_il2cpp_TypeInfo_var))->get_domainRegex_0();
		String_t* L_12 = ___targetUrl0;
		NullCheck(L_11);
		bool L_13 = Regex_IsMatch_m79684C4D2CE6C5495BCCE9A32AC029E1E5950B7C(L_11, L_12, /*hidden argument*/NULL);
		if (!L_13)
		{
			goto IL_006f;
		}
	}
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_14 = V_2;
		NullCheck(L_14);
		String_t* L_15 = Uri_get_Scheme_m14A8F0018D8AACADBEF39600A59944F33EE39187(L_14, /*hidden argument*/NULL);
		String_t* L_16 = ___targetUrl0;
		String_t* L_17 = String_Concat_mF4626905368D6558695A823466A1AF65EADB9923(L_15, _stringLiteralEF81042E1E86ACB765718EA37393A1292452BBCC, L_16, /*hidden argument*/NULL);
		___targetUrl0 = L_17;
		V_1 = (bool)1;
	}

IL_006f:
	{
		V_4 = (FormatException_t2808E076CDE4650AF89F55FD78F49290D0EC5BDC *)NULL;
	}

IL_0072:
	try
	{ // begin try (depth: 1)
		{
			Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_18 = V_3;
			IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
			bool L_19 = Uri_op_Equality_mFED3D4AFAB090B76D2088C485507F8F702ADA18F(L_18, (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)NULL, /*hidden argument*/NULL);
			if (!L_19)
			{
				goto IL_0094;
			}
		}

IL_007f:
		{
			String_t* L_20 = ___targetUrl0;
			NullCheck(L_20);
			Il2CppChar L_21 = String_get_Chars_m14308AC3B95F8C1D9F1D1055B116B37D595F1D96(L_20, 0, /*hidden argument*/NULL);
			if ((((int32_t)L_21) == ((int32_t)((int32_t)46))))
			{
				goto IL_0094;
			}
		}

IL_008d:
		{
			String_t* L_22 = ___targetUrl0;
			Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_23 = (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)il2cpp_codegen_object_new(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
			Uri__ctor_mBA69907A1D799CD12ED44B611985B25FE4C626A2(L_23, L_22, /*hidden argument*/NULL);
			V_3 = L_23;
		}

IL_0094:
		{
			goto IL_00a7;
		}
	} // end try (depth: 1)
	catch(Il2CppExceptionWrapper& e)
	{
		__exception_local = (Exception_t *)e.ex;
		if(il2cpp_codegen_class_is_assignable_from (FormatException_t2808E076CDE4650AF89F55FD78F49290D0EC5BDC_il2cpp_TypeInfo_var, il2cpp_codegen_object_class(e.ex)))
			goto CATCH_009a;
		throw e;
	}

CATCH_009a:
	{ // begin catch(System.FormatException)
		V_5 = ((FormatException_t2808E076CDE4650AF89F55FD78F49290D0EC5BDC *)__exception_local);
		FormatException_t2808E076CDE4650AF89F55FD78F49290D0EC5BDC * L_24 = V_5;
		V_4 = L_24;
		goto IL_00a7;
	} // end catch (depth: 1)

IL_00a7:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_25 = V_3;
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		bool L_26 = Uri_op_Equality_mFED3D4AFAB090B76D2088C485507F8F702ADA18F(L_25, (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)NULL, /*hidden argument*/NULL);
		if (!L_26)
		{
			goto IL_00c9;
		}
	}

IL_00b3:
	try
	{ // begin try (depth: 1)
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_27 = V_2;
		String_t* L_28 = ___targetUrl0;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_29 = (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)il2cpp_codegen_object_new(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		Uri__ctor_m41A759BF295FB902084DD289849793E01A65A14E(L_29, L_27, L_28, /*hidden argument*/NULL);
		V_3 = L_29;
		V_1 = (bool)1;
		goto IL_00c9;
	} // end try (depth: 1)
	catch(Il2CppExceptionWrapper& e)
	{
		__exception_local = (Exception_t *)e.ex;
		if(il2cpp_codegen_class_is_assignable_from (FormatException_t2808E076CDE4650AF89F55FD78F49290D0EC5BDC_il2cpp_TypeInfo_var, il2cpp_codegen_object_class(e.ex)))
			goto CATCH_00c4;
		throw e;
	}

CATCH_00c4:
	{ // begin catch(System.FormatException)
		FormatException_t2808E076CDE4650AF89F55FD78F49290D0EC5BDC * L_30 = V_4;
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_30, NULL, WebRequestUtils_MakeInitialUrl_m446CCE4EFB276BE27A9380D55B9E704D01107B83_RuntimeMethod_var);
	} // end catch (depth: 1)

IL_00c9:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_31 = V_3;
		String_t* L_32 = ___targetUrl0;
		bool L_33 = V_1;
		IL2CPP_RUNTIME_CLASS_INIT(WebRequestUtils_tBE8F8607E3A9633419968F6AF2F706A029AE1296_il2cpp_TypeInfo_var);
		String_t* L_34 = WebRequestUtils_MakeUriString_m5693EA04230335B9611278EFC189BD58339D01E4(L_31, L_32, L_33, /*hidden argument*/NULL);
		V_0 = L_34;
		goto IL_00d7;
	}

IL_00d7:
	{
		String_t* L_35 = V_0;
		return L_35;
	}
}
// System.String UnityEngineInternal.WebRequestUtils::MakeUriString(System.Uri,System.String,System.Boolean)
extern "C" IL2CPP_METHOD_ATTR String_t* WebRequestUtils_MakeUriString_m5693EA04230335B9611278EFC189BD58339D01E4 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___targetUri0, String_t* ___targetUrl1, bool ___prependProtocol2, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (WebRequestUtils_MakeUriString_m5693EA04230335B9611278EFC189BD58339D01E4_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	String_t* V_0 = NULL;
	String_t* V_1 = NULL;
	String_t* V_2 = NULL;
	StringBuilder_t * V_3 = NULL;
	String_t* V_4 = NULL;
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_0 = ___targetUri0;
		NullCheck(L_0);
		bool L_1 = Uri_get_IsFile_m06AB5A15E2A34BBC5177C6E902C5C9D7E766A213(L_0, /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_007b;
		}
	}
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_2 = ___targetUri0;
		NullCheck(L_2);
		bool L_3 = Uri_get_IsLoopback_mCD7E1228C8296730CBD31C713B0A81B660D99BC4(L_2, /*hidden argument*/NULL);
		if (L_3)
		{
			goto IL_0024;
		}
	}
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_4 = ___targetUri0;
		NullCheck(L_4);
		String_t* L_5 = Uri_get_OriginalString_m56099E46276F0A52524347F1F46A2F88E948504F(L_4, /*hidden argument*/NULL);
		V_0 = L_5;
		goto IL_01ac;
	}

IL_0024:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_6 = ___targetUri0;
		NullCheck(L_6);
		String_t* L_7 = Uri_get_AbsolutePath_mA9A825E2BBD0A43AD76EB9A9765E29E45FE32F31(L_6, /*hidden argument*/NULL);
		V_1 = L_7;
		String_t* L_8 = V_1;
		NullCheck(L_8);
		bool L_9 = String_Contains_m4488034AF8CB3EEA9A205EB8A1F25D438FF8704B(L_8, _stringLiteral4345CB1FA27885A8FBFE7C0C830A592CC76A552B, /*hidden argument*/NULL);
		if (!L_9)
		{
			goto IL_0042;
		}
	}
	{
		String_t* L_10 = V_1;
		IL2CPP_RUNTIME_CLASS_INIT(WebRequestUtils_tBE8F8607E3A9633419968F6AF2F706A029AE1296_il2cpp_TypeInfo_var);
		String_t* L_11 = WebRequestUtils_URLDecode_m3F75FA29F50FB340B93815988517E9208C52EE62(L_10, /*hidden argument*/NULL);
		V_1 = L_11;
	}

IL_0042:
	{
		String_t* L_12 = V_1;
		NullCheck(L_12);
		int32_t L_13 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_12, /*hidden argument*/NULL);
		if ((((int32_t)L_13) <= ((int32_t)0)))
		{
			goto IL_006a;
		}
	}
	{
		String_t* L_14 = V_1;
		NullCheck(L_14);
		Il2CppChar L_15 = String_get_Chars_m14308AC3B95F8C1D9F1D1055B116B37D595F1D96(L_14, 0, /*hidden argument*/NULL);
		if ((((int32_t)L_15) == ((int32_t)((int32_t)47))))
		{
			goto IL_006a;
		}
	}
	{
		Il2CppChar L_16 = ((Il2CppChar)((int32_t)47));
		RuntimeObject * L_17 = Box(Char_tBF22D9FC341BE970735250BB6FF1A4A92BBA58B9_il2cpp_TypeInfo_var, &L_16);
		String_t* L_18 = V_1;
		String_t* L_19 = String_Concat_mBB19C73816BDD1C3519F248E1ADC8E11A6FDB495(L_17, L_18, /*hidden argument*/NULL);
		V_1 = L_19;
	}

IL_006a:
	{
		String_t* L_20 = V_1;
		String_t* L_21 = String_Concat_mB78D0094592718DA6D5DB6C712A9C225631666BE(_stringLiteralA91E4897CA9F429677AFC57ED00D90DE8D3C7001, L_20, /*hidden argument*/NULL);
		V_0 = L_21;
		goto IL_01ac;
	}

IL_007b:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_22 = ___targetUri0;
		NullCheck(L_22);
		String_t* L_23 = Uri_get_Scheme_m14A8F0018D8AACADBEF39600A59944F33EE39187(L_22, /*hidden argument*/NULL);
		V_2 = L_23;
		bool L_24 = ___prependProtocol2;
		if (L_24)
		{
			goto IL_0184;
		}
	}
	{
		String_t* L_25 = ___targetUrl1;
		NullCheck(L_25);
		int32_t L_26 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_25, /*hidden argument*/NULL);
		String_t* L_27 = V_2;
		NullCheck(L_27);
		int32_t L_28 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_27, /*hidden argument*/NULL);
		if ((((int32_t)L_26) < ((int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_28, (int32_t)2)))))
		{
			goto IL_0184;
		}
	}
	{
		String_t* L_29 = ___targetUrl1;
		String_t* L_30 = V_2;
		NullCheck(L_30);
		int32_t L_31 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_30, /*hidden argument*/NULL);
		NullCheck(L_29);
		Il2CppChar L_32 = String_get_Chars_m14308AC3B95F8C1D9F1D1055B116B37D595F1D96(L_29, ((int32_t)il2cpp_codegen_add((int32_t)L_31, (int32_t)1)), /*hidden argument*/NULL);
		if ((((int32_t)L_32) == ((int32_t)((int32_t)47))))
		{
			goto IL_0184;
		}
	}
	{
		String_t* L_33 = V_2;
		String_t* L_34 = ___targetUrl1;
		NullCheck(L_34);
		int32_t L_35 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_34, /*hidden argument*/NULL);
		StringBuilder_t * L_36 = (StringBuilder_t *)il2cpp_codegen_object_new(StringBuilder_t_il2cpp_TypeInfo_var);
		StringBuilder__ctor_m786CAFE74FE0D479747A0D474BE6EBCFDA5743EA(L_36, L_33, L_35, /*hidden argument*/NULL);
		V_3 = L_36;
		StringBuilder_t * L_37 = V_3;
		NullCheck(L_37);
		StringBuilder_Append_m05C12F58ADC2D807613A9301DF438CB3CD09B75A(L_37, ((int32_t)58), /*hidden argument*/NULL);
		String_t* L_38 = V_2;
		bool L_39 = String_op_Equality_m139F0E4195AE2F856019E63B241F36F016997FCE(L_38, _stringLiteralF92E777F4341930BAD9B2422283C4680D00DBC06, /*hidden argument*/NULL);
		if (!L_39)
		{
			goto IL_015e;
		}
	}
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_40 = ___targetUri0;
		NullCheck(L_40);
		String_t* L_41 = Uri_get_AbsolutePath_mA9A825E2BBD0A43AD76EB9A9765E29E45FE32F31(L_40, /*hidden argument*/NULL);
		V_4 = L_41;
		String_t* L_42 = V_4;
		NullCheck(L_42);
		bool L_43 = String_Contains_m4488034AF8CB3EEA9A205EB8A1F25D438FF8704B(L_42, _stringLiteral4345CB1FA27885A8FBFE7C0C830A592CC76A552B, /*hidden argument*/NULL);
		if (!L_43)
		{
			goto IL_00fa;
		}
	}
	{
		String_t* L_44 = V_4;
		IL2CPP_RUNTIME_CLASS_INIT(WebRequestUtils_tBE8F8607E3A9633419968F6AF2F706A029AE1296_il2cpp_TypeInfo_var);
		String_t* L_45 = WebRequestUtils_URLDecode_m3F75FA29F50FB340B93815988517E9208C52EE62(L_44, /*hidden argument*/NULL);
		V_4 = L_45;
	}

IL_00fa:
	{
		String_t* L_46 = V_4;
		NullCheck(L_46);
		bool L_47 = String_StartsWith_m7D468FB7C801D9C2DBEEEEC86F8BA8F4EC3243C1(L_46, _stringLiteral947518D877FB275850A375D795BE6A44C27AB526, /*hidden argument*/NULL);
		if (!L_47)
		{
			goto IL_0149;
		}
	}
	{
		String_t* L_48 = V_4;
		NullCheck(L_48);
		int32_t L_49 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_48, /*hidden argument*/NULL);
		if ((((int32_t)L_49) <= ((int32_t)6)))
		{
			goto IL_0149;
		}
	}
	{
		String_t* L_50 = V_4;
		NullCheck(L_50);
		Il2CppChar L_51 = String_get_Chars_m14308AC3B95F8C1D9F1D1055B116B37D595F1D96(L_50, 6, /*hidden argument*/NULL);
		if ((((int32_t)L_51) == ((int32_t)((int32_t)47))))
		{
			goto IL_0149;
		}
	}
	{
		StringBuilder_t * L_52 = V_3;
		NullCheck(L_52);
		StringBuilder_Append_mDBB8CCBB7750C67BE2F2D92F47E6C0FA42793260(L_52, _stringLiteralA91E4897CA9F429677AFC57ED00D90DE8D3C7001, /*hidden argument*/NULL);
		StringBuilder_t * L_53 = V_3;
		String_t* L_54 = V_4;
		NullCheck(L_54);
		String_t* L_55 = String_Substring_m2C4AFF5E79DD8BADFD2DFBCF156BF728FBB8E1AE(L_54, 5, /*hidden argument*/NULL);
		NullCheck(L_53);
		StringBuilder_Append_mDBB8CCBB7750C67BE2F2D92F47E6C0FA42793260(L_53, L_55, /*hidden argument*/NULL);
		goto IL_0152;
	}

IL_0149:
	{
		StringBuilder_t * L_56 = V_3;
		String_t* L_57 = V_4;
		NullCheck(L_56);
		StringBuilder_Append_mDBB8CCBB7750C67BE2F2D92F47E6C0FA42793260(L_56, L_57, /*hidden argument*/NULL);
	}

IL_0152:
	{
		StringBuilder_t * L_58 = V_3;
		NullCheck(L_58);
		String_t* L_59 = VirtFuncInvoker0< String_t* >::Invoke(3 /* System.String System.Object::ToString() */, L_58);
		V_0 = L_59;
		goto IL_01ac;
	}

IL_015e:
	{
		StringBuilder_t * L_60 = V_3;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_61 = ___targetUri0;
		NullCheck(L_61);
		String_t* L_62 = Uri_get_PathAndQuery_mF079BA04B7A397B2729E5B5DEE72B3654A44E384(L_61, /*hidden argument*/NULL);
		NullCheck(L_60);
		StringBuilder_Append_mDBB8CCBB7750C67BE2F2D92F47E6C0FA42793260(L_60, L_62, /*hidden argument*/NULL);
		StringBuilder_t * L_63 = V_3;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_64 = ___targetUri0;
		NullCheck(L_64);
		String_t* L_65 = Uri_get_Fragment_m111666DD668AC59B9F3C3D3CEEEC7F70F6904D41(L_64, /*hidden argument*/NULL);
		NullCheck(L_63);
		StringBuilder_Append_mDBB8CCBB7750C67BE2F2D92F47E6C0FA42793260(L_63, L_65, /*hidden argument*/NULL);
		StringBuilder_t * L_66 = V_3;
		NullCheck(L_66);
		String_t* L_67 = VirtFuncInvoker0< String_t* >::Invoke(3 /* System.String System.Object::ToString() */, L_66);
		V_0 = L_67;
		goto IL_01ac;
	}

IL_0184:
	{
		String_t* L_68 = ___targetUrl1;
		NullCheck(L_68);
		bool L_69 = String_Contains_m4488034AF8CB3EEA9A205EB8A1F25D438FF8704B(L_68, _stringLiteral4345CB1FA27885A8FBFE7C0C830A592CC76A552B, /*hidden argument*/NULL);
		if (!L_69)
		{
			goto IL_01a0;
		}
	}
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_70 = ___targetUri0;
		NullCheck(L_70);
		String_t* L_71 = Uri_get_OriginalString_m56099E46276F0A52524347F1F46A2F88E948504F(L_70, /*hidden argument*/NULL);
		V_0 = L_71;
		goto IL_01ac;
	}

IL_01a0:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_72 = ___targetUri0;
		NullCheck(L_72);
		String_t* L_73 = Uri_get_AbsoluteUri_m4326730E572E7E3874021E802813EB6F49F7F99E(L_72, /*hidden argument*/NULL);
		V_0 = L_73;
		goto IL_01ac;
	}

IL_01ac:
	{
		String_t* L_74 = V_0;
		return L_74;
	}
}
// System.String UnityEngineInternal.WebRequestUtils::URLDecode(System.String)
extern "C" IL2CPP_METHOD_ATTR String_t* WebRequestUtils_URLDecode_m3F75FA29F50FB340B93815988517E9208C52EE62 (String_t* ___encoded0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (WebRequestUtils_URLDecode_m3F75FA29F50FB340B93815988517E9208C52EE62_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* V_0 = NULL;
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* V_1 = NULL;
	String_t* V_2 = NULL;
	{
		Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_0 = Encoding_get_UTF8_m67C8652936B681E7BC7505E459E88790E0FF16D9(/*hidden argument*/NULL);
		String_t* L_1 = ___encoded0;
		NullCheck(L_0);
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_2 = VirtFuncInvoker1< ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821*, String_t* >::Invoke(17 /* System.Byte[] System.Text.Encoding::GetBytes(System.String) */, L_0, L_1);
		V_0 = L_2;
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_3 = V_0;
		IL2CPP_RUNTIME_CLASS_INIT(WWWTranscoder_t0B24F1F17629756E6464A925870CC39236F39C61_il2cpp_TypeInfo_var);
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_4 = WWWTranscoder_URLDecode_m591A567154B1B8737ECBFE065AF4FCA59217F5D8(L_3, /*hidden argument*/NULL);
		V_1 = L_4;
		Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_5 = Encoding_get_UTF8_m67C8652936B681E7BC7505E459E88790E0FF16D9(/*hidden argument*/NULL);
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_6 = V_1;
		NullCheck(L_5);
		String_t* L_7 = VirtFuncInvoker1< String_t*, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* >::Invoke(34 /* System.String System.Text.Encoding::GetString(System.Byte[]) */, L_5, L_6);
		V_2 = L_7;
		goto IL_0025;
	}

IL_0025:
	{
		String_t* L_8 = V_2;
		return L_8;
	}
}
// System.Void UnityEngineInternal.WebRequestUtils::.cctor()
extern "C" IL2CPP_METHOD_ATTR void WebRequestUtils__cctor_m31EB3E45EC49AB6B33C7A10F79F1CD4FF2BE715A (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (WebRequestUtils__cctor_m31EB3E45EC49AB6B33C7A10F79F1CD4FF2BE715A_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF * L_0 = (Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF *)il2cpp_codegen_object_new(Regex_tFD46E63A462E852189FD6AB4E2B0B67C4D8FDBDF_il2cpp_TypeInfo_var);
		Regex__ctor_m2769A5BA7B7A835514F6C0E4D30FAD467C6B1B0C(L_0, _stringLiteral56F03F5F25FB2048BF4AB5FBBF7B5E3D39A3ECEB, /*hidden argument*/NULL);
		((WebRequestUtils_tBE8F8607E3A9633419968F6AF2F706A029AE1296_StaticFields*)il2cpp_codegen_static_fields_for(WebRequestUtils_tBE8F8607E3A9633419968F6AF2F706A029AE1296_il2cpp_TypeInfo_var))->set_domainRegex_0(L_0);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
