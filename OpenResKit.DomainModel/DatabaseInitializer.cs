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
  [Export(typeof (IDatabaseInitializer<DomainModelContext>))]
  internal class DatabaseInitializer : IDatabaseInitializer<DomainModelContext>
  {
    private readonly DatabaseMigratorFactory m_DatabaseMigratorFactory;
    
    [ImportingConstructor]
    public DatabaseInitializer([Import] DatabaseMigratorFactory databaseMigratorFactory)
    {
      m_DatabaseMigratorFactory = databaseMigratorFactory;
    }

    public void InitializeDatabase(DomainModelContext context)
    {
      if (context.Database.Exists())
      {
        if (!context.Database.CompatibleWithModel(true))
        {
          Migrate();
        }
      }
      else
      {
        context.Database.Create();
      }
    }

    private void Migrate()
    {
      foreach (var migrator in m_DatabaseMigratorFactory.CreateMigrators())
      {
        migrator.Update();
      }
    }
  }
}