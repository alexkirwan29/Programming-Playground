using UnityEngine;

public class TerrainManager : MonoBehaviour {
    Terrain terrain;
    TerrainData terrainData;

    public void SetTexture(Texture2D tex2D) {
        TerrainLayer[] tex = new TerrainLayer[1];
        tex[0] = new TerrainLayer {
            diffuseTexture = tex2D,    //Sets the texture
            tileSize = new Vector2(terrainData.alphamapWidth, terrainData.alphamapHeight)    //Sets the size of the texture
        };

        terrainData.terrainLayers = tex;
    }

    public Texture2D GetTexture() { return terrainData.terrainLayers[0].diffuseTexture; }
}
