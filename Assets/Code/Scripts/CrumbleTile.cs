using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class CrumbleTile : MonoBehaviour
{
    [SerializeField] private float crumbleTime = 1f;
    private Tilemap tileMap;
    private Camera m_camera;

    void Start()
    {
        tileMap = GetComponent<Tilemap>();
        m_camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3Int tilePos = Vector3Int.FloorToInt(m_camera.ScreenToWorldPoint(Input.mousePosition));
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
        Vector3Int tilePos = Vector3Int.FloorToInt(collision.transform.position);
        tilePos.y -= 1;
        tilePos.z = 0;
        IEnumerator coroutine = DeleteTile(tilePos);
        StartCoroutine(coroutine);
    }

    IEnumerator DeleteTile(Vector3Int tilePos)
    {
        yield return new WaitForSeconds(crumbleTime);
        Debug.Log("Crumbling Tile: " + tilePos);
        tileMap.SetTile(tilePos, null);
    }
}
