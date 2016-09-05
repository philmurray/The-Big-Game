using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testReset : MonoBehaviour {
    
    private List<GameObject> copy;

	// Use this for initialization
	void Start () {
        copy = new List<GameObject>();
        foreach (Transform child in transform)
        {
            copy.Add(child.gameObject);
        }
        copyThings();
    }
    void copyThings() {
        List<GameObject> newCopy = new List<GameObject>();
        foreach (GameObject go in copy)
        {
            GameObject goCopy = Instantiate(go, transform) as GameObject;
            goCopy.SetActive(false);
            newCopy.Add(goCopy);
        }
        copy = newCopy;
    }
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (Transform t in transform)
            {
                if (t.gameObject.activeSelf)
                {
                    Destroy(t.gameObject);
                }
            }
            copy.ForEach(g => g.SetActive(true));
            copyThings();
        }
    }
}
