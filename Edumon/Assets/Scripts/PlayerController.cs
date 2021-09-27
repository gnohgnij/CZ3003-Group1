using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private bool isMoving;
    private Vector2 input;

    public Animator animator;

    private void Update() 
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            
            //remove diagonal movement
            if (input.x != 0) input.y = 0;
            
            if (input != Vector2.zero)
            {
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                StartCoroutine(Move(targetPos));
            }
        }

        IEnumerator Move(Vector3 targetPos)
        {
            isMoving = true;
            
            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                if (targetPos.y > 0) {
                    animator.SetBool("moveRight", true);
                }
                yield return null;
            }

            animator.SetBool("moveRight", false);

            transform.position = targetPos;

            isMoving = false;
        }
        
        
        // if (Input.GetKey(KeyCode.LeftArrow))
        // {
        //     transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        // }
        // if (Input.GetKey(KeyCode.RightArrow))
        // {
        //     transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        // }
        // if (Input.GetKey(KeyCode.UpArrow))
        // {
        //     transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        // }
        // if (Input.GetKey(KeyCode.DownArrow))
        // {
        //     transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        // }
    }
}
