using QRCoder;
using System.Drawing.Printing;
namespace SYA.Helper
{
    public static class TagPrintHelper
    {
        static Font font = new Font("Arial Black", 8, FontStyle.Bold);
        static SolidBrush brush = new SolidBrush(Color.Black);
        public static void onlyGross(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("G: " + value, new Font("Arial", 12, FontStyle.Bold), brush, new RectangleF(4, 4, 75, 45), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
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
            e.Graphics.DrawString(value, new Font("Arial", 6, FontStyle.Bold), brush, new RectangleF(79, 28, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void yamuna(PrintPageEventArgs e)
        {
            e.Graphics.DrawString("YAMUNA", new Font("Arial", (float)4.5, FontStyle.Bold), brush, new RectangleF(79, (float)26.5, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void quality(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", 6, FontStyle.Bold), brush, new RectangleF(79, (float)37.75, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void QR(string value, PrintPageEventArgs e)
        {
            RectangleF qrCodeRect = new RectangleF(174, -2, 37, 37);
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(value, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeBitmap = qrCode.GetGraphic((int)qrCodeRect.Width, Color.Black, Color.White, true);
                e.Graphics.DrawImage(qrCodeBitmap, qrCodeRect);
            }
        }
        public static void labour(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("L: " + value, new Font("Arial", 7, FontStyle.Bold), brush, new RectangleF((float)113.5, 4, (float)56.5, 11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void wholeLabour(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("TL: " + value, new Font("Arial", 7, FontStyle.Bold), brush, new RectangleF((float)113.5, 4, (float)56.5, 11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void other(string value, PrintPageEventArgs e)
        { e.Graphics.DrawString("O: " + value, new Font("Arial", 7, FontStyle.Bold), brush, new RectangleF((float)113.5, 16, (float)56.5, 11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }); }
        public static void tagNumberFirstPart(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", 6, FontStyle.Bold), brush, new RectangleF(164, 32, 47, 7), new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
        }
        public static void tagNumberSecondPart(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", (float)6.25
                               , FontStyle.Bold), brush, new RectangleF(164, 42, 47, 7), new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
        }
        public static void tagNumberSingle(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", (float)6.25, FontStyle.Bold), brush, new RectangleF(164, 34, 47, 13), new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far });
        }
        public static void belowLabour1(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", 7, FontStyle.Bold), brush, new RectangleF((float)116.5, 29, (float)56.5, 10), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
        }
        public static void belowLabour2(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", 7, FontStyle.Bold), brush, new RectangleF((float)116.5, 38, (float)56.5, 12), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
        }
        public static void silverMainUpper(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", 12, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void silverSecondUpper(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", 10, FontStyle.Bold), brush, new RectangleF((float)113.5, 4, (float)56.5, 27), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void textBelowSilverMainUpper(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", 10, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        public static void textBelowSilverSecondUpper(string value, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(value, new Font("Arial", 10, FontStyle.Bold), brush, new RectangleF((float)113.5, 31, (float)56.5, 20), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
    }
}
