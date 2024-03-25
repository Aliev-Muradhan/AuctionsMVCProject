using Microsoft.EntityFrameworkCore;

namespace Auctions
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T>  items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count/(double)pageSize);

            this.AddRange(items);
        }
        public bool HasPreviousPage => PageIndex > 1; 
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync (IQueryable<T> sourse, int pageIndex, int pageSize)
        {
            var count = await sourse.CountAsync();
            var items = await sourse.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T> (items, count, pageIndex, pageSize);
        }
    }
}
