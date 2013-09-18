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
using System.Data.Services;
using System.Data.Services.Common;
using System.ServiceModel;
using OpenResKit.DomainModel;

namespace OpenResKit.ODataHost
{
  [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
  internal class DomainModelDataService : DataService<DomainModelContext>
  {
    private readonly Func<DomainModelContext> m_CreateContext;

    [ImportingConstructor]
    public DomainModelDataService([Import] Func<DomainModelContext> createContext)
    {
      m_CreateContext = createContext;
    }

    // This method is called only once to initialize service-wide policies.
    public static void InitializeService(DataServiceConfiguration config)
    {
      // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
      // Examples:
      // config.SetEntitySetAccessRule("MyEntityset", EntitySetRights.AllRead);

      config.SetEntitySetAccessRule("*", EntitySetRights.All);
      //config.SetServiceOperationAccessRule("*",
      //																		 ServiceOperationRights.All);
      config.UseVerboseErrors = true;
      config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
    }

    protected override DomainModelContext CreateDataSource()
    {
      return m_CreateContext();
    }
  }
}