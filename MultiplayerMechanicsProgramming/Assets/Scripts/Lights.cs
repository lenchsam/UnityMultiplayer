using UnityEngine;
public class Lights : MonoBehaviour
{
    private bool _isLit; // private backing field
    private FogOfWar fogOfWar;
    public bool IsLit{
		get { return _isLit; }
		set { 
            _isLit = value; 
            EnableFogOfWar();
        }
	}

    private void EnableFogOfWar(){
        fogOfWar = GameObject.FindAnyObjectByType<FogOfWar>();
        Debug.Log(fogOfWar.gameObject.name);
        //enable the fog of war around the light
        //Debug.Log("TEASSDHFHJKASDJKHLCAHJKSLDFHJKLA");
        fogOfWar.RevealFog(transform.position);
    }
}
