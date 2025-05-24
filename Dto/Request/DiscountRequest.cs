namespace COURSEPROJECT.Dto.Request
{
    public class DiscountRequest
    {
        public string Code { get; set; }
        public decimal Value { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
