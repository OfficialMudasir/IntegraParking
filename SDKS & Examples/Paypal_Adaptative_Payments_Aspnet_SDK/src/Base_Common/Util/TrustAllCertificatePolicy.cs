
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace PayPal.Platform.SDK
{
	/// <summary>
	/// This class is used to provide custom security certificate validation for an application
	/// </summary>
	public class TrustAllCertificatePolicy : ICertificatePolicy
	{
		private TrustAllCertificatePolicy()
		{
		}

		/// <summary>
		/// Public instance of this singleton.
		/// </summary>
		public static readonly TrustAllCertificatePolicy Instance = new TrustAllCertificatePolicy();

		/// <summary>
		/// Returns true
		/// </summary>
		/// <param name="sp">The ServicePoint that will use the certificate. </param>
		/// <param name="cert">The certificate to validate</param>
		/// <param name="request">The request that received the certificate.</param>
		/// <param name="problem">The request that received the certificate.</param>
		/// <returns>For this SDK it will always return TRUE</returns>
		public bool CheckValidationResult(ServicePoint sp, X509Certificate cert, WebRequest request, int problem)
		{        
			return true; 
		}    
	}
}
