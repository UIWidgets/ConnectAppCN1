#region Header

using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;

#endregion

namespace LitJson
{

	/**
	 */
	public class JsonKeyValuePair {
		/**
		 */
		public JsonKeyValuePair(string key, JsonData data) {
			this.key = key;
			this.data = data;
		}
		
		private JsonData data;
		private string key;

		/**
		 */
		public JsonData Value {
			set {
				data = value;
			}
			get {
				return data;
			}
		}

		/**
		 */
		public string Key {
			set {
				key = value;
			}
			get {
				return key;
			}
		}
		
	}

	/**
	 * JsonData.cs
	 *   Generic type to hold JSON data (objects, arrays, and so on). This is
	 *   the default type returned by JsonMapper.ToObject().
	 *
	 * This file was modified from the original to not use System.Collection.Generics namespace.	
	 *
	 * The authors disclaim copyright to this source code. For more details, see
	 * the COPYING file included with this distribution.
	 **/
	public class JsonData : IJsonWrapper, IDictionary, IComparable
    {
        #region Fields
        private IList			              inst_array;
        
        private bool                          inst_boolean;
        private double                        inst_double;
        private int                           inst_int;
        private long                          inst_long;
                
        private IDictionary inst_object;
        
        private string                        inst_string;
        private string                        json;
        private JsonType                      type;

        // Used to implement the IOrderedDictionary interface
        private IList object_list;
        
        #endregion


        #region Properties
		/**
		 */
		public bool Contains(object key) {
			return inst_object.Contains(key);
		}

		/**
		 */
		public int Count {
            get { return EnsureCollection ().Count; }
        }

		/**
		 */
		public bool IsArray {
            get { return type == JsonType.Array; }
        }

		/**
		 */
		public bool IsBoolean {
            get { return type == JsonType.Boolean; }
        }

		/**
		 */
		public bool IsDouble {
            get { return type == JsonType.Double; }
        }

		/**
		 */
		public bool IsInt {
            get { return type == JsonType.Int; }
        }

		/**
		 */
		public bool IsLong {
            get { return type == JsonType.Long; }
        }

		/**
		 */
		public bool IsObject {
            get { return type == JsonType.Object; }
        }

		/**
		 */
		public bool IsString {
            get { return type == JsonType.String; }
        }
        #endregion


        #region ICollection Properties
        int ICollection.Count {
            get {
                return Count;
            }
        }

        bool ICollection.IsSynchronized {
            get {
                return EnsureCollection ().IsSynchronized;
            }
        }

        object ICollection.SyncRoot {
            get {
                return EnsureCollection ().SyncRoot;
            }
        }
        #endregion


        #region IDictionary Properties
        bool IDictionary.IsFixedSize {
            get {
                return EnsureDictionary ().IsFixedSize;
            }
        }

        bool IDictionary.IsReadOnly {
            get {
                return EnsureDictionary ().IsReadOnly;
            }
        }

        ICollection IDictionary.Keys {
            get {
            	EnsureDictionary ();
                IList keys = new ArrayList();

                foreach (JsonKeyValuePair entry in object_list) {
                    keys.Add(entry.Key);
                }

                return (ICollection) keys;
                
            }
        }

        ICollection IDictionary.Values {
            get {
            	EnsureDictionary ();
                IList values = new ArrayList();

                foreach (JsonKeyValuePair entry in object_list) {
                    values.Add(entry.Value);
                }

               	return (ICollection) values;
            }
        }
        #endregion



        #region IJsonWrapper Properties
        bool IJsonWrapper.IsArray {
            get { return IsArray; }
        }

        bool IJsonWrapper.IsBoolean {
            get { return IsBoolean; }
        }

        bool IJsonWrapper.IsDouble {
            get { return IsDouble; }
        }

        bool IJsonWrapper.IsInt {
            get { return IsInt; }
        }

        bool IJsonWrapper.IsLong {
            get { return IsLong; }
        }

        bool IJsonWrapper.IsObject {
            get { return IsObject; }
        }

        bool IJsonWrapper.IsString {
            get { return IsString; }
        }
        #endregion


        #region IList Properties
        bool IList.IsFixedSize {
            get {
                return EnsureList ().IsFixedSize;
            }
        }

        bool IList.IsReadOnly {
            get {
                return EnsureList ().IsReadOnly;
            }
        }
        #endregion


        #region IDictionary Indexer
        object IDictionary.this[object key] {
            get {
                return EnsureDictionary ()[key];
            }

            set {
                if (! (key is String))
                    throw new ArgumentException (
                        "The key has to be a string");

                JsonData data = ToJsonData (value);

                this[(string) key] = data;
            }
        }
        #endregion

        #region IList Indexer
        object IList.this[int index] {
            get {
                return EnsureList ()[index];
            }

            set {
                EnsureList ();
                JsonData data = ToJsonData (value);

                this[index] = data;
            }
        }
        #endregion


        #region Public Indexers
		/**
		 */
		public JsonData this[string prop_name] {
            get {
            	EnsureDictionary ();
                return inst_object[prop_name] as JsonData;
                
            }

            set {
                EnsureDictionary ();

                JsonKeyValuePair entry = new JsonKeyValuePair(prop_name, value);
               
                if (inst_object.Contains(prop_name)) {
                    for (int i = 0; i < object_list.Count; i++) {
                        if ((object_list[i] as JsonKeyValuePair).Key == prop_name) {
                            object_list[i] = entry;
                            break;
                        }
                    }
                } else {
                    object_list.Add (entry);
                }    

                inst_object[prop_name] = value;

                json = null;
            }
        }

		/**
		 */
		public JsonData this[int index] {
            get {
                EnsureCollection ();

                if (type == JsonType.Array)
                    return inst_array[index] as JsonData;

                return (object_list[index] as JsonKeyValuePair).Value;
            }

            set {
                EnsureCollection ();

                if (type == JsonType.Array)
                    inst_array[index] = value;
                else {
                    JsonKeyValuePair entry = object_list[index] as JsonKeyValuePair;
                    JsonKeyValuePair new_entry = new JsonKeyValuePair(entry.Key, value);

                    object_list[index] = new_entry;
                    inst_object[entry.Key] = value;
                }

                json = null;
                
            }
        }
        #endregion


        #region Constructors
		/**
		 */
		public JsonData()
        {
        }

		/**
		 */
		public JsonData(bool boolean)
        {
            type = JsonType.Boolean;
            inst_boolean = boolean;
        }

		/**
		 */
		public JsonData(double number)
        {
            type = JsonType.Double;
            inst_double = number;
        }

		/**
		 */
		public JsonData(int number)
        {
            type = JsonType.Int;
            inst_int = number;
        }

		/**
		 */
		public JsonData(long number)
        {
            type = JsonType.Long;
            inst_long = number;
        }

		/**
		 */
		public JsonData(object obj)
        {
            if (obj is Boolean) {
                type = JsonType.Boolean;
                inst_boolean = (bool) obj;
                return;
            }

            if (obj is Double) {
                type = JsonType.Double;
                inst_double = (double) obj;
                return;
            }

            if (obj is Int32) {
                type = JsonType.Int;
                inst_int = (int) obj;
                return;
            }

            if (obj is Int64) {
                type = JsonType.Long;
                inst_long = (long) obj;
                return;
            }

            if (obj is String) {
                type = JsonType.String;
                inst_string = (string) obj;
                return;
            }

            throw new ArgumentException (
                "Unable to wrap the given object with JsonData");
        }

		/**
		 */
		public JsonData(string str)
        {
            type = JsonType.String;
            inst_string = str;
        }
        #endregion


        #region Implicit Conversions
		/**
		 */
		public static implicit operator JsonData(Boolean data)
        {
            return new JsonData (data);
        }

		/**
		 */
		public static implicit operator JsonData(Double data)
        {
            return new JsonData (data);
        }

		/**
		 */
		public static implicit operator JsonData(Int32 data)
        {
            return new JsonData (data);
        }

		/**
		 */
		public static implicit operator JsonData(Int64 data)
        {
            return new JsonData (data);
        }

		/**
		 */
		public static implicit operator JsonData(String data)
        {
            return new JsonData (data);
        }
        #endregion


        #region Explicit Conversions
		/**
		 */
		public static explicit operator Boolean(JsonData data)
        {
            if (data.type != JsonType.Boolean)
                throw new InvalidCastException (
                    "Instance of JsonData doesn't hold a double");

            return data.inst_boolean;
        }

		/**
		 */
		public static explicit operator Double(JsonData data)
        {
            if (data.type != JsonType.Double)
                throw new InvalidCastException (
                    "Instance of JsonData doesn't hold a double");

            return data.inst_double;
        }

		/**
		 */
		public static explicit operator Int32(JsonData data)
        {
            if (data.type != JsonType.Int)
                throw new InvalidCastException (
                    "Instance of JsonData doesn't hold an int");

            return data.inst_int;
        }

		/**
		 */
		public static explicit operator Int64(JsonData data)
        {
            if (data.type != JsonType.Long)
                throw new InvalidCastException (
                    "Instance of JsonData doesn't hold an int");

            return data.inst_long;
        }

		/**
		 */
		public static explicit operator String(JsonData data)
        {
            if (data.type != JsonType.String)
                throw new InvalidCastException (
                    "Instance of JsonData doesn't hold a string");

            return data.inst_string;
        }
        #endregion


        #region ICollection Methods
        void ICollection.CopyTo (Array array, int index)
        {
            EnsureCollection ().CopyTo (array, index);
        }
        #endregion


        #region IDictionary Methods
        void IDictionary.Add (object key, object value)
        {
        	JsonData data = ToJsonData (value);

            EnsureDictionary ().Add (key, data);

            JsonKeyValuePair entry = new JsonKeyValuePair((string) key, data);
            object_list.Add(entry);

            json = null;
        }

        void IDictionary.Clear ()
        {
            EnsureDictionary().Clear();
            object_list.Clear();
            json = null;
        }

        bool IDictionary.Contains (object key)
        {
            return EnsureDictionary ().Contains (key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator ()
        {
            return this.GetOrderedEnumerator();
        }

        void IDictionary.Remove (object key)
        {
        	EnsureDictionary().Remove(key);
		
            for (int i = 0; i < object_list.Count; i++) {
                if ((object_list[i] as JsonKeyValuePair).Key == (string) key) {
                    object_list.RemoveAt(i);
                    break;
                }
            }

            json = null;
        }
        #endregion


        #region IEnumerable Methods
        IEnumerator IEnumerable.GetEnumerator ()
        {
            return EnsureCollection ().GetEnumerator ();
        }
        #endregion


        #region IJsonWrapper Methods
        bool IJsonWrapper.GetBoolean ()
        {
            if (type != JsonType.Boolean)
                throw new InvalidOperationException (
                    "JsonData instance doesn't hold a boolean");

            return inst_boolean;
        }

        double IJsonWrapper.GetDouble ()
        {
            if (type != JsonType.Double)
                throw new InvalidOperationException (
                    "JsonData instance doesn't hold a double");

            return inst_double;
        }

        int IJsonWrapper.GetInt ()
        {
            if (type != JsonType.Int)
                throw new InvalidOperationException (
                    "JsonData instance doesn't hold an int");

            return inst_int;
        }

        long IJsonWrapper.GetLong ()
        {
            if (type != JsonType.Long)
                throw new InvalidOperationException (
                    "JsonData instance doesn't hold a long");

            return inst_long;
        }

        string IJsonWrapper.GetString ()
        {
            if (type != JsonType.String)
                throw new InvalidOperationException (
                    "JsonData instance doesn't hold a string");

            return inst_string;
        }

        void IJsonWrapper.SetBoolean (bool val)
        {
            type = JsonType.Boolean;
            inst_boolean = val;
            json = null;
        }

        void IJsonWrapper.SetDouble (double val)
        {
            type = JsonType.Double;
            inst_double = val;
            json = null;
        }

        void IJsonWrapper.SetInt (int val)
        {
            type = JsonType.Int;
            inst_int = val;
            json = null;
        }

        void IJsonWrapper.SetLong (long val)
        {
            type = JsonType.Long;
            inst_long = val;
            json = null;
        }

        void IJsonWrapper.SetString (string val)
        {
            type = JsonType.String;
            inst_string = val;
            json = null;
        }

        string IJsonWrapper.ToJson ()
        {
            return ToJson ();
        }

        void IJsonWrapper.ToJson (JsonWriter writer)
        {
            ToJson (writer);
        }
        #endregion


        #region IList Methods
        int IList.Add (object value)
        {
            return Add (value);
        }

        void IList.Clear ()
        {
            EnsureList ().Clear ();
            json = null;
        }

        bool IList.Contains (object value)
        {
            return EnsureList ().Contains (value);
        }

        int IList.IndexOf (object value)
        {
            return EnsureList ().IndexOf (value);
        }

        void IList.Insert (int index, object value)
        {
            EnsureList ().Insert (index, value);
            json = null;
        }

        void IList.Remove (object value)
        {
            EnsureList ().Remove (value);
            json = null;
        }

        void IList.RemoveAt (int index)
        {
            EnsureList ().RemoveAt (index);
            json = null;
        }
        #endregion


        #region Former IOrderedDictionary Methods
        
        private IDictionaryEnumerator GetOrderedEnumerator ()
        {
            EnsureDictionary ();
            return new OrderedDictionaryEnumerator (
                object_list.GetEnumerator());
        }
        
        #endregion


        #region Private Methods
        private ICollection EnsureCollection ()
        {
            if (type == JsonType.Array)
                return (ICollection) inst_array;

            if (type == JsonType.Object)
                return (ICollection) inst_object;

            throw new InvalidOperationException (
                "The JsonData instance has to be initialized first");
        }

        private IDictionary EnsureDictionary ()
        {
            if (type == JsonType.Object)
                return (IDictionary) inst_object;

            if (type != JsonType.None)
                throw new InvalidOperationException (
                    "Instance of JsonData is not a dictionary");

            type = JsonType.Object;
            
            inst_object = new Hashtable();
                        
            object_list = new ArrayList();

            return (IDictionary) inst_object;
        }

        private IList EnsureList ()
        {
            if (type == JsonType.Array)
                return (IList) inst_array;

            if (type != JsonType.None)
                throw new InvalidOperationException (
                    "Instance of JsonData is not a list");

            type = JsonType.Array;
            
            inst_array = new ArrayList();
				
            return (IList) inst_array;
        }

        private JsonData ToJsonData (object obj)
        {
            if (obj == null)
                return null;

            if (obj is JsonData)
                return (JsonData) obj;

            return new JsonData (obj);
        }

        private static void WriteJson (IJsonWrapper obj, JsonWriter writer)
        {
            if (obj.IsString) {
                writer.Write (obj.GetString ());
                return;
            }

            if (obj.IsBoolean) {
                writer.Write (obj.GetBoolean ());
                return;
            }

            if (obj.IsDouble) {
                writer.Write (obj.GetDouble ());
                return;
            }

            if (obj.IsInt) {
                writer.Write (obj.GetInt ());
                return;
            }

            if (obj.IsLong) {
                writer.Write (obj.GetLong ());
                return;
            }

            if (obj.IsArray) {
                writer.WriteArrayStart ();
                foreach (object elem in (IList) obj)
                    WriteJson ((JsonData) elem, writer);
                writer.WriteArrayEnd ();

                return;
            }

            if (obj.IsObject) {
                writer.WriteObjectStart ();

                foreach (DictionaryEntry entry in ((IDictionary) obj)) {
                    writer.WritePropertyName ((string) entry.Key);
                    WriteJson ((JsonData) entry.Value, writer);
                }
                writer.WriteObjectEnd ();

                return;
            }
        }
        #endregion


		/**
		 */
		public int Add(object value)
        {
            JsonData data = ToJsonData (value);

            json = null;

            return EnsureList ().Add (data);
        }

		/**
		 */
		public void Clear()
        {
            if (IsObject) {
                ((IDictionary) this).Clear ();
                return;
            }

            if (IsArray) {
                ((IList) this).Clear ();
                return;
            }
        }

		/**
		 */
		public int CompareTo(object obj)
        {
        	JsonData x = obj as JsonData;
        	
            if (x == null)
                return -1;

            if (x.type != this.type)
                return -1;

            switch (this.type) {
	            case JsonType.None:
	                return 0;
	            case JsonType.Object:
	                return this.inst_object.Equals(x.inst_object)?0:-1;
	
	            case JsonType.Array:
	                return this.inst_array.Equals(x.inst_array)?0:-1;
	
	            case JsonType.String:
	                return this.inst_string.CompareTo(x.inst_string);
	
	            case JsonType.Int:
	                return this.inst_int.CompareTo(x.inst_int);
	
	            case JsonType.Long:
	                return this.inst_long.CompareTo(x.inst_long);
	
	            case JsonType.Double:
	                return this.inst_double.CompareTo(x.inst_double);
	
	            case JsonType.Boolean:
	                return this.inst_boolean.CompareTo(x.inst_boolean);
            }

            return -1;
        }

		/**
		 */
		public JsonType GetJsonType()
        {
            return type;
        }

		/**
		 */
		public void SetJsonType(JsonType type)
        {
            if (this.type == type)
                return;

            switch (type) {
            case JsonType.None:
                break;

            case JsonType.Object:
            	inst_object = new Hashtable();
                object_list = new ArrayList();
                
                break;

            case JsonType.Array:
            	inst_array = new ArrayList();
                
                break;

            case JsonType.String:
                inst_string = "";
                break;

            case JsonType.Int:
                inst_int = new Int32();
                break;

            case JsonType.Long:
                inst_long = new Int64();
                break;

            case JsonType.Double:
                inst_double = new Double();
                break;

            case JsonType.Boolean:
                inst_boolean = new Boolean();
                break;
            }

            this.type = type;
        }

		/**
		 */
		public string ToJson()
        {
            if (json != null)
                return json;

            StringWriter sw = new StringWriter ();
            JsonWriter writer = new JsonWriter (sw);
            writer.Validate = false;

            WriteJson (this, writer);
            json = sw.ToString ();

            return json;
        }

		/**
		 */
		public void ToJson(JsonWriter writer)
        {
            bool old_validate = writer.Validate;

            writer.Validate = false;

            WriteJson (this, writer);

            writer.Validate = old_validate;
        }

		/**
		 */
		public override string ToString()
        {
            switch (type) {
            case JsonType.Array:
                return "JsonData array";

            case JsonType.Boolean:
                return inst_boolean.ToString ();

            case JsonType.Double:
                return inst_double.ToString ();

            case JsonType.Int:
                return inst_int.ToString ();

            case JsonType.Long:
                return inst_long.ToString ();

            case JsonType.Object:
                return "JsonData object";

            case JsonType.String:
                return inst_string;
            }

            return "Uninitialized JsonData";
        }
    }


    internal class OrderedDictionaryEnumerator : IDictionaryEnumerator
    {
        IEnumerator list_enumerator;

        public object Current {
            get { return Entry; }
        }

        public DictionaryEntry Entry {
            get {
            	JsonKeyValuePair curr = list_enumerator.Current as JsonKeyValuePair;
                return new DictionaryEntry(curr.Key, curr.Value);
            }
        }

        public object Key {
        	get { return (list_enumerator.Current as JsonKeyValuePair).Key; }
        }

        public object Value {
        	get { return (list_enumerator.Current as JsonKeyValuePair).Value; }
        }


        public OrderedDictionaryEnumerator (IEnumerator enumerator)
        {
            list_enumerator = enumerator;
        }


        public bool MoveNext ()
        {
            return list_enumerator.MoveNext();
        }

        public void Reset ()
        {
            list_enumerator.Reset();
        }
    }
}
