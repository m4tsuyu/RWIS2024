using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UImanager : MonoBehaviour
{
    [SerializeField] private AudioAnalyzer analyzer;
    [SerializeField] private GameManager manager;
    private VisualElement root;

    //動的なUIパーツ
    Button recordBtn;
    Button munuBtn;

    Label recordLabel;
    Label scoreLabel;

    int debug = 0;
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

    void Update()
    {
    }

    /* 
     * レコードボタンが押されたときの処理
     */
    void recordStart()
    {
        Debug.Log(debug);

        // RecordButtonとRecordLabelのUI変更
        changeRecordState(debug % 3);

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
        debug++;
    }

    public void changeRecordState(int state)
    {
        if (recordBtn is null || recordLabel is null)
        {
            Debug.LogError("レコードのボタンかラベルがnull");
            return;
        }

        switch (state)
        {
            case 0:
                //レコードボタンをデフォルトだけにする
                //ボタンの入力を受け付ける
                recordBtn.RemoveFromClassList("#RecordButton:active"); // スタイルを元に戻す
                recordLabel.RemoveFromClassList("#RecordLabel:active"); // スタイルを元に戻す
                recordLabel.text = "Tap!";
                break;
            case 1:
                //レコードボタンにactive追加
                //ボタン入力を受け付けない
                recordLabel.text = "Now Recording...";
                break;
            case 2:
                //ボタンはアクティブのまま、ラベルスタイルとテキストだけ変更
                //ボタン入力を受け付けないまま
                recordLabel.text = "Drop!";
                break;
        }
    }

    void displayInput()
    {

    }

    public void displayScore(int text)
    {
        scoreLabel.text = text.ToString("D6");
    }
}