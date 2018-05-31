using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class Wordlist : MonoBehaviour {
    private const string _PATH_ = "Wordlists/";


	public static string GenerateWordFromList(string[] files) {
        string res = "";
        foreach(string file in files) {
            TextAsset txt = (TextAsset) Resources.Load(_PATH_ + "" + file, typeof(TextAsset));
            string[] lines = Regex.Split(txt.text, "\r\n");
            res += FirstCharToUpper(lines[UnityEngine.Random.Range(0, lines.Length)]);
        }

        return res;
    }

    public static string FirstCharToUpper(string input) {
        if(string.IsNullOrEmpty(input))
            throw new ArgumentException("ARGH!");
        return input.First().ToString().ToUpper() + input.Substring(1);
    }
}
