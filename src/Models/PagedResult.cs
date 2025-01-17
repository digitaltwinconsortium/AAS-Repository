
using System;
using System.Collections.Generic;

public class PagedResult<T>
{
    public List<T> Result { get; set; }

    public int Cursor { get; set; }

    public static PagedResult<T> ToPagedList(List<T> sourceList, PaginationParameters paginationParameters)
    {
        List<T> outputList = new();

        if (sourceList.Count > 0)
        {
            int startIndex = paginationParameters.Cursor;
            int endIndex = Math.Min(sourceList.Count - 1, paginationParameters.Limit - 1);

            if (startIndex > endIndex)
            {
                throw new ArgumentException($"Requested pagination start index ({startIndex}) is greater than the size of the source list ({sourceList.Count}).");
            }

            // Build the outputList with the requested range
            for (int i = startIndex; i <= endIndex; i++)
            {
                outputList.Add(sourceList[i]);
            }
        }

        return new PagedResult<T>(){ Result = outputList, Cursor = paginationParameters.Cursor + outputList.Count - 1 };
    }
}
