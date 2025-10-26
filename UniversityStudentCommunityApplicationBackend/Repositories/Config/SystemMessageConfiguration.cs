using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Config
{
    public class SystemMessageConfiguration : IEntityTypeConfiguration<SystemMessage>
    {

        public void Configure(EntityTypeBuilder<SystemMessage> builder)
        {
            // 1. "Code" alanını benzersiz yap (Unique Index)
            builder.HasIndex(m => m.Code).IsUnique();

            // 2. Veriyi Seed Et (Kaydetme işlemi)
            // CreatedDate'i buradan da kaldırdık.
            builder.HasData(
                new SystemMessage
                {
                    Id = 1,
                    Code = "COMING_SOON",
                    Message = "Çok yakında hizmetinizdeyiz!"
                }
                // new SystemMessage { Id = 2, Code = "MAINTENANCE_MODE", ... }
            );
        }

    }
}
