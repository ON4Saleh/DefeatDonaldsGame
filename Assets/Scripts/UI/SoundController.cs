using UnityEngine;
using UnityEngine.UI;
public class SoundController : MonoBehaviour
{
    [SerializeField] Slider VolSlider;
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1f);
        }
        else
        {
            load();
        }
        
    }
    public void changevolume()
    {
        AudioListener.volume = VolSlider.value;
        save();
    }
    public void load()
    {
        VolSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    public void save()
    {
        PlayerPrefs.SetFloat("musicVolume", VolSlider.value);
    }
}
