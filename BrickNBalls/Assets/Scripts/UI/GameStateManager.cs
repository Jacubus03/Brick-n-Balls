using TMPro;
using Unity.Entities;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private TMP_Text _scoreText;

    private bool _shown;

    void Update()
    {
        if (_shown) return;

        var world = World.DefaultGameObjectInjectionWorld;
        if (world == null) return;

        var em = world.EntityManager;

        var gameStateQuery = em.CreateEntityQuery(typeof(GameStateData));
        var scoreQuery = em.CreateEntityQuery(typeof(ScoreData));

        if (gameStateQuery.IsEmpty) return;

        var gameStateEntity = gameStateQuery.GetSingletonEntity();
        var gameState = em.GetComponentData<GameStateData>(gameStateEntity);

        if (gameState.State != GameState.GameOver)
            return;

        int score = 0;
        if (!scoreQuery.IsEmpty)
        {
            var scoreEntity = scoreQuery.GetSingletonEntity();
            score = em.GetComponentData<ScoreData>(scoreEntity).Value;
        }

        _scoreText.text = $"Score: {score}";
        _gameOverUI.SetActive(true);
        _shown = true;
        Time.timeScale = 0f;
    }


    public void Reset()
    {
        _shown = false;
        Time.timeScale = 1f;
    }
}
