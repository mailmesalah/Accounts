using ServerServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAccountClientApp.Registers;
using WpfAccountClientApp.Reports;
using WpfAccountClientApp.Transactions;

namespace WpfAccountClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        
        public MainWindow()
        {
            InitializeComponent();

            //Console.WriteLine(proxy.GetData(100));

        }

        private void CashReceipts_Click(object sender, RoutedEventArgs e)
        {
            CashReceipts cr = new CashReceipts();
            cr.Show();
        }

        private void CashPayments_Click(object sender, RoutedEventArgs e)
        {
            CashPayments cp = new CashPayments();
            cp.Show();
        }

        private void BankDeposits_Click(object sender, RoutedEventArgs e)
        {
            BankDeposits bd = new BankDeposits();
            bd.Show();
        }

        private void BankWithdrawals_Click(object sender, RoutedEventArgs e)
        {
            BankWithdrawals bw = new BankWithdrawals();
            bw.Show();
        }

        private void JournalVouchers_Click(object sender, RoutedEventArgs e)
        {
            JournalVouchers jv = new JournalVouchers();
            jv.Show();
        }

        private void OpeningBalances_Click(object sender, RoutedEventArgs e)
        {
            OpeningBalances ob = new OpeningBalances();
            ob.Show();
        }

        private void Purchase_Click(object sender, RoutedEventArgs e)
        {
            Purchase p = new Purchase();
            p.Show();
        }

        private void PurchaseReturn_Click(object sender, RoutedEventArgs e)
        {
            PurchaseReturn pr = new PurchaseReturn();
            pr.Show();
        }

        private void Sales_Click(object sender, RoutedEventArgs e)
        {
            Sales s = new Sales();
            s.Show();
        }

        private void SalesReturn_Click(object sender, RoutedEventArgs e)
        {
            SalesReturn s = new SalesReturn();
            s.Show();
        }

        private void LedgerRegisters_Click(object sender, RoutedEventArgs e)
        {
            LedgerRegisters lr = new LedgerRegisters();
            lr.Show();
        }

        private void SupplierRegisters_Click(object sender, RoutedEventArgs e)
        {
            SupplierRegisters sr = new SupplierRegisters();
            sr.Show();
        }

        private void CustomerRegisters_Click(object sender, RoutedEventArgs e)
        {
            CustomerRegisters cr = new CustomerRegisters();
            cr.Show();
        }

        private void EmployeeRegisters_Click(object sender, RoutedEventArgs e)
        {
            EmployeeRegisters er = new EmployeeRegisters();
            er.Show();
        }

        private void BankRegisters_Click(object sender, RoutedEventArgs e)
        {
            BankRegisters br = new BankRegisters();
            br.Show();
        }

        private void TrialBalance_Click(object sender, RoutedEventArgs e)
        {
            TrialBalance tb = new TrialBalance();
            tb.Show();
        }

        private void BalanceSheet_Click(object sender, RoutedEventArgs e)
        {
            BalanceSheet bs = new BalanceSheet();
            bs.Show();
        }
    }
}
