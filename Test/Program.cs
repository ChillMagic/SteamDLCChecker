using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
	class Program
	{
		static void Main(string[] args) {
			var result = SteamInfo.SteamDB.GetAppInfoWithDLC("333600");
			Console.WriteLine(result.Key.ToString());
			if (result.Value != null) {
				foreach (var kv in result.Value) {
					Console.WriteLine(kv.Key + " " + kv.Value);
				}
			}
		}
	}
}
