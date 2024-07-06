using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveControlUI : MonoBehaviour
{
    public void Save()
    {
        SaveSystem.Instance.Save();
    }
    public void Load()
    {
        SaveSystem.Instance.Load();
    }
}
