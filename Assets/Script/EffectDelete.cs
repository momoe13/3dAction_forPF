using UnityEngine;

public class EffectDelete : MonoBehaviour
{
    public void EffectDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
