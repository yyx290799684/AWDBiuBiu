using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AWDBiuBiu.Util
{
    public class Utils
    {
        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }

        public static KeyValuePair<int, int> BuildStartAndEnd(string str)
        {
            int start = -1;
            int end = -1;
            if (str.IndexOf('-') == -1)
            {
                if (IsInt(str))
                    start = end = int.Parse(str);
            }
            else
            {
                var ars = str.Split('-');
                if (IsInt(ars[0]))
                    start = int.Parse(ars[0]);

                if (IsInt(ars[1]))
                    end = int.Parse(ars[1]);
            }
            return new KeyValuePair<int, int>(start, end);
        }

        public static List<KeyValuePair<string, string>> BuildKeyValuePairList(string str, char sp1, char sp2)
        {
            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
            if (string.IsNullOrEmpty(str))
            {
                return keyValuePairs;
            }
            try
            {
                var pars = str.Split(sp1);
                foreach (var item in pars)
                {
                    var kvs = item.Split(sp2);
                    if (item.EndsWith(sp2.ToString()))
                    {
                        keyValuePairs.Add(new KeyValuePair<string, string>(kvs[0].Trim(), string.Empty));
                    }
                    else
                    {
                        keyValuePairs.Add(new KeyValuePair<string, string>(kvs[0].Trim(), item.Replace(kvs[0].Trim() + sp2, string.Empty)));
                    }
                }
            }
            catch (Exception)
            {
            }
            return keyValuePairs;
        }

        public static string DoRegex(string ret, string reg)
        {
            var regex = Regex.Match(ret + "aaaaa", reg);
            if (regex.Success)
            {
                return regex.Value;
            }
            return string.Empty;
        }
    }
}
