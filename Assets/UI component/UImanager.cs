using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UImanager : MonoBehaviour
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
            recordBtn.AddToClassList("#RecordButton:active"); // �{�^���X�^�C���ύX
            recordLabel.AddToClassList("#RecordLabel:active"); // ���x���X�^�C���ύX
            displayLabel("Now Recording...");
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

    public void displayLabel(string text)
    {
        switch (text)
        {
            case "Tap!":
                //���R�[�h�{�^�����f�t�H���g�����ɂ���
                //�{�^���̓��͂��󂯕t����
                recordLabel.text = text;
                break;
            case "Now Recording...":
                //���R�[�h�{�^����active�ǉ�
                //�{�^�����͂��󂯕t���Ȃ�
                recordLabel.text = text;
                break;
            case "Drop!":
                //�{�^���̓A�N�e�B�u�̂܂܁A���x���X�^�C���ƃe�L�X�g�����ύX
                //�{�^�����͂��󂯕t���Ȃ��܂�
                recordLabel.text = text;
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