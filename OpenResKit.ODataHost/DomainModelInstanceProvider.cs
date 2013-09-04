#region License

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at
//  
// http://www.apache.org/licenses/LICENSE-2.0.html
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  
// Copyright (c) 2013, HTW Berlin

#endregion

using System;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace OpenResKit.ODataHost
{
  [Export]
  internal class DomainModelInstanceProvider : IInstanceProvider, IContractBehavior
  {
    private readonly Func<DomainModelDataService> m_CreateService;

    [ImportingConstructor]
    public DomainModelInstanceProvider([Import] Func<DomainModelDataService> createService)
    {
      m_CreateService = createService;
    }

    #region IInstanceProvider Members

    public object GetInstance(InstanceContext instanceContext, Message message)
    {
      return GetInstance(instanceContext);
    }

    public object GetInstance(InstanceContext instanceContext)
    {
      return m_CreateService();
    }

    public void ReleaseInstance(InstanceContext instanceContext, object instance)
    {
    }

    #endregion

    #region IContractBehavior Members

    public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
    {
    }

    public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    {
    }

    public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
    {
      dispatchRuntime.InstanceProvider = this;
    }

    public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
    {
    }

    #endregion
  }
}