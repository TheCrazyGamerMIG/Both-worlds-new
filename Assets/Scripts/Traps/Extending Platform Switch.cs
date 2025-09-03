using UnityEngine;

public class ExtendingPlatformSwitch : MonoBehaviour
{
    public bool pressed = true, singular = false;
    private Animator anim;
    private BoxCollider2D boxColl;
    public GameObject[] platforms;//EVERY switch can have its OWN set of platforms to switch!
    private ExtendingPlatform exT;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed)
        {
            anim.ResetTrigger("ToOff");
            if (singular)
            {
                anim.SetTrigger("ToMiddleman");
            }
            else
            {
                anim.ResetTrigger("ToMiddleman");
                anim.ResetTrigger("ToOn");
                anim.SetTrigger("ToOn");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        pressed = true;            
            if (platforms != null)
            {
                foreach (GameObject gm in platforms)
                {
                    exT = gm.GetComponent<ExtendingPlatform>();
                    exT.extended = true;
                    exT.reloading = true;
                    exT.deloading = false;
                }
            } 
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        pressed = false;
        anim.ResetTrigger("ToMiddleman");
        anim.ResetTrigger("ToOn");
        anim.SetTrigger("ToOff");
        foreach (GameObject gm in platforms)
        {
            exT = gm.GetComponent<ExtendingPlatform>();
            exT.extended = false;
            exT.reloading = false;
            exT.deloading = true;
        }
    }
}
