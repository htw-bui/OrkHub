using System;
using OpenResKitHub;

namespace OpenResKitHubConsole
{
  internal static class Program
  {
    private static void Main(string[] args)
    {
      new OrkHubService().StartFromConsole(args);
      Console.WriteLine("Services are up and running.");
      Console.ReadKey();
    }
  }
}