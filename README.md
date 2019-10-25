![titulo](/docs/titulo.JPG)

# NetCore3-EFCore

Connecting a .Net Core 3 web API with a MySQL database by using Entity Framework Core.

## Technologies

- [.Net Core 3](https://docs.microsoft.com/pt-br/dotnet/core/whats-new/dotnet-core-3-0)
- [MySQL](https://www.mysql.com/)
- [EFCore](https://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx)

## Topics

- [Installation](#installation)
- [Repository](#repository)

### Installation

You will need a .Net Core 3 project connected with a MySQL database, so follow the instructions from this [repository](https://github.com/lucianopereira86/NetCore3-MySQL) or from this [article](https://dev.to/lucianopereira86/net-core-web-api-part-2-mysql-3bje).

![print01](/docs/print01.JPG)

These are your current packages:

![print03](/docs/print03.JPG)

You don't need _Dapper_ anymore, so download the _Entity Framework Core_ package for MySQL, called _Pomelo.EntityFrameworkCore.MySql_, by Nuget:

![print04](/docs/print04.JPG)

Create a folder in the root called _Infra_ with three other folders inside of it: _interface_, _models_ and _repository_.

![print05](/docs/print05.JPG)

It's time for some coding!

### Repository

Inside the _repository_ folder, create a class called _DBContext_ with the following code:

```csharp
using Microsoft.EntityFrameworkCore;

namespace NetCore3WebAPI.Infra.Repository
{
    public class DBContext: DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuider)
        {
            base.OnModelCreating(modelBuider);
        }
    }
}
```

This class will be responsible for mirroring the database tables into Class objects.  
If you have followed the instructions from the article [NetCore3 MySQL](https://dev.to/lucianopereira86/net-core-web-api-part-2-mysql-3bje), your database must have a table called _user_. Create a class within the _Infra/Models_ folder containing the same properties as the table:

```csharp
namespace NetCore3WebAPI.Infra.Models
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
```

Modify the _DBContext_ class by adding a _DBSet_ for the _User_ class:

```csharp
public DbSet<User> User { get; set; }
```

This object will be used to manipulate the data from the table.  
Inside the _OnModelCreating_, you need to mirror each model with its respective table. So do this for the _User_ class:

```csharp
 modelBuider.Entity<User>(e =>
{
    e
    .ToTable("user")
    .HasKey(k => k.id);

    e
    .Property(p => p.id)
    .ValueGeneratedOnAdd();
});
```

This will be the result:

```csharp
using Microsoft.EntityFrameworkCore;
using NetCore3WebAPI.Infra.Models;

namespace NetCore3WebAPI.Infra.Repository
{
    public class DBContext: DbContext
    {
        public DbSet<User> User { get; set; }
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuider)
        {
            modelBuider.Entity<User>(e =>
            {
                e
                .ToTable("user")
                .HasKey(k => k.id);

                e
                .Property(p => p.id)
                .ValueGeneratedOnAdd();
            });
            base.OnModelCreating(modelBuider);
        }
    }
}
```

To build the repository, it will be necessary a super class containing the generic methods used by the _DBSet_ object.
Inside the _Infra/Interface_ folder, create two interfaces:

```csharp
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NetCore3WebAPI.Infra.Interface
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> List(Expression<Func<T, bool>> expression);
        bool Any(Expression<Func<T, bool>> expression);
        T Save(T entity);
        T Update(T entity);
        void Delete(T entity);
    }
}
```

```csharp
using NetCore3WebAPI.Infra.Models;

namespace NetCore3WebAPI.Infra.Interface
{
    public interface IUserRepository : IBaseRepository<User>
    {
    }
}
```

Inside the _Infra/Repository_ folder, create two classes:

```csharp
using Microsoft.EntityFrameworkCore;
using NetCore3WebAPI.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NetCore3WebAPI.Infra.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private DbSet<T> entity_;
        protected DBContext context;

        public BaseRepository(DBContext context)
        {
            this.context = context;
            entity_ = context.Set<T>();
        }

        public virtual IEnumerable<T> List(Expression<Func<T, bool>> expression)
        {
            return entity_.Where(expression).ToList();
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return entity_.Any(expression);
        }

        public T Save(T entity)
        {
            entity_.Add(entity);
            context.SaveChanges();

            return entity;
        }

        public T Update(T entity)
        {
            entity_.Update(entity);
            context.SaveChanges();
            return entity;
        }

        public void Delete(T entity)
        {
            entity_.Remove(entity);
            context.SaveChanges();
        }
    }
}
```

```csharp
using NetCore3WebAPI.Infra.Interface;
using NetCore3WebAPI.Infra.Models;

namespace NetCore3WebAPI.Infra.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DBContext context) : base(context)
        {
        }
    }
}

```

Inside the _Startup's ConfigureServices_ method, create a service for the _IUserRepository_ and inject the MySQL connection string (from the _appsettings.json_) into _DBContext_:

```csharp
(...)
using Microsoft.EntityFrameworkCore;

namespace NetCore3WebAPI
{
    public class Startup
    {
        (...)
        public void ConfigureServices(IServiceCollection services)
        {
            (...)
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddDbContext<DBContext>(o => o.UseMySql(Configuration.GetConnectionString("MySQL")));
        }
        (...)
    }
}
```

Now, we are ready to test our connection!  
Inside the _UserController_ class, inject the _IUserRepository_ interface inside the constructor:

```csharp
private readonly ConnectionStrings con;
private readonly IUserRepository repo;
public UserController(ConnectionStrings c, IUserRepository r)
{
    con = c;
    repo = r;
}
```

Import the Linq library and modify the _Get_ method like this:

```csharp
(...)
using System.Linq;

namespace NetCore3WebAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        (...)
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] User vm)
        {
            return await Task.Run(() =>
            {
                var result = repo.List(w =>
                            (vm.id == 0 || w.id == vm.id)
                            &&
                            (vm.name == null || w.name.ToUpper().Equals(w.name.ToUpper())));
                return Ok(result);
                //using (var c = new MySqlConnection(con.MySQL))
                //{
                //    var sql = @"SELECT * FROM user
                //                WHERE (@id = 0 OR id = @id)
                //                AND (@name IS NULL OR UPPER(name) = UPPER(@name))";
                //    var query = c.Query<User>(sql, vm, commandTimeout: 30);
                //    return Ok(query);
                //}
            });
        }
        (...)
    }
}
```

You did the same thing as the _Dapper_ code was doing, but without using SQL commands.  
Run the web API and access the Swagger by the URL:

```
http://localhost:53000/swagger/index.html
```

![print06](/docs/print06.JPG)

Test the _GET_ method:

![print07](/docs/print07.JPG)

The result will be something like this:

![print08](/docs/print08.JPG)

## Conclusion

It was much more complex to connect with a database by using _Entity Framework_ than by using _Dapper_. However, due to the interface/repository layers, the database access has become safer, better structured and object oriented.  
Entity Framework is advised for systems in which the business rules are kept in the application layer. This commonly happens when there are databases with different technologies integrated with the web API.
