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

using System.ComponentModel.Composition;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace OpenResKit.ODataHost
{
  [Export]
  [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single)]
  [ServiceContract]
  internal class PolicyRetriever
  {
    [OperationContract]
    [WebGet(UriTemplate = "/clientaccesspolicy.xml")]
    public Stream GetPolicy()
    {
      const string result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<access-policy>
		<cross-domain-access>
				<policy>
						<allow-from http-request-headers=""*"">
								<domain uri=""*""/>
						</allow-from>
						<grant-to>
								<resource path=""/"" include-subpaths=""true""/>
						</grant-to>
				</policy>
		</cross-domain-access>
</access-policy>";
      WebOperationContext.Current.OutgoingResponse.ContentType = "application/xml";
      return new MemoryStream(Encoding.UTF8.GetBytes(result));
    }
  }
}