using UnityEngine;
using System.Collections;

public class EnemyDamaged : MonoBehaviour
{
    private Renderer[] renderers;
    private Color[] originalColors;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.2f;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }
    }

    public void FlashDamage()
    {
        StopAllCoroutines();
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = flashColor;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }
    }
}

