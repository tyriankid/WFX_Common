using System;
using System.Text.RegularExpressions;
namespace Hidistro.Core
{
	public static class ObjectExtends
	{
		public static string ToNullString(this object obj)
		{
			string arg_1F_0;
			if (obj == null)
			{
				if (obj != DBNull.Value)
				{
					arg_1F_0 = string.Empty;
					return arg_1F_0;
				}
			}
			arg_1F_0 = obj.ToString().Trim();
			return arg_1F_0;
		}
		public static int ToInt(this object obj)
		{
			string s = obj.ToNullString();
			int result = 0;
			int.TryParse(s, out result);
			return result;
		}
		public static bool ToBool(this object obj)
		{
			bool result = false;
			string value = obj.ToNullString();
			bool.TryParse(value, out result);
			return result;
		}
		public static decimal ToDecimal(this object obj)
		{
			string s = obj.ToNullString();
			decimal result = 0m;
			decimal.TryParse(s, out result);
			return result;
		}
		public static DateTime? ToDateTime(this object obj)
		{
			string s = obj.ToNullString();
			DateTime minValue = DateTime.MinValue;
			DateTime.TryParse(s, out minValue);
			DateTime? result;
			if (minValue == DateTime.MinValue)
			{
				result = null;
			}
			else
			{
				result = new DateTime?(minValue);
			}
			return result;
		}
		public static bool IsDecimal(this object obj)
		{
			decimal num = 0m;
			return decimal.TryParse(obj.ToNullString(), out num);
		}
		public static bool IsInt(this object obj)
		{
			int num = 0;
			return int.TryParse(obj.ToNullString(), out num);
		}
		public static bool IsPositiveInteger(this object obj)
		{
			return Regex.IsMatch(obj.ToNullString(), "[0-9]\\d*");
		}
	}
}
