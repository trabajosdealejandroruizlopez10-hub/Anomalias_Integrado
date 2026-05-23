using UnityEngine;
using TMPro;

public class GlitchText : MonoBehaviour
{
    TMP_Text txt;
    Vector3 startPos;

    public float amount = 2f;
    public float colorSpeed = 0.1f;

    float timer;

    Color[] colors =
    {
        Color.cyan,
        Color.magenta,
        Color.red,
        Color.green,
    };

    void Start()
    {
        txt = GetComponent<TMP_Text>();
        startPos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = startPos + new Vector3(
            Random.Range(-amount, amount),
            Random.Range(-amount, amount),
            0
        );

        timer += Time.deltaTime;

        if (timer >= colorSpeed)
        {
            txt.color = colors[Random.Range(0, colors.Length)];
            timer = 0f;
        }
    }
}