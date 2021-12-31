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
            if (InputPointer + 1 < TokenStream.Count)
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

        Node Body()
        {
            Node func_body = new Node("Body");
            func_body.Children.Add(match(Token_Class.LeftPracit));
            func_body.Children.Add(Stat_Seq());
            func_body.Children.Add(match(Token_Class.RightPracit));
            return func_body;
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

            if (Token_Class.Semicolon == TokenStream[InputPointer].token_type)
            {
                state.Children.Add(match(Token_Class.Semicolon));
                state.Children.Add(Statement());
                state.Children.Add(State());
                return state;
            }

            return null;
        }

        Node Statement()
        {
            Node statement = new Node("Statement");




            return statement;
        }


        Node ElseClause()
        {
            Node ElseClause = new Node("ElseClause");
            if (Token_Class.Else == TokenStream[InputPointer].token_type)
            {
                ElseClause.Children.Add(match(Token_Class.Else));
                ElseClause.Children.Add(Stat_Seq());
                ElseClause.Children.Add(match(Token_Class.END));
            }
            else if (Token_Class.Elseif == TokenStream[InputPointer].token_type)
            {

            }
            else if (Token_Class.END == TokenStream[InputPointer].token_type)
            {
                ElseClause.Children.Add(match(Token_Class.END));
            }

            return ElseClause;
        }


        Node If_statement()
        {
            Node If_statement = new Node("if_statement");
            If_statement.Children.Add(match(Token_Class.If));
            If_statement.Children.Add(Condition());
            If_statement.Children.Add(match(Token_Class.Then));
            If_statement.Children.Add(Stat_Seq());
            If_statement.Children.Add(ElseClause());



            return If_statement;
        }

        Node Condition()
        {
            Node Condition = new Node("Condition");

            Condition.Children.Add(Expression());
            Condition.Children.Add(RelOp());
            Condition.Children.Add(Expression());

            return Condition;
        }

        Node ElseIf_statement()
        {
            Node ElseIf_statement = new Node("ElseIf_statement");

            ElseIf_statement.Children.Add(match(Token_Class.Elseif));
            ElseIf_statement.Children.Add(Condition());
            ElseIf_statement.Children.Add(match(Token_Class.Then));
            ElseIf_statement.Children.Add(Stat_Seq());
            ElseIf_statement.Children.Add(ElseClause());
            return ElseIf_statement;
        }


        Node Factor()
        {
            Node factor = new Node("factor");
            if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
            {
                factor.Children.Add(match(Token_Class.Idenifier));
            }

            else if (Token_Class.Constant == TokenStream[InputPointer].token_type)
            {
                factor.Children.Add(match(Token_Class.Constant));
            }

            return factor;
        }

        Node RelOp()
        {
            Node RelOp = new Node("RelOp");

            if (Token_Class.LessThanOp == TokenStream[InputPointer].token_type)
            {
                RelOp.Children.Add(match(Token_Class.LessThanOp));
            }

            else if (Token_Class.GreaterThanOp == TokenStream[InputPointer].token_type)
            {
                RelOp.Children.Add(match(Token_Class.GreaterThanOp));
            }
            else if (Token_Class.EqualOp == TokenStream[InputPointer].token_type)
            {
                RelOp.Children.Add(match(Token_Class.EqualOp));
            }
            else if (Token_Class.NotEqualOp == TokenStream[InputPointer].token_type)
            {
                RelOp.Children.Add(match(Token_Class.NotEqualOp));
            }

            return RelOp;
        }

        Node AddOp()
        {
            Node addOp = new Node("addOp");

            if (Token_Class.PlusOp == TokenStream[InputPointer].token_type)
            {
                addOp.Children.Add(match(Token_Class.PlusOp));
            }

            else if (Token_Class.MinusOp == TokenStream[InputPointer].token_type)
            {
                addOp.Children.Add(match(Token_Class.MinusOp));
            }

            return addOp;
        }

        Node MultiOp()
        {
            Node multiOp = new Node("multiOp");

            if (Token_Class.MultiplyOp == TokenStream[InputPointer].token_type)
            {
                multiOp.Children.Add(match(Token_Class.MultiplyOp));
            }

            else if (Token_Class.DivideOp == TokenStream[InputPointer].token_type)
            {
                multiOp.Children.Add(match(Token_Class.DivideOp));
            }

            return multiOp;
        }


        Node RepeatStatement()
        {
            Node RepeatStatement = new Node("RepeatStatement");
            RepeatStatement.Children.Add(match(Token_Class.Repeat));
            RepeatStatement.Children.Add(Stat_Seq());
            RepeatStatement.Children.Add(match(Token_Class.Until));
            RepeatStatement.Children.Add(Expression());
            return RepeatStatement;
        }


        Node AssignStatement()
        {
            Node AssignStatement = new Node("AssignStatement");
            AssignStatement.Children.Add(match(Token_Class.Idenifier));
            AssignStatement.Children.Add(match(Token_Class.ASSign));
            AssignStatement.Children.Add(Expression());
            return AssignStatement;
        }

        Node ReadStatement()
        {
            Node ReadStatement = new Node("ReadStatement");

            ReadStatement.Children.Add(match(Token_Class.Read));
            ReadStatement.Children.Add(match(Token_Class.Idenifier));

            return ReadStatement;
        }

        Node WriteStatement()
        {
            Node WriteStatement = new Node("WriteStatement");

            WriteStatement.Children.Add(match(Token_Class.Write));
            WriteStatement.Children.Add(Expression());

            return WriteStatement;
        }

        Node DeclStatement()
        {
            Node DeclStatement = new Node("DeclStatement");

            DeclStatement.Children.Add(Datatype());
            DeclStatement.Children.Add(match(Token_Class.Idenifier));

            return DeclStatement;
        }

        Node Id()
        {
            Node Id = new Node("Id");

            if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
            {
                Id.Children.Add(match(Token_Class.Idenifier));
            }
            else
            {

                Id.Children.Add(AssignStatement());
                Id.Children.Add(IdClause());
            }

            return Id;
        }

        Node IdClause()
        {
            Node IdClause = new Node("IdClause");

            if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
            {
                IdClause.Children.Add(Id());
                return IdClause;
            }
            return null;

        }


        Node FunCall()
        {
            Node FunCall = new Node("FunCall");
            FunCall.Children.Add(match(Token_Class.Idenifier));
            FunCall.Children.Add(CallArgList());
            return FunCall;
        }


        Node CallArgList()
        {
            Node CallArgList = new Node("CallArgList");

            CallArgList.Children.Add(match(Token_Class.LParanthesis));

            if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
            {
                CallArgList.Children.Add(ArgumentsCall());
            }

            CallArgList.Children.Add(match(Token_Class.RParanthesis));
            return CallArgList;



        }



        Node ArgumentsCall()
        {
            Node ArgumentsCall = new Node("ArgumentsCall");

            if (Token_Class.Comma == TokenStream[InputPointer].token_type)
            {

                ArgumentsCall.Children.Add(match(Token_Class.Idenifier));
                ArgumentsCall.Children.Add(ArgCall());
                return ArgumentsCall;
            }

            return null;
        }


        Node ArgCall()
        {
            Node Argcall = new Node("ArgCall");

            if (Token_Class.Comma == TokenStream[InputPointer].token_type)
            {
                Argcall.Children.Add(match(Token_Class.Comma));
                Argcall.Children.Add(match(Token_Class.Idenifier));
                Argcall.Children.Add(ArgCall());
                return Argcall;
            }

            return null;
        }


        Node Return_statement()
        {
            Node Return_statement = new Node("Return_statement");

            if (Token_Class.Return == TokenStream[InputPointer].token_type)
            {
                Return_statement.Children.Add(match(Token_Class.Return));
                Return_statement.Children.Add(Expression());
                Return_statement.Children.Add(match(Token_Class.Semicolon));
                return Return_statement;
            }

            return null;
        }




        Node Expression()
        {
            Node Ex = new Node("expression");
            Ex.Children.Add(Term());
            Ex.Children.Add(Exp());

            return Ex;
        }
        Node Term()
        {
            Node term = new Node("term");

            term.Children.Add(Factor());
            term.Children.Add(Ter());
            return term;
        }

        Node Ter()
        {
            Node ter = new Node("ter");
            if (Token_Class.MultiplyOp == TokenStream[InputPointer].token_type)
            {
                ter.Children.Add(MultiOp());
                ter.Children.Add(Factor());
                ter.Children.Add(Ter());
            }
            else
            {
                return null;
            }

            return ter;
        }

        Node Exp()
        {
            Node exp = new Node("exp");
            if (Token_Class.PlusOp == TokenStream[InputPointer].token_type
               || Token_Class.MinusOp == TokenStream[InputPointer].token_type)
            {
                exp.Children.Add(AddOp());
                exp.Children.Add(Term());
                exp.Children.Add(Exp());
            }
            else
            {
                return null;
            }

            return exp;
        }




        Node Arglist()
        {
            Node arglist = new Node("Arglist");
            arglist.Children.Add(match(Token_Class.LParanthesis));
            if (InputPointer < TokenStream.Count)
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
            if (InputPointer < TokenStream.Count)
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

