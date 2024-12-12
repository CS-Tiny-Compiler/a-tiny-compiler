﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Tiny_Compiler;

namespace Tiny_Compiler
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) // start compile
        {
            // clear the form
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
           // textBox1.Clear();
            Tiny_Compiler.TokenStream.Clear();

            //string Code=textBox1.Text.ToLower();
            string Code = textBox1.Text;
            Tiny_Compiler.Start_Compiling(Code);
            PrintTokens();
            PrintErrors();

        }

        void PrintTokens()
        {
            for (int i = 0; i < Tiny_Compiler.Tiny_Scanner.Tokens.Count; i++)
            {
                dataGridView1.Rows.Add(Tiny_Compiler.Tiny_Scanner.Tokens.ElementAt(i).lex, Tiny_Compiler.Tiny_Scanner.Tokens.ElementAt(i).token_type);
            }
        }
        void PrintErrors()
        {
            for (int i = 0; i < Errors.Error_List.Count; i++)
            {
                listBox1.Items.Add(Errors.Error_List[i]);
                //listBox1.Text += "\r\n";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
