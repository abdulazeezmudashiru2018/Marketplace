using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required, StringLength(100)] public string? FullName { get; set; }
        [Required, EmailAddress] public string? Email { get; set; }
        [Required, StringLength(200)] public string? Address { get; set; }
        [Required, StringLength(100)] public string? City { get; set; }
        [Required, StringLength(20)] public string? Zip { get; set; }
        [Required] public string? Payment { get; set; }

        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();


        public string? PaymentReference { get; set; }
        public bool IsPaid { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? Name { get; set; }     // belongs to OrderItem — OK
        public int Qty { get; set; }
        public decimal Price { get; set; }   // belongs to OrderItem — OK
        public decimal LineTotal => Qty * Price;

        public int OrderId { get; set; }
        public Order? Order { get; set; }

    }



}