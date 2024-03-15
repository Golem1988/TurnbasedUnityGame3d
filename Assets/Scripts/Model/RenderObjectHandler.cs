using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class RenderObjectHandler : MonoBehaviour
{
	//标记是否有赋值过
	public int setflag;
	static int flagShadowHeight = 1;
	static int flagMatColor = 2;
	static int flagOutline = 4;

	public List<Material> matList = new List<Material>();

    [HideInInspector]
    public float _shadowHeight;

    [HideInInspector]
    public Color _matColor;

    [HideInInspector]
    public float _outline;

	public bool isIndependentUpdate = false;

	List<Action<Material>> actionList = new List<Action<Material>>();
	void Awake()
	{
		AddBatchObject(this.GetInstanceID(), this);
	}

	void OnDestroy()
	{
		RemoveBatchObject(this.GetInstanceID());
	}
	public float shadowHeight
	{
		set
		{
			setflag |= flagShadowHeight;
			_shadowHeight = value;
            SetMatsFloat("_Terrain", _shadowHeight);
		}
	}
	public Color matColor
	{
		set
		{
			setflag |= flagMatColor;
			_matColor = value;
            SetMatsColor("_ColorAlpha", _matColor);
		}
	}
	public float outline
	{
		set
		{
			setflag |= flagOutline;
			_outline = value;
            SetMatsFloat("_Outline", _outline); 
		}
	}

	public void UpdateMatValue(Material mat)
	{
		if ((setflag & flagShadowHeight) > 0)
		{
			mat.SetFloat("_Terrain", _shadowHeight);
		}
		if ((setflag & flagMatColor) > 0)
		{
			mat.SetColor("_ColorAlpha", _matColor);
		}
		if ((setflag & flagOutline) > 0)
		{
			mat.SetFloat("_Outline", _outline);
		}
	}

	//public void SetMats(LuaTable luaTable)
	//{
	//	matList.Clear();
	//	for (int i = 1; i <= luaTable.Length; i++)
	//	{
	//		var mat = luaTable[i] as Material;
	//		UpdateMatValue(mat);
	//		matList.Add(mat);
	//	}
	//}

	public void DoMatsActionList()
	{
		if (matList.Count > 0 && actionList.Count > 0)
		{
            for (int k = 0; k < matList.Count; k++)
            {
                var mat = matList[k];
                for (int i = 0; i < actionList.Count; i++)
                {
                    var action = actionList[i];
                    action(mat);
                }
            }
        }
		actionList.Clear();
	}

	public void DoMatsAction(Action<Material> action)
	{
		if (matList.Count > 0)
		{
            for (int i = 0; i < matList.Count; i++)
            {
                action(matList[i]);
            }
		}
	}
	
	private void PushMatsAction(Action<Material> action)
	{
		actionList.Add(action);
	}

	public void SetMatsColor(string k, Color v)
	{
        for(int i = 0; i < matList.Count; i++)
        {
            matList[i].SetColor(k, v);
        }
	}

	public void SetMatsFloat(string k, float v)
	{
        for(int i = 0; i < matList.Count; i++)
        {
            matList[i].SetFloat(k, v);
        }
	}

	public void SetMatsInt(string k, int v)
	{
        for (int i = 0; i < matList.Count; i++)
        {
            matList[i].SetInt(k, v);;
        }
	}

	public void SetMatsVector(string k, Vector4 v)
	{
        for (int i = 0; i < matList.Count; i++)
        {
            matList[i].SetVector(k, v); ;
        }
	}

	//public void RenderAddMat(Renderer rend, Material mat)
	//{
	//	if ((rend==null) || (mat==null))
	//	{
	//		return;
	//	}
	//	var addMat = Utils.FUNCTAG_RenderAddMat(rend, mat);
	//	UpdateMatValue(addMat);
	//	matList.Add(addMat);
	//}

	public void AddRenderObj(Renderer rend)
	{
		if (rend==null)
		{
			return;
		}
        
        for (int i = 0; i < rend.materials.Length; i++)
        {
            Material mat = rend.materials[i];
			if (!matList.Contains(mat))
			{
				UpdateMatValue(mat);
				matList.Add(mat);
			}
		}
	}

	//public void DelRenderObj(Renderer rend, int oriMatCnt)
	//{
	//	if (rend == null)
	//	{
	//		return;
	//	}
	//	RenderResizeMatCnt(rend, oriMatCnt);

 //       for (int i = 0; i < rend.materials.Length; i++)
 //       {
 //           Material mat = rend.materials[i];
	//		if (matList.Contains(mat))
	//		{
	//			matList.Remove(mat);
	//		}
	//	}
	//}

	public void RenderDelMat(Renderer rend, string name)
	{
		if (rend == null)
		{
			return;
		}
		var newMats = new List<Material>();
		var delMats = new List<Material>();

        for (int i = 0; i < rend.materials.Length; i++)
        {
            Material mat = rend.materials[i];
			if (mat.name.IndexOf(name) >= 0)
			{
				delMats.Add(mat);
			}
			else
			{
				newMats.Add(mat);
			}
		}

		rend.materials = newMats.ToArray();
		if (matList.Count > 0)
		{
            for (int i = 0; i < delMats.Count; i++)
            {
                Material mat = delMats[i];
				matList.Remove(mat);
				UnityEngine.Object.Destroy(mat);
			}
		}
	}

	//public void RenderResizeMatCnt(Renderer rend, int cnt)
	//{
	//	if (rend == null)
	//	{
	//		return;
	//	}
	//	var mats = Utils.FUNCTAG_RenderResizeMatCnt(rend, cnt);
	//	if (mats != null)
	//	{
 //           for (int i = 0; i < mats.Length; i++)
 //           {
 //               Material mat = mats[i];
	//			if (matList.Contains(mat))
	//			{
	//				matList.Remove(mat);
	//			}
	//			UnityEngine.Object.Destroy(mat);
	//		}
	//	}
	//}

	//public void FadeShow(string name, float time)
	//{
 //       for (int i = 0; i < matList.Count; i++)
 //       {
 //           Material mat = matList[i];
 //           if (mat.name.IndexOf(name) >= 0)
 //           {
 //               DG.Tweening.ShortcutExtensions.DOKill(mat, false);
 //               if (time > 0)
 //               {
 //                   mat.SetFloat("_Alpha", 0);
 //                   var tweener = DG.Tweening.ShortcutExtensions.DOFloat(mat, 1, "_Alpha", time);
 //                   DG.Tweening.TweenSettingsExtensions.SetEase(tweener, DG.Tweening.Ease.Linear);
 //                   DG.Tweening.TweenSettingsExtensions.SetUpdate(tweener, isIndependentUpdate);
 //               }
 //               else
 //               {
 //                   mat.SetFloat("_Alpha", 1);
 //               }
 //           }
 //       }
	//}

	//public void FadeHide(string name, float time)
	//{
 //       for (int i = 0; i < matList.Count; i++)
 //       {
 //           Material mat = matList[i];
	//		if (mat.name.IndexOf(name) >= 0)
	//		{
	//			DG.Tweening.ShortcutExtensions.DOKill(mat, false);
	//			if (time > 0)
	//			{
	//				mat.SetFloat("_Alpha", 1);
	//				var tweener = DG.Tweening.ShortcutExtensions.DOFloat(mat, 0, "_Alpha", time);
	//				DG.Tweening.TweenSettingsExtensions.SetEase(tweener, DG.Tweening.Ease.Linear);
	//				DG.Tweening.TweenSettingsExtensions.SetUpdate(tweener, isIndependentUpdate);
	//			}
	//			else
	//			{
	//				mat.SetFloat("_Alpha", 0);
	//			}
	//		}
	//	}
	//}
	
	//BatchCall
	private static Dictionary<int, RenderObjectHandler> batchObjs = new Dictionary<int, RenderObjectHandler>();
	private static void AddBatchObject(int id, RenderObjectHandler obj)
	{
		batchObjs[id] = obj;
	}

	private static void RemoveBatchObject(int id)
	{
		batchObjs[id] = null;
	}

	public static RenderObjectHandler GetBatchObject(int id)
	{
		RenderObjectHandler obj;
		if (batchObjs.TryGetValue(id, out obj))
		{
			return obj;
		}
		return null;
	}
}