#region Header

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;

#endregion

namespace LitJson {
    enum Condition {
        InArray,
        InObject,
        NotAProperty,
        Property,
        Value
    }

    class WriterContext {
        public int Count;
        public bool InArray;
        public bool InObject;
        public bool ExpectingValue;
        public int Padding;
    }

    /**
     * JsonWriter.cs
     *   Stream-like facility to output JSON text.
     *
     * This file was modified from the original to not use System.Collection.Generics namespace.
     *
     * The authors disclaim copyright to this source code. For more details, see
     * the COPYING file included with this distribution.
     **/
    public class JsonWriter {
        #region Fields

        static NumberFormatInfo number_format;

        WriterContext context;
        Stack ctx_stack;
        bool has_reached_end;
        char[] hex_seq;
        int indentation;
        int indent_value;
        StringBuilder inst_string_builder;
        bool pretty_print;
        bool validate;
        TextWriter writer;

        #endregion


        #region Properties

        /**
         */
        public int IndentValue {
            get { return this.indent_value; }
            set {
                this.indentation = (this.indentation / this.indent_value) * value;
                this.indent_value = value;
            }
        }

        /**
         */
        public bool PrettyPrint {
            get { return this.pretty_print; }
            set { this.pretty_print = value; }
        }

        /**
         */
        public TextWriter TextWriter {
            get { return this.writer; }
        }

        /**
         */
        public bool Validate {
            get { return this.validate; }
            set { this.validate = value; }
        }

        #endregion


        #region Constructors

        static JsonWriter() {
            number_format = NumberFormatInfo.InvariantInfo;
        }

        /**
         */
        public JsonWriter() {
            this.inst_string_builder = new StringBuilder();
            this.writer = new StringWriter(this.inst_string_builder);

            this.Init();
        }

        /**
         */
        public JsonWriter(StringBuilder sb) :
            this(new StringWriter(sb)) {
        }

        /**
         */
        public JsonWriter(TextWriter writer) {
            if (writer == null) {
                throw new ArgumentNullException("writer");
            }

            this.writer = writer;

            this.Init();
        }

        #endregion


        #region Private Methods

        void DoValidation(Condition cond) {
            if (!this.context.ExpectingValue) {
                this.context.Count++;
            }

            if (!this.validate) {
                return;
            }

            if (this.has_reached_end) {
                throw new JsonException(
                    "A complete JSON symbol has already been written");
            }

            switch (cond) {
                case Condition.InArray:
                    if (!this.context.InArray) {
                        throw new JsonException(
                            "Can't close an array here");
                    }

                    break;

                case Condition.InObject:
                    if (!this.context.InObject || this.context.ExpectingValue) {
                        throw new JsonException(
                            "Can't close an object here");
                    }

                    break;

                case Condition.NotAProperty:
                    if (this.context.InObject && !this.context.ExpectingValue) {
                        throw new JsonException(
                            "Expected a property");
                    }

                    break;

                case Condition.Property:
                    if (!this.context.InObject || this.context.ExpectingValue) {
                        throw new JsonException(
                            "Can't add a property here");
                    }

                    break;

                case Condition.Value:
                    if (!this.context.InArray &&
                        (!this.context.InObject || !this.context.ExpectingValue)) {
                        throw new JsonException(
                            "Can't add a value here");
                    }

                    break;
            }
        }

        void Init() {
            this.has_reached_end = false;
            this.hex_seq = new char[4];
            this.indentation = 0;
            this.indent_value = 4;
            this.pretty_print = false;
            this.validate = true;

            this.ctx_stack = new Stack();
            this.context = new WriterContext();
            this.ctx_stack.Push(this.context);
        }

        static void IntToHex(int n, char[] hex) {
            int num;

            for (int i = 0; i < 4; i++) {
                num = n % 16;

                if (num < 10) {
                    hex[3 - i] = (char) ('0' + num);
                }
                else {
                    hex[3 - i] = (char) ('A' + (num - 10));
                }

                n >>= 4;
            }
        }

        void Indent() {
            if (this.pretty_print) {
                this.indentation += this.indent_value;
            }
        }


        void Put(string str) {
            if (this.pretty_print && !this.context.ExpectingValue) {
                for (int i = 0; i < this.indentation; i++) {
                    this.writer.Write(' ');
                }
            }

            this.writer.Write(str);
        }

        void PutNewline() {
            this.PutNewline(true);
        }

        void PutNewline(bool add_comma) {
            if (add_comma && !this.context.ExpectingValue && this.context.Count > 1) {
                this.writer.Write(',');
            }

            if (this.pretty_print && !this.context.ExpectingValue) {
                this.writer.Write('\n');
            }
        }

        void PutString(string str) {
            this.Put(string.Empty);

            this.writer.Write('"');

            int n = str.Length;
            for (int i = 0; i < n; i++) {
                switch (str[i]) {
                    case '\n':
                        this.writer.Write("\\n");
                        continue;

                    case '\r':
                        this.writer.Write("\\r");
                        continue;

                    case '\t':
                        this.writer.Write("\\t");
                        continue;

                    case '"':
                    case '\\':
                        this.writer.Write('\\');
                        this.writer.Write(str[i]);
                        continue;

                    case '\f':
                        this.writer.Write("\\f");
                        continue;

                    case '\b':
                        this.writer.Write("\\b");
                        continue;
                }

                if ((int) str[i] >= 32 && (int) str[i] <= 126) {
                    this.writer.Write(str[i]);
                    continue;
                }

                // Default, turn into a \uXXXX sequence
                IntToHex((int) str[i], this.hex_seq);
                this.writer.Write("\\u");
                this.writer.Write(this.hex_seq);
            }

            this.writer.Write('"');
        }

        void Unindent() {
            if (this.pretty_print) {
                this.indentation -= this.indent_value;
            }
        }

        #endregion


        /**
         */
        public override string ToString() {
            if (this.inst_string_builder == null) {
                return string.Empty;
            }

            return this.inst_string_builder.ToString();
        }

        /**
         */
        public void Reset() {
            this.has_reached_end = false;

            this.ctx_stack.Clear();
            this.context = new WriterContext();
            this.ctx_stack.Push(this.context);

            if (this.inst_string_builder != null) {
                this.inst_string_builder.Remove(0, this.inst_string_builder.Length);
            }
        }

        /**
         */
        public void Write(bool boolean) {
            this.DoValidation(Condition.Value);
            this.PutNewline();

            this.Put(boolean ? "true" : "false");

            this.context.ExpectingValue = false;
        }

        /**
         */
        public void Write(decimal number) {
            this.DoValidation(Condition.Value);
            this.PutNewline();

            this.Put(Convert.ToString(number, number_format));

            this.context.ExpectingValue = false;
        }

        /**
         */
        public void Write(double number) {
            this.DoValidation(Condition.Value);
            this.PutNewline();

            string str = Convert.ToString(number, number_format);
            this.Put(str);

            if (str.IndexOf('.') == -1 &&
                str.IndexOf('E') == -1) {
                this.writer.Write(".0");
            }

            this.context.ExpectingValue = false;
        }

        /**
         */
        public void Write(int number) {
            this.DoValidation(Condition.Value);
            this.PutNewline();

            this.Put(Convert.ToString(number, number_format));

            this.context.ExpectingValue = false;
        }

        /**
         */
        public void Write(long number) {
            this.DoValidation(Condition.Value);
            this.PutNewline();

            this.Put(Convert.ToString(number, number_format));

            this.context.ExpectingValue = false;
        }

        /**
         */
        public void Write(string str) {
            this.DoValidation(Condition.Value);
            this.PutNewline();

            if (str == null) {
                this.Put("null");
            }
            else {
                this.PutString(str);
            }

            this.context.ExpectingValue = false;
        }

        /**
         */
        public void Write(ulong number) {
            this.DoValidation(Condition.Value);
            this.PutNewline();

            this.Put(Convert.ToString(number, number_format));

            this.context.ExpectingValue = false;
        }

        /**
         */
        public void WriteArrayEnd() {
            this.DoValidation(Condition.InArray);
            this.PutNewline(false);

            this.ctx_stack.Pop();
            if (this.ctx_stack.Count == 1) {
                this.has_reached_end = true;
            }
            else {
                this.context = this.ctx_stack.Peek() as WriterContext;
                this.context.ExpectingValue = false;
            }

            this.Unindent();
            this.Put("]");
        }

        /**
         */
        public void WriteArrayStart() {
            this.DoValidation(Condition.NotAProperty);
            this.PutNewline();

            this.Put("[");

            this.context = new WriterContext();
            this.context.InArray = true;
            this.ctx_stack.Push(this.context);

            this.Indent();
        }

        /**
         */
        public void WriteObjectEnd() {
            this.DoValidation(Condition.InObject);
            this.PutNewline(false);

            this.ctx_stack.Pop();
            if (this.ctx_stack.Count == 1) {
                this.has_reached_end = true;
            }
            else {
                this.context = this.ctx_stack.Peek() as WriterContext;
                this.context.ExpectingValue = false;
            }

            this.Unindent();
            this.Put("}");
        }

        /**
         */
        public void WriteObjectStart() {
            this.DoValidation(Condition.NotAProperty);
            this.PutNewline();

            this.Put("{");

            this.context = new WriterContext();
            this.context.InObject = true;
            this.ctx_stack.Push(this.context);

            this.Indent();
        }

        /**
         */
        public void WritePropertyName(string property_name) {
            this.DoValidation(Condition.Property);
            this.PutNewline();

            this.PutString(property_name);

            if (this.pretty_print) {
                if (property_name.Length > this.context.Padding) {
                    this.context.Padding = property_name.Length;
                }

                for (int i = this.context.Padding - property_name.Length;
                    i >= 0;
                    i--) {
                    this.writer.Write(' ');
                }

                this.writer.Write(": ");
            }
            else {
                this.writer.Write(':');
            }

            this.context.ExpectingValue = true;
        }
    }
}