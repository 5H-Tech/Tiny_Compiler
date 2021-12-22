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
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        

        // Implement your logic here
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
            program.Children.Add(MianFunc());
            //program.Children.Add(Function());
            //program.Children.Add(MianFunc());
            //program.Children.Add(match(Token_Class.Dot));
            MessageBox.Show("Success");
            return program;
        }

        Node MianFunc()
        {
            if (InputPointer < TokenStream.Count)
            {
                Node mainFunc = new Node("Main Function");
                mainFunc.Children.Add(Datatype());
                mainFunc.Children.Add(match(Token_Class.Main));
                mainFunc.Children.Add(match(Token_Class.LParanthesis));
                mainFunc.Children.Add(match(Token_Class.RParanthesis));
                mainFunc.Children.Add(Body());
                return mainFunc;
            }
            Errors.Error_List.Add("Parsing Error: You must enter A Main Function \r\n");
            return null;
            
        }
        Node UserFunc()
        {
            Node userFunc = new Node("User Function");
            if (InputPointer+1 <TokenStream.Count)
            {
                if (Token_Class.Idenifier == TokenStream[InputPointer + 1].token_type)
                {
                    userFunc.Children.Add(Func_dec());
                    userFunc.Children.Add(Body());
                    userFunc.Children.Add(UserFunc());
                    return userFunc;
                }
                else
                {
                    return null;
                }
            }
            return userFunc;
           
            
        }
     /*   Node Function()
        {
            Node function = new Node("User Function");
            if (InputPointer + 1 < TokenStream.Count)
            {
                if ((Token_Class.Int == TokenStream[InputPointer].token_type 
                    || Token_Class.String == TokenStream[InputPointer].token_type
                    || Token_Class.Float == TokenStream[InputPointer].token_type) &&
                Token_Class.Idenifier == TokenStream[InputPointer + 1].token_type)
                {
                    
                    function.Children.Add(Function()); ;
                }
                else
                {
                    return null;
                }
                
            }
            return function;
        }
        */
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
            arglist.Children.Add(match(Token_Class.LParanthesis));
            if(InputPointer<TokenStream.Count)
            {
                if (Token_Class.Int == TokenStream[InputPointer].token_type
                || Token_Class.Float == TokenStream[InputPointer].token_type
                || Token_Class.String == TokenStream[InputPointer].token_type)
                {
                    arglist.Children.Add(Arguments());
                    arglist.Children.Add(match(Token_Class.RParanthesis));
                }
                else
                {
                    arglist.Children.Add(match(Token_Class.RParanthesis));
                }
                return arglist;
            }
            Errors.Error_List.Add("You must enter aruments \r\n");
            return null;
            
        }
        Node Arguments()
        {
            Node arguments = new Node("Arguments");
            arguments.Children.Add(Datatype());
            arguments.Children.Add(match(Token_Class.Idenifier));
            arguments.Children.Add(Arg());

            return arguments;
        }

        Node Arg()
        {
            Node arg = new Node("Arg");

            if (Token_Class.Comma == TokenStream[InputPointer].token_type)
            {
                arg.Children.Add(match(Token_Class.Comma));
                arg.Children.Add(Datatype());
                arg.Children.Add(match(Token_Class.Idenifier));
                arg.Children.Add(Arg());
                return arg;
            }

            return null;

        }

        Node Datatype()
        {
            Node datatype = new Node("Datatype");
            if (InputPointer<TokenStream.Count)
            { 
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
                return datatype;
            }
            Errors.Error_List.Add("YOU HAVE TO ENTER DATAEA TYPE  \r\n");
            return null;
           
        }

        Node Body()
        {
            Node body = new Node("Body");
            body.Children.Add(match(Token_Class.LeftPracit));
          //  body.Children.Add(Stat_Seq());
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

            if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
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

       /* Node MainFunc()
        {
            Node mainfunc = new Node("MainFunc");
            mainfunc.Children.Add(Datatype());
            //main
            mainfunc.Children.Add(Arglist());
            mainfunc.Children.Add(Body());
            return mainfunc;
        }*/

        //Node Expression()
        //{
        //    Node Ex = new Node("expression");
        //    Ex.Children.Add(Term());
        //    Ex.Children.Add(Exp());

        //    return Ex;
        //}
        //Node Term()
        //{
        //    Node term = new Node("term");

        //    term.Children.Add(Factor());
        //    term.Children.Add(Ter());
        //    return term;
        //}

        //Node Ter()
        //{
        //    Node ter = new Node("ter");
        //    if (Token_Class.MultiplyOp == TokenStream[InputPointer].token_type)
        //    {
        //        ter.Children.Add(MultiOp());
        //        ter.Children.Add(Factor());
        //        ter.Children.Add(Ter());
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //    return ter;
        //}

        //Node Exp()
        //{
        //    Node exp = new Node("exp");
        //    if (Token_Class.PlusOp == TokenStream[InputPointer].token_type
        //       || Token_Class.MinusOp == TokenStream[InputPointer].token_type)
        //    {
        //        exp.Children.Add(AddOp());
        //        exp.Children.Add(Term());
        //        exp.Children.Add(Exp());
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //    return exp;
        //}

        //Node Factor()
        //{
        //    Node factor = new Node("factor");
        //    if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
        //    {
        //        factor.Children.Add(match(Token_Class.Idenifier));
        //    }

        //    else if (Token_Class.Constant == TokenStream[InputPointer].token_type)
        //    {
        //        factor.Children.Add(match(Token_Class.Constant));
        //    }

        //    return factor;
        //}

        //Node AddOp()
        //{
        //    Node addOp = new Node("addOp");

        //    if (Token_Class.PlusOp == TokenStream[InputPointer].token_type)
        //    {
        //        addOp.Children.Add(match(Token_Class.PlusOp));
        //    }

        //    else if (Token_Class.MinusOp == TokenStream[InputPointer].token_type)
        //    {
        //        addOp.Children.Add(match(Token_Class.MinusOp));
        //    }

        //    return addOp;
        //}

        //Node MultiOp()
        //{
        //    Node multiOp = new Node("multiOp");

        //    if (Token_Class.MultiplyOp == TokenStream[InputPointer].token_type)
        //    {
        //        multiOp.Children.Add(match(Token_Class.MultiplyOp));
        //    }

        //    else if (Token_Class.DivideOp == TokenStream[InputPointer].token_type)
        //    {
        //        multiOp.Children.Add(match(Token_Class.DivideOp));
        //    }

        //    return multiOp;
        //}

        //Node ArgList()
        //{
        //    Node argList = new Node("arglist");

        //    if (Token_Class.LParanthesis == TokenStream[InputPointer].token_type)
        //    {
        //        argList.Children.Add(match(Token_Class.LParanthesis));
        //        argList.Children.Add(Arguments());
        //        argList.Children.Add(match(Token_Class.RParanthesis));
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //    return argList;
        //}

        //Node Arguments()
        //{
        //    Node arguments = new Node("arguments");
        //    arguments.Children.Add(match(Token_Class.Idenifier));
        //    arguments.Children.Add(Arg());

        //    return arguments;
        //}

        //Node Arg()
        //{
        //    Node arg = new Node("arg");

        //    if (Token_Class.Comma == TokenStream[InputPointer].token_type)
        //    {
        //        arg.Children.Add(match(Token_Class.Comma));
        //        arg.Children.Add(match(Token_Class.Idenifier));
        //        arg.Children.Add(Arg());
        //    }

        //    else
        //    {
        //        return null;
        //    }

        //    return arg;
        //}

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

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
/*
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

//            if ()
//            { 
//
//            }
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
//            else if (Token_Class.void == TokenStream[InputPointer].token_type)
//            {
//                datatype.Children.Add(match(Token_Class.Float));
//            }

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

*/
