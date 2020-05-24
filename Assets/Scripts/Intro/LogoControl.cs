using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoControl : MonoBehaviour {
    float counter = 0f;
    float radius = 1f;
    float remove = 0.1f;
    AudioSource asrc;
    RectTransform recttrans;

    public AudioClip sndintrocoin;
    public AudioClip sndintrorupee;
    public AudioClip sndintromario;
    public AudioClip sndintrolink;

    bool introlink = false;
    bool voiceplay = false;

    void Start() {
        asrc = GetComponent<AudioSource>();
        recttrans = GetComponent<RectTransform>();

        if (Random.Range(0, 2) == 1)
            introlink = true;

        if (!introlink)
            asrc.PlayOneShot(sndintrocoin);
        else
            asrc.PlayOneShot(sndintrorupee);
    }

    void Update() {
        // changing size of the image
        recttrans.sizeDelta = new Vector2(449f - Mathf.Cos(counter) * radius * 449f, 213f - Mathf.Cos(counter) * radius * 213f);

        counter += 12f * Time.deltaTime;
        if (radius > 0.3f)
            radius -= 3f * Time.deltaTime;

        if (radius <= 0.3f && radius > 0)
            radius -= 0.5f * Time.deltaTime;

        if (radius < 0)
            radius = 0f;

        // playing mario's or link's voice
        if (counter > 8f && !voiceplay) {
            if (!introlink)
                asrc.PlayOneShot(sndintromario);
            else
                asrc.PlayOneShot(sndintrolink);
            
            voiceplay = true;
        }

        if (counter > 40f) {
            recttrans.sizeDelta = new Vector2(460f - remove * 449f, 224f - remove * 213f);
            remove *= 1.05f;
        }

        if (counter > 50f)
            SceneManager.LoadScene("Testing");
    }
}
