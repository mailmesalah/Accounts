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
   
    public class PurchaseService : IPurchase
    {
        

        public bool CreateBill(CPurchase oPurchase)
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


                        int cbillNo = bs.ReadNextPurchaseBillNo(oPurchase.FinancialCode);
                        bs.UpdatePurchaseBillNo(oPurchase.FinancialCode,cbillNo+1);
                        
                        int ltBillNo = bs.ReadNextLedgerTransactionBillNo(oPurchase.FinancialCode);
                        bs.UpdateLedgerTransactionBillNo(oPurchase.FinancialCode, ltBillNo + 1);
                        //Saving bill Amount
                        ledger_transactions lt1 = new ledger_transactions();
                        ledger_transactions lt2 = new ledger_transactions();

                        int serNo = 0;
                        if (oPurchase.BillAmount > 0)
                        {
                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oPurchase.BillDateTime;
                            lt1.bill_type = "P";
                            lt1.serial_no = ++serNo;
                            lt1.ledger_code = oPurchase.SupplierCode;
                            lt1.ledger = oPurchase.Supplier;
                            lt1.narration = oPurchase.Narration;
                            lt1.debit = 0;
                            lt1.credit = oPurchase.BillAmount; ;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oPurchase.SupplierCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oPurchase.SupplierCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oPurchase.SupplierCode);
                            lt1.co_ledger = "Purchase Account";
                            lt1.ref_bill_no = cbillNo.ToString();
                            lt1.ref_bill_date_time = oPurchase.BillDateTime;
                            lt1.financial_code = oPurchase.FinancialCode;

                            //Second entry
                            
                            ++serNo;
                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oPurchase.BillDateTime;
                            lt2.bill_type = "P";
                            lt2.serial_no = serNo;
                            lt2.ledger_code = UniqueLedgers.LedgerCode["Purchase Account"];
                            lt2.ledger = "Purchase Account";
                            lt2.narration = oPurchase.Narration;
                            lt2.debit = oPurchase.BillAmount;
                            lt2.credit = 0;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(UniqueLedgers.LedgerCode["Purchase Account"]);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(UniqueLedgers.LedgerCode["Purchase Account"]);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(UniqueLedgers.LedgerCode["Purchase Account"]);
                            lt2.co_ledger = oPurchase.Supplier;
                            lt2.ref_bill_no = cbillNo.ToString();
                            lt2.ref_bill_date_time = oPurchase.BillDateTime;
                            lt2.financial_code = oPurchase.FinancialCode;

                            dataB.ledger_transactions.Add(lt1);
                            dataB.ledger_transactions.Add(lt2);
                        }

                        for (int i = 0; i < oPurchase.Details.Count; i++)
                        {
                            purchase p = new purchase();

                            p.bill_no = cbillNo.ToString();
                            p.bill_date_time = oPurchase.BillDateTime;
                            p.serial_no = oPurchase.Details.ElementAt(i).SerialNo;
                            p.ledger_code= oPurchase.Details.ElementAt(i).LedgerCode;
                            p.ledger = oPurchase.Details.ElementAt(i).Ledger;
                            p.narration = oPurchase.Narration;
                            p.debit = oPurchase.Details.ElementAt(i).Debit;
                            p.credit = oPurchase.Details.ElementAt(i).Credit;
                            p.financial_code = oPurchase.FinancialCode;
                            p.bill_amount = oPurchase.BillAmount;
                            p.party = oPurchase.Supplier;
                            p.party_code = oPurchase.SupplierCode;
                            p.ref_bill_date_time = oPurchase.RefDateTime;
                            p.ref_bill_no = oPurchase.RefBillNo;
                            
                            dataB.purchase.Add(p);


                            //Saving to Ledger Transaction
                            //First entry
                            lt1 = new ledger_transactions();
                            
                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oPurchase.BillDateTime;
                            lt1.bill_type = "P";
                            lt1.serial_no = ++serNo;
                            lt1.ledger_code = oPurchase.Details.ElementAt(i).LedgerCode;
                            lt1.ledger = oPurchase.Details.ElementAt(i).Ledger;
                            lt1.narration = oPurchase.Narration;
                            lt1.debit = oPurchase.Details.ElementAt(i).Debit;
                            lt1.credit = oPurchase.Details.ElementAt(i).Credit;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oPurchase.Details.ElementAt(i).LedgerCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oPurchase.Details.ElementAt(i).LedgerCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oPurchase.Details.ElementAt(i).LedgerCode);
                            lt1.co_ledger = oPurchase.Supplier;
                            lt1.ref_bill_no = cbillNo.ToString();
                            lt1.ref_bill_date_time = oPurchase.BillDateTime;
                            lt1.financial_code = oPurchase.FinancialCode;

                            //Second entry
                            lt2 = new ledger_transactions();

                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oPurchase.BillDateTime;
                            lt2.bill_type = "P";
                            lt2.serial_no = ++serNo;
                            lt2.ledger_code = oPurchase.SupplierCode;
                            lt2.ledger = oPurchase.Supplier;
                            lt2.narration = oPurchase.Narration;
                            lt2.debit = oPurchase.Details.ElementAt(i).Credit;
                            lt2.credit = oPurchase.Details.ElementAt(i).Debit;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(oPurchase.SupplierCode);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(oPurchase.SupplierCode);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(oPurchase.SupplierCode);
                            lt2.co_ledger = oPurchase.Details.ElementAt(i).Ledger;
                            lt2.ref_bill_no = cbillNo.ToString();
                            lt2.ref_bill_date_time = oPurchase.BillDateTime;
                            lt2.financial_code = oPurchase.FinancialCode;

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
                        var p = dataB.purchase.Select(c => c).Where(x=>x.bill_no==billNo&&x.financial_code==financialCode);
                        dataB.purchase.RemoveRange(p);
                                                                        

                        //Delete from Ledger Transaction
                        var lt = dataB.ledger_transactions.Select(c => c).Where(x => x.ref_bill_no == billNo && x.financial_code == financialCode&&x.bill_type=="P");
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

        public CPurchase ReadBill(string billNo,string financialCode)
        {
            CPurchase ps = null;

            using (var dataB = new AccountsdbEntities())
            {
                var psr = dataB.purchase.Select(c => c).Where(x => x.bill_no == billNo && x.financial_code == financialCode).OrderBy(y=>y.serial_no);
                
                if (psr.Count() > 0)
                {
                    ps = new CPurchase();

                    var p = psr.FirstOrDefault();
                    ps.Id = p.id;
                    ps.BillNo = p.bill_no;
                    ps.BillDateTime = p.bill_date_time;
                    ps.RefBillNo = p.ref_bill_no;
                    ps.RefDateTime = p.ref_bill_date_time;
                    ps.FinancialCode = p.financial_code;
                    ps.Supplier = p.party;
                    ps.SupplierCode = p.party_code;
                    ps.Narration = p.narration;
                    ps.BillAmount = p.bill_amount;
                    
                    foreach (var item in psr)
                    {                        
                        ps.Details.Add(new CPurchaseDetails() { SerialNo=item.serial_no,LedgerCode=item.ledger_code,Ledger=item.ledger,  Debit=item.debit, Credit=item.credit });
                    }
                }
                
            }

            return ps;
        }

        public int ReadNextBillNo(string financialCode)
        {
            
            BillNoService bns = new BillNoService();
            return bns.ReadNextPurchaseBillNo(financialCode);
            
        }

        public bool UpdateBill(CPurchase oPurchase)
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

                        var ps = dataB.purchase_return.Select(c => c).Where(x => x.bill_no == oPurchase.BillNo&& x.financial_code==oPurchase.FinancialCode);
                        dataB.purchase_return.RemoveRange(ps);                        
                        var lt = dataB.ledger_transactions.Select(c => c).Where(x => x.ref_bill_no == oPurchase.BillNo && x.financial_code == oPurchase.FinancialCode && x.bill_type == "P");
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
                        if(oPurchase.BillAmount > 0){
                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oPurchase.BillDateTime;
                            lt1.bill_type = "P";
                            lt1.serial_no = ++serNo;
                            lt1.ledger_code = oPurchase.SupplierCode;
                            lt1.ledger = oPurchase.Supplier;
                            lt1.narration = oPurchase.Narration;
                            lt1.debit = 0;
                            lt1.credit = oPurchase.BillAmount;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oPurchase.SupplierCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oPurchase.SupplierCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oPurchase.SupplierCode);
                            lt1.co_ledger = "Purchase Account";
                            lt1.ref_bill_no = oPurchase.BillNo;
                            lt1.ref_bill_date_time = oPurchase.BillDateTime;
                            lt1.financial_code = oPurchase.FinancialCode;

                            ++serNo;
                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oPurchase.BillDateTime;
                            lt2.bill_type = "P";
                            lt2.serial_no = serNo;
                            lt2.ledger_code = UniqueLedgers.LedgerCode["Purchase Account"];
                            lt2.ledger = "Purchase Account";
                            lt2.narration = oPurchase.Narration;
                            lt2.debit = oPurchase.BillAmount;
                            lt2.credit = 0;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(UniqueLedgers.LedgerCode["Purchase Account"]);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(UniqueLedgers.LedgerCode["Purchase Account"]);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(UniqueLedgers.LedgerCode["Purchase Account"]);
                            lt2.co_ledger = oPurchase.Supplier;
                            lt2.ref_bill_no = oPurchase.BillNo;
                            lt2.ref_bill_date_time = oPurchase.BillDateTime;
                            lt2.financial_code = oPurchase.FinancialCode;

                            dataB.ledger_transactions.Add(lt1);
                            dataB.ledger_transactions.Add(lt2);
                        }

                        for (int i = 0; i < oPurchase.Details.Count; i++)
                        {
                            purchase p = new purchase();

                            p.bill_no = oPurchase.BillNo;
                            p.bill_date_time = oPurchase.BillDateTime;
                            p.serial_no = oPurchase.Details.ElementAt(i).SerialNo;
                            p.ledger_code = oPurchase.Details.ElementAt(i).LedgerCode;
                            p.ledger = oPurchase.Details.ElementAt(i).Ledger;
                            p.narration = oPurchase.Narration;
                            p.debit = oPurchase.Details.ElementAt(i).Debit;
                            p.credit = oPurchase.Details.ElementAt(i).Credit;
                            p.financial_code = oPurchase.FinancialCode;
                            p.bill_amount = oPurchase.BillAmount;
                            p.party = oPurchase.Supplier;
                            p.party_code = oPurchase.SupplierCode;
                            p.ref_bill_date_time = oPurchase.RefDateTime;
                            p.ref_bill_no = oPurchase.RefBillNo;
                            
                            dataB.purchase.Add(p);

                            //Saving to Ledger Transaction
                            //First entry
                            lt1 = new ledger_transactions();

                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oPurchase.BillDateTime;
                            lt1.bill_type = "P";
                            lt1.serial_no = ++serNo;
                            lt1.ledger_code = oPurchase.Details.ElementAt(i).LedgerCode;
                            lt1.ledger = oPurchase.Details.ElementAt(i).Ledger;
                            lt1.narration = oPurchase.Narration;
                            lt1.debit = oPurchase.Details.ElementAt(i).Debit;
                            lt1.credit = oPurchase.Details.ElementAt(i).Credit;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oPurchase.Details.ElementAt(i).LedgerCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oPurchase.Details.ElementAt(i).LedgerCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oPurchase.Details.ElementAt(i).LedgerCode);
                            lt1.co_ledger = oPurchase.Supplier;
                            lt1.ref_bill_no = oPurchase.BillNo;
                            lt1.ref_bill_date_time = oPurchase.BillDateTime;
                            lt1.financial_code = oPurchase.FinancialCode;

                            //Second entry
                            lt2 = new ledger_transactions();

                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oPurchase.BillDateTime;
                            lt2.bill_type = "P";
                            lt2.serial_no = ++serNo;
                            lt2.ledger_code = oPurchase.SupplierCode;
                            lt2.ledger = oPurchase.Supplier;
                            lt2.narration = oPurchase.Narration;
                            lt2.debit = oPurchase.Details.ElementAt(i).Credit;
                            lt2.credit = oPurchase.Details.ElementAt(i).Debit;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(oPurchase.SupplierCode);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(oPurchase.SupplierCode);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(oPurchase.SupplierCode);
                            lt2.co_ledger = oPurchase.Details.ElementAt(i).Ledger;
                            lt2.ref_bill_no = oPurchase.BillNo;
                            lt2.ref_bill_date_time = oPurchase.BillDateTime;
                            lt2.financial_code = oPurchase.FinancialCode;

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
