using System.Security.Cryptography;
using InterfaceAdapters.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebService.Controllers;

[ApiController]
public class SignatureController : ControllerBase
{
    static readonly char[] padding = { '=' };
    
    [HttpGet]
    [Route("signature/token")]
    public async Task<string> Token()
    {
        //var token = getToken();
        var token =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhcHBLZXkiOiJhVGE0ZXRNWkYyVjFuZDFPbGRWbDVGSjZVMDRrQW5Ba2tvTzAiLCJzZGtLZXkiOiJhVGE0ZXRNWkYyVjFuZDFPbGRWbDVGSjZVMDRrQW5Ba2tvTzAiLCJtbiI6MzYxNjQ4ODU0OSwicm9sZSI6MCwiaWF0IjoxNjUxODYyNTExLCJleHAiOjE2NTE5NDg5MTEsInRva2VuRXhwIjoxNjUxOTQ4OTExfQ.ptoNij6DM62fPJEmgrARfvwn13casny0vtXc7hRew2I";
        return $"\"{token}\"";
    }
    
    private static string getToken () {
        Console.WriteLine ("Zoom copyright!");
        Console.WriteLine ("generate websdk token!");
        string apiKey = "7UWJinnmQqa4toSVGj8WTA";
        string apiSecret = "PeX0UYFVTaRIhhg3FCmjYEdKbsr1BfJydJhx";
        string meetingNumber = "3616488549";
        String ts = (ToTimestamp(DateTime.UtcNow.ToUniversalTime()) - 30000).ToString();
        string role = "1"; // 0 to join, 1 to start.
        string token = GenerateToken (apiKey, apiSecret, meetingNumber, ts, role);
        Console.WriteLine (token);
        return token;
    }
    
    private static long ToTimestamp (DateTime value) {
        long epoch = (value.Ticks - 621355968000000000) / 10000;
        return epoch;
    }
    
    private static string GenerateToken (string apiKey, string apiSecret, string meetingNumber, string ts, string role) {
        string message = String.Format ("{0}{1}{2}{3}", apiKey, meetingNumber, ts, role);
        apiSecret ??= "";
        var encoding = new System.Text.ASCIIEncoding ();
        byte[] keyByte = encoding.GetBytes (apiSecret);
        byte[] messageBytesTest = encoding.GetBytes (message);
        string msgHashPreHmac = System.Convert.ToBase64String (messageBytesTest);
        byte[] messageBytes = encoding.GetBytes (msgHashPreHmac);
        using (var hmacsha256 = new HMACSHA256 (keyByte)) {
            byte[] hashmessage = hmacsha256.ComputeHash (messageBytes);
            string msgHash = System.Convert.ToBase64String (hashmessage);
            string token = String.Format ("{0}.{1}.{2}.{3}.{4}", apiKey, meetingNumber, ts, role, msgHash);
            var tokenBytes = System.Text.Encoding.UTF8.GetBytes (token);
            return System.Convert.ToBase64String (tokenBytes).TrimEnd (padding);
        }
    }
}