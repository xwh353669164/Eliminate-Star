using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AccountResult
{
    Success,
    InvalidFormat,
    AccountExists,
    AccountNotFound,
    PasswordIncorrect
}

[Serializable]
public class RankRecord
{
    public int score;
    public long timeTicks;

    public RankRecord(int score, long timeTicks)
    {
        this.score = score;
        this.timeTicks = timeTicks;
    }

    public string GetDisplayTime()
    {
        return new DateTime(timeTicks).ToString("yyyy年MM月dd日HH:mm");
    }
}

public sealed class LocalDataService
{
    private const string AccountsKey = "EliminateStar.Accounts";
    private const string RanksKey = "EliminateStar.Ranks";
    private const string EmptyAccountsJson = "{\"accounts\":[]}";
    private const string EmptyRanksJson = "{\"ranks\":[]}";
    private const int MaxRankCount = 20;

    private static readonly LocalDataService instance = new LocalDataService();
    public static LocalDataService Instance => instance;

    private LocalDataService()
    {
    }

    public AccountResult Register(string userName, string password)
    {
        if (!IsAsciiAlphaNumeric(userName) || !IsAsciiAlphaNumeric(password))
            return AccountResult.InvalidFormat;

        AccountCollection data = LoadAccounts();
        if (data.accounts.Exists(account => account.userName == userName))
            return AccountResult.AccountExists;

        data.accounts.Add(new AccountRecord(userName, password));
        PlayerPrefs.SetString(AccountsKey, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
        return AccountResult.Success;
    }

    public AccountResult Login(string userName, string password)
    {
        if (!IsAsciiAlphaNumeric(userName) || !IsAsciiAlphaNumeric(password))
            return AccountResult.InvalidFormat;

        List<AccountRecord> accounts = LoadAccounts().accounts;
        int accountIndex = accounts.FindIndex(item => item.userName == userName);
        if (accountIndex < 0)
            return AccountResult.AccountNotFound;

        if (accounts[accountIndex].password != password)
            return AccountResult.PasswordIncorrect;

        return AccountResult.Success;
    }

    public void AddRank(int score, DateTime recordTime)
    {
        RankCollection data = LoadRanks();
        data.ranks.Add(new RankRecord(score, recordTime.Ticks));
        data.ranks = data.ranks
            .OrderByDescending(record => record.score)
            .ThenBy(record => record.timeTicks)
            .Take(MaxRankCount)
            .ToList();

        PlayerPrefs.SetString(RanksKey, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    public List<RankRecord> GetRanks()
    {
        return LoadRanks().ranks
            .OrderByDescending(record => record.score)
            .ThenBy(record => record.timeTicks)
            .Take(MaxRankCount)
            .ToList();
    }

    private static bool IsAsciiAlphaNumeric(string value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        foreach (char character in value)
        {
            bool isNumber = character >= '0' && character <= '9';
            bool isUpper = character >= 'A' && character <= 'Z';
            bool isLower = character >= 'a' && character <= 'z';
            if (!isNumber && !isUpper && !isLower)
                return false;
        }

        return true;
    }

    private static AccountCollection LoadAccounts()
    {
        string json = PlayerPrefs.GetString(AccountsKey, EmptyAccountsJson);
        return JsonUtility.FromJson<AccountCollection>(json);
    }

    private static RankCollection LoadRanks()
    {
        string json = PlayerPrefs.GetString(RanksKey, EmptyRanksJson);
        return JsonUtility.FromJson<RankCollection>(json);
    }

    [Serializable]
    private class AccountRecord
    {
        public string userName;
        public string password;

        public AccountRecord(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }
    }

    [Serializable]
    private class AccountCollection
    {
        public List<AccountRecord> accounts = new List<AccountRecord>();
    }

    [Serializable]
    private class RankCollection
    {
        public List<RankRecord> ranks = new List<RankRecord>();
    }
}
