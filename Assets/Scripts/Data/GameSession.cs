using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public UserData Data => _userData;

    [SerializeField] private Connection _connection;

    private UserData _userData;
    private string _filePath;

    private void Awake()
    {
        _filePath = Path.Combine(Application.persistentDataPath, "userData.json");
        LoadUserData();
        //_userData.Money = _connection.GetScore();
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(_connection.ReferralId))
        {
            AwardReferralBonus(_connection.ReferralId);
        }
    }

    public void OnDestroy()
    {
       // _connection.SendScore(_userData.Money);
        SaveUserData();
    }

    public void UpdateMoney(int money)
    {
        _userData.Money += money;
        SaveUserData();
    }

    private void AwardReferralBonus(string referralId)
    {
        // Implement the referral bonus logic here
    }

    private void SaveUserData()
    {
        string json = JsonUtility.ToJson(_userData);
        File.WriteAllText(_filePath, json);
    }

    private void LoadUserData()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            _userData = JsonUtility.FromJson<UserData>(json);
        }
        else
        {
            _userData = new UserData();
        }
    }
}

[System.Serializable]
public struct UserData
{
    public int Money;
    public int Referals;
}

