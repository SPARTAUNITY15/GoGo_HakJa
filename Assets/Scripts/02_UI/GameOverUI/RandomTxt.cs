using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RandomTxt : MonoBehaviour
{
    public List<string> textList = new List<string>();
    public TMP_Text outputText;
    public Image targetImage; // 퍼블릭으로 선언한 이미지

    void Start()
    {
        if (outputText == null)
        {
            Debug.LogError("Text 오브젝트가 할당안됨");
        }

        if (targetImage == null)
        {
            Debug.LogError("이미지 오브젝트가 할당안됨");
        }
        DisplayRandomText();
    }

    public string GetRandomText()
    {
        if (textList.Count == 0)
        {
            Debug.LogWarning("리스트가 비어 있음");
            return string.Empty;
        }

        int randomIndex = Random.Range(0, textList.Count);
        return textList[randomIndex];
    }

    public void DisplayRandomText()
    {
        if (outputText != null)
        {
            string randomText = GetRandomText();
            outputText.text = randomText;
            StartCoroutine(FadeTextIn(outputText, 5f)); // 5초 동안 텍스트 투명도 증가
        }
        else
        {
            Debug.LogError("Text가 할당안됨");
        }

        if (targetImage != null)
        {
            StartCoroutine(FadeImageIn(targetImage, 10f)); // 10초 동안 이미지 투명도 증가
        }
        else
        {
            Debug.LogError("Image가 할당안됨");
        }
    }

    IEnumerator FadeTextIn(TMP_Text text, float duration)
    {
        Color color = text.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            text.color = new Color(color.r, color.g, color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.color = new Color(color.r, color.g, color.b, 1f);
    }

    IEnumerator FadeImageIn(Image image, float duration)
    {
        Color color = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            image.color = new Color(color.r, color.g, color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = new Color(color.r, color.g, color.b, 1f);
    }
}
