using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FirefighterAnimationController : NetworkBehaviour
{
    private bool isMove;
    private int direction;
    private Animator animator;
    private PlayerControl playerControl;
    private PlayerDirection playerDirection;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerControl = GetComponent<PlayerControl>();
        playerDirection = GetComponent<PlayerDirection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            isMove = playerControl.IsMove;
            if (playerDirection.Direction == "left")
            {
                direction = 0;
            }
            else if (playerDirection.Direction == "right")
            {
                direction = 1;
            }
            else if (playerDirection.Direction == "up")
            {
                direction = 2;
            }
            else if (playerDirection.Direction == "down")
            {
                direction = 3;
            }
            animator.SetBool("IsMove", isMove);
            animator.SetInteger("Direction", direction);
        }
    }
}
