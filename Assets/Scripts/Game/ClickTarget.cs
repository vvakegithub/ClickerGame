using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ClickTarget : MonoBehaviour, IDisposable
{
    [SerializeField] private GameSession _gameSession;

    private int _scoreFactor = 1;

    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(Click);
    }

    public void Dispose()
    {
        gameObject.GetComponent<Button>().onClick.RemoveListener(Click);
    }

    private void Click()
    {
        _gameSession.UpdateMoney(_scoreFactor);
    }
}
