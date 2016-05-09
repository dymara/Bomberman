using System.Collections.Generic;
using UnityEngine;

public static class ShuffleListExtension
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int index = Random.Range(0, i);
            T tmp = list[index];
            list[index] = list[i];
            list[i] = tmp;
        }  
    }
}