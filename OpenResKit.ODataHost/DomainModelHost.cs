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
using System.Data.Services;
using System.ServiceModel.Description;

namespace OpenResKit.ODataHost
{
  internal class DomainModelHost : DataServiceHost
  {
    private readonly IContractBehavior m_InstanceProvider;

    internal DomainModelHost(DomainModelInstanceProvider instanceProvider, Type serviceType, Uri[] uris)
      : base(serviceType, uris)
    {
      m_InstanceProvider = instanceProvider;

      foreach (var cd in ImplementedContracts.Values)
      {
        cd.Behaviors.Add(m_InstanceProvider);
      }
    }
  }
}