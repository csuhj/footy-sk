﻿using System.Reflection;
using System.Text.RegularExpressions;

namespace FootySk.Database.Player;

public class PlayerCsvParser
{
    private static readonly Regex csvParseRegex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
    private static readonly Regex heightInCmRegex = new Regex("(\\d+)cm.*");
    private static readonly Regex weightInKgRegex = new Regex("(\\d+)kg.*");

    private readonly string filename;
    private readonly FieldInfo[] fields;
    private readonly Func<string, object>[] conversions;

    public PlayerCsvParser(string filename)
    {
        this.filename = filename;
        fields = typeof(PlayerRecord).GetFields();

        conversions = fields.Select<FieldInfo, Func<string, object>>(f => {
            if (f.Name == nameof(PlayerRecord.HeightInCm))
                return ParseHeightInCm;
            else if (f.Name == nameof(PlayerRecord.WeightInKg))
                return ParseWeightInKg;
            else if (f.Name == nameof(PlayerRecord.GenderRank))
                return s => (int)ParseInt(s) + 1;
            else if (f.FieldType == typeof(int))
                return ParseInt;
            else if (f.FieldType == typeof(string[]))
                return ParseStringArray;
            else
                return ParseStringRemovingQuotes;
        }).ToArray();
    }

    public async Task<PlayerRecord[]> Parse()
    {
        using (var reader = new StreamReader(filename))
        {
            List<PlayerRecord> players = new List<PlayerRecord>();
            await reader.ReadLineAsync();
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                players.Add(Parse(line));
            }

            return players.ToArray();
        }
    }

    private PlayerRecord Parse(string line)
    {
        string[] data = csvParseRegex.Split(line);
        PlayerRecord player = new PlayerRecord();

        for(int i=0; i<data.Length; i++)
        {
            object dataAsType = conversions[i](data[i]);
            fields[i].SetValue(player, dataAsType);
        }
        return player;
    }

    private static object ParseHeightInCm(string s)
    {
        s = ParseStringRemovingQuotes(s);
        var match = heightInCmRegex.Match(s);
        if (!match.Success)
            throw new Exception($"Couldn't parse {s} as a height in cm");
        
        return int.Parse(match.Groups[1].Value);
    }

    private static object ParseWeightInKg(string s)
    {
        s = ParseStringRemovingQuotes(s);
        var match = weightInKgRegex.Match(s);
        if (!match.Success)
            throw new Exception($"Couldn't parse {s} as a weight in kg");
        
        return int.Parse(match.Groups[1].Value);
    }

    private static object ParseInt(string s)
    {
        if (s == "")
            return 0;
        
        if (double.TryParse(s, out double d))
            return (int)Math.Round(d);

        return int.Parse(s);
    }

    private static string ParseStringRemovingQuotes(string s)
    {
        if (s.StartsWith("\"") && s.EndsWith("\""))
            s = s.Substring(1, s.Length - 2);

        s = s.Replace("\"\"","\"");
        return s;
    }

    private static string[] ParseStringArray(string s)
    {
        s = ParseStringRemovingQuotes(s);
        return csvParseRegex.Split(s).Where(s => !string.IsNullOrEmpty(s)).ToArray();
    }
}
