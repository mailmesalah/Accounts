using ServerServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Windows;
using WpfAccountServerApp.General;

namespace WpfAccountServerApp.Services
{
   
    public class BankDepositService : IBankDeposit
    {
        

        public bool CreateBill(CBankDeposit oBankDeposit)
        {
            bool returnValue = false;

            lock (Synchronizer.@lock)
            {
                
                using (var dataB = new AccountsdbEntities())
                {
                    var dataBTransaction = dataB.Database.BeginTransaction();
                    try
                    {

                        LedgerService ls = new LedgerService();
                        BillNoService bs = new BillNoService();


                        int cbillNo = bs.ReadNextBankDepositBillNo(oBankDeposit.FinancialCode);
                        bs.UpdateBankDepositBillNo(oBankDeposit.FinancialCode,cbillNo+1);
                        //Saving to Ledger Transaction
                        int ltBillNo = bs.ReadNextLedgerTransactionBillNo(oBankDeposit.FinancialCode);
                        bs.UpdateLedgerTransactionBillNo(oBankDeposit.FinancialCode, ltBillNo + 1);

                        for (int i = 0; i < oBankDeposit.Details.Count; i++)
                        {
                            bank_deposits bd = new bank_deposits();

                            bd.bill_no = cbillNo.ToString();
                            bd.bill_date_time = oBankDeposit.BillDateTime;
                            bd.serial_no = oBankDeposit.Details.ElementAt(i).SerialNo;
                            bd.ledger_code= oBankDeposit.Details.ElementAt(i).LedgerCode;
                            bd.ledger = oBankDeposit.Details.ElementAt(i).Ledger;
                            bd.narration = oBankDeposit.Details.ElementAt(i).Narration;
                            bd.amount = oBankDeposit.Details.ElementAt(i).Amount;
                            bd.financial_code = oBankDeposit.FinancialCode;
                            bd.status = oBankDeposit.Details.ElementAt(i).Status;
                            bd.bank = oBankDeposit.Bank;
                            bd.bank_code = oBankDeposit.BankCode;

                            dataB.bank_deposits.Add(bd);
                            
                            //First entry
                            ledger_transactions lt1 = new ledger_transactions();

                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oBankDeposit.BillDateTime;
                            lt1.bill_type = "BD";
                            lt1.serial_no = i+1;
                            lt1.ledger_code = oBankDeposit.Details.ElementAt(i).LedgerCode;
                            lt1.ledger = oBankDeposit.Details.ElementAt(i).Ledger;
                            lt1.narration = oBankDeposit.Details.ElementAt(i).Narration;
                            lt1.debit = 0;
                            lt1.credit = oBankDeposit.Details.ElementAt(i).Amount;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oBankDeposit.Details.ElementAt(i).LedgerCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oBankDeposit.Details.ElementAt(i).LedgerCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oBankDeposit.Details.ElementAt(i).LedgerCode);
                            lt1.co_ledger = oBankDeposit.Bank;
                            lt1.ref_bill_no = cbillNo.ToString();
                            lt1.ref_bill_date_time = oBankDeposit.BillDateTime;
                            lt1.financial_code = oBankDeposit.FinancialCode;

                            //Second entry
                            ledger_transactions lt2 = new ledger_transactions();

                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oBankDeposit.BillDateTime;
                            lt2.bill_type = "BD";
                            lt2.serial_no = i+2;
                            lt2.ledger_code = oBankDeposit.BankCode;
                            lt2.ledger = oBankDeposit.Bank;
                            lt2.narration = oBankDeposit.Details.ElementAt(i).Narration;
                            lt2.debit = oBankDeposit.Details.ElementAt(i).Amount;
                            lt2.credit = 0;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(oBankDeposit.BankCode);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(oBankDeposit.BankCode);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(oBankDeposit.BankCode);
                            lt2.co_ledger = oBankDeposit.Details.ElementAt(i).Ledger;
                            lt2.ref_bill_no = cbillNo.ToString();
                            lt2.ref_bill_date_time = oBankDeposit.BillDateTime;
                            lt2.financial_code = oBankDeposit.FinancialCode;

                            dataB.ledger_transactions.Add(lt1);
                            dataB.ledger_transactions.Add(lt2);
                        }

                        dataB.SaveChanges();
                        //Success
                        returnValue = true;

                        dataBTransaction.Commit();
                    }
                    catch
                    {
                        dataBTransaction.Rollback();
                    }
                    finally
                    {

                    }
                }                
            }

            return returnValue;
        }

        public bool DeleteBill(string billNo,string financialCode)
        {
            bool returnValue = true;

            lock (Synchronizer.@lock)
            {
                using (var dataB = new AccountsdbEntities())
                {
                    var dataBTransaction = dataB.Database.BeginTransaction();
                    try
                    {
                        //Delete from Cash Receipt
                        var bd = dataB.bank_deposits.Select(c => c).Where(x=>x.bill_no==billNo&&x.financial_code==financialCode);
                        dataB.bank_deposits.RemoveRange(bd);
                                                                        

                        //Delete from Ledger Transaction
                        var lt = dataB.ledger_transactions.Select(c => c).Where(x => x.ref_bill_no == billNo && x.financial_code == financialCode&&x.bill_type=="BD");
                        dataB.ledger_transactions.RemoveRange(lt);

                        dataB.SaveChanges();                        
                        dataBTransaction.Commit();
                    }
                    catch
                    {
                        returnValue = false;
                        dataBTransaction.Rollback();
                    }
                    finally
                    {

                    }
                }
            }
            return returnValue;
        }

        public CBankDeposit ReadBill(string billNo,string financialCode)
        {
            CBankDeposit cbd = null;

            using (var dataB = new AccountsdbEntities())
            {
                var dbs = dataB.bank_deposits.Select(c => c).Where(x => x.bill_no == billNo && x.financial_code == financialCode).OrderBy(y=>y.serial_no);
                
                if (dbs.Count() > 0)
                {
                    cbd = new CBankDeposit();

                    var bd = dbs.FirstOrDefault();
                    cbd.Id = bd.id;
                    cbd.BillNo = bd.bill_no;
                    cbd.BillDateTime = bd.bill_date_time;
                    cbd.FinancialCode = bd.financial_code;
                    cbd.Bank = bd.bank;
                    cbd.BankCode = bd.bank_code;
                    foreach (var item in dbs)
                    {
                        cbd.Details.Add(new CBankDepositDetails() { SerialNo=item.serial_no,LedgerCode=item.ledger_code,Ledger=item.ledger, Narration=item.narration, Amount=item.amount, Status=item.status });
                    }
                }
                
            }

            return cbd;
        }

        public int ReadNextBillNo(string financialCode)
        {
            
            BillNoService bns = new BillNoService();
            return bns.ReadNextBankDepositBillNo(financialCode);
            
        }

        public bool UpdateBill(CBankDeposit oBankDeposit)
        {
            bool returnValue = false;

            lock (Synchronizer.@lock)
            {
                using (var dataB = new AccountsdbEntities())
                {
                    var dataBTransaction = dataB.Database.BeginTransaction();
                    try
                    {                        
                        var bds = dataB.bank_deposits.Select(c => c).Where(x => x.bill_no == oBankDeposit.BillNo&& x.financial_code==oBankDeposit.FinancialCode);
                        dataB.bank_deposits.RemoveRange(bds);                        
                        var lt = dataB.ledger_transactions.Select(c => c).Where(x => x.ref_bill_no == oBankDeposit.BillNo && x.financial_code == oBankDeposit.FinancialCode && x.bill_type == "BD");
                        string ltBillNo = "";
                        foreach (var item in lt)
                        {
                            ltBillNo = item.bill_no;
                            break;
                        }
                        dataB.ledger_transactions.RemoveRange(lt);

                        LedgerService ls = new LedgerService();
                        
                        for (int i = 0; i < oBankDeposit.Details.Count; i++)
                        {
                            bank_deposits bd = new bank_deposits();

                            bd.bill_no = oBankDeposit.BillNo;
                            bd.bill_date_time = oBankDeposit.BillDateTime;
                            bd.serial_no = oBankDeposit.Details.ElementAt(i).SerialNo;
                            bd.ledger_code = oBankDeposit.Details.ElementAt(i).LedgerCode;
                            bd.ledger = oBankDeposit.Details.ElementAt(i).Ledger;
                            bd.narration = oBankDeposit.Details.ElementAt(i).Narration;
                            bd.amount = oBankDeposit.Details.ElementAt(i).Amount;
                            bd.financial_code = oBankDeposit.FinancialCode;
                            bd.status = oBankDeposit.Details.ElementAt(i).Status;
                            bd.bank = oBankDeposit.Bank;
                            bd.bank_code = oBankDeposit.BankCode;

                            dataB.bank_deposits.Add(bd);
                            
                            //Saving to Ledger Transaction
                            //First entry
                            ledger_transactions lt1 = new ledger_transactions();

                            lt1.bill_no = ltBillNo;
                            lt1.bill_date_time = oBankDeposit.BillDateTime;
                            lt1.bill_type = "BD";
                            lt1.serial_no = i+1;
                            lt1.ledger_code = oBankDeposit.Details.ElementAt(i).LedgerCode;
                            lt1.ledger = oBankDeposit.Details.ElementAt(i).Ledger;
                            lt1.narration = oBankDeposit.Details.ElementAt(i).Narration;
                            lt1.debit = 0;
                            lt1.credit = oBankDeposit.Details.ElementAt(i).Amount;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oBankDeposit.Details.ElementAt(i).LedgerCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oBankDeposit.Details.ElementAt(i).LedgerCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oBankDeposit.Details.ElementAt(i).LedgerCode);
                            lt1.co_ledger = oBankDeposit.Bank;
                            lt1.ref_bill_no = oBankDeposit.BillNo;
                            lt1.ref_bill_date_time = oBankDeposit.BillDateTime;
                            lt1.financial_code = oBankDeposit.FinancialCode;

                            //Second entry
                            ledger_transactions lt2 = new ledger_transactions();

                            lt2.bill_no = ltBillNo;
                            lt2.bill_date_time = oBankDeposit.BillDateTime;
                            lt2.bill_type = "BD";
                            lt2.serial_no = i+2;
                            lt2.ledger_code = oBankDeposit.BankCode;
                            lt2.ledger = oBankDeposit.Bank;
                            lt2.narration = oBankDeposit.Details.ElementAt(i).Narration;
                            lt2.debit = oBankDeposit.Details.ElementAt(i).Amount;
                            lt2.credit = 0;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(oBankDeposit.BankCode);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(oBankDeposit.BankCode);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(oBankDeposit.BankCode);
                            lt2.co_ledger = oBankDeposit.Details.ElementAt(i).Ledger;
                            lt2.ref_bill_no = oBankDeposit.BillNo;
                            lt2.ref_bill_date_time = oBankDeposit.BillDateTime;
                            lt2.financial_code = oBankDeposit.FinancialCode;

                            dataB.ledger_transactions.Add(lt1);
                            dataB.ledger_transactions.Add(lt2);
                        }

                        dataB.SaveChanges();
                        //Success
                        returnValue = true;

                        dataBTransaction.Commit();
                    }
                    catch(Exception e)
                    {                        
                        dataBTransaction.Rollback();
                    }
                    finally
                    {

                    }
                }
            }
            return returnValue;
        }
    }
}
