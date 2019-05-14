#region Header

using System;
using System.Collections;
using System.IO;

#endregion

namespace LitJson {
    /**
     */
    public class JsonKeyValuePair {
        /**
         */
        public JsonKeyValuePair(string key, JsonData data) {
            this.key = key;
            this.data = data;
        }

        JsonData data;
        string key;

        /**
         */
        public JsonData Value {
            set { this.data = value; }
            get { return this.data; }
        }

        /**
         */
        public string Key {
            set { this.key = value; }
            get { return this.key; }
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
    public class JsonData : IJsonWrapper, IDictionary, IComparable {
        #region Fields

        IList inst_array;

        bool inst_boolean;
        double inst_double;
        int inst_int;
        long inst_long;

        IDictionary inst_object;

        string inst_string;
        string json;
        JsonType type;

        // Used to implement the IOrderedDictionary interface
        IList object_list;

        #endregion


        #region Properties

        /**
         */
        public bool Contains(object key) {
            return this.inst_object.Contains(key);
        }

        /**
         */
        public int Count {
            get { return this.EnsureCollection().Count; }
        }

        /**
         */
        public bool IsArray {
            get { return this.type == JsonType.Array; }
        }

        /**
         */
        public bool IsBoolean {
            get { return this.type == JsonType.Boolean; }
        }

        /**
         */
        public bool IsDouble {
            get { return this.type == JsonType.Double; }
        }

        /**
         */
        public bool IsInt {
            get { return this.type == JsonType.Int; }
        }

        /**
         */
        public bool IsLong {
            get { return this.type == JsonType.Long; }
        }

        /**
         */
        public bool IsObject {
            get { return this.type == JsonType.Object; }
        }

        /**
         */
        public bool IsString {
            get { return this.type == JsonType.String; }
        }

        #endregion


        #region ICollection Properties

        int ICollection.Count {
            get { return this.Count; }
        }

        bool ICollection.IsSynchronized {
            get { return this.EnsureCollection().IsSynchronized; }
        }

        object ICollection.SyncRoot {
            get { return this.EnsureCollection().SyncRoot; }
        }

        #endregion


        #region IDictionary Properties

        bool IDictionary.IsFixedSize {
            get { return this.EnsureDictionary().IsFixedSize; }
        }

        bool IDictionary.IsReadOnly {
            get { return this.EnsureDictionary().IsReadOnly; }
        }

        ICollection IDictionary.Keys {
            get {
                this.EnsureDictionary();
                IList keys = new ArrayList();

                foreach (JsonKeyValuePair entry in this.object_list) {
                    keys.Add(entry.Key);
                }

                return (ICollection) keys;
            }
        }

        ICollection IDictionary.Values {
            get {
                this.EnsureDictionary();
                IList values = new ArrayList();

                foreach (JsonKeyValuePair entry in this.object_list) {
                    values.Add(entry.Value);
                }

                return (ICollection) values;
            }
        }

        #endregion


        #region IJsonWrapper Properties

        bool IJsonWrapper.IsArray {
            get { return this.IsArray; }
        }

        bool IJsonWrapper.IsBoolean {
            get { return this.IsBoolean; }
        }

        bool IJsonWrapper.IsDouble {
            get { return this.IsDouble; }
        }

        bool IJsonWrapper.IsInt {
            get { return this.IsInt; }
        }

        bool IJsonWrapper.IsLong {
            get { return this.IsLong; }
        }

        bool IJsonWrapper.IsObject {
            get { return this.IsObject; }
        }

        bool IJsonWrapper.IsString {
            get { return this.IsString; }
        }

        #endregion


        #region IList Properties

        bool IList.IsFixedSize {
            get { return this.EnsureList().IsFixedSize; }
        }

        bool IList.IsReadOnly {
            get { return this.EnsureList().IsReadOnly; }
        }

        #endregion


        #region IDictionary Indexer

        object IDictionary.this[object key] {
            get { return this.EnsureDictionary()[key]; }

            set {
                if (!(key is string)) {
                    throw new ArgumentException(
                        "The key has to be a string");
                }

                JsonData data = this.ToJsonData(value);

                this[(string) key] = data;
            }
        }

        #endregion

        #region IList Indexer

        object IList.this[int index] {
            get { return this.EnsureList()[index]; }

            set {
                this.EnsureList();
                JsonData data = this.ToJsonData(value);

                this[index] = data;
            }
        }

        #endregion


        #region Public Indexers

        /**
         */
        public JsonData this[string prop_name] {
            get {
                this.EnsureDictionary();
                return this.inst_object[prop_name] as JsonData;
            }

            set {
                this.EnsureDictionary();

                JsonKeyValuePair entry = new JsonKeyValuePair(prop_name, value);

                if (this.inst_object.Contains(prop_name)) {
                    for (int i = 0; i < this.object_list.Count; i++) {
                        if ((this.object_list[i] as JsonKeyValuePair).Key == prop_name) {
                            this.object_list[i] = entry;
                            break;
                        }
                    }
                }
                else {
                    this.object_list.Add(entry);
                }

                this.inst_object[prop_name] = value;

                this.json = null;
            }
        }

        /**
         */
        public JsonData this[int index] {
            get {
                this.EnsureCollection();

                if (this.type == JsonType.Array) {
                    return this.inst_array[index] as JsonData;
                }

                return (this.object_list[index] as JsonKeyValuePair).Value;
            }

            set {
                this.EnsureCollection();

                if (this.type == JsonType.Array) {
                    this.inst_array[index] = value;
                }
                else {
                    JsonKeyValuePair entry = this.object_list[index] as JsonKeyValuePair;
                    JsonKeyValuePair new_entry = new JsonKeyValuePair(entry.Key, value);

                    this.object_list[index] = new_entry;
                    this.inst_object[entry.Key] = value;
                }

                this.json = null;
            }
        }

        #endregion


        #region Constructors

        /**
         */
        public JsonData() {
        }

        /**
         */
        public JsonData(bool boolean) {
            this.type = JsonType.Boolean;
            this.inst_boolean = boolean;
        }

        /**
         */
        public JsonData(double number) {
            this.type = JsonType.Double;
            this.inst_double = number;
        }

        /**
         */
        public JsonData(int number) {
            this.type = JsonType.Int;
            this.inst_int = number;
        }

        /**
         */
        public JsonData(long number) {
            this.type = JsonType.Long;
            this.inst_long = number;
        }

        /**
         */
        public JsonData(object obj) {
            if (obj is bool) {
                this.type = JsonType.Boolean;
                this.inst_boolean = (bool) obj;
                return;
            }

            if (obj is double) {
                this.type = JsonType.Double;
                this.inst_double = (double) obj;
                return;
            }

            if (obj is int) {
                this.type = JsonType.Int;
                this.inst_int = (int) obj;
                return;
            }

            if (obj is long) {
                this.type = JsonType.Long;
                this.inst_long = (long) obj;
                return;
            }

            if (obj is string) {
                this.type = JsonType.String;
                this.inst_string = (string) obj;
                return;
            }

            throw new ArgumentException(
                "Unable to wrap the given object with JsonData");
        }

        /**
         */
        public JsonData(string str) {
            this.type = JsonType.String;
            this.inst_string = str;
        }

        #endregion


        #region Implicit Conversions

        /**
         */
        public static implicit operator JsonData(bool data) {
            return new JsonData(data);
        }

        /**
         */
        public static implicit operator JsonData(double data) {
            return new JsonData(data);
        }

        /**
         */
        public static implicit operator JsonData(int data) {
            return new JsonData(data);
        }

        /**
         */
        public static implicit operator JsonData(long data) {
            return new JsonData(data);
        }

        /**
         */
        public static implicit operator JsonData(string data) {
            return new JsonData(data);
        }

        #endregion


        #region Explicit Conversions

        /**
         */
        public static explicit operator bool(JsonData data) {
            if (data.type != JsonType.Boolean) {
                throw new InvalidCastException(
                    "Instance of JsonData doesn't hold a double");
            }

            return data.inst_boolean;
        }

        /**
         */
        public static explicit operator double(JsonData data) {
            if (data.type != JsonType.Double) {
                throw new InvalidCastException(
                    "Instance of JsonData doesn't hold a double");
            }

            return data.inst_double;
        }

        /**
         */
        public static explicit operator int(JsonData data) {
            if (data.type != JsonType.Int) {
                throw new InvalidCastException(
                    "Instance of JsonData doesn't hold an int");
            }

            return data.inst_int;
        }

        /**
         */
        public static explicit operator long(JsonData data) {
            if (data.type != JsonType.Long) {
                throw new InvalidCastException(
                    "Instance of JsonData doesn't hold an int");
            }

            return data.inst_long;
        }

        /**
         */
        public static explicit operator string(JsonData data) {
            if (data.type != JsonType.String) {
                throw new InvalidCastException(
                    "Instance of JsonData doesn't hold a string");
            }

            return data.inst_string;
        }

        #endregion


        #region ICollection Methods

        void ICollection.CopyTo(Array array, int index) {
            this.EnsureCollection().CopyTo(array, index);
        }

        #endregion


        #region IDictionary Methods

        void IDictionary.Add(object key, object value) {
            JsonData data = this.ToJsonData(value);

            this.EnsureDictionary().Add(key, data);

            JsonKeyValuePair entry = new JsonKeyValuePair((string) key, data);
            this.object_list.Add(entry);

            this.json = null;
        }

        void IDictionary.Clear() {
            this.EnsureDictionary().Clear();
            this.object_list.Clear();
            this.json = null;
        }

        bool IDictionary.Contains(object key) {
            return this.EnsureDictionary().Contains(key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator() {
            return this.GetOrderedEnumerator();
        }

        void IDictionary.Remove(object key) {
            this.EnsureDictionary().Remove(key);

            for (int i = 0; i < this.object_list.Count; i++) {
                if ((this.object_list[i] as JsonKeyValuePair).Key == (string) key) {
                    this.object_list.RemoveAt(i);
                    break;
                }
            }

            this.json = null;
        }

        #endregion


        #region IEnumerable Methods

        IEnumerator IEnumerable.GetEnumerator() {
            return this.EnsureCollection().GetEnumerator();
        }

        #endregion


        #region IJsonWrapper Methods

        bool IJsonWrapper.GetBoolean() {
            if (this.type != JsonType.Boolean) {
                throw new InvalidOperationException(
                    "JsonData instance doesn't hold a boolean");
            }

            return this.inst_boolean;
        }

        double IJsonWrapper.GetDouble() {
            if (this.type != JsonType.Double) {
                throw new InvalidOperationException(
                    "JsonData instance doesn't hold a double");
            }

            return this.inst_double;
        }

        int IJsonWrapper.GetInt() {
            if (this.type != JsonType.Int) {
                throw new InvalidOperationException(
                    "JsonData instance doesn't hold an int");
            }

            return this.inst_int;
        }

        long IJsonWrapper.GetLong() {
            if (this.type != JsonType.Long) {
                throw new InvalidOperationException(
                    "JsonData instance doesn't hold a long");
            }

            return this.inst_long;
        }

        string IJsonWrapper.GetString() {
            if (this.type != JsonType.String) {
                throw new InvalidOperationException(
                    "JsonData instance doesn't hold a string");
            }

            return this.inst_string;
        }

        void IJsonWrapper.SetBoolean(bool val) {
            this.type = JsonType.Boolean;
            this.inst_boolean = val;
            this.json = null;
        }

        void IJsonWrapper.SetDouble(double val) {
            this.type = JsonType.Double;
            this.inst_double = val;
            this.json = null;
        }

        void IJsonWrapper.SetInt(int val) {
            this.type = JsonType.Int;
            this.inst_int = val;
            this.json = null;
        }

        void IJsonWrapper.SetLong(long val) {
            this.type = JsonType.Long;
            this.inst_long = val;
            this.json = null;
        }

        void IJsonWrapper.SetString(string val) {
            this.type = JsonType.String;
            this.inst_string = val;
            this.json = null;
        }

        string IJsonWrapper.ToJson() {
            return this.ToJson();
        }

        void IJsonWrapper.ToJson(JsonWriter writer) {
            this.ToJson(writer);
        }

        #endregion


        #region IList Methods

        int IList.Add(object value) {
            return this.Add(value);
        }

        void IList.Clear() {
            this.EnsureList().Clear();
            this.json = null;
        }

        bool IList.Contains(object value) {
            return this.EnsureList().Contains(value);
        }

        int IList.IndexOf(object value) {
            return this.EnsureList().IndexOf(value);
        }

        void IList.Insert(int index, object value) {
            this.EnsureList().Insert(index, value);
            this.json = null;
        }

        void IList.Remove(object value) {
            this.EnsureList().Remove(value);
            this.json = null;
        }

        void IList.RemoveAt(int index) {
            this.EnsureList().RemoveAt(index);
            this.json = null;
        }

        #endregion


        #region Former IOrderedDictionary Methods

        IDictionaryEnumerator GetOrderedEnumerator() {
            this.EnsureDictionary();
            return new OrderedDictionaryEnumerator(this.object_list.GetEnumerator());
        }

        #endregion


        #region Private Methods

        ICollection EnsureCollection() {
            if (this.type == JsonType.Array) {
                return (ICollection) this.inst_array;
            }

            if (this.type == JsonType.Object) {
                return (ICollection) this.inst_object;
            }

            throw new InvalidOperationException(
                "The JsonData instance has to be initialized first");
        }

        IDictionary EnsureDictionary() {
            if (this.type == JsonType.Object) {
                return (IDictionary) this.inst_object;
            }

            if (this.type != JsonType.None) {
                throw new InvalidOperationException(
                    "Instance of JsonData is not a dictionary");
            }

            this.type = JsonType.Object;

            this.inst_object = new Hashtable();

            this.object_list = new ArrayList();

            return (IDictionary) this.inst_object;
        }

        IList EnsureList() {
            if (this.type == JsonType.Array) {
                return (IList) this.inst_array;
            }

            if (this.type != JsonType.None) {
                throw new InvalidOperationException(
                    "Instance of JsonData is not a list");
            }

            this.type = JsonType.Array;

            this.inst_array = new ArrayList();

            return (IList) this.inst_array;
        }

        JsonData ToJsonData(object obj) {
            if (obj == null) {
                return null;
            }

            if (obj is JsonData) {
                return (JsonData) obj;
            }

            return new JsonData(obj);
        }

        static void WriteJson(IJsonWrapper obj, JsonWriter writer) {
            if (obj.IsString) {
                writer.Write(obj.GetString());
                return;
            }

            if (obj.IsBoolean) {
                writer.Write(obj.GetBoolean());
                return;
            }

            if (obj.IsDouble) {
                writer.Write(obj.GetDouble());
                return;
            }

            if (obj.IsInt) {
                writer.Write(obj.GetInt());
                return;
            }

            if (obj.IsLong) {
                writer.Write(obj.GetLong());
                return;
            }

            if (obj.IsArray) {
                writer.WriteArrayStart();
                foreach (object elem in (IList) obj) {
                    WriteJson((JsonData) elem, writer);
                }

                writer.WriteArrayEnd();

                return;
            }

            if (obj.IsObject) {
                writer.WriteObjectStart();

                foreach (DictionaryEntry entry in ((IDictionary) obj)) {
                    writer.WritePropertyName((string) entry.Key);
                    WriteJson((JsonData) entry.Value, writer);
                }

                writer.WriteObjectEnd();

                return;
            }
        }

        #endregion


        /**
         */
        public int Add(object value) {
            JsonData data = this.ToJsonData(value);

            this.json = null;

            return this.EnsureList().Add(data);
        }

        /**
         */
        public void Clear() {
            if (this.IsObject) {
                ((IDictionary) this).Clear();
                return;
            }

            if (this.IsArray) {
                ((IList) this).Clear();
                return;
            }
        }

        /**
         */
        public int CompareTo(object obj) {
            JsonData x = obj as JsonData;

            if (x == null) {
                return -1;
            }

            if (x.type != this.type) {
                return -1;
            }

            switch (this.type) {
                case JsonType.None:
                    return 0;
                case JsonType.Object:
                    return this.inst_object.Equals(x.inst_object) ? 0 : -1;

                case JsonType.Array:
                    return this.inst_array.Equals(x.inst_array) ? 0 : -1;

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
        public JsonType GetJsonType() {
            return this.type;
        }

        /**
         */
        public void SetJsonType(JsonType type) {
            if (this.type == type) {
                return;
            }

            switch (type) {
                case JsonType.None:
                    break;

                case JsonType.Object:
                    this.inst_object = new Hashtable();
                    this.object_list = new ArrayList();

                    break;

                case JsonType.Array:
                    this.inst_array = new ArrayList();

                    break;

                case JsonType.String:
                    this.inst_string = "";
                    break;

                case JsonType.Int:
                    this.inst_int = new int();
                    break;

                case JsonType.Long:
                    this.inst_long = new long();
                    break;

                case JsonType.Double:
                    this.inst_double = new double();
                    break;

                case JsonType.Boolean:
                    this.inst_boolean = new bool();
                    break;
            }

            this.type = type;
        }

        /**
         */
        public string ToJson() {
            if (this.json != null) {
                return this.json;
            }

            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonWriter(sw);
            writer.Validate = false;

            WriteJson(this, writer);
            this.json = sw.ToString();

            return this.json;
        }

        /**
         */
        public void ToJson(JsonWriter writer) {
            bool old_validate = writer.Validate;

            writer.Validate = false;

            WriteJson(this, writer);

            writer.Validate = old_validate;
        }

        /**
         */
        public override string ToString() {
            switch (this.type) {
                case JsonType.Array:
                    return "JsonData array";

                case JsonType.Boolean:
                    return this.inst_boolean.ToString();

                case JsonType.Double:
                    return this.inst_double.ToString();

                case JsonType.Int:
                    return this.inst_int.ToString();

                case JsonType.Long:
                    return this.inst_long.ToString();

                case JsonType.Object:
                    return "JsonData object";

                case JsonType.String:
                    return this.inst_string;
            }

            return "Uninitialized JsonData";
        }
    }


    class OrderedDictionaryEnumerator : IDictionaryEnumerator {
        IEnumerator list_enumerator;

        public object Current {
            get { return this.Entry; }
        }

        public DictionaryEntry Entry {
            get {
                JsonKeyValuePair curr = this.list_enumerator.Current as JsonKeyValuePair;
                return new DictionaryEntry(curr.Key, curr.Value);
            }
        }

        public object Key {
            get { return (this.list_enumerator.Current as JsonKeyValuePair).Key; }
        }

        public object Value {
            get { return (this.list_enumerator.Current as JsonKeyValuePair).Value; }
        }


        public OrderedDictionaryEnumerator(IEnumerator enumerator) {
            this.list_enumerator = enumerator;
        }


        public bool MoveNext() {
            return this.list_enumerator.MoveNext();
        }

        public void Reset() {
            this.list_enumerator.Reset();
        }
    }
}