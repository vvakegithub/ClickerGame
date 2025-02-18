using UnityEngine;

public class GameSession : MonoBehaviour
{
    public UserData Data => _userData;

    [SerializeField] private Connection _connection;

    private UserData _userData;

    private void Start()
    {
        _userData.Money = _connection.GetScore();
    }

    public void UpdateMoney(int money)
    {
        _userData.Money += money;
        _connection.SendScore(_userData.Money);
    }
}

public struct UserData
{
    public int Money;
}
