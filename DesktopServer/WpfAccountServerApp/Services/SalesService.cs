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
   
    public class SalesService : ISales
    {
        

        public bool CreateBill(CSales oSales)
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


                        int cbillNo = bs.ReadNextSalesBillNo(oSales.FinancialCode);
                        bs.UpdateSalesBillNo(oSales.FinancialCode,cbillNo+1);
                        
                        int ltBillNo = bs.ReadNextLedgerTransactionBillNo(oSales.FinancialCode);
                        bs.UpdateLedgerTransactionBillNo(oSales.FinancialCode, ltBillNo + 1);
                        //Saving bill Amount
                        ledger_transactions lt1 = new ledger_transactions();
                        ledger_transactions lt2 = new ledger_transactions();

                        int serNo = 0;

                        lt1.bill_no = ltBillNo.ToString();
                        lt1.bill_date_time = oSales.BillDateTime;
                        lt1.bill_type = "S";
                        lt1.serial_no = ++serNo;
                        lt1.ledger_code = oSales.CustomerCode;
                        lt1.ledger = oSales.Customer;
                        lt1.narration = oSales.Narration;
                        lt1.debit = oSales.BillAmount;
                        lt1.credit = 0;
                        lt1.a_group_code = ls.ReadAGroupCodeOf(oSales.CustomerCode);
                        lt1.b_group_code = ls.ReadBGroupCodeOf(oSales.CustomerCode);
                        lt1.c_group_code = ls.ReadCGroupCodeOf(oSales.CustomerCode);
                        lt1.co_ledger = "Sales Account";
                        lt1.ref_bill_no = cbillNo.ToString();
                        lt1.ref_bill_date_time = oSales.BillDateTime;
                        lt1.financial_code = oSales.FinancialCode;
                        
                        lt2.bill_no = ltBillNo.ToString();
                        lt2.bill_date_time = oSales.BillDateTime;
                        lt2.bill_type = "S";
                        lt2.serial_no = ++serNo;
                        lt2.ledger_code = UniqueLedgers.LedgerCode["Sales Account"];
                        lt2.ledger = "Sales Account";
                        lt2.narration = oSales.Narration;
                        lt2.debit = 0;
                        lt2.credit = oSales.BillAmount;
                        lt2.a_group_code = ls.ReadAGroupCodeOf(UniqueLedgers.LedgerCode["Sales Account"]);
                        lt2.b_group_code = ls.ReadBGroupCodeOf(UniqueLedgers.LedgerCode["Sales Account"]);
                        lt2.c_group_code = ls.ReadCGroupCodeOf(UniqueLedgers.LedgerCode["Sales Account"]);
                        lt2.co_ledger = oSales.Customer;
                        lt2.ref_bill_no = cbillNo.ToString();
                        lt2.ref_bill_date_time = oSales.BillDateTime;
                        lt2.financial_code = oSales.FinancialCode;

                        dataB.ledger_transactions.Add(lt1);
                        dataB.ledger_transactions.Add(lt2);


                        for (int i = 0; i < oSales.Details.Count; i++)
                        {
                            sales sl = new sales();

                            sl.bill_no = cbillNo.ToString();
                            sl.bill_date_time = oSales.BillDateTime;
                            sl.serial_no = oSales.Details.ElementAt(i).SerialNo;
                            sl.ledger_code= oSales.Details.ElementAt(i).LedgerCode;
                            sl.ledger = oSales.Details.ElementAt(i).Ledger;
                            sl.narration = oSales.Narration;
                            sl.debit = oSales.Details.ElementAt(i).Debit;
                            sl.credit = oSales.Details.ElementAt(i).Credit;
                            sl.financial_code = oSales.FinancialCode;
                            sl.bill_amount = oSales.BillAmount;
                            sl.party = oSales.Customer;
                            sl.party_code = oSales.CustomerCode;
                            sl.ref_bill_date_time = oSales.RefDateTime;
                            sl.ref_bill_no = oSales.RefBillNo;
                            
                            dataB.sales.Add(sl);


                            //Saving to Ledger Transaction
                            //First entry
                            lt1 = new ledger_transactions();

                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oSales.BillDateTime;
                            lt1.bill_type = "S";
                            lt1.serial_no = ++serNo;
                            lt1.ledger_code = oSales.Details.ElementAt(i).LedgerCode;
                            lt1.ledger = oSales.Details.ElementAt(i).Ledger;
                            lt1.narration = oSales.Narration;
                            lt1.debit = oSales.Details.ElementAt(i).Debit;
                            lt1.credit = oSales.Details.ElementAt(i).Credit;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oSales.Details.ElementAt(i).LedgerCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oSales.Details.ElementAt(i).LedgerCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oSales.Details.ElementAt(i).LedgerCode);
                            lt1.co_ledger = oSales.Customer;
                            lt1.ref_bill_no = cbillNo.ToString();
                            lt1.ref_bill_date_time = oSales.BillDateTime;
                            lt1.financial_code = oSales.FinancialCode;
                            
                            lt2 = new ledger_transactions();

                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oSales.BillDateTime;
                            lt2.bill_type = "S";
                            lt2.serial_no = ++serNo;
                            lt2.ledger_code = oSales.CustomerCode;
                            lt2.ledger = oSales.Customer;
                            lt2.narration = oSales.Narration;
                            lt2.debit = oSales.Details.ElementAt(i).Credit;
                            lt2.credit = oSales.Details.ElementAt(i).Debit;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(oSales.CustomerCode);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(oSales.CustomerCode);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(oSales.CustomerCode);
                            lt2.co_ledger = oSales.Details.ElementAt(i).Ledger;
                            lt2.ref_bill_no = cbillNo.ToString();
                            lt2.ref_bill_date_time = oSales.BillDateTime;
                            lt2.financial_code = oSales.FinancialCode;

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
                        var sl = dataB.sales.Select(c => c).Where(x=>x.bill_no==billNo&&x.financial_code==financialCode);
                        dataB.sales.RemoveRange(sl);
                                                                        

                        //Delete from Ledger Transaction
                        var lt = dataB.ledger_transactions.Select(c => c).Where(x => x.ref_bill_no == billNo && x.financial_code == financialCode&&x.bill_type=="S");
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

        public CSales ReadBill(string billNo,string financialCode)
        {
            CSales sls = null;

            using (var dataB = new AccountsdbEntities())
            {
                var slr = dataB.sales.Select(c => c).Where(x => x.bill_no == billNo && x.financial_code == financialCode).OrderBy(y=>y.serial_no);
                
                if (slr.Count() > 0)
                {
                    sls = new CSales();

                    var sl = slr.FirstOrDefault();
                    sls.Id = sl.id;
                    sls.BillNo = sl.bill_no;
                    sls.BillDateTime = sl.bill_date_time;
                    sls.RefBillNo = sl.ref_bill_no;
                    sls.RefDateTime = sl.ref_bill_date_time;
                    sls.FinancialCode = sl.financial_code;
                    sls.Customer = sl.party;
                    sls.CustomerCode = sl.party_code;
                    sls.Narration = sl.narration;
                    sls.BillAmount = sl.bill_amount;
                    
                    foreach (var item in slr)
                    {                        
                        sls.Details.Add(new CSalesDetails() { SerialNo=item.serial_no,LedgerCode=item.ledger_code,Ledger=item.ledger,  Debit=item.debit, Credit=item.credit });
                    }
                }
                
            }

            return sls;
        }

        public int ReadNextBillNo(string financialCode)
        {
            
            BillNoService bns = new BillNoService();
            return bns.ReadNextSalesBillNo(financialCode);
            
        }

        public bool UpdateBill(CSales oSales)
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

                        var sls = dataB.sales.Select(c => c).Where(x => x.bill_no == oSales.BillNo&& x.financial_code==oSales.FinancialCode);
                        dataB.sales.RemoveRange(sls);                        
                        var lt = dataB.ledger_transactions.Select(c => c).Where(x => x.ref_bill_no == oSales.BillNo && x.financial_code == oSales.FinancialCode && x.bill_type == "S");
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

                        if (oSales.BillAmount > 0)
                        {

                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oSales.BillDateTime;
                            lt1.bill_type = "S";
                            lt1.serial_no = ++serNo;
                            lt1.ledger_code = oSales.CustomerCode;
                            lt1.ledger = oSales.Customer;
                            lt1.narration = oSales.Narration;
                            lt1.debit = oSales.BillAmount;
                            lt1.credit = 0;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oSales.CustomerCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oSales.CustomerCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oSales.CustomerCode);
                            lt1.co_ledger = "Sales Account";
                            lt1.ref_bill_no = oSales.BillNo;
                            lt1.ref_bill_date_time = oSales.BillDateTime;
                            lt1.financial_code = oSales.FinancialCode;

                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oSales.BillDateTime;
                            lt2.bill_type = "S";
                            lt2.serial_no = ++serNo;
                            lt2.ledger_code = UniqueLedgers.LedgerCode["Sales Account"];
                            lt2.ledger = "Sales Account";
                            lt2.narration = oSales.Narration;
                            lt2.debit = 0;
                            lt2.credit = oSales.BillAmount;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(UniqueLedgers.LedgerCode["Sales Account"]);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(UniqueLedgers.LedgerCode["Sales Account"]);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(UniqueLedgers.LedgerCode["Sales Account"]);
                            lt2.co_ledger = oSales.Customer;
                            lt2.ref_bill_no = oSales.BillNo;
                            lt2.ref_bill_date_time = oSales.BillDateTime;
                            lt2.financial_code = oSales.FinancialCode;

                            dataB.ledger_transactions.Add(lt1);
                            dataB.ledger_transactions.Add(lt2);
                        }

                        for (int i = 0; i < oSales.Details.Count; i++)
                        {
                            sales sl = new sales();

                            sl.bill_no = oSales.BillNo;
                            sl.bill_date_time = oSales.BillDateTime;
                            sl.serial_no = oSales.Details.ElementAt(i).SerialNo;
                            sl.ledger_code = oSales.Details.ElementAt(i).LedgerCode;
                            sl.ledger = oSales.Details.ElementAt(i).Ledger;
                            sl.narration = oSales.Narration;
                            sl.debit = oSales.Details.ElementAt(i).Debit;
                            sl.credit = oSales.Details.ElementAt(i).Credit;
                            sl.financial_code = oSales.FinancialCode;
                            sl.bill_amount = oSales.BillAmount;
                            sl.party = oSales.Customer;
                            sl.party_code = oSales.CustomerCode;
                            sl.ref_bill_date_time = oSales.RefDateTime;
                            sl.ref_bill_no = oSales.RefBillNo;
                            
                            dataB.sales.Add(sl);

                            //Saving to Ledger Transaction
                            //First entry
                            lt1 = new ledger_transactions();

                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oSales.BillDateTime;
                            lt1.bill_type = "S";
                            lt1.serial_no = ++serNo;
                            lt1.ledger_code = oSales.Details.ElementAt(i).LedgerCode;
                            lt1.ledger = oSales.Details.ElementAt(i).Ledger;
                            lt1.narration = oSales.Narration;
                            lt1.debit = oSales.Details.ElementAt(i).Debit;
                            lt1.credit = oSales.Details.ElementAt(i).Credit;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oSales.Details.ElementAt(i).LedgerCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oSales.Details.ElementAt(i).LedgerCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oSales.Details.ElementAt(i).LedgerCode);
                            lt1.co_ledger = oSales.Customer;
                            lt1.ref_bill_no = oSales.BillNo;
                            lt1.ref_bill_date_time = oSales.BillDateTime;
                            lt1.financial_code = oSales.FinancialCode;

                            //Second entry
                            lt2 = new ledger_transactions();

                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oSales.BillDateTime;
                            lt2.bill_type = "S";
                            lt2.serial_no = ++serNo;
                            lt2.ledger_code = oSales.CustomerCode;
                            lt2.ledger = oSales.Customer;
                            lt2.narration = oSales.Narration;
                            lt2.debit = oSales.Details.ElementAt(i).Credit;
                            lt2.credit = oSales.Details.ElementAt(i).Debit;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(oSales.CustomerCode);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(oSales.CustomerCode);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(oSales.CustomerCode);
                            lt2.co_ledger = oSales.Details.ElementAt(i).Ledger;
                            lt2.ref_bill_no = oSales.BillNo;
                            lt2.ref_bill_date_time = oSales.BillDateTime;
                            lt2.financial_code = oSales.FinancialCode;

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
