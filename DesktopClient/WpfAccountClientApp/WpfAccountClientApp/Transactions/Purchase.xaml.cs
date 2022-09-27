using ServerServiceInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using WpfAccountClientApp.General;

namespace WpfAccountClientApp.Transactions
{
    /// <summary>
    /// Interaction logic for Purchase.xaml
    /// </summary>
    public partial class Purchase : Window
    {
        
        CPurchase mPurchase = new CPurchase();
        ObservableCollection<CPurchaseDetails> mGridContent = new ObservableCollection<CPurchaseDetails>();
        String mPurchaseID = "";
        
        
        public Purchase()
        {
            InitializeComponent();
            loadInitialDetails();
                        
        }

        //Member methods
        private void loadInitialDetails()
        {
            getSupplierLedgers();
            getLedgers();
            newBill();
        }

        private void newBill()
        {
            mPurchaseID = "";
            mPurchase = new CPurchase();
            mTextBoxBillNo.Text = getLastBillNo();
            mTextBoxRefBillNo.Text = "";
            mDTPRefDate.SelectedDate= DateTime.Now;
            mComboSupplierLedgers.Text = ""; 
            loadFinancialCodes();
            mDTPDate.SelectedDate = DateTime.Now;
            mComboFinancialYear.Text = CommonMethods.getFinancialCode(DateTime.Now);
            mGridContent.Clear();
            mDataGridContent.ItemsSource = mGridContent;
            clearEditBoxes();
            mTextBoxRefBillNo.Focus();
        }

        private void clearEditBoxes(){
            mLabelSerialNo.Content = mDataGridContent.Items.Count+1;
            mComboLedgers.Text = "";
            mTextBoxNarration.Text = "";
            mTextBoxDebit.Text = "";
            mTextBoxCredit.Text = "";            
        }

        private void loadFinancialCodes()
        {
            try
            {
                using (ChannelFactory<IBillNo> billNoProxy = new ChannelFactory<ServerServiceInterface.IBillNo>("BillNoEndpoint"))
                {
                    billNoProxy.Open();
                    IBillNo billNoService = billNoProxy.CreateChannel();

                    List<String> fcodes = billNoService.ReadAllFinancialCodes();
                    mComboFinancialYear.ItemsSource = fcodes;                    
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private string getLastBillNo()
        {
            string billNo = "";
            try {
                using (ChannelFactory<IPurchase> PurchaseProxy = new ChannelFactory<ServerServiceInterface.IPurchase>("PurchaseEndpoint"))
                {
                    PurchaseProxy.Open();
                    IPurchase Purchaseervice = PurchaseProxy.CreateChannel();
                    billNo=Purchaseervice.ReadNextBillNo(CommonMethods.getFinancialCode(mDTPDate.SelectedDate.Value)).ToString();                    
                }
            }
            catch
            {

            }
            return billNo;
        }

        private void getLedgers()
        {
            try {
                using (ChannelFactory<ILedger> ledgerProxy = new ChannelFactory<ServerServiceInterface.ILedger>("LedgerEndpoint"))
                {
                    ledgerProxy.Open();
                    ILedger ledgerService = ledgerProxy.CreateChannel();                    
                    List<CLedger> ledgers = ledgerService.ReadAllLedgers();
                    mComboLedgers.ItemsSource = ledgers;
                    mComboLedgers.DisplayMemberPath = "Ledger";
                    mComboLedgers.SelectedValuePath = "LedgerCode";
                }
            }
            catch
            {

            }        
        }

        private void getSupplierLedgers()
        {
            try
            {
                using (ChannelFactory<ILedger> ledgerProxy = new ChannelFactory<ServerServiceInterface.ILedger>("LedgerEndpoint"))
                {
                    ledgerProxy.Open();
                    ILedger ledgerService = ledgerProxy.CreateChannel();
                    List<CLedger> ledgers = ledgerService.ReadSupplierLedgers();
                    mComboSupplierLedgers.ItemsSource = ledgers;
                    mComboSupplierLedgers.DisplayMemberPath = "Ledger";
                    mComboSupplierLedgers.SelectedValuePath = "LedgerCode";
                }
            }
            catch
            {

            }
        }

        private void addDataToGrid()
        {
            if (mComboLedgers.SelectedIndex == -1)
            {
                mComboLedgers.Focus();
                return;
            }

            decimal debit = 0;
            try
            {
                
                debit = decimal.Parse(mTextBoxDebit.Text);

                if (debit < 0)
                {
                    mTextBoxDebit.Focus();
                    return;
                }
            }
            catch
            {
                mTextBoxDebit.Focus();
                return;
            }

            decimal credit;
            try
            {

                credit = decimal.Parse(mTextBoxCredit.Text);

                if (credit < 0)
                {
                    mTextBoxCredit.Focus();
                    return;
                }
            }
            catch
            {
                mTextBoxCredit.Focus();
                return;
            }
            

            int serialNo = int.Parse(mLabelSerialNo.Content.ToString());
            if (serialNo <= mDataGridContent.Items.Count)
            {
                //Edit
                int index = mDataGridContent.SelectedIndex;
                mGridContent.Remove(mDataGridContent.SelectedItem as CPurchaseDetails);
                mGridContent.Insert(index, new CPurchaseDetails() { SerialNo = serialNo, Ledger = mComboLedgers.Text.ToString(), LedgerCode = mComboLedgers.SelectedValue.ToString(), Debit = debit, Credit=credit });
            }
            else
            {
                //Add
                CPurchaseDetails crd = new CPurchaseDetails() { SerialNo = serialNo, Ledger = mComboLedgers.Text.ToString(), LedgerCode = mComboLedgers.SelectedValue.ToString(), Debit = debit, Credit=credit };                
                mGridContent.Add(crd);
            }
            
            clearEditBoxes();
            mDataGridContent.ScrollIntoView(mDataGridContent.Items.GetItemAt(mDataGridContent.Items.Count-1));
            mComboLedgers.Focus();
        }

        private void selectDataToEditBoxes()
        {
            if (mDataGridContent.SelectedIndex > -1)
            {
                CPurchaseDetails crd=(CPurchaseDetails)mDataGridContent.Items.GetItemAt(mDataGridContent.SelectedIndex);
                mLabelSerialNo.Content = crd.SerialNo;
                mComboLedgers.Text = crd.Ledger;                
                mTextBoxDebit.Text = crd.Debit.ToString();
                mTextBoxCredit.Text = crd.Credit.ToString();
            }
        }

        private void removeFromGrid()
        {
            if (mDataGridContent.SelectedIndex > -1)
            {
                mGridContent.Remove(mDataGridContent.SelectedItem as CPurchaseDetails);

                //Reseting the Serial Nos
                for(int i = 0; i < mGridContent.Count; i++)
                {
                    mGridContent.ElementAt(i).SerialNo = i + 1;                    
                }
                mDataGridContent.Items.Refresh();
                clearEditBoxes();
            }
            
        }

        private void showDataFromDatabase()
        {
            try
            {
                using (ChannelFactory<IPurchase> PurchaseProxy = new ChannelFactory<ServerServiceInterface.IPurchase>("PurchaseEndpoint"))
                {
                    PurchaseProxy.Open();
                    IPurchase Purchaseervice = PurchaseProxy.CreateChannel();

                    CPurchase ccr= Purchaseervice.ReadBill(mTextBoxBillNo.Text.Trim(), CommonMethods.getFinancialCode(mDTPDate.SelectedDate.Value));
                    
                    if (ccr != null)
                    {                        
                        mPurchaseID = ccr.Id.ToString();
                        mComboSupplierLedgers.Text = ccr.Supplier;                        
                        mDTPDate.SelectedDate = ccr.BillDateTime;
                        mTextBoxRefBillNo.Text = ccr.RefBillNo;
                        mDTPRefDate.SelectedDate = ccr.RefDateTime;
                        mTextBoxBillAmount.Text = ccr.BillAmount.ToString();
                                                
                        mGridContent.Clear();
                        foreach (var item in ccr.Details)
                        {
                            mGridContent.Add(item);
                        }
                        mDataGridContent.Items.Refresh();
                    }                    
                }
            }
            catch
            {

            }
        }
     
        private void saveDataToDatabase()
        {
            try
            {
                if (mTextBoxRefBillNo.Text.Trim()=="")
                {
                    mTextBoxRefBillNo.Focus();
                    return;
                }

                if (mComboSupplierLedgers.SelectedItem == null)
                {
                    mComboSupplierLedgers.Focus();
                    return;
                }

                decimal billA = 0;
                try
                {
                    if (mTextBoxBillAmount.Text.Trim() == "")
                    {
                        billA = 0;
                    }
                    else {
                        billA = decimal.Parse(mTextBoxBillAmount.Text.Trim());
                    }
                }
                catch
                {
                    mTextBoxBillAmount.Focus();
                    return;
                }


                if (mDataGridContent.Items.Count==0)
                {
                    mComboLedgers.Focus();
                    return;
                }

                using (ChannelFactory<IPurchase> PurchaseProxy = new ChannelFactory<ServerServiceInterface.IPurchase>("PurchaseEndpoint"))
                {
                    PurchaseProxy.Open();
                    IPurchase PurchaseService = PurchaseProxy.CreateChannel();

                    CPurchase ccr = new CPurchase();
                    ccr.BillNo = mTextBoxBillNo.Text.Trim();
                    ccr.BillDateTime = mDTPDate.SelectedDate.Value;
                    ccr.RefBillNo = mTextBoxRefBillNo.Text;
                    ccr.RefDateTime = mDTPRefDate.SelectedDate.Value;
                    ccr.BillAmount = billA;
                    ccr.Supplier = mComboSupplierLedgers.Text.Trim();
                    ccr.SupplierCode = mComboSupplierLedgers.SelectedValue.ToString();
                    ccr.FinancialCode = CommonMethods.getFinancialCode(mDTPDate.SelectedDate.Value);                                    
                    foreach (var item in mGridContent)
                    {
                        ccr.Details.Add(item);
                    }

                    bool success = false;
                    if (mPurchaseID != "")
                    { 
                        success = PurchaseService.UpdateBill(ccr);
                    }
                    else
                    {                    
                        success = PurchaseService.CreateBill(ccr);
                    }

                    if (success)
                    {
                        newBill();
                    }                    
                }
            }
            catch(Exception e)
            {
                
            }
        }

        private void deleteDataFromDatabase()
        {
            try
            {
                using (ChannelFactory<IPurchase> PurchaseProxy = new ChannelFactory<ServerServiceInterface.IPurchase>("PurchaseEndpoint"))
                {
                    PurchaseProxy.Open();
                    IPurchase Purchaseervice = PurchaseProxy.CreateChannel();
                    
                    bool success= Purchaseervice.DeleteBill(mTextBoxBillNo.Text.Trim(), CommonMethods.getFinancialCode(mDTPDate.SelectedDate.Value));

                    if (success)
                    {
                        newBill();
                    }                   
                }
            }
            catch
            {
                
            }
        }

        //Events
        private void mButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
       
        private void mButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            addDataToGrid();
        }

        private void mDTPDate_LostFocus(object sender, RoutedEventArgs e)
        {
            mComboFinancialYear.Text= CommonMethods.getFinancialCode(mDTPDate.SelectedDate.Value);
        }

        private void mButtonNew_Click(object sender, RoutedEventArgs e)
        {
            newBill();
        }

        private void mButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            deleteDataFromDatabase();
        }

        private void mComboFinancialYear_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mComboFinancialYear.SelectedIndex > -1)
                {
                    int year = int.Parse(mComboFinancialYear.SelectedItem.ToString());
                    if (!mComboFinancialYear.SelectedItem.ToString().Equals(CommonMethods.getFinancialCode(mDTPDate.SelectedDate.Value)))
                    {
                        mDTPDate.SelectedDate = new DateTime(year, CommonMethods.FinancialStartDate.Month, CommonMethods.FinancialStartDate.Day);
                    }
                }
            }
            catch
            {

            }
        }        

        private void mDataGridContent_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectDataToEditBoxes();
        }

        private void mButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            removeFromGrid();
        }        

        private void mTextBoxBillNo_LostFocus(object sender, RoutedEventArgs e)
        {
            showDataFromDatabase();
        }

        private void mButtonSave_Click(object sender, RoutedEventArgs e)
        {
            saveDataToDatabase();
        }

        private void mTextBoxDebit_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            decimal debit;
            try
            {
                debit=decimal.Parse(mTextBoxDebit.Text);
                if (debit > 0)
                {
                    mTextBoxCredit.IsEnabled = false;
                    mTextBoxCredit.Text = "0";
                }
                else
                {
                    mTextBoxCredit.IsEnabled = true;
                }
            }
            catch
            {
                mTextBoxCredit.IsEnabled = true;
            }
            
        }

        private void mTextBoxCredit_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            decimal credit;
            try
            {
                credit = decimal.Parse(mTextBoxCredit.Text);
                if (credit > 0)
                {
                    mTextBoxDebit.IsEnabled = false;
                    mTextBoxDebit.Text = "0";
                }
                else
                {
                    mTextBoxDebit.IsEnabled = true;
                }
            }
            catch
            {
                mTextBoxDebit.IsEnabled = true;
            }
        }
    }
}
