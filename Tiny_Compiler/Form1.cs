using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiny_Compiler
{
    public partial class Form1 : Form
    {
        public string[] rsrvdwrds = { "read", "write", "repeat", "until", "if", "elseif", "else", "then", "return", "endl", "while", "program", "main" };
        public string[] dtatyps = { "int", "float", "string" };

        public void CheckKeyword(string word, Color color, int startIndex)
        {
            if (this.richTextBox1.Text.Contains(word))
            {
                int index = -1;
                int selectStart = this.richTextBox1.SelectionStart;

                while ((index = this.richTextBox1.Text.IndexOf(word, (index + 1))) != -1)
                {
                    this.richTextBox1.Select((index + startIndex), word.Length);
                    this.richTextBox1.SelectionColor = color;
                    this.richTextBox1.Select(selectStart, 0);
                    this.richTextBox1.SelectionColor = Color.White;
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
        }
        void PrintTokens()
        {
            for (int i = 0; i < Tiny_Compiler.Tiny_Scanner.Tokens.Count; i++)   
            {
                if(Tiny_Compiler.Tiny_Scanner.Tokens.ElementAt(i).token_type==Token_Class.Int||Tiny_Compiler.Tiny_Scanner.Tokens.ElementAt(i).token_type==Token_Class.Float||Tiny_Compiler.Tiny_Scanner.Tokens.ElementAt(i).token_type==Token_Class.String)
                {
                    dataGridView1.Rows.Add(Tiny_Compiler.Tiny_Scanner.Tokens.ElementAt(i).lex, "DataType("+Tiny_Compiler.Tiny_Scanner.Tokens.ElementAt(i).token_type+")");
                }
                else
                dataGridView1.Rows.Add(Tiny_Compiler.Tiny_Scanner.Tokens.ElementAt(i).lex, Tiny_Compiler.Tiny_Scanner.Tokens.ElementAt(i).token_type);
            }
        }

        void PrintErrors()
        {
            for (int i = 0; i < Errors.Error_List.Count; i++)
            {
                textBox2.Text += Errors.Error_List[i];
                textBox2.Text += "\r\n";
            }
        }
    

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            string Code = richTextBox1.Text.ToLower();
            Tiny_Compiler.Start_Compiling(Code);
            PrintTokens();
            treeView1.Nodes.Add(Parser.PrintParseTree(Tiny_Compiler.treeroot));
            PrintErrors();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            textBox2.Text = "";
            Tiny_Compiler.TokenStream.Clear();
            dataGridView1.Rows.Clear();
            treeView1.Nodes.Clear();
            Errors.Error_List.Clear();
        }



        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < rsrvdwrds.Length; i++) this.CheckKeyword(rsrvdwrds[i], Color.FromArgb(57, 135, 214), 0);
            for (int i = 0; i < dtatyps.Length; i++) this.CheckKeyword(dtatyps[i], Color.FromArgb(69, 201, 153), 0);
        }

      
    }
}
