using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialControl : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material material;
    // Start is called before the first frame update
    void Start()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        material = skinnedMeshRenderer.materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            UpdateAlpha();
            Debug.Log("Key K pressed");
        }
    }

    void UpdateAlpha()
    {
        //Material material = skinnedMeshRenderer.materials[0];
        Color newColor = new Color(1f, 0f, 0f, 0.5f);
        material.color = newColor;
    }
}
