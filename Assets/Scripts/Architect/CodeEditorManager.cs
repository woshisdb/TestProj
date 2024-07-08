//using System.Collections;
//using System.Collections.Generic;
//using Sirenix.OdinInspector;
//using UnityEngine;
//using ZenFulcrum.EmbeddedBrowser;
//[RequireComponent(typeof(Browser))]
//public class CodeEditorManager : MonoBehaviour
//{
//    private string editorUrl;
//    private Browser browser;
//    void Start()
//    {
//        // Instantiate the browser prefab
//        var browserPrefab = Resources.Load<Browser>("Browser (GUI)");
//        browser = Instantiate(browserPrefab, transform);

//        // Set the browser URL to the HTML file in StreamingAssets
//        editorUrl = Application.streamingAssetsPath + "/monaco-editor.html";
//        browser.Url = "file:///" + editorUrl;

//        browser.onConsoleMessage += (string s1, string s2) =>
//        {
//            Debug.Log("js console info:" + s1 + s2);
//        };
//    }
//    [Button]
//    // Example methods to interact with the editor
//    public void GetEditorContent()
//    {
//        browser.CallFunction("setval").Done();
//    }
//    //[Button]
//    //public void SetEditorContent(string content)
//    //{
//    //    browser.CallFunction("setEditorContent", content).Done();
//    //}
//}