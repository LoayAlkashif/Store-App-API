using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.Order;

namespace Store.Repository.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(O => O.SubTotal).HasColumnType("decimal(18,2)");
            builder.Property(O => O.Status)
                .HasConversion(OStatus => OStatus.ToString(),
                OStatus => (OrderStatus) Enum.Parse(typeof(OrderStatus), OStatus));
            //table order hyb8a fe goz2 mn el table bta3tak el data bta3et el address
            builder.OwnsOne(O => O.ShippingAddress, SA => SA.WithOwner());

            // relationship
            builder.HasOne(o => o.DeliveryMethod).WithMany().HasForeignKey(o => o.DeliveryMethodId);
        }
    }
}
