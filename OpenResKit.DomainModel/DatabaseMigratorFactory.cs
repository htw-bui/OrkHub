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
using System.Data.Entity.Migrations;

namespace OpenResKit.DomainModel
{
  [Export]
  internal class DatabaseMigratorFactory
  {
    private readonly IEnumerable<DbMigrationsConfiguration> m_DatabaseMigrationsConfigurations;

    [ImportingConstructor]
    public DatabaseMigratorFactory([ImportMany] IEnumerable<DbMigrationsConfiguration> databaseMigrationsConfigurations)
    {
      m_DatabaseMigrationsConfigurations = databaseMigrationsConfigurations;
    }

    public IEnumerable<DbMigrator> CreateMigrators()
    {
      foreach (var databaseMigrationsConfiguration in m_DatabaseMigrationsConfigurations)
      {
        yield return new DbMigrator(databaseMigrationsConfiguration);
      }
    }
  }
}