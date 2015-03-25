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
using System.Data.Entity;

namespace OpenResKit.DomainModel
{
  public sealed class DomainModelContext : DbContext
  {
    private readonly IEnumerable<IDomainEntityConfiguration> m_DomainEntityConfigurations;

    public DomainModelContext()
    {
    }

    public DomainModelContext(IDatabaseInitializer<DomainModelContext> initializer, IEnumerable<IDomainEntityConfiguration> domainEntityConfigurations)
    {
      m_DomainEntityConfigurations = domainEntityConfigurations;
      Database.SetInitializer(initializer);
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      foreach (var domainEntityConfiguration in m_DomainEntityConfigurations)
      {
        domainEntityConfiguration.AddConfigurationToModel(modelBuilder.Configurations);
      }

      base.OnModelCreating(modelBuilder);
    }
  }
}