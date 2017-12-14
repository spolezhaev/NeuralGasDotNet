﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Defaults;

namespace NeuralGasDotNet.Extensions
{
    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static Random Local;

        public static Random ThisThreadsRandom
        {
            get
            {
                return Local ?? (Local =
                           new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)));
            }
        }
    }

    static class ListExtension
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                var k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static ChartValues<ObservablePoint> ToChartValues(this List<(double, double)> weights) => 
            new ChartValues<ObservablePoint>(weights.Select(w => new ObservablePoint(w.Item1, w.Item2)));
    }
}