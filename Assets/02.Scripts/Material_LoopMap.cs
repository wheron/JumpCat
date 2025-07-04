using UnityEngine;

public class Material_LoopMap : MonoBehaviour
{
    private new Renderer renderer;

    public float offsetSpeed = 0.1f;

    private void Start()
    {
        renderer = GetComponent<Renderer>();    
    }

    private void Update()
    {
        Vector2 offset = Vector2.right * offsetSpeed * Time.deltaTime;

        renderer.material.SetTextureOffset("_MainTex", renderer.material.mainTextureOffset + offset);
    }
}
