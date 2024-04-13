using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Transform parent;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem system; 
    void Start()
    {
       parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation=Quaternion.Euler(-90f+parent.rotation.z*2,0f,transform.rotation.z);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y,0f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Death")
        {
            Destroy(gameObject);
           /* CharactersPlace.Instance.changeAmount(-1);*/
            Instantiate(system,transform.position,Quaternion.identity);
        }
        if(collision.transform.tag == "Gem")
        {
            Destroy(collision.gameObject);
        }
        if (collision.transform.tag == "Character")
        {
            CharactersPlace.Instance.changeAmount(1);
            Destroy(collision.gameObject);
        }
    }
}
