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
            //root.Children.Add(Program());
            return root;
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

        Node ArgList()
        {
            Node argList = new Node("arglist");

            if (Token_Class.LParanthesis == TokenStream[InputPointer].token_type)
            {
                argList.Children.Add(match(Token_Class.LParanthesis));
                argList.Children.Add(Arguments());
                argList.Children.Add(match(Token_Class.RParanthesis));
            }
            else
            {
                return null;
            }
            return argList;
        }

        Node Arguments()
        {
            Node arguments = new Node("arguments");
            arguments.Children.Add(match(Token_Class.Idenifier));
            arguments.Children.Add(Arg());

            return arguments;
        }

        Node Arg()
        {
            Node arg = new Node("arg");

            if (Token_Class.Comma == TokenStream[InputPointer].token_type)
            {
                arg.Children.Add(match(Token_Class.Comma));
                arg.Children.Add(match(Token_Class.Idenifier));
                arg.Children.Add(Arg());
            }

            else
            {
                return null;
            }

            return arg;
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
