
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace Hidistro.UI.Web.API
{
	public class GetQRCode : IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}


		public static Image CombinImage(Image imgBack, Image img)
		{
			if (img.Height != 65 || img.Width != 65)
			{
				img = GetQRCode.KiResizeImage(img, 250, 250, 0);
			}
			Graphics graphic = Graphics.FromImage(imgBack);
			graphic.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);
			graphic.DrawImage(img, imgBack.Width / 2 - img.Width / 2 + 10, imgBack.Width / 2 - img.Width / 2 + 85, 136, 136);
			GC.Collect();
			return imgBack;
		}

		public static Image KiResizeImage(Image bmp, int newW, int newH, int Mode)
		{
			Image image;
			try
			{
				Image bitmap = new Bitmap(newW, newH);
				Graphics graphic = Graphics.FromImage(bitmap);
				graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphic.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
				graphic.Dispose();
				image = bitmap;
			}
			catch
			{
				image = null;
			}
			return image;
		}

		public void ProcessRequest(HttpContext context)
		{
            string item = context.Request["code"];
			if (!string.IsNullOrEmpty(item))
			{
				QRCodeEncoder qRCodeEncoder = new QRCodeEncoder()
				{
					QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
					QRCodeScale = 4,
					QRCodeVersion = 8,
					QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M
				};
				Image image = qRCodeEncoder.Encode(item);
				MemoryStream memoryStream = new MemoryStream();
				image.Save(memoryStream, ImageFormat.Png);
                string str = context.Server.MapPath("/Storage/master/QRcord.jpg");
				Image image1 = Image.FromFile(str);
				MemoryStream memoryStream1 = new MemoryStream();
				GetQRCode.CombinImage(image1, image).Save(memoryStream1, ImageFormat.Png);
				context.Response.ClearContent();
                context.Response.ContentType = "image/png";
				context.Response.BinaryWrite(memoryStream1.ToArray());
				memoryStream.Dispose();
				memoryStream1.Dispose();
			}
			context.Response.Flush();
			context.Response.End();
		}
	}
}