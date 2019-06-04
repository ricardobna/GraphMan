using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman_Movement : MonoBehaviour
{
    private float speed = 0.3f;
    private Vector2 pacmanDestination;

    //public bool visivel = true;
    // Start is called before the first frame update
    void Start()
    {
        pacmanDestination = transform.position;
    }

    private void FixedUpdate()
    {
        if(gameObject != null)
        {
        //Movendo o pacman
        Vector2 position = Vector2.MoveTowards(transform.position, pacmanDestination, speed);
        GetComponent<Rigidbody2D>().MovePosition(position);

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
            Debug.Log(x);
        //Input para mover o pacman
        if (y > 0 || Input.GetKey(KeyCode.UpArrow) && validPath(Vector2.up))
        {
            pacmanDestination = (Vector2)transform.position + Vector2.up;
        }
        else if (y < 0 || Input.GetKey(KeyCode.DownArrow) && validPath(Vector2.down))
        {
            pacmanDestination = (Vector2)transform.position + Vector2.down;
        }
        else if (x < 0 || Input.GetKey(KeyCode.LeftArrow) && validPath(Vector2.left))
        {
            pacmanDestination = (Vector2)transform.position + Vector2.left;
        }
        else if (x > 0 || Input.GetKey(KeyCode.RightArrow) && validPath(Vector2.right))
        {
            pacmanDestination = (Vector2)transform.position + Vector2.right;
        }

        //Animação
        Vector2 dir = pacmanDestination - (Vector2)transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
        }
        
    }

    bool validPath(Vector2 dir)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit.collider == GetComponent<Collider2D>() || hit.collider.name.Equals("Comida"));
    }

    public void destroyPacman()
    {
        //Game Over
        Destroy(gameObject);
    }
}
