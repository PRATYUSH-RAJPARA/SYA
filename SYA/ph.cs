using Newtonsoft.Json.Linq;
using QRCoder;
using Serilog.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SYA
{
    public static class ph
    {
        public static void GenerateQRCode(string url, PictureBox pictureBox)
        {
            RectangleF qrCodeRect = new RectangleF(0, 0, pictureBox.Width, pictureBox.Height);
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                System.Drawing.Bitmap qrCodeBitmap = qrCode.GetGraphic((int)qrCodeRect.Width, System.Drawing.Color.Black, System.Drawing.Color.White, true);
                pictureBox.Image = qrCodeBitmap;
            }
        }
        static Font font = new Font("Arial Black", 8, FontStyle.Bold);
        static SolidBrush brush = new SolidBrush(Color.Black);
        public static void onlyGross(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("G: " + value, new Font("Arial", (float)12, FontStyle.Bold), brush, new RectangleF(4, 4, (float)75, (float)45), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void gross(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("G: " + value, new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void net(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("N: " + value, new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void image(PrintPageEventArgs e)
        {
            Image logoImage = Image.FromFile(helper.ImageFolder + "\\logo.jpg"); e.Graphics.DrawImage(logoImage, new RectangleF(83, 4, (float)22.5, (float)22.5));
        }
        public static void sizeBelowLogo(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)28, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void yamuna(PrintPageEventArgs e)
        {
            e.Graphics.DrawString("YAMUNA", new Font("Arial", (float)4.5, FontStyle.Bold), brush, new RectangleF(79, (float)26.5, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void quality(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)37.75, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void QR(string value, PrintPageEventArgs e)
        {
            RectangleF qrCodeRect = new RectangleF(174, -2, 37, 37);
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(value, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                System.Drawing.Bitmap qrCodeBitmap = qrCode.GetGraphic((int)qrCodeRect.Width, System.Drawing.Color.Black, System.Drawing.Color.White, true);
                e.Graphics.DrawImage(qrCodeBitmap, qrCodeRect);
            }
        }
        public static void labour(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("L: " + value, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)4, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void wholeLabour(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("TL: " + value, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)4, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void other(string value, PrintPageEventArgs e)
        { e.Graphics.DrawString("O: " + value, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)16, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }); }
        public static void tagNumberFirstPart(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)164, (float)32, (float)47, (float)7), new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
        }
        public static void tagNumberSecondPart(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", (float)6.25
                               , FontStyle.Bold), brush, new RectangleF((float)164, (float)42, (float)47, (float)7), new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
        }
        public static void tagNumberSingle(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", (float)6.25, FontStyle.Bold), brush, new RectangleF((float)164, (float)34, (float)47, (float)13), new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far });
        }
        public static void belowLabour1(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)116.5, (float)29, (float)56.5, (float)10), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
        }
        public static void belowLabour2(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)116.5, (float)38, (float)56.5, (float)12), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
        }
        public static void silverMainUpper(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", (float)12, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void silverSecondUpper(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", (float)10, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)4, (float)56.5, (float)27), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void textBelowSilverMainUpper(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", (float)10, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void textBelowSilverSecondUpper(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", (float)10, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)31, (float)56.5, (float)20), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void a(string value, PrintPageEventArgs e) { }
    }
}
