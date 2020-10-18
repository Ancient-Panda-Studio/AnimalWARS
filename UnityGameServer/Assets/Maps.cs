using System.Collections.Generic;
using System.Linq;

public abstract class Maps
{
    public static List<Map> Parse(IEnumerable<int> mapOrder)
    {
        return mapOrder.Select(intMap => Dictionaries.Maps[intMap]).ToList();
    }
}