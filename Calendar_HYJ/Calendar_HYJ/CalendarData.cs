﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
/*
 * ***********************
 * 创建日期：2018/10/4
 * 创建人：HYJ
 * 文件描述：Calendar类（日历数据类）实现
 * ***********************
 */
namespace Calendar_HYJ
{
    /// <summary>
    /// 日历数据类
    /// </summary>
    class CalendarData:INotifyPropertyChanged
    {
        /// <summary>
        /// 当前年数据
        /// </summary>
        private int selYear;
        /// <summary>
        /// 当前月数据
        /// </summary>
        private int selMonth;
        /// <summary>
        /// 当前日期数据
        /// </summary>
        private List<DayData> selDays;
        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        public int SelYear
        {
            get => selYear;
            set
            {
                selYear = value;
                //激发事件
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("selYear"));
                    //this.SynchronizationSelYearMonth();
                    //this.SynchronizationSelDays();
                }
            }
        }
        public int SelMonth
        {
            get => selMonth;
            set
            {
                selMonth = value;
                //激发事件
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("selMonth"));
                    this.SynchronizationSelDays();
                }
            }
        }
        internal List<DayData> SelDays { get => selDays; set => selDays = value; }
        /// <summary>
        /// 构造
        /// </summary>
        public CalendarData(int y=1900, int m=1, int d=31)
        {
            this.selYear = y;
            this.selMonth = m;
            InitialSelDays(y, m);
            SynchronizationSelDays();
        }
        /// <summary>
        /// 初始化当前日期数据
        /// </summary>
        private void InitialSelDays(int y, int m)
        {
            this.selDays = new List<DayData>();
            for (int i = 0; i < 35; i++)
            {
                DayData d = new DayData(y, m);
                selDays.Add(d);
            }
        }
        /// <summary>
        /// 同步当前年月数据
        /// </summary>
        private void SynchronizationSelYearMonth(DayData d, int m)
        {
            if (m == 0)
            {
                d.SolarYear = this.selYear - 1;
                d.SolarMonth = 12;
            }
            else if (m == 13)
            {
                d.SolarYear = this.selYear + 1;
                d.SolarMonth = 1;
            }
            else
            {
                d.SolarYear = this.selYear;
                d.SolarMonth = m;
            }
        }
        /// <summary>
        /// 同步当前日期数据
        /// </summary>
        private void SynchronizationSelDays()
        {
            //y年m月的第一日
            DateTime dt = new DateTime(this.selYear, this.selMonth, 1);
            //y年m月1日 星期(Sunday 0
            int dw = (int)dt.DayOfWeek;
            if (dw == 0)
            {
                dw = 7;
            }
            //上个月天数
            int lastDays;
            if (this.selMonth == 1)
            {
                lastDays = DateTime.DaysInMonth(this.selYear, 12);
            }
            else
            {
                lastDays = DateTime.DaysInMonth(this.selYear, this.selMonth-1);
            }
            //当前月天数
            int nowDays = DateTime.DaysInMonth(this.selYear, this.selMonth);
            //上个月日期
            for (int i = 0; i < dw - 1; i++)
            {
                SynchronizationSelYearMonth(this.SelDays[i], this.selMonth - 1);
                this.SelDays[i].SolarDay = lastDays - i + dw +1;
            }
            //当前月日期
            for (int i = dw - 1; i < nowDays; i++)
            {
                SynchronizationSelYearMonth(this.SelDays[i], this.selMonth);
                this.SelDays[i].SolarDay = i - dw + 2;
            }
            //下个月日期
            for (int i = dw + nowDays - 1; i < 35; i++)
            {
                SynchronizationSelYearMonth(this.SelDays[i], this.selMonth + 1);
                this.SelDays[i].SolarDay = i - dw - nowDays + 2;
            }
        }//InitialSelDays 函数结束
    }
}
