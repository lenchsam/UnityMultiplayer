using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ViewLight : NetworkBehaviour
{
    [SerializeField] SphereCollider _SC;
    [SerializeField] float _radius = 2.0f;
    bool _makeVisible = false;
    GameObject _light;

    private void Awake()
    {
        _SC = GetComponent<SphereCollider>();
    }
    private void Start()
    {
        _SC.isTrigger = true;
        _SC.radius = _radius;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Light")
        {
            return;
        }
        //Debug.Log("make visible");
        _light = other.gameObject;
        _makeVisible = true;
        _light.transform.GetChild(0).gameObject.tag = "Untagged";
        _light.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Default");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Lights>() == null){return;}
        if (other.GetComponent<Lights>().IsLit == true){return;}
        _light = other.gameObject;
        //Debug.Log("exit");
        _makeVisible = false;
        _light.transform.GetChild(0).tag = "Light";
        _light.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Light");
    }
}
