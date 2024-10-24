namespace BookApplication.Utilities
{
    public class Utility
    {
        public static string BookAPIBase { get; set; }
        public enum ApiType
        {
            GET,
            PUT,
            DELETE,
            POST
        }
    }
}
