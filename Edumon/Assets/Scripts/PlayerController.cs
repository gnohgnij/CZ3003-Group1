using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private bool isMoving;
    private Vector2 input;

    public Animator animator;
    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;
    public LayerMask grassLayer;
    public WhereIsWaldoo whereIsWaldo;
    [SerializeField] Dialog dialog;

    private void Awake(){
        animator = GetComponent<Animator>();

        whereIsWaldo = GameObject.Find("WhereIsWaldo").GetComponent<WhereIsWaldoo>();

        if (whereIsWaldo.mapNumber == 1) {
            animator.transform.position = new Vector2(PlayerPrefs.GetFloat("Saved1XPosition"), PlayerPrefs.GetFloat("Saved1YPosition"));
            PlayerPrefs.DeleteKey("Saved4XPosition");
            PlayerPrefs.DeleteKey("Saved4YPosition");
        }
        else if (whereIsWaldo.mapNumber == 2) {
            PlayerPrefs.DeleteKey("Saved3XPosition");
            PlayerPrefs.DeleteKey("Saved3YPosition");
            animator.transform.position = new Vector2(PlayerPrefs.GetFloat("Saved2XPosition"), PlayerPrefs.GetFloat("Saved2YPosition"));
        }
        else if (whereIsWaldo.mapNumber == 3) {
            PlayerPrefs.DeleteKey("Saved2XPosition");
            PlayerPrefs.DeleteKey("Saved2YPosition");
            animator.transform.position = new Vector2(PlayerPrefs.GetFloat("Saved3XPosition"), PlayerPrefs.GetFloat("Saved3YPosition"));
        }
        else if (whereIsWaldo.mapNumber == 4) {
            PlayerPrefs.DeleteKey("Saved1XPosition");
            PlayerPrefs.DeleteKey("Saved1YPosition");
            animator.transform.position = new Vector2(PlayerPrefs.GetFloat("Saved4XPosition"), PlayerPrefs.GetFloat("Saved4YPosition"));
        }
    }

    // void Update()
    // {
    //     if (!isMoving)
    //     {
    //         input.x = Input.GetAxisRaw("Horizontal");
    //         input.y = Input.GetAxisRaw("Vertical");

    //         //remove diagonal movement
    //         if (input.x != 0) input.y = 0;

    //         if (input != Vector2.zero)
    //         {
    //             animator.SetFloat("moveX", input.x);
    //             animator.SetFloat("moveY", input.y);
    //             var targetPos = transform.position;
    //             targetPos.x += input.x;
    //             targetPos.y += input.y;

    //             if (IsWalkable(targetPos))
    //                 StartCoroutine(Move(targetPos));
    //         }
    //     }
    //     animator.SetBool("isMoving", isMoving);
    // }

    public void HandleUpdate()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //remove diagonal movement
            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos))
                    StartCoroutine(Move(targetPos));
            }
        }
        animator.SetBool("isMoving", isMoving);

        if(Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        //Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
        if(collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;

        CheckForEncounters();
    }

    bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactableLayer) != null)
        {
            return false;
        }

        return true;
    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer ) != null)
        {
           if(UnityEngine.Random.Range(1, 101) <= 10) //10% chance of encounter
           {
               Debug.Log("Random encounter occured");
               StartCoroutine(RandomEncountersManager.Instance.ShowEncounter(dialog));
           } 
        }
    }
}