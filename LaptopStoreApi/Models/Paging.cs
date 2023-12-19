namespace LaptopStoreApi.Models
{
    public class Paging<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPage { get; set; }
        public Paging(List<T> items, int count, int pageIndex,int pageSize ) 
        { 
            PageIndex = pageIndex;
            TotalPage = (int)Math.Ceiling(count/(double)pageSize);
            AddRange( items );
        }
        public static Paging<T> Create(IQueryable<T> query, int pageIndex, int pageSize) 
        { 
            var count = query.Count();
            var items = query.Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();
            return new Paging<T> (items, count, pageIndex, pageSize );
        }
        
    }
}
