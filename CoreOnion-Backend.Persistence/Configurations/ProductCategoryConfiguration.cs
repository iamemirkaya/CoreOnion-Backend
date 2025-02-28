using CoreOnion_Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Persistence.Configurations
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.CategoryId });

            builder.HasOne(p => p.Product)
                .WithMany(pc => pc.ProductCategories)
                .HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(c => c.Category)
                .WithMany(pc => pc.ProductCategories)
                .HasForeignKey(c => c.CategoryId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
