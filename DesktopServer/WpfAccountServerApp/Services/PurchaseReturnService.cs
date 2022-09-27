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
   
    public class PurchaseReturnService : IPurchaseReturn
    {
        

        public bool CreateBill(CPurchaseReturn oPurchaseReturn)
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


                        int cbillNo = bs.ReadNextPurchaseReturnBillNo(oPurchaseReturn.FinancialCode);
                        bs.UpdatePurchaseReturnBillNo(oPurchaseReturn.FinancialCode,cbillNo+1);
                        
                        int ltBillNo = bs.ReadNextLedgerTransactionBillNo(oPurchaseReturn.FinancialCode);
                        bs.UpdateLedgerTransactionBillNo(oPurchaseReturn.FinancialCode, ltBillNo + 1);
                        //Saving bill Amount
                        ledger_transactions lt1 = new ledger_transactions();
                        ledger_transactions lt2 = new ledger_transactions();

                        int serNo = 0;

                        if (oPurchaseReturn.BillAmount > 0)
                        {
                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oPurchaseReturn.BillDateTime;
                            lt1.bill_type = "PR";
                            lt1.serial_no = ++serNo;
                            lt1.ledger_code = oPurchaseReturn.SupplierCode;
                            lt1.ledger = oPurchaseReturn.Supplier;
                            lt1.narration = oPurchaseReturn.Narration;
                            lt1.debit = oPurchaseReturn.BillAmount;
                            lt1.credit = 0;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oPurchaseReturn.SupplierCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oPurchaseReturn.SupplierCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oPurchaseReturn.SupplierCode);
                            lt1.co_ledger = "Purchase Account";
                            lt1.ref_bill_no = cbillNo.ToString();
                            lt1.ref_bill_date_time = oPurchaseReturn.BillDateTime;
                            lt1.financial_code = oPurchaseReturn.FinancialCode;


                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oPurchaseReturn.BillDateTime;
                            lt2.bill_type = "PR";
                            lt2.serial_no = ++serNo;
                            lt2.ledger_code = UniqueLedgers.LedgerCode["Purchase Account"];
                            lt2.ledger = "Purchase Account";
                            lt2.narration = oPurchaseReturn.Narration;
                            lt2.debit = 0;
                            lt2.credit = oPurchaseReturn.BillAmount;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(UniqueLedgers.LedgerCode["Purchase Account"]);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(UniqueLedgers.LedgerCode["Purchase Account"]);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(UniqueLedgers.LedgerCode["Purchase Account"]);
                            lt2.co_ledger = oPurchaseReturn.Supplier;
                            lt2.ref_bill_no = cbillNo.ToString();
                            lt2.ref_bill_date_time = oPurchaseReturn.BillDateTime;
                            lt2.financial_code = oPurchaseReturn.FinancialCode;

                            dataB.ledger_transactions.Add(lt1);
                            dataB.ledger_transactions.Add(lt2);
                        }

                        for (int i = 0; i < oPurchaseReturn.Details.Count; i++)
                        {
                            purchase_return pr = new purchase_return();

                            pr.bill_no = cbillNo.ToString();
                            pr.bill_date_time = oPurchaseReturn.BillDateTime;
                            pr.serial_no = oPurchaseReturn.Details.ElementAt(i).SerialNo;
                            pr.ledger_code= oPurchaseReturn.Details.ElementAt(i).LedgerCode;
                            pr.ledger = oPurchaseReturn.Details.ElementAt(i).Ledger;
                            pr.narration = oPurchaseReturn.Narration;
                            pr.debit = oPurchaseReturn.Details.ElementAt(i).Debit;
                            pr.credit = oPurchaseReturn.Details.ElementAt(i).Credit;
                            pr.financial_code = oPurchaseReturn.FinancialCode;
                            pr.bill_amount = oPurchaseReturn.BillAmount;
                            pr.party = oPurchaseReturn.Supplier;
                            pr.party_code = oPurchaseReturn.SupplierCode;
                            pr.ref_bill_date_time = oPurchaseReturn.RefDateTime;
                            pr.ref_bill_no = oPurchaseReturn.RefBillNo;
                            
                            dataB.purchase_return.Add(pr);


                            //Saving to Ledger Transaction
                            //First entry
                            lt1 = new ledger_transactions();

                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oPurchaseReturn.BillDateTime;
                            lt1.bill_type = "PR";
                            lt1.serial_no = ++serNo;
                            lt1.ledger_code = oPurchaseReturn.Details.ElementAt(i).LedgerCode;
                            lt1.ledger = oPurchaseReturn.Details.ElementAt(i).Ledger;
                            lt1.narration = oPurchaseReturn.Narration;
                            lt1.debit = oPurchaseReturn.Details.ElementAt(i).Debit;
                            lt1.credit = oPurchaseReturn.Details.ElementAt(i).Credit;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oPurchaseReturn.Details.ElementAt(i).LedgerCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oPurchaseReturn.Details.ElementAt(i).LedgerCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oPurchaseReturn.Details.ElementAt(i).LedgerCode);
                            lt1.co_ledger = oPurchaseReturn.Supplier;
                            lt1.ref_bill_no = cbillNo.ToString();
                            lt1.ref_bill_date_time = oPurchaseReturn.BillDateTime;
                            lt1.financial_code = oPurchaseReturn.FinancialCode;

                            //Second entry
                            lt2 = new ledger_transactions();

                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oPurchaseReturn.BillDateTime;
                            lt2.bill_type = "PR";
                            lt2.serial_no = ++serNo;
                            lt2.ledger_code = oPurchaseReturn.SupplierCode;
                            lt2.ledger = oPurchaseReturn.Supplier;
                            lt2.narration = oPurchaseReturn.Narration;
                            lt2.debit = oPurchaseReturn.Details.ElementAt(i).Credit;
                            lt2.credit = oPurchaseReturn.Details.ElementAt(i).Debit;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(oPurchaseReturn.SupplierCode);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(oPurchaseReturn.SupplierCode);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(oPurchaseReturn.SupplierCode);
                            lt2.co_ledger = oPurchaseReturn.Details.ElementAt(i).Ledger;
                            lt2.ref_bill_no = cbillNo.ToString();
                            lt2.ref_bill_date_time = oPurchaseReturn.BillDateTime;
                            lt2.financial_code = oPurchaseReturn.FinancialCode;

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
                        var pr = dataB.purchase_return.Select(c => c).Where(x=>x.bill_no==billNo&&x.financial_code==financialCode);
                        dataB.purchase_return.RemoveRange(pr);
                                                                        

                        //Delete from Ledger Transaction
                        var lt = dataB.ledger_transactions.Select(c => c).Where(x => x.ref_bill_no == billNo && x.financial_code == financialCode&&x.bill_type=="PR");
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

        public CPurchaseReturn ReadBill(string billNo,string financialCode)
        {
            CPurchaseReturn pls = null;

            using (var dataB = new AccountsdbEntities())
            {
                var plr = dataB.purchase_return.Select(c => c).Where(x => x.bill_no == billNo && x.financial_code == financialCode).OrderBy(y=>y.serial_no);
                
                if (plr.Count() > 0)
                {
                    pls = new CPurchaseReturn();

                    var pr = plr.FirstOrDefault();
                    pls.Id = pr.id;
                    pls.BillNo = pr.bill_no;
                    pls.BillDateTime = pr.bill_date_time;
                    pls.RefBillNo = pr.ref_bill_no;
                    pls.RefDateTime = pr.ref_bill_date_time;
                    pls.FinancialCode = pr.financial_code;
                    pls.Supplier = pr.party;
                    pls.SupplierCode = pr.party_code;
                    pls.Narration = pr.narration;
                    pls.BillAmount = pr.bill_amount;
                    
                    foreach (var item in plr)
                    {                        
                        pls.Details.Add(new CPurchaseReturnDetails() { SerialNo=item.serial_no,LedgerCode=item.ledger_code,Ledger=item.ledger,  Debit=item.debit, Credit=item.credit });
                    }
                }
                
            }

            return pls;
        }

        public int ReadNextBillNo(string financialCode)
        {
            
            BillNoService bns = new BillNoService();
            return bns.ReadNextPurchaseReturnBillNo(financialCode);
            
        }

        public bool UpdateBill(CPurchaseReturn oPurchaseReturn)
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

                        var prs = dataB.purchase_return.Select(c => c).Where(x => x.bill_no == oPurchaseReturn.BillNo&& x.financial_code==oPurchaseReturn.FinancialCode);
                        dataB.purchase_return.RemoveRange(prs);                        
                        var lt = dataB.ledger_transactions.Select(c => c).Where(x => x.ref_bill_no == oPurchaseReturn.BillNo && x.financial_code == oPurchaseReturn.FinancialCode && x.bill_type == "PR");
                        string ltBillNo = "";
                        foreach (var item in lt)
                        {
                            ltBillNo = item.bill_no;
                            break;
                        }
                        dataB.ledger_transactions.RemoveRange(lt);

                        ledger_transactions lt1 = new ledger_transactions();
                        ledger_transactions lt2 = new ledger_transactions();

                        int serNo = 0;
                        if (oPurchaseReturn.BillAmount > 0)
                        {
                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oPurchaseReturn.BillDateTime;
                            lt1.bill_type = "PR";
                            lt1.serial_no = ++serNo;
                            lt1.ledger_code = oPurchaseReturn.SupplierCode;
                            lt1.ledger = oPurchaseReturn.Supplier;
                            lt1.narration = oPurchaseReturn.Narration;
                            lt1.debit = oPurchaseReturn.BillAmount;
                            lt1.credit = 0;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oPurchaseReturn.SupplierCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oPurchaseReturn.SupplierCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oPurchaseReturn.SupplierCode);
                            lt1.co_ledger = "Purchase Account";
                            lt1.ref_bill_no = oPurchaseReturn.BillNo;
                            lt1.ref_bill_date_time = oPurchaseReturn.BillDateTime;
                            lt1.financial_code = oPurchaseReturn.FinancialCode;

                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oPurchaseReturn.BillDateTime;
                            lt2.bill_type = "PR";
                            lt2.serial_no = ++serNo;
                            lt2.ledger_code = UniqueLedgers.LedgerCode["Purchase Account"];
                            lt2.ledger = "Purchase Account";
                            lt2.narration = oPurchaseReturn.Narration;
                            lt2.debit = 0;
                            lt2.credit = oPurchaseReturn.BillAmount;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(UniqueLedgers.LedgerCode["Purchase Account"]);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(UniqueLedgers.LedgerCode["Purchase Account"]);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(UniqueLedgers.LedgerCode["Purchase Account"]);
                            lt2.co_ledger = oPurchaseReturn.Supplier;
                            lt2.ref_bill_no = oPurchaseReturn.BillNo;
                            lt2.ref_bill_date_time = oPurchaseReturn.BillDateTime;
                            lt2.financial_code = oPurchaseReturn.FinancialCode;

                            dataB.ledger_transactions.Add(lt1);
                            dataB.ledger_transactions.Add(lt2);
                        }

                        for (int i = 0; i < oPurchaseReturn.Details.Count; i++)
                        {
                            purchase_return pr = new purchase_return();

                            pr.bill_no = oPurchaseReturn.BillNo;
                            pr.bill_date_time = oPurchaseReturn.BillDateTime;
                            pr.serial_no = oPurchaseReturn.Details.ElementAt(i).SerialNo;
                            pr.ledger_code = oPurchaseReturn.Details.ElementAt(i).LedgerCode;
                            pr.ledger = oPurchaseReturn.Details.ElementAt(i).Ledger;
                            pr.narration = oPurchaseReturn.Narration;
                            pr.debit = oPurchaseReturn.Details.ElementAt(i).Debit;
                            pr.credit = oPurchaseReturn.Details.ElementAt(i).Credit;
                            pr.financial_code = oPurchaseReturn.FinancialCode;
                            pr.bill_amount = oPurchaseReturn.BillAmount;
                            pr.party = oPurchaseReturn.Supplier;
                            pr.party_code = oPurchaseReturn.SupplierCode;
                            pr.ref_bill_date_time = oPurchaseReturn.RefDateTime;
                            pr.ref_bill_no = oPurchaseReturn.RefBillNo;
                            
                            dataB.purchase_return.Add(pr);

                            //Saving to Ledger Transaction
                            //First entry
                            lt1 = new ledger_transactions();

                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oPurchaseReturn.BillDateTime;
                            lt1.bill_type = "PR";
                            lt1.serial_no = ++serNo;
                            lt1.ledger_code = oPurchaseReturn.Details.ElementAt(i).LedgerCode;
                            lt1.ledger = oPurchaseReturn.Details.ElementAt(i).Ledger;
                            lt1.narration = oPurchaseReturn.Narration;
                            lt1.debit = oPurchaseReturn.Details.ElementAt(i).Debit;
                            lt1.credit = oPurchaseReturn.Details.ElementAt(i).Credit;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oPurchaseReturn.Details.ElementAt(i).LedgerCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oPurchaseReturn.Details.ElementAt(i).LedgerCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oPurchaseReturn.Details.ElementAt(i).LedgerCode);
                            lt1.co_ledger = oPurchaseReturn.Supplier;
                            lt1.ref_bill_no = oPurchaseReturn.BillNo;
                            lt1.ref_bill_date_time = oPurchaseReturn.BillDateTime;
                            lt1.financial_code = oPurchaseReturn.FinancialCode;

                            //Second entry
                            lt2 = new ledger_transactions();

                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oPurchaseReturn.BillDateTime;
                            lt2.bill_type = "PR";
                            lt2.serial_no = ++serNo;
                            lt2.ledger_code = oPurchaseReturn.SupplierCode;
                            lt2.ledger = oPurchaseReturn.Supplier;
                            lt2.narration = oPurchaseReturn.Narration;
                            lt2.debit = oPurchaseReturn.Details.ElementAt(i).Credit;
                            lt2.credit = oPurchaseReturn.Details.ElementAt(i).Debit;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(oPurchaseReturn.SupplierCode);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(oPurchaseReturn.SupplierCode);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(oPurchaseReturn.SupplierCode);
                            lt2.co_ledger = oPurchaseReturn.Details.ElementAt(i).Ledger;
                            lt2.ref_bill_no = oPurchaseReturn.BillNo;
                            lt2.ref_bill_date_time = oPurchaseReturn.BillDateTime;
                            lt2.financial_code = oPurchaseReturn.FinancialCode;

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
