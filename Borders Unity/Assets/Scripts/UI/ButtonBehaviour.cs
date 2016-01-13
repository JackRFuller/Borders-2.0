using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonBehaviour : MonoBehaviour {

    [Header("Button Elements")]
    [SerializeField] private Text buttonText;
    [SerializeField] private Image buttonInner;
    [SerializeField] private Image buttonIcon;

    [Header("Transition Colors")]
    public Color[] transitionalColors = new Color[2];

	// Use this for initialization
	void Start () {

        
	
	}

    public void ButtonDown()
    {        
        buttonInner.color = transitionalColors[0];
        buttonIcon.color = transitionalColors[1];
        buttonText.color = transitionalColors[1];
    }

    public void ButtonUp()
    {
        buttonInner.color = transitionalColors[1];
        buttonIcon.color = transitionalColors[0];
        buttonText.color = transitionalColors[0];
    }
	
	
}
