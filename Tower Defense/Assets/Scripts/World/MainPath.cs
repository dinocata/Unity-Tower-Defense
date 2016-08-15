using UnityEngine;
using System.Collections;

public class MainPath : MonoBehaviour {

    private Transform[] mainPath;

	// Use this for initialization
	void Start () {
        mainPath = new Transform[gameObject.transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            mainPath[i] = transform.GetChild(i);
        }
	}

    public Transform getPathNode(int index)
    {
        return mainPath[index];
    }

    public int getNodeCount()
    {
        return mainPath.Length;
    }
}
