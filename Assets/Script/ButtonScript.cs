using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    //ゲーム終了:ボタンから呼び出す
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
             Application.Quit();//ゲームプレイ終了
#endif
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Map_1");
    }
}
