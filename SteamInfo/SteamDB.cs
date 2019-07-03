using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SteamInfo
{
	public class SteamDB
	{
		static string GetURL() {
			return "https://steamdb.info";
		}
		static string GetAppURL(string appid) {
			return GetURL() + "/app" + String.Format("/{0}", appid);
		}
		static string GetAppDLCURL(string appid) {
			return GetAppURL(appid) + "/dlc";
		}

		public static KeyValuePair<AppInfo, AppInfoMap> GetAppInfoWithDLC(string appid) {
			string url = GetAppDLCURL(appid);
			string context = Utils.GetContext(url);

			bool matchMode = false;
			AppInfo currentApp = null;
			AppInfo thisApp = new AppInfo { ID = appid, DLCs = new List<string>(), InStore = true, };
			int gettingPrice = 0; // 0: starting, 1~2: doing, 3: done

			AppInfoMap dlcMap = new AppInfoMap();

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
						else if (matchString == "Demo")
							thisApp.Type = AppType.Demo;
						else
							thisApp.Type = AppType.Unknown;

						if (thisApp.Type == AppType.DLC) {
							return new KeyValuePair<AppInfo, AppInfoMap>(thisApp, null);
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
					dlcMap[currentID] = currentApp;
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

			return new KeyValuePair<AppInfo, AppInfoMap>(thisApp, dlcMap);
		}
	}
}
