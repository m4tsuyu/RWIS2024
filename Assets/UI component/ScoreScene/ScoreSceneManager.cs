using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public static class GetScore
{
    public static int score = 0;
}

public class ScoreSceneManager : MonoBehaviour
{
    private VisualElement root;
    
    Button titleBtn;

    Label scoreLabel;


    // Start is called before the first frame update
    void Start()
    {
        root = this.GetComponent<UIDocument>().rootVisualElement;
        titleBtn = root.Q<Button>("titleButton");
        if (titleBtn is not null) titleBtn.clicked += () => { SceneManager.LoadScene("TitleScene"); };

        scoreLabel = root.Q<Label>("ScoreLabel");
        if (scoreLabel is not null) scoreLabel.text = GetScore.score.ToString("D6");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
