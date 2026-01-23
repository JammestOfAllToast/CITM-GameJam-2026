using UnityEngine;
using TMPro;
public class TimeController : MonoBehaviour
{
    [Header("Time Variables")]
    public float StartingHour;
    public float TimeMultiplier;
    public float EndHour;
    public float CurrentTime;
    private int seconds;
    private int minutes;
    [Header("TextMeshPro")]
    [SerializeField] TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentTime = StartingHour;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime += TimeMultiplier * Time.deltaTime;
        minutes = (int)CurrentTime / 60;
        seconds = (int)CurrentTime % 60;
        text.text = (minutes.ToString("00"))+":"+(seconds.ToString("00"));
    }
}
