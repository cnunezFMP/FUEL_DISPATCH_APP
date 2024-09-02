using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace FUEL_DISPATCH_API.DataAccess.Models;
public partial class FUEL_DISPATCH_DBContext : DbContext
{
    public FUEL_DISPATCH_DBContext(DbContextOptions<FUEL_DISPATCH_DBContext> options)
        : base(options)
    {
    }
    public virtual DbSet<AllComsuption> AllComsuption { get; set; }
    public virtual DbSet<ArticleDataMaster> ArticleDataMaster { get; set; }
    public virtual DbSet<UsersBranchOffices> UsersBranchOffices { get; set; }
    public virtual DbSet<BranchOffices> BranchOffices { get; set; }
    public virtual DbSet<BranchIsland> BranchIslands { get; set; }
    public virtual DbSet<Booking> Booking { get; set; }
    public virtual DbSet<CalculatedComsuptionReport> CalculatedComsuption { get; set; }
    public virtual DbSet<vw_ActualStock> vw_ActualStock { get; set; }
    public virtual DbSet<vw_WareHouseHistory> Vw_WareHouseHistories { get; set; }
    public virtual DbSet<Companies> Companies { get; set; }
    public virtual DbSet<ComsuptionByDay> ComsuptionByDay { get; set; }
    public virtual DbSet<ComsuptionByMonth> ComsuptionByMonth { get; set; }
    public virtual DbSet<Dispenser> Dispenser { get; set; }
    public virtual DbSet<Driver> Driver { get; set; }
    public virtual DbSet<Part> Part { get; set; }
    public virtual DbSet<Maintenance> Maintenance { get; set; }
    public virtual DbSet<EmployeeConsumptionLimits> EmployeeConsumptionLimits { get; set; }
    public virtual DbSet<Generation> Generation { get; set; }
    public virtual DbSet<Make> Make { get; set; }
    public virtual DbSet<OdometerMeasure> Measure { get; set; }
    public virtual DbSet<DriverMethodOfComsuption> DriverMethodOfComsuption { get; set; }
    public virtual DbSet<ModEngine> ModEngine { get; set; }
    public virtual DbSet<Model> Model { get; set; }
    public virtual DbSet<Road> Road { get; set; }
    public virtual DbSet<Role> Role { get; set; }
    public virtual DbSet<Stock> Stock { get; set; }
    public virtual DbSet<WareHouse> WareHouse { get; set; }
    public virtual DbSet<WareHouseMovement> WareHouseMovement { get; set; }
    public virtual DbSet<WareHouseMovementRequest> WareHouseMovementRequest { get; set; }
    public virtual DbSet<vw_LicenseExpDateAlert> Vw_LicenseExpDateAlertServices { get; set; }
    public virtual DbSet<User> User { get; set; }
    public virtual DbSet<UserToken> UserToken { get; set; }
    public virtual DbSet<UsersRols> UsersRols { get; set; }
    public virtual DbSet<Vehicle> Vehicle { get; set; }
    public virtual DbSet<Zone> Zone { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AllComsuption>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("AllComsuption");

            entity.Property(e => e.TotalCalculatedComsuption).HasColumnType("decimal(38, 14)");
        });
        modelBuilder.Entity<ArticleDataMaster>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(x => x.CreatedBy)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CompanyId)
           .ValueGeneratedOnAdd()
           .HasValueGenerator<CompanyIdGenerator>();

            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();

            entity.ToTable("ArticleDataMaster");
            entity.HasOne(x => x.Company)
            .WithMany(x => x.Articles)
                    .HasForeignKey(x => x.CompanyId);
        });
        modelBuilder.Entity<BranchOffices>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("BranchOffices");
            entity.Property(x => x.CreatedBy)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CompanyId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CompanyIdGenerator>();

            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();

            entity.HasOne(d => d.Company)
            .WithMany(p => p.BranchOffices)
            .HasForeignKey(d => d.CompanyId);


            entity.HasMany(x => x.BranchIslands)
                  .WithOne()
                  .HasForeignKey(x => x.BranchOfficeId);
        });
        modelBuilder.Entity<CalculatedComsuptionReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("CalculatedComsuption");
        });
        modelBuilder.Entity<Companies>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Companies");
            entity.Property(x => x.CreatedBy)

            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CreatedAt)

            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();
            entity.HasMany(x => x.BranchOffices)
            .WithOne(x => x.Company)
            .HasForeignKey(x => x.CompanyId);

            entity.HasOne(x => x.CompanySAPParams)
                  .WithOne()
                  .HasForeignKey<CompanySAPParams>(x => x.CompanyId);

            entity.Navigation(x => x.CompanySAPParams)
                  .AutoInclude();

            entity.HasMany(x => x.BranchIslands)
                  .WithOne()
                  .HasForeignKey(x => x.CompanyId);
        });
        modelBuilder.Entity<ComsuptionByDay>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ComsuptionByDay");

            entity.Property(e => e.TotalFuelConsumed).HasColumnType("decimal(38, 0)");
        });
        modelBuilder.Entity<ComsuptionByMonth>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ComsuptionByMonth");

            entity.Property(e => e.TotalFuelConsumed).HasColumnType("decimal(38, 0)");
        });
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.CreatedBy)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CompanyId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CompanyIdGenerator>();

            entity.Property(x => x.BranchOfficeId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<BranchOfficeIdGenerator>();

            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();
            entity.HasOne(x => x.Vehicle).WithMany(x => x.Bookings).HasForeignKey(x => x.VehicleId);
            entity.HasOne(x => x.Driver).WithMany(x => x.Bookings).HasForeignKey(x => x.DriverId);
            entity.HasOne(x => x.Company).WithMany(x => x.Bookings).HasForeignKey(x => x.CompanyId);
            entity.HasOne(x => x.BranchOffice).WithMany(x => x.Bookings).HasForeignKey(x => x.BranchOfficeId);
        });
        modelBuilder.Entity<WareHouseMovement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable
                (
                    "WareHouseMovement",
                    entity =>
                    entity.HasTrigger("trg_UpdateStock")
                );

            entity.Property(x => x.CompanyId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CompanyIdGenerator>();

            entity.Property(x => x.BranchOfficeId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<BranchOfficeIdGenerator>();

            entity.Property(x => x.CreatedBy)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();
            entity.Property(x => x.Type)
            .HasConversion<EnumToStringConverter<MovementsTypesEnum>>();
            entity.HasOne(e => e.Vehicle)
            .WithMany(e => e.WareHouseMovements)
            .HasForeignKey(e => e.VehicleId);
            entity.HasOne(d => d.Driver).WithMany(p => p.WareHouseMovements).HasForeignKey(d => d.DriverId);
            entity.HasOne(e => e.BranchOffice).WithMany(e => e.WareHouseMovements).HasForeignKey(e => e.BranchOfficeId);
            entity.HasOne(d => d.Dispenser).WithMany(p => p.WareHouseMovements).HasForeignKey(d => d.DispenserId);
            entity.HasOne(e => e.Road).WithMany(e => e.WareHouseMovements).HasForeignKey(f => f.RoadId);

            entity.HasOne(e => e.WareHouse)
            .WithMany(e => e.WareHouseMovements)
            .HasForeignKey(f => f.WareHouseId);

            entity.HasOne(e => e.Company)
            .WithMany(e => e.WareHouseMovements);

            entity.HasOne(e => e.ToWareHouse)
            .WithMany(e => e.ToWareHouseMovements)
            .HasForeignKey(f => f.ToWareHouseId);

            entity.HasOne(e => e.ArticleDataMaster)
            .WithMany(e => e.WareHouseMovements)
            .HasForeignKey(f => f.ItemId);

            entity.Navigation(x => x.Vehicle).AutoInclude();
            entity.Navigation(x => x.Dispenser).AutoInclude();
            entity.Navigation(x => x.WareHouse).AutoInclude();
            entity.Navigation(x => x.ArticleDataMaster).AutoInclude();
        });
        modelBuilder.Entity<WareHouse>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("WareHouse");
            entity.Property(x => x.CreatedBy)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CompanyId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CompanyIdGenerator>();

            entity.Property(x => x.BranchOfficeId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<BranchOfficeIdGenerator>();


            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();
            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();
            entity.HasOne(x => x.Company)
            .WithMany(x => x.WareHouses)
            .HasForeignKey(x => x.CompanyId);
            entity.HasOne(e => e.BranchOffice)
            .WithMany(e => e.WareHouses)
            .HasForeignKey(e => e.BranchOfficeId);
        });

        modelBuilder.Entity<BranchIsland>(entity =>
        {
            entity.ToTable("BranchIsland");

            entity.HasKey(e => e.Id);
            entity.Property(x => x.CreatedBy)
                  .ValueGeneratedOnAdd()
                  .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CompanyId)
                  .ValueGeneratedOnAddOrUpdate()
                  .HasValueGenerator<CompanyIdGenerator>();

            entity.Property(x => x.BranchOfficeId)
                  .ValueGeneratedOnAddOrUpdate()
                  .HasValueGenerator<BranchOfficeIdGenerator>();

            entity.Property(x => x.CreatedAt)
                  .ValueGeneratedOnAdd()
                  .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
                  .ValueGeneratedOnUpdate()
                  .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.UpdatedAt)
                  .ValueGeneratedOnUpdate()
                  .HasValueGenerator<DateTimeGenerator>();

            entity.HasOne(e => e.BranchOffice)
                  .WithMany(e => e.BranchIslands)
                  .HasForeignKey(e => e.BranchOfficeId);

            entity.HasOne(e => e.Company)
                  .WithMany(e => e.BranchIslands)
                  .HasForeignKey(e => e.CompanyId);

        });
        modelBuilder.Entity<Dispenser>(entity =>
        {
            entity.ToTable("Dispenser");
            entity.HasKey(e => e.Id);
            entity.Property(x => x.CreatedBy)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();

            entity.HasOne(e => e.BranchIsland)
            .WithMany()
            .HasForeignKey(e => e.BranchIslandId);

            entity.Property(x => x.CompanyId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CompanyIdGenerator>();

            entity.Property(x => x.BranchOfficeId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<BranchOfficeIdGenerator>();

            entity.HasOne(e => e.BranchOffice)
            .WithMany(e => e.Dispensers)
            .HasForeignKey(e => e.BranchOfficeId);

            entity.HasOne(e => e.Company)
            .WithMany(e => e.Dispensers)
            .HasForeignKey(e => e.CompanyId);
        });
        modelBuilder.Entity<Driver>(entity =>
        {
            entity.ToTable("Driver");
            entity.HasKey(e => e.Id);

            entity.Property(x => x.CreatedBy)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CompanyId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CompanyIdGenerator>();

            entity.Property(x => x.BranchOfficeId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<BranchOfficeIdGenerator>();

            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();
            entity.HasMany(r => r.DriverMethodsOfComsuption)
                .WithMany(d => d.Drivers)
                .UsingEntity<EmployeeConsumptionLimits>(x => x.HasOne(x => x.DriverMethodOfComsuption)
                .WithMany().HasForeignKey(x => x.DriverMethodOfComsuptionId),
            x => x.HasOne(x => x.Driver)
                .WithMany()
                .HasForeignKey(x => x.DriverId));

            entity.HasOne(r => r.Company)
                .WithMany(x => x.Drivers)
                .HasForeignKey(x => x.CompanyId);

            entity.HasOne(x => x.BranchOffice)
                .WithMany(x => x.Drivers)
                .HasForeignKey(x => x.BranchOfficeId);
        });
        modelBuilder.Entity<EmployeeConsumptionLimits>(entity =>
        {
            entity.HasKey(e => new { e.DriverId, e.DriverMethodOfComsuptionId });

            entity.Property(x => x.CreatedBy)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CompanyId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CompanyIdGenerator>();

            entity.Property(x => x.BranchOfficeId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<BranchOfficeIdGenerator>();

            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();
            entity.HasOne(d => d.Driver).WithMany(p => p.EmployeeConsumptionLimits)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.DriverMethodOfComsuption).WithMany(p => p.EmployeeConsumptionLimits)
                .HasForeignKey(d => d.DriverMethodOfComsuptionId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Company).WithMany(p => p.EmployeeConsumptionLimits).HasForeignKey(d => d.CompanyId);

            entity.HasOne(d => d.BranchOffice).WithMany(p => p.EmployeeConsumptionLimits).HasForeignKey(d => d.BranchOfficeId);
        });
        modelBuilder.Entity<Generation>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Generation");

            entity.Property(e => e.ImgUrl).HasMaxLength(2000);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Url).HasMaxLength(2000);

            entity.HasOne(d => d.Model)
                .WithMany(p => p.Generations)
                .HasForeignKey(d => d.ModelId);
        });
        modelBuilder.Entity<Make>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Make");

        });
        modelBuilder.Entity<OdometerMeasure>(entity =>
        {
            entity.ToTable("OdometerMeasure");
            entity.HasKey(e => e.Id).HasName("PK__Measures__3214EC078BD6593A");

            entity.Property(e => e.Measurename).HasMaxLength(255);
        });
        modelBuilder.Entity<DriverMethodOfComsuption>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MethodName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
        });
        modelBuilder.Entity<ModEngine>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("ModEngine");
        });
        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Part");
            entity.Property(x => x.CreatedBy)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.CompanyId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CompanyIdGenerator>();

            entity.HasOne(x => x.Company)
            .WithMany(x => x.Parts)
            .HasForeignKey(x => x.CompanyId);

        });
        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Model");

            entity.HasOne(d => d.Make)
                .WithMany(p => p.Models)
                .HasForeignKey(d => d.MakeId);
        });
        modelBuilder.Entity<Maintenance>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Maintenance");

            entity.Property(x => x.CreatedBy)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.CompanyId)
            .ValueGeneratedNever()
            .HasValueGenerator<CompanyIdGenerator>();

            entity.Property(x => x.BranchOfficeId)
            .ValueGeneratedNever()
            .HasValueGenerator<BranchOfficeIdGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();

            entity.HasOne(x => x.Vehicle)
            .WithMany(x => x.Maintenances)
            .HasForeignKey(x => x.VehicleId);

            entity.HasOne(x => x.Part)
            .WithMany(x => x.Maintenances)
            .HasForeignKey(x => x.PartId);

            entity.HasOne(x => x.Part)
            .WithMany(x => x.Maintenances)
            .HasForeignKey(x => x.PartId);
        });
        modelBuilder.Entity<Road>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Road");
            entity.Property(x => x.CreatedBy)
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.CompanyId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CompanyIdGenerator>();
            entity.HasOne(x => x.Company)
            .WithMany(x => x.Roads)
            .HasForeignKey(x => x.CompanyId);

            entity.HasOne(d => d.Zone)
            .WithMany(p => p.Road)
            .HasForeignKey(d => d.ZoneId);
        });
        modelBuilder.Entity<WareHouseMovementRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("WareHouseMovementRequest");
            entity.Property(x => x.CreatedBy)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CompanyId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CompanyIdGenerator>();

            entity.Property(x => x.BranchOfficeId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<BranchOfficeIdGenerator>();

            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.UpdatedAt)
            .HasValueGenerator<DateTimeGenerator>();
            entity.HasOne(e => e.Vehicle).WithMany(e => e.Requests).HasForeignKey(e => e.VehicleId);
            entity.HasOne(e => e.Driver).WithMany(e => e.Requests).HasForeignKey(e => e.DriverId);
            entity.Property(x => x.Type).HasConversion<EnumToStringConverter<MovementsTypesEnum>>();
            entity.HasOne(e => e.WareHouse)
            .WithMany(e => e.WareHouseMovementRequests)
            .HasForeignKey(f => f.WareHouseId);

            entity.HasOne(e => e.ToWareHouse)
            .WithMany(e => e.ToWareHouseMovementRequests)
            .HasForeignKey(f => f.ToWareHouseId);

            entity.HasOne(e => e.BranchOffice).WithMany(e => e.WareHouseMovementRequests).HasForeignKey(e => e.BranchOfficeId);

            entity.HasOne(e => e.Company).WithMany(e => e.WareHouseMovementRequests).HasForeignKey(e => e.CompanyId);
        });
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RolName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
        });
        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Stock");
            entity.HasOne(e => e.WareHouse).WithMany(e => e.Stocks).HasForeignKey(e => e.WareHouseId);
            entity.HasOne(e => e.ArticleDataMaster).WithMany(e => e.Stocks).HasForeignKey(e => e.ItemId);
        });
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity
            .HasIndex(e => e.Username, "U_Username")
            .IsUnique();

            entity.Property(x => x.CreatedBy)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CompanyId);

            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(e => e.BirthDate)
            .HasColumnType("datetime");
            entity.Property(e => e.CreatedAt)
            .HasColumnType("datetime");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullDirection)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber).HasMaxLength(155);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity
                .HasOne(d => d.Driver).WithMany(p => p.User)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("FK__Users__DriverId__1B9317B3");

            entity.HasMany(r => r.Rols)
            .WithMany(d => d.Users)
            .UsingEntity<UsersRols>(x => x.HasOne(x => x.Rol)
            .WithMany().HasForeignKey(x => x.RolId),
            x => x.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId));

            entity.HasMany(r => r.BranchOffices)
            .WithMany(d => d.Users)
            .UsingEntity<UsersBranchOffices>(x => x.HasOne(x => x.BranchOffice)
            .WithMany().HasForeignKey(x => x.BranchOfficeId),
            x => x.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId));

            entity.HasOne(d => d.Company)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.CompanyId);

        });
        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsersTok__3214EC07AD286615");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpData).HasColumnType("datetime");
        });
        modelBuilder.Entity<UsersBranchOffices>(entity =>
        {

            entity.Property(x => x.CreatedBy)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.CompanyId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CompanyIdGenerator>();

            entity.Property(x => x.BranchOfficeId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<BranchOfficeIdGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();
            entity.HasOne(e => e.User).WithMany(e => e.UsersBranchOffices).HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.BranchOffice).WithMany(e => e.UsersBranchOffices).HasForeignKey(e => e.BranchOfficeId);

            entity.HasOne(e => e.Company).WithMany(e => e.UsersBranches).HasForeignKey(e => e.CompanyId);

            entity.HasOne(e => e.BranchOffice).WithMany(e => e.UsersBranchOffices).HasForeignKey(e => e.BranchOfficeId);


        });
        modelBuilder.Entity<UsersRols>(entity =>
        {

            entity.Property(x => x.CompanyId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CompanyIdGenerator>();

            entity.Property(x => x.BranchOfficeId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<BranchOfficeIdGenerator>();

            // entity.HasKey(e => new { e.UserId, e.RolId });
            entity.HasOne(d => d.Rol).WithMany(p => p.UsersRols)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.User).WithMany(p => p.UsersRols)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.Property(x => x.CreatedBy)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();

        });
        modelBuilder.Entity<vw_ActualStock>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ActualStock");
        });
        modelBuilder.Entity<vw_LicenseExpDateAlert>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_LicenseExpDateAlert");
        });
        modelBuilder.Entity<vw_WareHouseHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_WareHouseHistory");
        });
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Ficha, "U_VToken")
                   .IsUnique();
            entity.Property(e => e.AverageConsumption).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.FuelTankCapacity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Plate)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Ficha)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(x => x.CreatedBy)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.CompanyId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CompanyIdGenerator>();

            entity.Property(x => x.BranchOfficeId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<BranchOfficeIdGenerator>();

            entity.HasOne(d => d.Generation)
                .WithMany(d => d.Vehicles)
                .HasForeignKey(d => d.GenerationId);

            entity.HasOne(d => d.Driver)
                .WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.DriverId);

            entity.HasOne(d => d.Make)
                .WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.MakeId);

            entity.HasOne(d => d.Measure).WithMany(p => p.Vehicle)
                .HasForeignKey(d => d.OdometerMeasureId);


            entity.HasOne(d => d.ModEngine)
                .WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.ModEngineId);

            entity.HasOne(d => d.Model)
                .WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.ModelId);



            entity.HasOne(d => d.Company)
                .WithMany(x => x.Vehicles)
                .HasForeignKey(x => x.CompanyId);

            entity.HasOne(d => d.BranchOffice)
                .WithMany(x => x.Vehicles)
                .HasForeignKey(x => x.BranchOfficeId);

            entity.Navigation(x => x.Driver).AutoInclude();
            entity.Navigation(x => x.Generation).AutoInclude();
            entity.Navigation(x => x.Make).AutoInclude();
            entity.Navigation(x => x.ModEngine).AutoInclude();
            entity.Navigation(x => x.Model).AutoInclude();
        });
        modelBuilder.Entity<Zone>(entity =>
        {
            entity.HasKey(e => e.Id);


            entity.Property(e => e.Status).IsUnicode(false);
            entity.Property(x => x.CreatedBy)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.UpdatedBy)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<UserNameGenerator>();

            entity.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<DateTimeGenerator>();

            entity.Property(x => x.CompanyId)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CompanyIdGenerator>();

            entity.HasOne(x => x.Company)
            .WithMany(x => x.Zones)
            .HasForeignKey(x => x.CompanyId);
        });

        modelBuilder.Entity<CompanySAPParams>(entity =>
        {
            entity.ToTable("CompanySAPParams");

            entity.HasKey(x => x.CompanyId);
        });

        OnModelCreatingGeneratedProcedures(modelBuilder);
        OnModelCreatingGeneratedFunctions(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}