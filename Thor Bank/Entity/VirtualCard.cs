namespace Thor_Bank.Entity
{
    public class VirtualCard
    {
        public int CardOwnerID { get; set; }
        public string Cardnumber { get; set; }
        public string EXP { get; set; }
        public string Nameoncard { get; set; }
        public int CCV { get; set; }
        public DateTime createdDate { get; set; }

    }
}
