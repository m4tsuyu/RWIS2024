using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    VisualElement inputDisplay;
    Label[] inputTone = new Label[7];

    //UI用遷移
    private int tapCount;
    private int[] _TONEINDEX = new int[7]{0, 2, 4, 5,7,9,11};

    //[SerializeField, Range(0f, 1f)] 
    float time = 0f;

    void Start()
    {
        analyzer = GameObject.Find("AudioAnalyzer").GetComponent<AudioAnalyzer>();
        manager = GameManager.Instance;

        tapCount = 1;
        root = this.GetComponent<UIDocument>().rootVisualElement;

        //ボタンイベントの登録
        recordBtn = root.Q<Button>("RecordButton");
        if (recordBtn is not null) recordBtn.clicked += () => { recordStart(); };
        munuBtn = root.Q<Button>("MenuButton");
        if (munuBtn is not null) munuBtn.clicked += () => { gobackTitle(); };

        //動的なコンポーネントの取得
        scoreLabel = root.Q<Label>("ScoreLabel");
        recordLabel = root.Q<Label>("RecordLabel");

        //音階用ラベル
        
        inputTone[0] = root.Q<Label>("C");
        inputTone[1] = root.Q<Label>("D");
        inputTone[2] = root.Q<Label>("E");
        inputTone[3] = root.Q<Label>("F");
        inputTone[4] = root.Q<Label>("G");
        inputTone[5] = root.Q<Label>("A");
        inputTone[6] = root.Q<Label>("B");
    }

    void Update()
    {
        //float inputDisplayHeight = root.Q<VisualElement>("InputDisplay").resolvedStyle.height;
        //// ラベルの高さを設定
        //float offset = 0f;
        //foreach (Label l in inputTone)
        //{
        //    float rate = Mathf.Clamp01(Mathf.Sin(time + offset));
            
        //    offset += 0.1f;
        //}
        //time += 0.01f;
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

    public void displayInput(int[] toneRecord)
    {
        float inputDisplayHeight = root.Q<VisualElement>("InputDisplay").resolvedStyle.height;

        int sum = toneRecord.Sum();
        int l = 0;
        foreach(int index in _TONEINDEX)
        {
            int tone = toneRecord[index];
            float r = (float)tone / sum;
            // ラベルの高さを設定
            //inputTone[l].style.height = new StyleLength(r * inputDisplayHeight);
            Debug.Log(r);
            l++;
        }
    }

    public void displayScore(int text)
    {
        scoreLabel.text = text.ToString("D6");
    }
    public void displayNextNode(int text)
    {
        
    }
    public void gobackTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}