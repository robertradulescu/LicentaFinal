namespace LicentaFinal.Models
{
    public class InvoiceHistory
    {
        public int Id { get; set; }
        public DateTime DateChanged { get; set; }
        public int OrderId { get; set; }
        public Invoice Invoice { get; set; }
        public string OldCompanyName { get; set; }
        public string NewCompanyName { get; set; }

        public string OldSeries { get; set; }
        public string NewSeries { get; set; }

        public int OldNumber { get; set; }
        public int NewNumber { get; set; }

        public string OldCurrency { get; set; }

        public string NewCurrency { get; set; }

        public string OldAdress { get; set; }
        public string NewAdress { get; set; }

        public long OldIban { get; set; }
        public long NewIban { get; set; }

        public string OldBank { get; set; }
        public string NewBank { get; set; }

        public string OldAddressMail { get; set; }
        public string NewAddressMail { get; set; }


        public string OldObservation { get; set; }
        public string NewObservation { get; set; }

        public string OldCreator { get; set; }
        public string NewCreator { get; set; }

        public string OldBuyerAddress { get; set; }
        public string NewBuyerAddress { get; set; }

        public long OldCnpBuyer { get; set; }
        public long NewCnpBuyer { get; set; }

        public string OldTradeRegistrationNumber { get; set; }
        public string NewTradeRegistrationNumber { get; set; }
    }
}