# Entity framework

## EF core 2.0 Database First - xUnitTest

- Install packages
```
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
```

- Create the database add a table 

```
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RadioStation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](250) NULL
) ON [PRIMARY]
```


- Map the database table to the entity
```
public class RadioStation
{
    public int Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}
```

- Create a context 
```
public class StudyContext : DbContext
{
    public DbSet<RadioStation> RadioStation { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=DESKTOP-5PDDBJC;Database=StudyMvcAngular;Trusted_Connection=True;");
    }
}
```

- Add a xUnitTest for testing the context
```
[Fact]
public void Test1()
{
    using(var db = new StudyContext())
    {
        var radioStations = db.RadioStation.ToList();
    }
}
```   

## EF core 2.0 Code First - xUnitTest  

- Install packages
```
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
```

- Create an entity with annotation 
```
[Table("RadioStation")]
public class RadioStation
{
    [Key]
    [Required]
    public int Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}
```

- In the program just call the ensure create function which will trigger the creation
[Fact]
public void Test1()
{
    using(var db = new StudyContext())
    {
        db.Database.EnsureCreated();
        var radioStations = db.RadioStation.ToList();
    }
}


