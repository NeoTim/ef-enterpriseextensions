[![Build status](https://ci.appveyor.com/api/projects/status/6e5vk7s69ur34nd0?svg=true)](https://ci.appveyor.com/project/geoperez/ef-enterpriseextensions)

# ef-enterpriseextensions

*:star: Please star this project if you find it useful!*

Unosquare Labs EntityFramework.EnterpriseExtensions Library contains a set of useful helpers and classes to common tasks related to process and data manipulation in enterprise applications.

## NuGet Installation:

[![NuGet version](https://badge.fury.io/nu/Unosquare.EntityFramework.EnterpriseExtensions.svg)](https://badge.fury.io/nu/Unosquare.EntityFramework.EnterpriseExtensions)
```
PM> Install-Package Unosquare.EntityFramework.EnterpriseExtensions
```

If you are using Identity Entity Framework (`IdentityDbContext`), use the following command:

[![NuGet version](https://badge.fury.io/nu/Unosquare.Identity.EntityFramework.EnterpriseExtensions.svg)](https://badge.fury.io/nu/Unosquare.Identity.EntityFramework.EnterpriseExtensions)
```
PM> Install-Package Unosquare.Identity.EntityFramework.EnterpriseExtensions
```

## Usage

First you need to change your `DbContext` or `IdentityDbContext` to `BusinessDbContext` or `IdentityBusinessDbContext` respectively and you can 
attach Business Controllers to your DbContext in the constructor. They will execute before any time you call `SaveChanges` method. 
The controllers require specified a `BusinessRuleAttribute` to
identify what CRUD action and which Entity types will be processed.

EF Enterprise Extensions includes a `JobBase` abstract class (with a singleton extension named `SingletonJobBase`), so you can build business jobs easily. 
Check the Sample app.

### Business Rules - Audit Trail

Audit Trails is a task to save the changes to any operation performed in a record. In other words, capture what change between any data saving. 
This operation is important in many systems and you can accomplish with these extensions easily. The `AuditTrailController` can be attached to your `BusinessDbContext`and setup which Entities will be recorded in the three CRUD actions supported, create, update and delete.

To start using the `AuditTrailController` you need to specify a table where the Audit Trail data will be, you should implement the `IAuditTrailEntry` 
interface in your entity, for example:

```csharp
public class AuditTrailEntry : IAuditTrailEntry
    {
        [Key]
        public int AuditId { get; set; }

        public string UserId { get; set; }
        public string TableName { get; set; }
        public int Action { get; set; }
        public string JsonBody { get; set; }
        public DateTime DateCreated { get; set; }
    }
```

Since you need to know who change the data, you need to construct your DataContext with the user relation. For example, in Web API you can 
send the UserId from the request in the DataContext constructor. The `AuditTrailController` requires two parameters, the DbContext instance, 
and a string to identify the user, and two type parameters, the DbContext type and the AudiTrailEntry type.  
You can call the extension method `UseAuditTrail` to add the Business Controller to your Business DataContext.

```csharp
public class SampleDb : BusinessDbContext
    {
        public string UserId { get; set; }

        public SampleDb(DbConnection connection, string userid) : base(connection, true)
        {
            UserId = userid;
            this.UseAuditTrail<SampleDb, AuditTrailEntry>(userid);
        }

        public DbSet<AuditTrailEntry> AuditTrailEntrys { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
    }

```

By default, all the entities will be audited, you can access the Audit Trail controller and set up the action-types relation with the method `AddTypes`. All the data is stored as a JSON string. You can use this Business Controller or you can create your own and probably change to serialize the data change in XML or every property in one database record.

### Jobs

The *Jobs* is a wrapper for your tasks. They can run in a single instance (singletons with `SingletonJobBase`) or with multiple executions using `JobBase`. You can execute them in your ASP.NET applications (WebApi too) using the `HostingEnvironment.QueueBackgroundWorkItem` method in .NET 4.6 and set up your execution condition with a DateTime or a flag.

To begin using this Jobs, you need to create a class and inherit from SingletonJobBase or JobBase and fill the implementation methods. The jobs expose three executions modes:

* **Run** - The simple non-async way to execute the task. You can provide your own ThreadPool or mechanism to execute them. Returns true if the task was executed.
* **RunAsync** - An awaitable method to run your task.
* **RunBackgroundWork** - The ideal method to keep a scheduled task, with an optional idle timespan. You can use this method with `HostingEnvironment.QueueBackgroundWorkItem` and your BackgroundCondition method to generate a cron-like system in your OWIN application.
