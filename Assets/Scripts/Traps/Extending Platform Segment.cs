using UnityEngine;

public class ExtendingPlatformSegment : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D boxColl;
    public bool middleman = false, stretch = false;
    public float wait = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxColl = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wait > 0)
        {
            wait--;
        }
        else if (stretch && wait <= 0)
        {
                
            anim.ResetTrigger("ToOff");
            /*
            anim.ResetTrigger("ToOn");
            anim.ResetTrigger("ToMiddleman");*/
            if (middleman)
            {
                anim.SetTrigger("ToOn");
                anim.SetTrigger("ToMiddleman");
            }
            else
            {
                //anim.SetTrigger("ToEnd");
                anim.SetTrigger("ToOn");
            }
            boxColl.enabled = true;
        }
        else if (!stretch)
        {
            anim.ResetTrigger("ToMiddleman");
            anim.ResetTrigger("ToOn");
            anim.SetTrigger("ToOff");
            boxColl.enabled = false;
        }
        

    }
}
