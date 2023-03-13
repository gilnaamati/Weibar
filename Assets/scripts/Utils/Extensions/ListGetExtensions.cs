using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

    public static class ListGetExtensions
    {
        public static T GetLast<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }

        public static T GetRandom<T>(this List<T> list)
        {
            var index = Random.Range(0, list.Count);
            return list[index];
        }
    }

