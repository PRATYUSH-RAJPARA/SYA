using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYA
{
    public static class print
    {

        public static void PrintRTGS(object sender, PrintPageEventArgs e, List<string> rtgsdata)
        {

            SolidBrush brush = new SolidBrush(Color.Black);

            e.Graphics.DrawRectangle(Pens.Red, (float)120, (float)135, (float)582.5, (float)745);
            RTGSBackGround();
            branch();
            date();
            RTGSorNEFT();
            PayableAt();
            BenName();
            BenAdd();
            BenAcc();
            BenAccType();
            BenBank();
            BenAddIFSC();
            Amount();
            Commission();
            Total();
            TotalWords();
            AccNo();
            AccName();
            AccPan();
            AccPhone();
            AccSign();

            void RTGSBackGround()
            {
                Image logoImage = Image.FromFile(helper.ImageFolder + "\\RTGS.jpg"); // Replace with the actual path
                e.Graphics.DrawImage(logoImage, new RectangleF(100, 50, (float)625, (float)850));
            }
            void branch()
            {
                //e.Graphics.DrawRectangle(Pens.Red, (float)257.5, (float)187, (float)200, (float)22.5);
                e.Graphics.DrawString(rtgsdata[0].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)257.5, (float)187, (float)200, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });


            }
            void date()
            {

                e.Graphics.DrawString(rtgsdata[1][0].ToString(), new Font("Arial", (float)10, FontStyle.Bold), brush, new RectangleF((float)546, (float)187, (float)26, (float)23), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[1][1].ToString(), new Font("Arial", (float)10, FontStyle.Bold), brush, new RectangleF((float)572, (float)187, (float)26, (float)23), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[1][3].ToString(), new Font("Arial", (float)10, FontStyle.Bold), brush, new RectangleF((float)598, (float)187, (float)26, (float)23), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[1][4].ToString(), new Font("Arial", (float)10, FontStyle.Bold), brush, new RectangleF((float)624, (float)187, (float)26, (float)23), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[1][6].ToString(), new Font("Arial", (float)10, FontStyle.Bold), brush, new RectangleF((float)650, (float)187, (float)26, (float)23), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[1][7].ToString(), new Font("Arial", (float)10, FontStyle.Bold), brush, new RectangleF((float)676, (float)187, (float)26, (float)23), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            void RTGSorNEFT()
            {
                Image logoImage = Image.FromFile(helper.ImageFolder + "\\correct.png"); // Replace with the actual path
                if (rtgsdata[2].ToString() == "RTGS")
                {
                    e.Graphics.DrawImage(logoImage, new RectangleF((float)595, (float)216, (float)13.5, (float)10.75));
                }
                else
                {

                    e.Graphics.DrawImage(logoImage, new RectangleF((float)655, (float)216, (float)13.5, (float)10.75));
                }
            }
            void PayableAt()
            {
                e.Graphics.DrawString(rtgsdata[3], new Font("Arial", (float)12.5, FontStyle.Regular), brush, new RectangleF((float)315.5, (float)232, (float)387.5, (float)21.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            void BenName()
            {
                e.Graphics.DrawString(rtgsdata[4], new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)315.5, (float)253.5, (float)387.5, (float)21.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            void BenAdd()
            {
                e.Graphics.DrawString(rtgsdata[5], new Font("Arial", (float)12.5, FontStyle.Regular), brush, new RectangleF((float)405, (float)275, (float)297.5, (float)37), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            void BenAcc()
            {
                int size = rtgsdata[6].Length;
                for (int i = size; i <= 15; i++)
                {
                    rtgsdata[6] = rtgsdata[6] + " ";
                }
                e.Graphics.DrawString(rtgsdata[6][0].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)405, (float)312, (float)22.5, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[6][1].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)427.5, (float)312, (float)18, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[6][2].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)445.5, (float)312, (float)19, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[6][3].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)464.5, (float)312, (float)18, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[6][4].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)482.5, (float)312, (float)18, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[6][5].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)501.5, (float)312, (float)17.5, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[6][6].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)519.5, (float)312, (float)18, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[6][7].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)538, (float)312, (float)17.75, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[6][8].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)556, (float)312, (float)18, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[6][9].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)574, (float)312, (float)19, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[6][10].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)593, (float)312, (float)18, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[6][11].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)611.5, (float)312, (float)18, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[6][12].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)629.5, (float)312, (float)18, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[6][13].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)647.75, (float)312, (float)18.25, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[6][14].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)666, (float)312, (float)18, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[6][15].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)684, (float)312, (float)18, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            void BenAccType()
            {
                Image logoImage = Image.FromFile(helper.ImageFolder + "\\correct.png"); // Replace with the actual path
               
                //Saving
                if (rtgsdata[7].ToString() == "SA")
                {
                    e.Graphics.DrawImage(logoImage, new RectangleF((float)457, (float)341, (float)13.3, (float)10.75));

                }
                else
                {
                    e.Graphics.DrawImage(logoImage, new RectangleF((float)532.5, (float)341, (float)13.3, (float)10.75));

                }
                //Current
            }
            void BenBank()
            {
                e.Graphics.DrawString(rtgsdata[8].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)405, (float)357, (float)297.5, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, });
            }
            void BenAddIFSC()
            {
                int size = rtgsdata[9].Length;
                for (int i = size; i <= 10; i++)
                {
                    rtgsdata[9] = rtgsdata[9] + " ";
                }
                e.Graphics.DrawString(rtgsdata[9][0].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)457, (float)379, (float)22, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[9][1].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)479.5, (float)379, (float)22, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[9][2].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)502, (float)379, (float)22, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[9][3].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)524, (float)379, (float)22, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[9][4].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)546, (float)379, (float)23, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[9][5].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)569, (float)379, (float)22, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[9][6].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)591, (float)379, (float)22, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[9][7].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)613, (float)379, (float)22, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[9][8].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)636, (float)379, (float)22, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[9][9].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)658, (float)379, (float)22, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[9][10].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)680, (float)379, (float)22, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            void Amount()
            {
                e.Graphics.DrawString(rtgsdata[10].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)225, (float)401, (float)154, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            void Commission()
            {
                e.Graphics.DrawString(rtgsdata[11].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)225, (float)423, (float)154, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            void Total()
            {
                e.Graphics.DrawString(rtgsdata[12].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)225, (float)445, (float)154, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            void TotalWords()
            {
                e.Graphics.DrawString(rtgsdata[13].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)120, (float)490, (float)326.5, (float)44), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            void AccNo()
            {
                int size = rtgsdata[14].Length;
                for (int i = size; i <= 15; i++)
                {
                    rtgsdata[14] = rtgsdata[14] + " ";
                }
                e.Graphics.DrawString(rtgsdata[14][0].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)140, (float)643.25, (float)20, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[14][1].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)160, (float)643.25, (float)18, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[14][2].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)179, (float)643.25, (float)19, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[14][3].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)199, (float)643.25, (float)18, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[14][4].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)218, (float)643.25, (float)18, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[14][5].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)237, (float)643.25, (float)17.5, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[14][6].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)256, (float)643.25, (float)18, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[14][7].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)275, (float)643.25, (float)17.75, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[14][8].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)295, (float)643.25, (float)18, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[14][9].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)314, (float)643.25, (float)19, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[14][10].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)333, (float)643.25, (float)18, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[14][11].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)351, (float)643.25, (float)19, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[14][12].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)370, (float)643.25, (float)19, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[14][13].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)389, (float)643.25, (float)19, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[14][14].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)408, (float)643.25, (float)19, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString(rtgsdata[14][15].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)428.25, (float)643.25, (float)18.25, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            void AccName()
            {
                e.Graphics.DrawString(rtgsdata[15].ToString() ?? "", new Font("Arial", (float)10, FontStyle.Regular), brush, new RectangleF((float)213, (float)732, (float)203, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            void AccPan()
            {
                e.Graphics.DrawString(rtgsdata[16].ToString() ?? "", new Font("Arial", (float)12.5, FontStyle.Regular), brush, new RectangleF((float)213, (float)754, (float)203, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            void AccPhone()
            {
                e.Graphics.DrawString(rtgsdata[17].ToString() ?? "", new Font("Arial", (float)12.5, FontStyle.Regular), brush, new RectangleF((float)213, (float)776, (float)203, (float)22), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            void AccSign()
            {
                e.Graphics.DrawString(rtgsdata[18].ToString() ?? "", new Font("Arial", (float)12.5, FontStyle.Regular), brush, new RectangleF((float)120, (float)850, (float)582.5, (float)30), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }

        }

        public static void PrintPageSearch(object sender, PrintPageEventArgs e, DataGridView dataGridViewSearch)
        {
            DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;
            if (selectedRow != null)
            {
                string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                if (tagNumber.Length > 1)
                {
                    Font font = new Font("Arial Black", 8, FontStyle.Bold); SolidBrush brush = new SolidBrush(Color.Black);
                    float xPos = 0; float yPos = 0; float dpiX = e.PageSettings.PrinterResolution.X;
                    float dpiY = e.PageSettings.PrinterResolution.Y;
                    float rectX = 4; float rectY = 4; float rectWidth = 211; float rectHeight = 45; if ((selectedRow.Cells["gross"].Value ?? "0").ToString() == (selectedRow.Cells["net"].Value ?? "0").ToString())
                    {
                        e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)12, FontStyle.Bold), brush, new RectangleF(4, 4, (float)75, (float)45), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        e.Graphics.DrawString("N: " + (selectedRow.Cells["net"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    Image logoImage = Image.FromFile(helper.ImageFolder + "\\logo.jpg"); e.Graphics.DrawImage(logoImage, new RectangleF(83, 4, (float)22.5, (float)22.5));
                    int hyphenIndex = selectedRow.Cells["itemdesc"].Value.ToString().IndexOf("-");
                    string result = null;
                    if (hyphenIndex != -1 && hyphenIndex < selectedRow.Cells[4].Value.ToString().Length - 1)
                    {
                        result = selectedRow.Cells[4].Value.ToString().Substring(hyphenIndex + 3);
                    }
                    if (result == "BANGAL")
                    {
                        e.Graphics.DrawString((selectedRow.Cells[12].Value ?? "0").ToString(), new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)28, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        e.Graphics.DrawString("YAMUNA", new Font("Arial", (float)4.5, FontStyle.Bold), brush, new RectangleF(79, (float)26.5, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    e.Graphics.DrawString((selectedRow.Cells["itemdesc"].Value ?? "0").ToString().Split('-')[0].Trim() ?? "0", new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)37.75, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    RectangleF qrCodeRect = new RectangleF(174, 2, 37, 37);
                    using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                    {
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(tagNumber, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        System.Drawing.Bitmap qrCodeBitmap = qrCode.GetGraphic((int)qrCodeRect.Width, System.Drawing.Color.Black, System.Drawing.Color.White, true);
                        e.Graphics.DrawImage(qrCodeBitmap, qrCodeRect);
                    }
                    string labour = "0";
                    if (selectedRow.Cells["labour"].Value.ToString() != "0")
                    {
                        labour = (selectedRow.Cells["labour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("L: " + labour, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)4, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else if ((selectedRow.Cells["wholeLabour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["wholeLabour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("TL: " + labour, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)4, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    if ((selectedRow.Cells["other"].Value ?? "0").ToString() != "0")
                    {
                        e.Graphics.DrawString("O: " + (selectedRow.Cells["other"].Value ?? "0").ToString(), new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)16, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    string firstPart = null;
                    string secondPart = null;
                    int length = tagNumber.Length;
                    if (length >= 10)
                    {
                        int lastIndex = length - 5;
                        firstPart = tagNumber.Substring(lastIndex);
                        secondPart = tagNumber.Substring(0, lastIndex);
                        e.Graphics.DrawString(secondPart, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)29, (float)56.5, (float)10), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        e.Graphics.DrawString(firstPart, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)38, (float)56.5, (float)12), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        e.Graphics.DrawString(tagNumber, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)29, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    string huid1 = (selectedRow.Cells["huid1"].Value ?? "0").ToString();
                    string huid2 = (selectedRow.Cells["huid2"].Value ?? "0").ToString();
                    if (huid1.Length == 6)
                    {
                        e.Graphics.DrawString(huid1, new Font("Arial", (float)5, FontStyle.Bold), brush, new RectangleF((float)174, (float)38, (float)37, (float)7), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    if (huid2.Length == 6)
                    {
                        e.Graphics.DrawString(huid2, new Font("Arial", (float)5, FontStyle.Bold), brush, new RectangleF((float)174, (float)44, (float)37, (float)7), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    string updateQuery = $"UPDATE MAIN_DATA SET PRINT = '1'   WHERE TAG_NO = '{tagNumber}'";
                    helper.RunQueryWithoutParametersSYADataBase(updateQuery, "ExecuteNonQuery");
                }
            }
        }
        public static void PrintPageAddGold(object sender, PrintPageEventArgs e, DataGridViewRow selectedRow)
        {
            if (selectedRow != null)
            {
                string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                if (tagNumber.Length > 1)
                {

                    //  Font font = new Font("Arial Black", 8, FontStyle.Bold); // Adjust the font size
                    SolidBrush brush = new SolidBrush(Color.Black);


                    if ((selectedRow.Cells["gross"].Value ?? "0").ToString() == (selectedRow.Cells["net"].Value ?? "0").ToString())
                    {
                        e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)12, FontStyle.Bold), brush, new RectangleF(4, 4, (float)75, (float)45), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                        e.Graphics.DrawString("N: " + (selectedRow.Cells["net"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    }

                    //logo
                    Image logoImage = Image.FromFile(helper.ImageFolder + "\\logo.jpg"); // Replace with the actual path
                    e.Graphics.DrawImage(logoImage, new RectangleF(83, 4, (float)22.5, (float)22.5));

                    //logo name 

                    if (selectedRow.Cells["type"].Value.ToString() == "BANGAL")
                    {
                        e.Graphics.DrawString((selectedRow.Cells[10].Value ?? "0").ToString(), new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)28, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {

                        e.Graphics.DrawString("YAMUNA", new Font("Arial", (float)4.5, FontStyle.Bold), brush, new RectangleF(79, (float)26.5, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    //caret
                    e.Graphics.DrawString((selectedRow.Cells["caret"].Value ?? "0").ToString().Split('-')[0].Trim() ?? "0", new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)37.75, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    // Draw the QR code rectangle
                    RectangleF qrCodeRect = new RectangleF(174, 2, 37, 37);
                    //  e.Graphics.DrawRectangle(Pens.Red, qrCodeRect.X, qrCodeRect.Y, qrCodeRect.Width, qrCodeRect.Height);
                    using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                    {
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(tagNumber, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        System.Drawing.Bitmap qrCodeBitmap = qrCode.GetGraphic((int)qrCodeRect.Width, System.Drawing.Color.Black, System.Drawing.Color.White, true);
                        // Draw the QR code onto the printing surface
                        e.Graphics.DrawImage(qrCodeBitmap, qrCodeRect);
                    }

                    //labour                
                    string labour = "0";
                    if ((selectedRow.Cells["labour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["labour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("L: " + labour, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)4, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else if ((selectedRow.Cells["wholeLabour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["wholeLabour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("TL: " + labour, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)4, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    //other
                    if ((selectedRow.Cells["other"].Value ?? "0").ToString() != "0")
                    {

                        e.Graphics.DrawString("O: " + (selectedRow.Cells["other"].Value ?? "0").ToString(), new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)16, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    //Tag number
                    string firstPart = null;
                    string secondPart = null;
                    int length = tagNumber.Length;
                    if (length >= 10)
                    {
                        int lastIndex = length - 5;
                        firstPart = tagNumber.Substring(lastIndex);
                        secondPart = tagNumber.Substring(0, lastIndex);
                        e.Graphics.DrawString(secondPart, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)29, (float)56.5, (float)10), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        e.Graphics.DrawString(firstPart, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)38, (float)56.5, (float)12), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        e.Graphics.DrawString(tagNumber, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)30, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    //huid
                    //   e.Graphics.DrawRectangle(Pens.Red, (float)174, (float)37, (float)37, (float)5);
                    //  e.Graphics.DrawRectangle(Pens.Red, (float)174, (float)43, (float)37, (float)5);

                    string huid1 = (selectedRow.Cells["huid1"].Value ?? "0").ToString();
                    string huid2 = (selectedRow.Cells["huid2"].Value ?? "0").ToString();
                    if (huid1.Length == 6)
                    {
                        e.Graphics.DrawString(huid1, new Font("Arial", (float)5, FontStyle.Bold), brush, new RectangleF((float)174, (float)38, (float)37, (float)7), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    if (huid2.Length == 6)
                    {
                        e.Graphics.DrawString(huid2, new Font("Arial", (float)5, FontStyle.Bold), brush, new RectangleF((float)174, (float)44, (float)37, (float)7), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    string updateQuery = $"UPDATE MAIN_DATA SET PRINT = '1'   WHERE TAG_NO = '{tagNumber}'";

                    helper.RunQueryWithoutParametersSYADataBase(updateQuery, "ExecuteNonQuery");

                }
            }
        }

        public static void PrintPageAddSilver(object sender, PrintPageEventArgs e, DataGridView addSilverDataGridView, string tagtype)
        {
            DataGridViewRow selectedRow = addSilverDataGridView.CurrentRow;

            if (selectedRow != null)
            {
                string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                string caret = (selectedRow.Cells["caret"].Value ?? "").ToString() ?? "0";
                if (tagNumber.Length > 1 && tagtype == "price")

                // if (tagNumber.Length > 1 && caret == "925" && tagtype == "price")
                //if (tagNumber.Length > 1 && caret == "925")
                {
                    Font font = new Font("Arial Black", 8, FontStyle.Bold); // Adjust the font size
                    SolidBrush brush = new SolidBrush(Color.Black);

                    // Set the starting position for printing
                    float xPos = 0; // Adjust the starting X position
                    float yPos = 0; // Adjust the starting Y position

                    // Get the printer DPI
                    float dpiX = e.PageSettings.PrinterResolution.X;
                    float dpiY = e.PageSettings.PrinterResolution.Y;

                    float rectX = 4; // Adjust the X position of the rectangle
                    float rectY = 4; // Adjust the Y position of the rectangle
                    float rectWidth = 211; // Adjust the width of the rectangle
                    float rectHeight = 45; // Adjust the height of the rectangle

                    //price
                    e.Graphics.DrawString("\u20B9" + selectedRow.Cells["price"].Value.ToString(), new Font("Arial", (float)14, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)45), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    //logo
                    Image logoImage = Image.FromFile(helper.ImageFolder + "\\logo.jpg"); // Replace with the actual path
                    e.Graphics.DrawImage(logoImage, new RectangleF(83, 4, (float)22.5, (float)22.5));

                    //logo name 
                    e.Graphics.DrawString("YAMUNA", new Font("Arial", (float)4.5, FontStyle.Bold), brush, new RectangleF(79, (float)26.5, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    //caret
                    e.Graphics.DrawString(selectedRow.Cells["caret"].Value.ToString().Split('-')[0].Trim(), new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)37.75, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    // Draw the QR code rectangle
                    RectangleF qrCodeRect = new RectangleF(174, 4, 37, 37);
                    using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                    {
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(tagNumber, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        System.Drawing.Bitmap qrCodeBitmap = qrCode.GetGraphic((int)qrCodeRect.Width, System.Drawing.Color.Black, System.Drawing.Color.White, true);
                        // Draw the QR code onto the printing surface
                        e.Graphics.DrawImage(qrCodeBitmap, qrCodeRect);
                    }

                    //price2
                    e.Graphics.DrawString("\u20B9" + selectedRow.Cells["price"].Value.ToString(), new Font("Arial", (float)10, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)4, (float)56.5, (float)27), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    //Tag number
                    string firstPart = null;
                    string secondPart = null;
                    int length = tagNumber.Length;
                    if (length >= 10)
                    {
                        int lastIndex = length - 5;
                        firstPart = tagNumber.Substring(lastIndex);
                        secondPart = tagNumber.Substring(0, lastIndex);
                        e.Graphics.DrawString(secondPart, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)29, (float)56.5, (float)10), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        e.Graphics.DrawString(firstPart, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)38, (float)56.5, (float)12), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        e.Graphics.DrawString(tagNumber, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)30, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    //huid
                    string huid1 = (selectedRow.Cells[10].Value ?? "").ToString();
                    if (huid1.Length > 0)
                    {
                        e.Graphics.DrawString(huid1, new Font("Arial", (float)7.5, FontStyle.Bold), brush, new RectangleF((float)174, (float)40, (float)37, (float)9), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                }
                else if (tagNumber.Length > 1 && caret == "925" && tagtype == "weight")
                //if (tagNumber.Length > 1 && caret == "925")
                {
                    Font font = new Font("Arial Black", 8, FontStyle.Bold); // Adjust the font size
                    SolidBrush brush = new SolidBrush(Color.Black);

                    // Set the starting position for printing
                    float xPos = 0; // Adjust the starting X position
                    float yPos = 0; // Adjust the starting Y position

                    // Get the printer DPI
                    float dpiX = e.PageSettings.PrinterResolution.X;
                    float dpiY = e.PageSettings.PrinterResolution.Y;

                    float rectX = 4; // Adjust the X position of the rectangle
                    float rectY = 4; // Adjust the Y position of the rectangle
                    float rectWidth = 211; // Adjust the width of the rectangle
                    float rectHeight = 45; // Adjust the height of the rectangle


                    //gross weight
                    //net weight
                    if ((selectedRow.Cells["type"].Value ?? "0").ToString() == "KADALI" || (selectedRow.Cells["size"].Value ?? "0").ToString() != null)
                    {
                        if ((selectedRow.Cells["gross"].Value ?? "0").ToString() == (selectedRow.Cells["net"].Value ?? "0").ToString())
                        {
                            e.Graphics.DrawString((selectedRow.Cells["net"].Value ?? "0").ToString(), new Font("Arial", (float)12, FontStyle.Bold), brush, new RectangleF(4, (float)4, (float)75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                            e.Graphics.DrawString((selectedRow.Cells["size"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                        }
                        else
                        {
                            e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                            e.Graphics.DrawString("N: " + (selectedRow.Cells["net"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                        }
                        string huid1 = (selectedRow.Cells["size"].Value ?? "").ToString();
                        if (huid1.Length > 0)
                        {
                            e.Graphics.DrawString(huid1, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)174, (float)40, (float)37, (float)9), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        }
                    }
                    else
                    {
                        if ((selectedRow.Cells["gross"].Value ?? "0").ToString() == (selectedRow.Cells["net"].Value ?? "0").ToString())
                        {
                            e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)12, FontStyle.Bold), brush, new RectangleF(4, 4, (float)75, (float)45), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                        }
                        else
                        {
                            e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                            e.Graphics.DrawString("N: " + (selectedRow.Cells["net"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                        }
                        //huid

                        string huid1 = (selectedRow.Cells["size"].Value ?? "").ToString();
                        if (huid1.Length > 0)
                        {
                            e.Graphics.DrawString(huid1, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)174, (float)40, (float)37, (float)9), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        }
                    }
                    //logo
                    Image logoImage = Image.FromFile(helper.ImageFolder + "\\logo.jpg"); // Replace with the actual path
                    e.Graphics.DrawImage(logoImage, new RectangleF(83, 4, (float)22.5, (float)22.5));

                    //logo name 
                    e.Graphics.DrawString("YAMUNA", new Font("Arial", (float)4.5, FontStyle.Bold), brush, new RectangleF(79, (float)26.5, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    //caret
                    e.Graphics.DrawString(selectedRow.Cells["caret"].Value.ToString().Split('-')[0].Trim(), new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)37.75, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    // Draw the QR code rectangle
                    RectangleF qrCodeRect = new RectangleF(174, 4, 37, 37);
                    using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                    {
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(tagNumber, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        System.Drawing.Bitmap qrCodeBitmap = qrCode.GetGraphic((int)qrCodeRect.Width, System.Drawing.Color.Black, System.Drawing.Color.White, true);
                        // Draw the QR code onto the printing surface
                        e.Graphics.DrawImage(qrCodeBitmap, qrCodeRect);
                    }

                    //net
                    e.Graphics.DrawString(selectedRow.Cells["net"].Value.ToString(), new Font("Arial", (float)10, FontStyle.Bold), brush, new RectangleF((float)115.5, (float)4, (float)56.5, (float)17), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    //test
                    //tagno
                    e.Graphics.DrawString(tagNumber, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)115.5, (float)38, (float)105.5, (float)12), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    //labour                
                    string labour = "0";
                    if ((selectedRow.Cells["labour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["labour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("L: " + labour, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)119.5, (float)21, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }
                    else if ((selectedRow.Cells["wholeLabour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["wholeLabour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("TL: " + labour, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)119.5, (float)19, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }

                    //other
                    if ((selectedRow.Cells["other"].Value ?? "0").ToString() != "0")
                    {

                        e.Graphics.DrawString("O: " + (selectedRow.Cells["other"].Value ?? "0").ToString(), new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)119.5, (float)28.5, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }

                }
                else if (tagNumber.Length > 1 && caret == "SLO" && tagtype == "weight")
                {
                    Font font = new Font("Arial Black", 8, FontStyle.Bold); // Adjust the font size
                    SolidBrush brush = new SolidBrush(Color.Black);

                    // Set the starting position for printing
                    float xPos = 0; // Adjust the starting X position
                    float yPos = 0; // Adjust the starting Y position

                    // Get the printer DPI
                    float dpiX = e.PageSettings.PrinterResolution.X;
                    float dpiY = e.PageSettings.PrinterResolution.Y;

                    float rectX = 4; // Adjust the X position of the rectangle
                    float rectY = 4; // Adjust the Y position of the rectangle
                    float rectWidth = 211; // Adjust the width of the rectangle
                    float rectHeight = 45; // Adjust the height of the rectangle


                    //gross weight
                    //net weight
                    if ((selectedRow.Cells["gross"].Value ?? "0").ToString() == (selectedRow.Cells["net"].Value ?? "0").ToString())
                    {
                        e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)12, FontStyle.Bold), brush, new RectangleF(4, 4, (float)75, (float)45), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    }
                    else
                    {
                        e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                        e.Graphics.DrawString("N: " + (selectedRow.Cells["net"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    }
                    //logo
                    Image logoImage = Image.FromFile(helper.ImageFolder + "\\logo.jpg"); // Replace with the actual path
                    e.Graphics.DrawImage(logoImage, new RectangleF(83, 4, (float)22.5, (float)22.5));

                    //logo name 
                    e.Graphics.DrawString("YAMUNA", new Font("Arial", (float)4.5, FontStyle.Bold), brush, new RectangleF(79, (float)26.5, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    //caret
                    e.Graphics.DrawString(selectedRow.Cells["caret"].Value.ToString().Split('-')[0].Trim(), new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)37.75, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    // Draw the QR code rectangle
                    RectangleF qrCodeRect = new RectangleF(174, 4, 37, 37);
                    using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                    {
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(tagNumber, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        System.Drawing.Bitmap qrCodeBitmap = qrCode.GetGraphic((int)qrCodeRect.Width, System.Drawing.Color.Black, System.Drawing.Color.White, true);
                        // Draw the QR code onto the printing surface
                        e.Graphics.DrawImage(qrCodeBitmap, qrCodeRect);
                    }

                    //net
                    e.Graphics.DrawString(selectedRow.Cells["net"].Value.ToString(), new Font("Arial", (float)10, FontStyle.Bold), brush, new RectangleF((float)115.5, (float)4, (float)56.5, (float)17), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });

                    //test
                    //tagno
                    e.Graphics.DrawString(tagNumber, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)115.5, (float)38, (float)105.5, (float)12), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    //labour                
                    string labour = "0";
                    if ((selectedRow.Cells["labour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["labour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("L: " + labour, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)119.5, (float)21, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }
                    else if ((selectedRow.Cells["wholeLabour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["wholeLabour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("TL: " + labour, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)119.5, (float)19, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }

                    //other
                    if ((selectedRow.Cells["other"].Value ?? "0").ToString() != "0")
                    {

                        e.Graphics.DrawString("O: " + (selectedRow.Cells["other"].Value ?? "0").ToString(), new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)119.5, (float)28.5, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }




                }
            }
        }

    }
}
