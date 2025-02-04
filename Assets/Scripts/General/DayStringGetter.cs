using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayStringGetter
{
    public string GetNowDayString(){
        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;

        int hour = System.DateTime.Now.Hour;
        int min = System.DateTime.Now.Minute;

        return $"{year}年 {month}月{day}日 {hour:D2}:{min:D2}";
    }
}
