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
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace OpenResKit.ODataHost
{
  public class CustomHeaderMessageInspector : IDispatchMessageInspector
  {
    private Dictionary<string, string> requiredHeaders;

    public CustomHeaderMessageInspector(Dictionary<string, string> headers)
    {
      requiredHeaders = headers ?? new Dictionary<string, string>();
    }

    public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
    {
      return null;
    }

    public void BeforeSendReply(ref Message reply, object correlationState)
    {
      var httpHeader = reply.Properties["httpResponse"] as HttpResponseMessageProperty;
      foreach (var item in requiredHeaders)
      {
        httpHeader.Headers.Add(item.Key, item.Value);
      }
    }
  }
}