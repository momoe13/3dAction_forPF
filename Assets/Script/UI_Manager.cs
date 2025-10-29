using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Slider slider;

    Vector2 plHp;
    int nowHp,maxHp;

    private void Start()
    {
        slider.value = 1;

    }

    private void Update()
    {
        HpUpdate();
    }

    private void HpUpdate()
    {
        plHp = player.SetHP();
        nowHp = (int)plHp.x;
        maxHp = (int)plHp.y;
        slider.value = (float)nowHp / (float)maxHp;

    }
}
