using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ServerServiceInterface
{    
    [ServiceContract]
    public interface ICashPayment
    {
        [OperationContract]
        bool CreateBill(CCashPayment oCashPayment);
        [OperationContract]
        CCashPayment ReadBill(string billNo,string financialCode);
        [OperationContract]
        bool UpdateBill(CCashPayment oCashPayment);
        [OperationContract]
        bool DeleteBill(string billNo,string financialCode);        
        [OperationContract]
        int ReadNextBillNo(string financialCode);
     
    }


    
    [DataContract]
    public class CCashPayment
    {
        int id;
        string billNo;
        DateTime billDateTime = new DateTime();
        string financialCode;
        List<CCashPaymentDetails> details= new List<CCashPaymentDetails>();
 
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
        public List<CCashPaymentDetails> Details
        {
            get { return details; }
            set { details = value; }
        }
    }


    [DataContract]
    public class CCashPaymentDetails
    {
        int serialNo;
        string ledgerCode;
        string ledger;
        string narration;
        decimal amount;

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
        public string Narration
        {
            get { return narration; }
            set { narration = value; }
        }

        [DataMember]
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
    }
}
