using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mpesaIntegration.Models;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace mpesaIntegration.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;
  private readonly IHttpClientFactory _clientFactory;

  public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
  {
    _logger = logger;
    _clientFactory = clientFactory;
  }

  public IActionResult Index()
  {
    return View();
  }
  // Generating mpesa access token  
  public async Task<string> GetToken()
  {
    var client = _clientFactory.CreateClient("mpesa");
    var authString = "kdTvGClNib3fDpLKy2AkRSVRhg3G80bQ:LEv7QWPHAC0C8Tus";
    var encodedString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authString));
    var _url = "/oauth/v1/generate?grant_type=client_credentials";

    var request = new HttpRequestMessage(HttpMethod.Get, _url);
    request.Headers.Add("Authorization", $"Basic {encodedString}");

    var response = await client.SendAsync(request);
    var mpesaResponse = await response.Content.ReadAsStringAsync();

    Token tokenObject = JsonConvert.DeserializeObject<Token>(mpesaResponse);

    return tokenObject.access_token;

  }

  class Token
  {
    public string access_token { get; set; }
    public string expires_in { get; set; }
  }

  // register url
  public IActionResult RegisterURLS()
  {
    return View();
  }


  [HttpGet]
  [Route("register-urls")]
  public async Task<JsonResult> RegisterMpesaUrls()
  {
    var jsonBody = JsonConvert.SerializeObject(new
    {
      ValidationURL = "https://mydemo-url.com/validation",
      ConfirmationURL = "https://mydemo-url.com/confirmation",
      ResponseType = "Completed",
      Shortcode = 600983

    });
  }




  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
