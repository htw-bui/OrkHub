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
using System.IO;
using System.Reflection;

namespace OpenResKit.FacilityManagement
{
  [Export]
  public class TaskFactory
  {
    public IEnumerable<Task> CreateSiTasks()
    {
      var assembly = Assembly.LoadFrom("OpenResKit.FacilityManagement.dll");
      var log = Assembly.GetExecutingAssembly()
                        .GetManifestResourceNames();
      var imageStreamTafel = assembly.GetManifestResourceStream("OpenResKit.FacilityManagement.Resources.tafel.jpg");
      var imageStreamKabelsalat = assembly.GetManifestResourceStream("OpenResKit.FacilityManagement.Resources.kabelsalat.jpg");
      var byteArray1 = new byte[0];
      var byteArray2 = new byte[0];

      using (var br = new BinaryReader(imageStreamTafel))
      {
        byteArray1 = br.ReadBytes((int) imageStreamTafel.Length);
      }

      using (var br = new BinaryReader(imageStreamKabelsalat))
      {
        byteArray2 = br.ReadBytes((int) imageStreamKabelsalat.Length);
      }


      var taskOne = new Task
                    {
                      Description = "Tafel muss gewischt werden",
                      Location = "Entwicklerlabor",
                      ReportDate = DateTime.Now,
                      DueDate = DateTime.Now.AddDays(7),
                      Image = byteArray1,
                      IsTaskFixed = true
                    };

      yield return taskOne;

      var taskTwo = new Task
                    {
                      Description = "Kabelsalat aufräumen",
                      Location = "Raum F212",
                      ReportDate = DateTime.Now.AddDays(-1),
                      DueDate = DateTime.Now.AddDays(6),
                      Image = byteArray2,
                      IsTaskFixed = false
                    };

      yield return taskTwo;
    }
  }
}