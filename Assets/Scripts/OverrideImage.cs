using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverrideImage : MonoBehaviour {

	private Image img;

	private static int idMainTex = Shader.PropertyToID("_MainTex");
	private MaterialPropertyBlock block;

	[SerializeField]
	private Texture texture = null;
	public Texture overrideTexture
	{
		get { return texture; }
		set
		{
			texture = value;
			if (block == null)
			{
				Init();
			}
			block.SetTexture(idMainTex, texture);
		}
	}
	void Awake()
	{
		Init();
		overrideTexture = texture;
	}

	void OnValidate()
	{
		overrideTexture = texture;
	}

	void Init()
	{
		block = new MaterialPropertyBlock();
		if( img == null ){
			img = GetComponent<Image>();
		}
		img.material.mainTexture = overrideTexture;

	}

}
