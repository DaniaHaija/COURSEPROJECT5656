namespace COURSEPROJECT.Model
{
    public class Discount
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
