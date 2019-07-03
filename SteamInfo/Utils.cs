using System;
using System.IO;
using System.Net;

namespace SteamInfo
{
	class Utils
	{
		public static string GetContext(string url, int tryCount = 0, Exception e = null) {
			if (tryCount > 3) {
				Console.WriteLine("Get \"" + url + "\" error.");
				throw e;
			}
			string context = "";
			try {
				System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // TLS 1.2
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
				req.Timeout = 10000;
				HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
				Stream stream = resp.GetResponseStream();

				try {
					using (StreamReader reader = new StreamReader(stream)) {
						context = reader.ReadToEnd();
					}
				}
				finally {
					stream.Close();
				}
			}
			catch (Exception ex) {
				return GetContext(url, tryCount + 1, ex);
			}

			return context;
		}
	}
}
