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

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.ReflectionModel;
using System.Linq;

namespace OpenResKit.Runtime
{
  public class Container
  {
    private readonly CompositionContainer m_Container;

    public Container()
    {
      var directoryCatalog = new DirectoryCatalog(@"./");
      var assemblies = directoryCatalog.Parts.Select(part => ReflectionModelServices.GetPartType(part)
                                                                                    .Value.Assembly)
                                       .Distinct();
      var assemblyCatalogs = assemblies.Select(x => new AssemblyCatalog(x));

      var catalog = new AggregateCatalog(assemblyCatalogs);

      m_Container = new CompositionContainer(catalog);

      var batch = new CompositionBatch();
      m_Container.Compose(batch);
    }

    public T GetInstance<T>()
    {
      var contract = AttributedModelServices.GetContractName(typeof (T));
      return m_Container.GetExportedValue<T>(contract);
    }

    public IEnumerable<T> GetInstances<T>()
    {
      return m_Container.GetExportedValues<T>(AttributedModelServices.GetContractName(typeof (T)));
    }
  }
}