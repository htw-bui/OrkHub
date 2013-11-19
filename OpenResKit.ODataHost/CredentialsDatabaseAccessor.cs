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
    public void Create(string user, string password)
    {
      if (Credentials.Any(c => c.User == user))
      {
        throw new InvalidOperationException("The given user already exists.");
      }
      var passwordHash = PasswordHash.CreateHash(password);
      m_Credentials.Add(new Credential()
                        {
                          Password = passwordHash,
                          User = user
                        });
      SaveCredentials();
    }

    [OperationContract]
    public void UpdateUser(string oldUser, string newUser)
    {
      var existingUser = Credentials.SingleOrDefault(c => c.User == oldUser);
      if (existingUser == null)
      {
        throw new InvalidOperationException("The given user does not exist.");
      }
      m_Credentials.Remove(existingUser);
      m_Credentials.Add(new Credential()
                        {
                          Password = existingUser.Password,
                          User = newUser
                        });

      SaveCredentials();
    }

    [OperationContract]
    public void UpdatePassword(string user, string password)
    {
      var existingUser = Credentials.SingleOrDefault(c => c.User == user);
      var passwordHash = PasswordHash.CreateHash(password);
      if (existingUser == null)
      {
        throw new InvalidOperationException("The given user does not exist.");
      }

      existingUser.Password = passwordHash;
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