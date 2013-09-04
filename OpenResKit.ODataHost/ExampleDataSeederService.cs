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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ServiceModel;
using OpenResKit.DomainModel;


namespace OpenResKit.ODataHost
{
  [ExportMetadata("Name", "ExampleDataSeeder")]
  [Export(typeof(IWCFService))]
  [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single)]
  [ServiceContract]
  internal class ExampleDataSeederService : IWCFService
  {
    private readonly IEnumerable<IInitialSeed> m_InitialSeeds;
    private readonly DomainModelContext m_Context;

    [ImportingConstructor]
    public ExampleDataSeederService([Import] Func<DomainModelContext> createContext, [ImportMany] IEnumerable<IInitialSeed> initialSeeds)
    {
      m_InitialSeeds = initialSeeds;
      m_Context = createContext();
    }
    
    [OperationContract]
    public void Seed()
    {
      foreach (var initialSeed in m_InitialSeeds)
      {
        initialSeed.Seed(m_Context);
      }
    }

  }
}
