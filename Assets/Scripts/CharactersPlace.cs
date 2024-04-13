using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CharactersPlace : MonoBehaviour
{
    public static CharactersPlace Instance;
    [SerializeField] private int amount;
    private const float SCALE=0.1f;
    [SerializeField] SplineComputer _splineComputer;
    [SerializeField] private ObjectController _objectController;
    private List<SplinePoint> _points=new List<SplinePoint>();
    void Start()
    {
        if (Instance == null) Instance = this;
        else if(Instance != null) Destroy(gameObject);
        _splineComputer=GetComponent<SplineComputer>();
        _objectController = GetComponent<ObjectController>();
    
    }

    // Update is called once per frame
    void Update()
    {
        SetNormals();
        if(_objectController.spawnCount!=amount) _objectController.spawnCount=amount;
        
    }
    public void SetPoints(Vector3 point)
    {
        Debug.Log(transform.localPosition);
        _points.Add(new SplinePoint(new Vector3(((point.x)*(transform.localScale.x/400))- (((point.x+100) * (transform.localScale.x / 200))), (((point.y)*(transform.localScale.z-0.5f)/200)- (((point.y+50) * ((transform.localScale.z-0.5f) / 100))))+0.5f,0f)));
       

    }
    public void SetNormals()
    {
        for (int i = 0; i < _points.Count; i++)
        {
            _splineComputer.SetPointNormal(i, Vector3.up,SplineComputer.Space.World);
        }
    }
    public void SetSpline()
    {
        _splineComputer.SetPoints(_points.ToArray(), SplineComputer.Space.Local);
    }
    public void ReSetPoints()
    {
        _points.Clear();
    }
    public void changeAmount(int count)
    {
        amount += count;
    }
}
