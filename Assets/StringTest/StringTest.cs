using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class StringTest : MonoBehaviour
{

    public static class StringBuilderUtil
    {
        public static void AppendInt(System.Text.StringBuilder sb, int value)
        {
            int n = value;
            if (n < 0)
            {
                sb.Append('-');
                n *= -1;
            }
            int k = 1;
            int m = n;
            while (m / 10 != 0)
            {
                m /= 10;
                k *= 10;
            }
            while (k != 0)
            {
                int d = n / k;
                n = n - d * k;
                k /= 10;
                char c = (char)('0' + d);
                sb.Append(c);
            }
        }
    }

    public static class IntToStringUtil
    {
        public static string Padding0(int n, int d)
        {
            char[] c = new char[n < 0 ? d + 1 : d];
            int s = 0;
            if (n < 0)
            {
                c[0] = '-';
                n *= -1;
                s = 1;
            }
            for (int i = d + s - 1; i >= s; i--)
            {
                if (n > 0)
                {
                    c[i] = (char)(n % 10 + '0');
                    n /= 10;
                }
                else
                {
                    c[i] = '0';
                }
            }
            return new string(c);
        }

        public static string UnsafePadding0(int n, int d)
        {
            unsafe
            {
                char* c = stackalloc char[11];
                int s = 0;
                if (n < 0)
                {
                    c[0] = '-';
                    n *= -1;
                    s = 1;
                }
                for (int i = d + s - 1; i >= s; i--)
                {
                    if (n > 0)
                    {
                        c[i] = (char)(n % 10 + '0');
                        n /= 10;
                    }
                    else
                    {
                        c[i] = '0';
                    }
                }
                return new string(c, 0, d + s);
            }
        }
    }

    void Int2StringFormatTest(int n, int d)
    {
        Debug.Log($"{IntToStringUtil.Padding0(n, d)} : {IntToStringUtil.UnsafePadding0(n, d)} : {n.ToString(new string('0', d))}");
    }

    void Start()
    {
        Int2StringFormatTest(-0, 2);
        Int2StringFormatTest(-100, 3);
        Int2StringFormatTest(-123, 4);
        Int2StringFormatTest(-123, 5);
        Int2StringFormatTest(123, 5);
        Int2StringFormatTest(123, 2);

        string s0 = "aabbccddeeff";
        string s1 = "aa,bb,ccddee";

        {
            Profiler.BeginSample("char[] 1");
            char[] txt = new char[1] { 'a' };
            Profiler.EndSample();

            // 28byte
            Profiler.BeginSample("string 1");
            string s = new string(txt);
            Profiler.EndSample();

            // 28byte
            Profiler.BeginSample("string 1 copy");
            string ss = string.Copy(s);
            Profiler.EndSample();
        }

        {
            char[] txt = new char[10];
            for (int i = 0; i < txt.Length; i++)
            {
                txt[i] = '0';
            }

            // 46byte
            Profiler.BeginSample("string 10");
            string s = new string(txt);
            Profiler.EndSample();
        }

        // 90byte
        Profiler.BeginSample("split 1");
        string[] array = s0.Split(',');
        Profiler.EndSample();

        // 48byte
        {
            Profiler.BeginSample("no split");
            if (s0.IndexOf(',') >= 0)
            {

            }
            else
            {
                array = new string[] { s0 };
            }
            Profiler.EndSample();
        }

        // 204byte
        {
            Profiler.BeginSample("split 3");
            array = s1.Split(',');
            Profiler.EndSample();
        }

        // 2.6KB
        {
            Profiler.BeginSample("split separator no cache");
            for (int i = 0; i < 10; i++)
            {
                array = s1.Split(',');
            }
            Profiler.EndSample();
        }

        // 2.3KB
        {
            Profiler.BeginSample("split separator cache");
            char[] sep = { ',' };
            for (int i = 0; i < 10; i++)
            {
                array = s1.Split(sep);
            }
            Profiler.EndSample();
        }

        string[] num = { "0", "1", "2", "3", "4", "5" };

        {
            Profiler.BeginSample("concat3");
            string s = num[0] + num[1] + num[2];
            Profiler.EndSample();
        }

        // GC.Alloc x1 34B
        {
            Profiler.BeginSample("concat +");
            string s = num[0] + num[1] + num[2] + num[3];
            Profiler.EndSample();
        }

        // GC.Alloc x3 96B
        {
            Profiler.BeginSample("concat +=");
            string s = num[0];
            s += num[1];
            s += num[2];
            s += num[3];
            Profiler.EndSample();
        }

        // 180B
        {

            Profiler.BeginSample("concat + 5");
            // 5個以上の場合、配列が生成される
            string s = num[0] + num[1] + num[2] + num[3] + num[4];
            Profiler.EndSample();
        }

        // 180B
        {
            Profiler.BeginSample("concat new array");
            string s = string.Concat(new string[] { num[0], num[1], num[2], num[3], num[4] });
            Profiler.EndSample();
        }

        // 118B
        {
            Profiler.BeginSample("concat array");
            // Concatは配列がコピーされる(.NET Framework)
            string s = string.Concat(num);
            Profiler.EndSample();
        }

        // 38B
        {
            Profiler.BeginSample("join empty");
            string s = string.Join("", num);
            Profiler.EndSample();
        }

        // 150byte
        {
            Profiler.BeginSample("StringBuilder");
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < num.Length; i++)
            {
                sb.Append(num[i]);
            }
            string s = sb.ToString();
            Profiler.EndSample();
        }

        // 32byte
        {
            Profiler.BeginSample("$3");
            // string.Concatが呼ばれる
            string s = $"{num[0]}{num[1]}{num[2]}";
            Profiler.EndSample();
        }

        // 32byte
        {
            Profiler.BeginSample("Format3");
            string s = string.Format("{0}{1}{2}", num[0], num[1], num[2]);
            Profiler.EndSample();
        }

        // 50byte
        {
            Profiler.BeginSample("replace string");
            string r = s1.Replace(",", ".");
            Profiler.EndSample();
        }

        // 50byte
        {
            Profiler.BeginSample("replace char");
            string r = s1.Replace(',', '.');
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("no replace");
            string r = s1.Replace('@', '.');
            r = s1.Replace("@", ".");
            Profiler.EndSample();
        }

        // 32byte
        {
            Profiler.BeginSample("int -> string (ToString)");
            int i0 = 100;
            string s = i0.ToString();
            Profiler.EndSample();
        }

        // 506byte
        {
            Profiler.BeginSample("int -> string (ToString) 2");
            int i0 = 100;
            string s = (i0).ToString("0000");
            Profiler.EndSample();
        }

        // 34byte
        {
            Profiler.BeginSample("int -> string (ToString) 3");
            int i0 = 100;
            string s = (i0).ToString("D4");
            Profiler.EndSample();
        }

        // 84byte
        {
            Profiler.BeginSample("int -> string (Format)");
            int i0 = 100;
            string s = string.Format("{0}", i0);
            Profiler.EndSample();
        }

        // 0.7KB
        {
            Profiler.BeginSample("int -> string (Format) 2");
            int i0 = 100;
            string s = string.Format("{0:0000}", i0);
            Profiler.EndSample();

            Debug.Log(s);
        }

        // 74byte
        {
            Profiler.BeginSample("int -> string (Custom)");
            int i0 = 100;
            string s = IntToStringUtil.Padding0(i0, 4);
            Profiler.EndSample();

            Debug.Log(s);
        }

        // 34byte
        {
            Profiler.BeginSample("int -> string (Custom Unsafe)");
            int i0 = 100;
            string s = IntToStringUtil.UnsafePadding0(i0, 4);
            Profiler.EndSample();

            Debug.Log(s);
        }

        // 30byte
        {
            Profiler.BeginSample("int -> string Hex (ToString)");
            int i0 = 100;
            string s = i0.ToString("X");
            Profiler.EndSample();

            Debug.Log(s);
        }

        // 220byte
        {
            Profiler.BeginSample("int -> string Hex (Format)");
            int i0 = 100;
            string s = string.Format("{0:X}", i0);
            Profiler.EndSample();

            Debug.Log(s);
        }

        // 84byte
        {
            Profiler.BeginSample("int -> string ($)");
            int i0 = 100;
            string s = $"{i0}";
            Profiler.EndSample();
        }

        // 28byte
        {
            Profiler.BeginSample("char -> string (ToString)");
            char c = 'a';
            string s = c.ToString();
            Profiler.EndSample();
        }

        // 28byte
        {
            Profiler.BeginSample("last char (SubString)");
            string s = s0.Substring(s0.Length - 1);
            Profiler.EndSample();
        }

        // 28byte
        {
            Profiler.BeginSample("last char (ToString)");
            string s = s0[s0.Length - 1].ToString();
            Profiler.EndSample();
        }

        // 48byte
        {
            Profiler.BeginSample("join");
            string s = string.Join(",", num);
            Profiler.EndSample();
        }

        // 88byte
        {
            List<string> list = new List<string>(num);
            Profiler.BeginSample("join List");
            string s = string.Join(",", list);
            Profiler.EndSample();
        }

        // 162byte
        {
            Profiler.BeginSample("StringBuilder join");
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < num.Length; i++)
            {
                sb.Append(num[0]);
                sb.Append(",");
            }
            string s = sb.ToString();
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("concat null");
            string s = s0 + null;
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("concat empty");
            string s = s0 + "";
            Profiler.EndSample();
        }

        // 88byte
        {
            Profiler.BeginSample("concat int");
            int i = 123;
            string s = s0 + i;
            Profiler.EndSample();
        }

        // 88byte
        {
            Profiler.BeginSample("concat int.ToString");
            int i = 123;
            string s = s0 + i.ToString();
            Profiler.EndSample();
        }

        // 108byte
        // concat(object)が呼ばれるのでiがbox化されている
        {
            Profiler.BeginSample("concat $ int");
            int i = 123;
            string s = $"{s0}{i}";
            Profiler.EndSample();
        }

        // 80byte
        {
            Profiler.BeginSample("concat char");
            char c = '0';
            string s = s0 + c;
            Profiler.EndSample();

            Debug.Log(s);
        }

        // 80byte
        {
            Profiler.BeginSample("concat char.ToString");
            char c = '0';
            string s = s0 + c.ToString();
            Profiler.EndSample();

            Debug.Log(s);
        }

        // 0byte
        {
            Profiler.BeginSample("String.ToString");
            string s = s0.ToString();
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("String.ToString");
            object o = s0;
            string s = o.ToString();
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("string ==");
            string a = "hoge";
            string b = "fuga";
            for (int i = 0; i < 1000; i++)
            {
                bool ret = a == b;
            }
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("string.CompareTo");
            string a = "hoge";
            string b = "fuga";
            for (int i = 0; i < 1000; i++)
            {
                bool ret = a.CompareTo(b) == 0;
            }
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("string.Compare");
            string a = "hoge";
            string b = "fuga";
            for (int i = 0; i < 1000; i++)
            {
                bool ret = string.Compare(a, b, System.StringComparison.Ordinal) == 0;
            }
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("IndexOf default");
            for (int i = 0; i < 1000; i++)
            {
                bool ret = s0.IndexOf("bbccddee") >= 0;
            }
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("IndexOf Ordinal");
            for (int i = 0; i < 1000; i++)
            {
                // Containsと同じ
                bool ret = s0.IndexOf("bbccddee", System.StringComparison.Ordinal) >= 0;
            }
            Profiler.EndSample();
        }

        // 0は同じリテラルを返す
        // 0byte
        {
            Profiler.BeginSample("int.ToString 0");

            int n = 0;
            string s = n.ToString();

            Profiler.EndSample();

            int m = 0;
            string ss = m.ToString();
            // true
            Debug.Log(object.ReferenceEquals(ss, s));
        }

        // 1は文字列を生成して返す(.NET Coreでは1桁の数値はリテラルを返す)
        // 28byte
        {
            Profiler.BeginSample("int.ToString 1");

            int n = 1;

            string s = n.ToString();

            Profiler.EndSample();

            int m = 1;
            string ss = m.ToString();
            // false
            Debug.Log(object.ReferenceEquals(ss, s));
        }

        int[] numbers =
        {
            0,
            1,
            12,
            123,
            -3,
            -32,
            -321
        };

        // .NET Frameworkの実装ではint.ToStringされる
        // 186byte
        {
            var sb = new System.Text.StringBuilder(100);
            Profiler.BeginSample("StringBuilder number 1");

            for (int i = 0; i < numbers.Length; i++)
            {
                sb.Append(numbers[i]);
            }

            Profiler.EndSample();
            string s = sb.ToString();
            Debug.Log(s);
        }

        // 186byte
        {
            var sb = new System.Text.StringBuilder(100);
            Profiler.BeginSample("StringBuilder number 2");

            for (int i = 0; i < numbers.Length; i++)
            {
                sb.Append(numbers[i].ToString());
            }

            Profiler.EndSample();
            string s = sb.ToString();
            Debug.Log(s);
        }

        // 0byte
        {
            var sb = new System.Text.StringBuilder(100);
            Profiler.BeginSample("StringBuilder number 3");

            for (int i = 0; i < numbers.Length; i++)
            {
                int n = numbers[i];
                StringBuilderUtil.AppendInt(sb, n);
            }

            Profiler.EndSample();
            string s = sb.ToString();
            Debug.Log(s);
        }

        // boolは文字列リテラルが使われる
        // 0byte
        {
            var sb = new System.Text.StringBuilder(100);
            Profiler.BeginSample("StringBuilder bool");
            bool t = true;
            bool f = false;
            for (int i = 0; i < 10; i++)
            {
                sb.Append(i % 2 == 0 ? t : f);
            }
            Profiler.EndSample();
            string s = sb.ToString();
            Debug.Log(s);
        }

        // 350byte
        {
            var sb = new System.Text.StringBuilder(100);
            Profiler.BeginSample("StringBuilder bool lower");
            bool t = true;
            bool f = false;
            for (int i = 0; i < 10; i++)
            {
                bool b = i % 2 == 0 ? t : f;
                sb.Append(b.ToString().ToLower());
            }
            Profiler.EndSample();
            string s = sb.ToString();
            Debug.Log(s);
        }

        // 0byte
        {
            var sb = new System.Text.StringBuilder(100);
            Profiler.BeginSample("StringBuilder bool lower literal");
            for (int i = 0; i < 10; i++)
            {
                string b = i % 2 == 0 ? "true" : "false";
                sb.Append(b);
            }
            Profiler.EndSample();
            string s = sb.ToString();
            Debug.Log(s);
        }

        // 0byte
        {
            var sb = new System.Text.StringBuilder();
            Profiler.BeginSample("StringBuilder AppendFormat");
            string msg = "hoge";
            sb.AppendFormat("huga_{0}", msg);
            Profiler.EndSample();
            string s = sb.ToString();
            Debug.Log(s);
        }

        // 44byte
        {
            var sb = new System.Text.StringBuilder();
            Profiler.BeginSample("StringBuilder Append $");
            string msg = "hoge";
            sb.Append($"huga_{msg}");
            Profiler.EndSample();
            string s = sb.ToString();
            Debug.Log(s);
        }

        // 3.9KB
        {
            Profiler.BeginSample("ToUpper");
            string text = "abcdefg";

            for (int i = 0; i < 100; i++)
            {
                string upper = text.ToUpper();
            }

            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("int + string");
            string text = "abcdefg";
            int n = 99;

            string s = n + text;

            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("string + int");
            string text = "abcdefg";
            int n = 99;

            string s = text + n;

            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("string.StartsWith");
            string text = "abcdefg";

            for (int i = 0; i < 10000; i++)
            {
                bool ret = text.StartsWith("abc");
            }

            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("string.StartsWith ordinal");
            string text = "abcdefg";

            for (int i = 0; i < 10000; i++)
            {
                bool ret = text.StartsWith("abc", System.StringComparison.Ordinal);
            }

            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("string.StartsWith char");
            string text = "abcdefg";

            for (int i = 0; i < 10000; i++)
            {
                bool ret = text.StartsWith('a');
            }
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("string.StartsWith char 2");
            string text = "abcdefg";

            for (int i = 0; i < 10000; i++)
            {
                bool ret = text.StartsWith2('a');
            }
            Profiler.EndSample();
        }
    }
}

public static class StringUtil
{
    public static bool StartsWith(this string s, char c)
    {
        if (s == null) return false;
        return s.IndexOf(c) == 0;
    }

    public static bool StartsWith2(this string s, char c)
    {
        if (s == null) return false;
        return s.Length > 0 && s[0] == c;
    }
}
