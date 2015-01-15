Shader "Custom/3DMenu" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    Category {
       Lighting Off
       ZWrite Off
       Cull Off
       ZTest Always
       Blend SrcAlpha OneMinusSrcAlpha
       Fog {Mode off}
       Tags {Queue=Transparent}
       SubShader {
            Pass {
               SetTexture [_MainTex] {
                    constantColor [_Color]
                    Combine texture * constant, texture * constant 
                 }
            }
        } 
    }
}