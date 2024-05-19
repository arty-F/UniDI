using System;

namespace Assets.Core
{
    internal interface IResolver
    {
        void Resolve(object consumer, Type consumerType);
    }
}