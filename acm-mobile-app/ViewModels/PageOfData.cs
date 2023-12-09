namespace acm_mobile_app.ViewModels
{
    public class PageOfData<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<T>? PageItems { get; set; } = null;
    }
}
