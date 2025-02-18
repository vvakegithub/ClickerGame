using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Score : MonoBehaviour
{
    [SerializeField] private GameSession _gameSession;

    private void Update()
    {
        gameObject.GetComponent<Text>().text = _gameSession.Data.Money.ToString();
    }
}
