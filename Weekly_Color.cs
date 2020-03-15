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
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class Weekly_Color : Indicator
	{
		private List<IndiChartNotes.StringWrapper> YellowSundayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> YellowMondayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> YellowTuesdayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> YellowWednesdayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> YellowThursdayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> YellowFridayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> GreenSundayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> GreenMondayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> GreenTuesdayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> GreenWednesdayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> GreenThursdayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> GreenFridayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> RedSundayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> RedMondayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> RedTuesdayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> RedWednesdayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> RedThursdayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> RedFridayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> BlueSundayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> BlueMondayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> BlueTuesdayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> BlueWednesdayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> BlueThursdayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		private List<IndiChartNotes.StringWrapper> BlueFridayCollectionDefaults = new List<IndiChartNotes.StringWrapper>();
		
		private Collection<IndiChartNotes.StringWrapper> YellowSundayText;
		private Collection<IndiChartNotes.StringWrapper> YellowMondayText;
		private Collection<IndiChartNotes.StringWrapper> YellowTuesdayText;
		private Collection<IndiChartNotes.StringWrapper> YellowWednesdayText;
		private Collection<IndiChartNotes.StringWrapper> YellowThursdayText;
		private Collection<IndiChartNotes.StringWrapper> YellowFridayText;
		private Collection<IndiChartNotes.StringWrapper> GreenSundayText;
		private Collection<IndiChartNotes.StringWrapper> GreenMondayText;
		private Collection<IndiChartNotes.StringWrapper> GreenTuesdayText;
		private Collection<IndiChartNotes.StringWrapper> GreenWednesdayText;
		private Collection<IndiChartNotes.StringWrapper> GreenThursdayText;
		private Collection<IndiChartNotes.StringWrapper> GreenFridayText;
		private Collection<IndiChartNotes.StringWrapper> RedSundayText;
		private Collection<IndiChartNotes.StringWrapper> RedMondayText;
		private Collection<IndiChartNotes.StringWrapper> RedTuesdayText;
		private Collection<IndiChartNotes.StringWrapper> RedWednesdayText;
		private Collection<IndiChartNotes.StringWrapper> RedThursdayText;
		private Collection<IndiChartNotes.StringWrapper> RedFridayText;
		private Collection<IndiChartNotes.StringWrapper> BlueSundayText;
		private Collection<IndiChartNotes.StringWrapper> BlueMondayText;
		private Collection<IndiChartNotes.StringWrapper> BlueTuesdayText;
		private Collection<IndiChartNotes.StringWrapper> BlueWednesdayText;
		private Collection<IndiChartNotes.StringWrapper> BlueThursdayText;
		private Collection<IndiChartNotes.StringWrapper> BlueFridayText;

        int firstDate = 99;
        bool itsFriday = false;
        int DayCount = 0;
		int month = 0;
		bool hols = false;
        int oldDayCount = 0;
		String ColorOfTheWeek = "";
		int counter = 1;
		Brush ColorHolder1, ColorHolder2, ColorHolder3, ColorHolder4;
		
		public  List<NinjaTrader.NinjaScript.Indicators.Weekly_Color> _list;
		[Browsable(false)]	
		public DateTime Date {get; set;} 
		
		[Browsable(false)]
		public string HolidayName {get; set;} 
		
		private Weekly_Color(DateTime date, string holidayName)	
		{
			this.Date = date;
			this.HolidayName = holidayName;
		}
		
		public Weekly_Color()	
		{
			_list=new List<NinjaTrader.NinjaScript.Indicators.Weekly_Color>();
			HolidayDates();
		}
		
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Indicator here.";
				Name										= "Weekly_Color";
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
				OpenMarket						= DateTime.Parse("08:30", System.Globalization.CultureInfo.InvariantCulture);
				CloseMarket						= DateTime.Parse("15:15", System.Globalization.CultureInfo.InvariantCulture);
				Color1					= Brushes.Yellow;
				Color2					= Brushes.Green;
				Color3					= Brushes.Red;
				Color4					= Brushes.Blue;
				ExtraTime1						= DateTime.Parse("00:00", System.Globalization.CultureInfo.InvariantCulture);
				ExtraTime2						= DateTime.Parse("00:00", System.Globalization.CultureInfo.InvariantCulture);
				// YellowWeek
				YellowSundayText 									= new Collection<IndiChartNotes.StringWrapper>(YellowSundayCollectionDefaults);
				YellowMondayText 									= new Collection<IndiChartNotes.StringWrapper>(YellowMondayCollectionDefaults);
				YellowTuesdayText 									= new Collection<IndiChartNotes.StringWrapper>(YellowTuesdayCollectionDefaults);
				YellowWednesdayText 								= new Collection<IndiChartNotes.StringWrapper>(YellowWednesdayCollectionDefaults);
				YellowThursdayText 									= new Collection<IndiChartNotes.StringWrapper>(YellowThursdayCollectionDefaults);
				YellowFridayText 									= new Collection<IndiChartNotes.StringWrapper>(YellowFridayCollectionDefaults);
				ShowYellow											= true;
				YellowWeekFont									= new SimpleFont("Arial", 10);
				// GreenWeek
				GreenSundayText 									= new Collection<IndiChartNotes.StringWrapper>(GreenSundayCollectionDefaults);
				GreenMondayText 									= new Collection<IndiChartNotes.StringWrapper>(GreenMondayCollectionDefaults);
				GreenTuesdayText 									= new Collection<IndiChartNotes.StringWrapper>(GreenTuesdayCollectionDefaults);
				GreenWednesdayText 									= new Collection<IndiChartNotes.StringWrapper>(GreenWednesdayCollectionDefaults);
				GreenThursdayText 									= new Collection<IndiChartNotes.StringWrapper>(GreenThursdayCollectionDefaults);
				GreenFridayText 									= new Collection<IndiChartNotes.StringWrapper>(GreenFridayCollectionDefaults);
				ShowGreen											= true;
				GreenWeekFont									= new SimpleFont("Arial", 10);
				// RedWeek
				RedSundayText 										= new Collection<IndiChartNotes.StringWrapper>(RedSundayCollectionDefaults);
				RedMondayText 										= new Collection<IndiChartNotes.StringWrapper>(RedMondayCollectionDefaults);
				RedTuesdayText 										= new Collection<IndiChartNotes.StringWrapper>(RedTuesdayCollectionDefaults);
				RedWednesdayText 									= new Collection<IndiChartNotes.StringWrapper>(RedWednesdayCollectionDefaults);
				RedThursdayText 									= new Collection<IndiChartNotes.StringWrapper>(RedThursdayCollectionDefaults);
				RedFridayText 										= new Collection<IndiChartNotes.StringWrapper>(RedFridayCollectionDefaults);
				ShowRed												= true;
				RedWeekFont										= new SimpleFont("Arial", 10);
				// BlueWeek
				BlueSundayText 										= new Collection<IndiChartNotes.StringWrapper>(BlueSundayCollectionDefaults);
				BlueMondayText 										= new Collection<IndiChartNotes.StringWrapper>(BlueMondayCollectionDefaults);
				BlueTuesdayText 									= new Collection<IndiChartNotes.StringWrapper>(BlueTuesdayCollectionDefaults);
				BlueWednesdayText 									= new Collection<IndiChartNotes.StringWrapper>(BlueWednesdayCollectionDefaults);
				BlueThursdayText 									= new Collection<IndiChartNotes.StringWrapper>(BlueThursdayCollectionDefaults);
				BlueFridayText 										= new Collection<IndiChartNotes.StringWrapper>(BlueFridayCollectionDefaults);
				ShowBlue											= true;
				BlueWeekFont									= new SimpleFont("Arial", 10);
				
			}
			else if (State == State.Configure)
			{
				_list=new List<NinjaTrader.NinjaScript.Indicators.Weekly_Color>();
				HolidayDates();
			}
		}

		protected override void OnBarUpdate()
		{
			DateTime jun17 = new DateTime(2019,06,17); 
			DateTime jun24 = new DateTime(2019,06,24);
			DateTime jul7 = new DateTime(2019,07,01);
			DateTime jul8 = new DateTime(2019,07,08);
			
			double days = (Time[0].Date - jun17.Date).TotalDays;
            double days2 = (Time[0].Date - jun24.Date).TotalDays;
            double days3 = (Time[0].Date - jul7.Date).TotalDays;
            double days4 = (Time[0].Date - jul8.Date).TotalDays;

            if ((days%28==0) || (days == 0))
            {
                ColorOfTheWeek = "yellow";
                ColorHolder1 = Color1;
                ColorHolder2 = Color2;
                ColorHolder3 = Color3;
                ColorHolder4 = Color4;
                counter = 1;
                DayCount = 1;
            }
            if ((days2 % 28 == 0) || (days2 == 0))
            {
                ColorOfTheWeek = "blue";
                ColorHolder1 = Color4;
                ColorHolder2 = Color1;
                ColorHolder3 = Color2;
                ColorHolder4 = Color3;
                counter = 1;
                DayCount = 1;
            }
            if ((days3 % 28 == 0) || (days3 == 0))
            {
                ColorOfTheWeek = "red";
                ColorHolder1 = Color3;
                ColorHolder2 = Color4;
                ColorHolder3 = Color1;
                ColorHolder4 = Color2;
                counter = 1;
                DayCount = 1;
            }
            if ((days4 % 28 == 0) || (days4 == 0))
            {
                ColorOfTheWeek = "green";
                ColorHolder1 = Color2;
                ColorHolder2 = Color3;
                ColorHolder3 = Color4;
                ColorHolder4 = Color1;
                counter = 1;
                DayCount = 1;
            }
            if (counter<7){
                if (Time[0].Day != firstDate)
                {
                    counter = counter + 1; ;
                }
                DrawWeeks();
            }
        }

        private void DrawWeeks(){
			int[] colors={1,2,3,4};
			Brush ColorToUse;
			if (firstDate != Time[0].Day)
            {
            	DayCount = DayCount + 1;
				hols = CheckIfHoliday(Time[0]);
             	firstDate = Time[0].Day;
            }
			
			if (DayCount > colors.Count()){

				if (DayCount % 4 == 0){
					DayCount = DayCount/4;	
				}else {
					DayCount = DayCount % 4;	
				}
				
			}
			
			switch(DayCount){
				case 1 : 
					ColorToUse = ColorHolder1;
					break;
				case 2 : 
					ColorToUse = ColorHolder2;
					break;
				case 3 : 
					ColorToUse = ColorHolder3;
					break;
				case 4 : 
					ColorToUse = ColorHolder4;
					break;
				default :
					ColorToUse = Brushes.BlueViolet;
					break;	
			}
			
			if (oldDayCount != DayCount)
			 {
				CheckDay(Time[0], ColorToUse);	
                oldDayCount = DayCount;
			 }
			
//			if (Time[0].TimeOfDay == OpenMarket.TimeOfDay){
			if ((Time[0].DayOfWeek != DayOfWeek.Saturday) && (Time[0].DayOfWeek != DayOfWeek.Sunday)){
                DateTime openDt = Time[0].Date + OpenMarket.TimeOfDay;
				Draw.VerticalLine(this, openDt.ToString("MM/dd/yyyy h:mm tt"), openDt, ColorToUse, DashStyleHelper.Dot, 2, true);
//				Draw.Text(this, Time[0].ToString("MM/dd/yyyy h:mm tt") + "TEXT", true, Time[0].ToString("h:mm tt"), Time[0], 
//					MAX(High, 20)[0], 0, ColorToUse, myFont, TextAlignment.Center, Brushes.Transparent, Brushes.Transparent, 100);
			}
			if (!hols)
            {
                if ((Time[0].DayOfWeek != DayOfWeek.Saturday) && (Time[0].DayOfWeek != DayOfWeek.Sunday)){
                    DateTime closeDt = Time[0].Date + CloseMarket.TimeOfDay;
//					Draw.VerticalLine(this, Time[0].ToString("MM/dd/yyyy h:mm tt"), 0, ColorToUse, DashStyleHelper.Dot, 2);	
					Draw.VerticalLine(this, closeDt.ToString("MM/dd/yyyy h:mm tt"), closeDt, ColorToUse, DashStyleHelper.Dot, 2, true);
	//				Draw.Text(this, Time[0].ToString("MM/dd/yyyy h:mm tt") + "TEXT", true, Time[0].ToString("h:mm tt"), Time[0], 
	//					Low[0], -30, ColorToUse, myFont, TextAlignment.Center, Brushes.Transparent, Brushes.Transparent, 100);
				}
            }
			else if (hols)
			{
				TimeSpan t = new TimeSpan(12,00,00);
				DateTime HolDate = Time[0].Date + t;
				if ((Time[0].DayOfWeek != DayOfWeek.Saturday) && (Time[0].DayOfWeek != DayOfWeek.Sunday))
                {
					Draw.VerticalLine(this, HolDate.ToString("MM/dd/yyyy h:mm tt"), HolDate, ColorToUse, DashStyleHelper.Dot, 2, true);
//                    Draw.VerticalLine(this, HolDate.ToString("MM/dd/yyyy h:mm tt"), 0, ColorToUse, DashStyleHelper.Dot, 2);
	//				Draw.Text(this, HolDate.ToString("MM/dd/yyyy h:mm tt") + "TEXT", true, HolDate.ToString("h:mm tt"), Time[0], 
	//					Low[0], -30, ColorToUse, myFont, TextAlignment.Center, Brushes.Transparent, Brushes.Transparent, 100);
                }
			}
			if (Time[0].DayOfWeek == DayOfWeek.Monday){
				DateTime Sun6pm = Time[0].AddDays(-1).Date + new TimeSpan(18, 00, 00);
				DateTime midnight = Time[0].Date + new TimeSpan(00,00,00);				
				Draw.VerticalLine(this, Sun6pm.ToString("MM/dd/yyyy h:mm tt") + " solid", Sun6pm, ColorToUse, DashStyleHelper.Solid, 2, true);
				Draw.VerticalLine(this, midnight.ToString("MM/dd/yyyy h:mm tt") + " solid", midnight, ColorToUse, DashStyleHelper.Solid, 2, true);	
			}
			
			if (EnableExtraTime1)
			{
				if ((Time[0].DayOfWeek != DayOfWeek.Saturday) && (Time[0].DayOfWeek != DayOfWeek.Sunday)){
					DateTime extDt1 = Time[0].Date + ExtraTime1.TimeOfDay;
					Draw.VerticalLine(this, extDt1.ToString("MM/dd/yyyy h:mm tt"), extDt1, ColorToUse, DashStyleHelper.Dot, 2, true);
	//				Draw.Text(this, Time[0].ToString("MM/dd/yyyy h:mm tt") + "TEXT", true, Time[0].ToString("h:mm tt"), Time[0], 
	//					MAX(High, 20)[0], 0, ColorToUse, myFont, TextAlignment.Center, Brushes.Transparent, Brushes.Transparent, 100);
				}
			}
			
			if (EnableExtraTime2)
			{
				if ((Time[0].DayOfWeek != DayOfWeek.Saturday) && (Time[0].DayOfWeek != DayOfWeek.Sunday)){
					DateTime extDt2 = Time[0].Date + ExtraTime2.TimeOfDay;
					Draw.VerticalLine(this, extDt2.ToString("MM/dd/yyyy h:mm tt"), extDt2, ColorToUse, DashStyleHelper.Dot, 2, true);
	//				Draw.Text(this, Time[0].ToString("MM/dd/yyyy h:mm tt") + "TEXT", true, Time[0].ToString("h:mm tt"), Time[0], 
	//					MAX(High, 20)[0], 0, ColorToUse, myFont, TextAlignment.Center, Brushes.Transparent, Brushes.Transparent, 100);
				}
			}
		}
		
		//Check day & check the color of the week
		private void CheckDay(DateTime oldDate, Brush color){
			
			if (color == Color1){
				if(oldDate.DayOfWeek == DayOfWeek.Sunday){
					color = Color2;
				}
			}else if(color == Color2){
				if(oldDate.DayOfWeek == DayOfWeek.Sunday){
					color = Color3;	
				}
			}else if(color == Color3){
				if(oldDate.DayOfWeek == DayOfWeek.Sunday){
					color = Color4;		
				}
			}else if(color == Color4){
				if(oldDate.DayOfWeek == DayOfWeek.Sunday){
					color = Color1;		
				}
			}
			
			if (ColorOfTheWeek.Equals("yellow"))
			{
				if (ShowYellow)
				{
					YellowWeek(oldDate,color);
				}
			}
			else if (ColorOfTheWeek.Equals("green"))
			{
				if (ShowGreen)
				{
					GreenWeek(oldDate,color);
				}
			}
			else if (ColorOfTheWeek.Equals("red"))
			{
				if (ShowRed)
				{
					RedWeek(oldDate,color);
				}
			}
			else if (ColorOfTheWeek.Equals("blue"))
			{
				if (ShowBlue)
				{
					BlueWeek(oldDate,color);
				}
			}
        }
		
		//yellow week
		private void YellowWeek(DateTime dt, Brush color)
		{
			if(dt.DayOfWeek == DayOfWeek.Monday)
            {
                DateTime oldDt = dt.AddDays(-1);
                drawWeek(YellowSundayText, oldDt, color);
                drawWeek(YellowMondayText, dt, color);
			}else if(dt.DayOfWeek == DayOfWeek.Tuesday){
				drawWeek(YellowTuesdayText, dt, color);
			}else if(dt.DayOfWeek == DayOfWeek.Wednesday){
				drawWeek(YellowWednesdayText, dt, color);
			}else if(dt.DayOfWeek == DayOfWeek.Thursday){
				drawWeek(YellowThursdayText, dt, color);
			}else if(dt.DayOfWeek == DayOfWeek.Friday){
				drawWeek(YellowFridayText, dt, color);
			}
		}
		
		private void GreenWeek(DateTime dt, Brush color)
		{
			if(dt.DayOfWeek == DayOfWeek.Monday)
            {
                DateTime oldDt = dt.AddDays(-1);
                drawWeek(GreenSundayText, oldDt, color);
                drawWeek(GreenMondayText, dt, color);
			}else if(dt.DayOfWeek == DayOfWeek.Tuesday){
				drawWeek(GreenTuesdayText, dt, color);
			}else if(dt.DayOfWeek == DayOfWeek.Wednesday){
				drawWeek(GreenWednesdayText, dt, color);
			}else if(dt.DayOfWeek == DayOfWeek.Thursday){
				drawWeek(GreenThursdayText, dt, color);
			}else if(dt.DayOfWeek == DayOfWeek.Friday){
				drawWeek(GreenFridayText, dt, color);
			}
		}
		
		private void RedWeek(DateTime dt, Brush color)
		{
			if(dt.DayOfWeek == DayOfWeek.Monday)
            {
                DateTime oldDt = dt.AddDays(-1);
                drawWeek(RedSundayText, oldDt, color);
                drawWeek(RedMondayText, dt, color);
			}else if(dt.DayOfWeek == DayOfWeek.Tuesday){
				drawWeek(RedTuesdayText, dt, color);
			}else if(dt.DayOfWeek == DayOfWeek.Wednesday){
				drawWeek(RedWednesdayText, dt, color);
			}else if(dt.DayOfWeek == DayOfWeek.Thursday){
				drawWeek(RedThursdayText, dt, color);
			}else if(dt.DayOfWeek == DayOfWeek.Friday){
				drawWeek(RedFridayText, dt, color);
			}
		}
		
		private void BlueWeek(DateTime dt, Brush color)
		{
			if(dt.DayOfWeek == DayOfWeek.Monday)
            {
                DateTime oldDt = dt.AddDays(-1);
                drawWeek(BlueSundayText, oldDt, color);
                drawWeek(BlueMondayText, dt, color);
			}else if(dt.DayOfWeek == DayOfWeek.Tuesday){
				drawWeek(BlueTuesdayText, dt, color);
			}else if(dt.DayOfWeek == DayOfWeek.Wednesday){
				drawWeek(BlueWednesdayText, dt, color);
			}else if(dt.DayOfWeek == DayOfWeek.Thursday){
				drawWeek(BlueThursdayText, dt, color);
			}else if(dt.DayOfWeek == DayOfWeek.Friday){
				drawWeek(BlueFridayText, dt, color);
			}
		}
		
		//drawLines for the "times" in a week
		private void drawWeek(Collection<IndiChartNotes.StringWrapper> coll, DateTime dt, Brush color)
		{
			if(coll.Count > 0){
//				if(!CheckIfHoliday(dt))
//				{
				
					for(int i=0; i<coll.Count; i++){
						String time = ""; 
						time = time + coll[i];
						String hr = time.Substring(0,2);
						String min = time.Substring(2);
						TimeSpan ts = new TimeSpan(Int32.Parse(hr), Int32.Parse(min), 00);
						DateTime dt2 = dt.Date + ts;
						TimeSpan ts12 = new TimeSpan(12,00,00);
						DateTime dt12 = dt.Date + ts12;
					
						if(CheckIfHoliday(dt) && (dt2.TimeOfDay> dt12.TimeOfDay)){
							dt2 = dt12;	
						}
						// Draw.Dot(this, dt2.ToString("MM/dd/yy h:mm tt") + " dot", true, dt2, High[0], color);
	//					SimpleFont myFont = new NinjaTrader.Gui.Tools.SimpleFont("Courier New", 12) { Size = 10, Bold = true };
						double height = (Bars.GetHigh(Bars.GetBar(dt2)) - Bars.GetLow(Bars.GetBar(dt2)) ) * 5;
						Draw.Text(this, dt2.ToString("MM/dd/yy h:mm tt") + " text", true, hr+min, dt2, 
							Bars.GetHigh(Bars.GetBar(dt2)) + 0.05+ height, 0, Brushes.Black, YellowWeekFont, TextAlignment.Center, color, color, 100);
						Draw.Line(this, dt2.ToString("MM/dd/yy h:mm tt") + " line", true, dt2, Bars.GetHigh(Bars.GetBar(dt2)) + 0.05, dt2,Bars.GetHigh(Bars.GetBar(dt2)) + height, 
							color, DashStyleHelper.Solid, 2);
					}
//				}
			}
		}
		
		//CheckIfHoliday returns boolean value
		public bool CheckIfHoliday(DateTime date){
			TimeSpan t = new TimeSpan(00,00,00);
			DateTime dt2 = date.Date + t;
			bool isHoliday = false;
			for(int i=0; i <_list.Count(); i++)
			{
				if (dt2 == _list[i].Date)
				{
					isHoliday = true;
					break;
				}
			}
			return isHoliday;
			
		}
		
		//Holidays 2000-2040.
		public void HolidayDates()	
		{ 
			_list.Add(new Weekly_Color(new DateTime(2000,1,17),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2001,1,15),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2002,1,21),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2003,1,20),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2004,1,19),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2005,1,17),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2006,1,16),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2007,1,15),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2008,1,21),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2009,1,19),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2010,1,18),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2011,1,17),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2012,1,16),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2013,1,21),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2014,1,20),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2015,1,19),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2016,1,18),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2017,1,16),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2018,1,15),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2019,1,21),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2020,1,20),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2021,1,18),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2022,1,17),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2023,1,16),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2024,1,15),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2025,1,20),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2026,1,19),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2027,1,18),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2028,1,17),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2029,1,15),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2030,1,21),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2031,1,20),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2032,1,19),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2033,1,17),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2034,1,16),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2035,1,15),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2036,1,21),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2037,1,19),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2038,1,18),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2039,1,17),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2040,1,16),"MLK"));
			_list.Add(new Weekly_Color(new DateTime(2000,4,21),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2001,4,13),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2002,3,29),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2003,4,18),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2004,4,9),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2005,3,25),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2006,4,14),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2007,4,6),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2008,3,21),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2009,4,10),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2010,4,2),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2011,4,22),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2012,4,6),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2013,3,29),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2014,4,18),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2015,4,3),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2016,3,25),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2017,4,14),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2018,3,30),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2019,4,19),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2020,4,10),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2021,4,2),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2022,4,15),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2023,4,7),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2024,3,29),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2025,4,18),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2026,4,3),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2027,3,26),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2028,4,14),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2029,3,30),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2030,4,19),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2031,4,11),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2032,3,26),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2033,4,15),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2034,4,7),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2035,3,23),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2036,4,11),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2037,4,3),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2038,4,23),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2039,4,8),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2040,3,30),"Good Friday"));
			_list.Add(new Weekly_Color(new DateTime(2000,11,23),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2001,11,22),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2002,11,28),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2003,11,27),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2004,11,25),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2005,11,24),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2006,11,23),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2007,11,22),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2008,11,27),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2009,11,26),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2010,11,25),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2011,11,24),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2012,11,22),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2013,11,28),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2014,11,27),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2015,11,26),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2016,11,24),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2017,11,23),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2018,11,22),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2019,11,28),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2020,11,26),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2021,11,25),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2022,11,24),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2023,11,23),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2024,11,28),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2025,11,27),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2026,11,26),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2027,11,25),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2028,11,23),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2029,11,22),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2030,11,28),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2031,11,27),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2032,11,25),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2033,11,24),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2034,11,23),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2035,11,22),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2036,11,27),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2037,11,26),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2038,11,25),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2039,11,24),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2040,11,22),"Thanksgiving"));
			_list.Add(new Weekly_Color(new DateTime(2000,2,21),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2001,2,19),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2002,2,18),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2003,2,17),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2004,2,16),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2005,2,21),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2006,2,20),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2007,2,19),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2008,2,18),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2009,2,16),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2010,2,15),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2011,2,21),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2012,2,20),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2013,2,18),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2014,2,17),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2015,2,16),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2016,2,15),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2017,2,20),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2018,2,19),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2019,2,18),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2020,2,17),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2021,2,15),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2022,2,21),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2023,2,20),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2024,2,19),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2025,2,17),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2026,2,16),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2027,2,15),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2028,2,21),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2029,2,19),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2030,2,18),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2031,2,17),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2032,2,16),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2033,2,21),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2034,2,20),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2035,2,19),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2036,2,18),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2037,2,16),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2038,2,15),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2039,2,21),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2040,2,20),"Presidents Day"));
			_list.Add(new Weekly_Color(new DateTime(2000,9,4),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2001,9,3),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2002,9,2),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2003,9,1),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2004,9,6),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2005,9,5),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2006,9,4),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2007,9,3),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2008,9,1),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2009,9,7),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2010,9,6),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2011,9,5),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2012,9,3),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2013,9,2),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2014,9,1),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2015,9,7),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2016,9,5),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2017,9,4),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2018,9,3),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2019,9,2),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2020,9,7),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2021,9,6),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2022,9,5),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2023,9,4),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2024,9,2),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2025,9,1),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2026,9,7),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2027,9,6),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2028,9,4),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2029,9,3),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2030,9,2),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2031,9,1),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2032,9,6),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2033,9,5),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2034,9,4),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2035,9,3),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2036,9,1),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2037,9,7),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2038,9,6),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2039,9,5),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2040,9,3),"Labor Day"));
			_list.Add(new Weekly_Color(new DateTime(2000,5,29),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2001,5,28),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2002,5,27),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2003,5,26),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2004,5,31),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2005,5,30),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2006,5,29),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2007,5,28),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2008,5,26),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2009,5,25),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2010,5,31),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2011,5,30),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2012,5,28),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2013,5,27),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2014,5,26),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2015,5,25),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2016,5,30),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2017,5,29),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2018,5,28),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2019,5,27),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2020,5,25),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2021,5,31),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2022,5,30),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2023,5,29),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2024,5,27),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2025,5,26),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2026,5,25),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2027,5,31),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2028,5,29),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2029,5,28),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2030,5,27),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2031,5,26),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2032,5,31),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2033,5,30),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2034,5,29),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2035,5,28),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2036,5,26),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2037,5,25),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2038,5,31),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2039,5,30),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2040,5,28),"Memorial Day"));
			_list.Add(new Weekly_Color(new DateTime(2000,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2001,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2002,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2003,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2004,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2005,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2006,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2007,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2008,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2009,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2010,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2011,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2012,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2013,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2014,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2015,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2016,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2017,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2018,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2019,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2020,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2021,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2022,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2023,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2024,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2025,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2026,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2027,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2028,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2029,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2030,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2031,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2032,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2033,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2034,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2035,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2036,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2037,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2038,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2039,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2040,1,1),"New Years"));
			_list.Add(new Weekly_Color(new DateTime(2000,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2001,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2002,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2003,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2004,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2005,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2006,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2007,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2008,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2009,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2010,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2011,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2012,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2013,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2014,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2015,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2016,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2017,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2018,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2019,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2020,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2021,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2022,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2023,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2024,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2025,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2026,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2027,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2028,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2029,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2030,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2031,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2032,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2033,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2034,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2035,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2036,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2037,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2038,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2039,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2040,12,25),"Christmas"));
			_list.Add(new Weekly_Color(new DateTime(2000,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2001,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2002,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2003,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2004,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2005,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2006,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2007,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2008,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2009,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2010,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2011,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2012,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2013,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2014,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2015,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2016,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2017,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2018,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2019,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2020,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2021,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2022,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2023,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2024,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2025,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2026,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2027,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2028,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2029,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2030,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2031,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2032,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2033,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2034,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2035,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2036,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2037,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2038,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2039,7,4),"July4th"));
			_list.Add(new Weekly_Color(new DateTime(2040,7,4),"July4th"));
   		 }

		#region Properties
		[NinjaScriptProperty]
		[PropertyEditor("NinjaTrader.Gui.Tools.TimeEditorKey")]
		[Display(Name="OpenMarket", Description="Time when the Market is open", Order=1, GroupName="Parameters")]
		public DateTime OpenMarket
		{ get; set; }

		[NinjaScriptProperty]
		[PropertyEditor("NinjaTrader.Gui.Tools.TimeEditorKey")]
		[Display(Name="CloseMarket", Description="Time when Market is close", Order=2, GroupName="Parameters")]
		public DateTime CloseMarket
		{ get; set; }

		[NinjaScriptProperty]
		[XmlIgnore]
		[Display(Name="Color1", Description="First color", Order=3, GroupName="Parameters")]
		public Brush Color1
		{ get; set; }

		[Browsable(false)]
		public string Color1Serializable
		{
			get { return Serialize.BrushToString(Color1); }
			set { Color1 = Serialize.StringToBrush(value); }
		}			

		[NinjaScriptProperty]
		[XmlIgnore]
		[Display(Name="Color2", Description="Second Color", Order=4, GroupName="Parameters")]
		public Brush Color2
		{ get; set; }

		[Browsable(false)]
		public string Color2Serializable
		{
			get { return Serialize.BrushToString(Color2); }
			set { Color2 = Serialize.StringToBrush(value); }
		}			

		[NinjaScriptProperty]
		[XmlIgnore]
		[Display(Name="Color3", Description="Third color", Order=5, GroupName="Parameters")]
		public Brush Color3
		{ get; set; }

		[Browsable(false)]
		public string Color3Serializable
		{
			get { return Serialize.BrushToString(Color3); }
			set { Color3 = Serialize.StringToBrush(value); }
		}			

		[NinjaScriptProperty]
		[XmlIgnore]
		[Display(Name="Color4", Description="Fourth Color", Order=6, GroupName="Parameters")]
		public Brush Color4
		{ get; set; }

		[Browsable(false)]
		public string Color4Serializable
		{
			get { return Serialize.BrushToString(Color4); }
			set { Color4 = Serialize.StringToBrush(value); }
		}			

		[NinjaScriptProperty]
		[PropertyEditor("NinjaTrader.Gui.Tools.TimeEditorKey")]
		[Display(Name="ExtraTime1", Description="Extra time 1", Order=7, GroupName="Parameters")]
		public DateTime ExtraTime1
		{ get; set; }

		[NinjaScriptProperty]
		[PropertyEditor("NinjaTrader.Gui.Tools.TimeEditorKey")]
		[Display(Name="ExtraTime2", Description="ExtraTime2", Order=8, GroupName="Parameters")]
		public DateTime ExtraTime2
		{ get; set; }

		[NinjaScriptProperty]
		[Display(Name="EnableExtraTime1", Description="Enable ExtraTime1 so that it would also draw a vertical line", Order=9, GroupName="Parameters")]
		public bool EnableExtraTime1
		{ get; set; }

		[NinjaScriptProperty]
		[Display(Name="EnableExtraTime2", Description="Enable ExtraTime2 so that it would also draw a vertical line", Order=10, GroupName="Parameters")]
		public bool EnableExtraTime2
		{ get; set; }
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.Boolean")]
		[Display(Name="Yellow Indicator's Visibility", Order=0, GroupName="Yellow Week")]
		[SkipOnCopyTo(true)]
		public Boolean ShowYellow
		{ get;set;}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Sunday", Order=1, GroupName="Yellow Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> yellowSundayText
        {
			get 
			{
				return YellowSundayText;
			}
			set
			{
				YellowSundayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Monday", Order=2, GroupName="Yellow Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> yellowMondayText
        {
			get 
			{
				return YellowMondayText;
			}
			set
			{
				YellowMondayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Tuesday", Order=3, GroupName="Yellow Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> yellowTuesdayText
        {
			get 
			{
				return YellowTuesdayText;
			}
			set
			{
				YellowTuesdayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Wednesday", Order=4, GroupName="Yellow Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> yellowWednesdayText
        {
			get 
			{
				return YellowWednesdayText;
			}
			set
			{
				YellowWednesdayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Thursday", Order=5, GroupName="Yellow Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> yellowThursdayText
        {
			get 
			{
				return YellowThursdayText;
			}
			set
			{
				YellowThursdayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Friday", Order=6, GroupName="Yellow Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> yellowFridayText
        {
			get 
			{
				return YellowFridayText;
			}
			set
			{
				YellowFridayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}

		[NinjaScriptProperty]
		[Display(Name="Yellow Week Font", Description="YellowWeekFont", Order=7, GroupName="Yellow Week")]
		public SimpleFont YellowWeekFont
		{ get; set; }
		
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> YellowSundayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in YellowSundayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = YellowSundayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						YellowSundayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				YellowSundayText.All(p => p.IsDefault = false);
				
				return YellowSundayText;
			}
			set
			{
				YellowSundayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> YellowMondayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in YellowMondayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = YellowMondayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						YellowMondayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				YellowMondayText.All(p => p.IsDefault = false);
				
				return YellowMondayText;
			}
			set
			{
				YellowMondayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> YellowTuesdayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in YellowTuesdayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = YellowTuesdayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						YellowTuesdayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				YellowTuesdayText.All(p => p.IsDefault = false);
				
				return YellowTuesdayText;
			}
			set
			{
				YellowTuesdayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> YellowWednesdayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in YellowWednesdayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = YellowWednesdayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						YellowWednesdayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				YellowWednesdayText.All(p => p.IsDefault = false);
				
				return YellowWednesdayText;
			}
			set
			{
				YellowWednesdayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> YellowThursdayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in YellowThursdayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = YellowThursdayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						YellowThursdayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				YellowThursdayText.All(p => p.IsDefault = false);
				
				return YellowThursdayText;
			}
			set
			{
				YellowThursdayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> YellowFridayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in YellowFridayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = YellowFridayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						YellowFridayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				YellowFridayText.All(p => p.IsDefault = false);
				
				return YellowFridayText;
			}
			set
			{
				YellowFridayText = value;
			}
        }
				
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.Boolean")]
		[Display(Name="Green Indicator's Visibility", Order=8, GroupName="Green Week")]
		[SkipOnCopyTo(true)]
		public Boolean ShowGreen
		{ get;set;}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Sunday", Order=9, GroupName="Green Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> greenSundayText
        {
			get 
			{
				return GreenSundayText;
			}
			set
			{
				GreenSundayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Monday", Order=10, GroupName="Green Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> greenMondayText
        {
			get 
			{
				return GreenMondayText;
			}
			set
			{
				GreenMondayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Tuesday", Order=11, GroupName="Green Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> greenTuesdayText
        {
			get 
			{
				return GreenTuesdayText;
			}
			set
			{
				GreenTuesdayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Wednesday", Order=12, GroupName="Green Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> greenWednesdayText
        {
			get 
			{
				return GreenWednesdayText;
			}
			set
			{
				GreenWednesdayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Thursday", Order=13, GroupName="Green Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> greenThursdayText
        {
			get 
			{
				return GreenThursdayText;
			}
			set
			{
				GreenThursdayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Friday", Order=14, GroupName="Green Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> greenFridayText
        {
			get 
			{
				return GreenFridayText;
			}
			set
			{
				GreenFridayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[NinjaScriptProperty]
		[Display(Name="Green Week Font", Description="GreenWeekFont", Order=16, GroupName="Green Week")]
		public SimpleFont GreenWeekFont
		{ get; set; }
		
		
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> GreenSundayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in GreenSundayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = GreenSundayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						GreenSundayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				GreenSundayText.All(p => p.IsDefault = false);
				
				return GreenSundayText;
			}
			set
			{
				GreenSundayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> GreenMondayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in GreenMondayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = GreenMondayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						GreenMondayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				GreenMondayText.All(p => p.IsDefault = false);
				
				return GreenMondayText;
			}
			set
			{
				GreenMondayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> GreenTuesdayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in GreenTuesdayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = GreenTuesdayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						GreenTuesdayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				GreenTuesdayText.All(p => p.IsDefault = false);
				
				return GreenTuesdayText;
			}
			set
			{
				GreenTuesdayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> GreenWednesdayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in GreenWednesdayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = GreenWednesdayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						GreenWednesdayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				GreenWednesdayText.All(p => p.IsDefault = false);
				
				return GreenWednesdayText;
			}
			set
			{
				GreenWednesdayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> GreenThursdayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in GreenThursdayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = GreenThursdayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						GreenThursdayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				GreenThursdayText.All(p => p.IsDefault = false);
				
				return GreenThursdayText;
			}
			set
			{
				GreenThursdayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> GreenFridayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in GreenFridayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = GreenFridayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						GreenFridayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				GreenFridayText.All(p => p.IsDefault = false);
				
				return GreenFridayText;
			}
			set
			{
				GreenFridayText = value;
			}
        }
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.Boolean")]
		[Display(Name="Red Indicator's Visibility", Order=8, GroupName="Red Week")]
		[SkipOnCopyTo(true)]
		public Boolean ShowRed
		{ get;set;}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Sunday", Order=9, GroupName="Red Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> redSundayText
        {
			get 
			{
				return RedSundayText;
			}
			set
			{
				RedSundayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Monday", Order=10, GroupName="Red Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> redMondayText
        {
			get 
			{
				return RedMondayText;
			}
			set
			{
				RedMondayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Tuesday", Order=11, GroupName="Red Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> redTuesdayText
        {
			get 
			{
				return RedTuesdayText;
			}
			set
			{
				RedTuesdayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Wednesday", Order=12, GroupName="Red Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> redWednesdayText
        {
			get 
			{
				return RedWednesdayText;
			}
			set
			{
				RedWednesdayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Thursday", Order=13, GroupName="Red Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> redThursdayText
        {
			get 
			{
				return RedThursdayText;
			}
			set
			{
				RedThursdayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Friday", Order=14, GroupName="Red Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> redFridayText
        {
			get 
			{
				return RedFridayText;
			}
			set
			{
				RedFridayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[NinjaScriptProperty]
		[Display(Name="Red Week Font", Description="RedWeekFont", Order=16, GroupName="Red Week")]
		public SimpleFont RedWeekFont
		{ get; set; }
		
		
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> RedSundayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in RedSundayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = RedSundayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						RedSundayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				RedSundayText.All(p => p.IsDefault = false);
				
				return RedSundayText;
			}
			set
			{
				RedSundayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> RedMondayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in RedMondayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = RedMondayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						RedMondayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				RedMondayText.All(p => p.IsDefault = false);
				
				return RedMondayText;
			}
			set
			{
				RedMondayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> RedTuesdayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in RedTuesdayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = RedTuesdayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						RedTuesdayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				RedTuesdayText.All(p => p.IsDefault = false);
				
				return RedTuesdayText;
			}
			set
			{
				RedTuesdayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> RedWednesdayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in RedWednesdayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = RedWednesdayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						RedWednesdayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				RedWednesdayText.All(p => p.IsDefault = false);
				
				return RedWednesdayText;
			}
			set
			{
				RedWednesdayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> RedThursdayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in RedThursdayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = RedThursdayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						RedThursdayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				RedThursdayText.All(p => p.IsDefault = false);
				
				return RedThursdayText;
			}
			set
			{
				RedThursdayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> RedFridayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in RedFridayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = RedFridayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						RedFridayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				RedFridayText.All(p => p.IsDefault = false);
				
				return RedFridayText;
			}
			set
			{
				RedFridayText = value;
			}
        }
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.Boolean")]
		[Display(Name="Blue Indicator's Visibility", Order=8, GroupName="Blue Week")]
		[SkipOnCopyTo(true)]
		public Boolean ShowBlue
		{ get;set;}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Sunday", Order=9, GroupName="Blue Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> blueSundayText
        {
			get 
			{
				return BlueSundayText;
			}
			set
			{
				BlueSundayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Monday", Order=10, GroupName="Blue Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> blueMondayText
        {
			get 
			{
				return BlueMondayText;
			}
			set
			{
				BlueMondayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Tuesday", Order=11, GroupName="Blue Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> blueTuesdayText
        {
			get 
			{
				return BlueTuesdayText;
			}
			set
			{
				BlueTuesdayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Wednesday", Order=12, GroupName="Blue Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> blueWednesdayText
        {
			get 
			{
				return BlueWednesdayText;
			}
			set
			{
				BlueWednesdayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Thursday", Order=13, GroupName="Blue Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> blueThursdayText
        {
			get 
			{
				return BlueThursdayText;
			}
			set
			{
				BlueThursdayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[XmlIgnore]
		[Gui.PropertyEditor("NinjaTrader.Gui.Tools.CollectionEditor")]
		[Display(Name="Friday", Order=14, GroupName="Blue Week" , Prompt = "1 string|{0} Time|Add time...|Edit time...|Edit time...")]
		[SkipOnCopyTo(true)]
		public Collection<IndiChartNotes.StringWrapper> blueFridayText
        {
			get 
			{
				return BlueFridayText;
			}
			set
			{
				BlueFridayText = new Collection<IndiChartNotes.StringWrapper>(value.ToList());
			}
		}
		
		[NinjaScriptProperty]
		[Display(Name="Blue Week Font", Description="BlueWeekFont", Order=16, GroupName="Blue Week")]
		public SimpleFont BlueWeekFont
		{ get; set; }
		
		
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> BlueSundayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in BlueSundayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = BlueSundayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						BlueSundayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				BlueSundayText.All(p => p.IsDefault = false);
				
				return BlueSundayText;
			}
			set
			{
				BlueSundayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> BlueMondayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in BlueMondayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = BlueMondayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						BlueMondayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				BlueMondayText.All(p => p.IsDefault = false);
				
				return BlueMondayText;
			}
			set
			{
				BlueMondayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> BlueTuesdayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in BlueTuesdayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = BlueTuesdayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						BlueTuesdayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				BlueTuesdayText.All(p => p.IsDefault = false);
				
				return BlueTuesdayText;
			}
			set
			{
				BlueTuesdayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> BlueWednesdayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in BlueWednesdayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = BlueWednesdayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						BlueWednesdayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				BlueWednesdayText.All(p => p.IsDefault = false);
				
				return BlueWednesdayText;
			}
			set
			{
				BlueWednesdayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> BlueThursdayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in BlueThursdayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = BlueThursdayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						BlueThursdayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				BlueThursdayText.All(p => p.IsDefault = false);
				
				return BlueThursdayText;
			}
			set
			{
				BlueThursdayText = value;
			}
        }
		[Browsable(false)]
        public Collection<IndiChartNotes.StringWrapper> BlueFridayTextSerialize
        {
			get
			{
				//Remove actual defaults
				foreach(IndiChartNotes.StringWrapper sw in BlueFridayCollectionDefaults.ToList())
				{
					IndiChartNotes.StringWrapper temp = BlueFridayCollectionDefaults.FirstOrDefault(p => p.StringValue == sw.StringValue && p.IsDefault == true);
					if(temp != null)
						BlueFridayCollectionDefaults.Remove(temp);
				}
				
				//Force user added values to not be defaults
				BlueFridayText.All(p => p.IsDefault = false);
				
				return BlueFridayText;
			}
			set
			{
				BlueFridayText = value;
			}
        }
		
		
		#endregion

	}
}


#region StringWrapper
namespace IndiChartNotes
{
	[CategoryDefaultExpanded(true)]
    public class StringWrapper : NotifyPropertyChangedBase, ICloneable
    {
        // Parameterless constructor is needed for Clone and serialization
        public StringWrapper() : this(string.Empty)
        {
        }

        public StringWrapper(string value)
        {
            StringValue = value;
        }

        // Display attributes, XmlIgnore attributes, Browsable attributes, etc can be all applied to the object's properties as well.
        [Display(Name = "Time", GroupName = "Time")]
        public string StringValue
        { get; set; }

        // Cloned instance returned to the Collection editor with user defined value

		public object Clone()
		{
			StringWrapper p = new StringWrapper();
			p.StringValue = StringValue;
			return p;
		}
		
		//Default value handling
		[Browsable(false)]
		public bool IsDefault { get; set; }
		
        // Customize the displays on the left side of the editor window
        public override string ToString()
        { return StringValue; }
		
		// Use Reflection to be able to copy properties to new instance
		public object AssemblyClone(Type t)
		{
			Assembly a 			= t.Assembly;
			object noteText 	= a.CreateInstance(t.FullName);
			
			foreach (PropertyInfo p in t.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				if (p.CanWrite)
				{
					p.SetValue(noteText, this.GetType().GetProperty(p.Name).GetValue(this), null);
				}
			}
			
			return noteText;
		}
    }
	
}
#endregion

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private Weekly_Color[] cacheWeekly_Color;
		public Weekly_Color Weekly_Color(DateTime openMarket, DateTime closeMarket, Brush color1, Brush color2, Brush color3, Brush color4, DateTime extraTime1, DateTime extraTime2, bool enableExtraTime1, bool enableExtraTime2, SimpleFont yellowWeekFont, SimpleFont greenWeekFont, SimpleFont redWeekFont, SimpleFont blueWeekFont)
		{
			return Weekly_Color(Input, openMarket, closeMarket, color1, color2, color3, color4, extraTime1, extraTime2, enableExtraTime1, enableExtraTime2, yellowWeekFont, greenWeekFont, redWeekFont, blueWeekFont);
		}

		public Weekly_Color Weekly_Color(ISeries<double> input, DateTime openMarket, DateTime closeMarket, Brush color1, Brush color2, Brush color3, Brush color4, DateTime extraTime1, DateTime extraTime2, bool enableExtraTime1, bool enableExtraTime2, SimpleFont yellowWeekFont, SimpleFont greenWeekFont, SimpleFont redWeekFont, SimpleFont blueWeekFont)
		{
			if (cacheWeekly_Color != null)
				for (int idx = 0; idx < cacheWeekly_Color.Length; idx++)
					if (cacheWeekly_Color[idx] != null && cacheWeekly_Color[idx].OpenMarket == openMarket && cacheWeekly_Color[idx].CloseMarket == closeMarket && cacheWeekly_Color[idx].Color1 == color1 && cacheWeekly_Color[idx].Color2 == color2 && cacheWeekly_Color[idx].Color3 == color3 && cacheWeekly_Color[idx].Color4 == color4 && cacheWeekly_Color[idx].ExtraTime1 == extraTime1 && cacheWeekly_Color[idx].ExtraTime2 == extraTime2 && cacheWeekly_Color[idx].EnableExtraTime1 == enableExtraTime1 && cacheWeekly_Color[idx].EnableExtraTime2 == enableExtraTime2 && cacheWeekly_Color[idx].YellowWeekFont == yellowWeekFont && cacheWeekly_Color[idx].GreenWeekFont == greenWeekFont && cacheWeekly_Color[idx].RedWeekFont == redWeekFont && cacheWeekly_Color[idx].BlueWeekFont == blueWeekFont && cacheWeekly_Color[idx].EqualsInput(input))
						return cacheWeekly_Color[idx];
			return CacheIndicator<Weekly_Color>(new Weekly_Color(){ OpenMarket = openMarket, CloseMarket = closeMarket, Color1 = color1, Color2 = color2, Color3 = color3, Color4 = color4, ExtraTime1 = extraTime1, ExtraTime2 = extraTime2, EnableExtraTime1 = enableExtraTime1, EnableExtraTime2 = enableExtraTime2, YellowWeekFont = yellowWeekFont, GreenWeekFont = greenWeekFont, RedWeekFont = redWeekFont, BlueWeekFont = blueWeekFont }, input, ref cacheWeekly_Color);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.Weekly_Color Weekly_Color(DateTime openMarket, DateTime closeMarket, Brush color1, Brush color2, Brush color3, Brush color4, DateTime extraTime1, DateTime extraTime2, bool enableExtraTime1, bool enableExtraTime2, SimpleFont yellowWeekFont, SimpleFont greenWeekFont, SimpleFont redWeekFont, SimpleFont blueWeekFont)
		{
			return indicator.Weekly_Color(Input, openMarket, closeMarket, color1, color2, color3, color4, extraTime1, extraTime2, enableExtraTime1, enableExtraTime2, yellowWeekFont, greenWeekFont, redWeekFont, blueWeekFont);
		}

		public Indicators.Weekly_Color Weekly_Color(ISeries<double> input , DateTime openMarket, DateTime closeMarket, Brush color1, Brush color2, Brush color3, Brush color4, DateTime extraTime1, DateTime extraTime2, bool enableExtraTime1, bool enableExtraTime2, SimpleFont yellowWeekFont, SimpleFont greenWeekFont, SimpleFont redWeekFont, SimpleFont blueWeekFont)
		{
			return indicator.Weekly_Color(input, openMarket, closeMarket, color1, color2, color3, color4, extraTime1, extraTime2, enableExtraTime1, enableExtraTime2, yellowWeekFont, greenWeekFont, redWeekFont, blueWeekFont);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.Weekly_Color Weekly_Color(DateTime openMarket, DateTime closeMarket, Brush color1, Brush color2, Brush color3, Brush color4, DateTime extraTime1, DateTime extraTime2, bool enableExtraTime1, bool enableExtraTime2, SimpleFont yellowWeekFont, SimpleFont greenWeekFont, SimpleFont redWeekFont, SimpleFont blueWeekFont)
		{
			return indicator.Weekly_Color(Input, openMarket, closeMarket, color1, color2, color3, color4, extraTime1, extraTime2, enableExtraTime1, enableExtraTime2, yellowWeekFont, greenWeekFont, redWeekFont, blueWeekFont);
		}

		public Indicators.Weekly_Color Weekly_Color(ISeries<double> input , DateTime openMarket, DateTime closeMarket, Brush color1, Brush color2, Brush color3, Brush color4, DateTime extraTime1, DateTime extraTime2, bool enableExtraTime1, bool enableExtraTime2, SimpleFont yellowWeekFont, SimpleFont greenWeekFont, SimpleFont redWeekFont, SimpleFont blueWeekFont)
		{
			return indicator.Weekly_Color(input, openMarket, closeMarket, color1, color2, color3, color4, extraTime1, extraTime2, enableExtraTime1, enableExtraTime2, yellowWeekFont, greenWeekFont, redWeekFont, blueWeekFont);
		}
	}
}

#endregion
