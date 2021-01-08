using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimationController : MonoBehaviour
{
    private Animator animator;
    private Grabbable grabbable;
    private Survivor survivor;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        grabbable = GetComponent<Grabbable>();
        survivor = GetComponent<Survivor>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsGrabbed", grabbable.IsGrabed);
        animator.SetBool("IsRescued", survivor.IsRescued);
    }
}
