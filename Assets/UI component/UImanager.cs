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

    int debug = 0;
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
        Debug.Log(debug);

        // RecordButton��RecordLabel��UI�ύX
        changeRecordState(debug % 3);

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
        debug++;
    }

    public void changeRecordState(int state)
    {
        if (recordBtn is null || recordLabel is null)
        {
            Debug.LogError("���R�[�h�̃{�^�������x����null");
            return;
        }

        switch (state)
        {
            case 0:
                //���R�[�h�{�^�����f�t�H���g�����ɂ���
                //�{�^���̓��͂��󂯕t����
                recordBtn.RemoveFromClassList("#RecordButton:active"); // �X�^�C�������ɖ߂�
                recordLabel.RemoveFromClassList("#RecordLabel:active"); // �X�^�C�������ɖ߂�
                recordLabel.text = "Tap!";
                break;
            case 1:
                //���R�[�h�{�^����active�ǉ�
                //�{�^�����͂��󂯕t���Ȃ�
                recordLabel.text = "Now Recording...";
                break;
            case 2:
                //�{�^���̓A�N�e�B�u�̂܂܁A���x���X�^�C���ƃe�L�X�g�����ύX
                //�{�^�����͂��󂯕t���Ȃ��܂�
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