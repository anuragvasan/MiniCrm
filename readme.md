# MiniCrm

MiniCrm is a demo application consisting of basic CRM functionality such as adding and searching for customer information.  The application is written using the ASP.NET Core MVC stack using Entity Framework Core and Bootstrap 4.

A preview is available at: https://web-minicrm-use1.azurewebsites.net/

## Application Functionality

MiniCrm enables users to add new customers to the system and search for existing customers.  MiniCrm defines a customer as the following data points:

- Name (Full name only)
- Email Address
- Phone Number + Extension
- Address (Line 1, Line 2, City, State, Postal Code)

### Adding Customers

Since it is common to initially possess minimal information about a customer (perhaps only an email address), MiniCrm is flexible with what information is needed to store a new customer.  The following rules are in place:

- At least one major piece of information about the customer is required: Name, Phone Number, or Email Address.
- If any Address information is provided, a full address (line 1, city, state, and postal code) are required.
- If phone extension is provided, the number must also be provided.

Requests to add customers are audited (logged) into the standard .NET core logging pipeline (eg console) and Azure Application Insights (eg the Visual Studio integration locally.)

### Searching Customers

MiniCrm enables searching existing customers by either Name or Email Address.  Partial matches enable the user to search for only a first or last name, or part of an email address, even when MiniCrm has the full information stored.

## Development Setup & Process

- Install .NET Core SDK + CLI: https://dotnet.microsoft.com/download/dotnet-core/3.1
- Install Entity Framework CLI tools: `dotnet tool install --global dotnet-ef`
- Install or allocate a SQL Server instance.  Default: `Server=(localdb)\mssqllocaldb;Database=MiniCrm`
- Install Selenium IDE: https://chrome.google.com/webstore/detail/selenium-ide/mooikfkahbdckldjjndioackbalphokd?hl=en

### Running the Application

- Run EF Migrations, if necessary: `dotnet ef database update`
- From the MiniCrm.Web directory, execute `dotnet run` on the CLI.
- Request the specified URL in the browser, eg http://localhost:5000.
- Alternatively, start the application in Visual Studio using IIS Express.

### Running Tests

- .NET unit tests can be run within Visual Studio for the following projects:
  - /Tests/MiniCrm.Application.Tests
  - /Tests/MiniCrm.Persistence.Tests
  - /Tests/MiniCrm.Web.Tests
- Selenium tests can be run by:
  - Opening the Selenium IDE
  - Opening the /MiniCrm/Tests/MiniCrm.side project
  - Starting the website (see above.)
  - Updating the Selenium URL, if necessary.  Default: http://localhost:59778/
  - Execute tests.

### Entity Framework Migrations

- Modify Entity Framework code-first data structure (eg MiniCrm.DataModel.Customer class) locally.
- run `dotnet ef migrations add <name>` to generate migration scripts for the changes.
- To execute the migration on a local database, run `dotnet ef database update`.
- Commit both the code-first classes and migrations to source control.

### Azure Deployment

- The demo preview link is deployed to Azure and uses the following:
  - App Service Plan
  - App Service
  - Azure SQL Server
  - Azure SQL Database
  - Application Insights (optional)
- Create the resources above.
- Link the App Service to the App Insights resource.
- Update the App Service connection string to the SQL Database.
- Update firewall rules on the SQL Server to allow Azure resources to connect.
- To deploy the application, use Visual Studio's "Publish" feature to deploy to the App Service.
- To deploy the EF migrations to the database:
  - Update firewall rules on the SQL Server to allow the Client IP to connect.
  - Update appsettings.json "Crm" connection string.
  - Run `dotnet ef database update` from the MiniCrm.DataModel directory.
  - Undo the firewall rule and appsettings.json change.

## Technology Overview

- **ASP.NET Core MVC** - Provides a modern server-side web application framework with strong HTML templating via Razor, extensibility (eg action filters), and the underlying .NET ecosytem.
- **Entity Framework Core** - Provides a lot of quick-start value for the SQL database and can be optimized sufficiently for most use cases.  Code First and Migrations provide a relatively simple and effective schema declaration and update mechanism without involving external tools (MS SQL Projects, RedGate SQL Source Control, etc.)
- **MediatR** - Used to implement the CQS pattern in the application logic layer, providing a clear set of expected inputs and outputs, and flexibility/decoupling in logic execution.  Enables implementing cross-cutting concerns like logging/auditing via `IPipelineBehavior`.
- **AutoMapper** - Enables mapping between objects in different application layers, providing better separation between application concerns (eg logic and persistence.)
- **FluentValidation** - Provides powerful server-side validation (including outside of a direct web context), and hooks for ASP.NET MVC's standard client-side validation.  Capable of far more complex validations than DataAnnotations provides out of the box.
- **Boostrap 4** - A convienient quick-start UI framework for the responsive web.

## Code Architecture

The application code consists of several .NET projects, each encapsulating an area of reponsibility.

- **MiniCrm.Web** - ASP.NET Core MVC application providing a traditional web presentation layer using Razor views and Bootstrap 4 for responsive styling.  Provides startup logic (eg runtime configuration, DI bindings) for the application as a whole.
- **MiniCrm.Application** - The core application logic layer, intended to be presentation-layer agnostic.  A basic Command-Query Separation (CQS) approach is used, consisting of Query and Command objects (read and write, respectively) to represent application inputs.
- **MiniCrm.Persistence** - Handles integration concerns between the application's core logic (MiniCrm.Application) and other systems (in this case, SQL Server via MiniCrm.DataModel).  Implements the CQS CommandHandlers and QueryHandlers that actually satisfy the needs defined in MiniCrm.Application.
- **MiniCrm.DataModel** - Implements the SQL database schema via Entity Framework code-first and migrations.  Exposes the `CrmContext` and EF entities such as `Customer` for use by the MiniCrm.Persistence project.

### Design Decisions

- **MiniCrm.Web** provides the presentation layer exposing the features offered by the other proejcts.  Additionally, it provides all of the dependency injection configuration that enables those projects to work together.
  - Several customizations have been implemented  to reduce code duplication in the controllers:
    - **FluentValidation's ASP.NET integration**, which automatically validates models when a matching validator is registered in the DI container, eg `AddCustomerModelValidator`.
    - **ModelStateValidationFilter**, which removes the need to check ModelState and return the original View (form) back to the user with valiation errors in each POST controller action.
    - **ModelInitializerFilter** introduces an `IModelInitializer` interface which can contain logic to populate a ViewModel.  For example, filling a list of dropdown options.  This logic typically has to be performed for both rendering the initial form (GET) and re-displaying the form when validation errors occurs (POST).  Introducing this interface and action filter removes the need for duplicated code / method calls in the GET and POST controller actions.
- The **MiniCrm.Application** project composes the public application contract (ie what the user do with the application) and implementation of the core logic.  In this case, there is no significant "business logic" in the traditional sense (beyond validation rules), but this project would be the appropriate place for it.
  - The project is written without any ties to the web or SQL layers.  Other client layers (eg WebApi) could easily use it to expose the same functionality as the current MVC presentation layer.
  - CQS was chosen as an overall architecture for this layer because it provides a very explicit public contract.  Input and outputs are clearly documented on the Command, Query, and QueryResult classes.  Commands always accept input and return nothing (or exceptions for errors), and queries return the specified result type.
  - The MediatR library was chosen to implement the CQS pattern, using the `IRequestHandler<>` interface.  This library provides the basic types to represent commands, queries, query results, and handler classes, along with supporting infrastructure (eg running a series of handlers in sequence, cross-cutting concerns like logging, etc.)
- **MiniCrm.DataModel** encapsulates all SQL database & EntityFramework concerns, including both defning the database schema, performing migrations, and exposing the .NET API (via EF entities and DbContext) for interacting with it.
- **MiniCrm.Persistence** is a separate project from both Application and DataModel to communiucate clear intent and boundaries.  Application should not be concerned with system integrations and side effects outside of the core application features and logic.  DataModel handles only EF/SQL-specific persistence concerns, providing a set of features to other code that might make use of them.  
  - This leaves the Persistence project as the glue between them, and gives it flexibility to change as needed.  For example, if a search engine (eg Azure Search or Solr) was to be introduced, the Persistence project could implement a new CustomerSearch handler using that technology, and could perform indexing as part of adding customers.  Neither the DataModel or Application projects would need to change.
  - This project uses AutoMapper to convert between EF Entities and the Command/Query objects.  These mappings are somewhat tedious, but it is important to decouple the database data model from the application data structure so that they can change independently over time (performance optimizations, etc.)



## Known Issues / Further Considerations

- Server-side validation errors are not properly styled with the Bootstrap 4 UI classes.  Consider this approach: https://stackoverflow.com/a/63208680
- "Complex" validation rules, such as requiring any of Name, Email, and Phone Number for adding a customer, are only implemented on the server.  Client-side logic could be added to improve the UX.
- MediatR requires injecting an `IMediator` interface for invoking the RequestHandlers, which hides away more information than might be desirable.  For example, controllers simply depend on `IMediator`, rather than a specific list of `IRequestHandlers<TRequest, TResponse>`.  This makes it less clear what the exact dependencies of a controller are.  Although it is possible to take dependencies on particular RequestHandler interfaces directly (Eg `IRequestHandler<GetStates, IEnumerable<State>>`), this doesn't support PipelineBehaviors (such as the cross-cutting AuditingBehavior) and doesn't seem to be the intended way to use the library.
- No client-side package manager or build process for Bootstrap and other front-end assets.  If the front-end UI were to be enhanced further, this would likely be beneficial.  Not really warranted for the current basic UI.
- There exists duplication of the Customer definition across layers, and AutoMapper profiles are somewhat tedious and error-prone.  This might be improved with some centralized metadata / code-gen, or further use of AutoMapper convention techniques.  However, the value of 
- The MVC ViewModels have properties representing the Command and Query objects, rather than exposing a clean interface with only the necessary input/output values.  This rather significantly reduces complexity (MiniCrm.Application public contract is "passed through" to/from the UI, rather than adding another set of classes and mapping back and forth.  However, this does result in some oddities such as the AddCustomerModelValidator simply delegating immediately to AddCustomerValidator.  This is required because FluentValidation's ASP.NET integration looks for a validator matching the model type.  Further, the MiniCrm.Application validators are _not_ being run as part of the actual processing pipeline (since this would duplicate what's done at the ASP.NET layer).  This would need to be re-examined if MiniCrm.Application was actually to be used by other/additional presentation layers or use cases.
- Azure deployment is completely manual.  Introduce Azure DevOps build + release pipeline, with ARM templates.