using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SaveControlUI : MonoBehaviour
{
    [Button]
    public void Save()
    {
        SaveSystem.Instance.Save();
    }
    [Button]
    public void Load()
    {
        SaveSystem.Instance.Load();
    }
}
