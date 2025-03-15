using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    [SerializeField] GameObject _fogOfWarGameObject;
    [SerializeField] Transform _player;
    [SerializeField] LayerMask _fogLayer;
    [SerializeField] float _radius = 5.0f;
    [SerializeField] float _radiusSqr { get { return _radius * _radius; } }
    private Mesh _mesh;
    private Vector3[] _vertices;
    private Color[] _colours;

    bool playerInRange = false;

    void Start()
    {
        Initialise();
    }

    void Update()
    {
        RevealFog(_player.position);
        //if player is out of range of all points reset fog color to black
        if (!playerInRange)
        {
            for (int i = 0; i < _colours.Length; i++)
            {
                _colours[i].a = 1.0f; //1 = black
            }
        }

        UpdateColour();
    }

    public void RevealFog(Vector3 position){
        //check if the player is in range of any FOW vertex
        playerInRange = false;
        Ray ray = new Ray(transform.position, position - transform.position);
        RaycastHit hit;
        
        //raycast to find intersection with the fog layer
        if (Physics.Raycast(ray, out hit, 500, _fogLayer, QueryTriggerInteraction.Collide))
        {
            playerInRange = true;
            for (int i = 0; i < _vertices.Length; i++)
            {
                Vector3 worldPos = _fogOfWarGameObject.transform.TransformPoint(_vertices[i]);
                float dist = Vector3.SqrMagnitude(worldPos - hit.point);

                //if the vertex is within the radius reveal fog
                if (dist < _radiusSqr)
                {
                    float alpha = Mathf.Min(_colours[i].a, dist / _radiusSqr);
                    _colours[i].a = alpha;
                }
                else
                {
                    //keep fog black
                    _colours[i].a = 1.0f;
                }
            }
        }
    }
    void Initialise()
    {
        _fogOfWarGameObject = GameObject.Find("Fog");
        _mesh = _fogOfWarGameObject.GetComponent<MeshFilter>().mesh;
        _vertices = _mesh.vertices;
        _colours = new Color[_vertices.Length];

        //set initial fog color to black
        for (int i = 0; i < _colours.Length; i++)
        {
            _colours[i] = Color.black;
        }

        UpdateColour();
    }

    void UpdateColour()
    {
        _mesh.colors = _colours;
    }
}
