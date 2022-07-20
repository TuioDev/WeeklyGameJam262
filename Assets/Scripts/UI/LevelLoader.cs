using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName;
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;

    public void LoadGameplayScene()
    {
        StartCoroutine(LoadingScene(gameplaySceneName));
    }

    IEnumerator LoadingScene(string sceneName)
    {
        transition.SetTrigger("Start");
        
        yield return new WaitForSeconds(transitionTime);    

        SceneManager.LoadScene(sceneName);
    }
}
