using UnityEngine;

public class MaterialControl : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material material;
    // Start is called before the first frame update

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        UpdateAlpha();
    //        Debug.Log("Key K pressed");
    //    }
    //}

    public void UpdateAlpha(Color newColor)
    {
        //Material material = skinnedMeshRenderer.materials[0];
        //newColor = new Color(1f, 0.5f, 0f, 1f);
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        material = skinnedMeshRenderer.materials[0];
        material.color = newColor;
    }
}
