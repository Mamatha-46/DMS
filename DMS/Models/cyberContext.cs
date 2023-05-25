using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DMS.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DMS.Models
{
    public partial class cyberContext : DbContext
    {
        public cyberContext()
        {
        }

        public cyberContext(DbContextOptions<cyberContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<BrandFile> BrandFile { get; set; }
        public virtual DbSet<BrandModel> BrandModel { get; set; }
        public virtual DbSet<BrandVariant> BrandVariant { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<DistriDocu> DistriDocu { get; set; }
        public virtual DbSet<Distributor> Distributor { get; set; }
        public virtual DbSet<EmailForget> EmailForget { get; set; }
        public virtual DbSet<Industry> Industry { get; set; }
        public virtual DbSet<InvoiceLicence> InvoiceLicence { get; set; }
        public virtual DbSet<OrderEnq> OrderEnq { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductFile> ProductFile { get; set; }
        public virtual DbSet<ProductImg> ProductImg { get; set; }
        public virtual DbSet<ProductMap> ProductMap { get; set; }
        public virtual DbSet<ProductMapDis> ProductMapDis { get; set; }
        public virtual DbSet<PropInvoice> PropInvoice { get; set; }
        public virtual DbSet<ProsProd> ProsProd { get; set; }
        public virtual DbSet<ProsProdLoop> ProsProdLoop { get; set; }
        public virtual DbSet<ProsQuote> ProsQuote { get; set; }
        public virtual DbSet<Prospect> Prospect { get; set; }
        public virtual DbSet<States> States { get; set; }
        public virtual DbSet<SubUser> SubUser { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRenewal> UserRenewal { get; set; }
        public virtual DbSet<Zip> Zip { get; set; }
        public virtual DbSet<InvoiceViewModel> InvoiceViewModels { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=127.0.0.1;port=3306;user=root;password=root;database=cyber");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("brand");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.AddedBy).HasColumnName("added_by");

                entity.Property(e => e.AddedOn)
                    .HasColumnName("added_on")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Address1).HasColumnName("address1");

                entity.Property(e => e.Address2).HasColumnName("address2");

                entity.Property(e => e.BrandName)
                    .HasColumnName("brand_name")
                    .HasMaxLength(250);

                entity.Property(e => e.BrandType)
                    .HasColumnName("brand_type")
                    .HasMaxLength(250);

                entity.Property(e => e.City).HasColumnName("city");

                entity.Property(e => e.Country).HasColumnName("country");

                entity.Property(e => e.Logo)
                    .HasColumnName("logo")
                    .HasMaxLength(250);

                entity.Property(e => e.Material)
                    .HasColumnName("material")
                    .HasMaxLength(250);

                entity.Property(e => e.Province)
                    .HasColumnName("province")
                    .HasMaxLength(250);

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Zipcode)
                    .HasColumnName("zipcode")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<BrandFile>(entity =>
            {
                entity.ToTable("brand_file");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.File)
                    .HasColumnName("file")
                    .HasMaxLength(250);

                entity.Property(e => e.FileName)
                    .HasColumnName("fileName")
                    .HasMaxLength(250);

                entity.Property(e => e.FileType)
                    .HasColumnName("file_type")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<BrandModel>(entity =>
            {
                entity.ToTable("brand_model");

                entity.Property(e => e.BrandModelId).HasColumnName("brand_model_id");

                entity.Property(e => e.AddedOn)
                    .HasColumnName("added_on")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.BrandModelName)
                    .IsRequired()
                    .HasColumnName("brand_model_name")
                    .HasMaxLength(250);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("'1'");
            });

            modelBuilder.Entity<BrandVariant>(entity =>
            {
                entity.HasKey(e => e.VariantId)
                    .HasName("PRIMARY");

                entity.ToTable("brand_variant");

                entity.Property(e => e.VariantId).HasColumnName("variant_id");

                entity.Property(e => e.AddedOn)
                    .HasColumnName("added_on")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.ModelId).HasColumnName("model_id");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.VariantName)
                    .IsRequired()
                    .HasColumnName("variant_name")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.ToTable("cities");

                entity.HasIndex(e => e.CountryId)
                    .HasName("cities_test_ibfk_2");

                entity.HasIndex(e => e.StateId)
                    .HasName("cities_test_ibfk_1");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("mediumint unsigned");

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasColumnName("country_code")
                    .HasMaxLength(2)
                    .IsFixedLength();

                entity.Property(e => e.CountryId)
                    .HasColumnName("country_id")
                    .HasColumnType("mediumint unsigned");

                entity.Property(e => e.Flag)
                    .IsRequired()
                    .HasColumnName("flag")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("decimal(10,8)");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("decimal(11,8)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.StateCode)
                    .IsRequired()
                    .HasColumnName("state_code")
                    .HasMaxLength(255);

                entity.Property(e => e.StateId)
                    .HasColumnName("state_id")
                    .HasColumnType("mediumint unsigned");

                entity.Property(e => e.WikiDataId)
                    .HasColumnName("wikiDataId")
                    .HasMaxLength(255)
                    .HasComment("Rapid API GeoDB Cities");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cities_ibfk_2");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cities_ibfk_1");
            });

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.ToTable("countries");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("mediumint unsigned");

                entity.Property(e => e.Capital)
                    .HasColumnName("capital")
                    .HasMaxLength(255);

                entity.Property(e => e.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(255);

                entity.Property(e => e.CurrencyName)
                    .HasColumnName("currency_name")
                    .HasMaxLength(255);

                entity.Property(e => e.CurrencySymbol)
                    .HasColumnName("currency_symbol")
                    .HasMaxLength(255);

                entity.Property(e => e.Emoji)
                    .HasColumnName("emoji")
                    .HasMaxLength(191);

                entity.Property(e => e.EmojiU)
                    .HasColumnName("emojiU")
                    .HasMaxLength(191);

                entity.Property(e => e.Flag)
                    .IsRequired()
                    .HasColumnName("flag")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Flagico)
                    .IsRequired()
                    .HasColumnName("flagico")
                    .HasMaxLength(150);

                entity.Property(e => e.Iso2)
                    .HasColumnName("iso2")
                    .HasMaxLength(2)
                    .IsFixedLength();

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("decimal(10,8)");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("decimal(11,8)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.Native)
                    .HasColumnName("native")
                    .HasMaxLength(255);

                entity.Property(e => e.NumericCode)
                    .HasColumnName("numeric_code")
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.Phonecode)
                    .HasColumnName("phonecode")
                    .HasMaxLength(255);

                entity.Property(e => e.Region)
                    .HasColumnName("region")
                    .HasMaxLength(255);

                entity.Property(e => e.Shortname)
                    .HasColumnName("shortname")
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.Subregion)
                    .HasColumnName("subregion")
                    .HasMaxLength(255);

                entity.Property(e => e.TaxPerc).HasColumnName("tax_perc");

                entity.Property(e => e.Timezones).HasColumnName("timezones");

                entity.Property(e => e.Tld)
                    .HasColumnName("tld")
                    .HasMaxLength(255);

                entity.Property(e => e.Translations).HasColumnName("translations");

                entity.Property(e => e.WikiDataId)
                    .HasColumnName("wikiDataId")
                    .HasMaxLength(255)
                    .HasComment("Rapid API GeoDB Cities");
            });

            modelBuilder.Entity<DistriDocu>(entity =>
            {
                entity.ToTable("distri_docu");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddedBy)
                    .IsRequired()
                    .HasColumnName("added_by")
                    .HasMaxLength(250);

                entity.Property(e => e.AddedOn)
                    .HasColumnName("added_on")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.BRemark)
                    .HasColumnName("b_remark")
                    .HasComment("from Admin");

                entity.Property(e => e.BusiDocStatus)
                    .HasColumnName("busi_doc_status")
                    .HasDefaultValueSql("'2'")
                    .HasComment("1 = approve, 0 = block, 2= pending");

                entity.Property(e => e.Business)
                    .IsRequired()
                    .HasColumnName("business")
                    .HasMaxLength(250);

                entity.Property(e => e.DistriId).HasColumnName("distri_id");

                entity.Property(e => e.Gst)
                    .HasColumnName("gst")
                    .HasMaxLength(250);

                entity.Property(e => e.GstDocStatus)
                    .HasColumnName("gst_doc_status")
                    .HasDefaultValueSql("'2'")
                    .HasComment("1 = approve, 0 = block, 2= pending");

                entity.Property(e => e.Opened)
                    .HasColumnName("opened")
                    .HasComment("0 = no, 1 = yes");

                entity.Property(e => e.ReBRemarks)
                    .HasColumnName("re_b_remarks")
                    .HasComment("Bussiness Remarks from Reseller");

                entity.Property(e => e.ReGRemarks)
                    .HasColumnName("re_g_remarks")
                    .HasComment("GST Remarks from Reseller");

                entity.Property(e => e.Remarks)
                    .HasColumnName("remarks")
                    .HasComment("from Admin");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("'2'")
                    .HasComment("1=active, 0=block, 2=pending");

                entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
            });

            modelBuilder.Entity<Distributor>(entity =>
            {
                entity.HasKey(e => e.DisId)
                    .HasName("PRIMARY");

                entity.ToTable("distributor");

                entity.HasIndex(e => e.DistributorId)
                    .HasName("distributor Unique Id")
                    .IsUnique();

                entity.Property(e => e.DisId).HasColumnName("dis_id");

                entity.Property(e => e.AddedBy)
                    .HasColumnName("added_by")
                    .HasMaxLength(250);

                entity.Property(e => e.AddedOn).HasColumnName("added_on");

                entity.Property(e => e.Bank)
                    .HasColumnName("bank")
                    .HasMaxLength(250);

                entity.Property(e => e.Clauses).HasColumnName("clauses");

                entity.Property(e => e.DistributorAddress1)
                    .IsRequired()
                    .HasColumnName("distributor_address1");

                entity.Property(e => e.DistributorAddress2)
                    .IsRequired()
                    .HasColumnName("distributor_address2");

                entity.Property(e => e.DistributorCName)
                    .IsRequired()
                    .HasColumnName("distributor_c_name")
                    .HasMaxLength(250);

                entity.Property(e => e.DistributorCity).HasColumnName("distributor_city");

                entity.Property(e => e.DistributorCountry).HasColumnName("distributor_country");

                entity.Property(e => e.DistributorId)
                    .HasColumnName("distributor_id")
                    .HasMaxLength(250);

                entity.Property(e => e.DistributorName)
                    .HasColumnName("distributor_name")
                    .HasMaxLength(250);

                entity.Property(e => e.DistributorNumber).HasColumnName("distributor_number");

                entity.Property(e => e.DistributorState).HasColumnName("distributor_state");

                entity.Property(e => e.DistributorType)
                    .IsRequired()
                    .HasColumnName("distributor_type")
                    .HasMaxLength(250);

                entity.Property(e => e.DistributorVat)
                    .HasColumnName("distributor_vat")
                    .HasMaxLength(250);

                entity.Property(e => e.DistributorZip)
                    .HasColumnName("distributor_zip")
                    .HasMaxLength(250);

                entity.Property(e => e.Docfile)
                    .HasColumnName("docfile")
                    .HasMaxLength(250);

                entity.Property(e => e.Doctype)
                    .HasColumnName("doctype")
                    .HasMaxLength(250);

                entity.Property(e => e.Province)
                    .HasColumnName("province")
                    .HasMaxLength(250);

                entity.Property(e => e.Refer)
                    .HasColumnName("refer")
                    .HasMaxLength(250);

                entity.Property(e => e.Terms).HasColumnName("terms");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Website)
                    .IsRequired()
                    .HasColumnName("website")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<EmailForget>(entity =>
            {
                entity.ToTable("email_forget");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddedOn)
                    .HasColumnName("added_on")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsExpired)
                    .HasColumnName("is_expired")
                    .HasComment("1=active, 0= expired");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Industry>(entity =>
            {
                entity.ToTable("industry");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddedOn)
                    .HasColumnName("added_on")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(250);

                entity.Property(e => e.ShortName)
                    .HasColumnName("short_name")
                    .HasMaxLength(250);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasComment("0=block, 1= active");
            });

            modelBuilder.Entity<InvoiceLicence>(entity =>
            {
                entity.ToTable("invoice_licence");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.File1)
                    .HasColumnName("file1")
                    .HasMaxLength(250);

                entity.Property(e => e.File2)
                    .HasColumnName("file2")
                    .HasMaxLength(250);

                entity.Property(e => e.InvoiceId)
                    .HasColumnName("invoiceId")
                    .HasMaxLength(250);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasComment("0=not granted, 1= granted");
            });

            modelBuilder.Entity<OrderEnq>(entity =>
            {
                entity.ToTable("order_enq");

                entity.Property(e => e.OrderEnqId).HasColumnName("order_enq_id");

                entity.Property(e => e.AComment).HasColumnName("a_comment");

                entity.Property(e => e.AdddedOn)
                    .HasColumnName("addded_on")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.AddedBy)
                    .IsRequired()
                    .HasColumnName("added_by")
                    .HasMaxLength(250);

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment");

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.OrdersId).HasColumnName("orders_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Qty).HasColumnName("qty");

                entity.Property(e => e.RComment).HasColumnName("r_comment");

                entity.Property(e => e.STotal).HasColumnName("s_total");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasComment("1=aprooved, 2=pending,0=block");

                entity.Property(e => e.UnitPrice).HasColumnName("unit_price");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable("orders");

                entity.Property(e => e.OrdersId).HasColumnName("orders_id");

                entity.Property(e => e.AddedOn)
                    .HasColumnName("added_on")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasColumnName("address1");

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasColumnName("address2");

                entity.Property(e => e.CPerson)
                    .IsRequired()
                    .HasColumnName("c_person")
                    .HasMaxLength(250);

                entity.Property(e => e.CPhone).HasColumnName("c_phone");

                entity.Property(e => e.CType).HasColumnName("c_type");

                entity.Property(e => e.CVat)
                    .IsRequired()
                    .HasColumnName("c_vat")
                    .HasMaxLength(250);

                entity.Property(e => e.City).HasColumnName("city");

                entity.Property(e => e.Company)
                    .IsRequired()
                    .HasColumnName("company")
                    .HasMaxLength(250);

                entity.Property(e => e.Country).HasColumnName("country");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(250);

                entity.Property(e => e.OrderAmt).HasColumnName("order_amt");

                entity.Property(e => e.OrderedBy).HasColumnName("ordered_by");

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("'1'")
                    .HasComment("1=aprooved, 2=pending,0=block");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.HasIndex(e => e.ProductCode)
                    .HasName("product_code")
                    .IsUnique();

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.AddedBy)
                    .HasColumnName("added_by")
                    .HasMaxLength(250);

                entity.Property(e => e.AddedOn).HasColumnName("added_on");

                entity.Property(e => e.Brand).HasColumnName("brand");

                entity.Property(e => e.FromNode).HasColumnName("from_node");

                entity.Property(e => e.Model).HasColumnName("model");

                entity.Property(e => e.ProductCode)
                    .HasColumnName("product_code")
                    .HasMaxLength(250);

                entity.Property(e => e.ProductDesc).HasColumnName("product_desc");

                entity.Property(e => e.ProductImg).HasColumnName("product_img");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnName("product_name")
                    .HasMaxLength(250);

                entity.Property(e => e.ProductPrice).HasColumnName("product_price");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasComment("0=block, 1=active, 2= pending, 3=deleted");

                entity.Property(e => e.ToNode).HasColumnName("to_node");

                entity.Property(e => e.Variant)
                    .HasColumnName("variant")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<ProductFile>(entity =>
            {
                entity.ToTable("product_file");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.File)
                    .IsRequired()
                    .HasColumnName("file")
                    .HasMaxLength(250);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("fileName")
                    .HasMaxLength(250);

                entity.Property(e => e.FileType)
                    .IsRequired()
                    .HasColumnName("file_type")
                    .HasMaxLength(250);

                entity.Property(e => e.ProductId).HasColumnName("product_id");
            });

            modelBuilder.Entity<ProductImg>(entity =>
            {
                entity.ToTable("product_img");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("image")
                    .HasMaxLength(255);

                entity.Property(e => e.ProductId).HasColumnName("product_id");
            });

            modelBuilder.Entity<ProductMap>(entity =>
            {
                entity.ToTable("product_map");

                entity.Property(e => e.ProductMapId).HasColumnName("product_map_id");

                entity.Property(e => e.ADiscount).HasColumnName("a_discount");

                entity.Property(e => e.AddedOn).HasColumnName("added_on");

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.RId).HasColumnName("r_id");
            });

            modelBuilder.Entity<ProductMapDis>(entity =>
            {
                entity.ToTable("product_map_dis");

                entity.Property(e => e.ProductMapDisId).HasColumnName("product_map_dis_id");

                entity.Property(e => e.DisEnd).HasColumnName("dis_end");

                entity.Property(e => e.DisStart).HasColumnName("dis_start");

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.ProductMapId).HasColumnName("product_map_id");
            });

            modelBuilder.Entity<PropInvoice>(entity =>
            {
                entity.ToTable("prop_invoice");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ARead)
                    .HasColumnName("A_read")
                    .HasDefaultValueSql("'0'")
                    .HasComment("0= no, 1= yes");

                entity.Property(e => e.AddedOn)
                    .HasColumnName("added_on")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Invoice)
                    .HasColumnName("invoice")
                    .HasMaxLength(250);

                entity.Property(e => e.InvoiceAmt).HasColumnName("invoice_amt");

                entity.Property(e => e.InvoiceId)
                    .IsRequired()
                    .HasColumnName("invoice_id")
                    .HasMaxLength(250);

                entity.Property(e => e.Licence)
                    .HasColumnName("licence")
                    .HasComment("0=pending, 1=granted");

                entity.Property(e => e.PaymentAddedon).HasColumnName("payment_addedon");

                entity.Property(e => e.PaymentAddedon1).HasColumnName("payment_addedon1");

                entity.Property(e => e.PaymentMode)
                    .HasColumnName("payment_mode")
                    .HasMaxLength(250);

                entity.Property(e => e.PaymentRefImg)
                    .HasColumnName("payment_refImg")
                    .HasMaxLength(250);

                entity.Property(e => e.PaymentTranId)
                    .HasColumnName("payment_tranId")
                    .HasMaxLength(250);

                entity.Property(e => e.ProspectId).HasColumnName("prospect_id");

                entity.Property(e => e.Readed)
                    .HasColumnName("readed")
                    .HasComment("0=no, 1=yes");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasComment("0=not added, 1=paid, 2=approved");
            });

            modelBuilder.Entity<ProsProd>(entity =>
            {
                entity.ToTable("pros_prod");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddedOn)
                    .HasColumnName("added_on")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.EQty).HasColumnName("e_qty");

                entity.Property(e => e.FStage)
                    .HasColumnName("fStage")
                    .HasMaxLength(40)
                    .IsFixedLength();

                entity.Property(e => e.InvoiceId).HasColumnName("invoiceId");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.ProsId).HasColumnName("pros_id");

                entity.Property(e => e.STotal).HasColumnName("s_total");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UPrice).HasColumnName("u_price");
            });

            modelBuilder.Entity<ProsProdLoop>(entity =>
            {
                entity.ToTable("pros_prod_loop");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Comments)
                    .IsRequired()
                    .HasColumnName("comments");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.ProsProdId).HasColumnName("pros_prod_id");

                entity.Property(e => e.Stage)
                    .IsRequired()
                    .HasColumnName("stage")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<ProsQuote>(entity =>
            {
                entity.ToTable("pros_quote");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddedOn).HasColumnName("added_on");

                entity.Property(e => e.ProspectId).HasColumnName("prospect_id");

                entity.Property(e => e.Quote)
                    .IsRequired()
                    .HasColumnName("quote")
                    .HasMaxLength(250);

                entity.Property(e => e.QuoteAmt).HasColumnName("quote_amt");

                entity.Property(e => e.QuoteId)
                    .HasColumnName("quote_id")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Prospect>(entity =>
            {
                entity.ToTable("prospect");

                entity.HasIndex(e => e.Email)
                    .HasName("email")
                    .IsUnique();

                entity.HasIndex(e => e.Number)
                    .HasName("number")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AReaded)
                    .HasColumnName("A_readed")
                    .HasComment("0=no, 1=yes");

                entity.Property(e => e.AddedBy)
                    .HasColumnName("added_by")
                    .HasMaxLength(250);

                entity.Property(e => e.AddedId).HasColumnName("added_id");

                entity.Property(e => e.AddedOn).HasColumnName("added_on");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasColumnName("address1");

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasColumnName("address2");

                entity.Property(e => e.Bank)
                    .HasColumnName("bank")
                    .HasMaxLength(250);

                entity.Property(e => e.CPerson)
                    .HasColumnName("c_person")
                    .HasMaxLength(250);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(250);

                entity.Property(e => e.Converted)
                    .HasColumnName("converted")
                    .HasDefaultValueSql("'0'")
                    .HasComment("0=pros, 1=client");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.CountryName)
                    .HasColumnName("country_name")
                    .HasMaxLength(250);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(250);

                entity.Property(e => e.Gst)
                    .HasColumnName("gst")
                    .HasMaxLength(250);

                entity.Property(e => e.IndustryType)
                    .HasColumnName("industry_type")
                    .HasMaxLength(250);

                entity.Property(e => e.LeadType)
                    .HasColumnName("lead_type")
                    .HasMaxLength(250);

                entity.Property(e => e.Logo)
                    .IsRequired()
                    .HasColumnName("logo")
                    .HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(250);

                entity.Property(e => e.Number).HasColumnName("number");

                entity.Property(e => e.ProsType)
                    .HasColumnName("pros_type")
                    .HasComment("0= prospect, 1= client");

                entity.Property(e => e.Province)
                    .HasColumnName("province")
                    .HasMaxLength(250);

                entity.Property(e => e.Readed)
                    .HasColumnName("readed")
                    .HasDefaultValueSql("'0'")
                    .HasComment("0=no, 1=yes");

                entity.Property(e => e.Source)
                    .HasColumnName("source")
                    .HasMaxLength(250);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasMaxLength(250);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Terms).HasColumnName("terms");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasMaxLength(250);

                entity.Property(e => e.UpdatedId).HasColumnName("updated_id");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnName("updated_on")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Website)
                    .HasColumnName("website")
                    .HasMaxLength(250);

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<States>(entity =>
            {
                entity.ToTable("states");

                entity.HasIndex(e => e.CountryId)
                    .HasName("country_region");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("mediumint unsigned");

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasColumnName("country_code")
                    .HasMaxLength(2)
                    .IsFixedLength();

                entity.Property(e => e.CountryId)
                    .HasColumnName("country_id")
                    .HasColumnType("mediumint unsigned");

                entity.Property(e => e.FipsCode)
                    .HasColumnName("fips_code")
                    .HasMaxLength(255);

                entity.Property(e => e.Flag)
                    .IsRequired()
                    .HasColumnName("flag")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Iso2)
                    .HasColumnName("iso2")
                    .HasMaxLength(255);

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("decimal(10,8)");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("decimal(11,8)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(191);

                entity.Property(e => e.WikiDataId)
                    .HasColumnName("wikiDataId")
                    .HasMaxLength(255)
                    .HasComment("Rapid API GeoDB Cities");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.States)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("country_region_final");
            });

            modelBuilder.Entity<SubUser>(entity =>
            {
                entity.ToTable("sub_user");

                entity.Property(e => e.SubUserId).HasColumnName("sub_user_id");

                entity.Property(e => e.AddedBy).HasColumnName("added_by");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserType)
                    .IsRequired()
                    .HasColumnName("user_type")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.ToTable("test");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(250);

                entity.Property(e => e.Number).HasColumnName("number");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.UserMail)
                    .HasName("user_mail")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.AReaded)
                    .HasColumnName("A_readed")
                    .HasComment("0=no, 1= yes");

                entity.Property(e => e.AddedOn)
                    .HasColumnName("added_on")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Bank)
                    .HasColumnName("bank")
                    .HasMaxLength(250);

                entity.Property(e => e.Pwd)
                    .IsRequired()
                    .HasColumnName("pwd")
                    .HasMaxLength(250);

                entity.Property(e => e.Readed)
                    .HasColumnName("readed")
                    .HasComment("0=no, 1=yes");

                entity.Property(e => e.UserFname)
                    .IsRequired()
                    .HasColumnName("user_Fname")
                    .HasMaxLength(250);

                entity.Property(e => e.UserLname)
                    .HasColumnName("user_Lname")
                    .HasMaxLength(250);

                entity.Property(e => e.UserLogo)
                    .IsRequired()
                    .HasColumnName("user_logo")
                    .HasMaxLength(250);

                entity.Property(e => e.UserMail)
                    .IsRequired()
                    .HasColumnName("user_mail")
                    .HasMaxLength(250);

                entity.Property(e => e.UserNumber).HasColumnName("user_number");

                entity.Property(e => e.UserPic)
                    .IsRequired()
                    .HasColumnName("user_pic");

                entity.Property(e => e.UserPwd)
                    .IsRequired()
                    .HasColumnName("user_pwd")
                    .HasMaxLength(250);

                entity.Property(e => e.UserStatus)
                    .HasColumnName("user_status")
                    .HasComment("1=active, 2=pending, 0=block");

                entity.Property(e => e.UserType)
                    .IsRequired()
                    .HasColumnName("user_type")
                    .HasMaxLength(250)
                    .HasComment("A=admin, R= reseller, SR=sub-reseller");
            });

            modelBuilder.Entity<UserRenewal>(entity =>
            {
                entity.ToTable("user_renewal");

                entity.Property(e => e.UserRenewalId).HasColumnName("user_renewal_id");

                entity.Property(e => e.Agreement)
                    .HasColumnName("agreement")
                    .HasMaxLength(250);

                entity.Property(e => e.CidAgreement)
                    .HasColumnName("cid_agreement")
                    .HasMaxLength(250);

                entity.Property(e => e.CidConfirmAdmin)
                    .HasColumnName("cid_confirm_admin")
                    .HasDefaultValueSql("'2'")
                    .HasComment("1 = approve, 2= pending 0= block	");

                entity.Property(e => e.CidRSign)
                    .HasColumnName("cid_r_sign")
                    .HasComment("1 = approve, 2= pending 0= block	");

                entity.Property(e => e.CidRSignedAgreement)
                    .HasColumnName("cid_r_signed_agreement")
                    .HasMaxLength(250);

                entity.Property(e => e.CidRemarks).HasColumnName("cid_remarks");

                entity.Property(e => e.CompAgreement)
                    .HasColumnName("comp_agreement")
                    .HasMaxLength(250);

                entity.Property(e => e.CompAgreement2)
                    .HasColumnName("comp_agreement_2")
                    .HasMaxLength(250);

                entity.Property(e => e.CompRemarks).HasColumnName("comp_remarks");

                entity.Property(e => e.ConfirmAdmin)
                    .HasColumnName("confirm_admin")
                    .HasDefaultValueSql("'2'")
                    .HasComment("1 = approve, 2= pending 0= block");

                entity.Property(e => e.RSign)
                    .HasColumnName("r_sign")
                    .HasComment("1 = approve, 2= pending 0= block	");

                entity.Property(e => e.RSignedAgreement)
                    .HasColumnName("r_signed_agreement")
                    .HasMaxLength(250);

                entity.Property(e => e.Readed)
                    .HasColumnName("readed")
                    .HasComment("0=no, 1=yes");

                entity.Property(e => e.Remarks).HasColumnName("remarks");

                entity.Property(e => e.Terms).HasColumnName("terms");

                entity.Property(e => e.Terms2).HasColumnName("terms2");

                entity.Property(e => e.Terms3)
                    .HasColumnName("terms3")
                    .HasComment("Discount Terms");

                entity.Property(e => e.UserFrom).HasColumnName("user_from");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasComment(" distributor ID");

                entity.Property(e => e.UserTo).HasColumnName("user_to");
            });

            modelBuilder.Entity<Zip>(entity =>
            {
                entity.ToTable("zip");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Accuracy)
                    .HasColumnName("accuracy")
                    .HasComment("accuracy of lat/lng from 1=estimated, 4=geonameid, 6=centroid of addresses or shape");

                entity.Property(e => e.AreaName)
                    .HasColumnName("area_name")
                    .HasMaxLength(250);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(200);

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(200);

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.Division1)
                    .IsRequired()
                    .HasColumnName("division1")
                    .HasMaxLength(200);

                entity.Property(e => e.Division2)
                    .IsRequired()
                    .HasColumnName("division2")
                    .HasMaxLength(200);

                entity.Property(e => e.Latitude)
                    .IsRequired()
                    .HasColumnName("latitude")
                    .HasMaxLength(20)
                    .HasComment("estimated latitude (wgs84)");

                entity.Property(e => e.Longitude)
                    .IsRequired()
                    .HasColumnName("longitude")
                    .HasMaxLength(20)
                    .HasComment("estimated longitude (wgs84)");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasMaxLength(200);

                entity.Property(e => e.StateId).HasColumnName("state_id");

                entity.Property(e => e.Zipcode)
                    .IsRequired()
                    .HasColumnName("zipcode")
                    .HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<DMS.Models.prospectviewmodel> prospectviewmodel { get; set; }

        public DbSet<DMS.Models.ResellerDetailsView> ResellerDetailsView { get; set; }

        public DbSet<DMS.Models.ProductMapView> ProductMapView { get; set; }

        public DbSet<DMS.Models.IndustryViewModel> IndustryViewModel { get; set; }
    }
}
