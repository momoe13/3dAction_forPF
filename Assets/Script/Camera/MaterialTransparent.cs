using UnityEngine;

public class MaterialTransparent : MonoBehaviour
{
    private Color color = Color.white;

    //親 子オブジェクトを格納。
    private MeshRenderer[] meshRenderers;
    private MaterialPropertyBlock m_mpb;

    public MaterialPropertyBlock mpb
    {
        get { return m_mpb ?? (m_mpb = new MaterialPropertyBlock()); }
    }

    void Awake()
    {
        //子オブジェクトと親オブジェクトのmeshrendererを取得
        meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
    }

    /// <summary>
    /// 透過処理
    /// </summary>
    public void ClearMaterialInvoke()
    {
        color.a = 0.15f;

        mpb.SetColor(Shader.PropertyToID("_Color"), color);
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse ZWrite");
            meshRenderers[i].SetPropertyBlock(mpb);
        }
    }
    public void NotClearMaterialInvoke()
    {
        color.b = 1f;
        mpb.SetColor(Shader.PropertyToID("_Color"), color);
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].GetComponent<Renderer>().material.shader = Shader.Find("Mobile/Diffuse");
            meshRenderers[i].SetPropertyBlock(mpb);
        }
    }
}
