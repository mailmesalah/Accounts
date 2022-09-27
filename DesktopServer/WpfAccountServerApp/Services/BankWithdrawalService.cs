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
   
    public class BankWithdrawalService : IBankWithdrawal
    {
        

        public bool CreateBill(CBankWithdrawal oBankWithdrawal)
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


                        int cbillNo = bs.ReadNextBankWithdrawalBillNo(oBankWithdrawal.FinancialCode);
                        bs.UpdateBankWithdrawalBillNo(oBankWithdrawal.FinancialCode,cbillNo+1);
                        //Saving to Ledger Transaction
                        int ltBillNo = bs.ReadNextLedgerTransactionBillNo(oBankWithdrawal.FinancialCode);
                        bs.UpdateLedgerTransactionBillNo(oBankWithdrawal.FinancialCode, ltBillNo + 1);

                        for (int i = 0; i < oBankWithdrawal.Details.Count; i++)
                        {
                            bank_withdrawals bw = new bank_withdrawals();

                            bw.bill_no = cbillNo.ToString();
                            bw.bill_date_time = oBankWithdrawal.BillDateTime;
                            bw.serial_no = oBankWithdrawal.Details.ElementAt(i).SerialNo;
                            bw.ledger_code= oBankWithdrawal.Details.ElementAt(i).LedgerCode;
                            bw.ledger = oBankWithdrawal.Details.ElementAt(i).Ledger;
                            bw.narration = oBankWithdrawal.Details.ElementAt(i).Narration;
                            bw.amount = oBankWithdrawal.Details.ElementAt(i).Amount;
                            bw.financial_code = oBankWithdrawal.FinancialCode;
                            bw.status = oBankWithdrawal.Details.ElementAt(i).Status;
                            bw.bank = oBankWithdrawal.Bank;
                            bw.bank_code = oBankWithdrawal.BankCode;

                            dataB.bank_withdrawals.Add(bw);
                            
                            //First entry
                            ledger_transactions lt1 = new ledger_transactions();

                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oBankWithdrawal.BillDateTime;
                            lt1.bill_type = "BW";
                            lt1.serial_no = i+1;
                            lt1.ledger_code = oBankWithdrawal.Details.ElementAt(i).LedgerCode;
                            lt1.ledger = oBankWithdrawal.Details.ElementAt(i).Ledger;
                            lt1.narration = oBankWithdrawal.Details.ElementAt(i).Narration;
                            lt1.debit = oBankWithdrawal.Details.ElementAt(i).Amount;
                            lt1.credit = 0;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oBankWithdrawal.Details.ElementAt(i).LedgerCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oBankWithdrawal.Details.ElementAt(i).LedgerCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oBankWithdrawal.Details.ElementAt(i).LedgerCode);
                            lt1.co_ledger = oBankWithdrawal.Bank;
                            lt1.ref_bill_no = cbillNo.ToString();
                            lt1.ref_bill_date_time = oBankWithdrawal.BillDateTime;
                            lt1.financial_code = oBankWithdrawal.FinancialCode;

                            //Second entry
                            ledger_transactions lt2 = new ledger_transactions();

                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oBankWithdrawal.BillDateTime;
                            lt2.bill_type = "BW";
                            lt2.serial_no =i+2;
                            lt2.ledger_code = oBankWithdrawal.BankCode;
                            lt2.ledger = oBankWithdrawal.Bank;
                            lt2.narration = oBankWithdrawal.Details.ElementAt(i).Narration;
                            lt2.debit = 0;
                            lt2.credit = oBankWithdrawal.Details.ElementAt(i).Amount;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(oBankWithdrawal.BankCode);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(oBankWithdrawal.BankCode);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(oBankWithdrawal.BankCode);
                            lt2.co_ledger = oBankWithdrawal.Details.ElementAt(i).Ledger;
                            lt2.ref_bill_no = cbillNo.ToString();
                            lt2.ref_bill_date_time = oBankWithdrawal.BillDateTime;
                            lt2.financial_code = oBankWithdrawal.FinancialCode;

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
                        var bd = dataB.bank_withdrawals.Select(c => c).Where(x=>x.bill_no==billNo&&x.financial_code==financialCode);
                        dataB.bank_withdrawals.RemoveRange(bd);
                                                                        

                        //Delete from Ledger Transaction
                        var lt = dataB.ledger_transactions.Select(c => c).Where(x => x.ref_bill_no== billNo && x.financial_code == financialCode&&x.bill_type=="BW");
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

        public CBankWithdrawal ReadBill(string billNo,string financialCode)
        {
            CBankWithdrawal bwd = null;

            using (var dataB = new AccountsdbEntities())
            {
                var bws = dataB.bank_withdrawals.Select(c => c).Where(x => x.bill_no == billNo && x.financial_code == financialCode).OrderBy(y=>y.serial_no);
                
                if (bws.Count() > 0)
                {
                    bwd = new CBankWithdrawal();

                    var bw = bws.FirstOrDefault();
                    bwd.Id = bw.id;
                    bwd.BillNo = bw.bill_no;
                    bwd.BillDateTime = bw.bill_date_time;
                    bwd.FinancialCode = bw.financial_code;
                    bwd.Bank = bw.bank;
                    bwd.BankCode = bw.bank_code;
                    foreach (var item in bws)
                    {
                        bwd.Details.Add(new CBankWithdrawalDetails() { SerialNo=item.serial_no,LedgerCode=item.ledger_code,Ledger=item.ledger, Narration=item.narration, Amount=item.amount, Status=item.status });
                    }
                }
                
            }

            return bwd;
        }

        public int ReadNextBillNo(string financialCode)
        {
            
            BillNoService bns = new BillNoService();
            return bns.ReadNextBankWithdrawalBillNo(financialCode);
            
        }

        public bool UpdateBill(CBankWithdrawal oBankWithdrawal)
        {
            bool returnValue = false;

            lock (Synchronizer.@lock)
            {
                using (var dataB = new AccountsdbEntities())
                {
                    var dataBTransaction = dataB.Database.BeginTransaction();
                    try
                    {                        
                        var bws = dataB.bank_withdrawals.Select(c => c).Where(x => x.bill_no == oBankWithdrawal.BillNo&& x.financial_code==oBankWithdrawal.FinancialCode);
                        dataB.bank_withdrawals.RemoveRange(bws);                        
                        var lt = dataB.ledger_transactions.Select(c => c).Where(x => x.ref_bill_no == oBankWithdrawal.BillNo && x.financial_code == oBankWithdrawal.FinancialCode && x.bill_type == "BW");
                        string ltBillNo = "";
                        foreach (var item in lt)
                        {
                            ltBillNo = item.bill_no;
                            break;
                        }
                        dataB.ledger_transactions.RemoveRange(lt);

                        LedgerService ls = new LedgerService();
                        
                        for (int i = 0; i < oBankWithdrawal.Details.Count; i++)
                        {
                            bank_withdrawals bw = new bank_withdrawals();

                            bw.bill_no = oBankWithdrawal.BillNo;
                            bw.bill_date_time = oBankWithdrawal.BillDateTime;
                            bw.serial_no = oBankWithdrawal.Details.ElementAt(i).SerialNo;
                            bw.ledger_code = oBankWithdrawal.Details.ElementAt(i).LedgerCode;
                            bw.ledger = oBankWithdrawal.Details.ElementAt(i).Ledger;
                            bw.narration = oBankWithdrawal.Details.ElementAt(i).Narration;
                            bw.amount = oBankWithdrawal.Details.ElementAt(i).Amount;
                            bw.financial_code = oBankWithdrawal.FinancialCode;
                            bw.status = oBankWithdrawal.Details.ElementAt(i).Status;
                            bw.bank = oBankWithdrawal.Bank;
                            bw.bank_code = oBankWithdrawal.BankCode;

                            dataB.bank_withdrawals.Add(bw);
                            
                            //Saving to Ledger Transaction
                            //First entry
                            ledger_transactions lt1 = new ledger_transactions();

                            lt1.bill_no = ltBillNo;
                            lt1.bill_date_time = oBankWithdrawal.BillDateTime;
                            lt1.bill_type = "BW";
                            lt1.serial_no = i+1;
                            lt1.ledger_code = oBankWithdrawal.Details.ElementAt(i).LedgerCode;
                            lt1.ledger = oBankWithdrawal.Details.ElementAt(i).Ledger;
                            lt1.narration = oBankWithdrawal.Details.ElementAt(i).Narration;
                            lt1.debit = oBankWithdrawal.Details.ElementAt(i).Amount;
                            lt1.credit = 0;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oBankWithdrawal.Details.ElementAt(i).LedgerCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oBankWithdrawal.Details.ElementAt(i).LedgerCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oBankWithdrawal.Details.ElementAt(i).LedgerCode);
                            lt1.co_ledger = oBankWithdrawal.Bank;
                            lt1.ref_bill_no = oBankWithdrawal.BillNo;
                            lt1.ref_bill_date_time = oBankWithdrawal.BillDateTime;
                            lt1.financial_code = oBankWithdrawal.FinancialCode;

                            //Second entry
                            ledger_transactions lt2 = new ledger_transactions();

                            lt2.bill_no = ltBillNo;
                            lt2.bill_date_time = oBankWithdrawal.BillDateTime;
                            lt2.bill_type = "BW";
                            lt2.serial_no = i+2;
                            lt2.ledger_code = oBankWithdrawal.BankCode;
                            lt2.ledger = oBankWithdrawal.Bank;
                            lt2.narration = oBankWithdrawal.Details.ElementAt(i).Narration;
                            lt2.debit = 0;
                            lt2.credit = oBankWithdrawal.Details.ElementAt(i).Amount;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(oBankWithdrawal.BankCode);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(oBankWithdrawal.BankCode);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(oBankWithdrawal.BankCode);
                            lt2.co_ledger = oBankWithdrawal.Details.ElementAt(i).Ledger;
                            lt2.ref_bill_no = oBankWithdrawal.BillNo;
                            lt2.ref_bill_date_time = oBankWithdrawal.BillDateTime;
                            lt2.financial_code = oBankWithdrawal.FinancialCode;

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
