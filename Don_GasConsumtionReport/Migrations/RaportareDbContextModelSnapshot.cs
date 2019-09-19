﻿// <auto-generated />
using Don_GasConsumtionReport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Don_GasConsumtionReport.Migrations
{
    [DbContext(typeof(RaportareDbContext))]
    partial class RaportareDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Don_GasConsumtionReport.ConsumGazModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Data");

                    b.Property<int>("GazValue");

                    b.Property<string>("PlcName");

                    b.HasKey("Id");

                    b.ToTable("ConsumGazModels");
                });

            modelBuilder.Entity("Don_GasConsumtionReport.IndexModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Data");

                    b.Property<int>("GazValue");

                    b.Property<int>("IndexValue");

                    b.Property<string>("PlcName");

                    b.HasKey("Id");

                    b.ToTable("IndexModels");
                });
#pragma warning restore 612, 618
        }
    }
}
