using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

public class CounterSketchLibraryDataEntities1 : DbContext
{
	public DbSet<Attribute> Attributes { get; set; }

	public DbSet<AttributeLevel> AttributeLevels { get; set; }

	public DbSet<AttributeType> AttributeTypes { get; set; }

	public DbSet<Component> Components { get; set; }

	public DbSet<DesignAttribute> DesignAttributes { get; set; }

	public DbSet<DesignComponent> DesignComponents { get; set; }

	public DbSet<DesignGem> DesignGems { get; set; }

	public DbSet<DesignGemTable> DesignGemTables { get; set; }

	public DbSet<DesignGroup> DesignGroups { get; set; }

	public DbSet<DesignGroupDesign> DesignGroupDesigns { get; set; }

	public DbSet<Design> Designs { get; set; }

	public DbSet<GemTableShapeSize> GemTableShapeSizes { get; set; }

	public DbSet<DesignSearch> DesignSearches { get; set; }

	public CounterSketchLibraryDataEntities1(string connectionStringNameToUse)
		: base(connectionStringNameToUse)
	{
	}

	protected override void OnModelCreating(DbModelBuilder modelBuilder)
	{
		throw new UnintentionalCodeFirstException();
	}
}
