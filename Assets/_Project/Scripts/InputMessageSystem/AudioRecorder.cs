using Cysharp.Threading.Tasks;  
using System; 
using UnityEngine; 
using UnityEngine.UI;

[Serializable]
public class AudioRecorder 
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Button startButton;
    public event Action <byte[]> onStopRecord;
    public event Action  onStartRecord;
    public bool IsRecProcess { get; private set; }
    public void Init()
    {
        startButton.onClick.AddListener(StartRecord); 
    }
    public async void StartRecord()
    {
        if (IsRecProcess) { return; }
        IsRecProcess = true;
        onStartRecord?.Invoke();
        string device = Microphone.devices[0];
        audioClip = Microphone.Start(device, true, 999, 44100);
        startButton.image.color = Color.red;
        audioSource.clip = audioClip;
        audioSource.enabled = false;
        await CorStopAudioRecord();
    }


    private async UniTask CorStopAudioRecord()
    {
        await UniTask.Delay(4500);
        Microphone.End(null);
        startButton.image.color = Color.white;
        StopRecord();
    }

    public void StopRecord()
    {
        float[] data = new float[audioClip.samples * audioClip.channels];
        byte[] bytes = OggVorbis.VorbisPlugin.GetOggVorbis(audioClip, 1f);
        onStopRecord?.Invoke(bytes);
        IsRecProcess = false;
    }
}
