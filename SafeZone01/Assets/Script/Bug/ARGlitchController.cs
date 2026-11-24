using System.Collections;
using UnityEngine;

public class ARGlitchController : MonoBehaviour
{
    [SerializeField] private Material glitchMaterial;
    [SerializeField] private float glitchDuration = 0.3f;
    [SerializeField, Range(0f, 1f)]
    private float maxIntensity = 1f;

    private static readonly int IntensityID =
        Shader.PropertyToID("_GlitchIntensity");
    private static readonly int TimeID =
        Shader.PropertyToID("_GlitchTime");

    private Coroutine glitchRoutine;

    private void OnEnable()
    {
        if (glitchMaterial == null)
        {
            Debug.LogError("Glitch Material이 연결되지 않았습니다.");
            return;
        }

        glitchMaterial.SetFloat(IntensityID, 0f);
    }

    private void OnDisable()
    {
        if (glitchRoutine != null)
        {
            StopCoroutine(glitchRoutine);
            glitchRoutine = null;
        }

        if (glitchMaterial != null)
        {
            glitchMaterial.SetFloat(IntensityID, 0f);
        }
    }

    // Glitch Bug가 나타났을 때 호출
    public void TriggerGlitch()
    {
        if (glitchMaterial == null)
            return;

        if (glitchRoutine != null)
        {
            StopCoroutine(glitchRoutine);
        }

        glitchRoutine = StartCoroutine(PlayGlitch());
    }

    private IEnumerator PlayGlitch()
    {
        float elapsed = 0f;

        while (elapsed < glitchDuration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsed / glitchDuration
            );

            float intensity = Mathf.Sin(progress * Mathf.PI);

            glitchMaterial.SetFloat(
                IntensityID,
                intensity * maxIntensity
            );

            glitchMaterial.SetFloat(
                TimeID,
                Time.time
            );

            yield return null;
        }

        glitchMaterial.SetFloat(IntensityID, 0f);
        glitchRoutine = null;
    }
}