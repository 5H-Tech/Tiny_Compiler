using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public enum Token_Class
{
    Else, Endl, If, Int, Number,FloatNum,Main,
    Read, Then, Until, Repeat, Write, Comment, Return, Float, String, Elseif, Orsign,
    Dot, Semicolon, Comma, LParanthesis, RParanthesis, EqualOp, LessThanOp, Colon, ASSign, Andsign, END,
    GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, AndOp, OrOp, DivideOp, LeftPracit, RightPracit,
    Idenifier, Constant
}
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
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("main", Token_Class.Main);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("end", Token_Class.END);
          

            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add(":", Token_Class.Colon);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add(":=", Token_Class.ASSign);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("{", Token_Class.LeftPracit);
            Operators.Add("}", Token_Class.RightPracit);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("&", Token_Class.Andsign);
            Operators.Add("&&", Token_Class.AndOp);
            Operators.Add("|", Token_Class.Orsign);
            Operators.Add("||", Token_Class.OrOp);




        }

        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = null;

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n' )
                    continue;

                if (char.IsLetter(SourceCode[i])) //if you read a character
                {
                    while (char.IsLetterOrDigit(CurrentChar))
                    {
                        CurrentLexeme += CurrentChar;
                        j++;
                        if (j >= SourceCode.Length)
                        {
                            break;
                        }
                        CurrentChar = SourceCode[j];
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }
                else if (char.IsDigit(CurrentChar))
                {
                    while (char.IsDigit(CurrentChar) || CurrentChar == '.')
                    {
                        CurrentLexeme += CurrentChar;
                        j++;
                        if (j >= SourceCode.Length)
                        {
                            break;
                        }
                        CurrentChar = SourceCode[j];
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }
                else if (Operators.ContainsKey(SourceCode[i].ToString()))
                {
                    if (SourceCode[i] == '/' && i + 1 < SourceCode.Length && SourceCode[i + 1] == '*')
                    {
                        while (true)
                        {
                            CurrentChar = SourceCode[j];
                            CurrentLexeme += CurrentChar;
                            j++;
                            if (j >= SourceCode.Length)
                            {
                                break;
                            }
                            if (SourceCode[j] == '*' && SourceCode[j + 1] == '/')
                            {
                                CurrentLexeme += SourceCode[j];
                                CurrentLexeme += SourceCode[j + 1];
                                break;
                            }
                        }
                        FindTokenClass(CurrentLexeme);
                        i = j + 1;
                    }
                    else
                    {
                        while (Operators.ContainsKey(CurrentChar.ToString()))
                        {
                            CurrentLexeme += CurrentChar;

                            j++;
                            if (j >= SourceCode.Length)
                            {
                                break;
                            }
                            if (SourceCode[j] == '&' || SourceCode[j] == '|' || SourceCode[j] == '=' || SourceCode[j] == '>')
                                CurrentChar = SourceCode[j];
                            else
                                break;
                        }
                        FindTokenClass(CurrentLexeme);
                        i = j - 1;
                    }

                }
                else if (CurrentChar == '\"')
                {
                    while (true)
                    {
                        CurrentLexeme += CurrentChar;
                        j++;
                        if (j >= SourceCode.Length)
                        {
                            break;
                        }
                        CurrentChar = SourceCode[j];
                        
                        if (CurrentChar == '\"')
                        {
                            CurrentLexeme += CurrentChar;
                            break;
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j;
                }
                else
                {
                    Errors.Error_List.Add(CurrentChar.ToString());
                }
            }

            Tiny_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Lex))
            {
                if (Lex != "endl" )
                {
                    Tok.token_type = ReservedWords[Lex];
                    Tokens.Add(Tok);
                }
            }
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);
            }
            else if (isstring(Lex))
            {
                Tok.token_type = Token_Class.String;
                Tokens.Add(Tok);
            }
            /* else if (isArthimathic(Lex))
              {
                  Tok.token_type = Operators[Lex];
                  Tokens.Add(Tok);
              }*/
            /*else if (iscondition(Lex))
              {
                  Tok.token_type = Operators[Lex];
                  Tokens.Add(Tok);
              }*/
            /* else if (isboolean(Lex))
             {
                 Tok.token_type = Operators[Lex];
                 Tokens.Add(Tok);
             }*/
            else if (iscomment(Lex))
            {
                Tok.token_type = Token_Class.Comment;
                Tokens.Add(Tok);
            }

            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }
            else if (isnumber(Lex))
            {
                Tok.token_type = Token_Class.Number;
                Tokens.Add(Tok);
            }
            else if (isfloat(Lex))
            {
                Tok.token_type = Token_Class.FloatNum;
                Tokens.Add(Tok);
            }
            else
            {
                Errors.Error_List.Add(Lex);
            }
        }

        bool isIdentifier(string lex)
        {
            bool isValid = false;
            var iden = new Regex("^[A-Za-z]([A-Za-z]|[0-9])*$");
            // Check if the lex is an identifier or not.
            if (iden.IsMatch(lex))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }

        bool isArthimathic(string lex)
        {
            bool isValid = false;
            var iden = new Regex("^[\\+|\\-|\\*|\\/]$");
            if (iden.IsMatch(lex))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }

        bool isboolean(string lex)
        {
            bool isValid = false;
            var iden = new Regex("^&& |||$");
            if (iden.IsMatch(lex))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }

        bool isstring(string lex)
        {
            bool isValid = false;
            var iden = new Regex("^\"(.|\n)*\"$");
            if (iden.IsMatch(lex))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }

        bool iscondition(string lex)
        {
            bool isValid = false;
            var iden = new Regex("^<|>|:=|<>$");
            if (iden.IsMatch(lex))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }

        bool iscomment(string lex)
        {
            bool isValid = false;
            var iden = new Regex("^/\\*(.|\n)*\\*/$");
            if (iden.IsMatch(lex))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }

        bool isnumber(string lex)
        {
            bool isValid = false;
            var iden = new Regex("^[0-9]+$");
            if (iden.IsMatch(lex))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }

        bool isfloat(string lex)
        {
            bool isValid = false;
            var iden = new Regex("^[0-9](\\.[0-9]*)?$");
            if (iden.IsMatch(lex))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }

        bool isConstant(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.
            return isValid;
        }

    }
}

