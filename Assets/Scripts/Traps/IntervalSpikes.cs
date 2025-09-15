using UnityEngine;

public class IntervalSpikes : MonoBehaviour
{
    public float intervalOff = 800f, intervalOn = 800f, timer;
    public bool inside = true;
    private Animator anim;
    private BoxCollider2D boxColl;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer--;
        if (inside)
        {
            boxColl.enabled = false;
        } else
        {
            boxColl.enabled = true;
        }
        if (timer <= 0)
        {
            if (inside)
            {
                timer = intervalOn;
                anim.ResetTrigger("ToOff");
                anim.SetTrigger("ToOn");
                inside = false;
            }
            else
            {
                timer = intervalOff;
                anim.ResetTrigger("ToOn");
                anim.SetTrigger("ToOff");
                inside = true;
            }
        }
    }
}
