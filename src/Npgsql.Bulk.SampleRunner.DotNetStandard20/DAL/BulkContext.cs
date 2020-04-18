﻿using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using Npgsql.Bulk.SampleRunner.DotNetStandard20.DAL;

namespace Npgsql.Bulk.DAL
{
    public class BulkContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }

        public DbSet<Address2EF> Addresses2 { get; set; }

        public BulkContext(DbContextOptions<BulkContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Address>().Property(x => x.CreatedAt)
                .HasValueGenerator<ValueGen>().ValueGeneratedOnAdd();

            modelBuilder.Entity<Address>().HasKey(x => x.AddressId);

            modelBuilder.Entity<Address>().Property(x => x.PostalCode)
                .HasConversion(x => "1" + x, x => x.Substring(1));

            modelBuilder.Entity<Address>().UseXminAsConcurrencyToken();
        }
    }

    public class ValueGen : ValueGenerator
    {
        public override bool GeneratesTemporaryValues => false;

        protected override object NextValue(EntityEntry entry)
        {
            return DateTime.Now;
        }
    }
}
