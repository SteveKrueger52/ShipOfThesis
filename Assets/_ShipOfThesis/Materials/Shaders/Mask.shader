Shader "Mask/VolumeMask"
{
    SubShader
    {
        Tags { "Queue" = "Geometry+500" }
        ColorMask 0
        ZWrite On
        Pass { }
    }
}
