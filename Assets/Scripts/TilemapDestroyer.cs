using UnityEngine;
using UnityEngine.Tilemaps;
public class TilemapDestroyer : MonoBehaviour
{
    Tilemap tima;
    Vector3Int tilePos;
    Vector3 targetPoint;
    float errodeTimer = 0f, errodeTimerC = 10f;
    public GameObject digParticleObject;
    public DigParticle digScript;
    public GameObject[] particleList = new GameObject[256];
    public int particleCount = 0;
    public float particleDespawnTimer = 120f, particleDespawnTimerC = 120f;

    //Based on: Dynamic Tilemap Terrain Destruction in Unity - https://youtu.be/Lifz-qFOuWI?si=E1Gn-OohNJvQtdkH
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tima = GetComponent<Tilemap>();
        //digParticleObject = GetComponent<GameObject>();
        //digScript = digParticleObject.GetComponent<DigParticle>();
    }

    // Update is called once per frame
    void Update()
    {
        particleDespawnTimerC--;
        if (particleDespawnTimerC <= 0)
        {
            particleCount = 0;
        }
        if (errodeTimer > 0f)
        {
            errodeTimer--;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //for (int i = 0;i<collision.contactCaptureLayers.value;i++)
        //{
        //ContactPoint2D contact = collision.contacts[0];

        if (collision.transform.tag == "DiggingTool")
            {

                targetPoint = collision.transform.position;
                //print("Collision! "+ targetPoint);
                tilePos = Vector3Int.FloorToInt(targetPoint);
                errodeTimer = errodeTimerC;


            }
        //}
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        
        if (errodeTimer <= 0f)
        {
            //print("And? " + tilePos);

            for (int i=0;i<4;i++)
            {
                particleList[particleCount] = Instantiate(digParticleObject);
                particleList[particleCount].transform.position = tilePos;
                digScript = particleList[particleCount].GetComponent<DigParticle>();
                switch (Random.Range(0, 2))
                {
                    case 0:
                        digScript.moveUp = true;
                        digScript.moveDown = false;
                        break;
                    case 1:
                        digScript.moveDown = true;
                        digScript.moveUp = false;
                        break;
                }
                switch (Random.Range(0, 2))
                {
                    case 0:
                        digScript.moveLeft = true;
                        digScript.moveRight = false;
                        break;
                    case 1:
                        digScript.moveRight = true;
                        digScript.moveLeft = false;
                        break;
                }
                particleCount++;
            }

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
