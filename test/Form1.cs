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
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace test
{
    public partial class Form1 : Form
    {
        string atalantaDir = Directory.GetCurrentDirectory() + "\\atalanta";

        CUT cut = new CUT();
        string[] outputSeq = new string[50];
        string[] testSeqArray = new string[50];

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string seedVal = string.Empty;
            string new_seqVal = string.Empty;
            string value = string.Empty;
            string testSeq = string.Empty;
            int i = 0;

            Array.Clear(testSeqArray, 0, testSeqArray.Length);
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Please enter a seed value!");
            }
            else
            {
                seedVal = textBox1.Text;
                value = seedVal;
                testSeq = seedVal + "\n";
                testSeqArray[i++] = seedVal;

                Cursor.Current = Cursors.WaitCursor;
                while (seedVal != new_seqVal)
                {
                    new_seqVal = generateTestPattern(value);
                    value = new_seqVal;
                    if (seedVal != new_seqVal)
                    {
                        testSeq += new_seqVal + "\n";
                        if (i < 50)
                        {
                            testSeqArray[i] = new_seqVal;
                            i++;
                        }
                    }
                }
                File.WriteAllText("c200.pat", testSeq);
                Cursor.Current = Cursors.Default;
                Array.Clear(outputSeq, 0, outputSeq.Length);
                outputSeq = cut.faultFreeCUT(testSeqArray);
                //foreach(string outseq in outputSeq)
                //    Console.WriteLine(outseq);
            }

            //RC testing
            /*string RC = lfsrRC(outputSeq, 0);   //G25;
            Console.WriteLine(RC);
            RC = lfsrRC(outputSeq, 1);          //G26;
            Console.WriteLine(RC);
            RC = lfsrRC(outputSeq, 2);          //G27;
            Console.WriteLine(RC);
            RC = lfsrRC(outputSeq, 3);          //G28;
            Console.WriteLine(RC);

            //G28 sa1
            Array.Clear(outputSeq, 0, outputSeq.Length);
            outputSeq = cut.G3sa0CUT(testSeqArray);
            RC = lfsrRC(outputSeq, 0);          //G25;
            Console.WriteLine("\n" + RC);
            RC = lfsrRC(outputSeq, 1);          //G26;
            Console.WriteLine(RC);
            RC = lfsrRC(outputSeq, 2);          //G27;
            Console.WriteLine(RC);
            RC = lfsrRC(outputSeq, 3);          //G28;
            Console.WriteLine(RC); */
        }

        private string generateTestPattern(string seedVal)
        {
            //Polynomial characteristic: 1 + x^1 + x^7
            int[,] companion_matrix = new int[,] { {0,1,0,0,0,0,0 },
                                                   {0,0,1,0,0,0,0 },
                                                   {0,0,0,1,0,0,0 },
                                                   {0,0,0,0,1,0,0 },
                                                   {0,0,0,0,0,1,0 },
                                                   {0,0,0,0,0,0,1 },
                                                   {1,1,0,0,0,0,0 } };
            string new_seqVal = string.Empty;
            int r, c;   // r = row, c = column
            int XOR_value = 0;

            for (r = 0; r < 7; r++)
            {
                for (c = 0; c < 7; c++)     // multiply the row, r of companion matrix to the column, c of seed value 
                {
                    //Console.WriteLine("Companion Matrix [" + r + ", " + c + "] = " + companion_matrix[r, c]);
                    //Console.WriteLine("Seed value index " + c + " = " + new_seedVal[c]);
                    if (c == 0)
                        XOR_value = companion_matrix[r, c] * (int)char.GetNumericValue(seedVal[c]);
                    else
                        XOR_value ^= companion_matrix[r, c] * (int)char.GetNumericValue(seedVal[c]);
                }
                new_seqVal += XOR_value.ToString();
            }
            return new_seqVal;
        }

        //Polynomial characteristic: 1 + x^1 + x^3 + x^7
        private string lfsrRC(string[] poData, int outputIndex)
        {
            int[] output = new int[7];
            string rcOut = string.Empty;
            
            for(int i = 0; i < poData.Length; i++)
            {
                output[6] = output[5];
                output[5] = output[4];
                output[4] = output[3];
                output[3] = Convert.ToInt32(cut.XOR(intToBool(output[6]), intToBool(output[2])));
                output[2] = output[1];
                output[1] = Convert.ToInt32(cut.XOR(intToBool(output[6]), intToBool(output[0])));
                output[0] = Convert.ToInt32(cut.XOR(intToBool(output[6]), charToBool(poData[i][outputIndex])));
            }

            return rcOut = string.Join("", output);
        }

        private bool intToBool(int n){ return (n == 0) ? true : false; }

        private bool charToBool(char n) { return (n == '0') ? true : false; }

        private void runAtalanta()
        {
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.WorkingDirectory = atalantaDir;
            info.FileName = "cmd.exe";
            info.CreateNoWindow = true;
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;

            p.StartInfo = info;
            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("Atalanta-M -S -t test.pat -P test.rep -v c200.bench");
                }
            }
        }

        private double getFaultCoverage()
        {
            double fc = 0;
            int totalFault = 0, detectedFault = 0;

            string[] lines = File.ReadAllLines(atalantaDir + "\\test.rep", Encoding.UTF8);
            foreach (string line in lines)
            {
                if(Regex.IsMatch(line, @"\bfaults\b"))
                    totalFault = Convert.ToInt32(Regex.Match(line, @"\d+").Value);

                if (Regex.IsMatch(line, @"\bd_faults\b"))
                    detectedFault = Convert.ToInt32(Regex.Match(line, @"\d+").Value);
            }

            fc = (double)detectedFault / (double)totalFault * 100;

            return Math.Round(fc, 2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //runAtalanta();
        }
    }
}
