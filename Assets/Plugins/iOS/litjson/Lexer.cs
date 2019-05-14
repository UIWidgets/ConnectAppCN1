#region Header

using System;
using System.IO;
using System.Text;

#endregion


namespace LitJson {
    class FsmContext {
        public bool Return;
        public int NextState;
        public Lexer L;
        public int StateStack;
    }

    /**
     * Lexer.cs
     *   JSON lexer implementation based on a finite state machine.
     *
     * This file was modified from the original to not use System.Collection.Generics namespace.
     * 
     * The authors disclaim copyright to this source code. For more details, see
     * the COPYING file included with this distribution.
     **/

    class Lexer {
        #region Fields

        delegate bool StateHandler(FsmContext ctx);

        static int[] fsm_return_table;
        static StateHandler[] fsm_handler_table;

        bool allow_comments;
        bool allow_single_quoted_strings;
        bool end_of_input;
        FsmContext fsm_context;
        int input_buffer;
        int input_char;
        TextReader reader;
        int state;
        StringBuilder string_buffer;
        string string_value;
        int token;
        int unichar;

        #endregion


        #region Properties

        public bool AllowComments {
            get { return this.allow_comments; }
            set { this.allow_comments = value; }
        }

        public bool AllowSingleQuotedStrings {
            get { return this.allow_single_quoted_strings; }
            set { this.allow_single_quoted_strings = value; }
        }

        public bool EndOfInput {
            get { return this.end_of_input; }
        }

        public int Token {
            get { return this.token; }
        }

        public string StringValue {
            get { return this.string_value; }
        }

        #endregion


        #region Constructors

        static Lexer() {
            PopulateFsmTables();
        }

        public Lexer(TextReader reader) {
            this.allow_comments = true;
            this.allow_single_quoted_strings = true;

            this.input_buffer = 0;
            this.string_buffer = new StringBuilder(128);
            this.state = 1;
            this.end_of_input = false;
            this.reader = reader;

            this.fsm_context = new FsmContext();
            this.fsm_context.L = this;
        }

        #endregion


        #region Static Methods

        static int HexValue(int digit) {
            switch (digit) {
                case 'a':
                case 'A':
                    return 10;

                case 'b':
                case 'B':
                    return 11;

                case 'c':
                case 'C':
                    return 12;

                case 'd':
                case 'D':
                    return 13;

                case 'e':
                case 'E':
                    return 14;

                case 'f':
                case 'F':
                    return 15;

                default:
                    return digit - '0';
            }
        }

        static void PopulateFsmTables() {
            fsm_handler_table = new StateHandler[28] {
                State1,
                State2,
                State3,
                State4,
                State5,
                State6,
                State7,
                State8,
                State9,
                State10,
                State11,
                State12,
                State13,
                State14,
                State15,
                State16,
                State17,
                State18,
                State19,
                State20,
                State21,
                State22,
                State23,
                State24,
                State25,
                State26,
                State27,
                State28
            };

            fsm_return_table = new int[28] {
                (int) ParserToken.Char,
                0,
                (int) ParserToken.Number,
                (int) ParserToken.Number,
                0,
                (int) ParserToken.Number,
                0,
                (int) ParserToken.Number,
                0,
                0,
                (int) ParserToken.True,
                0,
                0,
                0,
                (int) ParserToken.False,
                0,
                0,
                (int) ParserToken.Null,
                (int) ParserToken.CharSeq,
                (int) ParserToken.Char,
                0,
                0,
                (int) ParserToken.CharSeq,
                (int) ParserToken.Char,
                0,
                0,
                0,
                0
            };
        }

        static char ProcessEscChar(int esc_char) {
            switch (esc_char) {
                case '"':
                case '\'':
                case '\\':
                case '/':
                    return Convert.ToChar(esc_char);

                case 'n':
                    return '\n';

                case 't':
                    return '\t';

                case 'r':
                    return '\r';

                case 'b':
                    return '\b';

                case 'f':
                    return '\f';

                default:
                    // Unreachable
                    return '?';
            }
        }

        static bool State1(FsmContext ctx) {
            while (ctx.L.GetChar()) {
                if (ctx.L.input_char == ' ' ||
                    ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r') {
                    continue;
                }

                if (ctx.L.input_char >= '1' && ctx.L.input_char <= '9') {
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                    ctx.NextState = 3;
                    return true;
                }

                switch (ctx.L.input_char) {
                    case '"':
                        ctx.NextState = 19;
                        ctx.Return = true;
                        return true;

                    case ',':
                    case ':':
                    case '[':
                    case ']':
                    case '{':
                    case '}':
                        ctx.NextState = 1;
                        ctx.Return = true;
                        return true;

                    case '-':
                        ctx.L.string_buffer.Append((char) ctx.L.input_char);
                        ctx.NextState = 2;
                        return true;

                    case '0':
                        ctx.L.string_buffer.Append((char) ctx.L.input_char);
                        ctx.NextState = 4;
                        return true;

                    case 'f':
                        ctx.NextState = 12;
                        return true;

                    case 'n':
                        ctx.NextState = 16;
                        return true;

                    case 't':
                        ctx.NextState = 9;
                        return true;

                    case '\'':
                        if (!ctx.L.allow_single_quoted_strings) {
                            return false;
                        }

                        ctx.L.input_char = '"';
                        ctx.NextState = 23;
                        ctx.Return = true;
                        return true;

                    case '/':
                        if (!ctx.L.allow_comments) {
                            return false;
                        }

                        ctx.NextState = 25;
                        return true;

                    default:
                        return false;
                }
            }

            return true;
        }

        static bool State2(FsmContext ctx) {
            ctx.L.GetChar();

            if (ctx.L.input_char >= '1' && ctx.L.input_char <= '9') {
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
                ctx.NextState = 3;
                return true;
            }

            switch (ctx.L.input_char) {
                case '0':
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                    ctx.NextState = 4;
                    return true;

                default:
                    return false;
            }
        }

        static bool State3(FsmContext ctx) {
            while (ctx.L.GetChar()) {
                if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9') {
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                    continue;
                }

                if (ctx.L.input_char == ' ' ||
                    ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r') {
                    ctx.Return = true;
                    ctx.NextState = 1;
                    return true;
                }

                switch (ctx.L.input_char) {
                    case ',':
                    case ']':
                    case '}':
                        ctx.L.UngetChar();
                        ctx.Return = true;
                        ctx.NextState = 1;
                        return true;

                    case '.':
                        ctx.L.string_buffer.Append((char) ctx.L.input_char);
                        ctx.NextState = 5;
                        return true;

                    case 'e':
                    case 'E':
                        ctx.L.string_buffer.Append((char) ctx.L.input_char);
                        ctx.NextState = 7;
                        return true;

                    default:
                        return false;
                }
            }

            return true;
        }

        static bool State4(FsmContext ctx) {
            ctx.L.GetChar();

            if (ctx.L.input_char == ' ' ||
                ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r') {
                ctx.Return = true;
                ctx.NextState = 1;
                return true;
            }

            switch (ctx.L.input_char) {
                case ',':
                case ']':
                case '}':
                    ctx.L.UngetChar();
                    ctx.Return = true;
                    ctx.NextState = 1;
                    return true;

                case '.':
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                    ctx.NextState = 5;
                    return true;

                case 'e':
                case 'E':
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                    ctx.NextState = 7;
                    return true;

                default:
                    return false;
            }
        }

        static bool State5(FsmContext ctx) {
            ctx.L.GetChar();

            if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9') {
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
                ctx.NextState = 6;
                return true;
            }

            return false;
        }

        static bool State6(FsmContext ctx) {
            while (ctx.L.GetChar()) {
                if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9') {
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                    continue;
                }

                if (ctx.L.input_char == ' ' ||
                    ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r') {
                    ctx.Return = true;
                    ctx.NextState = 1;
                    return true;
                }

                switch (ctx.L.input_char) {
                    case ',':
                    case ']':
                    case '}':
                        ctx.L.UngetChar();
                        ctx.Return = true;
                        ctx.NextState = 1;
                        return true;

                    case 'e':
                    case 'E':
                        ctx.L.string_buffer.Append((char) ctx.L.input_char);
                        ctx.NextState = 7;
                        return true;

                    default:
                        return false;
                }
            }

            return true;
        }

        static bool State7(FsmContext ctx) {
            ctx.L.GetChar();

            if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9') {
                ctx.L.string_buffer.Append((char) ctx.L.input_char);
                ctx.NextState = 8;
                return true;
            }

            switch (ctx.L.input_char) {
                case '+':
                case '-':
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                    ctx.NextState = 8;
                    return true;

                default:
                    return false;
            }
        }

        static bool State8(FsmContext ctx) {
            while (ctx.L.GetChar()) {
                if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9') {
                    ctx.L.string_buffer.Append((char) ctx.L.input_char);
                    continue;
                }

                if (ctx.L.input_char == ' ' ||
                    ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r') {
                    ctx.Return = true;
                    ctx.NextState = 1;
                    return true;
                }

                switch (ctx.L.input_char) {
                    case ',':
                    case ']':
                    case '}':
                        ctx.L.UngetChar();
                        ctx.Return = true;
                        ctx.NextState = 1;
                        return true;

                    default:
                        return false;
                }
            }

            return true;
        }

        static bool State9(FsmContext ctx) {
            ctx.L.GetChar();

            switch (ctx.L.input_char) {
                case 'r':
                    ctx.NextState = 10;
                    return true;

                default:
                    return false;
            }
        }

        static bool State10(FsmContext ctx) {
            ctx.L.GetChar();

            switch (ctx.L.input_char) {
                case 'u':
                    ctx.NextState = 11;
                    return true;

                default:
                    return false;
            }
        }

        static bool State11(FsmContext ctx) {
            ctx.L.GetChar();

            switch (ctx.L.input_char) {
                case 'e':
                    ctx.Return = true;
                    ctx.NextState = 1;
                    return true;

                default:
                    return false;
            }
        }

        static bool State12(FsmContext ctx) {
            ctx.L.GetChar();

            switch (ctx.L.input_char) {
                case 'a':
                    ctx.NextState = 13;
                    return true;

                default:
                    return false;
            }
        }

        static bool State13(FsmContext ctx) {
            ctx.L.GetChar();

            switch (ctx.L.input_char) {
                case 'l':
                    ctx.NextState = 14;
                    return true;

                default:
                    return false;
            }
        }

        static bool State14(FsmContext ctx) {
            ctx.L.GetChar();

            switch (ctx.L.input_char) {
                case 's':
                    ctx.NextState = 15;
                    return true;

                default:
                    return false;
            }
        }

        static bool State15(FsmContext ctx) {
            ctx.L.GetChar();

            switch (ctx.L.input_char) {
                case 'e':
                    ctx.Return = true;
                    ctx.NextState = 1;
                    return true;

                default:
                    return false;
            }
        }

        static bool State16(FsmContext ctx) {
            ctx.L.GetChar();

            switch (ctx.L.input_char) {
                case 'u':
                    ctx.NextState = 17;
                    return true;

                default:
                    return false;
            }
        }

        static bool State17(FsmContext ctx) {
            ctx.L.GetChar();

            switch (ctx.L.input_char) {
                case 'l':
                    ctx.NextState = 18;
                    return true;

                default:
                    return false;
            }
        }

        static bool State18(FsmContext ctx) {
            ctx.L.GetChar();

            switch (ctx.L.input_char) {
                case 'l':
                    ctx.Return = true;
                    ctx.NextState = 1;
                    return true;

                default:
                    return false;
            }
        }

        static bool State19(FsmContext ctx) {
            while (ctx.L.GetChar()) {
                switch (ctx.L.input_char) {
                    case '"':
                        ctx.L.UngetChar();
                        ctx.Return = true;
                        ctx.NextState = 20;
                        return true;

                    case '\\':
                        ctx.StateStack = 19;
                        ctx.NextState = 21;
                        return true;

                    default:
                        ctx.L.string_buffer.Append((char) ctx.L.input_char);
                        continue;
                }
            }

            return true;
        }

        static bool State20(FsmContext ctx) {
            ctx.L.GetChar();

            switch (ctx.L.input_char) {
                case '"':
                    ctx.Return = true;
                    ctx.NextState = 1;
                    return true;

                default:
                    return false;
            }
        }

        static bool State21(FsmContext ctx) {
            ctx.L.GetChar();

            switch (ctx.L.input_char) {
                case 'u':
                    ctx.NextState = 22;
                    return true;

                case '"':
                case '\'':
                case '/':
                case '\\':
                case 'b':
                case 'f':
                case 'n':
                case 'r':
                case 't':
                    ctx.L.string_buffer.Append(
                        ProcessEscChar(ctx.L.input_char));
                    ctx.NextState = ctx.StateStack;
                    return true;

                default:
                    return false;
            }
        }

        static bool State22(FsmContext ctx) {
            int counter = 0;
            int mult = 4096;

            ctx.L.unichar = 0;

            while (ctx.L.GetChar()) {
                if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9' ||
                    ctx.L.input_char >= 'A' && ctx.L.input_char <= 'F' ||
                    ctx.L.input_char >= 'a' && ctx.L.input_char <= 'f') {
                    ctx.L.unichar += HexValue(ctx.L.input_char) * mult;

                    counter++;
                    mult /= 16;

                    if (counter == 4) {
                        ctx.L.string_buffer.Append(
                            Convert.ToChar(ctx.L.unichar));
                        ctx.NextState = ctx.StateStack;
                        return true;
                    }

                    continue;
                }

                return false;
            }

            return true;
        }

        static bool State23(FsmContext ctx) {
            while (ctx.L.GetChar()) {
                switch (ctx.L.input_char) {
                    case '\'':
                        ctx.L.UngetChar();
                        ctx.Return = true;
                        ctx.NextState = 24;
                        return true;

                    case '\\':
                        ctx.StateStack = 23;
                        ctx.NextState = 21;
                        return true;

                    default:
                        ctx.L.string_buffer.Append((char) ctx.L.input_char);
                        continue;
                }
            }

            return true;
        }

        static bool State24(FsmContext ctx) {
            ctx.L.GetChar();

            switch (ctx.L.input_char) {
                case '\'':
                    ctx.L.input_char = '"';
                    ctx.Return = true;
                    ctx.NextState = 1;
                    return true;

                default:
                    return false;
            }
        }

        static bool State25(FsmContext ctx) {
            ctx.L.GetChar();

            switch (ctx.L.input_char) {
                case '*':
                    ctx.NextState = 27;
                    return true;

                case '/':
                    ctx.NextState = 26;
                    return true;

                default:
                    return false;
            }
        }

        static bool State26(FsmContext ctx) {
            while (ctx.L.GetChar()) {
                if (ctx.L.input_char == '\n') {
                    ctx.NextState = 1;
                    return true;
                }
            }

            return true;
        }

        static bool State27(FsmContext ctx) {
            while (ctx.L.GetChar()) {
                if (ctx.L.input_char == '*') {
                    ctx.NextState = 28;
                    return true;
                }
            }

            return true;
        }

        static bool State28(FsmContext ctx) {
            while (ctx.L.GetChar()) {
                if (ctx.L.input_char == '*') {
                    continue;
                }

                if (ctx.L.input_char == '/') {
                    ctx.NextState = 1;
                    return true;
                }

                ctx.NextState = 27;
                return true;
            }

            return true;
        }

        #endregion


        bool GetChar() {
            if ((this.input_char = this.NextChar()) != -1) {
                return true;
            }

            this.end_of_input = true;
            return false;
        }

        int NextChar() {
            if (this.input_buffer != 0) {
                int tmp = this.input_buffer;
                this.input_buffer = 0;

                return tmp;
            }

            return this.reader.Read();
        }

        public bool NextToken() {
            StateHandler handler;
            this.fsm_context.Return = false;

            while (true) {
                handler = fsm_handler_table[this.state - 1];

                if (!handler(this.fsm_context)) {
                    throw new JsonException(this.input_char);
                }

                if (this.end_of_input) {
                    return false;
                }

                if (this.fsm_context.Return) {
                    this.string_value = this.string_buffer.ToString();
                    this.string_buffer.Remove(0, this.string_buffer.Length);
                    this.token = fsm_return_table[this.state - 1];

                    if (this.token == (int) ParserToken.Char) {
                        this.token = this.input_char;
                    }

                    this.state = this.fsm_context.NextState;

                    return true;
                }

                this.state = this.fsm_context.NextState;
            }
        }

        void UngetChar() {
            this.input_buffer = this.input_char;
        }
    }
}