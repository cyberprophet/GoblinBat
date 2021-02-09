﻿using System.IO;

using AxKHOpenAPILib;

namespace ShareInvest.OpenAPI.Catalog
{
	class 주식체결 : Real
	{
		internal override AxKHOpenAPI API
		{
			get; set;
		}
		internal override StreamWriter Server
		{
			get; set;
		}
		internal override void OnReceiveRealData(_DKHOpenAPIEvents_OnReceiveRealDataEvent e)
		{
			string time = API.GetCommRealData(e.sRealKey, Fid[0]), current = API.GetCommRealData(e.sRealKey, Fid[1]), volume = API.GetCommRealData(e.sRealKey, Fid[6]);

			if (string.IsNullOrEmpty(volume) is false && string.IsNullOrEmpty(current) is false && string.IsNullOrEmpty(time) is false)
			{
				if (Connect.GetInstance().StocksHeld.TryGetValue(e.sRealKey, out Analysis analysis))
				{
					string str_bid = API.GetCommRealData(e.sRealKey, Fid[5]), str_offer = API.GetCommRealData(e.sRealKey, Fid[4]);

					if (int.TryParse(str_offer[0] is '-' ? str_offer[1..] : str_offer, out int offer) && int.TryParse(str_bid[0] is '-' ? str_bid[1..] : str_bid, out int bid))
					{
						analysis.Bid = bid;
						analysis.Offer = offer;
						analysis.OnReceiveEvent(time, current, volume);
					}
				}
				if (Lite)
					Server.WriteLine(string.Concat(e.sRealType, '|', e.sRealKey, '|', time, ';', current, ';', volume));
			}
		}
		internal override bool Lite
		{
			get; set;
		}
		protected internal override int[] Fid => new int[] { 20, 10, 11, 12, 27, 28, 15, 13, 14, 16, 17, 18, 25, 26, 29, 30, 31, 32, 228, 311, 290, 691, 567, 568 };
	}
}