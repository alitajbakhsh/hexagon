using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartTheGame : MonoBehaviour {
	public Slider ColorSlider;
	public Slider XrowSlider;
	public Slider YrowSlider;
	public Text ShowColorCount;
	public Text ShowXrowCount;
	public Text ShowYrowCount;

	void Update () {

		Floor.Xrow = (int)XrowSlider.value;
        Floor.Yrow = (int)YrowSlider.value;
        Floor.generatedcolor = new Color[(int)ColorSlider.value];
		ShowColorCount.text = ColorSlider.value.ToString();
		ShowXrowCount.text = XrowSlider.value.ToString();
		ShowYrowCount.text = YrowSlider.value.ToString();
	}
	public void StartPlaying()
    { 
		SceneManager.LoadScene(1);
    }
	public void QuitTheGame()
    {
		Application.Quit();
    }
}
