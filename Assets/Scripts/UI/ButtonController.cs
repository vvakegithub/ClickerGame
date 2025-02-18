using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject _mainUI;
    [SerializeField] private GameObject _missionsUI;
    [SerializeField] private GameObject _shopUI;

    public void EnableMainUI()
    {
        _missionsUI.SetActive(false);
        _shopUI.SetActive(false);
        _mainUI.SetActive(true);
    }

    public void EnableShopUI()
    {
        _missionsUI.SetActive(false);
        _mainUI.SetActive(false);
        _shopUI.SetActive(true);
    }

    public void EnableMissionsUI()
    {
        _mainUI.SetActive(false);
        _shopUI.SetActive(false);
        _missionsUI.SetActive(true);
    }
}
