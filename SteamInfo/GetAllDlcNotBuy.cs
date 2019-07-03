using System;
using System.Collections.Generic;
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

		static AppInfoMap GetAppInfoBase(string appid) {
			var info = SteamDB.GetAppInfoWithDLC(appid);
			AppInfoMap result = info.Value;
			result[info.Key.ID] = info.Key;
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
