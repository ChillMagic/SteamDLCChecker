using System.Collections.Generic;

namespace SteamInfo
{
	public enum AppType
	{
		Unknown,
		Game,
		DLC,
		Video,
		Demo,
	}
	public class AppInfo
	{
		public string ID;
		public string Name;
		public bool InStore;
		public AppType Type;
		public List<string> DLCs;
		public bool IsFree;

		public override string ToString() {
			string result = string.Format("AppInfo({0}, {1}, \"{2}\", InStore={3}", Type.ToString(), ID, Name, InStore);
			if (Type == AppType.Game) {
				result += ", DLCs=[";
				if (DLCs != null) {
					foreach (string DLC in DLCs) {
						result += DLC + ", ";
					}
					result = result.Substring(0, result.Length - 2);
				}
				result += "]";
			}
			if (IsFree) {
				result += ", Free";
			}
			result += ")";
			return result;
		}
	}

	public class AppInfoMap : Dictionary<string, AppInfo> { }
}
