//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfAccountServerApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class bill_nos
    {
        public int id { get; set; }
        public int bank_deposit { get; set; }
        public int bank_withdrawal { get; set; }
        public int cash_payment { get; set; }
        public int cash_receipt { get; set; }
        public int journal_voucher { get; set; }
        public int ledger_register { get; set; }
        public int ledger_transaction { get; set; }
        public int opening_balance { get; set; }
        public int purchase { get; set; }
        public int purchase_return { get; set; }
        public int sales { get; set; }
        public int sales_return { get; set; }
        public string financial_code { get; set; }
    }
}
