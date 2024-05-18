using System;

namespace Assets
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class Inject : Attribute
    {
    }
}
