using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.Profiling;

public class ArrayTest : MonoBehaviour
{
    struct CompStruct : IComparer<int>
    {
        public int Compare(int x, int y) => x - y;
    }

    class DefaultInitCollection : ICollection<int>
    {
        int _size = 0;
        public DefaultInitCollection(int size) => _size = size;

        public int Count => 1000;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(int item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(int item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(int[] array, int arrayIndex)
        {
        }

        public IEnumerator<int> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(int item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    List<T> CreateEmptyList<T>(int n)
    {
        var list = new List<T>(n);
        for (int i = 0; i < n; i++)
        {
            list.Add(default(T));
        }
        return list;
    }

    void Reverse<T>(T[] array)
    {
        int l = array.Length;
        for (int i = 0; i < l / 2; i++)
        {
            var t = array[i];
            array[i] = array[l - i - 1];
            array[l - i - 1] = t;
        }
    }

    void Start()
    {
        // 32 byte
        Profiler.BeginSample("int[0]");
        int[] b = new int[0];
        Profiler.EndSample();

        // 36 byte
        Profiler.BeginSample("int[1]");
        int[] a = new int[1];
        Profiler.EndSample();

        // 432 byte
        Profiler.BeginSample("int[]");
        int[] array = new int[10 * 10];

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                array[i * 10 + j] = 0;
            }
        }
        Profiler.EndSample();

        string[] arrayString = new string[100];
        for (int i = 0; i < arrayString.Length; i++)
        {
            arrayString[i] = i.ToString();
        }

        // 32 byte
        {
            Profiler.BeginSample("long[0]");
            long[] l = new long[0];
            Profiler.EndSample();
        }

        // 496 byte
        {
            Profiler.BeginSample("int[,]");
            int[,] array0 = new int[10, 10];

            for (int i = 0; i < array0.GetLength(0); i++)
            {
                for (int j = 0; j < array0.GetLength(1); j++)
                {
                    array0[i, j] = 0;
                }
            }
            Profiler.EndSample();
        }

        // 0.8 Kbyte
        {
            Profiler.BeginSample("int[][]");
            int[][] array1 = new int[10][];

            for (int i = 0; i < array1.Length; i++)
            {
                array1[i] = new int[10];
                for (int j = 0; j < array1[i].Length; j++)
                {
                    array1[i][j] = 0;
                }
            }
            Profiler.EndSample();
        }

        // 40byte
        Profiler.BeginSample("string[0]");
        string[] s = new string[0];
        Profiler.EndSample();

        // 48byte
        Profiler.BeginSample("string[1]");
        s = new string[1];
        Profiler.EndSample();

        // 32byte
        // int[] _items
        // int _size
        // int _version
        // (Object _syncRoot)
        Profiler.BeginSample("List<int>");
        List<int> list = new List<int>();
        Profiler.EndSample();

        // 76byte
        // 32(Listのサイズ) + 44(int[1]のサイズ)
        {
            Profiler.BeginSample("List<int>(1)");
            list = new List<int>(1);
            Profiler.EndSample();
        }

        // 88byte
        // デフォルトのキャパシティが4なので4つまではメモリ確保されない
        {
            Profiler.BeginSample("List<int> 2");
            List<int> list2 = new List<int>();
            list2.Add(1);
            list2.Add(2);
            list2.Add(3);
            list2.Add(4);
            Profiler.EndSample();
        }

        // 88byte
        {
            Profiler.BeginSample("List<int>(4)");
            List<int> list3 = new List<int>(4);
            Profiler.EndSample();
        }

        // 78.2KB
        // 32 + 40 + 10000 * 8
        {
            Profiler.BeginSample("List<string>(10000)");
            List<string> list4 = new List<string>(10000);
            Profiler.EndSample();
        }

        // 488byte
        {
            Func<int, int> orderFunc = x => x;
            Profiler.BeginSample("OrderBy");
            array.OrderBy(orderFunc);
            Profiler.EndSample();
        }

        // 10.9KB
        // Comparer.Compare -> Comparisionのキャストが発生する
        {
            Profiler.BeginSample("Sort");
            for (int i = 0; i < 100; i++)
            {
                Array.Sort(array);
            }
            Profiler.EndSample();
        }

        // 0byte
        // 要素数が0だと比較がないのでキャストも発生しない
        {
            int[] array2 = new int[0];
            Profiler.BeginSample("Sort 0");
            for (int i = 0; i < 100; i++)
            {
                Array.Sort(array2);
            }
            Profiler.EndSample();
        }

        // 10.9KB
        {
            Profiler.BeginSample("Sort Default");
            var comparer = Comparer<int>.Default;
            for (int i = 0; i < 100; i++)
            {
                Array.Sort(array, comparer);
            }
            Profiler.EndSample();
        }

        // 112byte
        {
            Profiler.BeginSample("Sort Lambda");
            for (int i = 0; i < 100; i++)
            {
                Array.Sort(array, (x, y) => x - y);
            }
            Profiler.EndSample();
        }

        // 10.9KB
        {
            Profiler.BeginSample("Sort null");
            for (int i = 0; i < 100; i++)
            {
                Array.Sort<int>(array, 0, array.Length, null);
            }
            Profiler.EndSample();
        }

        // boxingとComparisionへのキャストが発生する
        // 12.6KB
        {
            Profiler.BeginSample("Sort IComparer");
            for (int i = 0; i < 100; i++)
            {
                Array.Sort(array, new CompStruct());
            }
            Profiler.EndSample();
        }

        // 10.9KB
        {
            List<int> list5 = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                list5.Add(i);
            }

            Profiler.BeginSample("Sort List");
            for (int i = 0; i < 100; i++)
            {
                list5.Sort();
            }
            Profiler.EndSample();
        }

        // 要素がobjectにキャストされている
        // 2.0KB
        {
            Profiler.BeginSample("Reverse");
            Array.Reverse(array);
            Profiler.EndSample();
        }

        // 0.6KB
        {
            Profiler.BeginSample("Enumerable.Reverse");
            array = array.Reverse().ToArray();
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("Reverse string");
            Array.Reverse(arrayString);
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("Reverse Custom");
            Reverse(array);
            Profiler.EndSample();
        }

        // 8.3Kbyte
        {
            Profiler.BeginSample("List default");
            List<int> list5 = new List<int>();

            for (int i = 0; i < 1000; i++)
            {
                list5.Add(i);
            }

            Profiler.EndSample();
        }

        // 4Kbyte
        {
            Profiler.BeginSample("List Captacity");
            List<int> list5 = new List<int>(1000);

            for (int i = 0; i < 1000; i++)
            {
                list5.Add(i);
            }

            Profiler.EndSample();
        }

        // リストをデフォルト値で初期化
        // 4.0KB
        {
            Profiler.BeginSample("List init 0 for");
            List<int> list5 = new List<int>(1000);
            for (int i = 0; i < 1000; i++)
            {
                list5.Add(0);
            }
            Profiler.EndSample();
        }

        {
            Profiler.BeginSample("List init 0 func");
            List<int> list5 = CreateEmptyList<int>(1000);
            Profiler.EndSample();
        }

        // 7.9KB
        {
            Profiler.BeginSample("List init 0 array");
            List<int> list5 = new List<int>(new int[1000]);
            Profiler.EndSample();
        }

        // 8.4KB
        {
            Profiler.BeginSample("List init 0 enumerable");
            List<int> list5 = new List<int>(Enumerable.Repeat(0, 1000));
            Profiler.EndSample();
        }

        // 4.0KB
        {
            Profiler.BeginSample("List init 0 enumerable.ToList");
            List<int> list5 = Enumerable.Repeat(0, 1000).ToList();
            Profiler.EndSample();
        }

        // 4.0KB
        {
            Profiler.BeginSample("List init 0 enumerable AddRange");
            List<int> list5 = new List<int>(1000);
            list5.AddRange(Enumerable.Repeat(0, 1000));
            Profiler.EndSample();
        }

        // 4.0KB
        {
            Profiler.BeginSample("List init 0 Collection");
            List<int> list5 = new List<int>(new DefaultInitCollection(1000));
            Profiler.EndSample();
        }

        List<string> tmpList = new List<string>();
        for (int i = 0; i < 100; i++)
        {
            tmpList.Add("hoge");
        }

        // 0byte
        {
            Profiler.BeginSample("foreach List");
            string str = null;
            foreach (var e in tmpList)
            {
                str = e;
            }

            Profiler.EndSample();
        }

        // IEnumeratorへのボックス化がおこる
        // 40byte
        {
            Profiler.BeginSample("foreach IList");
            IList ilist = (IList)tmpList;
            string str = null;
            foreach (string e in ilist)
            {
                str = e;
            }

            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("for IList");
            IList ilist = (IList)tmpList;
            string str = null;
            for (int i = 0; i < ilist.Count; i++)
            {
                str = (string)ilist[i];
            }

            Profiler.EndSample();
        }

        // 10.9KB
        {
            List<int> list5 = new List<int>() { 3, 2, 1 };

            Profiler.BeginSample("List.Sort");
            for (int i = 0; i < 100; i++)
            {
                list5.Sort();
            }
            Profiler.EndSample();
        }

        // 32byte
        {
            string[] array2 = new string[10000];
            Profiler.BeginSample("Array foreach");
            
            var array3 = (Array)array2;
            
            foreach (var e in array3)
            {
                var ss = (string)e;
            }
            Profiler.EndSample();
        }

        // 0byte
        // foreachよりも遅い
        {
            string[] array2 = new string[10000];
            Profiler.BeginSample("Array for");

            var array3 = (Array)array2;
            int l = array3.Length;
            for (int i = 0; i < l; i++)
            {
                var ss = (string)array3.GetValue(i);
            }
            Profiler.EndSample();
        }

        {
            string[] array2 = new string[10000];
            Profiler.BeginSample("IList for");

            var array3 = (IList)array2;
            int l = array3.Count;
            for (int i = 0; i < l; i++)
            {
                var ss = (string)array3[i];
            }
            Profiler.EndSample();
        }

        // 0B
        {
            List<int> list5 = new List<int>(4);
            List<int> list6 = new List<int>(4);

            Profiler.BeginSample("List.AddRange list");
            list5.AddRange(list6);
            Profiler.EndSample();
        }

        // 88byte
        {
            List<int> list5 = new List<int>(4);
            Queue<int> queue = new Queue<int>(4);

            Profiler.BeginSample("List.AddRange queue");
            list5.AddRange(queue);
            Profiler.EndSample();
        }

        // 80byte
        {
            List<int> list5 = new List<int>();
            Profiler.BeginSample("List.AsReadOnly");
            var readonlyList = list5.AsReadOnly();
            Profiler.EndSample();
        }
    }
}
