using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnalyzer : MonoBehaviour
{
    [SerializeField] private int SamplingRate = 12000;
    [SerializeField] private int SampleSize = 2048; // 2��n��ɂ��邱��
    [SerializeField] private float DetectedMinSpectrum = 0.04f;
    [SerializeField] private float DetectedMaxSpectrum = 0.3f;
    [SerializeField] private int RecordSec = 3;

    const int TONE = 12;

    private AudioSource audio;
    private bool isSoundDetected = false;
    private bool isButtonPushed = false;
    private int[] toneRecord;
    private float time;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = Microphone.Start(Microphone.devices[0], true, RecordSec, SamplingRate);
        audio.loop = true;

        audio.Play();
    }

    private void Update()
    {
        if (!isButtonPushed) return;

        time += Time.deltaTime;
        if (time < RecordSec)
        {
            float[] spectrum = AudioToSpectrum(audio, SampleSize);
            int maxIndex = FindPitchIndex(spectrum);
            float pitch = SpectrumToPitch(spectrum, maxIndex, SampleSize);
            int tone = PitchToTwelveTone(pitch);
            if (0 <= tone && tone < TONE) toneRecord[tone]++;
        }
        else
        {
            int tone = System.Array.IndexOf(toneRecord, Mathf.Max(toneRecord)); // ����
            Debug.Log("tone = " + tone);

            isButtonPushed = false;
        }
    }

    /// <summary>
    /// �{�^�������ꂽ�Ƃ��̏���
    /// </summary>
    public void StartAnalyzer()
    {
        toneRecord = new int[TONE];
        time = 0f;

        isButtonPushed = true;
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
            if (spectrum[i] > DetectedMinSpectrum && maxVolume < spectrum[i])
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
        // 5 : F  (�~��)
        // 6 : F# (�t�@)
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

    /// <summary>
    /// ���o�������ʂ� min �` max �܂łŐ��K��
    /// </summary>
    float NormalizeVolume(float vol, float min, float max)
    {
        return (vol - min) / (max - min);
    }
}
