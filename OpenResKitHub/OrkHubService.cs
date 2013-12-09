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

using System.ServiceProcess;
using OpenResKit.ODataHost;
using OpenResKit.Runtime;

namespace OpenResKitHub
{
  public class OrkHubService : ServiceBase
  {
    private IHostService host;

    public OrkHubService()
    {
      ServiceName = "OpenResKit Hub";
      EventLog.Log = "Application";

      CanHandlePowerEvent = false;
      CanHandleSessionChangeEvent = false;
      CanPauseAndContinue = false;
      CanShutdown = false;
      CanStop = true;
    }

    public void StartFromConsole(string[] args)
    {
      OnStart(args);
    }

    protected override void OnStart(string[] args)
    {
      base.OnStart(args);
      var container = new Container();
      host = container.GetInstance<IHostService>();
      host.StartServices();
    }

    protected override void OnStop()
    {
      base.OnStop();
      host.StopServices();
    }
  }
}