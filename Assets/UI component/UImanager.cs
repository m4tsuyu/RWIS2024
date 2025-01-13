using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private int tapCount;

    void Start()
    {
        analyzer = GameObject.Find("AudioAnalyzer").GetComponent<AudioAnalyzer>();
        manager = GameManager.Instance;

        root = this.GetComponent<UIDocument>().rootVisualElement;

        //ボタンイベントの登録
        recordBtn = root.Q<Button>("RecordButton");
        if (recordBtn is not null) recordBtn.clicked += () => { recordStart(); };
        munuBtn = root.Q<Button>("MenuButton");
        if (munuBtn is not null) munuBtn.clicked += () => { gobackTitle(); };


        //動的なコンポーネントの取得
        scoreLabel = root.Q<Label>("ScoreLabel");
        recordLabel = root.Q<Label>("RecordLabel");
        tapCount = 1;
    }

    void Update()
    {
    }

    /* 
     * レコードボタンが押されたときの処理
     */
    void recordStart()
    {

        // RecordButtonとRecordLabelのUI変更
        changeRecordState();

        //レコードの開始処理
        if (analyzer is not null)
        {
            analyzer.StartAnalyzer();
        }
        else
        {
            Debug.LogError("analzerが指定されてない");
        }
        //デバック
        //displayScore(debug);
    }

    public void changeRecordState()
    {
        //デバック用
        if (recordBtn is null || recordLabel is null)
        {
            Debug.LogError("レコードのボタンかラベルがnull");
            return;
        }

        switch (tapCount%3)
        {
            case 0:
                //レコードボタンとレコードラベルのスタイルをデフォルトだけにする
                recordBtn.RemoveFromClassList("#RecordButton:active"); //スタイルを戻す
                recordLabel.style.fontSize = 25f;
                recordLabel.style.color = new Color(204/ 255f, 51/255f, 0); 
                recordLabel.style.textShadow = new TextShadow
                {
                    offset = new Vector2(2, 2),
                    color = new Color(63 / 255f, 198 / 255f, 88 / 255f),
                };

                recordLabel.text = "Tap!";

                //ボタンの入力を受け付けるよう変更
                recordBtn.SetEnabled(true);
                break;
            case 1:
                //レコードボタンにactive追加
                recordBtn.AddToClassList("#RecordButton:active"); //activeを追加
                recordLabel.style.fontSize = 10f;

                recordLabel.style.color = new Color(235 / 255f, 219 /255f, 0); 
                recordLabel.style.textShadow = new TextShadow
                {
                    offset = new Vector2(2, 2),
                    color = new Color(255 / 255f, 153 / 255f, 0), // オレンジ色
                };

                recordLabel.text = "Now Recording...";
                //ボタン入力を受け付けないよう変更
                recordBtn.SetEnabled(false);
                break;
            case 2:
                //ボタンはアクティブのまま、ラベルスタイルとテキストだけ変更
                recordLabel.style.fontSize = 25f;
                recordLabel.style.color = new Color(171 / 255f,0,  255 / 255f);
                recordLabel.style.textShadow = new TextShadow
                {
                    offset = new Vector2(2, 2),
                    color = new Color(244 / 255f, 192 / 255f, 63/255f), // オレンジ色
                };

                recordLabel.text = "Drop!";
                //ボタン入力を受け付けないまま
                break;
        }
        tapCount++;
    }

    void displayInput()
    {

    }

    public void displayScore(int text)
    {
        scoreLabel.text = text.ToString("D6");
    }

    public void gobackTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}