using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField] private AudioAnalyzer analyzer;
    [SerializeField] private GameManager manager;
    private VisualElement root;

    //動的なUIパーツ
    Button recordBtn;
    Button munuBtn;

    Label recordLabel;
    Label scoreLabel;
    void Start()
    {
        analyzer = GameObject.Find("AudioAnalyzer").GetComponent<AudioAnalyzer>();
        manager = GameManager.Instance;

        root = this.GetComponent<UIDocument>().rootVisualElement;

        //ボタンイベントの登録
        recordBtn = root.Q<Button>("RecordButton");
        if (recordBtn is not null) recordBtn.clicked += () => { recordStart(); };
        munuBtn = root.Q<Button>("Button1");
        if (munuBtn is not null) munuBtn.clicked += () => { /* クリック時の処理 */ };

        //動的なコンポーネントの取得
        scoreLabel = root.Q<Label>("ScoreLabel");
        recordLabel = root.Q<Label>("RecordLabel");
    }

    // Update is called once per frame
    void Update()
    {

    }

    /* 
     * レコードボタンが押されたときの処理
     */
    void recordStart()
    {
        Debug.Log("レコードスタート");

        // RecordButtonとRecordLabelのUI変更
        if (recordBtn != null && recordLabel != null)
        {
            recordBtn.AddToClassList("#RecordingButton:active"); // ボタンスタイル変更
            recordLabel.AddToClassList("#RecordingLabel:active"); // ラベルスタイル変更
            recordLabel.text = "Now Recording...";
        }

        //レコードの開始処理
        if (analyzer is not null)
        {
            //analyzer.StartAnalyzer();
        }
        else
        {
            Debug.LogError("analzerが指定されてない");
        }
        //デバック
        //displayScore(1000);
    }

    void displayInput()
    {

    }

    public void displayScore(int text)
    {
        scoreLabel.text = text.ToString("D6");
    }
}