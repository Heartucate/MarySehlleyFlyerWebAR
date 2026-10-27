using UnityEngine;

[System.Serializable]
public class ShaderSettings
{
    [Header("Sizes")]
    public float mainsize;
    public float size2;
    public float size3;
    public float size4;

    [Header("Other")]
    public float mainAlpha;
    public float deformationStrength;
}

public class MaryCondition : MonoBehaviour
{
    [Header("Shader Variants")]
    [SerializeField] private ShaderSettings[] _variants = new ShaderSettings[4];

    private int _currentVariant;

    public bool _voiceModule = false;

    [SerializeField]
    private float _headSize;

    [Header("References")]
    [SerializeField] private MaryGlitchSettings settings;

    public void Start()
    {
        ApplySettings();
        UpdateMaryShelley();
    }

    private void OnVariantChanged(int oldValue, int newValue)
    {
        UpdateMaryShelley();
    }

    private void OnVoiceModuleChanged(bool oldValue, bool newValue)
    {
        UpdateMaryShelley();
    }



    public void CmdAddProgress(int value)
    {
        _currentVariant = Mathf.Clamp(_currentVariant + value, 0, _variants.Length - 1);
    }

    public void CmdSetVoiceModule(bool enabled)
    {
        _voiceModule = enabled;
    }

    
    private void ApplySettings()
    {
        if (settings == null) return;

        Shader.SetGlobalFloat("_Mary_BandCenterLength", settings.BandAxisCenter);
        Shader.SetGlobalFloat("_Mary_BandAxisLength", settings.BandAxisLength);
        Shader.SetGlobalFloat("_Mary_BandSpeed", settings.BandSpeed);
        Shader.SetGlobalFloat("_Mary_BandTransitionLength", settings.TransitionLength);
        Shader.SetGlobalVector("_Mary_BandAxis", settings.BandAxis);

        Shader.SetGlobalFloat("_Mary_Band_Speed_1", settings.BandSpeed1);
        Shader.SetGlobalFloat("_Mary_Band_Offset_1", settings.BandOffset1);
        Shader.SetGlobalFloat("_Mary_Band_Speed_2", settings.BandSpeed2);
        Shader.SetGlobalFloat("_Mary_Band_Offset_2", settings.BandOffset2);
        Shader.SetGlobalFloat("_Mary_Band_Speed_3", settings.BandSpeed3);
        Shader.SetGlobalFloat("_Mary_Band_Offset_3", settings.BandOffset3);
        Shader.SetGlobalFloat("_Mary_Band_Speed_4", settings.BandSpeed4);
        Shader.SetGlobalFloat("_Mary_Band_Offset_4", settings.BandOffset4);
    }

    public void UpdateMaryShelley()
    {
        if (_variants == null || _variants.Length == 0)
            return;

        ShaderSettings v = _variants[Mathf.Clamp(_currentVariant, 0, _variants.Length - 1)];

        Shader.SetGlobalFloat("_Mary_Band_Size_2", v.size2);
        Shader.SetGlobalFloat("_Mary_Band_Size_3", v.size3);
        Shader.SetGlobalFloat("_Mary_Band_Size_4", v.size4);

        Shader.SetGlobalFloat("_Mary_Deformation_Strength", v.deformationStrength);
        Shader.SetGlobalFloat("_Mary_MainColor", v.mainAlpha);
        Shader.SetGlobalFloat("_Mary_BandTransitionLength", v.mainsize);

        Shader.SetGlobalFloat(
            "_Mary_Band_Size_1",
            _voiceModule ? 0f : _headSize
         );
        Debug.Log($"Updating Mary Shelley: Variant={_currentVariant}, VoiceModule={_voiceModule}, HeadSize={_headSize}");
    }

    [ContextMenu("Set First Variant")]
    public void SetFirstVariant()
    {
        _currentVariant = 0;
        UpdateMaryShelley();
    }

    [ContextMenu("Set Last Variant")]
    public void SetLastVariant()
    {
        _currentVariant = _variants.Length - 1;
        UpdateMaryShelley();
    }
}