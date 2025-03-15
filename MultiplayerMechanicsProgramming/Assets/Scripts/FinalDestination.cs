using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class FinalDestination : MonoBehaviour
{
    public UnityEvent GameOverEvent;
    [SerializeField] Lights[] _allLightScripts;
    void Start()
    {
        if (GameOverEvent == null)
        {
            GameOverEvent = new UnityEvent();
        }
        GameOverEvent.AddListener(GameOver);
    }

    void OnTriggerEnter(Collider other){
        if(other.tag != "player")return;

        //if all lights are triggered
        foreach (Lights lightScript in _allLightScripts){
            if(!lightScript.IsLit) return;
        }
        Debug.Log("ENDING");
        GameOverEvent.Invoke();
    }
    void GameOver(){
        Debug.Log("GAME OVER");
        //enable game over UI
        //restart game (resed FOW, player health etc)
    }
}
