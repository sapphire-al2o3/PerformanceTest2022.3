using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System.Linq;

public enum EnumType
{
    A,
    B,
    C,
    D,
    E
}

public class EnumTypeComparer : IEqualityComparer<EnumType>
{
    public bool Equals(EnumType x, EnumType y)
    {
        return x == y;
    }

    public int GetHashCode(EnumType obj)
    {
        return (int)obj;
    }
}

public struct ReadOnlyDictionaryWrapper<TKey, TValue>
{
    Dictionary<TKey, TValue> _dict;

    public ReadOnlyDictionaryWrapper(Dictionary<TKey, TValue> dict)
    {
        _dict = dict;
    }

    public TValue this[TKey key] => _dict[key];

    public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
    {
        return _dict.GetEnumerator();
    }
}

public class DictionaryTest : MonoBehaviour
{

    void Start()
    {
        Dictionary<int, int> dic = new Dictionary<int, int>();

        for (int i = 0; i < 10000; i++)
        {
            dic.Add(i, i);
        }

        int sum = 0;

        // 120byte
        {
            Profiler.BeginSample("values");
            foreach (var e in dic.Values)
            {
                sum += e;
            }
            Profiler.EndSample();
        }

        // 120byte
        {
            // 72byte
            Profiler.BeginSample("keys get");
            // new KeyCollection (24byte)
            Dictionary<int, int>.KeyCollection keys = dic.Keys;
            Profiler.EndSample();

            // 48byte
            Profiler.BeginSample("keys foreach");
            foreach (int k in keys)
            {
                sum += dic[k];
            }
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("keys 2");
            foreach (var k in dic.Keys)
            {
                sum += dic[k];
            }
            Profiler.EndSample();
        }

        // 96byte
        {
            Profiler.BeginSample("pair");
            foreach (var pair in dic)
            {
                sum += pair.Value;
            }
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("pair 2");
            foreach (var pair in dic)
            {
                sum += pair.Value;
            }
            Profiler.EndSample();
        }

        // pairでキャッシュが生成されているため0byte
        Profiler.BeginSample("enemurator");
        using (var itr = dic.GetEnumerator())
        {
            while (itr.MoveNext())
            {
                sum += itr.Current.Value;
            }
        }
        Profiler.EndSample();

        // 80B
        var comp = new EnumTypeComparer();
        Dictionary<EnumType, int> dic2 = new Dictionary<EnumType, int>();
        Profiler.BeginSample("dictionary<EnumType, int>");
        dic2 = new Dictionary<EnumType, int>();
        Profiler.EndSample();

        // 172B
        Profiler.BeginSample("dictionary<EnumType, int>.set_item");
        for (int i = 0; i < 100; i++)
        {
            dic2[EnumType.A] = 0;
            dic2[EnumType.B] = 1;
            dic2[EnumType.C] = 2;
        }
        Profiler.EndSample();

        // 0B
        {
            Profiler.BeginSample("dictionary<EnumType, int>.get_item");
            for (int i = 0; i < 100; i++)
            {
                int dic2_0 = dic2[EnumType.A];
                dic2_0 += dic2[EnumType.B];
                dic2_0 += dic2[EnumType.C];
            }
            Profiler.EndSample();
        }

        // 0B
        {
            Profiler.BeginSample("dictionary<EnumType, int>.ContainsKey");
            for (int i = 0; i < 100; i++)
            {
                if (dic2.ContainsKey(EnumType.A))
                {

                }
            }
            Profiler.EndSample();
        }

        // 96B
        {
            Profiler.BeginSample("dictionary<EnumType, int> foreach");
            for (int i = 0; i < 100; i++)
            {
                foreach (var pair in dic2)
                {
                    EnumType k = pair.Key;
                    int v = pair.Value;
                }
            }
            Profiler.EndSample();
        }

        // 80byte
        Dictionary<int, int> dic1 = new Dictionary<int, int>();
        Profiler.BeginSample("dictionary<int, int>");
        dic1 = new Dictionary<int, int>();
        Profiler.EndSample();

        // 0.8KB
        {
            Profiler.BeginSample("dictionary<string, int> x10");
            for (int i = 0; i < 10; i++)
            {
                var t = new Dictionary<string, int>();
            }
            Profiler.EndSample();
        }

        // 2.2KB
        {
            Profiler.BeginSample("dictionary<string, int>(1) x10");
            for (int i = 0; i < 10; i++)
            {
                // 80
                var t = new Dictionary<string, int>();
                // 44 + 104
                t.Add("a", i);
            }
            Profiler.EndSample();
        }

        Profiler.BeginSample("dictionary<int, int>.set_item");
        for (int i = 0; i < 100; i++)
        {
            dic1[0] = 0;
            dic1[1] = 1;
            dic1[2] = 2;
        }
        Profiler.EndSample();

        {
            Profiler.BeginSample("dictionary<int, int>.get_item");
            for (int i = 0; i < 100; i++)
            {
                int dic1_0 = dic1[0];
                dic1_0 += dic1[1];
                dic1_0 += dic1[2];
            }
            Profiler.EndSample();
        }

        // 3.1KB
        {
            Profiler.BeginSample("dictionary<string, bool>(100)");
            var d = new Dictionary<string, bool>(100);
            Profiler.EndSample();
        }

        // 197.5KB
        {
            Profiler.BeginSample("dictionary<int, int>(10000)");
            dic1 = new Dictionary<int, int>(10000);
            Profiler.EndSample();
        }

        // 80KB
        Profiler.BeginSample("dictionary<int, string>");
        Dictionary<int, string> dic3 = new Dictionary<int, string>();
        Profiler.EndSample();

        // 80B
        Profiler.BeginSample("dictionary<string, int>");
        var dicsi = new Dictionary<string, int>();
        Profiler.EndSample();

        // 80B
        Profiler.BeginSample("dictionary<string, string>");
        Dictionary<string, string> dic4 = new Dictionary<string, string>();
        Profiler.EndSample();

        // 276.4KB
        Profiler.BeginSample("dictionary<string, string>(10000)");
        dic4 = new Dictionary<string, string>(10000);
        Profiler.EndSample();

        // 39.1KB
        Profiler.BeginSample("reset dictionary (ToArray)");
        foreach (var key in dic.Keys.ToArray())
        {
            dic[key] = 0;
        }
        Profiler.EndSample();

        // 39.1KB
        Profiler.BeginSample("reset dictionary (new List)");
        foreach (var key in new List<int>(dic.Keys))
        {
            dic[key] = 0;
        }
        Profiler.EndSample();

        // 39.1KB
        Profiler.BeginSample("reset dictionary (ToList)");
        foreach (var key in dic.Keys.ToList())
        {
            dic[key] = 0;
        }
        Profiler.EndSample();

        Profiler.BeginSample("reset dictionary (values)");

        Profiler.EndSample();

        // 64B
        {
            Profiler.BeginSample("HashSet<string>");
            var hashSet = new HashSet<string>();
            Profiler.EndSample();
        }

        // 7.3KB
        {
            string[] items = new string[100];
            for (int i = 0; i < 100; i++)
            {
                items[i] = i.ToString();
            }
            Profiler.BeginSample("HashSet<string> add 100");
            var hashSet = new HashSet<string>();
            for (int i = 0; i < 100; i++)
            {
                hashSet.Add(items[i]);
            }
            Profiler.EndSample();
        }

        // 2.2KB
        {
            string[] items = new string[100];
            for (int i = 0; i < 100; i++)
            {
                items[i] = i.ToString();
            }
            Profiler.BeginSample("HashSet<string> 100");
            var hashSet = new HashSet<string>(items);
            Profiler.EndSample();
        }

        // 96B
        {
            Profiler.BeginSample("ReadOnlyDictionary");
            var rodic = new System.Collections.ObjectModel.ReadOnlyDictionary<int, int>(dic);
            foreach (var e in rodic)
            {
                sum += e.Value;
            }
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("ReadOnlyDictionary 2");
            var rodic = new System.Collections.ObjectModel.ReadOnlyDictionary<int, int>(dic);
            foreach (var e in rodic)
            {
                sum += e.Value;
            }
            Profiler.EndSample();
        }

        // 480Byte
        {
            Profiler.BeginSample("IReadOnlyDictionary");
            for (int i = 0; i < 10; i++)
            {
                IReadOnlyDictionary<int, int> rodic = dic;
                foreach (var e in rodic)
                {
                    sum += e.Value;
                }
            }
            Profiler.EndSample();
        }

        // 48byte
        {
            Profiler.BeginSample("ReadOnlyDictionaryWrapper");
            for (int i = 0; i < 10; i++)
            {
                var rodic = new ReadOnlyDictionaryWrapper<int, int>(dic);
                foreach (var e in rodic)
                {
                    sum += e.Value;
                }
            }
            Profiler.EndSample();
        }

        // 480byte
        {
            Profiler.BeginSample("IDictionary<int, int>");
            for (int i = 0; i < 10; i++)
            {
                IDictionary<int, int> rodic = dic;
                foreach (var e in rodic)
                {
                    sum += e.Value;
                }
            }
            Profiler.EndSample();
        }

        for (int i = 0; i < 100; i++)
        {
            dic4.Add(i.ToString(), i.ToString());
        }

        // おそらくKeyValuePairがボックス化される
        // 3.2KB
        {
            Profiler.BeginSample("IDictionary foreach");
            IDictionary idic = dic4;
            foreach (var e in idic)
            {

            }
            Profiler.EndSample();
        }

        // 0B
        {
            Profiler.BeginSample("Dictionary get_item");
            for (int i = 0; i < 10000; i++)
            {
                if (dic4.ContainsKey("99"))
                {
                    var s = dic4["99"];
                }
            }
            Profiler.EndSample();
        }

        // 0B
        {
            Profiler.BeginSample("IDictionary get_item");
            IDictionary idic = dic4;
            for (int i = 0; i < 10000; i++)
            {
                if (idic.Contains("99"))
                {
                    var s = idic["99"];
                }
            }
            Profiler.EndSample();
        }

        {
            SortedDictionary<string, string> sortedDcit = new SortedDictionary<string, string>();
            sortedDcit.Add("a", "a");
            sortedDcit.Add("b", "b");
            sortedDcit.Add("c", "c");

            foreach (var pair in sortedDcit)
            {
            }

            // 120byte
            Profiler.BeginSample("SortedDictionary foreach");
            foreach (var pair in sortedDcit)
            {
            }
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("Dictionary<string, int>.ctor");
            Dictionary<string, int> d = new Dictionary<string, int>(10000);
            Profiler.EndSample();
            for (int i = 0; i < 1000; i++)
            {
                d.Add(i.ToString(), i);
            }

            Profiler.BeginSample("Dictionary<string, int>.get");
            for (int i = 0; i < 10000; i++)
            {
                string k = "500";
                int v = d[k];
                v += d[k];
            }
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("Dictionary<object, int>.ctor");
            Dictionary<object, int> d = new Dictionary<object, int>(10000);
            Profiler.EndSample();
            for (int i = 0; i < 1000; i++)
            {
                d.Add(i.ToString(), i);
            }

            Profiler.BeginSample("Dictionary<object, int>.get");
            for (int i = 0; i < 10000; i++)
            {
                string k = "500";
                int v = d[k];
                v += d[k];
            }
            Profiler.EndSample();
        }

        // capacity = 4でも実際には7取られている
        // 0B
        {
            Dictionary<int, int> d = new Dictionary<int, int>(4);
            Profiler.BeginSample("capacity 4");
            for (int i = 0; i < 7; i++)
            {
                d.Add(i, i);
            }
            Profiler.EndSample();
            Profiler.BeginSample("capacity 4 -> 8");
            d.Add(7, 7);
            Profiler.EndSample();
        }

        // 66.0KB
        {
            Profiler.BeginSample("capacity 3000");
            Dictionary<int, int> d = new Dictionary<int, int>(3000);
            Profiler.EndSample();
        }

        // 59.7KB
        {
            Profiler.BeginSample("capacity 2801 + 239");
            Dictionary<int, int> d = new Dictionary<int, int>(2801);
            Dictionary<int, int> d2 = new Dictionary<int, int>(239);
            Profiler.EndSample();
        }
    }
}
