
using Autofac.Extras.Multitenant;
using PipeAndFiltersDesignPattern.Tenants;

namespace PipeAndFiltersDesignPattern.Autofac.Tenant
{
    public class SimpleTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        private TenantName _tenantName;

        public void SetTenant(TenantName tenantName)
        {
            _tenantName = tenantName;
        }
        public bool TryIdentifyTenant(out object tenantId)
        {
            tenantId = null;
            if (_tenantName == TenantName.Site1)
                tenantId = "1";

            if (_tenantName == TenantName.Site2)
                tenantId = "2";

            return tenantId != null;
        }
    }
}
