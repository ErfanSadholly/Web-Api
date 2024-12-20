﻿namespace Web_Api.Jwt
{
	public class JwtModel
	{
		public string SecretKey { get; set; } = string.Empty;
		public string Issuer { get; set; } = string.Empty;
		public string Audience { get; set; } = string.Empty;
		public int ExpirationInMinutes { get; set; }
	}
}
