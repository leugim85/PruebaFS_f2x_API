namespace F2xFullStackAssesment.Infraestructure.IResources
{
    public interface IMessagesProvider
    {
        string ClientInformationNotFound { get; }
        string PaymentGatewayInProcess { get; }
        string PaymentGatewayPaid { get; }
        string InvoiceCollectedInProcess { get; }
        string InvoiceCollectedPaid { get; }
        string InvoiceCollectedNotFound { get; }
        string InvoiceCollectedNoCreditCards { get; }
        string ReceiptHistoryInProcess { get; }
        string ReceiptHistoryPaid { get; }
        string GeneratePaymentError { get; }
        string FileNotFoundInStorage { get; }
        string PhotoFineDocumentProcessed { get; }
        string PhotoFineDocumentNotFound { get; }
    }
}
