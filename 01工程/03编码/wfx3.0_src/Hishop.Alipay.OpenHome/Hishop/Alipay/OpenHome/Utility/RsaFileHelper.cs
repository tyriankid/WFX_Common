namespace Hishop.Alipay.OpenHome.Utility
{
    using System;
    using System.IO;

    internal class RsaFileHelper
    {
        public static string GetRSAKeyContent(string path, bool isPubKey)
        {
            string str = string.Empty;
            string str2 = isPubKey ? "PUBLIC KEY" : "RSA PRIVATE KEY";
            using (StreamReader reader = new StreamReader(path))
            {
                str = reader.ReadToEnd();
                reader.Close();
            }
            string str3 = string.Format(@"-----BEGIN {0}-----\n", str2);
            string str4 = string.Format("-----END {0}-----", str2);
            int startIndex = str.IndexOf(str3) + str3.Length;
            int index = str.IndexOf(str4, startIndex);
            return str.Substring(startIndex, index - startIndex).Replace("\r", "").Replace("\n", "");
        }
    }
}

