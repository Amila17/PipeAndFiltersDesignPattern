# Pipe and Filters Design Pattern #

This project demonstrate the use of Pipe and Filters Design pattern. This pattern was something that was discussed as a solution to one of the problems that arose during an implementation discussion at work. As a result I decided to implement a possible solution that fits our scenario to demonstrate how the design pattern works.

More information on the design pattern could be found at the following links:



- [https://msdn.microsoft.com/en-gb/library/dn568100.aspx](https://msdn.microsoft.com/en-gb/library/dn568100.aspx "Pipes and Filters Design Pattern Concept")



- [http://eventuallyconsistent.net/tag/pipe-and-filters/](http://eventuallyconsistent.net/tag/pipe-and-filters/ "Pipes and Filters Design Pattern sample Implementation")


## Implementation Explaination ##

###Domain model:###

```Withdrawal``` class is the domain model which contains a property named ``Amount``. 


###Filters:###

```IFinancialRule<T>``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/IFinancialRule.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/IFinancialRule.cs)

Is the programming contract for the filters that could be applied for any financial process eg: withdrawal or deposit.

#####Implementations of the ```IFinancialRule<T>``` are:####

######Site1######

- ```DailyLimit100``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/Site1/DailyLimit100.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/Site1/DailyLimit100.cs)
- ```BelowMinimumAllowed100``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/Site1/BelowMinimumAllowed100.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/Site1/BelowMinimumAllowed100.cs)
- ```AboveMaximum1000``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/Site1/AboveMaximum1000.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/Site1/AboveMaximum1000.cs)

######Site2

- ```DailyLimit50``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/Site2/DailyLimit50.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/Site2/DailyLimit50.cs)
- ```BelowMinimumAllowed20``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/Site2/BelowMinimumAllowed20.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/Site2/BelowMinimumAllowed20.cs)
- ```AboveMaximum100``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/Site2/AboveMaximum100.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/Site2/AboveMaximum100.cs)

###Pipe:###

```IPipeline<T>``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Pipeline/IPipeline.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Pipeline/IPipeline.cs) 

Is the programming contract for the pipeline that allows the registrations of the filters (```IFinancialRule<T>```) implementations and executes these implementations to return back a final result.

```Pipeline<T>``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Pipeline/Pipeline.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Pipeline/Pipeline.cs)

Is the implementation for the ```IPipeline<T>``` interface.

```Context<T>``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Pipeline/Context.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Pipeline/Context.cs)

Is the class that carries any required objects through the pipeline. This class also keeps the state of any errors that occur during the filter execution process.


###Autofac Tenant Identification Strategy ###

```SimpleTenantIdentificationStrategy``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Autofac.Tenant/SimpleTenantIdentificationStrategy.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Autofac.Tenant/SimpleTenantIdentificationStrategy.cs) 

Implements ```ITenantIdentificationStrategy```. This class enables autofac to identify which tenant is in the current context of the application. A ideal implementation would be to extract a value from the current http request to identify the tenant.

More information on this could be found here: [http://autofac.readthedocs.org/en/latest/advanced/multitenant.html?highlight=multitenant#register-dependencies](http://autofac.readthedocs.org/en/latest/advanced/multitenant.html?highlight=multitenant#register-dependencies) 


----------

###Composition###

####Non IoC Composition####

The following code block demonstrates how all of the above code composes together to bring about the Pipe and Filters design pattern into action:


    //Valid withdrawal limit is amount > 100 and amount < 1000.

    var validWithdrawal = new Withdrawal() {Amount = 150};
    var inValidWithdrawalLimit = new Withdrawal() {Amount = 50};

    IFinancialRule<Withdrawal> dailyLimit = new DailyLimit();
    IFinancialRule<Withdrawal> belowMinimumAllowed = new BelowMinimumAllowed();
    IFinancialRule<Withdrawal> aboveMaximumAllowed = new AboveMaximum();

    IPipeline<Withdrawal> withdrawalPipeline = new Pipeline<Withdrawal>();
    withdrawalPipeline.Register(dailyLimit)
        .Register(belowMinimumAllowed)
        .Register(aboveMaximumAllowed);

    var validContext = new Context<Withdrawal>() {item = validWithdrawal};
    var result = withdrawalPipeline.Execute(validContext);
    Console.WriteLine(string.Format("The request for withdrawal is valid? {0}", result));

    var invalidContext = new Context<Withdrawal> {item = inValidWithdrawalLimit};
    result = withdrawalPipeline.Execute(invalidContext);
    Console.WriteLine(string.Format("The request for the withdrawal is valid? {0}", result));


####IoC Composition####

The following code block demonstrates how to register all of the above code with Autofac and use its ability to resolve different filters for different tenants with the use of Autofac.Multitenants package.

	var builder = new ContainerBuilder();
	
	builder.RegisterType<Pipeline<Withdrawal>>().As<IPipeline<Withdrawal>>();
	
	var container = builder.Build();
	
	var tenantIdentifier = new SimpleTenantIdentificationStrategy();
	
	var mtc = new MultitenantContainer(tenantIdentifier, container);
	
	mtc.ConfigureTenant("1", cb => cb.Register(ctx => 
	    new Pipeline<Withdrawal>()
	    .Register(new DailyLimit100())
	    .Register(new AboveMaximum1000())
	    .Register(new BelowMinimumAllowed100())).As<IPipeline<Withdrawal>>());
	
	mtc.ConfigureTenant("2", cb => cb.Register(ctx => 
	    new Pipeline<Withdrawal>()
	    .Register(new DailyLimit50())
	    .Register(new AboveMaximum100())
	    .Register(new BelowMinimumAllowed20())).As<IPipeline<Withdrawal>>());
	
	
	======== Tenant 1 (Site 1) Registrations ===========
	tenantIdentifier.SetTenant(TenantName.Site1);
	
	var withdrawalPipelineTenant1 = mtc.Resolve<IPipeline<Withdrawal>>();
	
	//Valid withdrawal limit is amount > 100 and amount < 1000.
	var validWithdrawal = new Withdrawal() { Amount = 150 };
	var inValidWithdrawalLimit = new Withdrawal() { Amount = 50 };
	
	var validContext = new Context<Withdrawal>() { item = validWithdrawal };
	var result = withdrawalPipelineTenant1.Execute(validContext);
	Console.WriteLine(string.Format("The request for Tenant 1 withdrawal is valid? {0}", result));
	
	var invalidContext = new Context<Withdrawal> { item = inValidWithdrawalLimit };
	result = withdrawalPipelineTenant1.Execute(invalidContext);
	Console.WriteLine(string.Format("The request for Tenant 1 withdrawal is valid? {0}", result));
	
	========== Tenant 2 (Site 2) Registrations ===========
	tenantIdentifier.SetTenant(TenantName.Site2);
	
	var withdrawalPipelineTenant2 = mtc.Resolve<IPipeline<Withdrawal>>();
	
	//Valid withdrawal limit is amount > 50 and amount < 100.
	validWithdrawal = new Withdrawal() { Amount = 60 };
	inValidWithdrawalLimit = new Withdrawal() { Amount = 10 };
	
	validContext = new Context<Withdrawal>() { item = validWithdrawal };
	result = withdrawalPipelineTenant2.Execute(validContext);
	Console.WriteLine(string.Format("The request for Tenant 2 withdrawal is valid? {0}", result));
	
	invalidContext = new Context<Withdrawal> { item = inValidWithdrawalLimit };
	result = withdrawalPipelineTenant2.Execute(invalidContext);
	Console.WriteLine(string.Format("The request for Tenant 2 withdrawal is valid? {0}", result));



Hope this helps in understanding the design pattern. 


P G Amila Prabandhika
