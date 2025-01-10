using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private AudioAnalyzer analyzer;
    private GameManager manager; 
    private VisualElement root;

    void Start()
    {
        analyzer = GameObject.Find("AudioAnalyzer").GetComponent<AudioAnalyzer>();
        manager = GameManager.Instance;

        root = this.GetComponent<UIDocument>().rootVisualElement;
        //ボタンイベントの登録
        var recordBtn = root.Q<Button>("RecordButton"); 
        if (recordBtn is not null) recordBtn.clicked += () => { /* クリック時の処理 */ };        
        var munuBtn = root.Q<Button>("Button1");
        if (munuBtn is not null) munuBtn.clicked += () => { /* クリック時の処理 */ };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
