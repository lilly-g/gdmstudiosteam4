using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class CrumbleTile : MonoBehaviour
{
    [SerializeField] private float crumbleTime = 0.5f;
    [SerializeField] private float respawnTime = 1f;
    [SerializeField] private bool canRespawn;
    [SerializeField] private bool onlyBreaksOnDash;
    private Tilemap tileMap;
    private Camera m_camera;
    private List<Vector3Int> crumblingTiles = new List<Vector3Int>();

    void Start()
    {
        tileMap = GetComponent<Tilemap>();
        m_camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3Int tilePos = tileMap.WorldToCell(m_camera.ScreenToWorldPoint(Input.mousePosition));
            tilePos.z = 0;
            Debug.Log(tilePos);
            Debug.Log(tileMap.GetTile(tilePos));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCrumbling(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        HandleCrumbling(collision);
    }

    public void HandleCrumbling(Collision2D collision)
    {
        //ContactPoint2D[] contacts = new ContactPoint2D[];
        float localCrumbleTime = collision.gameObject.GetComponent<PlayerController>().isDashing ? 0f : crumbleTime;
        if (collision.gameObject.GetComponent<PlayerController>().isDashing || !onlyBreaksOnDash)
        {
            foreach (ContactPoint2D hit in collision.contacts)
            {
                Vector3Int tilePos = tileMap.WorldToCell(new Vector3(hit.point.x + 0.1f * hit.normal.x, hit.point.y + 0.1f * hit.normal.y, 0));
                if (!crumblingTiles.Contains(tilePos))
                {
                    crumblingTiles.Add(tilePos);
                    IEnumerator crumbleCoroutine = DeleteTile(tilePos, localCrumbleTime);
                    StartCoroutine(crumbleCoroutine);
                }

            }
        }
    }

    IEnumerator DeleteTile(Vector3Int tilePos, float crumbleTime)
    {
        yield return new WaitForSeconds(crumbleTime);
        TileBase originalTile = tileMap.GetTile(tilePos);
        tileMap.SetTile(tilePos, null);
        crumblingTiles.Remove(tilePos);

        if (canRespawn)
        {
            IEnumerator respawnCoroutine = Respawn(tilePos, originalTile);
            StartCoroutine(respawnCoroutine);
        }
    }

    IEnumerator Respawn(Vector3Int tilePos, TileBase tile)
    {
        yield return new WaitForSeconds(respawnTime);
        tileMap.SetTile(tilePos, tile);
    }
}
