namespace acm_models
{
    public class RotYStats
    {
        public int RestaurantID { get; set; }
        public bool IsCurrentRotY { get; set; }
        public int CurrentYear { get; set; }
        public List<int> FormerYears { get; set; } = new();
    }
}
