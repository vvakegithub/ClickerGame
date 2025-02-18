using UnityEngine;
using UnityEngine.UI;

public class ReferalLink : MonoBehaviour
{
    [SerializeField] private Connection _connection;
    [SerializeField] private Text _text;

    private void Start()
    {
        _text.text = _connection.ReferralLink;
    }
}
