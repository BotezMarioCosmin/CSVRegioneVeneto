using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fileCSV
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            textBoxCitta.ReadOnly = true;
            textBoxCap.ReadOnly = true;
            textBoxProvincia.ReadOnly = true;
            textBoxCivico.ReadOnly = true;
            textBoxVia.ReadOnly = true;
            textBoxTelefono.ReadOnly = true;
            textBoxNome.ReadOnly = true;
            textBoxTipologia.ReadOnly = true;
            textBoxEmail.ReadOnly = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBoxCerca_TextChanged(object sender, EventArgs e)
        {
            string testo = textBoxCerca.Text.ToUpper();

            if (testo != "")
            {
                var f = new FileStream(@"veneto_verona.csv", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                BinaryReader reader = new BinaryReader(f);

                f.Seek(0, SeekOrigin.Begin);
                string linetot = "";
                bool a = true;

                while (a)
                {
                    byte[] temp = reader.ReadBytes(1);
                    linetot += Encoding.ASCII.GetString(temp);


                    if (temp[0] == '\n')
                        a = false;
                }

                int numm = linetot.Length;
                int m, i = 0, j = Convert.ToInt32(f.Length / numm), pos = -1;


                do
                {
                    m = (i + j) / 2;
                    f.Seek(m * numm, SeekOrigin.Begin);

                    string temp = Encoding.ASCII.GetString(reader.ReadBytes(numm));

                    if (compare(temp.Split(';')[0], testo) == 0)
                        pos = m * numm;
                    else if (compare(temp.Split(';')[0], testo) == -1)
                        i = m + 1;
                    else
                        j = m - 1;

                } while (i <= j && pos == -1);

                if (pos != -1)
                {
                    f.Seek(pos, SeekOrigin.Begin);

                    string tempp = Encoding.ASCII.GetString(reader.ReadBytes(numm));
                    string[] fields = tempp.Split(';');

                    TextBox[] prova = new TextBox[] { textBoxCitta, textBoxProvincia, textBoxTipologia, textBoxEmail, textBoxNome, textBoxVia, textBoxCivico, textBoxCap, textBoxTelefono };
                    int[] indiciValidi = new int[] { 0, 1, 3, 6, 7, 8, 9, 10, 11, 13, 14 };

                    for (int t = 0; t < prova.Length; t++)
                        prova[t].Text = fields[indiciValidi[t]];
                }
                f.Close();
            }
            else
            {
                textBoxCitta.Text = null;
                textBoxProvincia = null;
            }


   
        }

        static int compare(string stringa1, string stringa2)
        {
            if (stringa1 == stringa2)
                return 0;

            char[] char1 = stringa1.ToCharArray();

            char[] char2 = stringa2.ToCharArray();

            int l = char1.Length;

            if (char2.Length < l)
                l = char2.Length;

            for (int i = 0; i < l; i++)
            {
                if (char1[i] < char2[i])
                    return -1;
                if (char1[i] > char2[i])
                    return 1;
            }

            return 0;
        }
    }
}
