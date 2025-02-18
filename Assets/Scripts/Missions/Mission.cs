using UnityEngine;
using UnityEngine.UI;

public class Mission : MonoBehaviour
{
    [SerializeField] private int _moneyMission;
    [SerializeField] private int _totalReferals = 0;

    [SerializeField] private Text _moneyText;
    [SerializeField] private Image _claimImage;


    private void Awake()
    {
        _moneyText.text = _moneyMission.ToString();
        CheckMissionComplete();
    }

    private void CheckMissionComplete()
    {

    }
}
