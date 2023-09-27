using UnityEngine;

public class DecalSystem : MonoBehaviour
{
    public Texture2D texture;        // Starting image.
    public Texture2D stampTexture;   // Texture to Graphics.DrawTexture on my RenderTexture.
    private RenderTexture rt;        // RenderTexture to use as a buffer.

    private void Start()
    {
        InitializeRenderTexture();
    }

    private void InitializeRenderTexture()
    {
        // Create RenderTexture 512x512 pixels in size.
        rt = new RenderTexture(512, 512, 32);

        // Assign my RenderTexture to be the main texture of my object.
        GetComponent<Renderer>().material.SetTexture("_MainTex", rt);

        // Blit my starting texture to my RenderTexture.
        Graphics.Blit(texture, rt);
    }

    public void DrawSplat(float posX, float posY)
    {
        Vector2 originalCoords = new Vector2(posX, posY);
        Vector2 convertedCoords = ConvertCoordinates(originalCoords);

        RenderTexture.active = rt;

        // Setup a matrix for pixel-correct rendering.
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, 512, 512, 0);

        // Calculate the scaled width and height
        float scaledWidth = stampTexture.width * 0.1f;  // 10% of the original width
        float scaledHeight = stampTexture.height * 0.1f * 2.38f; // 10% of the original height

        // Draw my stampTexture on my RenderTexture positioned by posX and posY with scaling
        Graphics.DrawTexture(new Rect(convertedCoords.x - scaledWidth / 2, (rt.height - convertedCoords.y) - scaledHeight / 2, scaledWidth, scaledHeight), stampTexture);

        // Restore both projection and modelview matrices off the top of the matrix stack.
        GL.PopMatrix();

        RenderTexture.active = null;
    }

    private Vector2 ConvertCoordinates(Vector2 originalCoords)
    {
        originalCoords /= 5f;
        originalCoords *= -1f;
        originalCoords += new Vector2(1f, 1f);
        originalCoords /= 2f;
        originalCoords *= 512f;
        return originalCoords;
    }
}
