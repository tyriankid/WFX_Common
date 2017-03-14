namespace Hidistro.Membership.Context
{
    using Hidistro.Core;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Caching;
    using System.Xml;

    public sealed class CopyrightLicenser
    {
        public const string CacheCopyrightKey = "Hishop_SiteLicense";

        private CopyrightLicenser()
        {
        }

        public static bool CheckCopyright()
        {
            HiContext current = HiContext.Current;
            XmlDocument document = HiCache.Get("Hishop_SiteLicense") as XmlDocument;
            if (document == null)
            {
                string path = null;
                if (current.Context != null)
                {
                    path = current.Context.Request.MapPath("~/config/Hishop.lic");
                }
                else
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Hishop.lic");
                }
                if (!File.Exists(path))
                {
                    return false;
                }
                document = new XmlDocument();
                document.LoadXml(File.ReadAllText(path));
                HiCache.Max("Hishop_SiteLicense", document, new CacheDependency(path));
            }
            XmlNode node = document.DocumentElement.SelectSingleNode("//Host");
            XmlNode node2 = document.DocumentElement.SelectSingleNode("//LicenseDate");
            XmlNode node3 = document.DocumentElement.SelectSingleNode("//ExpiresDate");
            XmlNode node4 = document.DocumentElement.SelectSingleNode("//Signature");
            if (string.Compare(node.InnerText, current.SiteSettings.SiteUrl, true, CultureInfo.InvariantCulture) != 0)
            {
                return false;
            }
            string s = string.Format(CultureInfo.InvariantCulture, "Host={0}&LicenseDate={1}&ExpiresDate={2}&Key={3}", new object[] { current.SiteSettings.SiteUrl, node2.InnerText, node3.InnerText, Hidistro.Membership.Context.LicenseHelper.GetSiteHash() });
            bool flag = false;
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(Hidistro.Membership.Context.LicenseHelper.GetPublicKey());
                RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(provider);
                deformatter.SetHashAlgorithm("SHA1");
                byte[] rgbSignature = Convert.FromBase64String(node4.InnerText);
                byte[] rgbHash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(s));
                flag = deformatter.VerifySignature(rgbHash, rgbSignature);
            }
            return (flag && (DateTime.Now < DateTime.Parse(node3.InnerText)));
        }
    }
}

