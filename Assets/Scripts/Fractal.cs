using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour
{

    public Mesh[] meshes;
    public Material material;

    public int maxDepth;
    private int depth;

    public float childScale;
    public float spawnProbability;
    private Material[,] materials;

    public float maxRotationSpeed;
    public float maxTwist;
    private float rotationSpeed;

    private void InitializeMaterials()
    {
        materials = new Material[maxDepth + 1, 2];
        for (int i = 0; i <= maxDepth; i++)
        {
            float t = i / (maxDepth - 1f);
            t *= t;
            materials[i, 0] = new Material(material);
            materials[i, 0].color = Color.Lerp(Color.white, Color.yellow, t);
            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
        }
        materials[maxDepth, 0].color = Color.magenta;
        materials[maxDepth, 1].color = Color.red;
    }


    void Start()
    {
        if (materials == null)
        {
            InitializeMaterials();
        }
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);
        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = materials[depth, Random.Range(0, 2)];
      


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
            if (Random.value < spawnProbability)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
                new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, childDirections[i], childOrientations[i]);
            }
        }
    }


    void Initialize(Fractal parent, Vector3 direction, Quaternion orientation)
    {
        materials = parent.materials;
        meshes = parent.meshes;
        material = parent.material;
        spawnProbability = parent.spawnProbability;
        maxRotationSpeed = parent.maxRotationSpeed;
        maxTwist = parent.maxTwist;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        transform.parent = parent.transform;
        transform.localRotation = orientation;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = direction * (0.5f + 0.5f * childScale);
    }

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
