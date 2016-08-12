using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour {
    
    public Color SelectedColor;
    public bool IsSelected;

    private Image _image;
    private Color _normalColor;

	// Use this for initialization
	void Awake () {
        _image = GetComponent<Image>();
        _normalColor = _image.color;
	}

    public void Select(bool isSelected) {
        if (isSelected)
        {
            _image.color = SelectedColor;
        }
        else
        {
            _image.color = _normalColor;
        }
    }


}
