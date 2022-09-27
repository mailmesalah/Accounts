using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ServerServiceInterface
{
    [ServiceContract]
    public interface ILedger
    {
        [OperationContract]
        void LoadAllUniqueLedgers();
        [OperationContract]
        string ReadAGroupCodeOf(string ledgerCode);
        [OperationContract]
        string ReadBGroupCodeOf(string ledgerCode);
        [OperationContract]
        string ReadCGroupCodeOf(string ledgerCode);
        [OperationContract]
        List<CLedger> ReadLedgersWithoutCash();
        [OperationContract]
        List<CLedger> ReadBankLedgers();
        [OperationContract]
        List<CLedger> ReadAllLedgers();
        [OperationContract]
        List<CLedger> ReadSupplierLedgers();
        [OperationContract]
        List<CLedger> ReadCustomerLedgers();
        [OperationContract]
        ObservableCollection<CLedger> ReadAllLedgersWithGroupAsTree();
        [OperationContract]
        List<CLedger> ReadAllGroupLedgers();

        [OperationContract]
        CLedgerRegister ReadLedgerRegister(string ledgerCode);
        [OperationContract]
        bool DeleteLedgerRegister(string ledgerCode);
        [OperationContract]
        bool CreateLedgerRegister(CLedgerRegister ledger);
        [OperationContract]
        bool UpdateLedgerRegister(CLedgerRegister ledger);

        [OperationContract]
        List<CLedger> ReadAllSupplierRegisters();
        [OperationContract]
        List<CLedger> ReadSupplierGroupCode();

        [OperationContract]
        List<CLedger> ReadAllCustomerRegisters();
        [OperationContract]
        List<CLedger> ReadCustomerGroupCode();

        [OperationContract]
        List<CLedger> ReadAllEmployeeRegisters();
        [OperationContract]
        List<CLedger> ReadEmployeeGroupCode();

        [OperationContract]
        List<CLedger> ReadAllBankRegisters();
        [OperationContract]
        List<CLedger> ReadBankGroupCode();

        [OperationContract]
        List<CTrialBalance> FindTrialBalance(string financialCode, DateTime startDate,DateTime endDate);
        [OperationContract]
        List<CTrialBalance> FindTrialBalanceOfBGroup(string groupCode, string financialCode, DateTime startDate, DateTime endDate);
        [OperationContract]
        List<CTrialBalance> FindTrialBalanceOfCGroup(string groupCode, string financialCode, DateTime startDate, DateTime endDate);

    }

    [DataContract]
    public class CTrialBalance
    {
        string serialNo;
        string ledgerCode;        
        string ledger;
        string type;
        decimal debit;
        decimal credit;
        string groupCode;

        [DataMember]
        public string SerialNo
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
        public string LedgerType
        {
            get { return type; }
            set { type = value; }
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

        [DataMember]
        public string GroupCode
        {
            get { return groupCode; }
            set { groupCode = value; }
        }
    }

    [DataContract]
    public class CLedger
    {
        string ledgerCode;
        string ledger;
        string type;
        ObservableCollection<CLedger> members = new ObservableCollection<CLedger>();
        bool isSelected = false;
        bool isExpanded = false;


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
        public string LedgerType
        {
            get { return  type; }
            set { type = value; }
        }

        [DataMember]
        public ObservableCollection<CLedger> MemberList
        {
            get { return members; }
            set { members = value; }
        }
        [DataMember]
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }
        [DataMember]
        public bool IsExpanded
        {
            get { return isExpanded; }
            set { isExpanded = value; }
        }
    }

    [DataContract]
    public class CLedgerRegister
    {
        int id;
        string ledgerCode;
        string ledger;
        string type;        
        string groupCode;
        string alternateName;
        string address1;
        string address2;
        string address3;
        string details1;
        string details2;
        string details3;
        string details4;
        string details5;
        string details6;
        bool? isEnabled;
        string aGroupCode;
        string bGroupCode;
        string cGroupCode;

        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
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
        public string LedgerType
        {
            get { return type; }
            set { type = value; }
        }
        [DataMember]
        public string GroupCode
        {
            get { return groupCode; }
            set { groupCode = value; }
        }        
        [DataMember]
        public string AlternateName
        {
            get { return alternateName; }
            set { alternateName = value; }
        }
        [DataMember]
        public string Address1
        {
            get { return address1; }
            set { address1 = value; }
        }
        [DataMember]
        public string Address2
        {
            get { return address2; }
            set { address2 = value; }
        }
        [DataMember]
        public string Address3
        {
            get { return address3; }
            set { address3 = value; }
        }
        [DataMember]
        public string Details1
        {
            get { return details1; }
            set { details1 = value; }
        }
        [DataMember]
        public string Details2
        {
            get { return details2; }
            set { details2 = value; }
        }
        [DataMember]
        public string Details3
        {
            get { return details3; }
            set { details3 = value; }
        }
        [DataMember]
        public string Details4
        {
            get { return details4; }
            set { details4 = value; }
        }
        [DataMember]
        public string Details5
        {
            get { return details5; }
            set { details5 = value; }
        }
        [DataMember]
        public string Details6
        {
            get { return details6; }
            set { details6 = value; }
        }
        [DataMember]
        public bool? IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }
        [DataMember]
        public string AGroupCode
        {
            get { return aGroupCode; }
            set { aGroupCode = value; }
        }
        [DataMember]
        public string BGroupCode
        {
            get { return bGroupCode; }
            set { bGroupCode = value; }
        }
        [DataMember]
        public string CGroupCode
        {
            get { return cGroupCode; }
            set { cGroupCode = value; }
        }
    }
}
