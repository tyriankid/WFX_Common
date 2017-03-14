using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

namespace Hidistro.Entities
{
    /// <summary>
    /// 文件常用类【通用处理类】
    /// 最后更新 JHB: ON 2012-11-29
    /// </summary>
    public class FilePlus
    {
        /// <summary>
        /// 从服务器上下载文件
        /// </summary>
        /// <param name="path">服务器文件绝对路径</param>
        public static void DownFile(Page page, string path)
        {

                System.IO.FileInfo myFile = new System.IO.FileInfo(path);
                page.Response.Clear();
                page.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(myFile.Name));
                page.Response.AddHeader("Content-Length", myFile.Length.ToString());
                page.Response.ContentType = "application/octet-stream";
                page.Response.TransmitFile(myFile.FullName);
                page.Response.End();
            
            //catch(Exception ex)
            //{
            //    page.Response.Write("<script language='javascript'>alert('下载文件时发生错误！可能是文件不存在或者被管理员删除。');window.close();</script>");
            //}
        }

        /// <summary>
        /// 删除某临时目录下的临时文件
        /// </summary>
        /// <param name="strPath">待删除文件所在的临时目录</param>
        /// <param name="minutes">多少分钟前？</param>
        public static void DeleteFileEx(string strPath, int minutes)
        {
            string[] files = Directory.GetFiles(strPath);
            for (int i = 0; i < files.Length; i++)
            {
                DateTime time = File.GetLastAccessTime(files[i]);
                if (time.AddMinutes(minutes) < DateTime.Now)
                {
                    try
                    {
                        File.Delete(files[i]);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }
    }
}
