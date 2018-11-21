using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public class StringUtil
    {
       public static bool isEmpty(string str) {
		if(str == null) 
			return true; 
		string tempStr = str.Trim(); 
		if(tempStr.Length == 0)
			return true; 
		if(tempStr == "null")
			return true;
		return false; 
	}
	
	/**
	 * 把数组所有元素排序，并按照“参数=参数值”的模式用“&”字符拼接成字符串
	 * 
	 * @param paras
	 *            需要排序并参与字符拼接的参数组
	 * @return 拼接后字符串
	 */
	public static string createLinkString(Dictionary<string, string> paras) {
		
		List<string> keys = new List<string>(paras.Keys);
		keys.Sort();
		string prestr = "";
		for (int i = 0; i < keys.Count; i++) {
			string key = keys[i];
			string value = paras[key];
			if (i == keys.Count - 1) {// 拼接时，不包括最后一个&字符
				prestr = prestr + key + "=" + value;
			} else {
				prestr = prestr + key + "=" + value + "&";
			}
		}
		return prestr;
	}
    }
}
