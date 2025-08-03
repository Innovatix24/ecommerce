using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Extensions;

public static class DateTimeExt
{
    public static string GetAgoStr(this DateTime dateTime)
    {
        var timeSpan = DateTime.UtcNow - dateTime;

        if (timeSpan.TotalSeconds < 60)
            return "just now";
        if (timeSpan.TotalMinutes < 60)
            return $"{(int)timeSpan.TotalMinutes} minute{(timeSpan.TotalMinutes >= 2 ? "s" : "")} ago";
        if (timeSpan.TotalHours < 24)
            return $"{(int)timeSpan.TotalHours} hour{(timeSpan.TotalHours >= 2 ? "s" : "")} ago";
        if (timeSpan.TotalDays < 7)
            return $"{(int)timeSpan.TotalDays} day{(timeSpan.TotalDays >= 2 ? "s" : "")} ago";
        if (timeSpan.TotalDays < 30)
            return $"{(int)(timeSpan.TotalDays / 7)} week{(timeSpan.TotalDays / 7 >= 2 ? "s" : "")} ago";
        if (timeSpan.TotalDays < 365)
            return $"{(int)(timeSpan.TotalDays / 30)} month{(timeSpan.TotalDays / 30 >= 2 ? "s" : "")} ago";

        return $"{(int)(timeSpan.TotalDays / 365)} year{(timeSpan.TotalDays / 365 >= 2 ? "s" : "")} ago";
    }

    public static string GetAgoStrBangla(this DateTime dateTime)
    {
        var timeSpan = DateTime.UtcNow - dateTime;

        if (timeSpan.TotalSeconds < 60)
            return "just now";
        if (timeSpan.TotalMinutes < 60)
            return $"{(int)timeSpan.TotalMinutes} minute{(timeSpan.TotalMinutes >= 2 ? "s" : "")} ago";
        if (timeSpan.TotalHours < 24)
            return $"{(int)timeSpan.TotalHours} hour{(timeSpan.TotalHours >= 2 ? "s" : "")} ago";
        if (timeSpan.TotalDays < 7)
            return $"{(int)timeSpan.TotalDays} day{(timeSpan.TotalDays >= 2 ? "s" : "")} ago";
        if (timeSpan.TotalDays < 30)
            return $"{(int)(timeSpan.TotalDays / 7)} week{(timeSpan.TotalDays / 7 >= 2 ? "s" : "")} ago";
        if (timeSpan.TotalDays < 365)
            return $"{(int)(timeSpan.TotalDays / 30)} month{(timeSpan.TotalDays / 30 >= 2 ? "s" : "")} ago";

        return $"{(int)(timeSpan.TotalDays / 365)} year{(timeSpan.TotalDays / 365 >= 2 ? "s" : "")} ago";
    }

    public static string GetTimeAgoInBangla(DateTime dateTime)
    {
        var timeSpan = DateTime.UtcNow - dateTime;

        if (timeSpan.TotalSeconds < 60)
            return "এখনই";

        if (timeSpan.TotalMinutes < 60)
            return $"{ToBanglaNumber((int)timeSpan.TotalMinutes)} মিনিট আগে";

        if (timeSpan.TotalHours < 24)
            return $"{ToBanglaNumber((int)timeSpan.TotalHours)} ঘণ্টা আগে";

        if (timeSpan.TotalDays < 7)
            return $"{ToBanglaNumber((int)timeSpan.TotalDays)} দিন আগে";

        if (timeSpan.TotalDays < 30)
            return $"{ToBanglaNumber((int)(timeSpan.TotalDays / 7))} সপ্তাহ আগে";

        if (timeSpan.TotalDays < 365)
            return $"{ToBanglaNumber((int)(timeSpan.TotalDays / 30))} মাস আগে";

        return $"{ToBanglaNumber((int)(timeSpan.TotalDays / 365))} বছর আগে";
    }

    public static string ToBanglaNumber(int number)
    {
        string english = number.ToString();
        string[] banglaDigits = { "০", "১", "২", "৩", "৪", "৫", "৬", "৭", "৮", "৯" };
        string result = "";

        foreach (char c in english)
        {
            if (char.IsDigit(c))
            {
                result += banglaDigits[c - '0'];
            }
            else
            {
                result += c;
            }
        }

        return result;
    }
}
