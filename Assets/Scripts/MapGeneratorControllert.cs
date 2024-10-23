using System.Collections;
using UnityEngine;

public class MapGeneratorControllert : MonoBehaviour
{
    public TextAsset txtMap;
    public string[] arrayRowsInformation;
    public string[] arrayColumsInformation;
    public Sprite[] arrayMapSprite;
    public GameObject objectMapPrefab;
    public Vector2 positionInitialMapParts;
    public Vector2 separationFromMapParts;
    private void Start()
    {
        DrawMap();
    }
    private void DrawMap()
    {
        int index = 0;
        GameObject currentMapPart;
        Vector2 positionToCreatMapPart;
        arrayRowsInformation = txtMap.text.Split('\n');
        for (int i = 0; i < arrayRowsInformation.Length; ++i)
        {
            arrayColumsInformation = arrayRowsInformation[i].Split(",");
            for (int j = 0; j < arrayColumsInformation.Length; ++j)
            {
                index = (int.Parse(arrayColumsInformation[j]));

                positionToCreatMapPart = new Vector2(positionInitialMapParts.x + separationFromMapParts.x * j, 
                positionInitialMapParts.y - separationFromMapParts.y * i);

                currentMapPart = Instantiate(objectMapPrefab, positionToCreatMapPart, transform.rotation);
                currentMapPart.GetComponent<MapPartControl>().SetSprite(arrayMapSprite[index]);

                currentMapPart.transform.SetParent(this.transform);
            }
        }
    }
}
