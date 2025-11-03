using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SmartMeter.Models;

namespace SmartMeter.Data;

public partial class SmartMeterDbContext : DbContext
{
    public SmartMeterDbContext()
    {
    }

    public SmartMeterDbContext(DbContextOptions<SmartMeterDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Arrear> Arrears { get; set; }

    public virtual DbSet<Billing> Billings { get; set; }

    public virtual DbSet<Consumer> Consumers { get; set; }

    public virtual DbSet<Meter> Meters { get; set; }

    public virtual DbSet<Meterreading> Meterreadings { get; set; }

    public virtual DbSet<Orgunit> Orgunits { get; set; }

    public virtual DbSet<Tariff> Tariffs { get; set; }

    public virtual DbSet<Tariffdetail> Tariffdetails { get; set; }

    public virtual DbSet<Tariffslab> Tariffslabs { get; set; }

    public virtual DbSet<Todrule> Todrules { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=SmartMeterDatabase;Username=postgres;Password=Admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Addressid).HasName("address_pkey");

            entity.ToTable("address");

            entity.Property(e => e.Addressid).HasColumnName("addressid");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Houseno)
                .HasMaxLength(50)
                .HasColumnName("houseno");
            entity.Property(e => e.Lanelocality)
                .HasMaxLength(200)
                .HasColumnName("lanelocality");
            entity.Property(e => e.Pincode)
                .HasMaxLength(20)
                .HasColumnName("pincode");
            entity.Property(e => e.States)
                .HasMaxLength(100)
                .HasColumnName("states");
        });

        modelBuilder.Entity<Arrear>(entity =>
        {
            entity.HasKey(e => e.Aid).HasName("arrears_pkey");

            entity.ToTable("arrears");

            entity.Property(e => e.Aid).HasColumnName("aid");
            entity.Property(e => e.Atype)
                .HasMaxLength(50)
                .HasColumnName("atype");
            entity.Property(e => e.Billid).HasColumnName("billid");
            entity.Property(e => e.Consumerid).HasColumnName("consumerid");
            entity.Property(e => e.Paidstatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Unpaid'::character varying")
                .HasColumnName("paidstatus");

            entity.HasOne(d => d.Bill).WithMany(p => p.Arrears)
                .HasForeignKey(d => d.Billid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("arrears_billid_fkey");

            entity.HasOne(d => d.Consumer).WithMany(p => p.Arrears)
                .HasForeignKey(d => d.Consumerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("arrears_consumerid_fkey");
        });

        modelBuilder.Entity<Billing>(entity =>
        {
            entity.HasKey(e => e.Billid).HasName("billing_pkey");

            entity.ToTable("billing");

            entity.Property(e => e.Billid).HasColumnName("billid");
            entity.Property(e => e.Baseamount)
                .HasPrecision(18, 4)
                .HasColumnName("baseamount");
            entity.Property(e => e.Billingperiodend).HasColumnName("billingperiodend");
            entity.Property(e => e.Billingperiodstart).HasColumnName("billingperiodstart");
            entity.Property(e => e.Consumerid).HasColumnName("consumerid");
            entity.Property(e => e.Disconnectiondate)
                .HasPrecision(3)
                .HasColumnName("disconnectiondate");
            entity.Property(e => e.Duedate).HasColumnName("duedate");
            entity.Property(e => e.Generatedat)
                .HasPrecision(3)
                .HasDefaultValueSql("now()")
                .HasColumnName("generatedat");
            entity.Property(e => e.Meterid)
                .HasMaxLength(50)
                .HasColumnName("meterid");
            entity.Property(e => e.Paiddate)
                .HasPrecision(3)
                .HasColumnName("paiddate");
            entity.Property(e => e.Paymentstatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Unpaid'::character varying")
                .HasColumnName("paymentstatus");
            entity.Property(e => e.Taxamount)
                .HasPrecision(18, 4)
                .HasColumnName("taxamount");
            entity.Property(e => e.Totalamount)
                .HasPrecision(18, 4)
                .HasComputedColumnSql("(baseamount + taxamount)", true)
                .HasColumnName("totalamount");
            entity.Property(e => e.Totalunitsconsumed)
                .HasPrecision(18, 6)
                .HasColumnName("totalunitsconsumed");

            entity.HasOne(d => d.Consumer).WithMany(p => p.Billings)
                .HasForeignKey(d => d.Consumerid)
                .HasConstraintName("billing_consumerid_fkey");

            entity.HasOne(d => d.Meter).WithMany(p => p.Billings)
                .HasForeignKey(d => d.Meterid)
                .HasConstraintName("billing_meterid_fkey");
        });

        modelBuilder.Entity<Consumer>(entity =>
        {
            entity.HasKey(e => e.Consumerid).HasName("consumer_pkey");

            entity.ToTable("consumer");

            entity.HasIndex(e => e.Email, "consumer_email_key").IsUnique();

            entity.Property(e => e.Consumerid).HasColumnName("consumerid");
            entity.Property(e => e.Addressid).HasColumnName("addressid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp with time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby)
                .HasMaxLength(100)
                .HasDefaultValueSql("'system'::character varying")
                .HasColumnName("createdby");
            entity.Property(e => e.Deleted)
                .HasDefaultValue(false)
                .HasColumnName("deleted");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Orgunitid).HasColumnName("orgunitid");
            entity.Property(e => e.Passwordhash)
                .HasDefaultValueSql("'\\x3635366665626565'::bytea")
                .HasColumnName("passwordhash");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Tariffid).HasColumnName("tariffid");
            entity.Property(e => e.Updatedat)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("updatedat");
            entity.Property(e => e.Updatedby)
                .HasMaxLength(100)
                .HasColumnName("updatedby");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Address).WithMany(p => p.Consumers)
                .HasForeignKey(d => d.Addressid)
                .HasConstraintName("consumer_addressid_fkey");

            entity.HasOne(d => d.Orgunit).WithMany(p => p.Consumers)
                .HasForeignKey(d => d.Orgunitid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("consumer_orgunitid_fkey");

            entity.HasOne(d => d.Tariff).WithMany(p => p.Consumers)
                .HasForeignKey(d => d.Tariffid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("consumer_tariffid_fkey");
        });

        modelBuilder.Entity<Meter>(entity =>
        {
            entity.HasKey(e => e.Meterserialno).HasName("meter_pkey");

            entity.ToTable("meter");

            entity.Property(e => e.Meterserialno)
                .HasMaxLength(50)
                .HasColumnName("meterserialno");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.Consumerid).HasColumnName("consumerid");
            entity.Property(e => e.Firmware)
                .HasMaxLength(50)
                .HasColumnName("firmware");
            entity.Property(e => e.Iccid)
                .HasMaxLength(30)
                .HasColumnName("iccid");
            entity.Property(e => e.Imsi)
                .HasMaxLength(30)
                .HasColumnName("imsi");
            entity.Property(e => e.Installtsutc)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("installtsutc");
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(45)
                .HasColumnName("ipaddress");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(100)
                .HasColumnName("manufacturer");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");

            entity.HasOne(d => d.Consumer).WithMany(p => p.Meters)
                .HasForeignKey(d => d.Consumerid)
                .HasConstraintName("meter_consumerid_fkey");
        });

        modelBuilder.Entity<Meterreading>(entity =>
        {
            entity.HasKey(e => e.Meterreadingid).HasName("meterreading_pkey");

            entity.ToTable("meterreading");

            entity.Property(e => e.Meterreadingid).HasColumnName("meterreadingid");
            entity.Property(e => e.Current)
                .HasPrecision(10, 3)
                .HasColumnName("current");
            entity.Property(e => e.Energyconsumed)
                .HasPrecision(18, 6)
                .HasColumnName("energyconsumed");
            entity.Property(e => e.Meterid)
                .HasMaxLength(50)
                .HasColumnName("meterid");
            entity.Property(e => e.Meterreadingdate)
                .HasPrecision(3)
                .HasColumnName("meterreadingdate");
            entity.Property(e => e.Voltage)
                .HasPrecision(10, 3)
                .HasColumnName("voltage");

            entity.HasOne(d => d.Meter).WithMany(p => p.Meterreadings)
                .HasForeignKey(d => d.Meterid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("meterreading_meterid_fkey");
        });

        modelBuilder.Entity<Orgunit>(entity =>
        {
            entity.HasKey(e => e.Orgunitid).HasName("orgunit_pkey");

            entity.ToTable("orgunit");

            entity.HasIndex(e => e.Type, "ix_orgunit_type_including");

            entity.Property(e => e.Orgunitid).HasColumnName("orgunitid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Parentid).HasColumnName("parentid");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.Parentid)
                .HasConstraintName("orgunit_parentid_fkey");
        });

        modelBuilder.Entity<Tariff>(entity =>
        {
            entity.HasKey(e => e.Tariffid).HasName("tariff_pkey");

            entity.ToTable("tariff");

            entity.Property(e => e.Tariffid).HasColumnName("tariffid");
            entity.Property(e => e.Baserate)
                .HasPrecision(18, 4)
                .HasColumnName("baserate");
            entity.Property(e => e.Effectivefrom).HasColumnName("effectivefrom");
            entity.Property(e => e.Effectiveto).HasColumnName("effectiveto");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Taxrate)
                .HasPrecision(18, 4)
                .HasColumnName("taxrate");
        });

        modelBuilder.Entity<Tariffdetail>(entity =>
        {
            entity.HasKey(e => e.Tariffdetailsid).HasName("tariffdetails_pkey");

            entity.ToTable("tariffdetails");

            entity.Property(e => e.Tariffdetailsid).HasColumnName("tariffdetailsid");
            entity.Property(e => e.Tariffid).HasColumnName("tariffid");
            entity.Property(e => e.Tariffslabid).HasColumnName("tariffslabid");
            entity.Property(e => e.Tarifftodid).HasColumnName("tarifftodid");

            entity.HasOne(d => d.Tariff).WithMany(p => p.Tariffdetails)
                .HasForeignKey(d => d.Tariffid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tariffdetails_tariffid_fkey");

            entity.HasOne(d => d.Tariffslab).WithMany(p => p.Tariffdetails)
                .HasForeignKey(d => d.Tariffslabid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tariffdetails_tariffslabid_fkey");

            entity.HasOne(d => d.Tarifftod).WithMany(p => p.Tariffdetails)
                .HasForeignKey(d => d.Tarifftodid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tariffdetails_tarifftodid_fkey");
        });

        modelBuilder.Entity<Tariffslab>(entity =>
        {
            entity.HasKey(e => e.Tariffslabid).HasName("tariffslab_pkey");

            entity.ToTable("tariffslab");

            entity.Property(e => e.Tariffslabid).HasColumnName("tariffslabid");
            entity.Property(e => e.Deleted)
                .HasDefaultValue(false)
                .HasColumnName("deleted");
            entity.Property(e => e.Fromkwh)
                .HasPrecision(18, 6)
                .HasColumnName("fromkwh");
            entity.Property(e => e.Rateperkwh)
                .HasPrecision(18, 6)
                .HasColumnName("rateperkwh");
            entity.Property(e => e.Tariffid).HasColumnName("tariffid");
            entity.Property(e => e.Tokwh)
                .HasPrecision(18, 6)
                .HasColumnName("tokwh");

            entity.HasOne(d => d.Tariff).WithMany(p => p.Tariffslabs)
                .HasForeignKey(d => d.Tariffid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tariffslab_tariffid_fkey");
        });

        modelBuilder.Entity<Todrule>(entity =>
        {
            entity.HasKey(e => e.Todruleid).HasName("todrule_pkey");

            entity.ToTable("todrule");

            entity.HasIndex(e => e.Name, "ix_todrule_name");

            entity.Property(e => e.Todruleid).HasColumnName("todruleid");
            entity.Property(e => e.Deleted)
                .HasDefaultValue(false)
                .HasColumnName("deleted");
            entity.Property(e => e.Endtime)
                .HasPrecision(0)
                .HasColumnName("endtime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Rateperkwh)
                .HasPrecision(18, 6)
                .HasColumnName("rateperkwh");
            entity.Property(e => e.Starttime)
                .HasPrecision(0)
                .HasColumnName("starttime");
            entity.Property(e => e.Tariffid).HasColumnName("tariffid");

            entity.HasOne(d => d.Tariff).WithMany(p => p.Todrules)
                .HasForeignKey(d => d.Tariffid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("todrule_tariffid_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("User_pkey");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "User_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "User_username_key").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Displayname)
                .HasMaxLength(150)
                .HasColumnName("displayname");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Lastloginutc)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("lastloginutc");
            entity.Property(e => e.Passwordhash).HasColumnName("passwordhash");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .HasColumnName("phone");
            entity.Property(e => e.Profilepic).HasColumnName("profilepic");
            entity.Property(e => e.Roles)
                .HasMaxLength(15)
                .HasColumnName("roles");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
