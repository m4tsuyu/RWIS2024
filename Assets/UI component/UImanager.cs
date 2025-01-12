using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField] private AudioAnalyzer analyzer;
    [SerializeField] private GameManager manager;
    private VisualElement root;

    //���I��UI�p�[�c
    Button recordBtn;
    Button munuBtn;

    Label recordLabel;
    Label scoreLabel;
    void Start()
    {
        analyzer = GameObject.Find("AudioAnalyzer").GetComponent<AudioAnalyzer>();
        manager = GameManager.Instance;

        root = this.GetComponent<UIDocument>().rootVisualElement;

        //�{�^���C�x���g�̓o�^
        recordBtn = root.Q<Button>("RecordButton");
        if (recordBtn is not null) recordBtn.clicked += () => { recordStart(); };
        munuBtn = root.Q<Button>("Button1");
        if (munuBtn is not null) munuBtn.clicked += () => { /* �N���b�N���̏��� */ };

        //���I�ȃR���|�[�l���g�̎擾
        scoreLabel = root.Q<Label>("ScoreLabel");
        recordLabel = root.Q<Label>("RecordLabel");
    }

    // Update is called once per frame
    void Update()
    {

    }

    /* 
     * ���R�[�h�{�^���������ꂽ�Ƃ��̏���
     */
    void recordStart()
    {
        Debug.Log("���R�[�h�X�^�[�g");

        // RecordButton��RecordLabel��UI�ύX
        if (recordBtn != null && recordLabel != null)
        {
            recordBtn.AddToClassList("#RecordingButton:active"); // �{�^���X�^�C���ύX
            recordLabel.AddToClassList("#RecordingLabel:active"); // ���x���X�^�C���ύX
            recordLabel.text = "Now Recording...";
        }

        //���R�[�h�̊J�n����
        if (analyzer is not null)
        {
            //analyzer.StartAnalyzer();
        }
        else
        {
            Debug.LogError("analzer���w�肳��ĂȂ�");
        }
        //�f�o�b�N
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