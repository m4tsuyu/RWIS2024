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
    [SerializeField] private Sprite[] nodeImages; 

    private VisualElement root;

    //動的なUIパーツ
    Button recordBtn;
    Button munuBtn;

    Label recordLabel;
    Label nextLabel;
    Label scoreLabel;
    Label ojamaLabel;

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
        nextLabel = root.Q<Label>("NextNodeLabel");
        ojamaLabel = root.Q<Label>("OjamaLabel");

        //初期化
        displayNextNode(-1);
        displayNextOjama(-1);
        
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

                recordLabel.style.color = new Color(192 / 255f, 0, 192 /255f); 
                recordLabel.style.textShadow = new TextShadow
                {
                    offset = new Vector2(2, 2),
                    color = new Color(0, 235 / 255f, 219 / 255f), //水色
                };

                recordLabel.text = "Now Recording...";
                //ボタン入力を受け付けないよう変更
                recordBtn.SetEnabled(false);
                break;
            case 2:
                //ボタンはアクティブのまま、ラベルスタイルとテキストだけ変更
                //ボタン入力を受け付けないまま
                recordLabel.style.fontSize = 25f;
                recordLabel.style.color = new Color(1f, 150/ 255f,0); //オレンジ
                recordLabel.style.textShadow = new TextShadow
                {
                    offset = new Vector2(2, 2),
                    color = new Color(230 / 255f, 0, 92 / 255f), // ピンク色
                };

                recordLabel.text = "Drop!";
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
            inputTone[l].style.height = new StyleLength(r * inputDisplayHeight);
            //Debug.Log(r);
            l++;
        }
    }

    public void displayNextOjama(int next)
    {
        switch (next)
        {
            case 0: //C
                ojamaLabel.style.color = new Color(1f, 45/255f, 85 / 255f);
                ojamaLabel.text = "C";
                break;
            case 1: //D
                ojamaLabel.style.color = new Color(1f, 59 / 255f, 48 / 255f);
                ojamaLabel.text = "D";
                break;
            case 2: //E
                ojamaLabel.style.color = new Color(1f, 149 / 255f, 0);
                ojamaLabel.text = "E";
                break;
            case 3: //F
                ojamaLabel.style.color = new Color(1f, 204 / 255f, 0);
                ojamaLabel.text = "F";
                break;
            case 4: //G
                ojamaLabel.style.color = new Color(131 / 255f, 211 / 255f, 19 / 255f);
                ojamaLabel.text = "G";
                break;
            case 5: //A
                ojamaLabel.style.color = new Color(52 / 255f, 199 / 255f, 89 / 255f);
                ojamaLabel.text = "A";
                break;
            case 6: //B
                ojamaLabel.style.color = new Color(0f, 235 / 255f, 219 / 255f);
                ojamaLabel.text = "B";
                break;
            default: //ojama
                ojamaLabel.style.color = new Color(178 / 255f, 178 / 255f, 178 / 255f);
                ojamaLabel.text = "×";
                break;
        }
    }

    public void displayScore(int text)
    {
        scoreLabel.text = text.ToString("D6");
    }
    public void displayNextNode(int nextnode)
    {
        Debug.Log("next"+nextnode);
        switch (nextnode)
        {
            case 0: //C
                nextLabel.text = "C";
                break;
            case 1: //D
                nextLabel.text = "D";
                break;
            case 2: //E
                nextLabel.text = "E";
                break;
            case 3: //F
                nextLabel.text = "F";
                break;
            case 4: //G
                nextLabel.text = "G";
                break;
            case 5: //A
                nextLabel.text = "A";
                break;
            case 6: //B
                nextLabel.text = "B";
                break;
            default:
                nextLabel.text = "×";
                break;
        }
        if(nextnode >= 0)
        {
            nextLabel.style.backgroundImage = new StyleBackground(nodeImages[nextnode+1]);
        }
        else
        {
            nextLabel.style.backgroundImage = new StyleBackground(nodeImages[0]);
        }
    }
    public void gobackTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}