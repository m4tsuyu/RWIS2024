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
        //�{�^���C�x���g�̓o�^
        var recordBtn = root.Q<Button>("RecordButton"); 
        if (recordBtn is not null) recordBtn.clicked += () => { /* �N���b�N���̏��� */ };        
        var munuBtn = root.Q<Button>("Button1");
        if (munuBtn is not null) munuBtn.clicked += () => { /* �N���b�N���̏��� */ };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
