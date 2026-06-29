namespace MarketPlace.Models
{
    public class SalesReceipt
    {
        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerPhone { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? DateTime { get; set; }
        public decimal GrandTotal { get; set; }
        public string? AmountInWords { get; set; }

        // This MUST be a String
        public string? SalesItemsJson { get; set; }
    }
}