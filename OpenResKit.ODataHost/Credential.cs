using FileHelpers;

namespace OpenResKit.ODataHost
{
  [DelimitedRecord(";")]
  internal class Credential
  {
    public string Password;
    public string User;
  }
}