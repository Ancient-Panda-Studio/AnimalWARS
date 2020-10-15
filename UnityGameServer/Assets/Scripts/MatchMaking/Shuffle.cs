using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class Shuffle
{
    public static Random rng = new Random();
    public static void ShuffleList<T>(this IList<T> list)  
    {  
        var n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            var value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }
    }
}
