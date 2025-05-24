namespace COURSEPROJECT.Model
{
    public enum OrderStatus
    {
        Pending,
        Cancelled,
        Approved,
        Paid,         
        Failed,

    }
    public class Order
    {
        public int Id { get; set; }
        public OrderStatus orderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        
        public string? SessionId { get; set; }
        public string? TransactionId { get; set;}
        public int? DiscountId { get; set; }
        public Discount? Discount { get; set; }

        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }

        public ApplicationUser User { get; set; }
     public   string UserId { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
