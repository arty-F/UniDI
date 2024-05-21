using Assets.Core.Utils;
using System;
using System.Collections.Generic;

namespace Assets.Core.Providers
{
    internal class ResolvingStopListProvider
    {
        private readonly List<Type> _fieldStopList = new();
        private readonly List<Type> _propertyStopList = new();
        private readonly List<Type> _methodStopList = new();
        private readonly Dictionary<ResolvingType, List<Type>> _dictionaryMap;

        public ResolvingStopListProvider()
        {
            _dictionaryMap = new()
            {
                { ResolvingType.Field, _fieldStopList },
                { ResolvingType.Property, _propertyStopList },
                { ResolvingType.Method, _methodStopList },
            };
        }

        internal bool IsResolvingStoped(ResolvingType resolvingType, Type consumerType)
        {
            if (_dictionaryMap[resolvingType].Contains(consumerType))
            {
                return true;
            }

            return false;
        }

        internal void StopResolving(ResolvingType resolvingType, Type consumerType)
        {
            _dictionaryMap[resolvingType].Add(consumerType);
        }
    }
}
