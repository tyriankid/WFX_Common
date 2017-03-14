<%@ WebHandler Language="c#" Class="File_WebHandler" Debug="true" %>

using System;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

public class File_WebHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string action = context.Request["action"];

        switch (action)
        {
            case "LogoUpload":
                LogoUpload(context);
                break;
            case "BakUpload":
                BakUpload(context);
                break;
            default:
                break;
        }




    }
    public void LogoUpload(HttpContext context)
    {
        HttpFileCollection files = context.Request.Files;
        if (files.Count > 0)
        {
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];

                if (file.ContentLength > 0)
                {
                    string imageUrl = string.Empty;
                    if (file != null && !string.IsNullOrEmpty(file.FileName))
                    {
                        string uploadPath = Hidistro.Core.Globals.GetStoragePath() + "/Logo";
                        string newFilename = Guid.NewGuid().ToString("N", System.Globalization.CultureInfo.InvariantCulture) +
                                             Path.GetExtension(file.FileName);
                        //��ǰ���ϴ��ķ����·��
                        imageUrl = uploadPath + "/" + newFilename;
                        //��ǰ�ļ���׺��
                        string ext = Path.GetExtension(file.FileName).ToLower();
                        //��֤�ļ������Ƿ���ȷgif��jpeg��jpg��bmp��png
                        if (!ext.Equals(".gif") && !ext.Equals(".jpg") && !ext.Equals(".jpeg") && !ext.Equals(".png") && !ext.Equals(".bmp"))
                        {
                            //����window.parent.uploadSuccess()������ǰ��ҳ����д�õ�javascript function,�˷�����Ҫ��������쳣���ϴ��ɹ����ͼƬ��ַ
                            context.Response.Write("2");
                            context.Response.End();
                        }
                        //��֤�ļ��Ĵ�С
                        if (file.ContentLength > 2097152)
                        {
                            //����window.parent.uploadSuccess()������ǰ��ҳ����д�õ�javascript function,�˷�����Ҫ��������쳣���ϴ��ɹ����ͼƬ��ַ 
                            context.Response.Write("1");
                            context.Response.End();
                        }
                        //��ʼ�ϴ�
                        file.SaveAs(context.Request.MapPath(imageUrl));
                        context.Response.Write(imageUrl);
                        context.Response.End();
                    }
                    else
                    {
                        //�ϴ�ʧ��
                        context.Response.Write("0");
                        context.Response.End();
                    }
                }
            }
        }
        else
        {
            //û��ѡ��ͼƬ 
            context.Response.Write("3");
            context.Response.End();
        }
    }
    public void BakUpload(HttpContext context)
    {
        HttpFileCollection files = context.Request.Files;
        string imageurl = "";
        if (files.Count > 0)
        {

            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];

                if (file.ContentLength > 0)
                {
                    string imageUrl = string.Empty;
                    string imageUrlOld = string.Empty;
                    if (file != null && !string.IsNullOrEmpty(file.FileName))
                    {
                        string uploadPath = "/Storage/data/DistributorBackgroundPic";
                        string newFilename = Guid.NewGuid().ToString("N", System.Globalization.CultureInfo.InvariantCulture) +
                                             Path.GetExtension(file.FileName);
                        string newFilenameOld = Guid.NewGuid().ToString("N", System.Globalization.CultureInfo.InvariantCulture) +"2"+
                                             Path.GetExtension(file.FileName);
                        
                        //��ǰ���ϴ��ķ����·��
                        imageUrlOld = uploadPath + "/" + newFilenameOld;
                        imageUrl = uploadPath + "/" + newFilename;
                        //��ǰ�ļ���׺��
                        string ext = Path.GetExtension(file.FileName).ToLower();
                        //��֤�ļ������Ƿ���ȷ
                        if (!ext.Equals(".gif") && !ext.Equals(".jpg") && !ext.Equals(".png") && !ext.Equals(".bmp"))
                        {
                            //����window.parent.uploadSuccess()������ǰ��ҳ����д�õ�javascript function,�˷�����Ҫ��������쳣���ϴ��ɹ����ͼƬ��ַ
                            context.Response.Write("2");
                            context.Response.End();
                        }
                        //��֤�ļ��Ĵ�С
                        if (file.ContentLength > 4048576)
                        {
                            //����window.parent.uploadSuccess()������ǰ��ҳ����д�õ�javascript function,�˷�����Ҫ��������쳣���ϴ��ɹ����ͼƬ��ַ 
                            context.Response.Write("1");
                            context.Response.End();
                        }
                        //��ʼ�ϴ�
                        file.SaveAs(context.Request.MapPath(imageUrlOld));
                        BuildMin(context.Request.MapPath(imageUrlOld), context.Request.MapPath(imageUrl),650,390);
                        imageurl += imageUrl + "|";
                        
                    }
                    else
                    {
                        //�ϴ�ʧ��
                        context.Response.Write("0");
                        context.Response.End();
                    }
                }
            }
            if(!String.IsNullOrEmpty(imageurl))
            {
                context.Response.Write(imageurl);
                context.Response.End();
            }
        }
        else
        {
            //û��ѡ��ͼƬ 
            context.Response.Write("3");
            context.Response.End();
        }
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// ��������ͼ
    /// </summary>
    /// <param name="strOldPic">ԭʼͼƬ���Ե�ַ</param>
    /// <param name="strNewPic">����ͼͼƬ���Ե�ַ</param>
    /// <param name="intHeight">����ͼ���</param>
    /// <param name="intHeight">����ͼ�߶�</param>
    public void BuildMin(string strOldPic, string strNewPic, int intWidth, int intHeight)
    {
        System.Drawing.Bitmap objPic, objNewPic;
        try
        {
            objPic = new System.Drawing.Bitmap(strOldPic);
            if (objPic.Height >= objPic.Width && objPic.Height >= intHeight)
            {
                intWidth = intHeight * objPic.Width / objPic.Height;
                objNewPic = new System.Drawing.Bitmap(objPic, intWidth, intHeight);
                objNewPic.Save(strNewPic);
                objPic.Dispose();
                objNewPic.Dispose();
            }
            else if (objPic.Height <= objPic.Width && objPic.Width > intWidth)
            {
                intHeight = intWidth * objPic.Height / objPic.Width;
                objNewPic = new System.Drawing.Bitmap(objPic, intWidth, intHeight);
                objNewPic.Save(strNewPic);
                objPic.Dispose();
                objNewPic.Dispose();
            }
            else
            {
                objPic.Save(strNewPic);
                objPic.Dispose();
            }
        }
        catch { }
    }
    
    
}