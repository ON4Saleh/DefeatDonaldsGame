using NUnit.Framework;
using UnityEngine;

public class JsonRead : MonoBehaviour
{
    public TextAsset myJsonFile;
    public BanditsData banditlist;
    void Start()
    {
        banditlist = JsonUtility.FromJson<BanditsData>(myJsonFile.text);
    }

}
