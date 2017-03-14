using Hidistro.Core;
using Hidistro.Core.Function;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Vshop_Img : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ProcessRequest();
    }

    public void ProcessRequest()
    {
        string item = Request.QueryString["id"];
        if (!string.IsNullOrEmpty(item))
        {
            Bitmap image = Code128Helper.Instance().GetCodeImage(Request.QueryString["id"] + "0", Code128Helper.Encode.Code128C);
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Png);
            Response.ClearContent();
            Response.ContentType = "image/png";
            Response.BinaryWrite(memoryStream.ToArray());
            memoryStream.Dispose();
            memoryStream.Dispose();
        }
        Response.Flush();
        Response.End();
    }
}