#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.Indicators;
using NinjaTrader.NinjaScript.DrawingTools;

using System.Text.RegularExpressions;
using System.Collections;				
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class TF_Lunar : Indicator
	{
		int firstDate = 99;
		DateTime oldNow = new DateTime();
		
		public  List<DateTime> _newMoon;
		public  List<DateTime> _firstQrt;
		public  List<DateTime> _fullMoon;
		public  List<DateTime> _thrdQrtr;
		
		public TF_Lunar()	
		{
			_newMoon=new List<DateTime>();
			NewMoonDates();
			_firstQrt=new List<DateTime>();
			firstQrtrDates();
			_fullMoon=new List<DateTime>();
			FullMoonDates();
			_thrdQrtr=new List<DateTime>();
			ThrdQrtrDates();
		}
		
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Indicator here.";
				Name										= "TF_Lunar";
				Calculate									= Calculate.OnEachTick;
				IsOverlay									= false;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= true;
				DrawVerticalGridLines						= true;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;
				FullMoon					= Brushes.White;
				NewMoon					= Brushes.Fuchsia;
				Q1					= Brushes.DarkGoldenrod;
				Q3					= Brushes.DarkMagenta;
				DistanceFromBars					= 8.6;
			}
			else if (State == State.Configure)
			{
				_newMoon=new List<DateTime>();
				NewMoonDates();
				_firstQrt=new List<DateTime>();
				firstQrtrDates();
				_fullMoon=new List<DateTime>();
				FullMoonDates();
				_thrdQrtr=new List<DateTime>();
				ThrdQrtrDates();
			}
		}

		protected override void OnBarUpdate()
		{
				
			DateTime nearestMoon = NearestMoon();
            String nearestPhase = NearestMoonPhase(nearestMoon);
			Duration(nearestMoon, nearestPhase);
			
			if (BarsPeriod.BarsPeriodType == BarsPeriodType.Day) {
				if (oldNow.Date < DateTime.Now.Date){
					oldNow = DateTime.Now;
					DailyBars();	
				}
				
				
			}else{
				
				if (oldNow.Date < DateTime.Now.Date){
					List<DateTime> _FM = new List<DateTime>();
					List<DateTime> _NM = new List<DateTime>();
					List<DateTime> _Q1 = new List<DateTime>();
					List<DateTime> _Q3 = new List<DateTime>();
					for (int i=0; i<_fullMoon.Count(); i++){
						if (Time[0] <= _fullMoon[i].ToLocalTime()  && DateTime.Now > _fullMoon[i].ToLocalTime()){
							_FM.Add(_fullMoon[i].ToLocalTime().AddDays(-3));	
						}
					}
					for (int i=0; i<_newMoon.Count(); i++){
						if (Time[0] <= _newMoon[i].ToLocalTime() && DateTime.Now > _newMoon[i].ToLocalTime()){
							_NM.Add(_newMoon[i].ToLocalTime().AddDays(-3));	
						}
					}
					for (int i=0; i<_firstQrt.Count(); i++){
						if (Time[0] <= _firstQrt[i].ToLocalTime()  && DateTime.Now > _firstQrt[i].ToLocalTime()){
							_Q1.Add(_firstQrt[i].ToLocalTime().AddDays(-3));	
						}
					}
					for (int i=0; i<_thrdQrtr.Count(); i++){
						if (Time[0] <= _thrdQrtr[i].ToLocalTime()  && DateTime.Now > _thrdQrtr[i].ToLocalTime()){
							_Q3.Add(_thrdQrtr[i].ToLocalTime().AddDays(-3));	
						}
					}
					if (_FM.Count() > 0 ){
						for(int i=0; i<_FM.Count(); i++){
							DrawTextAndDot(_FM[i], FullMoon, "FM");
						}
					}
					
					if (_NM.Count() > 0 ){
						for(int i=0; i<_NM.Count(); i++){
							DrawTextAndDot(_NM[i], NewMoon, "NM");
						}
					}
					if (_Q1.Count() > 0 ){
						for(int i=0; i<_Q1.Count(); i++){
							DrawTextAndDot(_Q1[i], Q1, "Q1");
						}
					}
					if (_Q3.Count() > 0 ){
						for(int i=0; i< _Q3.Count(); i++){
							DrawTextAndDot(_Q3[i], Q3, "Q3");
						}
					}	
				}
				
			}
			
		}
		
		private void DailyBars(){
			List<DateTime> _FM = new List<DateTime>();
			List<DateTime> _NM = new List<DateTime>();
			List<DateTime> _Q1 = new List<DateTime>();
			List<DateTime> _Q3 = new List<DateTime>();
			for (int i=0; i<_fullMoon.Count(); i++){
				if (Time[0] <= _fullMoon[i].ToLocalTime()  && DateTime.Now > _fullMoon[i].ToLocalTime()){
					_FM.Add(_fullMoon[i].ToLocalTime().AddDays(-3));	
				}
			}
			for (int i=0; i<_newMoon.Count(); i++){
				if (Time[0] <= _newMoon[i].ToLocalTime() && DateTime.Now > _newMoon[i].ToLocalTime()){
					_NM.Add(_newMoon[i].ToLocalTime().AddDays(-3));	
				}
			}
			for (int i=0; i<_firstQrt.Count(); i++){
				if (Time[0] <= _firstQrt[i].ToLocalTime()  && DateTime.Now > _firstQrt[i].ToLocalTime()){
					_Q1.Add(_firstQrt[i].ToLocalTime().AddDays(-3));	
				}
			}
			for (int i=0; i<_thrdQrtr.Count(); i++){
				if (Time[0] <= _thrdQrtr[i].ToLocalTime()  && DateTime.Now > _thrdQrtr[i].ToLocalTime()){
					_Q3.Add(_thrdQrtr[i].ToLocalTime().AddDays(-3));	
				}
			}
			if (_FM.Count() > 0 ){
				for(int i=0; i<_FM.Count(); i++){
					DrawDaily(_FM[i], FullMoon, "FM");
				}
			}
			
			if (_NM.Count() > 0 ){
				for(int i=0; i<_NM.Count(); i++){
					DrawDaily(_NM[i], NewMoon, "NM");
				}
			}
			if (_Q1.Count() > 0 ){
				for(int i=0; i<_Q1.Count(); i++){
					DrawDaily(_Q1[i], Q1, "Q1");
				}
			}
			if (_Q3.Count() > 0 ){
				for(int i=0; i< _Q3.Count(); i++){
					DrawDaily(_Q3[i], Q3, "Q3");
				}
			}
		}
		
		private void Duration(DateTime dt, String phase){
			DateTime now = DateTime.Now;
			TimeSpan diff = dt - now;
			double days = (dt-now).TotalDays;
			days = Math.Floor(days);
			string a = diff.ToString(@"hh\:mm");
			
			if (days > 0){
				a = days.ToString() + " days and " + a;	
			}
			
//			string str = "The nearest " + phase + " : " + a;
//			string str = "Next phase - " + phase + " " + dt.ToString("dd/MM/yyyy") + " @ " + dt.ToString(@"hh\:mm tt");
			switch(phase){
				case "FM":
					phase = "Full Moon";
					break;
				case "NM":
					phase = "New Moon";
					break;
				case "Q1":
					phase = "Q1";
					break;
				case "Q3":
					phase = "Q3";
					break;
			}
			
			string str = a + " until " + phase;
				
			Draw.TextFixed(this, dt.ToString("MM/dd/yyyy h:mm tt") + " right", str, TextPosition.TopRight, Brushes.RoyalBlue, 
				new SimpleFont("Arial", 15){Bold = true}, Brushes.Transparent, Brushes.Transparent, 100);
				
		}
		
		private String CheckIfFull(DateTime dt){
			String isFull = "";
			DateTime dt1 = dt.AddDays(1);
			for (int i=0; i < _fullMoon.Count(); i++){
				if (dt.Date == _fullMoon[i].ToLocalTime().Date){
					isFull = "=";	
					break;
				} 
				else if (dt.Date < _fullMoon[i].ToLocalTime().Date && dt1.Date == _fullMoon[i].ToLocalTime().Date){
					isFull = ">";
					break;	
				}
				
			}
			return isFull;
		}
		
		private String CheckIfNM(DateTime dt){
			String isNM = "";
			DateTime dt1 = dt.AddDays(1);
			for (int i=0; i < _newMoon.Count(); i++){
				if (dt.Date == _newMoon[i].ToLocalTime().Date){
					isNM = "=";	
					break;
				} 
				else if (dt.Date < _newMoon[i].ToLocalTime().Date && dt1.Date == _newMoon[i].ToLocalTime().Date){
					isNM = ">";
					break;	
				}
				
			}
			return isNM;
		}
		
		private String CheckIfQ1(DateTime dt){
			String isQ1 = "";
			DateTime dt1 = dt.AddDays(1);
			for (int i=0; i < _firstQrt.Count(); i++){
				if (dt.Date == _firstQrt[i].ToLocalTime().Date){
					isQ1 = "=";	
					break;
				} 
				else if (dt.Date < _firstQrt[i].ToLocalTime().Date && dt1.Date == _firstQrt[i].ToLocalTime().Date){
					isQ1 = ">";
					break;	
				}
				
			}
			return isQ1;
		}
		
		private String CheckIf3Q(DateTime dt){
			String is3Q = "";
			DateTime dt1 = dt.AddDays(1);
			for (int i=0; i < _thrdQrtr.Count(); i++){
				if (dt.Date == _thrdQrtr[i].ToLocalTime().Date){
					is3Q = "=";	
					break;
				} 
				else if (dt.Date < _thrdQrtr[i].ToLocalTime().Date && dt1.Date == _thrdQrtr[i].ToLocalTime().Date){
					is3Q = ">";
					break;	
				}
				
			}
			return is3Q;
		}
		
		private void DrawTextAndDot(DateTime dt, Brush color, String text){
			
			SimpleFont sf = new SimpleFont("Arial", 15) {Bold = true};
			
			if (dt.DayOfWeek == DayOfWeek.Sunday){
				dt = dt.AddDays(-2);	
			}else if(dt.DayOfWeek == DayOfWeek.Saturday){
				dt = dt.AddDays(-1);	
			}					
			TimeSpan ts = new TimeSpan(01,00,00);
			DateTime dt2 = dt.Date;
			dt = dt.Date + ts;
			double height = (Bars.GetHigh(Bars.GetBar(dt2)) - Bars.GetLow(Bars.GetBar(dt2)) ) * 5;
			double width = ChartControl.BarWidth;
			
				
			if (text.Equals("FM") || text.Equals("Q1")){
				Dot dot = Draw.Dot(this, dt.ToString("MM/dd/yyyy h:mm tt") + text, true, dt, Bars.GetLow(Bars.GetBar(dt)) - DistanceFromBars, color);
				dot.OutlineBrush = color;
				Double price = dot.Anchor.Price;
				Draw.Text(this, dt.ToString("MM/dd/yy h:mm tt") + " text", true, text, dt, 
						Bars.GetLow(Bars.GetBar(dt)) - width - DistanceFromBars - (DistanceFromBars/2), 0, color, sf, TextAlignment.Center, Brushes.Transparent,  Brushes.Transparent, 100);
			}else{
				Dot dot = Draw.Dot(this, dt.ToString("MM/dd/yyyy h:mm tt") + text, true, dt, Bars.GetHigh(Bars.GetBar(dt)) + DistanceFromBars/*+ 0.05 + height*/ , color);
				dot.OutlineBrush = color;
				Double price = dot.Anchor.Price;
				Draw.Text(this, dt.ToString("MM/dd/yy h:mm tt") + " text", true, text, dt, 
						Bars.GetHigh(Bars.GetBar(dt)) + width + DistanceFromBars /*price + 1*/, 0, color, sf, TextAlignment.Center, Brushes.Transparent,  Brushes.Transparent, 100);	
			}
					
			
		}
		
		private void DrawDaily(DateTime dt, Brush color, String text){
			SimpleFont sf = new SimpleFont("Arial", 15) {Bold = true};
			if (dt.DayOfWeek == DayOfWeek.Sunday){
				dt = dt.AddDays(-2);	
			}else if(dt.DayOfWeek == DayOfWeek.Saturday){
				dt = dt.AddDays(-1);	
			}					
			DateTime dt2 = dt.Date;
			double height = (Bars.GetHigh(Bars.GetBar(dt2)) - Bars.GetLow(Bars.GetBar(dt2)) ) * 2;
			double width = ChartControl.BarWidth;
			int barsAgo = CurrentBar - Bars.GetBar(dt);
			TimeSpan ts = new TimeSpan(15,00,00);
			dt = dt.Date + ts;
				
			if (text.Equals("FM") || text.Equals("Q1")){
				Dot dot = Draw.Dot(this, dt.ToString("MM/dd/yyyy h:mm tt") + text, true, dt, Bars.GetLow(Bars.GetBar(dt)) - DistanceFromBars, color);
				dot.OutlineBrush = color;
				Double price = dot.Anchor.Price;
				Draw.Text(this, dt.ToString("MM/dd/yy h:mm tt") + " text", true, text, dt, Bars.GetLow(Bars.GetBar(dt))- width - DistanceFromBars - (DistanceFromBars/2), 0, color,
						sf, TextAlignment.Center, Brushes.Transparent,  Brushes.Transparent, 100);	
			}else{
                		Dot dot = Draw.Dot(this, dt.ToString("MM/dd/yyyy h:mm tt") + text, true, dt, Bars.GetHigh(Bars.GetBar(dt)) + DistanceFromBars, color);
                dot.OutlineBrush = color;	
				Draw.Text(this, dt.ToString("MM/dd/yy h:mm tt") + " text", true, text, dt, Bars.GetHigh(Bars.GetBar(dt)) + width + DistanceFromBars + (DistanceFromBars/2), 0, color,
					sf, TextAlignment.Center, Brushes.Transparent,  Brushes.Transparent, 100);	
			}
		}
		
		public DateTime NearestMoon(){
			DateTime now = DateTime.Now;
			DateTime closestDate, closeNewMoon, closeFull, closeQ1, closeQ3;
			closeNewMoon = now;
            closeFull = now;
            closeQ1 = now;
            closeQ3 = now;
            for (int i = 0; i < _newMoon.Count(); i++)
            {
                if (_newMoon[i].ToLocalTime() > now)
                {
                    closeNewMoon = _newMoon[i].ToLocalTime();
                    break;
                }
            }

            for (int i = 0; i < _fullMoon.Count(); i++)
            {
                if (_fullMoon[i].ToLocalTime() > now)
                {
                    closeFull = _fullMoon[i].ToLocalTime();
                    break;
                }
            }

            for (int i = 0; i < _firstQrt.Count(); i++)
            {
                if (_firstQrt[i].ToLocalTime() >= now)
                {
                    closeQ1 = _firstQrt[i].ToLocalTime();
                    break;
                }
            }

            for (int i = 0; i < _thrdQrtr.Count(); i++)
            {
                if (_thrdQrtr[i].ToLocalTime() > now)
                {
                    closeQ3 = _thrdQrtr[i].ToLocalTime();
                    break;
                }
            }
			
			if (closeNewMoon < closeFull){
				closestDate = closeNewMoon;	
			}else{
				closestDate = closeFull;
			}

            if (closestDate > closeQ1)
            {
                closestDate = closeQ1;
            }

            if(closestDate > closeQ3)
            {
                closestDate = closeQ3;
            }

            return closestDate;
		}

        public String NearestMoonPhase(DateTime dt)
        {
            String phase = "";
            for (int i = 0; i <  _newMoon.Count(); i++)
            {
                if (dt == _newMoon[i].ToLocalTime())
                {
                    phase = "NM";
                }
            }
            for (int i = 0; i <_fullMoon.Count(); i++)
            {
                if (dt == _fullMoon[i].ToLocalTime())
                {
                    phase = "FM";
                }
            }
            for (int i = 0; i < _firstQrt.Count(); i++)
            {
                if (dt == _firstQrt[i].ToLocalTime())
                {
                    phase = "Q1";
                }
            }
            for (int i = 0; i < _thrdQrtr.Count(); i++)
            {
                if (dt == _thrdQrtr[i].ToLocalTime())
                {
                    phase = "Q3";
                }
            }
            return phase;
        }
		
		public void NewMoonDates(){
			_newMoon.Add(new DateTime(2018,01,17,02,17,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2018,02,15,21,05,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2018,03,17,13,11,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2018,04,16,01,57,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2018,05,15,11,47,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2018,06,13,19,43,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2018,07,13,02,47,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2018,08,11,09,57,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2018,09,09,18,01,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2018,10,09,03,46,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2018,11,07,16,01,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2018,12,07,07,20,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2019,01,06,01,28,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2019,02,04,21,03,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2019,03,06,16,03,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2019,04,05,08,50,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2019,05,04,22,45,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2019,06,03,10,01,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2019,07,02,19,16,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2019,08,01,03,11,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2019,08,30,10,37,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2019,09,28,18,26,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2019,10,28,03,38,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2019,11,26,15,05,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2019,12,26,05,13,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2020,01,24,21,42,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2020,02,23,15,32,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2020,03,24,09,28,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2020,04,23,02,25,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2020,05,22,17,38,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2020,06,21,06,41,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2020,07,20,17,32,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2020,08,19,02,41,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2020,09,17,11,00,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2020,10,16,19,31,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2020,11,15,05,07,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2020,12,14,16,16,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2021,01,13,05,00,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2021,02,11,19,05,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2021,03,13,10,21,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2021,04,12,02,30,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2021,05,11,18,59,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2021,06,10,10,52,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2021,07,10,01,16,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2021,08,08,13,50,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2021,09,07,00,51,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2021,10,06,11,05,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2021,11,04,21,14,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2021,12,04,07,43,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2022,01,2,18,33,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2022,02,1,05,45,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2022,03,2,17,34,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2022,04,1,06,24,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2022,04,30,20,28,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2022,05,30,11,30,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2022,06,29,02,52,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2022,07,28,17,54,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2022,08,27,08,17,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2022,09,25,21,54,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2022,10,25,10,48,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2022,11,23,22,57,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2022,12,23,10,16,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2023,01,21,20,53,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2023,02,20,07,05,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2023,03,21,17,23,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2023,04,20,04,12,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2023,05,19,15,53,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2023,06,18,04,37,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2023,07,17,18,31,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2023,08,16,09,38,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2023,09,15,01,39,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2023,10,14,17,55,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2023,11,13,09,27,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2023,12,12,23,32,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2024,01,11,11,57,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2024,02,9,22,59,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2024,03,10,09,00,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2024,04,8,18,20,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2024,05,8,03,21,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2024,06,6,12,37,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2024,07,5,22,57,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2024,08,4,11,13,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2024,09,3,01,55,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2024,10,2,18,49,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2024,11,1,12,47,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2024,12,1,06,21,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2024,12,30,22,26,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2025,01,29,12,35,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2025,02,28,00,44,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2025,03,29,10,57,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2025,04,27,19,31,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2025,05,27,03,02,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2025,06,25,10,31,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2025,07,24,19,11,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2025,08,23,06,06,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2025,09,21,19,54,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2025,10,21,12,25,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2025,11,20,06,47,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2025,12,20,01,43,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2026,01,18,19,51,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2026,02,17,12,01,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2026,03,19,01,23,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2026,04,17,11,53,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2026,05,16,20,01,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2026,06,15,02,54,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2026,07,14,09,43,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2026,08,12,17,36,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2026,09,11,03,26,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2026,10,10,15,50,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2026,11,9,07,02,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2026,12,9,00,51,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2027,01,7,20,24,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2027,02,6,15,56,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2027,03,8,09,29,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2027,04,6,23,51,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2027,05,6,10,58,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2027,06,4,19,40,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2027,07,4,03,02,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2027,08,2,10,05,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2027,08,31,17,41,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2027,09,30,02,36,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2027,10,29,13,36,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2027,11,28,03,24,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2027,12,27,20,12,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2028,01,26,15,12,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2028,02,25,10,37,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2028,03,26,04,31,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2028,04,24,19,46,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2028,05,24,08,16,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2028,06,22,18,27,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2028,07,22,03,01,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2028,08,20,10,43,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2028,09,18,18,23,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2028,10,18,02,56,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2028,11,16,13,17,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2028,12,16,02,06,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2029,01,14,17,24,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2029,02,13,10,31,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2029,03,15,04,19,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2029,04,13,21,40,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2029,05,13,13,42,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2029,06,12,03,50,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2029,07,11,15,51,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2029,08,10,01,55,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2029,09,08,10,44,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2029,10,07,19,14,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2029,11,06,04,24,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2029,12,05,14,52,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2030,01,4,02,49,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2030,02,2,16,07,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2030,03,4,06,34,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2030,04,2,22,02,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2030,05,2,14,12,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2030,06,1,06,21,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2030,06,30,21,34,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2030,07,30,11,10,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2030,08,28,23,07,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2030,09,27,09,54,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2030,10,26,20,16,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2030,11,25,06,46,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2030,12,24,17,32,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2031,01,23,04,30,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2031,02,21,15,48,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2031,03,23,03,49,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2031,04,21,16,57,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2031,05,21,07,17,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2031,06,19,22,24,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2031,07,19,13,40,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2031,08,18,04,32,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2031,09,16,18,46,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2031,10,16,08,20,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2031,11,14,21,09,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2031,12,14,09,05,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2032,01,12,20,06,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2032,02,11,06,24,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2032,03,11,16,24,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2032,04,10,02,39,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2032,05,09,13,35,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2032,06,08,01,32,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2032,07,07,14,41,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2032,08,06,05,11,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2032,09,04,20,56,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2032,10,04,13,26,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2032,11,03,05,44,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2032,12,02,20,52,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2033,01,01,10,16,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2033,01,30,21,59,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2033,03,01,08,23,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2033,03,30,17,51,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2033,04,29,02,46,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2033,05,28,11,36,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2033,06,26,21,07,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2033,07,26,08,12,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2033,08,24,21,39,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2033,09,23,13,39,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2033,10,23,07,28,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2033,11,22,01,39,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2033,12,21,18,46,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2034,01,20,10,01,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2034,02,18,23,10,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2034,03,20,10,14,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2034,04,18,19,25,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2034,05,18,03,12,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2034,06,16,10,25,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2034,07,15,18,15,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2034,08,14,03,53,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2034,09,12,16,13,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2034,10,12,07,32,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2034,11,11,01,16,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2034,12,10,20,14,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2035,01,9,15,03,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2035,02,8,08,22,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2035,03,9,23,09,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2035,04,8,10,57,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2035,05,7,20,03,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2035,06,6,03,20,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2035,07,5,09,59,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2035,08,3,17,11,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2035,09,2,01,59,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2035,10,1,13,06,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2035,10,31,02,58,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2035,11,29,19,37,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2035,12,29,14,30,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2036,01,28,10,17,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2036,02,27,04,59,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2036,03,27,20,56,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2036,04,26,09,33,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2036,05,25,19,16,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2036,06,24,03,09,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2036,07,23,10,17,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2036,08,21,17,35,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2036,09,20,01,51,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2036,10,19,11,49,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2036,11,18,00,14,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2036,12,17,15,34,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2037,01,16,09,34,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2037,02,15,04,54,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2037,03,16,23,56,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2037,04,15,16,07,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2037,05,15,05,54,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2037,06,13,17,10,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2037,07,13,02,31,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2037,08,11,10,41,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2037,09,09,18,25,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2037,10,09,02,34,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2037,11,07,10,03,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2037,12,06,23,38,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2038,01,5,13,41,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2038,02,4,05,52,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2038,03,5,23,14,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2038,04,4,16,42,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2038,05,4,09,19,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2038,06,3,00,24,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2038,07,2,13,32,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2038,08,1,00,40,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2038,08,30,10,12,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2038,09,28,18,57,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2038,10,28,03,52,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2038,11,26,13,46,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2038,12,26,01,01,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2039,01,24,13,36,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2039,02,23,03,17,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2039,03,24,17,59,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2039,04,23,09,34,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2039,05,23,01,37,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2039,06,21,17,21,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2039,07,21,07,54,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2039,08,19,20,50,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2039,09,18,08,22,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2039,10,17,19,08,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2039,11,16,05,46,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2039,12,15,16,32,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2040,01,14,03,25,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2040,02,12,14,24,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2040,03,13,01,46,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2040,04,11,14,00,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2040,05,11,03,27,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2040,06,09,18,03,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2040,07,09,09,14,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2040,08,08,00,26,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2040,09,06,15,13,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2040,10,06,05,25,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2040,11,04,18,55,00,DateTimeKind.Utc));
			_newMoon.Add(new DateTime(2040,12,04,07,33,00,DateTimeKind.Utc));
		}
		
		public void firstQrtrDates(){
			_firstQrt.Add(new DateTime(2018,01,24,22,20,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2018,02,23,08,09,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2018,03,24,15,35,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2018,04,22,21,45,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2018,05,22,03,49,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2018,06,20,10,50,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2018,07,19,19,52,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2018,08,18,07,48,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2018,09,16,23,14,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2018,10,16,18,01,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2018,11,15,14,54,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2018,12,15,11,49,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2019,01,14,06,45,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2019,02,12,22,26,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2019,03,14,10,27,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2019,04,12,19,05,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2019,05,12,01,12,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2019,06,10,05,59,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2019,07,09,10,54,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2019,08,07,17,30,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2019,09,06,03,10,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2019,10,05,16,47,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2019,11,04,10,23,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2019,12,04,06,58,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2020,01,03,04,45,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2020,02,02,01,41,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2020,03,02,19,57,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2020,04,01,10,21,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2020,04,30,20,38,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2020,05,30,03,29,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2020,06,28,08,15,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2020,07,27,12,32,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2020,08,25,17,57,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2020,09,24,01,54,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2020,10,23,13,22,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2020,11,22,04,45,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2020,12,21,23,41,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2021,01,20,21,01,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2021,02,19,18,47,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2021,03,21,14,40,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2021,04,20,06,58,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2021,05,19,19,12,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2021,06,18,03,54,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2021,07,17,10,10,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2021,08,15,15,19,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2021,09,13,20,39,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2021,10,13,03,25,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2021,11,11,12,45,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2021,12,11,01,35,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2022,01,9,18,11,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2022,02,8,13,50,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2022,03,10,10,45,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2022,04,09,06,47,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2022,05,09,00,21,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2022,06,07,14,48,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2022,07,07,02,14,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2022,08,5,11,06,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2022,09,3,18,07,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2022,10,3,00,14,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2022,11,1,06,37,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2022,11,30,14,36,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2022,12,30,01,20,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2023,01,28,15,18,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2023,02,27,08,05,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2023,03,29,02,32,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2023,04,27,21,19,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2023,05,27,15,22,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2023,06,26,07,49,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2023,07,25,22,06,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2023,08,24,09,57,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2023,09,22,19,31,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2023,10,22,03,29,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2023,11,20,10,49,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2023,12,19,18,39,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2024,01,18,03,52,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2024,02,16,15,00,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2024,03,17,04,10,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2024,04,15,19,13,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2024,05,15,11,48,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2024,06,14,05,18,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2024,07,13,22,48,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2024,08,12,15,18,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2024,09,11,06,05,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2024,10,10,18,55,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2024,11,9,05,55,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2024,12,8,15,26,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2025,01,6,23,56,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2025,02,5,08,02,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2025,03,6,16,31,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2025,04,5,02,14,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2025,05,4,13,51,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2025,06,3,03,40,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2025,07,2,19,30,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2025,08,1,12,41,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2025,08,31,06,25,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2025,09,29,23,53,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2025,10,29,16,20,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2025,11,28,06,58,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2025,12,27,19,09,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2026,01,26,04,47,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2026,02,24,12,27,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2026,03,25,19,17,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2026,04,24,12,27,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2026,05,23,19,17,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2026,06,21,02,31,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2026,07,21,11,10,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2026,08,20,21,55,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2026,09,18,11,05,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2026,10,18,02,46,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2026,11,17,20,43,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2026,12,17,16,12,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2027,01,15,20,34,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2027,02,14,07,58,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2027,03,15,16,25,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2027,04,13,22,56,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2027,05,13,04,43,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2027,06,11,10,56,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2027,07,10,18,38,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2027,08,9,04,54,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2027,09,7,18,31,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2027,10,7,11,47,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2027,11,6,07,59,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2027,12,6,05,22,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2028,01,5,01,40,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2028,02,3,19,10,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2028,03,4,09,02,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2028,04,2,19,15,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2028,05,2,02,25,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2028,05,31,07,36,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2028,06,29,12,10,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2028,07,28,17,40,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2028,08,27,01,35,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2028,09,25,13,09,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2028,10,25,04,53,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2028,11,24,00,14,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2028,12,23,21,44,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2029,01,22,19,23,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2029,02,21,15,09,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2029,03,23,07,33,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2029,04,21,19,50,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2029,05,21,04,16,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2029,06,19,09,54,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2029,07,18,14,14,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2029,08,16,18,55,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2029,09,15,01,29,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2029,10,14,11,08,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2029,11,13,00,35,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2029,12,12,17,49,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2030,01,11,14,06,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2030,02,10,11,49,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2030,03,12,08,47,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2030,04,11,02,56,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2030,05,10,17,11,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2030,06,9,03,35,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2030,07,8,11,01,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2030,08,6,16,42,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2030,09,4,21,55,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2030,10,4,03,56,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2030,11,2,11,55,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2030,12,1,22,56,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2030,12,31,13,36,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2031,01,30,07,43,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2031,03,1,04,02,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2031,03,31,00,32,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2031,04,29,19,19,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2031,05,29,11,19,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2031,06,28,00,18,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2031,07,27,10,34,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2031,08,25,18,39,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2031,09,24,01,19,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2031,10,23,07,36,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2031,11,21,14,44,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2031,12,21,00,00,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2032,01,19,12,14,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2032,02,18,03,28,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2032,03,18,20,56,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2032,04,17,15,24,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2032,05,17,09,43,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2032,06,16,02,59,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2032,07,15,18,32,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2032,08,14,07,50,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2032,09,12,18,49,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2032,10,12,03,47,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2032,11,10,11,33,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2032,12,09,19,08,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2033,01,08,03,34,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2033,02,06,13,34,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2033,03,08,01,27,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2033,04,06,15,13,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2033,05,06,06,45,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2033,06,04,23,38,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2033,07,04,17,12,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2033,08,03,10,25,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2033,09,02,02,23,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2033,10,01,16,32,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2033,10,31,04,46,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2033,11,29,15,15,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2033,12,29,00,20,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2034,01,27,08,31,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2034,02,25,16,34,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2034,03,27,01,18,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2034,04,25,11,34,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2034,05,24,23,57,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2034,06,23,14,35,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2034,07,23,07,05,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2034,08,22,00,43,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2034,09,20,18,39,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2034,10,20,12,02,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2034,11,19,04,01,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2034,12,18,17,44,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2035,01,17,04,45,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2035,02,15,13,16,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2035,03,16,20,14,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2035,04,15,02,54,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2035,05,14,10,28,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2035,06,12,19,49,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2035,07,12,07,32,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2035,08,10,21,52,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2035,09,09,14,47,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2035,10,09,09,49,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2035,11,08,05,50,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2035,12,08,01,05,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2036,01,06,17,48,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2036,02,05,07,00,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2036,03,05,16,48,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2036,04,04,00,03,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2036,05,03,05,54,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2036,06,01,11,34,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2036,06,30,18,13,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2036,07,30,02,56,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2036,08,28,14,43,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2036,09,27,06,12,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2036,10,27,01,13,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2036,11,25,22,28,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2036,12,25,19,44,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2037,01,24,14,55,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2037,02,23,06,40,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2037,03,24,18,39,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2037,04,23,03,11,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2037,05,22,09,08,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2037,06,20,13,45,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2037,07,19,18,31,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2037,08,18,00,59,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2037,09,16,10,36,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2037,10,16,00,15,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2037,11,14,17,58,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2037,12,14,14,42,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2038,01,13,12,33,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2038,02,12,09,29,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2038,03,14,03,41,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2038,04,12,18,01,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2038,05,12,04,18,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2038,06,10,11,11,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2038,07,9,16,00,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2038,08,7,20,21,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2038,09,6,01,50,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2038,10,5,09,52,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2038,11,3,21,23,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2038,12,3,12,46,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2039,01,02,07,36,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2039,02,01,04,45,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2039,03,03,02,14,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2039,04,01,21,54,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2039,05,01,14,07,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2039,05,31,02,24,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2039,06,29,11,17,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2039,07,28,17,49,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2039,08,26,23,16,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2039,09,25,04,52,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2039,10,24,11,50,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2039,11,22,21,16,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2039,12,22,10,01,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2040,01,21,02,21,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2040,02,19,21,33,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2040,03,20,17,59,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2040,04,19,13,37,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2040,05,19,07,00,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2040,06,17,21,32,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2040,07,17,09,16,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2040,08,15,18,35,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2040,09,14,02,07,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2040,10,13,08,41,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2040,11,11,15,23,00,DateTimeKind.Utc));
			_firstQrt.Add(new DateTime(2040,12,10,23,29,00,DateTimeKind.Utc));
		}
		
		public void FullMoonDates(){
			_fullMoon.Add(new DateTime(2018,01,02,02,24,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2018,01,31,13,26,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2018,03,02,00,51,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2018,03,31,12,36,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2018,04,30,00,58,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2018,05,29,14,19,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2018,06,28,04,53,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2018,07,27,20,20,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2018,08,26,11,56,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2018,09,25,02,52,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2018,10,24,16,45,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2018,11,23,05,39,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2018,12,22,07,48,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2019,01,21,05,16,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2019,02,19,15,53,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2019,03,21,01,42,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2019,04,19,11,12,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2019,05,18,21,11,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2019,06,17,08,30,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2019,07,16,21,38,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2019,08,15,12,29,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2019,09,14,04,32,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2019,10,13,21,07,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2019,11,12,13,34,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2019,12,12,05,12,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2020,01,10,19,21,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2020,02,09,07,33,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2020,03,09,17,47,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2020,04,08,02,35,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2020,05,07,10,45,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2020,06,05,19,12,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2020,07,05,04,44,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2020,08,03,15,58,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2020,09,02,05,22,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2020,10,01,21,05,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2020,10,31,14,49,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2020,11,30,09,29,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2020,12,30,03,28,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2021,01,28,19,16,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2021,02,27,08,17,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2021,03,28,18,48,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2021,04,27,03,31,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2021,05,26,11,13,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2021,06,24,18,39,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2021,07,24,02,36,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2021,08,22,12,01,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2021,09,20,23,54,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2021,10,20,14,56,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2021,11,19,08,57,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2021,12,19,04,35,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2022,01,17,23,48,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2022,02,16,16,56,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2022,03,18,07,17,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2022,04,16,18,55,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2022,05,16,04,14,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2022,06,14,11,51,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2022,07,13,18,37,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2022,08,12,01,35,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2022,09,10,09,59,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2022,10,09,20,54,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2022,11,08,11,02,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2022,12,08,04,08,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2023,01,06,23,07,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2023,02,05,18,28,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2023,03,07,12,40,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2023,04,06,04,34,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2023,05,05,17,34,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2023,06,04,03,41,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2023,07,03,11,38,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2023,08,01,18,31,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2023,08,31,01,35,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2023,09,29,09,57,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2023,10,28,20,24,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2023,11,27,09,16,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2023,12,27,00,33,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2024,01,25,17,53,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2024,02,24,12,30,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2024,03,25,07,00,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2024,04,23,23,48,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2024,05,23,13,53,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2024,06,22,01,07,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2024,07,21,10,17,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2024,08,19,18,25,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2024,09,18,02,34,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2024,10,17,11,26,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2024,11,15,21,28,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2024,12,15,09,01,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2025,01,13,22,26,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2025,02,12,13,53,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2025,03,14,06,54,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2025,04,13,00,22,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2025,05,12,16,55,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2025,06,11,07,43,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2025,07,10,20,36,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2025,08,9,07,55,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2025,09,7,18,08,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2025,10,7,03,47,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2025,11,5,13,19,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2025,12,4,23,14,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2026,01,3,10,02,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2026,02,1,22,09,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2026,03,3,11,37,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2026,04,2,02,11,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2026,05,1,17,23,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2026,05,31,08,45,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2026,06,29,23,56,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2026,07,29,14,35,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2026,08,28,04,18,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2026,09,26,16,49,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2026,10,26,04,11,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2026,11,24,14,53,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2026,12,24,01,28,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2027,01,22,12,17,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2027,02,20,23,23,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2027,03,22,10,43,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2027,04,20,22,27,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2027,05,20,10,58,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2027,06,19,00,44,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2027,07,18,15,44,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2027,08,17,07,28,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2027,09,15,23,03,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2027,10,15,13,47,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2027,11,14,03,25,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2027,12,13,16,08,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2028,01,12,04,03,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2028,02,10,15,03,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2028,03,11,01,06,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2028,04,09,10,26,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2028,05,08,19,48,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2028,06,07,06,08,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2028,07,06,18,10,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2028,08,05,08,09,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2028,09,03,23,47,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2028,10,03,16,25,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2028,11,02,09,17,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2028,12,02,01,40,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2028,12,31,16,48,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2029,01,30,06,03,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2029,02,28,17,10,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2029,03,30,02,26,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2029,04,28,10,36,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2029,05,27,18,37,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2029,06,26,03,22,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2029,07,25,13,35,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2029,08,24,01,51,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2029,09,22,16,29,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2029,10,22,09,27,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2029,11,21,04,02,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2029,12,20,22,46,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2030,01,19,15,54,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2030,02,18,06,19,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2030,03,19,17,56,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2030,04,18,03,19,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2030,05,17,11,19,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2030,06,15,18,40,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2030,07,15,02,11,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2030,08,13,10,44,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2030,09,11,21,17,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2030,10,11,10,46,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2030,11,10,03,30,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2030,12,09,22,40,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2031,01,08,18,25,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2031,02,07,12,46,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2031,03,09,04,29,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2031,04,07,17,21,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2031,05,07,03,39,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2031,06,05,11,58,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2031,07,04,19,01,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2031,08,03,01,45,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2031,09,01,09,20,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2031,09,30,18,57,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2031,10,30,07,32,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2031,11,28,23,18,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2031,12,28,17,32,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2032,01,27,12,52,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2032,02,26,07,43,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2032,03,27,00,46,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2032,04,25,15,09,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2032,05,25,02,37,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2032,06,23,11,32,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2032,07,22,18,51,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2032,08,21,01,46,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2032,09,19,09,30,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2032,10,18,18,58,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2032,11,17,06,42,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2032,12,16,20,49,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2033,01,15,13,07,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2033,02,14,07,04,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2033,03,16,01,37,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2033,04,14,19,17,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2033,05,14,10,42,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2033,06,12,23,19,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2033,07,12,09,28,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2033,08,10,18,07,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2033,09,9,02,20,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2033,10,8,10,58,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2033,11,6,20,32,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2033,12,6,07,22,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2034,01,4,19,47,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2034,02,3,10,04,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2034,03,5,02,10,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2034,04,3,19,18,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2034,05,3,12,15,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2034,06,2,03,54,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2034,07,1,17,44,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2034,07,31,05,54,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2034,08,29,16,49,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2034,09,28,02,56,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2034,10,27,12,42,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2034,11,25,22,32,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2034,12,25,08,54,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2035,01,23,20,16,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2035,02,22,08,53,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2035,03,23,22,42,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2035,04,22,13,20,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2035,05,22,04,25,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2035,06,20,19,37,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2035,07,20,10,36,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2035,08,19,01,00,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2035,09,17,14,23,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2035,10,17,02,35,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2035,11,15,13,48,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2035,12,15,00,33,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2036,01,13,11,16,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2036,02,11,22,08,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2036,03,12,09,09,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2036,04,10,20,22,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2036,05,10,08,09,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2036,06,08,21,01,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2036,07,08,11,19,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2036,08,07,02,48,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2036,09,05,18,45,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2036,10,05,10,15,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2036,11,04,00,44,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2036,12,03,14,08,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2037,01,02,02,35,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2037,01,31,14,04,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2037,03,02,00,28,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2037,03,31,09,53,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2037,04,29,18,53,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2037,05,29,04,24,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2037,06,27,15,19,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2037,07,27,04,15,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2037,08,25,19,09,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2037,09,24,11,31,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2037,10,24,04,36,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2037,11,22,21,35,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2037,12,22,13,38,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2038,01,21,03,59,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2038,02,19,16,09,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2038,03,21,02,09,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2038,04,19,10,35,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2038,05,18,18,23,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2038,06,17,02,30,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2038,07,16,11,48,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2038,08,14,22,56,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2038,09,13,12,24,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2038,10,13,04,21,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2038,11,11,22,27,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2038,12,11,17,30,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2039,01,10,11,45,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2039,02,09,03,39,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2039,03,10,16,34,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2039,04,09,02,52,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2039,05,08,11,19,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2039,06,06,18,47,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2039,07,06,02,03,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2039,08,04,09,56,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2039,09,02,19,23,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2039,10,02,07,23,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2039,10,31,22,36,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2039,11,30,16,49,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2039,12,30,12,37,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2040,01,29,07,54,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2040,02,28,00,59,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2040,03,28,15,11,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2040,04,27,02,37,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2040,05,26,11,47,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2040,06,24,19,19,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2040,07,24,02,05,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2040,08,22,09,09,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2040,09,20,17,42,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2040,10,20,04,49,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2040,11,18,19,06,00,DateTimeKind.Utc));
			_fullMoon.Add(new DateTime(2040,12,18,12,15,00,DateTimeKind.Utc));
		}
		
		public void ThrdQrtrDates(){
			_thrdQrtr.Add(new DateTime(2018,01,8,22,25,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2018,02,7,15,53,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2018,03,9,11,19,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2018,04,8,07,17,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2018,05,8,02,08,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2018,06,6,18,31,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2018,07,6,07,50,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2018,08,4,18,17,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2018,09,3,02,37,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2018,10,2,09,45,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2018,10,31,16,40,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2018,11,30,00,18,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2018,12,29,09,34,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2019,01,27,21,10,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2019,02,26,11,27,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2019,03,28,04,09,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2019,04,26,22,18,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2019,05,26,16,33,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2019,06,25,09,46,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2019,07,25,01,18,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2019,08,23,14,56,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2019,09,22,02,40,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2019,10,21,12,39,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2019,11,19,21,10,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2019,12,19,04,57,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2020,01,17,12,58,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2020,02,15,22,17,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2020,03,16,09,34,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2020,04,14,22,56,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2020,05,14,14,02,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2020,06,13,06,23,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2020,07,12,23,28,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2020,08,11,16,44,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2020,09,10,09,25,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2020,10,10,00,39,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2020,11,08,13,46,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2020,12,08,00,36,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2021,01,6,09,37,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2021,02,4,17,37,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2021,03,6,01,30,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2021,04,4,10,02,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2021,05,3,19,50,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2021,06,2,07,24,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2021,07,1,21,10,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2021,07,31,13,15,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2021,08,30,07,13,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2021,09,29,01,57,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2021,10,28,20,05,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2021,11,27,12,27,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2021,12,27,02,23,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2022,01,25,13,40,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2022,02,23,22,32,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2022,03,25,05,37,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2022,04,23,11,56,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2022,05,22,18,43,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2022,06,21,03,10,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2022,07,20,14,18,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2022,08,19,04,36,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2022,09,17,21,51,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2022,10,17,17,15,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2022,11,16,13,27,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2022,12,16,08,56,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2023,01,15,02,10,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2023,02,13,16,00,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2023,03,15,02,08,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2023,04,13,09,11,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2023,05,12,14,28,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2023,06,10,19,31,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2023,07,10,01,47,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2023,08,8,10,28,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2023,09,6,22,21,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2023,10,6,13,47,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2023,11,5,08,36,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2023,12,5,05,49,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2024,01,4,03,30,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2024,02,2,23,18,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2024,03,3,15,23,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2024,04,2,03,14,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2024,05,1,11,27,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2024,05,30,17,12,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2024,06,28,21,53,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2024,07,28,02,51,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2024,08,26,09,25,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2024,09,24,18,49,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2024,10,24,08,03,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2024,11,23,01,27,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2024,12,22,22,18,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2025,01,21,20,30,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2025,02,20,17,32,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2025,03,22,11,29,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2025,04,21,01,35,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2025,05,20,11,58,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2025,06,18,19,19,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2025,07,18,00,37,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2025,08,16,05,12,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2025,09,14,10,32,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2025,10,13,18,12,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2025,11,12,05,28,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2025,12,11,20,51,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2026,01,10,15,48,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2026,02,9,12,43,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2026,03,11,09,38,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2026,04,10,04,51,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2026,05,9,21,10,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2026,06,8,10,00,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2026,07,7,19,28,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2026,08,6,02,21,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2026,09,4,07,51,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2026,10,3,13,25,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2026,11,1,20,28,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2026,12,1,06,08,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2026,12,30,18,59,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2027,01,29,10,55,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2027,02,28,05,16,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2027,03,30,00,53,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2027,04,28,20,17,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2027,05,28,13,57,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2027,06,27,04,54,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2027,07,26,16,54,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2027,08,25,02,27,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2027,09,23,10,20,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2027,10,22,17,29,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2027,11,21,00,48,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2027,12,20,09,10,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2028,01,18,19,25,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2028,02,17,08,07,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2028,03,17,23,22,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2028,04,16,16,36,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2028,05,16,10,43,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2028,06,15,04,27,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2028,07,14,20,56,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2028,08,13,11,45,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2028,09,12,00,45,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2028,10,11,11,56,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2028,11,09,21,25,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2028,12,09,05,38,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2029,01,7,13,26,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2029,02,5,21,52,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2029,03,7,07,51,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2029,04,5,19,51,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2029,05,5,09,48,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2029,06,4,01,18,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2029,07,3,17,57,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2029,08,2,11,15,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2029,09,1,04,33,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2029,09,30,20,56,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2029,10,30,11,32,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2029,11,28,23,47,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2029,12,28,09,49,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2030,01,26,18,14,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2030,02,25,01,57,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2030,03,26,09,51,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2030,04,24,18,38,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2030,05,24,04,57,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2030,06,22,17,19,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2030,07,22,08,07,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2030,08,21,01,15,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2030,09,19,19,56,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2030,10,19,14,50,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2030,11,18,08,32,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2030,12,18,00,01,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2031,01,16,12,47,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2031,02,14,22,49,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2031,03,16,06,35,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2031,04,14,12,57,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2031,05,13,19,06,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2031,06,12,02,20,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2031,07,11,11,49,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2031,08,10,00,23,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2031,09,08,16,14,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2031,10,08,10,50,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2031,11,07,07,02,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2031,12,07,03,19,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2032,01,5,22,04,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2032,02,4,13,48,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2032,03,5,01,46,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2032,04,3,10,10,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2032,05,2,16,01,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2032,05,31,20,51,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2032,06,30,02,11,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2032,07,29,09,25,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2032,08,27,19,33,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2032,09,26,09,12,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2032,10,26,02,28,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2032,11,24,22,47,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2032,12,24,20,39,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2033,01,23,17,45,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2033,02,22,11,53,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2033,03,24,01,49,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2033,04,22,11,42,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2033,05,21,18,29,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2033,06,19,23,29,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2033,07,19,04,07,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2033,08,17,09,42,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2033,09,15,17,33,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2033,10,15,04,47,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2033,11,13,20,08,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2033,12,13,15,27,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2034,01,12,13,17,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2034,02,11,11,08,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2034,03,13,06,44,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2034,04,11,22,45,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2034,05,11,10,56,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2034,06,09,19,43,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2034,07,09,01,59,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2034,08,07,06,50,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2034,09,05,11,41,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2034,10,04,18,04,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2034,11,03,03,27,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2034,12,02,16,46,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2035,01,01,10,00,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2035,01,31,06,02,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2035,03,02,03,01,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2035,03,31,23,06,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2035,04,30,16,53,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2035,05,30,07,30,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2035,06,28,18,42,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2035,07,28,02,55,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2035,08,26,09,07,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2035,09,24,14,39,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2035,10,23,20,56,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2035,11,22,05,16,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2035,12,21,16,28,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2036,01,20,06,46,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2036,02,18,23,46,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2036,03,19,18,38,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2036,04,18,14,05,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2036,05,18,08,39,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2036,06,17,01,03,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2036,07,16,14,39,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2036,08,15,01,35,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2036,09,13,10,29,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2036,10,12,18,09,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2036,11,11,01,28,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2036,12,10,09,18,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2037,01,08,18,29,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2037,02,07,05,43,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2037,03,08,19,25,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2037,04,07,11,24,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2037,05,07,04,56,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2037,06,05,22,48,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2037,07,05,16,00,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2037,08,04,07,51,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2037,09,02,22,03,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2037,10,02,10,29,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2037,10,31,21,06,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2037,11,30,06,06,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2037,12,29,14,04,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2038,01,27,22,00,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2038,02,26,06,55,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2038,03,27,17,36,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2038,04,26,06,15,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2038,05,25,20,43,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2038,06,24,12,39,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2038,07,24,05,39,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2038,08,22,23,12,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2038,09,21,16,27,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2038,10,21,08,23,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2038,11,19,22,10,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2038,12,19,09,28,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2039,01,17,18,41,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2039,02,16,02,35,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2039,03,17,10,07,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2039,04,15,18,07,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2039,05,15,03,16,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2039,06,13,14,16,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2039,07,13,03,38,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2039,08,11,19,35,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2039,09,10,13,45,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2039,10,10,08,59,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2039,11,09,03,46,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2039,12,08,20,44,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2040,01,07,11,05,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2040,02,05,22,32,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2040,03,06,07,18,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2040,04,04,14,06,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2040,05,03,19,59,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2040,06,02,02,17,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2040,07,01,10,17,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2040,07,30,21,05,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2040,08,29,11,16,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2040,09,28,04,41,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2040,10,28,00,26,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2040,11,26,21,07,00,DateTimeKind.Utc));
			_thrdQrtr.Add(new DateTime(2040,12,26,17,02,00,DateTimeKind.Utc));
		}
		
		#region Properties
		[NinjaScriptProperty]
		[XmlIgnore]
		[Display(Name="FullMoon", Description="Color Indicator for full moon", Order=1, GroupName="Parameters")]
		public Brush FullMoon
		{ get; set; }

		[Browsable(false)]
		public string FullMoonSerializable
		{
			get { return Serialize.BrushToString(FullMoon); }
			set { FullMoon = Serialize.StringToBrush(value); }
		}			

		[NinjaScriptProperty]
		[XmlIgnore]
		[Display(Name="NewMoon", Description="Color Indicator for new moon", Order=2, GroupName="Parameters")]
		public Brush NewMoon
		{ get; set; }

		[Browsable(false)]
		public string NewMoonSerializable
		{
			get { return Serialize.BrushToString(NewMoon); }
			set { NewMoon = Serialize.StringToBrush(value); }
		}			

		[NinjaScriptProperty]
		[XmlIgnore]
		[Display(Name="Q1", Description="Color Indicator for Q1", Order=3, GroupName="Parameters")]
		public Brush Q1
		{ get; set; }

		[Browsable(false)]
		public string Q1Serializable
		{
			get { return Serialize.BrushToString(Q1); }
			set { Q1 = Serialize.StringToBrush(value); }
		}			

		[NinjaScriptProperty]
		[XmlIgnore]
		[Display(Name="Q3", Description="Color Indicator for Q3", Order=4, GroupName="Parameters")]
		public Brush Q3
		{ get; set; }

		[Browsable(false)]
		public string Q3Serializable
		{
			get { return Serialize.BrushToString(Q3); }
			set { Q3 = Serialize.StringToBrush(value); }
		}			
		
		
		[NinjaScriptProperty]
		[Range(0, double.MaxValue)]
		[Display(Name="DistanceFromBars", Description="Distance from Bars", Order=1, GroupName="Parameters")]
		public double DistanceFromBars
		{ get; set; }
		
		#endregion
		
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private TF_Lunar[] cacheTF_Lunar;
		public TF_Lunar TF_Lunar(Brush fullMoon, Brush newMoon, Brush q1, Brush q3, double distanceFromBars)
		{
			return TF_Lunar(Input, fullMoon, newMoon, q1, q3, distanceFromBars);
		}

		public TF_Lunar TF_Lunar(ISeries<double> input, Brush fullMoon, Brush newMoon, Brush q1, Brush q3, double distanceFromBars)
		{
			if (cacheTF_Lunar != null)
				for (int idx = 0; idx < cacheTF_Lunar.Length; idx++)
					if (cacheTF_Lunar[idx] != null && cacheTF_Lunar[idx].FullMoon == fullMoon && cacheTF_Lunar[idx].NewMoon == newMoon && cacheTF_Lunar[idx].Q1 == q1 && cacheTF_Lunar[idx].Q3 == q3 && cacheTF_Lunar[idx].DistanceFromBars == distanceFromBars && cacheTF_Lunar[idx].EqualsInput(input))
						return cacheTF_Lunar[idx];
			return CacheIndicator<TF_Lunar>(new TF_Lunar(){ FullMoon = fullMoon, NewMoon = newMoon, Q1 = q1, Q3 = q3, DistanceFromBars = distanceFromBars }, input, ref cacheTF_Lunar);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.TF_Lunar TF_Lunar(Brush fullMoon, Brush newMoon, Brush q1, Brush q3, double distanceFromBars)
		{
			return indicator.TF_Lunar(Input, fullMoon, newMoon, q1, q3, distanceFromBars);
		}

		public Indicators.TF_Lunar TF_Lunar(ISeries<double> input , Brush fullMoon, Brush newMoon, Brush q1, Brush q3, double distanceFromBars)
		{
			return indicator.TF_Lunar(input, fullMoon, newMoon, q1, q3, distanceFromBars);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.TF_Lunar TF_Lunar(Brush fullMoon, Brush newMoon, Brush q1, Brush q3, double distanceFromBars)
		{
			return indicator.TF_Lunar(Input, fullMoon, newMoon, q1, q3, distanceFromBars);
		}

		public Indicators.TF_Lunar TF_Lunar(ISeries<double> input , Brush fullMoon, Brush newMoon, Brush q1, Brush q3, double distanceFromBars)
		{
			return indicator.TF_Lunar(input, fullMoon, newMoon, q1, q3, distanceFromBars);
		}
	}
}

#endregion
