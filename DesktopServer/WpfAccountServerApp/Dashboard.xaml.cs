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
using WpfAccountServerApp.Services;

namespace WpfAccountServerApp
{
    
    public partial class MainWindow : Window
    {
        ServiceHost hostCashReceiptService;
        ServiceHost hostLedgerService;
        ServiceHost hostBillNoService;
        ServiceHost hostCashPaymentService;
        ServiceHost hostBankDepositService;
        ServiceHost hostbankWithdrawalService;
        ServiceHost hostJournalVoucherService;
        ServiceHost hostOpeningBalanceService;
        ServiceHost hostPurchaseService;
        ServiceHost hostPurchaseReturnService;
        ServiceHost hostSalesService;
        ServiceHost hostSalesReturnService;
        public MainWindow()
        {
            InitializeComponent();

            //Initialising host object
            hostCashReceiptService = new ServiceHost(typeof(Services.CashReceiptService));
            hostLedgerService = new ServiceHost(typeof(Services.LedgerService));
            hostBillNoService = new ServiceHost(typeof(Services.BillNoService));
            hostCashPaymentService = new ServiceHost(typeof(Services.CashPaymentService));
            hostBankDepositService = new ServiceHost(typeof(Services.BankDepositService));
            hostbankWithdrawalService = new ServiceHost(typeof(Services.BankWithdrawalService));
            hostJournalVoucherService = new ServiceHost(typeof(Services.JournalVoucherService));
            hostOpeningBalanceService = new ServiceHost(typeof(Services.OpeningBalanceService));
            hostPurchaseService = new ServiceHost(typeof(Services.PurchaseService));
            hostPurchaseReturnService = new ServiceHost(typeof(Services.PurchaseReturnService));
            hostSalesService = new ServiceHost(typeof(Services.SalesService));
            hostSalesReturnService = new ServiceHost(typeof(Services.SalesReturnService));

            hostBillNoService.Open();
            hostCashReceiptService.Open();
            hostLedgerService.Open();
            hostCashPaymentService.Open();
            hostBankDepositService.Open();
            hostbankWithdrawalService.Open();
            hostJournalVoucherService.Open();
            hostOpeningBalanceService.Open();
            hostPurchaseService.Open();
            hostPurchaseReturnService.Open();
            hostSalesService.Open();
            hostSalesReturnService.Open();



            //Loading the Unique Ledgers
            LedgerService ls = new LedgerService();
            ls.LoadAllUniqueLedgers();

            Console.WriteLine("Services are started and running");
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            //Closing the service hoster
            hostCashReceiptService.Close();
            hostLedgerService.Close();
            hostBillNoService.Close();
            hostCashPaymentService.Close();
            hostBankDepositService.Close();
            hostbankWithdrawalService.Close();
            hostJournalVoucherService.Close();
            hostOpeningBalanceService.Close();
            hostPurchaseService.Close();
            hostPurchaseReturnService.Close();
            hostSalesService.Close();
            hostSalesReturnService.Close();
            Console.WriteLine("Services are stopped");
        }
    }
}
