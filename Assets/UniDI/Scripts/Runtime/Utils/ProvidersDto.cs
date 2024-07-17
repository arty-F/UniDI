﻿using UniDI.Providers;

namespace UniDI.Utils
{
    internal class ProvidersDto
    {
        internal GenericMethodsProvider GenericMethodsProvider { get; private set; }
        internal SettersProvider SettersProvider { get; private set; }
        internal MemberInfoProvider MemberInfoProvider { get; private set; }
        internal InstancesProvider InstancesProvider { get; private set; }
        internal ParameterTypesProvider ParameterTypesProvider { get; private set; }

        internal ProvidersDto(
            GenericMethodsProvider genericMethodsProvider, 
            SettersProvider settersProvider, 
            MemberInfoProvider memberInfoProvider, 
            InstancesProvider instancesProvider, 
            ParameterTypesProvider parameterTypesProvider)
        {
            GenericMethodsProvider = genericMethodsProvider;
            SettersProvider = settersProvider;
            MemberInfoProvider = memberInfoProvider;
            InstancesProvider = instancesProvider;
            ParameterTypesProvider = parameterTypesProvider;
        }
    }
}
