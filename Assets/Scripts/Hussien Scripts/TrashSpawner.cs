using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Unity.VisualScripting.Member;
using UnityEngine.Events;

[System.Serializable]
public class Wave
{
    public string WaveName;
    public GameObject[] trashTypes;
    public UnityEvent waveEvent;
    public int trashCount;
}

public class TrashSpawner : MonoBehaviour
{
    [Header("Game System")]
    public static TrashSpawner instance;

    [Header("Spawning System")]
    public Transform[] spawnPoints;

    public Wave[] Waves;

    public int WaveIndex = 0;

    public int health = 10;
    public int CollectPoints = 0;

    public int CurrentTrashCount = 0;

    [Header("Polish")]
    public TextMeshPro WaitTimer_TXT;
    public TextMeshProUGUI health_TXT;
    public TextMeshProUGUI Collected_TXT;
    private AudioSource source;
    public AudioClip CorreectClip;
    public AudioClip ErrorClip;

    [Header("End game events")]
    public UnityEvent OnEndEvent;

    [Header("Leaderboard")]
    [SerializeField] TMP_InputField nameField;
    [SerializeField] LeaderboardManager leaderboardManager;
    [SerializeField] levelMover lm;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(StartSpawnTrash());
    }

    IEnumerator StartSpawnTrash()
    {
        //wait timer
        yield return new WaitForSeconds(2f);
        WaitTimer_TXT.gameObject.SetActive(true);
        WaitTimer_TXT.text = "3";
        yield return new WaitForSeconds(1f);
        WaitTimer_TXT.text = "2";
        yield return new WaitForSeconds(1f);
        WaitTimer_TXT.text = "1";
        yield return new WaitForSeconds(1f);
        WaitTimer_TXT.text = "Start Cleaning";
        yield return new WaitForSeconds(1f);
        WaitTimer_TXT.gameObject.SetActive(false);
        //end wait timer

        StartCoroutine(SpawnTrash());
    }

    IEnumerator SpawnTrash()
    {
        if(WaveIndex > Waves.Length - 1)
        {
            Debug.Log("Game Over");
            Win();
            yield break;
        }

        Waves[WaveIndex].waveEvent?.Invoke();
        CurrentTrashCount = Waves[WaveIndex].trashCount;
        for (int i = 0; i < Waves[WaveIndex].trashCount; i++)
        {
            int randomTrash = Random.Range(0, Waves[WaveIndex].trashTypes.Length);
            int randomSpawnPoint = Random.Range(0, spawnPoints.Length);

            Instantiate(Waves[WaveIndex].trashTypes[randomTrash], spawnPoints[randomSpawnPoint].position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
        
    }

    public void CleanATrash()
    {
        CollectPoints++;

        CurrentTrashCount -= 1;

        Collected_TXT.text = CollectPoints.ToString();

        source.PlayOneShot(CorreectClip);

        if (CurrentTrashCount <= 0)
        {
            WaveIndex++;
            StartCoroutine(SpawnTrash());
        }
    }

    public void LostATrash()
    {
        health--;

        CurrentTrashCount -= 1;

        health_TXT.text = health.ToString();

        source.PlayOneShot(ErrorClip);

        if (health <= 0)
        {
            Death();
            return;
        }

        if (CurrentTrashCount <= 0)
        {
            WaveIndex++;
            StartCoroutine(SpawnTrash());
        }

    }

    void Death()
    {
        OnEndEvent.Invoke();
    }

    void Win()
    {
        OnEndEvent.Invoke();
    }

    public void SaveScore()
    {
        leaderboardManager.SaveEntry(nameField.text, CollectPoints);
        lm.MoveScene(0);
    }
}
