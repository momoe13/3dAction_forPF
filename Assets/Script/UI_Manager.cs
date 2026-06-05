using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Slider slider;

    Vector2 plHp;
    int nowHp,maxHp;

    [SerializeField] Text scoreTxt;
    int score = 0;
    int maxScore = 3;

    [SerializeField] Text FinishTxt;

    bool isGame = true;


    private void Start()
    {
        slider.value = 1;
        scoreTxt.text = score+"/"+maxScore;
        FinishTxt.enabled = false;
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

    private void Finish()
    {
        FinishTxt.enabled=true;
    }

    public void SetScore(){ 
        score++;
        scoreTxt.text = score + "/" + maxScore;

        if (score >= maxScore) Finish(); 
    }
}
