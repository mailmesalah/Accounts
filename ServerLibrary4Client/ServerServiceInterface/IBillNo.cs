using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerServiceInterface
{
    [ServiceContract]
    public interface IBillNo
    {
        [OperationContract]
        int ReadNextCashReceiptBillNo(string financialCode);
        [OperationContract]
        int ReadNextCashPaymentBillNo(string financialCode);
        [OperationContract]
        int ReadNextBankDepositBillNo(string financialCode);
        [OperationContract]
        int ReadNextBankWithdrawalBillNo(string financialCode);
        [OperationContract]
        int ReadNextJournalVoucherBillNo(string financialCode);
        [OperationContract]
        int ReadNextLedgerRegisterBillNo();
        [OperationContract]
        int ReadNextLedgerTransactionBillNo(string financialCode);
        [OperationContract]
        int ReadNextOpeningBalanceBillNo(string financialCode);
        [OperationContract]
        int ReadNextPurchaseBillNo(string financialCode);
        [OperationContract]
        int ReadNextPurchaseReturnBillNo(string financialCode);
        [OperationContract]
        int ReadNextSalesBillNo(string financialCode);
        [OperationContract]
        int ReadNextSalesReturnBillNo(string financialCode);
        [OperationContract]
        bool UpdateCashReceiptBillNo(string financialCode, int billNo);
        [OperationContract]
        bool UpdateCashPaymentBillNo(string financialCode, int billNo);
        [OperationContract]
        bool UpdateBankDepositBillNo(string financialCode, int billNo);
        [OperationContract]
        bool UpdateBankWithdrawalBillNo(string financialCode, int billNo);
        [OperationContract]
        bool UpdateJournalVoucherBillNo(string financialCode, int billNo);
        [OperationContract]
        bool UpdateLedgerRegisterBillNo(int billNo);
        [OperationContract]
        bool UpdateLedgerTransactionBillNo(string financialCode, int billNo);
        [OperationContract]
        bool UpdateOpeningBalanceBillNo(string financialCode, int billNo);
        [OperationContract]
        bool UpdatePurchaseBillNo(string financialCode, int billNo);
        [OperationContract]
        bool UpdatePurchaseReturnBillNo(string financialCode, int billNo);
        [OperationContract]
        bool UpdateSalesBillNo(string financialCode, int billNo);
        [OperationContract]
        bool UpdateSalesReturnBillNo(string financialCode, int billNo);
        [OperationContract]
        List<string> ReadAllFinancialCodes();
        [OperationContract]
        bool DeleteFinancialYear(string financialCode);

    }
}
