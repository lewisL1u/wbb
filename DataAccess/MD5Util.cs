using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;  

namespace DataAccess
{
    public class MD5Util
    {
        
	/**
	 * 生成签名结果
	 * 
	 * @param sArray
	 *            要签名的数组
	 * @return 签名结果字符串
	 */
	public static string buildMysignV1(Dictionary<string, string> sArray, string secretKey) {
		string mysign = "";
		try {
			string prestr =  StringUtil.createLinkString(sArray); // 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
			prestr = prestr + "&secret_key=" + secretKey; // 把拼接后的字符串再与安全校验码连接起来
			mysign = getMD5String(prestr);
		} catch (Exception e) {
            return e.Message;
		}
		return mysign;
	}

	/**
	 * 生成32位大写MD5值
	 */
	private static char[] HEX_DIGITS = new char[]{ '0', '1', '2', '3', '4', '5',
			'6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

	public static string getMD5String(string str) {
		try {
			if (str == null || str.Trim().Length == 0) {
				return "";
			}
			byte[] result = Encoding.Default.GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(result);  
			
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < bytes.Length; i++) {
				sb.Append(HEX_DIGITS[(bytes[i] & 0xf0) >> 4] + "" + HEX_DIGITS[bytes[i] & 0xf]);
			}
            return sb.ToString();
		} catch (Exception e) {
            return e.Message;
		}
		return "";
	}
    }
}
