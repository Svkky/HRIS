namespace HRIS.Domain.Entities
{
    public class CustomerVoucher : BaseEntity
    {
        public int CustomerVoucherId { get; set; }
        public int CustomerId { get; set; }
        public int VoucherId { get; set; }
        public string VoucherNo { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }

        //Relationships
        public Customer Customer { get; set; }
        public Voucher Voucher { get; set; }
    }
}
