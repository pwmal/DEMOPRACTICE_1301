using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Reflection.Emit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.Http;
using ZXing.Common;
using ZXing;
using System.Collections;

namespace DEMOPRACTICE_1301
{
    public partial class Form1 : Form
    {
        Hashtable codeStructure = new Hashtable();
        Hashtable numCodeL = new Hashtable();
        Hashtable numCodeR = new Hashtable();
        Hashtable numCodeG = new Hashtable();


        public Form1()
        {
            InitializeComponent();
            codeStructure.Add("0", "LLLLLLRRRRRR");
            codeStructure.Add("1", "LLGLGGRRRRRR");
            codeStructure.Add("2", "LLGGLGRRRRRR");
            codeStructure.Add("3", "LLGGGLRRRRRR");
            codeStructure.Add("4", "LGLLGGRRRRRR");
            codeStructure.Add("5", "LGGLLGRRRRRR");
            codeStructure.Add("6", "LGGGLLRRRRRR");
            codeStructure.Add("7", "LGLGLGRRRRRR");
            codeStructure.Add("8", "LGLGGLRRRRRR");
            codeStructure.Add("9", "LGGLGLRRRRRR");

            numCodeL.Add("0", "0001101");
            numCodeL.Add("1", "0011001");
            numCodeL.Add("2", "0010011");
            numCodeL.Add("3", "0111101");
            numCodeL.Add("4", "0100011");
            numCodeL.Add("5", "0110001");
            numCodeL.Add("6", "0101111");
            numCodeL.Add("7", "0111011");
            numCodeL.Add("8", "0110111");
            numCodeL.Add("9", "0001011");

            numCodeR.Add("0", "1110010");
            numCodeR.Add("1", "1100110");
            numCodeR.Add("2", "1101100");
            numCodeR.Add("3", "1000010");
            numCodeR.Add("4", "1011100");
            numCodeR.Add("5", "1001110");
            numCodeR.Add("6", "1010000");
            numCodeR.Add("7", "1000100");
            numCodeR.Add("8", "1001000");
            numCodeR.Add("9", "1110100");

            numCodeG.Add("0", "0100111");
            numCodeG.Add("1", "0110011");
            numCodeG.Add("2", "0011011");
            numCodeG.Add("3", "0100001");
            numCodeG.Add("4", "0011101");
            numCodeG.Add("5", "0111001");
            numCodeG.Add("6", "0000101");
            numCodeG.Add("7", "0010001");
            numCodeG.Add("8", "0001001");
            numCodeG.Add("9", "0010111");
        }

        //Тест дата = 8711253001202

        private void DrawBarcode(string code, int resolution = 20)
        {
            float height = 26f * resolution; 
            float lineHeight = 15f * resolution;
            float leftOffset = 5f * resolution;
            float rightOffset = 2.5f * resolution;
            float longLineHeight = lineHeight + 1.65f * resolution;
            float fontHeight = 2.75f * resolution; 
            float lineToFontOffset = 0.165f * resolution; 
            float lineWidthDelta = 0.22f * resolution;
            float lineWidthFull = 1.35f * resolution; 
            float lineOffset = 0.2f * resolution;
            float width = leftOffset + rightOffset + 6 * (lineWidthDelta + lineOffset) + 13 * (lineWidthFull + lineOffset);

            Bitmap bitmap = new Bitmap((int)width, (int)height);
            Graphics g = Graphics.FromImage(bitmap);

            Font font = new Font("Arial", 48, FontStyle.Regular, GraphicsUnit.Pixel);

            StringFormat fontFormat = new StringFormat();
            fontFormat.Alignment = StringAlignment.Center;
            fontFormat.LineAlignment = StringAlignment.Center;

            string strCodeStructure = "";
            strCodeStructure = codeStructure[$"{code[0]}"].ToString();
            float x = leftOffset;

            //Рисование первой цифры и первого разделителя
            RectangleF fontRect = new RectangleF( x * (float)0.5, lineHeight + lineToFontOffset, lineWidthFull, fontHeight);
            g.DrawString(code[0].ToString(), font, Brushes.Black, fontRect, fontFormat);

            string strSeparator = "101";
            foreach (char element in strSeparator)
            {
                g.FillRectangle(Brushes.Black, x, 0, Convert.ToInt32(element.ToString()) * lineWidthDelta, longLineHeight);
                x += lineWidthDelta;
            }
            
            for (int i = 1; i < code.Length; i++)
            {
                //Рисование всех цифр и штрихов
                if (strCodeStructure[i - 1] == 'L')
                {
                    fontRect = new RectangleF(x, lineHeight + lineToFontOffset, lineWidthFull, fontHeight);
                    g.DrawString(code[i].ToString(), font, Brushes.Black, fontRect, fontFormat);

                    string strBitNum = numCodeL[$"{code[i]}"].ToString();
                    for (int j = 0; j < strBitNum.Length; j++)
                    {
                        g.FillRectangle(Brushes.Black, x, 0, Convert.ToInt32(strBitNum[j].ToString()) * lineWidthDelta, lineHeight);
                        x += lineWidthDelta;
                    }
                }
                else if (strCodeStructure[i - 1] == 'G')
                {
                    fontRect = new RectangleF(x, lineHeight + lineToFontOffset, lineWidthFull, fontHeight);
                    g.DrawString(code[i].ToString(), font, Brushes.Black, fontRect, fontFormat);

                    string strBitNum = numCodeG[$"{code[i]}"].ToString();
                    for (int j = 0; j < strBitNum.Length; j++)
                    {
                        g.FillRectangle(Brushes.Black, x, 0, Convert.ToInt32(strBitNum[j].ToString()) * lineWidthDelta, lineHeight);
                        x += lineWidthDelta;
                    }
                }
                else if (strCodeStructure[i - 1] == 'R')
                {
                    fontRect = new RectangleF(x, lineHeight + lineToFontOffset, lineWidthFull, fontHeight);
                    g.DrawString(code[i].ToString(), font, Brushes.Black, fontRect, fontFormat);

                    string strBitNum = numCodeR[$"{code[i]}"].ToString();
                    for (int j = 0; j < strBitNum.Length; j++)
                    {
                        g.FillRectangle(Brushes.Black, x, 0, Convert.ToInt32(strBitNum[j].ToString()) * lineWidthDelta, lineHeight);
                        x += lineWidthDelta;
                    }
                }

                if (i == code.Length / 2)
                {
                    strSeparator = "01010";
                    foreach (char element in strSeparator)
                    {
                        g.FillRectangle(Brushes.Black, x, 0, Convert.ToInt32(element.ToString()) * lineWidthDelta, longLineHeight);
                        x += lineWidthDelta;
                    }
                }

                if (i == code.Length - 1)
                {
                    strSeparator = "101";
                    foreach (char element in strSeparator)
                    {
                        g.FillRectangle(Brushes.Black, x, 0, Convert.ToInt32(element.ToString()) * lineWidthDelta, longLineHeight);
                        x += lineWidthDelta;
                    }
                }
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; // делаем чтобы картинка помещалась в pictureBox
            pictureBox1.Image = bitmap; // устанавливаем картинку
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawBarcode(textBox1.Text);
        }
    }
}