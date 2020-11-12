﻿using System;
using System.Text;

using ShareInvest.Catalog.Models;

namespace ShareInvest.EventHandler
{
    public class SendSecuritiesAPI : EventArgs
    {
        public SendSecuritiesAPI(short error) => Convey = error;
        public SendSecuritiesAPI(Codes codes) => Convey = codes;
        public SendSecuritiesAPI(Tuple<string, string> tuple) => Convey = tuple;
        public SendSecuritiesAPI(Tuple<string, string, string> operation) => Convey = operation;
        public SendSecuritiesAPI(string message) => Convey = message;
        public SendSecuritiesAPI(string name, string[] param) => Convey = new Tuple<string, string[]>(name, param);
        public SendSecuritiesAPI(string code, StringBuilder sb) => Convey = new Tuple<string, StringBuilder>(code, sb);
        public SendSecuritiesAPI(string code, int offer, int bid) => Convey = new Tuple<string, int, int>(code, offer, bid);
        public SendSecuritiesAPI(string code, int price, int offer, int bid) => Convey = new Tuple<string, int, int, int>(code, price, offer, bid);
        public SendSecuritiesAPI(string code, double price, double offer, double bid) => Convey = new Tuple<string, double, double, double>(code, price, offer, bid);
        public SendSecuritiesAPI(string code, string time, string price, string volume) => Convey = new Tuple<string, string, string, string>(code, time, price, volume);
        public SendSecuritiesAPI(string code, string name, string retention, string price, int market) => Convey = new Tuple<string, string, string, string, int>(code, name, retention, price, market);
        public object Convey
        {
            get; private set;
        }
    }
}