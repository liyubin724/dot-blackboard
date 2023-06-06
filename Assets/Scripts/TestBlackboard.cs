using DotEngine.BB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    string name { get; set; }
}

public class BaseItem : IItem
{
    public string name { get; set; }
    public string description { get; set; }
    public int count { get; set; }
}

public class BagItem : BaseItem
{
    public int level { get; set; } = 0;
}

public class TestBlackboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TypeBlackboard bb = new TypeBlackboard();
        bb.AddValue(typeof(IItem), new BagItem()
        {
            name = "TTF001"
        });

        var data = bb.GetValue<BagItem>(typeof(IItem));
        Debug.Log(data?.name);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
