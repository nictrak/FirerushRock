using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationController : MonoBehaviour
{
    private Animator animator;
    private Breakable breakable;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        breakable = GetComponent<Breakable>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void LateUpdate()
    {
        animator.SetInteger("Toughness", breakable.Toughness);
    }
}
