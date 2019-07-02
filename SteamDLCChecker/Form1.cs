using Microsoft.Win32;
using Newtonsoft.Json;
using SteamInfo;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace SteamDLCChecker
{
	public partial class Form1 : Form
	{
		public Form1() {
			InitializeComponent();
		}

		private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			System.Diagnostics.Process.Start("https://store.steampowered.com/dynamicstore/userdata");
		}

		private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			System.Diagnostics.Process.Start("https://github.com/ChillMagic/SteamDLCChecker/releases/latest");
		}

		Queue<string> taskList;
		Thread mainThread;
		bool isPause = false;
		GetAllDlcNotBuy getAllDlcNotBuy = null;
		Dictionary<string, List<AppInfo>> appDlcMap = new Dictionary<string, List<AppInfo>>();

		private void Button1_Click(object sender, EventArgs e) {
			if (taskList is null) {
				UserData userData = JsonConvert.DeserializeObject<UserData>(userInfo.Text);
				if (userData is null) {
					return;
				}
				getAllDlcNotBuy = new GetAllDlcNotBuy(userData);
				if (getAllDlcNotBuy.OwnedList.Count > 0) {
					progressBar.Value = 0;
					progressBar.Maximum = getAllDlcNotBuy.OwnedList.Count;
					taskList = new Queue<string>();
					foreach (var appid in getAllDlcNotBuy.OwnedList) {
						taskList.Enqueue(appid);
					}
					StartMainThread();
					this.button1.Text = "暂停";
				}
			}
			else {
				if (!isPause) { // Now doing
					mainThread.Abort();
					mainThread = null;
					isPause = true;
					this.button1.Text = "继续";
				}
				else { // Now pause
					StartMainThread();
					isPause = false;
					this.button1.Text = "暂停";
				}
			}
		}

		private void MainThread() {
			while (taskList != null && taskList.Count > 0) {
				string appid = taskList.Peek();
				try {
					List<AppInfo> dlcs = getAllDlcNotBuy.getNeedBuy(appid);
					if (dlcs.Count > 0) {
						appList.Invoke(new Action<string>((item) => { this.appList.Items.Add(item); }),
							appid + " " + getAllDlcNotBuy.appInfoMap[appid].Name);
						appDlcMap[appid] = dlcs;
					}
					this.progressBar.Invoke(new Action(() => { this.progressBar.Increment(1); }));
				}
				catch (ThreadAbortException) {
					return;
				}
				taskList.Dequeue();
				if (taskList.Count == 0) {
					taskList = null;
					this.button1.Text = "开始";
				}
			}
		}

		private void StartMainThread() {
			mainThread = new Thread(MainThread);
			mainThread.Start();
		}

		private void Form1_Load(object sender, EventArgs e) {
		}

		protected override void OnClosed(EventArgs e) {
			Environment.Exit(0);
		}

		private void AppList_SelectedIndexChanged(object sender, EventArgs e) {
			if (appList.SelectedItem != null) {
				string appid_with_name = (string)appList.SelectedItem;
				var match = Regex.Match(appid_with_name, @"(\d+) .*");
				if (match.Success) {
					string appid = match.Groups[1].ToString();
					dlcList.Items.Clear();
					if (appDlcMap.ContainsKey(appid)) {
						foreach (AppInfo dlc in appDlcMap[appid]) {
							dlcList.Items.Add(dlc.ID + " " + dlc.Name);
						}
					}
				}
			}
		}
	}
}
