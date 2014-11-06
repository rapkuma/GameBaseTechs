using UnityEngine;
using System.Collections;

public class LightMapLoad : MonoBehaviour
{    
	void Start () {
        // 1. Set Lightmap Textures
        int lightmapDataCount = 2;

        LightmapData[] lightmapData = new LightmapData[2];
        for (int i = 0; i < lightmapDataCount; i++)
        lightmapData[i] = new LightmapData();

        for (int i = 0; i < lightmapDataCount; i++)
        {
            Debug.Log("Lightmap and path : Yourlightmapfolder/LightmapFar-" + i.ToString() );
            lightmapData[i].lightmapFar = Resources.Load("Level_Backstreet/LightmapFar-" + i.ToString(), typeof(Texture2D)) as Texture2D;
        }
        LightmapSettings.lightmaps = lightmapData;

        // 2. Set LightProbes
        LightmapSettings.lightProbes = Resources.Load("Level_Backstreet/LightProbes", typeof(LightProbes)) as LightProbes;

        // 3. Set Skybox       
        Material MatTest = Resources.Load("Level_Backstreet/BD_Easy_Depot_Sky", typeof(Material)) as Material;
        RenderSettings.skybox = MatTest;
	}

	void Update () {
	
	}
}
