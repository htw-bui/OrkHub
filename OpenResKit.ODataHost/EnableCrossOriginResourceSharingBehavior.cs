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
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace OpenResKit.ODataHost
{
  public class EnableCrossOriginResourceSharingBehavior : BehaviorExtensionElement, IEndpointBehavior
  {
    public override Type BehaviorType
    {
      get { return typeof (EnableCrossOriginResourceSharingBehavior); }
    }

    public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
    {
    }

    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    {
    }

    public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
    {
      var requiredHeaders = new Dictionary<string, string>();

      requiredHeaders.Add("Access-Control-Allow-Origin", "*");
      requiredHeaders.Add("Access-Control-Request-Method", "POST,GET,PUT,DELETE,OPTIONS");
      requiredHeaders.Add("Access-Control-Allow-Headers", "X-Requested-With,Content-Type");

      endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new CustomHeaderMessageInspector(requiredHeaders));
    }

    public void Validate(ServiceEndpoint endpoint)
    {
    }

    protected override object CreateBehavior()
    {
      return new EnableCrossOriginResourceSharingBehavior();
    }
  }
}