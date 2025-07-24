using UnityEngine;

public class Parallex : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    public float animationSpeed = 0.5f; // Speed of the parallax effect

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer component not found on the GameObject. Please add a MeshRenderer.");
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer.material.mainTextureOffset += new Vector2(animationSpeed * Time.deltaTime, 0f);
    }
}
