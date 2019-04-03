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


// System.Char[]
struct CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2;
// System.Collections.ArrayList
struct ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4;
// System.Collections.Hashtable
struct Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9;
// System.Collections.Hashtable/bucket[]
struct bucketU5BU5D_t6FF2C2C4B21F2206885CD19A78F68B874C8DC84A;
// System.Collections.ICollection
struct ICollection_tA3BAB2482E28132A7CA9E0E21393027353C28B54;
// System.Collections.IComparer
struct IComparer_t6A5E1BC727C7FF28888E407A797CE1ED92DA8E95;
// System.Collections.IDictionary
struct IDictionary_t1BD5C1546718A374EA8122FBD6C6EE45331E8CE7;
// System.Collections.IEnumerator
struct IEnumerator_t8789118187258CC88B77AFAC6315B5AF87D3E18A;
// System.Collections.IEqualityComparer
struct IEqualityComparer_t3102D0F5BABD60224F6DFF4815BCA1045831FB7C;
// System.Collections.IHashCodeProvider
struct IHashCodeProvider_tEA652F45F84FA62675B746607F7AAFA71515D856;
// System.Collections.Specialized.ListDictionary
struct ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D;
// System.Collections.Specialized.ListDictionary/DictionaryNode
struct DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E;
// System.Collections.Specialized.NameObjectCollectionBase
struct NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D;
// System.Collections.Specialized.NameObjectCollectionBase/NameObjectEntry
struct NameObjectEntry_tC137E0E1F256300B1F9F0ED88EE02B6611918B54;
// System.Diagnostics.StackTrace[]
struct StackTraceU5BU5D_t855F09649EA34DEE7C1B6F088E0538E3CCC3F196;
// System.IntPtr[]
struct IntPtrU5BU5D_t4DC01DCB9A6DF6C9792A6513595D7A11E637DCDD;
// System.Net.Cache.RequestCache
struct RequestCache_t41E1BB0AF0CD41C778E51C62180B844887B6EAF8;
// System.Net.Cache.RequestCacheValidator
struct RequestCacheValidator_t21A4A8B8ECF2EDCDD0C34B0D26FCAB2F77190F41;
// System.Net.IWebProxy
struct IWebProxy_tA24C0862A1ACA35D20FD079E2672CA5786C1A67E;
// System.Runtime.Serialization.SafeSerializationManager
struct SafeSerializationManager_t4A754D86B0F784B18CBC36C073BA564BED109770;
// System.Runtime.Serialization.SerializationInfo
struct SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26;
// System.String
struct String_t;
// System.StringComparer
struct StringComparer_t588BC7FEF85D6E7425E0A8147A3D5A334F1F82DE;
// System.String[]
struct StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E;
// System.Void
struct Void_t22962CB4C05B1D89B55A6E1139F0E87A90987017;

struct Exception_t_marshaled_com;
struct Exception_t_marshaled_pinvoke;



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
#ifndef COMPATIBLECOMPARER_T3AF98635FCA9D8C4830435F5FEEC183B10385EBF_H
#define COMPATIBLECOMPARER_T3AF98635FCA9D8C4830435F5FEEC183B10385EBF_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Specialized.CompatibleComparer
struct  CompatibleComparer_t3AF98635FCA9D8C4830435F5FEEC183B10385EBF  : public RuntimeObject
{
public:
	// System.Collections.IComparer System.Collections.Specialized.CompatibleComparer::_comparer
	RuntimeObject* ____comparer_0;
	// System.Collections.IHashCodeProvider System.Collections.Specialized.CompatibleComparer::_hcp
	RuntimeObject* ____hcp_2;

public:
	inline static int32_t get_offset_of__comparer_0() { return static_cast<int32_t>(offsetof(CompatibleComparer_t3AF98635FCA9D8C4830435F5FEEC183B10385EBF, ____comparer_0)); }
	inline RuntimeObject* get__comparer_0() const { return ____comparer_0; }
	inline RuntimeObject** get_address_of__comparer_0() { return &____comparer_0; }
	inline void set__comparer_0(RuntimeObject* value)
	{
		____comparer_0 = value;
		Il2CppCodeGenWriteBarrier((&____comparer_0), value);
	}

	inline static int32_t get_offset_of__hcp_2() { return static_cast<int32_t>(offsetof(CompatibleComparer_t3AF98635FCA9D8C4830435F5FEEC183B10385EBF, ____hcp_2)); }
	inline RuntimeObject* get__hcp_2() const { return ____hcp_2; }
	inline RuntimeObject** get_address_of__hcp_2() { return &____hcp_2; }
	inline void set__hcp_2(RuntimeObject* value)
	{
		____hcp_2 = value;
		Il2CppCodeGenWriteBarrier((&____hcp_2), value);
	}
};

struct CompatibleComparer_t3AF98635FCA9D8C4830435F5FEEC183B10385EBF_StaticFields
{
public:
	// System.Collections.IComparer modreq(System.Runtime.CompilerServices.IsVolatile) System.Collections.Specialized.CompatibleComparer::defaultComparer
	RuntimeObject* ___defaultComparer_1;
	// System.Collections.IHashCodeProvider modreq(System.Runtime.CompilerServices.IsVolatile) System.Collections.Specialized.CompatibleComparer::defaultHashProvider
	RuntimeObject* ___defaultHashProvider_3;

public:
	inline static int32_t get_offset_of_defaultComparer_1() { return static_cast<int32_t>(offsetof(CompatibleComparer_t3AF98635FCA9D8C4830435F5FEEC183B10385EBF_StaticFields, ___defaultComparer_1)); }
	inline RuntimeObject* get_defaultComparer_1() const { return ___defaultComparer_1; }
	inline RuntimeObject** get_address_of_defaultComparer_1() { return &___defaultComparer_1; }
	inline void set_defaultComparer_1(RuntimeObject* value)
	{
		___defaultComparer_1 = value;
		Il2CppCodeGenWriteBarrier((&___defaultComparer_1), value);
	}

	inline static int32_t get_offset_of_defaultHashProvider_3() { return static_cast<int32_t>(offsetof(CompatibleComparer_t3AF98635FCA9D8C4830435F5FEEC183B10385EBF_StaticFields, ___defaultHashProvider_3)); }
	inline RuntimeObject* get_defaultHashProvider_3() const { return ___defaultHashProvider_3; }
	inline RuntimeObject** get_address_of_defaultHashProvider_3() { return &___defaultHashProvider_3; }
	inline void set_defaultHashProvider_3(RuntimeObject* value)
	{
		___defaultHashProvider_3 = value;
		Il2CppCodeGenWriteBarrier((&___defaultHashProvider_3), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // COMPATIBLECOMPARER_T3AF98635FCA9D8C4830435F5FEEC183B10385EBF_H
#ifndef HYBRIDDICTIONARY_T885F953154C575D3408650DCE5D0B76F1EE4E549_H
#define HYBRIDDICTIONARY_T885F953154C575D3408650DCE5D0B76F1EE4E549_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Specialized.HybridDictionary
struct  HybridDictionary_t885F953154C575D3408650DCE5D0B76F1EE4E549  : public RuntimeObject
{
public:
	// System.Collections.Specialized.ListDictionary System.Collections.Specialized.HybridDictionary::list
	ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D * ___list_0;
	// System.Collections.Hashtable System.Collections.Specialized.HybridDictionary::hashtable
	Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * ___hashtable_1;
	// System.Boolean System.Collections.Specialized.HybridDictionary::caseInsensitive
	bool ___caseInsensitive_2;

public:
	inline static int32_t get_offset_of_list_0() { return static_cast<int32_t>(offsetof(HybridDictionary_t885F953154C575D3408650DCE5D0B76F1EE4E549, ___list_0)); }
	inline ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D * get_list_0() const { return ___list_0; }
	inline ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D ** get_address_of_list_0() { return &___list_0; }
	inline void set_list_0(ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D * value)
	{
		___list_0 = value;
		Il2CppCodeGenWriteBarrier((&___list_0), value);
	}

	inline static int32_t get_offset_of_hashtable_1() { return static_cast<int32_t>(offsetof(HybridDictionary_t885F953154C575D3408650DCE5D0B76F1EE4E549, ___hashtable_1)); }
	inline Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * get_hashtable_1() const { return ___hashtable_1; }
	inline Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 ** get_address_of_hashtable_1() { return &___hashtable_1; }
	inline void set_hashtable_1(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * value)
	{
		___hashtable_1 = value;
		Il2CppCodeGenWriteBarrier((&___hashtable_1), value);
	}

	inline static int32_t get_offset_of_caseInsensitive_2() { return static_cast<int32_t>(offsetof(HybridDictionary_t885F953154C575D3408650DCE5D0B76F1EE4E549, ___caseInsensitive_2)); }
	inline bool get_caseInsensitive_2() const { return ___caseInsensitive_2; }
	inline bool* get_address_of_caseInsensitive_2() { return &___caseInsensitive_2; }
	inline void set_caseInsensitive_2(bool value)
	{
		___caseInsensitive_2 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // HYBRIDDICTIONARY_T885F953154C575D3408650DCE5D0B76F1EE4E549_H
#ifndef LISTDICTIONARY_TE68C8A5DB37EB10F3AA958AB3BF214346402074D_H
#define LISTDICTIONARY_TE68C8A5DB37EB10F3AA958AB3BF214346402074D_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Specialized.ListDictionary
struct  ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D  : public RuntimeObject
{
public:
	// System.Collections.Specialized.ListDictionary_DictionaryNode System.Collections.Specialized.ListDictionary::head
	DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E * ___head_0;
	// System.Int32 System.Collections.Specialized.ListDictionary::version
	int32_t ___version_1;
	// System.Int32 System.Collections.Specialized.ListDictionary::count
	int32_t ___count_2;
	// System.Collections.IComparer System.Collections.Specialized.ListDictionary::comparer
	RuntimeObject* ___comparer_3;
	// System.Object System.Collections.Specialized.ListDictionary::_syncRoot
	RuntimeObject * ____syncRoot_4;

public:
	inline static int32_t get_offset_of_head_0() { return static_cast<int32_t>(offsetof(ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D, ___head_0)); }
	inline DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E * get_head_0() const { return ___head_0; }
	inline DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E ** get_address_of_head_0() { return &___head_0; }
	inline void set_head_0(DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E * value)
	{
		___head_0 = value;
		Il2CppCodeGenWriteBarrier((&___head_0), value);
	}

	inline static int32_t get_offset_of_version_1() { return static_cast<int32_t>(offsetof(ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D, ___version_1)); }
	inline int32_t get_version_1() const { return ___version_1; }
	inline int32_t* get_address_of_version_1() { return &___version_1; }
	inline void set_version_1(int32_t value)
	{
		___version_1 = value;
	}

	inline static int32_t get_offset_of_count_2() { return static_cast<int32_t>(offsetof(ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D, ___count_2)); }
	inline int32_t get_count_2() const { return ___count_2; }
	inline int32_t* get_address_of_count_2() { return &___count_2; }
	inline void set_count_2(int32_t value)
	{
		___count_2 = value;
	}

	inline static int32_t get_offset_of_comparer_3() { return static_cast<int32_t>(offsetof(ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D, ___comparer_3)); }
	inline RuntimeObject* get_comparer_3() const { return ___comparer_3; }
	inline RuntimeObject** get_address_of_comparer_3() { return &___comparer_3; }
	inline void set_comparer_3(RuntimeObject* value)
	{
		___comparer_3 = value;
		Il2CppCodeGenWriteBarrier((&___comparer_3), value);
	}

	inline static int32_t get_offset_of__syncRoot_4() { return static_cast<int32_t>(offsetof(ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D, ____syncRoot_4)); }
	inline RuntimeObject * get__syncRoot_4() const { return ____syncRoot_4; }
	inline RuntimeObject ** get_address_of__syncRoot_4() { return &____syncRoot_4; }
	inline void set__syncRoot_4(RuntimeObject * value)
	{
		____syncRoot_4 = value;
		Il2CppCodeGenWriteBarrier((&____syncRoot_4), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // LISTDICTIONARY_TE68C8A5DB37EB10F3AA958AB3BF214346402074D_H
#ifndef DICTIONARYNODE_T41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E_H
#define DICTIONARYNODE_T41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Specialized.ListDictionary_DictionaryNode
struct  DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E  : public RuntimeObject
{
public:
	// System.Object System.Collections.Specialized.ListDictionary_DictionaryNode::key
	RuntimeObject * ___key_0;
	// System.Object System.Collections.Specialized.ListDictionary_DictionaryNode::value
	RuntimeObject * ___value_1;
	// System.Collections.Specialized.ListDictionary_DictionaryNode System.Collections.Specialized.ListDictionary_DictionaryNode::next
	DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E * ___next_2;

public:
	inline static int32_t get_offset_of_key_0() { return static_cast<int32_t>(offsetof(DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E, ___key_0)); }
	inline RuntimeObject * get_key_0() const { return ___key_0; }
	inline RuntimeObject ** get_address_of_key_0() { return &___key_0; }
	inline void set_key_0(RuntimeObject * value)
	{
		___key_0 = value;
		Il2CppCodeGenWriteBarrier((&___key_0), value);
	}

	inline static int32_t get_offset_of_value_1() { return static_cast<int32_t>(offsetof(DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E, ___value_1)); }
	inline RuntimeObject * get_value_1() const { return ___value_1; }
	inline RuntimeObject ** get_address_of_value_1() { return &___value_1; }
	inline void set_value_1(RuntimeObject * value)
	{
		___value_1 = value;
		Il2CppCodeGenWriteBarrier((&___value_1), value);
	}

	inline static int32_t get_offset_of_next_2() { return static_cast<int32_t>(offsetof(DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E, ___next_2)); }
	inline DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E * get_next_2() const { return ___next_2; }
	inline DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E ** get_address_of_next_2() { return &___next_2; }
	inline void set_next_2(DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E * value)
	{
		___next_2 = value;
		Il2CppCodeGenWriteBarrier((&___next_2), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // DICTIONARYNODE_T41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E_H
#ifndef NODEENUMERATOR_T3E4259603410865D72993AD4CF725707784C9D58_H
#define NODEENUMERATOR_T3E4259603410865D72993AD4CF725707784C9D58_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Specialized.ListDictionary_NodeEnumerator
struct  NodeEnumerator_t3E4259603410865D72993AD4CF725707784C9D58  : public RuntimeObject
{
public:
	// System.Collections.Specialized.ListDictionary System.Collections.Specialized.ListDictionary_NodeEnumerator::list
	ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D * ___list_0;
	// System.Collections.Specialized.ListDictionary_DictionaryNode System.Collections.Specialized.ListDictionary_NodeEnumerator::current
	DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E * ___current_1;
	// System.Int32 System.Collections.Specialized.ListDictionary_NodeEnumerator::version
	int32_t ___version_2;
	// System.Boolean System.Collections.Specialized.ListDictionary_NodeEnumerator::start
	bool ___start_3;

public:
	inline static int32_t get_offset_of_list_0() { return static_cast<int32_t>(offsetof(NodeEnumerator_t3E4259603410865D72993AD4CF725707784C9D58, ___list_0)); }
	inline ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D * get_list_0() const { return ___list_0; }
	inline ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D ** get_address_of_list_0() { return &___list_0; }
	inline void set_list_0(ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D * value)
	{
		___list_0 = value;
		Il2CppCodeGenWriteBarrier((&___list_0), value);
	}

	inline static int32_t get_offset_of_current_1() { return static_cast<int32_t>(offsetof(NodeEnumerator_t3E4259603410865D72993AD4CF725707784C9D58, ___current_1)); }
	inline DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E * get_current_1() const { return ___current_1; }
	inline DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E ** get_address_of_current_1() { return &___current_1; }
	inline void set_current_1(DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E * value)
	{
		___current_1 = value;
		Il2CppCodeGenWriteBarrier((&___current_1), value);
	}

	inline static int32_t get_offset_of_version_2() { return static_cast<int32_t>(offsetof(NodeEnumerator_t3E4259603410865D72993AD4CF725707784C9D58, ___version_2)); }
	inline int32_t get_version_2() const { return ___version_2; }
	inline int32_t* get_address_of_version_2() { return &___version_2; }
	inline void set_version_2(int32_t value)
	{
		___version_2 = value;
	}

	inline static int32_t get_offset_of_start_3() { return static_cast<int32_t>(offsetof(NodeEnumerator_t3E4259603410865D72993AD4CF725707784C9D58, ___start_3)); }
	inline bool get_start_3() const { return ___start_3; }
	inline bool* get_address_of_start_3() { return &___start_3; }
	inline void set_start_3(bool value)
	{
		___start_3 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // NODEENUMERATOR_T3E4259603410865D72993AD4CF725707784C9D58_H
#ifndef NODEKEYVALUECOLLECTION_T9C8FF8CA57B0DECB6C364A1A1D0014BC117C9A47_H
#define NODEKEYVALUECOLLECTION_T9C8FF8CA57B0DECB6C364A1A1D0014BC117C9A47_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Specialized.ListDictionary_NodeKeyValueCollection
struct  NodeKeyValueCollection_t9C8FF8CA57B0DECB6C364A1A1D0014BC117C9A47  : public RuntimeObject
{
public:
	// System.Collections.Specialized.ListDictionary System.Collections.Specialized.ListDictionary_NodeKeyValueCollection::list
	ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D * ___list_0;
	// System.Boolean System.Collections.Specialized.ListDictionary_NodeKeyValueCollection::isKeys
	bool ___isKeys_1;

public:
	inline static int32_t get_offset_of_list_0() { return static_cast<int32_t>(offsetof(NodeKeyValueCollection_t9C8FF8CA57B0DECB6C364A1A1D0014BC117C9A47, ___list_0)); }
	inline ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D * get_list_0() const { return ___list_0; }
	inline ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D ** get_address_of_list_0() { return &___list_0; }
	inline void set_list_0(ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D * value)
	{
		___list_0 = value;
		Il2CppCodeGenWriteBarrier((&___list_0), value);
	}

	inline static int32_t get_offset_of_isKeys_1() { return static_cast<int32_t>(offsetof(NodeKeyValueCollection_t9C8FF8CA57B0DECB6C364A1A1D0014BC117C9A47, ___isKeys_1)); }
	inline bool get_isKeys_1() const { return ___isKeys_1; }
	inline bool* get_address_of_isKeys_1() { return &___isKeys_1; }
	inline void set_isKeys_1(bool value)
	{
		___isKeys_1 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // NODEKEYVALUECOLLECTION_T9C8FF8CA57B0DECB6C364A1A1D0014BC117C9A47_H
#ifndef NODEKEYVALUEENUMERATOR_TCAAF9E36B21D1175ECCFFBA44766AA58F51D409B_H
#define NODEKEYVALUEENUMERATOR_TCAAF9E36B21D1175ECCFFBA44766AA58F51D409B_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Specialized.ListDictionary_NodeKeyValueCollection_NodeKeyValueEnumerator
struct  NodeKeyValueEnumerator_tCAAF9E36B21D1175ECCFFBA44766AA58F51D409B  : public RuntimeObject
{
public:
	// System.Collections.Specialized.ListDictionary System.Collections.Specialized.ListDictionary_NodeKeyValueCollection_NodeKeyValueEnumerator::list
	ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D * ___list_0;
	// System.Collections.Specialized.ListDictionary_DictionaryNode System.Collections.Specialized.ListDictionary_NodeKeyValueCollection_NodeKeyValueEnumerator::current
	DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E * ___current_1;
	// System.Int32 System.Collections.Specialized.ListDictionary_NodeKeyValueCollection_NodeKeyValueEnumerator::version
	int32_t ___version_2;
	// System.Boolean System.Collections.Specialized.ListDictionary_NodeKeyValueCollection_NodeKeyValueEnumerator::isKeys
	bool ___isKeys_3;
	// System.Boolean System.Collections.Specialized.ListDictionary_NodeKeyValueCollection_NodeKeyValueEnumerator::start
	bool ___start_4;

public:
	inline static int32_t get_offset_of_list_0() { return static_cast<int32_t>(offsetof(NodeKeyValueEnumerator_tCAAF9E36B21D1175ECCFFBA44766AA58F51D409B, ___list_0)); }
	inline ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D * get_list_0() const { return ___list_0; }
	inline ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D ** get_address_of_list_0() { return &___list_0; }
	inline void set_list_0(ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D * value)
	{
		___list_0 = value;
		Il2CppCodeGenWriteBarrier((&___list_0), value);
	}

	inline static int32_t get_offset_of_current_1() { return static_cast<int32_t>(offsetof(NodeKeyValueEnumerator_tCAAF9E36B21D1175ECCFFBA44766AA58F51D409B, ___current_1)); }
	inline DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E * get_current_1() const { return ___current_1; }
	inline DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E ** get_address_of_current_1() { return &___current_1; }
	inline void set_current_1(DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E * value)
	{
		___current_1 = value;
		Il2CppCodeGenWriteBarrier((&___current_1), value);
	}

	inline static int32_t get_offset_of_version_2() { return static_cast<int32_t>(offsetof(NodeKeyValueEnumerator_tCAAF9E36B21D1175ECCFFBA44766AA58F51D409B, ___version_2)); }
	inline int32_t get_version_2() const { return ___version_2; }
	inline int32_t* get_address_of_version_2() { return &___version_2; }
	inline void set_version_2(int32_t value)
	{
		___version_2 = value;
	}

	inline static int32_t get_offset_of_isKeys_3() { return static_cast<int32_t>(offsetof(NodeKeyValueEnumerator_tCAAF9E36B21D1175ECCFFBA44766AA58F51D409B, ___isKeys_3)); }
	inline bool get_isKeys_3() const { return ___isKeys_3; }
	inline bool* get_address_of_isKeys_3() { return &___isKeys_3; }
	inline void set_isKeys_3(bool value)
	{
		___isKeys_3 = value;
	}

	inline static int32_t get_offset_of_start_4() { return static_cast<int32_t>(offsetof(NodeKeyValueEnumerator_tCAAF9E36B21D1175ECCFFBA44766AA58F51D409B, ___start_4)); }
	inline bool get_start_4() const { return ___start_4; }
	inline bool* get_address_of_start_4() { return &___start_4; }
	inline void set_start_4(bool value)
	{
		___start_4 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // NODEKEYVALUEENUMERATOR_TCAAF9E36B21D1175ECCFFBA44766AA58F51D409B_H
#ifndef NAMEOBJECTCOLLECTIONBASE_T593D97BF1A2AEA0C7FC1684B447BF92A5383883D_H
#define NAMEOBJECTCOLLECTIONBASE_T593D97BF1A2AEA0C7FC1684B447BF92A5383883D_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Specialized.NameObjectCollectionBase
struct  NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D  : public RuntimeObject
{
public:
	// System.Boolean System.Collections.Specialized.NameObjectCollectionBase::_readOnly
	bool ____readOnly_0;
	// System.Collections.ArrayList System.Collections.Specialized.NameObjectCollectionBase::_entriesArray
	ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4 * ____entriesArray_1;
	// System.Collections.IEqualityComparer System.Collections.Specialized.NameObjectCollectionBase::_keyComparer
	RuntimeObject* ____keyComparer_2;
	// System.Collections.Hashtable modreq(System.Runtime.CompilerServices.IsVolatile) System.Collections.Specialized.NameObjectCollectionBase::_entriesTable
	Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * ____entriesTable_3;
	// System.Collections.Specialized.NameObjectCollectionBase_NameObjectEntry modreq(System.Runtime.CompilerServices.IsVolatile) System.Collections.Specialized.NameObjectCollectionBase::_nullKeyEntry
	NameObjectEntry_tC137E0E1F256300B1F9F0ED88EE02B6611918B54 * ____nullKeyEntry_4;
	// System.Runtime.Serialization.SerializationInfo System.Collections.Specialized.NameObjectCollectionBase::_serializationInfo
	SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26 * ____serializationInfo_5;
	// System.Int32 System.Collections.Specialized.NameObjectCollectionBase::_version
	int32_t ____version_6;
	// System.Object System.Collections.Specialized.NameObjectCollectionBase::_syncRoot
	RuntimeObject * ____syncRoot_7;

public:
	inline static int32_t get_offset_of__readOnly_0() { return static_cast<int32_t>(offsetof(NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D, ____readOnly_0)); }
	inline bool get__readOnly_0() const { return ____readOnly_0; }
	inline bool* get_address_of__readOnly_0() { return &____readOnly_0; }
	inline void set__readOnly_0(bool value)
	{
		____readOnly_0 = value;
	}

	inline static int32_t get_offset_of__entriesArray_1() { return static_cast<int32_t>(offsetof(NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D, ____entriesArray_1)); }
	inline ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4 * get__entriesArray_1() const { return ____entriesArray_1; }
	inline ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4 ** get_address_of__entriesArray_1() { return &____entriesArray_1; }
	inline void set__entriesArray_1(ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4 * value)
	{
		____entriesArray_1 = value;
		Il2CppCodeGenWriteBarrier((&____entriesArray_1), value);
	}

	inline static int32_t get_offset_of__keyComparer_2() { return static_cast<int32_t>(offsetof(NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D, ____keyComparer_2)); }
	inline RuntimeObject* get__keyComparer_2() const { return ____keyComparer_2; }
	inline RuntimeObject** get_address_of__keyComparer_2() { return &____keyComparer_2; }
	inline void set__keyComparer_2(RuntimeObject* value)
	{
		____keyComparer_2 = value;
		Il2CppCodeGenWriteBarrier((&____keyComparer_2), value);
	}

	inline static int32_t get_offset_of__entriesTable_3() { return static_cast<int32_t>(offsetof(NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D, ____entriesTable_3)); }
	inline Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * get__entriesTable_3() const { return ____entriesTable_3; }
	inline Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 ** get_address_of__entriesTable_3() { return &____entriesTable_3; }
	inline void set__entriesTable_3(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * value)
	{
		____entriesTable_3 = value;
		Il2CppCodeGenWriteBarrier((&____entriesTable_3), value);
	}

	inline static int32_t get_offset_of__nullKeyEntry_4() { return static_cast<int32_t>(offsetof(NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D, ____nullKeyEntry_4)); }
	inline NameObjectEntry_tC137E0E1F256300B1F9F0ED88EE02B6611918B54 * get__nullKeyEntry_4() const { return ____nullKeyEntry_4; }
	inline NameObjectEntry_tC137E0E1F256300B1F9F0ED88EE02B6611918B54 ** get_address_of__nullKeyEntry_4() { return &____nullKeyEntry_4; }
	inline void set__nullKeyEntry_4(NameObjectEntry_tC137E0E1F256300B1F9F0ED88EE02B6611918B54 * value)
	{
		____nullKeyEntry_4 = value;
		Il2CppCodeGenWriteBarrier((&____nullKeyEntry_4), value);
	}

	inline static int32_t get_offset_of__serializationInfo_5() { return static_cast<int32_t>(offsetof(NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D, ____serializationInfo_5)); }
	inline SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26 * get__serializationInfo_5() const { return ____serializationInfo_5; }
	inline SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26 ** get_address_of__serializationInfo_5() { return &____serializationInfo_5; }
	inline void set__serializationInfo_5(SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26 * value)
	{
		____serializationInfo_5 = value;
		Il2CppCodeGenWriteBarrier((&____serializationInfo_5), value);
	}

	inline static int32_t get_offset_of__version_6() { return static_cast<int32_t>(offsetof(NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D, ____version_6)); }
	inline int32_t get__version_6() const { return ____version_6; }
	inline int32_t* get_address_of__version_6() { return &____version_6; }
	inline void set__version_6(int32_t value)
	{
		____version_6 = value;
	}

	inline static int32_t get_offset_of__syncRoot_7() { return static_cast<int32_t>(offsetof(NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D, ____syncRoot_7)); }
	inline RuntimeObject * get__syncRoot_7() const { return ____syncRoot_7; }
	inline RuntimeObject ** get_address_of__syncRoot_7() { return &____syncRoot_7; }
	inline void set__syncRoot_7(RuntimeObject * value)
	{
		____syncRoot_7 = value;
		Il2CppCodeGenWriteBarrier((&____syncRoot_7), value);
	}
};

struct NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D_StaticFields
{
public:
	// System.StringComparer System.Collections.Specialized.NameObjectCollectionBase::defaultComparer
	StringComparer_t588BC7FEF85D6E7425E0A8147A3D5A334F1F82DE * ___defaultComparer_8;

public:
	inline static int32_t get_offset_of_defaultComparer_8() { return static_cast<int32_t>(offsetof(NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D_StaticFields, ___defaultComparer_8)); }
	inline StringComparer_t588BC7FEF85D6E7425E0A8147A3D5A334F1F82DE * get_defaultComparer_8() const { return ___defaultComparer_8; }
	inline StringComparer_t588BC7FEF85D6E7425E0A8147A3D5A334F1F82DE ** get_address_of_defaultComparer_8() { return &___defaultComparer_8; }
	inline void set_defaultComparer_8(StringComparer_t588BC7FEF85D6E7425E0A8147A3D5A334F1F82DE * value)
	{
		___defaultComparer_8 = value;
		Il2CppCodeGenWriteBarrier((&___defaultComparer_8), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // NAMEOBJECTCOLLECTIONBASE_T593D97BF1A2AEA0C7FC1684B447BF92A5383883D_H
#ifndef NAMEOBJECTENTRY_TC137E0E1F256300B1F9F0ED88EE02B6611918B54_H
#define NAMEOBJECTENTRY_TC137E0E1F256300B1F9F0ED88EE02B6611918B54_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Specialized.NameObjectCollectionBase_NameObjectEntry
struct  NameObjectEntry_tC137E0E1F256300B1F9F0ED88EE02B6611918B54  : public RuntimeObject
{
public:
	// System.String System.Collections.Specialized.NameObjectCollectionBase_NameObjectEntry::Key
	String_t* ___Key_0;
	// System.Object System.Collections.Specialized.NameObjectCollectionBase_NameObjectEntry::Value
	RuntimeObject * ___Value_1;

public:
	inline static int32_t get_offset_of_Key_0() { return static_cast<int32_t>(offsetof(NameObjectEntry_tC137E0E1F256300B1F9F0ED88EE02B6611918B54, ___Key_0)); }
	inline String_t* get_Key_0() const { return ___Key_0; }
	inline String_t** get_address_of_Key_0() { return &___Key_0; }
	inline void set_Key_0(String_t* value)
	{
		___Key_0 = value;
		Il2CppCodeGenWriteBarrier((&___Key_0), value);
	}

	inline static int32_t get_offset_of_Value_1() { return static_cast<int32_t>(offsetof(NameObjectEntry_tC137E0E1F256300B1F9F0ED88EE02B6611918B54, ___Value_1)); }
	inline RuntimeObject * get_Value_1() const { return ___Value_1; }
	inline RuntimeObject ** get_address_of_Value_1() { return &___Value_1; }
	inline void set_Value_1(RuntimeObject * value)
	{
		___Value_1 = value;
		Il2CppCodeGenWriteBarrier((&___Value_1), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // NAMEOBJECTENTRY_TC137E0E1F256300B1F9F0ED88EE02B6611918B54_H
#ifndef NAMEOBJECTKEYSENUMERATOR_TF732067271E844365B1FF19A6A37852ABCD990D5_H
#define NAMEOBJECTKEYSENUMERATOR_TF732067271E844365B1FF19A6A37852ABCD990D5_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Specialized.NameObjectCollectionBase_NameObjectKeysEnumerator
struct  NameObjectKeysEnumerator_tF732067271E844365B1FF19A6A37852ABCD990D5  : public RuntimeObject
{
public:
	// System.Int32 System.Collections.Specialized.NameObjectCollectionBase_NameObjectKeysEnumerator::_pos
	int32_t ____pos_0;
	// System.Collections.Specialized.NameObjectCollectionBase System.Collections.Specialized.NameObjectCollectionBase_NameObjectKeysEnumerator::_coll
	NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D * ____coll_1;
	// System.Int32 System.Collections.Specialized.NameObjectCollectionBase_NameObjectKeysEnumerator::_version
	int32_t ____version_2;

public:
	inline static int32_t get_offset_of__pos_0() { return static_cast<int32_t>(offsetof(NameObjectKeysEnumerator_tF732067271E844365B1FF19A6A37852ABCD990D5, ____pos_0)); }
	inline int32_t get__pos_0() const { return ____pos_0; }
	inline int32_t* get_address_of__pos_0() { return &____pos_0; }
	inline void set__pos_0(int32_t value)
	{
		____pos_0 = value;
	}

	inline static int32_t get_offset_of__coll_1() { return static_cast<int32_t>(offsetof(NameObjectKeysEnumerator_tF732067271E844365B1FF19A6A37852ABCD990D5, ____coll_1)); }
	inline NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D * get__coll_1() const { return ____coll_1; }
	inline NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D ** get_address_of__coll_1() { return &____coll_1; }
	inline void set__coll_1(NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D * value)
	{
		____coll_1 = value;
		Il2CppCodeGenWriteBarrier((&____coll_1), value);
	}

	inline static int32_t get_offset_of__version_2() { return static_cast<int32_t>(offsetof(NameObjectKeysEnumerator_tF732067271E844365B1FF19A6A37852ABCD990D5, ____version_2)); }
	inline int32_t get__version_2() const { return ____version_2; }
	inline int32_t* get_address_of__version_2() { return &____version_2; }
	inline void set__version_2(int32_t value)
	{
		____version_2 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // NAMEOBJECTKEYSENUMERATOR_TF732067271E844365B1FF19A6A37852ABCD990D5_H
#ifndef ORDEREDDICTIONARY_T2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D_H
#define ORDEREDDICTIONARY_T2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Specialized.OrderedDictionary
struct  OrderedDictionary_t2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D  : public RuntimeObject
{
public:
	// System.Collections.ArrayList System.Collections.Specialized.OrderedDictionary::_objectsArray
	ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4 * ____objectsArray_0;
	// System.Collections.Hashtable System.Collections.Specialized.OrderedDictionary::_objectsTable
	Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * ____objectsTable_1;
	// System.Int32 System.Collections.Specialized.OrderedDictionary::_initialCapacity
	int32_t ____initialCapacity_2;
	// System.Collections.IEqualityComparer System.Collections.Specialized.OrderedDictionary::_comparer
	RuntimeObject* ____comparer_3;
	// System.Boolean System.Collections.Specialized.OrderedDictionary::_readOnly
	bool ____readOnly_4;
	// System.Object System.Collections.Specialized.OrderedDictionary::_syncRoot
	RuntimeObject * ____syncRoot_5;
	// System.Runtime.Serialization.SerializationInfo System.Collections.Specialized.OrderedDictionary::_siInfo
	SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26 * ____siInfo_6;

public:
	inline static int32_t get_offset_of__objectsArray_0() { return static_cast<int32_t>(offsetof(OrderedDictionary_t2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D, ____objectsArray_0)); }
	inline ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4 * get__objectsArray_0() const { return ____objectsArray_0; }
	inline ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4 ** get_address_of__objectsArray_0() { return &____objectsArray_0; }
	inline void set__objectsArray_0(ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4 * value)
	{
		____objectsArray_0 = value;
		Il2CppCodeGenWriteBarrier((&____objectsArray_0), value);
	}

	inline static int32_t get_offset_of__objectsTable_1() { return static_cast<int32_t>(offsetof(OrderedDictionary_t2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D, ____objectsTable_1)); }
	inline Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * get__objectsTable_1() const { return ____objectsTable_1; }
	inline Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 ** get_address_of__objectsTable_1() { return &____objectsTable_1; }
	inline void set__objectsTable_1(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9 * value)
	{
		____objectsTable_1 = value;
		Il2CppCodeGenWriteBarrier((&____objectsTable_1), value);
	}

	inline static int32_t get_offset_of__initialCapacity_2() { return static_cast<int32_t>(offsetof(OrderedDictionary_t2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D, ____initialCapacity_2)); }
	inline int32_t get__initialCapacity_2() const { return ____initialCapacity_2; }
	inline int32_t* get_address_of__initialCapacity_2() { return &____initialCapacity_2; }
	inline void set__initialCapacity_2(int32_t value)
	{
		____initialCapacity_2 = value;
	}

	inline static int32_t get_offset_of__comparer_3() { return static_cast<int32_t>(offsetof(OrderedDictionary_t2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D, ____comparer_3)); }
	inline RuntimeObject* get__comparer_3() const { return ____comparer_3; }
	inline RuntimeObject** get_address_of__comparer_3() { return &____comparer_3; }
	inline void set__comparer_3(RuntimeObject* value)
	{
		____comparer_3 = value;
		Il2CppCodeGenWriteBarrier((&____comparer_3), value);
	}

	inline static int32_t get_offset_of__readOnly_4() { return static_cast<int32_t>(offsetof(OrderedDictionary_t2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D, ____readOnly_4)); }
	inline bool get__readOnly_4() const { return ____readOnly_4; }
	inline bool* get_address_of__readOnly_4() { return &____readOnly_4; }
	inline void set__readOnly_4(bool value)
	{
		____readOnly_4 = value;
	}

	inline static int32_t get_offset_of__syncRoot_5() { return static_cast<int32_t>(offsetof(OrderedDictionary_t2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D, ____syncRoot_5)); }
	inline RuntimeObject * get__syncRoot_5() const { return ____syncRoot_5; }
	inline RuntimeObject ** get_address_of__syncRoot_5() { return &____syncRoot_5; }
	inline void set__syncRoot_5(RuntimeObject * value)
	{
		____syncRoot_5 = value;
		Il2CppCodeGenWriteBarrier((&____syncRoot_5), value);
	}

	inline static int32_t get_offset_of__siInfo_6() { return static_cast<int32_t>(offsetof(OrderedDictionary_t2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D, ____siInfo_6)); }
	inline SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26 * get__siInfo_6() const { return ____siInfo_6; }
	inline SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26 ** get_address_of__siInfo_6() { return &____siInfo_6; }
	inline void set__siInfo_6(SerializationInfo_t1BB80E9C9DEA52DBF464487234B045E2930ADA26 * value)
	{
		____siInfo_6 = value;
		Il2CppCodeGenWriteBarrier((&____siInfo_6), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // ORDEREDDICTIONARY_T2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D_H
#ifndef ORDEREDDICTIONARYENUMERATOR_T09B48F68CFC2EEF1C7C57597E5AE3A4AF903C648_H
#define ORDEREDDICTIONARYENUMERATOR_T09B48F68CFC2EEF1C7C57597E5AE3A4AF903C648_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Specialized.OrderedDictionary_OrderedDictionaryEnumerator
struct  OrderedDictionaryEnumerator_t09B48F68CFC2EEF1C7C57597E5AE3A4AF903C648  : public RuntimeObject
{
public:
	// System.Int32 System.Collections.Specialized.OrderedDictionary_OrderedDictionaryEnumerator::_objectReturnType
	int32_t ____objectReturnType_0;
	// System.Collections.IEnumerator System.Collections.Specialized.OrderedDictionary_OrderedDictionaryEnumerator::arrayEnumerator
	RuntimeObject* ___arrayEnumerator_1;

public:
	inline static int32_t get_offset_of__objectReturnType_0() { return static_cast<int32_t>(offsetof(OrderedDictionaryEnumerator_t09B48F68CFC2EEF1C7C57597E5AE3A4AF903C648, ____objectReturnType_0)); }
	inline int32_t get__objectReturnType_0() const { return ____objectReturnType_0; }
	inline int32_t* get_address_of__objectReturnType_0() { return &____objectReturnType_0; }
	inline void set__objectReturnType_0(int32_t value)
	{
		____objectReturnType_0 = value;
	}

	inline static int32_t get_offset_of_arrayEnumerator_1() { return static_cast<int32_t>(offsetof(OrderedDictionaryEnumerator_t09B48F68CFC2EEF1C7C57597E5AE3A4AF903C648, ___arrayEnumerator_1)); }
	inline RuntimeObject* get_arrayEnumerator_1() const { return ___arrayEnumerator_1; }
	inline RuntimeObject** get_address_of_arrayEnumerator_1() { return &___arrayEnumerator_1; }
	inline void set_arrayEnumerator_1(RuntimeObject* value)
	{
		___arrayEnumerator_1 = value;
		Il2CppCodeGenWriteBarrier((&___arrayEnumerator_1), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // ORDEREDDICTIONARYENUMERATOR_T09B48F68CFC2EEF1C7C57597E5AE3A4AF903C648_H
#ifndef ORDEREDDICTIONARYKEYVALUECOLLECTION_T14B475F99F262455F11F305A747787FACF756B84_H
#define ORDEREDDICTIONARYKEYVALUECOLLECTION_T14B475F99F262455F11F305A747787FACF756B84_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Specialized.OrderedDictionary_OrderedDictionaryKeyValueCollection
struct  OrderedDictionaryKeyValueCollection_t14B475F99F262455F11F305A747787FACF756B84  : public RuntimeObject
{
public:
	// System.Collections.ArrayList System.Collections.Specialized.OrderedDictionary_OrderedDictionaryKeyValueCollection::_objects
	ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4 * ____objects_0;
	// System.Boolean System.Collections.Specialized.OrderedDictionary_OrderedDictionaryKeyValueCollection::isKeys
	bool ___isKeys_1;

public:
	inline static int32_t get_offset_of__objects_0() { return static_cast<int32_t>(offsetof(OrderedDictionaryKeyValueCollection_t14B475F99F262455F11F305A747787FACF756B84, ____objects_0)); }
	inline ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4 * get__objects_0() const { return ____objects_0; }
	inline ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4 ** get_address_of__objects_0() { return &____objects_0; }
	inline void set__objects_0(ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4 * value)
	{
		____objects_0 = value;
		Il2CppCodeGenWriteBarrier((&____objects_0), value);
	}

	inline static int32_t get_offset_of_isKeys_1() { return static_cast<int32_t>(offsetof(OrderedDictionaryKeyValueCollection_t14B475F99F262455F11F305A747787FACF756B84, ___isKeys_1)); }
	inline bool get_isKeys_1() const { return ___isKeys_1; }
	inline bool* get_address_of_isKeys_1() { return &___isKeys_1; }
	inline void set_isKeys_1(bool value)
	{
		___isKeys_1 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // ORDEREDDICTIONARYKEYVALUECOLLECTION_T14B475F99F262455F11F305A747787FACF756B84_H
#ifndef STRINGCOLLECTION_TFF1A487B535F709103604F9DBC2C63FEB1434EFB_H
#define STRINGCOLLECTION_TFF1A487B535F709103604F9DBC2C63FEB1434EFB_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Specialized.StringCollection
struct  StringCollection_tFF1A487B535F709103604F9DBC2C63FEB1434EFB  : public RuntimeObject
{
public:
	// System.Collections.ArrayList System.Collections.Specialized.StringCollection::data
	ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4 * ___data_0;

public:
	inline static int32_t get_offset_of_data_0() { return static_cast<int32_t>(offsetof(StringCollection_tFF1A487B535F709103604F9DBC2C63FEB1434EFB, ___data_0)); }
	inline ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4 * get_data_0() const { return ___data_0; }
	inline ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4 ** get_address_of_data_0() { return &___data_0; }
	inline void set_data_0(ArrayList_t4131E0C29C7E1B9BC9DFE37BEC41A5EB1481ADF4 * value)
	{
		___data_0 = value;
		Il2CppCodeGenWriteBarrier((&___data_0), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // STRINGCOLLECTION_TFF1A487B535F709103604F9DBC2C63FEB1434EFB_H
#ifndef CONFIGURATIONELEMENT_TF3ECE1CDFD3304CD9D595E758276F014321AD9FE_H
#define CONFIGURATIONELEMENT_TF3ECE1CDFD3304CD9D595E758276F014321AD9FE_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.ConfigurationElement
struct  ConfigurationElement_tF3ECE1CDFD3304CD9D595E758276F014321AD9FE  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // CONFIGURATIONELEMENT_TF3ECE1CDFD3304CD9D595E758276F014321AD9FE_H
#ifndef CONFIGURATIONSECTIONGROUP_T64AC7C211E1F868ABF1BD604DA43815564D304E6_H
#define CONFIGURATIONSECTIONGROUP_T64AC7C211E1F868ABF1BD604DA43815564D304E6_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.ConfigurationSectionGroup
struct  ConfigurationSectionGroup_t64AC7C211E1F868ABF1BD604DA43815564D304E6  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // CONFIGURATIONSECTIONGROUP_T64AC7C211E1F868ABF1BD604DA43815564D304E6_H
#ifndef PROVIDERBASE_T641C8553A4C4845B0AA0FCC6F2D667ACFA1B23A5_H
#define PROVIDERBASE_T641C8553A4C4845B0AA0FCC6F2D667ACFA1B23A5_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.Provider.ProviderBase
struct  ProviderBase_t641C8553A4C4845B0AA0FCC6F2D667ACFA1B23A5  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // PROVIDERBASE_T641C8553A4C4845B0AA0FCC6F2D667ACFA1B23A5_H
#ifndef PROVIDERCOLLECTION_TFD3DBCCCADFAE3992F50D32B0688CF4A8E6AE325_H
#define PROVIDERCOLLECTION_TFD3DBCCCADFAE3992F50D32B0688CF4A8E6AE325_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.Provider.ProviderCollection
struct  ProviderCollection_tFD3DBCCCADFAE3992F50D32B0688CF4A8E6AE325  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // PROVIDERCOLLECTION_TFD3DBCCCADFAE3992F50D32B0688CF4A8E6AE325_H
#ifndef SETTINGSBASE_T453379E2BF927282BC6DD98F1121BDB819EF2940_H
#define SETTINGSBASE_T453379E2BF927282BC6DD98F1121BDB819EF2940_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.SettingsBase
struct  SettingsBase_t453379E2BF927282BC6DD98F1121BDB819EF2940  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // SETTINGSBASE_T453379E2BF927282BC6DD98F1121BDB819EF2940_H
#ifndef SETTINGSPROPERTY_T0846992AB26C1256AB1BB63F8C6EF9D903B883FC_H
#define SETTINGSPROPERTY_T0846992AB26C1256AB1BB63F8C6EF9D903B883FC_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.SettingsProperty
struct  SettingsProperty_t0846992AB26C1256AB1BB63F8C6EF9D903B883FC  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // SETTINGSPROPERTY_T0846992AB26C1256AB1BB63F8C6EF9D903B883FC_H
#ifndef SETTINGSPROPERTYCOLLECTION_T542B41A7BB30DADC2C0B6C229515108847B84F98_H
#define SETTINGSPROPERTYCOLLECTION_T542B41A7BB30DADC2C0B6C229515108847B84F98_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.SettingsPropertyCollection
struct  SettingsPropertyCollection_t542B41A7BB30DADC2C0B6C229515108847B84F98  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // SETTINGSPROPERTYCOLLECTION_T542B41A7BB30DADC2C0B6C229515108847B84F98_H
#ifndef SETTINGSPROPERTYVALUE_TEC71581CE766661B3FF55134336818C330A81939_H
#define SETTINGSPROPERTYVALUE_TEC71581CE766661B3FF55134336818C330A81939_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.SettingsPropertyValue
struct  SettingsPropertyValue_tEC71581CE766661B3FF55134336818C330A81939  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // SETTINGSPROPERTYVALUE_TEC71581CE766661B3FF55134336818C330A81939_H
#ifndef SETTINGSPROPERTYVALUECOLLECTION_TF506F16C9E5015DBAAF5AA4C29E03418DDF423ED_H
#define SETTINGSPROPERTYVALUECOLLECTION_TF506F16C9E5015DBAAF5AA4C29E03418DDF423ED_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.SettingsPropertyValueCollection
struct  SettingsPropertyValueCollection_tF506F16C9E5015DBAAF5AA4C29E03418DDF423ED  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // SETTINGSPROPERTYVALUECOLLECTION_TF506F16C9E5015DBAAF5AA4C29E03418DDF423ED_H
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
#ifndef REQUESTCACHE_T41E1BB0AF0CD41C778E51C62180B844887B6EAF8_H
#define REQUESTCACHE_T41E1BB0AF0CD41C778E51C62180B844887B6EAF8_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Cache.RequestCache
struct  RequestCache_t41E1BB0AF0CD41C778E51C62180B844887B6EAF8  : public RuntimeObject
{
public:

public:
};

struct RequestCache_t41E1BB0AF0CD41C778E51C62180B844887B6EAF8_StaticFields
{
public:
	// System.Char[] System.Net.Cache.RequestCache::LineSplits
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___LineSplits_0;

public:
	inline static int32_t get_offset_of_LineSplits_0() { return static_cast<int32_t>(offsetof(RequestCache_t41E1BB0AF0CD41C778E51C62180B844887B6EAF8_StaticFields, ___LineSplits_0)); }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* get_LineSplits_0() const { return ___LineSplits_0; }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2** get_address_of_LineSplits_0() { return &___LineSplits_0; }
	inline void set_LineSplits_0(CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* value)
	{
		___LineSplits_0 = value;
		Il2CppCodeGenWriteBarrier((&___LineSplits_0), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // REQUESTCACHE_T41E1BB0AF0CD41C778E51C62180B844887B6EAF8_H
#ifndef REQUESTCACHEBINDING_TB84D71781C4BCEF43DEBC72165283C4543BA4724_H
#define REQUESTCACHEBINDING_TB84D71781C4BCEF43DEBC72165283C4543BA4724_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Cache.RequestCacheBinding
struct  RequestCacheBinding_tB84D71781C4BCEF43DEBC72165283C4543BA4724  : public RuntimeObject
{
public:
	// System.Net.Cache.RequestCache System.Net.Cache.RequestCacheBinding::m_RequestCache
	RequestCache_t41E1BB0AF0CD41C778E51C62180B844887B6EAF8 * ___m_RequestCache_0;
	// System.Net.Cache.RequestCacheValidator System.Net.Cache.RequestCacheBinding::m_CacheValidator
	RequestCacheValidator_t21A4A8B8ECF2EDCDD0C34B0D26FCAB2F77190F41 * ___m_CacheValidator_1;

public:
	inline static int32_t get_offset_of_m_RequestCache_0() { return static_cast<int32_t>(offsetof(RequestCacheBinding_tB84D71781C4BCEF43DEBC72165283C4543BA4724, ___m_RequestCache_0)); }
	inline RequestCache_t41E1BB0AF0CD41C778E51C62180B844887B6EAF8 * get_m_RequestCache_0() const { return ___m_RequestCache_0; }
	inline RequestCache_t41E1BB0AF0CD41C778E51C62180B844887B6EAF8 ** get_address_of_m_RequestCache_0() { return &___m_RequestCache_0; }
	inline void set_m_RequestCache_0(RequestCache_t41E1BB0AF0CD41C778E51C62180B844887B6EAF8 * value)
	{
		___m_RequestCache_0 = value;
		Il2CppCodeGenWriteBarrier((&___m_RequestCache_0), value);
	}

	inline static int32_t get_offset_of_m_CacheValidator_1() { return static_cast<int32_t>(offsetof(RequestCacheBinding_tB84D71781C4BCEF43DEBC72165283C4543BA4724, ___m_CacheValidator_1)); }
	inline RequestCacheValidator_t21A4A8B8ECF2EDCDD0C34B0D26FCAB2F77190F41 * get_m_CacheValidator_1() const { return ___m_CacheValidator_1; }
	inline RequestCacheValidator_t21A4A8B8ECF2EDCDD0C34B0D26FCAB2F77190F41 ** get_address_of_m_CacheValidator_1() { return &___m_CacheValidator_1; }
	inline void set_m_CacheValidator_1(RequestCacheValidator_t21A4A8B8ECF2EDCDD0C34B0D26FCAB2F77190F41 * value)
	{
		___m_CacheValidator_1 = value;
		Il2CppCodeGenWriteBarrier((&___m_CacheValidator_1), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // REQUESTCACHEBINDING_TB84D71781C4BCEF43DEBC72165283C4543BA4724_H
#ifndef REQUESTCACHEPROTOCOL_T51DE21412EAD66CAD600D3A6940942920340D35D_H
#define REQUESTCACHEPROTOCOL_T51DE21412EAD66CAD600D3A6940942920340D35D_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Cache.RequestCacheProtocol
struct  RequestCacheProtocol_t51DE21412EAD66CAD600D3A6940942920340D35D  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // REQUESTCACHEPROTOCOL_T51DE21412EAD66CAD600D3A6940942920340D35D_H
#ifndef REQUESTCACHEVALIDATOR_T21A4A8B8ECF2EDCDD0C34B0D26FCAB2F77190F41_H
#define REQUESTCACHEVALIDATOR_T21A4A8B8ECF2EDCDD0C34B0D26FCAB2F77190F41_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Cache.RequestCacheValidator
struct  RequestCacheValidator_t21A4A8B8ECF2EDCDD0C34B0D26FCAB2F77190F41  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // REQUESTCACHEVALIDATOR_T21A4A8B8ECF2EDCDD0C34B0D26FCAB2F77190F41_H
#ifndef DEFAULTPROXYSECTIONINTERNAL_TF2CCE31F75FAA00492E00F045768C58A69F53459_H
#define DEFAULTPROXYSECTIONINTERNAL_TF2CCE31F75FAA00492E00F045768C58A69F53459_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.DefaultProxySectionInternal
struct  DefaultProxySectionInternal_tF2CCE31F75FAA00492E00F045768C58A69F53459  : public RuntimeObject
{
public:
	// System.Net.IWebProxy System.Net.Configuration.DefaultProxySectionInternal::webProxy
	RuntimeObject* ___webProxy_0;

public:
	inline static int32_t get_offset_of_webProxy_0() { return static_cast<int32_t>(offsetof(DefaultProxySectionInternal_tF2CCE31F75FAA00492E00F045768C58A69F53459, ___webProxy_0)); }
	inline RuntimeObject* get_webProxy_0() const { return ___webProxy_0; }
	inline RuntimeObject** get_address_of_webProxy_0() { return &___webProxy_0; }
	inline void set_webProxy_0(RuntimeObject* value)
	{
		___webProxy_0 = value;
		Il2CppCodeGenWriteBarrier((&___webProxy_0), value);
	}
};

struct DefaultProxySectionInternal_tF2CCE31F75FAA00492E00F045768C58A69F53459_StaticFields
{
public:
	// System.Object System.Net.Configuration.DefaultProxySectionInternal::classSyncObject
	RuntimeObject * ___classSyncObject_1;

public:
	inline static int32_t get_offset_of_classSyncObject_1() { return static_cast<int32_t>(offsetof(DefaultProxySectionInternal_tF2CCE31F75FAA00492E00F045768C58A69F53459_StaticFields, ___classSyncObject_1)); }
	inline RuntimeObject * get_classSyncObject_1() const { return ___classSyncObject_1; }
	inline RuntimeObject ** get_address_of_classSyncObject_1() { return &___classSyncObject_1; }
	inline void set_classSyncObject_1(RuntimeObject * value)
	{
		___classSyncObject_1 = value;
		Il2CppCodeGenWriteBarrier((&___classSyncObject_1), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // DEFAULTPROXYSECTIONINTERNAL_TF2CCE31F75FAA00492E00F045768C58A69F53459_H
#ifndef IPGLOBALPROPERTIES_T7E7512A45C7685568CA6214D97F31262B754285C_H
#define IPGLOBALPROPERTIES_T7E7512A45C7685568CA6214D97F31262B754285C_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.NetworkInformation.IPGlobalProperties
struct  IPGlobalProperties_t7E7512A45C7685568CA6214D97F31262B754285C  : public RuntimeObject
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // IPGLOBALPROPERTIES_T7E7512A45C7685568CA6214D97F31262B754285C_H
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
#ifndef BITVECTOR32_TDE1C7920F117A283D9595A597572E8910691A0F1_H
#define BITVECTOR32_TDE1C7920F117A283D9595A597572E8910691A0F1_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Specialized.BitVector32
struct  BitVector32_tDE1C7920F117A283D9595A597572E8910691A0F1 
{
public:
	// System.UInt32 System.Collections.Specialized.BitVector32::data
	uint32_t ___data_0;

public:
	inline static int32_t get_offset_of_data_0() { return static_cast<int32_t>(offsetof(BitVector32_tDE1C7920F117A283D9595A597572E8910691A0F1, ___data_0)); }
	inline uint32_t get_data_0() const { return ___data_0; }
	inline uint32_t* get_address_of_data_0() { return &___data_0; }
	inline void set_data_0(uint32_t value)
	{
		___data_0 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // BITVECTOR32_TDE1C7920F117A283D9595A597572E8910691A0F1_H
#ifndef NAMEVALUECOLLECTION_T7C7CED43E4C6E997E3C8012F1D2CC4027FAD10D1_H
#define NAMEVALUECOLLECTION_T7C7CED43E4C6E997E3C8012F1D2CC4027FAD10D1_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Specialized.NameValueCollection
struct  NameValueCollection_t7C7CED43E4C6E997E3C8012F1D2CC4027FAD10D1  : public NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D
{
public:
	// System.String[] System.Collections.Specialized.NameValueCollection::_all
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* ____all_9;
	// System.String[] System.Collections.Specialized.NameValueCollection::_allKeys
	StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* ____allKeys_10;

public:
	inline static int32_t get_offset_of__all_9() { return static_cast<int32_t>(offsetof(NameValueCollection_t7C7CED43E4C6E997E3C8012F1D2CC4027FAD10D1, ____all_9)); }
	inline StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* get__all_9() const { return ____all_9; }
	inline StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E** get_address_of__all_9() { return &____all_9; }
	inline void set__all_9(StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* value)
	{
		____all_9 = value;
		Il2CppCodeGenWriteBarrier((&____all_9), value);
	}

	inline static int32_t get_offset_of__allKeys_10() { return static_cast<int32_t>(offsetof(NameValueCollection_t7C7CED43E4C6E997E3C8012F1D2CC4027FAD10D1, ____allKeys_10)); }
	inline StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* get__allKeys_10() const { return ____allKeys_10; }
	inline StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E** get_address_of__allKeys_10() { return &____allKeys_10; }
	inline void set__allKeys_10(StringU5BU5D_t933FB07893230EA91C40FF900D5400665E87B14E* value)
	{
		____allKeys_10 = value;
		Il2CppCodeGenWriteBarrier((&____allKeys_10), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // NAMEVALUECOLLECTION_T7C7CED43E4C6E997E3C8012F1D2CC4027FAD10D1_H
#ifndef CONFIGURATIONELEMENTCOLLECTION_TB0DA3194B9C1528D2627B291C79B560C68A78FCC_H
#define CONFIGURATIONELEMENTCOLLECTION_TB0DA3194B9C1528D2627B291C79B560C68A78FCC_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.ConfigurationElementCollection
struct  ConfigurationElementCollection_tB0DA3194B9C1528D2627B291C79B560C68A78FCC  : public ConfigurationElement_tF3ECE1CDFD3304CD9D595E758276F014321AD9FE
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // CONFIGURATIONELEMENTCOLLECTION_TB0DA3194B9C1528D2627B291C79B560C68A78FCC_H
#ifndef CONFIGURATIONSECTION_T044F68052218C8000611AE9ADD5F66E62A632B34_H
#define CONFIGURATIONSECTION_T044F68052218C8000611AE9ADD5F66E62A632B34_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.ConfigurationSection
struct  ConfigurationSection_t044F68052218C8000611AE9ADD5F66E62A632B34  : public ConfigurationElement_tF3ECE1CDFD3304CD9D595E758276F014321AD9FE
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // CONFIGURATIONSECTION_T044F68052218C8000611AE9ADD5F66E62A632B34_H
#ifndef SETTINGSPROVIDER_TA65864D41FA268FA6A7CF31021AA6F0CFE1C149A_H
#define SETTINGSPROVIDER_TA65864D41FA268FA6A7CF31021AA6F0CFE1C149A_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.SettingsProvider
struct  SettingsProvider_tA65864D41FA268FA6A7CF31021AA6F0CFE1C149A  : public ProviderBase_t641C8553A4C4845B0AA0FCC6F2D667ACFA1B23A5
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // SETTINGSPROVIDER_TA65864D41FA268FA6A7CF31021AA6F0CFE1C149A_H
#ifndef SETTINGSPROVIDERCOLLECTION_TC3B57BEEADC329BA242C343EF406A2B20BFAC1F5_H
#define SETTINGSPROVIDERCOLLECTION_TC3B57BEEADC329BA242C343EF406A2B20BFAC1F5_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.SettingsProviderCollection
struct  SettingsProviderCollection_tC3B57BEEADC329BA242C343EF406A2B20BFAC1F5  : public ProviderCollection_tFD3DBCCCADFAE3992F50D32B0688CF4A8E6AE325
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // SETTINGSPROVIDERCOLLECTION_TC3B57BEEADC329BA242C343EF406A2B20BFAC1F5_H
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
#ifndef AUTHENTICATIONMODULEELEMENT_T849BD00E7B5095C8FAE3848BD08BA1D929FD1E80_H
#define AUTHENTICATIONMODULEELEMENT_T849BD00E7B5095C8FAE3848BD08BA1D929FD1E80_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.AuthenticationModuleElement
struct  AuthenticationModuleElement_t849BD00E7B5095C8FAE3848BD08BA1D929FD1E80  : public ConfigurationElement_tF3ECE1CDFD3304CD9D595E758276F014321AD9FE
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // AUTHENTICATIONMODULEELEMENT_T849BD00E7B5095C8FAE3848BD08BA1D929FD1E80_H
#ifndef BYPASSELEMENT_T89C59A549C7A25609AA5C200352CD9E310172BAF_H
#define BYPASSELEMENT_T89C59A549C7A25609AA5C200352CD9E310172BAF_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.BypassElement
struct  BypassElement_t89C59A549C7A25609AA5C200352CD9E310172BAF  : public ConfigurationElement_tF3ECE1CDFD3304CD9D595E758276F014321AD9FE
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // BYPASSELEMENT_T89C59A549C7A25609AA5C200352CD9E310172BAF_H
#ifndef CONNECTIONMANAGEMENTELEMENT_TABDA95F63A9CBFC2720D7D3F15C5B352EC5CE7AD_H
#define CONNECTIONMANAGEMENTELEMENT_TABDA95F63A9CBFC2720D7D3F15C5B352EC5CE7AD_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.ConnectionManagementElement
struct  ConnectionManagementElement_tABDA95F63A9CBFC2720D7D3F15C5B352EC5CE7AD  : public ConfigurationElement_tF3ECE1CDFD3304CD9D595E758276F014321AD9FE
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // CONNECTIONMANAGEMENTELEMENT_TABDA95F63A9CBFC2720D7D3F15C5B352EC5CE7AD_H
#ifndef FTPCACHEPOLICYELEMENT_T75AA63C2C74DCE55747733DB9F7F19EFA7C72D92_H
#define FTPCACHEPOLICYELEMENT_T75AA63C2C74DCE55747733DB9F7F19EFA7C72D92_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.FtpCachePolicyElement
struct  FtpCachePolicyElement_t75AA63C2C74DCE55747733DB9F7F19EFA7C72D92  : public ConfigurationElement_tF3ECE1CDFD3304CD9D595E758276F014321AD9FE
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // FTPCACHEPOLICYELEMENT_T75AA63C2C74DCE55747733DB9F7F19EFA7C72D92_H
#ifndef HTTPCACHEPOLICYELEMENT_T913ED10DF4916C215BC26E7D4F58D1B84B41E721_H
#define HTTPCACHEPOLICYELEMENT_T913ED10DF4916C215BC26E7D4F58D1B84B41E721_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.HttpCachePolicyElement
struct  HttpCachePolicyElement_t913ED10DF4916C215BC26E7D4F58D1B84B41E721  : public ConfigurationElement_tF3ECE1CDFD3304CD9D595E758276F014321AD9FE
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // HTTPCACHEPOLICYELEMENT_T913ED10DF4916C215BC26E7D4F58D1B84B41E721_H
#ifndef HTTPLISTENERELEMENT_T160115D829A4163491F955C387C7A7B3924F93AF_H
#define HTTPLISTENERELEMENT_T160115D829A4163491F955C387C7A7B3924F93AF_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.HttpListenerElement
struct  HttpListenerElement_t160115D829A4163491F955C387C7A7B3924F93AF  : public ConfigurationElement_tF3ECE1CDFD3304CD9D595E758276F014321AD9FE
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // HTTPLISTENERELEMENT_T160115D829A4163491F955C387C7A7B3924F93AF_H
#ifndef HTTPLISTENERTIMEOUTSELEMENT_T6C71FADDF95D0625B99DD4BBB266E518B6C8CEDD_H
#define HTTPLISTENERTIMEOUTSELEMENT_T6C71FADDF95D0625B99DD4BBB266E518B6C8CEDD_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.HttpListenerTimeoutsElement
struct  HttpListenerTimeoutsElement_t6C71FADDF95D0625B99DD4BBB266E518B6C8CEDD  : public ConfigurationElement_tF3ECE1CDFD3304CD9D595E758276F014321AD9FE
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // HTTPLISTENERTIMEOUTSELEMENT_T6C71FADDF95D0625B99DD4BBB266E518B6C8CEDD_H
#ifndef HTTPWEBREQUESTELEMENT_T3E2FC0EB83C362CC92300949AF90A0B0BE01EA3D_H
#define HTTPWEBREQUESTELEMENT_T3E2FC0EB83C362CC92300949AF90A0B0BE01EA3D_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.HttpWebRequestElement
struct  HttpWebRequestElement_t3E2FC0EB83C362CC92300949AF90A0B0BE01EA3D  : public ConfigurationElement_tF3ECE1CDFD3304CD9D595E758276F014321AD9FE
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // HTTPWEBREQUESTELEMENT_T3E2FC0EB83C362CC92300949AF90A0B0BE01EA3D_H
#ifndef IPV6ELEMENT_TCA869DC79FE3740DBDECC47877F1676294DB4A23_H
#define IPV6ELEMENT_TCA869DC79FE3740DBDECC47877F1676294DB4A23_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.Ipv6Element
struct  Ipv6Element_tCA869DC79FE3740DBDECC47877F1676294DB4A23  : public ConfigurationElement_tF3ECE1CDFD3304CD9D595E758276F014321AD9FE
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // IPV6ELEMENT_TCA869DC79FE3740DBDECC47877F1676294DB4A23_H
#ifndef MAILSETTINGSSECTIONGROUP_TDF447B7F8FB2FD1788402B2B8350DD8F3C74DBCB_H
#define MAILSETTINGSSECTIONGROUP_TDF447B7F8FB2FD1788402B2B8350DD8F3C74DBCB_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.MailSettingsSectionGroup
struct  MailSettingsSectionGroup_tDF447B7F8FB2FD1788402B2B8350DD8F3C74DBCB  : public ConfigurationSectionGroup_t64AC7C211E1F868ABF1BD604DA43815564D304E6
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // MAILSETTINGSSECTIONGROUP_TDF447B7F8FB2FD1788402B2B8350DD8F3C74DBCB_H
#ifndef MODULEELEMENT_T99D93E6480F780316895A76DCC49E6AB7212507F_H
#define MODULEELEMENT_T99D93E6480F780316895A76DCC49E6AB7212507F_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.ModuleElement
struct  ModuleElement_t99D93E6480F780316895A76DCC49E6AB7212507F  : public ConfigurationElement_tF3ECE1CDFD3304CD9D595E758276F014321AD9FE
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // MODULEELEMENT_T99D93E6480F780316895A76DCC49E6AB7212507F_H
#ifndef PROXYELEMENT_TBD5D75620576BA5BB5521C11D09E0A6E996F9449_H
#define PROXYELEMENT_TBD5D75620576BA5BB5521C11D09E0A6E996F9449_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.ProxyElement
struct  ProxyElement_tBD5D75620576BA5BB5521C11D09E0A6E996F9449  : public ConfigurationElement_tF3ECE1CDFD3304CD9D595E758276F014321AD9FE
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // PROXYELEMENT_TBD5D75620576BA5BB5521C11D09E0A6E996F9449_H
#ifndef COMMONUNIXIPGLOBALPROPERTIES_T4B4AB0ED66A999A38F78E29F99C1094FB0609FD7_H
#define COMMONUNIXIPGLOBALPROPERTIES_T4B4AB0ED66A999A38F78E29F99C1094FB0609FD7_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.NetworkInformation.CommonUnixIPGlobalProperties
struct  CommonUnixIPGlobalProperties_t4B4AB0ED66A999A38F78E29F99C1094FB0609FD7  : public IPGlobalProperties_t7E7512A45C7685568CA6214D97F31262B754285C
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // COMMONUNIXIPGLOBALPROPERTIES_T4B4AB0ED66A999A38F78E29F99C1094FB0609FD7_H
#ifndef WIN32IPGLOBALPROPERTIES_T766A18F55535460667F6B45056DAC0638120030C_H
#define WIN32IPGLOBALPROPERTIES_T766A18F55535460667F6B45056DAC0638120030C_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.NetworkInformation.Win32IPGlobalProperties
struct  Win32IPGlobalProperties_t766A18F55535460667F6B45056DAC0638120030C  : public IPGlobalProperties_t7E7512A45C7685568CA6214D97F31262B754285C
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // WIN32IPGLOBALPROPERTIES_T766A18F55535460667F6B45056DAC0638120030C_H
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
#ifndef HASHTABLE_T978F65B8006C8F5504B286526AEC6608FF983FC9_H
#define HASHTABLE_T978F65B8006C8F5504B286526AEC6608FF983FC9_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Hashtable
struct  Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9  : public RuntimeObject
{
public:
	// System.Collections.Hashtable_bucket[] System.Collections.Hashtable::buckets
	bucketU5BU5D_t6FF2C2C4B21F2206885CD19A78F68B874C8DC84A* ___buckets_0;
	// System.Int32 System.Collections.Hashtable::count
	int32_t ___count_1;
	// System.Int32 System.Collections.Hashtable::occupancy
	int32_t ___occupancy_2;
	// System.Int32 System.Collections.Hashtable::loadsize
	int32_t ___loadsize_3;
	// System.Single System.Collections.Hashtable::loadFactor
	float ___loadFactor_4;
	// System.Int32 modreq(System.Runtime.CompilerServices.IsVolatile) System.Collections.Hashtable::version
	int32_t ___version_5;
	// System.Boolean modreq(System.Runtime.CompilerServices.IsVolatile) System.Collections.Hashtable::isWriterInProgress
	bool ___isWriterInProgress_6;
	// System.Collections.ICollection System.Collections.Hashtable::keys
	RuntimeObject* ___keys_7;
	// System.Collections.ICollection System.Collections.Hashtable::values
	RuntimeObject* ___values_8;
	// System.Collections.IEqualityComparer System.Collections.Hashtable::_keycomparer
	RuntimeObject* ____keycomparer_9;
	// System.Object System.Collections.Hashtable::_syncRoot
	RuntimeObject * ____syncRoot_10;

public:
	inline static int32_t get_offset_of_buckets_0() { return static_cast<int32_t>(offsetof(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9, ___buckets_0)); }
	inline bucketU5BU5D_t6FF2C2C4B21F2206885CD19A78F68B874C8DC84A* get_buckets_0() const { return ___buckets_0; }
	inline bucketU5BU5D_t6FF2C2C4B21F2206885CD19A78F68B874C8DC84A** get_address_of_buckets_0() { return &___buckets_0; }
	inline void set_buckets_0(bucketU5BU5D_t6FF2C2C4B21F2206885CD19A78F68B874C8DC84A* value)
	{
		___buckets_0 = value;
		Il2CppCodeGenWriteBarrier((&___buckets_0), value);
	}

	inline static int32_t get_offset_of_count_1() { return static_cast<int32_t>(offsetof(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9, ___count_1)); }
	inline int32_t get_count_1() const { return ___count_1; }
	inline int32_t* get_address_of_count_1() { return &___count_1; }
	inline void set_count_1(int32_t value)
	{
		___count_1 = value;
	}

	inline static int32_t get_offset_of_occupancy_2() { return static_cast<int32_t>(offsetof(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9, ___occupancy_2)); }
	inline int32_t get_occupancy_2() const { return ___occupancy_2; }
	inline int32_t* get_address_of_occupancy_2() { return &___occupancy_2; }
	inline void set_occupancy_2(int32_t value)
	{
		___occupancy_2 = value;
	}

	inline static int32_t get_offset_of_loadsize_3() { return static_cast<int32_t>(offsetof(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9, ___loadsize_3)); }
	inline int32_t get_loadsize_3() const { return ___loadsize_3; }
	inline int32_t* get_address_of_loadsize_3() { return &___loadsize_3; }
	inline void set_loadsize_3(int32_t value)
	{
		___loadsize_3 = value;
	}

	inline static int32_t get_offset_of_loadFactor_4() { return static_cast<int32_t>(offsetof(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9, ___loadFactor_4)); }
	inline float get_loadFactor_4() const { return ___loadFactor_4; }
	inline float* get_address_of_loadFactor_4() { return &___loadFactor_4; }
	inline void set_loadFactor_4(float value)
	{
		___loadFactor_4 = value;
	}

	inline static int32_t get_offset_of_version_5() { return static_cast<int32_t>(offsetof(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9, ___version_5)); }
	inline int32_t get_version_5() const { return ___version_5; }
	inline int32_t* get_address_of_version_5() { return &___version_5; }
	inline void set_version_5(int32_t value)
	{
		___version_5 = value;
	}

	inline static int32_t get_offset_of_isWriterInProgress_6() { return static_cast<int32_t>(offsetof(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9, ___isWriterInProgress_6)); }
	inline bool get_isWriterInProgress_6() const { return ___isWriterInProgress_6; }
	inline bool* get_address_of_isWriterInProgress_6() { return &___isWriterInProgress_6; }
	inline void set_isWriterInProgress_6(bool value)
	{
		___isWriterInProgress_6 = value;
	}

	inline static int32_t get_offset_of_keys_7() { return static_cast<int32_t>(offsetof(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9, ___keys_7)); }
	inline RuntimeObject* get_keys_7() const { return ___keys_7; }
	inline RuntimeObject** get_address_of_keys_7() { return &___keys_7; }
	inline void set_keys_7(RuntimeObject* value)
	{
		___keys_7 = value;
		Il2CppCodeGenWriteBarrier((&___keys_7), value);
	}

	inline static int32_t get_offset_of_values_8() { return static_cast<int32_t>(offsetof(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9, ___values_8)); }
	inline RuntimeObject* get_values_8() const { return ___values_8; }
	inline RuntimeObject** get_address_of_values_8() { return &___values_8; }
	inline void set_values_8(RuntimeObject* value)
	{
		___values_8 = value;
		Il2CppCodeGenWriteBarrier((&___values_8), value);
	}

	inline static int32_t get_offset_of__keycomparer_9() { return static_cast<int32_t>(offsetof(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9, ____keycomparer_9)); }
	inline RuntimeObject* get__keycomparer_9() const { return ____keycomparer_9; }
	inline RuntimeObject** get_address_of__keycomparer_9() { return &____keycomparer_9; }
	inline void set__keycomparer_9(RuntimeObject* value)
	{
		____keycomparer_9 = value;
		Il2CppCodeGenWriteBarrier((&____keycomparer_9), value);
	}

	inline static int32_t get_offset_of__syncRoot_10() { return static_cast<int32_t>(offsetof(Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9, ____syncRoot_10)); }
	inline RuntimeObject * get__syncRoot_10() const { return ____syncRoot_10; }
	inline RuntimeObject ** get_address_of__syncRoot_10() { return &____syncRoot_10; }
	inline void set__syncRoot_10(RuntimeObject * value)
	{
		____syncRoot_10 = value;
		Il2CppCodeGenWriteBarrier((&____syncRoot_10), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // HASHTABLE_T978F65B8006C8F5504B286526AEC6608FF983FC9_H
#ifndef CONFIGURATIONEXCEPTION_T35BCE2F5F07966C4500F43903BEFDF468E395FE7_H
#define CONFIGURATIONEXCEPTION_T35BCE2F5F07966C4500F43903BEFDF468E395FE7_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.ConfigurationException
struct  ConfigurationException_t35BCE2F5F07966C4500F43903BEFDF468E395FE7  : public SystemException_t5380468142AA850BE4A341D7AF3EAB9C78746782
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // CONFIGURATIONEXCEPTION_T35BCE2F5F07966C4500F43903BEFDF468E395FE7_H
#ifndef SETTINGSSERIALIZEAS_TB7D9625AE7AAC3CBA5D013B21693C56AFA20D634_H
#define SETTINGSSERIALIZEAS_TB7D9625AE7AAC3CBA5D013B21693C56AFA20D634_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.SettingsSerializeAs
struct  SettingsSerializeAs_tB7D9625AE7AAC3CBA5D013B21693C56AFA20D634 
{
public:
	// System.Int32 System.Configuration.SettingsSerializeAs::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(SettingsSerializeAs_tB7D9625AE7AAC3CBA5D013B21693C56AFA20D634, ___value___2)); }
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
#endif // SETTINGSSERIALIZEAS_TB7D9625AE7AAC3CBA5D013B21693C56AFA20D634_H
#ifndef HTTPREQUESTCACHELEVEL_T31DC66727179F1D7225A780117CB094302F6DC23_H
#define HTTPREQUESTCACHELEVEL_T31DC66727179F1D7225A780117CB094302F6DC23_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Cache.HttpRequestCacheLevel
struct  HttpRequestCacheLevel_t31DC66727179F1D7225A780117CB094302F6DC23 
{
public:
	// System.Int32 System.Net.Cache.HttpRequestCacheLevel::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(HttpRequestCacheLevel_t31DC66727179F1D7225A780117CB094302F6DC23, ___value___2)); }
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
#endif // HTTPREQUESTCACHELEVEL_T31DC66727179F1D7225A780117CB094302F6DC23_H
#ifndef REQUESTCACHELEVEL_TB7692FD08BFC2E0F0CDB6499F58D77BEFD576D8B_H
#define REQUESTCACHELEVEL_TB7692FD08BFC2E0F0CDB6499F58D77BEFD576D8B_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Cache.RequestCacheLevel
struct  RequestCacheLevel_tB7692FD08BFC2E0F0CDB6499F58D77BEFD576D8B 
{
public:
	// System.Int32 System.Net.Cache.RequestCacheLevel::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(RequestCacheLevel_tB7692FD08BFC2E0F0CDB6499F58D77BEFD576D8B, ___value___2)); }
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
#endif // REQUESTCACHELEVEL_TB7692FD08BFC2E0F0CDB6499F58D77BEFD576D8B_H
#ifndef AUTHENTICATIONMODULEELEMENTCOLLECTION_TF7B605BB9AEB4E9C477799AE3A2A641E03A1D7DD_H
#define AUTHENTICATIONMODULEELEMENTCOLLECTION_TF7B605BB9AEB4E9C477799AE3A2A641E03A1D7DD_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.AuthenticationModuleElementCollection
struct  AuthenticationModuleElementCollection_tF7B605BB9AEB4E9C477799AE3A2A641E03A1D7DD  : public ConfigurationElementCollection_tB0DA3194B9C1528D2627B291C79B560C68A78FCC
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // AUTHENTICATIONMODULEELEMENTCOLLECTION_TF7B605BB9AEB4E9C477799AE3A2A641E03A1D7DD_H
#ifndef AUTHENTICATIONMODULESSECTION_TC01D5227FFEEA3135D0B519F7EF1425CE65DDC41_H
#define AUTHENTICATIONMODULESSECTION_TC01D5227FFEEA3135D0B519F7EF1425CE65DDC41_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.AuthenticationModulesSection
struct  AuthenticationModulesSection_tC01D5227FFEEA3135D0B519F7EF1425CE65DDC41  : public ConfigurationSection_t044F68052218C8000611AE9ADD5F66E62A632B34
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // AUTHENTICATIONMODULESSECTION_TC01D5227FFEEA3135D0B519F7EF1425CE65DDC41_H
#ifndef BYPASSELEMENTCOLLECTION_T5CCE032F76311FCEFC3128DA5A88D25568A234A7_H
#define BYPASSELEMENTCOLLECTION_T5CCE032F76311FCEFC3128DA5A88D25568A234A7_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.BypassElementCollection
struct  BypassElementCollection_t5CCE032F76311FCEFC3128DA5A88D25568A234A7  : public ConfigurationElementCollection_tB0DA3194B9C1528D2627B291C79B560C68A78FCC
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // BYPASSELEMENTCOLLECTION_T5CCE032F76311FCEFC3128DA5A88D25568A234A7_H
#ifndef CONNECTIONMANAGEMENTELEMENTCOLLECTION_T83F843AEC2D2354836CC863E346FE2ECFEED2572_H
#define CONNECTIONMANAGEMENTELEMENTCOLLECTION_T83F843AEC2D2354836CC863E346FE2ECFEED2572_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.ConnectionManagementElementCollection
struct  ConnectionManagementElementCollection_t83F843AEC2D2354836CC863E346FE2ECFEED2572  : public ConfigurationElementCollection_tB0DA3194B9C1528D2627B291C79B560C68A78FCC
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // CONNECTIONMANAGEMENTELEMENTCOLLECTION_T83F843AEC2D2354836CC863E346FE2ECFEED2572_H
#ifndef CONNECTIONMANAGEMENTSECTION_TA88F9BAD144E401AB524A9579B50050140592447_H
#define CONNECTIONMANAGEMENTSECTION_TA88F9BAD144E401AB524A9579B50050140592447_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.ConnectionManagementSection
struct  ConnectionManagementSection_tA88F9BAD144E401AB524A9579B50050140592447  : public ConfigurationSection_t044F68052218C8000611AE9ADD5F66E62A632B34
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // CONNECTIONMANAGEMENTSECTION_TA88F9BAD144E401AB524A9579B50050140592447_H
#ifndef DEFAULTPROXYSECTION_TB752851846FC0CEBA83C36C2BF6553211029AA3B_H
#define DEFAULTPROXYSECTION_TB752851846FC0CEBA83C36C2BF6553211029AA3B_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.DefaultProxySection
struct  DefaultProxySection_tB752851846FC0CEBA83C36C2BF6553211029AA3B  : public ConfigurationSection_t044F68052218C8000611AE9ADD5F66E62A632B34
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // DEFAULTPROXYSECTION_TB752851846FC0CEBA83C36C2BF6553211029AA3B_H
#ifndef AUTODETECTVALUES_T5FBC7CB20B71835CA991AD2467E1E855EDC2B3AB_H
#define AUTODETECTVALUES_T5FBC7CB20B71835CA991AD2467E1E855EDC2B3AB_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.ProxyElement_AutoDetectValues
struct  AutoDetectValues_t5FBC7CB20B71835CA991AD2467E1E855EDC2B3AB 
{
public:
	// System.Int32 System.Net.Configuration.ProxyElement_AutoDetectValues::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(AutoDetectValues_t5FBC7CB20B71835CA991AD2467E1E855EDC2B3AB, ___value___2)); }
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
#endif // AUTODETECTVALUES_T5FBC7CB20B71835CA991AD2467E1E855EDC2B3AB_H
#ifndef BYPASSONLOCALVALUES_T42DDF7A85427F95302AEFDD4E70F5A4069622025_H
#define BYPASSONLOCALVALUES_T42DDF7A85427F95302AEFDD4E70F5A4069622025_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.ProxyElement_BypassOnLocalValues
struct  BypassOnLocalValues_t42DDF7A85427F95302AEFDD4E70F5A4069622025 
{
public:
	// System.Int32 System.Net.Configuration.ProxyElement_BypassOnLocalValues::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(BypassOnLocalValues_t42DDF7A85427F95302AEFDD4E70F5A4069622025, ___value___2)); }
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
#endif // BYPASSONLOCALVALUES_T42DDF7A85427F95302AEFDD4E70F5A4069622025_H
#ifndef USESYSTEMDEFAULTVALUES_TAE0283B8FBFE533D3F8F9ED4E9BD937878A86A55_H
#define USESYSTEMDEFAULTVALUES_TAE0283B8FBFE533D3F8F9ED4E9BD937878A86A55_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.ProxyElement_UseSystemDefaultValues
struct  UseSystemDefaultValues_tAE0283B8FBFE533D3F8F9ED4E9BD937878A86A55 
{
public:
	// System.Int32 System.Net.Configuration.ProxyElement_UseSystemDefaultValues::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(UseSystemDefaultValues_tAE0283B8FBFE533D3F8F9ED4E9BD937878A86A55, ___value___2)); }
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
#endif // USESYSTEMDEFAULTVALUES_TAE0283B8FBFE533D3F8F9ED4E9BD937878A86A55_H
#ifndef UNICODEDECODINGCONFORMANCE_T9467A928E4A46098930CA0A67315E0159230771E_H
#define UNICODEDECODINGCONFORMANCE_T9467A928E4A46098930CA0A67315E0159230771E_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.UnicodeDecodingConformance
struct  UnicodeDecodingConformance_t9467A928E4A46098930CA0A67315E0159230771E 
{
public:
	// System.Int32 System.Net.Configuration.UnicodeDecodingConformance::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(UnicodeDecodingConformance_t9467A928E4A46098930CA0A67315E0159230771E, ___value___2)); }
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
#endif // UNICODEDECODINGCONFORMANCE_T9467A928E4A46098930CA0A67315E0159230771E_H
#ifndef UNICODEENCODINGCONFORMANCE_TB184A12AA9972C115D899779A92DCB82719B487A_H
#define UNICODEENCODINGCONFORMANCE_TB184A12AA9972C115D899779A92DCB82719B487A_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.UnicodeEncodingConformance
struct  UnicodeEncodingConformance_tB184A12AA9972C115D899779A92DCB82719B487A 
{
public:
	// System.Int32 System.Net.Configuration.UnicodeEncodingConformance::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(UnicodeEncodingConformance_tB184A12AA9972C115D899779A92DCB82719B487A, ___value___2)); }
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
#endif // UNICODEENCODINGCONFORMANCE_TB184A12AA9972C115D899779A92DCB82719B487A_H
#ifndef NETBIOSNODETYPE_TBF92483BC76709F1A2FC1B6DA61A8E9176861C8F_H
#define NETBIOSNODETYPE_TBF92483BC76709F1A2FC1B6DA61A8E9176861C8F_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.NetworkInformation.NetBiosNodeType
struct  NetBiosNodeType_tBF92483BC76709F1A2FC1B6DA61A8E9176861C8F 
{
public:
	// System.Int32 System.Net.NetworkInformation.NetBiosNodeType::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(NetBiosNodeType_tBF92483BC76709F1A2FC1B6DA61A8E9176861C8F, ___value___2)); }
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
#endif // NETBIOSNODETYPE_TBF92483BC76709F1A2FC1B6DA61A8E9176861C8F_H
#ifndef UNIXIPGLOBALPROPERTIES_T71C0A1709AE39166494F9CDAC6EC6F96704E11D6_H
#define UNIXIPGLOBALPROPERTIES_T71C0A1709AE39166494F9CDAC6EC6F96704E11D6_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.NetworkInformation.UnixIPGlobalProperties
struct  UnixIPGlobalProperties_t71C0A1709AE39166494F9CDAC6EC6F96704E11D6  : public CommonUnixIPGlobalProperties_t4B4AB0ED66A999A38F78E29F99C1094FB0609FD7
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // UNIXIPGLOBALPROPERTIES_T71C0A1709AE39166494F9CDAC6EC6F96704E11D6_H
#ifndef WIN32_IP_ADDR_STRING_TDA9F56F72EA92CA09591BA7A512706A1A3BCC16F_H
#define WIN32_IP_ADDR_STRING_TDA9F56F72EA92CA09591BA7A512706A1A3BCC16F_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.NetworkInformation.Win32_IP_ADDR_STRING
struct  Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F 
{
public:
	// System.IntPtr System.Net.NetworkInformation.Win32_IP_ADDR_STRING::Next
	intptr_t ___Next_0;
	// System.String System.Net.NetworkInformation.Win32_IP_ADDR_STRING::IpAddress
	String_t* ___IpAddress_1;
	// System.String System.Net.NetworkInformation.Win32_IP_ADDR_STRING::IpMask
	String_t* ___IpMask_2;
	// System.UInt32 System.Net.NetworkInformation.Win32_IP_ADDR_STRING::Context
	uint32_t ___Context_3;

public:
	inline static int32_t get_offset_of_Next_0() { return static_cast<int32_t>(offsetof(Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F, ___Next_0)); }
	inline intptr_t get_Next_0() const { return ___Next_0; }
	inline intptr_t* get_address_of_Next_0() { return &___Next_0; }
	inline void set_Next_0(intptr_t value)
	{
		___Next_0 = value;
	}

	inline static int32_t get_offset_of_IpAddress_1() { return static_cast<int32_t>(offsetof(Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F, ___IpAddress_1)); }
	inline String_t* get_IpAddress_1() const { return ___IpAddress_1; }
	inline String_t** get_address_of_IpAddress_1() { return &___IpAddress_1; }
	inline void set_IpAddress_1(String_t* value)
	{
		___IpAddress_1 = value;
		Il2CppCodeGenWriteBarrier((&___IpAddress_1), value);
	}

	inline static int32_t get_offset_of_IpMask_2() { return static_cast<int32_t>(offsetof(Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F, ___IpMask_2)); }
	inline String_t* get_IpMask_2() const { return ___IpMask_2; }
	inline String_t** get_address_of_IpMask_2() { return &___IpMask_2; }
	inline void set_IpMask_2(String_t* value)
	{
		___IpMask_2 = value;
		Il2CppCodeGenWriteBarrier((&___IpMask_2), value);
	}

	inline static int32_t get_offset_of_Context_3() { return static_cast<int32_t>(offsetof(Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F, ___Context_3)); }
	inline uint32_t get_Context_3() const { return ___Context_3; }
	inline uint32_t* get_address_of_Context_3() { return &___Context_3; }
	inline void set_Context_3(uint32_t value)
	{
		___Context_3 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of System.Net.NetworkInformation.Win32_IP_ADDR_STRING
struct Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F_marshaled_pinvoke
{
	intptr_t ___Next_0;
	char ___IpAddress_1[16];
	char ___IpMask_2[16];
	uint32_t ___Context_3;
};
// Native definition for COM marshalling of System.Net.NetworkInformation.Win32_IP_ADDR_STRING
struct Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F_marshaled_com
{
	intptr_t ___Next_0;
	char ___IpAddress_1[16];
	char ___IpMask_2[16];
	uint32_t ___Context_3;
};
#endif // WIN32_IP_ADDR_STRING_TDA9F56F72EA92CA09591BA7A512706A1A3BCC16F_H
#ifndef ENCRYPTIONPOLICY_T70A16CF79F2A5DC5349D68BA747658E4E3507C18_H
#define ENCRYPTIONPOLICY_T70A16CF79F2A5DC5349D68BA747658E4E3507C18_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Security.EncryptionPolicy
struct  EncryptionPolicy_t70A16CF79F2A5DC5349D68BA747658E4E3507C18 
{
public:
	// System.Int32 System.Net.Security.EncryptionPolicy::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(EncryptionPolicy_t70A16CF79F2A5DC5349D68BA747658E4E3507C18, ___value___2)); }
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
#endif // ENCRYPTIONPOLICY_T70A16CF79F2A5DC5349D68BA747658E4E3507C18_H
#ifndef IPPROTECTIONLEVEL_T63BF0274CCC5A1BFF42B658316B3092B8C0AA95E_H
#define IPPROTECTIONLEVEL_T63BF0274CCC5A1BFF42B658316B3092B8C0AA95E_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Sockets.IPProtectionLevel
struct  IPProtectionLevel_t63BF0274CCC5A1BFF42B658316B3092B8C0AA95E 
{
public:
	// System.Int32 System.Net.Sockets.IPProtectionLevel::value__
	int32_t ___value___2;

public:
	inline static int32_t get_offset_of_value___2() { return static_cast<int32_t>(offsetof(IPProtectionLevel_t63BF0274CCC5A1BFF42B658316B3092B8C0AA95E, ___value___2)); }
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
#endif // IPPROTECTIONLEVEL_T63BF0274CCC5A1BFF42B658316B3092B8C0AA95E_H
#ifndef SETTINGSATTRIBUTEDICTIONARY_T7DAB45F9F421E95E78F6C215FB535B05E13D57FE_H
#define SETTINGSATTRIBUTEDICTIONARY_T7DAB45F9F421E95E78F6C215FB535B05E13D57FE_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.SettingsAttributeDictionary
struct  SettingsAttributeDictionary_t7DAB45F9F421E95E78F6C215FB535B05E13D57FE  : public Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // SETTINGSATTRIBUTEDICTIONARY_T7DAB45F9F421E95E78F6C215FB535B05E13D57FE_H
#ifndef SETTINGSCONTEXT_TB16593CFDBF1640E960767CCF354052721423CF7_H
#define SETTINGSCONTEXT_TB16593CFDBF1640E960767CCF354052721423CF7_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Configuration.SettingsContext
struct  SettingsContext_tB16593CFDBF1640E960767CCF354052721423CF7  : public Hashtable_t978F65B8006C8F5504B286526AEC6608FF983FC9
{
public:

public:
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // SETTINGSCONTEXT_TB16593CFDBF1640E960767CCF354052721423CF7_H
#ifndef REQUESTCACHEPOLICY_T30D7352C7E9D49EEADD492A70EC92C118D90CD61_H
#define REQUESTCACHEPOLICY_T30D7352C7E9D49EEADD492A70EC92C118D90CD61_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Cache.RequestCachePolicy
struct  RequestCachePolicy_t30D7352C7E9D49EEADD492A70EC92C118D90CD61  : public RuntimeObject
{
public:
	// System.Net.Cache.RequestCacheLevel System.Net.Cache.RequestCachePolicy::m_Level
	int32_t ___m_Level_0;

public:
	inline static int32_t get_offset_of_m_Level_0() { return static_cast<int32_t>(offsetof(RequestCachePolicy_t30D7352C7E9D49EEADD492A70EC92C118D90CD61, ___m_Level_0)); }
	inline int32_t get_m_Level_0() const { return ___m_Level_0; }
	inline int32_t* get_address_of_m_Level_0() { return &___m_Level_0; }
	inline void set_m_Level_0(int32_t value)
	{
		___m_Level_0 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // REQUESTCACHEPOLICY_T30D7352C7E9D49EEADD492A70EC92C118D90CD61_H
#ifndef SETTINGSSECTIONINTERNAL_TF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821_H
#define SETTINGSSECTIONINTERNAL_TF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.Configuration.SettingsSectionInternal
struct  SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821  : public RuntimeObject
{
public:
	// System.Boolean System.Net.Configuration.SettingsSectionInternal::HttpListenerUnescapeRequestUrl
	bool ___HttpListenerUnescapeRequestUrl_1;
	// System.Net.Sockets.IPProtectionLevel System.Net.Configuration.SettingsSectionInternal::IPProtectionLevel
	int32_t ___IPProtectionLevel_2;
	// System.Boolean System.Net.Configuration.SettingsSectionInternal::<UseNagleAlgorithm>k__BackingField
	bool ___U3CUseNagleAlgorithmU3Ek__BackingField_3;
	// System.Boolean System.Net.Configuration.SettingsSectionInternal::<Expect100Continue>k__BackingField
	bool ___U3CExpect100ContinueU3Ek__BackingField_4;
	// System.Boolean System.Net.Configuration.SettingsSectionInternal::<CheckCertificateName>k__BackingField
	bool ___U3CCheckCertificateNameU3Ek__BackingField_5;
	// System.Int32 System.Net.Configuration.SettingsSectionInternal::<DnsRefreshTimeout>k__BackingField
	int32_t ___U3CDnsRefreshTimeoutU3Ek__BackingField_6;
	// System.Boolean System.Net.Configuration.SettingsSectionInternal::<EnableDnsRoundRobin>k__BackingField
	bool ___U3CEnableDnsRoundRobinU3Ek__BackingField_7;
	// System.Boolean System.Net.Configuration.SettingsSectionInternal::<CheckCertificateRevocationList>k__BackingField
	bool ___U3CCheckCertificateRevocationListU3Ek__BackingField_8;
	// System.Net.Security.EncryptionPolicy System.Net.Configuration.SettingsSectionInternal::<EncryptionPolicy>k__BackingField
	int32_t ___U3CEncryptionPolicyU3Ek__BackingField_9;

public:
	inline static int32_t get_offset_of_HttpListenerUnescapeRequestUrl_1() { return static_cast<int32_t>(offsetof(SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821, ___HttpListenerUnescapeRequestUrl_1)); }
	inline bool get_HttpListenerUnescapeRequestUrl_1() const { return ___HttpListenerUnescapeRequestUrl_1; }
	inline bool* get_address_of_HttpListenerUnescapeRequestUrl_1() { return &___HttpListenerUnescapeRequestUrl_1; }
	inline void set_HttpListenerUnescapeRequestUrl_1(bool value)
	{
		___HttpListenerUnescapeRequestUrl_1 = value;
	}

	inline static int32_t get_offset_of_IPProtectionLevel_2() { return static_cast<int32_t>(offsetof(SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821, ___IPProtectionLevel_2)); }
	inline int32_t get_IPProtectionLevel_2() const { return ___IPProtectionLevel_2; }
	inline int32_t* get_address_of_IPProtectionLevel_2() { return &___IPProtectionLevel_2; }
	inline void set_IPProtectionLevel_2(int32_t value)
	{
		___IPProtectionLevel_2 = value;
	}

	inline static int32_t get_offset_of_U3CUseNagleAlgorithmU3Ek__BackingField_3() { return static_cast<int32_t>(offsetof(SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821, ___U3CUseNagleAlgorithmU3Ek__BackingField_3)); }
	inline bool get_U3CUseNagleAlgorithmU3Ek__BackingField_3() const { return ___U3CUseNagleAlgorithmU3Ek__BackingField_3; }
	inline bool* get_address_of_U3CUseNagleAlgorithmU3Ek__BackingField_3() { return &___U3CUseNagleAlgorithmU3Ek__BackingField_3; }
	inline void set_U3CUseNagleAlgorithmU3Ek__BackingField_3(bool value)
	{
		___U3CUseNagleAlgorithmU3Ek__BackingField_3 = value;
	}

	inline static int32_t get_offset_of_U3CExpect100ContinueU3Ek__BackingField_4() { return static_cast<int32_t>(offsetof(SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821, ___U3CExpect100ContinueU3Ek__BackingField_4)); }
	inline bool get_U3CExpect100ContinueU3Ek__BackingField_4() const { return ___U3CExpect100ContinueU3Ek__BackingField_4; }
	inline bool* get_address_of_U3CExpect100ContinueU3Ek__BackingField_4() { return &___U3CExpect100ContinueU3Ek__BackingField_4; }
	inline void set_U3CExpect100ContinueU3Ek__BackingField_4(bool value)
	{
		___U3CExpect100ContinueU3Ek__BackingField_4 = value;
	}

	inline static int32_t get_offset_of_U3CCheckCertificateNameU3Ek__BackingField_5() { return static_cast<int32_t>(offsetof(SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821, ___U3CCheckCertificateNameU3Ek__BackingField_5)); }
	inline bool get_U3CCheckCertificateNameU3Ek__BackingField_5() const { return ___U3CCheckCertificateNameU3Ek__BackingField_5; }
	inline bool* get_address_of_U3CCheckCertificateNameU3Ek__BackingField_5() { return &___U3CCheckCertificateNameU3Ek__BackingField_5; }
	inline void set_U3CCheckCertificateNameU3Ek__BackingField_5(bool value)
	{
		___U3CCheckCertificateNameU3Ek__BackingField_5 = value;
	}

	inline static int32_t get_offset_of_U3CDnsRefreshTimeoutU3Ek__BackingField_6() { return static_cast<int32_t>(offsetof(SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821, ___U3CDnsRefreshTimeoutU3Ek__BackingField_6)); }
	inline int32_t get_U3CDnsRefreshTimeoutU3Ek__BackingField_6() const { return ___U3CDnsRefreshTimeoutU3Ek__BackingField_6; }
	inline int32_t* get_address_of_U3CDnsRefreshTimeoutU3Ek__BackingField_6() { return &___U3CDnsRefreshTimeoutU3Ek__BackingField_6; }
	inline void set_U3CDnsRefreshTimeoutU3Ek__BackingField_6(int32_t value)
	{
		___U3CDnsRefreshTimeoutU3Ek__BackingField_6 = value;
	}

	inline static int32_t get_offset_of_U3CEnableDnsRoundRobinU3Ek__BackingField_7() { return static_cast<int32_t>(offsetof(SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821, ___U3CEnableDnsRoundRobinU3Ek__BackingField_7)); }
	inline bool get_U3CEnableDnsRoundRobinU3Ek__BackingField_7() const { return ___U3CEnableDnsRoundRobinU3Ek__BackingField_7; }
	inline bool* get_address_of_U3CEnableDnsRoundRobinU3Ek__BackingField_7() { return &___U3CEnableDnsRoundRobinU3Ek__BackingField_7; }
	inline void set_U3CEnableDnsRoundRobinU3Ek__BackingField_7(bool value)
	{
		___U3CEnableDnsRoundRobinU3Ek__BackingField_7 = value;
	}

	inline static int32_t get_offset_of_U3CCheckCertificateRevocationListU3Ek__BackingField_8() { return static_cast<int32_t>(offsetof(SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821, ___U3CCheckCertificateRevocationListU3Ek__BackingField_8)); }
	inline bool get_U3CCheckCertificateRevocationListU3Ek__BackingField_8() const { return ___U3CCheckCertificateRevocationListU3Ek__BackingField_8; }
	inline bool* get_address_of_U3CCheckCertificateRevocationListU3Ek__BackingField_8() { return &___U3CCheckCertificateRevocationListU3Ek__BackingField_8; }
	inline void set_U3CCheckCertificateRevocationListU3Ek__BackingField_8(bool value)
	{
		___U3CCheckCertificateRevocationListU3Ek__BackingField_8 = value;
	}

	inline static int32_t get_offset_of_U3CEncryptionPolicyU3Ek__BackingField_9() { return static_cast<int32_t>(offsetof(SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821, ___U3CEncryptionPolicyU3Ek__BackingField_9)); }
	inline int32_t get_U3CEncryptionPolicyU3Ek__BackingField_9() const { return ___U3CEncryptionPolicyU3Ek__BackingField_9; }
	inline int32_t* get_address_of_U3CEncryptionPolicyU3Ek__BackingField_9() { return &___U3CEncryptionPolicyU3Ek__BackingField_9; }
	inline void set_U3CEncryptionPolicyU3Ek__BackingField_9(int32_t value)
	{
		___U3CEncryptionPolicyU3Ek__BackingField_9 = value;
	}
};

struct SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821_StaticFields
{
public:
	// System.Net.Configuration.SettingsSectionInternal System.Net.Configuration.SettingsSectionInternal::instance
	SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821 * ___instance_0;

public:
	inline static int32_t get_offset_of_instance_0() { return static_cast<int32_t>(offsetof(SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821_StaticFields, ___instance_0)); }
	inline SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821 * get_instance_0() const { return ___instance_0; }
	inline SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821 ** get_address_of_instance_0() { return &___instance_0; }
	inline void set_instance_0(SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821 * value)
	{
		___instance_0 = value;
		Il2CppCodeGenWriteBarrier((&___instance_0), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // SETTINGSSECTIONINTERNAL_TF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821_H
#ifndef MIBIPGLOBALPROPERTIES_TCBF9C39EA385DEE5EE1415E38DD919E81F37B676_H
#define MIBIPGLOBALPROPERTIES_TCBF9C39EA385DEE5EE1415E38DD919E81F37B676_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.NetworkInformation.MibIPGlobalProperties
struct  MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676  : public UnixIPGlobalProperties_t71C0A1709AE39166494F9CDAC6EC6F96704E11D6
{
public:
	// System.String System.Net.NetworkInformation.MibIPGlobalProperties::StatisticsFile
	String_t* ___StatisticsFile_0;
	// System.String System.Net.NetworkInformation.MibIPGlobalProperties::StatisticsFileIPv6
	String_t* ___StatisticsFileIPv6_1;
	// System.String System.Net.NetworkInformation.MibIPGlobalProperties::TcpFile
	String_t* ___TcpFile_2;
	// System.String System.Net.NetworkInformation.MibIPGlobalProperties::Tcp6File
	String_t* ___Tcp6File_3;
	// System.String System.Net.NetworkInformation.MibIPGlobalProperties::UdpFile
	String_t* ___UdpFile_4;
	// System.String System.Net.NetworkInformation.MibIPGlobalProperties::Udp6File
	String_t* ___Udp6File_5;

public:
	inline static int32_t get_offset_of_StatisticsFile_0() { return static_cast<int32_t>(offsetof(MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676, ___StatisticsFile_0)); }
	inline String_t* get_StatisticsFile_0() const { return ___StatisticsFile_0; }
	inline String_t** get_address_of_StatisticsFile_0() { return &___StatisticsFile_0; }
	inline void set_StatisticsFile_0(String_t* value)
	{
		___StatisticsFile_0 = value;
		Il2CppCodeGenWriteBarrier((&___StatisticsFile_0), value);
	}

	inline static int32_t get_offset_of_StatisticsFileIPv6_1() { return static_cast<int32_t>(offsetof(MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676, ___StatisticsFileIPv6_1)); }
	inline String_t* get_StatisticsFileIPv6_1() const { return ___StatisticsFileIPv6_1; }
	inline String_t** get_address_of_StatisticsFileIPv6_1() { return &___StatisticsFileIPv6_1; }
	inline void set_StatisticsFileIPv6_1(String_t* value)
	{
		___StatisticsFileIPv6_1 = value;
		Il2CppCodeGenWriteBarrier((&___StatisticsFileIPv6_1), value);
	}

	inline static int32_t get_offset_of_TcpFile_2() { return static_cast<int32_t>(offsetof(MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676, ___TcpFile_2)); }
	inline String_t* get_TcpFile_2() const { return ___TcpFile_2; }
	inline String_t** get_address_of_TcpFile_2() { return &___TcpFile_2; }
	inline void set_TcpFile_2(String_t* value)
	{
		___TcpFile_2 = value;
		Il2CppCodeGenWriteBarrier((&___TcpFile_2), value);
	}

	inline static int32_t get_offset_of_Tcp6File_3() { return static_cast<int32_t>(offsetof(MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676, ___Tcp6File_3)); }
	inline String_t* get_Tcp6File_3() const { return ___Tcp6File_3; }
	inline String_t** get_address_of_Tcp6File_3() { return &___Tcp6File_3; }
	inline void set_Tcp6File_3(String_t* value)
	{
		___Tcp6File_3 = value;
		Il2CppCodeGenWriteBarrier((&___Tcp6File_3), value);
	}

	inline static int32_t get_offset_of_UdpFile_4() { return static_cast<int32_t>(offsetof(MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676, ___UdpFile_4)); }
	inline String_t* get_UdpFile_4() const { return ___UdpFile_4; }
	inline String_t** get_address_of_UdpFile_4() { return &___UdpFile_4; }
	inline void set_UdpFile_4(String_t* value)
	{
		___UdpFile_4 = value;
		Il2CppCodeGenWriteBarrier((&___UdpFile_4), value);
	}

	inline static int32_t get_offset_of_Udp6File_5() { return static_cast<int32_t>(offsetof(MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676, ___Udp6File_5)); }
	inline String_t* get_Udp6File_5() const { return ___Udp6File_5; }
	inline String_t** get_address_of_Udp6File_5() { return &___Udp6File_5; }
	inline void set_Udp6File_5(String_t* value)
	{
		___Udp6File_5 = value;
		Il2CppCodeGenWriteBarrier((&___Udp6File_5), value);
	}
};

struct MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676_StaticFields
{
public:
	// System.Char[] System.Net.NetworkInformation.MibIPGlobalProperties::wsChars
	CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* ___wsChars_6;

public:
	inline static int32_t get_offset_of_wsChars_6() { return static_cast<int32_t>(offsetof(MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676_StaticFields, ___wsChars_6)); }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* get_wsChars_6() const { return ___wsChars_6; }
	inline CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2** get_address_of_wsChars_6() { return &___wsChars_6; }
	inline void set_wsChars_6(CharU5BU5D_t4CC6ABF0AD71BEC97E3C2F1E9C5677E46D3A75C2* value)
	{
		___wsChars_6 = value;
		Il2CppCodeGenWriteBarrier((&___wsChars_6), value);
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // MIBIPGLOBALPROPERTIES_TCBF9C39EA385DEE5EE1415E38DD919E81F37B676_H
#ifndef WIN32_FIXED_INFO_T3A3D06BDBE4DDA090E3A7151E5D761E867A870DD_H
#define WIN32_FIXED_INFO_T3A3D06BDBE4DDA090E3A7151E5D761E867A870DD_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.NetworkInformation.Win32_FIXED_INFO
struct  Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD 
{
public:
	// System.String System.Net.NetworkInformation.Win32_FIXED_INFO::HostName
	String_t* ___HostName_0;
	// System.String System.Net.NetworkInformation.Win32_FIXED_INFO::DomainName
	String_t* ___DomainName_1;
	// System.IntPtr System.Net.NetworkInformation.Win32_FIXED_INFO::CurrentDnsServer
	intptr_t ___CurrentDnsServer_2;
	// System.Net.NetworkInformation.Win32_IP_ADDR_STRING System.Net.NetworkInformation.Win32_FIXED_INFO::DnsServerList
	Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F  ___DnsServerList_3;
	// System.Net.NetworkInformation.NetBiosNodeType System.Net.NetworkInformation.Win32_FIXED_INFO::NodeType
	int32_t ___NodeType_4;
	// System.String System.Net.NetworkInformation.Win32_FIXED_INFO::ScopeId
	String_t* ___ScopeId_5;
	// System.UInt32 System.Net.NetworkInformation.Win32_FIXED_INFO::EnableRouting
	uint32_t ___EnableRouting_6;
	// System.UInt32 System.Net.NetworkInformation.Win32_FIXED_INFO::EnableProxy
	uint32_t ___EnableProxy_7;
	// System.UInt32 System.Net.NetworkInformation.Win32_FIXED_INFO::EnableDns
	uint32_t ___EnableDns_8;

public:
	inline static int32_t get_offset_of_HostName_0() { return static_cast<int32_t>(offsetof(Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD, ___HostName_0)); }
	inline String_t* get_HostName_0() const { return ___HostName_0; }
	inline String_t** get_address_of_HostName_0() { return &___HostName_0; }
	inline void set_HostName_0(String_t* value)
	{
		___HostName_0 = value;
		Il2CppCodeGenWriteBarrier((&___HostName_0), value);
	}

	inline static int32_t get_offset_of_DomainName_1() { return static_cast<int32_t>(offsetof(Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD, ___DomainName_1)); }
	inline String_t* get_DomainName_1() const { return ___DomainName_1; }
	inline String_t** get_address_of_DomainName_1() { return &___DomainName_1; }
	inline void set_DomainName_1(String_t* value)
	{
		___DomainName_1 = value;
		Il2CppCodeGenWriteBarrier((&___DomainName_1), value);
	}

	inline static int32_t get_offset_of_CurrentDnsServer_2() { return static_cast<int32_t>(offsetof(Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD, ___CurrentDnsServer_2)); }
	inline intptr_t get_CurrentDnsServer_2() const { return ___CurrentDnsServer_2; }
	inline intptr_t* get_address_of_CurrentDnsServer_2() { return &___CurrentDnsServer_2; }
	inline void set_CurrentDnsServer_2(intptr_t value)
	{
		___CurrentDnsServer_2 = value;
	}

	inline static int32_t get_offset_of_DnsServerList_3() { return static_cast<int32_t>(offsetof(Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD, ___DnsServerList_3)); }
	inline Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F  get_DnsServerList_3() const { return ___DnsServerList_3; }
	inline Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F * get_address_of_DnsServerList_3() { return &___DnsServerList_3; }
	inline void set_DnsServerList_3(Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F  value)
	{
		___DnsServerList_3 = value;
	}

	inline static int32_t get_offset_of_NodeType_4() { return static_cast<int32_t>(offsetof(Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD, ___NodeType_4)); }
	inline int32_t get_NodeType_4() const { return ___NodeType_4; }
	inline int32_t* get_address_of_NodeType_4() { return &___NodeType_4; }
	inline void set_NodeType_4(int32_t value)
	{
		___NodeType_4 = value;
	}

	inline static int32_t get_offset_of_ScopeId_5() { return static_cast<int32_t>(offsetof(Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD, ___ScopeId_5)); }
	inline String_t* get_ScopeId_5() const { return ___ScopeId_5; }
	inline String_t** get_address_of_ScopeId_5() { return &___ScopeId_5; }
	inline void set_ScopeId_5(String_t* value)
	{
		___ScopeId_5 = value;
		Il2CppCodeGenWriteBarrier((&___ScopeId_5), value);
	}

	inline static int32_t get_offset_of_EnableRouting_6() { return static_cast<int32_t>(offsetof(Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD, ___EnableRouting_6)); }
	inline uint32_t get_EnableRouting_6() const { return ___EnableRouting_6; }
	inline uint32_t* get_address_of_EnableRouting_6() { return &___EnableRouting_6; }
	inline void set_EnableRouting_6(uint32_t value)
	{
		___EnableRouting_6 = value;
	}

	inline static int32_t get_offset_of_EnableProxy_7() { return static_cast<int32_t>(offsetof(Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD, ___EnableProxy_7)); }
	inline uint32_t get_EnableProxy_7() const { return ___EnableProxy_7; }
	inline uint32_t* get_address_of_EnableProxy_7() { return &___EnableProxy_7; }
	inline void set_EnableProxy_7(uint32_t value)
	{
		___EnableProxy_7 = value;
	}

	inline static int32_t get_offset_of_EnableDns_8() { return static_cast<int32_t>(offsetof(Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD, ___EnableDns_8)); }
	inline uint32_t get_EnableDns_8() const { return ___EnableDns_8; }
	inline uint32_t* get_address_of_EnableDns_8() { return &___EnableDns_8; }
	inline void set_EnableDns_8(uint32_t value)
	{
		___EnableDns_8 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Native definition for P/Invoke marshalling of System.Net.NetworkInformation.Win32_FIXED_INFO
struct Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD_marshaled_pinvoke
{
	char ___HostName_0[132];
	char ___DomainName_1[132];
	intptr_t ___CurrentDnsServer_2;
	Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F_marshaled_pinvoke ___DnsServerList_3;
	int32_t ___NodeType_4;
	char ___ScopeId_5[260];
	uint32_t ___EnableRouting_6;
	uint32_t ___EnableProxy_7;
	uint32_t ___EnableDns_8;
};
// Native definition for COM marshalling of System.Net.NetworkInformation.Win32_FIXED_INFO
struct Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD_marshaled_com
{
	char ___HostName_0[132];
	char ___DomainName_1[132];
	intptr_t ___CurrentDnsServer_2;
	Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F_marshaled_com ___DnsServerList_3;
	int32_t ___NodeType_4;
	char ___ScopeId_5[260];
	uint32_t ___EnableRouting_6;
	uint32_t ___EnableProxy_7;
	uint32_t ___EnableDns_8;
};
#endif // WIN32_FIXED_INFO_T3A3D06BDBE4DDA090E3A7151E5D761E867A870DD_H
#ifndef WIN32NETWORKINTERFACE_T487FF10999655B26E03634B0C8D36B7F0B88D837_H
#define WIN32NETWORKINTERFACE_T487FF10999655B26E03634B0C8D36B7F0B88D837_H
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Net.NetworkInformation.Win32NetworkInterface
struct  Win32NetworkInterface_t487FF10999655B26E03634B0C8D36B7F0B88D837  : public RuntimeObject
{
public:

public:
};

struct Win32NetworkInterface_t487FF10999655B26E03634B0C8D36B7F0B88D837_StaticFields
{
public:
	// System.Net.NetworkInformation.Win32_FIXED_INFO System.Net.NetworkInformation.Win32NetworkInterface::fixedInfo
	Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD  ___fixedInfo_0;
	// System.Boolean System.Net.NetworkInformation.Win32NetworkInterface::initialized
	bool ___initialized_1;

public:
	inline static int32_t get_offset_of_fixedInfo_0() { return static_cast<int32_t>(offsetof(Win32NetworkInterface_t487FF10999655B26E03634B0C8D36B7F0B88D837_StaticFields, ___fixedInfo_0)); }
	inline Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD  get_fixedInfo_0() const { return ___fixedInfo_0; }
	inline Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD * get_address_of_fixedInfo_0() { return &___fixedInfo_0; }
	inline void set_fixedInfo_0(Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD  value)
	{
		___fixedInfo_0 = value;
	}

	inline static int32_t get_offset_of_initialized_1() { return static_cast<int32_t>(offsetof(Win32NetworkInterface_t487FF10999655B26E03634B0C8D36B7F0B88D837_StaticFields, ___initialized_1)); }
	inline bool get_initialized_1() const { return ___initialized_1; }
	inline bool* get_address_of_initialized_1() { return &___initialized_1; }
	inline void set_initialized_1(bool value)
	{
		___initialized_1 = value;
	}
};

#ifdef __clang__
#pragma clang diagnostic pop
#endif
#endif // WIN32NETWORKINTERFACE_T487FF10999655B26E03634B0C8D36B7F0B88D837_H





#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2700 = { sizeof (CommonUnixIPGlobalProperties_t4B4AB0ED66A999A38F78E29F99C1094FB0609FD7), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2701 = { sizeof (UnixIPGlobalProperties_t71C0A1709AE39166494F9CDAC6EC6F96704E11D6), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2702 = { sizeof (MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676), -1, sizeof(MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676_StaticFields), 0 };
extern const int32_t g_FieldOffsetTable2702[7] = 
{
	MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676::get_offset_of_StatisticsFile_0(),
	MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676::get_offset_of_StatisticsFileIPv6_1(),
	MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676::get_offset_of_TcpFile_2(),
	MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676::get_offset_of_Tcp6File_3(),
	MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676::get_offset_of_UdpFile_4(),
	MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676::get_offset_of_Udp6File_5(),
	MibIPGlobalProperties_tCBF9C39EA385DEE5EE1415E38DD919E81F37B676_StaticFields::get_offset_of_wsChars_6(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2703 = { sizeof (Win32IPGlobalProperties_t766A18F55535460667F6B45056DAC0638120030C), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2704 = { sizeof (Win32NetworkInterface_t487FF10999655B26E03634B0C8D36B7F0B88D837), -1, sizeof(Win32NetworkInterface_t487FF10999655B26E03634B0C8D36B7F0B88D837_StaticFields), 0 };
extern const int32_t g_FieldOffsetTable2704[2] = 
{
	Win32NetworkInterface_t487FF10999655B26E03634B0C8D36B7F0B88D837_StaticFields::get_offset_of_fixedInfo_0(),
	Win32NetworkInterface_t487FF10999655B26E03634B0C8D36B7F0B88D837_StaticFields::get_offset_of_initialized_1(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2705 = { sizeof (Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD)+ sizeof (RuntimeObject), sizeof(Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD_marshaled_pinvoke), 0, 0 };
extern const int32_t g_FieldOffsetTable2705[9] = 
{
	Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD::get_offset_of_HostName_0() + static_cast<int32_t>(sizeof(RuntimeObject)),
	Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD::get_offset_of_DomainName_1() + static_cast<int32_t>(sizeof(RuntimeObject)),
	Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD::get_offset_of_CurrentDnsServer_2() + static_cast<int32_t>(sizeof(RuntimeObject)),
	Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD::get_offset_of_DnsServerList_3() + static_cast<int32_t>(sizeof(RuntimeObject)),
	Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD::get_offset_of_NodeType_4() + static_cast<int32_t>(sizeof(RuntimeObject)),
	Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD::get_offset_of_ScopeId_5() + static_cast<int32_t>(sizeof(RuntimeObject)),
	Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD::get_offset_of_EnableRouting_6() + static_cast<int32_t>(sizeof(RuntimeObject)),
	Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD::get_offset_of_EnableProxy_7() + static_cast<int32_t>(sizeof(RuntimeObject)),
	Win32_FIXED_INFO_t3A3D06BDBE4DDA090E3A7151E5D761E867A870DD::get_offset_of_EnableDns_8() + static_cast<int32_t>(sizeof(RuntimeObject)),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2706 = { sizeof (Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F)+ sizeof (RuntimeObject), sizeof(Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F_marshaled_pinvoke), 0, 0 };
extern const int32_t g_FieldOffsetTable2706[4] = 
{
	Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F::get_offset_of_Next_0() + static_cast<int32_t>(sizeof(RuntimeObject)),
	Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F::get_offset_of_IpAddress_1() + static_cast<int32_t>(sizeof(RuntimeObject)),
	Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F::get_offset_of_IpMask_2() + static_cast<int32_t>(sizeof(RuntimeObject)),
	Win32_IP_ADDR_STRING_tDA9F56F72EA92CA09591BA7A512706A1A3BCC16F::get_offset_of_Context_3() + static_cast<int32_t>(sizeof(RuntimeObject)),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2707 = { sizeof (DefaultProxySectionInternal_tF2CCE31F75FAA00492E00F045768C58A69F53459), -1, sizeof(DefaultProxySectionInternal_tF2CCE31F75FAA00492E00F045768C58A69F53459_StaticFields), 0 };
extern const int32_t g_FieldOffsetTable2707[2] = 
{
	DefaultProxySectionInternal_tF2CCE31F75FAA00492E00F045768C58A69F53459::get_offset_of_webProxy_0(),
	DefaultProxySectionInternal_tF2CCE31F75FAA00492E00F045768C58A69F53459_StaticFields::get_offset_of_classSyncObject_1(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2708 = { sizeof (UnicodeDecodingConformance_t9467A928E4A46098930CA0A67315E0159230771E)+ sizeof (RuntimeObject), sizeof(int32_t), 0, 0 };
extern const int32_t g_FieldOffsetTable2708[5] = 
{
	UnicodeDecodingConformance_t9467A928E4A46098930CA0A67315E0159230771E::get_offset_of_value___2() + static_cast<int32_t>(sizeof(RuntimeObject)),
	0,
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2709 = { sizeof (UnicodeEncodingConformance_tB184A12AA9972C115D899779A92DCB82719B487A)+ sizeof (RuntimeObject), sizeof(int32_t), 0, 0 };
extern const int32_t g_FieldOffsetTable2709[4] = 
{
	UnicodeEncodingConformance_tB184A12AA9972C115D899779A92DCB82719B487A::get_offset_of_value___2() + static_cast<int32_t>(sizeof(RuntimeObject)),
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2710 = { sizeof (SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821), -1, sizeof(SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821_StaticFields), 0 };
extern const int32_t g_FieldOffsetTable2710[10] = 
{
	SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821_StaticFields::get_offset_of_instance_0(),
	SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821::get_offset_of_HttpListenerUnescapeRequestUrl_1(),
	SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821::get_offset_of_IPProtectionLevel_2(),
	SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821::get_offset_of_U3CUseNagleAlgorithmU3Ek__BackingField_3(),
	SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821::get_offset_of_U3CExpect100ContinueU3Ek__BackingField_4(),
	SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821::get_offset_of_U3CCheckCertificateNameU3Ek__BackingField_5(),
	SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821::get_offset_of_U3CDnsRefreshTimeoutU3Ek__BackingField_6(),
	SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821::get_offset_of_U3CEnableDnsRoundRobinU3Ek__BackingField_7(),
	SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821::get_offset_of_U3CCheckCertificateRevocationListU3Ek__BackingField_8(),
	SettingsSectionInternal_tF2FB73EEA570541CE4BEE9C77A920B8C4EE6C821::get_offset_of_U3CEncryptionPolicyU3Ek__BackingField_9(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2711 = { sizeof (RequestCache_t41E1BB0AF0CD41C778E51C62180B844887B6EAF8), -1, sizeof(RequestCache_t41E1BB0AF0CD41C778E51C62180B844887B6EAF8_StaticFields), 0 };
extern const int32_t g_FieldOffsetTable2711[1] = 
{
	RequestCache_t41E1BB0AF0CD41C778E51C62180B844887B6EAF8_StaticFields::get_offset_of_LineSplits_0(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2712 = { sizeof (RequestCacheValidator_t21A4A8B8ECF2EDCDD0C34B0D26FCAB2F77190F41), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2713 = { sizeof (RequestCacheBinding_tB84D71781C4BCEF43DEBC72165283C4543BA4724), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable2713[2] = 
{
	RequestCacheBinding_tB84D71781C4BCEF43DEBC72165283C4543BA4724::get_offset_of_m_RequestCache_0(),
	RequestCacheBinding_tB84D71781C4BCEF43DEBC72165283C4543BA4724::get_offset_of_m_CacheValidator_1(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2714 = { sizeof (RequestCacheLevel_tB7692FD08BFC2E0F0CDB6499F58D77BEFD576D8B)+ sizeof (RuntimeObject), sizeof(int32_t), 0, 0 };
extern const int32_t g_FieldOffsetTable2714[8] = 
{
	RequestCacheLevel_tB7692FD08BFC2E0F0CDB6499F58D77BEFD576D8B::get_offset_of_value___2() + static_cast<int32_t>(sizeof(RuntimeObject)),
	0,
	0,
	0,
	0,
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2715 = { sizeof (RequestCachePolicy_t30D7352C7E9D49EEADD492A70EC92C118D90CD61), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable2715[1] = 
{
	RequestCachePolicy_t30D7352C7E9D49EEADD492A70EC92C118D90CD61::get_offset_of_m_Level_0(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2716 = { sizeof (HttpRequestCacheLevel_t31DC66727179F1D7225A780117CB094302F6DC23)+ sizeof (RuntimeObject), sizeof(int32_t), 0, 0 };
extern const int32_t g_FieldOffsetTable2716[10] = 
{
	HttpRequestCacheLevel_t31DC66727179F1D7225A780117CB094302F6DC23::get_offset_of_value___2() + static_cast<int32_t>(sizeof(RuntimeObject)),
	0,
	0,
	0,
	0,
	0,
	0,
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2717 = { sizeof (RequestCacheProtocol_t51DE21412EAD66CAD600D3A6940942920340D35D), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2718 = { sizeof (BitVector32_tDE1C7920F117A283D9595A597572E8910691A0F1)+ sizeof (RuntimeObject), sizeof(BitVector32_tDE1C7920F117A283D9595A597572E8910691A0F1 ), 0, 0 };
extern const int32_t g_FieldOffsetTable2718[1] = 
{
	BitVector32_tDE1C7920F117A283D9595A597572E8910691A0F1::get_offset_of_data_0() + static_cast<int32_t>(sizeof(RuntimeObject)),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2719 = { sizeof (HybridDictionary_t885F953154C575D3408650DCE5D0B76F1EE4E549), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable2719[3] = 
{
	HybridDictionary_t885F953154C575D3408650DCE5D0B76F1EE4E549::get_offset_of_list_0(),
	HybridDictionary_t885F953154C575D3408650DCE5D0B76F1EE4E549::get_offset_of_hashtable_1(),
	HybridDictionary_t885F953154C575D3408650DCE5D0B76F1EE4E549::get_offset_of_caseInsensitive_2(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2720 = { 0, -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2721 = { sizeof (ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable2721[5] = 
{
	ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D::get_offset_of_head_0(),
	ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D::get_offset_of_version_1(),
	ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D::get_offset_of_count_2(),
	ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D::get_offset_of_comparer_3(),
	ListDictionary_tE68C8A5DB37EB10F3AA958AB3BF214346402074D::get_offset_of__syncRoot_4(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2722 = { sizeof (NodeEnumerator_t3E4259603410865D72993AD4CF725707784C9D58), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable2722[4] = 
{
	NodeEnumerator_t3E4259603410865D72993AD4CF725707784C9D58::get_offset_of_list_0(),
	NodeEnumerator_t3E4259603410865D72993AD4CF725707784C9D58::get_offset_of_current_1(),
	NodeEnumerator_t3E4259603410865D72993AD4CF725707784C9D58::get_offset_of_version_2(),
	NodeEnumerator_t3E4259603410865D72993AD4CF725707784C9D58::get_offset_of_start_3(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2723 = { sizeof (NodeKeyValueCollection_t9C8FF8CA57B0DECB6C364A1A1D0014BC117C9A47), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable2723[2] = 
{
	NodeKeyValueCollection_t9C8FF8CA57B0DECB6C364A1A1D0014BC117C9A47::get_offset_of_list_0(),
	NodeKeyValueCollection_t9C8FF8CA57B0DECB6C364A1A1D0014BC117C9A47::get_offset_of_isKeys_1(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2724 = { sizeof (NodeKeyValueEnumerator_tCAAF9E36B21D1175ECCFFBA44766AA58F51D409B), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable2724[5] = 
{
	NodeKeyValueEnumerator_tCAAF9E36B21D1175ECCFFBA44766AA58F51D409B::get_offset_of_list_0(),
	NodeKeyValueEnumerator_tCAAF9E36B21D1175ECCFFBA44766AA58F51D409B::get_offset_of_current_1(),
	NodeKeyValueEnumerator_tCAAF9E36B21D1175ECCFFBA44766AA58F51D409B::get_offset_of_version_2(),
	NodeKeyValueEnumerator_tCAAF9E36B21D1175ECCFFBA44766AA58F51D409B::get_offset_of_isKeys_3(),
	NodeKeyValueEnumerator_tCAAF9E36B21D1175ECCFFBA44766AA58F51D409B::get_offset_of_start_4(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2725 = { sizeof (DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable2725[3] = 
{
	DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E::get_offset_of_key_0(),
	DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E::get_offset_of_value_1(),
	DictionaryNode_t41301C6A2DB2EC1BC25B6C192F9AC0E753AE1A9E::get_offset_of_next_2(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2726 = { sizeof (NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D), -1, sizeof(NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D_StaticFields), 0 };
extern const int32_t g_FieldOffsetTable2726[9] = 
{
	NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D::get_offset_of__readOnly_0(),
	NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D::get_offset_of__entriesArray_1(),
	NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D::get_offset_of__keyComparer_2(),
	NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D::get_offset_of__entriesTable_3(),
	NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D::get_offset_of__nullKeyEntry_4(),
	NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D::get_offset_of__serializationInfo_5(),
	NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D::get_offset_of__version_6(),
	NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D::get_offset_of__syncRoot_7(),
	NameObjectCollectionBase_t593D97BF1A2AEA0C7FC1684B447BF92A5383883D_StaticFields::get_offset_of_defaultComparer_8(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2727 = { sizeof (NameObjectEntry_tC137E0E1F256300B1F9F0ED88EE02B6611918B54), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable2727[2] = 
{
	NameObjectEntry_tC137E0E1F256300B1F9F0ED88EE02B6611918B54::get_offset_of_Key_0(),
	NameObjectEntry_tC137E0E1F256300B1F9F0ED88EE02B6611918B54::get_offset_of_Value_1(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2728 = { sizeof (NameObjectKeysEnumerator_tF732067271E844365B1FF19A6A37852ABCD990D5), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable2728[3] = 
{
	NameObjectKeysEnumerator_tF732067271E844365B1FF19A6A37852ABCD990D5::get_offset_of__pos_0(),
	NameObjectKeysEnumerator_tF732067271E844365B1FF19A6A37852ABCD990D5::get_offset_of__coll_1(),
	NameObjectKeysEnumerator_tF732067271E844365B1FF19A6A37852ABCD990D5::get_offset_of__version_2(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2729 = { sizeof (CompatibleComparer_t3AF98635FCA9D8C4830435F5FEEC183B10385EBF), -1, sizeof(CompatibleComparer_t3AF98635FCA9D8C4830435F5FEEC183B10385EBF_StaticFields), 0 };
extern const int32_t g_FieldOffsetTable2729[4] = 
{
	CompatibleComparer_t3AF98635FCA9D8C4830435F5FEEC183B10385EBF::get_offset_of__comparer_0(),
	CompatibleComparer_t3AF98635FCA9D8C4830435F5FEEC183B10385EBF_StaticFields::get_offset_of_defaultComparer_1(),
	CompatibleComparer_t3AF98635FCA9D8C4830435F5FEEC183B10385EBF::get_offset_of__hcp_2(),
	CompatibleComparer_t3AF98635FCA9D8C4830435F5FEEC183B10385EBF_StaticFields::get_offset_of_defaultHashProvider_3(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2730 = { sizeof (NameValueCollection_t7C7CED43E4C6E997E3C8012F1D2CC4027FAD10D1), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable2730[2] = 
{
	NameValueCollection_t7C7CED43E4C6E997E3C8012F1D2CC4027FAD10D1::get_offset_of__all_9(),
	NameValueCollection_t7C7CED43E4C6E997E3C8012F1D2CC4027FAD10D1::get_offset_of__allKeys_10(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2731 = { sizeof (OrderedDictionary_t2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable2731[7] = 
{
	OrderedDictionary_t2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D::get_offset_of__objectsArray_0(),
	OrderedDictionary_t2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D::get_offset_of__objectsTable_1(),
	OrderedDictionary_t2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D::get_offset_of__initialCapacity_2(),
	OrderedDictionary_t2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D::get_offset_of__comparer_3(),
	OrderedDictionary_t2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D::get_offset_of__readOnly_4(),
	OrderedDictionary_t2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D::get_offset_of__syncRoot_5(),
	OrderedDictionary_t2F2E6D6ADEA347CFB4BC084B5300A83300FEF18D::get_offset_of__siInfo_6(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2732 = { sizeof (OrderedDictionaryEnumerator_t09B48F68CFC2EEF1C7C57597E5AE3A4AF903C648), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable2732[2] = 
{
	OrderedDictionaryEnumerator_t09B48F68CFC2EEF1C7C57597E5AE3A4AF903C648::get_offset_of__objectReturnType_0(),
	OrderedDictionaryEnumerator_t09B48F68CFC2EEF1C7C57597E5AE3A4AF903C648::get_offset_of_arrayEnumerator_1(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2733 = { sizeof (OrderedDictionaryKeyValueCollection_t14B475F99F262455F11F305A747787FACF756B84), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable2733[2] = 
{
	OrderedDictionaryKeyValueCollection_t14B475F99F262455F11F305A747787FACF756B84::get_offset_of__objects_0(),
	OrderedDictionaryKeyValueCollection_t14B475F99F262455F11F305A747787FACF756B84::get_offset_of_isKeys_1(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2734 = { sizeof (StringCollection_tFF1A487B535F709103604F9DBC2C63FEB1434EFB), -1, 0, 0 };
extern const int32_t g_FieldOffsetTable2734[1] = 
{
	StringCollection_tFF1A487B535F709103604F9DBC2C63FEB1434EFB::get_offset_of_data_0(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2735 = { 0, 0, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2736 = { 0, 0, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2737 = { 0, 0, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2738 = { 0, 0, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2739 = { 0, 0, 0, 0 };
extern const int32_t g_FieldOffsetTable2739[5] = 
{
	0,
	0,
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2740 = { 0, 0, 0, 0 };
extern const int32_t g_FieldOffsetTable2740[5] = 
{
	0,
	0,
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2741 = { 0, 0, 0, 0 };
extern const int32_t g_FieldOffsetTable2741[4] = 
{
	0,
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2742 = { 0, 0, 0, 0 };
extern const int32_t g_FieldOffsetTable2742[6] = 
{
	0,
	0,
	0,
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2743 = { 0, 0, 0, 0 };
extern const int32_t g_FieldOffsetTable2743[4] = 
{
	0,
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2744 = { 0, 0, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2745 = { 0, 0, 0, 0 };
extern const int32_t g_FieldOffsetTable2745[8] = 
{
	0,
	0,
	0,
	0,
	0,
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2746 = { 0, 0, 0, 0 };
extern const int32_t g_FieldOffsetTable2746[6] = 
{
	0,
	0,
	0,
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2747 = { 0, 0, 0, 0 };
extern const int32_t g_FieldOffsetTable2747[4] = 
{
	0,
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2748 = { 0, 0, 0, 0 };
extern const int32_t g_FieldOffsetTable2748[4] = 
{
	0,
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2749 = { 0, 0, 0, 0 };
extern const int32_t g_FieldOffsetTable2749[1] = 
{
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2750 = { 0, 0, 0, 0 };
extern const int32_t g_FieldOffsetTable2750[1] = 
{
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2751 = { 0, 0, 0, 0 };
extern const int32_t g_FieldOffsetTable2751[4] = 
{
	0,
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2752 = { 0, 0, 0, 0 };
extern const int32_t g_FieldOffsetTable2752[4] = 
{
	0,
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2753 = { 0, 0, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2754 = { 0, 0, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2755 = { sizeof (U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291), -1, sizeof(U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields), 0 };
extern const int32_t g_FieldOffsetTable2755[17] = 
{
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_U30283A6AF88802AB45989B29549915BEA0F6CD515_0(),
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_U303F4297FCC30D0FD5E420E5D26E7FA711167C7EF_1(),
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_U31A39764B112685485A5BA7B2880D878B858C1A7A_2(),
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_U31A84029C80CB5518379F199F53FF08A7B764F8FD_3(),
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_U33BE77BF818331C2D8400FFFFF9FADD3F16AD89AC_4(),
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_U353437C3B2572EDB9B8640C3195DF3BC2729C5EA1_5(),
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_U359F5BD34B6C013DEACC784F69C67E95150033A84_6(),
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_U35BC3486B05BA8CF4689C7BDB198B3F477BB4E20C_7(),
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_U36D49C9D487D7AD3491ECE08732D68A593CC2038D_8(),
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_U36F3AD3DC3AF8047587C4C9D696EB68A01FEF796E_9(),
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_U38E0EF3D67A3EB1863224EE3CACB424BC2F8CFBA3_10(),
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_U398A44A6F8606AE6F23FE230286C1D6FBCC407226_11(),
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_C02C28AFEBE998F767E4AF43E3BE8F5E9FA11536_12(),
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_CCEEADA43268372341F81AE0C9208C6856441C04_13(),
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_E5BC1BAFADE1862DD6E0B9FB632BFAA6C3873A78_14(),
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_EC5842B3154E1AF94500B57220EB9F684BCCC42A_15(),
	U3CPrivateImplementationDetailsU3E_tD3F45A95FC1F3A32916F221D83F290D182AD6291_StaticFields::get_offset_of_EEAFE8C6E1AB017237567305EE925C976CDB6458_16(),
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2756 = { sizeof (__StaticArrayInitTypeSizeU3D3_t4D597C014C0C24F294DC84275F0264DCFCD4C575)+ sizeof (RuntimeObject), sizeof(__StaticArrayInitTypeSizeU3D3_t4D597C014C0C24F294DC84275F0264DCFCD4C575 ), 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2757 = { sizeof (__StaticArrayInitTypeSizeU3D6_tB024AE1C3AEB5C43235E76FFA23E64CD5EC3D87F)+ sizeof (RuntimeObject), sizeof(__StaticArrayInitTypeSizeU3D6_tB024AE1C3AEB5C43235E76FFA23E64CD5EC3D87F ), 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2758 = { sizeof (__StaticArrayInitTypeSizeU3D9_tAB3C7ADC1E437C21F21AAF2C925676D0F9801BCB)+ sizeof (RuntimeObject), sizeof(__StaticArrayInitTypeSizeU3D9_tAB3C7ADC1E437C21F21AAF2C925676D0F9801BCB ), 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2759 = { sizeof (__StaticArrayInitTypeSizeU3D10_tE6F7FB38485D609454F9A89335B38F479C5B6086)+ sizeof (RuntimeObject), sizeof(__StaticArrayInitTypeSizeU3D10_tE6F7FB38485D609454F9A89335B38F479C5B6086 ), 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2760 = { sizeof (__StaticArrayInitTypeSizeU3D12_t6EBCA221EDFF79F50821238316CFA0302EE70E48)+ sizeof (RuntimeObject), sizeof(__StaticArrayInitTypeSizeU3D12_t6EBCA221EDFF79F50821238316CFA0302EE70E48 ), 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2761 = { sizeof (__StaticArrayInitTypeSizeU3D14_tC5D421D768E79910C98FB4504BA3B07E43FA77F0)+ sizeof (RuntimeObject), sizeof(__StaticArrayInitTypeSizeU3D14_tC5D421D768E79910C98FB4504BA3B07E43FA77F0 ), 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2762 = { sizeof (__StaticArrayInitTypeSizeU3D32_t5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA)+ sizeof (RuntimeObject), sizeof(__StaticArrayInitTypeSizeU3D32_t5300E5FCBD58716E8A4EBB9470E4FAE1A0A964FA ), 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2763 = { sizeof (__StaticArrayInitTypeSizeU3D44_tE99A9434272A367C976B32D1235A23DA85CC9671)+ sizeof (RuntimeObject), sizeof(__StaticArrayInitTypeSizeU3D44_tE99A9434272A367C976B32D1235A23DA85CC9671 ), 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2764 = { sizeof (__StaticArrayInitTypeSizeU3D128_t4A42759E6E25B0C61E6036A661F4344DE92C2905)+ sizeof (RuntimeObject), sizeof(__StaticArrayInitTypeSizeU3D128_t4A42759E6E25B0C61E6036A661F4344DE92C2905 ), 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2765 = { sizeof (__StaticArrayInitTypeSizeU3D256_t548520FAA2CCFC11107E283BF9E43588FAE5F6C7)+ sizeof (RuntimeObject), sizeof(__StaticArrayInitTypeSizeU3D256_t548520FAA2CCFC11107E283BF9E43588FAE5F6C7 ), 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2766 = { sizeof (ConfigurationException_t35BCE2F5F07966C4500F43903BEFDF468E395FE7), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2767 = { 0, -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2768 = { sizeof (SettingsBase_t453379E2BF927282BC6DD98F1121BDB819EF2940), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2769 = { sizeof (SettingsContext_tB16593CFDBF1640E960767CCF354052721423CF7), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2770 = { sizeof (SettingsPropertyCollection_t542B41A7BB30DADC2C0B6C229515108847B84F98), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2771 = { sizeof (SettingsProperty_t0846992AB26C1256AB1BB63F8C6EF9D903B883FC), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2772 = { sizeof (SettingsProvider_tA65864D41FA268FA6A7CF31021AA6F0CFE1C149A), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2773 = { sizeof (SettingsPropertyValueCollection_tF506F16C9E5015DBAAF5AA4C29E03418DDF423ED), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2774 = { sizeof (SettingsPropertyValue_tEC71581CE766661B3FF55134336818C330A81939), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2775 = { sizeof (SettingsSerializeAs_tB7D9625AE7AAC3CBA5D013B21693C56AFA20D634)+ sizeof (RuntimeObject), sizeof(int32_t), 0, 0 };
extern const int32_t g_FieldOffsetTable2775[5] = 
{
	SettingsSerializeAs_tB7D9625AE7AAC3CBA5D013B21693C56AFA20D634::get_offset_of_value___2() + static_cast<int32_t>(sizeof(RuntimeObject)),
	0,
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2776 = { sizeof (SettingsAttributeDictionary_t7DAB45F9F421E95E78F6C215FB535B05E13D57FE), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2777 = { sizeof (SettingsProviderCollection_tC3B57BEEADC329BA242C343EF406A2B20BFAC1F5), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2778 = { 0, -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2779 = { sizeof (AuthenticationModuleElement_t849BD00E7B5095C8FAE3848BD08BA1D929FD1E80), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2780 = { sizeof (AuthenticationModuleElementCollection_tF7B605BB9AEB4E9C477799AE3A2A641E03A1D7DD), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2781 = { sizeof (AuthenticationModulesSection_tC01D5227FFEEA3135D0B519F7EF1425CE65DDC41), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2782 = { sizeof (BypassElement_t89C59A549C7A25609AA5C200352CD9E310172BAF), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2783 = { sizeof (BypassElementCollection_t5CCE032F76311FCEFC3128DA5A88D25568A234A7), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2784 = { sizeof (ConnectionManagementElement_tABDA95F63A9CBFC2720D7D3F15C5B352EC5CE7AD), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2785 = { sizeof (ConnectionManagementElementCollection_t83F843AEC2D2354836CC863E346FE2ECFEED2572), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2786 = { sizeof (ConnectionManagementSection_tA88F9BAD144E401AB524A9579B50050140592447), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2787 = { sizeof (DefaultProxySection_tB752851846FC0CEBA83C36C2BF6553211029AA3B), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2788 = { sizeof (ModuleElement_t99D93E6480F780316895A76DCC49E6AB7212507F), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2789 = { sizeof (ProxyElement_tBD5D75620576BA5BB5521C11D09E0A6E996F9449), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2790 = { sizeof (AutoDetectValues_t5FBC7CB20B71835CA991AD2467E1E855EDC2B3AB)+ sizeof (RuntimeObject), sizeof(int32_t), 0, 0 };
extern const int32_t g_FieldOffsetTable2790[4] = 
{
	AutoDetectValues_t5FBC7CB20B71835CA991AD2467E1E855EDC2B3AB::get_offset_of_value___2() + static_cast<int32_t>(sizeof(RuntimeObject)),
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2791 = { sizeof (BypassOnLocalValues_t42DDF7A85427F95302AEFDD4E70F5A4069622025)+ sizeof (RuntimeObject), sizeof(int32_t), 0, 0 };
extern const int32_t g_FieldOffsetTable2791[4] = 
{
	BypassOnLocalValues_t42DDF7A85427F95302AEFDD4E70F5A4069622025::get_offset_of_value___2() + static_cast<int32_t>(sizeof(RuntimeObject)),
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2792 = { sizeof (UseSystemDefaultValues_tAE0283B8FBFE533D3F8F9ED4E9BD937878A86A55)+ sizeof (RuntimeObject), sizeof(int32_t), 0, 0 };
extern const int32_t g_FieldOffsetTable2792[4] = 
{
	UseSystemDefaultValues_tAE0283B8FBFE533D3F8F9ED4E9BD937878A86A55::get_offset_of_value___2() + static_cast<int32_t>(sizeof(RuntimeObject)),
	0,
	0,
	0,
};
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2793 = { sizeof (FtpCachePolicyElement_t75AA63C2C74DCE55747733DB9F7F19EFA7C72D92), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2794 = { sizeof (HttpCachePolicyElement_t913ED10DF4916C215BC26E7D4F58D1B84B41E721), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2795 = { sizeof (HttpListenerElement_t160115D829A4163491F955C387C7A7B3924F93AF), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2796 = { sizeof (HttpListenerTimeoutsElement_t6C71FADDF95D0625B99DD4BBB266E518B6C8CEDD), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2797 = { sizeof (HttpWebRequestElement_t3E2FC0EB83C362CC92300949AF90A0B0BE01EA3D), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2798 = { sizeof (Ipv6Element_tCA869DC79FE3740DBDECC47877F1676294DB4A23), -1, 0, 0 };
extern const Il2CppTypeDefinitionSizes g_typeDefinitionSize2799 = { sizeof (MailSettingsSectionGroup_tDF447B7F8FB2FD1788402B2B8350DD8F3C74DBCB), -1, 0, 0 };
#ifdef __clang__
#pragma clang diagnostic pop
#endif
