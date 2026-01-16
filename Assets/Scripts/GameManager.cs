using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject winPanel;

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyTag;
        public int count;
        public float spawnRate;
    }

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public float waveDelay;
    }

    public List<Wave> waves;
    public float minSpawnRadius = 8f;
    public float maxSpawnRadius = 15f;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemyText;

    private Transform playerTransform;
    private int currentWaveIndex = 0;
    private int remainingWaveEnemies = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    private void Start()
    {
        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        while (currentWaveIndex < waves.Count)
        {
            Wave currentWave = waves[currentWaveIndex];

            CalculateTotalEnemiesInWave(currentWave);
            UpdateWaveUI();
            UpdateEnemyUI();

            foreach (var group in currentWave.enemyGroups)
            {
                for (int i = 0; i < group.count; i++)
                {
                    Vector2 spawnPos = CalculateSpawnPosition();
                    ObjectPoolManager.Instance.GetPooledObject(group.enemyTag, spawnPos, Quaternion.identity);
                    yield return new WaitForSeconds(group.spawnRate);
                }
            }

            while (remainingWaveEnemies > 0)
            {
                yield return null;
            }

            yield return new WaitForSeconds(currentWave.waveDelay);
            currentWaveIndex++;
        }

        if (winPanel != null) winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void CalculateTotalEnemiesInWave(Wave wave)
    {
        remainingWaveEnemies = 0;
        foreach (var group in wave.enemyGroups)
        {
            remainingWaveEnemies += group.count;
        }
    }

    private Vector2 CalculateSpawnPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minSpawnRadius, maxSpawnRadius);
        return (Vector2)playerTransform.position + (randomDirection * randomDistance);
    }

    public void EnemyDied()
    {
        remainingWaveEnemies--;
        UpdateEnemyUI();
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
            waveText.text = "Wave: " + (currentWaveIndex + 1) + " / " + waves.Count;
    }

    private void UpdateEnemyUI()
    {
        if (enemyText != null)
            enemyText.text = "Enemy: " + remainingWaveEnemies;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}