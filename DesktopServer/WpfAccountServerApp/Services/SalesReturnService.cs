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
   
    public class SalesReturnService : ISalesReturn
    {
        

        public bool CreateBill(CSalesReturn oSalesReturn)
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


                        int cbillNo = bs.ReadNextSalesReturnBillNo(oSalesReturn.FinancialCode);
                        bs.UpdateSalesReturnBillNo(oSalesReturn.FinancialCode,cbillNo+1);
                        
                        int ltBillNo = bs.ReadNextLedgerTransactionBillNo(oSalesReturn.FinancialCode);
                        bs.UpdateLedgerTransactionBillNo(oSalesReturn.FinancialCode, ltBillNo + 1);
                        //Saving bill Amount
                        ledger_transactions lt1 = new ledger_transactions();
                        ledger_transactions lt2 = new ledger_transactions();

                        int serNo = 0;

                        if (oSalesReturn.BillAmount > 0)
                        {
                            
                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oSalesReturn.BillDateTime;
                            lt1.bill_type = "SR";
                            lt1.serial_no = ++serNo;
                            lt1.ledger_code = oSalesReturn.CustomerCode;
                            lt1.ledger = oSalesReturn.Customer;
                            lt1.narration = oSalesReturn.Narration;
                            lt1.debit = 0;
                            lt1.credit = oSalesReturn.BillAmount;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oSalesReturn.CustomerCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oSalesReturn.CustomerCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oSalesReturn.CustomerCode);
                            lt1.co_ledger = "Sales Account";
                            lt1.ref_bill_no = cbillNo.ToString();
                            lt1.ref_bill_date_time = oSalesReturn.BillDateTime;
                            lt1.financial_code = oSalesReturn.FinancialCode;
                            
                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oSalesReturn.BillDateTime;
                            lt2.bill_type = "SR";
                            lt2.serial_no = ++serNo;
                            lt2.ledger_code = UniqueLedgers.LedgerCode["Sales Account"];
                            lt2.ledger = "Sales Account";
                            lt2.narration = oSalesReturn.Narration;
                            lt2.debit = oSalesReturn.BillAmount;
                            lt2.credit = 0;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(UniqueLedgers.LedgerCode["Sales Account"]);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(UniqueLedgers.LedgerCode["Sales Account"]);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(UniqueLedgers.LedgerCode["Sales Account"]);
                            lt2.co_ledger = oSalesReturn.Customer;
                            lt2.ref_bill_no = cbillNo.ToString();
                            lt2.ref_bill_date_time = oSalesReturn.BillDateTime;
                            lt2.financial_code = oSalesReturn.FinancialCode;

                            dataB.ledger_transactions.Add(lt1);
                            dataB.ledger_transactions.Add(lt2);
                        }

                        for (int i = 0; i < oSalesReturn.Details.Count; i++)
                        {
                            sales_return sr = new sales_return();

                            sr.bill_no = cbillNo.ToString();
                            sr.bill_date_time = oSalesReturn.BillDateTime;
                            sr.serial_no = oSalesReturn.Details.ElementAt(i).SerialNo;
                            sr.ledger_code= oSalesReturn.Details.ElementAt(i).LedgerCode;
                            sr.ledger = oSalesReturn.Details.ElementAt(i).Ledger;
                            sr.narration = oSalesReturn.Narration;
                            sr.debit = oSalesReturn.Details.ElementAt(i).Debit;
                            sr.credit = oSalesReturn.Details.ElementAt(i).Credit;
                            sr.financial_code = oSalesReturn.FinancialCode;
                            sr.bill_amount = oSalesReturn.BillAmount;
                            sr.party = oSalesReturn.Customer;
                            sr.party_code = oSalesReturn.CustomerCode;
                            sr.ref_bill_date_time = oSalesReturn.RefDateTime;
                            sr.ref_bill_no = oSalesReturn.RefBillNo;
                            
                            dataB.sales_return.Add(sr);


                            //Saving to Ledger Transaction
                            //First entry
                            lt1 = new ledger_transactions();

                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oSalesReturn.BillDateTime;
                            lt1.bill_type = "SR";
                            lt1.serial_no = ++serNo;
                            lt1.ledger_code = oSalesReturn.Details.ElementAt(i).LedgerCode;
                            lt1.ledger = oSalesReturn.Details.ElementAt(i).Ledger;
                            lt1.narration = oSalesReturn.Narration;
                            lt1.debit = oSalesReturn.Details.ElementAt(i).Debit;
                            lt1.credit = oSalesReturn.Details.ElementAt(i).Credit;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oSalesReturn.Details.ElementAt(i).LedgerCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oSalesReturn.Details.ElementAt(i).LedgerCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oSalesReturn.Details.ElementAt(i).LedgerCode);
                            lt1.co_ledger = oSalesReturn.Customer;
                            lt1.ref_bill_no = cbillNo.ToString();
                            lt1.ref_bill_date_time = oSalesReturn.BillDateTime;
                            lt1.financial_code = oSalesReturn.FinancialCode;

                            //Second entry
                            lt2 = new ledger_transactions();

                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oSalesReturn.BillDateTime;
                            lt2.bill_type = "SR";
                            lt2.serial_no = ++serNo;
                            lt2.ledger_code = oSalesReturn.CustomerCode;
                            lt2.ledger = oSalesReturn.Customer;
                            lt2.narration = oSalesReturn.Narration;
                            lt2.debit = oSalesReturn.Details.ElementAt(i).Credit;
                            lt2.credit = oSalesReturn.Details.ElementAt(i).Debit;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(oSalesReturn.CustomerCode);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(oSalesReturn.CustomerCode);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(oSalesReturn.CustomerCode);
                            lt2.co_ledger = oSalesReturn.Details.ElementAt(i).Ledger;
                            lt2.ref_bill_no = cbillNo.ToString();
                            lt2.ref_bill_date_time = oSalesReturn.BillDateTime;
                            lt2.financial_code = oSalesReturn.FinancialCode;

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
                        var sr = dataB.sales_return.Select(c => c).Where(x=>x.bill_no==billNo&&x.financial_code==financialCode);
                        dataB.sales_return.RemoveRange(sr);
                                                                        

                        //Delete from Ledger Transaction
                        var lt = dataB.ledger_transactions.Select(c => c).Where(x => x.ref_bill_no == billNo && x.financial_code == financialCode&&x.bill_type=="SR");
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

        public CSalesReturn ReadBill(string billNo,string financialCode)
        {
            CSalesReturn srs = null;

            using (var dataB = new AccountsdbEntities())
            {
                var srr = dataB.sales_return.Select(c => c).Where(x => x.bill_no == billNo && x.financial_code == financialCode).OrderBy(y=>y.serial_no);
                
                if (srr.Count() > 0)
                {
                    srs = new CSalesReturn();

                    var sr = srr.FirstOrDefault();
                    srs.Id = sr.id;
                    srs.BillNo = sr.bill_no;
                    srs.BillDateTime = sr.bill_date_time;
                    srs.RefBillNo = sr.ref_bill_no;
                    srs.RefDateTime = sr.ref_bill_date_time;
                    srs.FinancialCode = sr.financial_code;
                    srs.Customer = sr.party;
                    srs.CustomerCode = sr.party_code;
                    srs.Narration = sr.narration;
                    srs.BillAmount = sr.bill_amount;
                    
                    foreach (var item in srr)
                    {                        
                        srs.Details.Add(new CSalesReturnDetails() { SerialNo=item.serial_no,LedgerCode=item.ledger_code,Ledger=item.ledger,  Debit=item.debit, Credit=item.credit });
                    }
                }
                
            }

            return srs;
        }

        public int ReadNextBillNo(string financialCode)
        {
            
            BillNoService bns = new BillNoService();
            return bns.ReadNextSalesReturnBillNo(financialCode);
            
        }

        public bool UpdateBill(CSalesReturn oSalesReturn)
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

                        var srs = dataB.sales_return.Select(c => c).Where(x => x.bill_no == oSalesReturn.BillNo&& x.financial_code==oSalesReturn.FinancialCode);
                        dataB.sales_return.RemoveRange(srs);                        
                        var lt = dataB.ledger_transactions.Select(c => c).Where(x => x.ref_bill_no == oSalesReturn.BillNo && x.financial_code == oSalesReturn.FinancialCode && x.bill_type == "SR");
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

                        if (oSalesReturn.BillAmount > 0)
                        {
                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oSalesReturn.BillDateTime;
                            lt1.bill_type = "SR";
                            lt1.serial_no = ++serNo;
                            lt1.ledger_code = oSalesReturn.CustomerCode;
                            lt1.ledger = oSalesReturn.Customer;
                            lt1.narration = oSalesReturn.Narration;
                            lt1.debit = 0;
                            lt1.credit = oSalesReturn.BillAmount;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oSalesReturn.CustomerCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oSalesReturn.CustomerCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oSalesReturn.CustomerCode);
                            lt1.co_ledger = "Sales Account";
                            lt1.ref_bill_no = oSalesReturn.BillNo;
                            lt1.ref_bill_date_time = oSalesReturn.BillDateTime;
                            lt1.financial_code = oSalesReturn.FinancialCode;
                            
                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oSalesReturn.BillDateTime;
                            lt2.bill_type = "SR";
                            lt2.serial_no = ++serNo;
                            lt2.ledger_code = UniqueLedgers.LedgerCode["Sales Account"];
                            lt2.ledger = "Sales Account";
                            lt2.narration = oSalesReturn.Narration;
                            lt2.debit = oSalesReturn.BillAmount;
                            lt2.credit = 0;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(UniqueLedgers.LedgerCode["Sales Account"]);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(UniqueLedgers.LedgerCode["Sales Account"]);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(UniqueLedgers.LedgerCode["Sales Account"]);
                            lt2.co_ledger = oSalesReturn.Customer;
                            lt2.ref_bill_no = oSalesReturn.BillNo;
                            lt2.ref_bill_date_time = oSalesReturn.BillDateTime;
                            lt2.financial_code = oSalesReturn.FinancialCode;

                            dataB.ledger_transactions.Add(lt1);
                            dataB.ledger_transactions.Add(lt2);
                        }

                        for (int i = 0; i < oSalesReturn.Details.Count; i++)
                        {
                            sales_return sr = new sales_return();

                            sr.bill_no = oSalesReturn.BillNo;
                            sr.bill_date_time = oSalesReturn.BillDateTime;
                            sr.serial_no = oSalesReturn.Details.ElementAt(i).SerialNo;
                            sr.ledger_code = oSalesReturn.Details.ElementAt(i).LedgerCode;
                            sr.ledger = oSalesReturn.Details.ElementAt(i).Ledger;
                            sr.narration = oSalesReturn.Narration;
                            sr.debit = oSalesReturn.Details.ElementAt(i).Debit;
                            sr.credit = oSalesReturn.Details.ElementAt(i).Credit;
                            sr.financial_code = oSalesReturn.FinancialCode;
                            sr.bill_amount = oSalesReturn.BillAmount;
                            sr.party = oSalesReturn.Customer;
                            sr.party_code = oSalesReturn.CustomerCode;
                            sr.ref_bill_date_time = oSalesReturn.RefDateTime;
                            sr.ref_bill_no = oSalesReturn.RefBillNo;
                           
                            dataB.sales_return.Add(sr);

                            //Saving to Ledger Transaction
                            //First entry
                            lt1 = new ledger_transactions();

                            lt1.bill_no = ltBillNo.ToString();
                            lt1.bill_date_time = oSalesReturn.BillDateTime;
                            lt1.bill_type = "SR";
                            lt1.serial_no = ++serNo;
                            lt1.ledger_code = oSalesReturn.Details.ElementAt(i).LedgerCode;
                            lt1.ledger = oSalesReturn.Details.ElementAt(i).Ledger;
                            lt1.narration = oSalesReturn.Narration;
                            lt1.debit = oSalesReturn.Details.ElementAt(i).Debit;
                            lt1.credit = oSalesReturn.Details.ElementAt(i).Credit;
                            lt1.a_group_code = ls.ReadAGroupCodeOf(oSalesReturn.Details.ElementAt(i).LedgerCode);
                            lt1.b_group_code = ls.ReadBGroupCodeOf(oSalesReturn.Details.ElementAt(i).LedgerCode);
                            lt1.c_group_code = ls.ReadCGroupCodeOf(oSalesReturn.Details.ElementAt(i).LedgerCode);
                            lt1.co_ledger = oSalesReturn.Customer;
                            lt1.ref_bill_no = oSalesReturn.BillNo;
                            lt1.ref_bill_date_time = oSalesReturn.BillDateTime;
                            lt1.financial_code = oSalesReturn.FinancialCode;

                            //Second entry
                            lt2 = new ledger_transactions();

                            lt2.bill_no = ltBillNo.ToString();
                            lt2.bill_date_time = oSalesReturn.BillDateTime;
                            lt2.bill_type = "SR";
                            lt2.serial_no = ++serNo;
                            lt2.ledger_code = oSalesReturn.CustomerCode;
                            lt2.ledger = oSalesReturn.Customer;
                            lt2.narration = oSalesReturn.Narration;
                            lt2.debit = oSalesReturn.Details.ElementAt(i).Credit;
                            lt2.credit = oSalesReturn.Details.ElementAt(i).Debit;
                            lt2.a_group_code = ls.ReadAGroupCodeOf(oSalesReturn.CustomerCode);
                            lt2.b_group_code = ls.ReadBGroupCodeOf(oSalesReturn.CustomerCode);
                            lt2.c_group_code = ls.ReadCGroupCodeOf(oSalesReturn.CustomerCode);
                            lt2.co_ledger = oSalesReturn.Details.ElementAt(i).Ledger;
                            lt2.ref_bill_no = oSalesReturn.BillNo;
                            lt2.ref_bill_date_time = oSalesReturn.BillDateTime;
                            lt2.financial_code = oSalesReturn.FinancialCode;

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
