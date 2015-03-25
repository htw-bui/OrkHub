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

namespace OpenResKit.FacilityManagement
{
  public class Task
  {
    public virtual int Id { get; set; }

    public virtual string Description { get; set; }

    public virtual string Location { get; set; }

    public virtual byte[] Image { get; set; }

    public virtual DateTime ReportDate { get; set; }

    public virtual DateTime DueDate { get; set; }

    public virtual bool IsTaskFixed { get; set; }

    public void ComputeEndDate()
    {
      DueDate = ReportDate.AddDays(7);
    }
  }
}