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
using System.Data.Objects.DataClasses;
using System.ServiceModel;
using OpenResKit.DomainModel;

namespace OpenResKit.ODataHost
{
  [Export(typeof (IWCFService))]
  [ExportMetadata("Name", "DeleteEvaluationService")]
  [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single)]
  [ServiceContract]
  internal class DeleteEvaluationService : IWCFService
  {
    private readonly Func<DomainModelContext> m_CreateContext;

    [ImportingConstructor]
    public DeleteEvaluationService([Import] Func<DomainModelContext> createContext)
    {
      m_CreateContext = createContext;
    }

    [OperationContract]
    public bool CanDelete(string type, int id)
    {
      var context = m_CreateContext();
      var set = context.Set(Type.GetType("OpenResKit.Waste.WasteContainer, OpenResKit.Waste"));
      var entity = set.Find(id);
      var result = ((IEntityWithRelationships) entity).RelationshipManager.GetAllRelatedEnds();
      foreach (var relatedEnd in result)
      {
        if (!relatedEnd.IsLoaded)
        {
          return false;
        }
      }
      return true;
    }
  }
}