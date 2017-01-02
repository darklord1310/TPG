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
        CUT cut = new CUT();
        List<string> outputSeq = new List<string>();
        List<string> testSeqArray = new List<string>();
        List<string> initialSeed = new List<string>();
        string atalantaDir = Directory.GetCurrentDirectory() + "\\atalanta";

        public delegate List<string> generateOutputSeq(List<string> inputs);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string seedVal = string.Empty;

            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Please enter a seed value!");
            }
            else
            {
                seedVal = textBox1.Text;
                generateFullTestPattern(seedVal, 50);
                //exhaustiveTestGeneration();
                generateRCSignature(cut.faultFreeCUT);
                generateRCSignature(cut.G3sa0CUT);
            }
        }

        private static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        private void generateFullTestPattern(string seedVal, int totalTestPattern)
        {
            testSeqArray.TrimExcess();
            testSeqArray.Clear();
            string new_seqVal = string.Empty;
            string value = seedVal;
            string testSeq = seedVal + "\n";
            int i = 1;

            testSeqArray.Add(value);
            Cursor.Current = Cursors.WaitCursor;
            while (seedVal != new_seqVal)
            {
                new_seqVal = generateSingleTestPattern(value);
                value = new_seqVal;
                if (seedVal != new_seqVal)
                {
                    testSeq += new_seqVal + "\n";
                    if(i < totalTestPattern)
                        testSeqArray.Add(new_seqVal);
                }
                i++;
                if (i == totalTestPattern)
                    break;
            }
            File.WriteAllText(atalantaDir + "\\" + seedVal + ".pat", testSeq);
            Cursor.Current = Cursors.Default;         
        }

        private void exhaustiveTestGeneration()
        {
            string new_seqVal = string.Empty;
            string value, seedVal;
            string testSeq = string.Empty;
            int i = 0;

            for (int j = 1; j < 128; j++)
            {
                seedVal = ToBin(j, 7);
                value = seedVal;
                new_seqVal = string.Empty;
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
            //Polynomial characteristic: 1 + x^1 + x^7
            int[,] companion_matrix = new int[,] { {0,1,0,0,0,0,0 },
                                                   {0,0,1,0,0,0,0 },
                                                   {0,0,0,1,0,0,0 },
                                                   {0,0,0,0,1,0,0 },
                                                   {0,0,0,0,0,1,0 },
                                                   {0,0,0,0,0,0,1 },
                                                   {1,1,0,0,0,0,0 } };
            /*
            int[,] companion_matrix = new int[,] { {0,0,0,0,0,0,1 },
                                                   {1,0,0,0,0,0,1 },
                                                   {0,1,0,0,0,0,0 },
                                                   {0,0,1,0,0,0,0 },
                                                   {0,0,0,1,0,0,0 },
                                                   {0,0,0,0,1,0,0 },
                                                   {0,0,0,0,0,1,0 } }; // JH's test matrix
            */
            string new_seqVal = string.Empty;
            
            int r, c;   // r = row, c = column
            int XOR_value = 0;

            for (r = 0; r < 7; r++)
            {
                for (c = 0; c < 7; c++)     // multiply the row, r of companion matrix to the column, c of seed value 
                {
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
        private string lfsrRC(List<string> poData, int outputIndex)
        {
            int[] output = new int[7];
            string rcOut = string.Empty;
            
            for(int i = 0; i < poData.Count(); i++)
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

        private void generateRCSignature(generateOutputSeq f)
        {
            outputSeq.TrimExcess();
            outputSeq.Clear();

            //Fault free signature
            outputSeq = f(testSeqArray);     //Get output sequence from CUT
            //foreach(string outseq in outputSeq)
            //    Console.WriteLine(outseq);

            string RC = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                RC = lfsrRC(outputSeq, i);
                Console.Write(RC + " ");
            }

            Console.WriteLine();
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
            //Console.WriteLine("Runnning atalanta...");    //debug purpose
            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("Atalanta-M -S -t test.pat -P test.rep -v c200.bench");
                }
            }

            p.WaitForExit();
            p.Close();
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

        private string searchASeed(int noOfPI, int targetFaultCoverage)
        {
            int BestCost = 50;  // since 50 test vectors is all we need
            double P_findBetterSolution = 1;
            double p = 0.0;
            double temperature = 30000;
            string seed, BestSeed = string.Empty;
            int cost, deltaCost;
            List<string> repeatedSeed = new List<string>();
            Random rnd = new Random();

            Console.WriteLine("\n****Start search best seed****");
            repeatedSeed = new List<string>(initialSeed);           //clone the list to not include the best seed
            while (P_findBetterSolution > p)
            {
                seed = new_seed(noOfPI);
                if (repeatedSeed.Count() == 127) break;
                while(repeatedSeed.Exists(element => element == seed))
                {
                    seed = new_seed(noOfPI);
                }
                Console.WriteLine("generated new seed = {0}", seed);    //debug purpose
                repeatedSeed.Add(seed);
                cost = return_patterns(seed,targetFaultCoverage);
                Console.WriteLine("cost = {0}", cost.ToString());       //debug purpose
                deltaCost = cost - BestCost;
                if(deltaCost <= 0)
                {
                    BestCost = cost;
                    BestSeed = seed;
                    Console.WriteLine("Best cost = {0}", BestCost.ToString());   //debug purpose
                }
                else
                {
                    p = rnd.NextDouble();
                    P_findBetterSolution = Math.Exp(-deltaCost/temperature);
                    Console.WriteLine("P_findBetterSolution = {0}", P_findBetterSolution.ToString());   //debug purpose
                }
                temperature-=30;
                if (temperature <= 0) break;
                temperature = temperature * 0.85;
            }
            return BestSeed;
        }

        private int return_patterns(string seed, int targetFC)
        {
            int FC = 0;
            int numberOfPatterns = 0;
            string nextPattern = seed;

            if (new FileInfo(atalantaDir + "\\test.pat").Length != 0)
            {
                File.WriteAllText(atalantaDir + "\\test.pat", string.Empty);
            }

            do
            {
                numberOfPatterns++;
                using (StreamWriter file = new StreamWriter(atalantaDir + "\\test.pat", true))
                {
                    file.WriteLine(nextPattern);
                }
                runAtalanta();
                System.Threading.Thread.Sleep(300);         //To avoid access unfinish process
                FC = (int)getFaultCoverage();
                nextPattern = generateSingleTestPattern(nextPattern);
            } while (FC < targetFC);

            return numberOfPatterns;
        }

        private string new_seed(int no)
        {
            Random rnd = new Random();
            return ToBin(rnd.Next(1, (int)Math.Pow(2, no)),7);
        }

        public static string ToBin(int value, int len)
        {
            return (len > 1 ? ToBin(value >> 1, len - 1) : null) + "01"[value & 1];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < 10; i++)
            //    Console.WriteLine(return_patterns("0000001", 90).ToString());
            for (int i = 0; i < 10; i++)
                initialSeed.Add(searchASeed(7, 100));

            Console.WriteLine("Best seed\n");
            for (int i = 0; i < initialSeed.Count(); i++)
            {
                //Console.WriteLine(initialSeed[i]);
                using (StreamWriter file = new StreamWriter("bestSeed.txt", true))
                {
                    file.WriteLine(initialSeed[i]);
                }
            }
            //Console.WriteLine("Done");
        }
    }
}
