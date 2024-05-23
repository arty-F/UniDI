using System;

namespace MonoInjector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class Inject : Attribute
    {
    }
}
