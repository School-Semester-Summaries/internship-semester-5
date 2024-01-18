using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

public class SaveButtonClick : MonoBehaviour // i never really made this, everything gets saved automatically
{
	public Button SaveButton;

	void Start()
	{
		Button btn = SaveButton.GetComponent<Button>();
		btn.onClick.AddListener(Save);
	}
	void Save()
	{
		// Read Editor.txt save
		// Put all data in new textfile
		// user can give name to this textfile
		//Debug.Log("Saved!");
		// empty Editor.txt
	}
}