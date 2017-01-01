using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class CUT
    {
        public CUT() { }

        public string[] faultFreeCUT(string[] inputs)
        {
            bool[] G = new bool[29];
            string[] output = new string[inputs.Count()];
            int i = 0;

            foreach (string input in inputs)
            {
                G[1] = Convert.ToBoolean(int.Parse(input[0].ToString()));
                G[2] = Convert.ToBoolean(int.Parse(input[1].ToString()));
                G[3] = Convert.ToBoolean(int.Parse(input[2].ToString()));
                G[4] = Convert.ToBoolean(int.Parse(input[3].ToString()));
                G[5] = Convert.ToBoolean(int.Parse(input[4].ToString()));
                G[6] = Convert.ToBoolean(int.Parse(input[5].ToString()));
                G[7] = Convert.ToBoolean(int.Parse(input[6].ToString()));

                G[8] = NOR(G[1], G[2]);
                G[9] = XOR(G[3], G[4]);
                G[10] = AND(G[5], G[6]);
                G[11] = NOR(G[7], G[8]);
                G[12] = XOR(G[9], G[10]);
                G[13] = AND(G[11], G[12]);
                G[14] = NOR(G[13], G[8]);
                G[15] = XOR(G[9], G[5]);
                G[16] = AND(G[3], G[4]);
                G[17] = NOR(G[7], G[5]);
                G[18] = XOR(G[15], G[16]);
                G[19] = AND(G[6], G[4]);
                G[20] = NOR(G[17], G[19]);
                G[21] = AND(G[15], G[2]);
                G[22] = NOR(G[13], G[3]);
                G[23] = NOR(G[21], G[22]);
                G[24] = NOR(G[23], G[4]);
                G[25] = NOR(G[22], G[10]);
                G[26] = NOR(G[18], G[5]);
                G[27] = NOR(G[24], G[14]);
                G[28] = NOR(G[13], G[20]);
                output[i] += Convert.ToInt32(G[25]).ToString() +
                             Convert.ToInt32(G[26]).ToString() +
                             Convert.ToInt32(G[27]).ToString() +
                             Convert.ToInt32(G[28]).ToString();
                i++;
            }

            return output;
        }

        public string[] G28sa1CUT(string[] inputs)
        {
            bool[] G = new bool[29];
            string[] output = new string[inputs.Count()];
            int i = 0;

            foreach (string input in inputs)
            {
                G[1] = Convert.ToBoolean(int.Parse(input[0].ToString()));
                G[2] = Convert.ToBoolean(int.Parse(input[1].ToString()));
                G[3] = Convert.ToBoolean(int.Parse(input[2].ToString()));
                G[4] = Convert.ToBoolean(int.Parse(input[3].ToString()));
                G[5] = Convert.ToBoolean(int.Parse(input[4].ToString()));
                G[6] = Convert.ToBoolean(int.Parse(input[5].ToString()));
                G[7] = Convert.ToBoolean(int.Parse(input[6].ToString()));

                G[8] = NOR(G[1], G[2]);
                G[9] = XOR(G[3], G[4]);
                G[10] = AND(G[5], G[6]);
                G[11] = NOR(G[7], G[8]);
                G[12] = XOR(G[9], G[10]);
                G[13] = AND(G[11], G[12]);
                G[14] = NOR(G[13], G[8]);
                G[15] = XOR(G[9], G[5]);
                G[16] = AND(G[3], G[4]);
                G[17] = NOR(G[7], G[5]);
                G[18] = XOR(G[15], G[16]);
                G[19] = AND(G[6], G[4]);
                G[20] = NOR(G[17], G[19]);
                G[21] = AND(G[15], G[2]);
                G[22] = NOR(G[13], G[3]);
                G[23] = NOR(G[21], G[22]);
                G[24] = NOR(G[23], G[4]);
                G[25] = NOR(G[22], G[10]);
                G[26] = NOR(G[18], G[5]);
                G[27] = NOR(G[24], G[14]);
                G[28] = true;
                output[i] += Convert.ToInt32(G[25]).ToString() +
                             Convert.ToInt32(G[26]).ToString() +
                             Convert.ToInt32(G[27]).ToString() +
                             Convert.ToInt32(G[28]).ToString();
                i++;
            }

            return output;
        }

        public string[] G3sa0CUT(string[] inputs)
        {
            bool[] G = new bool[29];
            string[] output = new string[inputs.Count()];
            int i = 0;

            foreach (string input in inputs)
            {
                G[1] = Convert.ToBoolean(int.Parse(input[0].ToString()));
                G[2] = Convert.ToBoolean(int.Parse(input[1].ToString()));
                G[3] = false;
                G[4] = Convert.ToBoolean(int.Parse(input[3].ToString()));
                G[5] = Convert.ToBoolean(int.Parse(input[4].ToString()));
                G[6] = Convert.ToBoolean(int.Parse(input[5].ToString()));
                G[7] = Convert.ToBoolean(int.Parse(input[6].ToString()));

                G[8] = NOR(G[1], G[2]);
                G[9] = XOR(G[3], G[4]);
                G[10] = AND(G[5], G[6]);
                G[11] = NOR(G[7], G[8]);
                G[12] = XOR(G[9], G[10]);
                G[13] = AND(G[11], G[12]);
                G[14] = NOR(G[13], G[8]);
                G[15] = XOR(G[9], G[5]);
                G[16] = AND(G[3], G[4]);
                G[17] = NOR(G[7], G[5]);
                G[18] = XOR(G[15], G[16]);
                G[19] = AND(G[6], G[4]);
                G[20] = NOR(G[17], G[19]);
                G[21] = AND(G[15], G[2]);
                G[22] = NOR(G[13], G[3]);
                G[23] = NOR(G[21], G[22]);
                G[24] = NOR(G[23], G[4]);
                G[25] = NOR(G[22], G[10]);
                G[26] = NOR(G[18], G[5]);
                G[27] = NOR(G[24], G[14]);
                G[28] = NOR(G[13], G[20]);
                output[i] += Convert.ToInt32(G[25]).ToString() +
                             Convert.ToInt32(G[26]).ToString() +
                             Convert.ToInt32(G[27]).ToString() +
                             Convert.ToInt32(G[28]).ToString();
                i++;
            }

            return output;
        }

        public string[] G5toG10sa1CUT(string[] inputs)
        {
            bool[] G = new bool[29];
            string[] output = new string[inputs.Count()];
            int i = 0;

            foreach (string input in inputs)
            {
                G[1] = Convert.ToBoolean(int.Parse(input[0].ToString()));
                G[2] = Convert.ToBoolean(int.Parse(input[1].ToString()));
                G[3] = Convert.ToBoolean(int.Parse(input[2].ToString()));
                G[4] = Convert.ToBoolean(int.Parse(input[3].ToString()));
                G[5] = Convert.ToBoolean(int.Parse(input[4].ToString()));
                G[6] = Convert.ToBoolean(int.Parse(input[5].ToString()));
                G[7] = Convert.ToBoolean(int.Parse(input[6].ToString()));

                G[8] = NOR(G[1], G[2]);
                G[9] = XOR(G[3], G[4]);
                G[10] = AND(true, G[6]);
                G[11] = NOR(G[7], G[8]);
                G[12] = XOR(G[9], G[10]);
                G[13] = AND(G[11], G[12]);
                G[14] = NOR(G[13], G[8]);
                G[15] = XOR(G[9], G[5]);
                G[16] = AND(G[3], G[4]);
                G[17] = NOR(G[7], G[5]);
                G[18] = XOR(G[15], G[16]);
                G[19] = AND(G[6], G[4]);
                G[20] = NOR(G[17], G[19]);
                G[21] = AND(G[15], G[2]);
                G[22] = NOR(G[13], G[3]);
                G[23] = NOR(G[21], G[22]);
                G[24] = NOR(G[23], G[4]);
                G[25] = NOR(G[22], G[10]);
                G[26] = NOR(G[18], G[5]);
                G[27] = NOR(G[24], G[14]);
                G[28] = NOR(G[13], G[20]);
                output[i] += Convert.ToInt32(G[25]).ToString() +
                             Convert.ToInt32(G[26]).ToString() +
                             Convert.ToInt32(G[27]).ToString() +
                             Convert.ToInt32(G[28]).ToString();
                i++;
            }

            return output;
        }

        public bool NOR(bool in1, bool in2) { return !(in1 | in2); }

        public bool AND(bool in1, bool in2) { return in1 & in2; }

        public bool XOR(bool in1, bool in2) { return in1 ^ in2; }
    }
}
