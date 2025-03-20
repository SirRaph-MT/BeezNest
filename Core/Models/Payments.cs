using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Payments
    {
        [Key]
        public int Id { get; set; }
        public string? Stock { get; set; }
        public bool Active { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? GrandTotal { get; set; }
        public string? ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public virtual ApplicationUser? Client { get; set; }

        public string? ProofOfPaymentPath { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }

    public class PaymentDetailViewModels
    {
        public IEnumerable<PaymentsViewModel> PaymentList { get; set; }
        public int PendingOrdersCount { get; set; } = 0;
    }

    public class PaymentsViewModel
    {
        public int Id { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? User { get; set; }
        public string? ClientEmail { get; set; }
        public string? StocksInString { get; set; }
        public List<Stock> Stocks { get; set; }
        public string? UserId { get; set; }
        public decimal? GrandTotal { get; set; }
        public string? Specifications { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public double? Quantity { get; set; }
        public string? ProofOfPaymentPath { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
    
    public enum PaymentStatus
    {
        Pending = 1,
        Confirm = 2,
        Decline = 3
    }
    public class Stock
    {
        // THIS SHOULD MATCH WITH PRODUCIID FROM UPLOADED PRODUCT DB
        public int StockId { get; set; }
        public string? Name { get; set; }
        public string? Specifications { get; set; }
        public decimal? UnitPrice { get; set; }
        public double? Quantity { get; set; }
        public decimal? Total { get; set; }
    }
}

