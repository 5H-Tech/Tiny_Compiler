using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiny_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }

    class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }

        Node Program()
        {
            Node program = new Node("Program");
            program.Children.Add(UserFunc());
            program.Children.Add(MainFunc());
            MessageBox.Show("Success");
            return program;
        }

        Node UserFunc()
        {
            Node userfunc = new Node("UserFunc");
            /*
            if ()
            { 

            }
            */
            return userfunc;
        }

        Node Function()
        {
            Node function = new Node("Function");
            function.Children.Add(Func_dec());
            function.Children.Add(Body());

            return function;
        }

        Node Func_dec()
        {
            Node func_dec = new Node("Func_dec");
            func_dec.Children.Add(Datatype());
            func_dec.Children.Add(match(Token_Class.Idenifier));
            func_dec.Children.Add(Arglist());

            return func_dec;
        }
        Node Arglist()
        {
            Node arglist = new Node("Arglist");
            if (Token_Class.LParanthesis == TokenStream[InputPointer].token_type)
            {
                arglist.Children.Add(match(Token_Class.LParanthesis));
                arglist.Children.Add(Arguments());
                arglist.Children.Add(match(Token_Class.RParanthesis));
            }
            else
            {
                arglist.Children.Add(match(Token_Class.LParanthesis));
                arglist.Children.Add(match(Token_Class.RParanthesis));
            }

            return arglist;
        }
        Node Arguments()
        {
            Node arguments = new Node("Arguments");
            arguments.Children.Add(match(Token_Class.Idenifier));
            arguments.Children.Add(Arg());

            return arguments;
        }

        Node Arg()
        {
            Node arg = new Node("Arg");

            if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
            {
                arg.Children.Add(match(Token_Class.Comma));
                arg.Children.Add(match(Token_Class.Idenifier));
                arg.Children.Add(Arg());
                return arg;
            }

            return null;

        }

        Node Datatype()
        {
            Node datatype = new Node("Datatype");
            if (Token_Class.Int == TokenStream[InputPointer].token_type)
            {
                datatype.Children.Add(match(Token_Class.Int));
            }
            else if (Token_Class.Float == TokenStream[InputPointer].token_type)
            {
                datatype.Children.Add(match(Token_Class.Float));
            }
            else if (Token_Class.String == TokenStream[InputPointer].token_type)
            {
                datatype.Children.Add(match(Token_Class.String));
            }
 /*
            else if (Token_Class.void == TokenStream[InputPointer].token_type)
            {
                datatype.Children.Add(match(Token_Class.Float));
            }
 */           
            return datatype;
        }

        Node Body()
        {
            Node body = new Node("Body");
            body.Children.Add(match(Token_Class.LeftPracit));
            body.Children.Add(Stat_Seq());
            body.Children.Add(match(Token_Class.RightPracit));

            return body;
        }

        Node Stat_Seq()
        {
            Node stat_seq = new Node("Stat_Seq");

            stat_seq.Children.Add(Statement());
            stat_seq.Children.Add(State());

            return stat_seq;
        }

        Node State()
        {
            Node state = new Node("State");

            if(Token_Class.Idenifier == TokenStream[InputPointer].token_type)
            {
                state.Children.Add(match(Token_Class.Idenifier));
                state.Children.Add(Statement());
                state.Children.Add(State());
                return state;
            }

            return null;
        }

        Node Statement()
        {
            Node statement = new Node("Statement");

            statement.Children.Add(Statement());

            return statement;
        }

        Node MainFunc()
        {
            Node mainfunc = new Node("MainFunc");
            mainfunc.Children.Add(Datatype());
            //main
            mainfunc.Children.Add(Arglist());
            mainfunc.Children.Add(Body());
            return mainfunc;
        }


        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }
    }
}
