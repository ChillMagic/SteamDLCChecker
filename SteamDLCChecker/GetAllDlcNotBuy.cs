using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace SteamInfo
{
	public class GetAllDlcNotBuy
	{
		List<string> ownedList;
		public AppInfoMap appInfoMap;

		public List<string> OwnedList { get { return ownedList; } }

		public GetAllDlcNotBuy(UserData userData) {
			this.ownedList = userData.rgOwnedApps;
			this.appInfoMap = new AppInfoMap();
		}

		public void start() {
			int count = 0;
			ownedList = ownedList.GetRange(count, ownedList.Count - count);
			foreach (string appid in ownedList) {
				Console.WriteLine("Getting(" + count + ") " + appid);
				++count;

				var needBuy = getNeedBuy(appid);

				if (needBuy.Count > 0) {
					Console.WriteLine(appInfoMap[appid].Name);
					Console.WriteLine("Need Buy:");
					foreach (AppInfo app in needBuy) {
						Console.WriteLine(app);
					}
				}
			}
		}

		public List<AppInfo> getNeedBuy(string appid) {
			List<string> dlcs = GetDlcInfo(appid, appInfoMap);
			List<AppInfo> needBuy = new List<AppInfo>();
			foreach (string dlc in dlcs) {
				if (!ownedList.Contains(dlc)) {
					needBuy.Add(appInfoMap[dlc]);
				}
			}
			if (0 < needBuy.Count && needBuy.Count <= 3) {
				List<AppInfo> newNeedBuy = new List<AppInfo>();
				foreach (AppInfo app in needBuy) {
					if (!IsFree(app.ID)) {
						newNeedBuy.Add(app);
					}
				}
				needBuy = newNeedBuy;
			}
			return needBuy;
		}

		static string GetContext(string url, int tryCount = 0, Exception e = null) {
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

		static AppInfoMap GetAppInfoBase(string appid) {
			string url = "https://steamdb.info/app/" + appid + "/dlc";
			string context = GetContext(url);

			bool matchMode = false;
			AppInfo currentApp = null;
			AppInfo thisApp = new AppInfo { ID = appid, DLCs = new List<string>(), InStore = true, };
			int gettingPrice = 0; // 0: starting, 1~2: doing, 3: done

			AppInfoMap result = new AppInfoMap();

			result[appid] = thisApp;

			foreach (string line in context.Split('\n')) {
				if (gettingPrice != 3) {
					if (gettingPrice == 0) {
						if (Regex.IsMatch(line, "<span class=\"panel-ownership todo-rm-style\"")) {
							gettingPrice = 1;
						}
					}
					else if (gettingPrice == 1) {
						gettingPrice = 2;
					}
					else if (gettingPrice == 2) {
						string[] list = line.Split(' ');
						if (list[list.Length - 1] == "Free") {
							thisApp.IsFree = true;
						}
						gettingPrice = 3;
					}
				}
				if (thisApp.Type is AppType.Unknown) {
					Match matchType = Regex.Match(line, "<td itemprop=\"applicationCategory\">(.*)</td>");
					if (matchType.Success) {
						string matchString = matchType.Groups[1].ToString();
						if (matchString == "Game")
							thisApp.Type = AppType.Game;
						else if (matchString == "Downloadable Content")
							thisApp.Type = AppType.DLC;
						else if (matchString == "Video")
							thisApp.Type = AppType.Video;
						else
							thisApp.Type = AppType.Unknown;

						if (thisApp.Type == AppType.DLC) {
							return result;
						}
					}
				}
				else if (thisApp.Name is null) {
					Match matchName = Regex.Match(line, "<td itemprop=\"name\">(.*)</td>");
					if (matchName.Success) {
						string matchString = matchName.Groups[1].ToString();
						thisApp.Name = matchString;
					}
				}

				Match match = Regex.Match(line, "<tr class=\"app\" data-appid=\"(\\d+)\">");
				if (match.Success) {
					matchMode = true;
					string currentID = match.Groups[1].ToString();
					currentApp = new AppInfo { ID = currentID, Name = "", InStore = false, Type = AppType.DLC };
					result[currentID] = currentApp;
					thisApp.DLCs.Add(currentID);
				}

				if (matchMode) {
					if (line.Length >= 4 && line.Substring(0, 4) == "<svg") {
						currentApp.InStore = true;
						continue;
					}
					if (line.Length >= 1 && line.Substring(0, 1) != "<") {
						currentApp.Name = line;
						continue;
					}
				}

				if (Regex.IsMatch(line, "</tr>")) {
					matchMode = false;
				}
			}

			return result;
		}

		static AppInfo GetAppInfo(string appid, AppInfoMap appInfoMap) {
			if (!appInfoMap.ContainsKey(appid)) {
				AppInfoMap r = GetAppInfoBase(appid);
				foreach (var kv in r) {
					if (appInfoMap.ContainsKey(kv.Key)) {
						Console.WriteLine(kv.Key);
						Console.WriteLine(kv.Value);
						Console.WriteLine(appInfoMap[kv.Key]);
					}
					else {
						appInfoMap[kv.Key] = kv.Value;
					}
				}
			}
			return appInfoMap[appid];
		}

		static bool IsFree(string appid) {
			return GetAppInfoBase(appid)[appid].IsFree;
		}

		public static List<string> GetDlcInfo(string appid, AppInfoMap appInfoMap) {
			List<string> result = new List<string>();
			AppInfo appInfo = GetAppInfo(appid, appInfoMap);

			if (appInfo.Type == AppType.Game) {
				foreach (string appidx in appInfo.DLCs) {
					if (appInfoMap[appidx].InStore) {
						result.Add(appidx);
					}
				}
			}

			return result;
		}
	}
}
