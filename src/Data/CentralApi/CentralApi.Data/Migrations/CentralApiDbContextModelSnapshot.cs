﻿// <auto-generated />
namespace CentralApi.Data.Migrations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Metadata;

    [DbContext(typeof(CentralApiDbContext))]
    partial class CentralApiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CentralApi.Models.Bank", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApiAddress")
                        .IsRequired();

                    b.Property<string>("ApiKey")
                        .IsRequired();

                    b.Property<string>("BankIdentificationCardNumbers");

                    b.Property<string>("CardPaymentUrl");

                    b.Property<string>("Location")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PaymentUrl");

                    b.Property<string>("ShortName")
                        .IsRequired();

                    b.Property<string>("SwiftCode")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Banks");
                });
#pragma warning restore 612, 618
        }
    }
}
