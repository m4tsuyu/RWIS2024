using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnalyzer : MonoBehaviour
{
    [SerializeField] private int SampleSize = 2048; // 2��n��ɂ��邱��
    //[SerializeField] private float DetectedMinSpectrum = 0.04f;
    //[SerializeField] private float DetectedMaxSpectrum = 0.3f;
    [SerializeField] private int RecordSec = 2;

    const int TONE = 12;
    const int SamplingRate = 48000; // 48000�ɌŒ肵�Ȃ��Ƃ��܂������Ȃ�

    private AudioSource audio;
    private GameObject manager;
    private GameManager managerScript;
    private UImanager uimanager;
    private bool isSoundDetected = false;
    private bool isButtonPushed = false;
    private int[] toneRecord;
    private float time;

    public int Tone { get; private set; }

    private void Start()
    {
        manager = GameObject.Find("GameManager");
        managerScript = manager.GetComponent<GameManager>();
        uimanager = GameObject.Find("UIDocument").GetComponent<UImanager>();

        Tone = -1;
        audio = GetComponent<AudioSource>();
        audio.clip = Microphone.Start(Microphone.devices[0], true, RecordSec, SamplingRate);
        audio.loop = true;

        audio.Play();
    }

    private void Update()
    {
        if (!isButtonPushed) return;
        // if (!managerScript.isNext) return;
        time += Time.deltaTime;
        if (time < RecordSec)
        {
            float[] spectrum = AudioToSpectrum(audio, SampleSize);
            int maxIndex = FindPitchIndex(spectrum);
            float pitch = SpectrumToPitch(spectrum, maxIndex, SampleSize);
            int tone = PitchToTwelveTone(pitch);
            if (0 <= tone && tone < TONE) 
            {
                toneRecord[tone]++;
                uimanager.displayInput(toneRecord);
            }
        }
        else
        {
            int tone = System.Array.IndexOf(toneRecord, Mathf.Max(toneRecord)); // ����
            Tone = MyTone(tone);
            managerScript.isRecord = true;
            Debug.Log("tone = " + Tone);

            isButtonPushed = false;
        }
    }

    /// <summary>
    /// �{�^�������ꂽ�Ƃ��̏���
    /// </summary>
    public void StartAnalyzer()
    {
        if (!managerScript.isNext) return;
        toneRecord = new int[TONE];
        time = 0f;

        isButtonPushed = true;
        managerScript.isNext=false;
    }

    /// <summary>
    /// RecordSec �b�Ԃ̉����f�[�^���}�C�N����擾
    /// </summary>
    public void StartRecordind()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("Microphone device missing");
            return;
        }

        audio = GetComponent<AudioSource>();
        audio.clip = Microphone.Start(Microphone.devices[0], false, RecordSec, SamplingRate);

        audio.Play();
    }

    /// <summary>
    /// FFT�ŉ����f�[�^������g���ɕϊ�
    /// </summary>
    float[] AudioToSpectrum(AudioSource audio, int sampleSize)
    {
        float[] spectrum = new float[sampleSize];
        audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        return spectrum;
    }

    /// <summary>
    /// ���g���f�[�^���特���Ɏg����C���f�b�N�X���擾
    /// </summary>
    int FindPitchIndex(float[] spectrum)
    {
        int maxIndex = -1;
        float maxVolume = -1f;

        for (int i = 0; i < spectrum.Length; i++)
        {
            /*
            if (spectrum[i] > DetectedMinSpectrum && maxVolume < spectrum[i])
            {
                maxVolume = spectrum[i];
                maxIndex = i;
            }
            */
            if (spectrum[i] > maxVolume)
            {
                maxVolume = spectrum[i];
                maxIndex = i;
            }
        }

        return maxIndex;
    }

    /// <summary>
    ///  ���g���f�[�^���特�����擾�iA0 = 0�j
    /// </summary>
    float SpectrumToPitch(float[] spectrum, int maxIndex, int sampleSize)
    {
        if (maxIndex < 0) return -1;

        // ����[Hz]�擾
        float freq = (float)maxIndex;

        // �X�y�N�g�����[�N�̕␳
        if (maxIndex > 0 && maxIndex < sampleSize - 1)
        {
            float dL = spectrum[maxIndex - 1] / spectrum[maxIndex];
            float dR = spectrum[maxIndex + 1] / spectrum[maxIndex];
            freq += 0.5f * (dR * dR - dL * dL);
        }

        float pitchHz = 0.5f * freq * SamplingRate / sampleSize;

        // ���K�ւ̕ϊ��iA0��j
        float A0 = 55f;
        if (pitchHz == 0f) return -1;

        float pitchScale = (float)TONE * (Mathf.Log(pitchHz) - Mathf.Log(A0)) / Mathf.Log(2f);
        return pitchScale;
    }

    /// <summary>
    /// ���l�̉�����12���K�ɕύX�i�h = 0�j
    /// </summary>
    int PitchToTwelveTone(float pitch)
    {
        // --------------
        // 0 : C  (�h)
        // 1 : C# (�h��)
        // 2 : D  (��)
        // 3 : D# (����)
        // 4 : E  (�~)
        // 5 : F  (�t�@)
        // 6 : F# (�t�@#)
        // 7 : G  (�\)
        // 8 : G# (�\��)
        // 9 : A  (��)
        // 10: A# (����)
        // 11: B  (�V)
        // --------------
        int pitchInt = Mathf.RoundToInt(pitch);
        if (pitchInt < 0)
        {
            return -1;
        }
        const int C_STANDARD = 3; // ���A����C�ɕύX����
        return (pitchInt + TONE - C_STANDARD) % TONE;
    }

    int MyTone(int tone)
    {
        // --------------
        // C#, D#, F#, G#, A# -> -1
        // C -> 0 (�h)
        // D -> 1 (��)
        // E -> 2 (�~)
        // F -> 3 (�t�@)
        // G -> 4 (�\)
        // A -> 5 (��)
        // B -> 6 (�V)
        switch (tone)
        {
            case 0:  return 0;  // C
            case 2:  return 1;  // D
            case 4:  return 2;  // E
            case 5:  return 3;  // F
            case 7:  return 4;  // G
            case 9:  return 5;  // A
            case 11: return 6;  // B
            default: return -1; // others
        }
    }

    /// <summary>
    /// ���o�������ʂ� min �` max �܂łŐ��K��
    /// </summary>
    float NormalizeVolume(float vol, float min, float max)
    {
        return (vol - min) / (max - min);
    }
}
