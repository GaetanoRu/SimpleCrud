using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCrud.Shared.Models.Responses
{
    public class ListResultPagination<T>
    {
        public int Result { get; }
        public int TotalResult { get; }
        public int TotalPages { get; }
        public bool HasNextPage { get; }
        public int CurrentPage { get; }
        public IEnumerable<T> Content { get; }

        public ListResultPagination(IEnumerable<T> content)
        {
            Content = content;
            TotalResult = content?.Count() ?? 0;
            HasNextPage = false;
        }
        public ListResultPagination(IEnumerable<T> content, int totalResult, int totalPages, int currentPage, bool hasNextPage = false)
        {
            TotalResult = totalResult;
            HasNextPage = hasNextPage;
            TotalPages = totalPages;
            CurrentPage = currentPage;
            Content = content;
        }
    }
}
