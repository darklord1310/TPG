using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace test
{
    public partial class Form1 : Form
    {
        //List<string> gen_pattern = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string seedVal = "";
            string new_seqVal = "";
            if (textBox1.Text != "")
                seedVal = textBox1.Text;
            string value = seedVal;
            string testSeq = seedVal + "\n";

            Cursor.Current = Cursors.WaitCursor;
            while (seedVal != new_seqVal)
            {
                new_seqVal = generateTestPattern(value);
                //gen_pattern.Add(new_seqVal);
                value = new_seqVal;
                if(seedVal != new_seqVal)
                    testSeq += new_seqVal + "\n";
            }
            File.WriteAllText(seedVal + ".pat", testSeq);
            Cursor.Current = Cursors.Default;
        }

        private string generateTestPattern(string seedVal)
        {
            int[,] companion_matrix = new int[,] { {0,1,0,0,0,0,0 },
                                                   {0,0,1,0,0,0,0 },
                                                   {0,0,0,1,0,0,0 },
                                                   {0,0,0,0,1,0,0 },
                                                   {0,0,0,0,0,1,0 },
                                                   {0,0,0,0,0,0,1 },
                                                   {1,1,0,0,0,0,0 } };
            string new_seqVal = "";
            int[] mul_result = new int[7];
            int r, c;   // r = row, c = column
            int XOR_value = 0;

            for (r = 0; r < 7; r++)
            {
                for (c = 0; c < 7; c++)     // multiply the row, r of companion matrix to the column, c of seed value 
                {
                    //Console.WriteLine("Companion Matrix [" + r + ", " + c + "] = " + companion_matrix[r, c]);
                    //Console.WriteLine("Seed value index " + c + " = " + new_seedVal[c]);
                    if (c == 0)
                        XOR_value = companion_matrix[r, c] * convertCharToInt(seedVal[c]);
                    else
                    {
                        if ((companion_matrix[r, c] * convertCharToInt(seedVal[c])) != XOR_value)
                            XOR_value = 1;
                        else
                            XOR_value = 0;
                    }
                }
                new_seqVal += XOR_value.ToString();
            }
            return new_seqVal;
        }

        private int convertCharToInt(char c)
        {
            if (c == '0')
                return 0;
            else
                return 1;
        }
    }
}
