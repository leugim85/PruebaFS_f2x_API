using F2xFullStackAssesment.Infraestructure.IResources;
using System.Resources;

namespace F2xFullStackAssesment.Infraestructure.Resources
{
    public class MessagesProvider : IMessagesProvider
    {
        private readonly ResourceManager resourceManager;

        public MessagesProvider()
        {
            resourceManager = new ResourceManager("F2xFullStackAssesment.Infraestructure.Resources.Messages", typeof(MessagesProvider).Assembly);
        }

        public string ClientInformationNotFound => resourceManager.GetString("ClientInformationNotFound");
        public string PaymentGatewayInProcess => resourceManager.GetString("PaymentGatewayInProcess");
        public string PaymentGatewayPaid => resourceManager.GetString("PaymentGatewayPaid");
        public string InvoiceCollectedInProcess => resourceManager.GetString("InvoiceCollectedInProcess");
        public string InvoiceCollectedPaid => resourceManager.GetString("InvoiceCollectedPaid");
        public string InvoiceCollectedNotFound => resourceManager.GetString("InvoiceCollectedNotFound");
        public string InvoiceCollectedNoCreditCards => resourceManager.GetString("InvoiceCollectedNoCreditCards");
        public string ReceiptHistoryInProcess => resourceManager.GetString("ReceiptHistoryInProcess");
        public string ReceiptHistoryPaid => resourceManager.GetString("ReceiptHistoryPaid");
        public string GeneratePaymentError => resourceManager.GetString("GeneratePaymentError");
        public string FileNotFoundInStorage => resourceManager.GetString("FileNotFoundInStorage");
        public string PhotoFineDocumentProcessed => resourceManager.GetString("PhotoFineDocumentProcessed");
        public string PhotoFineDocumentNotFound => resourceManager.GetString("PhotoFineDocumentNotFound");
    }
}
