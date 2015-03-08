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


- ```DailyLimit<Withdrawal>``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/DailyLimit.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/DailyLimit.cs)
- ```BelowMinimumAllowed<Withdrawal``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/BelowMinimumAllowed.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/BelowMinimumAllowed.cs)
- ```AboveMaximumAllowed<Withdrawal``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/DailyLimit.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Filters/Withdrawal/DailyLimit.cs)

###Pipe:###

```IPipeline<T>``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Pipeline/IPipeline.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Pipeline/IPipeline.cs) 

Is the programming contract for the pipeline that allows the registrations of the filters (```IFinancialRule<T>```) implementations and executes these implementations to return back a final result.

```Pipeline<T>``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Pipeline/Pipeline.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Pipeline/Pipeline.cs)

Is the implementation for the ```IPipeline<T>``` interface.

```Context<T>``` - [https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Pipeline/Context.cs](https://github.com/Amila17/PipeAndFiltersDesignPattern/blob/master/src/PipeAndFiltersDesignPattern/Pipeline/Context.cs)

Is the class that carries any required objects through the pipeline. This class also keeps the state of any errors that occur during the filter execution process.


###Composition ###

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



Hope this helps in understanding the design pattern. 


P G Amila Prabandhika