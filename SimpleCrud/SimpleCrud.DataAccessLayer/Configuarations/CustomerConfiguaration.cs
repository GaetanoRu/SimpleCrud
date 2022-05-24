using SimpleCrud.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCrud.DataAccessLayer.Configuarations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.StreetAddress)
                .HasMaxLength(100);

            builder.Property(c => c.City)
                .HasMaxLength(100);

            builder.Property(c => c.Country)
                .HasMaxLength(50);

            builder.Property(c => c.PostalCode)
                .HasMaxLength(5);

            builder.Property(c => c.Phone)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}
