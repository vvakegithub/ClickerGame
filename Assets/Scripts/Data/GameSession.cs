using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class GameSession : MonoBehaviour, IDisposable
{
    public UserData Data => _userData;

    [SerializeField] private Connection _connection;

    private UserData _userData;

    private void Awake()
    {
        _userData.Money = _connection.GetScore();
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(_connection.ReferralId))
        {
            AwardReferralBonus(_connection.ReferralId);
        }
    }

    public void Dispose()
    {
        _connection.SendScore(_userData.Money);
    }

    public void UpdateMoney(int money)
    {
        _userData.Money += money;
    }

    private void AwardReferralBonus(string referralId)
    {
        // TODO: Implement logic to award a bonus to the referrer
    }
}

public struct UserData
{
    public int Money;
    public int Referals;
}
