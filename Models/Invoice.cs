namespace LicentaFinal.Models
{   public class Invoice
    {
        public int Id { get; set; }

        public DateTime Creat { get; set; }

        public string NumeFirma { get; set; }

        public string Serie { get; set; }

        public int Numar { get; set; }

        public string Moneda { get; set; }

        public string Cumparator { get; set; }

        public string Adresa { get; set; }

        public long Iban { get; set; }

        public string Banca { get; set; }

        public string AdresaMail { get; set; }

        public string Observatii { get; set; }

        public string Creator { get; set; }

        public string AdresaCumparator { get; set; }

        public long CnpCumparator { get; set; }

        public string NrInregistrareComert { get; set; }

        public List<OrderItem> Items { get; set; }


        public int NumarIteme
        {
            get => Items.Count;
        }

        public Invoice()
        {
            Items = new List<OrderItem>();
        }
    }


}

