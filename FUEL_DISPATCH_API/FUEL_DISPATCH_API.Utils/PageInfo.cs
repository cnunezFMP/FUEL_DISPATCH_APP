namespace FUEL_DISPATCH_API.Utils
{
    public static class PageInfo
    {
        public static object PageI(int totalEntities, int currentPageE, int pageSizeE)
        {
            int totalData = totalEntities;
            int totalPages = (int)Math.Ceiling((double)totalData / pageSizeE); ;
            int currentPages = currentPageE;
            int remainingPages = totalData - currentPages;

            return new
            {
                TotalData = totalData,
                TotalPages = totalPages,
                CurrentPage = currentPages,
                RemainingPages = remainingPages,
            };
        }
    }
}