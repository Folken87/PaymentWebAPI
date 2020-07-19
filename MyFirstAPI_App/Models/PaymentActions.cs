using System;
using System.ComponentModel.DataAnnotations;

public class PaymentActions
{
    [Key]
    public int Id { get; set; }
    public string transaction_price { get; set; }
    public string appointment { get; set; }
    public long card_number { get; set; }
    public string card_date { get; set; }
    public int card_cvv { get; set; }
    public DateTime date_transaction { get; set; }
}