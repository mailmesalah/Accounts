using ServerServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAccountServerApp.Services
{
    class BillNoService : IBillNo
    {
        private static readonly object @billNoLock = new object();

        public bool DeleteFinancialYear(string financialCode)
        {
            bool success = true;

            try
            {
                throw new NotImplementedException();
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public List<string> ReadAllFinancialCodes()
        {
            List<string> fcodes = new List<string>();
            try
            {
                
                using (var dataB = new AccountsdbEntities())
                {
                    var vals = dataB.bill_nos.Select(e=>e.financial_code);
                    foreach (var item in vals)
                    {
                        fcodes.Add(item);
                    }
                }                
            }
            catch
            {

            }
            return fcodes;
        }

        public int ReadNextBankDepositBillNo(string financialCode)
        {
            int billNo = 1;
            try
            {                
                using (var dataB = new AccountsdbEntities())
                {
                    var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                    if (val == null)//Create a row for new Financial year
                    {
                        lock (@billNoLock) {
                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;

                            dataB.SaveChanges();
                        }
                    }
                    else
                    {
                        billNo = val.bank_deposit;
                    }                    
                }                
            }
            catch
            {

            }
            return billNo;
        }

        public int ReadNextBankWithdrawalBillNo(string financialCode)
        {
            int billNo = 1;
            try
            {
                using (var dataB = new AccountsdbEntities())
                {
                    var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                    if (val == null)//Create a row for new Financial year
                    {
                        lock (@billNoLock)
                        {
                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;

                            dataB.SaveChanges();
                        }
                    }
                    else
                    {
                        billNo = val.bank_withdrawal;
                    }                    
                }
            }
            catch
            {

            }
            return billNo;
        }

        public int ReadNextCashPaymentBillNo(string financialCode)
        {
            int billNo = 1;
            try
            {
                using (var dataB = new AccountsdbEntities())
                {
                    var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                    if (val == null)//Create a row for new Financial year
                    {
                        lock (@billNoLock)
                        {
                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;

                            dataB.SaveChanges();
                        }
                    }
                    else
                    {
                        billNo = val.cash_payment;
                    }
                }
            }
            catch
            {

            }
            return billNo;
        }

        public int ReadNextCashReceiptBillNo(string financialCode)
        {
            int billNo = 1;
            try
            {
                using (var dataB = new AccountsdbEntities())
                {
                    var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                    if (val == null)//Create a row for new Financial year
                    {
                        lock (@billNoLock)
                        {
                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;

                            dataB.SaveChanges();
                        }
                    }
                    else
                    {
                        billNo = val.cash_receipt;
                    }
                }
            }
            catch
            {

            }
            return billNo;
        }

        public int ReadNextJournalVoucherBillNo(string financialCode)
        {
            int billNo = 1;
            try
            {
                using (var dataB = new AccountsdbEntities())
                {
                    var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                    if (val == null)//Create a row for new Financial year
                    {
                        lock (@billNoLock)
                        {
                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;

                            dataB.SaveChanges();
                        }
                    }
                    else
                    {
                        billNo = val.journal_voucher;
                    }                    
                }
            }
            catch
            {

            }
            return billNo;
        }

        public int ReadNextLedgerRegisterBillNo()
        {
            int billNo = 1;
            try
            {
                using (var dataB = new AccountsdbEntities())
                {
                    var val = dataB.bill_nos.Where(e => e.financial_code == "2015").FirstOrDefault();

                    if (val == null)//Create a row for new Financial year
                    {
                        lock (@billNoLock)
                        {
                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 45;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = "2015";

                            dataB.SaveChanges();
                        }
                    }
                    else
                    {
                        billNo = val.ledger_register;
                    }                    
                }
            }
            catch
            {

            }
            return billNo;
        }

        public int ReadNextLedgerTransactionBillNo(string financialCode)
        {
            int billNo = 1;
            try
            {
                using (var dataB = new AccountsdbEntities())
                {
                    var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                    if (val == null)//Create a row for new Financial year
                    {
                        lock (@billNoLock)
                        {
                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;

                            dataB.SaveChanges();
                        }
                    }
                    else
                    {
                        billNo = val.ledger_transaction;
                    }
                }
            }
            catch
            {

            }
            return billNo;
        }

        public int ReadNextOpeningBalanceBillNo(string financialCode)
        {
            int billNo = 1;
            try
            {
                using (var dataB = new AccountsdbEntities())
                {
                    var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                    if (val == null)//Create a row for new Financial year
                    {
                        lock (@billNoLock)
                        {
                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;

                            dataB.SaveChanges();
                        }
                    }
                    else
                    {
                        billNo = val.opening_balance;
                    }                    
                }
            }
            catch
            {

            }
            return billNo;
        }

        public int ReadNextPurchaseBillNo(string financialCode)
        {
            int billNo = 1;
            try
            {
                using (var dataB = new AccountsdbEntities())
                {
                    var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                    if (val == null)//Create a row for new Financial year
                    {
                        lock (@billNoLock)
                        {
                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;

                            dataB.SaveChanges();
                        }
                    }
                    else
                    {
                        billNo = val.purchase;
                    }
                }
            }
            catch
            {

            }
            return billNo;
        }

        public int ReadNextPurchaseReturnBillNo(string financialCode)
        {
            int billNo = 1;
            try
            {
                using (var dataB = new AccountsdbEntities())
                {
                    var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                    if (val == null)//Create a row for new Financial year
                    {
                        lock (@billNoLock)
                        {
                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;

                            dataB.SaveChanges();
                        }
                    }
                    else
                    {
                        billNo = val.purchase_return;
                    }
                }
            }
            catch
            {

            }
            return billNo;
        }

        public int ReadNextSalesBillNo(string financialCode)
        {
            int billNo = 1;
            try
            {
                using (var dataB = new AccountsdbEntities())
                {
                    var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                    if (val == null)//Create a row for new Financial year
                    {
                        lock (@billNoLock)
                        {
                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;

                            dataB.SaveChanges();
                        }
                    }
                    else
                    {
                        billNo = val.sales;
                    }
                }
            }
            catch
            {

            }
            return billNo;
        }

        public int ReadNextSalesReturnBillNo(string financialCode)
        {
            int billNo = 1;
            try
            {
                using (var dataB = new AccountsdbEntities())
                {
                    var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                    if (val == null)//Create a row for new Financial year
                    {
                        lock (@billNoLock)
                        {
                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;

                            dataB.SaveChanges();
                        }
                    }
                    else
                    {
                        billNo = val.sales_return;
                    }
                }
            }
            catch
            {

            }
            return billNo;
        }

        public bool UpdateBankDepositBillNo(string financialCode, int billNo)
        {
            bool success = true;  
            try
            {
                lock (@billNoLock)
                {
                    using (var dataB = new AccountsdbEntities())
                    {
                        var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                        if (val == null)//Create a row for new Financial year
                        {

                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = billNo;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;                            
                        }
                        else//Or Edit
                        {
                            val.bank_deposit = billNo;
                        }

                        dataB.SaveChanges();                        
                    }
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public bool UpdateBankWithdrawalBillNo(string financialCode, int billNo)
        {
            bool success = true;
            try
            {
                lock (@billNoLock)
                {
                    using (var dataB = new AccountsdbEntities())
                    {
                        var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                        if (val == null)//Create a row for new Financial year
                        {

                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = billNo;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;
                        }
                        else//Or Edit
                        {
                            val.bank_withdrawal = billNo;
                        }

                        dataB.SaveChanges();
                    }
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public bool UpdateCashPaymentBillNo(string financialCode, int billNo)
        {
            bool success = true;
            try
            {
                lock (@billNoLock)
                {
                    using (var dataB = new AccountsdbEntities())
                    {
                        var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                        if (val == null)//Create a row for new Financial year
                        {

                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = billNo;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;
                        }
                        else//Or Edit
                        {
                            val.cash_payment = billNo;
                        }

                        dataB.SaveChanges();
                    }
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public bool UpdateCashReceiptBillNo(string financialCode, int billNo)
        {
            bool success = true;
            try
            {
                lock (@billNoLock)
                {
                    using (var dataB = new AccountsdbEntities())
                    {
                        var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                        if (val == null)//Create a row for new Financial year
                        {

                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = billNo;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;
                        }
                        else//Or Edit
                        {
                            val.cash_receipt = billNo;
                        }

                        dataB.SaveChanges();
                    }
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public bool UpdateJournalVoucherBillNo(string financialCode, int billNo)
        {
            bool success = true;
            try
            {
                lock (@billNoLock)
                {
                    using (var dataB = new AccountsdbEntities())
                    {
                        var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                        if (val == null)//Create a row for new Financial year
                        {

                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = billNo;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;
                        }
                        else//Or Edit
                        {
                            val.journal_voucher = billNo;
                        }

                        dataB.SaveChanges();
                    }
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public bool UpdateLedgerRegisterBillNo(int billNo)
        {
            bool success = true;
            try
            {
                lock (@billNoLock)
                {
                    using (var dataB = new AccountsdbEntities())
                    {
                        var val = dataB.bill_nos.Where(e => e.financial_code == "2015").FirstOrDefault();

                        if (val == null)//Create a row for new Financial year
                        {

                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = billNo;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = "2015";
                        }
                        else//Or Edit
                        {
                            val.ledger_register = billNo;
                        }

                        dataB.SaveChanges();
                    }
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public bool UpdateLedgerTransactionBillNo(string financialCode, int billNo)
        {
            bool success = true;
            try
            {
                lock (@billNoLock)
                {
                    using (var dataB = new AccountsdbEntities())
                    {
                        var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                        if (val == null)//Create a row for new Financial year
                        {

                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = billNo;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;
                        }
                        else//Or Edit
                        {
                            val.ledger_transaction = billNo;
                        }

                        dataB.SaveChanges();
                    }
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public bool UpdateOpeningBalanceBillNo(string financialCode, int billNo)
        {
            bool success = true;
            try
            {
                lock (@billNoLock)
                {
                    using (var dataB = new AccountsdbEntities())
                    {
                        var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                        if (val == null)//Create a row for new Financial year
                        {

                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = billNo;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;
                        }
                        else//Or Edit
                        {
                            val.opening_balance = billNo;
                        }

                        dataB.SaveChanges();
                    }
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public bool UpdatePurchaseBillNo(string financialCode, int billNo)
        {
            bool success = true;
            try
            {
                lock (@billNoLock)
                {
                    using (var dataB = new AccountsdbEntities())
                    {
                        var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                        if (val == null)//Create a row for new Financial year
                        {

                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = billNo;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;
                        }
                        else//Or Edit
                        {
                            val.purchase = billNo;
                        }

                        dataB.SaveChanges();
                    }
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public bool UpdatePurchaseReturnBillNo(string financialCode, int billNo)
        {
            bool success = true;
            try
            {
                lock (@billNoLock)
                {
                    using (var dataB = new AccountsdbEntities())
                    {
                        var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                        if (val == null)//Create a row for new Financial year
                        {

                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = billNo;
                            bn.sales = 1;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;
                        }
                        else//Or Edit
                        {
                            val.purchase_return = billNo;
                        }

                        dataB.SaveChanges();
                    }
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public bool UpdateSalesBillNo(string financialCode, int billNo)
        {
            bool success = true;
            try
            {
                lock (@billNoLock)
                {
                    using (var dataB = new AccountsdbEntities())
                    {
                        var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                        if (val == null)//Create a row for new Financial year
                        {

                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = billNo;
                            bn.sales_return = 1;
                            bn.financial_code = financialCode;
                        }
                        else//Or Edit
                        {
                            val.sales = billNo;
                        }

                        dataB.SaveChanges();
                    }
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public bool UpdateSalesReturnBillNo(string financialCode, int billNo)
        {
            bool success = true;
            try
            {
                lock (@billNoLock)
                {
                    using (var dataB = new AccountsdbEntities())
                    {
                        var val = dataB.bill_nos.Where(e => e.financial_code == financialCode).FirstOrDefault();

                        if (val == null)//Create a row for new Financial year
                        {

                            bill_nos bn = dataB.bill_nos.Create();
                            bn.bank_deposit = 1;
                            bn.bank_withdrawal = 1;
                            bn.cash_payment = 1;
                            bn.cash_receipt = 1;
                            bn.journal_voucher = 1;
                            bn.ledger_register = 1;
                            bn.ledger_transaction = 1;
                            bn.opening_balance = 1;
                            bn.purchase = 1;
                            bn.purchase_return = 1;
                            bn.sales = 1;
                            bn.sales_return = billNo;
                            bn.financial_code = financialCode;
                        }
                        else//Or Edit
                        {
                            val.sales_return = billNo;
                        }

                        dataB.SaveChanges();
                    }
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }
    }
}
