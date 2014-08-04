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
using System.Data.Services;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using OpenResKit.ODataHost.Properties;

namespace OpenResKit.ODataHost
{
  [Export(typeof (IHostService))]
  internal class HostService : IHostService
  {
    private readonly CustomUserNameValidator m_CustomUserNameValidator;
    private readonly DomainModelInstanceProvider m_DomainModelInstanceProvider;
    private readonly PolicyRetriever m_PolicyRetriever;
    private readonly IList<ServiceHost> m_ServiceHosts = new List<ServiceHost>();
    private readonly IEnumerable<Lazy<IWCFService, Dictionary<string, object>>> m_Services;
    private DataServiceHost m_DataHost;
    private ServiceHost m_PolicyHost;
    private readonly string m_BaseAdress = string.Format("http://{0}:{1}/", Settings.Default.Url, Settings.Default.Port);

    [ImportingConstructor]
    public HostService([ImportMany(typeof (IWCFService))] IEnumerable<Lazy<IWCFService, Dictionary<string, object>>> services, [Import] PolicyRetriever policyRetriever,
      [Import] DomainModelInstanceProvider domainModelInstanceProvider, [Import] CustomUserNameValidator customUserNameValidator)
    {
      m_Services = services;
      m_PolicyRetriever = policyRetriever;
      m_DomainModelInstanceProvider = domainModelInstanceProvider;
      m_CustomUserNameValidator = customUserNameValidator;
    }

    public void StartServices()
    {
      StartDataHost(m_BaseAdress);
      StartWCFHosts(m_BaseAdress);
      StartPolicyHost(m_BaseAdress);
    }

    public void StopServices()
    {
      m_PolicyHost.Close();
      m_DataHost.Close();
      foreach (var serviceHost in m_ServiceHosts)
      {
        serviceHost.Close();
      }
    }

    private void StartDataHost(string baseAdress)
    {
      m_DataHost = new DomainModelHost(m_DomainModelInstanceProvider, typeof (DomainModelDataService), new[]
                                                                                                       {
                                                                                                         new Uri(baseAdress + "OpenResKitHub")
                                                                                                       });

      var binding = new WebHttpBinding();
      binding.MaxReceivedMessageSize = int.MaxValue;
      binding.Security.Mode = WebHttpSecurityMode.TransportCredentialOnly;
      binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
      m_DataHost.AddServiceEndpoint(typeof (IRequestHandler), binding, "")
                .Behaviors.Add(new EnableCrossOriginResourceSharingBehavior());
      m_DataHost.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
      m_DataHost.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = m_CustomUserNameValidator;
      m_DataHost.Open();
    }

    private void StartWCFHosts(string baseAdress)
    {
      foreach (var serviceMetadata in m_Services)
      {
        var metadata = serviceMetadata.Metadata;
        var service = serviceMetadata.Value;

        var serviceHost = new ServiceHost(service, new[]
                                                   {
                                                     new Uri(baseAdress + metadata["Name"])
                                                   });

        var http = new BasicHttpBinding();
        http.MaxReceivedMessageSize = 2147483647;
        http.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
        http.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
        serviceHost.AddServiceEndpoint(service.GetType(), http, "");
        serviceHost.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
        serviceHost.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = m_CustomUserNameValidator;

        var stp = serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();
        stp.HttpHelpPageEnabled = false;
        var smb = new ServiceMetadataBehavior
                  {
                    HttpGetEnabled = true
                  };
        serviceHost.Description.Behaviors.Add(smb);

        m_ServiceHosts.Add(serviceHost);
        serviceHost.Open();
      }
    }

    private void StartPolicyHost(string baseAdress)
    {
      m_PolicyHost = new ServiceHost(m_PolicyRetriever, new[]
                                                        {
                                                          new Uri(baseAdress)
                                                        });
      var binding = new WebHttpBinding();
      //binding.Security.Mode = WebHttpSecurityMode.TransportCredentialOnly;
      //binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

      m_PolicyHost.AddServiceEndpoint(m_PolicyRetriever.GetType(), binding, "")
                  .Behaviors.Add(new WebHttpBehavior());
      //m_PolicyHost.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
      //m_PolicyHost.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = m_CustomUserNameValidator;
      var smb = new ServiceMetadataBehavior
                {
                  HttpGetEnabled = true
                };
      m_PolicyHost.Description.Behaviors.Add(smb);
      var stp = m_PolicyHost.Description.Behaviors.Find<ServiceDebugBehavior>();
      stp.HttpHelpPageEnabled = false;
      m_PolicyHost.Open();
    }
  }
}