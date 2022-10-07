using System;
using System.Collections;
using System.Diagnostics;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

namespace Timing
{
    class Program
    {
        public static void FillArray(ref int[] arr)
        {
            Random randNum = new Random();
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = randNum.Next(0, 100);
            }
        }
        public static void FillHashTable(ref Hashtable a)
        {
            Random randNum = new Random();
            for (int i = 0; i < a.Count; i++)
            {
                a.Add(i, randNum.Next(0, 100));
            }
        }
        static void Main(string[] args)
        {
            int[] a = new int[10000];
            Hashtable aHt = new Hashtable();
            FillArray(ref a);
            FillHashTable(ref aHt);
            Timing t = new Timing();
            Stopwatch sw = new Stopwatch();
            TimeSpan TimeTaken;
            
            //Методы сортировки
            ExchangeSorting exchangeSorting = new ExchangeSorting();
            //Сортировка пузыриками (через StopWatch)
            sw.Start();
            exchangeSorting.BubleSort(a); 
            sw.Stop();
            TimeTaken = sw.Elapsed;
            Console.WriteLine("Время выполнения сортировки пузырьком (StopWatch): " + TimeTaken.ToString(@"m\:ss\.fff"));
            //Сортировка пузыриками (через Timing)
            t = new Timing();
            t.StartTime();
            exchangeSorting.BubleSort(a); 
            t.StopTime();
            Console.WriteLine($"Время выполнения сортировки пузырьком (Timing): {t.Result().ToString()}");
            //Простой поиск
            SimpleSearch simpleSearch = new SimpleSearch();
            //Простой поиск (через StopWatch)
            sw.Start();
            simpleSearch.Search(a, 100); 
            sw.Stop();
            TimeTaken = sw.Elapsed;

            Console.WriteLine("Время выполнения простого поиска(StopWatch): " + TimeTaken.ToString(@"m\:ss\.fff"));
            //Простой поиск (через Timing)
            t = new Timing();
            t.StartTime();
            simpleSearch.Search(a, 100);
            t.StopTime();
            Console.WriteLine($"Время выполнения простого поиска(Timing): {t.Result().ToString()}");
            //Бинарный поиск
            SearchBinary searchBinary = new SearchBinary();
            //Бинарный поиск (через StopWatch)
            sw.Start();
            searchBinary.Search(a, 100); 
            sw.Stop();
            TimeTaken = sw.Elapsed;

            Console.WriteLine("Время выполнения бинарного поиска(StopWatch): " + TimeTaken.ToString(@"m\:ss\.fff"));
            //Бинарный поиск (через Timing)
            t = new Timing();
            t.StartTime();
            searchBinary.Search(a, 100); 
            t.StopTime();
            Console.WriteLine($"Время выполнения бинарного поиска(Timing): {t.Result().ToString()}");
            //Поиск по хэш таблице (через StopWatch)
            sw.Start();
            aHt.Values.OfType<int>().Where(s => s == 100);
            sw.Stop();
            TimeTaken = sw.Elapsed;
            Console.WriteLine("Время выполнения простого поиска по хэш таблице(StopWatch): " + TimeTaken.ToString(@"m\:ss\.fff"));
            //Поиск по хэш таблице (через Timing)
            t = new Timing();
            t.StartTime();
            aHt.Values.OfType<int>().Where(s => s == 100);
            t.StopTime();
            Console.WriteLine($"Время выполнения простого поиска по хэш таблице (Timing): {t.Result().ToString()}");
        }
        internal class ExchangeSorting
        {
            public void BubleSort(int[] a)
            {
                int N = a.Length;
                for (int i = 1; i < N; i++)
                {
                    for (int j = N - 1; j >= i; j--)
                    {
                        if (a[j - 1] > a[j])
                        {
                            int t = a[j - 1];
                            a[j - 1] = a[j];
                            a[j] = t;
                        }
                    }
                }
            }
        }
        internal class SearchBinary
        {
            public int Search(int[] a, int x)
            {
                int middle, left = 0, right = a.Length - 1;
                do
                {
                    middle = (left + right) / 2;
                    if (x > a[middle])
                        left = middle + 1;
                    else
                        right = middle - 1;
                }
                while ((a[middle] != x) && (left <= right));
                if (a[middle] == x)
                    return middle;
                else return -1;
            }
        }
        internal class SimpleSearch
        {
            public int Search(int[] a, int x)
            {
                int i = 0;
                while (i < a.Length && a[i] != x) i++;
                if (i < a.Length)
                    return i;
                else return -1;
            }
        }

    }

    internal class Timing

    {

        TimeSpan duration; //хранение результата измерения

        TimeSpan[] threads; // значения времени для всех потоков процесса

        public Timing()

        {

            duration = new TimeSpan(0);

            threads = new TimeSpan[Process.GetCurrentProcess().

            Threads.Count];

        }

        public void StartTime() //инициализация массива threads после вызова сборщика мусора

        {

            GC.Collect();

            GC.WaitForPendingFinalizers();

            for (int i = 0; i < threads.Length; i++)

                threads[i] = Process.GetCurrentProcess().Threads[i].

                UserProcessorTime;

        }

        public void StopTime() //повторный запрос текущего времени и выбирается тот, у которого результат отличается от

        {
            //предыдущего

            TimeSpan tmp;

            for (int i = 0; i < threads.Length; i++)

            {

                tmp = Process.GetCurrentProcess().Threads[i].

                UserProcessorTime.Subtract(threads[i]);

                if (tmp > TimeSpan.Zero)

                    duration = tmp;

            }

        }

        public TimeSpan Result()

        {

            return duration;

        }
    }


}




