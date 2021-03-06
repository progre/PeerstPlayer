﻿using AxShockwaveFlashObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace PeerstPlayer.Controls.MoviePlayer
{
	class FlashMoviePlayerManager
	{
		private AxShockwaveFlash flash;

		private const string PlayVideoMethod = "PlayVideo";
		private const string ChangeVolumeMethod = "ChangeVolume";
		private const string GetVideoWidthMethod = "GetVideoWidth";
		private const string GetVideoHeightMethod = "GetVideoHeight";
		private const string GetDurationStringMethod = "GetDurationString";
		private const string GetNowFrameRateMethod = "GetNowFrameRate";
		private const string GetFrameRateMethod = "GetFrameRate";
		private const string GetNowBitRateMethod = "GetNowBitRate";
		private const string GetBitRateMethod = "GetBitRate";

		public FlashMoviePlayerManager(AxShockwaveFlash flash)
		{
			this.flash = flash;

			flash.FlashCall += ExternalCall;
		}

		/// <summary>
		/// Flashから呼び出し
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExternalCall(object sender, _IShockwaveFlashEvents_FlashCallEvent e)
		{
			var doc = new XmlDocument();
			doc.Load(new StringReader(e.request));
			var methodName = doc.SelectSingleNode("invoke").Attributes["name"].Value;
			var nodes = doc.SelectSingleNode("invoke/arguments").ChildNodes;
			var args = new List<string>();
			foreach (XmlElement arg in nodes)
			{
				args.Add(arg.InnerText);
			}

			switch (methodName)
			{
				case "OpenStateChange":
					openStateChange(flash, new EventArgs());
					break;
			}
		}

		public event EventHandler OpenStateChange
		{
			add { openStateChange += value; }
			remove { openStateChange -= value; }
		}
		event EventHandler openStateChange = delegate { };

		/// <summary>
		/// 動画再生
		/// </summary>
		/// <param name="streamUrl">ストリームURL</param>
		public void PlayVideo(string streamUrl)
		{
			CallFlashMethod(PlayVideoMethod, streamUrl);
		}

		/// <summary>
		/// 音量変更
		/// </summary>
		/// <param name="volume">音量</param>
		public void ChangeVolume(int volume)
		{
			CallFlashMethod(ChangeVolumeMethod, volume.ToString());
		}

		/// <summary>
		/// 縦サイズの取得
		/// </summary>
		public int GetVideoWidth()
		{
			int result;
			int.TryParse(CallFlashMethod(GetVideoWidthMethod), out result);
			return result;
		}

		/// <summary>
		/// 横サイズの取得
		/// </summary>
		public int GetVideoHeight()
		{
			int result;
			int.TryParse(CallFlashMethod(GetVideoHeightMethod), out result);
			return result;
		}

		/// <summary>
		/// 再生時間を取得
		/// </summary>
		/// <returns></returns>
		public string GetDurationString()
		{
			return CallFlashMethod(GetDurationStringMethod);
		}

		/// <summary>
		/// 現在のフレームレートを取得
		/// </summary>
		public int GetNowFrameRate()
		{
			int result;
			int.TryParse(CallFlashMethod(GetNowFrameRateMethod), out result);
			return result;
		}

		/// <summary>
		/// フレームレートを取得
		/// </summary>
		/// <returns></returns>
		public int GetFrameRate()
		{
			int result;
			int.TryParse(CallFlashMethod(GetFrameRateMethod), out result);
			return result;
		}

		/// <summary>
		/// 現在のビットレートを取得
		/// </summary>
		/// <returns></returns>
		public int GetNowBitRate()
		{
			int result;
			int.TryParse(CallFlashMethod(GetNowBitRateMethod), out result);
			return result;
		}

		/// <summary>
		/// ビットレートを取得
		/// </summary>
		/// <returns></returns>
		public int GetBitRate()
		{
			int result;
			int.TryParse(CallFlashMethod(GetBitRateMethod), out result);
			return result;
		}

		/// <summary>
		/// Flashの関数を実行する
		/// </summary>
		/// <param name="methodName">メソッド名</param>
		/// <param name="param">引数</param>
		private void CallFlashMethod(string methodName, string param)
		{
			if (flash.FrameLoaded(0))
			{
				flash.CallFunction("<invoke name=\"" + methodName + "\" returntype=\"xml\"><arguments><string>" + param + "</string></arguments></invoke>");
			}
		}

		/// <summary>
		/// Flashの関数を実行する
		/// </summary>
		/// <param name="methodName">メソッド名</param>
		/// <param name="parameters">引数</param>
		private string CallFlashMethod(string methodName, params object[] parameters)
		{
			if (flash.FrameLoaded(0))
			{
				var request = "<invoke name=\"" + methodName + "\" returntype=\"xml\"><arguments>";
				foreach (var param in parameters)
				{
					request += string.Format("<string>{0}</string>", param.ToString());
				}
				request += "</arguments></invoke>";

				return CleanStringTag(flash.CallFunction(request));
			}
			return "";
		}

		private string CleanStringTag(string text)
		{
			return text.Replace("<string>", "").Replace("</string>", "");
		}
	}
}
