namespace Acl.Net.Core;

internal static class Utils
{
    public static T[] RemoveDuplicates<T>(T[] s)
    {
        var objSet = new HashSet<T>(s);
        var array = new T[objSet.Count];
        objSet.CopyTo(array);
        return array;
    }

    public static T[] JoinArray<T>(T[] arr1, T[] arr2)
    {
        var objArray = new T[arr1.Length + arr2.Length];
        arr1.CopyTo(objArray, 0);
        arr2.CopyTo(objArray, arr1.Length);
        return objArray;
    }
}