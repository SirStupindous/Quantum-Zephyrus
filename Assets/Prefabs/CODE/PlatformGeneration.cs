using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGeneration : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] int minHeight, maxHeight;
    [SerializeField] int repeatNum;
    [SerializeField] GameObject dirt, grass;

    // Start is called before the first frame update
    void Start()
    {
        Generation();
    }

    // Update is called once per frame
    void Generation()
    {
        int reapeatValue = 0;
        for (int x = 0; x < width; x++)     //helps spawn tiles on the x axis
        {
            if (reapeatValue == 0)
            {
                height = Random.Range(minHeight, maxHeight);    //randomizes the height of the grass
                GenerateFlatPlatform(x);
                reapeatValue = repeatNum;
            }
            else
            {
                GenerateFlatPlatform(x);
                reapeatValue--;
            }
        }
    }

    void GenerateFlatPlatform(int x) 
    {
        for (int y = 0; y < height; y++)    //helps spawn tiles on the y axis
        {
            spawnObject(dirt, x, y);   //if the y value is less than the total stone spawn distance, spawn grass
        }
        spawnObject(grass, x, height);  //if the height is greater than the total stone spawn distance, spawn grass
    }

    // Set the spawned items as children of the proc gen object
    void spawnObject(GameObject obj, int width, int height)
    {
        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);
        // obj.transform.SetParent(transform);
        obj.transform.parent = this.transform;
    }
}
