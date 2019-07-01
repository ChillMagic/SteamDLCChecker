using System.Collections.Generic;

namespace SteamInfo
{
	public class UserData
	{
		public List<string> rgWishlist;
		public List<string> rgOwnedPackages;
		public List<string> rgOwnedApps;
		public List<string> rgPackagesInCart;
		public List<string> rgAppsInCart;
		public List<Dictionary<string, string>> rgRecommendedTags;
		public Dictionary<string, string> rgIgnoredApps;
		public List<string> rgIgnoredPackages;
		public List<string> rgCurators;
		public List<string> rgCurations;
		public List<string> rgCreatorsFollowed;
		public List<string> rgCreatorsIgnored;
		public List<string> rgExcludedTags;
		public List<string> rgExcludedContentDescriptorIDs;
		public List<string> rgAutoGrantApps;
		public int nRemainingCartDiscount;
		public int nTotalCartDiscount;
	}
}
