using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public enum Token_Class
{
    Main, Int, Float, String,
    Read, Write, Repeat, Until, If, ElseIf, Else, Then, Return, Endl, End,
    Dot, Semicolon, Comma, LCurlyParanthesis, RCurlyParanthesis, LRoundParanthesis, RRoundParanthesis,
    EqualOp, LessThanOp, GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp,
    DivideOp, AndOp, OrOp, AssignOp, Idenifier, Constant, Literal // string literal
}// comment

namespace Tiny_Compiler
{
    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            // TODO: Update ReservedWords to Match Tokens
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("elseif", Token_Class.ElseIf);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("main", Token_Class.Main);

            // TODO: Update Operators to match tokens
            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("{", Token_Class.LCurlyParanthesis);
            Operators.Add("}", Token_Class.RCurlyParanthesis);
            Operators.Add("(", Token_Class.LRoundParanthesis);
            Operators.Add(")", Token_Class.RRoundParanthesis);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("&&", Token_Class.AndOp);
            Operators.Add("||", Token_Class.OrOp);
            Operators.Add(":=", Token_Class.AssignOp);
        }

        // TODO: Update Scanning Function
        public void StartScanning(string SourceCode)
        {
            Errors.Error_List.Clear();   

            for (int i = 0; i < SourceCode.Length; i++)
            {
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;

                // Identifiers & Reserved keywords
                if (isLetter(CurrentChar) || CurrentChar == '_')
                {
                    while (i + 1 < SourceCode.Length && (isLetter(SourceCode[i + 1]) || isDigit(SourceCode[i + 1]) || SourceCode[i + 1] == '_'))
                    {
                        CurrentLexeme += SourceCode[++i];
                    }
                    FindTokenClass(CurrentLexeme);
                }
                // Number + MutliDot error case
                else if (isDigit(CurrentChar))
                {
                    int numOfDecPoint = 0;

                    while (i + 1 < SourceCode.Length && (isDigit(SourceCode[i + 1]) || SourceCode[i + 1] == '.'))
                    {
                        if (SourceCode[i + 1] == '.')
                        {
                            numOfDecPoint++;

                            if (numOfDecPoint > 1)
                            {
                                // generate the rest of the error lexeme
                                while (i + 1 < SourceCode.Length && (isDigit(SourceCode[i + 1]) || SourceCode[i + 1] == '.'))
                                {
                                    CurrentLexeme += SourceCode[++i];
                                }

                                Errors.Error_List.Add("More than one dot in number: " + CurrentLexeme);
                                break;
                            }
                        }

                        CurrentLexeme += SourceCode[++i];
                    }

                    if (numOfDecPoint < 2)
                    {
                        FindTokenClass(CurrentLexeme);
                    }

                }
                // String Literals + Unclosed string error case
                else if (CurrentChar == '\"')
                {
           
                    while (i + 1 < SourceCode.Length && SourceCode[i+1] != '\"')
                    {
                        CurrentLexeme += SourceCode[++i];
                    }

                    if (i + 1 < SourceCode.Length && SourceCode[i + 1] == '\"')
                    {
                        CurrentLexeme += SourceCode[++i];
                        FindTokenClass(CurrentLexeme);
                    }
                    else
                    {
                        Errors.Error_List.Add("Unclosed string literal: " + CurrentLexeme);
                    }

                }
                // Block Comment + Unclosed comment error case
                else if (SourceCode[i] == '/' && (i + 1 < SourceCode.Length && SourceCode[i + 1] == '*'))
                {
                    CurrentLexeme = "/*";
                    i += 2;

                    while (i + 1 < SourceCode.Length && !(SourceCode[i] == '*' && SourceCode[i + 1] == '/'))
                    {
                        CurrentLexeme += SourceCode[i];
                        i++;
                    }

                    if (i + 1 < SourceCode.Length && SourceCode[i] == '*' && SourceCode[i + 1] == '/')
                    {
                        CurrentLexeme += "*/";
                        i++;
                    }
                    else
                    {
                        CurrentLexeme += SourceCode[i];
                        Errors.Error_List.Add("Unclosed comment: " + CurrentLexeme);
                    }
                }
                // Operators
                else if (isOperator(CurrentLexeme) || CurrentChar == '|' || CurrentChar == '&' || CurrentChar == ':')  // operator or incompelete operator
                {
                    if (i + 1 < SourceCode.Length && (CurrentChar == ':' || CurrentChar == '|' || CurrentChar == '&' || CurrentChar == '<'))
                    {
                        string comp_operator = CurrentChar + SourceCode[i + 1].ToString(); // generate complete operator
                        if (isOperator(comp_operator))
                        {
                            CurrentLexeme = comp_operator;
                            i++;
                        }
                    }

                    FindTokenClass(CurrentLexeme);
                }
                // Error
                else
                {
                    Errors.Error_List.Add("Unrecognized token: " + CurrentLexeme);
                }
            }

            Tiny_Compiler.TokenStream = Tokens;
        }

        // TODO: Implement FindTokenClass
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;

            Lex=Lex.ToLower();

            if (isString(Lex))
            {
                Tok.token_type = Token_Class.Literal;
                Tokens.Add(Tok);
            }
            else if (isKeyWord(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }
            else if (isConstant(Lex))
            {
                Tok.token_type = Token_Class.Constant;
                Tokens.Add(Tok);
            }
            else if (isOperator(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);
            }
            else
            {
                Errors.Error_List.Add("Unrecognized token: " + Lex);
            }
        }

        // Helper Functions
        bool isLetter(char c)
        {
            return ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'));
        }
        bool isDigit(char c)
        {
            return (c >= '0' && c <= '9');
        }

        // TODO: Implement Checker Functions
        bool isOperator(string lex)
        {
            bool isValid = false;

            if (Operators.ContainsKey(lex))
            {
                isValid = true;
            }

            return isValid;
        }
        bool isKeyWord(string lex)
        {
            bool isValid = false;

            if (ReservedWords.ContainsKey(lex))
            {
                isValid = true;
            }

            return isValid;
        }
        bool isString(string lex)
        {
            bool isValid = false;
            var regx = new Regex("^\"[^\"]*\"$", RegexOptions.Compiled);

            if (regx.IsMatch(lex))
            {
                isValid = true;
            }

            return isValid;
        }
        bool isIdentifier(string lex)
        {
            bool isValid = false;
            var regx = new Regex(@"[_A-Za-z][_A-Za-z0-9]*", RegexOptions.Compiled);
            
            if (regx.IsMatch(lex))
            {
                isValid = true;
            }

            return isValid;
        }
        bool isConstant(string lex)
        {
            bool isValid = false;
            var regx = new Regex(@"[\+\-]?[0-9]+(\.[0-9]+)?", RegexOptions.Compiled);

            if (regx.IsMatch(lex))
            {
                isValid = true;
            }

            return isValid;
        }

    }
}
