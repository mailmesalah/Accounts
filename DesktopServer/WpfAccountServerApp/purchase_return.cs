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
    
    public partial class purchase_return
    {
        public int id { get; set; }
        public string bill_no { get; set; }
        public System.DateTime bill_date_time { get; set; }
        public int serial_no { get; set; }
        public string ledger_code { get; set; }
        public string ledger { get; set; }
        public string narration { get; set; }
        public decimal debit { get; set; }
        public decimal credit { get; set; }
        public string financial_code { get; set; }
        public string ref_bill_no { get; set; }
        public Nullable<System.DateTime> ref_bill_date_time { get; set; }
        public string party_code { get; set; }
        public string party { get; set; }
        public decimal bill_amount { get; set; }
    }
}
