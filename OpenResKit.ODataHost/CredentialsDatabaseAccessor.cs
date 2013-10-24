using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.ServiceModel;
using FileHelpers;

namespace OpenResKit.ODataHost
{
  [ExportMetadata("Name", "CredentialsManagement")]
  [Export(typeof (IWCFService))]
  [Export]
  [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single)]
  [ServiceContract]
  internal class CredentialsDatabaseAccessor : IWCFService
  {
    private const string AuthFileName = "/users.csv";
    private readonly string m_AuthFile;
    private readonly List<Credential> m_Credentials = new List<Credential>();
    private readonly FileHelperEngine<Credential> m_Engine = new FileHelperEngine<Credential>();

    public CredentialsDatabaseAccessor()
    {
      m_AuthFile = Environment.CurrentDirectory + AuthFileName;
      if (File.Exists(m_AuthFile))
      {
        m_Credentials.AddRange(m_Engine.ReadFile(m_AuthFile));
      }
    }

    public IEnumerable<Credential> Credentials
    {
      get { return m_Credentials; }
    }

    [OperationContract]
    public void CreateOrUpdate(string user, string password)
    {
      var existingUser = Credentials.SingleOrDefault(c => c.User == user);
      var passwordHash = PasswordHash.CreateHash(password);
      if (existingUser != null)
      {
        existingUser.Password = passwordHash;
      }
      else
      {
        m_Credentials.Add(new Credential()
                          {
                            Password = passwordHash,
                            User = user
                          });
      }
      SaveCredentials();
    }

    private void SaveCredentials()
    {
      m_Engine.WriteFile(m_AuthFile, Credentials);
    }

    [OperationContract]
    public void Delete(string user)
    {
      var existingUser = Credentials.SingleOrDefault(c => c.User == user);
      if (existingUser != null)
      {
        m_Credentials.Remove(existingUser);
      }
      else
      {
        throw new KeyNotFoundException("User to delete not found!");
      }
      SaveCredentials();
    }
  }
}