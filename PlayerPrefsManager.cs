using UnityEngine;
using System.Collections;

public class PlayerPrefsManager : MonoBehaviour {

	const string MASTER_VOLUME_KEY ="master_volume";
	const string FIXED_JOYSTICK ="fixed_joystick";

	public static void SetMasterVolume (float volume){
		if (volume >= 0f && volume <= 1f){	
			PlayerPrefs.SetFloat (MASTER_VOLUME_KEY, volume);
		}else{
			Debug.LogError ("Master volume out of range");
		}
	}
	
	public static float GetMasterVolume(){
		return PlayerPrefs.GetFloat (MASTER_VOLUME_KEY);
	}

	public static void InvertFixedJoystick(){
		int aim = PlayerPrefs.GetInt(FIXED_JOYSTICK);
		if (aim == 0)
			PlayerPrefs.SetInt(FIXED_JOYSTICK, 1);
		else 
			PlayerPrefs.SetInt(FIXED_JOYSTICK, 0);	
	}

	public static int GetFixedJoystick(){
		return PlayerPrefs.GetInt(FIXED_JOYSTICK);
	}
}
