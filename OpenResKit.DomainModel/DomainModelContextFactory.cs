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
using System.Data.Entity;

namespace OpenResKit.DomainModel
{
  internal class DomainModelContextFactory
  {
    private readonly IEnumerable<IDomainEntityConfiguration> m_DomainEntityConfigurations;
    private readonly IDatabaseInitializer<DomainModelContext> m_Initializer;

    [ImportingConstructor]
    public DomainModelContextFactory([Import] IDatabaseInitializer<DomainModelContext> initializer, [ImportMany] IEnumerable<IDomainEntityConfiguration> domainEntityConfigurations)
    {
      m_Initializer = initializer;
      m_DomainEntityConfigurations = domainEntityConfigurations;
    }

    [Export]
    public DomainModelContext CreateContext()
    {
      return new DomainModelContext(m_Initializer, m_DomainEntityConfigurations);
    }
  }
}