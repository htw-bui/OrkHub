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

using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace OpenResKitHub
{
  [RunInstaller(true)]
  public class OrkHubServiceInstaller : Installer
  {
    public OrkHubServiceInstaller()
    {
      var serviceProcessInstaller = new ServiceProcessInstaller();
      var serviceInstaller = new ServiceInstaller();

      //# Service Account Information
      serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
      serviceProcessInstaller.Username = null;
      serviceProcessInstaller.Password = null;

      //# Service Information
      serviceInstaller.DisplayName = "OpenResKit Hub";
      serviceInstaller.Description = "Stellt den OData des OpenResKit Hubs zur Verfügung";
      serviceInstaller.StartType = ServiceStartMode.Automatic;

      //# This must be identical to the WindowsService.ServiceBase name
      //# set in the constructor of WindowsService.cs
      serviceInstaller.ServiceName = "OpenResKit Hub";

      Installers.Add(serviceProcessInstaller);
      Installers.Add(serviceInstaller);
    }
  }
}