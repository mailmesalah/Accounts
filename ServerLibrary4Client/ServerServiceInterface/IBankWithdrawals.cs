using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ServerServiceInterface
{    
    [ServiceContract]
    public interface IBankWithdrawal
    {
        [OperationContract]
        bool CreateBill(CBankWithdrawal oBankWithdrawal);
        [OperationContract]
        CBankWithdrawal ReadBill(string billNo,string financialCode);
        [OperationContract]
        bool UpdateBill(CBankWithdrawal oBankWithdrawal);
        [OperationContract]
        bool DeleteBill(string billNo,string financialCode);        
        [OperationContract]
        int ReadNextBillNo(string financialCode);
     
    }


    
    [DataContract]
    public class CBankWithdrawal
    {
        int id;
        string billNo;
        DateTime billDateTime = new DateTime();
        string financialCode;
        string bankCode;
        string bank;

        List<CBankWithdrawalDetails> details= new List<CBankWithdrawalDetails>();
 
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
        public string BankCode
        {
            get { return bankCode; }
            set { bankCode = value; }
        }

        [DataMember]
        public string Bank
        {
            get { return bank; }
            set { bank = value; }
        }

        [DataMember]
        public List<CBankWithdrawalDetails> Details
        {
            get { return details; }
            set { details = value; }
        }
    }


    [DataContract]
    public class CBankWithdrawalDetails
    {
        int serialNo;
        string ledgerCode;
        string ledger;
        string narration;
        decimal amount;
        string status;
        
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

        [DataMember]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }        
    }
}
