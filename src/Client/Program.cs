using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Client {
  class Program {
    static async Task Main (string[] args) {
      var client = new HttpClient ();
      var disco = await client.GetDiscoveryDocumentAsync ("https://localhost:5001");
      if (disco.IsError) {
        Console.WriteLine (disco.Error);
        return;
      }

      var tokenResponse = await client.RequestClientCredentialsTokenAsync (new ClientCredentialsTokenRequest {
        Address = disco.TokenEndpoint,
          ClientId = "client",
          ClientSecret = "secret",
          Scope = "api1"
      });

      if (tokenResponse.IsError) {
        Console.WriteLine (tokenResponse.Error);
        return;
      }

      Console.WriteLine (tokenResponse.Json);
      Console.WriteLine ("\n\n");
    }
  }
}