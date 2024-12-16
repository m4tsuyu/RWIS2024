using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnalyzer : MonoBehaviour
{
    [SerializeField] private int SamplingRate = 12000;
    [SerializeField] private int SampleSize = 2048; // 2のn乗にすること
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
            int tone = System.Array.IndexOf(toneRecord, Mathf.Max(toneRecord)); // 結果
            Debug.Log("tone = " + tone);

            isButtonPushed = false;
        }
    }

    /// <summary>
    /// ボタン押されたときの処理
    /// </summary>
    public void StartAnalyzer()
    {
        toneRecord = new int[TONE];
        time = 0f;

        isButtonPushed = true;
    }

    /// <summary>
    /// RecordSec 秒間の音声データをマイクから取得
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
    /// FFTで音声データから周波数に変換
    /// </summary>
    float[] AudioToSpectrum(AudioSource audio, int sampleSize)
    {
        float[] spectrum = new float[sampleSize];
        audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        return spectrum;
    }

    /// <summary>
    /// 周波数データから音程に使われるインデックスを取得
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
    ///  周波数データから音程を取得（A0 = 0）
    /// </summary>
    float SpectrumToPitch(float[] spectrum, int maxIndex, int sampleSize)
    {
        if (maxIndex < 0) return -1;

        // 音程[Hz]取得
        float freq = (float)maxIndex;

        // スペクトルリークの補正
        if (maxIndex > 0 && maxIndex < sampleSize - 1)
        {
            float dL = spectrum[maxIndex - 1] / spectrum[maxIndex];
            float dR = spectrum[maxIndex + 1] / spectrum[maxIndex];
            freq += 0.5f * (dR * dR - dL * dL);
        }

        float pitchHz = 0.5f * freq * SamplingRate / sampleSize;

        // 音階への変換（A0基準）
        float A0 = 55f;
        if (pitchHz == 0f) return -1;

        float pitchScale = (float)TONE * (Mathf.Log(pitchHz) - Mathf.Log(A0)) / Mathf.Log(2f);
        return pitchScale;
    }

    /// <summary>
    /// 数値の音程を12音階に変更（ド = 0）
    /// </summary>
    int PitchToTwelveTone(float pitch)
    {
        // --------------
        // 0 : C  (ド)
        // 1 : C# (ド＃)
        // 2 : D  (レ)
        // 3 : D# (レ＃)
        // 4 : E  (ミ)
        // 5 : F  (ミ＃)
        // 6 : F# (ファ)
        // 7 : G  (ソ)
        // 8 : G# (ソ＃)
        // 9 : A  (ラ)
        // 10: A# (ラ＃)
        // 11: B  (シ)
        // --------------
        int pitchInt = Mathf.RoundToInt(pitch);
        if (pitchInt < 0)
        {
            return -1;
        }
        const int C_STANDARD = 3; // 基準をAからCに変更する
        return (pitchInt + TONE - C_STANDARD) % TONE;
    }

    /// <summary>
    /// 検出した音量を min ～ max までで正規化
    /// </summary>
    float NormalizeVolume(float vol, float min, float max)
    {
        return (vol - min) / (max - min);
    }
}
