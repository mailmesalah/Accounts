﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AccountsdbEntities : DbContext
    {
        public AccountsdbEntities()
            : base("name=AccountsdbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<bank_deposits> bank_deposits { get; set; }
        public virtual DbSet<bank_withdrawals> bank_withdrawals { get; set; }
        public virtual DbSet<bill_nos> bill_nos { get; set; }
        public virtual DbSet<cash_payments> cash_payments { get; set; }
        public virtual DbSet<cash_receipts> cash_receipts { get; set; }
        public virtual DbSet<journal_vouchers> journal_vouchers { get; set; }
        public virtual DbSet<ledger_register> ledger_register { get; set; }
        public virtual DbSet<ledger_transactions> ledger_transactions { get; set; }
        public virtual DbSet<opening_balances> opening_balances { get; set; }
        public virtual DbSet<purchase> purchase { get; set; }
        public virtual DbSet<purchase_return> purchase_return { get; set; }
        public virtual DbSet<sales> sales { get; set; }
        public virtual DbSet<sales_return> sales_return { get; set; }
    }
}
