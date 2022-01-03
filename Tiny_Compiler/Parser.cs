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
        bool sime = false;
        bool returnCall = false;
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;
        // Implement your logic here
        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(UserFunc());
            root.Children.Add(MianFunc());
            //root.Children.Add(Program());
            return root;
        }
        /*Node Program()
        {
            Node program = new Node("Program");

            program.Children.Add(UserFunc());
            program.Children.Add(MianFunc());
            //MessageBox.Show("Success");
            return program;
        }*/

        //-------------------------------------------------------------------- Function ------------------------------------------

        Node MianFunc()
        {
            
            Node mainFunc = new Node("Main-Function");
            mainFunc.Children.Add(Datatype());
            mainFunc.Children.Add(match(Token_Class.Main));
            mainFunc.Children.Add(match(Token_Class.LParanthesis));
            mainFunc.Children.Add(match(Token_Class.RParanthesis));
            returnCall = false;
            mainFunc.Children.Add(Body());
            return mainFunc;
        }
        Node UserFunc()
        {
            Node userFunc = new Node("User-Function");
            if (InputPointer + 1 < TokenStream.Count)
            {
                if (Token_Class.Idenifier == TokenStream[InputPointer + 1].token_type)
                {
                    userFunc.Children.Add(Func_dec());
                    returnCall = false;
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

        Node Func_dec()
        {
            Node func_dec = new Node("Function-Decleration");
            func_dec.Children.Add(Datatype());
            func_dec.Children.Add(match(Token_Class.Idenifier));
            func_dec.Children.Add(Arglist());
            return func_dec;
        }

        Node Arglist()
        {
            Node arglist = new Node("Arglist");


            arglist.Children.Add(match(Token_Class.LParanthesis));
            arglist.Children.Add(Arguments());
            arglist.Children.Add(match(Token_Class.RParanthesis));
            return arglist;

            //Errors.Error_List.Add("Parsing Error: YOU MUST ENTER THE ARGUMENT LIST  \r\n");
            //return null;
        }
        Node Arguments()
        {
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.RParanthesis != TokenStream[InputPointer].token_type)
                {
                    Node arguments = new Node("Arguments");
                    arguments.Children.Add(Datatype());
                    arguments.Children.Add(match(Token_Class.Idenifier));
                    arguments.Children.Add(Arg());
                    return arguments;
                }
                else
                {
                    return null;
                }
            }
            return null;
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

        Node Body()
        {
            Node func_body = new Node("Body");
            func_body.Children.Add(match(Token_Class.LeftPracit));
            func_body.Children.Add(Stat_Seq());
            if(!returnCall)
                Errors.Error_List.Add("Missing Return Statement  \r\n");
            func_body.Children.Add(match(Token_Class.RightPracit));
            return func_body;
        }




        //-----------------------------  Enf of Function ----------------------------------


        //------------------------- Body ----------------------------------

        Node Stat_Seq() 
        {
            Node stat_seq = new Node("Set-of-Statmentes");

            stat_seq.Children.Add(Statement());
            stat_seq.Children.Add(State());

            return stat_seq;
        }


        Node State()
        {
            Node state = new Node("State");
            if (InputPointer < TokenStream.Count)
            {
                if(!sime)
                {
                    if (Token_Class.Semicolon == TokenStream[InputPointer].token_type)
                    {
                        state.Children.Add(match(Token_Class.Semicolon));
                        state.Children.Add(Statement());
                        state.Children.Add(State());
                        return state;
                    }
                    else
                    {
                        if(Token_Class.Semicolon == TokenStream[InputPointer-1].token_type  || Token_Class.LeftPracit == TokenStream[InputPointer-1].token_type)
                        {
                            return null;
                        }
                        else
                        {
                            Errors.Error_List.Add("Parsing Error: Missing Simecolon  \r\n");
                            return null;
                        }

                    }
                }
                else
                {
                    sime = false;
                    if (Token_Class.Semicolon == TokenStream[InputPointer ].token_type)
                    {
                        Errors.Error_List.Add("Parsing Error: Expected statement and found Semicolon  \r\n");
                        return null;
                    }
                    state.Children.Add(Statement());
                    state.Children.Add(State());
                    return state;
                }
            }
           

            return null;
        }

        Node Statement()
        {
            Node statement = new Node("Statement");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.If == TokenStream[InputPointer].token_type)
                {
                    statement.Children.Add(If_statement());
                    sime = true;
                }
                else if (Token_Class.Repeat == TokenStream[InputPointer].token_type)
                {
                    statement.Children.Add(RepeatStatement());
                    sime = true;
                }
                else if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
                {
                    statement.Children.Add(Fun_or_identfier());
                }
                else if (Token_Class.Read == TokenStream[InputPointer].token_type)
                {
                    statement.Children.Add(ReadStatement());
                }
                else if (Token_Class.Write == TokenStream[InputPointer].token_type)
                {
                    statement.Children.Add(WriteStatement());
                }
                else if (Token_Class.Return == TokenStream[InputPointer].token_type)
                {
                    statement.Children.Add(Return_statement());
                    returnCall = true;
                }

                else if (Token_Class.Int == TokenStream[InputPointer].token_type
                    || Token_Class.String == TokenStream[InputPointer].token_type
                    || Token_Class.Float == TokenStream[InputPointer].token_type)
                {
                    statement.Children.Add(DeclStatement());
                }
                return statement;
            }
            return null;
        }

       
        //----------------------- End of Body -----------------------------

        //----------------------- Statements ------------------------------

        Node Fun_or_identfier()
        {
            string s = "Function call or Assign Statement";

            Node fun_or_id = new Node(s);
            fun_or_id.Children.Add(match(Token_Class.Idenifier));
            if (Token_Class.LParanthesis != TokenStream[InputPointer].token_type)
            {
                fun_or_id.Children.Add(AssignStatement());
                return fun_or_id;
            }
            else if (Token_Class.LParanthesis == TokenStream[InputPointer].token_type)
            {
                fun_or_id.Children.Add(FunCall());
                return fun_or_id;
            }
            else
                return null;
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
                ElseClause.Children.Add(ElseIf_statement());
            }
            else
            {
                ElseClause.Children.Add(match(Token_Class.END));
            }


            return ElseClause;
        }

        Node Condition()
        {
            Node condition = new Node("Condition");

            condition.Children.Add(Expression());
            condition.Children.Add(RelOp());
            condition.Children.Add(Expression());
            condition.Children.Add(ConditionClosuer());

            return condition;
        }
        Node ConditionClosuer()
        {
            Node conditionClosuer = new Node("Condition-Closuer");
            if (Token_Class.AndOp == TokenStream[InputPointer].token_type
                || Token_Class.OrOp == TokenStream[InputPointer].token_type)
            {
                conditionClosuer.Children.Add(ConditionOps());
                conditionClosuer.Children.Add(Condition());
                return conditionClosuer;
            }
            else
            {
                return null;
            }           
        }

        Node ConditionOps()
        {
            Node conditionOps = new Node("Condition-Oprators");
            if (Token_Class.AndOp == TokenStream[InputPointer].token_type)
            {
                conditionOps.Children.Add(match(Token_Class.AndOp));
            }
            else
            {
                conditionOps.Children.Add(match(Token_Class.OrOp));
            }
            return conditionOps;
        }
        Node Factor()
        {
            Node factor = new Node("Factor");
            if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
            {
                factor.Children.Add(match(Token_Class.Idenifier));
                if (Token_Class.LParanthesis == TokenStream[InputPointer].token_type)
                {
                    factor.Children.Add(FunCall());
                }
            }

            else if (Token_Class.Number == TokenStream[InputPointer].token_type)
            {
                factor.Children.Add(match(Token_Class.Number));
            }
            else if (Token_Class.FloatNum == TokenStream[InputPointer].token_type)
            {
                factor.Children.Add(match(Token_Class.FloatNum));
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
            Node RepeatStatement = new Node("Repeat-Statement");
            RepeatStatement.Children.Add(match(Token_Class.Repeat));
            RepeatStatement.Children.Add(Stat_Seq());
            RepeatStatement.Children.Add(match(Token_Class.Until));
            RepeatStatement.Children.Add(Condition());
            return RepeatStatement;
        }

        Node AssignStatement()
        {
            Node AssignStatement = new Node("Assign-Statement");
            //AssignStatement.Children.Add(match(Token_Class.Idenifier));
            AssignStatement.Children.Add(match(Token_Class.ASSign));
            AssignStatement.Children.Add(Expression());
            return AssignStatement;
        }

        Node ReadStatement()
        {
            Node ReadStatement = new Node("Read-Statement");

            ReadStatement.Children.Add(match(Token_Class.Read));
            ReadStatement.Children.Add(match(Token_Class.Idenifier));

            return ReadStatement;
        }

        Node WriteStatement()
        {
            Node WriteStatement = new Node("Write-Statement");

            WriteStatement.Children.Add(match(Token_Class.Write));
            WriteStatement.Children.Add(Expression());

            return WriteStatement;
        }

        Node DeclStatement()
        {
            Node DeclStatement = new Node("Decleration-Statement");

            DeclStatement.Children.Add(Datatype());
            DeclStatement.Children.Add(Id());

            return DeclStatement;
        }

        Node Id()
        {
            Node Id = new Node("Id");
            Id.Children.Add(match(Token_Class.Idenifier));
            if (Token_Class.ASSign == TokenStream[InputPointer].token_type)
            {
                Id.Children.Add(AssignStatement());

            }
            Id.Children.Add(IdClause());

            return Id;
        }

        Node IdClause()
        {
            Node IdClause = new Node("Id-Clause");

            if (Token_Class.Comma == TokenStream[InputPointer].token_type)
            {
                IdClause.Children.Add(match(Token_Class.Comma));
                IdClause.Children.Add(Id());
                return IdClause;
            }
            return null;

        }

        Node FunCall()
        {
            Node FunCall = new Node("Function-Call");
            //FunCall.Children.Add(match(Token_Class.Idenifier));
            FunCall.Children.Add(CallArgList());
            return FunCall;
        }


        Node CallArgList()
        {
            Node CallArgList = new Node("Call-ArgList");

            CallArgList.Children.Add(match(Token_Class.LParanthesis));

            if (Token_Class.Idenifier == TokenStream[InputPointer].token_type
                || Token_Class.Number == TokenStream[InputPointer].token_type
                || Token_Class.FloatNum == TokenStream[InputPointer].token_type
                || Token_Class.String == TokenStream[InputPointer].token_type)
            {
                CallArgList.Children.Add(Expression());
                CallArgList.Children.Add(ArgumentsCall());
            }

            CallArgList.Children.Add(match(Token_Class.RParanthesis));
            return CallArgList;



        }

        Node ArgumentsCall()
        {
            Node ArgumentsCall = new Node("Arguments-Call");

            if (Token_Class.Comma == TokenStream[InputPointer].token_type)
            {
                ArgumentsCall.Children.Add(match(Token_Class.Comma));
                ArgumentsCall.Children.Add(Expression());
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
            Node Return_statement = new Node("Return-statement");

            if (Token_Class.Return == TokenStream[InputPointer].token_type)
            {
                Return_statement.Children.Add(match(Token_Class.Return));
                Return_statement.Children.Add(Expression());
                //Return_statement.Children.Add(match(Token_Class.Semicolon));
                return Return_statement;
            }

            return null;
        }

        Node Expression()
        {
            Node Ex = new Node("Expression");
            //Ex.Children.Add(Term());
            if (Token_Class.String == TokenStream[InputPointer].token_type)
            {
                Ex.Children.Add(match(Token_Class.String));
            }
            else
            {
                Ex.Children.Add(Exp());
            }
            return Ex;
        }

        Node Exp()
        {
            Node exp = new Node("Exp");
            exp.Children.Add(Term());
            exp.Children.Add(E());
            return exp;
        }
        Node E()
        {
            Node e = new Node("E");

            if (Token_Class.PlusOp == TokenStream[InputPointer].token_type
                || Token_Class.MinusOp == TokenStream[InputPointer].token_type)
            {
                e.Children.Add(Equ());
                return e;
            }
            return null;

        }

        Node Equation()
        {
            Node Equation = new Node("Equation");
            Equation.Children.Add(Term());
            Equation.Children.Add(Equ());

            return Equation;
        }

        Node Equ()
        {
            if (Token_Class.PlusOp == TokenStream[InputPointer].token_type || Token_Class.MinusOp == TokenStream[InputPointer].token_type)
            {
                Node Equa = new Node("Equa");
                Equa.Children.Add(AddOp());
                Equa.Children.Add(Term());
                Equa.Children.Add(Equ());
                return Equa;
            }
            return null;
        }


        Node Term()
        {
            Node term = new Node("Term");

            term.Children.Add(Factor());
            term.Children.Add(Ter());
            return term;
        }

        Node Ter()
        {
            Node ter = new Node("Ter");
            if (Token_Class.MultiplyOp == TokenStream[InputPointer].token_type ||
                Token_Class.DivideOp == TokenStream[InputPointer].token_type)
            {
                ter.Children.Add(MultiOp());
                ter.Children.Add(Factor());
                ter.Children.Add(Ter());
                return ter;
            }
            else
            {
                return null;
            }

        }

        Node Datatype()
        {
            Node datatype = new Node("Datatype");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Int == TokenStream[InputPointer].token_type)
                {
                    datatype.Children.Add(match(Token_Class.Int));
                    return datatype;
                }
                else if (Token_Class.Float == TokenStream[InputPointer].token_type)
                {
                    datatype.Children.Add(match(Token_Class.Float));
                    return datatype;
                }
                else if (Token_Class.String == TokenStream[InputPointer].token_type)
                {
                    datatype.Children.Add(match(Token_Class.String));
                    return datatype;
                }

                Errors.Error_List.Add("Parsing Error: YOU HAVE TO ENTER DATAEA TYPE  \r\n");
                return null;
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: YOU HAVE TO ENTER DATAEA TYPE  \r\n");
                return null;
            }


        }

        //---------------------------------------------------------------------------------------------------------------
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
                    Errors.Error_List.Add("Parsing Error: Expected " + ExpectedToken.ToString() + " and " + TokenStream[InputPointer].token_type.ToString() + "  found\r\n");
                    Console.WriteLine("Parsing Error: Expected " + ExpectedToken.ToString() + " and " + TokenStream[InputPointer].token_type.ToString() + "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected " + ExpectedToken.ToString() + "\r\n");
                Console.WriteLine("Parsing Error: Expected " + ExpectedToken.ToString() + "\r\n");
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

