using Microsoft.AspNetCore.Mvc;
using MimeKit.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace WebApp.Controllers;

public class NonceController : Controller
{
	[HttpPost]
	public IActionResult GetNonceFromMiddleware()
	{
		var middleware = new NonceTimestampValidatorMiddleware(null);
		var nonce = middleware.GetNonce();
		return Json(new { nonce });
	}
}

public class NonceTimestampValidatorMiddleware
{
	private readonly RequestDelegate _next;
	private static readonly HashSet<string?>? NonceSet = new HashSet<string?>();

	public NonceTimestampValidatorMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		return;
	}

	//public async Task Invoke(HttpContext context)
	//{
	//	try
	//	{
	//		if (SecurityManagementController.context is null || SecurityManagementController.context.Count == 0)
	//		{
	//			SecurityManagementController.context?.Add(context);
	//		}

	//		if (context.Request.Path.Value != "" && context.Request.Path.Value != "/" && IsHighRiskEndpoint(context.Request.Path))
	//		{
	//			if (string.IsNullOrEmpty(context.Request.Headers["X-Nonce"]) || string.IsNullOrEmpty(context.Request.Headers["X-Timestamp"]))
	//			{
	//				context.Response.StatusCode = 404; // Bad Request
	//				await context.Response.WriteAsync("X-Nonce header is missing or empty");
	//				return;
	//			}

	//			// Retrieve nonce and timestamp from request headers
	//			string? nonce = context.Request.Headers["X-Nonce"];
	//			string? time = context.Request.Headers["X-Timestamp"];
	//			long timestamp = long.Parse(time);

	//			// Validate nonce and timestamp
	//			if (!IsValidNonce(nonce) || !IsValidTimestamp(timestamp))
	//			{
	//				context.Response.StatusCode = 404; // Unauthorized
	//				await context.Response.WriteAsync("Invalid nonce or timestamp \n" + context.Request.Path.Value + "\n" + context.Request.Path.Value);
	//				return;
	//			}

	//			string? hashedData = context.Request.Headers["X-WTF"];
	//			if (!string.IsNullOrEmpty(hashedData))
	//			{
	//				string[] hashedDataArray = hashedData.Split(',');
	//				hashedDataArray = hashedDataArray.Select(s => s.Trim()).ToArray();

	//				try
	//				{
	//					for (int i = 0; i < context.Request.Form.Count; i++)
	//					{
	//						if (DigitalSignature.VerifySignature(new StringBuilder(hashedDataArray[i]).Remove(19, i.ToString().Length).ToString(), "so Complicated"))
	//						{
	//							await _next(context);
	//							return;
	//						}

	//						var key = context.Request.Form.Keys.ElementAt(i);
	//						var val = "";
	//						if (string.Equals(context.Request.Form[key].GetType().Name, "StringValues", StringComparison.OrdinalIgnoreCase))
	//						{
	//							val = string.Join("--", context.Request.Form[key]);
	//						}
	//						else
	//						{
	//							val = context.Request.Form[key][0];
	//						}

	//						if (!DigitalSignature.VerifySignature(new StringBuilder(hashedDataArray[i]).Remove(19, i.ToString().Length).ToString(), val))
	//						{
	//							context.Response.StatusCode = 404; // Unauthorized
	//							await context.Response.WriteAsync("digital seg \n" + context.Request.Path.Value + "\n" + context.Request.Path.Value);
	//							return;
	//						}
	//					}
	//				}

	//				catch (Exception)
	//				{
	//					context.Response.StatusCode = 404; // Unauthorized
	//					await context.Response.WriteAsync("digital seg \n" + context.Request.Path.Value + "\n" + context.Request.Path.Value);
	//					return;
	//				}
	//			}
	//		}

	//		await _next(context);
	//	}
	//	catch (Exception e)
	//	{
	//		context.Response.StatusCode = 404; // Unauthorized
	//		await context.Response.WriteAsync(context.Request.Path.Value + "\n" + e.StackTrace);
	//	}
	//}

	private bool IsHighRiskEndpoint(PathString path)
	{
		string[] endPoints = {
			"Account/ResetUserNamePassword",
			"Account/update_password",
			"Account/resend_ResetUserNamePassword",
			"Account/saveStatusChanges",
			"Account/getStatesByCountry",
			"Account/checkAffiliation_AccountNotExist",
			"Account/remove_file_from_folder",
			"Account/GetFileNames",
			"Account/check_InstitutionhasDepartments",
			"Account/saveUserProfile",
			"Delete/DeleteAll",
			"Delete/DeletePHI",
			"Delete/DeleteExcel",
			"Download/DownloadExcel",
			"Download/GetButtons",
			"Download/DownloadSelected",
			"Download/DownloadSelectedAnonym",
			"Message/get_message_by_date",
			"Message/delete_message_by_id",
			"Message/delete_all_message",
			"UploadMri/MoveTojob",
			"UploadMri/AddExcelName",
			"UploadMri/ChangeExcelName",
		};

		return Array.Exists(endPoints, endpoint => path.Value.Contains(endpoint));
	}

	public bool checkNonce(string nonce, long timestamp)
	{
		return IsValidNonce(nonce) && IsValidTimestamp(timestamp);
	}

	private bool IsValidNonce(string? nonce)
	{
		// Verify the nonce received from the client
		if (NonceSet.Contains(nonce))
		{
			NonceSet.Remove(nonce);  // this line should not be committed
			return true;
		}

		else
		{
			return false;
		}
	}

	private bool IsValidTimestamp(long timestamp)
	{

		long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		long maxAllowedTimestampDelta = 60; // 30 sec
		return Math.Abs(currentTimestamp - timestamp) <= maxAllowedTimestampDelta;
	}

	private string? GenerateNonce()
	{
		// Generate a random nonce using cryptographic functions
		byte[] nonceBytes = new byte[16];
		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(nonceBytes);
		}
		return Convert.ToBase64String(nonceBytes);
	}

	public string? GetNonce()
	{
		// Generate and return a nonce
		var nonce = GenerateNonce();
		// Store the nonce (for verification later)
		NonceSet.Add(nonce);
		return nonce;
	}
}