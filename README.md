# <a name="home"></a>DutchTreat
## Building a Web App with Dotnet Core, MVC, Entity framework core, Bootstrap and Angular
* [CSS](#headCss)
* [Package.json](#headPackageJson)
* [Enabling MVC 6](#headEnablingMvc6)
* [Creating a Layout](#headCreatingLayout)
* [Adding more Views](#headAddingMoreViews)
* [Using Tag Helpers](#headUsingTagHelpers)
* [Razor Pages](#headRazorPages)
* [Implementing A View](#headImplementingView)
* [Model Binding](#headModelBinding)

### <a name="headCss"></a>CSS
* space in css means class of child.
* by default divs are displayed in a type called "block": and "block" takes as many horizontal space as possible.

### <a name="headPackageJson"></a>package.json
* Loads all of the different js frameworks that we'll need.
* Add "dependencies":{"jquery":"3.4.1"}
* Save -> packages are installed.
* Click Show all files -> (node_modules)
* go to Startup.cs -> Configure() -> app.UseNodeModules();
* Red squiggly -> Right click on Solution -> Manage NuGet Packages -> search UseNodeModules -> OdeToCode
* This middleware allows "node_modules" folder to be used as if it's inside "wwwroot" folder.
```html
    <script src="/node_modules/jquery/dist/jquery.min.js"></script>  								
```
* [Back to Index](#home)

### <a name="headEnablingMvc6"></a>Enabling MVC 6
* Add a folder called Controllers -> Add "Name"Controller.cs inside it.
* Add a method with return type IActionResult 
* Add a folder called Views -> Add sub-folder with "Name" -> Add the method name as .cshtml
* Running the application now won't make the MVC to work.
* We need to configure certain middlewares in Startup.
* app.UseMvcWithDefaultRoute() 
* or app.UseMvc(routes =>{ routes.MapRoute("default", "{controller=App}/{action=Index}/{id?}");});
* In ConfigureServices add services.AddMvc();

### <a name="headCreatingLayout"></a>Creating a Layout
* Add a sub-folder inside Views folder -> name it Shared
* Add _Layout.cshtml inside the sub-folder.
* Add head, body -> inside body add header, section, footer -> inside section add @RenderBody()
* Running the project now won't show the layout.
* We need to either set it inside each page below ViewBag.Title (not recommended) or
* Add MVC View Start Page inside the Views folder.
* [Back to Index](#home)

### <a name="headAddingMoreViews"></a>Adding more views
* Add more views in a similar way.
* Add the following in the _Layout : 

```html
    <header>
        <h1>Welcome to Dutch Treat</h1>
            <menu>
                <ul>
                    <li><a href="/">Home</a></li>
                    <li><a href="/app/contact">Contact</a></li>
                    <li><a href="/app/about">About</a></li>
                </ul>
            </menu>
    </header>
```

* menu li (vs) menu>ul>li : the first one means that li being some child of menu 
* and the latter means that li strictly being inside ul and that in turn inside menu.
* add the following css :

```css
	menu li {
		display: inline;
	}
		menu li::after{
			content: ' |';
		}
		menu li:last-of-type::after{
			content: "";
		}
```

### <a name="headUsingTagHelpers"></a>Using Tag Helpers
* The links created above are fragile.
* We can make it better using Tag Helpers.
* To use Tag Helpers, we need to add "MVC View Imports Page" : _ViewImports.cshtml inside Views folder.
* We can add any namespace that is required for all the view pages inside this.
* Add the following:
```csharp
	@using DutchTreat.Controllers
	@addTagHelper "*, Microsoft.AspNetCore.Mvc.TagHelpers"
```
* Instead of href, we give "asp-" tag helpers, eg:
```html
    <menu>
        <ul>
            <li><a asp-controller="App" asp-action="Home" >Home</a></li>
            <li><a asp-controller="App" asp-action="Contact">Contact</a></li>
            <li><a asp-controller="App" asp-action="About">About</a></li>
        </ul>
    </menu>
```
* Sometimes we give default routing without giving the controller name, eg: www.Website.com/Contact 
* For that we can add Attribute like this:
```csharp
	[HttpGet("contact")]
    public IActionResult Contact()
    {
        ViewBag.Title = "Contact Us";
        return View();
    }
```

### <a name="headRazorPages"></a>Razor Pages
* Sometimes we need to create a view just to show some message; eg error page.
* Up until now we had to create it through controller and action.
* But with Razor Pages we can directly use a view without creating a controller.
* Right click on project solution 
	-> add a folder with name "Pages" (Default convention) 
  	-> Add a .cshtml file
    -> The page must start with @page declatation.
	
### <a name="headImplementingView"></a>Implementing a View
* Add a form with method="post" as attribute in form tag.
* Add a post method for contact in Controller: eg:
```csharp
	[HttpPost("contact")]
    public IActionResult Contact(object model)
    {
        ViewBag.Title = "Contact Us";
        return View();
    }
```
* This in itself won't send any data to model.
* So to send the data we need to add "name" attribute to all the input tags.

### <a name="headModelBinding"></a>Model Binding
* For easier access of the passed in data we can use model binding.
* Create POCO class.
* Replace object with the "ContactViewModel".
* Now the data posted from the view will automatically get mapped to the model object.
* Case is ignored.
* This works same as API.
* Further we can replace "name" attribute with certain "asp-" tag helpers.
* But to use that we need to declare ViewModel namespace.
* eg: @model ContactViewModel
* Complete path of ContactViewModel can be set in _ViewImports.
* Once the declatation is added, we can now use "asp-" tag helpers eg: asp-for="Name".
* We can add the tag helpers to label too. 
* This will make clicking on label send the curzor to the input field directly plus it's mobile touch friendly too.

Using Validation
* Add the attribute validation, eg:
	public class ContactViewModel
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        [MaxLength(250,ErrorMessage ="Too long")]
        public string Message { get; set; }
    }
* In the post method, we need to further check if the model passed is valid, by using: ModelState.IsValid
* <div asp-validation-summary="All"></div> shows all the errors at one place after the validation fails at server.
* If we want to show the error message before pressing the send button:
* <div asp-validation-summary="ModelOnly"></div>
* And add the following: 
	<input asp-for="Name" />
    <span asp-validation-for="Name"></span>
* We also need to add 2 more dependencies: in package.json
	"jquery-validation": "^1.17.0",
    "jquery-validation-unobtrusive": "^3.2.10"
* After adding the scripts via package.json, we also need to include it in the pages that we are using it.
* To do this we'll be using	@RenderSection("Scripts", false); inside _Layout.cshtml page.
* "false" above indicates that it is not mandatory to be used in all the views.
* Add the following wherever required:
	@section Scripts{
		<script src="~/node_modules/jquery-validation/dist/jquery.validate.min.js"></script>
		<script src="~/node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js"></script>
	}

Adding a service
* _mailService.SendMessage("test@test.com", model.Subject, $"From: {model.Name} - {model.Email}, Message: {model.Message}");
* Add a folder called "Services" to the project.
* Add a file inside it : NullMailService.cs
* Create a method: SendMessage; add a logger.
	private readonly ILogger<NullMailService> _logger;
	
    public NullMailService(ILogger<NullMailService> logger)
    {
        _logger = logger;
    }
    public void SendMessage(string to, string subject, string body)
    {
        //Log the message.
        _logger.LogInformation($"To: {to} Subject: {subject} Body: {body}");
    }
* Extract an interface from the class. Reason being, we can have multiple implementations.
* In order to use this service we need to configure it in Startup.cs
* We can configure it mainly in 3 ways: services.AddTransient(), services.AddScoped(), services.AddSingleton()		
* services.AddTransient<INullMailService, NullMailService>();
* Again, in the AppController, use the constructor injection to inject the service created:
	private readonly INullMailService _mailService;
    public AppController (INullMailService nullMailService)
    {
        _mailService = nullMailService;
    }
* [HttpPost("contact")]
   public IActionResult Contact(ContactViewModel model)
   {
       ViewBag.Title = "Contact Us";
       if (ModelState.IsValid)
       {
           // Send the email
           _mailService.SendMessage("test@test.com", model.Subject, $"From: {model.Name} - {model.Email}, Message: {model.Message}");
           ViewBag.UserMessage = "Mail Sent";
           ModelState.Clear();
       }         
       return View();
   }

Adding Bootstrap
* Add the package in package.json
* In the layout page, add the bootstrap css link inside head tag.
* Add the js script link: bootstrap.bundle.min.js link below jquery.min.js
* To leverage bootstrap, comment out all the css in site.css
* Add class="container" to all the main sections: header, section and footer: this brings the sections to center of the page from left.
* Add class="text-center" to footer; class="btn btn-success" to button.

Building a NavBar
* We'll replace menu tag in Layout page with a nav tag.
* This isn't required but we are just telling HTML5 about where the navigation exists.
* <nav class="navbar"> and move this inside nav:<h1 class="navbar-brand">Welcome to Dutch Treat</h1>
* Add this:
  <nav class="navbar navbar-dark bg-dark navbar-expand-md ">
	<h1 class="navbar-brand">Welcome to Dutch Treat</h1>
    <button class="navbar-toggler" data-toggle="collapse" data-target="#theMenu">
        <span class="navbar-toggler-icon"></span>
    </button>
    <div id="theMenu" class="navbar-collapse collapse">
        <ul class="navbar-nav">
            <li class="nav-item">
                <a class="nav-link" asp-controller="App" asp-action="Index">Home</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" asp-controller="App" asp-action="Contact">Contact</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" asp-controller="App" asp-action="About">About</a>
            </li>
        </ul>
    </div>
  </nav>	
* Add a button to show a hamburger symbol whenever screen size becomes smaller than the given breakpoint i.e. navbar-expand-md
* Add data-toggle="collapse" and data-target="#theMenu": common pattern in bootstrap to add data- attributes.

Bootstrap Grid System
* 12-column system.
* .col-6 | .col-6 ; .col-4 | .col-4 | .col-4; ......
* |0|0|1|1|1|1|1|1|1|1|0|0| --> .col-8 .offset-2
* 5 sizes: 
	.col-xl-xx: >=1200px
	.col-lg-xx: >=992px
	.col-md-xx: >=768px
	.col-sm-xx: <768px
	.col-xx: <576px

Bootstrap Forms
* We can now put each group of logical elements in a separate div with class="form-group"
	<div class="form-group">
        <label asp-for="Name">Your Name:</label>
        <input asp-for="Name" />
        <span asp-validation-for="Name"></span>
    </div>
* Now we can also eliminate the hard coded line breaks <br />
* <input type="submit" value="Send Message" class="btn btn-primary" />
* for span with asp-validation-for add the class="text-danger" and class="text-success" for success message.

Using font-awesome
* These are used to add icons.
* The way to use it is to add the i tag and add respective class.
* In the package.json file add "font-awesome" 
* From the node_modules folder pick the css sub-folder and add the min  version of font-awesome.
* Add the link before site.css; just in case we want to override it.
* Adding the font-awesome solves the change in font-size issues as well.

Entity Framework Core
* Creating Entities.
* Using EF Core Tooling.
* Using Configuration.
* Using DbContext.
* Seeding the Database.
* The Repository Pattern.
* Showing the Products.
* Logging Errors.

Creating Entities
* Create a folder called Data - That will contain the classes for our different interfaces to our DB.
* Create a sub-folder called Entities - That will contain the classes for the shapes of data that we are gonna store in our DB.
* Add the classes like: Product, Order and OrderItem.cs
* Add Entity Framework Core to the project - Open NuGet Package Manager and add "entityframeworkcore sqlserver" & "entityframeworkcore design"
* Add a class file called DutchContext - Derive it from DbContext - using Microsoft.EntityFrameworkCore;
* It's a class that will know how to execute quieries to a Data store.
* In order to create these queryable end points, we need to create some properties that will interact with entities.
* These properties are of type DbSet<Entity>

Using EF Core Tooling
* install the efcore before we can use it in the root of the project: 
	dotnet tool install dotnet-ef --version 2.2.6 -g
* To create and update Database from out project use the below command
	dotnet ef database update
	Error: System.InvalidOperationException: No database provider has been configured for this DbContext.
	A provider can be configured by overriding the DbContext.OnConfiguring method or by using AddDbContext 
	on the application service provider. If AddDbContext is used, then also ensure that your DbContext type 
	accepts a DbContextOptions<TContext> object in its constructor and passes it to the base construct
	or for DbContext.
* Go to Startup class and in ConfigureServices add:
	services.AddDbContext<DutchContext>(); and run the cmd again
	Error: Same as above.
* The reason is we need to tell the exact database and it's configure it.

Using Configuration
* Add a ctor in the Startup.
* Using ctor to inject IConfiguration with a readonly field _config.
* _config => similar to appsettings key value pair.
* from here we can pass the value of our connectionStrings.
	1) cfg.UseSqlServer(_config[""]); or using it's own method.
	2) cfg.UseSqlServer(_config.GetConnectionString("DutchConnectionString"));
* Right now it won't work as CreateDefaultBuilder(Program.cs) doesn't know where to find the config file.
* We can add and use the default config file : AppConfig.json 
* To add the custom configuration file add this:
	.ConfigureAppConfiguration(SetupConfiguration) after .CreateDefaultBuilder(args)
	SetupConfiguration - A method
	private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
    {
        //Removing the default configuration options.
        builder.Sources.Clear();
        builder.AddJsonFile("config.json",false,true)
               .AddXmlFile("config.xml",true)
               .AddEnvironmentVariables();
        //This will automatically reload the changes done in config files unlike previously.
        //The heirarchy at which it will implement the config is bottom to top i.e. AddEnvironmentVariables will override the config changes above it.
        //AddEnvironmentVariables - usually done by IT team in production deployments
    }
* Let's create a file called config.json: 
	{
		"Colors": {
			"Favorite": "blue"
		},
		"ConnectionStrings": {
			"DutchConnectionString": "server=(localdb)\\MSSQLLocalDB;Database=DutchTreatDb;Integrated Security=True;MultipleActiveResultSets=true;"
		}
	}
* Let's run this command again: 
	dotnet ef database update
	Error: 
		An error occurred while accessing the IWebHost on class 'Program'. Continuing wi
		thout the application service provider. Error: AddDbContext was called with conf
		iguration, but the context type 'DutchContext' only declares a parameterless con
		structor. This means that the configuration passed to AddDbContext will never be
		used. If configuration is passed to AddDbContext, then 'DutchContext' should de
		clare a constructor that accepts a DbContextOptions<DutchContext> and must pass
		it to the base constructor for DbContext.
	
	// copy DbContextOptions<DutchContext> and add in the ctor of DutchContext.cs and name the var as options
	    public DutchContext(DbContextOptions<DutchContext> options):base(options)
        {

        }
* run this command again: 
	dotnet ef database update		
* After success of above command, run this:
	dotnet ef migrations add InitialDb
* This adds Migrations folder to the project, move the folder to Data folder.
* run this command again: 
	dotnet ef database update
	
Using DbContext
* Add a Method called Shop inside AppController
* In the ctor add DutchContext context readonly field.
* Using context save the results in a var and pass it to the view directly:
	public IActionResult Shop()
    {
        var results = _context.Products
            .OrderBy(p => p.Category)
            .ToList();
        return View(results);
    }

Seeding the Database
* In the DutchContext file override OnModelCreating method.
* This method can be used to specify how the mapping is gonna happen in between Entities and actual DB.
	eg: modelBuilder.Entity<Product>()
                .Property(p => p.Title)
                .HasMaxLength(50);
* Particularly for seeding: 
		modelBuilder.Entity<Order>()
            .HasData(new Order() 
            {
                Id = 1,
                OrderDate = DateTime.UtcNow,
                OrderNumber = "1234"
            });	
* Run the following command:
	dotnet ef migrations add SeedData
	-> This should crete a ..._SeedData.cs inside Migrations folder.
* This works well with simple data but not with complex ones.
* All the code above will be regenerated everytime DutchContext is new up.
* Add DutchSeeder.cs inside Data folder.
* In order to seed data we'are gonna 've to read data from DutchContext.
* So create a ctor and pass DutchContext parameter and create a readonly field for it.
* Create a simple method called Seed()
* Let's import all the data from a json file and then push it to DB.
* To read all the data as json, simply read all the text from it as a string.
* var json = File.ReadAllText(filepath);
* To get the filepath, create a _hosting readonly field of type IHostingEnvironment
	var filepath = Path.Combine(_hosting.ContentRootPath,"Data/art.json");
* _hosting.ContentRootPath can be used for all environment without hardcoding the root path.
* var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
* This is what the seed function looks:
	public void Seed()
	{
		_ctx.Database.EnsureCreated();
		if (!_ctx.Products.Any())
		{
			//Need to create sample data
			var filepath = Path.Combine(_hosting.ContentRootPath,"Data/art.json");
			var json = File.ReadAllText(filepath);
			var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
			//Add the products to DB
			_ctx.Products.AddRange(products);
			var order = _ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();
			if (order != null)
			{
				order.Items = new List<OrderItem>()
				{
					new OrderItem(){
						Product = products.First(),
						Quantity = 5,
						UnitPrice = products.First().Price
					}
				};
			}
			_ctx.SaveChanges();
		}
	}
* Now that we've created the seed function, let's execute it.
* To get the instance of DutchSeeder class and execute the Seed method, We'll now go to Program.cs file and use Main method .
* Many people do it in Startup.cs; But we've leared that seeding should happen way before setting up the web server.
* So the trick here is to get the web host by still calling BuildWebHost, but we're gonna run the host later.
* var host = CreateWebHostBuilder(args).Build();
* Instantiate and buildup Seeder
	RunSeeding(host);
* And then run the host later:
	host.Run();
* var seeder = host.Services.GetService<DutchSeeder>();//Microsoft.Extensions.DependencyInjection
* Before we use Seed(), we need to fulfill all the dependencies => Go to Startup.cs and ad the services for same
* services.AddTransient<DutchSeeder>();
* The above code might work; but inorder to bullet proof it, we need to understand that DutchSeeder actually contains scoped dependencies.
* How do we know this? => In the Startup class, services.AddDbContext<DutchSeeder> creates it as Scoped dependency
* Even though we know that it is a scoped dependency we'll still verify if it's a scoped dependency or not.
* var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
* This is the way outside of the web server to create a scope. 
* Interesting thing about it is that during every request the scopeFactory actually creates the scope for entire lifetime of the request.
* I.e. we get the instance of the context object that is true throught the entire request.
* using (var scope = scopeFactory.CreateScope())
  {
      //var seeder = host.Services.GetService<DutchSeeder>();//Microsoft.Extensions.DependencyInjection
      var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
      seeder.Seed();
  }

The Repository Pattern
* Create a new class inside data folder: DutchRepository.cs
* In this class inject the DutchContext.
* Add few Methods using the context to Get the data. eg: GetAllProducts
* Extract interface: so that it may be useful during testing as we can create mock version of repository.
* Add the method:
	public bool SaveAll()
    {
        return _ctx.SaveChanges()>0;
    }
* In order to use the repository let's register it as a service in Startup.cs
* services.AddScoped<IDutchRepository, DutchRepository>();
* Back in the AppController, replace the DutchContext with IDutchRepository and call the respective methods.

Showing The Products
<div class="row">
    @foreach (var p in Model)
    {

        <div class="col-md-3">
            <div class="border bg-light rounded p-1">
                <img src="/img/@(p.ArtId).jpg" class="img-fluid" alt="@p.Title"/>
                <h3>@p.Category</h3>
                <ul>
                    <li>Price: $@p.Price</li>
                    <li>Artist: @p.Artist</li>
                    <li>Title: @p.Title</li>
                    <li>Description: @p.ArtDescription</li>
                </ul>
                <button id="buyButton" class="btn btn-success">Buy</button>
            </div>
        </div>
    }
</div>

Logging the Errors
* Open the cmd from root of the project.
* set ASPNETCORE_ENVIRONMENT=Development -> dotnet run
* We can see the info or warn tags in the console window.
* This hasppens because logging is setup by default to write out to the console.
* Each and every movement in the site is logged; While this might be useful but may be not helpful at this point.
* We can configure the level of logging directly in the config file:
	"logging": {
		"loglevel": {
			"Default": "Warning"
		}
	}
* This setting will show us only warning and above log levels; info won't be shown.
* If we wan't to use logging in our own code: 
	In the DutchRepository -> ctor -> inject ILogger<DutchRepository> -> add readonly field.
* So when we call GetAllProducts Method, let's call the logger:
	_logger.LogInformation("GetAllProducts method called");
* We won't be able to see the above log now, but with the below settings we can see:
	"logging": {
		"loglevel": {
			"Default": "Information",
			"Microsoft": "Warning"
		}
	}
* This will not show info from every logger that starts with the word Microsoft but will show "DutchTreat" info.
* We can basically use the logger for error logging in the following way:
	public IEnumerable<Product> GetAllProducts()
    {
        try
        {
            _logger.LogInformation("GetAllProducts method called");
            return _ctx.Products.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to get all products: {ex}");
            throw;
        }
       
    }	

Create an API Controller
* Before starting this Install Postman.
* In VS -> open project settings -> Rt click project -> open properties -> Debug -> Uncheck Launch Browser
* Add a ProductsController class that inherits from Controller.
* In dotnet core all controller are exposed for API end points unlike the MVC API where in WebAPI which was a separate library.
* Inside the controller -> ctor -> create readonly fields : IDutchRepository, ILogger
* Add Route attribute to the controller :
	[Route("api/[Controller]")]
* Add Get method of return type IEnumerable<Product> and return GetAllProducts.
	public IEnumerable<Product> Get()
    {
        return _repository.GetAllProducts();
    }
* The above will work fine when an API get call is made, but in reality we may add try catch.
* In this case we'll have to either throw ex or return null.
* This doesn't work in a robust way.
* We can return JsonResult: 
	public JsonResult Get()
    {
        try
        {
            return Json(_repository.GetAllProducts());
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to get products: {ex}");
            return Json("Bad Request");
        }
    }
* This again makes it to return Json only.
* To have more flexibility: 
	public IActionResult Get()
    {
        try
        {
            return Ok(_repository.GetAllProducts());
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to get products: {ex}");
            return BadRequest("Failed to get products");
        }
    }
* Now we are able to tie the return type with actual status code as well.
* Plus IActionResult gives the flexibility for any type of content negotiation.
* Again, when we are building public API's we have requirement of documenting it.	
* There are some solutions like Swagger that looks into API and builds the documentation.
* Though the above code will return proper status codes, IActionResult doesn't know the data type returned that will be useful for documentation.
* So for the above we can make return type to ActionResult<IEnumerable<Product>>.
* We can also add attributes like [ApiController], [Produces("application/json")] etc at controller level.
* [ProducesResponseType(200)], [ProducesResponseType(400)] at method level that will help those tools.

Returning Data
* Add OrdersController and all the boiler plate code like below:
	[Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger _logger;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_repository.GetAllOrders());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders: {ex}");
                return BadRequest("Failed to get orders");
            }
        }
    }
* From the above code we get the following results:
    {
        "id": 1,
        "orderDate": "2019-11-26T08:19:09.3101452",
        "orderNumber": "1234",
        "items": null
    }
* Now, if we notice, items are null even though corresponding values is present.
* This happens because of the following: 
	return _ctx.Orders.ToList(); // Code from repository
* Since we are returning only Orders, EF Core returns only that level of heirarchy by it's design.
* To make the next level of heirarchy to work change it to following:
	return _ctx.Orders.Include(o => o.Items).ToList();
* Again, this may either fail to return anything or may throw response error because of self-referencing loop.
* To resolve such error we have some options provided in the dotnet core.
* Startup -> services.AddMvc() -> convert this to following:
	services.AddMvc().AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
* Now the Get request will return the following:
	{
        "id": 1,
        "orderDate": "2019-11-27T14:08:35.2640114",
        "orderNumber": "1234",
        "items": [
            {
                "id": 1,
                "product": null,
                "quantity": 5,
                "unitPrice": 89.99
            }
        ]
    }	
* We still get "product": null
* In the DutchRepository make the following change:
	public IEnumerable<Order> GetAllOrders()
    {
        return _ctx.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ToList();
    }
* Now we get the following results:
	[
    {
        "id": 1,
        "orderDate": "2019-11-27T14:08:35.2640114",
        "orderNumber": "1234",
        "items": [
            {
                "id": 1,
                "product": {
                    "id": 1,
                    "category": "Poster",
                    "size": "48\" x 36\"",
                    "price": 89.99,
                    "title": "Self-portrait",
                    "artDescription": "Vincent moved to Paris in 1886, after hearing from his brother Theo about the new, colourful style of French painting. Wasting no time, he tried it out in several self-portraits. He did this mostly to avoid having to pay for a model. Using rhythmic brushstrokes in striking colours, he portrayed himself here as a fashionably dressed Parisian.",
                    "artDating": "1887",
                    "artId": "SK-A-3262",
                    "artist": "Vincent van Gogh",
                    "artistBirthDate": "1853-03-30T00:00:00",
                    "artistDeathDate": "1890-07-29T00:00:00",
                    "artistNationality": "Nederlands"
                },
                "quantity": 5,
                "unitPrice": 89.99
            }
        ]
    }
]
* Let's implement individual order:
	public Order GetOrderById(int id)//code from DutchRepository
    {
        return _ctx.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Where(o => o.Id == id)
            .FirstOrDefault();
    }
	[HttpGet("{id:int}")] //code from OrdersController
    public IActionResult Get(int id)
    {
        try
        {
            var order = _repository.GetOrderById(id);
            if (order != null)
            {
                return Ok(order);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to get the order: {ex}");
            return BadRequest("Failed to get the order");
        }
    }

Implementing Post
* Add the following boiler plate:
  [HttpPost]
  public IActionResult Post(Order model)
  {
      //add it to the db()
      return Ok();
  }
* Now from the postman send post request using the same URL used for GET request(http://localhost:60365/api/orders)
* All the object data is either null or 0 respectively.
* To pass the data along with the request, we can use query string as a key value pair that gets mapped automatically.
* But using QS will support only flat level of heirarchy; Can't pass complex object as a single  logical unit of data.
* For that, we'll need to pass it in body.
* There are few different ways to send the data using body:
	-> form-data
	-> urlencoded
	-> raw (json, text, xml etc)
* Mostly we'll be using raw-json.
* Now if we send data from postman - Body :
	{
		"orderDate" : "2015-06-06"
	}
	-> The data won't get auto-mapped and show up.
	-> To make it work we'll have to add [FromBody] attribute in the method parameter.
	-> By default it expects data from QS.
* Add the following boiler plate:
	public IActionResult Post([FromBody]Order model)
	{
		try
		{
			_repository.AddEntity(model);
			_repository.SaveAll();
		}
		catch (Exception ex)
		{
			_logger.LogError($"Failed to save a new order: {ex}");
		}
	}	
* AddEntity is a generic method that should be implemented in the repository:
	public void AddEntity(object model)
    {
        _ctx.Add(model);
    }
	-> here the parameter type is object so that it can be used for any type of model.
* Back in the controller we could say return Ok(model); but it voilates the way http works.
* We actually need to return Created(model) - returns 201 status code.
* Created() not only returns where on API the data is created but also pass back the data.
* return Created($"/api/orders/{model.Id}", model); -> URI should match our get method.
* This is an important idea here: The model will not have any id when passed to us.
* But, when we call SaveAll() it fixes and assigns the id by DbContext.
	[HttpPost]
    public IActionResult Post([FromBody]Order model)
    {
        //add it to the db()
        try
        {
            _repository.AddEntity(model);
            if (_repository.SaveAll())
            {
                return Created($"/api/orders/{model.Id}", model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to save a new order: {ex}");
        }
        return BadRequest("Failed to save a new order");
    }
* Now though the data being sent is saved and works fine, we do have a problem:
	The data being sent is not validated.

Validation and View Models
* Earlier in the course, we saw how the ContactViewModel was created and validated using attributes.
* Exactly the same way API's too can be validated using ViewModels
* The same subsystem works for both Views validation and API validation.
* Create the OrderViewModel
*  public class OrderViewModel
   {
       public int OrderId { get; set; }
       public DateTime OrderDate { get; set; }
       [Required]
       [MinLength(4)]
       public string OrderNumber { get; set; }
   }
* Now the updated post method along with model <-> viewModel mapping looks like this:
	[HttpPost]
	public IActionResult Post([FromBody]OrderViewModel model)
	{
		//add it to the db()
		try
		{
			if (ModelState.IsValid)
			{
				var newOrder = new Order()
				{
					OrderDate = model.OrderDate,
					OrderNumber = model.OrderNumber,
					Id = model.OrderId
				};
				if (newOrder.OrderDate == DateTime.MinValue)
				{
					newOrder.OrderDate = DateTime.Now;
				}
				_repository.AddEntity(newOrder);
				if (_repository.SaveAll())
				{
					var vm = new OrderViewModel()
					{
						OrderId = newOrder.Id,
						OrderDate = newOrder.OrderDate,
						OrderNumber = newOrder.OrderNumber
					};
					return Created($"/api/orders/{vm.OrderId}", vm);
				}
			}
			else
			{
				return BadRequest(ModelState);
			}
			
		}
		catch (Exception ex)
		{
			_logger.LogError($"Failed to save a new order: {ex}");
		}
		return BadRequest("Failed to save a new order");
	}
* If we see the code above it becomes very tedious while mapping back and forth.

Using Automapper
* Go to Manage NuGet Package Manager -> Browse for Automapper.
* Install this: AutoMapper.Extensions.Microsoft.DependencyInjection
* It will automatically include AutoMapper as well.
* Once it's installed -> Open Startup.cs -> To wire AutoMapper up with the project.
* Inside ConfigureServices -> services.AddAutoMapper(Assembly.GetExecutingAssembly());
* Go to OrdersController -> inject IMapper and it's readonly field.
* In Get method with id -> return Ok(_mapper.Map<Order,OrderViewModel>(order));
* The above mapping will convert data from Order type to OrderViewModel, this alone won't work.
* It will throw missing type map configuration or unsupported mapping.
* To create mapping config, add DutchMappingProfile.cs inside Data folder. 
* Derive the class from Profile: using AutoMapper.
* Inside the ctor create the map:
	public DutchMappingProfile()
    {
        CreateMap<Order, OrderViewModel>();
    }
* Now getting the order by id will return the following:
	{
		"orderId": 0,
		"orderDate": "2019-11-27T14:08:35.2640114",
		"orderNumber": "1234"
	}
* Here orderId is not returning the correct value.
* This is because of change in property name in both the classes that are being mapped.
* To fix it add the following:
	CreateMap<Order, OrderViewModel>()
                .ForMember(o => o.OrderId, ex =>ex.MapFrom(o => o.Id));
* This will work for collection as well; In the Get call where we are returning bulk data use the following:
	return Ok(_mapper.Map<IEnumerable<Order>,IEnumerable<OrderViewModel>>(_repository.GetAllOrders()));
* Back in Post method we change it to following:
	[HttpPost]
    public IActionResult Post([FromBody]OrderViewModel model)
    {
        //add it to the db()
        try
        {
            if (ModelState.IsValid)
            {
                var newOrder = _mapper.Map<OrderViewModel, Order>(model);
                if (newOrder.OrderDate == DateTime.MinValue)
                {
                    newOrder.OrderDate = DateTime.Now;
                }
                _repository.AddEntity(newOrder);
                if (_repository.SaveAll())
                {
                    return Created($"/api/orders/{newOrder.Id}", _mapper.Map<Order,OrderViewModel>(newOrder));
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
            
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to save a new order: {ex}");
        }
        return BadRequest("Failed to save a new order");
    }
* For Reverse mapping to work:
	CreateMap<Order, OrderViewModel>()
                .ForMember(o => o.OrderId, ex =>ex.MapFrom(o => o.Id))
                .ReverseMap();

Creating Association Controllers
* One of the things that we saw on Postman is that it's cutting out the items from return data.
* That's because OrderViewModel doesn't contain property for the collection.
* Let's add it: public ICollection<OrderItemViewModel> Items { get; set; }
* Where OrderItemViewModel :
	public class OrderItemViewModel
    {
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
    }
* We also need to CreateMapin our DutchMappingProfile.
* A get request to orders should now return the items along with orders.
* Now, suppose we just want to get the Items without needing the other data from Orders;
* Instead of getting all the orders we can actually create something called as Association Controllers.
* It will return items from this URL:"http://localhost:60365/api/orders/1/items": For this to work, follow the steps:
* Add OrderItemController with the following boiler plate code:
   [Route("/api/orders/{orderid}/items")]
	public class OrderItemsController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public OrderItemsController(IDutchRepository repository, ILogger<OrderItemsController> logger,IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
    }	
*     [Route("/api/orders/{orderid}/items")]
    public class OrderItemsController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public OrderItemsController(IDutchRepository repository, ILogger<OrderItemsController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Get(int orderid)
        {
            var order = _repository.GetOrderById(orderid);
            if (order != null)
            {
                return Ok(_mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
            }
            return NotFound();
        }
        [HttpGet("{id}")]
        public IActionResult Get(int orderId,int id)
        {
            var order = _repository.GetOrderById(orderId);
            if (order != null)
            {
                var item = order.Items.Where(i => i.Id == id).FirstOrDefault();
                if (item != null)
                {
                    return Ok(_mapper.Map<OrderItem, OrderItemViewModel>(item));
                }
            }
            return NotFound();
        }
    }
* Suppose we want the Product details along with items, One way would be to create it's own Association Controller.
* Other way using automapper is just by prefixing the property name with "Product":
	public class OrderItemViewModel
    {
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int ProductId { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSize { get; set; }
        public string ProductTitle { get; set; }
        public string ProductArtId { get; set; }
        public string ProductArtist { get; set; }

    }
* This approach is basically flattening the properties and is a good approach when we want to have readonly operations. 

Using QueryStrings for API's
* Suppose we don't want items information when we call a get request.
* We can include QS something like includeItems:
	public IActionResult Get(bool includeItems = true)
    {
        try
        {
            return Ok(_mapper.Map<IEnumerable<Order>,IEnumerable<OrderViewModel>>(_repository.GetAllOrders()));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to get orders: {ex}");
            return BadRequest("Failed to get orders");
        }
    }
	repository:
	[HttpGet]
    public IActionResult Get(bool includeItems = true)
    {
        try
        {
            var results = _repository.GetAllOrders(includeItems);
            return Ok(_mapper.Map<IEnumerable<Order>,IEnumerable<OrderViewModel>>(results));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to get orders: {ex}");
            return BadRequest("Failed to get orders");
        }
    }

ASP.NET Core Identity
* Replacement for ASP.NET Memnership
* Supports more types of authentication. - Cookies, OAuth2, etc.
* Pluggable
	- Have complete control over Uder Entities
	- Support non-relational identities
	- Even LDAP/AD stores
	
Using Authorize attribute
* Just adding [Authorize] attribute on the method won't work.
* Before making it work we need to store identities in the database.

Storing Identities in the Database
* In the entities folder add a class called StoreUser.
* Add properties inside StoreUser class, that will inherit other props on top of it from IdentityUser.
	public class StoreUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string LastName { get; set; }
    }
* Go to DutchContext -> instead of deriving from DbContext derive from IdentityDbContext.
* In the Order entity, -> Add a property User of StoreUser type:
	public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public StoreUser User { get; set; }
    }
* This way we are tying Indentity with Order system.
* To integrate the new properties and inherited ones go to console window :
	dotnet ef migrations add Identity
* It has the code in it to create the new tables, new relationships and even to modify order table
* Before that :
	dotnet ef migrations add Identity
* Open DutchSeeder.cs -> 
	public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext ctx, IHostingEnvironment hosting, UserManager<StoreUser> userManager )
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }
        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();
            StoreUser user = await _userManager.FindByEmailAsync("prashant.kumar@dreamorbit.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    Firstname = "Prashant",
                    LastName = "Kumar",
                    Email = "prashant.kumar@dreamorbit.com",
                    UserName = "prashant.kumar@dreamorbit.com"
                };
                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }
            }
            if (!_ctx.Products.Any())
            {
                //Need to create sample data
                var filepath = Path.Combine(_hosting.ContentRootPath,"Data/art.json");
                var json = File.ReadAllText(filepath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                //Add the products to DB
                _ctx.Products.AddRange(products);
                var order = _ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();
                if (order != null)
                {
                    order.User = user;
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem(){
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    };
                }
                _ctx.SaveChanges();
            }
        }
    }
* In Program.cs -> change seeder.Seed() to seeder.SeedAsync().Wait();
	
Configuring Identity
* In the startup ConfigureServices:
	services.AddIdentity<StoreUser, IdentityRole>(cfg =>
                    cfg.User.RequireUniqueEmail = true
                )
                .AddEntityFrameworkStores<DutchContext>();
* Add app.UseAuthentication();
      app.UseAuthorization(); // in case of .net core 2.1
* Clicking on Shop tab will now try to redirectvia Login, which is not present.	 

Designing the Login View & Implementing Login and Logout
* Login.cshtml
@model LoginViewModel
@section Scripts{
    <script src="~/node_modules/jquery-validation/dist/jquery.validate.min.js"></script>
    <script src="~/node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js"></script>
}

<div class="row">
    <div class="col-md-4 offset-md-4">
        <form method="post">
            <div asp-validation-summary="ModelOnly" class="text-error"></div>
            <div class="form-group">
                <label asp-for="Username">Username</label>
                <input asp-for="Username" class="form-control" />
                <span asp-validation-for="Username" class="text-warning"></span>
                
            </div>
            <div class="form-group">
                <label asp-for="Password">Password</label>
                <input asp-for="Password" type="password" class="form-control"/>
                <span asp-validation-for="Password" class="text-warning"></span>    
            </div>
            <div class="form-group">
                <div class="form-check">
                    <input asp-for="RememberMe" type="checkbox" class="form-check-input"/>
                    <label asp-for="RememberMe" class="form-check-label">RememberMe</label>
                </div>
                <span asp-validation-for="RememberMe" class="text-warning"></span>
            </div>
            <div class="form-group">
                <input type="submit" value="Login" class="btn btn-success" />
            </div>
            

        </form>
    </div>
</div>
* AccountController
@model LoginViewModel
@section Scripts{
    <script src="~/node_modules/jquery-validation/dist/jquery.validate.min.js"></script>
    <script src="~/node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js"></script>
}

<div class="row">
    <div class="col-md-4 offset-md-4">
        <form method="post">
            <div asp-validation-summary="ModelOnly" class="text-error"></div>
            <div class="form-group">
                <label asp-for="Username">Username</label>
                <input asp-for="Username" class="form-control" />
                <span asp-validation-for="Username" class="text-warning"></span>
                
            </div>
            <div class="form-group">
                <label asp-for="Password">Password</label>
                <input asp-for="Password" type="password" class="form-control"/>
                <span asp-validation-for="Password" class="text-warning"></span>    
            </div>
            <div class="form-group">
                <div class="form-check">
                    <input asp-for="RememberMe" type="checkbox" class="form-check-input"/>
                    <label asp-for="RememberMe" class="form-check-label">RememberMe</label>
                </div>
                <span asp-validation-for="RememberMe" class="text-warning"></span>
            </div>
            <div class="form-group">
                <input type="submit" value="Login" class="btn btn-success" />
            </div>
            

        </form>
    </div>
</div>
* LoginViewModel
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
* Add the following in the _Layout:
@if (User.Identity.IsAuthenticated)
                    {
                        <li class="nav-item">
                            <a class="nav-link" asp-controller="Account" asp-action="Logout">Logout</a>
                        </li>
                    }
                    else
                    {
                        <li class="nav-item">
                            <a class="nav-link" asp-controller="Account" asp-action="Login">Login</a>
                        </li>
                    }

Using Identity in the API
* "Can't I Just Use Cookie Auth for my API:
	-> Cookies are easiest, but least secure.
	-> Open ID, OAuth2, or JWT Tokens are best.
	-> Depends most on your security requirements, not the effort level.
* Add the following configuration in ConfigureServices(after services.AddIdentity):
	services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer();	
* 























