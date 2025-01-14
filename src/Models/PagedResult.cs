
using System;
using System.Collections.Generic;

public class PagedResult<T>
{
    public List<T> Result { get; set; }

    public int Cursor { get; set; }

    public static PagedResult<T> ToPagedList(List<T> sourceList, PaginationParameters paginationParameters)
    {
        ArgumentNullException.ThrowIfNull(sourceList);
        ArgumentNullException.ThrowIfNull(paginationParameters);

        List<T> outputList = GetPaginatedList(sourceList, paginationParameters);
        int cursor = UpdateCursor(sourceList, paginationParameters, outputList);

        return new PagedResult<T>(){ Result = outputList, Cursor = cursor };
    }

    private static List<T> GetPaginatedList(List<T> sourceList, PaginationParameters paginationParameters)
    {
        var startIndex = paginationParameters.Cursor;
        var endIndex   = startIndex + paginationParameters.Limit - 1;

        // Cap the endIndex to the last index of the sourceList
        endIndex = Math.Min(endIndex, sourceList.Count - 1);

        // Log a warning if startIndex is out of bounds
        if (startIndex > sourceList.Count - 1)
        {
            Console.WriteLine($"Warning: Requested pagination start index ({startIndex}) is greater than the size of the source list ({sourceList.Count}).");
        }

        // Build the outputList with the requested range
        var outputList = new List<T>();
        for (var i = startIndex; i <= endIndex; i++)
        {
            outputList.Add(sourceList[i]);
        }

        return outputList;
    }

    private static int UpdateCursor(List<T> sourceList, PaginationParameters paginationParameters, List<T> outputList)
    {
        var endIndex = paginationParameters.Cursor + paginationParameters.Limit - 1;

        return endIndex < sourceList.Count - 1 ? endIndex + 1 : -1;
    }
}
