using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour
{

    public Mesh mesh;
    public Material material;

    public int maxDepth;
    private int depth;

    public float childScale;

    private Material[] materials;

    private void InitializeMaterials()
    {
        materials = new Material[maxDepth + 1];
        for (int i = 0; i <= maxDepth; ++i)
        {
            materials[i] = new Material(material);
            materials[i].color = Color.Lerp(Color.white, Color.yellow, (float)i / maxDepth);
        }
    }


    void Start()
    {
        if (materials == null)
        {
            InitializeMaterials();
        }

        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = material;
        GetComponent<MeshRenderer>().material.color = materials[depth].color;
        if (depth < maxDepth)
        {
            StartCoroutine(CreateChild());
        }

    }

    Vector3[] childDirections =
    {
        Vector3.up,
        Vector3.left,
        Vector3.right,
         Vector3.forward,
        Vector3.back
    };

    Quaternion[] childOrientations =
    {
         Quaternion.identity,
         Quaternion.Euler(0, 0, 90),
         Quaternion.Euler(0, 0, -90),
         Quaternion.Euler(90f, 0f, 0f),
         Quaternion.Euler(-90f, 0f, 0f)
    };


    IEnumerator CreateChild()
    {
        for (int i = 0; i < childDirections.Length; ++i)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, childDirections[i], childOrientations[i]);
        }
    }


    void Initialize(Fractal parent, Vector3 direction, Quaternion orientation)
    {
        materials = parent.materials;
        mesh = parent.mesh;
        material = parent.material;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        transform.parent = parent.transform;
        transform.localRotation = orientation;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = direction * (0.5f + 0.5f * childScale);
    }


}
