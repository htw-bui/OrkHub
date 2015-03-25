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
using System.Linq;
using System.ServiceModel;
using OpenResKit.DomainModel;
using OpenResKit.ODataHost;

namespace OpenResKit.FacilityManagement
{
  [ExportMetadata("Name", "ComputeEndDate")]
  [Export(typeof (IWCFService))]
  [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single)]
  [ServiceContract]
  internal class ComputeEndDate : IWCFService
  {
    private readonly Func<DomainModelContext> m_CreateContext;

    [ImportingConstructor]
    public ComputeEndDate([Import] Func<DomainModelContext> createContext)
    {
      m_CreateContext = createContext;
    }


    [OperationContract]
    public long Calculate(int id)
    {
      using (var context = m_CreateContext())
      {
        var task = context.Set<Task>()
                          .Single(t => t.Id == id);
        task.ComputeEndDate();
        context.SaveChanges();

        var ms = (task.DueDate.Ticks - 621355968000000000) / TimeSpan.TicksPerMillisecond;
        return ms;
      }
    }
  }
}