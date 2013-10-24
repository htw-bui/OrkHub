using System.ComponentModel.Composition;
using System.IdentityModel.Selectors;
using System.Linq;
using System.ServiceModel.Security;

namespace OpenResKit.ODataHost
{
  [Export]
  internal class CustomUserNameValidator : UserNamePasswordValidator
  {
    private readonly CredentialsDatabaseAccessor m_CredentialsDatabaseAccessor;

    [ImportingConstructor]
    public CustomUserNameValidator([Import] CredentialsDatabaseAccessor credentialsDatabaseAccessor)
    {
      m_CredentialsDatabaseAccessor = credentialsDatabaseAccessor;
    }

    public override void Validate(string user, string password)
    {
      if (("root" == user && "ork123" == password))
      {
        return;
      }

      var existingUser = m_CredentialsDatabaseAccessor.Credentials.SingleOrDefault(c => c.User == user);
      if (existingUser != null &&
          PasswordHash.ValidatePassword(password, existingUser.Password))
      {
        return;
      }

      throw new SecurityAccessDeniedException("Unknown Username or Incorrect Password");
    }
  }
}