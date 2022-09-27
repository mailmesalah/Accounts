using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ServerServiceInterface
{    
    [ServiceContract]
    public interface ISales
    {
        [OperationContract]
        bool CreateBill(CSales oSales);
        [OperationContract]
        CSales ReadBill(string billNo,string financialCode);
        [OperationContract]
        bool UpdateBill(CSales oSales);
        [OperationContract]
        bool DeleteBill(string billNo,string financialCode);        
        [OperationContract]
        int ReadNextBillNo(string financialCode);
     
    }
    
    [DataContract]
    public class CSales
    {
        int id;
        string billNo;
        DateTime billDateTime = new DateTime();
        string financialCode;
        string narration;
        string refBillNo;
        DateTime? refDateTime = new DateTime();
        string customerCode;
        string customer;
        decimal billAmount;
        List<CSalesDetails> details= new List<CSalesDetails>();
 
        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string BillNo
        {
            get { return billNo; }
            set { billNo = value; }
        }

        [DataMember]
        public DateTime BillDateTime
        {
            get { return billDateTime; }
            set { billDateTime = value; }
        }
        
        [DataMember]
        public string FinancialCode
        {
            get { return financialCode; }
            set { financialCode = value; }
        }

        [DataMember]
        public string Narration
        {
            get { return narration; }
            set { narration = value; }
        }

        [DataMember]
        public string RefBillNo
        {
            get { return refBillNo; }
            set { refBillNo = value; }
        }

        [DataMember]
        public DateTime? RefDateTime
        {
            get { return refDateTime; }
            set { refDateTime = value; }
        }

        [DataMember]
        public string CustomerCode
        {
            get { return customerCode; }
            set { customerCode = value; }
        }

        [DataMember]
        public string Customer
        {
            get { return customer; }
            set { customer = value; }
        }        

        [DataMember]
        public decimal BillAmount
        {
            get { return billAmount; }
            set { billAmount = value; }
        }        

        [DataMember]
        public List<CSalesDetails> Details
        {
            get { return details; }
            set { details = value; }
        }
    }


    [DataContract]
    public class CSalesDetails
    {
        int serialNo;
        string ledgerCode;
        string ledger;
        decimal debit;
        decimal credit;
        
        [DataMember]
        public int SerialNo
        {
            get { return serialNo; }
            set { serialNo = value; }
        }

        [DataMember]
        public string LedgerCode
        {
            get { return ledgerCode; }
            set { ledgerCode = value; }
        }

        [DataMember]
        public string Ledger
        {
            get { return ledger; }
            set { ledger = value; }
        }        

        [DataMember]
        public decimal Debit
        {
            get { return debit; }
            set { debit = value; }
        }

        [DataMember]
        public decimal Credit
        {
            get { return credit; }
            set { credit = value; }
        }        
    }
}
