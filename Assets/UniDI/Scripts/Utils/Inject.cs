using System;

namespace UniDI
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class Inject : Attribute
    {
    }
}
