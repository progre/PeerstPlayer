﻿using System.Collections.Generic;
namespace Shule.Peerst.BBS
{
	/// <summary>
	/// NULL掲示板ストラテジクラス(NULLオブジェクトパターン)
	/// </summary>
	class NullBbsStrategy : IBbsStrategy
	{
		/// <summary>
		/// 掲示板書き込み
		/// </summary>
		/// <param name="name">名前</param>
		/// <param name="mail">メール欄</param>
		/// <param name="message">本文</param>
		public bool Write(string name, string mail, string message)
		{
			return true;
		}

		/// <summary>
		/// スレッド一覧
		/// </summary>
		public List<ThreadInfo> ThreadList { get { return new List<ThreadInfo>(); } }

		/// <summary>
		/// 掲示板名
		/// </summary>
		public string BbsName { get { return ""; } }

		/// <summary>
		/// 掲示板情報
		/// </summary>
		public BbsInfo BbsInfo { get { return new BbsInfo(BbsServer.UnSupport, "", "", ""); } }

		/// <summary>
		/// 指定スレッドの情報取得
		/// </summary>
		public ThreadInfo GetThreadInfo(string threadNo)
		{
			return new ThreadInfo("", threadNo, "");
		}

		/// <summary>
		/// スレッド読み込み
		/// </summary>
		public List<ResInfo> ReadThread(string threadNo, int resNo)
		{
			return new List<ResInfo>();
		}

		/// <summary>
		/// スレッド変更
		/// </summary>
		public void ChangeThread(string threadNo)
		{
		}
	}
}
