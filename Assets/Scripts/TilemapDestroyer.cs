using UnityEngine;
using UnityEngine.Tilemaps;
public class TilemapDestroyer : MonoBehaviour
{
    Tilemap tima;
    Vector3Int tilePos;
    Vector3 targetPoint;
    float errodeTimer = 0f, errodeTimerC = 10f;

    //Based on: Dynamic Tilemap Terrain Destruction in Unity - https://youtu.be/Lifz-qFOuWI?si=E1Gn-OohNJvQtdkH
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tima = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "DiggingTool")
        {
            
            targetPoint = collision.transform.position;
            print("Collision! "+ targetPoint);
            tilePos = Vector3Int.FloorToInt(targetPoint);
            errodeTimer = errodeTimerC;
            
            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (errodeTimer > 0f)
        {
            errodeTimer--;
        }
        if (errodeTimer <= 0f)
        {
            print("And? " + tilePos);
            tima.SetTile(tilePos, null);
            tima.SetTile(tilePos + Vector3Int.down, null);
            tima.SetTile(tilePos + Vector3Int.right, null);
            tima.SetTile(tilePos + Vector3Int.left, null);
            tima.SetTile(tilePos + Vector3Int.up, null);
            /*if (collision.GetComponent<CircleCollider2D>().radius >= 1f)
            {
                print("BIG SHOT!!");
                tima.SetTile(tilePos + Vector3Int.down * 2, null);
                tima.SetTile(tilePos + Vector3Int.down + Vector3Int.right, null);
                tima.SetTile(tilePos + Vector3Int.down + Vector3Int.left, null);
                tima.SetTile(tilePos + Vector3Int.down * 2 + Vector3Int.right, null);
                tima.SetTile(tilePos + Vector3Int.down * 2 + Vector3Int.left, null);
                tima.SetTile(tilePos + Vector3Int.up * 2, null);
            }*/
        }
    }
}
