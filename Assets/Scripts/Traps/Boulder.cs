using UnityEngine;

public class Boulder : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    public GameObject[] digBoxDirecion = new GameObject[8];
    public bool moveUp = false, moveDown = false, moveLeft = false, moveRight = false, isDigger = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        foreach (GameObject boxesYo in digBoxDirecion)
        {
            BoxCollider2D bd = boxesYo.GetComponent<BoxCollider2D>();
            bd.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocityX > 1f)
        {
            moveRight = true;
            moveLeft = false;
        }
        if (rb.linearVelocityX < -1f)
        {
            moveLeft = true;
            moveRight = true;
        }
        if (rb.linearVelocityY > 1f)
        {
            moveUp = true;
            moveDown = false;
        }
        if (rb.linearVelocityY < -1f)
        {
            moveDown = true;
            moveUp = false;
        }

        if (rb.linearVelocityX > 2f)
        {
            anim.SetBool("RollAnticlockwise", false);
            anim.SetBool("RollClockwise", true);
        } else if (rb.linearVelocityX < -2f)
        {
            anim.SetBool("RollClockwise", false);
            anim.SetBool("RollAnticlockwise", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDigger)
        {
            if (rb.GetComponent<CircleCollider2D>().radius < 1f)
            {
                if (moveUp)
                {
                    this.digBoxDirecion[0].GetComponent<BoxCollider2D>().enabled = true;
                }
                else if (moveDown)
                {
                    this.digBoxDirecion[2].GetComponent<BoxCollider2D>().enabled = true;
                }
                if (moveRight)
                {
                    this.digBoxDirecion[1].GetComponent<BoxCollider2D>().enabled = true;
                }
                else if (moveLeft)
                {
                    this.digBoxDirecion[4].GetComponent<BoxCollider2D>().enabled = true;
                }
            } else if (rb.GetComponent<CircleCollider2D>().radius >= 1f)
            {
                if (moveUp)
                {
                    this.digBoxDirecion[0].GetComponent<BoxCollider2D>().enabled = true;
                    this.digBoxDirecion[1].GetComponent<BoxCollider2D>().enabled = true;
                }
                else if (moveDown)
                {
                    this.digBoxDirecion[4].GetComponent<BoxCollider2D>().enabled = true;
                    this.digBoxDirecion[5].GetComponent<BoxCollider2D>().enabled = true;
                }
                if (moveRight)
                {
                    this.digBoxDirecion[2].GetComponent<BoxCollider2D>().enabled = true;
                    this.digBoxDirecion[3].GetComponent<BoxCollider2D>().enabled = true;
                }
                else if (moveLeft)
                {
                    this.digBoxDirecion[6].GetComponent<BoxCollider2D>().enabled = true;
                    this.digBoxDirecion[7].GetComponent<BoxCollider2D>().enabled = true;
                }

            }
        }
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        foreach (GameObject boxesYo in digBoxDirecion)
        {
            BoxCollider2D bd = boxesYo.GetComponent<BoxCollider2D>();
            bd.enabled = false;
        }
    }
}
