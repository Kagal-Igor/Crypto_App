using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CryptoBase;

namespace Crypto_App
{
    public partial class Main_window : Form
    {
        RC5 rc5;
        byte[] key_RC5 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        string text_RC5, cyptext_RC5, decyptext_RC5;

        Elgamal elgamal;
        byte[] key_Elgamal = { 251, 6, 148 };
        string text_Elgamal, cyptext_Elgamal, decyptext_Elgamal;

        MD5 md5;
        string text_MD5, myHash, hash;

        SignatureRSA rsa;
        string text_RSA, sign;
        MemoryStream outputRSA;

        public Main_window()
        {
            InitializeComponent();
        }

        private void label12_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button_c_RC5_Click(object sender, EventArgs e)
        {
            text_RC5 = cyptext_RC5 = decyptext_RC5 = "";
            listBox_RC5.Items.Clear();

            MemoryStream skey_RC5 = new MemoryStream(key_RC5);
            rc5 = new RC5(skey_RC5);

            text_RC5 = this.textBox_RC5.Text;

            MemoryStream encryptedDataStream = new MemoryStream();
            MemoryStream sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(text_RC5));
            rc5.Encrypt(sourceDataStream, encryptedDataStream);//шифруем
            cyptext_RC5 = Encoding.Default.GetString(encryptedDataStream.ToArray()).TrimEnd('\0');


            string encryptedText = Encoding.Default.GetString(encryptedDataStream.ToArray()).TrimEnd('\0');
            byte[] byteArray = Encoding.Default.GetBytes(encryptedText);
            var hexString = BitConverter.ToString(byteArray);
            hexString = hexString.Replace("-", " ");

            listBox_RC5.Items.Add(hexString);
           // listBox_RC5.Items.Add(cyptext_RC5);
        }

        private void button_d_RC5_Click(object sender, EventArgs e)
        {
            listBox_RC5_d.Items.Clear();

            MemoryStream decryptedDataStream = new MemoryStream();
            if (cyptext_RC5 == null) return;
            MemoryStream encryptedDataStream = new MemoryStream(Encoding.Default.GetBytes(cyptext_RC5));

            rc5.Decrypt(encryptedDataStream, decryptedDataStream);//дешифруем
            decyptext_RC5 = Encoding.Default.GetString(decryptedDataStream.ToArray()).TrimEnd('\0');

            listBox_RC5_d.Items.Add(decyptext_RC5);
            
        }

        private void button_c_Elgamal_Click(object sender, EventArgs e)
        {
            text_Elgamal = cyptext_Elgamal = decyptext_Elgamal = "";
            listBox_Elgamal.Items.Clear();

            MemoryStream skey = new MemoryStream(key_Elgamal);
            text_Elgamal = this.textBox_Elgamal.Text;
            elgamal = new Elgamal(skey);

            MemoryStream sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(text_Elgamal));
            MemoryStream encryptedDataStream = new MemoryStream();
            elgamal.Encrypt(sourceDataStream, encryptedDataStream);
            cyptext_Elgamal = Encoding.Default.GetString(encryptedDataStream.ToArray()).TrimEnd('\0');

            string encryptedText = Encoding.Default.GetString(encryptedDataStream.ToArray()).TrimEnd('\0');
            byte[] byteArray = Encoding.Default.GetBytes(encryptedText);
            var hexString = BitConverter.ToString(byteArray);
            hexString = hexString.Replace("-", " ");


            listBox_Elgamal.Items.Add(hexString);
           // listBox_Elgamal.Items.Add(cyptext_Elgamal);
        }

        private void button_d_Elgamal_Click(object sender, EventArgs e)
        {
            listBox_Elgamal_d.Items.Clear();

            MemoryStream decryptedDataStream = new MemoryStream();
            if (cyptext_Elgamal == null) return;
            MemoryStream encryptedDataStream = new MemoryStream(Encoding.Default.GetBytes(cyptext_Elgamal));

            elgamal.Decrypt(encryptedDataStream, decryptedDataStream);//дешифруем
            decyptext_Elgamal = Encoding.Default.GetString(decryptedDataStream.ToArray()).TrimEnd('\0');

            listBox_Elgamal_d.Items.Add(decyptext_Elgamal);
        }

        private void button_c_MD5_Click(object sender, EventArgs e)
        {
            text_MD5 = myHash = hash = "";
            listBox_MD5.Items.Clear();

            text_MD5 = this.textBox_MD5.Text;

            md5 = new MD5();
            MemoryStream input = new MemoryStream(Encoding.Default.GetBytes(text_MD5));
            MemoryStream output = new MemoryStream();

            md5.GetHash(input, output);
            myHash = Encoding.Default.GetString(output.ToArray()).TrimEnd('\0');


            string encryptedText = Encoding.Default.GetString(output.ToArray()).TrimEnd('\0');
            byte[] byteArray = Encoding.Default.GetBytes(encryptedText);
            var hexString = BitConverter.ToString(byteArray);
            hexString = hexString.Replace("-", " ");

            listBox_MD5.Items.Add(hexString);
            //listBox_MD5.Items.Add(myHash);
        }

        private void button_d_MD5_Click(object sender, EventArgs e)
        {
            listBox_MD5_d.Items.Clear();

            System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create();

            if (text_MD5 == null) return;
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(text_MD5));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            hash = sBuilder.ToString();

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (comparer.Compare(myHash, hash) == 0) listBox_MD5_d.Items.Add("Хэш верен");
            else listBox_MD5_d.Items.Add("Хэш НЕверен");
        }

        private void button_c_RSA_Click(object sender, EventArgs e)
        {
            listBox_RSA.Items.Clear();

            text_RSA = this.textBox_RSA.Text;
            MemoryStream input = new MemoryStream(Encoding.Default.GetBytes(text_RSA));
            outputRSA = new MemoryStream();

            rsa = new SignatureRSA();
            rsa.RSA_Params();
            rsa.SetHashFunction(new MD5());
            rsa.Sign(input, outputRSA);

            sign = Encoding.Default.GetString(outputRSA.ToArray()).TrimEnd('\0');


            string encryptedText = Encoding.Default.GetString(outputRSA.ToArray()).TrimEnd('\0');
            byte[] byteArray = Encoding.Default.GetBytes(encryptedText);
            var hexString = BitConverter.ToString(byteArray);
            hexString = hexString.Replace("-", " ");

            listBox_RSA.Items.Add(hexString);
          //  listBox_RSA.Items.Add(sign);
        }

        private void button_d_RSA_Click(object sender, EventArgs e)
        {
            listBox_RSA_d.Items.Clear();

            if (outputRSA == null) return;
            if (rsa.Verify(outputRSA) == true) listBox_RSA_d.Items.Add("Подпись верна");
            else listBox_RSA_d.Items.Add("Подпись НЕверна");
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }
    }
}
