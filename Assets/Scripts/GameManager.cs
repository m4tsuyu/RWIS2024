using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool isNext { get; set; }
    public bool isRecord { get; set; }
    public bool isMissed {get; set;}
    public int MaxSeedNo { get; private set; }

    [SerializeField] private seed[] seedPrefab;
    [SerializeField] private GameObject ojamaPrefab;
    [SerializeField] private Transform seedPosition;
    [SerializeField] private Transform ojamaPosition;
    [SerializeField] private Text txtScore;

    private int totalscore;
    
    void Start()
    {
        // 諸々初期化
        Instance = this;
        MaxSeedNo = seedPrefab.Length;
        // scoreの初期化
        totalscore = 0;
        SetScore(totalscore);
        //ここisNext=trueにしてAudioAnalyzerに投げる
        isNext=true;
    }
    void Update()
    {
        if (isNext)
        {
            // isnextが立ったらseed生成
            isNext = false;
            Invoke("CreateSeed", 2f);
        }
    }
     private void CreateSeed()
    {
        isMissed = false;
        // 0~n-3の中でランダム生成→音程に合わせた値に変更してね
        int i = Random.Range(-1, 0);
        //iがお邪魔の値ならisMissed trueに
        if (i<0)
        {
            isMissed = true;
        }
        //isMissed trueのときお邪魔生成
        if(isMissed)
        {
            Instantiate(ojamaPrefab, ojamaPosition);
        }
        else
        {
        seed seedIns = Instantiate(seedPrefab[i], seedPosition);
        seedIns.seedNo = i;
        seedIns.gameObject.SetActive(true);
        }
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

    private void SetScore(int score)
    {
        txtScore.text = score.ToString();
    }
}