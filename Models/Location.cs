namespace AutoRenter.API.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string SiteId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
    }
}