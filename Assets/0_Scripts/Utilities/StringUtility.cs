using UnityEngine;
using System.Text.RegularExpressions;

public class StringUtility
{
    public static string AddSpacesToSentence(string value)
    {
        return Regex.Replace(value, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
    }
}
