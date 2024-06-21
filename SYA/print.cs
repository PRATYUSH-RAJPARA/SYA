﻿using Newtonsoft.Json.Linq;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
namespace SYA
{
    public static class print
    {
        public static void PrintLabel(object sender, PrintPageEventArgs e, string name, string price, string type) {
            SolidBrush brush = new SolidBrush(Color.Black);
            if (type == "name")
            {
                e.Graphics.DrawString(name, new Font("Arial", (float)15, FontStyle.Bold), brush, new RectangleF(4, 4, (float)217.5, (float)45), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }

            if (type == "price")
            {
                e.Graphics.DrawString("\u20B9" + " " + price, new Font("Arial", (float)15, FontStyle.Bold), brush, new RectangleF(4, 4, (float)217.5, (float)45), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }

            if (type == "nameandprice")
            {
                e.Graphics.DrawString(name, new Font("Arial", (float)15, FontStyle.Bold), brush, new RectangleF(4, (float)4, (float)217.5, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                e.Graphics.DrawString("\u20B9" + " " + price, new Font("Arial", (float)15, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, (float)217.5, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            



        }
        public static void PrintReparing(object sender, PrintPageEventArgs e, List<string> ReparingData)
        {
            SolidBrush brush = new SolidBrush(Color.Black);
            e.Graphics.DrawLine(Pens.Red, (float)111.5, 0, (float)111.5, (float)27);
            e.Graphics.DrawLine(Pens.Red, (float)112, 0, (float)112.5, (float)27);
            e.Graphics.DrawLine(Pens.Red, (float)112.5, 0, (float)111.5, (float)27);
            e.Graphics.DrawLine(Pens.Red, (float)113, 0, (float)113.5, (float)27);
            e.Graphics.DrawLine(Pens.Red, (float)113.5, 0, (float)111.5, (float)27);
            e.Graphics.DrawLine(Pens.Red, (float)114, 0, (float)114.5, (float)27);
            e.Graphics.DrawLine(Pens.Red, (float)170, (float)4, (float)170, (float)29);
            e.Graphics.DrawLine(Pens.Red, 4, (float)26.5, (float)213, (float)26.5);
            e.Graphics.DrawString(ReparingData[0].ToString(), new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF(4, 4, (float)109.5, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            e.Graphics.DrawString(ReparingData[4].ToString() + "  - ((  "+ ReparingData[2].ToString() +"  ))", new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, (float)213, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            e.Graphics.DrawString(ReparingData[1].ToString(), new Font("Arial", (float)6.5, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)4, (float)56.5, (float)23), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });       
            e.Graphics.DrawString(ReparingData[3].ToString(), new Font("Arial", (float)9, FontStyle.Bold), brush, new RectangleF((float)170, (float)4, (float)47, (float)23), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void PrintRTGS(object sender, PrintPageEventArgs e, List<string> rtgsdata)
        {
            SolidBrush brush = new SolidBrush(Color.Black);
            e.Graphics.DrawRectangle(Pens.Red, (float)120, (float)135, (float)582.5, (float)745);


            RectangleF qrCodeRect = new RectangleF(100, 100, (float)300, (float)300);
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode("https://www.instagram.com/pushti.art", QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    System.Drawing.Bitmap qrCodeBitmap = qrCode.GetGraphic((int)qrCodeRect.Width, System.Drawing.Color.Black, System.Drawing.Color.White, true);
                    e.Graphics.DrawImage(qrCodeBitmap, qrCodeRect);
                }
            

           // ph.QR("https://www.instagram.com/pushti.art", e);
            //RTGSBackGround();
            //branch();
            //date();
            //RTGSorNEFT();
            //PayableAt();
            //BenName();
            //BenAdd();
            //BenAcc();
            //BenAccType();
            //BenBank();
            //BenAddIFSC();
            //Amount();
            //Commission();
            //Total();
            //TotalWords();
            //AccNo();
            //AccName();
            //AccPan();
            //AccPhone();
            //AccSign();
            void RTGSBackGround()
            {
                Image logoImage = Image.FromFile(helper.ImageFolder + "\\RTGS.jpg");
                e.Graphics.DrawImage(logoImage, new RectangleF(100, 50, (float)625, (float)850));
            }
            void branch()
            {
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
                Image logoImage = Image.FromFile(helper.ImageFolder + "\\correct.png");
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
                Image logoImage = Image.FromFile(helper.ImageFolder + "\\correct.png");
                if (rtgsdata[7].ToString() == "SA")
                {
                    e.Graphics.DrawImage(logoImage, new RectangleF((float)457, (float)341, (float)13.3, (float)10.75));
                }
                else
                {
                    e.Graphics.DrawImage(logoImage, new RectangleF((float)532.5, (float)341, (float)13.3, (float)10.75));
                }
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
        public static void PrintPageSearch(object sender, PrintPageEventArgs e, DataGridView dataGridViewSearch, string tagtype)
        {
            DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;
            if (selectedRow != null)
            {
                string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                if (tagNumber.Length > 1)
                {
                    if (selectedRow.Cells["IT_TYPE"].Value.ToString() == "G")
                    {
                        string caret = (selectedRow.Cells["itemdesc"].Value ?? "0").ToString().Split('-')[0].Trim() ?? "0";
                        if ((selectedRow.Cells["gross"].Value ?? "0").ToString() == (selectedRow.Cells["net"].Value ?? "0").ToString())
                        {
                            ph.onlyGross((selectedRow.Cells["gross"].Value ?? "0").ToString(), e);
                        }
                        else
                        {
                            ph.gross((selectedRow.Cells["gross"].Value ?? "0").ToString(), e);
                            ph.net((selectedRow.Cells["net"].Value ?? "0").ToString(), e);
                        }
                        ph.image(e);
                        if ((selectedRow.Cells[12].Value ?? "0").ToString().Length > 0 && (selectedRow.Cells[11].Value ?? "0").ToString() != "0")
                        {
                            ph.sizeBelowLogo((selectedRow.Cells[12].Value ?? "0").ToString(), e);
                        }
                        else
                        {
                            ph.yamuna(e);
                        }
                        ph.quality(caret, e);
                        ph.QR(tagNumber, e);
                        string labour = "0";
                        if ((selectedRow.Cells["labour"].Value ?? "-").ToString() != "0")
                        {
                            labour = (selectedRow.Cells["labour"].Value ?? "-").ToString();
                            ph.labour(labour, e);
                        }
                        else if ((selectedRow.Cells["wholeLabour"].Value ?? "-").ToString() != "0")
                        {
                            labour = (selectedRow.Cells["wholeLabour"].Value ?? "-").ToString();
                            ph.wholeLabour(labour, e);
                        }
                        if ((selectedRow.Cells["other"].Value ?? "0").ToString() != "0")
                        {
                            ph.other((selectedRow.Cells["other"].Value ?? "0").ToString(), e);
                        }
                        int length = tagNumber.Length;
                        if (length >= 10)
                        {
                            int lastIndex = length - 8;
                            string firstPart = tagNumber.Substring(3, lastIndex);
                            string secondPart = tagNumber.Substring(lastIndex + 3);
                            ph.tagNumberFirstPart(firstPart, e);
                            ph.tagNumberSecondPart(secondPart, e);
                        }
                        else
                        {
                            ph.tagNumberSingle(tagNumber, e);
                        }
                        string huid1 = (selectedRow.Cells["huid1"].Value ?? "0").ToString();
                        string huid2 = (selectedRow.Cells["huid2"].Value ?? "0").ToString();
                        if (huid1.Length == 6)
                        {
                            ph.belowLabour1(huid1, e);
                        }
                        if (huid2.Length == 6)
                        {
                            ph.belowLabour2(huid2, e);
                        }
                        string updateQuery = $"UPDATE MAIN_DATA SET PRINT = '1'   WHERE TAG_NO = '{tagNumber}'";
                        helper.RunQueryWithoutParametersSYADataBase(updateQuery, "ExecuteNonQuery");
                    }
                    if (selectedRow.Cells["IT_TYPE"].Value.ToString() == "S")
                    {
                        string caret = (selectedRow.Cells["itemdesc"].Value ?? "0").ToString().Split('-')[0].Trim() ?? "0";
                        if (tagtype == "weight")
                        {
                            if ((selectedRow.Cells["gross"].Value ?? "0").ToString() == (selectedRow.Cells["net"].Value ?? "0").ToString())
                            {
                                ph.silverMainUpper((selectedRow.Cells["gross"].Value ?? "0").ToString(), e);
                                if ((selectedRow.Cells["size"].Value ?? "").ToString().Length > 0)
                                {
                                    ph.textBelowSilverMainUpper((selectedRow.Cells["size"].Value ?? "").ToString(), e);
                                    ph.belowLabour2((selectedRow.Cells[10].Value ?? "").ToString(), e);
                                }
                                else
                                {
                                    ph.textBelowSilverMainUpper(caret, e);
                                    ph.belowLabour2(caret, e);
                                }
                            }
                            else
                            {
                                ph.gross((selectedRow.Cells["gross"].Value ?? "0").ToString(), e);
                                ph.net((selectedRow.Cells["net"].Value ?? "0").ToString(), e);
                                if ((selectedRow.Cells["size"].Value ?? "").ToString().Length > 0)
                                {
                                    ph.belowLabour2((selectedRow.Cells["size"].Value ?? "").ToString(), e);
                                }
                                else
                                {
                                    ph.belowLabour2(caret, e);
                                }
                            }
                            ph.belowLabour1((selectedRow.Cells["gross"].Value ?? "0").ToString(), e);
                            ph.image(e);
                            ph.yamuna(e);
                            ph.quality(caret, e);
                            ph.QR(tagNumber, e);
                            string labour = "0";
                            if ((selectedRow.Cells["labour"].Value ?? "-").ToString() != "0")
                            {
                                labour = (selectedRow.Cells["labour"].Value ?? "-").ToString();
                                ph.labour(labour, e);
                            }
                            else if ((selectedRow.Cells["wholeLabour"].Value ?? "-").ToString() != "0")
                            {
                                labour = (selectedRow.Cells["wholeLabour"].Value ?? "-").ToString();
                                ph.wholeLabour(labour, e);
                            }
                            if ((selectedRow.Cells["other"].Value ?? "0").ToString() != "0")
                            {
                                ph.other((selectedRow.Cells["other"].Value ?? "0").ToString(), e);
                            }
                            int length = tagNumber.Length;
                            if (length >= 10)
                            {
                                int lastIndex = length - 8;
                                string firstPart = tagNumber.Substring(3, lastIndex);
                                string secondPart = tagNumber.Substring(lastIndex + 3);
                                ph.tagNumberFirstPart(firstPart, e);
                                ph.tagNumberSecondPart(secondPart, e);
                            }
                            else
                            {
                                ph.tagNumberSingle(tagNumber, e);
                            }
                        }
                        else if (tagtype == "price")
                        {
                            ph.silverMainUpper("\u20B9" + selectedRow.Cells["price"].Value.ToString(), e);
                            ph.image(e);
                            ph.yamuna(e);
                            ph.quality(caret, e);
                            ph.QR(tagNumber, e);
                            ph.silverSecondUpper("\u20B9" + selectedRow.Cells["price"].Value.ToString(), e);
                            int length = tagNumber.Length;
                            if (length >= 10)
                            {
                                int lastIndex = length - 5;
                                string firstPart = tagNumber.Substring(0, lastIndex);
                                string secondPart = tagNumber.Substring(lastIndex);
                                ph.tagNumberFirstPart(firstPart, e);
                                ph.tagNumberSecondPart(secondPart, e);
                            }
                            else
                            {
                                ph.tagNumberSingle(tagNumber, e);
                            }
                            if ((selectedRow.Cells["size"].Value ?? "").ToString().Length > 0)
                            {
                                ph.textBelowSilverMainUpper((selectedRow.Cells["size"].Value ?? "").ToString(), e);
                                ph.textBelowSilverSecondUpper((selectedRow.Cells["size"].Value ?? "").ToString(), e);
                            }
                            else
                            {
                                ph.textBelowSilverMainUpper(caret, e);
                                ph.textBelowSilverSecondUpper(caret, e);
                            }
                        }
                    }
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
                    if ((selectedRow.Cells["gross"].Value ?? "0").ToString() == (selectedRow.Cells["net"].Value ?? "0").ToString())
                    {
                        ph.onlyGross((selectedRow.Cells["gross"].Value ?? "0").ToString(), e);
                    }
                    else
                    {
                        ph.gross((selectedRow.Cells["gross"].Value ?? "0").ToString(), e);
                        ph.net((selectedRow.Cells["net"].Value ?? "0").ToString(), e);
                    }
                    ph.image(e);
                    if ((selectedRow.Cells[11].Value ?? "0").ToString().Length > 0 && (selectedRow.Cells[11].Value ?? "0").ToString() != "0")
                    {
                        ph.sizeBelowLogo((selectedRow.Cells[11].Value ?? "0").ToString(), e);
                    }
                    else
                    {
                        ph.yamuna(e);
                    }
                    ph.quality((selectedRow.Cells["caret"].Value ?? "0").ToString().Split('-')[0].Trim() ?? "0", e);
                    ph.QR(tagNumber, e);
                    string labour = "0";
                    if ((selectedRow.Cells["labour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["labour"].Value ?? "-").ToString();
                        ph.labour(labour, e);
                    }
                    else if ((selectedRow.Cells["wholeLabour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["wholeLabour"].Value ?? "-").ToString();
                        ph.wholeLabour(labour, e);
                    }
                    if ((selectedRow.Cells["other"].Value ?? "0").ToString() != "0")
                    {
                        ph.other((selectedRow.Cells["other"].Value ?? "0").ToString(), e);
                    }
                    int length = tagNumber.Length;
                    if (length >= 10)
                    {
                        int lastIndex = length - 8;
                        string firstPart = tagNumber.Substring(3, lastIndex);
                        string secondPart = tagNumber.Substring(lastIndex + 3);
                        ph.tagNumberFirstPart(firstPart, e);
                        ph.tagNumberSecondPart(secondPart, e);
                    }
                    else
                    {
                        ph.tagNumberSingle(tagNumber, e);
                    }
                    string huid1 = (selectedRow.Cells["huid1"].Value ?? "0").ToString();
                    string huid2 = (selectedRow.Cells["huid2"].Value ?? "0").ToString();
                    if (huid1.Length == 6)
                    {
                        ph.belowLabour1(huid1, e);
                    }
                    if (huid2.Length == 6)
                    {
                        ph.belowLabour2(huid2, e);
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
                if (tagNumber.Length > 1)
                {
                    if (tagtype == "weight")
                    {
                        if ((selectedRow.Cells["gross"].Value ?? "0").ToString() == (selectedRow.Cells["net"].Value ?? "0").ToString())
                        {
                            ph.silverMainUpper((selectedRow.Cells["gross"].Value ?? "0").ToString(), e);
                            if ((selectedRow.Cells[10].Value ?? "").ToString().Length > 0)
                            {
                                ph.textBelowSilverMainUpper((selectedRow.Cells[10].Value ?? "").ToString(), e);
                                ph.belowLabour2((selectedRow.Cells[10].Value ?? "").ToString(), e);
                            }
                            else
                            {
                                ph.textBelowSilverMainUpper(selectedRow.Cells["caret"].Value.ToString().Split('-')[0].Trim(), e);
                                ph.belowLabour2(selectedRow.Cells["caret"].Value.ToString().Split('-')[0].Trim(), e);
                            }
                        }
                        else
                        {
                            ph.gross((selectedRow.Cells["gross"].Value ?? "0").ToString(), e);
                            ph.net((selectedRow.Cells["net"].Value ?? "0").ToString(), e);
                            if ((selectedRow.Cells[10].Value ?? "").ToString().Length > 0)
                            {
                                ph.belowLabour2((selectedRow.Cells[10].Value ?? "").ToString(), e);
                            }
                            else
                            {
                                ph.belowLabour2(selectedRow.Cells["caret"].Value.ToString().Split('-')[0].Trim(), e);
                            }
                        }
                        ph.belowLabour1((selectedRow.Cells["gross"].Value ?? "0").ToString(), e);
                        ph.image(e);
                        ph.yamuna(e);
                        ph.quality(selectedRow.Cells["caret"].Value.ToString().Split('-')[0].Trim(), e);
                        ph.QR(tagNumber, e);
                        string labour = "0";
                        if ((selectedRow.Cells["labour"].Value ?? "-").ToString() != "0")
                        {
                            labour = (selectedRow.Cells["labour"].Value ?? "-").ToString();
                            ph.labour(labour, e);
                        }
                        else if ((selectedRow.Cells["wholeLabour"].Value ?? "-").ToString() != "0")
                        {
                            labour = (selectedRow.Cells["wholeLabour"].Value ?? "-").ToString();
                            ph.wholeLabour(labour, e);
                        }
                        if ((selectedRow.Cells["other"].Value ?? "0").ToString() != "0")
                        {
                            ph.other((selectedRow.Cells["other"].Value ?? "0").ToString(), e);
                        }
                        int length = tagNumber.Length;
                        if (length >= 10)
                        {
                            int lastIndex = length - 8;
                            string firstPart = tagNumber.Substring(3, lastIndex);
                            string secondPart = tagNumber.Substring(lastIndex + 3);
                            ph.tagNumberFirstPart(firstPart, e);
                            ph.tagNumberSecondPart(secondPart, e);
                        }
                        else
                        {
                            ph.tagNumberSingle(tagNumber, e);
                        }
                    }
                    else if (tagtype == "price")
                    {
                        ph.silverMainUpper("\u20B9" + selectedRow.Cells["price"].Value.ToString(), e);
                        ph.image(e);
                        ph.yamuna(e);
                        ph.quality(selectedRow.Cells["caret"].Value.ToString().Split('-')[0].Trim(), e);
                        ph.QR(tagNumber, e);
                        ph.silverSecondUpper("\u20B9" + selectedRow.Cells["price"].Value.ToString(), e);
                        int length = tagNumber.Length;
                        if (length >= 10)
                        {
                            int lastIndex = length - 8;
                            string firstPart = tagNumber.Substring(3, lastIndex);
                            string secondPart = tagNumber.Substring(lastIndex + 3);
                            ph.tagNumberFirstPart(firstPart, e);
                            ph.tagNumberSecondPart(secondPart, e);
                        }
                        else
                        {
                            ph.tagNumberSingle(tagNumber, e);
                        }
                        if ((selectedRow.Cells[10].Value ?? "").ToString().Length > 0)
                        {
                            ph.textBelowSilverMainUpper((selectedRow.Cells[10].Value ?? "").ToString(), e);
                            ph.textBelowSilverSecondUpper((selectedRow.Cells[10].Value ?? "").ToString(), e);
                        }
                        else
                        {
                            ph.textBelowSilverMainUpper(selectedRow.Cells["caret"].Value.ToString().Split('-')[0].Trim(), e);
                            ph.textBelowSilverSecondUpper(selectedRow.Cells["caret"].Value.ToString().Split('-')[0].Trim(), e);
                        }
                    }
                }
            }
        }
    }
}
