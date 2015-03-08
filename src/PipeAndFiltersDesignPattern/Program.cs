using System;
using Autofac;
using Autofac.Extras.Multitenant;
using PipeAndFiltersDesignPattern.Autofac.Tenant;
using PipeAndFiltersDesignPattern.DomainObjects;
using PipeAndFiltersDesignPattern.Filters;
using PipeAndFiltersDesignPattern.Filters.Withdrawal.Site1;
using PipeAndFiltersDesignPattern.Filters.Withdrawal.Site2;
using PipeAndFiltersDesignPattern.Pipeline;
using PipeAndFiltersDesignPattern.Tenants;

namespace PipeAndFiltersDesignPattern
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Select method of execution:");
            Console.WriteLine("A - Non IoC implementation.");
            Console.WriteLine("B - IoC implementation.");
            Console.WriteLine("Enter input: ");

            var input = Console.ReadLine();

            if(String.Equals(input, "A", StringComparison.InvariantCultureIgnoreCase))
                NonIoCRegistration();

            else if (String.Equals(input, "B", StringComparison.InvariantCultureIgnoreCase))
                IoCRegistrationWithMultiTenancy();

            else
                Console.WriteLine("Invalid input. Program closing.");

            Console.WriteLine("Program execution complete.");
            Console.ReadKey();
        }

        private static void NonIoCRegistration()
        {
            //Valid withdrawal limit is amount > 100 and amount < 1000.
            var validWithdrawal = new Withdrawal() { Amount = 150 };
            var inValidWithdrawalLimit = new Withdrawal() { Amount = 50 };

            IFinancialRule<Withdrawal> dailyLimit = new DailyLimit100();
            IFinancialRule<Withdrawal> belowMinimumAllowed = new BelowMinimumAllowed100();
            IFinancialRule<Withdrawal> aboveMaximumAllowed = new AboveMaximum1000();

            IPipeline<Withdrawal> withdrawalPipeline = new Pipeline<Withdrawal>();
            withdrawalPipeline.Register(dailyLimit)
                .Register(belowMinimumAllowed)
                .Register(aboveMaximumAllowed);

            var validContext = new Context<Withdrawal>() { item = validWithdrawal };
            var result = withdrawalPipeline.Execute(validContext);
            Console.WriteLine(string.Format("The request for withdrawal is valid? {0}", result));

            var invalidContext = new Context<Withdrawal> { item = inValidWithdrawalLimit };
            result = withdrawalPipeline.Execute(invalidContext);
            Console.WriteLine(string.Format("The request for the withdrawal is valid? {0}", result));

        }

        private static void IoCRegistrationWithMultiTenancy()
        {
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
        }
    }
}
