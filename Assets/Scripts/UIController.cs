using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
	public EasyTween EasyTweenReference;
	public GameObject LeaderboardPanel;
	public GameObject DrawerBtn;
	public GameObject Settingpanel;
    public GameObject DatenschutzPanel;


    void Start()
	{
		EasyTweenReference.OpenCloseObjectAnimation();
	}

	public void StartScanning()
	{
		EasyTweenReference.OpenCloseObjectAnimation();
	}
	public void LeaderBoardOn()
	{
		LeaderboardPanel.SetActive(true);
		DrawerBtn.SetActive((false));
	}
	public void LeaderBoardOff()
	{
		LeaderboardPanel.SetActive(false);
		DrawerBtn.SetActive((true));
	}
	public void SettingON()
	{
		Settingpanel.SetActive(true);
		DrawerBtn.SetActive((false));
	}
	public void SettingOff()
	{
		Settingpanel.SetActive(false);
		DrawerBtn.SetActive((true));
	}

    public void Datenschutz(bool status)
    {
        DatenschutzPanel.SetActive(status);
    }



}
