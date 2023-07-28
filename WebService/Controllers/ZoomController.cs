using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebService.Controllers;

[ApiController]
public class ZoomController : ControllerBase
{
    /// <summary>
    /// Gets a token that's needed for Zoom Video SDK calls.
    /// </summary>
    [HttpGet]
    [Route("api/zoom/token")]
    public async Task<string> GetToken(string authToken, string topic)
    {
        if (string.IsNullOrEmpty(topic))
        {
            throw new ArgumentException("Topic cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(authToken))
        {
            throw new ArgumentException("Auth token cannot be null or empty.");
        }
        
        // Define const Key this should be private secret key stored in some safe place
        string key = await GetZoomSecretKeyAsync();

        // Create Security key  using private key above:
        // not that latest version of JWT using Microsoft namespace instead of System
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        // Also note that securityKey length should be >256b
        // so you have to make sure that your private key has a proper length
        var credentials = new SigningCredentials(securityKey, "HS256");

        //  Finally create a Token
        var header = new JwtHeader(credentials);

        var timeSpan = DateTime.UtcNow - DateTime.UnixEpoch;
        int secondsSinceEpoch = (int)timeSpan.TotalSeconds;
        
        //Some PayLoad that contain information about the  customer
        var payload = new JwtPayload
        {
            { "app_key", "pq8ApuGHRB46p2T4TVbe7UybFfxbBMJ8tylM"},
            { "tpc", topic },
            { "version", 1 },
            { "role_type", 0 }, // TODO: This is based on whether is a chair.
            { "user_identity", authToken }, // TODO: Get the authed user and use their ID. 
            { "iat", secondsSinceEpoch },
            { "exp", secondsSinceEpoch + (60 * 60 * 24) }
        };

        //
        var secToken = new JwtSecurityToken(header, payload);
        var handler = new JwtSecurityTokenHandler();

        // Token to String so you can use it in your client
        var tokenString = handler.WriteToken(secToken);

        Console.WriteLine(tokenString);
        Console.WriteLine("Consume Token");
        
        return JsonSerializer.Serialize(tokenString);
    }
    
    private async Task<string> GetZoomSecretKeyAsync()
    {
        const string secretName = @"zoomVideoSdkSecret";
        var secretClient = new SecretClient(new Uri("https://org-hub-key-vault.vault.azure.net/"), new DefaultAzureCredential());
        KeyVaultSecret secret = await secretClient.GetSecretAsync(secretName);
        return secret.Value;
    }
}