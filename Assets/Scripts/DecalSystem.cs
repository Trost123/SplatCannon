using UnityEngine;

public class DecalSystem : MonoBehaviour
{
    public Texture2D texture;        // Starting image.
    public Texture2D stampTexture;   // Texture to Graphics.DrawTexture on my RenderTexture.
    RenderTexture rt;                // RenderTexture to use as buffer.

    void Start()
    {
        rt = new RenderTexture(512, 512, 32);           // Create RenderTexture 512x512 pixels in size.
        GetComponent<Renderer>().material.SetTexture("_MainTex", rt);   // Assign my RenderTexture to be the main texture of my object.
        Graphics.Blit(texture, rt);                     // Blit my starting texture to my RenderTexture.
    }

    public void DrawSplat(float posX, float posY)
    {
        Vector2 originalCoords = new Vector2(posX, posY); // Example original coordinates
        Vector2 convertedCoords = ConvertCoordinates(originalCoords);

        
        RenderTexture.active = rt;                      // Set my RenderTexture active so DrawTexture will draw to it.
        GL.PushMatrix();								// Saves both projection and modelview matrices to the matrix stack.
        GL.LoadPixelMatrix(0, 512, 512, 0);			// Setup a matrix for pixel-correct rendering.
        
        // Calculate the scaled width and height
        float scaledWidth = stampTexture.width * 0.1f;  // 10% of the original width
        float scaledHeight = stampTexture.height * 0.1f * 2.38f; // 10% of the original height

        // Draw my stampTexture on my RenderTexture positioned by posX and posY with scaling
        Graphics.DrawTexture(new Rect(convertedCoords.x - scaledWidth / 2, (rt.height - convertedCoords.y) - scaledHeight / 2, scaledWidth, scaledHeight), stampTexture);
        
        GL.PopMatrix();								// Restores both projection and modelview matrices off the top of the matrix stack.
        RenderTexture.active = null;
    }
    
    Vector2 ConvertCoordinates(Vector2 originalCoords)
    {
        originalCoords /= 5f;
        originalCoords *= -1f;
        originalCoords += new Vector2(1f, 1f);
        originalCoords /= 2f;
        originalCoords *= 512f;
        return originalCoords;
    }
}