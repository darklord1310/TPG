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

            if (textBox1.Text != "")
                seedVal = textBox1.Text;

            //generateFullTestPattern(seedVal,false);
            exhaustiveTestGeneration();
        }

        private void generateFullTestPattern(string seedVal, bool limit50)
        {
            string new_seqVal = "";
            string value = seedVal;
            string testSeq = seedVal + "\n";
            int i = 0;

            if(limit50 == true)
            {
                value = seedVal;
                new_seqVal = "";
                Cursor.Current = Cursors.WaitCursor;

                while (seedVal != new_seqVal)
                {
                    new_seqVal = generateSingleTestPattern(value);
                    value = new_seqVal;
                    if (seedVal != new_seqVal)
                        testSeq += new_seqVal + "\n";
                    i++;
                    if (i == 49)
                        break;
                }
                File.WriteAllText(seedVal + ".pat", testSeq);
            }
            else
            {
                value = seedVal;
                new_seqVal = "";
                Cursor.Current = Cursors.WaitCursor;

                while (seedVal != new_seqVal)
                {
                    new_seqVal = generateSingleTestPattern(value);
                    value = new_seqVal;
                    if (seedVal != new_seqVal)
                        testSeq += new_seqVal + "\n";
                }
                File.WriteAllText(seedVal + ".pat", testSeq);
            }
            Cursor.Current = Cursors.Default;
        }


        private void exhaustiveTestGeneration()
        {
            string new_seqVal = "";
            string value,seedVal;
            string testSeq = "";
            int i = 0;

            for (int j = 1; j < 128; j++)
            {
                seedVal = ToBin(j, 7);
                value = seedVal;
                new_seqVal = "";
                testSeq = seedVal + "\n";
                Cursor.Current = Cursors.WaitCursor;
                i = 0;
                while (seedVal != new_seqVal)
                {
                    new_seqVal = generateSingleTestPattern(value);
                    value = new_seqVal;
                    if (seedVal != new_seqVal)
                        testSeq += new_seqVal + "\n";
                    i++;
                    if (i == 49)
                        break;
                }
                File.WriteAllText(seedVal + ".pat", testSeq);
            }
            Cursor.Current = Cursors.Default;
        }


        private string generateSingleTestPattern(string seedVal)
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

        private string searchASeed(int noOfPI, int targetFaultCoverage)
        {
            int BestCost = 50;  // since 50 test vectors is all we need
            double P_findBetterSolution = 1;
            double p = 0.0;
            double temperature = 30000;
            string seed, BestSeed = "";
            int cost, deltaCost;
            List<string> repeatedSeed = new List<string>();
            Random rnd = new Random();
            while(P_findBetterSolution > p)
            {
                seed = new_seed(noOfPI);
                while(repeatedSeed.Exists(element => element == seed))
                {
                    seed = new_seed(noOfPI);
                }
                repeatedSeed.Add(seed);
                cost = return_patterns(seed,targetFaultCoverage);
                deltaCost = cost - BestCost;
                if(deltaCost <= 0)
                {
                    BestCost = cost;
                    BestSeed = seed;
                }
                else
                {
                    p = rnd.NextDouble();
                    P_findBetterSolution = Math.Exp(-deltaCost/temperature);
                }
                temperature--;
                temperature = temperature * 0.85;
            }
            return BestSeed;
        }

        private int return_patterns(string seed, int targetFC)
        {
            int FC = 0;
            int numberOfPatterns = 0;

            /*
            while(FC < targetFC)
            {
                //FC = generateTestPattern(seed);
                numberOfPatterns++;
            }
            */
            //return numberOfPatterns;
            return 30;
        }


        private string new_seed(int no)
        {
            Random rnd = new Random();
            return ToBin(rnd.Next(1,no),7);
        }


        public static string ToBin(int value, int len)
        {
            return (len > 1 ? ToBin(value >> 1, len - 1) : null) + "01"[value & 1];
        }
    }
}
