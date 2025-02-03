namespace Thor_Bank.Entity
{
    public class WalletTransactions
    {
        public int TransactionID { get; set; }

        public string Sender { get; set; }

        public string Reciptient { get; set; }

        public int value { get; set; }

        public string Type { get; set; }

        public DateTime createdDate { get; set; }
    }
}
