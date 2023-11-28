using System.Collections;
using UnityEngine;

// I am using two materials for fading
// 1st material is opaque, 2nd is using a shader to lerp transparency
// (Shader because: Directly changing value through shader will be more performant
// than altering alpha of a mesh renderer)

public class DoorMaterialFader : MonoBehaviour
{
    [SerializeField] bool fadeOnExit = false;       // enable for faded to opaque transition on exit

    [SerializeField] Material[] opaqueMaterials;
    [SerializeField] Material fadeMaterial;
    [SerializeField] MeshRenderer doorRenderer;
    [SerializeField] HideTrigger hideTrigger;

    [SerializeField] float fadeDuration = 0.5f;       // Duration of the fade in seconds
    public float FadeDuration { get { return fadeDuration; } }
    public float noTransparency = 0.0f;
    public float fadedTransparency = 0.8f;

    bool isFaded = false;

    private void Start()
    {
        // by default set the door material to opaque since door is open
        Material[] materials = doorRenderer.sharedMaterials;
        materials[0] = opaqueMaterials[0];
        materials[1] = opaqueMaterials[1];
        doorRenderer.sharedMaterials = materials;
    }

    private void Update()
    {
        // if player is fast enough to leave door on close, reset fade
        if (isFaded && !hideTrigger.playerIsInside)
        {
            QuickResetToOpaque();
        }
    }

    // called externally via player interaction script
    public void PlayDoorFade(bool isOpen)
    {
        if (isOpen)
        {
            StartCoroutine(MakeDoorInvisible());
        }
        else
        {
            if (fadeOnExit)
            {
                StartCoroutine(MakeDoorVisible());
            }
            else
            {
                QuickResetToOpaque();
            }
        }
    }

    // lerp from faded to original
    IEnumerator MakeDoorInvisible()
    {
        isFaded = true;

        if (!hideTrigger.playerIsInside) { yield break; }

        // set both materials to faded
        Material[] materials = doorRenderer.sharedMaterials;
        materials[0] = fadeMaterial;
        materials[1] = fadeMaterial;
        doorRenderer.sharedMaterials = materials;

        // Lerp transparency
        float timeElapsed = 0;
        float lerpedTransparency = fadeMaterial.GetFloat("_TransparencySlider");

        while (timeElapsed < fadeDuration)
        {
            if (!hideTrigger.playerIsInside) { yield break; }

            lerpedTransparency = Mathf.Lerp(lerpedTransparency, fadedTransparency, timeElapsed / fadeDuration);
            
            // Apply the lerped transparency to the material
            fadeMaterial.SetFloat("_TransparencySlider", lerpedTransparency);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Apply the lerped transparency to the material
        fadeMaterial.SetFloat("_TransparencySlider", lerpedTransparency);
    }

    IEnumerator MakeDoorVisible()
    {
        // Lerp transparency
        float timeElapsed = 0;
        float lerpedTransparency = fadeMaterial.GetFloat("_TransparencySlider");
        float duration = fadeDuration / 3;      // faster transition to visible

        while (timeElapsed < duration)
        {
            lerpedTransparency = Mathf.Lerp(lerpedTransparency, noTransparency, timeElapsed / duration);

            // Apply the lerped transparency to the material
            fadeMaterial.SetFloat("_TransparencySlider", lerpedTransparency);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Apply the lerped transparency to the material
        fadeMaterial.SetFloat("_TransparencySlider", lerpedTransparency);

        // set both materials back to opaque
        Material[] materials = doorRenderer.sharedMaterials;
        materials[0] = opaqueMaterials[0];
        materials[1] = opaqueMaterials[1];
        doorRenderer.sharedMaterials = materials;

        isFaded = false;
    }

    private void QuickResetToOpaque()
    {
        StopAllCoroutines();

        // Apply the lerped transparency to the material
        fadeMaterial.SetFloat("_TransparencySlider", noTransparency);

        // set both materials back to opaque
        Material[] materials = doorRenderer.sharedMaterials;
        materials[0] = opaqueMaterials[0];
        materials[1] = opaqueMaterials[1];
        doorRenderer.sharedMaterials = materials;

        isFaded = false;
    }

    // reset back to original values before exiting editor
    private void OnApplicationQuit()
    {
        fadeMaterial.SetFloat("_TransparencySlider", noTransparency);
    }
}
