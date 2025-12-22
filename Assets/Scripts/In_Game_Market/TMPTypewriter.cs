using System.Collections;
using TMPro;
using UnityEngine;

public class TMPTypewriter : MonoBehaviour
{
    [SerializeField] float charDelay = 0.03f;
    [SerializeField] bool autoPlayOnEnable = true;

    TextMeshProUGUI tmp;
    Coroutine routine;
    string lastText;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        if (!autoPlayOnEnable) return;

        Play(tmp.text);
    }

    public void Play(string text)
    {
        if (tmp == null) tmp = GetComponent<TextMeshProUGUI>();

        lastText = text ?? "";

        if (routine != null)
            StopCoroutine(routine);

        tmp.text = lastText;

        tmp.maxVisibleCharacters = 0;
        tmp.ForceMeshUpdate(true, true);

        routine = StartCoroutine(TypeRoutine());
    }

    IEnumerator TypeRoutine()
    {
        int total = tmp.textInfo.characterCount;

        if (total == 0 && !string.IsNullOrEmpty(tmp.text))
        {
            yield return null;
            tmp.ForceMeshUpdate(true, true);
            total = tmp.textInfo.characterCount;
        }

        for (int i = 0; i <= total; i++)
        {
            tmp.maxVisibleCharacters = i;
            yield return new WaitForSeconds(charDelay);
        }

        routine = null;
    }

    public void Skip()
    {
        if (tmp == null) tmp = GetComponent<TextMeshProUGUI>();

        if (routine != null)
            StopCoroutine(routine);

        routine = null;
        tmp.maxVisibleCharacters = int.MaxValue;
    }

    public bool IsTyping => routine != null;
}
