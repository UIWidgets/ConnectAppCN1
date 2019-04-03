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
template <typename R, typename T1, typename T2, typename T3, typename T4>
struct VirtFuncInvoker4
{
	typedef R (*Func)(void*, T1, T2, T3, T4, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1, T2 p2, T3 p3, T4 p4)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj, p1, p2, p3, p4, invokeData.method);
	}
};
template <typename R, typename T1, typename T2, typename T3, typename T4, typename T5>
struct VirtFuncInvoker5
{
	typedef R (*Func)(void*, T1, T2, T3, T4, T5, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj, p1, p2, p3, p4, p5, invokeData.method);
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
template <typename T1, typename T2>
struct VirtActionInvoker2
{
	typedef void (*Action)(void*, T1, T2, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1, T2 p2)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		((Action)invokeData.methodPtr)(obj, p1, p2, invokeData.method);
	}
};
template <typename R, typename T1, typename T2>
struct VirtFuncInvoker2
{
	typedef R (*Func)(void*, T1, T2, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1, T2 p2)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj, p1, p2, invokeData.method);
	}
};

// System.ArgumentException
struct ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1;
// System.ArgumentNullException
struct ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD;
// System.ArgumentOutOfRangeException
struct ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA;
// System.Byte[]
struct ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821;
// System.Char[]
struct CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2;
// System.Collections.Generic.Dictionary`2/Entry<System.String,System.UriParser>[]
struct EntryU5BU5D_t78690744AC973DECF2010068DBDBD973FD216AAF;
// System.Collections.Generic.Dictionary`2/KeyCollection<System.String,System.UriParser>
struct KeyCollection_t0A494A02669573F9DB0645810A3CE95699AF12CF;
// System.Collections.Generic.Dictionary`2/ValueCollection<System.String,System.UriParser>
struct ValueCollection_tB32C5B99C1808F9DF958AF03D289C64F31A50E38;
// System.Collections.Generic.Dictionary`2<System.Int32,System.Globalization.CultureInfo>
struct Dictionary_2_tC88A56872F7C79DBB9582D4F3FC22ED5D8E0B98B;
// System.Collections.Generic.Dictionary`2<System.Object,System.Object>
struct Dictionary_2_t32F25F093828AA9F93CB11C2A2B4648FD62A09BA;
// System.Collections.Generic.Dictionary`2<System.String,System.Globalization.CultureInfo>
struct Dictionary_2_tBA5388DBB42BF620266F9A48E8B859BBBB224E25;
// System.Collections.Generic.Dictionary`2<System.String,System.Int32>
struct Dictionary_2_tD6E204872BA9FD506A0287EF68E285BEB9EC0DFB;
// System.Collections.Generic.Dictionary`2<System.String,System.UriParser>
struct Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE;
// System.Collections.Generic.IEqualityComparer`1<System.String>
struct IEqualityComparer_1_t1F07EAC22CC1D4F279164B144240E4718BD7E7A9;
// System.Collections.Hashtable
struct Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9;
// System.Collections.IDictionary
struct IDictionary_t1BD5C1546718A374EA8122FBD6C6EE45331E8CE7;
// System.ComponentModel.ITypeDescriptorContext
struct ITypeDescriptorContext_tE299A513DA3526C32BFAC7D1FDFFC55AFB1D0CD6;
// System.ComponentModel.TypeConverter
struct TypeConverter_t8306AE03734853B551DDF089C1F17836A7764DBB;
// System.Diagnostics.StackTrace[]
struct StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196;
// System.Exception
struct Exception_t;
// System.FormatException
struct FormatException_t2808E076CDE4650AF89F55FD78F49290D0EC5BDC;
// System.Globalization.Calendar
struct Calendar_tF55A785ACD277504CF0D2F2C6AD56F76C6E91BD5;
// System.Globalization.CodePageDataItem
struct CodePageDataItem_t6E34BEE9CCCBB35C88D714664633AF6E5F5671FB;
// System.Globalization.CompareInfo
struct CompareInfo_tB9A071DBC11AC00AF2EA2066D0C2AE1DCB1865D1;
// System.Globalization.CultureData
struct CultureData_tF43B080FFA6EB278F4F289BCDA3FB74B6C208ECD;
// System.Globalization.CultureInfo
struct CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F;
// System.Globalization.DateTimeFormatInfo
struct DateTimeFormatInfo_tF4BB3AA482C2F772D2A9022F78BF8727830FAF5F;
// System.Globalization.NumberFormatInfo
struct NumberFormatInfo_tFDF57037EBC5BC833D0A53EF0327B805994860A8;
// System.Globalization.TextInfo
struct TextInfo_t5F1E697CB6A7E5EC80F0DC3A968B9B4A70C291D8;
// System.Int32[]
struct Int32U5BU5D_t2B9E4FDDDB9F0A00EC0AC631BA2DA915EB1ECF83;
// System.IntPtr[]
struct IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD;
// System.InvalidOperationException
struct InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1;
// System.NotSupportedException
struct NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010;
// System.Object[]
struct ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A;
// System.PlatformNotSupportedException
struct PlatformNotSupportedException_t14FE109377F8FA8B3B2F9A0C4FE3BF10662C73B5;
// System.Reflection.Binder
struct Binder_t4D5CB06963501D32847C057B57157D6DC49CA759;
// System.Reflection.MemberFilter
struct MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381;
// System.Runtime.Serialization.IFormatterConverter
struct IFormatterConverter_tC3280D64D358F47EA4DAF1A65609BA0FC081888A;
// System.Runtime.Serialization.SafeSerializationManager
struct SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770;
// System.Runtime.Serialization.SerializationInfo
struct SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26;
// System.String
struct String_t;
// System.String[]
struct StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E;
// System.Text.DecoderFallback
struct DecoderFallback_t128445EB7676870485230893338EF044F6B72F60;
// System.Text.DecoderReplacementFallback
struct DecoderReplacementFallback_t8CF74B2DAE2A08AEA7DF6366778D2E3EA75FC742;
// System.Text.EncoderFallback
struct EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63;
// System.Text.EncoderReplacementFallback
struct EncoderReplacementFallback_tC2E8A94C82BBF7A4CFC8E3FDBA8A381DCF29F998;
// System.Text.Encoding
struct Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4;
// System.Type
struct Type_t;
// System.Type[]
struct TypeU5BU5D_t7FE623A666B49176DE123306221193E888A12F5F;
// System.Uri
struct Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E;
// System.Uri/MoreInfo
struct MoreInfo_t83B9EC79244C26B468C115E54C0BEF09BB2E05B5;
// System.Uri/UriInfo
struct UriInfo_t9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E;
// System.UriBuilder
struct UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905;
// System.UriFormatException
struct UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A;
// System.UriParser
struct UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC;
// System.UriParser/BuiltInUriParser
struct BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B;
// System.UriTypeConverter
struct UriTypeConverter_t96793526764A246FBAEE2F4F639AFAF270EE81D1;
// System.Void
struct Void_t22962CB4C05B1D89B55A6E1139F0E87A90987017;

extern RuntimeClass* ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1_il2cpp_TypeInfo_var;
extern RuntimeClass* ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD_il2cpp_TypeInfo_var;
extern RuntimeClass* ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA_il2cpp_TypeInfo_var;
extern RuntimeClass* BinaryCompatibility_t06B1B8D34764DB1710459778EB22433728A665A8_il2cpp_TypeInfo_var;
extern RuntimeClass* BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var;
extern RuntimeClass* ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821_il2cpp_TypeInfo_var;
extern RuntimeClass* CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2_il2cpp_TypeInfo_var;
extern RuntimeClass* Char_tBF22D9FC341BE970735250BB6FF1A4A92BBA58B9_il2cpp_TypeInfo_var;
extern RuntimeClass* DecoderReplacementFallback_t8CF74B2DAE2A08AEA7DF6366778D2E3EA75FC742_il2cpp_TypeInfo_var;
extern RuntimeClass* Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE_il2cpp_TypeInfo_var;
extern RuntimeClass* EncoderReplacementFallback_tC2E8A94C82BBF7A4CFC8E3FDBA8A381DCF29F998_il2cpp_TypeInfo_var;
extern RuntimeClass* Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4_il2cpp_TypeInfo_var;
extern RuntimeClass* Exception_t_il2cpp_TypeInfo_var;
extern RuntimeClass* InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var;
extern RuntimeClass* Math_tFB388E53C7FDC6FCCF9A19ABF5A4E521FBD52E19_il2cpp_TypeInfo_var;
extern RuntimeClass* NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010_il2cpp_TypeInfo_var;
extern RuntimeClass* ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A_il2cpp_TypeInfo_var;
extern RuntimeClass* OutOfMemoryException_t2DF3EAC178583BD1DEFAAECBEDB2AF1EA86FBFC7_il2cpp_TypeInfo_var;
extern RuntimeClass* PlatformNotSupportedException_t14FE109377F8FA8B3B2F9A0C4FE3BF10662C73B5_il2cpp_TypeInfo_var;
extern RuntimeClass* StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E_il2cpp_TypeInfo_var;
extern RuntimeClass* String_t_il2cpp_TypeInfo_var;
extern RuntimeClass* TypeConverter_t8306AE03734853B551DDF089C1F17836A7764DBB_il2cpp_TypeInfo_var;
extern RuntimeClass* Type_t_il2cpp_TypeInfo_var;
extern RuntimeClass* UriComponents_tE42D5229291668DE73323E1C519E4E1459A64CFF_il2cpp_TypeInfo_var;
extern RuntimeClass* UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A_il2cpp_TypeInfo_var;
extern RuntimeClass* UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var;
extern RuntimeClass* UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var;
extern RuntimeClass* Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var;
extern RuntimeField* U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291____59F5BD34B6C013DEACC784F69C67E95150033A84_6_FieldInfo_var;
extern String_t* _stringLiteral05A79F06CF3F67F726DAE68D18A2290F6C9A50C9;
extern String_t* _stringLiteral0765DEEFD5C1509444309BD8D09E7ACA927165F8;
extern String_t* _stringLiteral11C5773832E60D2F376C6E197271A225FD74EE27;
extern String_t* _stringLiteral12B6FF7C47BB4C2C973EF6E38B06B1AD0DACA96F;
extern String_t* _stringLiteral1457B75DC8C5500C0F1D4503CF801B60DEB045A4;
extern String_t* _stringLiteral1E5C2F367F02E47A8C160CDA1CD9D91DECBAC441;
extern String_t* _stringLiteral1F8A1C4B94F61170B94E9FD827F36A60174238C7;
extern String_t* _stringLiteral2028E589D6BB0C12D880EFA6E4DAB4AF32821B19;
extern String_t* _stringLiteral22E9F56882C87C3DA193BE3FE6D8C77FFDAF27BC;
extern String_t* _stringLiteral2B2243B6036E7AC7834F59C17B6FBD1E6AB6D2CF;
extern String_t* _stringLiteral2C6D680F5C570BA21D22697CD028F230E9F4CD56;
extern String_t* _stringLiteral2E0BECBCAE1D61359E07C21D53B187CD25DCC4B1;
extern String_t* _stringLiteral334389048B872A533002B34D73F8C29FD09EFC50;
extern String_t* _stringLiteral3AE3AD09884E848958DF67AFEC6B436733CEB84C;
extern String_t* _stringLiteral3C6BDCDDC94F64BF77DEB306AAE490A90A6FC300;
extern String_t* _stringLiteral4188736A00FBFB506ACA06281ACF338290455C21;
extern String_t* _stringLiteral42099B4AF021E53FD8FD4E056C2568D7C2E3FFA8;
extern String_t* _stringLiteral422C2FC455DA8AB1CCF099E014DADE733913E48A;
extern String_t* _stringLiteral4321CB5243173998A1F5A7D3F9F1C39DB3F00458;
extern String_t* _stringLiteral48E3462CBEEDD9B70CED95702E2E262CEBA217DA;
extern String_t* _stringLiteral4931F5B26E4E3B67A69DCEAE7622810683E83201;
extern String_t* _stringLiteral4FF447B8EF42CA51FA6FB287BED8D40F49BE58F1;
extern String_t* _stringLiteral57E68B8AF3FD3A50C789D0A6C6B204E28654550B;
extern String_t* _stringLiteral5BAB61EB53176449E25C2C82F172B82CB13FFB9D;
extern String_t* _stringLiteral5D7FEFA52F916FB1F734F27D1226BA1556F23E16;
extern String_t* _stringLiteral5E6A1BC91A4C36E5A0E45B3C8F8A2CF3F48785C5;
extern String_t* _stringLiteral61A135089EAC561A2FF7CEDEEFB03975BED000F8;
extern String_t* _stringLiteral666948CC54CBC3FC2C70107A835E27C872F476E6;
extern String_t* _stringLiteral685AA46800DA1134A27CF09D92AB8FB9481ABE68;
extern String_t* _stringLiteral6ABF563E8335FCAA5CA55811FECE36F4B0D6DC07;
extern String_t* _stringLiteral7608E1FF0B8CFEF39D687771BAC4DCB767C2C102;
extern String_t* _stringLiteral7616BB87BD05F6439E3672BA1B2BE55D5BEB68B3;
extern String_t* _stringLiteral77B5F8E343A90F6F597751021FB8B7A08FE83083;
extern String_t* _stringLiteral785987648F85190CFDE9EADC69FC7C46FE8A7433;
extern String_t* _stringLiteral8313799DB2EC33E29A24C7AA3B2B19EE6B301F73;
extern String_t* _stringLiteral971C419DD609331343DEE105FFFD0F4608DC0BF2;
extern String_t* _stringLiteral9A78211436F6D425EC38F5C4E02270801F3524F8;
extern String_t* _stringLiteralBA2B0DD158763C472A7D7B22AEF6FF6571B9365C;
extern String_t* _stringLiteralC212F08ED1157AE268FD83D142AFD5CCD48664B2;
extern String_t* _stringLiteralC3437DBC7C1255D3A21D444D86EBF2E9234C22BD;
extern String_t* _stringLiteralD08F88DF745FA7950B104E4A707A31CFCE7B5841;
extern String_t* _stringLiteralDA39A3EE5E6B4B0D3255BFEF95601890AFD80709;
extern String_t* _stringLiteralE3D9B2CC0C1CB7037696A2D9C2B9B4C1FEF5EB9B;
extern String_t* _stringLiteralF32B67C7E26342AF42EFABC674D441DCA0A281C5;
extern String_t* _stringLiteralFE710CD089CB0BA74F588270FE079A392B5E9810;
extern const RuntimeMethod* Dictionary_2_TryGetValue_mB7FEE5E187FD932CA98FA958AFCC096E123BCDC4_RuntimeMethod_var;
extern const RuntimeMethod* Dictionary_2__ctor_m9AA6FFC23A9032DF2BF483986951F06E722B3445_RuntimeMethod_var;
extern const RuntimeMethod* Dictionary_2_get_Count_mEC5A51E9EC624CA697AFE307D4CD767026962AE3_RuntimeMethod_var;
extern const RuntimeMethod* Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var;
extern const RuntimeMethod* ThrowStub_ThrowNotSupportedException_mF1DE187697F740D8C18B8966BBEB276878CD86FD_RuntimeMethod_var;
extern const RuntimeMethod* UriBuilder_ToString_m5BF9ED358C61008561663680C0D843C22C25443D_RuntimeMethod_var;
extern const RuntimeMethod* UriBuilder__ctor_m1B050A706B91D8EDCF5DD4A98CA1F1A0FE6EA8AE_RuntimeMethod_var;
extern const RuntimeMethod* UriBuilder__ctor_m5B1EA7F0F855706B9725EAE9EFE4A3DE5DB97D50_RuntimeMethod_var;
extern const RuntimeMethod* UriBuilder_set_Extra_mBA671DFDA9152422D2C52BD7087F78ACB3A3673C_RuntimeMethod_var;
extern const RuntimeMethod* UriBuilder_set_Port_m14DBA6E597BED983B73F4AD7F1215C6E474DB6F3_RuntimeMethod_var;
extern const RuntimeMethod* UriBuilder_set_Scheme_mD20C10C2D43C0C2C96D9098BE4331D46FCC45921_RuntimeMethod_var;
extern const RuntimeMethod* UriHelper_EscapeString_mF0077A016F05127923308DF7E7E99BD7B9837E8B_RuntimeMethod_var;
extern const RuntimeMethod* UriHelper_UnescapeString_mD4815AEAF34E25D31AA4BB4A76B88055F0A49E89_RuntimeMethod_var;
extern const RuntimeMethod* UriParser_GetComponents_m8A226F43638FA7CD135A651CDE3D4E475E8FC181_RuntimeMethod_var;
extern const RuntimeMethod* UriParser_Resolve_mF21D3AA42AB1EC2B173617D76E4041EB3481D979_RuntimeMethod_var;
extern const RuntimeMethod* UriTypeConverter_CanConvertFrom_m1D18F7B5924B6B682AB1CC90FB814DC3331DFF47_RuntimeMethod_var;
extern const RuntimeMethod* UriTypeConverter_ConvertFrom_m2FE8479F26F35A578983E194038CF186D6CD2F85_RuntimeMethod_var;
extern const RuntimeMethod* UriTypeConverter_ConvertTo_m2059A4086714BACA32E7618BD97713CCD5DCFEF4_RuntimeMethod_var;
extern const RuntimeType* String_t_0_0_0_var;
extern const RuntimeType* Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_0_0_0_var;
extern const uint32_t BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C_MetadataUsageId;
extern const uint32_t ThrowStub_ThrowNotSupportedException_mF1DE187697F740D8C18B8966BBEB276878CD86FD_MetadataUsageId;
extern const uint32_t UriBuilder_Init_mB18B3A4578F102E7E99F18542236A6B5B6ABA174_MetadataUsageId;
extern const uint32_t UriBuilder_SetFieldsFromUri_m54B4EB1ACEF01F2B0B11EC81768BB7D56245447F_MetadataUsageId;
extern const uint32_t UriBuilder_ToString_m5BF9ED358C61008561663680C0D843C22C25443D_MetadataUsageId;
extern const uint32_t UriBuilder__ctor_m1B050A706B91D8EDCF5DD4A98CA1F1A0FE6EA8AE_MetadataUsageId;
extern const uint32_t UriBuilder__ctor_m5B1EA7F0F855706B9725EAE9EFE4A3DE5DB97D50_MetadataUsageId;
extern const uint32_t UriBuilder__ctor_m7A9B7FFE61632B2181BBF326580950494257464F_MetadataUsageId;
extern const uint32_t UriBuilder__ctor_mFC8729DAB4291B5B6500207C85DCB3985B46BB52_MetadataUsageId;
extern const uint32_t UriBuilder_get_Uri_mDCABA4CD1D05D4B9C4CBA063BC7CA94EE6CCC631_MetadataUsageId;
extern const uint32_t UriBuilder_set_Extra_mBA671DFDA9152422D2C52BD7087F78ACB3A3673C_MetadataUsageId;
extern const uint32_t UriBuilder_set_Fragment_m2283E0E13EE8BBA2F4FFA10CDAD1EA2D6771EE11_MetadataUsageId;
extern const uint32_t UriBuilder_set_Host_m7213BE98F62DE6A099EA8EEFF479949C5F1EA680_MetadataUsageId;
extern const uint32_t UriBuilder_set_Path_mB5E891CD6B419F1310178B20F5E47E49D0F828E8_MetadataUsageId;
extern const uint32_t UriBuilder_set_Port_m14DBA6E597BED983B73F4AD7F1215C6E474DB6F3_MetadataUsageId;
extern const uint32_t UriBuilder_set_Query_m392BC383004E6922D6B53870D2195E7F871FCEC8_MetadataUsageId;
extern const uint32_t UriBuilder_set_Scheme_mD20C10C2D43C0C2C96D9098BE4331D46FCC45921_MetadataUsageId;
extern const uint32_t UriHelper_EnsureDestinationSize_m64F4907D0411AAAD1C05E0AD0D2EB120DCBA9217_MetadataUsageId;
extern const uint32_t UriHelper_EscapeAsciiChar_mFD7DE796BD53CBD2B1E73080FE0346D37F358902_MetadataUsageId;
extern const uint32_t UriHelper_EscapeString_mF0077A016F05127923308DF7E7E99BD7B9837E8B_MetadataUsageId;
extern const uint32_t UriHelper_Is3986Unreserved_m3799F2ADA8C63DDB4995F82B974C8EC1DEEBA76A_MetadataUsageId;
extern const uint32_t UriHelper_IsReservedUnreservedOrHash_m3D7256DABA7F540F8D379FC1D1C54F1C63E46059_MetadataUsageId;
extern const uint32_t UriHelper_IsUnreserved_mAADC7DCEEA864AFB49311696ABBDD76811FAAE48_MetadataUsageId;
extern const uint32_t UriHelper_MatchUTF8Sequence_m4835D9BB77C2701643B14D6FFD3D7057F8C9007F_MetadataUsageId;
extern const uint32_t UriHelper_UnescapeString_mC172F713349E3D22985A92BC4F5B51D0BCEE61AF_MetadataUsageId;
extern const uint32_t UriHelper_UnescapeString_mD4815AEAF34E25D31AA4BB4A76B88055F0A49E89_MetadataUsageId;
extern const uint32_t UriHelper__cctor_m9537B8AAAA1D6EF77D29A179EC79F5511C662F27_MetadataUsageId;
extern const uint32_t UriParser_FindOrFetchAsUnknownV1Syntax_m3A57CA15FE27DC7982F186E8321B810B56EBD9AD_MetadataUsageId;
extern const uint32_t UriParser_GetComponents_m8A226F43638FA7CD135A651CDE3D4E475E8FC181_MetadataUsageId;
extern const uint32_t UriParser_GetSyntax_mC2FEAF79ECEB6550573A1C0578141BB236F7EF16_MetadataUsageId;
extern const uint32_t UriParser_Resolve_mF21D3AA42AB1EC2B173617D76E4041EB3481D979_MetadataUsageId;
extern const uint32_t UriParser__cctor_m00C2855D5C8C07790C5627BBB90AC84A7E8B6BC2_MetadataUsageId;
extern const uint32_t UriParser__ctor_mAF168F2B88BC5301B722C1BAAD45E381FBA22E3D_MetadataUsageId;
extern const uint32_t UriParser_get_ShouldUseLegacyV2Quirks_mD4C8DF67677ACCCC3B5E026099ECC0BDA24D96DD_MetadataUsageId;
extern const uint32_t UriTypeConverter_CanConvertFrom_m1D18F7B5924B6B682AB1CC90FB814DC3331DFF47_MetadataUsageId;
extern const uint32_t UriTypeConverter_CanConvertTo_mC19530C1DD75AC92C20697EFDD0A0E2DB568E099_MetadataUsageId;
extern const uint32_t UriTypeConverter_CanConvert_m0F0FB34A1DC16C677BF8F4ED0E720144C17C4795_MetadataUsageId;
extern const uint32_t UriTypeConverter_ConvertFrom_m2FE8479F26F35A578983E194038CF186D6CD2F85_MetadataUsageId;
extern const uint32_t UriTypeConverter_ConvertTo_m2059A4086714BACA32E7618BD97713CCD5DCFEF4_MetadataUsageId;
extern const uint32_t UriTypeConverter__ctor_m1CAEEF1C615B28212B83C76D892938E0A77D3A64_MetadataUsageId;
struct CultureData_tF43B080FFA6EB278F4F289BCDA3FB74B6C208ECD_marshaled_com;
struct CultureData_tF43B080FFA6EB278F4F289BCDA3FB74B6C208ECD_marshaled_pinvoke;
struct CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F_marshaled_com;
struct CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F_marshaled_pinvoke;
struct Exception_t_marshaled_com;
struct Exception_t_marshaled_pinvoke;

struct ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821;
struct CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2;
struct ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A;
struct StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E;


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
#ifndef DICTIONARY_2_TB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE_H
#define DICTIONARY_2_TB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Generic.Dictionary`2<System.String,System.UriParser>
struct  Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE  : public RuntimeObject
{
public:
	// System.Int32[] System.Collections.Generic.Dictionary`2::buckets
	Int32U5BU5D_t2B9E4FDDDB9F0A00EC0AC631BA2DA915EB1ECF83* ___buckets_0;
	// System.Collections.Generic.Dictionary`2_Entry<TKey,TValue>[] System.Collections.Generic.Dictionary`2::entries
	EntryU5BU5D_t78690744AC973DECF2010068DBDBD973FD216AAF* ___entries_1;
	// System.Int32 System.Collections.Generic.Dictionary`2::count
	int32_t ___count_2;
	// System.Int32 System.Collections.Generic.Dictionary`2::version
	int32_t ___version_3;
	// System.Int32 System.Collections.Generic.Dictionary`2::freeList
	int32_t ___freeList_4;
	// System.Int32 System.Collections.Generic.Dictionary`2::freeCount
	int32_t ___freeCount_5;
	// System.Collections.Generic.IEqualityComparer`1<TKey> System.Collections.Generic.Dictionary`2::comparer
	RuntimeObject* ___comparer_6;
	// System.Collections.Generic.Dictionary`2_KeyCollection<TKey,TValue> System.Collections.Generic.Dictionary`2::keys
	KeyCollection_t0A494A02669573F9DB0645810A3CE95699AF12CF * ___keys_7;
	// System.Collections.Generic.Dictionary`2_ValueCollection<TKey,TValue> System.Collections.Generic.Dictionary`2::values
	ValueCollection_tB32C5B99C1808F9DF958AF03D289C64F31A50E38 * ___values_8;
	// System.Object System.Collections.Generic.Dictionary`2::_syncRoot
	RuntimeObject * ____syncRoot_9;

public:
	inline static int32_t get_offset_of_buckets_0() { return static_cast<int32_t>(offsetof(Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE, ___buckets_0)); }
	inline Int32U5BU5D_t2B9E4FDDDB9F0A00EC0AC631BA2DA915EB1ECF83* get_buckets_0() const { return ___buckets_0; }
	inline Int32U5BU5D_t2B9E4FDDDB9F0A00EC0AC631BA2DA915EB1ECF83** get_address_of_buckets_0() { return &___buckets_0; }
	inline void set_buckets_0(Int32U5BU5D_t2B9E4FDDDB9F0A00EC0AC631BA2DA915EB1ECF83* value)
	{
		___buckets_0 = value;
		Il2CppCodeGenWriteBarrier((&___buckets_0), value);
	}

	inline static int32_t get_offset_of_entries_1() { return static_cast<int32_t>(offsetof(Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE, ___entries_1)); }
	inline EntryU5BU5D_t78690744AC973DECF2010068DBDBD973FD216AAF* get_entries_1() const { return ___entries_1; }
	inline EntryU5BU5D_t78690744AC973DECF2010068DBDBD973FD216AAF** get_address_of_entries_1() { return &___entries_1; }
	inline void set_entries_1(EntryU5BU5D_t78690744AC973DECF2010068DBDBD973FD216AAF* value)
	{
		___entries_1 = value;
		Il2CppCodeGenWriteBarrier((&___entries_1), value);
	}

	inline static int32_t get_offset_of_count_2() { return static_cast<int32_t>(offsetof(Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE, ___count_2)); }
	inline int32_t get_count_2() const { return ___count_2; }
	inline int32_t* get_address_of_count_2() { return &___count_2; }
	inline void set_count_2(int32_t value)
	{
		___count_2 = value;
	}

	inline static int32_t get_offset_of_version_3() { return static_cast<int32_t>(offsetof(Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE, ___version_3)); }
	inline int32_t get_version_3() const { return ___version_3; }
	inline int32_t* get_address_of_version_3() { return &___version_3; }
	inline void set_version_3(int32_t value)
	{
		___version_3 = value;
	}

	inline static int32_t get_offset_of_freeList_4() { return static_cast<int32_t>(offsetof(Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE, ___freeList_4)); }
	inline int32_t get_freeList_4() const { return ___freeList_4; }
	inline int32_t* get_address_of_freeList_4() { return &___freeList_4; }
	inline void set_freeList_4(int32_t value)
	{
		___freeList_4 = value;
	}

	inline static int32_t get_offset_of_freeCount_5() { return static_cast<int32_t>(offsetof(Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE, ___freeCount_5)); }
	inline int32_t get_freeCount_5() const { return ___freeCount_5; }
	inline int32_t* get_address_of_freeCount_5() { return &___freeCount_5; }
	inline void set_freeCount_5(int32_t value)
	{
		___freeCount_5 = value;
	}

	inline static int32_t get_offset_of_comparer_6() { return static_cast<int32_t>(offsetof(Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE, ___comparer_6)); }
	inline RuntimeObject* get_comparer_6() const { return ___comparer_6; }
	inline RuntimeObject** get_address_of_comparer_6() { return &___comparer_6; }
	inline void set_comparer_6(RuntimeObject* value)
	{
		___comparer_6 = value;
		Il2CppCodeGenWriteBarrier((&___comparer_6), value);
	}

	inline static int32_t get_offset_of_keys_7() { return static_cast<int32_t>(offsetof(Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE, ___keys_7)); }
	inline KeyCollection_t0A494A02669573F9DB0645810A3CE95699AF12CF * get_keys_7() const { return ___keys_7; }
	inline KeyCollection_t0A494A02669573F9DB0645810A3CE95699AF12CF ** get_address_of_keys_7() { return &___keys_7; }
	inline void set_keys_7(KeyCollection_t0A494A02669573F9DB0645810A3CE95699AF12CF * value)
	{
		___keys_7 = value;
		Il2CppCodeGenWriteBarrier((&___keys_7), value);
	}

	inline static int32_t get_offset_of_values_8() { return static_cast<int32_t>(offsetof(Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE, ___values_8)); }
	inline ValueCollection_tB32C5B99C1808F9DF958AF03D289C64F31A50E38 * get_values_8() const { return ___values_8; }
	inline ValueCollection_tB32C5B99C1808F9DF958AF03D289C64F31A50E38 ** get_address_of_values_8() { return &___values_8; }
	inline void set_values_8(ValueCollection_tB32C5B99C1808F9DF958AF03D289C64F31A50E38 * value)
	{
		___values_8 = value;
		Il2CppCodeGenWriteBarrier((&___values_8), value);
	}

	inline static int32_t get_offset_of__syncRoot_9() { return static_cast<int32_t>(offsetof(Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE, ____syncRoot_9)); }
	inline RuntimeObject * get__syncRoot_9() const { return ____syncRoot_9; }
	inline RuntimeObject ** get_address_of__syncRoot_9() { return &____syncRoot_9; }
	inline void set__syncRoot_9(RuntimeObject * value)
	{
		____syncRoot_9 = value;
		Il2CppCodeGenWriteBarrier((&____syncRoot_9), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // DICTIONARY_2_TB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE_H
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
#ifndef CULTUREINFO_T345AC6924134F039ED9A11F3E03F8E91B6A3225F_H
#define CULTUREINFO_T345AC6924134F039ED9A11F3E03F8E91B6A3225F_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Globalization.CultureInfo
struct  CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F  : public RuntimeObject
{
public:
	// System.Boolean System.Globalization.CultureInfo::m_isReadOnly
	bool ___m_isReadOnly_3;
	// System.Int32 System.Globalization.CultureInfo::cultureID
	int32_t ___cultureID_4;
	// System.Int32 System.Globalization.CultureInfo::parent_lcid
	int32_t ___parent_lcid_5;
	// System.Int32 System.Globalization.CultureInfo::datetime_index
	int32_t ___datetime_index_6;
	// System.Int32 System.Globalization.CultureInfo::number_index
	int32_t ___number_index_7;
	// System.Int32 System.Globalization.CultureInfo::default_calendar_type
	int32_t ___default_calendar_type_8;
	// System.Boolean System.Globalization.CultureInfo::m_useUserOverride
	bool ___m_useUserOverride_9;
	// System.Globalization.NumberFormatInfo modreq(System.Runtime.CompilerServices.IsVolatile) System.Globalization.CultureInfo::numInfo
	NumberFormatInfo_tFDF57037EBC5BC833D0A53EF0327B805994860A8 * ___numInfo_10;
	// System.Globalization.DateTimeFormatInfo modreq(System.Runtime.CompilerServices.IsVolatile) System.Globalization.CultureInfo::dateTimeInfo
	DateTimeFormatInfo_tF4BB3AA482C2F772D2A9022F78BF8727830FAF5F * ___dateTimeInfo_11;
	// System.Globalization.TextInfo modreq(System.Runtime.CompilerServices.IsVolatile) System.Globalization.CultureInfo::textInfo
	TextInfo_t5F1E697CB6A7E5EC80F0DC3A968B9B4A70C291D8 * ___textInfo_12;
	// System.String System.Globalization.CultureInfo::m_name
	String_t* ___m_name_13;
	// System.String System.Globalization.CultureInfo::englishname
	String_t* ___englishname_14;
	// System.String System.Globalization.CultureInfo::nativename
	String_t* ___nativename_15;
	// System.String System.Globalization.CultureInfo::iso3lang
	String_t* ___iso3lang_16;
	// System.String System.Globalization.CultureInfo::iso2lang
	String_t* ___iso2lang_17;
	// System.String System.Globalization.CultureInfo::win3lang
	String_t* ___win3lang_18;
	// System.String System.Globalization.CultureInfo::territory
	String_t* ___territory_19;
	// System.String[] System.Globalization.CultureInfo::native_calendar_names
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* ___native_calendar_names_20;
	// System.Globalization.CompareInfo modreq(System.Runtime.CompilerServices.IsVolatile) System.Globalization.CultureInfo::compareInfo
	CompareInfo_tB9A071DBC11AC00AF2EA2066D0C2AE1DCB1865D1 * ___compareInfo_21;
	// System.Void* System.Globalization.CultureInfo::textinfo_data
	void* ___textinfo_data_22;
	// System.Int32 System.Globalization.CultureInfo::m_dataItem
	int32_t ___m_dataItem_23;
	// System.Globalization.Calendar System.Globalization.CultureInfo::calendar
	Calendar_tF55A785ACD277504CF0D2F2C6AD56F76C6E91BD5 * ___calendar_24;
	// System.Globalization.CultureInfo System.Globalization.CultureInfo::parent_culture
	CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * ___parent_culture_25;
	// System.Boolean System.Globalization.CultureInfo::constructed
	bool ___constructed_26;
	// System.Byte[] System.Globalization.CultureInfo::cached_serialized_form
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___cached_serialized_form_27;
	// System.Globalization.CultureData System.Globalization.CultureInfo::m_cultureData
	CultureData_tF43B080FFA6EB278F4F289BCDA3FB74B6C208ECD * ___m_cultureData_28;
	// System.Boolean System.Globalization.CultureInfo::m_isInherited
	bool ___m_isInherited_29;

public:
	inline static int32_t get_offset_of_m_isReadOnly_3() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___m_isReadOnly_3)); }
	inline bool get_m_isReadOnly_3() const { return ___m_isReadOnly_3; }
	inline bool* get_address_of_m_isReadOnly_3() { return &___m_isReadOnly_3; }
	inline void set_m_isReadOnly_3(bool value)
	{
		___m_isReadOnly_3 = value;
	}

	inline static int32_t get_offset_of_cultureID_4() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___cultureID_4)); }
	inline int32_t get_cultureID_4() const { return ___cultureID_4; }
	inline int32_t* get_address_of_cultureID_4() { return &___cultureID_4; }
	inline void set_cultureID_4(int32_t value)
	{
		___cultureID_4 = value;
	}

	inline static int32_t get_offset_of_parent_lcid_5() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___parent_lcid_5)); }
	inline int32_t get_parent_lcid_5() const { return ___parent_lcid_5; }
	inline int32_t* get_address_of_parent_lcid_5() { return &___parent_lcid_5; }
	inline void set_parent_lcid_5(int32_t value)
	{
		___parent_lcid_5 = value;
	}

	inline static int32_t get_offset_of_datetime_index_6() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___datetime_index_6)); }
	inline int32_t get_datetime_index_6() const { return ___datetime_index_6; }
	inline int32_t* get_address_of_datetime_index_6() { return &___datetime_index_6; }
	inline void set_datetime_index_6(int32_t value)
	{
		___datetime_index_6 = value;
	}

	inline static int32_t get_offset_of_number_index_7() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___number_index_7)); }
	inline int32_t get_number_index_7() const { return ___number_index_7; }
	inline int32_t* get_address_of_number_index_7() { return &___number_index_7; }
	inline void set_number_index_7(int32_t value)
	{
		___number_index_7 = value;
	}

	inline static int32_t get_offset_of_default_calendar_type_8() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___default_calendar_type_8)); }
	inline int32_t get_default_calendar_type_8() const { return ___default_calendar_type_8; }
	inline int32_t* get_address_of_default_calendar_type_8() { return &___default_calendar_type_8; }
	inline void set_default_calendar_type_8(int32_t value)
	{
		___default_calendar_type_8 = value;
	}

	inline static int32_t get_offset_of_m_useUserOverride_9() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___m_useUserOverride_9)); }
	inline bool get_m_useUserOverride_9() const { return ___m_useUserOverride_9; }
	inline bool* get_address_of_m_useUserOverride_9() { return &___m_useUserOverride_9; }
	inline void set_m_useUserOverride_9(bool value)
	{
		___m_useUserOverride_9 = value;
	}

	inline static int32_t get_offset_of_numInfo_10() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___numInfo_10)); }
	inline NumberFormatInfo_tFDF57037EBC5BC833D0A53EF0327B805994860A8 * get_numInfo_10() const { return ___numInfo_10; }
	inline NumberFormatInfo_tFDF57037EBC5BC833D0A53EF0327B805994860A8 ** get_address_of_numInfo_10() { return &___numInfo_10; }
	inline void set_numInfo_10(NumberFormatInfo_tFDF57037EBC5BC833D0A53EF0327B805994860A8 * value)
	{
		___numInfo_10 = value;
		Il2CppCodeGenWriteBarrier((&___numInfo_10), value);
	}

	inline static int32_t get_offset_of_dateTimeInfo_11() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___dateTimeInfo_11)); }
	inline DateTimeFormatInfo_tF4BB3AA482C2F772D2A9022F78BF8727830FAF5F * get_dateTimeInfo_11() const { return ___dateTimeInfo_11; }
	inline DateTimeFormatInfo_tF4BB3AA482C2F772D2A9022F78BF8727830FAF5F ** get_address_of_dateTimeInfo_11() { return &___dateTimeInfo_11; }
	inline void set_dateTimeInfo_11(DateTimeFormatInfo_tF4BB3AA482C2F772D2A9022F78BF8727830FAF5F * value)
	{
		___dateTimeInfo_11 = value;
		Il2CppCodeGenWriteBarrier((&___dateTimeInfo_11), value);
	}

	inline static int32_t get_offset_of_textInfo_12() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___textInfo_12)); }
	inline TextInfo_t5F1E697CB6A7E5EC80F0DC3A968B9B4A70C291D8 * get_textInfo_12() const { return ___textInfo_12; }
	inline TextInfo_t5F1E697CB6A7E5EC80F0DC3A968B9B4A70C291D8 ** get_address_of_textInfo_12() { return &___textInfo_12; }
	inline void set_textInfo_12(TextInfo_t5F1E697CB6A7E5EC80F0DC3A968B9B4A70C291D8 * value)
	{
		___textInfo_12 = value;
		Il2CppCodeGenWriteBarrier((&___textInfo_12), value);
	}

	inline static int32_t get_offset_of_m_name_13() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___m_name_13)); }
	inline String_t* get_m_name_13() const { return ___m_name_13; }
	inline String_t** get_address_of_m_name_13() { return &___m_name_13; }
	inline void set_m_name_13(String_t* value)
	{
		___m_name_13 = value;
		Il2CppCodeGenWriteBarrier((&___m_name_13), value);
	}

	inline static int32_t get_offset_of_englishname_14() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___englishname_14)); }
	inline String_t* get_englishname_14() const { return ___englishname_14; }
	inline String_t** get_address_of_englishname_14() { return &___englishname_14; }
	inline void set_englishname_14(String_t* value)
	{
		___englishname_14 = value;
		Il2CppCodeGenWriteBarrier((&___englishname_14), value);
	}

	inline static int32_t get_offset_of_nativename_15() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___nativename_15)); }
	inline String_t* get_nativename_15() const { return ___nativename_15; }
	inline String_t** get_address_of_nativename_15() { return &___nativename_15; }
	inline void set_nativename_15(String_t* value)
	{
		___nativename_15 = value;
		Il2CppCodeGenWriteBarrier((&___nativename_15), value);
	}

	inline static int32_t get_offset_of_iso3lang_16() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___iso3lang_16)); }
	inline String_t* get_iso3lang_16() const { return ___iso3lang_16; }
	inline String_t** get_address_of_iso3lang_16() { return &___iso3lang_16; }
	inline void set_iso3lang_16(String_t* value)
	{
		___iso3lang_16 = value;
		Il2CppCodeGenWriteBarrier((&___iso3lang_16), value);
	}

	inline static int32_t get_offset_of_iso2lang_17() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___iso2lang_17)); }
	inline String_t* get_iso2lang_17() const { return ___iso2lang_17; }
	inline String_t** get_address_of_iso2lang_17() { return &___iso2lang_17; }
	inline void set_iso2lang_17(String_t* value)
	{
		___iso2lang_17 = value;
		Il2CppCodeGenWriteBarrier((&___iso2lang_17), value);
	}

	inline static int32_t get_offset_of_win3lang_18() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___win3lang_18)); }
	inline String_t* get_win3lang_18() const { return ___win3lang_18; }
	inline String_t** get_address_of_win3lang_18() { return &___win3lang_18; }
	inline void set_win3lang_18(String_t* value)
	{
		___win3lang_18 = value;
		Il2CppCodeGenWriteBarrier((&___win3lang_18), value);
	}

	inline static int32_t get_offset_of_territory_19() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___territory_19)); }
	inline String_t* get_territory_19() const { return ___territory_19; }
	inline String_t** get_address_of_territory_19() { return &___territory_19; }
	inline void set_territory_19(String_t* value)
	{
		___territory_19 = value;
		Il2CppCodeGenWriteBarrier((&___territory_19), value);
	}

	inline static int32_t get_offset_of_native_calendar_names_20() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___native_calendar_names_20)); }
	inline StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* get_native_calendar_names_20() const { return ___native_calendar_names_20; }
	inline StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E** get_address_of_native_calendar_names_20() { return &___native_calendar_names_20; }
	inline void set_native_calendar_names_20(StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* value)
	{
		___native_calendar_names_20 = value;
		Il2CppCodeGenWriteBarrier((&___native_calendar_names_20), value);
	}

	inline static int32_t get_offset_of_compareInfo_21() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___compareInfo_21)); }
	inline CompareInfo_tB9A071DBC11AC00AF2EA2066D0C2AE1DCB1865D1 * get_compareInfo_21() const { return ___compareInfo_21; }
	inline CompareInfo_tB9A071DBC11AC00AF2EA2066D0C2AE1DCB1865D1 ** get_address_of_compareInfo_21() { return &___compareInfo_21; }
	inline void set_compareInfo_21(CompareInfo_tB9A071DBC11AC00AF2EA2066D0C2AE1DCB1865D1 * value)
	{
		___compareInfo_21 = value;
		Il2CppCodeGenWriteBarrier((&___compareInfo_21), value);
	}

	inline static int32_t get_offset_of_textinfo_data_22() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___textinfo_data_22)); }
	inline void* get_textinfo_data_22() const { return ___textinfo_data_22; }
	inline void** get_address_of_textinfo_data_22() { return &___textinfo_data_22; }
	inline void set_textinfo_data_22(void* value)
	{
		___textinfo_data_22 = value;
	}

	inline static int32_t get_offset_of_m_dataItem_23() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___m_dataItem_23)); }
	inline int32_t get_m_dataItem_23() const { return ___m_dataItem_23; }
	inline int32_t* get_address_of_m_dataItem_23() { return &___m_dataItem_23; }
	inline void set_m_dataItem_23(int32_t value)
	{
		___m_dataItem_23 = value;
	}

	inline static int32_t get_offset_of_calendar_24() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___calendar_24)); }
	inline Calendar_tF55A785ACD277504CF0D2F2C6AD56F76C6E91BD5 * get_calendar_24() const { return ___calendar_24; }
	inline Calendar_tF55A785ACD277504CF0D2F2C6AD56F76C6E91BD5 ** get_address_of_calendar_24() { return &___calendar_24; }
	inline void set_calendar_24(Calendar_tF55A785ACD277504CF0D2F2C6AD56F76C6E91BD5 * value)
	{
		___calendar_24 = value;
		Il2CppCodeGenWriteBarrier((&___calendar_24), value);
	}

	inline static int32_t get_offset_of_parent_culture_25() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___parent_culture_25)); }
	inline CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * get_parent_culture_25() const { return ___parent_culture_25; }
	inline CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F ** get_address_of_parent_culture_25() { return &___parent_culture_25; }
	inline void set_parent_culture_25(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * value)
	{
		___parent_culture_25 = value;
		Il2CppCodeGenWriteBarrier((&___parent_culture_25), value);
	}

	inline static int32_t get_offset_of_constructed_26() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___constructed_26)); }
	inline bool get_constructed_26() const { return ___constructed_26; }
	inline bool* get_address_of_constructed_26() { return &___constructed_26; }
	inline void set_constructed_26(bool value)
	{
		___constructed_26 = value;
	}

	inline static int32_t get_offset_of_cached_serialized_form_27() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___cached_serialized_form_27)); }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* get_cached_serialized_form_27() const { return ___cached_serialized_form_27; }
	inline ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821** get_address_of_cached_serialized_form_27() { return &___cached_serialized_form_27; }
	inline void set_cached_serialized_form_27(ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* value)
	{
		___cached_serialized_form_27 = value;
		Il2CppCodeGenWriteBarrier((&___cached_serialized_form_27), value);
	}

	inline static int32_t get_offset_of_m_cultureData_28() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___m_cultureData_28)); }
	inline CultureData_tF43B080FFA6EB278F4F289BCDA3FB74B6C208ECD * get_m_cultureData_28() const { return ___m_cultureData_28; }
	inline CultureData_tF43B080FFA6EB278F4F289BCDA3FB74B6C208ECD ** get_address_of_m_cultureData_28() { return &___m_cultureData_28; }
	inline void set_m_cultureData_28(CultureData_tF43B080FFA6EB278F4F289BCDA3FB74B6C208ECD * value)
	{
		___m_cultureData_28 = value;
		Il2CppCodeGenWriteBarrier((&___m_cultureData_28), value);
	}

	inline static int32_t get_offset_of_m_isInherited_29() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F, ___m_isInherited_29)); }
	inline bool get_m_isInherited_29() const { return ___m_isInherited_29; }
	inline bool* get_address_of_m_isInherited_29() { return &___m_isInherited_29; }
	inline void set_m_isInherited_29(bool value)
	{
		___m_isInherited_29 = value;
	}
};

struct CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F_StaticFields
{
public:
	// System.Globalization.CultureInfo modreq(System.Runtime.CompilerServices.IsVolatile) System.Globalization.CultureInfo::invariant_culture_info
	CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * ___invariant_culture_info_0;
	// System.Object System.Globalization.CultureInfo::shared_table_lock
	RuntimeObject * ___shared_table_lock_1;
	// System.Globalization.CultureInfo System.Globalization.CultureInfo::default_current_culture
	CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * ___default_current_culture_2;
	// System.Globalization.CultureInfo modreq(System.Runtime.CompilerServices.IsVolatile) System.Globalization.CultureInfo::s_DefaultThreadCurrentUICulture
	CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * ___s_DefaultThreadCurrentUICulture_33;
	// System.Globalization.CultureInfo modreq(System.Runtime.CompilerServices.IsVolatile) System.Globalization.CultureInfo::s_DefaultThreadCurrentCulture
	CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * ___s_DefaultThreadCurrentCulture_34;
	// System.Collections.Generic.Dictionary`2<System.Int32,System.Globalization.CultureInfo> System.Globalization.CultureInfo::shared_by_number
	Dictionary_2_tC88A56872F7C79DBB9582D4F3FC22ED5D8E0B98B * ___shared_by_number_35;
	// System.Collections.Generic.Dictionary`2<System.String,System.Globalization.CultureInfo> System.Globalization.CultureInfo::shared_by_name
	Dictionary_2_tBA5388DBB42BF620266F9A48E8B859BBBB224E25 * ___shared_by_name_36;
	// System.Boolean System.Globalization.CultureInfo::IsTaiwanSku
	bool ___IsTaiwanSku_37;

public:
	inline static int32_t get_offset_of_invariant_culture_info_0() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F_StaticFields, ___invariant_culture_info_0)); }
	inline CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * get_invariant_culture_info_0() const { return ___invariant_culture_info_0; }
	inline CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F ** get_address_of_invariant_culture_info_0() { return &___invariant_culture_info_0; }
	inline void set_invariant_culture_info_0(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * value)
	{
		___invariant_culture_info_0 = value;
		Il2CppCodeGenWriteBarrier((&___invariant_culture_info_0), value);
	}

	inline static int32_t get_offset_of_shared_table_lock_1() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F_StaticFields, ___shared_table_lock_1)); }
	inline RuntimeObject * get_shared_table_lock_1() const { return ___shared_table_lock_1; }
	inline RuntimeObject ** get_address_of_shared_table_lock_1() { return &___shared_table_lock_1; }
	inline void set_shared_table_lock_1(RuntimeObject * value)
	{
		___shared_table_lock_1 = value;
		Il2CppCodeGenWriteBarrier((&___shared_table_lock_1), value);
	}

	inline static int32_t get_offset_of_default_current_culture_2() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F_StaticFields, ___default_current_culture_2)); }
	inline CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * get_default_current_culture_2() const { return ___default_current_culture_2; }
	inline CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F ** get_address_of_default_current_culture_2() { return &___default_current_culture_2; }
	inline void set_default_current_culture_2(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * value)
	{
		___default_current_culture_2 = value;
		Il2CppCodeGenWriteBarrier((&___default_current_culture_2), value);
	}

	inline static int32_t get_offset_of_s_DefaultThreadCurrentUICulture_33() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F_StaticFields, ___s_DefaultThreadCurrentUICulture_33)); }
	inline CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * get_s_DefaultThreadCurrentUICulture_33() const { return ___s_DefaultThreadCurrentUICulture_33; }
	inline CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F ** get_address_of_s_DefaultThreadCurrentUICulture_33() { return &___s_DefaultThreadCurrentUICulture_33; }
	inline void set_s_DefaultThreadCurrentUICulture_33(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * value)
	{
		___s_DefaultThreadCurrentUICulture_33 = value;
		Il2CppCodeGenWriteBarrier((&___s_DefaultThreadCurrentUICulture_33), value);
	}

	inline static int32_t get_offset_of_s_DefaultThreadCurrentCulture_34() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F_StaticFields, ___s_DefaultThreadCurrentCulture_34)); }
	inline CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * get_s_DefaultThreadCurrentCulture_34() const { return ___s_DefaultThreadCurrentCulture_34; }
	inline CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F ** get_address_of_s_DefaultThreadCurrentCulture_34() { return &___s_DefaultThreadCurrentCulture_34; }
	inline void set_s_DefaultThreadCurrentCulture_34(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * value)
	{
		___s_DefaultThreadCurrentCulture_34 = value;
		Il2CppCodeGenWriteBarrier((&___s_DefaultThreadCurrentCulture_34), value);
	}

	inline static int32_t get_offset_of_shared_by_number_35() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F_StaticFields, ___shared_by_number_35)); }
	inline Dictionary_2_tC88A56872F7C79DBB9582D4F3FC22ED5D8E0B98B * get_shared_by_number_35() const { return ___shared_by_number_35; }
	inline Dictionary_2_tC88A56872F7C79DBB9582D4F3FC22ED5D8E0B98B ** get_address_of_shared_by_number_35() { return &___shared_by_number_35; }
	inline void set_shared_by_number_35(Dictionary_2_tC88A56872F7C79DBB9582D4F3FC22ED5D8E0B98B * value)
	{
		___shared_by_number_35 = value;
		Il2CppCodeGenWriteBarrier((&___shared_by_number_35), value);
	}

	inline static int32_t get_offset_of_shared_by_name_36() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F_StaticFields, ___shared_by_name_36)); }
	inline Dictionary_2_tBA5388DBB42BF620266F9A48E8B859BBBB224E25 * get_shared_by_name_36() const { return ___shared_by_name_36; }
	inline Dictionary_2_tBA5388DBB42BF620266F9A48E8B859BBBB224E25 ** get_address_of_shared_by_name_36() { return &___shared_by_name_36; }
	inline void set_shared_by_name_36(Dictionary_2_tBA5388DBB42BF620266F9A48E8B859BBBB224E25 * value)
	{
		___shared_by_name_36 = value;
		Il2CppCodeGenWriteBarrier((&___shared_by_name_36), value);
	}

	inline static int32_t get_offset_of_IsTaiwanSku_37() { return static_cast<int32_t>(offsetof(CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F_StaticFields, ___IsTaiwanSku_37)); }
	inline bool get_IsTaiwanSku_37() const { return ___IsTaiwanSku_37; }
	inline bool* get_address_of_IsTaiwanSku_37() { return &___IsTaiwanSku_37; }
	inline void set_IsTaiwanSku_37(bool value)
	{
		___IsTaiwanSku_37 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of System.Globalization.CultureInfo
struct CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F_marshaled_pinvoke
{
	int32_t ___m_isReadOnly_3;
	int32_t ___cultureID_4;
	int32_t ___parent_lcid_5;
	int32_t ___datetime_index_6;
	int32_t ___number_index_7;
	int32_t ___default_calendar_type_8;
	int32_t ___m_useUserOverride_9;
	NumberFormatInfo_tFDF57037EBC5BC833D0A53EF0327B805994860A8 * ___numInfo_10;
	DateTimeFormatInfo_tF4BB3AA482C2F772D2A9022F78BF8727830FAF5F * ___dateTimeInfo_11;
	TextInfo_t5F1E697CB6A7E5EC80F0DC3A968B9B4A70C291D8 * ___textInfo_12;
	char* ___m_name_13;
	char* ___englishname_14;
	char* ___nativename_15;
	char* ___iso3lang_16;
	char* ___iso2lang_17;
	char* ___win3lang_18;
	char* ___territory_19;
	char** ___native_calendar_names_20;
	CompareInfo_tB9A071DBC11AC00AF2EA2066D0C2AE1DCB1865D1 * ___compareInfo_21;
	void* ___textinfo_data_22;
	int32_t ___m_dataItem_23;
	Calendar_tF55A785ACD277504CF0D2F2C6AD56F76C6E91BD5 * ___calendar_24;
	CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F_marshaled_pinvoke* ___parent_culture_25;
	int32_t ___constructed_26;
	uint8_t* ___cached_serialized_form_27;
	CultureData_tF43B080FFA6EB278F4F289BCDA3FB74B6C208ECD_marshaled_pinvoke* ___m_cultureData_28;
	int32_t ___m_isInherited_29;
};
// Native definition for COM marshalling of System.Globalization.CultureInfo
struct CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F_marshaled_com
{
	int32_t ___m_isReadOnly_3;
	int32_t ___cultureID_4;
	int32_t ___parent_lcid_5;
	int32_t ___datetime_index_6;
	int32_t ___number_index_7;
	int32_t ___default_calendar_type_8;
	int32_t ___m_useUserOverride_9;
	NumberFormatInfo_tFDF57037EBC5BC833D0A53EF0327B805994860A8 * ___numInfo_10;
	DateTimeFormatInfo_tF4BB3AA482C2F772D2A9022F78BF8727830FAF5F * ___dateTimeInfo_11;
	TextInfo_t5F1E697CB6A7E5EC80F0DC3A968B9B4A70C291D8 * ___textInfo_12;
	Il2CppChar* ___m_name_13;
	Il2CppChar* ___englishname_14;
	Il2CppChar* ___nativename_15;
	Il2CppChar* ___iso3lang_16;
	Il2CppChar* ___iso2lang_17;
	Il2CppChar* ___win3lang_18;
	Il2CppChar* ___territory_19;
	Il2CppChar** ___native_calendar_names_20;
	CompareInfo_tB9A071DBC11AC00AF2EA2066D0C2AE1DCB1865D1 * ___compareInfo_21;
	void* ___textinfo_data_22;
	int32_t ___m_dataItem_23;
	Calendar_tF55A785ACD277504CF0D2F2C6AD56F76C6E91BD5 * ___calendar_24;
	CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F_marshaled_com* ___parent_culture_25;
	int32_t ___constructed_26;
	uint8_t* ___cached_serialized_form_27;
	CultureData_tF43B080FFA6EB278F4F289BCDA3FB74B6C208ECD_marshaled_com* ___m_cultureData_28;
	int32_t ___m_isInherited_29;
};
#endif // CULTUREINFO_T345AC6924134F039ED9A11F3E03F8E91B6A3225F_H
#ifndef MEMBERINFO_T_H
#define MEMBERINFO_T_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Reflection.MemberInfo
struct  MemberInfo_t  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // MEMBERINFO_T_H
#ifndef SERIALIZATIONINFO_T1BB80E9C9DEA52DBF464487234B045E2930ADA26_H
#define SERIALIZATIONINFO_T1BB80E9C9DEA52DBF464487234B045E2930ADA26_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Runtime.Serialization.SerializationInfo
struct  SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26  : public RuntimeObject
{
public:
	// System.String[] System.Runtime.Serialization.SerializationInfo::m_members
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* ___m_members_0;
	// System.Object[] System.Runtime.Serialization.SerializationInfo::m_data
	ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* ___m_data_1;
	// System.Type[] System.Runtime.Serialization.SerializationInfo::m_types
	TypeU5BU5D_t7FE623A666B49176DE123306221193E888A12F5F* ___m_types_2;
	// System.Collections.Generic.Dictionary`2<System.String,System.Int32> System.Runtime.Serialization.SerializationInfo::m_nameToIndex
	Dictionary_2_tD6E204872BA9FD506A0287EF68E285BEB9EC0DFB * ___m_nameToIndex_3;
	// System.Int32 System.Runtime.Serialization.SerializationInfo::m_currMember
	int32_t ___m_currMember_4;
	// System.Runtime.Serialization.IFormatterConverter System.Runtime.Serialization.SerializationInfo::m_converter
	RuntimeObject* ___m_converter_5;
	// System.String System.Runtime.Serialization.SerializationInfo::m_fullTypeName
	String_t* ___m_fullTypeName_6;
	// System.String System.Runtime.Serialization.SerializationInfo::m_assemName
	String_t* ___m_assemName_7;
	// System.Type System.Runtime.Serialization.SerializationInfo::objectType
	Type_t * ___objectType_8;
	// System.Boolean System.Runtime.Serialization.SerializationInfo::isFullTypeNameSetExplicit
	bool ___isFullTypeNameSetExplicit_9;
	// System.Boolean System.Runtime.Serialization.SerializationInfo::isAssemblyNameSetExplicit
	bool ___isAssemblyNameSetExplicit_10;
	// System.Boolean System.Runtime.Serialization.SerializationInfo::requireSameTokenInPartialTrust
	bool ___requireSameTokenInPartialTrust_11;

public:
	inline static int32_t get_offset_of_m_members_0() { return static_cast<int32_t>(offsetof(SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26, ___m_members_0)); }
	inline StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* get_m_members_0() const { return ___m_members_0; }
	inline StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E** get_address_of_m_members_0() { return &___m_members_0; }
	inline void set_m_members_0(StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* value)
	{
		___m_members_0 = value;
		Il2CppCodeGenWriteBarrier((&___m_members_0), value);
	}

	inline static int32_t get_offset_of_m_data_1() { return static_cast<int32_t>(offsetof(SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26, ___m_data_1)); }
	inline ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* get_m_data_1() const { return ___m_data_1; }
	inline ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A** get_address_of_m_data_1() { return &___m_data_1; }
	inline void set_m_data_1(ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* value)
	{
		___m_data_1 = value;
		Il2CppCodeGenWriteBarrier((&___m_data_1), value);
	}

	inline static int32_t get_offset_of_m_types_2() { return static_cast<int32_t>(offsetof(SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26, ___m_types_2)); }
	inline TypeU5BU5D_t7FE623A666B49176DE123306221193E888A12F5F* get_m_types_2() const { return ___m_types_2; }
	inline TypeU5BU5D_t7FE623A666B49176DE123306221193E888A12F5F** get_address_of_m_types_2() { return &___m_types_2; }
	inline void set_m_types_2(TypeU5BU5D_t7FE623A666B49176DE123306221193E888A12F5F* value)
	{
		___m_types_2 = value;
		Il2CppCodeGenWriteBarrier((&___m_types_2), value);
	}

	inline static int32_t get_offset_of_m_nameToIndex_3() { return static_cast<int32_t>(offsetof(SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26, ___m_nameToIndex_3)); }
	inline Dictionary_2_tD6E204872BA9FD506A0287EF68E285BEB9EC0DFB * get_m_nameToIndex_3() const { return ___m_nameToIndex_3; }
	inline Dictionary_2_tD6E204872BA9FD506A0287EF68E285BEB9EC0DFB ** get_address_of_m_nameToIndex_3() { return &___m_nameToIndex_3; }
	inline void set_m_nameToIndex_3(Dictionary_2_tD6E204872BA9FD506A0287EF68E285BEB9EC0DFB * value)
	{
		___m_nameToIndex_3 = value;
		Il2CppCodeGenWriteBarrier((&___m_nameToIndex_3), value);
	}

	inline static int32_t get_offset_of_m_currMember_4() { return static_cast<int32_t>(offsetof(SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26, ___m_currMember_4)); }
	inline int32_t get_m_currMember_4() const { return ___m_currMember_4; }
	inline int32_t* get_address_of_m_currMember_4() { return &___m_currMember_4; }
	inline void set_m_currMember_4(int32_t value)
	{
		___m_currMember_4 = value;
	}

	inline static int32_t get_offset_of_m_converter_5() { return static_cast<int32_t>(offsetof(SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26, ___m_converter_5)); }
	inline RuntimeObject* get_m_converter_5() const { return ___m_converter_5; }
	inline RuntimeObject** get_address_of_m_converter_5() { return &___m_converter_5; }
	inline void set_m_converter_5(RuntimeObject* value)
	{
		___m_converter_5 = value;
		Il2CppCodeGenWriteBarrier((&___m_converter_5), value);
	}

	inline static int32_t get_offset_of_m_fullTypeName_6() { return static_cast<int32_t>(offsetof(SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26, ___m_fullTypeName_6)); }
	inline String_t* get_m_fullTypeName_6() const { return ___m_fullTypeName_6; }
	inline String_t** get_address_of_m_fullTypeName_6() { return &___m_fullTypeName_6; }
	inline void set_m_fullTypeName_6(String_t* value)
	{
		___m_fullTypeName_6 = value;
		Il2CppCodeGenWriteBarrier((&___m_fullTypeName_6), value);
	}

	inline static int32_t get_offset_of_m_assemName_7() { return static_cast<int32_t>(offsetof(SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26, ___m_assemName_7)); }
	inline String_t* get_m_assemName_7() const { return ___m_assemName_7; }
	inline String_t** get_address_of_m_assemName_7() { return &___m_assemName_7; }
	inline void set_m_assemName_7(String_t* value)
	{
		___m_assemName_7 = value;
		Il2CppCodeGenWriteBarrier((&___m_assemName_7), value);
	}

	inline static int32_t get_offset_of_objectType_8() { return static_cast<int32_t>(offsetof(SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26, ___objectType_8)); }
	inline Type_t * get_objectType_8() const { return ___objectType_8; }
	inline Type_t ** get_address_of_objectType_8() { return &___objectType_8; }
	inline void set_objectType_8(Type_t * value)
	{
		___objectType_8 = value;
		Il2CppCodeGenWriteBarrier((&___objectType_8), value);
	}

	inline static int32_t get_offset_of_isFullTypeNameSetExplicit_9() { return static_cast<int32_t>(offsetof(SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26, ___isFullTypeNameSetExplicit_9)); }
	inline bool get_isFullTypeNameSetExplicit_9() const { return ___isFullTypeNameSetExplicit_9; }
	inline bool* get_address_of_isFullTypeNameSetExplicit_9() { return &___isFullTypeNameSetExplicit_9; }
	inline void set_isFullTypeNameSetExplicit_9(bool value)
	{
		___isFullTypeNameSetExplicit_9 = value;
	}

	inline static int32_t get_offset_of_isAssemblyNameSetExplicit_10() { return static_cast<int32_t>(offsetof(SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26, ___isAssemblyNameSetExplicit_10)); }
	inline bool get_isAssemblyNameSetExplicit_10() const { return ___isAssemblyNameSetExplicit_10; }
	inline bool* get_address_of_isAssemblyNameSetExplicit_10() { return &___isAssemblyNameSetExplicit_10; }
	inline void set_isAssemblyNameSetExplicit_10(bool value)
	{
		___isAssemblyNameSetExplicit_10 = value;
	}

	inline static int32_t get_offset_of_requireSameTokenInPartialTrust_11() { return static_cast<int32_t>(offsetof(SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26, ___requireSameTokenInPartialTrust_11)); }
	inline bool get_requireSameTokenInPartialTrust_11() const { return ___requireSameTokenInPartialTrust_11; }
	inline bool* get_address_of_requireSameTokenInPartialTrust_11() { return &___requireSameTokenInPartialTrust_11; }
	inline void set_requireSameTokenInPartialTrust_11(bool value)
	{
		___requireSameTokenInPartialTrust_11 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // SERIALIZATIONINFO_T1BB80E9C9DEA52DBF464487234B045E2930ADA26_H
#ifndef BINARYCOMPATIBILITY_T06B1B8D34764DB1710459778EB22433728A665A8_H
#define BINARYCOMPATIBILITY_T06B1B8D34764DB1710459778EB22433728A665A8_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Runtime.Versioning.BinaryCompatibility
struct  BinaryCompatibility_t06B1B8D34764DB1710459778EB22433728A665A8  : public RuntimeObject
{
public:

public:
};

struct BinaryCompatibility_t06B1B8D34764DB1710459778EB22433728A665A8_StaticFields
{
public:
	// System.Boolean System.Runtime.Versioning.BinaryCompatibility::TargetsAtLeast_Desktop_V4_5
	bool ___TargetsAtLeast_Desktop_V4_5_0;
	// System.Boolean System.Runtime.Versioning.BinaryCompatibility::TargetsAtLeast_Desktop_V4_5_1
	bool ___TargetsAtLeast_Desktop_V4_5_1_1;

public:
	inline static int32_t get_offset_of_TargetsAtLeast_Desktop_V4_5_0() { return static_cast<int32_t>(offsetof(BinaryCompatibility_t06B1B8D34764DB1710459778EB22433728A665A8_StaticFields, ___TargetsAtLeast_Desktop_V4_5_0)); }
	inline bool get_TargetsAtLeast_Desktop_V4_5_0() const { return ___TargetsAtLeast_Desktop_V4_5_0; }
	inline bool* get_address_of_TargetsAtLeast_Desktop_V4_5_0() { return &___TargetsAtLeast_Desktop_V4_5_0; }
	inline void set_TargetsAtLeast_Desktop_V4_5_0(bool value)
	{
		___TargetsAtLeast_Desktop_V4_5_0 = value;
	}

	inline static int32_t get_offset_of_TargetsAtLeast_Desktop_V4_5_1_1() { return static_cast<int32_t>(offsetof(BinaryCompatibility_t06B1B8D34764DB1710459778EB22433728A665A8_StaticFields, ___TargetsAtLeast_Desktop_V4_5_1_1)); }
	inline bool get_TargetsAtLeast_Desktop_V4_5_1_1() const { return ___TargetsAtLeast_Desktop_V4_5_1_1; }
	inline bool* get_address_of_TargetsAtLeast_Desktop_V4_5_1_1() { return &___TargetsAtLeast_Desktop_V4_5_1_1; }
	inline void set_TargetsAtLeast_Desktop_V4_5_1_1(bool value)
	{
		___TargetsAtLeast_Desktop_V4_5_1_1 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // BINARYCOMPATIBILITY_T06B1B8D34764DB1710459778EB22433728A665A8_H
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
#ifndef DECODERFALLBACK_T128445EB7676870485230893338EF044F6B72F60_H
#define DECODERFALLBACK_T128445EB7676870485230893338EF044F6B72F60_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Text.DecoderFallback
struct  DecoderFallback_t128445EB7676870485230893338EF044F6B72F60  : public RuntimeObject
{
public:
	// System.Boolean System.Text.DecoderFallback::bIsMicrosoftBestFitFallback
	bool ___bIsMicrosoftBestFitFallback_0;

public:
	inline static int32_t get_offset_of_bIsMicrosoftBestFitFallback_0() { return static_cast<int32_t>(offsetof(DecoderFallback_t128445EB7676870485230893338EF044F6B72F60, ___bIsMicrosoftBestFitFallback_0)); }
	inline bool get_bIsMicrosoftBestFitFallback_0() const { return ___bIsMicrosoftBestFitFallback_0; }
	inline bool* get_address_of_bIsMicrosoftBestFitFallback_0() { return &___bIsMicrosoftBestFitFallback_0; }
	inline void set_bIsMicrosoftBestFitFallback_0(bool value)
	{
		___bIsMicrosoftBestFitFallback_0 = value;
	}
};

struct DecoderFallback_t128445EB7676870485230893338EF044F6B72F60_StaticFields
{
public:
	// System.Text.DecoderFallback modreq(System.Runtime.CompilerServices.IsVolatile) System.Text.DecoderFallback::replacementFallback
	DecoderFallback_t128445EB7676870485230893338EF044F6B72F60 * ___replacementFallback_1;
	// System.Text.DecoderFallback modreq(System.Runtime.CompilerServices.IsVolatile) System.Text.DecoderFallback::exceptionFallback
	DecoderFallback_t128445EB7676870485230893338EF044F6B72F60 * ___exceptionFallback_2;
	// System.Object System.Text.DecoderFallback::s_InternalSyncObject
	RuntimeObject * ___s_InternalSyncObject_3;

public:
	inline static int32_t get_offset_of_replacementFallback_1() { return static_cast<int32_t>(offsetof(DecoderFallback_t128445EB7676870485230893338EF044F6B72F60_StaticFields, ___replacementFallback_1)); }
	inline DecoderFallback_t128445EB7676870485230893338EF044F6B72F60 * get_replacementFallback_1() const { return ___replacementFallback_1; }
	inline DecoderFallback_t128445EB7676870485230893338EF044F6B72F60 ** get_address_of_replacementFallback_1() { return &___replacementFallback_1; }
	inline void set_replacementFallback_1(DecoderFallback_t128445EB7676870485230893338EF044F6B72F60 * value)
	{
		___replacementFallback_1 = value;
		Il2CppCodeGenWriteBarrier((&___replacementFallback_1), value);
	}

	inline static int32_t get_offset_of_exceptionFallback_2() { return static_cast<int32_t>(offsetof(DecoderFallback_t128445EB7676870485230893338EF044F6B72F60_StaticFields, ___exceptionFallback_2)); }
	inline DecoderFallback_t128445EB7676870485230893338EF044F6B72F60 * get_exceptionFallback_2() const { return ___exceptionFallback_2; }
	inline DecoderFallback_t128445EB7676870485230893338EF044F6B72F60 ** get_address_of_exceptionFallback_2() { return &___exceptionFallback_2; }
	inline void set_exceptionFallback_2(DecoderFallback_t128445EB7676870485230893338EF044F6B72F60 * value)
	{
		___exceptionFallback_2 = value;
		Il2CppCodeGenWriteBarrier((&___exceptionFallback_2), value);
	}

	inline static int32_t get_offset_of_s_InternalSyncObject_3() { return static_cast<int32_t>(offsetof(DecoderFallback_t128445EB7676870485230893338EF044F6B72F60_StaticFields, ___s_InternalSyncObject_3)); }
	inline RuntimeObject * get_s_InternalSyncObject_3() const { return ___s_InternalSyncObject_3; }
	inline RuntimeObject ** get_address_of_s_InternalSyncObject_3() { return &___s_InternalSyncObject_3; }
	inline void set_s_InternalSyncObject_3(RuntimeObject * value)
	{
		___s_InternalSyncObject_3 = value;
		Il2CppCodeGenWriteBarrier((&___s_InternalSyncObject_3), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // DECODERFALLBACK_T128445EB7676870485230893338EF044F6B72F60_H
#ifndef ENCODERFALLBACK_TDE342346D01608628F1BCEBB652D31009852CF63_H
#define ENCODERFALLBACK_TDE342346D01608628F1BCEBB652D31009852CF63_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Text.EncoderFallback
struct  EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63  : public RuntimeObject
{
public:
	// System.Boolean System.Text.EncoderFallback::bIsMicrosoftBestFitFallback
	bool ___bIsMicrosoftBestFitFallback_0;

public:
	inline static int32_t get_offset_of_bIsMicrosoftBestFitFallback_0() { return static_cast<int32_t>(offsetof(EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63, ___bIsMicrosoftBestFitFallback_0)); }
	inline bool get_bIsMicrosoftBestFitFallback_0() const { return ___bIsMicrosoftBestFitFallback_0; }
	inline bool* get_address_of_bIsMicrosoftBestFitFallback_0() { return &___bIsMicrosoftBestFitFallback_0; }
	inline void set_bIsMicrosoftBestFitFallback_0(bool value)
	{
		___bIsMicrosoftBestFitFallback_0 = value;
	}
};

struct EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63_StaticFields
{
public:
	// System.Text.EncoderFallback modreq(System.Runtime.CompilerServices.IsVolatile) System.Text.EncoderFallback::replacementFallback
	EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63 * ___replacementFallback_1;
	// System.Text.EncoderFallback modreq(System.Runtime.CompilerServices.IsVolatile) System.Text.EncoderFallback::exceptionFallback
	EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63 * ___exceptionFallback_2;
	// System.Object System.Text.EncoderFallback::s_InternalSyncObject
	RuntimeObject * ___s_InternalSyncObject_3;

public:
	inline static int32_t get_offset_of_replacementFallback_1() { return static_cast<int32_t>(offsetof(EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63_StaticFields, ___replacementFallback_1)); }
	inline EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63 * get_replacementFallback_1() const { return ___replacementFallback_1; }
	inline EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63 ** get_address_of_replacementFallback_1() { return &___replacementFallback_1; }
	inline void set_replacementFallback_1(EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63 * value)
	{
		___replacementFallback_1 = value;
		Il2CppCodeGenWriteBarrier((&___replacementFallback_1), value);
	}

	inline static int32_t get_offset_of_exceptionFallback_2() { return static_cast<int32_t>(offsetof(EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63_StaticFields, ___exceptionFallback_2)); }
	inline EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63 * get_exceptionFallback_2() const { return ___exceptionFallback_2; }
	inline EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63 ** get_address_of_exceptionFallback_2() { return &___exceptionFallback_2; }
	inline void set_exceptionFallback_2(EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63 * value)
	{
		___exceptionFallback_2 = value;
		Il2CppCodeGenWriteBarrier((&___exceptionFallback_2), value);
	}

	inline static int32_t get_offset_of_s_InternalSyncObject_3() { return static_cast<int32_t>(offsetof(EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63_StaticFields, ___s_InternalSyncObject_3)); }
	inline RuntimeObject * get_s_InternalSyncObject_3() const { return ___s_InternalSyncObject_3; }
	inline RuntimeObject ** get_address_of_s_InternalSyncObject_3() { return &___s_InternalSyncObject_3; }
	inline void set_s_InternalSyncObject_3(RuntimeObject * value)
	{
		___s_InternalSyncObject_3 = value;
		Il2CppCodeGenWriteBarrier((&___s_InternalSyncObject_3), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // ENCODERFALLBACK_TDE342346D01608628F1BCEBB652D31009852CF63_H
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
#ifndef MOREINFO_T83B9EC79244C26B468C115E54C0BEF09BB2E05B5_H
#define MOREINFO_T83B9EC79244C26B468C115E54C0BEF09BB2E05B5_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Uri_MoreInfo
struct  MoreInfo_t83B9EC79244C26B468C115E54C0BEF09BB2E05B5  : public RuntimeObject
{
public:
	// System.String System.Uri_MoreInfo::Path
	String_t* ___Path_0;
	// System.String System.Uri_MoreInfo::Query
	String_t* ___Query_1;
	// System.String System.Uri_MoreInfo::Fragment
	String_t* ___Fragment_2;
	// System.String System.Uri_MoreInfo::AbsoluteUri
	String_t* ___AbsoluteUri_3;
	// System.Int32 System.Uri_MoreInfo::Hash
	int32_t ___Hash_4;
	// System.String System.Uri_MoreInfo::RemoteUrl
	String_t* ___RemoteUrl_5;

public:
	inline static int32_t get_offset_of_Path_0() { return static_cast<int32_t>(offsetof(MoreInfo_t83B9EC79244C26B468C115E54C0BEF09BB2E05B5, ___Path_0)); }
	inline String_t* get_Path_0() const { return ___Path_0; }
	inline String_t** get_address_of_Path_0() { return &___Path_0; }
	inline void set_Path_0(String_t* value)
	{
		___Path_0 = value;
		Il2CppCodeGenWriteBarrier((&___Path_0), value);
	}

	inline static int32_t get_offset_of_Query_1() { return static_cast<int32_t>(offsetof(MoreInfo_t83B9EC79244C26B468C115E54C0BEF09BB2E05B5, ___Query_1)); }
	inline String_t* get_Query_1() const { return ___Query_1; }
	inline String_t** get_address_of_Query_1() { return &___Query_1; }
	inline void set_Query_1(String_t* value)
	{
		___Query_1 = value;
		Il2CppCodeGenWriteBarrier((&___Query_1), value);
	}

	inline static int32_t get_offset_of_Fragment_2() { return static_cast<int32_t>(offsetof(MoreInfo_t83B9EC79244C26B468C115E54C0BEF09BB2E05B5, ___Fragment_2)); }
	inline String_t* get_Fragment_2() const { return ___Fragment_2; }
	inline String_t** get_address_of_Fragment_2() { return &___Fragment_2; }
	inline void set_Fragment_2(String_t* value)
	{
		___Fragment_2 = value;
		Il2CppCodeGenWriteBarrier((&___Fragment_2), value);
	}

	inline static int32_t get_offset_of_AbsoluteUri_3() { return static_cast<int32_t>(offsetof(MoreInfo_t83B9EC79244C26B468C115E54C0BEF09BB2E05B5, ___AbsoluteUri_3)); }
	inline String_t* get_AbsoluteUri_3() const { return ___AbsoluteUri_3; }
	inline String_t** get_address_of_AbsoluteUri_3() { return &___AbsoluteUri_3; }
	inline void set_AbsoluteUri_3(String_t* value)
	{
		___AbsoluteUri_3 = value;
		Il2CppCodeGenWriteBarrier((&___AbsoluteUri_3), value);
	}

	inline static int32_t get_offset_of_Hash_4() { return static_cast<int32_t>(offsetof(MoreInfo_t83B9EC79244C26B468C115E54C0BEF09BB2E05B5, ___Hash_4)); }
	inline int32_t get_Hash_4() const { return ___Hash_4; }
	inline int32_t* get_address_of_Hash_4() { return &___Hash_4; }
	inline void set_Hash_4(int32_t value)
	{
		___Hash_4 = value;
	}

	inline static int32_t get_offset_of_RemoteUrl_5() { return static_cast<int32_t>(offsetof(MoreInfo_t83B9EC79244C26B468C115E54C0BEF09BB2E05B5, ___RemoteUrl_5)); }
	inline String_t* get_RemoteUrl_5() const { return ___RemoteUrl_5; }
	inline String_t** get_address_of_RemoteUrl_5() { return &___RemoteUrl_5; }
	inline void set_RemoteUrl_5(String_t* value)
	{
		___RemoteUrl_5 = value;
		Il2CppCodeGenWriteBarrier((&___RemoteUrl_5), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // MOREINFO_T83B9EC79244C26B468C115E54C0BEF09BB2E05B5_H
#ifndef URIBUILDER_T5823C3516668F40DA57B8F41E2AF01261B988905_H
#define URIBUILDER_T5823C3516668F40DA57B8F41E2AF01261B988905_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.UriBuilder
struct  UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905  : public RuntimeObject
{
public:
	// System.Boolean System.UriBuilder::_changed
	bool ____changed_0;
	// System.String System.UriBuilder::_fragment
	String_t* ____fragment_1;
	// System.String System.UriBuilder::_host
	String_t* ____host_2;
	// System.String System.UriBuilder::_password
	String_t* ____password_3;
	// System.String System.UriBuilder::_path
	String_t* ____path_4;
	// System.Int32 System.UriBuilder::_port
	int32_t ____port_5;
	// System.String System.UriBuilder::_query
	String_t* ____query_6;
	// System.String System.UriBuilder::_scheme
	String_t* ____scheme_7;
	// System.String System.UriBuilder::_schemeDelimiter
	String_t* ____schemeDelimiter_8;
	// System.Uri System.UriBuilder::_uri
	Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ____uri_9;
	// System.String System.UriBuilder::_username
	String_t* ____username_10;

public:
	inline static int32_t get_offset_of__changed_0() { return static_cast<int32_t>(offsetof(UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905, ____changed_0)); }
	inline bool get__changed_0() const { return ____changed_0; }
	inline bool* get_address_of__changed_0() { return &____changed_0; }
	inline void set__changed_0(bool value)
	{
		____changed_0 = value;
	}

	inline static int32_t get_offset_of__fragment_1() { return static_cast<int32_t>(offsetof(UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905, ____fragment_1)); }
	inline String_t* get__fragment_1() const { return ____fragment_1; }
	inline String_t** get_address_of__fragment_1() { return &____fragment_1; }
	inline void set__fragment_1(String_t* value)
	{
		____fragment_1 = value;
		Il2CppCodeGenWriteBarrier((&____fragment_1), value);
	}

	inline static int32_t get_offset_of__host_2() { return static_cast<int32_t>(offsetof(UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905, ____host_2)); }
	inline String_t* get__host_2() const { return ____host_2; }
	inline String_t** get_address_of__host_2() { return &____host_2; }
	inline void set__host_2(String_t* value)
	{
		____host_2 = value;
		Il2CppCodeGenWriteBarrier((&____host_2), value);
	}

	inline static int32_t get_offset_of__password_3() { return static_cast<int32_t>(offsetof(UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905, ____password_3)); }
	inline String_t* get__password_3() const { return ____password_3; }
	inline String_t** get_address_of__password_3() { return &____password_3; }
	inline void set__password_3(String_t* value)
	{
		____password_3 = value;
		Il2CppCodeGenWriteBarrier((&____password_3), value);
	}

	inline static int32_t get_offset_of__path_4() { return static_cast<int32_t>(offsetof(UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905, ____path_4)); }
	inline String_t* get__path_4() const { return ____path_4; }
	inline String_t** get_address_of__path_4() { return &____path_4; }
	inline void set__path_4(String_t* value)
	{
		____path_4 = value;
		Il2CppCodeGenWriteBarrier((&____path_4), value);
	}

	inline static int32_t get_offset_of__port_5() { return static_cast<int32_t>(offsetof(UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905, ____port_5)); }
	inline int32_t get__port_5() const { return ____port_5; }
	inline int32_t* get_address_of__port_5() { return &____port_5; }
	inline void set__port_5(int32_t value)
	{
		____port_5 = value;
	}

	inline static int32_t get_offset_of__query_6() { return static_cast<int32_t>(offsetof(UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905, ____query_6)); }
	inline String_t* get__query_6() const { return ____query_6; }
	inline String_t** get_address_of__query_6() { return &____query_6; }
	inline void set__query_6(String_t* value)
	{
		____query_6 = value;
		Il2CppCodeGenWriteBarrier((&____query_6), value);
	}

	inline static int32_t get_offset_of__scheme_7() { return static_cast<int32_t>(offsetof(UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905, ____scheme_7)); }
	inline String_t* get__scheme_7() const { return ____scheme_7; }
	inline String_t** get_address_of__scheme_7() { return &____scheme_7; }
	inline void set__scheme_7(String_t* value)
	{
		____scheme_7 = value;
		Il2CppCodeGenWriteBarrier((&____scheme_7), value);
	}

	inline static int32_t get_offset_of__schemeDelimiter_8() { return static_cast<int32_t>(offsetof(UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905, ____schemeDelimiter_8)); }
	inline String_t* get__schemeDelimiter_8() const { return ____schemeDelimiter_8; }
	inline String_t** get_address_of__schemeDelimiter_8() { return &____schemeDelimiter_8; }
	inline void set__schemeDelimiter_8(String_t* value)
	{
		____schemeDelimiter_8 = value;
		Il2CppCodeGenWriteBarrier((&____schemeDelimiter_8), value);
	}

	inline static int32_t get_offset_of__uri_9() { return static_cast<int32_t>(offsetof(UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905, ____uri_9)); }
	inline Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * get__uri_9() const { return ____uri_9; }
	inline Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E ** get_address_of__uri_9() { return &____uri_9; }
	inline void set__uri_9(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * value)
	{
		____uri_9 = value;
		Il2CppCodeGenWriteBarrier((&____uri_9), value);
	}

	inline static int32_t get_offset_of__username_10() { return static_cast<int32_t>(offsetof(UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905, ____username_10)); }
	inline String_t* get__username_10() const { return ____username_10; }
	inline String_t** get_address_of__username_10() { return &____username_10; }
	inline void set__username_10(String_t* value)
	{
		____username_10 = value;
		Il2CppCodeGenWriteBarrier((&____username_10), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // URIBUILDER_T5823C3516668F40DA57B8F41E2AF01261B988905_H
#ifndef URIHELPER_TA44F3057604BAA4E6EF06A8EE4E6825D471592DF_H
#define URIHELPER_TA44F3057604BAA4E6EF06A8EE4E6825D471592DF_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.UriHelper
struct  UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF  : public RuntimeObject
{
public:

public:
};

struct UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_StaticFields
{
public:
	// System.Char[] System.UriHelper::HexUpperChars
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___HexUpperChars_0;

public:
	inline static int32_t get_offset_of_HexUpperChars_0() { return static_cast<int32_t>(offsetof(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_StaticFields, ___HexUpperChars_0)); }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* get_HexUpperChars_0() const { return ___HexUpperChars_0; }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2** get_address_of_HexUpperChars_0() { return &___HexUpperChars_0; }
	inline void set_HexUpperChars_0(CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* value)
	{
		___HexUpperChars_0 = value;
		Il2CppCodeGenWriteBarrier((&___HexUpperChars_0), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // URIHELPER_TA44F3057604BAA4E6EF06A8EE4E6825D471592DF_H
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
#ifndef __STATICARRAYINITTYPESIZEU3D10_TE6F7FB38485D609454F9A89335B38F479C5B6086_H
#define __STATICARRAYINITTYPESIZEU3D10_TE6F7FB38485D609454F9A89335B38F479C5B6086_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D10
struct  __StaticArrayInitTypeSizeU3D10_tE6F7FB38485D609454F9A89335B38F479C5B6086 
{
public:
	union
	{
		struct
		{
			union
			{
			};
		};
		uint8_t __StaticArrayInitTypeSizeU3D10_tE6F7FB38485D609454F9A89335B38F479C5B6086__padding[10];
	};

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // __STATICARRAYINITTYPESIZEU3D10_TE6F7FB38485D609454F9A89335B38F479C5B6086_H
#ifndef __STATICARRAYINITTYPESIZEU3D12_T6EBCA221EDFF79F50821238316CFA0302EE70E48_H
#define __STATICARRAYINITTYPESIZEU3D12_T6EBCA221EDFF79F50821238316CFA0302EE70E48_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D12
struct  __StaticArrayInitTypeSizeU3D12_t6EBCA221EDFF79F50821238316CFA0302EE70E48 
{
public:
	union
	{
		struct
		{
			union
			{
			};
		};
		uint8_t __StaticArrayInitTypeSizeU3D12_t6EBCA221EDFF79F50821238316CFA0302EE70E48__padding[12];
	};

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // __STATICARRAYINITTYPESIZEU3D12_T6EBCA221EDFF79F50821238316CFA0302EE70E48_H
#ifndef __STATICARRAYINITTYPESIZEU3D128_T4A42759E6E25B0C61E6036A661F4344DE92C2905_H
#define __STATICARRAYINITTYPESIZEU3D128_T4A42759E6E25B0C61E6036A661F4344DE92C2905_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D128
struct  __StaticArrayInitTypeSizeU3D128_t4A42759E6E25B0C61E6036A661F4344DE92C2905 
{
public:
	union
	{
		struct
		{
			union
			{
			};
		};
		uint8_t __StaticArrayInitTypeSizeU3D128_t4A42759E6E25B0C61E6036A661F4344DE92C2905__padding[128];
	};

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // __STATICARRAYINITTYPESIZEU3D128_T4A42759E6E25B0C61E6036A661F4344DE92C2905_H
#ifndef __STATICARRAYINITTYPESIZEU3D14_TC5D421D768E79910C98FB4504BA3B07E43FA77F0_H
#define __STATICARRAYINITTYPESIZEU3D14_TC5D421D768E79910C98FB4504BA3B07E43FA77F0_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D14
struct  __StaticArrayInitTypeSizeU3D14_tC5D421D768E79910C98FB4504BA3B07E43FA77F0 
{
public:
	union
	{
		struct
		{
			union
			{
			};
		};
		uint8_t __StaticArrayInitTypeSizeU3D14_tC5D421D768E79910C98FB4504BA3B07E43FA77F0__padding[14];
	};

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // __STATICARRAYINITTYPESIZEU3D14_TC5D421D768E79910C98FB4504BA3B07E43FA77F0_H
#ifndef __STATICARRAYINITTYPESIZEU3D256_T548520FAA2CCFC11107E283BF9E43588FAE5F6C7_H
#define __STATICARRAYINITTYPESIZEU3D256_T548520FAA2CCFC11107E283BF9E43588FAE5F6C7_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D256
struct  __StaticArrayInitTypeSizeU3D256_t548520FAA2CCFC11107E283BF9E43588FAE5F6C7 
{
public:
	union
	{
		struct
		{
			union
			{
			};
		};
		uint8_t __StaticArrayInitTypeSizeU3D256_t548520FAA2CCFC11107E283BF9E43588FAE5F6C7__padding[256];
	};

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // __STATICARRAYINITTYPESIZEU3D256_T548520FAA2CCFC11107E283BF9E43588FAE5F6C7_H
#ifndef __STATICARRAYINITTYPESIZEU3D3_T4D597C014C0C24F294DC84275F0264DCFCD4C575_H
#define __STATICARRAYINITTYPESIZEU3D3_T4D597C014C0C24F294DC84275F0264DCFCD4C575_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D3
struct  __StaticArrayInitTypeSizeU3D3_t4D597C014C0C24F294DC84275F0264DCFCD4C575 
{
public:
	union
	{
		struct
		{
			union
			{
			};
		};
		uint8_t __StaticArrayInitTypeSizeU3D3_t4D597C014C0C24F294DC84275F0264DCFCD4C575__padding[3];
	};

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // __STATICARRAYINITTYPESIZEU3D3_T4D597C014C0C24F294DC84275F0264DCFCD4C575_H
#ifndef __STATICARRAYINITTYPESIZEU3D32_T5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA_H
#define __STATICARRAYINITTYPESIZEU3D32_T5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D32
struct  __StaticArrayInitTypeSizeU3D32_t5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA 
{
public:
	union
	{
		struct
		{
			union
			{
			};
		};
		uint8_t __StaticArrayInitTypeSizeU3D32_t5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA__padding[32];
	};

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // __STATICARRAYINITTYPESIZEU3D32_T5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA_H
#ifndef __STATICARRAYINITTYPESIZEU3D44_TE99A9434272A367C976B32D1235A23DA85CC9671_H
#define __STATICARRAYINITTYPESIZEU3D44_TE99A9434272A367C976B32D1235A23DA85CC9671_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D44
struct  __StaticArrayInitTypeSizeU3D44_tE99A9434272A367C976B32D1235A23DA85CC9671 
{
public:
	union
	{
		struct
		{
			union
			{
			};
		};
		uint8_t __StaticArrayInitTypeSizeU3D44_tE99A9434272A367C976B32D1235A23DA85CC9671__padding[44];
	};

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // __STATICARRAYINITTYPESIZEU3D44_TE99A9434272A367C976B32D1235A23DA85CC9671_H
#ifndef __STATICARRAYINITTYPESIZEU3D6_TB024AE1C3AEB5C43235E76FFA23E64CD5EC3D87F_H
#define __STATICARRAYINITTYPESIZEU3D6_TB024AE1C3AEB5C43235E76FFA23E64CD5EC3D87F_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D6
struct  __StaticArrayInitTypeSizeU3D6_tB024AE1C3AEB5C43235E76FFA23E64CD5EC3D87F 
{
public:
	union
	{
		struct
		{
			union
			{
			};
		};
		uint8_t __StaticArrayInitTypeSizeU3D6_tB024AE1C3AEB5C43235E76FFA23E64CD5EC3D87F__padding[6];
	};

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // __STATICARRAYINITTYPESIZEU3D6_TB024AE1C3AEB5C43235E76FFA23E64CD5EC3D87F_H
#ifndef __STATICARRAYINITTYPESIZEU3D9_TAB3C7ADC1E437C21F21AAF2C925676D0F9801BCB_H
#define __STATICARRAYINITTYPESIZEU3D9_TAB3C7ADC1E437C21F21AAF2C925676D0F9801BCB_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D9
struct  __StaticArrayInitTypeSizeU3D9_tAB3C7ADC1E437C21F21AAF2C925676D0F9801BCB 
{
public:
	union
	{
		struct
		{
			union
			{
			};
		};
		uint8_t __StaticArrayInitTypeSizeU3D9_tAB3C7ADC1E437C21F21AAF2C925676D0F9801BCB__padding[9];
	};

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // __STATICARRAYINITTYPESIZEU3D9_TAB3C7ADC1E437C21F21AAF2C925676D0F9801BCB_H
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
#ifndef INT16_T823A20635DAF5A3D93A1E01CFBF3CBA27CF00B4D_H
#define INT16_T823A20635DAF5A3D93A1E01CFBF3CBA27CF00B4D_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Int16
struct  Int16_t823A20635DAF5A3D93A1E01CFBF3CBA27CF00B4D 
{
public:
	// System.Int16 System.Int16::m_value
	int16_t ___m_value_0;

public:
	inline static int32_t get_offset_of_m_value_0() { return static_cast<int32_t>(offsetof(Int16_t823A20635DAF5A3D93A1E01CFBF3CBA27CF00B4D, ___m_value_0)); }
	inline int16_t get_m_value_0() const { return ___m_value_0; }
	inline int16_t* get_address_of_m_value_0() { return &___m_value_0; }
	inline void set_m_value_0(int16_t value)
	{
		___m_value_0 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // INT16_T823A20635DAF5A3D93A1E01CFBF3CBA27CF00B4D_H
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
#ifndef DECODERREPLACEMENTFALLBACK_T8CF74B2DAE2A08AEA7DF6366778D2E3EA75FC742_H
#define DECODERREPLACEMENTFALLBACK_T8CF74B2DAE2A08AEA7DF6366778D2E3EA75FC742_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Text.DecoderReplacementFallback
struct  DecoderReplacementFallback_t8CF74B2DAE2A08AEA7DF6366778D2E3EA75FC742  : public DecoderFallback_t128445EB7676870485230893338EF044F6B72F60
{
public:
	// System.String System.Text.DecoderReplacementFallback::strDefault
	String_t* ___strDefault_4;

public:
	inline static int32_t get_offset_of_strDefault_4() { return static_cast<int32_t>(offsetof(DecoderReplacementFallback_t8CF74B2DAE2A08AEA7DF6366778D2E3EA75FC742, ___strDefault_4)); }
	inline String_t* get_strDefault_4() const { return ___strDefault_4; }
	inline String_t** get_address_of_strDefault_4() { return &___strDefault_4; }
	inline void set_strDefault_4(String_t* value)
	{
		___strDefault_4 = value;
		Il2CppCodeGenWriteBarrier((&___strDefault_4), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // DECODERREPLACEMENTFALLBACK_T8CF74B2DAE2A08AEA7DF6366778D2E3EA75FC742_H
#ifndef ENCODERREPLACEMENTFALLBACK_TC2E8A94C82BBF7A4CFC8E3FDBA8A381DCF29F998_H
#define ENCODERREPLACEMENTFALLBACK_TC2E8A94C82BBF7A4CFC8E3FDBA8A381DCF29F998_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Text.EncoderReplacementFallback
struct  EncoderReplacementFallback_tC2E8A94C82BBF7A4CFC8E3FDBA8A381DCF29F998  : public EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63
{
public:
	// System.String System.Text.EncoderReplacementFallback::strDefault
	String_t* ___strDefault_4;

public:
	inline static int32_t get_offset_of_strDefault_4() { return static_cast<int32_t>(offsetof(EncoderReplacementFallback_tC2E8A94C82BBF7A4CFC8E3FDBA8A381DCF29F998, ___strDefault_4)); }
	inline String_t* get_strDefault_4() const { return ___strDefault_4; }
	inline String_t** get_address_of_strDefault_4() { return &___strDefault_4; }
	inline void set_strDefault_4(String_t* value)
	{
		___strDefault_4 = value;
		Il2CppCodeGenWriteBarrier((&___strDefault_4), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // ENCODERREPLACEMENTFALLBACK_TC2E8A94C82BBF7A4CFC8E3FDBA8A381DCF29F998_H
#ifndef OFFSET_T4D3750A78885B564FB4602C405B9EFF5A32066C7_H
#define OFFSET_T4D3750A78885B564FB4602C405B9EFF5A32066C7_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Uri_Offset
#pragma pack(push, tp, 1)
struct  Offset_t4D3750A78885B564FB4602C405B9EFF5A32066C7 
{
public:
	// System.UInt16 System.Uri_Offset::Scheme
	uint16_t ___Scheme_0;
	// System.UInt16 System.Uri_Offset::User
	uint16_t ___User_1;
	// System.UInt16 System.Uri_Offset::Host
	uint16_t ___Host_2;
	// System.UInt16 System.Uri_Offset::PortValue
	uint16_t ___PortValue_3;
	// System.UInt16 System.Uri_Offset::Path
	uint16_t ___Path_4;
	// System.UInt16 System.Uri_Offset::Query
	uint16_t ___Query_5;
	// System.UInt16 System.Uri_Offset::Fragment
	uint16_t ___Fragment_6;
	// System.UInt16 System.Uri_Offset::End
	uint16_t ___End_7;

public:
	inline static int32_t get_offset_of_Scheme_0() { return static_cast<int32_t>(offsetof(Offset_t4D3750A78885B564FB4602C405B9EFF5A32066C7, ___Scheme_0)); }
	inline uint16_t get_Scheme_0() const { return ___Scheme_0; }
	inline uint16_t* get_address_of_Scheme_0() { return &___Scheme_0; }
	inline void set_Scheme_0(uint16_t value)
	{
		___Scheme_0 = value;
	}

	inline static int32_t get_offset_of_User_1() { return static_cast<int32_t>(offsetof(Offset_t4D3750A78885B564FB4602C405B9EFF5A32066C7, ___User_1)); }
	inline uint16_t get_User_1() const { return ___User_1; }
	inline uint16_t* get_address_of_User_1() { return &___User_1; }
	inline void set_User_1(uint16_t value)
	{
		___User_1 = value;
	}

	inline static int32_t get_offset_of_Host_2() { return static_cast<int32_t>(offsetof(Offset_t4D3750A78885B564FB4602C405B9EFF5A32066C7, ___Host_2)); }
	inline uint16_t get_Host_2() const { return ___Host_2; }
	inline uint16_t* get_address_of_Host_2() { return &___Host_2; }
	inline void set_Host_2(uint16_t value)
	{
		___Host_2 = value;
	}

	inline static int32_t get_offset_of_PortValue_3() { return static_cast<int32_t>(offsetof(Offset_t4D3750A78885B564FB4602C405B9EFF5A32066C7, ___PortValue_3)); }
	inline uint16_t get_PortValue_3() const { return ___PortValue_3; }
	inline uint16_t* get_address_of_PortValue_3() { return &___PortValue_3; }
	inline void set_PortValue_3(uint16_t value)
	{
		___PortValue_3 = value;
	}

	inline static int32_t get_offset_of_Path_4() { return static_cast<int32_t>(offsetof(Offset_t4D3750A78885B564FB4602C405B9EFF5A32066C7, ___Path_4)); }
	inline uint16_t get_Path_4() const { return ___Path_4; }
	inline uint16_t* get_address_of_Path_4() { return &___Path_4; }
	inline void set_Path_4(uint16_t value)
	{
		___Path_4 = value;
	}

	inline static int32_t get_offset_of_Query_5() { return static_cast<int32_t>(offsetof(Offset_t4D3750A78885B564FB4602C405B9EFF5A32066C7, ___Query_5)); }
	inline uint16_t get_Query_5() const { return ___Query_5; }
	inline uint16_t* get_address_of_Query_5() { return &___Query_5; }
	inline void set_Query_5(uint16_t value)
	{
		___Query_5 = value;
	}

	inline static int32_t get_offset_of_Fragment_6() { return static_cast<int32_t>(offsetof(Offset_t4D3750A78885B564FB4602C405B9EFF5A32066C7, ___Fragment_6)); }
	inline uint16_t get_Fragment_6() const { return ___Fragment_6; }
	inline uint16_t* get_address_of_Fragment_6() { return &___Fragment_6; }
	inline void set_Fragment_6(uint16_t value)
	{
		___Fragment_6 = value;
	}

	inline static int32_t get_offset_of_End_7() { return static_cast<int32_t>(offsetof(Offset_t4D3750A78885B564FB4602C405B9EFF5A32066C7, ___End_7)); }
	inline uint16_t get_End_7() const { return ___End_7; }
	inline uint16_t* get_address_of_End_7() { return &___End_7; }
	inline void set_End_7(uint16_t value)
	{
		___End_7 = value;
	}
};
#pragma pack(pop, tp)

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // OFFSET_T4D3750A78885B564FB4602C405B9EFF5A32066C7_H
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
#ifndef U3CPRIVATEIMPLEMENTATIONDETAILSU3E_TD3F45A95FC1F3A32916F221D83F290D182AD6291_H
#define U3CPRIVATEIMPLEMENTATIONDETAILSU3E_TD3F45A95FC1F3A32916F221D83F290D182AD6291_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// <PrivateImplementationDetails>
struct  U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291  : public RuntimeObject
{
public:

public:
};

struct U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields
{
public:
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D14 <PrivateImplementationDetails>::0283A6AF88802AB45989B29549915BEA0F6CD515
	__StaticArrayInitTypeSizeU3D14_tC5D421D768E79910C98FB4504BA3B07E43FA77F0  ___0283A6AF88802AB45989B29549915BEA0F6CD515_0;
	// System.Int64 <PrivateImplementationDetails>::03F4297FCC30D0FD5E420E5D26E7FA711167C7EF
	int64_t ___03F4297FCC30D0FD5E420E5D26E7FA711167C7EF_1;
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D9 <PrivateImplementationDetails>::1A39764B112685485A5BA7B2880D878B858C1A7A
	__StaticArrayInitTypeSizeU3D9_tAB3C7ADC1E437C21F21AAF2C925676D0F9801BCB  ___1A39764B112685485A5BA7B2880D878B858C1A7A_2;
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D3 <PrivateImplementationDetails>::1A84029C80CB5518379F199F53FF08A7B764F8FD
	__StaticArrayInitTypeSizeU3D3_t4D597C014C0C24F294DC84275F0264DCFCD4C575  ___1A84029C80CB5518379F199F53FF08A7B764F8FD_3;
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D12 <PrivateImplementationDetails>::3BE77BF818331C2D8400FFFFF9FADD3F16AD89AC
	__StaticArrayInitTypeSizeU3D12_t6EBCA221EDFF79F50821238316CFA0302EE70E48  ___3BE77BF818331C2D8400FFFFF9FADD3F16AD89AC_4;
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D10 <PrivateImplementationDetails>::53437C3B2572EDB9B8640C3195DF3BC2729C5EA1
	__StaticArrayInitTypeSizeU3D10_tE6F7FB38485D609454F9A89335B38F479C5B6086  ___53437C3B2572EDB9B8640C3195DF3BC2729C5EA1_5;
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D32 <PrivateImplementationDetails>::59F5BD34B6C013DEACC784F69C67E95150033A84
	__StaticArrayInitTypeSizeU3D32_t5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA  ___59F5BD34B6C013DEACC784F69C67E95150033A84_6;
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D6 <PrivateImplementationDetails>::5BC3486B05BA8CF4689C7BDB198B3F477BB4E20C
	__StaticArrayInitTypeSizeU3D6_tB024AE1C3AEB5C43235E76FFA23E64CD5EC3D87F  ___5BC3486B05BA8CF4689C7BDB198B3F477BB4E20C_7;
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D9 <PrivateImplementationDetails>::6D49C9D487D7AD3491ECE08732D68A593CC2038D
	__StaticArrayInitTypeSizeU3D9_tAB3C7ADC1E437C21F21AAF2C925676D0F9801BCB  ___6D49C9D487D7AD3491ECE08732D68A593CC2038D_8;
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D128 <PrivateImplementationDetails>::6F3AD3DC3AF8047587C4C9D696EB68A01FEF796E
	__StaticArrayInitTypeSizeU3D128_t4A42759E6E25B0C61E6036A661F4344DE92C2905  ___6F3AD3DC3AF8047587C4C9D696EB68A01FEF796E_9;
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D44 <PrivateImplementationDetails>::8E0EF3D67A3EB1863224EE3CACB424BC2F8CFBA3
	__StaticArrayInitTypeSizeU3D44_tE99A9434272A367C976B32D1235A23DA85CC9671  ___8E0EF3D67A3EB1863224EE3CACB424BC2F8CFBA3_10;
	// System.Int64 <PrivateImplementationDetails>::98A44A6F8606AE6F23FE230286C1D6FBCC407226
	int64_t ___98A44A6F8606AE6F23FE230286C1D6FBCC407226_11;
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D32 <PrivateImplementationDetails>::C02C28AFEBE998F767E4AF43E3BE8F5E9FA11536
	__StaticArrayInitTypeSizeU3D32_t5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA  ___C02C28AFEBE998F767E4AF43E3BE8F5E9FA11536_12;
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D128 <PrivateImplementationDetails>::CCEEADA43268372341F81AE0C9208C6856441C04
	__StaticArrayInitTypeSizeU3D128_t4A42759E6E25B0C61E6036A661F4344DE92C2905  ___CCEEADA43268372341F81AE0C9208C6856441C04_13;
	// System.Int64 <PrivateImplementationDetails>::E5BC1BAFADE1862DD6E0B9FB632BFAA6C3873A78
	int64_t ___E5BC1BAFADE1862DD6E0B9FB632BFAA6C3873A78_14;
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D32 <PrivateImplementationDetails>::EC5842B3154E1AF94500B57220EB9F684BCCC42A
	__StaticArrayInitTypeSizeU3D32_t5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA  ___EC5842B3154E1AF94500B57220EB9F684BCCC42A_15;
	// <PrivateImplementationDetails>___StaticArrayInitTypeSizeU3D256 <PrivateImplementationDetails>::EEAFE8C6E1AB017237567305EE925C976CDB6458
	__StaticArrayInitTypeSizeU3D256_t548520FAA2CCFC11107E283BF9E43588FAE5F6C7  ___EEAFE8C6E1AB017237567305EE925C976CDB6458_16;

public:
	inline static int32_t get_offset_of_U30283A6AF88802AB45989B29549915BEA0F6CD515_0() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___0283A6AF88802AB45989B29549915BEA0F6CD515_0)); }
	inline __StaticArrayInitTypeSizeU3D14_tC5D421D768E79910C98FB4504BA3B07E43FA77F0  get_U30283A6AF88802AB45989B29549915BEA0F6CD515_0() const { return ___0283A6AF88802AB45989B29549915BEA0F6CD515_0; }
	inline __StaticArrayInitTypeSizeU3D14_tC5D421D768E79910C98FB4504BA3B07E43FA77F0 * get_address_of_U30283A6AF88802AB45989B29549915BEA0F6CD515_0() { return &___0283A6AF88802AB45989B29549915BEA0F6CD515_0; }
	inline void set_U30283A6AF88802AB45989B29549915BEA0F6CD515_0(__StaticArrayInitTypeSizeU3D14_tC5D421D768E79910C98FB4504BA3B07E43FA77F0  value)
	{
		___0283A6AF88802AB45989B29549915BEA0F6CD515_0 = value;
	}

	inline static int32_t get_offset_of_U303F4297FCC30D0FD5E420E5D26E7FA711167C7EF_1() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___03F4297FCC30D0FD5E420E5D26E7FA711167C7EF_1)); }
	inline int64_t get_U303F4297FCC30D0FD5E420E5D26E7FA711167C7EF_1() const { return ___03F4297FCC30D0FD5E420E5D26E7FA711167C7EF_1; }
	inline int64_t* get_address_of_U303F4297FCC30D0FD5E420E5D26E7FA711167C7EF_1() { return &___03F4297FCC30D0FD5E420E5D26E7FA711167C7EF_1; }
	inline void set_U303F4297FCC30D0FD5E420E5D26E7FA711167C7EF_1(int64_t value)
	{
		___03F4297FCC30D0FD5E420E5D26E7FA711167C7EF_1 = value;
	}

	inline static int32_t get_offset_of_U31A39764B112685485A5BA7B2880D878B858C1A7A_2() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___1A39764B112685485A5BA7B2880D878B858C1A7A_2)); }
	inline __StaticArrayInitTypeSizeU3D9_tAB3C7ADC1E437C21F21AAF2C925676D0F9801BCB  get_U31A39764B112685485A5BA7B2880D878B858C1A7A_2() const { return ___1A39764B112685485A5BA7B2880D878B858C1A7A_2; }
	inline __StaticArrayInitTypeSizeU3D9_tAB3C7ADC1E437C21F21AAF2C925676D0F9801BCB * get_address_of_U31A39764B112685485A5BA7B2880D878B858C1A7A_2() { return &___1A39764B112685485A5BA7B2880D878B858C1A7A_2; }
	inline void set_U31A39764B112685485A5BA7B2880D878B858C1A7A_2(__StaticArrayInitTypeSizeU3D9_tAB3C7ADC1E437C21F21AAF2C925676D0F9801BCB  value)
	{
		___1A39764B112685485A5BA7B2880D878B858C1A7A_2 = value;
	}

	inline static int32_t get_offset_of_U31A84029C80CB5518379F199F53FF08A7B764F8FD_3() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___1A84029C80CB5518379F199F53FF08A7B764F8FD_3)); }
	inline __StaticArrayInitTypeSizeU3D3_t4D597C014C0C24F294DC84275F0264DCFCD4C575  get_U31A84029C80CB5518379F199F53FF08A7B764F8FD_3() const { return ___1A84029C80CB5518379F199F53FF08A7B764F8FD_3; }
	inline __StaticArrayInitTypeSizeU3D3_t4D597C014C0C24F294DC84275F0264DCFCD4C575 * get_address_of_U31A84029C80CB5518379F199F53FF08A7B764F8FD_3() { return &___1A84029C80CB5518379F199F53FF08A7B764F8FD_3; }
	inline void set_U31A84029C80CB5518379F199F53FF08A7B764F8FD_3(__StaticArrayInitTypeSizeU3D3_t4D597C014C0C24F294DC84275F0264DCFCD4C575  value)
	{
		___1A84029C80CB5518379F199F53FF08A7B764F8FD_3 = value;
	}

	inline static int32_t get_offset_of_U33BE77BF818331C2D8400FFFFF9FADD3F16AD89AC_4() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___3BE77BF818331C2D8400FFFFF9FADD3F16AD89AC_4)); }
	inline __StaticArrayInitTypeSizeU3D12_t6EBCA221EDFF79F50821238316CFA0302EE70E48  get_U33BE77BF818331C2D8400FFFFF9FADD3F16AD89AC_4() const { return ___3BE77BF818331C2D8400FFFFF9FADD3F16AD89AC_4; }
	inline __StaticArrayInitTypeSizeU3D12_t6EBCA221EDFF79F50821238316CFA0302EE70E48 * get_address_of_U33BE77BF818331C2D8400FFFFF9FADD3F16AD89AC_4() { return &___3BE77BF818331C2D8400FFFFF9FADD3F16AD89AC_4; }
	inline void set_U33BE77BF818331C2D8400FFFFF9FADD3F16AD89AC_4(__StaticArrayInitTypeSizeU3D12_t6EBCA221EDFF79F50821238316CFA0302EE70E48  value)
	{
		___3BE77BF818331C2D8400FFFFF9FADD3F16AD89AC_4 = value;
	}

	inline static int32_t get_offset_of_U353437C3B2572EDB9B8640C3195DF3BC2729C5EA1_5() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___53437C3B2572EDB9B8640C3195DF3BC2729C5EA1_5)); }
	inline __StaticArrayInitTypeSizeU3D10_tE6F7FB38485D609454F9A89335B38F479C5B6086  get_U353437C3B2572EDB9B8640C3195DF3BC2729C5EA1_5() const { return ___53437C3B2572EDB9B8640C3195DF3BC2729C5EA1_5; }
	inline __StaticArrayInitTypeSizeU3D10_tE6F7FB38485D609454F9A89335B38F479C5B6086 * get_address_of_U353437C3B2572EDB9B8640C3195DF3BC2729C5EA1_5() { return &___53437C3B2572EDB9B8640C3195DF3BC2729C5EA1_5; }
	inline void set_U353437C3B2572EDB9B8640C3195DF3BC2729C5EA1_5(__StaticArrayInitTypeSizeU3D10_tE6F7FB38485D609454F9A89335B38F479C5B6086  value)
	{
		___53437C3B2572EDB9B8640C3195DF3BC2729C5EA1_5 = value;
	}

	inline static int32_t get_offset_of_U359F5BD34B6C013DEACC784F69C67E95150033A84_6() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___59F5BD34B6C013DEACC784F69C67E95150033A84_6)); }
	inline __StaticArrayInitTypeSizeU3D32_t5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA  get_U359F5BD34B6C013DEACC784F69C67E95150033A84_6() const { return ___59F5BD34B6C013DEACC784F69C67E95150033A84_6; }
	inline __StaticArrayInitTypeSizeU3D32_t5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA * get_address_of_U359F5BD34B6C013DEACC784F69C67E95150033A84_6() { return &___59F5BD34B6C013DEACC784F69C67E95150033A84_6; }
	inline void set_U359F5BD34B6C013DEACC784F69C67E95150033A84_6(__StaticArrayInitTypeSizeU3D32_t5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA  value)
	{
		___59F5BD34B6C013DEACC784F69C67E95150033A84_6 = value;
	}

	inline static int32_t get_offset_of_U35BC3486B05BA8CF4689C7BDB198B3F477BB4E20C_7() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___5BC3486B05BA8CF4689C7BDB198B3F477BB4E20C_7)); }
	inline __StaticArrayInitTypeSizeU3D6_tB024AE1C3AEB5C43235E76FFA23E64CD5EC3D87F  get_U35BC3486B05BA8CF4689C7BDB198B3F477BB4E20C_7() const { return ___5BC3486B05BA8CF4689C7BDB198B3F477BB4E20C_7; }
	inline __StaticArrayInitTypeSizeU3D6_tB024AE1C3AEB5C43235E76FFA23E64CD5EC3D87F * get_address_of_U35BC3486B05BA8CF4689C7BDB198B3F477BB4E20C_7() { return &___5BC3486B05BA8CF4689C7BDB198B3F477BB4E20C_7; }
	inline void set_U35BC3486B05BA8CF4689C7BDB198B3F477BB4E20C_7(__StaticArrayInitTypeSizeU3D6_tB024AE1C3AEB5C43235E76FFA23E64CD5EC3D87F  value)
	{
		___5BC3486B05BA8CF4689C7BDB198B3F477BB4E20C_7 = value;
	}

	inline static int32_t get_offset_of_U36D49C9D487D7AD3491ECE08732D68A593CC2038D_8() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___6D49C9D487D7AD3491ECE08732D68A593CC2038D_8)); }
	inline __StaticArrayInitTypeSizeU3D9_tAB3C7ADC1E437C21F21AAF2C925676D0F9801BCB  get_U36D49C9D487D7AD3491ECE08732D68A593CC2038D_8() const { return ___6D49C9D487D7AD3491ECE08732D68A593CC2038D_8; }
	inline __StaticArrayInitTypeSizeU3D9_tAB3C7ADC1E437C21F21AAF2C925676D0F9801BCB * get_address_of_U36D49C9D487D7AD3491ECE08732D68A593CC2038D_8() { return &___6D49C9D487D7AD3491ECE08732D68A593CC2038D_8; }
	inline void set_U36D49C9D487D7AD3491ECE08732D68A593CC2038D_8(__StaticArrayInitTypeSizeU3D9_tAB3C7ADC1E437C21F21AAF2C925676D0F9801BCB  value)
	{
		___6D49C9D487D7AD3491ECE08732D68A593CC2038D_8 = value;
	}

	inline static int32_t get_offset_of_U36F3AD3DC3AF8047587C4C9D696EB68A01FEF796E_9() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___6F3AD3DC3AF8047587C4C9D696EB68A01FEF796E_9)); }
	inline __StaticArrayInitTypeSizeU3D128_t4A42759E6E25B0C61E6036A661F4344DE92C2905  get_U36F3AD3DC3AF8047587C4C9D696EB68A01FEF796E_9() const { return ___6F3AD3DC3AF8047587C4C9D696EB68A01FEF796E_9; }
	inline __StaticArrayInitTypeSizeU3D128_t4A42759E6E25B0C61E6036A661F4344DE92C2905 * get_address_of_U36F3AD3DC3AF8047587C4C9D696EB68A01FEF796E_9() { return &___6F3AD3DC3AF8047587C4C9D696EB68A01FEF796E_9; }
	inline void set_U36F3AD3DC3AF8047587C4C9D696EB68A01FEF796E_9(__StaticArrayInitTypeSizeU3D128_t4A42759E6E25B0C61E6036A661F4344DE92C2905  value)
	{
		___6F3AD3DC3AF8047587C4C9D696EB68A01FEF796E_9 = value;
	}

	inline static int32_t get_offset_of_U38E0EF3D67A3EB1863224EE3CACB424BC2F8CFBA3_10() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___8E0EF3D67A3EB1863224EE3CACB424BC2F8CFBA3_10)); }
	inline __StaticArrayInitTypeSizeU3D44_tE99A9434272A367C976B32D1235A23DA85CC9671  get_U38E0EF3D67A3EB1863224EE3CACB424BC2F8CFBA3_10() const { return ___8E0EF3D67A3EB1863224EE3CACB424BC2F8CFBA3_10; }
	inline __StaticArrayInitTypeSizeU3D44_tE99A9434272A367C976B32D1235A23DA85CC9671 * get_address_of_U38E0EF3D67A3EB1863224EE3CACB424BC2F8CFBA3_10() { return &___8E0EF3D67A3EB1863224EE3CACB424BC2F8CFBA3_10; }
	inline void set_U38E0EF3D67A3EB1863224EE3CACB424BC2F8CFBA3_10(__StaticArrayInitTypeSizeU3D44_tE99A9434272A367C976B32D1235A23DA85CC9671  value)
	{
		___8E0EF3D67A3EB1863224EE3CACB424BC2F8CFBA3_10 = value;
	}

	inline static int32_t get_offset_of_U398A44A6F8606AE6F23FE230286C1D6FBCC407226_11() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___98A44A6F8606AE6F23FE230286C1D6FBCC407226_11)); }
	inline int64_t get_U398A44A6F8606AE6F23FE230286C1D6FBCC407226_11() const { return ___98A44A6F8606AE6F23FE230286C1D6FBCC407226_11; }
	inline int64_t* get_address_of_U398A44A6F8606AE6F23FE230286C1D6FBCC407226_11() { return &___98A44A6F8606AE6F23FE230286C1D6FBCC407226_11; }
	inline void set_U398A44A6F8606AE6F23FE230286C1D6FBCC407226_11(int64_t value)
	{
		___98A44A6F8606AE6F23FE230286C1D6FBCC407226_11 = value;
	}

	inline static int32_t get_offset_of_C02C28AFEBE998F767E4AF43E3BE8F5E9FA11536_12() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___C02C28AFEBE998F767E4AF43E3BE8F5E9FA11536_12)); }
	inline __StaticArrayInitTypeSizeU3D32_t5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA  get_C02C28AFEBE998F767E4AF43E3BE8F5E9FA11536_12() const { return ___C02C28AFEBE998F767E4AF43E3BE8F5E9FA11536_12; }
	inline __StaticArrayInitTypeSizeU3D32_t5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA * get_address_of_C02C28AFEBE998F767E4AF43E3BE8F5E9FA11536_12() { return &___C02C28AFEBE998F767E4AF43E3BE8F5E9FA11536_12; }
	inline void set_C02C28AFEBE998F767E4AF43E3BE8F5E9FA11536_12(__StaticArrayInitTypeSizeU3D32_t5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA  value)
	{
		___C02C28AFEBE998F767E4AF43E3BE8F5E9FA11536_12 = value;
	}

	inline static int32_t get_offset_of_CCEEADA43268372341F81AE0C9208C6856441C04_13() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___CCEEADA43268372341F81AE0C9208C6856441C04_13)); }
	inline __StaticArrayInitTypeSizeU3D128_t4A42759E6E25B0C61E6036A661F4344DE92C2905  get_CCEEADA43268372341F81AE0C9208C6856441C04_13() const { return ___CCEEADA43268372341F81AE0C9208C6856441C04_13; }
	inline __StaticArrayInitTypeSizeU3D128_t4A42759E6E25B0C61E6036A661F4344DE92C2905 * get_address_of_CCEEADA43268372341F81AE0C9208C6856441C04_13() { return &___CCEEADA43268372341F81AE0C9208C6856441C04_13; }
	inline void set_CCEEADA43268372341F81AE0C9208C6856441C04_13(__StaticArrayInitTypeSizeU3D128_t4A42759E6E25B0C61E6036A661F4344DE92C2905  value)
	{
		___CCEEADA43268372341F81AE0C9208C6856441C04_13 = value;
	}

	inline static int32_t get_offset_of_E5BC1BAFADE1862DD6E0B9FB632BFAA6C3873A78_14() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___E5BC1BAFADE1862DD6E0B9FB632BFAA6C3873A78_14)); }
	inline int64_t get_E5BC1BAFADE1862DD6E0B9FB632BFAA6C3873A78_14() const { return ___E5BC1BAFADE1862DD6E0B9FB632BFAA6C3873A78_14; }
	inline int64_t* get_address_of_E5BC1BAFADE1862DD6E0B9FB632BFAA6C3873A78_14() { return &___E5BC1BAFADE1862DD6E0B9FB632BFAA6C3873A78_14; }
	inline void set_E5BC1BAFADE1862DD6E0B9FB632BFAA6C3873A78_14(int64_t value)
	{
		___E5BC1BAFADE1862DD6E0B9FB632BFAA6C3873A78_14 = value;
	}

	inline static int32_t get_offset_of_EC5842B3154E1AF94500B57220EB9F684BCCC42A_15() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___EC5842B3154E1AF94500B57220EB9F684BCCC42A_15)); }
	inline __StaticArrayInitTypeSizeU3D32_t5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA  get_EC5842B3154E1AF94500B57220EB9F684BCCC42A_15() const { return ___EC5842B3154E1AF94500B57220EB9F684BCCC42A_15; }
	inline __StaticArrayInitTypeSizeU3D32_t5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA * get_address_of_EC5842B3154E1AF94500B57220EB9F684BCCC42A_15() { return &___EC5842B3154E1AF94500B57220EB9F684BCCC42A_15; }
	inline void set_EC5842B3154E1AF94500B57220EB9F684BCCC42A_15(__StaticArrayInitTypeSizeU3D32_t5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA  value)
	{
		___EC5842B3154E1AF94500B57220EB9F684BCCC42A_15 = value;
	}

	inline static int32_t get_offset_of_EEAFE8C6E1AB017237567305EE925C976CDB6458_16() { return static_cast<int32_t>(offsetof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields, ___EEAFE8C6E1AB017237567305EE925C976CDB6458_16)); }
	inline __StaticArrayInitTypeSizeU3D256_t548520FAA2CCFC11107E283BF9E43588FAE5F6C7  get_EEAFE8C6E1AB017237567305EE925C976CDB6458_16() const { return ___EEAFE8C6E1AB017237567305EE925C976CDB6458_16; }
	inline __StaticArrayInitTypeSizeU3D256_t548520FAA2CCFC11107E283BF9E43588FAE5F6C7 * get_address_of_EEAFE8C6E1AB017237567305EE925C976CDB6458_16() { return &___EEAFE8C6E1AB017237567305EE925C976CDB6458_16; }
	inline void set_EEAFE8C6E1AB017237567305EE925C976CDB6458_16(__StaticArrayInitTypeSizeU3D256_t548520FAA2CCFC11107E283BF9E43588FAE5F6C7  value)
	{
		___EEAFE8C6E1AB017237567305EE925C976CDB6458_16 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // U3CPRIVATEIMPLEMENTATIONDETAILSU3E_TD3F45A95FC1F3A32916F221D83F290D182AD6291_H
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
#ifndef TYPECONVERTER_T8306AE03734853B551DDF089C1F17836A7764DBB_H
#define TYPECONVERTER_T8306AE03734853B551DDF089C1F17836A7764DBB_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.ComponentModel.TypeConverter
struct  TypeConverter_t8306AE03734853B551DDF089C1F17836A7764DBB  : public RuntimeObject
{
public:

public:
};

struct TypeConverter_t8306AE03734853B551DDF089C1F17836A7764DBB_StaticFields
{
public:
	// System.Boolean modreq(System.Runtime.CompilerServices.IsVolatile) System.ComponentModel.TypeConverter::useCompatibleTypeConversion
	bool ___useCompatibleTypeConversion_1;

public:
	inline static int32_t get_offset_of_useCompatibleTypeConversion_1() { return static_cast<int32_t>(offsetof(TypeConverter_t8306AE03734853B551DDF089C1F17836A7764DBB_StaticFields, ___useCompatibleTypeConversion_1)); }
	inline bool get_useCompatibleTypeConversion_1() const { return ___useCompatibleTypeConversion_1; }
	inline bool* get_address_of_useCompatibleTypeConversion_1() { return &___useCompatibleTypeConversion_1; }
	inline void set_useCompatibleTypeConversion_1(bool value)
	{
		___useCompatibleTypeConversion_1 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // TYPECONVERTER_T8306AE03734853B551DDF089C1F17836A7764DBB_H
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
#ifndef NOTSUPPORTEDEXCEPTION_TE75B318D6590A02A5D9B29FD97409B1750FA0010_H
#define NOTSUPPORTEDEXCEPTION_TE75B318D6590A02A5D9B29FD97409B1750FA0010_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.NotSupportedException
struct  NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010  : public SystemException_t5380468142AA850BE4A341D7AF3EAB9C78746782
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // NOTSUPPORTEDEXCEPTION_TE75B318D6590A02A5D9B29FD97409B1750FA0010_H
#ifndef OUTOFMEMORYEXCEPTION_T2DF3EAC178583BD1DEFAAECBEDB2AF1EA86FBFC7_H
#define OUTOFMEMORYEXCEPTION_T2DF3EAC178583BD1DEFAAECBEDB2AF1EA86FBFC7_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.OutOfMemoryException
struct  OutOfMemoryException_t2DF3EAC178583BD1DEFAAECBEDB2AF1EA86FBFC7  : public SystemException_t5380468142AA850BE4A341D7AF3EAB9C78746782
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // OUTOFMEMORYEXCEPTION_T2DF3EAC178583BD1DEFAAECBEDB2AF1EA86FBFC7_H
#ifndef BINDINGFLAGS_TE35C91D046E63A1B92BB9AB909FCF9DA84379ED0_H
#define BINDINGFLAGS_TE35C91D046E63A1B92BB9AB909FCF9DA84379ED0_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Reflection.BindingFlags
struct  BindingFlags_tE35C91D046E63A1B92BB9AB909FCF9DA84379ED0 
{
public:
	// System.Int32 System.Reflection.BindingFlags::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(BindingFlags_tE35C91D046E63A1B92BB9AB909FCF9DA84379ED0, ___value___2)); }
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
#endif // BINDINGFLAGS_TE35C91D046E63A1B92BB9AB909FCF9DA84379ED0_H
#ifndef STREAMINGCONTEXTSTATES_T6D16CD7BC584A66A29B702F5FD59DF62BB1BDD3F_H
#define STREAMINGCONTEXTSTATES_T6D16CD7BC584A66A29B702F5FD59DF62BB1BDD3F_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Runtime.Serialization.StreamingContextStates
struct  StreamingContextStates_t6D16CD7BC584A66A29B702F5FD59DF62BB1BDD3F 
{
public:
	// System.Int32 System.Runtime.Serialization.StreamingContextStates::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(StreamingContextStates_t6D16CD7BC584A66A29B702F5FD59DF62BB1BDD3F, ___value___2)); }
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
#endif // STREAMINGCONTEXTSTATES_T6D16CD7BC584A66A29B702F5FD59DF62BB1BDD3F_H
#ifndef RUNTIMEFIELDHANDLE_T844BDF00E8E6FE69D9AEAA7657F09018B864F4EF_H
#define RUNTIMEFIELDHANDLE_T844BDF00E8E6FE69D9AEAA7657F09018B864F4EF_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.RuntimeFieldHandle
struct  RuntimeFieldHandle_t844BDF00E8E6FE69D9AEAA7657F09018B864F4EF 
{
public:
	// System.IntPtr System.RuntimeFieldHandle::value
	intptr_t ___value_0;

public:
	inline static int32_t get_offset_of_value_0() { return static_cast<int32_t>(offsetof(RuntimeFieldHandle_t844BDF00E8E6FE69D9AEAA7657F09018B864F4EF, ___value_0)); }
	inline intptr_t get_value_0() const { return ___value_0; }
	inline intptr_t* get_address_of_value_0() { return &___value_0; }
	inline void set_value_0(intptr_t value)
	{
		___value_0 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // RUNTIMEFIELDHANDLE_T844BDF00E8E6FE69D9AEAA7657F09018B864F4EF_H
#ifndef RUNTIMETYPEHANDLE_T7B542280A22F0EC4EAC2061C29178845847A8B2D_H
#define RUNTIMETYPEHANDLE_T7B542280A22F0EC4EAC2061C29178845847A8B2D_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.RuntimeTypeHandle
struct  RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D 
{
public:
	// System.IntPtr System.RuntimeTypeHandle::value
	intptr_t ___value_0;

public:
	inline static int32_t get_offset_of_value_0() { return static_cast<int32_t>(offsetof(RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D, ___value_0)); }
	inline intptr_t get_value_0() const { return ___value_0; }
	inline intptr_t* get_address_of_value_0() { return &___value_0; }
	inline void set_value_0(intptr_t value)
	{
		___value_0 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // RUNTIMETYPEHANDLE_T7B542280A22F0EC4EAC2061C29178845847A8B2D_H
#ifndef UNESCAPEMODE_T22E9EF2FB775920C1538E221765EE5B0D91E7470_H
#define UNESCAPEMODE_T22E9EF2FB775920C1538E221765EE5B0D91E7470_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.UnescapeMode
struct  UnescapeMode_t22E9EF2FB775920C1538E221765EE5B0D91E7470 
{
public:
	// System.Int32 System.UnescapeMode::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(UnescapeMode_t22E9EF2FB775920C1538E221765EE5B0D91E7470, ___value___2)); }
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
#endif // UNESCAPEMODE_T22E9EF2FB775920C1538E221765EE5B0D91E7470_H
#ifndef CHECK_T597B1C13F5DD4DAAA857F961852721AE4DD0BD5E_H
#define CHECK_T597B1C13F5DD4DAAA857F961852721AE4DD0BD5E_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Uri_Check
struct  Check_t597B1C13F5DD4DAAA857F961852721AE4DD0BD5E 
{
public:
	// System.Int32 System.Uri_Check::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(Check_t597B1C13F5DD4DAAA857F961852721AE4DD0BD5E, ___value___2)); }
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
#endif // CHECK_T597B1C13F5DD4DAAA857F961852721AE4DD0BD5E_H
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
#ifndef URIINFO_T9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E_H
#define URIINFO_T9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Uri_UriInfo
struct  UriInfo_t9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E  : public RuntimeObject
{
public:
	// System.String System.Uri_UriInfo::Host
	String_t* ___Host_0;
	// System.String System.Uri_UriInfo::ScopeId
	String_t* ___ScopeId_1;
	// System.String System.Uri_UriInfo::String
	String_t* ___String_2;
	// System.Uri_Offset System.Uri_UriInfo::Offset
	Offset_t4D3750A78885B564FB4602C405B9EFF5A32066C7  ___Offset_3;
	// System.String System.Uri_UriInfo::DnsSafeHost
	String_t* ___DnsSafeHost_4;
	// System.Uri_MoreInfo System.Uri_UriInfo::MoreInfo
	MoreInfo_t83B9EC79244C26B468C115E54C0BEF09BB2E05B5 * ___MoreInfo_5;

public:
	inline static int32_t get_offset_of_Host_0() { return static_cast<int32_t>(offsetof(UriInfo_t9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E, ___Host_0)); }
	inline String_t* get_Host_0() const { return ___Host_0; }
	inline String_t** get_address_of_Host_0() { return &___Host_0; }
	inline void set_Host_0(String_t* value)
	{
		___Host_0 = value;
		Il2CppCodeGenWriteBarrier((&___Host_0), value);
	}

	inline static int32_t get_offset_of_ScopeId_1() { return static_cast<int32_t>(offsetof(UriInfo_t9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E, ___ScopeId_1)); }
	inline String_t* get_ScopeId_1() const { return ___ScopeId_1; }
	inline String_t** get_address_of_ScopeId_1() { return &___ScopeId_1; }
	inline void set_ScopeId_1(String_t* value)
	{
		___ScopeId_1 = value;
		Il2CppCodeGenWriteBarrier((&___ScopeId_1), value);
	}

	inline static int32_t get_offset_of_String_2() { return static_cast<int32_t>(offsetof(UriInfo_t9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E, ___String_2)); }
	inline String_t* get_String_2() const { return ___String_2; }
	inline String_t** get_address_of_String_2() { return &___String_2; }
	inline void set_String_2(String_t* value)
	{
		___String_2 = value;
		Il2CppCodeGenWriteBarrier((&___String_2), value);
	}

	inline static int32_t get_offset_of_Offset_3() { return static_cast<int32_t>(offsetof(UriInfo_t9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E, ___Offset_3)); }
	inline Offset_t4D3750A78885B564FB4602C405B9EFF5A32066C7  get_Offset_3() const { return ___Offset_3; }
	inline Offset_t4D3750A78885B564FB4602C405B9EFF5A32066C7 * get_address_of_Offset_3() { return &___Offset_3; }
	inline void set_Offset_3(Offset_t4D3750A78885B564FB4602C405B9EFF5A32066C7  value)
	{
		___Offset_3 = value;
	}

	inline static int32_t get_offset_of_DnsSafeHost_4() { return static_cast<int32_t>(offsetof(UriInfo_t9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E, ___DnsSafeHost_4)); }
	inline String_t* get_DnsSafeHost_4() const { return ___DnsSafeHost_4; }
	inline String_t** get_address_of_DnsSafeHost_4() { return &___DnsSafeHost_4; }
	inline void set_DnsSafeHost_4(String_t* value)
	{
		___DnsSafeHost_4 = value;
		Il2CppCodeGenWriteBarrier((&___DnsSafeHost_4), value);
	}

	inline static int32_t get_offset_of_MoreInfo_5() { return static_cast<int32_t>(offsetof(UriInfo_t9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E, ___MoreInfo_5)); }
	inline MoreInfo_t83B9EC79244C26B468C115E54C0BEF09BB2E05B5 * get_MoreInfo_5() const { return ___MoreInfo_5; }
	inline MoreInfo_t83B9EC79244C26B468C115E54C0BEF09BB2E05B5 ** get_address_of_MoreInfo_5() { return &___MoreInfo_5; }
	inline void set_MoreInfo_5(MoreInfo_t83B9EC79244C26B468C115E54C0BEF09BB2E05B5 * value)
	{
		___MoreInfo_5 = value;
		Il2CppCodeGenWriteBarrier((&___MoreInfo_5), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // URIINFO_T9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E_H
#ifndef URICOMPONENTS_TE42D5229291668DE73323E1C519E4E1459A64CFF_H
#define URICOMPONENTS_TE42D5229291668DE73323E1C519E4E1459A64CFF_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.UriComponents
struct  UriComponents_tE42D5229291668DE73323E1C519E4E1459A64CFF 
{
public:
	// System.Int32 System.UriComponents::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(UriComponents_tE42D5229291668DE73323E1C519E4E1459A64CFF, ___value___2)); }
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
#endif // URICOMPONENTS_TE42D5229291668DE73323E1C519E4E1459A64CFF_H
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
#ifndef URIHOSTNAMETYPE_T878C7F8270044471359846197DD0FB290E566858_H
#define URIHOSTNAMETYPE_T878C7F8270044471359846197DD0FB290E566858_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.UriHostNameType
struct  UriHostNameType_t878C7F8270044471359846197DD0FB290E566858 
{
public:
	// System.Int32 System.UriHostNameType::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(UriHostNameType_t878C7F8270044471359846197DD0FB290E566858, ___value___2)); }
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
#endif // URIHOSTNAMETYPE_T878C7F8270044471359846197DD0FB290E566858_H
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
#ifndef URIQUIRKSVERSION_TB044080854D030F26EB17D99FFE997D0FFB8A374_H
#define URIQUIRKSVERSION_TB044080854D030F26EB17D99FFE997D0FFB8A374_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.UriParser_UriQuirksVersion
struct  UriQuirksVersion_tB044080854D030F26EB17D99FFE997D0FFB8A374 
{
public:
	// System.Int32 System.UriParser_UriQuirksVersion::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(UriQuirksVersion_tB044080854D030F26EB17D99FFE997D0FFB8A374, ___value___2)); }
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
#endif // URIQUIRKSVERSION_TB044080854D030F26EB17D99FFE997D0FFB8A374_H
#ifndef URISYNTAXFLAGS_T8773DD32DE8871701F05FBED115A2B51679D5D46_H
#define URISYNTAXFLAGS_T8773DD32DE8871701F05FBED115A2B51679D5D46_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.UriSyntaxFlags
struct  UriSyntaxFlags_t8773DD32DE8871701F05FBED115A2B51679D5D46 
{
public:
	// System.Int32 System.UriSyntaxFlags::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(UriSyntaxFlags_t8773DD32DE8871701F05FBED115A2B51679D5D46, ___value___2)); }
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
#endif // URISYNTAXFLAGS_T8773DD32DE8871701F05FBED115A2B51679D5D46_H
#ifndef ARGUMENTNULLEXCEPTION_T581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD_H
#define ARGUMENTNULLEXCEPTION_T581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.ArgumentNullException
struct  ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD  : public ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // ARGUMENTNULLEXCEPTION_T581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD_H
#ifndef ARGUMENTOUTOFRANGEEXCEPTION_T94D19DF918A54511AEDF4784C9A08741BAD1DEDA_H
#define ARGUMENTOUTOFRANGEEXCEPTION_T94D19DF918A54511AEDF4784C9A08741BAD1DEDA_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.ArgumentOutOfRangeException
struct  ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA  : public ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1
{
public:
	// System.Object System.ArgumentOutOfRangeException::m_actualValue
	RuntimeObject * ___m_actualValue_19;

public:
	inline static int32_t get_offset_of_m_actualValue_19() { return static_cast<int32_t>(offsetof(ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA, ___m_actualValue_19)); }
	inline RuntimeObject * get_m_actualValue_19() const { return ___m_actualValue_19; }
	inline RuntimeObject ** get_address_of_m_actualValue_19() { return &___m_actualValue_19; }
	inline void set_m_actualValue_19(RuntimeObject * value)
	{
		___m_actualValue_19 = value;
		Il2CppCodeGenWriteBarrier((&___m_actualValue_19), value);
	}
};

struct ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA_StaticFields
{
public:
	// System.String modreq(System.Runtime.CompilerServices.IsVolatile) System.ArgumentOutOfRangeException::_rangeMessage
	String_t* ____rangeMessage_18;

public:
	inline static int32_t get_offset_of__rangeMessage_18() { return static_cast<int32_t>(offsetof(ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA_StaticFields, ____rangeMessage_18)); }
	inline String_t* get__rangeMessage_18() const { return ____rangeMessage_18; }
	inline String_t** get_address_of__rangeMessage_18() { return &____rangeMessage_18; }
	inline void set__rangeMessage_18(String_t* value)
	{
		____rangeMessage_18 = value;
		Il2CppCodeGenWriteBarrier((&____rangeMessage_18), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // ARGUMENTOUTOFRANGEEXCEPTION_T94D19DF918A54511AEDF4784C9A08741BAD1DEDA_H
#ifndef OBJECTDISPOSEDEXCEPTION_TF68E471ECD1419AD7C51137B742837395F50B69A_H
#define OBJECTDISPOSEDEXCEPTION_TF68E471ECD1419AD7C51137B742837395F50B69A_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.ObjectDisposedException
struct  ObjectDisposedException_tF68E471ECD1419AD7C51137B742837395F50B69A  : public InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1
{
public:
	// System.String System.ObjectDisposedException::objectName
	String_t* ___objectName_17;

public:
	inline static int32_t get_offset_of_objectName_17() { return static_cast<int32_t>(offsetof(ObjectDisposedException_tF68E471ECD1419AD7C51137B742837395F50B69A, ___objectName_17)); }
	inline String_t* get_objectName_17() const { return ___objectName_17; }
	inline String_t** get_address_of_objectName_17() { return &___objectName_17; }
	inline void set_objectName_17(String_t* value)
	{
		___objectName_17 = value;
		Il2CppCodeGenWriteBarrier((&___objectName_17), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // OBJECTDISPOSEDEXCEPTION_TF68E471ECD1419AD7C51137B742837395F50B69A_H
#ifndef PLATFORMNOTSUPPORTEDEXCEPTION_T14FE109377F8FA8B3B2F9A0C4FE3BF10662C73B5_H
#define PLATFORMNOTSUPPORTEDEXCEPTION_T14FE109377F8FA8B3B2F9A0C4FE3BF10662C73B5_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.PlatformNotSupportedException
struct  PlatformNotSupportedException_t14FE109377F8FA8B3B2F9A0C4FE3BF10662C73B5  : public NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // PLATFORMNOTSUPPORTEDEXCEPTION_T14FE109377F8FA8B3B2F9A0C4FE3BF10662C73B5_H
#ifndef STREAMINGCONTEXT_T2CCDC54E0E8D078AF4A50E3A8B921B828A900034_H
#define STREAMINGCONTEXT_T2CCDC54E0E8D078AF4A50E3A8B921B828A900034_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Runtime.Serialization.StreamingContext
struct  StreamingContext_t2CCDC54E0E8D078AF4A50E3A8B921B828A900034 
{
public:
	// System.Object System.Runtime.Serialization.StreamingContext::m_additionalContext
	RuntimeObject * ___m_additionalContext_0;
	// System.Runtime.Serialization.StreamingContextStates System.Runtime.Serialization.StreamingContext::m_state
	int32_t ___m_state_1;

public:
	inline static int32_t get_offset_of_m_additionalContext_0() { return static_cast<int32_t>(offsetof(StreamingContext_t2CCDC54E0E8D078AF4A50E3A8B921B828A900034, ___m_additionalContext_0)); }
	inline RuntimeObject * get_m_additionalContext_0() const { return ___m_additionalContext_0; }
	inline RuntimeObject ** get_address_of_m_additionalContext_0() { return &___m_additionalContext_0; }
	inline void set_m_additionalContext_0(RuntimeObject * value)
	{
		___m_additionalContext_0 = value;
		Il2CppCodeGenWriteBarrier((&___m_additionalContext_0), value);
	}

	inline static int32_t get_offset_of_m_state_1() { return static_cast<int32_t>(offsetof(StreamingContext_t2CCDC54E0E8D078AF4A50E3A8B921B828A900034, ___m_state_1)); }
	inline int32_t get_m_state_1() const { return ___m_state_1; }
	inline int32_t* get_address_of_m_state_1() { return &___m_state_1; }
	inline void set_m_state_1(int32_t value)
	{
		___m_state_1 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of System.Runtime.Serialization.StreamingContext
struct StreamingContext_t2CCDC54E0E8D078AF4A50E3A8B921B828A900034_marshaled_pinvoke
{
	Il2CppIUnknown* ___m_additionalContext_0;
	int32_t ___m_state_1;
};
// Native definition for COM marshalling of System.Runtime.Serialization.StreamingContext
struct StreamingContext_t2CCDC54E0E8D078AF4A50E3A8B921B828A900034_marshaled_com
{
	Il2CppIUnknown* ___m_additionalContext_0;
	int32_t ___m_state_1;
};
#endif // STREAMINGCONTEXT_T2CCDC54E0E8D078AF4A50E3A8B921B828A900034_H
#ifndef TYPE_T_H
#define TYPE_T_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Type
struct  Type_t  : public MemberInfo_t
{
public:
	// System.RuntimeTypeHandle System.Type::_impl
	RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D  ____impl_9;

public:
	inline static int32_t get_offset_of__impl_9() { return static_cast<int32_t>(offsetof(Type_t, ____impl_9)); }
	inline RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D  get__impl_9() const { return ____impl_9; }
	inline RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D * get_address_of__impl_9() { return &____impl_9; }
	inline void set__impl_9(RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D  value)
	{
		____impl_9 = value;
	}
};

struct Type_t_StaticFields
{
public:
	// System.Reflection.MemberFilter System.Type::FilterAttribute
	MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * ___FilterAttribute_0;
	// System.Reflection.MemberFilter System.Type::FilterName
	MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * ___FilterName_1;
	// System.Reflection.MemberFilter System.Type::FilterNameIgnoreCase
	MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * ___FilterNameIgnoreCase_2;
	// System.Object System.Type::Missing
	RuntimeObject * ___Missing_3;
	// System.Char System.Type::Delimiter
	Il2CppChar ___Delimiter_4;
	// System.Type[] System.Type::EmptyTypes
	TypeU5BU5D_t7FE623A666B49176DE123306221193E888A12F5F* ___EmptyTypes_5;
	// System.Reflection.Binder System.Type::defaultBinder
	Binder_t4D5CB06963501D32847C057B57157D6DC49CA759 * ___defaultBinder_6;

public:
	inline static int32_t get_offset_of_FilterAttribute_0() { return static_cast<int32_t>(offsetof(Type_t_StaticFields, ___FilterAttribute_0)); }
	inline MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * get_FilterAttribute_0() const { return ___FilterAttribute_0; }
	inline MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 ** get_address_of_FilterAttribute_0() { return &___FilterAttribute_0; }
	inline void set_FilterAttribute_0(MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * value)
	{
		___FilterAttribute_0 = value;
		Il2CppCodeGenWriteBarrier((&___FilterAttribute_0), value);
	}

	inline static int32_t get_offset_of_FilterName_1() { return static_cast<int32_t>(offsetof(Type_t_StaticFields, ___FilterName_1)); }
	inline MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * get_FilterName_1() const { return ___FilterName_1; }
	inline MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 ** get_address_of_FilterName_1() { return &___FilterName_1; }
	inline void set_FilterName_1(MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * value)
	{
		___FilterName_1 = value;
		Il2CppCodeGenWriteBarrier((&___FilterName_1), value);
	}

	inline static int32_t get_offset_of_FilterNameIgnoreCase_2() { return static_cast<int32_t>(offsetof(Type_t_StaticFields, ___FilterNameIgnoreCase_2)); }
	inline MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * get_FilterNameIgnoreCase_2() const { return ___FilterNameIgnoreCase_2; }
	inline MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 ** get_address_of_FilterNameIgnoreCase_2() { return &___FilterNameIgnoreCase_2; }
	inline void set_FilterNameIgnoreCase_2(MemberFilter_t25C1BD92C42BE94426E300787C13C452CB89B381 * value)
	{
		___FilterNameIgnoreCase_2 = value;
		Il2CppCodeGenWriteBarrier((&___FilterNameIgnoreCase_2), value);
	}

	inline static int32_t get_offset_of_Missing_3() { return static_cast<int32_t>(offsetof(Type_t_StaticFields, ___Missing_3)); }
	inline RuntimeObject * get_Missing_3() const { return ___Missing_3; }
	inline RuntimeObject ** get_address_of_Missing_3() { return &___Missing_3; }
	inline void set_Missing_3(RuntimeObject * value)
	{
		___Missing_3 = value;
		Il2CppCodeGenWriteBarrier((&___Missing_3), value);
	}

	inline static int32_t get_offset_of_Delimiter_4() { return static_cast<int32_t>(offsetof(Type_t_StaticFields, ___Delimiter_4)); }
	inline Il2CppChar get_Delimiter_4() const { return ___Delimiter_4; }
	inline Il2CppChar* get_address_of_Delimiter_4() { return &___Delimiter_4; }
	inline void set_Delimiter_4(Il2CppChar value)
	{
		___Delimiter_4 = value;
	}

	inline static int32_t get_offset_of_EmptyTypes_5() { return static_cast<int32_t>(offsetof(Type_t_StaticFields, ___EmptyTypes_5)); }
	inline TypeU5BU5D_t7FE623A666B49176DE123306221193E888A12F5F* get_EmptyTypes_5() const { return ___EmptyTypes_5; }
	inline TypeU5BU5D_t7FE623A666B49176DE123306221193E888A12F5F** get_address_of_EmptyTypes_5() { return &___EmptyTypes_5; }
	inline void set_EmptyTypes_5(TypeU5BU5D_t7FE623A666B49176DE123306221193E888A12F5F* value)
	{
		___EmptyTypes_5 = value;
		Il2CppCodeGenWriteBarrier((&___EmptyTypes_5), value);
	}

	inline static int32_t get_offset_of_defaultBinder_6() { return static_cast<int32_t>(offsetof(Type_t_StaticFields, ___defaultBinder_6)); }
	inline Binder_t4D5CB06963501D32847C057B57157D6DC49CA759 * get_defaultBinder_6() const { return ___defaultBinder_6; }
	inline Binder_t4D5CB06963501D32847C057B57157D6DC49CA759 ** get_address_of_defaultBinder_6() { return &___defaultBinder_6; }
	inline void set_defaultBinder_6(Binder_t4D5CB06963501D32847C057B57157D6DC49CA759 * value)
	{
		___defaultBinder_6 = value;
		Il2CppCodeGenWriteBarrier((&___defaultBinder_6), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // TYPE_T_H
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
#ifndef URIFORMATEXCEPTION_T86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A_H
#define URIFORMATEXCEPTION_T86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.UriFormatException
struct  UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A  : public FormatException_t2808E076CDE4650AF89F55FD78F49290D0EC5BDC
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // URIFORMATEXCEPTION_T86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A_H
#ifndef URIPARSER_T07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_H
#define URIPARSER_T07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.UriParser
struct  UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC  : public RuntimeObject
{
public:
	// System.UriSyntaxFlags System.UriParser::m_Flags
	int32_t ___m_Flags_2;
	// System.UriSyntaxFlags modreq(System.Runtime.CompilerServices.IsVolatile) System.UriParser::m_UpdatableFlags
	int32_t ___m_UpdatableFlags_3;
	// System.Boolean modreq(System.Runtime.CompilerServices.IsVolatile) System.UriParser::m_UpdatableFlagsUsed
	bool ___m_UpdatableFlagsUsed_4;
	// System.Int32 System.UriParser::m_Port
	int32_t ___m_Port_5;
	// System.String System.UriParser::m_Scheme
	String_t* ___m_Scheme_6;

public:
	inline static int32_t get_offset_of_m_Flags_2() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC, ___m_Flags_2)); }
	inline int32_t get_m_Flags_2() const { return ___m_Flags_2; }
	inline int32_t* get_address_of_m_Flags_2() { return &___m_Flags_2; }
	inline void set_m_Flags_2(int32_t value)
	{
		___m_Flags_2 = value;
	}

	inline static int32_t get_offset_of_m_UpdatableFlags_3() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC, ___m_UpdatableFlags_3)); }
	inline int32_t get_m_UpdatableFlags_3() const { return ___m_UpdatableFlags_3; }
	inline int32_t* get_address_of_m_UpdatableFlags_3() { return &___m_UpdatableFlags_3; }
	inline void set_m_UpdatableFlags_3(int32_t value)
	{
		___m_UpdatableFlags_3 = value;
	}

	inline static int32_t get_offset_of_m_UpdatableFlagsUsed_4() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC, ___m_UpdatableFlagsUsed_4)); }
	inline bool get_m_UpdatableFlagsUsed_4() const { return ___m_UpdatableFlagsUsed_4; }
	inline bool* get_address_of_m_UpdatableFlagsUsed_4() { return &___m_UpdatableFlagsUsed_4; }
	inline void set_m_UpdatableFlagsUsed_4(bool value)
	{
		___m_UpdatableFlagsUsed_4 = value;
	}

	inline static int32_t get_offset_of_m_Port_5() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC, ___m_Port_5)); }
	inline int32_t get_m_Port_5() const { return ___m_Port_5; }
	inline int32_t* get_address_of_m_Port_5() { return &___m_Port_5; }
	inline void set_m_Port_5(int32_t value)
	{
		___m_Port_5 = value;
	}

	inline static int32_t get_offset_of_m_Scheme_6() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC, ___m_Scheme_6)); }
	inline String_t* get_m_Scheme_6() const { return ___m_Scheme_6; }
	inline String_t** get_address_of_m_Scheme_6() { return &___m_Scheme_6; }
	inline void set_m_Scheme_6(String_t* value)
	{
		___m_Scheme_6 = value;
		Il2CppCodeGenWriteBarrier((&___m_Scheme_6), value);
	}
};

struct UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields
{
public:
	// System.Collections.Generic.Dictionary`2<System.String,System.UriParser> System.UriParser::m_Table
	Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * ___m_Table_0;
	// System.Collections.Generic.Dictionary`2<System.String,System.UriParser> System.UriParser::m_TempTable
	Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * ___m_TempTable_1;
	// System.UriParser System.UriParser::HttpUri
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___HttpUri_7;
	// System.UriParser System.UriParser::HttpsUri
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___HttpsUri_8;
	// System.UriParser System.UriParser::WsUri
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___WsUri_9;
	// System.UriParser System.UriParser::WssUri
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___WssUri_10;
	// System.UriParser System.UriParser::FtpUri
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___FtpUri_11;
	// System.UriParser System.UriParser::FileUri
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___FileUri_12;
	// System.UriParser System.UriParser::GopherUri
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___GopherUri_13;
	// System.UriParser System.UriParser::NntpUri
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___NntpUri_14;
	// System.UriParser System.UriParser::NewsUri
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___NewsUri_15;
	// System.UriParser System.UriParser::MailToUri
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___MailToUri_16;
	// System.UriParser System.UriParser::UuidUri
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___UuidUri_17;
	// System.UriParser System.UriParser::TelnetUri
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___TelnetUri_18;
	// System.UriParser System.UriParser::LdapUri
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___LdapUri_19;
	// System.UriParser System.UriParser::NetTcpUri
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___NetTcpUri_20;
	// System.UriParser System.UriParser::NetPipeUri
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___NetPipeUri_21;
	// System.UriParser System.UriParser::VsMacrosUri
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___VsMacrosUri_22;
	// System.UriParser_UriQuirksVersion System.UriParser::s_QuirksVersion
	int32_t ___s_QuirksVersion_23;
	// System.UriSyntaxFlags System.UriParser::HttpSyntaxFlags
	int32_t ___HttpSyntaxFlags_24;
	// System.UriSyntaxFlags System.UriParser::FileSyntaxFlags
	int32_t ___FileSyntaxFlags_25;

public:
	inline static int32_t get_offset_of_m_Table_0() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___m_Table_0)); }
	inline Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * get_m_Table_0() const { return ___m_Table_0; }
	inline Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE ** get_address_of_m_Table_0() { return &___m_Table_0; }
	inline void set_m_Table_0(Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * value)
	{
		___m_Table_0 = value;
		Il2CppCodeGenWriteBarrier((&___m_Table_0), value);
	}

	inline static int32_t get_offset_of_m_TempTable_1() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___m_TempTable_1)); }
	inline Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * get_m_TempTable_1() const { return ___m_TempTable_1; }
	inline Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE ** get_address_of_m_TempTable_1() { return &___m_TempTable_1; }
	inline void set_m_TempTable_1(Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * value)
	{
		___m_TempTable_1 = value;
		Il2CppCodeGenWriteBarrier((&___m_TempTable_1), value);
	}

	inline static int32_t get_offset_of_HttpUri_7() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___HttpUri_7)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_HttpUri_7() const { return ___HttpUri_7; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_HttpUri_7() { return &___HttpUri_7; }
	inline void set_HttpUri_7(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___HttpUri_7 = value;
		Il2CppCodeGenWriteBarrier((&___HttpUri_7), value);
	}

	inline static int32_t get_offset_of_HttpsUri_8() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___HttpsUri_8)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_HttpsUri_8() const { return ___HttpsUri_8; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_HttpsUri_8() { return &___HttpsUri_8; }
	inline void set_HttpsUri_8(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___HttpsUri_8 = value;
		Il2CppCodeGenWriteBarrier((&___HttpsUri_8), value);
	}

	inline static int32_t get_offset_of_WsUri_9() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___WsUri_9)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_WsUri_9() const { return ___WsUri_9; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_WsUri_9() { return &___WsUri_9; }
	inline void set_WsUri_9(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___WsUri_9 = value;
		Il2CppCodeGenWriteBarrier((&___WsUri_9), value);
	}

	inline static int32_t get_offset_of_WssUri_10() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___WssUri_10)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_WssUri_10() const { return ___WssUri_10; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_WssUri_10() { return &___WssUri_10; }
	inline void set_WssUri_10(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___WssUri_10 = value;
		Il2CppCodeGenWriteBarrier((&___WssUri_10), value);
	}

	inline static int32_t get_offset_of_FtpUri_11() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___FtpUri_11)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_FtpUri_11() const { return ___FtpUri_11; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_FtpUri_11() { return &___FtpUri_11; }
	inline void set_FtpUri_11(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___FtpUri_11 = value;
		Il2CppCodeGenWriteBarrier((&___FtpUri_11), value);
	}

	inline static int32_t get_offset_of_FileUri_12() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___FileUri_12)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_FileUri_12() const { return ___FileUri_12; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_FileUri_12() { return &___FileUri_12; }
	inline void set_FileUri_12(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___FileUri_12 = value;
		Il2CppCodeGenWriteBarrier((&___FileUri_12), value);
	}

	inline static int32_t get_offset_of_GopherUri_13() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___GopherUri_13)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_GopherUri_13() const { return ___GopherUri_13; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_GopherUri_13() { return &___GopherUri_13; }
	inline void set_GopherUri_13(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___GopherUri_13 = value;
		Il2CppCodeGenWriteBarrier((&___GopherUri_13), value);
	}

	inline static int32_t get_offset_of_NntpUri_14() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___NntpUri_14)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_NntpUri_14() const { return ___NntpUri_14; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_NntpUri_14() { return &___NntpUri_14; }
	inline void set_NntpUri_14(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___NntpUri_14 = value;
		Il2CppCodeGenWriteBarrier((&___NntpUri_14), value);
	}

	inline static int32_t get_offset_of_NewsUri_15() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___NewsUri_15)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_NewsUri_15() const { return ___NewsUri_15; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_NewsUri_15() { return &___NewsUri_15; }
	inline void set_NewsUri_15(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___NewsUri_15 = value;
		Il2CppCodeGenWriteBarrier((&___NewsUri_15), value);
	}

	inline static int32_t get_offset_of_MailToUri_16() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___MailToUri_16)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_MailToUri_16() const { return ___MailToUri_16; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_MailToUri_16() { return &___MailToUri_16; }
	inline void set_MailToUri_16(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___MailToUri_16 = value;
		Il2CppCodeGenWriteBarrier((&___MailToUri_16), value);
	}

	inline static int32_t get_offset_of_UuidUri_17() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___UuidUri_17)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_UuidUri_17() const { return ___UuidUri_17; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_UuidUri_17() { return &___UuidUri_17; }
	inline void set_UuidUri_17(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___UuidUri_17 = value;
		Il2CppCodeGenWriteBarrier((&___UuidUri_17), value);
	}

	inline static int32_t get_offset_of_TelnetUri_18() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___TelnetUri_18)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_TelnetUri_18() const { return ___TelnetUri_18; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_TelnetUri_18() { return &___TelnetUri_18; }
	inline void set_TelnetUri_18(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___TelnetUri_18 = value;
		Il2CppCodeGenWriteBarrier((&___TelnetUri_18), value);
	}

	inline static int32_t get_offset_of_LdapUri_19() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___LdapUri_19)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_LdapUri_19() const { return ___LdapUri_19; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_LdapUri_19() { return &___LdapUri_19; }
	inline void set_LdapUri_19(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___LdapUri_19 = value;
		Il2CppCodeGenWriteBarrier((&___LdapUri_19), value);
	}

	inline static int32_t get_offset_of_NetTcpUri_20() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___NetTcpUri_20)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_NetTcpUri_20() const { return ___NetTcpUri_20; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_NetTcpUri_20() { return &___NetTcpUri_20; }
	inline void set_NetTcpUri_20(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___NetTcpUri_20 = value;
		Il2CppCodeGenWriteBarrier((&___NetTcpUri_20), value);
	}

	inline static int32_t get_offset_of_NetPipeUri_21() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___NetPipeUri_21)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_NetPipeUri_21() const { return ___NetPipeUri_21; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_NetPipeUri_21() { return &___NetPipeUri_21; }
	inline void set_NetPipeUri_21(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___NetPipeUri_21 = value;
		Il2CppCodeGenWriteBarrier((&___NetPipeUri_21), value);
	}

	inline static int32_t get_offset_of_VsMacrosUri_22() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___VsMacrosUri_22)); }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * get_VsMacrosUri_22() const { return ___VsMacrosUri_22; }
	inline UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** get_address_of_VsMacrosUri_22() { return &___VsMacrosUri_22; }
	inline void set_VsMacrosUri_22(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * value)
	{
		___VsMacrosUri_22 = value;
		Il2CppCodeGenWriteBarrier((&___VsMacrosUri_22), value);
	}

	inline static int32_t get_offset_of_s_QuirksVersion_23() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___s_QuirksVersion_23)); }
	inline int32_t get_s_QuirksVersion_23() const { return ___s_QuirksVersion_23; }
	inline int32_t* get_address_of_s_QuirksVersion_23() { return &___s_QuirksVersion_23; }
	inline void set_s_QuirksVersion_23(int32_t value)
	{
		___s_QuirksVersion_23 = value;
	}

	inline static int32_t get_offset_of_HttpSyntaxFlags_24() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___HttpSyntaxFlags_24)); }
	inline int32_t get_HttpSyntaxFlags_24() const { return ___HttpSyntaxFlags_24; }
	inline int32_t* get_address_of_HttpSyntaxFlags_24() { return &___HttpSyntaxFlags_24; }
	inline void set_HttpSyntaxFlags_24(int32_t value)
	{
		___HttpSyntaxFlags_24 = value;
	}

	inline static int32_t get_offset_of_FileSyntaxFlags_25() { return static_cast<int32_t>(offsetof(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields, ___FileSyntaxFlags_25)); }
	inline int32_t get_FileSyntaxFlags_25() const { return ___FileSyntaxFlags_25; }
	inline int32_t* get_address_of_FileSyntaxFlags_25() { return &___FileSyntaxFlags_25; }
	inline void set_FileSyntaxFlags_25(int32_t value)
	{
		___FileSyntaxFlags_25 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // URIPARSER_T07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_H
#ifndef URITYPECONVERTER_T96793526764A246FBAEE2F4F639AFAF270EE81D1_H
#define URITYPECONVERTER_T96793526764A246FBAEE2F4F639AFAF270EE81D1_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.UriTypeConverter
struct  UriTypeConverter_t96793526764A246FBAEE2F4F639AFAF270EE81D1  : public TypeConverter_t8306AE03734853B551DDF089C1F17836A7764DBB
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // URITYPECONVERTER_T96793526764A246FBAEE2F4F639AFAF270EE81D1_H
#ifndef BUILTINURIPARSER_T5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_H
#define BUILTINURIPARSER_T5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.UriParser_BuiltInUriParser
struct  BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B  : public UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // BUILTINURIPARSER_T5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_H
#ifndef THROWSTUB_T03526C535287FADF58CBFA05084AE89A0ACFFEFA_H
#define THROWSTUB_T03526C535287FADF58CBFA05084AE89A0ACFFEFA_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// Unity.ThrowStub
struct  ThrowStub_t03526C535287FADF58CBFA05084AE89A0ACFFEFA  : public ObjectDisposedException_tF68E471ECD1419AD7C51137B742837395F50B69A
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // THROWSTUB_T03526C535287FADF58CBFA05084AE89A0ACFFEFA_H
// System.String[]
struct StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E  : public RuntimeArray
{
public:
	ALIGN_FIELD (8) String_t* m_Items[1];

public:
	inline String_t* GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline String_t** GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, String_t* value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier(m_Items + index, value);
	}
	inline String_t* GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline String_t** GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, String_t* value)
	{
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier(m_Items + index, value);
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
// System.Object[]
struct ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A  : public RuntimeArray
{
public:
	ALIGN_FIELD (8) RuntimeObject * m_Items[1];

public:
	inline RuntimeObject * GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline RuntimeObject ** GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, RuntimeObject * value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier(m_Items + index, value);
	}
	inline RuntimeObject * GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline RuntimeObject ** GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, RuntimeObject * value)
	{
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier(m_Items + index, value);
	}
};


// System.Void System.Collections.Generic.Dictionary`2<System.Object,System.Object>::.ctor(System.Int32)
extern "C" IL2CPP_METHOD_ATTR void Dictionary_2__ctor_m2895EBB13AA7D9232058658A7DC404DC5F608923_gshared (Dictionary_2_t32F25F093828AA9F93CB11C2A2B4648FD62A09BA * __this, int32_t p0, const RuntimeMethod* method);
// System.Void System.Collections.Generic.Dictionary`2<System.Object,System.Object>::set_Item(!0,!1)
extern "C" IL2CPP_METHOD_ATTR void Dictionary_2_set_Item_m466D001F105E25DEB5C9BCB17837EE92A27FDE93_gshared (Dictionary_2_t32F25F093828AA9F93CB11C2A2B4648FD62A09BA * __this, RuntimeObject * p0, RuntimeObject * p1, const RuntimeMethod* method);
// System.Boolean System.Collections.Generic.Dictionary`2<System.Object,System.Object>::TryGetValue(!0,!1&)
extern "C" IL2CPP_METHOD_ATTR bool Dictionary_2_TryGetValue_m3455807C552312C60038DF52EF328C3687442DE3_gshared (Dictionary_2_t32F25F093828AA9F93CB11C2A2B4648FD62A09BA * __this, RuntimeObject * p0, RuntimeObject ** p1, const RuntimeMethod* method);
// System.Int32 System.Collections.Generic.Dictionary`2<System.Object,System.Object>::get_Count()
extern "C" IL2CPP_METHOD_ATTR int32_t Dictionary_2_get_Count_m1B06EB9D28DDA7E38DDC20D88532DFF246F03DF6_gshared (Dictionary_2_t32F25F093828AA9F93CB11C2A2B4648FD62A09BA * __this, const RuntimeMethod* method);

// System.Void System.Object::.ctor()
extern "C" IL2CPP_METHOD_ATTR void Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0 (RuntimeObject * __this, const RuntimeMethod* method);
// System.Void System.ArgumentNullException::.ctor(System.String)
extern "C" IL2CPP_METHOD_ATTR void ArgumentNullException__ctor_mEE0C0D6FCB2D08CD7967DBB1329A0854BBED49ED (ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD * __this, String_t* p0, const RuntimeMethod* method);
// System.Void System.UriBuilder::Init(System.Uri)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_Init_mB18B3A4578F102E7E99F18542236A6B5B6ABA174 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___uri0, const RuntimeMethod* method);
// System.String System.Uri::get_Fragment()
extern "C" IL2CPP_METHOD_ATTR String_t* Uri_get_Fragment_m111666DD668AC59B9F3C3D3CEEEC7F70F6904D41 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.String System.Uri::get_Query()
extern "C" IL2CPP_METHOD_ATTR String_t* Uri_get_Query_m3F64514B4DB7C849C8255BA3FE08C6BE983D2D56 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.String System.Uri::get_Host()
extern "C" IL2CPP_METHOD_ATTR String_t* Uri_get_Host_m2D942F397A36DBDA5E93452CBD983E0714018151 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.String System.Uri::get_AbsolutePath()
extern "C" IL2CPP_METHOD_ATTR String_t* Uri_get_AbsolutePath_mA9A825E2BBD0A43AD76EB9A9765E29E45FE32F31 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.Int32 System.Uri::get_Port()
extern "C" IL2CPP_METHOD_ATTR int32_t Uri_get_Port_m4E64AB9B50CCC50E7B1F139D7AF1403FAF97147C (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.String System.Uri::get_Scheme()
extern "C" IL2CPP_METHOD_ATTR String_t* Uri_get_Scheme_m14A8F0018D8AACADBEF39600A59944F33EE39187 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.Boolean System.Uri::get_HasAuthority()
extern "C" IL2CPP_METHOD_ATTR bool Uri_get_HasAuthority_m969936D80AE0309273733487C8B38BEE33468712 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.String System.Uri::get_UserInfo()
extern "C" IL2CPP_METHOD_ATTR String_t* Uri_get_UserInfo_m201C93A932C446805E9143EBE969048D7E75C71E (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.Boolean System.String::IsNullOrEmpty(System.String)
extern "C" IL2CPP_METHOD_ATTR bool String_IsNullOrEmpty_m06A85A206AC2106D1982826C5665B9BD35324229 (String_t* p0, const RuntimeMethod* method);
// System.Int32 System.String::IndexOf(System.Char)
extern "C" IL2CPP_METHOD_ATTR int32_t String_IndexOf_m2909B8CF585E1BD0C81E11ACA2F48012156FD5BD (String_t* __this, Il2CppChar p0, const RuntimeMethod* method);
// System.String System.String::Substring(System.Int32)
extern "C" IL2CPP_METHOD_ATTR String_t* String_Substring_m2C4AFF5E79DD8BADFD2DFBCF156BF728FBB8E1AE (String_t* __this, int32_t p0, const RuntimeMethod* method);
// System.String System.String::Substring(System.Int32,System.Int32)
extern "C" IL2CPP_METHOD_ATTR String_t* String_Substring_mB593C0A320C683E6E47EFFC0A12B7A465E5E43BB (String_t* __this, int32_t p0, int32_t p1, const RuntimeMethod* method);
// System.Void System.UriBuilder::SetFieldsFromUri(System.Uri)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_SetFieldsFromUri_m54B4EB1ACEF01F2B0B11EC81768BB7D56245447F (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___uri0, const RuntimeMethod* method);
// System.Void System.UriBuilder::set_Scheme(System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_set_Scheme_mD20C10C2D43C0C2C96D9098BE4331D46FCC45921 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___value0, const RuntimeMethod* method);
// System.Void System.UriBuilder::set_Host(System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_set_Host_m7213BE98F62DE6A099EA8EEFF479949C5F1EA680 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___value0, const RuntimeMethod* method);
// System.Void System.UriBuilder::.ctor(System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder__ctor_m7A9B7FFE61632B2181BBF326580950494257464F (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___schemeName0, String_t* ___hostName1, const RuntimeMethod* method);
// System.Void System.UriBuilder::set_Port(System.Int32)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_set_Port_m14DBA6E597BED983B73F4AD7F1215C6E474DB6F3 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, int32_t ___value0, const RuntimeMethod* method);
// System.Void System.UriBuilder::.ctor(System.String,System.String,System.Int32)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder__ctor_mBF5DB989568C4C36033EAF9AE939734782DC722D (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___scheme0, String_t* ___host1, int32_t ___portNumber2, const RuntimeMethod* method);
// System.Void System.UriBuilder::set_Path(System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_set_Path_mB5E891CD6B419F1310178B20F5E47E49D0F828E8 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___value0, const RuntimeMethod* method);
// System.Void System.UriBuilder::.ctor(System.String,System.String,System.Int32,System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder__ctor_mD85B55F4C0313B5E28C9F10277C2E81CF5650215 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___scheme0, String_t* ___host1, int32_t ___port2, String_t* ___pathValue3, const RuntimeMethod* method);
// System.Void System.UriBuilder::set_Extra(System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_set_Extra_mBA671DFDA9152422D2C52BD7087F78ACB3A3673C (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___value0, const RuntimeMethod* method);
// System.Void System.ArgumentException::.ctor(System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR void ArgumentException__ctor_m26DC3463C6F3C98BF33EA39598DD2B32F0249CA8 (ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 * __this, String_t* p0, String_t* p1, const RuntimeMethod* method);
// System.Int32 System.String::get_Length()
extern "C" IL2CPP_METHOD_ATTR int32_t String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018 (String_t* __this, const RuntimeMethod* method);
// System.Char System.String::get_Chars(System.Int32)
extern "C" IL2CPP_METHOD_ATTR Il2CppChar String_get_Chars_m14308AC3B95F8C1D9F1D1055B116B37D595F1D96 (String_t* __this, int32_t p0, const RuntimeMethod* method);
// System.Void System.UriBuilder::set_Fragment(System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_set_Fragment_m2283E0E13EE8BBA2F4FFA10CDAD1EA2D6771EE11 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___value0, const RuntimeMethod* method);
// System.Void System.UriBuilder::set_Query(System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_set_Query_m392BC383004E6922D6B53870D2195E7F871FCEC8 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___value0, const RuntimeMethod* method);
// System.String System.String::Concat(System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR String_t* String_Concat_mB78D0094592718DA6D5DB6C712A9C225631666BE (String_t* p0, String_t* p1, const RuntimeMethod* method);
// System.String System.String::Concat(System.String,System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR String_t* String_Concat_mF4626905368D6558695A823466A1AF65EADB9923 (String_t* p0, String_t* p1, String_t* p2, const RuntimeMethod* method);
// System.String System.String::Replace(System.Char,System.Char)
extern "C" IL2CPP_METHOD_ATTR String_t* String_Replace_m276641366A463205C185A9B3DC0E24ECB95122C9 (String_t* __this, Il2CppChar p0, Il2CppChar p1, const RuntimeMethod* method);
// System.String System.Uri::InternalEscapeString(System.String)
extern "C" IL2CPP_METHOD_ATTR String_t* Uri_InternalEscapeString_m4A79B5EFDD0254232524BA43FAC32297A825F873 (String_t* ___rawString0, const RuntimeMethod* method);
// System.Void System.ArgumentOutOfRangeException::.ctor(System.String)
extern "C" IL2CPP_METHOD_ATTR void ArgumentOutOfRangeException__ctor_m6B36E60C989DC798A8B44556DB35960282B133A6 (ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA * __this, String_t* p0, const RuntimeMethod* method);
// System.Boolean System.Uri::CheckSchemeName(System.String)
extern "C" IL2CPP_METHOD_ATTR bool Uri_CheckSchemeName_m351E06F4546E0F84E2DDB286B531F39D379589BC (String_t* ___schemeName0, const RuntimeMethod* method);
// System.String System.String::ToLowerInvariant()
extern "C" IL2CPP_METHOD_ATTR String_t* String_ToLowerInvariant_m197BD65B6582DC546FF1BC398161EEFA708F799E (String_t* __this, const RuntimeMethod* method);
// System.Void System.Uri::.ctor(System.String)
extern "C" IL2CPP_METHOD_ATTR void Uri__ctor_mBA69907A1D799CD12ED44B611985B25FE4C626A2 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, String_t* ___uriString0, const RuntimeMethod* method);
// System.Uri System.UriBuilder::get_Uri()
extern "C" IL2CPP_METHOD_ATTR Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * UriBuilder_get_Uri_mDCABA4CD1D05D4B9C4CBA063BC7CA94EE6CCC631 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, const RuntimeMethod* method);
// System.Void System.UriFormatException::.ctor(System.String)
extern "C" IL2CPP_METHOD_ATTR void UriFormatException__ctor_mE1D46962CC168EB07B59D1265F5734A8F587567D (UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A * __this, String_t* ___textString0, const RuntimeMethod* method);
// System.UriParser System.UriParser::GetSyntax(System.String)
extern "C" IL2CPP_METHOD_ATTR UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * UriParser_GetSyntax_mC2FEAF79ECEB6550573A1C0578141BB236F7EF16 (String_t* ___lwrCaseScheme0, const RuntimeMethod* method);
// System.Boolean System.UriParser::InFact(System.UriSyntaxFlags)
extern "C" IL2CPP_METHOD_ATTR bool UriParser_InFact_mDD42FA932B6830D99AA04C2AE7875BA5067C86F3 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, int32_t ___flags0, const RuntimeMethod* method);
// System.Boolean System.UriParser::NotAny(System.UriSyntaxFlags)
extern "C" IL2CPP_METHOD_ATTR bool UriParser_NotAny_mC998A35DC290F35FFAFFB6A8B66C7B881F2559D3 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, int32_t ___flags0, const RuntimeMethod* method);
// System.String System.Int32::ToString()
extern "C" IL2CPP_METHOD_ATTR String_t* Int32_ToString_m1863896DE712BF97C031D55B12E1583F1982DC02 (int32_t* __this, const RuntimeMethod* method);
// System.String System.String::Concat(System.String[])
extern "C" IL2CPP_METHOD_ATTR String_t* String_Concat_m232E857CA5107EA6AC52E7DD7018716C021F237B (StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* p0, const RuntimeMethod* method);
// System.Void System.FormatException::.ctor()
extern "C" IL2CPP_METHOD_ATTR void FormatException__ctor_m6DAD3E32EE0445420B4893EA683425AC3441609B (FormatException_t2808E076CDE4650AF89F55FD78F49290D0EC5BDC * __this, const RuntimeMethod* method);
// System.Void System.FormatException::.ctor(System.String)
extern "C" IL2CPP_METHOD_ATTR void FormatException__ctor_m89167FF9884AE20232190FE9286DC50E146A4F14 (FormatException_t2808E076CDE4650AF89F55FD78F49290D0EC5BDC * __this, String_t* p0, const RuntimeMethod* method);
// System.Void System.FormatException::.ctor(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)
extern "C" IL2CPP_METHOD_ATTR void FormatException__ctor_mDC141C414E24BE865FC8853970BF83C5B8C7676C (FormatException_t2808E076CDE4650AF89F55FD78F49290D0EC5BDC * __this, SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26 * p0, StreamingContext_t2CCDC54E0E8D078AF4A50E3A8B921B828A900034  p1, const RuntimeMethod* method);
// System.Void System.Exception::GetObjectData(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)
extern "C" IL2CPP_METHOD_ATTR void Exception_GetObjectData_m76F759ED00FA218FFC522C32626B851FDE849AD6 (Exception_t * __this, SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26 * p0, StreamingContext_t2CCDC54E0E8D078AF4A50E3A8B921B828A900034  p1, const RuntimeMethod* method);
// System.String SR::GetString(System.String)
extern "C" IL2CPP_METHOD_ATTR String_t* SR_GetString_m3FC710B15474A9B651DA02B303241B6D8B87E2A7 (String_t* ___name0, const RuntimeMethod* method);
// System.Int32 System.Runtime.CompilerServices.RuntimeHelpers::get_OffsetToStringData()
extern "C" IL2CPP_METHOD_ATTR int32_t RuntimeHelpers_get_OffsetToStringData_mF3B79A906181F1A2734590DA161E2AF183853F8B (const RuntimeMethod* method);
// System.Int32 System.Math::Min(System.Int32,System.Int32)
extern "C" IL2CPP_METHOD_ATTR int32_t Math_Min_mC950438198519FB2B0260FCB91220847EE4BB525 (int32_t p0, int32_t p1, const RuntimeMethod* method);
// System.Char[] System.UriHelper::EnsureDestinationSize(System.Char*,System.Char[],System.Int32,System.Int16,System.Int16,System.Int32&,System.Int32)
extern "C" IL2CPP_METHOD_ATTR CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* UriHelper_EnsureDestinationSize_m64F4907D0411AAAD1C05E0AD0D2EB120DCBA9217 (Il2CppChar* ___pStr0, CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___dest1, int32_t ___currentInputPos2, int16_t ___charsToAdd3, int16_t ___minReallocateChars4, int32_t* ___destPos5, int32_t ___prevInputPos6, const RuntimeMethod* method);
// System.Text.Encoding System.Text.Encoding::get_UTF8()
extern "C" IL2CPP_METHOD_ATTR Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * Encoding_get_UTF8_m67C8652936B681E7BC7505E459E88790E0FF16D9 (const RuntimeMethod* method);
// System.Void System.UriHelper::EscapeAsciiChar(System.Char,System.Char[],System.Int32&)
extern "C" IL2CPP_METHOD_ATTR void UriHelper_EscapeAsciiChar_mFD7DE796BD53CBD2B1E73080FE0346D37F358902 (Il2CppChar ___ch0, CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___to1, int32_t* ___pos2, const RuntimeMethod* method);
// System.Char System.UriHelper::EscapedAscii(System.Char,System.Char)
extern "C" IL2CPP_METHOD_ATTR Il2CppChar UriHelper_EscapedAscii_m06D556717795E649EBBB30E4CBCF3D221C1FEB78 (Il2CppChar ___digit0, Il2CppChar ___next1, const RuntimeMethod* method);
// System.Boolean System.UriHelper::IsUnreserved(System.Char)
extern "C" IL2CPP_METHOD_ATTR bool UriHelper_IsUnreserved_mAADC7DCEEA864AFB49311696ABBDD76811FAAE48 (Il2CppChar ___c0, const RuntimeMethod* method);
// System.Boolean System.UriHelper::IsReservedUnreservedOrHash(System.Char)
extern "C" IL2CPP_METHOD_ATTR bool UriHelper_IsReservedUnreservedOrHash_m3D7256DABA7F540F8D379FC1D1C54F1C63E46059 (Il2CppChar ___c0, const RuntimeMethod* method);
// System.Void System.Buffer::BlockCopy(System.Array,System.Int32,System.Array,System.Int32,System.Int32)
extern "C" IL2CPP_METHOD_ATTR void Buffer_BlockCopy_m1F882D595976063718AF6E405664FC761924D353 (RuntimeArray * p0, int32_t p1, RuntimeArray * p2, int32_t p3, int32_t p4, const RuntimeMethod* method);
// System.Char[] System.UriHelper::UnescapeString(System.Char*,System.Int32,System.Int32,System.Char[],System.Int32&,System.Char,System.Char,System.Char,System.UnescapeMode,System.UriParser,System.Boolean)
extern "C" IL2CPP_METHOD_ATTR CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* UriHelper_UnescapeString_mD4815AEAF34E25D31AA4BB4A76B88055F0A49E89 (Il2CppChar* ___pStr0, int32_t ___start1, int32_t ___end2, CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___dest3, int32_t* ___destPosition4, Il2CppChar ___rsvd15, Il2CppChar ___rsvd26, Il2CppChar ___rsvd37, int32_t ___unescapeMode8, UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___syntax9, bool ___isQuery10, const RuntimeMethod* method);
// System.Boolean System.Uri::IriParsingStatic(System.UriParser)
extern "C" IL2CPP_METHOD_ATTR bool Uri_IriParsingStatic_m39FC9677B4B9EFBADF814F2EEA58280F35A1D3E5 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___syntax0, const RuntimeMethod* method);
// System.Boolean System.UriHelper::IsNotSafeForUnescape(System.Char)
extern "C" IL2CPP_METHOD_ATTR bool UriHelper_IsNotSafeForUnescape_m1D0461E7C5A3CFBD7A2A7F7322B66BC68CCE741D (Il2CppChar ___ch0, const RuntimeMethod* method);
// System.Boolean System.IriHelper::CheckIriUnicodeRange(System.Char,System.Boolean)
extern "C" IL2CPP_METHOD_ATTR bool IriHelper_CheckIriUnicodeRange_mA9BAAD6D244ADEE8986FDC0DFB3DFDA90C093A6C (Il2CppChar ___unicode0, bool ___isQuery1, const RuntimeMethod* method);
// System.Void System.Text.EncoderReplacementFallback::.ctor(System.String)
extern "C" IL2CPP_METHOD_ATTR void EncoderReplacementFallback__ctor_mAE97C6B5EF9A81A90315A21E68271FAE87A738FD (EncoderReplacementFallback_tC2E8A94C82BBF7A4CFC8E3FDBA8A381DCF29F998 * __this, String_t* p0, const RuntimeMethod* method);
// System.Void System.Text.Encoding::set_EncoderFallback(System.Text.EncoderFallback)
extern "C" IL2CPP_METHOD_ATTR void Encoding_set_EncoderFallback_m24306F093457AE12D59A36AB84F1E03C840BD10A (Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * __this, EncoderFallback_tDE342346D01608628F1BCEBB652D31009852CF63 * p0, const RuntimeMethod* method);
// System.Void System.Text.DecoderReplacementFallback::.ctor(System.String)
extern "C" IL2CPP_METHOD_ATTR void DecoderReplacementFallback__ctor_m9D82FC93423AD9B954F28E30B20BF14DAFB01A5B (DecoderReplacementFallback_t8CF74B2DAE2A08AEA7DF6366778D2E3EA75FC742 * __this, String_t* p0, const RuntimeMethod* method);
// System.Void System.Text.Encoding::set_DecoderFallback(System.Text.DecoderFallback)
extern "C" IL2CPP_METHOD_ATTR void Encoding_set_DecoderFallback_mB321EB8D6C34B8935A169C0E4FAC7A4E0A99FACC (Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * __this, DecoderFallback_t128445EB7676870485230893338EF044F6B72F60 * p0, const RuntimeMethod* method);
// System.Void System.UriHelper::MatchUTF8Sequence(System.Char*,System.Char[],System.Int32&,System.Char[],System.Int32,System.Byte[],System.Int32,System.Boolean,System.Boolean)
extern "C" IL2CPP_METHOD_ATTR void UriHelper_MatchUTF8Sequence_m4835D9BB77C2701643B14D6FFD3D7057F8C9007F (Il2CppChar* ___pDest0, CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___dest1, int32_t* ___destOffset2, CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___unescapedChars3, int32_t ___charCount4, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___bytes5, int32_t ___byteCount6, bool ___isQuery7, bool ___iriParsing8, const RuntimeMethod* method);
// System.Boolean System.Char::IsHighSurrogate(System.Char)
extern "C" IL2CPP_METHOD_ATTR bool Char_IsHighSurrogate_m64C60C09A8561520E43C8527D3DC38FF97E6274D (Il2CppChar p0, const RuntimeMethod* method);
// System.Boolean System.IriHelper::CheckIriUnicodeRange(System.Char,System.Char,System.Boolean&,System.Boolean)
extern "C" IL2CPP_METHOD_ATTR bool IriHelper_CheckIriUnicodeRange_m5ED29083C22062AEAB8B5787C9A27CFEEC397AD9 (Il2CppChar ___highSurr0, Il2CppChar ___lowSurr1, bool* ___surrogatePair2, bool ___isQuery3, const RuntimeMethod* method);
// System.Boolean System.Uri::IsBidiControlCharacter(System.Char)
extern "C" IL2CPP_METHOD_ATTR bool Uri_IsBidiControlCharacter_mB14EA5816A434B7CE382EB9ACBD1432916EC341D (Il2CppChar ___ch0, const RuntimeMethod* method);
// System.Boolean System.UriParser::get_ShouldUseLegacyV2Quirks()
extern "C" IL2CPP_METHOD_ATTR bool UriParser_get_ShouldUseLegacyV2Quirks_mD4C8DF67677ACCCC3B5E026099ECC0BDA24D96DD (const RuntimeMethod* method);
// System.Boolean System.Uri::IsAsciiLetterOrDigit(System.Char)
extern "C" IL2CPP_METHOD_ATTR bool Uri_IsAsciiLetterOrDigit_mEBA81E735141504B5804F0B3C94EC39B24AF8661 (Il2CppChar ___character0, const RuntimeMethod* method);
// System.Void System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray(System.Array,System.RuntimeFieldHandle)
extern "C" IL2CPP_METHOD_ATTR void RuntimeHelpers_InitializeArray_m29F50CDFEEE0AB868200291366253DD4737BC76A (RuntimeArray * p0, RuntimeFieldHandle_t844BDF00E8E6FE69D9AEAA7657F09018B864F4EF  p1, const RuntimeMethod* method);
// System.UriFormatException System.Uri::ParseMinimal()
extern "C" IL2CPP_METHOD_ATTR UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A * Uri_ParseMinimal_m35FCFE52F12315DA60733B807E7C0AB408C0A9CF (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.Boolean System.Uri::get_UserDrivenParsing()
extern "C" IL2CPP_METHOD_ATTR bool Uri_get_UserDrivenParsing_mFF27964894B5C0432C37E425F319D6C915BCDC39 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.Type System.Object::GetType()
extern "C" IL2CPP_METHOD_ATTR Type_t * Object_GetType_m2E0B62414ECCAA3094B703790CE88CBB2F83EA60 (RuntimeObject * __this, const RuntimeMethod* method);
// System.String SR::GetString(System.String,System.Object[])
extern "C" IL2CPP_METHOD_ATTR String_t* SR_GetString_m9548BD6DD52DFDB46372F211078AE57FA2401E39 (String_t* ___name0, ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* ___args1, const RuntimeMethod* method);
// System.Void System.InvalidOperationException::.ctor(System.String)
extern "C" IL2CPP_METHOD_ATTR void InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706 (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * __this, String_t* p0, const RuntimeMethod* method);
// System.Boolean System.Uri::get_IsAbsoluteUri()
extern "C" IL2CPP_METHOD_ATTR bool Uri_get_IsAbsoluteUri_m8C189085F1C675DBC3148AA70C38074EC075D722 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.Uri System.Uri::ResolveHelper(System.Uri,System.Uri,System.String&,System.Boolean&,System.UriFormatException&)
extern "C" IL2CPP_METHOD_ATTR Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * Uri_ResolveHelper_mEDF1549C3E9AC1CF6177DCF93B17D574411916BC (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___baseUri0, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___relativeUri1, String_t** ___newUriString2, bool* ___userEscaped3, UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A ** ___e4, const RuntimeMethod* method);
// System.Boolean System.Uri::op_Inequality(System.Uri,System.Uri)
extern "C" IL2CPP_METHOD_ATTR bool Uri_op_Inequality_m07015206F59460E87CDE2A8D303D5712E30A7F6B (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___uri10, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___uri21, const RuntimeMethod* method);
// System.String System.Uri::get_OriginalString()
extern "C" IL2CPP_METHOD_ATTR String_t* Uri_get_OriginalString_m56099E46276F0A52524347F1F46A2F88E948504F (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.Void System.ArgumentOutOfRangeException::.ctor(System.String,System.Object,System.String)
extern "C" IL2CPP_METHOD_ATTR void ArgumentOutOfRangeException__ctor_m755B01B4B4595B447596E3281F22FD7CE6DAE378 (ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA * __this, String_t* p0, RuntimeObject * p1, String_t* p2, const RuntimeMethod* method);
// System.String System.Uri::GetComponentsHelper(System.UriComponents,System.UriFormat)
extern "C" IL2CPP_METHOD_ATTR String_t* Uri_GetComponentsHelper_m28B0D80FD94A40685C0F70652AB26755C457B2D3 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, int32_t ___uriComponents0, int32_t ___uriFormat1, const RuntimeMethod* method);
// System.Boolean System.Uri::InternalIsWellFormedOriginalString()
extern "C" IL2CPP_METHOD_ATTR bool Uri_InternalIsWellFormedOriginalString_mC5B6EDD6C06519FC6E5176DB89237CCCFFE56CAB (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, const RuntimeMethod* method);
// System.Void System.Collections.Generic.Dictionary`2<System.String,System.UriParser>::.ctor(System.Int32)
inline void Dictionary_2__ctor_m9AA6FFC23A9032DF2BF483986951F06E722B3445 (Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * __this, int32_t p0, const RuntimeMethod* method)
{
	((  void (*) (Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE *, int32_t, const RuntimeMethod*))Dictionary_2__ctor_m2895EBB13AA7D9232058658A7DC404DC5F608923_gshared)(__this, p0, method);
}
// System.Void System.UriParser/BuiltInUriParser::.ctor(System.String,System.Int32,System.UriSyntaxFlags)
extern "C" IL2CPP_METHOD_ATTR void BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * __this, String_t* ___lwrCaseScheme0, int32_t ___defaultPort1, int32_t ___syntaxFlags2, const RuntimeMethod* method);
// System.String System.UriParser::get_SchemeName()
extern "C" IL2CPP_METHOD_ATTR String_t* UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, const RuntimeMethod* method);
// System.Void System.Collections.Generic.Dictionary`2<System.String,System.UriParser>::set_Item(!0,!1)
inline void Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB (Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * __this, String_t* p0, UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * p1, const RuntimeMethod* method)
{
	((  void (*) (Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE *, String_t*, UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC *, const RuntimeMethod*))Dictionary_2_set_Item_m466D001F105E25DEB5C9BCB17837EE92A27FDE93_gshared)(__this, p0, p1, method);
}
// System.Boolean System.UriParser::IsFullMatch(System.UriSyntaxFlags,System.UriSyntaxFlags)
extern "C" IL2CPP_METHOD_ATTR bool UriParser_IsFullMatch_m7B5F47A62FA721E550C5439FAA4C6AFAC34EB23E (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, int32_t ___flags0, int32_t ___expected1, const RuntimeMethod* method);
// System.Boolean System.Collections.Generic.Dictionary`2<System.String,System.UriParser>::TryGetValue(!0,!1&)
inline bool Dictionary_2_TryGetValue_mB7FEE5E187FD932CA98FA958AFCC096E123BCDC4 (Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * __this, String_t* p0, UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC ** p1, const RuntimeMethod* method)
{
	return ((  bool (*) (Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE *, String_t*, UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC **, const RuntimeMethod*))Dictionary_2_TryGetValue_m3455807C552312C60038DF52EF328C3687442DE3_gshared)(__this, p0, p1, method);
}
// System.Void System.Threading.Monitor::Enter(System.Object,System.Boolean&)
extern "C" IL2CPP_METHOD_ATTR void Monitor_Enter_mC5B353DD83A0B0155DF6FBCC4DF5A580C25534C5 (RuntimeObject * p0, bool* p1, const RuntimeMethod* method);
// System.Int32 System.Collections.Generic.Dictionary`2<System.String,System.UriParser>::get_Count()
inline int32_t Dictionary_2_get_Count_mEC5A51E9EC624CA697AFE307D4CD767026962AE3 (Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE *, const RuntimeMethod*))Dictionary_2_get_Count_m1B06EB9D28DDA7E38DDC20D88532DFF246F03DF6_gshared)(__this, method);
}
// System.Void System.Threading.Monitor::Exit(System.Object)
extern "C" IL2CPP_METHOD_ATTR void Monitor_Exit_m49A1E5356D984D0B934BB97A305E2E5E207225C2 (RuntimeObject * p0, const RuntimeMethod* method);
// System.Void System.UriParser::.ctor(System.UriSyntaxFlags)
extern "C" IL2CPP_METHOD_ATTR void UriParser__ctor_mAF168F2B88BC5301B722C1BAAD45E381FBA22E3D (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, int32_t ___flags0, const RuntimeMethod* method);
// System.Void System.ComponentModel.TypeConverter::.ctor()
extern "C" IL2CPP_METHOD_ATTR void TypeConverter__ctor_m7F8A006E775CCB83A8ACB042B296E48B0AE501CD (TypeConverter_t8306AE03734853B551DDF089C1F17836A7764DBB * __this, const RuntimeMethod* method);
// System.Type System.Type::GetTypeFromHandle(System.RuntimeTypeHandle)
extern "C" IL2CPP_METHOD_ATTR Type_t * Type_GetTypeFromHandle_m9DC58ADF0512987012A8A016FB64B068F3B1AFF6 (RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D  p0, const RuntimeMethod* method);
// System.Boolean System.Type::op_Equality(System.Type,System.Type)
extern "C" IL2CPP_METHOD_ATTR bool Type_op_Equality_m7040622C9E1037EFC73E1F0EDB1DD241282BE3D8 (Type_t * p0, Type_t * p1, const RuntimeMethod* method);
// System.Boolean System.UriTypeConverter::CanConvert(System.Type)
extern "C" IL2CPP_METHOD_ATTR bool UriTypeConverter_CanConvert_m0F0FB34A1DC16C677BF8F4ED0E720144C17C4795 (UriTypeConverter_t96793526764A246FBAEE2F4F639AFAF270EE81D1 * __this, Type_t * ___type0, const RuntimeMethod* method);
// System.String Locale::GetText(System.String)
extern "C" IL2CPP_METHOD_ATTR String_t* Locale_GetText_m41F0CB4E76BAAB1E97D9D92D109C846A8ECC1324 (String_t* p0, const RuntimeMethod* method);
// System.Void System.NotSupportedException::.ctor(System.String)
extern "C" IL2CPP_METHOD_ATTR void NotSupportedException__ctor_mD023A89A5C1F740F43F0A9CD6C49DC21230B3CEE (NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 * __this, String_t* p0, const RuntimeMethod* method);
// System.Boolean System.String::op_Equality(System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR bool String_op_Equality_m139F0E4195AE2F856019E63B241F36F016997FCE (String_t* p0, String_t* p1, const RuntimeMethod* method);
// System.Void System.Uri::.ctor(System.String,System.UriKind)
extern "C" IL2CPP_METHOD_ATTR void Uri__ctor_mA02DB222F4F35380DE2700D84F58EB42497FDDE4 (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * __this, String_t* ___uriString0, int32_t ___uriKind1, const RuntimeMethod* method);
// System.Object System.ComponentModel.TypeConverter::ConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,System.Object)
extern "C" IL2CPP_METHOD_ATTR RuntimeObject * TypeConverter_ConvertFrom_mD5AE49E422520F6E07B3C0D6202788E49B4698A3 (TypeConverter_t8306AE03734853B551DDF089C1F17836A7764DBB * __this, RuntimeObject* ___context0, CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * ___culture1, RuntimeObject * ___value2, const RuntimeMethod* method);
// System.Void System.PlatformNotSupportedException::.ctor()
extern "C" IL2CPP_METHOD_ATTR void PlatformNotSupportedException__ctor_m651139B17C9EE918551490BC675754EA8EA3E7C7 (PlatformNotSupportedException_t14FE109377F8FA8B3B2F9A0C4FE3BF10662C73B5 * __this, const RuntimeMethod* method);
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
// System.Void System.Uri_MoreInfo::.ctor()
extern "C" IL2CPP_METHOD_ATTR void MoreInfo__ctor_mFE29F028646C12EDCAF7F0F78F9A85D52C10B83C (MoreInfo_t83B9EC79244C26B468C115E54C0BEF09BB2E05B5 * __this, const RuntimeMethod* method)
{
	{
		Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0(__this, /*hidden argument*/NULL);
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
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void System.Uri_UriInfo::.ctor()
extern "C" IL2CPP_METHOD_ATTR void UriInfo__ctor_m24EFE7B4E03C9FFB8B797770D626680947C87D98 (UriInfo_t9FCC6BD4EC1EA14D75209E6A35417057BF6EDC5E * __this, const RuntimeMethod* method)
{
	{
		Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0(__this, /*hidden argument*/NULL);
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
// System.Void System.UriBuilder::.ctor()
extern "C" IL2CPP_METHOD_ATTR void UriBuilder__ctor_mFC8729DAB4291B5B6500207C85DCB3985B46BB52 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriBuilder__ctor_mFC8729DAB4291B5B6500207C85DCB3985B46BB52_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		__this->set__changed_0((bool)1);
		String_t* L_0 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		__this->set__fragment_1(L_0);
		__this->set__host_2(_stringLiteral334389048B872A533002B34D73F8C29FD09EFC50);
		String_t* L_1 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		__this->set__password_3(L_1);
		__this->set__path_4(_stringLiteral42099B4AF021E53FD8FD4E056C2568D7C2E3FFA8);
		__this->set__port_5((-1));
		String_t* L_2 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		__this->set__query_6(L_2);
		__this->set__scheme_7(_stringLiteral77B5F8E343A90F6F597751021FB8B7A08FE83083);
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		String_t* L_3 = ((Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields*)il2cpp_codegen_static_fields_for(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var))->get_SchemeDelimiter_12();
		__this->set__schemeDelimiter_8(L_3);
		String_t* L_4 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		__this->set__username_10(L_4);
		Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0(__this, /*hidden argument*/NULL);
		return;
	}
}
// System.Void System.UriBuilder::.ctor(System.Uri)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder__ctor_m1B050A706B91D8EDCF5DD4A98CA1F1A0FE6EA8AE (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___uri0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriBuilder__ctor_m1B050A706B91D8EDCF5DD4A98CA1F1A0FE6EA8AE_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		__this->set__changed_0((bool)1);
		String_t* L_0 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		__this->set__fragment_1(L_0);
		__this->set__host_2(_stringLiteral334389048B872A533002B34D73F8C29FD09EFC50);
		String_t* L_1 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		__this->set__password_3(L_1);
		__this->set__path_4(_stringLiteral42099B4AF021E53FD8FD4E056C2568D7C2E3FFA8);
		__this->set__port_5((-1));
		String_t* L_2 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		__this->set__query_6(L_2);
		__this->set__scheme_7(_stringLiteral77B5F8E343A90F6F597751021FB8B7A08FE83083);
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		String_t* L_3 = ((Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields*)il2cpp_codegen_static_fields_for(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var))->get_SchemeDelimiter_12();
		__this->set__schemeDelimiter_8(L_3);
		String_t* L_4 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		__this->set__username_10(L_4);
		Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0(__this, /*hidden argument*/NULL);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_5 = ___uri0;
		if (L_5)
		{
			goto IL_007a;
		}
	}
	{
		ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD * L_6 = (ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD *)il2cpp_codegen_object_new(ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD_il2cpp_TypeInfo_var);
		ArgumentNullException__ctor_mEE0C0D6FCB2D08CD7967DBB1329A0854BBED49ED(L_6, _stringLiteral2C6D680F5C570BA21D22697CD028F230E9F4CD56, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_6, NULL, UriBuilder__ctor_m1B050A706B91D8EDCF5DD4A98CA1F1A0FE6EA8AE_RuntimeMethod_var);
	}

IL_007a:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_7 = ___uri0;
		UriBuilder_Init_mB18B3A4578F102E7E99F18542236A6B5B6ABA174(__this, L_7, /*hidden argument*/NULL);
		return;
	}
}
// System.Void System.UriBuilder::Init(System.Uri)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_Init_mB18B3A4578F102E7E99F18542236A6B5B6ABA174 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___uri0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriBuilder_Init_mB18B3A4578F102E7E99F18542236A6B5B6ABA174_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	String_t* V_0 = NULL;
	int32_t V_1 = 0;
	UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * G_B2_0 = NULL;
	UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * G_B1_0 = NULL;
	String_t* G_B3_0 = NULL;
	UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * G_B3_1 = NULL;
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_0 = ___uri0;
		NullCheck(L_0);
		String_t* L_1 = Uri_get_Fragment_m111666DD668AC59B9F3C3D3CEEEC7F70F6904D41(L_0, /*hidden argument*/NULL);
		__this->set__fragment_1(L_1);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_2 = ___uri0;
		NullCheck(L_2);
		String_t* L_3 = Uri_get_Query_m3F64514B4DB7C849C8255BA3FE08C6BE983D2D56(L_2, /*hidden argument*/NULL);
		__this->set__query_6(L_3);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_4 = ___uri0;
		NullCheck(L_4);
		String_t* L_5 = Uri_get_Host_m2D942F397A36DBDA5E93452CBD983E0714018151(L_4, /*hidden argument*/NULL);
		__this->set__host_2(L_5);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_6 = ___uri0;
		NullCheck(L_6);
		String_t* L_7 = Uri_get_AbsolutePath_mA9A825E2BBD0A43AD76EB9A9765E29E45FE32F31(L_6, /*hidden argument*/NULL);
		__this->set__path_4(L_7);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_8 = ___uri0;
		NullCheck(L_8);
		int32_t L_9 = Uri_get_Port_m4E64AB9B50CCC50E7B1F139D7AF1403FAF97147C(L_8, /*hidden argument*/NULL);
		__this->set__port_5(L_9);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_10 = ___uri0;
		NullCheck(L_10);
		String_t* L_11 = Uri_get_Scheme_m14A8F0018D8AACADBEF39600A59944F33EE39187(L_10, /*hidden argument*/NULL);
		__this->set__scheme_7(L_11);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_12 = ___uri0;
		NullCheck(L_12);
		bool L_13 = Uri_get_HasAuthority_m969936D80AE0309273733487C8B38BEE33468712(L_12, /*hidden argument*/NULL);
		G_B1_0 = __this;
		if (L_13)
		{
			G_B2_0 = __this;
			goto IL_0058;
		}
	}
	{
		G_B3_0 = _stringLiteral05A79F06CF3F67F726DAE68D18A2290F6C9A50C9;
		G_B3_1 = G_B1_0;
		goto IL_005d;
	}

IL_0058:
	{
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		String_t* L_14 = ((Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields*)il2cpp_codegen_static_fields_for(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var))->get_SchemeDelimiter_12();
		G_B3_0 = L_14;
		G_B3_1 = G_B2_0;
	}

IL_005d:
	{
		NullCheck(G_B3_1);
		G_B3_1->set__schemeDelimiter_8(G_B3_0);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_15 = ___uri0;
		NullCheck(L_15);
		String_t* L_16 = Uri_get_UserInfo_m201C93A932C446805E9143EBE969048D7E75C71E(L_15, /*hidden argument*/NULL);
		V_0 = L_16;
		String_t* L_17 = V_0;
		bool L_18 = String_IsNullOrEmpty_m06A85A206AC2106D1982826C5665B9BD35324229(L_17, /*hidden argument*/NULL);
		if (L_18)
		{
			goto IL_00a4;
		}
	}
	{
		String_t* L_19 = V_0;
		NullCheck(L_19);
		int32_t L_20 = String_IndexOf_m2909B8CF585E1BD0C81E11ACA2F48012156FD5BD(L_19, ((int32_t)58), /*hidden argument*/NULL);
		V_1 = L_20;
		int32_t L_21 = V_1;
		if ((((int32_t)L_21) == ((int32_t)(-1))))
		{
			goto IL_009d;
		}
	}
	{
		String_t* L_22 = V_0;
		int32_t L_23 = V_1;
		NullCheck(L_22);
		String_t* L_24 = String_Substring_m2C4AFF5E79DD8BADFD2DFBCF156BF728FBB8E1AE(L_22, ((int32_t)il2cpp_codegen_add((int32_t)L_23, (int32_t)1)), /*hidden argument*/NULL);
		__this->set__password_3(L_24);
		String_t* L_25 = V_0;
		int32_t L_26 = V_1;
		NullCheck(L_25);
		String_t* L_27 = String_Substring_mB593C0A320C683E6E47EFFC0A12B7A465E5E43BB(L_25, 0, L_26, /*hidden argument*/NULL);
		__this->set__username_10(L_27);
		goto IL_00a4;
	}

IL_009d:
	{
		String_t* L_28 = V_0;
		__this->set__username_10(L_28);
	}

IL_00a4:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_29 = ___uri0;
		UriBuilder_SetFieldsFromUri_m54B4EB1ACEF01F2B0B11EC81768BB7D56245447F(__this, L_29, /*hidden argument*/NULL);
		return;
	}
}
// System.Void System.UriBuilder::.ctor(System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder__ctor_m7A9B7FFE61632B2181BBF326580950494257464F (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___schemeName0, String_t* ___hostName1, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriBuilder__ctor_m7A9B7FFE61632B2181BBF326580950494257464F_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		__this->set__changed_0((bool)1);
		String_t* L_0 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		__this->set__fragment_1(L_0);
		__this->set__host_2(_stringLiteral334389048B872A533002B34D73F8C29FD09EFC50);
		String_t* L_1 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		__this->set__password_3(L_1);
		__this->set__path_4(_stringLiteral42099B4AF021E53FD8FD4E056C2568D7C2E3FFA8);
		__this->set__port_5((-1));
		String_t* L_2 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		__this->set__query_6(L_2);
		__this->set__scheme_7(_stringLiteral77B5F8E343A90F6F597751021FB8B7A08FE83083);
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		String_t* L_3 = ((Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields*)il2cpp_codegen_static_fields_for(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var))->get_SchemeDelimiter_12();
		__this->set__schemeDelimiter_8(L_3);
		String_t* L_4 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		__this->set__username_10(L_4);
		Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0(__this, /*hidden argument*/NULL);
		String_t* L_5 = ___schemeName0;
		UriBuilder_set_Scheme_mD20C10C2D43C0C2C96D9098BE4331D46FCC45921(__this, L_5, /*hidden argument*/NULL);
		String_t* L_6 = ___hostName1;
		UriBuilder_set_Host_m7213BE98F62DE6A099EA8EEFF479949C5F1EA680(__this, L_6, /*hidden argument*/NULL);
		return;
	}
}
// System.Void System.UriBuilder::.ctor(System.String,System.String,System.Int32)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder__ctor_mBF5DB989568C4C36033EAF9AE939734782DC722D (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___scheme0, String_t* ___host1, int32_t ___portNumber2, const RuntimeMethod* method)
{
	{
		String_t* L_0 = ___scheme0;
		String_t* L_1 = ___host1;
		UriBuilder__ctor_m7A9B7FFE61632B2181BBF326580950494257464F(__this, L_0, L_1, /*hidden argument*/NULL);
		int32_t L_2 = ___portNumber2;
		UriBuilder_set_Port_m14DBA6E597BED983B73F4AD7F1215C6E474DB6F3(__this, L_2, /*hidden argument*/NULL);
		return;
	}
}
// System.Void System.UriBuilder::.ctor(System.String,System.String,System.Int32,System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder__ctor_mD85B55F4C0313B5E28C9F10277C2E81CF5650215 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___scheme0, String_t* ___host1, int32_t ___port2, String_t* ___pathValue3, const RuntimeMethod* method)
{
	{
		String_t* L_0 = ___scheme0;
		String_t* L_1 = ___host1;
		int32_t L_2 = ___port2;
		UriBuilder__ctor_mBF5DB989568C4C36033EAF9AE939734782DC722D(__this, L_0, L_1, L_2, /*hidden argument*/NULL);
		String_t* L_3 = ___pathValue3;
		UriBuilder_set_Path_mB5E891CD6B419F1310178B20F5E47E49D0F828E8(__this, L_3, /*hidden argument*/NULL);
		return;
	}
}
// System.Void System.UriBuilder::.ctor(System.String,System.String,System.Int32,System.String,System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder__ctor_m5B1EA7F0F855706B9725EAE9EFE4A3DE5DB97D50 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___scheme0, String_t* ___host1, int32_t ___port2, String_t* ___path3, String_t* ___extraValue4, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriBuilder__ctor_m5B1EA7F0F855706B9725EAE9EFE4A3DE5DB97D50_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	Exception_t * __last_unhandled_exception = 0;
	NO_UNUSED_WARNING (__last_unhandled_exception);
	Exception_t * __exception_local = 0;
	NO_UNUSED_WARNING (__exception_local);
	int32_t __leave_target = -1;
	NO_UNUSED_WARNING (__leave_target);
	{
		String_t* L_0 = ___scheme0;
		String_t* L_1 = ___host1;
		int32_t L_2 = ___port2;
		String_t* L_3 = ___path3;
		UriBuilder__ctor_mD85B55F4C0313B5E28C9F10277C2E81CF5650215(__this, L_0, L_1, L_2, L_3, /*hidden argument*/NULL);
	}

IL_000b:
	try
	{ // begin try (depth: 1)
		String_t* L_4 = ___extraValue4;
		UriBuilder_set_Extra_mBA671DFDA9152422D2C52BD7087F78ACB3A3673C(__this, L_4, /*hidden argument*/NULL);
		goto IL_002e;
	} // end try (depth: 1)
	catch(Il2CppExceptionWrapper& e)
	{
		__exception_local = (Exception_t *)e.ex;
		if(il2cpp_codegen_class_is_assignable_from (Exception_t_il2cpp_TypeInfo_var, il2cpp_codegen_object_class(e.ex)))
			goto CATCH_0015;
		throw e;
	}

CATCH_0015:
	{ // begin catch(System.Exception)
		{
			if (!((OutOfMemoryException_t2DF3EAC178583BD1DEFAAECBEDB2AF1EA86FBFC7 *)IsInstClass((RuntimeObject*)((Exception_t *)__exception_local), OutOfMemoryException_t2DF3EAC178583BD1DEFAAECBEDB2AF1EA86FBFC7_il2cpp_TypeInfo_var)))
			{
				goto IL_001e;
			}
		}

IL_001c:
		{
			IL2CPP_RAISE_MANAGED_EXCEPTION(__exception_local, NULL, UriBuilder__ctor_m5B1EA7F0F855706B9725EAE9EFE4A3DE5DB97D50_RuntimeMethod_var);
		}

IL_001e:
		{
			ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 * L_5 = (ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 *)il2cpp_codegen_object_new(ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1_il2cpp_TypeInfo_var);
			ArgumentException__ctor_m26DC3463C6F3C98BF33EA39598DD2B32F0249CA8(L_5, _stringLiteralE3D9B2CC0C1CB7037696A2D9C2B9B4C1FEF5EB9B, _stringLiteral4321CB5243173998A1F5A7D3F9F1C39DB3F00458, /*hidden argument*/NULL);
			IL2CPP_RAISE_MANAGED_EXCEPTION(L_5, NULL, UriBuilder__ctor_m5B1EA7F0F855706B9725EAE9EFE4A3DE5DB97D50_RuntimeMethod_var);
		}
	} // end catch (depth: 1)

IL_002e:
	{
		return;
	}
}
// System.Void System.UriBuilder::set_Extra(System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_set_Extra_mBA671DFDA9152422D2C52BD7087F78ACB3A3673C (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___value0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriBuilder_set_Extra_mBA671DFDA9152422D2C52BD7087F78ACB3A3673C_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	int32_t V_0 = 0;
	{
		String_t* L_0 = ___value0;
		if (L_0)
		{
			goto IL_000a;
		}
	}
	{
		String_t* L_1 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		___value0 = L_1;
	}

IL_000a:
	{
		String_t* L_2 = ___value0;
		NullCheck(L_2);
		int32_t L_3 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_2, /*hidden argument*/NULL);
		if ((((int32_t)L_3) <= ((int32_t)0)))
		{
			goto IL_007d;
		}
	}
	{
		String_t* L_4 = ___value0;
		NullCheck(L_4);
		Il2CppChar L_5 = String_get_Chars_m14308AC3B95F8C1D9F1D1055B116B37D595F1D96(L_4, 0, /*hidden argument*/NULL);
		if ((!(((uint32_t)L_5) == ((uint32_t)((int32_t)35)))))
		{
			goto IL_002c;
		}
	}
	{
		String_t* L_6 = ___value0;
		NullCheck(L_6);
		String_t* L_7 = String_Substring_m2C4AFF5E79DD8BADFD2DFBCF156BF728FBB8E1AE(L_6, 1, /*hidden argument*/NULL);
		UriBuilder_set_Fragment_m2283E0E13EE8BBA2F4FFA10CDAD1EA2D6771EE11(__this, L_7, /*hidden argument*/NULL);
		return;
	}

IL_002c:
	{
		String_t* L_8 = ___value0;
		NullCheck(L_8);
		Il2CppChar L_9 = String_get_Chars_m14308AC3B95F8C1D9F1D1055B116B37D595F1D96(L_8, 0, /*hidden argument*/NULL);
		if ((!(((uint32_t)L_9) == ((uint32_t)((int32_t)63)))))
		{
			goto IL_006d;
		}
	}
	{
		String_t* L_10 = ___value0;
		NullCheck(L_10);
		int32_t L_11 = String_IndexOf_m2909B8CF585E1BD0C81E11ACA2F48012156FD5BD(L_10, ((int32_t)35), /*hidden argument*/NULL);
		V_0 = L_11;
		int32_t L_12 = V_0;
		if ((!(((uint32_t)L_12) == ((uint32_t)(-1)))))
		{
			goto IL_004d;
		}
	}
	{
		String_t* L_13 = ___value0;
		NullCheck(L_13);
		int32_t L_14 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_13, /*hidden argument*/NULL);
		V_0 = L_14;
		goto IL_005c;
	}

IL_004d:
	{
		String_t* L_15 = ___value0;
		int32_t L_16 = V_0;
		NullCheck(L_15);
		String_t* L_17 = String_Substring_m2C4AFF5E79DD8BADFD2DFBCF156BF728FBB8E1AE(L_15, ((int32_t)il2cpp_codegen_add((int32_t)L_16, (int32_t)1)), /*hidden argument*/NULL);
		UriBuilder_set_Fragment_m2283E0E13EE8BBA2F4FFA10CDAD1EA2D6771EE11(__this, L_17, /*hidden argument*/NULL);
	}

IL_005c:
	{
		String_t* L_18 = ___value0;
		int32_t L_19 = V_0;
		NullCheck(L_18);
		String_t* L_20 = String_Substring_mB593C0A320C683E6E47EFFC0A12B7A465E5E43BB(L_18, 1, ((int32_t)il2cpp_codegen_subtract((int32_t)L_19, (int32_t)1)), /*hidden argument*/NULL);
		UriBuilder_set_Query_m392BC383004E6922D6B53870D2195E7F871FCEC8(__this, L_20, /*hidden argument*/NULL);
		return;
	}

IL_006d:
	{
		ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 * L_21 = (ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 *)il2cpp_codegen_object_new(ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1_il2cpp_TypeInfo_var);
		ArgumentException__ctor_m26DC3463C6F3C98BF33EA39598DD2B32F0249CA8(L_21, _stringLiteralE3D9B2CC0C1CB7037696A2D9C2B9B4C1FEF5EB9B, _stringLiteralF32B67C7E26342AF42EFABC674D441DCA0A281C5, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_21, NULL, UriBuilder_set_Extra_mBA671DFDA9152422D2C52BD7087F78ACB3A3673C_RuntimeMethod_var);
	}

IL_007d:
	{
		String_t* L_22 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		UriBuilder_set_Fragment_m2283E0E13EE8BBA2F4FFA10CDAD1EA2D6771EE11(__this, L_22, /*hidden argument*/NULL);
		String_t* L_23 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		UriBuilder_set_Query_m392BC383004E6922D6B53870D2195E7F871FCEC8(__this, L_23, /*hidden argument*/NULL);
		return;
	}
}
// System.Void System.UriBuilder::set_Fragment(System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_set_Fragment_m2283E0E13EE8BBA2F4FFA10CDAD1EA2D6771EE11 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___value0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriBuilder_set_Fragment_m2283E0E13EE8BBA2F4FFA10CDAD1EA2D6771EE11_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		String_t* L_0 = ___value0;
		if (L_0)
		{
			goto IL_000a;
		}
	}
	{
		String_t* L_1 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		___value0 = L_1;
	}

IL_000a:
	{
		String_t* L_2 = ___value0;
		NullCheck(L_2);
		int32_t L_3 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_2, /*hidden argument*/NULL);
		if ((((int32_t)L_3) <= ((int32_t)0)))
		{
			goto IL_002b;
		}
	}
	{
		String_t* L_4 = ___value0;
		NullCheck(L_4);
		Il2CppChar L_5 = String_get_Chars_m14308AC3B95F8C1D9F1D1055B116B37D595F1D96(L_4, 0, /*hidden argument*/NULL);
		if ((((int32_t)L_5) == ((int32_t)((int32_t)35))))
		{
			goto IL_002b;
		}
	}
	{
		String_t* L_6 = ___value0;
		String_t* L_7 = String_Concat_mB78D0094592718DA6D5DB6C712A9C225631666BE(_stringLiteralD08F88DF745FA7950B104E4A707A31CFCE7B5841, L_6, /*hidden argument*/NULL);
		___value0 = L_7;
	}

IL_002b:
	{
		String_t* L_8 = ___value0;
		__this->set__fragment_1(L_8);
		__this->set__changed_0((bool)1);
		return;
	}
}
// System.Void System.UriBuilder::set_Host(System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_set_Host_m7213BE98F62DE6A099EA8EEFF479949C5F1EA680 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___value0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriBuilder_set_Host_m7213BE98F62DE6A099EA8EEFF479949C5F1EA680_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		String_t* L_0 = ___value0;
		if (L_0)
		{
			goto IL_000a;
		}
	}
	{
		String_t* L_1 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		___value0 = L_1;
	}

IL_000a:
	{
		String_t* L_2 = ___value0;
		__this->set__host_2(L_2);
		String_t* L_3 = __this->get__host_2();
		NullCheck(L_3);
		int32_t L_4 = String_IndexOf_m2909B8CF585E1BD0C81E11ACA2F48012156FD5BD(L_3, ((int32_t)58), /*hidden argument*/NULL);
		if ((((int32_t)L_4) < ((int32_t)0)))
		{
			goto IL_004c;
		}
	}
	{
		String_t* L_5 = __this->get__host_2();
		NullCheck(L_5);
		Il2CppChar L_6 = String_get_Chars_m14308AC3B95F8C1D9F1D1055B116B37D595F1D96(L_5, 0, /*hidden argument*/NULL);
		if ((((int32_t)L_6) == ((int32_t)((int32_t)91))))
		{
			goto IL_004c;
		}
	}
	{
		String_t* L_7 = __this->get__host_2();
		String_t* L_8 = String_Concat_mF4626905368D6558695A823466A1AF65EADB9923(_stringLiteral1E5C2F367F02E47A8C160CDA1CD9D91DECBAC441, L_7, _stringLiteral4FF447B8EF42CA51FA6FB287BED8D40F49BE58F1, /*hidden argument*/NULL);
		__this->set__host_2(L_8);
	}

IL_004c:
	{
		__this->set__changed_0((bool)1);
		return;
	}
}
// System.String System.UriBuilder::get_Path()
extern "C" IL2CPP_METHOD_ATTR String_t* UriBuilder_get_Path_mA25F7788A90683E9C36E0226247BE5B1226B1D56 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, const RuntimeMethod* method)
{
	{
		String_t* L_0 = __this->get__path_4();
		return L_0;
	}
}
// System.Void System.UriBuilder::set_Path(System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_set_Path_mB5E891CD6B419F1310178B20F5E47E49D0F828E8 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___value0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriBuilder_set_Path_mB5E891CD6B419F1310178B20F5E47E49D0F828E8_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		String_t* L_0 = ___value0;
		if (!L_0)
		{
			goto IL_000b;
		}
	}
	{
		String_t* L_1 = ___value0;
		NullCheck(L_1);
		int32_t L_2 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_1, /*hidden argument*/NULL);
		if (L_2)
		{
			goto IL_0012;
		}
	}

IL_000b:
	{
		___value0 = _stringLiteral42099B4AF021E53FD8FD4E056C2568D7C2E3FFA8;
	}

IL_0012:
	{
		String_t* L_3 = ___value0;
		NullCheck(L_3);
		String_t* L_4 = String_Replace_m276641366A463205C185A9B3DC0E24ECB95122C9(L_3, ((int32_t)92), ((int32_t)47), /*hidden argument*/NULL);
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		String_t* L_5 = Uri_InternalEscapeString_m4A79B5EFDD0254232524BA43FAC32297A825F873(L_4, /*hidden argument*/NULL);
		__this->set__path_4(L_5);
		__this->set__changed_0((bool)1);
		return;
	}
}
// System.Void System.UriBuilder::set_Port(System.Int32)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_set_Port_m14DBA6E597BED983B73F4AD7F1215C6E474DB6F3 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, int32_t ___value0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriBuilder_set_Port_m14DBA6E597BED983B73F4AD7F1215C6E474DB6F3_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		int32_t L_0 = ___value0;
		if ((((int32_t)L_0) < ((int32_t)(-1))))
		{
			goto IL_000c;
		}
	}
	{
		int32_t L_1 = ___value0;
		if ((((int32_t)L_1) <= ((int32_t)((int32_t)65535))))
		{
			goto IL_0017;
		}
	}

IL_000c:
	{
		ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA * L_2 = (ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA *)il2cpp_codegen_object_new(ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA_il2cpp_TypeInfo_var);
		ArgumentOutOfRangeException__ctor_m6B36E60C989DC798A8B44556DB35960282B133A6(L_2, _stringLiteralF32B67C7E26342AF42EFABC674D441DCA0A281C5, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_2, NULL, UriBuilder_set_Port_m14DBA6E597BED983B73F4AD7F1215C6E474DB6F3_RuntimeMethod_var);
	}

IL_0017:
	{
		int32_t L_3 = ___value0;
		__this->set__port_5(L_3);
		__this->set__changed_0((bool)1);
		return;
	}
}
// System.Void System.UriBuilder::set_Query(System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_set_Query_m392BC383004E6922D6B53870D2195E7F871FCEC8 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___value0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriBuilder_set_Query_m392BC383004E6922D6B53870D2195E7F871FCEC8_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		String_t* L_0 = ___value0;
		if (L_0)
		{
			goto IL_000a;
		}
	}
	{
		String_t* L_1 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		___value0 = L_1;
	}

IL_000a:
	{
		String_t* L_2 = ___value0;
		NullCheck(L_2);
		int32_t L_3 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_2, /*hidden argument*/NULL);
		if ((((int32_t)L_3) <= ((int32_t)0)))
		{
			goto IL_002b;
		}
	}
	{
		String_t* L_4 = ___value0;
		NullCheck(L_4);
		Il2CppChar L_5 = String_get_Chars_m14308AC3B95F8C1D9F1D1055B116B37D595F1D96(L_4, 0, /*hidden argument*/NULL);
		if ((((int32_t)L_5) == ((int32_t)((int32_t)63))))
		{
			goto IL_002b;
		}
	}
	{
		String_t* L_6 = ___value0;
		String_t* L_7 = String_Concat_mB78D0094592718DA6D5DB6C712A9C225631666BE(_stringLiteral5BAB61EB53176449E25C2C82F172B82CB13FFB9D, L_6, /*hidden argument*/NULL);
		___value0 = L_7;
	}

IL_002b:
	{
		String_t* L_8 = ___value0;
		__this->set__query_6(L_8);
		__this->set__changed_0((bool)1);
		return;
	}
}
// System.Void System.UriBuilder::set_Scheme(System.String)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_set_Scheme_mD20C10C2D43C0C2C96D9098BE4331D46FCC45921 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, String_t* ___value0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriBuilder_set_Scheme_mD20C10C2D43C0C2C96D9098BE4331D46FCC45921_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	int32_t V_0 = 0;
	{
		String_t* L_0 = ___value0;
		if (L_0)
		{
			goto IL_000a;
		}
	}
	{
		String_t* L_1 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		___value0 = L_1;
	}

IL_000a:
	{
		String_t* L_2 = ___value0;
		NullCheck(L_2);
		int32_t L_3 = String_IndexOf_m2909B8CF585E1BD0C81E11ACA2F48012156FD5BD(L_2, ((int32_t)58), /*hidden argument*/NULL);
		V_0 = L_3;
		int32_t L_4 = V_0;
		if ((((int32_t)L_4) == ((int32_t)(-1))))
		{
			goto IL_0021;
		}
	}
	{
		String_t* L_5 = ___value0;
		int32_t L_6 = V_0;
		NullCheck(L_5);
		String_t* L_7 = String_Substring_mB593C0A320C683E6E47EFFC0A12B7A465E5E43BB(L_5, 0, L_6, /*hidden argument*/NULL);
		___value0 = L_7;
	}

IL_0021:
	{
		String_t* L_8 = ___value0;
		NullCheck(L_8);
		int32_t L_9 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_8, /*hidden argument*/NULL);
		if (!L_9)
		{
			goto IL_0049;
		}
	}
	{
		String_t* L_10 = ___value0;
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		bool L_11 = Uri_CheckSchemeName_m351E06F4546E0F84E2DDB286B531F39D379589BC(L_10, /*hidden argument*/NULL);
		if (L_11)
		{
			goto IL_0041;
		}
	}
	{
		ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 * L_12 = (ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1 *)il2cpp_codegen_object_new(ArgumentException_tEDCD16F20A09ECE461C3DA766C16EDA8864057D1_il2cpp_TypeInfo_var);
		ArgumentException__ctor_m26DC3463C6F3C98BF33EA39598DD2B32F0249CA8(L_12, _stringLiteral57E68B8AF3FD3A50C789D0A6C6B204E28654550B, _stringLiteralF32B67C7E26342AF42EFABC674D441DCA0A281C5, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_12, NULL, UriBuilder_set_Scheme_mD20C10C2D43C0C2C96D9098BE4331D46FCC45921_RuntimeMethod_var);
	}

IL_0041:
	{
		String_t* L_13 = ___value0;
		NullCheck(L_13);
		String_t* L_14 = String_ToLowerInvariant_m197BD65B6582DC546FF1BC398161EEFA708F799E(L_13, /*hidden argument*/NULL);
		___value0 = L_14;
	}

IL_0049:
	{
		String_t* L_15 = ___value0;
		__this->set__scheme_7(L_15);
		__this->set__changed_0((bool)1);
		return;
	}
}
// System.Uri System.UriBuilder::get_Uri()
extern "C" IL2CPP_METHOD_ATTR Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * UriBuilder_get_Uri_mDCABA4CD1D05D4B9C4CBA063BC7CA94EE6CCC631 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriBuilder_get_Uri_mDCABA4CD1D05D4B9C4CBA063BC7CA94EE6CCC631_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		bool L_0 = __this->get__changed_0();
		if (!L_0)
		{
			goto IL_002c;
		}
	}
	{
		String_t* L_1 = VirtFuncInvoker0< String_t* >::Invoke(3 /* System.String System.Object::ToString() */, __this);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_2 = (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)il2cpp_codegen_object_new(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		Uri__ctor_mBA69907A1D799CD12ED44B611985B25FE4C626A2(L_2, L_1, /*hidden argument*/NULL);
		__this->set__uri_9(L_2);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_3 = __this->get__uri_9();
		UriBuilder_SetFieldsFromUri_m54B4EB1ACEF01F2B0B11EC81768BB7D56245447F(__this, L_3, /*hidden argument*/NULL);
		__this->set__changed_0((bool)0);
	}

IL_002c:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_4 = __this->get__uri_9();
		return L_4;
	}
}
// System.Boolean System.UriBuilder::Equals(System.Object)
extern "C" IL2CPP_METHOD_ATTR bool UriBuilder_Equals_m370D16A9DCA721B688549EE192A9B79C737FE16D (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, RuntimeObject * ___rparam0, const RuntimeMethod* method)
{
	{
		RuntimeObject * L_0 = ___rparam0;
		if (L_0)
		{
			goto IL_0005;
		}
	}
	{
		return (bool)0;
	}

IL_0005:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_1 = UriBuilder_get_Uri_mDCABA4CD1D05D4B9C4CBA063BC7CA94EE6CCC631(__this, /*hidden argument*/NULL);
		RuntimeObject * L_2 = ___rparam0;
		NullCheck(L_2);
		String_t* L_3 = VirtFuncInvoker0< String_t* >::Invoke(3 /* System.String System.Object::ToString() */, L_2);
		NullCheck(L_1);
		bool L_4 = VirtFuncInvoker1< bool, RuntimeObject * >::Invoke(0 /* System.Boolean System.Object::Equals(System.Object) */, L_1, L_3);
		return L_4;
	}
}
// System.Int32 System.UriBuilder::GetHashCode()
extern "C" IL2CPP_METHOD_ATTR int32_t UriBuilder_GetHashCode_m4FBBBBE01B56EF5DF92C4F5A2BCAF5E86A585BE7 (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, const RuntimeMethod* method)
{
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_0 = UriBuilder_get_Uri_mDCABA4CD1D05D4B9C4CBA063BC7CA94EE6CCC631(__this, /*hidden argument*/NULL);
		NullCheck(L_0);
		int32_t L_1 = VirtFuncInvoker0< int32_t >::Invoke(2 /* System.Int32 System.Object::GetHashCode() */, L_0);
		return L_1;
	}
}
// System.Void System.UriBuilder::SetFieldsFromUri(System.Uri)
extern "C" IL2CPP_METHOD_ATTR void UriBuilder_SetFieldsFromUri_m54B4EB1ACEF01F2B0B11EC81768BB7D56245447F (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___uri0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriBuilder_SetFieldsFromUri_m54B4EB1ACEF01F2B0B11EC81768BB7D56245447F_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	String_t* V_0 = NULL;
	int32_t V_1 = 0;
	UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * G_B2_0 = NULL;
	UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * G_B1_0 = NULL;
	String_t* G_B3_0 = NULL;
	UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * G_B3_1 = NULL;
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_0 = ___uri0;
		NullCheck(L_0);
		String_t* L_1 = Uri_get_Fragment_m111666DD668AC59B9F3C3D3CEEEC7F70F6904D41(L_0, /*hidden argument*/NULL);
		__this->set__fragment_1(L_1);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_2 = ___uri0;
		NullCheck(L_2);
		String_t* L_3 = Uri_get_Query_m3F64514B4DB7C849C8255BA3FE08C6BE983D2D56(L_2, /*hidden argument*/NULL);
		__this->set__query_6(L_3);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_4 = ___uri0;
		NullCheck(L_4);
		String_t* L_5 = Uri_get_Host_m2D942F397A36DBDA5E93452CBD983E0714018151(L_4, /*hidden argument*/NULL);
		__this->set__host_2(L_5);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_6 = ___uri0;
		NullCheck(L_6);
		String_t* L_7 = Uri_get_AbsolutePath_mA9A825E2BBD0A43AD76EB9A9765E29E45FE32F31(L_6, /*hidden argument*/NULL);
		__this->set__path_4(L_7);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_8 = ___uri0;
		NullCheck(L_8);
		int32_t L_9 = Uri_get_Port_m4E64AB9B50CCC50E7B1F139D7AF1403FAF97147C(L_8, /*hidden argument*/NULL);
		__this->set__port_5(L_9);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_10 = ___uri0;
		NullCheck(L_10);
		String_t* L_11 = Uri_get_Scheme_m14A8F0018D8AACADBEF39600A59944F33EE39187(L_10, /*hidden argument*/NULL);
		__this->set__scheme_7(L_11);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_12 = ___uri0;
		NullCheck(L_12);
		bool L_13 = Uri_get_HasAuthority_m969936D80AE0309273733487C8B38BEE33468712(L_12, /*hidden argument*/NULL);
		G_B1_0 = __this;
		if (L_13)
		{
			G_B2_0 = __this;
			goto IL_0058;
		}
	}
	{
		G_B3_0 = _stringLiteral05A79F06CF3F67F726DAE68D18A2290F6C9A50C9;
		G_B3_1 = G_B1_0;
		goto IL_005d;
	}

IL_0058:
	{
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		String_t* L_14 = ((Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields*)il2cpp_codegen_static_fields_for(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var))->get_SchemeDelimiter_12();
		G_B3_0 = L_14;
		G_B3_1 = G_B2_0;
	}

IL_005d:
	{
		NullCheck(G_B3_1);
		G_B3_1->set__schemeDelimiter_8(G_B3_0);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_15 = ___uri0;
		NullCheck(L_15);
		String_t* L_16 = Uri_get_UserInfo_m201C93A932C446805E9143EBE969048D7E75C71E(L_15, /*hidden argument*/NULL);
		V_0 = L_16;
		String_t* L_17 = V_0;
		NullCheck(L_17);
		int32_t L_18 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_17, /*hidden argument*/NULL);
		if ((((int32_t)L_18) <= ((int32_t)0)))
		{
			goto IL_00a4;
		}
	}
	{
		String_t* L_19 = V_0;
		NullCheck(L_19);
		int32_t L_20 = String_IndexOf_m2909B8CF585E1BD0C81E11ACA2F48012156FD5BD(L_19, ((int32_t)58), /*hidden argument*/NULL);
		V_1 = L_20;
		int32_t L_21 = V_1;
		if ((((int32_t)L_21) == ((int32_t)(-1))))
		{
			goto IL_009d;
		}
	}
	{
		String_t* L_22 = V_0;
		int32_t L_23 = V_1;
		NullCheck(L_22);
		String_t* L_24 = String_Substring_m2C4AFF5E79DD8BADFD2DFBCF156BF728FBB8E1AE(L_22, ((int32_t)il2cpp_codegen_add((int32_t)L_23, (int32_t)1)), /*hidden argument*/NULL);
		__this->set__password_3(L_24);
		String_t* L_25 = V_0;
		int32_t L_26 = V_1;
		NullCheck(L_25);
		String_t* L_27 = String_Substring_mB593C0A320C683E6E47EFFC0A12B7A465E5E43BB(L_25, 0, L_26, /*hidden argument*/NULL);
		__this->set__username_10(L_27);
		return;
	}

IL_009d:
	{
		String_t* L_28 = V_0;
		__this->set__username_10(L_28);
	}

IL_00a4:
	{
		return;
	}
}
// System.String System.UriBuilder::ToString()
extern "C" IL2CPP_METHOD_ATTR String_t* UriBuilder_ToString_m5BF9ED358C61008561663680C0D843C22C25443D (UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriBuilder_ToString_m5BF9ED358C61008561663680C0D843C22C25443D_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	String_t* V_0 = NULL;
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * V_1 = NULL;
	UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * G_B10_0 = NULL;
	UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * G_B6_0 = NULL;
	UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * G_B9_0 = NULL;
	UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * G_B7_0 = NULL;
	UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * G_B8_0 = NULL;
	String_t* G_B11_0 = NULL;
	UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * G_B11_1 = NULL;
	UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * G_B14_0 = NULL;
	UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * G_B13_0 = NULL;
	String_t* G_B15_0 = NULL;
	UriBuilder_t5823C3516668F40DA57B8F41E2AF01261B988905 * G_B15_1 = NULL;
	String_t* G_B19_0 = NULL;
	int32_t G_B21_0 = 0;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B21_1 = NULL;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B21_2 = NULL;
	int32_t G_B20_0 = 0;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B20_1 = NULL;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B20_2 = NULL;
	String_t* G_B22_0 = NULL;
	int32_t G_B22_1 = 0;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B22_2 = NULL;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B22_3 = NULL;
	int32_t G_B24_0 = 0;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B24_1 = NULL;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B24_2 = NULL;
	int32_t G_B23_0 = 0;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B23_1 = NULL;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B23_2 = NULL;
	String_t* G_B25_0 = NULL;
	int32_t G_B25_1 = 0;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B25_2 = NULL;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B25_3 = NULL;
	int32_t G_B27_0 = 0;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B27_1 = NULL;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B27_2 = NULL;
	int32_t G_B26_0 = 0;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B26_1 = NULL;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B26_2 = NULL;
	int32_t G_B28_0 = 0;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B28_1 = NULL;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B28_2 = NULL;
	String_t* G_B29_0 = NULL;
	int32_t G_B29_1 = 0;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B29_2 = NULL;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B29_3 = NULL;
	int32_t G_B32_0 = 0;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B32_1 = NULL;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B32_2 = NULL;
	int32_t G_B30_0 = 0;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B30_1 = NULL;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B30_2 = NULL;
	int32_t G_B31_0 = 0;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B31_1 = NULL;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B31_2 = NULL;
	int32_t G_B33_0 = 0;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B33_1 = NULL;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B33_2 = NULL;
	String_t* G_B34_0 = NULL;
	int32_t G_B34_1 = 0;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B34_2 = NULL;
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* G_B34_3 = NULL;
	{
		String_t* L_0 = __this->get__username_10();
		NullCheck(L_0);
		int32_t L_1 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_0, /*hidden argument*/NULL);
		if (L_1)
		{
			goto IL_0026;
		}
	}
	{
		String_t* L_2 = __this->get__password_3();
		NullCheck(L_2);
		int32_t L_3 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_2, /*hidden argument*/NULL);
		if ((((int32_t)L_3) <= ((int32_t)0)))
		{
			goto IL_0026;
		}
	}
	{
		UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A * L_4 = (UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A *)il2cpp_codegen_object_new(UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A_il2cpp_TypeInfo_var);
		UriFormatException__ctor_mE1D46962CC168EB07B59D1265F5734A8F587567D(L_4, _stringLiteral2B2243B6036E7AC7834F59C17B6FBD1E6AB6D2CF, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_4, NULL, UriBuilder_ToString_m5BF9ED358C61008561663680C0D843C22C25443D_RuntimeMethod_var);
	}

IL_0026:
	{
		String_t* L_5 = __this->get__scheme_7();
		NullCheck(L_5);
		int32_t L_6 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_5, /*hidden argument*/NULL);
		if (!L_6)
		{
			goto IL_00a1;
		}
	}
	{
		String_t* L_7 = __this->get__scheme_7();
		IL2CPP_RUNTIME_CLASS_INIT(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_8 = UriParser_GetSyntax_mC2FEAF79ECEB6550573A1C0578141BB236F7EF16(L_7, /*hidden argument*/NULL);
		V_1 = L_8;
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_9 = V_1;
		if (!L_9)
		{
			goto IL_0082;
		}
	}
	{
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_10 = V_1;
		NullCheck(L_10);
		bool L_11 = UriParser_InFact_mDD42FA932B6830D99AA04C2AE7875BA5067C86F3(L_10, 1, /*hidden argument*/NULL);
		G_B6_0 = __this;
		if (L_11)
		{
			G_B10_0 = __this;
			goto IL_0076;
		}
	}
	{
		String_t* L_12 = __this->get__host_2();
		NullCheck(L_12);
		int32_t L_13 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_12, /*hidden argument*/NULL);
		G_B7_0 = G_B6_0;
		if (!L_13)
		{
			G_B9_0 = G_B6_0;
			goto IL_006f;
		}
	}
	{
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_14 = V_1;
		NullCheck(L_14);
		bool L_15 = UriParser_NotAny_mC998A35DC290F35FFAFFB6A8B66C7B881F2559D3(L_14, ((int32_t)16384), /*hidden argument*/NULL);
		G_B8_0 = G_B7_0;
		if (!L_15)
		{
			G_B9_0 = G_B7_0;
			goto IL_006f;
		}
	}
	{
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_16 = V_1;
		NullCheck(L_16);
		bool L_17 = UriParser_InFact_mDD42FA932B6830D99AA04C2AE7875BA5067C86F3(L_16, 2, /*hidden argument*/NULL);
		G_B9_0 = G_B8_0;
		if (L_17)
		{
			G_B10_0 = G_B8_0;
			goto IL_0076;
		}
	}

IL_006f:
	{
		G_B11_0 = _stringLiteral05A79F06CF3F67F726DAE68D18A2290F6C9A50C9;
		G_B11_1 = G_B9_0;
		goto IL_007b;
	}

IL_0076:
	{
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		String_t* L_18 = ((Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields*)il2cpp_codegen_static_fields_for(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var))->get_SchemeDelimiter_12();
		G_B11_0 = L_18;
		G_B11_1 = G_B10_0;
	}

IL_007b:
	{
		NullCheck(G_B11_1);
		G_B11_1->set__schemeDelimiter_8(G_B11_0);
		goto IL_00a1;
	}

IL_0082:
	{
		String_t* L_19 = __this->get__host_2();
		NullCheck(L_19);
		int32_t L_20 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_19, /*hidden argument*/NULL);
		G_B13_0 = __this;
		if (L_20)
		{
			G_B14_0 = __this;
			goto IL_0097;
		}
	}
	{
		G_B15_0 = _stringLiteral05A79F06CF3F67F726DAE68D18A2290F6C9A50C9;
		G_B15_1 = G_B13_0;
		goto IL_009c;
	}

IL_0097:
	{
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		String_t* L_21 = ((Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_StaticFields*)il2cpp_codegen_static_fields_for(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var))->get_SchemeDelimiter_12();
		G_B15_0 = L_21;
		G_B15_1 = G_B14_0;
	}

IL_009c:
	{
		NullCheck(G_B15_1);
		G_B15_1->set__schemeDelimiter_8(G_B15_0);
	}

IL_00a1:
	{
		String_t* L_22 = __this->get__scheme_7();
		NullCheck(L_22);
		int32_t L_23 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_22, /*hidden argument*/NULL);
		if (L_23)
		{
			goto IL_00b5;
		}
	}
	{
		String_t* L_24 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		G_B19_0 = L_24;
		goto IL_00c6;
	}

IL_00b5:
	{
		String_t* L_25 = __this->get__scheme_7();
		String_t* L_26 = __this->get__schemeDelimiter_8();
		String_t* L_27 = String_Concat_mB78D0094592718DA6D5DB6C712A9C225631666BE(L_25, L_26, /*hidden argument*/NULL);
		G_B19_0 = L_27;
	}

IL_00c6:
	{
		V_0 = G_B19_0;
		StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* L_28 = (StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E*)SZArrayNew(StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E_il2cpp_TypeInfo_var, (uint32_t)((int32_t)10));
		StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* L_29 = L_28;
		String_t* L_30 = V_0;
		NullCheck(L_29);
		ArrayElementTypeCheck (L_29, L_30);
		(L_29)->SetAt(static_cast<il2cpp_array_size_t>(0), (String_t*)L_30);
		StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* L_31 = L_29;
		String_t* L_32 = __this->get__username_10();
		NullCheck(L_31);
		ArrayElementTypeCheck (L_31, L_32);
		(L_31)->SetAt(static_cast<il2cpp_array_size_t>(1), (String_t*)L_32);
		StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* L_33 = L_31;
		String_t* L_34 = __this->get__password_3();
		NullCheck(L_34);
		int32_t L_35 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_34, /*hidden argument*/NULL);
		G_B20_0 = 2;
		G_B20_1 = L_33;
		G_B20_2 = L_33;
		if ((((int32_t)L_35) > ((int32_t)0)))
		{
			G_B21_0 = 2;
			G_B21_1 = L_33;
			G_B21_2 = L_33;
			goto IL_00f2;
		}
	}
	{
		String_t* L_36 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		G_B22_0 = L_36;
		G_B22_1 = G_B20_0;
		G_B22_2 = G_B20_1;
		G_B22_3 = G_B20_2;
		goto IL_0102;
	}

IL_00f2:
	{
		String_t* L_37 = __this->get__password_3();
		String_t* L_38 = String_Concat_mB78D0094592718DA6D5DB6C712A9C225631666BE(_stringLiteral05A79F06CF3F67F726DAE68D18A2290F6C9A50C9, L_37, /*hidden argument*/NULL);
		G_B22_0 = L_38;
		G_B22_1 = G_B21_0;
		G_B22_2 = G_B21_1;
		G_B22_3 = G_B21_2;
	}

IL_0102:
	{
		NullCheck(G_B22_2);
		ArrayElementTypeCheck (G_B22_2, G_B22_0);
		(G_B22_2)->SetAt(static_cast<il2cpp_array_size_t>(G_B22_1), (String_t*)G_B22_0);
		StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* L_39 = G_B22_3;
		String_t* L_40 = __this->get__username_10();
		NullCheck(L_40);
		int32_t L_41 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_40, /*hidden argument*/NULL);
		G_B23_0 = 3;
		G_B23_1 = L_39;
		G_B23_2 = L_39;
		if ((((int32_t)L_41) > ((int32_t)0)))
		{
			G_B24_0 = 3;
			G_B24_1 = L_39;
			G_B24_2 = L_39;
			goto IL_011a;
		}
	}
	{
		String_t* L_42 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		G_B25_0 = L_42;
		G_B25_1 = G_B23_0;
		G_B25_2 = G_B23_1;
		G_B25_3 = G_B23_2;
		goto IL_011f;
	}

IL_011a:
	{
		G_B25_0 = _stringLiteral9A78211436F6D425EC38F5C4E02270801F3524F8;
		G_B25_1 = G_B24_0;
		G_B25_2 = G_B24_1;
		G_B25_3 = G_B24_2;
	}

IL_011f:
	{
		NullCheck(G_B25_2);
		ArrayElementTypeCheck (G_B25_2, G_B25_0);
		(G_B25_2)->SetAt(static_cast<il2cpp_array_size_t>(G_B25_1), (String_t*)G_B25_0);
		StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* L_43 = G_B25_3;
		String_t* L_44 = __this->get__host_2();
		NullCheck(L_43);
		ArrayElementTypeCheck (L_43, L_44);
		(L_43)->SetAt(static_cast<il2cpp_array_size_t>(4), (String_t*)L_44);
		StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* L_45 = L_43;
		int32_t L_46 = __this->get__port_5();
		G_B26_0 = 5;
		G_B26_1 = L_45;
		G_B26_2 = L_45;
		if ((((int32_t)L_46) == ((int32_t)(-1))))
		{
			G_B27_0 = 5;
			G_B27_1 = L_45;
			G_B27_2 = L_45;
			goto IL_0142;
		}
	}
	{
		String_t* L_47 = __this->get__host_2();
		NullCheck(L_47);
		int32_t L_48 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_47, /*hidden argument*/NULL);
		G_B27_0 = G_B26_0;
		G_B27_1 = G_B26_1;
		G_B27_2 = G_B26_2;
		if ((((int32_t)L_48) > ((int32_t)0)))
		{
			G_B28_0 = G_B26_0;
			G_B28_1 = G_B26_1;
			G_B28_2 = G_B26_2;
			goto IL_0149;
		}
	}

IL_0142:
	{
		String_t* L_49 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		G_B29_0 = L_49;
		G_B29_1 = G_B27_0;
		G_B29_2 = G_B27_1;
		G_B29_3 = G_B27_2;
		goto IL_015e;
	}

IL_0149:
	{
		int32_t* L_50 = __this->get_address_of__port_5();
		String_t* L_51 = Int32_ToString_m1863896DE712BF97C031D55B12E1583F1982DC02((int32_t*)L_50, /*hidden argument*/NULL);
		String_t* L_52 = String_Concat_mB78D0094592718DA6D5DB6C712A9C225631666BE(_stringLiteral05A79F06CF3F67F726DAE68D18A2290F6C9A50C9, L_51, /*hidden argument*/NULL);
		G_B29_0 = L_52;
		G_B29_1 = G_B28_0;
		G_B29_2 = G_B28_1;
		G_B29_3 = G_B28_2;
	}

IL_015e:
	{
		NullCheck(G_B29_2);
		ArrayElementTypeCheck (G_B29_2, G_B29_0);
		(G_B29_2)->SetAt(static_cast<il2cpp_array_size_t>(G_B29_1), (String_t*)G_B29_0);
		StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* L_53 = G_B29_3;
		String_t* L_54 = __this->get__host_2();
		NullCheck(L_54);
		int32_t L_55 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_54, /*hidden argument*/NULL);
		G_B30_0 = 6;
		G_B30_1 = L_53;
		G_B30_2 = L_53;
		if ((((int32_t)L_55) <= ((int32_t)0)))
		{
			G_B32_0 = 6;
			G_B32_1 = L_53;
			G_B32_2 = L_53;
			goto IL_018c;
		}
	}
	{
		String_t* L_56 = __this->get__path_4();
		NullCheck(L_56);
		int32_t L_57 = String_get_Length_mD48C8A16A5CF1914F330DCE82D9BE15C3BEDD018(L_56, /*hidden argument*/NULL);
		G_B31_0 = G_B30_0;
		G_B31_1 = G_B30_1;
		G_B31_2 = G_B30_2;
		if (!L_57)
		{
			G_B32_0 = G_B30_0;
			G_B32_1 = G_B30_1;
			G_B32_2 = G_B30_2;
			goto IL_018c;
		}
	}
	{
		String_t* L_58 = __this->get__path_4();
		NullCheck(L_58);
		Il2CppChar L_59 = String_get_Chars_m14308AC3B95F8C1D9F1D1055B116B37D595F1D96(L_58, 0, /*hidden argument*/NULL);
		G_B32_0 = G_B31_0;
		G_B32_1 = G_B31_1;
		G_B32_2 = G_B31_2;
		if ((!(((uint32_t)L_59) == ((uint32_t)((int32_t)47)))))
		{
			G_B33_0 = G_B31_0;
			G_B33_1 = G_B31_1;
			G_B33_2 = G_B31_2;
			goto IL_0193;
		}
	}

IL_018c:
	{
		String_t* L_60 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		G_B34_0 = L_60;
		G_B34_1 = G_B32_0;
		G_B34_2 = G_B32_1;
		G_B34_3 = G_B32_2;
		goto IL_0198;
	}

IL_0193:
	{
		G_B34_0 = _stringLiteral42099B4AF021E53FD8FD4E056C2568D7C2E3FFA8;
		G_B34_1 = G_B33_0;
		G_B34_2 = G_B33_1;
		G_B34_3 = G_B33_2;
	}

IL_0198:
	{
		NullCheck(G_B34_2);
		ArrayElementTypeCheck (G_B34_2, G_B34_0);
		(G_B34_2)->SetAt(static_cast<il2cpp_array_size_t>(G_B34_1), (String_t*)G_B34_0);
		StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* L_61 = G_B34_3;
		String_t* L_62 = __this->get__path_4();
		NullCheck(L_61);
		ArrayElementTypeCheck (L_61, L_62);
		(L_61)->SetAt(static_cast<il2cpp_array_size_t>(7), (String_t*)L_62);
		StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* L_63 = L_61;
		String_t* L_64 = __this->get__query_6();
		NullCheck(L_63);
		ArrayElementTypeCheck (L_63, L_64);
		(L_63)->SetAt(static_cast<il2cpp_array_size_t>(8), (String_t*)L_64);
		StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* L_65 = L_63;
		String_t* L_66 = __this->get__fragment_1();
		NullCheck(L_65);
		ArrayElementTypeCheck (L_65, L_66);
		(L_65)->SetAt(static_cast<il2cpp_array_size_t>(((int32_t)9)), (String_t*)L_66);
		String_t* L_67 = String_Concat_m232E857CA5107EA6AC52E7DD7018716C021F237B(L_65, /*hidden argument*/NULL);
		return L_67;
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
// System.Void System.UriFormatException::.ctor()
extern "C" IL2CPP_METHOD_ATTR void UriFormatException__ctor_mBA5F8C423C09F600B1AF895521C892EA356CA424 (UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A * __this, const RuntimeMethod* method)
{
	{
		FormatException__ctor_m6DAD3E32EE0445420B4893EA683425AC3441609B(__this, /*hidden argument*/NULL);
		return;
	}
}
// System.Void System.UriFormatException::.ctor(System.String)
extern "C" IL2CPP_METHOD_ATTR void UriFormatException__ctor_mE1D46962CC168EB07B59D1265F5734A8F587567D (UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A * __this, String_t* ___textString0, const RuntimeMethod* method)
{
	{
		String_t* L_0 = ___textString0;
		FormatException__ctor_m89167FF9884AE20232190FE9286DC50E146A4F14(__this, L_0, /*hidden argument*/NULL);
		return;
	}
}
// System.Void System.UriFormatException::.ctor(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)
extern "C" IL2CPP_METHOD_ATTR void UriFormatException__ctor_mE7F5B073E9F9DB5F22536C54959BEB0D1E7DA1D5 (UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A * __this, SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26 * ___serializationInfo0, StreamingContext_t2CCDC54E0E8D078AF4A50E3A8B921B828A900034  ___streamingContext1, const RuntimeMethod* method)
{
	{
		SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26 * L_0 = ___serializationInfo0;
		StreamingContext_t2CCDC54E0E8D078AF4A50E3A8B921B828A900034  L_1 = ___streamingContext1;
		FormatException__ctor_mDC141C414E24BE865FC8853970BF83C5B8C7676C(__this, L_0, L_1, /*hidden argument*/NULL);
		return;
	}
}
// System.Void System.UriFormatException::System.Runtime.Serialization.ISerializable.GetObjectData(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)
extern "C" IL2CPP_METHOD_ATTR void UriFormatException_System_Runtime_Serialization_ISerializable_GetObjectData_mED4C06AC35B7F94955ECC0D8F00383888C1127DC (UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A * __this, SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26 * ___serializationInfo0, StreamingContext_t2CCDC54E0E8D078AF4A50E3A8B921B828A900034  ___streamingContext1, const RuntimeMethod* method)
{
	{
		SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26 * L_0 = ___serializationInfo0;
		StreamingContext_t2CCDC54E0E8D078AF4A50E3A8B921B828A900034  L_1 = ___streamingContext1;
		Exception_GetObjectData_m76F759ED00FA218FFC522C32626B851FDE849AD6(__this, L_0, L_1, /*hidden argument*/NULL);
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
// System.Char[] System.UriHelper::EscapeString(System.String,System.Int32,System.Int32,System.Char[],System.Int32U26,System.Boolean,System.Char,System.Char,System.Char)
extern "C" IL2CPP_METHOD_ATTR CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* UriHelper_EscapeString_mF0077A016F05127923308DF7E7E99BD7B9837E8B (String_t* ___input0, int32_t ___start1, int32_t ___end2, CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___dest3, int32_t* ___destPos4, bool ___isUriString5, Il2CppChar ___force16, Il2CppChar ___force27, Il2CppChar ___rsvd8, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriHelper_EscapeString_mF0077A016F05127923308DF7E7E99BD7B9837E8B_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	int32_t V_0 = 0;
	int32_t V_1 = 0;
	uint8_t* V_2 = NULL;
	Il2CppChar* V_3 = NULL;
	String_t* V_4 = NULL;
	Il2CppChar V_5 = 0x0;
	int16_t V_6 = 0;
	int16_t V_7 = 0;
	int16_t V_8 = 0;
	int32_t V_9 = 0;
	int32_t G_B35_0 = 0;
	{
		int32_t L_0 = ___end2;
		int32_t L_1 = ___start1;
		if ((((int32_t)((int32_t)il2cpp_codegen_subtract((int32_t)L_0, (int32_t)L_1))) < ((int32_t)((int32_t)65520))))
		{
			goto IL_001a;
		}
	}
	{
		String_t* L_2 = SR_GetString_m3FC710B15474A9B651DA02B303241B6D8B87E2A7(_stringLiteral8313799DB2EC33E29A24C7AA3B2B19EE6B301F73, /*hidden argument*/NULL);
		UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A * L_3 = (UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A *)il2cpp_codegen_object_new(UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A_il2cpp_TypeInfo_var);
		UriFormatException__ctor_mE1D46962CC168EB07B59D1265F5734A8F587567D(L_3, L_2, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_3, NULL, UriHelper_EscapeString_mF0077A016F05127923308DF7E7E99BD7B9837E8B_RuntimeMethod_var);
	}

IL_001a:
	{
		int32_t L_4 = ___start1;
		V_0 = L_4;
		int32_t L_5 = ___start1;
		V_1 = L_5;
		int8_t* L_6 = (int8_t*) alloca((((uintptr_t)((int32_t)160))));
		memset(L_6,0,(((uintptr_t)((int32_t)160))));
		V_2 = (uint8_t*)(L_6);
		String_t* L_7 = ___input0;
		V_4 = L_7;
		String_t* L_8 = V_4;
		V_3 = (Il2CppChar*)(((uintptr_t)L_8));
		Il2CppChar* L_9 = V_3;
		if (!L_9)
		{
			goto IL_0250;
		}
	}
	{
		Il2CppChar* L_10 = V_3;
		int32_t L_11 = RuntimeHelpers_get_OffsetToStringData_mF3B79A906181F1A2734590DA161E2AF183853F8B(/*hidden argument*/NULL);
		V_3 = (Il2CppChar*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_10, (int32_t)L_11));
		goto IL_0250;
	}

IL_0041:
	{
		Il2CppChar* L_12 = V_3;
		int32_t L_13 = V_0;
		int32_t L_14 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_12, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_13)), (int32_t)2)))));
		V_5 = L_14;
		Il2CppChar L_15 = V_5;
		if ((((int32_t)L_15) <= ((int32_t)((int32_t)127))))
		{
			goto IL_0140;
		}
	}
	{
		int32_t L_16 = ___end2;
		int32_t L_17 = V_0;
		IL2CPP_RUNTIME_CLASS_INIT(Math_tFB388E53C7FDC6FCCF9A19ABF5A4E521FBD52E19_il2cpp_TypeInfo_var);
		int32_t L_18 = Math_Min_mC950438198519FB2B0260FCB91220847EE4BB525(((int32_t)il2cpp_codegen_subtract((int32_t)L_16, (int32_t)L_17)), ((int32_t)39), /*hidden argument*/NULL);
		V_6 = (((int16_t)((int16_t)L_18)));
		V_7 = (int16_t)1;
		goto IL_006c;
	}

IL_0065:
	{
		int16_t L_19 = V_7;
		V_7 = (((int16_t)((int16_t)((int32_t)il2cpp_codegen_add((int32_t)L_19, (int32_t)1)))));
	}

IL_006c:
	{
		int16_t L_20 = V_7;
		int16_t L_21 = V_6;
		if ((((int32_t)L_20) >= ((int32_t)L_21)))
		{
			goto IL_0080;
		}
	}
	{
		Il2CppChar* L_22 = V_3;
		int32_t L_23 = V_0;
		int16_t L_24 = V_7;
		int32_t L_25 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_22, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)((int32_t)il2cpp_codegen_add((int32_t)L_23, (int32_t)L_24)))), (int32_t)2)))));
		if ((((int32_t)L_25) > ((int32_t)((int32_t)127))))
		{
			goto IL_0065;
		}
	}

IL_0080:
	{
		Il2CppChar* L_26 = V_3;
		int32_t L_27 = V_0;
		int16_t L_28 = V_7;
		int32_t L_29 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_26, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)((int32_t)il2cpp_codegen_subtract((int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_27, (int32_t)L_28)), (int32_t)1)))), (int32_t)2)))));
		if ((((int32_t)L_29) < ((int32_t)((int32_t)55296))))
		{
			goto IL_00c9;
		}
	}
	{
		Il2CppChar* L_30 = V_3;
		int32_t L_31 = V_0;
		int16_t L_32 = V_7;
		int32_t L_33 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_30, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)((int32_t)il2cpp_codegen_subtract((int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_31, (int32_t)L_32)), (int32_t)1)))), (int32_t)2)))));
		if ((((int32_t)L_33) > ((int32_t)((int32_t)56319))))
		{
			goto IL_00c9;
		}
	}
	{
		int16_t L_34 = V_7;
		if ((((int32_t)L_34) == ((int32_t)1)))
		{
			goto IL_00b2;
		}
	}
	{
		int16_t L_35 = V_7;
		int32_t L_36 = ___end2;
		int32_t L_37 = V_0;
		if ((!(((uint32_t)L_35) == ((uint32_t)((int32_t)il2cpp_codegen_subtract((int32_t)L_36, (int32_t)L_37))))))
		{
			goto IL_00c2;
		}
	}

IL_00b2:
	{
		String_t* L_38 = SR_GetString_m3FC710B15474A9B651DA02B303241B6D8B87E2A7(_stringLiteral2028E589D6BB0C12D880EFA6E4DAB4AF32821B19, /*hidden argument*/NULL);
		UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A * L_39 = (UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A *)il2cpp_codegen_object_new(UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A_il2cpp_TypeInfo_var);
		UriFormatException__ctor_mE1D46962CC168EB07B59D1265F5734A8F587567D(L_39, L_38, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_39, NULL, UriHelper_EscapeString_mF0077A016F05127923308DF7E7E99BD7B9837E8B_RuntimeMethod_var);
	}

IL_00c2:
	{
		int16_t L_40 = V_7;
		V_7 = (((int16_t)((int16_t)((int32_t)il2cpp_codegen_add((int32_t)L_40, (int32_t)1)))));
	}

IL_00c9:
	{
		Il2CppChar* L_41 = V_3;
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_42 = ___dest3;
		int32_t L_43 = V_0;
		int16_t L_44 = V_7;
		int32_t* L_45 = ___destPos4;
		int32_t L_46 = V_1;
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_47 = UriHelper_EnsureDestinationSize_m64F4907D0411AAAD1C05E0AD0D2EB120DCBA9217((Il2CppChar*)(Il2CppChar*)L_41, L_42, L_43, (((int16_t)((int16_t)((int32_t)il2cpp_codegen_multiply((int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)L_44, (int32_t)4)), (int32_t)3))))), (int16_t)((int32_t)480), (int32_t*)L_45, L_46, /*hidden argument*/NULL);
		___dest3 = L_47;
		Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_48 = Encoding_get_UTF8_m67C8652936B681E7BC7505E459E88790E0FF16D9(/*hidden argument*/NULL);
		Il2CppChar* L_49 = V_3;
		int32_t L_50 = V_0;
		int16_t L_51 = V_7;
		uint8_t* L_52 = V_2;
		NullCheck(L_48);
		int32_t L_53 = VirtFuncInvoker4< int32_t, Il2CppChar*, int32_t, uint8_t*, int32_t >::Invoke(20 /* System.Int32 System.Text.Encoding::GetBytes(System.Char*,System.Int32,System.Byte*,System.Int32) */, L_48, (Il2CppChar*)(Il2CppChar*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_49, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_50)), (int32_t)2)))), L_51, (uint8_t*)(uint8_t*)L_52, ((int32_t)160));
		V_8 = (((int16_t)((int16_t)L_53)));
		int16_t L_54 = V_8;
		if (L_54)
		{
			goto IL_0111;
		}
	}
	{
		String_t* L_55 = SR_GetString_m3FC710B15474A9B651DA02B303241B6D8B87E2A7(_stringLiteral2028E589D6BB0C12D880EFA6E4DAB4AF32821B19, /*hidden argument*/NULL);
		UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A * L_56 = (UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A *)il2cpp_codegen_object_new(UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A_il2cpp_TypeInfo_var);
		UriFormatException__ctor_mE1D46962CC168EB07B59D1265F5734A8F587567D(L_56, L_55, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_56, NULL, UriHelper_EscapeString_mF0077A016F05127923308DF7E7E99BD7B9837E8B_RuntimeMethod_var);
	}

IL_0111:
	{
		int32_t L_57 = V_0;
		int16_t L_58 = V_7;
		V_0 = ((int32_t)il2cpp_codegen_add((int32_t)L_57, (int32_t)((int32_t)il2cpp_codegen_subtract((int32_t)L_58, (int32_t)1))));
		V_7 = (int16_t)0;
		goto IL_0131;
	}

IL_011d:
	{
		uint8_t* L_59 = V_2;
		int16_t L_60 = V_7;
		int32_t L_61 = *((uint8_t*)((uint8_t*)il2cpp_codegen_add((intptr_t)L_59, (int32_t)L_60)));
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_62 = ___dest3;
		int32_t* L_63 = ___destPos4;
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		UriHelper_EscapeAsciiChar_mFD7DE796BD53CBD2B1E73080FE0346D37F358902(L_61, L_62, (int32_t*)L_63, /*hidden argument*/NULL);
		int16_t L_64 = V_7;
		V_7 = (((int16_t)((int16_t)((int32_t)il2cpp_codegen_add((int32_t)L_64, (int32_t)1)))));
	}

IL_0131:
	{
		int16_t L_65 = V_7;
		int16_t L_66 = V_8;
		if ((((int32_t)L_65) < ((int32_t)L_66)))
		{
			goto IL_011d;
		}
	}
	{
		int32_t L_67 = V_0;
		V_1 = ((int32_t)il2cpp_codegen_add((int32_t)L_67, (int32_t)1));
		goto IL_024c;
	}

IL_0140:
	{
		Il2CppChar L_68 = V_5;
		if ((!(((uint32_t)L_68) == ((uint32_t)((int32_t)37)))))
		{
			goto IL_01e0;
		}
	}
	{
		Il2CppChar L_69 = ___rsvd8;
		if ((!(((uint32_t)L_69) == ((uint32_t)((int32_t)37)))))
		{
			goto IL_01e0;
		}
	}
	{
		Il2CppChar* L_70 = V_3;
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_71 = ___dest3;
		int32_t L_72 = V_0;
		int32_t* L_73 = ___destPos4;
		int32_t L_74 = V_1;
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_75 = UriHelper_EnsureDestinationSize_m64F4907D0411AAAD1C05E0AD0D2EB120DCBA9217((Il2CppChar*)(Il2CppChar*)L_70, L_71, L_72, (int16_t)3, (int16_t)((int32_t)120), (int32_t*)L_73, L_74, /*hidden argument*/NULL);
		___dest3 = L_75;
		int32_t L_76 = V_0;
		int32_t L_77 = ___end2;
		if ((((int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_76, (int32_t)2))) >= ((int32_t)L_77)))
		{
			goto IL_01d0;
		}
	}
	{
		Il2CppChar* L_78 = V_3;
		int32_t L_79 = V_0;
		int32_t L_80 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_78, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)((int32_t)il2cpp_codegen_add((int32_t)L_79, (int32_t)1)))), (int32_t)2)))));
		Il2CppChar* L_81 = V_3;
		int32_t L_82 = V_0;
		int32_t L_83 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_81, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)((int32_t)il2cpp_codegen_add((int32_t)L_82, (int32_t)2)))), (int32_t)2)))));
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		Il2CppChar L_84 = UriHelper_EscapedAscii_m06D556717795E649EBBB30E4CBCF3D221C1FEB78(L_80, L_83, /*hidden argument*/NULL);
		if ((((int32_t)L_84) == ((int32_t)((int32_t)65535))))
		{
			goto IL_01d0;
		}
	}
	{
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_85 = ___dest3;
		int32_t* L_86 = ___destPos4;
		int32_t* L_87 = ___destPos4;
		int32_t L_88 = *((int32_t*)L_87);
		V_9 = L_88;
		int32_t L_89 = V_9;
		*((int32_t*)L_86) = (int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_89, (int32_t)1));
		int32_t L_90 = V_9;
		NullCheck(L_85);
		(L_85)->SetAt(static_cast<il2cpp_array_size_t>(L_90), (Il2CppChar)((int32_t)37));
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_91 = ___dest3;
		int32_t* L_92 = ___destPos4;
		int32_t* L_93 = ___destPos4;
		int32_t L_94 = *((int32_t*)L_93);
		V_9 = L_94;
		int32_t L_95 = V_9;
		*((int32_t*)L_92) = (int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_95, (int32_t)1));
		int32_t L_96 = V_9;
		Il2CppChar* L_97 = V_3;
		int32_t L_98 = V_0;
		int32_t L_99 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_97, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)((int32_t)il2cpp_codegen_add((int32_t)L_98, (int32_t)1)))), (int32_t)2)))));
		NullCheck(L_91);
		(L_91)->SetAt(static_cast<il2cpp_array_size_t>(L_96), (Il2CppChar)L_99);
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_100 = ___dest3;
		int32_t* L_101 = ___destPos4;
		int32_t* L_102 = ___destPos4;
		int32_t L_103 = *((int32_t*)L_102);
		V_9 = L_103;
		int32_t L_104 = V_9;
		*((int32_t*)L_101) = (int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_104, (int32_t)1));
		int32_t L_105 = V_9;
		Il2CppChar* L_106 = V_3;
		int32_t L_107 = V_0;
		int32_t L_108 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_106, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)((int32_t)il2cpp_codegen_add((int32_t)L_107, (int32_t)2)))), (int32_t)2)))));
		NullCheck(L_100);
		(L_100)->SetAt(static_cast<il2cpp_array_size_t>(L_105), (Il2CppChar)L_108);
		int32_t L_109 = V_0;
		V_0 = ((int32_t)il2cpp_codegen_add((int32_t)L_109, (int32_t)2));
		goto IL_01da;
	}

IL_01d0:
	{
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_110 = ___dest3;
		int32_t* L_111 = ___destPos4;
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		UriHelper_EscapeAsciiChar_mFD7DE796BD53CBD2B1E73080FE0346D37F358902(((int32_t)37), L_110, (int32_t*)L_111, /*hidden argument*/NULL);
	}

IL_01da:
	{
		int32_t L_112 = V_0;
		V_1 = ((int32_t)il2cpp_codegen_add((int32_t)L_112, (int32_t)1));
		goto IL_024c;
	}

IL_01e0:
	{
		Il2CppChar L_113 = V_5;
		Il2CppChar L_114 = ___force16;
		if ((((int32_t)L_113) == ((int32_t)L_114)))
		{
			goto IL_01ec;
		}
	}
	{
		Il2CppChar L_115 = V_5;
		Il2CppChar L_116 = ___force27;
		if ((!(((uint32_t)L_115) == ((uint32_t)L_116))))
		{
			goto IL_020c;
		}
	}

IL_01ec:
	{
		Il2CppChar* L_117 = V_3;
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_118 = ___dest3;
		int32_t L_119 = V_0;
		int32_t* L_120 = ___destPos4;
		int32_t L_121 = V_1;
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_122 = UriHelper_EnsureDestinationSize_m64F4907D0411AAAD1C05E0AD0D2EB120DCBA9217((Il2CppChar*)(Il2CppChar*)L_117, L_118, L_119, (int16_t)3, (int16_t)((int32_t)120), (int32_t*)L_120, L_121, /*hidden argument*/NULL);
		___dest3 = L_122;
		Il2CppChar L_123 = V_5;
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_124 = ___dest3;
		int32_t* L_125 = ___destPos4;
		UriHelper_EscapeAsciiChar_mFD7DE796BD53CBD2B1E73080FE0346D37F358902(L_123, L_124, (int32_t*)L_125, /*hidden argument*/NULL);
		int32_t L_126 = V_0;
		V_1 = ((int32_t)il2cpp_codegen_add((int32_t)L_126, (int32_t)1));
		goto IL_024c;
	}

IL_020c:
	{
		Il2CppChar L_127 = V_5;
		Il2CppChar L_128 = ___rsvd8;
		if ((((int32_t)L_127) == ((int32_t)L_128)))
		{
			goto IL_024c;
		}
	}
	{
		bool L_129 = ___isUriString5;
		if (L_129)
		{
			goto IL_0222;
		}
	}
	{
		Il2CppChar L_130 = V_5;
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		bool L_131 = UriHelper_IsUnreserved_mAADC7DCEEA864AFB49311696ABBDD76811FAAE48(L_130, /*hidden argument*/NULL);
		G_B35_0 = ((((int32_t)L_131) == ((int32_t)0))? 1 : 0);
		goto IL_022c;
	}

IL_0222:
	{
		Il2CppChar L_132 = V_5;
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		bool L_133 = UriHelper_IsReservedUnreservedOrHash_m3D7256DABA7F540F8D379FC1D1C54F1C63E46059(L_132, /*hidden argument*/NULL);
		G_B35_0 = ((((int32_t)L_133) == ((int32_t)0))? 1 : 0);
	}

IL_022c:
	{
		if (!G_B35_0)
		{
			goto IL_024c;
		}
	}
	{
		Il2CppChar* L_134 = V_3;
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_135 = ___dest3;
		int32_t L_136 = V_0;
		int32_t* L_137 = ___destPos4;
		int32_t L_138 = V_1;
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_139 = UriHelper_EnsureDestinationSize_m64F4907D0411AAAD1C05E0AD0D2EB120DCBA9217((Il2CppChar*)(Il2CppChar*)L_134, L_135, L_136, (int16_t)3, (int16_t)((int32_t)120), (int32_t*)L_137, L_138, /*hidden argument*/NULL);
		___dest3 = L_139;
		Il2CppChar L_140 = V_5;
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_141 = ___dest3;
		int32_t* L_142 = ___destPos4;
		UriHelper_EscapeAsciiChar_mFD7DE796BD53CBD2B1E73080FE0346D37F358902(L_140, L_141, (int32_t*)L_142, /*hidden argument*/NULL);
		int32_t L_143 = V_0;
		V_1 = ((int32_t)il2cpp_codegen_add((int32_t)L_143, (int32_t)1));
	}

IL_024c:
	{
		int32_t L_144 = V_0;
		V_0 = ((int32_t)il2cpp_codegen_add((int32_t)L_144, (int32_t)1));
	}

IL_0250:
	{
		int32_t L_145 = V_0;
		int32_t L_146 = ___end2;
		if ((((int32_t)L_145) < ((int32_t)L_146)))
		{
			goto IL_0041;
		}
	}
	{
		int32_t L_147 = V_1;
		int32_t L_148 = V_0;
		if ((((int32_t)L_147) == ((int32_t)L_148)))
		{
			goto IL_0271;
		}
	}
	{
		int32_t L_149 = V_1;
		int32_t L_150 = ___start1;
		if ((!(((uint32_t)L_149) == ((uint32_t)L_150))))
		{
			goto IL_0262;
		}
	}
	{
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_151 = ___dest3;
		if (!L_151)
		{
			goto IL_0271;
		}
	}

IL_0262:
	{
		Il2CppChar* L_152 = V_3;
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_153 = ___dest3;
		int32_t L_154 = V_0;
		int32_t* L_155 = ___destPos4;
		int32_t L_156 = V_1;
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_157 = UriHelper_EnsureDestinationSize_m64F4907D0411AAAD1C05E0AD0D2EB120DCBA9217((Il2CppChar*)(Il2CppChar*)L_152, L_153, L_154, (int16_t)0, (int16_t)0, (int32_t*)L_155, L_156, /*hidden argument*/NULL);
		___dest3 = L_157;
	}

IL_0271:
	{
		V_4 = (String_t*)NULL;
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_158 = ___dest3;
		return L_158;
	}
}
// System.Char[] System.UriHelper::EnsureDestinationSize(System.Char*,System.Char[],System.Int32,System.Int16,System.Int16,System.Int32U26,System.Int32)
extern "C" IL2CPP_METHOD_ATTR CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* UriHelper_EnsureDestinationSize_m64F4907D0411AAAD1C05E0AD0D2EB120DCBA9217 (Il2CppChar* ___pStr0, CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___dest1, int32_t ___currentInputPos2, int16_t ___charsToAdd3, int16_t ___minReallocateChars4, int32_t* ___destPos5, int32_t ___prevInputPos6, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriHelper_EnsureDestinationSize_m64F4907D0411AAAD1C05E0AD0D2EB120DCBA9217_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* V_0 = NULL;
	int32_t V_1 = 0;
	{
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_0 = ___dest1;
		if (!L_0)
		{
			goto IL_0012;
		}
	}
	{
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_1 = ___dest1;
		NullCheck(L_1);
		int32_t* L_2 = ___destPos5;
		int32_t L_3 = *((int32_t*)L_2);
		int32_t L_4 = ___currentInputPos2;
		int32_t L_5 = ___prevInputPos6;
		int16_t L_6 = ___charsToAdd3;
		if ((((int32_t)(((int32_t)((int32_t)(((RuntimeArray *)L_1)->max_length))))) >= ((int32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_3, (int32_t)((int32_t)il2cpp_codegen_subtract((int32_t)L_4, (int32_t)L_5)))), (int32_t)L_6)))))
		{
			goto IL_0058;
		}
	}

IL_0012:
	{
		int32_t* L_7 = ___destPos5;
		int32_t L_8 = *((int32_t*)L_7);
		int32_t L_9 = ___currentInputPos2;
		int32_t L_10 = ___prevInputPos6;
		int16_t L_11 = ___minReallocateChars4;
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_12 = (CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2*)SZArrayNew(CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2_il2cpp_TypeInfo_var, (uint32_t)((int32_t)il2cpp_codegen_add((int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_8, (int32_t)((int32_t)il2cpp_codegen_subtract((int32_t)L_9, (int32_t)L_10)))), (int32_t)L_11)));
		V_0 = L_12;
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_13 = ___dest1;
		if (!L_13)
		{
			goto IL_0039;
		}
	}
	{
		int32_t* L_14 = ___destPos5;
		int32_t L_15 = *((int32_t*)L_14);
		if (!L_15)
		{
			goto IL_0039;
		}
	}
	{
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_16 = ___dest1;
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_17 = V_0;
		int32_t* L_18 = ___destPos5;
		int32_t L_19 = *((int32_t*)L_18);
		Buffer_BlockCopy_m1F882D595976063718AF6E405664FC761924D353((RuntimeArray *)(RuntimeArray *)L_16, 0, (RuntimeArray *)(RuntimeArray *)L_17, 0, ((int32_t)((int32_t)L_19<<(int32_t)1)), /*hidden argument*/NULL);
	}

IL_0039:
	{
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_20 = V_0;
		___dest1 = L_20;
		goto IL_0058;
	}

IL_003e:
	{
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_21 = ___dest1;
		int32_t* L_22 = ___destPos5;
		int32_t* L_23 = ___destPos5;
		int32_t L_24 = *((int32_t*)L_23);
		V_1 = L_24;
		int32_t L_25 = V_1;
		*((int32_t*)L_22) = (int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_25, (int32_t)1));
		int32_t L_26 = V_1;
		Il2CppChar* L_27 = ___pStr0;
		int32_t L_28 = ___prevInputPos6;
		int32_t L_29 = L_28;
		___prevInputPos6 = ((int32_t)il2cpp_codegen_add((int32_t)L_29, (int32_t)1));
		int32_t L_30 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_27, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_29)), (int32_t)2)))));
		NullCheck(L_21);
		(L_21)->SetAt(static_cast<il2cpp_array_size_t>(L_26), (Il2CppChar)L_30);
	}

IL_0058:
	{
		int32_t L_31 = ___prevInputPos6;
		int32_t L_32 = ___currentInputPos2;
		if ((!(((uint32_t)L_31) == ((uint32_t)L_32))))
		{
			goto IL_003e;
		}
	}
	{
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_33 = ___dest1;
		return L_33;
	}
}
// System.Char[] System.UriHelper::UnescapeString(System.String,System.Int32,System.Int32,System.Char[],System.Int32U26,System.Char,System.Char,System.Char,System.UnescapeMode,System.UriParser,System.Boolean)
extern "C" IL2CPP_METHOD_ATTR CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* UriHelper_UnescapeString_mC172F713349E3D22985A92BC4F5B51D0BCEE61AF (String_t* ___input0, int32_t ___start1, int32_t ___end2, CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___dest3, int32_t* ___destPosition4, Il2CppChar ___rsvd15, Il2CppChar ___rsvd26, Il2CppChar ___rsvd37, int32_t ___unescapeMode8, UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___syntax9, bool ___isQuery10, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriHelper_UnescapeString_mC172F713349E3D22985A92BC4F5B51D0BCEE61AF_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	Il2CppChar* V_0 = NULL;
	String_t* V_1 = NULL;
	{
		String_t* L_0 = ___input0;
		V_1 = L_0;
		String_t* L_1 = V_1;
		V_0 = (Il2CppChar*)(((uintptr_t)L_1));
		Il2CppChar* L_2 = V_0;
		if (!L_2)
		{
			goto IL_0010;
		}
	}
	{
		Il2CppChar* L_3 = V_0;
		int32_t L_4 = RuntimeHelpers_get_OffsetToStringData_mF3B79A906181F1A2734590DA161E2AF183853F8B(/*hidden argument*/NULL);
		V_0 = (Il2CppChar*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_3, (int32_t)L_4));
	}

IL_0010:
	{
		Il2CppChar* L_5 = V_0;
		int32_t L_6 = ___start1;
		int32_t L_7 = ___end2;
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_8 = ___dest3;
		int32_t* L_9 = ___destPosition4;
		Il2CppChar L_10 = ___rsvd15;
		Il2CppChar L_11 = ___rsvd26;
		Il2CppChar L_12 = ___rsvd37;
		int32_t L_13 = ___unescapeMode8;
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_14 = ___syntax9;
		bool L_15 = ___isQuery10;
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_16 = UriHelper_UnescapeString_mD4815AEAF34E25D31AA4BB4A76B88055F0A49E89((Il2CppChar*)(Il2CppChar*)L_5, L_6, L_7, L_8, (int32_t*)L_9, L_10, L_11, L_12, L_13, L_14, L_15, /*hidden argument*/NULL);
		return L_16;
	}
}
// System.Char[] System.UriHelper::UnescapeString(System.Char*,System.Int32,System.Int32,System.Char[],System.Int32U26,System.Char,System.Char,System.Char,System.UnescapeMode,System.UriParser,System.Boolean)
extern "C" IL2CPP_METHOD_ATTR CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* UriHelper_UnescapeString_mD4815AEAF34E25D31AA4BB4A76B88055F0A49E89 (Il2CppChar* ___pStr0, int32_t ___start1, int32_t ___end2, CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___dest3, int32_t* ___destPosition4, Il2CppChar ___rsvd15, Il2CppChar ___rsvd26, Il2CppChar ___rsvd37, int32_t ___unescapeMode8, UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * ___syntax9, bool ___isQuery10, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriHelper_UnescapeString_mD4815AEAF34E25D31AA4BB4A76B88055F0A49E89_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* V_0 = NULL;
	uint8_t V_1 = 0x0;
	bool V_2 = false;
	int32_t V_3 = 0;
	bool V_4 = false;
	Il2CppChar* V_5 = NULL;
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* V_6 = NULL;
	int32_t V_7 = 0;
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* V_8 = NULL;
	Il2CppChar V_9 = 0x0;
	int32_t V_10 = 0;
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* V_11 = NULL;
	int32_t V_12 = 0;
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* V_13 = NULL;
	Il2CppChar* V_14 = NULL;
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* V_15 = NULL;
	int32_t V_16 = 0;
	Exception_t * __last_unhandled_exception = 0;
	NO_UNUSED_WARNING (__last_unhandled_exception);
	Exception_t * __exception_local = 0;
	NO_UNUSED_WARNING (__exception_local);
	int32_t __leave_target = -1;
	NO_UNUSED_WARNING (__leave_target);
	int32_t G_B3_0 = 0;
	{
		V_0 = (ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821*)NULL;
		V_1 = (uint8_t)0;
		V_2 = (bool)0;
		int32_t L_0 = ___start1;
		V_3 = L_0;
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_1 = ___syntax9;
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		bool L_2 = Uri_IriParsingStatic_m39FC9677B4B9EFBADF814F2EEA58280F35A1D3E5(L_1, /*hidden argument*/NULL);
		if (!L_2)
		{
			goto IL_001a;
		}
	}
	{
		int32_t L_3 = ___unescapeMode8;
		G_B3_0 = ((((int32_t)((int32_t)((int32_t)L_3&(int32_t)3))) == ((int32_t)3))? 1 : 0);
		goto IL_001b;
	}

IL_001a:
	{
		G_B3_0 = 0;
	}

IL_001b:
	{
		V_4 = (bool)G_B3_0;
	}

IL_001d:
	{
	}

IL_001e:
	try
	{ // begin try (depth: 1)
		{
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_4 = ___dest3;
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_5 = L_4;
			V_6 = L_5;
			if (!L_5)
			{
				goto IL_002a;
			}
		}

IL_0024:
		{
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_6 = V_6;
			NullCheck(L_6);
			if ((((int32_t)((int32_t)(((RuntimeArray *)L_6)->max_length)))))
			{
				goto IL_0030;
			}
		}

IL_002a:
		{
			V_5 = (Il2CppChar*)(((uintptr_t)0));
			goto IL_003b;
		}

IL_0030:
		{
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_7 = V_6;
			NullCheck(L_7);
			V_5 = (Il2CppChar*)(((uintptr_t)((L_7)->GetAddressAt(static_cast<il2cpp_array_size_t>(0)))));
		}

IL_003b:
		{
			int32_t L_8 = ___unescapeMode8;
			if (((int32_t)((int32_t)L_8&(int32_t)3)))
			{
				goto IL_0070;
			}
		}

IL_0041:
		{
			goto IL_0064;
		}

IL_0043:
		{
			Il2CppChar* L_9 = V_5;
			int32_t* L_10 = ___destPosition4;
			int32_t* L_11 = ___destPosition4;
			int32_t L_12 = *((int32_t*)L_11);
			V_7 = L_12;
			int32_t L_13 = V_7;
			*((int32_t*)L_10) = (int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_13, (int32_t)1));
			int32_t L_14 = V_7;
			Il2CppChar* L_15 = ___pStr0;
			int32_t L_16 = ___start1;
			int32_t L_17 = L_16;
			___start1 = ((int32_t)il2cpp_codegen_add((int32_t)L_17, (int32_t)1));
			int32_t L_18 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_15, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_17)), (int32_t)2)))));
			*((int16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_9, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_14)), (int32_t)2))))) = (int16_t)L_18;
		}

IL_0064:
		{
			int32_t L_19 = ___start1;
			int32_t L_20 = ___end2;
			if ((((int32_t)L_19) < ((int32_t)L_20)))
			{
				goto IL_0043;
			}
		}

IL_0068:
		{
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_21 = ___dest3;
			V_8 = L_21;
			IL2CPP_LEAVE(0x39C, FINALLY_0396);
		}

IL_0070:
		{
			V_9 = 0;
			goto IL_01dd;
		}

IL_0078:
		{
			Il2CppChar* L_22 = ___pStr0;
			int32_t L_23 = V_3;
			int32_t L_24 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_22, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_23)), (int32_t)2)))));
			int32_t L_25 = L_24;
			V_9 = L_25;
			if ((!(((uint32_t)L_25) == ((uint32_t)((int32_t)37)))))
			{
				goto IL_0195;
			}
		}

IL_0089:
		{
			int32_t L_26 = ___unescapeMode8;
			if (((int32_t)((int32_t)L_26&(int32_t)2)))
			{
				goto IL_0096;
			}
		}

IL_008f:
		{
			V_2 = (bool)1;
			goto IL_0207;
		}

IL_0096:
		{
			int32_t L_27 = V_3;
			int32_t L_28 = ___end2;
			if ((((int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_27, (int32_t)2))) >= ((int32_t)L_28)))
			{
				goto IL_0176;
			}
		}

IL_009f:
		{
			Il2CppChar* L_29 = ___pStr0;
			int32_t L_30 = V_3;
			int32_t L_31 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_29, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)((int32_t)il2cpp_codegen_add((int32_t)L_30, (int32_t)1)))), (int32_t)2)))));
			Il2CppChar* L_32 = ___pStr0;
			int32_t L_33 = V_3;
			int32_t L_34 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_32, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)((int32_t)il2cpp_codegen_add((int32_t)L_33, (int32_t)2)))), (int32_t)2)))));
			IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
			Il2CppChar L_35 = UriHelper_EscapedAscii_m06D556717795E649EBBB30E4CBCF3D221C1FEB78(L_31, L_34, /*hidden argument*/NULL);
			V_9 = L_35;
			int32_t L_36 = ___unescapeMode8;
			if ((((int32_t)L_36) < ((int32_t)8)))
			{
				goto IL_00e2;
			}
		}

IL_00bd:
		{
			Il2CppChar L_37 = V_9;
			if ((!(((uint32_t)L_37) == ((uint32_t)((int32_t)65535)))))
			{
				goto IL_0207;
			}
		}

IL_00c9:
		{
			int32_t L_38 = ___unescapeMode8;
			if ((((int32_t)L_38) < ((int32_t)((int32_t)24))))
			{
				goto IL_01d9;
			}
		}

IL_00d2:
		{
			String_t* L_39 = SR_GetString_m3FC710B15474A9B651DA02B303241B6D8B87E2A7(_stringLiteral2028E589D6BB0C12D880EFA6E4DAB4AF32821B19, /*hidden argument*/NULL);
			UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A * L_40 = (UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A *)il2cpp_codegen_object_new(UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A_il2cpp_TypeInfo_var);
			UriFormatException__ctor_mE1D46962CC168EB07B59D1265F5734A8F587567D(L_40, L_39, /*hidden argument*/NULL);
			IL2CPP_RAISE_MANAGED_EXCEPTION(L_40, NULL, UriHelper_UnescapeString_mD4815AEAF34E25D31AA4BB4A76B88055F0A49E89_RuntimeMethod_var);
		}

IL_00e2:
		{
			Il2CppChar L_41 = V_9;
			if ((!(((uint32_t)L_41) == ((uint32_t)((int32_t)65535)))))
			{
				goto IL_00fb;
			}
		}

IL_00eb:
		{
			int32_t L_42 = ___unescapeMode8;
			if (!((int32_t)((int32_t)L_42&(int32_t)1)))
			{
				goto IL_01d9;
			}
		}

IL_00f4:
		{
			V_2 = (bool)1;
			goto IL_0207;
		}

IL_00fb:
		{
			Il2CppChar L_43 = V_9;
			if ((!(((uint32_t)L_43) == ((uint32_t)((int32_t)37)))))
			{
				goto IL_010a;
			}
		}

IL_0101:
		{
			int32_t L_44 = V_3;
			V_3 = ((int32_t)il2cpp_codegen_add((int32_t)L_44, (int32_t)2));
			goto IL_01d9;
		}

IL_010a:
		{
			Il2CppChar L_45 = V_9;
			Il2CppChar L_46 = ___rsvd15;
			if ((((int32_t)L_45) == ((int32_t)L_46)))
			{
				goto IL_011c;
			}
		}

IL_0110:
		{
			Il2CppChar L_47 = V_9;
			Il2CppChar L_48 = ___rsvd26;
			if ((((int32_t)L_47) == ((int32_t)L_48)))
			{
				goto IL_011c;
			}
		}

IL_0116:
		{
			Il2CppChar L_49 = V_9;
			Il2CppChar L_50 = ___rsvd37;
			if ((!(((uint32_t)L_49) == ((uint32_t)L_50))))
			{
				goto IL_0125;
			}
		}

IL_011c:
		{
			int32_t L_51 = V_3;
			V_3 = ((int32_t)il2cpp_codegen_add((int32_t)L_51, (int32_t)2));
			goto IL_01d9;
		}

IL_0125:
		{
			int32_t L_52 = ___unescapeMode8;
			if (((int32_t)((int32_t)L_52&(int32_t)4)))
			{
				goto IL_013d;
			}
		}

IL_012b:
		{
			Il2CppChar L_53 = V_9;
			IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
			bool L_54 = UriHelper_IsNotSafeForUnescape_m1D0461E7C5A3CFBD7A2A7F7322B66BC68CCE741D(L_53, /*hidden argument*/NULL);
			if (!L_54)
			{
				goto IL_013d;
			}
		}

IL_0134:
		{
			int32_t L_55 = V_3;
			V_3 = ((int32_t)il2cpp_codegen_add((int32_t)L_55, (int32_t)2));
			goto IL_01d9;
		}

IL_013d:
		{
			bool L_56 = V_4;
			if (!L_56)
			{
				goto IL_0207;
			}
		}

IL_0144:
		{
			Il2CppChar L_57 = V_9;
			if ((((int32_t)L_57) > ((int32_t)((int32_t)159))))
			{
				goto IL_0156;
			}
		}

IL_014d:
		{
			Il2CppChar L_58 = V_9;
			IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
			bool L_59 = UriHelper_IsNotSafeForUnescape_m1D0461E7C5A3CFBD7A2A7F7322B66BC68CCE741D(L_58, /*hidden argument*/NULL);
			if (L_59)
			{
				goto IL_0170;
			}
		}

IL_0156:
		{
			Il2CppChar L_60 = V_9;
			if ((((int32_t)L_60) <= ((int32_t)((int32_t)159))))
			{
				goto IL_0207;
			}
		}

IL_0162:
		{
			Il2CppChar L_61 = V_9;
			bool L_62 = ___isQuery10;
			bool L_63 = IriHelper_CheckIriUnicodeRange_mA9BAAD6D244ADEE8986FDC0DFB3DFDA90C093A6C(L_61, L_62, /*hidden argument*/NULL);
			if (L_63)
			{
				goto IL_0207;
			}
		}

IL_0170:
		{
			int32_t L_64 = V_3;
			V_3 = ((int32_t)il2cpp_codegen_add((int32_t)L_64, (int32_t)2));
			goto IL_01d9;
		}

IL_0176:
		{
			int32_t L_65 = ___unescapeMode8;
			if ((((int32_t)L_65) < ((int32_t)8)))
			{
				goto IL_0191;
			}
		}

IL_017b:
		{
			int32_t L_66 = ___unescapeMode8;
			if ((((int32_t)L_66) < ((int32_t)((int32_t)24))))
			{
				goto IL_01d9;
			}
		}

IL_0181:
		{
			String_t* L_67 = SR_GetString_m3FC710B15474A9B651DA02B303241B6D8B87E2A7(_stringLiteral2028E589D6BB0C12D880EFA6E4DAB4AF32821B19, /*hidden argument*/NULL);
			UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A * L_68 = (UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A *)il2cpp_codegen_object_new(UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A_il2cpp_TypeInfo_var);
			UriFormatException__ctor_mE1D46962CC168EB07B59D1265F5734A8F587567D(L_68, L_67, /*hidden argument*/NULL);
			IL2CPP_RAISE_MANAGED_EXCEPTION(L_68, NULL, UriHelper_UnescapeString_mD4815AEAF34E25D31AA4BB4A76B88055F0A49E89_RuntimeMethod_var);
		}

IL_0191:
		{
			V_2 = (bool)1;
			goto IL_0207;
		}

IL_0195:
		{
			int32_t L_69 = ___unescapeMode8;
			if ((((int32_t)((int32_t)((int32_t)L_69&(int32_t)((int32_t)10)))) == ((int32_t)((int32_t)10))))
			{
				goto IL_01d9;
			}
		}

IL_019e:
		{
			int32_t L_70 = ___unescapeMode8;
			if (!((int32_t)((int32_t)L_70&(int32_t)1)))
			{
				goto IL_01d9;
			}
		}

IL_01a4:
		{
			Il2CppChar L_71 = V_9;
			Il2CppChar L_72 = ___rsvd15;
			if ((((int32_t)L_71) == ((int32_t)L_72)))
			{
				goto IL_01b6;
			}
		}

IL_01aa:
		{
			Il2CppChar L_73 = V_9;
			Il2CppChar L_74 = ___rsvd26;
			if ((((int32_t)L_73) == ((int32_t)L_74)))
			{
				goto IL_01b6;
			}
		}

IL_01b0:
		{
			Il2CppChar L_75 = V_9;
			Il2CppChar L_76 = ___rsvd37;
			if ((!(((uint32_t)L_75) == ((uint32_t)L_76))))
			{
				goto IL_01ba;
			}
		}

IL_01b6:
		{
			V_2 = (bool)1;
			goto IL_0207;
		}

IL_01ba:
		{
			int32_t L_77 = ___unescapeMode8;
			if (((int32_t)((int32_t)L_77&(int32_t)4)))
			{
				goto IL_01d9;
			}
		}

IL_01c0:
		{
			Il2CppChar L_78 = V_9;
			if ((((int32_t)L_78) <= ((int32_t)((int32_t)31))))
			{
				goto IL_01d5;
			}
		}

IL_01c6:
		{
			Il2CppChar L_79 = V_9;
			if ((((int32_t)L_79) < ((int32_t)((int32_t)127))))
			{
				goto IL_01d9;
			}
		}

IL_01cc:
		{
			Il2CppChar L_80 = V_9;
			if ((((int32_t)L_80) > ((int32_t)((int32_t)159))))
			{
				goto IL_01d9;
			}
		}

IL_01d5:
		{
			V_2 = (bool)1;
			goto IL_0207;
		}

IL_01d9:
		{
			int32_t L_81 = V_3;
			V_3 = ((int32_t)il2cpp_codegen_add((int32_t)L_81, (int32_t)1));
		}

IL_01dd:
		{
			int32_t L_82 = V_3;
			int32_t L_83 = ___end2;
			if ((((int32_t)L_82) < ((int32_t)L_83)))
			{
				goto IL_0078;
			}
		}

IL_01e4:
		{
			goto IL_0207;
		}

IL_01e6:
		{
			Il2CppChar* L_84 = V_5;
			int32_t* L_85 = ___destPosition4;
			int32_t* L_86 = ___destPosition4;
			int32_t L_87 = *((int32_t*)L_86);
			V_7 = L_87;
			int32_t L_88 = V_7;
			*((int32_t*)L_85) = (int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_88, (int32_t)1));
			int32_t L_89 = V_7;
			Il2CppChar* L_90 = ___pStr0;
			int32_t L_91 = ___start1;
			int32_t L_92 = L_91;
			___start1 = ((int32_t)il2cpp_codegen_add((int32_t)L_92, (int32_t)1));
			int32_t L_93 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_90, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_92)), (int32_t)2)))));
			*((int16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_84, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_89)), (int32_t)2))))) = (int16_t)L_93;
		}

IL_0207:
		{
			int32_t L_94 = ___start1;
			int32_t L_95 = V_3;
			if ((((int32_t)L_94) < ((int32_t)L_95)))
			{
				goto IL_01e6;
			}
		}

IL_020b:
		{
			int32_t L_96 = V_3;
			int32_t L_97 = ___end2;
			if ((((int32_t)L_96) == ((int32_t)L_97)))
			{
				goto IL_038d;
			}
		}

IL_0212:
		{
			bool L_98 = V_2;
			if (!L_98)
			{
				goto IL_029c;
			}
		}

IL_0218:
		{
			uint8_t L_99 = V_1;
			if (L_99)
			{
				goto IL_027a;
			}
		}

IL_021b:
		{
			V_1 = (uint8_t)((int32_t)30);
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_100 = ___dest3;
			NullCheck(L_100);
			uint8_t L_101 = V_1;
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_102 = (CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2*)SZArrayNew(CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2_il2cpp_TypeInfo_var, (uint32_t)((int32_t)il2cpp_codegen_add((int32_t)(((int32_t)((int32_t)(((RuntimeArray *)L_100)->max_length)))), (int32_t)((int32_t)il2cpp_codegen_multiply((int32_t)L_101, (int32_t)3)))));
			V_13 = L_102;
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_103 = V_13;
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_104 = L_103;
			V_15 = L_104;
			if (!L_104)
			{
				goto IL_0239;
			}
		}

IL_0233:
		{
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_105 = V_15;
			NullCheck(L_105);
			if ((((int32_t)((int32_t)(((RuntimeArray *)L_105)->max_length)))))
			{
				goto IL_023f;
			}
		}

IL_0239:
		{
			V_14 = (Il2CppChar*)(((uintptr_t)0));
			goto IL_024a;
		}

IL_023f:
		{
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_106 = V_15;
			NullCheck(L_106);
			V_14 = (Il2CppChar*)(((uintptr_t)((L_106)->GetAddressAt(static_cast<il2cpp_array_size_t>(0)))));
		}

IL_024a:
		{
			V_16 = 0;
			goto IL_0267;
		}

IL_024f:
		{
			Il2CppChar* L_107 = V_14;
			int32_t L_108 = V_16;
			Il2CppChar* L_109 = V_5;
			int32_t L_110 = V_16;
			int32_t L_111 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_109, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_110)), (int32_t)2)))));
			*((int16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_107, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_108)), (int32_t)2))))) = (int16_t)L_111;
			int32_t L_112 = V_16;
			V_16 = ((int32_t)il2cpp_codegen_add((int32_t)L_112, (int32_t)1));
		}

IL_0267:
		{
			int32_t L_113 = V_16;
			int32_t* L_114 = ___destPosition4;
			int32_t L_115 = *((int32_t*)L_114);
			if ((((int32_t)L_113) < ((int32_t)L_115)))
			{
				goto IL_024f;
			}
		}

IL_026e:
		{
			V_15 = (CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2*)NULL;
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_116 = V_13;
			___dest3 = L_116;
			IL2CPP_LEAVE(0x1D, FINALLY_0396);
		}

IL_027a:
		{
			uint8_t L_117 = V_1;
			V_1 = (uint8_t)(((int32_t)((uint8_t)((int32_t)il2cpp_codegen_subtract((int32_t)L_117, (int32_t)1)))));
			Il2CppChar* L_118 = ___pStr0;
			int32_t L_119 = V_3;
			int32_t L_120 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_118, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_119)), (int32_t)2)))));
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_121 = ___dest3;
			int32_t* L_122 = ___destPosition4;
			IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
			UriHelper_EscapeAsciiChar_mFD7DE796BD53CBD2B1E73080FE0346D37F358902(L_120, L_121, (int32_t*)L_122, /*hidden argument*/NULL);
			V_2 = (bool)0;
			int32_t L_123 = V_3;
			int32_t L_124 = ((int32_t)il2cpp_codegen_add((int32_t)L_123, (int32_t)1));
			V_3 = L_124;
			___start1 = L_124;
			goto IL_0070;
		}

IL_029c:
		{
			Il2CppChar L_125 = V_9;
			if ((((int32_t)L_125) > ((int32_t)((int32_t)127))))
			{
				goto IL_02c0;
			}
		}

IL_02a2:
		{
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_126 = ___dest3;
			int32_t* L_127 = ___destPosition4;
			int32_t* L_128 = ___destPosition4;
			int32_t L_129 = *((int32_t*)L_128);
			V_7 = L_129;
			int32_t L_130 = V_7;
			*((int32_t*)L_127) = (int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_130, (int32_t)1));
			int32_t L_131 = V_7;
			Il2CppChar L_132 = V_9;
			NullCheck(L_126);
			(L_126)->SetAt(static_cast<il2cpp_array_size_t>(L_131), (Il2CppChar)L_132);
			int32_t L_133 = V_3;
			V_3 = ((int32_t)il2cpp_codegen_add((int32_t)L_133, (int32_t)3));
			int32_t L_134 = V_3;
			___start1 = L_134;
			goto IL_0070;
		}

IL_02c0:
		{
			V_10 = 1;
			ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_135 = V_0;
			if (L_135)
			{
				goto IL_02cf;
			}
		}

IL_02c6:
		{
			int32_t L_136 = ___end2;
			int32_t L_137 = V_3;
			ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_138 = (ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821*)SZArrayNew(ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821_il2cpp_TypeInfo_var, (uint32_t)((int32_t)il2cpp_codegen_subtract((int32_t)L_136, (int32_t)L_137)));
			V_0 = L_138;
		}

IL_02cf:
		{
			ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_139 = V_0;
			Il2CppChar L_140 = V_9;
			NullCheck(L_139);
			(L_139)->SetAt(static_cast<il2cpp_array_size_t>(0), (uint8_t)(((int32_t)((uint8_t)L_140))));
			int32_t L_141 = V_3;
			V_3 = ((int32_t)il2cpp_codegen_add((int32_t)L_141, (int32_t)3));
			goto IL_032a;
		}

IL_02db:
		{
			Il2CppChar* L_142 = ___pStr0;
			int32_t L_143 = V_3;
			int32_t L_144 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_142, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_143)), (int32_t)2)))));
			int32_t L_145 = L_144;
			V_9 = L_145;
			if ((!(((uint32_t)L_145) == ((uint32_t)((int32_t)37)))))
			{
				goto IL_032e;
			}
		}

IL_02e9:
		{
			int32_t L_146 = V_3;
			int32_t L_147 = ___end2;
			if ((((int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_146, (int32_t)2))) >= ((int32_t)L_147)))
			{
				goto IL_032e;
			}
		}

IL_02ef:
		{
			Il2CppChar* L_148 = ___pStr0;
			int32_t L_149 = V_3;
			int32_t L_150 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_148, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)((int32_t)il2cpp_codegen_add((int32_t)L_149, (int32_t)1)))), (int32_t)2)))));
			Il2CppChar* L_151 = ___pStr0;
			int32_t L_152 = V_3;
			int32_t L_153 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_151, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)((int32_t)il2cpp_codegen_add((int32_t)L_152, (int32_t)2)))), (int32_t)2)))));
			IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
			Il2CppChar L_154 = UriHelper_EscapedAscii_m06D556717795E649EBBB30E4CBCF3D221C1FEB78(L_150, L_153, /*hidden argument*/NULL);
			V_9 = L_154;
			Il2CppChar L_155 = V_9;
			if ((((int32_t)L_155) == ((int32_t)((int32_t)65535))))
			{
				goto IL_032e;
			}
		}

IL_0311:
		{
			Il2CppChar L_156 = V_9;
			if ((((int32_t)L_156) < ((int32_t)((int32_t)128))))
			{
				goto IL_032e;
			}
		}

IL_031a:
		{
			ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_157 = V_0;
			int32_t L_158 = V_10;
			int32_t L_159 = L_158;
			V_10 = ((int32_t)il2cpp_codegen_add((int32_t)L_159, (int32_t)1));
			Il2CppChar L_160 = V_9;
			NullCheck(L_157);
			(L_157)->SetAt(static_cast<il2cpp_array_size_t>(L_159), (uint8_t)(((int32_t)((uint8_t)L_160))));
			int32_t L_161 = V_3;
			V_3 = ((int32_t)il2cpp_codegen_add((int32_t)L_161, (int32_t)3));
		}

IL_032a:
		{
			int32_t L_162 = V_3;
			int32_t L_163 = ___end2;
			if ((((int32_t)L_162) < ((int32_t)L_163)))
			{
				goto IL_02db;
			}
		}

IL_032e:
		{
			Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_164 = Encoding_get_UTF8_m67C8652936B681E7BC7505E459E88790E0FF16D9(/*hidden argument*/NULL);
			NullCheck(L_164);
			RuntimeObject * L_165 = VirtFuncInvoker0< RuntimeObject * >::Invoke(9 /* System.Object System.Text.Encoding::Clone() */, L_164);
			Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_166 = ((Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 *)CastclassClass((RuntimeObject*)L_165, Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4_il2cpp_TypeInfo_var));
			EncoderReplacementFallback_tC2E8A94C82BBF7A4CFC8E3FDBA8A381DCF29F998 * L_167 = (EncoderReplacementFallback_tC2E8A94C82BBF7A4CFC8E3FDBA8A381DCF29F998 *)il2cpp_codegen_object_new(EncoderReplacementFallback_tC2E8A94C82BBF7A4CFC8E3FDBA8A381DCF29F998_il2cpp_TypeInfo_var);
			EncoderReplacementFallback__ctor_mAE97C6B5EF9A81A90315A21E68271FAE87A738FD(L_167, _stringLiteralDA39A3EE5E6B4B0D3255BFEF95601890AFD80709, /*hidden argument*/NULL);
			NullCheck(L_166);
			Encoding_set_EncoderFallback_m24306F093457AE12D59A36AB84F1E03C840BD10A(L_166, L_167, /*hidden argument*/NULL);
			Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_168 = L_166;
			DecoderReplacementFallback_t8CF74B2DAE2A08AEA7DF6366778D2E3EA75FC742 * L_169 = (DecoderReplacementFallback_t8CF74B2DAE2A08AEA7DF6366778D2E3EA75FC742 *)il2cpp_codegen_object_new(DecoderReplacementFallback_t8CF74B2DAE2A08AEA7DF6366778D2E3EA75FC742_il2cpp_TypeInfo_var);
			DecoderReplacementFallback__ctor_m9D82FC93423AD9B954F28E30B20BF14DAFB01A5B(L_169, _stringLiteralDA39A3EE5E6B4B0D3255BFEF95601890AFD80709, /*hidden argument*/NULL);
			NullCheck(L_168);
			Encoding_set_DecoderFallback_mB321EB8D6C34B8935A169C0E4FAC7A4E0A99FACC(L_168, L_169, /*hidden argument*/NULL);
			ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_170 = V_0;
			NullCheck(L_170);
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_171 = (CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2*)SZArrayNew(CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2_il2cpp_TypeInfo_var, (uint32_t)(((int32_t)((int32_t)(((RuntimeArray *)L_170)->max_length)))));
			V_11 = L_171;
			ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_172 = V_0;
			int32_t L_173 = V_10;
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_174 = V_11;
			NullCheck(L_168);
			int32_t L_175 = VirtFuncInvoker5< int32_t, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821*, int32_t, int32_t, CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2*, int32_t >::Invoke(26 /* System.Int32 System.Text.Encoding::GetChars(System.Byte[],System.Int32,System.Int32,System.Char[],System.Int32) */, L_168, L_172, 0, L_173, L_174, 0);
			V_12 = L_175;
			int32_t L_176 = V_3;
			___start1 = L_176;
			Il2CppChar* L_177 = V_5;
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_178 = ___dest3;
			int32_t* L_179 = ___destPosition4;
			CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_180 = V_11;
			int32_t L_181 = V_12;
			ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_182 = V_0;
			int32_t L_183 = V_10;
			bool L_184 = ___isQuery10;
			bool L_185 = V_4;
			IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
			UriHelper_MatchUTF8Sequence_m4835D9BB77C2701643B14D6FFD3D7057F8C9007F((Il2CppChar*)(Il2CppChar*)L_177, L_178, (int32_t*)L_179, L_180, L_181, L_182, L_183, L_184, L_185, /*hidden argument*/NULL);
		}

IL_038d:
		{
			int32_t L_186 = V_3;
			int32_t L_187 = ___end2;
			if ((!(((uint32_t)L_186) == ((uint32_t)L_187))))
			{
				goto IL_0070;
			}
		}

IL_0394:
		{
			IL2CPP_LEAVE(0x39A, FINALLY_0396);
		}
	} // end try (depth: 1)
	catch(Il2CppExceptionWrapper& e)
	{
		__last_unhandled_exception = (Exception_t *)e.ex;
		goto FINALLY_0396;
	}

FINALLY_0396:
	{ // begin finally (depth: 1)
		V_6 = (CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2*)NULL;
		IL2CPP_END_FINALLY(918)
	} // end finally (depth: 1)
	IL2CPP_CLEANUP(918)
	{
		IL2CPP_JUMP_TBL(0x39C, IL_039c)
		IL2CPP_JUMP_TBL(0x1D, IL_001d)
		IL2CPP_JUMP_TBL(0x39A, IL_039a)
		IL2CPP_RETHROW_IF_UNHANDLED(Exception_t *)
	}

IL_039a:
	{
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_188 = ___dest3;
		return L_188;
	}

IL_039c:
	{
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_189 = V_8;
		return L_189;
	}
}
// System.Void System.UriHelper::MatchUTF8Sequence(System.Char*,System.Char[],System.Int32U26,System.Char[],System.Int32,System.Byte[],System.Int32,System.Boolean,System.Boolean)
extern "C" IL2CPP_METHOD_ATTR void UriHelper_MatchUTF8Sequence_m4835D9BB77C2701643B14D6FFD3D7057F8C9007F (Il2CppChar* ___pDest0, CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___dest1, int32_t* ___destOffset2, CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___unescapedChars3, int32_t ___charCount4, ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* ___bytes5, int32_t ___byteCount6, bool ___isQuery7, bool ___iriParsing8, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriHelper_MatchUTF8Sequence_m4835D9BB77C2701643B14D6FFD3D7057F8C9007F_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	int32_t V_0 = 0;
	Il2CppChar* V_1 = NULL;
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* V_2 = NULL;
	int32_t V_3 = 0;
	bool V_4 = false;
	ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* V_5 = NULL;
	int32_t V_6 = 0;
	bool V_7 = false;
	bool V_8 = false;
	bool V_9 = false;
	int32_t V_10 = 0;
	int32_t V_11 = 0;
	int32_t V_12 = 0;
	int32_t V_13 = 0;
	int32_t G_B7_0 = 0;
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* G_B7_1 = NULL;
	Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * G_B7_2 = NULL;
	int32_t G_B6_0 = 0;
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* G_B6_1 = NULL;
	Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * G_B6_2 = NULL;
	int32_t G_B8_0 = 0;
	int32_t G_B8_1 = 0;
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* G_B8_2 = NULL;
	Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * G_B8_3 = NULL;
	{
		V_0 = 0;
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_0 = ___unescapedChars3;
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_1 = L_0;
		V_2 = L_1;
		if (!L_1)
		{
			goto IL_000c;
		}
	}
	{
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_2 = V_2;
		NullCheck(L_2);
		if ((((int32_t)((int32_t)(((RuntimeArray *)L_2)->max_length)))))
		{
			goto IL_0011;
		}
	}

IL_000c:
	{
		V_1 = (Il2CppChar*)(((uintptr_t)0));
		goto IL_001a;
	}

IL_0011:
	{
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_3 = V_2;
		NullCheck(L_3);
		V_1 = (Il2CppChar*)(((uintptr_t)((L_3)->GetAddressAt(static_cast<il2cpp_array_size_t>(0)))));
	}

IL_001a:
	{
		V_3 = 0;
		goto IL_01aa;
	}

IL_0021:
	{
		Il2CppChar* L_4 = V_1;
		int32_t L_5 = V_3;
		int32_t L_6 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_4, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_5)), (int32_t)2)))));
		IL2CPP_RUNTIME_CLASS_INIT(Char_tBF22D9FC341BE970735250BB6FF1A4A92BBA58B9_il2cpp_TypeInfo_var);
		bool L_7 = Char_IsHighSurrogate_m64C60C09A8561520E43C8527D3DC38FF97E6274D(L_6, /*hidden argument*/NULL);
		V_4 = L_7;
		Encoding_t7837A3C0F55EAE0E3959A53C6D6E88B113ED78A4 * L_8 = Encoding_get_UTF8_m67C8652936B681E7BC7505E459E88790E0FF16D9(/*hidden argument*/NULL);
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_9 = ___unescapedChars3;
		int32_t L_10 = V_3;
		bool L_11 = V_4;
		G_B6_0 = L_10;
		G_B6_1 = L_9;
		G_B6_2 = L_8;
		if (L_11)
		{
			G_B7_0 = L_10;
			G_B7_1 = L_9;
			G_B7_2 = L_8;
			goto IL_003d;
		}
	}
	{
		G_B8_0 = 1;
		G_B8_1 = G_B6_0;
		G_B8_2 = G_B6_1;
		G_B8_3 = G_B6_2;
		goto IL_003e;
	}

IL_003d:
	{
		G_B8_0 = 2;
		G_B8_1 = G_B7_0;
		G_B8_2 = G_B7_1;
		G_B8_3 = G_B7_2;
	}

IL_003e:
	{
		NullCheck(G_B8_3);
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_12 = VirtFuncInvoker3< ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821*, CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2*, int32_t, int32_t >::Invoke(15 /* System.Byte[] System.Text.Encoding::GetBytes(System.Char[],System.Int32,System.Int32) */, G_B8_3, G_B8_2, G_B8_1, G_B8_0);
		V_5 = L_12;
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_13 = V_5;
		NullCheck(L_13);
		V_6 = (((int32_t)((int32_t)(((RuntimeArray *)L_13)->max_length))));
		V_7 = (bool)0;
		bool L_14 = ___iriParsing8;
		if (!L_14)
		{
			goto IL_008b;
		}
	}
	{
		bool L_15 = V_4;
		if (L_15)
		{
			goto IL_0064;
		}
	}
	{
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_16 = ___unescapedChars3;
		int32_t L_17 = V_3;
		NullCheck(L_16);
		int32_t L_18 = L_17;
		uint16_t L_19 = (uint16_t)(L_16)->GetAt(static_cast<il2cpp_array_size_t>(L_18));
		bool L_20 = ___isQuery7;
		bool L_21 = IriHelper_CheckIriUnicodeRange_mA9BAAD6D244ADEE8986FDC0DFB3DFDA90C093A6C(L_19, L_20, /*hidden argument*/NULL);
		V_7 = L_21;
		goto IL_008b;
	}

IL_0064:
	{
		V_8 = (bool)0;
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_22 = ___unescapedChars3;
		int32_t L_23 = V_3;
		NullCheck(L_22);
		int32_t L_24 = L_23;
		uint16_t L_25 = (uint16_t)(L_22)->GetAt(static_cast<il2cpp_array_size_t>(L_24));
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_26 = ___unescapedChars3;
		int32_t L_27 = V_3;
		NullCheck(L_26);
		int32_t L_28 = ((int32_t)il2cpp_codegen_add((int32_t)L_27, (int32_t)1));
		uint16_t L_29 = (uint16_t)(L_26)->GetAt(static_cast<il2cpp_array_size_t>(L_28));
		bool L_30 = ___isQuery7;
		bool L_31 = IriHelper_CheckIriUnicodeRange_m5ED29083C22062AEAB8B5787C9A27CFEEC397AD9(L_25, L_29, (bool*)(&V_8), L_30, /*hidden argument*/NULL);
		V_7 = L_31;
		goto IL_008b;
	}

IL_007c:
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_32 = ___bytes5;
		int32_t L_33 = V_0;
		int32_t L_34 = L_33;
		V_0 = ((int32_t)il2cpp_codegen_add((int32_t)L_34, (int32_t)1));
		NullCheck(L_32);
		int32_t L_35 = L_34;
		uint8_t L_36 = (L_32)->GetAt(static_cast<il2cpp_array_size_t>(L_35));
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_37 = ___dest1;
		int32_t* L_38 = ___destOffset2;
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		UriHelper_EscapeAsciiChar_mFD7DE796BD53CBD2B1E73080FE0346D37F358902(L_36, L_37, (int32_t*)L_38, /*hidden argument*/NULL);
	}

IL_008b:
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_39 = ___bytes5;
		int32_t L_40 = V_0;
		NullCheck(L_39);
		int32_t L_41 = L_40;
		uint8_t L_42 = (L_39)->GetAt(static_cast<il2cpp_array_size_t>(L_41));
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_43 = V_5;
		NullCheck(L_43);
		int32_t L_44 = 0;
		uint8_t L_45 = (L_43)->GetAt(static_cast<il2cpp_array_size_t>(L_44));
		if ((!(((uint32_t)L_42) == ((uint32_t)L_45))))
		{
			goto IL_007c;
		}
	}
	{
		V_9 = (bool)1;
		V_10 = 0;
		goto IL_00b6;
	}

IL_009d:
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_46 = ___bytes5;
		int32_t L_47 = V_0;
		int32_t L_48 = V_10;
		NullCheck(L_46);
		int32_t L_49 = ((int32_t)il2cpp_codegen_add((int32_t)L_47, (int32_t)L_48));
		uint8_t L_50 = (L_46)->GetAt(static_cast<il2cpp_array_size_t>(L_49));
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_51 = V_5;
		int32_t L_52 = V_10;
		NullCheck(L_51);
		int32_t L_53 = L_52;
		uint8_t L_54 = (L_51)->GetAt(static_cast<il2cpp_array_size_t>(L_53));
		if ((((int32_t)L_50) == ((int32_t)L_54)))
		{
			goto IL_00b0;
		}
	}
	{
		V_9 = (bool)0;
		goto IL_00bc;
	}

IL_00b0:
	{
		int32_t L_55 = V_10;
		V_10 = ((int32_t)il2cpp_codegen_add((int32_t)L_55, (int32_t)1));
	}

IL_00b6:
	{
		int32_t L_56 = V_10;
		int32_t L_57 = V_6;
		if ((((int32_t)L_56) < ((int32_t)L_57)))
		{
			goto IL_009d;
		}
	}

IL_00bc:
	{
		bool L_58 = V_9;
		if (!L_58)
		{
			goto IL_0179;
		}
	}
	{
		int32_t L_59 = V_0;
		int32_t L_60 = V_6;
		V_0 = ((int32_t)il2cpp_codegen_add((int32_t)L_59, (int32_t)L_60));
		bool L_61 = ___iriParsing8;
		if (!L_61)
		{
			goto IL_013f;
		}
	}
	{
		bool L_62 = V_7;
		if (L_62)
		{
			goto IL_00f4;
		}
	}
	{
		V_11 = 0;
		goto IL_00e7;
	}

IL_00d5:
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_63 = V_5;
		int32_t L_64 = V_11;
		NullCheck(L_63);
		int32_t L_65 = L_64;
		uint8_t L_66 = (L_63)->GetAt(static_cast<il2cpp_array_size_t>(L_65));
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_67 = ___dest1;
		int32_t* L_68 = ___destOffset2;
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		UriHelper_EscapeAsciiChar_mFD7DE796BD53CBD2B1E73080FE0346D37F358902(L_66, L_67, (int32_t*)L_68, /*hidden argument*/NULL);
		int32_t L_69 = V_11;
		V_11 = ((int32_t)il2cpp_codegen_add((int32_t)L_69, (int32_t)1));
	}

IL_00e7:
	{
		int32_t L_70 = V_11;
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_71 = V_5;
		NullCheck(L_71);
		if ((((int32_t)L_70) < ((int32_t)(((int32_t)((int32_t)(((RuntimeArray *)L_71)->max_length)))))))
		{
			goto IL_00d5;
		}
	}
	{
		goto IL_019e;
	}

IL_00f4:
	{
		Il2CppChar* L_72 = V_1;
		int32_t L_73 = V_3;
		int32_t L_74 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_72, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_73)), (int32_t)2)))));
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		bool L_75 = Uri_IsBidiControlCharacter_mB14EA5816A434B7CE382EB9ACBD1432916EC341D(L_74, /*hidden argument*/NULL);
		if (L_75)
		{
			goto IL_019e;
		}
	}
	{
		Il2CppChar* L_76 = ___pDest0;
		int32_t* L_77 = ___destOffset2;
		int32_t* L_78 = ___destOffset2;
		int32_t L_79 = *((int32_t*)L_78);
		V_12 = L_79;
		int32_t L_80 = V_12;
		*((int32_t*)L_77) = (int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_80, (int32_t)1));
		int32_t L_81 = V_12;
		Il2CppChar* L_82 = V_1;
		int32_t L_83 = V_3;
		int32_t L_84 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_82, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_83)), (int32_t)2)))));
		*((int16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_76, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_81)), (int32_t)2))))) = (int16_t)L_84;
		bool L_85 = V_4;
		if (!L_85)
		{
			goto IL_019e;
		}
	}
	{
		Il2CppChar* L_86 = ___pDest0;
		int32_t* L_87 = ___destOffset2;
		int32_t* L_88 = ___destOffset2;
		int32_t L_89 = *((int32_t*)L_88);
		V_12 = L_89;
		int32_t L_90 = V_12;
		*((int32_t*)L_87) = (int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_90, (int32_t)1));
		int32_t L_91 = V_12;
		Il2CppChar* L_92 = V_1;
		int32_t L_93 = V_3;
		int32_t L_94 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_92, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)((int32_t)il2cpp_codegen_add((int32_t)L_93, (int32_t)1)))), (int32_t)2)))));
		*((int16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_86, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_91)), (int32_t)2))))) = (int16_t)L_94;
		goto IL_019e;
	}

IL_013f:
	{
		Il2CppChar* L_95 = ___pDest0;
		int32_t* L_96 = ___destOffset2;
		int32_t* L_97 = ___destOffset2;
		int32_t L_98 = *((int32_t*)L_97);
		V_12 = L_98;
		int32_t L_99 = V_12;
		*((int32_t*)L_96) = (int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_99, (int32_t)1));
		int32_t L_100 = V_12;
		Il2CppChar* L_101 = V_1;
		int32_t L_102 = V_3;
		int32_t L_103 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_101, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_102)), (int32_t)2)))));
		*((int16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_95, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_100)), (int32_t)2))))) = (int16_t)L_103;
		bool L_104 = V_4;
		if (!L_104)
		{
			goto IL_019e;
		}
	}
	{
		Il2CppChar* L_105 = ___pDest0;
		int32_t* L_106 = ___destOffset2;
		int32_t* L_107 = ___destOffset2;
		int32_t L_108 = *((int32_t*)L_107);
		V_12 = L_108;
		int32_t L_109 = V_12;
		*((int32_t*)L_106) = (int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_109, (int32_t)1));
		int32_t L_110 = V_12;
		Il2CppChar* L_111 = V_1;
		int32_t L_112 = V_3;
		int32_t L_113 = *((uint16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_111, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)((int32_t)il2cpp_codegen_add((int32_t)L_112, (int32_t)1)))), (int32_t)2)))));
		*((int16_t*)((Il2CppChar*)il2cpp_codegen_add((intptr_t)L_105, (intptr_t)((intptr_t)il2cpp_codegen_multiply((intptr_t)(((intptr_t)L_110)), (int32_t)2))))) = (int16_t)L_113;
		goto IL_019e;
	}

IL_0179:
	{
		V_13 = 0;
		goto IL_0193;
	}

IL_017e:
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_114 = ___bytes5;
		int32_t L_115 = V_0;
		int32_t L_116 = L_115;
		V_0 = ((int32_t)il2cpp_codegen_add((int32_t)L_116, (int32_t)1));
		NullCheck(L_114);
		int32_t L_117 = L_116;
		uint8_t L_118 = (L_114)->GetAt(static_cast<il2cpp_array_size_t>(L_117));
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_119 = ___dest1;
		int32_t* L_120 = ___destOffset2;
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		UriHelper_EscapeAsciiChar_mFD7DE796BD53CBD2B1E73080FE0346D37F358902(L_118, L_119, (int32_t*)L_120, /*hidden argument*/NULL);
		int32_t L_121 = V_13;
		V_13 = ((int32_t)il2cpp_codegen_add((int32_t)L_121, (int32_t)1));
	}

IL_0193:
	{
		int32_t L_122 = V_13;
		int32_t L_123 = V_10;
		if ((((int32_t)L_122) < ((int32_t)L_123)))
		{
			goto IL_017e;
		}
	}
	{
		goto IL_008b;
	}

IL_019e:
	{
		bool L_124 = V_4;
		if (!L_124)
		{
			goto IL_01a6;
		}
	}
	{
		int32_t L_125 = V_3;
		V_3 = ((int32_t)il2cpp_codegen_add((int32_t)L_125, (int32_t)1));
	}

IL_01a6:
	{
		int32_t L_126 = V_3;
		V_3 = ((int32_t)il2cpp_codegen_add((int32_t)L_126, (int32_t)1));
	}

IL_01aa:
	{
		int32_t L_127 = V_3;
		int32_t L_128 = ___charCount4;
		if ((((int32_t)L_127) < ((int32_t)L_128)))
		{
			goto IL_0021;
		}
	}
	{
		V_2 = (CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2*)NULL;
		goto IL_01c5;
	}

IL_01b6:
	{
		ByteU5BU5D_tD06FDBE8142446525DF1C40351D523A228373821* L_129 = ___bytes5;
		int32_t L_130 = V_0;
		int32_t L_131 = L_130;
		V_0 = ((int32_t)il2cpp_codegen_add((int32_t)L_131, (int32_t)1));
		NullCheck(L_129);
		int32_t L_132 = L_131;
		uint8_t L_133 = (L_129)->GetAt(static_cast<il2cpp_array_size_t>(L_132));
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_134 = ___dest1;
		int32_t* L_135 = ___destOffset2;
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		UriHelper_EscapeAsciiChar_mFD7DE796BD53CBD2B1E73080FE0346D37F358902(L_133, L_134, (int32_t*)L_135, /*hidden argument*/NULL);
	}

IL_01c5:
	{
		int32_t L_136 = V_0;
		int32_t L_137 = ___byteCount6;
		if ((((int32_t)L_136) < ((int32_t)L_137)))
		{
			goto IL_01b6;
		}
	}
	{
		return;
	}
}
// System.Void System.UriHelper::EscapeAsciiChar(System.Char,System.Char[],System.Int32U26)
extern "C" IL2CPP_METHOD_ATTR void UriHelper_EscapeAsciiChar_mFD7DE796BD53CBD2B1E73080FE0346D37F358902 (Il2CppChar ___ch0, CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___to1, int32_t* ___pos2, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriHelper_EscapeAsciiChar_mFD7DE796BD53CBD2B1E73080FE0346D37F358902_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	int32_t V_0 = 0;
	{
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_0 = ___to1;
		int32_t* L_1 = ___pos2;
		int32_t* L_2 = ___pos2;
		int32_t L_3 = *((int32_t*)L_2);
		V_0 = L_3;
		int32_t L_4 = V_0;
		*((int32_t*)L_1) = (int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_4, (int32_t)1));
		int32_t L_5 = V_0;
		NullCheck(L_0);
		(L_0)->SetAt(static_cast<il2cpp_array_size_t>(L_5), (Il2CppChar)((int32_t)37));
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_6 = ___to1;
		int32_t* L_7 = ___pos2;
		int32_t* L_8 = ___pos2;
		int32_t L_9 = *((int32_t*)L_8);
		V_0 = L_9;
		int32_t L_10 = V_0;
		*((int32_t*)L_7) = (int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_10, (int32_t)1));
		int32_t L_11 = V_0;
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_12 = ((UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_StaticFields*)il2cpp_codegen_static_fields_for(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var))->get_HexUpperChars_0();
		Il2CppChar L_13 = ___ch0;
		NullCheck(L_12);
		int32_t L_14 = ((int32_t)((int32_t)((int32_t)((int32_t)L_13&(int32_t)((int32_t)240)))>>(int32_t)4));
		uint16_t L_15 = (uint16_t)(L_12)->GetAt(static_cast<il2cpp_array_size_t>(L_14));
		NullCheck(L_6);
		(L_6)->SetAt(static_cast<il2cpp_array_size_t>(L_11), (Il2CppChar)L_15);
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_16 = ___to1;
		int32_t* L_17 = ___pos2;
		int32_t* L_18 = ___pos2;
		int32_t L_19 = *((int32_t*)L_18);
		V_0 = L_19;
		int32_t L_20 = V_0;
		*((int32_t*)L_17) = (int32_t)((int32_t)il2cpp_codegen_add((int32_t)L_20, (int32_t)1));
		int32_t L_21 = V_0;
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_22 = ((UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_StaticFields*)il2cpp_codegen_static_fields_for(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var))->get_HexUpperChars_0();
		Il2CppChar L_23 = ___ch0;
		NullCheck(L_22);
		int32_t L_24 = ((int32_t)((int32_t)L_23&(int32_t)((int32_t)15)));
		uint16_t L_25 = (uint16_t)(L_22)->GetAt(static_cast<il2cpp_array_size_t>(L_24));
		NullCheck(L_16);
		(L_16)->SetAt(static_cast<il2cpp_array_size_t>(L_21), (Il2CppChar)L_25);
		return;
	}
}
// System.Char System.UriHelper::EscapedAscii(System.Char,System.Char)
extern "C" IL2CPP_METHOD_ATTR Il2CppChar UriHelper_EscapedAscii_m06D556717795E649EBBB30E4CBCF3D221C1FEB78 (Il2CppChar ___digit0, Il2CppChar ___next1, const RuntimeMethod* method)
{
	int32_t V_0 = 0;
	int32_t G_B11_0 = 0;
	int32_t G_B13_0 = 0;
	int32_t G_B25_0 = 0;
	int32_t G_B21_0 = 0;
	int32_t G_B23_0 = 0;
	int32_t G_B22_0 = 0;
	int32_t G_B24_0 = 0;
	int32_t G_B24_1 = 0;
	int32_t G_B26_0 = 0;
	int32_t G_B26_1 = 0;
	{
		Il2CppChar L_0 = ___digit0;
		if ((((int32_t)L_0) < ((int32_t)((int32_t)48))))
		{
			goto IL_000a;
		}
	}
	{
		Il2CppChar L_1 = ___digit0;
		if ((((int32_t)L_1) <= ((int32_t)((int32_t)57))))
		{
			goto IL_0024;
		}
	}

IL_000a:
	{
		Il2CppChar L_2 = ___digit0;
		if ((((int32_t)L_2) < ((int32_t)((int32_t)65))))
		{
			goto IL_0014;
		}
	}
	{
		Il2CppChar L_3 = ___digit0;
		if ((((int32_t)L_3) <= ((int32_t)((int32_t)70))))
		{
			goto IL_0024;
		}
	}

IL_0014:
	{
		Il2CppChar L_4 = ___digit0;
		if ((((int32_t)L_4) < ((int32_t)((int32_t)97))))
		{
			goto IL_001e;
		}
	}
	{
		Il2CppChar L_5 = ___digit0;
		if ((((int32_t)L_5) <= ((int32_t)((int32_t)102))))
		{
			goto IL_0024;
		}
	}

IL_001e:
	{
		return ((int32_t)65535);
	}

IL_0024:
	{
		Il2CppChar L_6 = ___digit0;
		if ((((int32_t)L_6) <= ((int32_t)((int32_t)57))))
		{
			goto IL_003d;
		}
	}
	{
		Il2CppChar L_7 = ___digit0;
		if ((((int32_t)L_7) <= ((int32_t)((int32_t)70))))
		{
			goto IL_0034;
		}
	}
	{
		Il2CppChar L_8 = ___digit0;
		G_B11_0 = ((int32_t)il2cpp_codegen_subtract((int32_t)L_8, (int32_t)((int32_t)97)));
		goto IL_0038;
	}

IL_0034:
	{
		Il2CppChar L_9 = ___digit0;
		G_B11_0 = ((int32_t)il2cpp_codegen_subtract((int32_t)L_9, (int32_t)((int32_t)65)));
	}

IL_0038:
	{
		G_B13_0 = ((int32_t)il2cpp_codegen_add((int32_t)G_B11_0, (int32_t)((int32_t)10)));
		goto IL_0041;
	}

IL_003d:
	{
		Il2CppChar L_10 = ___digit0;
		G_B13_0 = ((int32_t)il2cpp_codegen_subtract((int32_t)L_10, (int32_t)((int32_t)48)));
	}

IL_0041:
	{
		V_0 = G_B13_0;
		Il2CppChar L_11 = ___next1;
		if ((((int32_t)L_11) < ((int32_t)((int32_t)48))))
		{
			goto IL_004c;
		}
	}
	{
		Il2CppChar L_12 = ___next1;
		if ((((int32_t)L_12) <= ((int32_t)((int32_t)57))))
		{
			goto IL_0066;
		}
	}

IL_004c:
	{
		Il2CppChar L_13 = ___next1;
		if ((((int32_t)L_13) < ((int32_t)((int32_t)65))))
		{
			goto IL_0056;
		}
	}
	{
		Il2CppChar L_14 = ___next1;
		if ((((int32_t)L_14) <= ((int32_t)((int32_t)70))))
		{
			goto IL_0066;
		}
	}

IL_0056:
	{
		Il2CppChar L_15 = ___next1;
		if ((((int32_t)L_15) < ((int32_t)((int32_t)97))))
		{
			goto IL_0060;
		}
	}
	{
		Il2CppChar L_16 = ___next1;
		if ((((int32_t)L_16) <= ((int32_t)((int32_t)102))))
		{
			goto IL_0066;
		}
	}

IL_0060:
	{
		return ((int32_t)65535);
	}

IL_0066:
	{
		int32_t L_17 = V_0;
		Il2CppChar L_18 = ___next1;
		G_B21_0 = ((int32_t)((int32_t)L_17<<(int32_t)4));
		if ((((int32_t)L_18) <= ((int32_t)((int32_t)57))))
		{
			G_B25_0 = ((int32_t)((int32_t)L_17<<(int32_t)4));
			goto IL_0082;
		}
	}
	{
		Il2CppChar L_19 = ___next1;
		G_B22_0 = G_B21_0;
		if ((((int32_t)L_19) <= ((int32_t)((int32_t)70))))
		{
			G_B23_0 = G_B21_0;
			goto IL_0079;
		}
	}
	{
		Il2CppChar L_20 = ___next1;
		G_B24_0 = ((int32_t)il2cpp_codegen_subtract((int32_t)L_20, (int32_t)((int32_t)97)));
		G_B24_1 = G_B22_0;
		goto IL_007d;
	}

IL_0079:
	{
		Il2CppChar L_21 = ___next1;
		G_B24_0 = ((int32_t)il2cpp_codegen_subtract((int32_t)L_21, (int32_t)((int32_t)65)));
		G_B24_1 = G_B23_0;
	}

IL_007d:
	{
		G_B26_0 = ((int32_t)il2cpp_codegen_add((int32_t)G_B24_0, (int32_t)((int32_t)10)));
		G_B26_1 = G_B24_1;
		goto IL_0086;
	}

IL_0082:
	{
		Il2CppChar L_22 = ___next1;
		G_B26_0 = ((int32_t)il2cpp_codegen_subtract((int32_t)L_22, (int32_t)((int32_t)48)));
		G_B26_1 = G_B25_0;
	}

IL_0086:
	{
		return (((int32_t)((uint16_t)((int32_t)il2cpp_codegen_add((int32_t)G_B26_1, (int32_t)G_B26_0)))));
	}
}
// System.Boolean System.UriHelper::IsNotSafeForUnescape(System.Char)
extern "C" IL2CPP_METHOD_ATTR bool UriHelper_IsNotSafeForUnescape_m1D0461E7C5A3CFBD7A2A7F7322B66BC68CCE741D (Il2CppChar ___ch0, const RuntimeMethod* method)
{
	{
		Il2CppChar L_0 = ___ch0;
		if ((((int32_t)L_0) <= ((int32_t)((int32_t)31))))
		{
			goto IL_0012;
		}
	}
	{
		Il2CppChar L_1 = ___ch0;
		if ((((int32_t)L_1) < ((int32_t)((int32_t)127))))
		{
			goto IL_0014;
		}
	}
	{
		Il2CppChar L_2 = ___ch0;
		if ((((int32_t)L_2) > ((int32_t)((int32_t)159))))
		{
			goto IL_0014;
		}
	}

IL_0012:
	{
		return (bool)1;
	}

IL_0014:
	{
		Il2CppChar L_3 = ___ch0;
		if ((((int32_t)L_3) < ((int32_t)((int32_t)59))))
		{
			goto IL_0025;
		}
	}
	{
		Il2CppChar L_4 = ___ch0;
		if ((((int32_t)L_4) > ((int32_t)((int32_t)64))))
		{
			goto IL_0025;
		}
	}
	{
		Il2CppChar L_5 = ___ch0;
		if ((!(((uint32_t)((int32_t)((int32_t)L_5|(int32_t)2))) == ((uint32_t)((int32_t)62)))))
		{
			goto IL_0043;
		}
	}

IL_0025:
	{
		Il2CppChar L_6 = ___ch0;
		if ((((int32_t)L_6) < ((int32_t)((int32_t)35))))
		{
			goto IL_002f;
		}
	}
	{
		Il2CppChar L_7 = ___ch0;
		if ((((int32_t)L_7) <= ((int32_t)((int32_t)38))))
		{
			goto IL_0043;
		}
	}

IL_002f:
	{
		Il2CppChar L_8 = ___ch0;
		if ((((int32_t)L_8) == ((int32_t)((int32_t)43))))
		{
			goto IL_0043;
		}
	}
	{
		Il2CppChar L_9 = ___ch0;
		if ((((int32_t)L_9) == ((int32_t)((int32_t)44))))
		{
			goto IL_0043;
		}
	}
	{
		Il2CppChar L_10 = ___ch0;
		if ((((int32_t)L_10) == ((int32_t)((int32_t)47))))
		{
			goto IL_0043;
		}
	}
	{
		Il2CppChar L_11 = ___ch0;
		if ((!(((uint32_t)L_11) == ((uint32_t)((int32_t)92)))))
		{
			goto IL_0045;
		}
	}

IL_0043:
	{
		return (bool)1;
	}

IL_0045:
	{
		return (bool)0;
	}
}
// System.Boolean System.UriHelper::IsReservedUnreservedOrHash(System.Char)
extern "C" IL2CPP_METHOD_ATTR bool UriHelper_IsReservedUnreservedOrHash_m3D7256DABA7F540F8D379FC1D1C54F1C63E46059 (Il2CppChar ___c0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriHelper_IsReservedUnreservedOrHash_m3D7256DABA7F540F8D379FC1D1C54F1C63E46059_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Il2CppChar L_0 = ___c0;
		IL2CPP_RUNTIME_CLASS_INIT(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var);
		bool L_1 = UriHelper_IsUnreserved_mAADC7DCEEA864AFB49311696ABBDD76811FAAE48(L_0, /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_000a;
		}
	}
	{
		return (bool)1;
	}

IL_000a:
	{
		IL2CPP_RUNTIME_CLASS_INIT(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var);
		bool L_2 = UriParser_get_ShouldUseLegacyV2Quirks_mD4C8DF67677ACCCC3B5E026099ECC0BDA24D96DD(/*hidden argument*/NULL);
		if (!L_2)
		{
			goto IL_0027;
		}
	}
	{
		Il2CppChar L_3 = ___c0;
		NullCheck(_stringLiteral422C2FC455DA8AB1CCF099E014DADE733913E48A);
		int32_t L_4 = String_IndexOf_m2909B8CF585E1BD0C81E11ACA2F48012156FD5BD(_stringLiteral422C2FC455DA8AB1CCF099E014DADE733913E48A, L_3, /*hidden argument*/NULL);
		if ((((int32_t)L_4) >= ((int32_t)0)))
		{
			goto IL_0025;
		}
	}
	{
		Il2CppChar L_5 = ___c0;
		return (bool)((((int32_t)L_5) == ((int32_t)((int32_t)35)))? 1 : 0);
	}

IL_0025:
	{
		return (bool)1;
	}

IL_0027:
	{
		Il2CppChar L_6 = ___c0;
		NullCheck(_stringLiteral7608E1FF0B8CFEF39D687771BAC4DCB767C2C102);
		int32_t L_7 = String_IndexOf_m2909B8CF585E1BD0C81E11ACA2F48012156FD5BD(_stringLiteral7608E1FF0B8CFEF39D687771BAC4DCB767C2C102, L_6, /*hidden argument*/NULL);
		return (bool)((((int32_t)((((int32_t)L_7) < ((int32_t)0))? 1 : 0)) == ((int32_t)0))? 1 : 0);
	}
}
// System.Boolean System.UriHelper::IsUnreserved(System.Char)
extern "C" IL2CPP_METHOD_ATTR bool UriHelper_IsUnreserved_mAADC7DCEEA864AFB49311696ABBDD76811FAAE48 (Il2CppChar ___c0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriHelper_IsUnreserved_mAADC7DCEEA864AFB49311696ABBDD76811FAAE48_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Il2CppChar L_0 = ___c0;
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		bool L_1 = Uri_IsAsciiLetterOrDigit_mEBA81E735141504B5804F0B3C94EC39B24AF8661(L_0, /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_000a;
		}
	}
	{
		return (bool)1;
	}

IL_000a:
	{
		IL2CPP_RUNTIME_CLASS_INIT(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var);
		bool L_2 = UriParser_get_ShouldUseLegacyV2Quirks_mD4C8DF67677ACCCC3B5E026099ECC0BDA24D96DD(/*hidden argument*/NULL);
		if (!L_2)
		{
			goto IL_0023;
		}
	}
	{
		Il2CppChar L_3 = ___c0;
		NullCheck(_stringLiteral5D7FEFA52F916FB1F734F27D1226BA1556F23E16);
		int32_t L_4 = String_IndexOf_m2909B8CF585E1BD0C81E11ACA2F48012156FD5BD(_stringLiteral5D7FEFA52F916FB1F734F27D1226BA1556F23E16, L_3, /*hidden argument*/NULL);
		return (bool)((((int32_t)((((int32_t)L_4) < ((int32_t)0))? 1 : 0)) == ((int32_t)0))? 1 : 0);
	}

IL_0023:
	{
		Il2CppChar L_5 = ___c0;
		NullCheck(_stringLiteral3AE3AD09884E848958DF67AFEC6B436733CEB84C);
		int32_t L_6 = String_IndexOf_m2909B8CF585E1BD0C81E11ACA2F48012156FD5BD(_stringLiteral3AE3AD09884E848958DF67AFEC6B436733CEB84C, L_5, /*hidden argument*/NULL);
		return (bool)((((int32_t)((((int32_t)L_6) < ((int32_t)0))? 1 : 0)) == ((int32_t)0))? 1 : 0);
	}
}
// System.Boolean System.UriHelper::Is3986Unreserved(System.Char)
extern "C" IL2CPP_METHOD_ATTR bool UriHelper_Is3986Unreserved_m3799F2ADA8C63DDB4995F82B974C8EC1DEEBA76A (Il2CppChar ___c0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriHelper_Is3986Unreserved_m3799F2ADA8C63DDB4995F82B974C8EC1DEEBA76A_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Il2CppChar L_0 = ___c0;
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		bool L_1 = Uri_IsAsciiLetterOrDigit_mEBA81E735141504B5804F0B3C94EC39B24AF8661(L_0, /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_000a;
		}
	}
	{
		return (bool)1;
	}

IL_000a:
	{
		Il2CppChar L_2 = ___c0;
		NullCheck(_stringLiteral3AE3AD09884E848958DF67AFEC6B436733CEB84C);
		int32_t L_3 = String_IndexOf_m2909B8CF585E1BD0C81E11ACA2F48012156FD5BD(_stringLiteral3AE3AD09884E848958DF67AFEC6B436733CEB84C, L_2, /*hidden argument*/NULL);
		return (bool)((((int32_t)((((int32_t)L_3) < ((int32_t)0))? 1 : 0)) == ((int32_t)0))? 1 : 0);
	}
}
// System.Void System.UriHelper::.cctor()
extern "C" IL2CPP_METHOD_ATTR void UriHelper__cctor_m9537B8AAAA1D6EF77D29A179EC79F5511C662F27 (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriHelper__cctor_m9537B8AAAA1D6EF77D29A179EC79F5511C662F27_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_0 = (CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2*)SZArrayNew(CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2_il2cpp_TypeInfo_var, (uint32_t)((int32_t)16));
		CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* L_1 = L_0;
		RuntimeFieldHandle_t844BDF00E8E6FE69D9AEAA7657F09018B864F4EF  L_2 = { reinterpret_cast<intptr_t> (U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291____59F5BD34B6C013DEACC784F69C67E95150033A84_6_FieldInfo_var) };
		RuntimeHelpers_InitializeArray_m29F50CDFEEE0AB868200291366253DD4737BC76A((RuntimeArray *)(RuntimeArray *)L_1, L_2, /*hidden argument*/NULL);
		((UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_StaticFields*)il2cpp_codegen_static_fields_for(UriHelper_tA44F3057604BAA4E6EF06A8EE4E6825D471592DF_il2cpp_TypeInfo_var))->set_HexUpperChars_0(L_1);
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
// System.String System.UriParser::get_SchemeName()
extern "C" IL2CPP_METHOD_ATTR String_t* UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, const RuntimeMethod* method)
{
	{
		String_t* L_0 = __this->get_m_Scheme_6();
		return L_0;
	}
}
// System.Int32 System.UriParser::get_DefaultPort()
extern "C" IL2CPP_METHOD_ATTR int32_t UriParser_get_DefaultPort_m050510870CCD4DD08DF7E98E2AF3D616446AD99D (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_Port_5();
		return L_0;
	}
}
// System.UriParser System.UriParser::OnNewUri()
extern "C" IL2CPP_METHOD_ATTR UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * UriParser_OnNewUri_m7D55337A7A9B6B67FB0AD7CA96F472751EF5A897 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, const RuntimeMethod* method)
{
	{
		return __this;
	}
}
// System.Void System.UriParser::InitializeAndValidate(System.Uri,System.UriFormatExceptionU26)
extern "C" IL2CPP_METHOD_ATTR void UriParser_InitializeAndValidate_m3E31D86FEE445E313BB7141F760626301767A0E0 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___uri0, UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A ** ___parsingError1, const RuntimeMethod* method)
{
	{
		UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A ** L_0 = ___parsingError1;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_1 = ___uri0;
		NullCheck(L_1);
		UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A * L_2 = Uri_ParseMinimal_m35FCFE52F12315DA60733B807E7C0AB408C0A9CF(L_1, /*hidden argument*/NULL);
		*((RuntimeObject **)L_0) = (RuntimeObject *)L_2;
		Il2CppCodeGenWriteBarrier((RuntimeObject **)L_0, (RuntimeObject *)L_2);
		return;
	}
}
// System.String System.UriParser::Resolve(System.Uri,System.Uri,System.UriFormatExceptionU26)
extern "C" IL2CPP_METHOD_ATTR String_t* UriParser_Resolve_mF21D3AA42AB1EC2B173617D76E4041EB3481D979 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___baseUri0, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___relativeUri1, UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A ** ___parsingError2, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriParser_Resolve_mF21D3AA42AB1EC2B173617D76E4041EB3481D979_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	String_t* V_0 = NULL;
	bool V_1 = false;
	Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * V_2 = NULL;
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_0 = ___baseUri0;
		NullCheck(L_0);
		bool L_1 = Uri_get_UserDrivenParsing_mFF27964894B5C0432C37E425F319D6C915BCDC39(L_0, /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_002c;
		}
	}
	{
		ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* L_2 = (ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A*)SZArrayNew(ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* L_3 = L_2;
		Type_t * L_4 = Object_GetType_m2E0B62414ECCAA3094B703790CE88CBB2F83EA60(__this, /*hidden argument*/NULL);
		NullCheck(L_4);
		String_t* L_5 = VirtFuncInvoker0< String_t* >::Invoke(26 /* System.String System.Type::get_FullName() */, L_4);
		NullCheck(L_3);
		ArrayElementTypeCheck (L_3, L_5);
		(L_3)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject *)L_5);
		String_t* L_6 = SR_GetString_m9548BD6DD52DFDB46372F211078AE57FA2401E39(_stringLiteral685AA46800DA1134A27CF09D92AB8FB9481ABE68, L_3, /*hidden argument*/NULL);
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_7 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_7, L_6, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_7, NULL, UriParser_Resolve_mF21D3AA42AB1EC2B173617D76E4041EB3481D979_RuntimeMethod_var);
	}

IL_002c:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_8 = ___baseUri0;
		NullCheck(L_8);
		bool L_9 = Uri_get_IsAbsoluteUri_m8C189085F1C675DBC3148AA70C38074EC075D722(L_8, /*hidden argument*/NULL);
		if (L_9)
		{
			goto IL_0044;
		}
	}
	{
		String_t* L_10 = SR_GetString_m3FC710B15474A9B651DA02B303241B6D8B87E2A7(_stringLiteral12B6FF7C47BB4C2C973EF6E38B06B1AD0DACA96F, /*hidden argument*/NULL);
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_11 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_11, L_10, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_11, NULL, UriParser_Resolve_mF21D3AA42AB1EC2B173617D76E4041EB3481D979_RuntimeMethod_var);
	}

IL_0044:
	{
		V_0 = (String_t*)NULL;
		V_1 = (bool)0;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_12 = ___baseUri0;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_13 = ___relativeUri1;
		UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A ** L_14 = ___parsingError2;
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_15 = Uri_ResolveHelper_mEDF1549C3E9AC1CF6177DCF93B17D574411916BC(L_12, L_13, (String_t**)(&V_0), (bool*)(&V_1), (UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A **)L_14, /*hidden argument*/NULL);
		V_2 = L_15;
		UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A ** L_16 = ___parsingError2;
		UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A * L_17 = *((UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A **)L_16);
		if (!L_17)
		{
			goto IL_005b;
		}
	}
	{
		return (String_t*)NULL;
	}

IL_005b:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_18 = V_2;
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		bool L_19 = Uri_op_Inequality_m07015206F59460E87CDE2A8D303D5712E30A7F6B(L_18, (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)NULL, /*hidden argument*/NULL);
		if (!L_19)
		{
			goto IL_006b;
		}
	}
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_20 = V_2;
		NullCheck(L_20);
		String_t* L_21 = Uri_get_OriginalString_m56099E46276F0A52524347F1F46A2F88E948504F(L_20, /*hidden argument*/NULL);
		return L_21;
	}

IL_006b:
	{
		String_t* L_22 = V_0;
		return L_22;
	}
}
// System.String System.UriParser::GetComponents(System.Uri,System.UriComponents,System.UriFormat)
extern "C" IL2CPP_METHOD_ATTR String_t* UriParser_GetComponents_m8A226F43638FA7CD135A651CDE3D4E475E8FC181 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___uri0, int32_t ___components1, int32_t ___format2, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriParser_GetComponents_m8A226F43638FA7CD135A651CDE3D4E475E8FC181_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		int32_t L_0 = ___components1;
		if (!((int32_t)((int32_t)L_0&(int32_t)((int32_t)-2147483648LL))))
		{
			goto IL_002c;
		}
	}
	{
		int32_t L_1 = ___components1;
		if ((((int32_t)L_1) == ((int32_t)((int32_t)-2147483648LL))))
		{
			goto IL_002c;
		}
	}
	{
		int32_t L_2 = ___components1;
		int32_t L_3 = L_2;
		RuntimeObject * L_4 = Box(UriComponents_tE42D5229291668DE73323E1C519E4E1459A64CFF_il2cpp_TypeInfo_var, &L_3);
		String_t* L_5 = SR_GetString_m3FC710B15474A9B651DA02B303241B6D8B87E2A7(_stringLiteral4931F5B26E4E3B67A69DCEAE7622810683E83201, /*hidden argument*/NULL);
		ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA * L_6 = (ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA *)il2cpp_codegen_object_new(ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA_il2cpp_TypeInfo_var);
		ArgumentOutOfRangeException__ctor_m755B01B4B4595B447596E3281F22FD7CE6DAE378(L_6, _stringLiteralC212F08ED1157AE268FD83D142AFD5CCD48664B2, L_4, L_5, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_6, NULL, UriParser_GetComponents_m8A226F43638FA7CD135A651CDE3D4E475E8FC181_RuntimeMethod_var);
	}

IL_002c:
	{
		int32_t L_7 = ___format2;
		if (!((int32_t)((int32_t)L_7&(int32_t)((int32_t)-4))))
		{
			goto IL_003d;
		}
	}
	{
		ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA * L_8 = (ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA *)il2cpp_codegen_object_new(ArgumentOutOfRangeException_t94D19DF918A54511AEDF4784C9A08741BAD1DEDA_il2cpp_TypeInfo_var);
		ArgumentOutOfRangeException__ctor_m6B36E60C989DC798A8B44556DB35960282B133A6(L_8, _stringLiteral785987648F85190CFDE9EADC69FC7C46FE8A7433, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_8, NULL, UriParser_GetComponents_m8A226F43638FA7CD135A651CDE3D4E475E8FC181_RuntimeMethod_var);
	}

IL_003d:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_9 = ___uri0;
		NullCheck(L_9);
		bool L_10 = Uri_get_UserDrivenParsing_mFF27964894B5C0432C37E425F319D6C915BCDC39(L_9, /*hidden argument*/NULL);
		if (!L_10)
		{
			goto IL_0069;
		}
	}
	{
		ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* L_11 = (ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A*)SZArrayNew(ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t3C9242B5C88A48B2A5BD9FDA6CD0024E792AF08A* L_12 = L_11;
		Type_t * L_13 = Object_GetType_m2E0B62414ECCAA3094B703790CE88CBB2F83EA60(__this, /*hidden argument*/NULL);
		NullCheck(L_13);
		String_t* L_14 = VirtFuncInvoker0< String_t* >::Invoke(26 /* System.String System.Type::get_FullName() */, L_13);
		NullCheck(L_12);
		ArrayElementTypeCheck (L_12, L_14);
		(L_12)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject *)L_14);
		String_t* L_15 = SR_GetString_m9548BD6DD52DFDB46372F211078AE57FA2401E39(_stringLiteral685AA46800DA1134A27CF09D92AB8FB9481ABE68, L_12, /*hidden argument*/NULL);
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_16 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_16, L_15, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_16, NULL, UriParser_GetComponents_m8A226F43638FA7CD135A651CDE3D4E475E8FC181_RuntimeMethod_var);
	}

IL_0069:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_17 = ___uri0;
		NullCheck(L_17);
		bool L_18 = Uri_get_IsAbsoluteUri_m8C189085F1C675DBC3148AA70C38074EC075D722(L_17, /*hidden argument*/NULL);
		if (L_18)
		{
			goto IL_0081;
		}
	}
	{
		String_t* L_19 = SR_GetString_m3FC710B15474A9B651DA02B303241B6D8B87E2A7(_stringLiteral12B6FF7C47BB4C2C973EF6E38B06B1AD0DACA96F, /*hidden argument*/NULL);
		InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 * L_20 = (InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1 *)il2cpp_codegen_object_new(InvalidOperationException_t0530E734D823F78310CAFAFA424CA5164D93A1F1_il2cpp_TypeInfo_var);
		InvalidOperationException__ctor_m72027D5F1D513C25C05137E203EEED8FD8297706(L_20, L_19, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_20, NULL, UriParser_GetComponents_m8A226F43638FA7CD135A651CDE3D4E475E8FC181_RuntimeMethod_var);
	}

IL_0081:
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_21 = ___uri0;
		int32_t L_22 = ___components1;
		int32_t L_23 = ___format2;
		NullCheck(L_21);
		String_t* L_24 = Uri_GetComponentsHelper_m28B0D80FD94A40685C0F70652AB26755C457B2D3(L_21, L_22, L_23, /*hidden argument*/NULL);
		return L_24;
	}
}
// System.Boolean System.UriParser::IsWellFormedOriginalString(System.Uri)
extern "C" IL2CPP_METHOD_ATTR bool UriParser_IsWellFormedOriginalString_m5F6B55E961AD93ADA67353BECBC046BAF1743228 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___uri0, const RuntimeMethod* method)
{
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_0 = ___uri0;
		NullCheck(L_0);
		bool L_1 = Uri_InternalIsWellFormedOriginalString_mC5B6EDD6C06519FC6E5176DB89237CCCFFE56CAB(L_0, /*hidden argument*/NULL);
		return L_1;
	}
}
// System.Boolean System.UriParser::get_ShouldUseLegacyV2Quirks()
extern "C" IL2CPP_METHOD_ATTR bool UriParser_get_ShouldUseLegacyV2Quirks_mD4C8DF67677ACCCC3B5E026099ECC0BDA24D96DD (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriParser_get_ShouldUseLegacyV2Quirks_mD4C8DF67677ACCCC3B5E026099ECC0BDA24D96DD_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		IL2CPP_RUNTIME_CLASS_INIT(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var);
		int32_t L_0 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_s_QuirksVersion_23();
		return (bool)((((int32_t)((((int32_t)L_0) > ((int32_t)2))? 1 : 0)) == ((int32_t)0))? 1 : 0);
	}
}
// System.Void System.UriParser::.cctor()
extern "C" IL2CPP_METHOD_ATTR void UriParser__cctor_m00C2855D5C8C07790C5627BBB90AC84A7E8B6BC2 (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriParser__cctor_m00C2855D5C8C07790C5627BBB90AC84A7E8B6BC2_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	int32_t G_B3_0 = 0;
	int32_t G_B5_0 = 0;
	int32_t G_B4_0 = 0;
	int32_t G_B6_0 = 0;
	int32_t G_B6_1 = 0;
	int32_t G_B8_0 = 0;
	int32_t G_B7_0 = 0;
	int32_t G_B9_0 = 0;
	int32_t G_B9_1 = 0;
	{
		IL2CPP_RUNTIME_CLASS_INIT(BinaryCompatibility_t06B1B8D34764DB1710459778EB22433728A665A8_il2cpp_TypeInfo_var);
		bool L_0 = ((BinaryCompatibility_t06B1B8D34764DB1710459778EB22433728A665A8_StaticFields*)il2cpp_codegen_static_fields_for(BinaryCompatibility_t06B1B8D34764DB1710459778EB22433728A665A8_il2cpp_TypeInfo_var))->get_TargetsAtLeast_Desktop_V4_5_0();
		if (L_0)
		{
			goto IL_000a;
		}
	}
	{
		G_B3_0 = 2;
		goto IL_000b;
	}

IL_000a:
	{
		G_B3_0 = 3;
	}

IL_000b:
	{
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_s_QuirksVersion_23(G_B3_0);
		bool L_1 = UriParser_get_ShouldUseLegacyV2Quirks_mD4C8DF67677ACCCC3B5E026099ECC0BDA24D96DD(/*hidden argument*/NULL);
		G_B4_0 = ((int32_t)31461245);
		if (L_1)
		{
			G_B5_0 = ((int32_t)31461245);
			goto IL_001f;
		}
	}
	{
		G_B6_0 = 0;
		G_B6_1 = G_B4_0;
		goto IL_0024;
	}

IL_001f:
	{
		G_B6_0 = ((int32_t)33554432);
		G_B6_1 = G_B5_0;
	}

IL_0024:
	{
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_HttpSyntaxFlags_24(((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)G_B6_1|(int32_t)G_B6_0))|(int32_t)((int32_t)67108864)))|(int32_t)((int32_t)268435456))));
		bool L_2 = UriParser_get_ShouldUseLegacyV2Quirks_mD4C8DF67677ACCCC3B5E026099ECC0BDA24D96DD(/*hidden argument*/NULL);
		G_B7_0 = ((int32_t)4049);
		if (L_2)
		{
			G_B8_0 = ((int32_t)4049);
			goto IL_0046;
		}
	}
	{
		G_B9_0 = ((int32_t)32);
		G_B9_1 = G_B7_0;
		goto IL_0047;
	}

IL_0046:
	{
		G_B9_0 = 0;
		G_B9_1 = G_B8_0;
	}

IL_0047:
	{
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_FileSyntaxFlags_25(((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)((int32_t)G_B9_1|(int32_t)G_B9_0))|(int32_t)((int32_t)8192)))|(int32_t)((int32_t)2097152)))|(int32_t)((int32_t)1048576)))|(int32_t)((int32_t)4194304)))|(int32_t)((int32_t)8388608)))|(int32_t)((int32_t)16777216)))|(int32_t)((int32_t)33554432)))|(int32_t)((int32_t)67108864)))|(int32_t)((int32_t)268435456))));
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_3 = (Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE *)il2cpp_codegen_object_new(Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE_il2cpp_TypeInfo_var);
		Dictionary_2__ctor_m9AA6FFC23A9032DF2BF483986951F06E722B3445(L_3, ((int32_t)25), /*hidden argument*/Dictionary_2__ctor_m9AA6FFC23A9032DF2BF483986951F06E722B3445_RuntimeMethod_var);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_m_Table_0(L_3);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_4 = (Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE *)il2cpp_codegen_object_new(Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE_il2cpp_TypeInfo_var);
		Dictionary_2__ctor_m9AA6FFC23A9032DF2BF483986951F06E722B3445(L_4, ((int32_t)25), /*hidden argument*/Dictionary_2__ctor_m9AA6FFC23A9032DF2BF483986951F06E722B3445_RuntimeMethod_var);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_m_TempTable_1(L_4);
		int32_t L_5 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_HttpSyntaxFlags_24();
		BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_6 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
		BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_6, _stringLiteral77B5F8E343A90F6F597751021FB8B7A08FE83083, ((int32_t)80), L_5, /*hidden argument*/NULL);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_HttpUri_7(L_6);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_7 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_8 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_HttpUri_7();
		NullCheck(L_8);
		String_t* L_9 = UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1(L_8, /*hidden argument*/NULL);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_10 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_HttpUri_7();
		NullCheck(L_7);
		Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_7, L_9, L_10, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_11 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_HttpUri_7();
		NullCheck(L_11);
		int32_t L_12 = L_11->get_m_Flags_2();
		BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_13 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
		BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_13, _stringLiteralC3437DBC7C1255D3A21D444D86EBF2E9234C22BD, ((int32_t)443), L_12, /*hidden argument*/NULL);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_HttpsUri_8(L_13);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_14 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_15 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_HttpsUri_8();
		NullCheck(L_15);
		String_t* L_16 = UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1(L_15, /*hidden argument*/NULL);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_17 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_HttpsUri_8();
		NullCheck(L_14);
		Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_14, L_16, L_17, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
		int32_t L_18 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_HttpSyntaxFlags_24();
		BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_19 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
		BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_19, _stringLiteral1457B75DC8C5500C0F1D4503CF801B60DEB045A4, ((int32_t)80), L_18, /*hidden argument*/NULL);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_WsUri_9(L_19);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_20 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_21 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_WsUri_9();
		NullCheck(L_21);
		String_t* L_22 = UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1(L_21, /*hidden argument*/NULL);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_23 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_WsUri_9();
		NullCheck(L_20);
		Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_20, L_22, L_23, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
		int32_t L_24 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_HttpSyntaxFlags_24();
		BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_25 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
		BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_25, _stringLiteralBA2B0DD158763C472A7D7B22AEF6FF6571B9365C, ((int32_t)443), L_24, /*hidden argument*/NULL);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_WssUri_10(L_25);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_26 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_27 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_WssUri_10();
		NullCheck(L_27);
		String_t* L_28 = UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1(L_27, /*hidden argument*/NULL);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_29 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_WssUri_10();
		NullCheck(L_26);
		Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_26, L_28, L_29, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
		BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_30 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
		BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_30, _stringLiteral7616BB87BD05F6439E3672BA1B2BE55D5BEB68B3, ((int32_t)21), ((int32_t)367005533), /*hidden argument*/NULL);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_FtpUri_11(L_30);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_31 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_32 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_FtpUri_11();
		NullCheck(L_32);
		String_t* L_33 = UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1(L_32, /*hidden argument*/NULL);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_34 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_FtpUri_11();
		NullCheck(L_31);
		Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_31, L_33, L_34, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
		int32_t L_35 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_FileSyntaxFlags_25();
		BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_36 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
		BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_36, _stringLiteral971C419DD609331343DEE105FFFD0F4608DC0BF2, (-1), L_35, /*hidden argument*/NULL);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_FileUri_12(L_36);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_37 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_38 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_FileUri_12();
		NullCheck(L_38);
		String_t* L_39 = UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1(L_38, /*hidden argument*/NULL);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_40 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_FileUri_12();
		NullCheck(L_37);
		Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_37, L_39, L_40, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
		BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_41 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
		BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_41, _stringLiteral4188736A00FBFB506ACA06281ACF338290455C21, ((int32_t)70), ((int32_t)337645405), /*hidden argument*/NULL);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_GopherUri_13(L_41);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_42 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_43 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_GopherUri_13();
		NullCheck(L_43);
		String_t* L_44 = UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1(L_43, /*hidden argument*/NULL);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_45 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_GopherUri_13();
		NullCheck(L_42);
		Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_42, L_44, L_45, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
		BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_46 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
		BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_46, _stringLiteral666948CC54CBC3FC2C70107A835E27C872F476E6, ((int32_t)119), ((int32_t)337645405), /*hidden argument*/NULL);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_NntpUri_14(L_46);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_47 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_48 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_NntpUri_14();
		NullCheck(L_48);
		String_t* L_49 = UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1(L_48, /*hidden argument*/NULL);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_50 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_NntpUri_14();
		NullCheck(L_47);
		Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_47, L_49, L_50, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
		BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_51 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
		BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_51, _stringLiteral3C6BDCDDC94F64BF77DEB306AAE490A90A6FC300, (-1), ((int32_t)268435536), /*hidden argument*/NULL);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_NewsUri_15(L_51);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_52 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_53 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_NewsUri_15();
		NullCheck(L_53);
		String_t* L_54 = UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1(L_53, /*hidden argument*/NULL);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_55 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_NewsUri_15();
		NullCheck(L_52);
		Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_52, L_54, L_55, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
		BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_56 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
		BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_56, _stringLiteralFE710CD089CB0BA74F588270FE079A392B5E9810, ((int32_t)25), ((int32_t)335564796), /*hidden argument*/NULL);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_MailToUri_16(L_56);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_57 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_58 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_MailToUri_16();
		NullCheck(L_58);
		String_t* L_59 = UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1(L_58, /*hidden argument*/NULL);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_60 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_MailToUri_16();
		NullCheck(L_57);
		Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_57, L_59, L_60, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_61 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_NewsUri_15();
		NullCheck(L_61);
		int32_t L_62 = L_61->get_m_Flags_2();
		BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_63 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
		BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_63, _stringLiteral48E3462CBEEDD9B70CED95702E2E262CEBA217DA, (-1), L_62, /*hidden argument*/NULL);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_UuidUri_17(L_63);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_64 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_65 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_UuidUri_17();
		NullCheck(L_65);
		String_t* L_66 = UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1(L_65, /*hidden argument*/NULL);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_67 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_UuidUri_17();
		NullCheck(L_64);
		Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_64, L_66, L_67, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
		BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_68 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
		BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_68, _stringLiteral22E9F56882C87C3DA193BE3FE6D8C77FFDAF27BC, ((int32_t)23), ((int32_t)337645405), /*hidden argument*/NULL);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_TelnetUri_18(L_68);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_69 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_70 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_TelnetUri_18();
		NullCheck(L_70);
		String_t* L_71 = UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1(L_70, /*hidden argument*/NULL);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_72 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_TelnetUri_18();
		NullCheck(L_69);
		Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_69, L_71, L_72, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
		BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_73 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
		BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_73, _stringLiteral61A135089EAC561A2FF7CEDEEFB03975BED000F8, ((int32_t)389), ((int32_t)337645565), /*hidden argument*/NULL);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_LdapUri_19(L_73);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_74 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_75 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_LdapUri_19();
		NullCheck(L_75);
		String_t* L_76 = UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1(L_75, /*hidden argument*/NULL);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_77 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_LdapUri_19();
		NullCheck(L_74);
		Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_74, L_76, L_77, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
		BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_78 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
		BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_78, _stringLiteral0765DEEFD5C1509444309BD8D09E7ACA927165F8, ((int32_t)808), ((int32_t)400559737), /*hidden argument*/NULL);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_NetTcpUri_20(L_78);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_79 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_80 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_NetTcpUri_20();
		NullCheck(L_80);
		String_t* L_81 = UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1(L_80, /*hidden argument*/NULL);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_82 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_NetTcpUri_20();
		NullCheck(L_79);
		Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_79, L_81, L_82, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
		BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_83 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
		BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_83, _stringLiteral1F8A1C4B94F61170B94E9FD827F36A60174238C7, (-1), ((int32_t)400559729), /*hidden argument*/NULL);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_NetPipeUri_21(L_83);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_84 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_85 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_NetPipeUri_21();
		NullCheck(L_85);
		String_t* L_86 = UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1(L_85, /*hidden argument*/NULL);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_87 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_NetPipeUri_21();
		NullCheck(L_84);
		Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_84, L_86, L_87, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
		BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_88 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
		BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_88, _stringLiteral5E6A1BC91A4C36E5A0E45B3C8F8A2CF3F48785C5, (-1), ((int32_t)399519697), /*hidden argument*/NULL);
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_VsMacrosUri_22(L_88);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_89 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_90 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_VsMacrosUri_22();
		NullCheck(L_90);
		String_t* L_91 = UriParser_get_SchemeName_mFC9EFD71512A64E640866792CCB7DAC5187DE9F1(L_90, /*hidden argument*/NULL);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_92 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_VsMacrosUri_22();
		NullCheck(L_89);
		Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_89, L_91, L_92, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
		return;
	}
}
// System.UriSyntaxFlags System.UriParser::get_Flags()
extern "C" IL2CPP_METHOD_ATTR int32_t UriParser_get_Flags_mBCF4C3E94892F00B6E8856BFED1B650FB6A0C039 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, const RuntimeMethod* method)
{
	{
		int32_t L_0 = __this->get_m_Flags_2();
		return L_0;
	}
}
// System.Boolean System.UriParser::NotAny(System.UriSyntaxFlags)
extern "C" IL2CPP_METHOD_ATTR bool UriParser_NotAny_mC998A35DC290F35FFAFFB6A8B66C7B881F2559D3 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, int32_t ___flags0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___flags0;
		bool L_1 = UriParser_IsFullMatch_m7B5F47A62FA721E550C5439FAA4C6AFAC34EB23E(__this, L_0, 0, /*hidden argument*/NULL);
		return L_1;
	}
}
// System.Boolean System.UriParser::InFact(System.UriSyntaxFlags)
extern "C" IL2CPP_METHOD_ATTR bool UriParser_InFact_mDD42FA932B6830D99AA04C2AE7875BA5067C86F3 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, int32_t ___flags0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___flags0;
		bool L_1 = UriParser_IsFullMatch_m7B5F47A62FA721E550C5439FAA4C6AFAC34EB23E(__this, L_0, 0, /*hidden argument*/NULL);
		return (bool)((((int32_t)L_1) == ((int32_t)0))? 1 : 0);
	}
}
// System.Boolean System.UriParser::IsAllSet(System.UriSyntaxFlags)
extern "C" IL2CPP_METHOD_ATTR bool UriParser_IsAllSet_m74BEC412DC8AF3B1A33E11964EBB3164D9D8C77E (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, int32_t ___flags0, const RuntimeMethod* method)
{
	{
		int32_t L_0 = ___flags0;
		int32_t L_1 = ___flags0;
		bool L_2 = UriParser_IsFullMatch_m7B5F47A62FA721E550C5439FAA4C6AFAC34EB23E(__this, L_0, L_1, /*hidden argument*/NULL);
		return L_2;
	}
}
// System.Boolean System.UriParser::IsFullMatch(System.UriSyntaxFlags,System.UriSyntaxFlags)
extern "C" IL2CPP_METHOD_ATTR bool UriParser_IsFullMatch_m7B5F47A62FA721E550C5439FAA4C6AFAC34EB23E (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, int32_t ___flags0, int32_t ___expected1, const RuntimeMethod* method)
{
	int32_t V_0 = 0;
	{
		int32_t L_0 = ___flags0;
		if (!((int32_t)((int32_t)L_0&(int32_t)((int32_t)33554432))))
		{
			goto IL_0013;
		}
	}
	{
		bool L_1 = __this->get_m_UpdatableFlagsUsed_4();
		il2cpp_codegen_memory_barrier();
		if (L_1)
		{
			goto IL_001c;
		}
	}

IL_0013:
	{
		int32_t L_2 = __this->get_m_Flags_2();
		V_0 = L_2;
		goto IL_0032;
	}

IL_001c:
	{
		int32_t L_3 = __this->get_m_Flags_2();
		int32_t L_4 = __this->get_m_UpdatableFlags_3();
		il2cpp_codegen_memory_barrier();
		V_0 = ((int32_t)((int32_t)((int32_t)((int32_t)L_3&(int32_t)((int32_t)-33554433)))|(int32_t)L_4));
	}

IL_0032:
	{
		int32_t L_5 = V_0;
		int32_t L_6 = ___flags0;
		int32_t L_7 = ___expected1;
		return (bool)((((int32_t)((int32_t)((int32_t)L_5&(int32_t)L_6))) == ((int32_t)L_7))? 1 : 0);
	}
}
// System.Void System.UriParser::.ctor(System.UriSyntaxFlags)
extern "C" IL2CPP_METHOD_ATTR void UriParser__ctor_mAF168F2B88BC5301B722C1BAAD45E381FBA22E3D (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, int32_t ___flags0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriParser__ctor_mAF168F2B88BC5301B722C1BAAD45E381FBA22E3D_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Object__ctor_m925ECA5E85CA100E3FB86A4F9E15C120E9A184C0(__this, /*hidden argument*/NULL);
		int32_t L_0 = ___flags0;
		__this->set_m_Flags_2(L_0);
		String_t* L_1 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->get_Empty_5();
		__this->set_m_Scheme_6(L_1);
		return;
	}
}
// System.UriParser System.UriParser::FindOrFetchAsUnknownV1Syntax(System.String)
extern "C" IL2CPP_METHOD_ATTR UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * UriParser_FindOrFetchAsUnknownV1Syntax_m3A57CA15FE27DC7982F186E8321B810B56EBD9AD (String_t* ___lwrCaseScheme0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriParser_FindOrFetchAsUnknownV1Syntax_m3A57CA15FE27DC7982F186E8321B810B56EBD9AD_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * V_0 = NULL;
	Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * V_1 = NULL;
	bool V_2 = false;
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * V_3 = NULL;
	Exception_t * __last_unhandled_exception = 0;
	NO_UNUSED_WARNING (__last_unhandled_exception);
	Exception_t * __exception_local = 0;
	NO_UNUSED_WARNING (__exception_local);
	int32_t __leave_target = -1;
	NO_UNUSED_WARNING (__leave_target);
	{
		V_0 = (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC *)NULL;
		IL2CPP_RUNTIME_CLASS_INIT(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_0 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		String_t* L_1 = ___lwrCaseScheme0;
		NullCheck(L_0);
		Dictionary_2_TryGetValue_mB7FEE5E187FD932CA98FA958AFCC096E123BCDC4(L_0, L_1, (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC **)(&V_0), /*hidden argument*/Dictionary_2_TryGetValue_mB7FEE5E187FD932CA98FA958AFCC096E123BCDC4_RuntimeMethod_var);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_2 = V_0;
		if (!L_2)
		{
			goto IL_0015;
		}
	}
	{
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_3 = V_0;
		return L_3;
	}

IL_0015:
	{
		IL2CPP_RUNTIME_CLASS_INIT(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_4 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_TempTable_1();
		String_t* L_5 = ___lwrCaseScheme0;
		NullCheck(L_4);
		Dictionary_2_TryGetValue_mB7FEE5E187FD932CA98FA958AFCC096E123BCDC4(L_4, L_5, (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC **)(&V_0), /*hidden argument*/Dictionary_2_TryGetValue_mB7FEE5E187FD932CA98FA958AFCC096E123BCDC4_RuntimeMethod_var);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_6 = V_0;
		if (!L_6)
		{
			goto IL_0028;
		}
	}
	{
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_7 = V_0;
		return L_7;
	}

IL_0028:
	{
		IL2CPP_RUNTIME_CLASS_INIT(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_8 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		V_1 = L_8;
		V_2 = (bool)0;
	}

IL_0030:
	try
	{ // begin try (depth: 1)
		{
			Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_9 = V_1;
			Monitor_Enter_mC5B353DD83A0B0155DF6FBCC4DF5A580C25534C5(L_9, (bool*)(&V_2), /*hidden argument*/NULL);
			IL2CPP_RUNTIME_CLASS_INIT(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var);
			Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_10 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_TempTable_1();
			NullCheck(L_10);
			int32_t L_11 = Dictionary_2_get_Count_mEC5A51E9EC624CA697AFE307D4CD767026962AE3(L_10, /*hidden argument*/Dictionary_2_get_Count_mEC5A51E9EC624CA697AFE307D4CD767026962AE3_RuntimeMethod_var);
			if ((((int32_t)L_11) < ((int32_t)((int32_t)512))))
			{
				goto IL_0055;
			}
		}

IL_0049:
		{
			Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_12 = (Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE *)il2cpp_codegen_object_new(Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE_il2cpp_TypeInfo_var);
			Dictionary_2__ctor_m9AA6FFC23A9032DF2BF483986951F06E722B3445(L_12, ((int32_t)25), /*hidden argument*/Dictionary_2__ctor_m9AA6FFC23A9032DF2BF483986951F06E722B3445_RuntimeMethod_var);
			IL2CPP_RUNTIME_CLASS_INIT(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var);
			((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->set_m_TempTable_1(L_12);
		}

IL_0055:
		{
			String_t* L_13 = ___lwrCaseScheme0;
			BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * L_14 = (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B *)il2cpp_codegen_object_new(BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B_il2cpp_TypeInfo_var);
			BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C(L_14, L_13, (-1), ((int32_t)351342590), /*hidden argument*/NULL);
			V_0 = L_14;
			IL2CPP_RUNTIME_CLASS_INIT(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var);
			Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_15 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_TempTable_1();
			String_t* L_16 = ___lwrCaseScheme0;
			UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_17 = V_0;
			NullCheck(L_15);
			Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB(L_15, L_16, L_17, /*hidden argument*/Dictionary_2_set_Item_mB84FA35EFF6271F4923FCAF307D576087CD554AB_RuntimeMethod_var);
			UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_18 = V_0;
			V_3 = L_18;
			IL2CPP_LEAVE(0x7C, FINALLY_0072);
		}
	} // end try (depth: 1)
	catch(Il2CppExceptionWrapper& e)
	{
		__last_unhandled_exception = (Exception_t *)e.ex;
		goto FINALLY_0072;
	}

FINALLY_0072:
	{ // begin finally (depth: 1)
		{
			bool L_19 = V_2;
			if (!L_19)
			{
				goto IL_007b;
			}
		}

IL_0075:
		{
			Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_20 = V_1;
			Monitor_Exit_m49A1E5356D984D0B934BB97A305E2E5E207225C2(L_20, /*hidden argument*/NULL);
		}

IL_007b:
		{
			IL2CPP_RESET_LEAVE(0x7C);
			IL2CPP_END_FINALLY(114)
		}
	} // end finally (depth: 1)
	IL2CPP_CLEANUP(114)
	{
		IL2CPP_JUMP_TBL(0x7C, IL_007c)
		IL2CPP_RETHROW_IF_UNHANDLED(Exception_t *)
	}

IL_007c:
	{
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_21 = V_3;
		return L_21;
	}
}
// System.UriParser System.UriParser::GetSyntax(System.String)
extern "C" IL2CPP_METHOD_ATTR UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * UriParser_GetSyntax_mC2FEAF79ECEB6550573A1C0578141BB236F7EF16 (String_t* ___lwrCaseScheme0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriParser_GetSyntax_mC2FEAF79ECEB6550573A1C0578141BB236F7EF16_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * V_0 = NULL;
	{
		V_0 = (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC *)NULL;
		IL2CPP_RUNTIME_CLASS_INIT(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_0 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_Table_0();
		String_t* L_1 = ___lwrCaseScheme0;
		NullCheck(L_0);
		Dictionary_2_TryGetValue_mB7FEE5E187FD932CA98FA958AFCC096E123BCDC4(L_0, L_1, (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC **)(&V_0), /*hidden argument*/Dictionary_2_TryGetValue_mB7FEE5E187FD932CA98FA958AFCC096E123BCDC4_RuntimeMethod_var);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_2 = V_0;
		if (L_2)
		{
			goto IL_0021;
		}
	}
	{
		IL2CPP_RUNTIME_CLASS_INIT(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var);
		Dictionary_2_tB0B3F0D7A7E98EDBC0C35218EEA8560D1F0CCFCE * L_3 = ((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_StaticFields*)il2cpp_codegen_static_fields_for(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var))->get_m_TempTable_1();
		String_t* L_4 = ___lwrCaseScheme0;
		NullCheck(L_3);
		Dictionary_2_TryGetValue_mB7FEE5E187FD932CA98FA958AFCC096E123BCDC4(L_3, L_4, (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC **)(&V_0), /*hidden argument*/Dictionary_2_TryGetValue_mB7FEE5E187FD932CA98FA958AFCC096E123BCDC4_RuntimeMethod_var);
	}

IL_0021:
	{
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_5 = V_0;
		return L_5;
	}
}
// System.Boolean System.UriParser::get_IsSimple()
extern "C" IL2CPP_METHOD_ATTR bool UriParser_get_IsSimple_mDDB03A5F6EEE6E92926A386655E5BBD553719B9C (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, const RuntimeMethod* method)
{
	{
		bool L_0 = UriParser_InFact_mDD42FA932B6830D99AA04C2AE7875BA5067C86F3(__this, ((int32_t)131072), /*hidden argument*/NULL);
		return L_0;
	}
}
// System.UriParser System.UriParser::InternalOnNewUri()
extern "C" IL2CPP_METHOD_ATTR UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * UriParser_InternalOnNewUri_m7D55F5CD59A3B9BF57BC68F715A27CC1A44566CA (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, const RuntimeMethod* method)
{
	UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * V_0 = NULL;
	{
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_0 = VirtFuncInvoker0< UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * >::Invoke(4 /* System.UriParser System.UriParser::OnNewUri() */, __this);
		V_0 = L_0;
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_1 = V_0;
		if ((((RuntimeObject*)(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC *)__this) == ((RuntimeObject*)(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC *)L_1)))
		{
			goto IL_002f;
		}
	}
	{
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_2 = V_0;
		String_t* L_3 = __this->get_m_Scheme_6();
		NullCheck(L_2);
		L_2->set_m_Scheme_6(L_3);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_4 = V_0;
		int32_t L_5 = __this->get_m_Port_5();
		NullCheck(L_4);
		L_4->set_m_Port_5(L_5);
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_6 = V_0;
		int32_t L_7 = __this->get_m_Flags_2();
		NullCheck(L_6);
		L_6->set_m_Flags_2(L_7);
	}

IL_002f:
	{
		UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * L_8 = V_0;
		return L_8;
	}
}
// System.Void System.UriParser::InternalValidate(System.Uri,System.UriFormatExceptionU26)
extern "C" IL2CPP_METHOD_ATTR void UriParser_InternalValidate_mF2FEB0E76E48B621EB2058FBE7DCC6A42A1681E2 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___thisUri0, UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A ** ___parsingError1, const RuntimeMethod* method)
{
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_0 = ___thisUri0;
		UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A ** L_1 = ___parsingError1;
		VirtActionInvoker2< Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *, UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A ** >::Invoke(5 /* System.Void System.UriParser::InitializeAndValidate(System.Uri,System.UriFormatException&) */, __this, L_0, (UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A **)L_1);
		return;
	}
}
// System.String System.UriParser::InternalResolve(System.Uri,System.Uri,System.UriFormatExceptionU26)
extern "C" IL2CPP_METHOD_ATTR String_t* UriParser_InternalResolve_m2A027789CB5105E32B09810E81810E8E35DD1F26 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___thisBaseUri0, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___uriLink1, UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A ** ___parsingError2, const RuntimeMethod* method)
{
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_0 = ___thisBaseUri0;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_1 = ___uriLink1;
		UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A ** L_2 = ___parsingError2;
		String_t* L_3 = VirtFuncInvoker3< String_t*, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *, UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A ** >::Invoke(6 /* System.String System.UriParser::Resolve(System.Uri,System.Uri,System.UriFormatException&) */, __this, L_0, L_1, (UriFormatException_t86B375C9E56DBEE5BD4CC9D71C4C40AE5141808A **)L_2);
		return L_3;
	}
}
// System.String System.UriParser::InternalGetComponents(System.Uri,System.UriComponents,System.UriFormat)
extern "C" IL2CPP_METHOD_ATTR String_t* UriParser_InternalGetComponents_mFD4B211C71E0506AE4E4E99D92ECAF1780CE4674 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___thisUri0, int32_t ___uriComponents1, int32_t ___uriFormat2, const RuntimeMethod* method)
{
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_0 = ___thisUri0;
		int32_t L_1 = ___uriComponents1;
		int32_t L_2 = ___uriFormat2;
		String_t* L_3 = VirtFuncInvoker3< String_t*, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *, int32_t, int32_t >::Invoke(7 /* System.String System.UriParser::GetComponents(System.Uri,System.UriComponents,System.UriFormat) */, __this, L_0, L_1, L_2);
		return L_3;
	}
}
// System.Boolean System.UriParser::InternalIsWellFormedOriginalString(System.Uri)
extern "C" IL2CPP_METHOD_ATTR bool UriParser_InternalIsWellFormedOriginalString_mC28478C9513F7B7C7B38CFE1660C6B10D2F4F973 (UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC * __this, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * ___thisUri0, const RuntimeMethod* method)
{
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_0 = ___thisUri0;
		bool L_1 = VirtFuncInvoker1< bool, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * >::Invoke(8 /* System.Boolean System.UriParser::IsWellFormedOriginalString(System.Uri) */, __this, L_0);
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
// System.Void System.UriParser_BuiltInUriParser::.ctor(System.String,System.Int32,System.UriSyntaxFlags)
extern "C" IL2CPP_METHOD_ATTR void BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C (BuiltInUriParser_t5C00B9ABDAFDD2FFEAAA5C4A6FF01FF0BE58E57B * __this, String_t* ___lwrCaseScheme0, int32_t ___defaultPort1, int32_t ___syntaxFlags2, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (BuiltInUriParser__ctor_m66250DC53CE01410149D46279D0B413FC1C5CA1C_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		int32_t L_0 = ___syntaxFlags2;
		IL2CPP_RUNTIME_CLASS_INIT(UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC_il2cpp_TypeInfo_var);
		UriParser__ctor_mAF168F2B88BC5301B722C1BAAD45E381FBA22E3D(__this, ((int32_t)((int32_t)((int32_t)((int32_t)L_0|(int32_t)((int32_t)131072)))|(int32_t)((int32_t)262144))), /*hidden argument*/NULL);
		String_t* L_1 = ___lwrCaseScheme0;
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC *)__this)->set_m_Scheme_6(L_1);
		int32_t L_2 = ___defaultPort1;
		((UriParser_t07C77D673CCE8D2DA253B8A7ACCB010147F1A4AC *)__this)->set_m_Port_5(L_2);
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
// System.Void System.UriTypeConverter::.ctor()
extern "C" IL2CPP_METHOD_ATTR void UriTypeConverter__ctor_m1CAEEF1C615B28212B83C76D892938E0A77D3A64 (UriTypeConverter_t96793526764A246FBAEE2F4F639AFAF270EE81D1 * __this, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriTypeConverter__ctor_m1CAEEF1C615B28212B83C76D892938E0A77D3A64_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		IL2CPP_RUNTIME_CLASS_INIT(TypeConverter_t8306AE03734853B551DDF089C1F17836A7764DBB_il2cpp_TypeInfo_var);
		TypeConverter__ctor_m7F8A006E775CCB83A8ACB042B296E48B0AE501CD(__this, /*hidden argument*/NULL);
		return;
	}
}
// System.Boolean System.UriTypeConverter::CanConvert(System.Type)
extern "C" IL2CPP_METHOD_ATTR bool UriTypeConverter_CanConvert_m0F0FB34A1DC16C677BF8F4ED0E720144C17C4795 (UriTypeConverter_t96793526764A246FBAEE2F4F639AFAF270EE81D1 * __this, Type_t * ___type0, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriTypeConverter_CanConvert_m0F0FB34A1DC16C677BF8F4ED0E720144C17C4795_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Type_t * L_0 = ___type0;
		RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D  L_1 = { reinterpret_cast<intptr_t> (String_t_0_0_0_var) };
		IL2CPP_RUNTIME_CLASS_INIT(Type_t_il2cpp_TypeInfo_var);
		Type_t * L_2 = Type_GetTypeFromHandle_m9DC58ADF0512987012A8A016FB64B068F3B1AFF6(L_1, /*hidden argument*/NULL);
		bool L_3 = Type_op_Equality_m7040622C9E1037EFC73E1F0EDB1DD241282BE3D8(L_0, L_2, /*hidden argument*/NULL);
		if (!L_3)
		{
			goto IL_0014;
		}
	}
	{
		return (bool)1;
	}

IL_0014:
	{
		Type_t * L_4 = ___type0;
		RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D  L_5 = { reinterpret_cast<intptr_t> (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_0_0_0_var) };
		IL2CPP_RUNTIME_CLASS_INIT(Type_t_il2cpp_TypeInfo_var);
		Type_t * L_6 = Type_GetTypeFromHandle_m9DC58ADF0512987012A8A016FB64B068F3B1AFF6(L_5, /*hidden argument*/NULL);
		bool L_7 = Type_op_Equality_m7040622C9E1037EFC73E1F0EDB1DD241282BE3D8(L_4, L_6, /*hidden argument*/NULL);
		if (!L_7)
		{
			goto IL_0028;
		}
	}
	{
		return (bool)1;
	}

IL_0028:
	{
		return (bool)0;
	}
}
// System.Boolean System.UriTypeConverter::CanConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Type)
extern "C" IL2CPP_METHOD_ATTR bool UriTypeConverter_CanConvertFrom_m1D18F7B5924B6B682AB1CC90FB814DC3331DFF47 (UriTypeConverter_t96793526764A246FBAEE2F4F639AFAF270EE81D1 * __this, RuntimeObject* ___context0, Type_t * ___sourceType1, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriTypeConverter_CanConvertFrom_m1D18F7B5924B6B682AB1CC90FB814DC3331DFF47_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Type_t * L_0 = ___sourceType1;
		IL2CPP_RUNTIME_CLASS_INIT(Type_t_il2cpp_TypeInfo_var);
		bool L_1 = Type_op_Equality_m7040622C9E1037EFC73E1F0EDB1DD241282BE3D8(L_0, (Type_t *)NULL, /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_0014;
		}
	}
	{
		ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD * L_2 = (ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD *)il2cpp_codegen_object_new(ArgumentNullException_t581DF992B1F3E0EC6EFB30CC5DC43519A79B27AD_il2cpp_TypeInfo_var);
		ArgumentNullException__ctor_mEE0C0D6FCB2D08CD7967DBB1329A0854BBED49ED(L_2, _stringLiteral11C5773832E60D2F376C6E197271A225FD74EE27, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_2, NULL, UriTypeConverter_CanConvertFrom_m1D18F7B5924B6B682AB1CC90FB814DC3331DFF47_RuntimeMethod_var);
	}

IL_0014:
	{
		Type_t * L_3 = ___sourceType1;
		bool L_4 = UriTypeConverter_CanConvert_m0F0FB34A1DC16C677BF8F4ED0E720144C17C4795(__this, L_3, /*hidden argument*/NULL);
		return L_4;
	}
}
// System.Boolean System.UriTypeConverter::CanConvertTo(System.ComponentModel.ITypeDescriptorContext,System.Type)
extern "C" IL2CPP_METHOD_ATTR bool UriTypeConverter_CanConvertTo_mC19530C1DD75AC92C20697EFDD0A0E2DB568E099 (UriTypeConverter_t96793526764A246FBAEE2F4F639AFAF270EE81D1 * __this, RuntimeObject* ___context0, Type_t * ___destinationType1, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriTypeConverter_CanConvertTo_mC19530C1DD75AC92C20697EFDD0A0E2DB568E099_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		Type_t * L_0 = ___destinationType1;
		IL2CPP_RUNTIME_CLASS_INIT(Type_t_il2cpp_TypeInfo_var);
		bool L_1 = Type_op_Equality_m7040622C9E1037EFC73E1F0EDB1DD241282BE3D8(L_0, (Type_t *)NULL, /*hidden argument*/NULL);
		if (!L_1)
		{
			goto IL_000b;
		}
	}
	{
		return (bool)0;
	}

IL_000b:
	{
		Type_t * L_2 = ___destinationType1;
		bool L_3 = UriTypeConverter_CanConvert_m0F0FB34A1DC16C677BF8F4ED0E720144C17C4795(__this, L_2, /*hidden argument*/NULL);
		return L_3;
	}
}
// System.Object System.UriTypeConverter::ConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,System.Object)
extern "C" IL2CPP_METHOD_ATTR RuntimeObject * UriTypeConverter_ConvertFrom_m2FE8479F26F35A578983E194038CF186D6CD2F85 (UriTypeConverter_t96793526764A246FBAEE2F4F639AFAF270EE81D1 * __this, RuntimeObject* ___context0, CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * ___culture1, RuntimeObject * ___value2, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriTypeConverter_ConvertFrom_m2FE8479F26F35A578983E194038CF186D6CD2F85_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	String_t* V_0 = NULL;
	{
		RuntimeObject * L_0 = ___value2;
		if (L_0)
		{
			goto IL_0013;
		}
	}
	{
		String_t* L_1 = Locale_GetText_m41F0CB4E76BAAB1E97D9D92D109C846A8ECC1324(_stringLiteral6ABF563E8335FCAA5CA55811FECE36F4B0D6DC07, /*hidden argument*/NULL);
		NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 * L_2 = (NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 *)il2cpp_codegen_object_new(NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010_il2cpp_TypeInfo_var);
		NotSupportedException__ctor_mD023A89A5C1F740F43F0A9CD6C49DC21230B3CEE(L_2, L_1, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_2, NULL, UriTypeConverter_ConvertFrom_m2FE8479F26F35A578983E194038CF186D6CD2F85_RuntimeMethod_var);
	}

IL_0013:
	{
		RuntimeObject* L_3 = ___context0;
		RuntimeObject * L_4 = ___value2;
		NullCheck(L_4);
		Type_t * L_5 = Object_GetType_m2E0B62414ECCAA3094B703790CE88CBB2F83EA60(L_4, /*hidden argument*/NULL);
		bool L_6 = VirtFuncInvoker2< bool, RuntimeObject*, Type_t * >::Invoke(4 /* System.Boolean System.ComponentModel.TypeConverter::CanConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Type) */, __this, L_3, L_5);
		if (L_6)
		{
			goto IL_0032;
		}
	}
	{
		String_t* L_7 = Locale_GetText_m41F0CB4E76BAAB1E97D9D92D109C846A8ECC1324(_stringLiteral6ABF563E8335FCAA5CA55811FECE36F4B0D6DC07, /*hidden argument*/NULL);
		NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 * L_8 = (NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 *)il2cpp_codegen_object_new(NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010_il2cpp_TypeInfo_var);
		NotSupportedException__ctor_mD023A89A5C1F740F43F0A9CD6C49DC21230B3CEE(L_8, L_7, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_8, NULL, UriTypeConverter_ConvertFrom_m2FE8479F26F35A578983E194038CF186D6CD2F85_RuntimeMethod_var);
	}

IL_0032:
	{
		RuntimeObject * L_9 = ___value2;
		if (!((Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)IsInstClass((RuntimeObject*)L_9, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var)))
		{
			goto IL_003c;
		}
	}
	{
		RuntimeObject * L_10 = ___value2;
		return L_10;
	}

IL_003c:
	{
		RuntimeObject * L_11 = ___value2;
		V_0 = ((String_t*)IsInstSealed((RuntimeObject*)L_11, String_t_il2cpp_TypeInfo_var));
		String_t* L_12 = V_0;
		if (!L_12)
		{
			goto IL_005d;
		}
	}
	{
		String_t* L_13 = V_0;
		bool L_14 = String_op_Equality_m139F0E4195AE2F856019E63B241F36F016997FCE(L_13, _stringLiteralDA39A3EE5E6B4B0D3255BFEF95601890AFD80709, /*hidden argument*/NULL);
		if (!L_14)
		{
			goto IL_0055;
		}
	}
	{
		return NULL;
	}

IL_0055:
	{
		String_t* L_15 = V_0;
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_16 = (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)il2cpp_codegen_object_new(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		Uri__ctor_mA02DB222F4F35380DE2700D84F58EB42497FDDE4(L_16, L_15, 0, /*hidden argument*/NULL);
		return L_16;
	}

IL_005d:
	{
		RuntimeObject* L_17 = ___context0;
		CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * L_18 = ___culture1;
		RuntimeObject * L_19 = ___value2;
		RuntimeObject * L_20 = TypeConverter_ConvertFrom_mD5AE49E422520F6E07B3C0D6202788E49B4698A3(__this, L_17, L_18, L_19, /*hidden argument*/NULL);
		return L_20;
	}
}
// System.Object System.UriTypeConverter::ConvertTo(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,System.Object,System.Type)
extern "C" IL2CPP_METHOD_ATTR RuntimeObject * UriTypeConverter_ConvertTo_m2059A4086714BACA32E7618BD97713CCD5DCFEF4 (UriTypeConverter_t96793526764A246FBAEE2F4F639AFAF270EE81D1 * __this, RuntimeObject* ___context0, CultureInfo_t345AC6924134F039ED9A11F3E03F8E91B6A3225F * ___culture1, RuntimeObject * ___value2, Type_t * ___destinationType3, const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (UriTypeConverter_ConvertTo_m2059A4086714BACA32E7618BD97713CCD5DCFEF4_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * V_0 = NULL;
	{
		RuntimeObject* L_0 = ___context0;
		Type_t * L_1 = ___destinationType3;
		bool L_2 = VirtFuncInvoker2< bool, RuntimeObject*, Type_t * >::Invoke(5 /* System.Boolean System.ComponentModel.TypeConverter::CanConvertTo(System.ComponentModel.ITypeDescriptorContext,System.Type) */, __this, L_0, L_1);
		if (L_2)
		{
			goto IL_001b;
		}
	}
	{
		String_t* L_3 = Locale_GetText_m41F0CB4E76BAAB1E97D9D92D109C846A8ECC1324(_stringLiteral2E0BECBCAE1D61359E07C21D53B187CD25DCC4B1, /*hidden argument*/NULL);
		NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 * L_4 = (NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 *)il2cpp_codegen_object_new(NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010_il2cpp_TypeInfo_var);
		NotSupportedException__ctor_mD023A89A5C1F740F43F0A9CD6C49DC21230B3CEE(L_4, L_3, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_4, NULL, UriTypeConverter_ConvertTo_m2059A4086714BACA32E7618BD97713CCD5DCFEF4_RuntimeMethod_var);
	}

IL_001b:
	{
		RuntimeObject * L_5 = ___value2;
		V_0 = ((Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)IsInstClass((RuntimeObject*)L_5, Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var));
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_6 = V_0;
		IL2CPP_RUNTIME_CLASS_INIT(Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_il2cpp_TypeInfo_var);
		bool L_7 = Uri_op_Inequality_m07015206F59460E87CDE2A8D303D5712E30A7F6B(L_6, (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E *)NULL, /*hidden argument*/NULL);
		if (!L_7)
		{
			goto IL_006a;
		}
	}
	{
		Type_t * L_8 = ___destinationType3;
		RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D  L_9 = { reinterpret_cast<intptr_t> (String_t_0_0_0_var) };
		IL2CPP_RUNTIME_CLASS_INIT(Type_t_il2cpp_TypeInfo_var);
		Type_t * L_10 = Type_GetTypeFromHandle_m9DC58ADF0512987012A8A016FB64B068F3B1AFF6(L_9, /*hidden argument*/NULL);
		bool L_11 = Type_op_Equality_m7040622C9E1037EFC73E1F0EDB1DD241282BE3D8(L_8, L_10, /*hidden argument*/NULL);
		if (!L_11)
		{
			goto IL_0045;
		}
	}
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_12 = V_0;
		NullCheck(L_12);
		String_t* L_13 = VirtFuncInvoker0< String_t* >::Invoke(3 /* System.String System.Object::ToString() */, L_12);
		return L_13;
	}

IL_0045:
	{
		Type_t * L_14 = ___destinationType3;
		RuntimeTypeHandle_t7B542280A22F0EC4EAC2061C29178845847A8B2D  L_15 = { reinterpret_cast<intptr_t> (Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E_0_0_0_var) };
		IL2CPP_RUNTIME_CLASS_INIT(Type_t_il2cpp_TypeInfo_var);
		Type_t * L_16 = Type_GetTypeFromHandle_m9DC58ADF0512987012A8A016FB64B068F3B1AFF6(L_15, /*hidden argument*/NULL);
		bool L_17 = Type_op_Equality_m7040622C9E1037EFC73E1F0EDB1DD241282BE3D8(L_14, L_16, /*hidden argument*/NULL);
		if (!L_17)
		{
			goto IL_005a;
		}
	}
	{
		Uri_t87E4A94B2901F5EEDD18AA72C3DB1B00E672D68E * L_18 = V_0;
		return L_18;
	}

IL_005a:
	{
		String_t* L_19 = Locale_GetText_m41F0CB4E76BAAB1E97D9D92D109C846A8ECC1324(_stringLiteral2E0BECBCAE1D61359E07C21D53B187CD25DCC4B1, /*hidden argument*/NULL);
		NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 * L_20 = (NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 *)il2cpp_codegen_object_new(NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010_il2cpp_TypeInfo_var);
		NotSupportedException__ctor_mD023A89A5C1F740F43F0A9CD6C49DC21230B3CEE(L_20, L_19, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_20, NULL, UriTypeConverter_ConvertTo_m2059A4086714BACA32E7618BD97713CCD5DCFEF4_RuntimeMethod_var);
	}

IL_006a:
	{
		String_t* L_21 = Locale_GetText_m41F0CB4E76BAAB1E97D9D92D109C846A8ECC1324(_stringLiteral2E0BECBCAE1D61359E07C21D53B187CD25DCC4B1, /*hidden argument*/NULL);
		NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 * L_22 = (NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010 *)il2cpp_codegen_object_new(NotSupportedException_tE75B318D6590A02A5D9B29FD97409B1750FA0010_il2cpp_TypeInfo_var);
		NotSupportedException__ctor_mD023A89A5C1F740F43F0A9CD6C49DC21230B3CEE(L_22, L_21, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_22, NULL, UriTypeConverter_ConvertTo_m2059A4086714BACA32E7618BD97713CCD5DCFEF4_RuntimeMethod_var);
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
// System.Void Unity.ThrowStub::ThrowNotSupportedException()
extern "C" IL2CPP_METHOD_ATTR void ThrowStub_ThrowNotSupportedException_mF1DE187697F740D8C18B8966BBEB276878CD86FD (const RuntimeMethod* method)
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_method (ThrowStub_ThrowNotSupportedException_mF1DE187697F740D8C18B8966BBEB276878CD86FD_MetadataUsageId);
		s_Il2CppMethodInitialized = true;
	}
	{
		PlatformNotSupportedException_t14FE109377F8FA8B3B2F9A0C4FE3BF10662C73B5 * L_0 = (PlatformNotSupportedException_t14FE109377F8FA8B3B2F9A0C4FE3BF10662C73B5 *)il2cpp_codegen_object_new(PlatformNotSupportedException_t14FE109377F8FA8B3B2F9A0C4FE3BF10662C73B5_il2cpp_TypeInfo_var);
		PlatformNotSupportedException__ctor_m651139B17C9EE918551490BC675754EA8EA3E7C7(L_0, /*hidden argument*/NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_0, NULL, ThrowStub_ThrowNotSupportedException_mF1DE187697F740D8C18B8966BBEB276878CD86FD_RuntimeMethod_var);
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
