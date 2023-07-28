using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;

public class ButtonTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //一个方法  log“jijijij”
    [ProPlayButton]
    private void ButtonTest1()
    {
        Debug.Log("jijijij");
    }

    [ProButton]
    private void ButtonTest2()
    {
        Debug.Log("jijijij");
    }

    //一个方法  遍历自己的子物体，把子物体的名字是eee的物体删除
    [ProButton]
    private void ButtonTest3()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "eee")
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }
}
