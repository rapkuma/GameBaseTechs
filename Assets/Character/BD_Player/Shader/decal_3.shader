Shader "Custom/decal_3" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_DecalTex ("Decal (RGBA)", 2D) = "black" {}
	_DecalTex_2 ("Decal_2 (RGBA)", 2D) = "black" {}
}

SubShader {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 250
	
CGPROGRAM
#pragma surface surf Lambert alpha

sampler2D _MainTex;
sampler2D _DecalTex;
sampler2D _DecalTex_2;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
	float2 uv_DecalTex;
	float2 uv_DecalTex_2;
};

void surf (Input IN, inout SurfaceOutput o) {

	half4 c;
	half4 org = tex2D(_MainTex, IN.uv_MainTex);
	half4 decal = tex2D(_DecalTex, IN.uv_DecalTex);
	c.rgb = lerp (org.rgb, decal.rgb, decal.a);
	
	half4 decal_2 = tex2D(_DecalTex_2, IN.uv_DecalTex_2);
	c.rgb = lerp (c.rgb, decal_2.rgb, decal_2.a);
	
	o.Albedo = c.rgb*_Color.rgb;
	o.Alpha = org.a + decal.a + decal_2.a;
}
ENDCG
}

Fallback "Transparent/VertexLit"
}