using UnityEngine;
public class ThemeMusic : MonoBehaviour
{
    private AudioSource source;

    public static ThemeMusic instance;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

    }


    private void Start()
    {
        source = gameObject.GetComponent<AudioSource>();

        if (PlayerPrefs.GetInt("isMusicOn") == 0)
        {
            source.enabled = true;
            source.mute = false;
            source.Play();
        }
        else
        {
            source.mute = true;
            source.enabled = false;
        }
    }

    public void CalculateMusic()
    {
        if (PlayerPrefs.GetInt("isMusicOn") == 0)
        {
            source.enabled = true;
            source.mute = false;
        }
        else
        {
            source.mute = true;
            source.enabled = false;
        }

    }

    public void StopMusic()
    {
        source.mute = true;
    }

    public void PlayMusic()
    {
        source.mute = false;
    }
}