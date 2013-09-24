using System.ComponentModel.Composition;
using System.IdentityModel.Selectors;
using System.ServiceModel;

namespace OpenResKit.ODataHost
{
  [Export]
  internal class CustomUserNameValidator: UserNamePasswordValidator
  {
    public override void Validate(string userName, string password)
    {
      if ("root" == userName && "ork123" == password)
      {
        return;
      }

      throw new FaultException("Unknown Username or Incorrect Password");
    }
  }
}