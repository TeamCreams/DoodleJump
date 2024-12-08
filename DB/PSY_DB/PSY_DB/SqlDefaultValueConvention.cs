using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

public class SqlDefaultValueConvention
    : PropertyAttributeConventionBase<SqlDefaultValueAttribute>
{
    public SqlDefaultValueConvention(ProviderConventionSetBuilderDependencies dependencies)
        : base(dependencies) { }

    protected override void ProcessPropertyAdded(
        IConventionPropertyBuilder propertyBuilder,
        SqlDefaultValueAttribute attribute,
        MemberInfo clrMember,
        IConventionContext context)
    {
        propertyBuilder.HasDefaultValueSql(attribute.DefaultValue);
    }
}