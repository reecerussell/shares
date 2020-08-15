using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Shares.Core;
using Shares.Orders.Infrastructure.Configuration;

namespace Shares.Orders.Infrastructure
{
    public class OrdersReadContext : DbContext
    {
        private readonly IConnectionStringProvider _provider;

        public OrdersReadContext(IConnectionStringProvider provider)
        {
            _provider = provider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new InstrumentConfiguration());
            modelBuilder.ApplyConfiguration(new SellOrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            optionsBuilder.UseMySQL(_provider.Get());

            base.OnConfiguring(optionsBuilder);
        }
    }
}
