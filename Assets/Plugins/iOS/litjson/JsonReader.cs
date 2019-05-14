#region Header

using System;
using System.Collections;
using System.Globalization;
using System.IO;

#endregion

namespace LitJson {
    /**
     */
    public enum JsonToken {
        /**
         */
        None,

        /**
         */
        ObjectStart,

        /**
         */
        PropertyName,

        /**
         */
        ObjectEnd,

        /**
         */
        ArrayStart,

        /**
         */
        ArrayEnd,

        /**
         */
        Int,

        /**
         */
        Long,

        /**
         */
        Double,

        /**
         */
        String,

        /**
         */
        Boolean,

        /**
         */
        Null
    }


    /**
     * JsonReader.cs
     *   Stream-like access to JSON text.
     *
     * This file was modified from the original to not use System.Collection.Generics namespace.
     *
     * The authors disclaim copyright to this source code. For more details, see
     * the COPYING file included with this distribution.
     **/
    public class JsonReader {
        #region Fields

        static IDictionary parse_table;

        Stack automaton_stack;
        int current_input;
        int current_symbol;
        bool end_of_json;
        bool end_of_input;
        Lexer lexer;
        bool parser_in_string;
        bool parser_return;
        bool read_started;
        TextReader reader;
        bool reader_is_owned;
        object token_value;
        JsonToken token;

        #endregion


        #region Public Properties

        /**
         */
        public bool AllowComments {
            get { return this.lexer.AllowComments; }
            set { this.lexer.AllowComments = value; }
        }

        /**
         */
        public bool AllowSingleQuotedStrings {
            get { return this.lexer.AllowSingleQuotedStrings; }
            set { this.lexer.AllowSingleQuotedStrings = value; }
        }

        /**
         */
        public bool EndOfInput {
            get { return this.end_of_input; }
        }

        /**
         */
        public bool EndOfJson {
            get { return this.end_of_json; }
        }

        /**
         */
        public JsonToken Token {
            get { return this.token; }
        }

        /**
         */
        public object Value {
            get { return this.token_value; }
        }

        #endregion


        #region Constructors

        static JsonReader() {
            PopulateParseTable();
        }

        /**
         */
        public JsonReader(string json_text) :
            this(new StringReader(json_text), true) {
        }

        /**
         */
        public JsonReader(TextReader reader) :
            this(reader, false) {
        }

        JsonReader(TextReader reader, bool owned) {
            if (reader == null) {
                throw new ArgumentNullException("reader");
            }

            this.parser_in_string = false;
            this.parser_return = false;

            this.read_started = false;
            this.automaton_stack = new Stack();
            this.automaton_stack.Push((int) ParserToken.End);
            this.automaton_stack.Push((int) ParserToken.Text);

            this.lexer = new Lexer(reader);

            this.end_of_input = false;
            this.end_of_json = false;

            this.reader = reader;
            this.reader_is_owned = owned;
        }

        #endregion


        #region Static Methods

        static void PopulateParseTable() {
            parse_table = new Hashtable();

            TableAddRow(ParserToken.Array);
            TableAddCol(ParserToken.Array, '[',
                '[',
                (int) ParserToken.ArrayPrime);

            TableAddRow(ParserToken.ArrayPrime);
            TableAddCol(ParserToken.ArrayPrime, '"',
                (int) ParserToken.Value,
                (int) ParserToken.ValueRest,
                ']');
            TableAddCol(ParserToken.ArrayPrime, '[',
                (int) ParserToken.Value,
                (int) ParserToken.ValueRest,
                ']');
            TableAddCol(ParserToken.ArrayPrime, ']',
                ']');
            TableAddCol(ParserToken.ArrayPrime, '{',
                (int) ParserToken.Value,
                (int) ParserToken.ValueRest,
                ']');
            TableAddCol(ParserToken.ArrayPrime, (int) ParserToken.Number,
                (int) ParserToken.Value,
                (int) ParserToken.ValueRest,
                ']');
            TableAddCol(ParserToken.ArrayPrime, (int) ParserToken.True,
                (int) ParserToken.Value,
                (int) ParserToken.ValueRest,
                ']');
            TableAddCol(ParserToken.ArrayPrime, (int) ParserToken.False,
                (int) ParserToken.Value,
                (int) ParserToken.ValueRest,
                ']');
            TableAddCol(ParserToken.ArrayPrime, (int) ParserToken.Null,
                (int) ParserToken.Value,
                (int) ParserToken.ValueRest,
                ']');

            TableAddRow(ParserToken.Object);
            TableAddCol(ParserToken.Object, '{',
                '{',
                (int) ParserToken.ObjectPrime);

            TableAddRow(ParserToken.ObjectPrime);
            TableAddCol(ParserToken.ObjectPrime, '"',
                (int) ParserToken.Pair,
                (int) ParserToken.PairRest,
                '}');
            TableAddCol(ParserToken.ObjectPrime, '}',
                '}');

            TableAddRow(ParserToken.Pair);
            TableAddCol(ParserToken.Pair, '"',
                (int) ParserToken.String,
                ':',
                (int) ParserToken.Value);

            TableAddRow(ParserToken.PairRest);
            TableAddCol(ParserToken.PairRest, ',',
                ',',
                (int) ParserToken.Pair,
                (int) ParserToken.PairRest);
            TableAddCol(ParserToken.PairRest, '}',
                (int) ParserToken.Epsilon);

            TableAddRow(ParserToken.String);
            TableAddCol(ParserToken.String, '"',
                '"',
                (int) ParserToken.CharSeq,
                '"');

            TableAddRow(ParserToken.Text);
            TableAddCol(ParserToken.Text, '[',
                (int) ParserToken.Array);
            TableAddCol(ParserToken.Text, '{',
                (int) ParserToken.Object);

            TableAddRow(ParserToken.Value);
            TableAddCol(ParserToken.Value, '"',
                (int) ParserToken.String);
            TableAddCol(ParserToken.Value, '[',
                (int) ParserToken.Array);
            TableAddCol(ParserToken.Value, '{',
                (int) ParserToken.Object);
            TableAddCol(ParserToken.Value, (int) ParserToken.Number,
                (int) ParserToken.Number);
            TableAddCol(ParserToken.Value, (int) ParserToken.True,
                (int) ParserToken.True);
            TableAddCol(ParserToken.Value, (int) ParserToken.False,
                (int) ParserToken.False);
            TableAddCol(ParserToken.Value, (int) ParserToken.Null,
                (int) ParserToken.Null);

            TableAddRow(ParserToken.ValueRest);
            TableAddCol(ParserToken.ValueRest, ',',
                ',',
                (int) ParserToken.Value,
                (int) ParserToken.ValueRest);
            TableAddCol(ParserToken.ValueRest, ']',
                (int) ParserToken.Epsilon);
        }

        static void TableAddCol(ParserToken row, int col,
            params int[] symbols) {
            (parse_table[(int) row] as IDictionary).Add(col, symbols);
        }

        static void TableAddRow(ParserToken rule) {
            parse_table.Add((int) rule, new Hashtable());
        }

        #endregion


        #region Private Methods

        void ProcessNumber(string number) {
            if (number.IndexOf('.') != -1 ||
                number.IndexOf('e') != -1 ||
                number.IndexOf('E') != -1) {
                double n_double;

                try {
                    NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                    n_double = double.Parse(number, nfi);
                    this.token = JsonToken.Double;
                    this.token_value = n_double;
                    return;
                }
                catch {
                }
            }

            int n_int32;

            try {
                n_int32 = int.Parse(number);
                this.token = JsonToken.Int;
                this.token_value = n_int32;
                return;
            }
            catch {
            }

            long n_int64;
            try {
                n_int64 = long.Parse(number);
                this.token = JsonToken.Long;
                this.token_value = n_int64;
                return;
            }
            catch {
            }

            // Shouldn't happen, but just in case, return something
            this.token = JsonToken.Int;
            this.token_value = 0;
        }

        void ProcessSymbol() {
            if (this.current_symbol == '[') {
                this.token = JsonToken.ArrayStart;
                this.parser_return = true;
            }
            else if (this.current_symbol == ']') {
                this.token = JsonToken.ArrayEnd;
                this.parser_return = true;
            }
            else if (this.current_symbol == '{') {
                this.token = JsonToken.ObjectStart;
                this.parser_return = true;
            }
            else if (this.current_symbol == '}') {
                this.token = JsonToken.ObjectEnd;
                this.parser_return = true;
            }
            else if (this.current_symbol == '"') {
                if (this.parser_in_string) {
                    this.parser_in_string = false;

                    this.parser_return = true;
                }
                else {
                    if (this.token == JsonToken.None) {
                        this.token = JsonToken.String;
                    }

                    this.parser_in_string = true;
                }
            }
            else if (this.current_symbol == (int) ParserToken.CharSeq) {
                this.token_value = this.lexer.StringValue;
            }
            else if (this.current_symbol == (int) ParserToken.False) {
                this.token = JsonToken.Boolean;
                this.token_value = false;
                this.parser_return = true;
            }
            else if (this.current_symbol == (int) ParserToken.Null) {
                this.token = JsonToken.Null;
                this.parser_return = true;
            }
            else if (this.current_symbol == (int) ParserToken.Number) {
                this.ProcessNumber(this.lexer.StringValue);

                this.parser_return = true;
            }
            else if (this.current_symbol == (int) ParserToken.Pair) {
                this.token = JsonToken.PropertyName;
            }
            else if (this.current_symbol == (int) ParserToken.True) {
                this.token = JsonToken.Boolean;
                this.token_value = true;
                this.parser_return = true;
            }
        }

        bool ReadToken() {
            if (this.end_of_input) {
                return false;
            }

            this.lexer.NextToken();

            if (this.lexer.EndOfInput) {
                this.Close();

                return false;
            }

            this.current_input = this.lexer.Token;

            return true;
        }

        #endregion


        /**
         */
        public void Close() {
            if (this.end_of_input) {
                return;
            }

            this.end_of_input = true;
            this.end_of_json = true;

            if (this.reader_is_owned) {
                this.reader.Close();
            }

            this.reader = null;
        }

        /**
         */
        public bool Read() {
            if (this.end_of_input) {
                return false;
            }

            if (this.end_of_json) {
                this.end_of_json = false;
                this.automaton_stack.Clear();
                this.automaton_stack.Push((int) ParserToken.End);
                this.automaton_stack.Push((int) ParserToken.Text);
            }

            this.parser_in_string = false;
            this.parser_return = false;

            this.token = JsonToken.None;
            this.token_value = null;

            if (!this.read_started) {
                this.read_started = true;

                if (!this.ReadToken()) {
                    return false;
                }
            }


            int[] entry_symbols;

            while (true) {
                if (this.parser_return) {
                    if ((int) this.automaton_stack.Peek() == (int) ParserToken.End) {
                        this.end_of_json = true;
                    }

                    return true;
                }

                this.current_symbol = (int) this.automaton_stack.Pop();

                this.ProcessSymbol();

                if (this.current_symbol == this.current_input) {
                    if (!this.ReadToken()) {
                        if ((int) this.automaton_stack.Peek() != (int) ParserToken.End) {
                            throw new JsonException(
                                "Input doesn't evaluate to proper JSON text");
                        }

                        if (this.parser_return) {
                            return true;
                        }

                        return false;
                    }

                    continue;
                }

                try {
                    entry_symbols = (int[]) ((parse_table[this.current_symbol] as IDictionary)[this.current_input]);
                }
                catch (Exception e) {
                    throw new JsonException((ParserToken) this.current_input, e);
                }

                if (entry_symbols[0] == (int) ParserToken.Epsilon) {
                    continue;
                }

                for (int i = entry_symbols.Length - 1; i >= 0; i--) {
                    this.automaton_stack.Push(entry_symbols[i]);
                }
            }
        }
    }
}