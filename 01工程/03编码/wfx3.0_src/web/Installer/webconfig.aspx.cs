using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Installer_CustomWebCofing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //得到当前配置文件
        Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
        //得到节部分
        ConfigurationSection section = config.GetSection("connectionStrings");
        //如果节不为空并且这个节没被保护
        if (section != null && !section.SectionInformation.IsProtected)
        {
            //保护指定的节使用RSA加密算法对数据进行加密和解密
            //section.SectionInformation.ProtectSection("RSAProtectedConfigurationProvider");
            section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");

            //保存
            config.Save();

            RegisterStartupScript("", "<script>alert('加密成功！')</script>");
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        //得到当前配置文件
        Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
        //得到节部分
        ConfigurationSection section = config.GetSection("connectionStrings");
        //如果节不为空并且这个节被保护
        if (section != null && section.SectionInformation.IsProtected)
        {
            //保护指定的节使用RSA加密算法对数据进行加密和解密
            section.SectionInformation.UnprotectSection();
            //保存
            config.Save();
            RegisterStartupScript("", "<script>alert('解密成功！')</script>");
        }
    }
}