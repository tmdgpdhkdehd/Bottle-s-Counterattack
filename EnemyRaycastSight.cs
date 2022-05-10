using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRaycastSight : MonoBehaviour
{
    public LayerMask layerMask;
    public float fov = 90f;
    public int rayCount = 50;
    public float viewDistance = 50f;
    float startingAngle;
    Vector3 origin;

    Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 offsetPos = transform.parent.position;


        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin - offsetPos;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 angleVector = new Vector3(Mathf.Cos(angle * (Mathf.PI / 180f)), Mathf.Sin(angle * (Mathf.PI / 180f)));
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, angleVector, viewDistance, layerMask);
            if (raycastHit2D.collider == null)
            {
                vertex = origin - offsetPos + angleVector * viewDistance;
            }
            else
            {
                vertex = raycastHit2D.point - (Vector2)offsetPos;
                if (PlayerController.instance.playerState != PlayerController.PLAYERSTATE.HIDE && raycastHit2D.collider.tag == "Player")
                {
                    transform.parent.SendMessage("Chase", SendMessageOptions.DontRequireReceiver);
                    EnemyManager.instance.CallOtherEnemy(transform.parent.GetComponent<SodaCanEnemy>());
                }
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
            
        }


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetStartingAngle(Vector3 aimDirection)
    {
        aimDirection = aimDirection.normalized;
        float n = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        startingAngle = n+fov/2;
    }
    public void SetStartingAngle(float n)
    {
        if (n < 0) n += 360;
        startingAngle = n + fov / 2;
    }

    public void RotateStartingAngle(float n)
    {
        startingAngle += n * Time.deltaTime;
    }
}
