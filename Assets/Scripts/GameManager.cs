using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool isNext { get; set; }
    public bool isRecord { get; set; }
    public bool isMissed {get; set;}
    public int MaxSeedNo { get; private set; }
    public int MaxOjamaNo { get; private set; }

    [SerializeField] private seed[] seedPrefab;
    [SerializeField] private ojama[] ojamaPrefab;
    [SerializeField] private Transform seedPosition;
    [SerializeField] private Transform ojamaPosition;
    [SerializeField] private Text txtScore;

    private int totalscore;
    private GameObject audio;
    private AudioAnalyzer audioScript;
    private UImanager uimanager;
    private int prevSeedNo = -1;

    void Start()
    {
        // 諸々初期化
        audio = GameObject.Find("AudioAnalyzer");
        audioScript = audio.GetComponent<AudioAnalyzer>();
        uimanager = GameObject.Find("UIDocument").GetComponent<UImanager>();

        Instance = this;
        MaxSeedNo = seedPrefab.Length;
        MaxOjamaNo = ojamaPrefab.Length;
        // scoreの初期化
        totalscore = 0;
        //SetScore(totalscore);

        //ここisNext=trueにしてAudioAnalyzerに投げる
        isNext=true;
    }
    void Update()
    {
        //ここAudioAnalyzer側の方に変えてほしい→変えた
        if (isRecord)
        {
            // isRecordが立ったらseed生成
            isRecord = false;
            Invoke("CreateSeed", 1f);

            //UIの変更
            uimanager.changeRecordState();
        }
    }
     private void CreateSeed()
    {
        isMissed = false;
        // 0~n-3の中でランダム生成→音程に合わせた値に変更してね
        // int i = Random.Range(-4, 7);
        int i = audioScript.Tone;
        if(i == prevSeedNo)
        {
            i = -1;
        }
        //iがお邪魔の値ならisMissed trueに
        if (i<0)
        {
            isMissed = true;
        }
        //isMissed trueのときお邪魔生成
        if(isMissed)
        {
            ojama ojamaIns = Instantiate(ojamaPrefab[0], ojamaPosition);
            ojamaIns.seedNo = 0;
            ojamaIns.gameObject.SetActive(true);
            prevSeedNo = i;
        }
        else
        {
        seed seedIns = Instantiate(seedPrefab[i], seedPosition);
        seedIns.seedNo = i;
        seedIns.gameObject.SetActive(true);
        prevSeedNo = i;
        }

        uimanager.displayNextNode(i);
        uimanager.displayNextOjama(prevSeedNo);
    }

    public void MergeNext(Vector3 target,int seedNo)
    {
        seed seedIns = Instantiate(seedPrefab[seedNo + 1], target, Quaternion.identity, seedPosition);
        seedIns.seedNo = seedNo + 1;
        seedIns.isDrop = true;
        seedIns.GetComponent<Rigidbody2D>().simulated = true;
        seedIns.gameObject.SetActive(true);
        // スコア加算
        totalscore += (int)Mathf.Pow(3, seedNo);
        SetScore(totalscore);
    }

    public void MergeLargest(Vector3 target,int seedNo)
    {
        seed seedIns = Instantiate(seedPrefab[0], target, Quaternion.identity, seedPosition);
        seedIns.seedNo = 0;
        seedIns.isDrop = true;
        seedIns.GetComponent<Rigidbody2D>().simulated = true;
        seedIns.gameObject.SetActive(true);
        // スコア加算
        totalscore += (int)Mathf.Pow(3, seedNo);
        SetScore(totalscore);
    }

    public void MergeNextOjama(Vector3 target,int seedNo)
    {
        ojama ojamaIns = Instantiate(ojamaPrefab[seedNo + 1], target, Quaternion.identity, seedPosition);
        ojamaIns.seedNo = seedNo + 1;
        ojamaIns.isDrop = true;
        ojamaIns.GetComponent<Rigidbody2D>().simulated = true;
        ojamaIns.gameObject.SetActive(true);
    }

    private void SetScore(int score)
    {
        //txtScore.text = score.ToString();
        uimanager.displayScore(score);
    }

    public int GetScore()
    {
        return totalscore;
    }
}