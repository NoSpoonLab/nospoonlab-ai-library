using System;

namespace DependencyInjectionCore
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class InjectAttribute : Attribute
    {
    }
}

