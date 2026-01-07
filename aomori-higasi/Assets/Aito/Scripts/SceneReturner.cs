using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReturner : MonoBehaviour
{
    public static void StartFlow(string sceneToShow, float seconds, string returnScene)
    {
        var go = new GameObject("SceneReturner");
        var comp = go.AddComponent<SceneReturner>();
        DontDestroyOnLoad(go);
        comp.StartCoroutine(comp.Run(sceneToShow, seconds, returnScene));
    }

    private IEnumerator Run(string sceneToShow, float seconds, string returnScene)
    {
        // 目的のシーンへ
        SceneManager.LoadScene(sceneToShow, LoadSceneMode.Single);

        // 必要ならロード完了待ち（次フレーム）
        yield return null;

        // 規定時間表示
        yield return new WaitForSeconds(seconds);

        // 元のシーンに戻る
        SceneManager.LoadScene(returnScene, LoadSceneMode.Single);

        // 片付け（戻った後に破棄）
        yield return null;
        Destroy(gameObject);
    }
}
