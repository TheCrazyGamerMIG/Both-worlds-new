using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class ExtendingPlatform : MonoBehaviour
{
    public int extension = 1;
    public bool extended = false;
    private BoxCollider2D boxColl;
    private Animator anim;
    public GameObject[] extens;
    private ExtendingPlatformSegment segment;
    int i = 0;
    public bool reloading = false, deloading = false;
    float delay = 500f, wait = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxColl = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (extended)
        {
            
            boxColl.enabled = true;
            anim.ResetTrigger("ToOff");
            anim.SetTrigger("ToOn");
            if (extension>1)
            {
                anim.SetTrigger("ToMiddleman");
            }               
            
            if (extens != null && reloading)
            {
                i = 0;
                foreach (GameObject gm in extens)
                {

                    segment = gm.GetComponent<ExtendingPlatformSegment>();
                    if (i < extension-1)
                    {

                    segment.wait = (100 * i + 100f);
                    //print(i+", "+(extension-1));
                    
                    //print(segment.middleman);                    
                    segment.stretch = true;
                    }
                    if (i<extension-2)
                    {
                        segment.middleman = true;
                    } else
                    {
                        segment.middleman = false;
                    }
                    i++;

                }
                i = 0;
                reloading = false;
                wait = delay;
            }
        }
        else 
        {
            
            boxColl.enabled = false;
            if (wait>0)
            {
                wait--;
            }
            if (wait<=0)
            {
                anim.ResetTrigger("ToOn");
                anim.ResetTrigger("ToMiddleman");
                anim.SetTrigger("ToOff");
            }
            if (deloading && !reloading)
            {
                i = 0;
                foreach (GameObject gm in extens)
                {
                    segment = gm.GetComponent<ExtendingPlatformSegment>();
                    segment.wait = delay - (100 * i + 100f);
                    if (i < extension+1)
                    {
                        segment.middleman = false;
                    }
                    else
                    {
                        segment.middleman = true;
                    }
                    segment.stretch = false;
                    i++;
                }
                deloading = false;
            }
            i = 0;
        }
    }
}
