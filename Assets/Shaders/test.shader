Shader "Custom/test"
{
    Properties
    {
        _Color("Color", color) = (0,0,0,0)
        _WorldSize("World Size", vector) = (1, 1, 1, 1)
        _WindSpeed("Wind Speed", vector) = (1, 1, 1, 1)
        _WindTex("Wind Texture", 2D) = "white" {}
        _WaveSpeed("Wave Speed", float) = 1.0
        _WaveAmp("Wave Amp", float) = 1.0
        _HeightCutoff("Height Cutoff", float) = 1.0
        _HeightFactor("Height Factor", float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float3 normal : NORMAL;
                float2 sp : TEXCOORD0;
			};

            sampler2D _WindTex;
            float4 _WindTex_ST;
            float4 _WorldSize;
            float4 _Color;
            float4 _WindSpeed;
            float _WaveSpeed;
            float _WaveAmp;
            float _HeightCutoff;
            float _HeightFactor;

            vertexOutput vert (vertexInput input)
            {
                vertexOutput o;
                o.pos = UnityObjectToClipPos(input.vertex);
				float4 normal4 = float4(input.normal, 0.0);
				o.normal = normalize(mul(normal4, unity_WorldToObject).xyz);

                float4 worldPos = mul(input.vertex, unity_ObjectToWorld);
                float2 samplePos = worldPos.xz/_WorldSize.xz;
                samplePos += _Time.x * _WindSpeed.xz;
                float windSample = tex2Dlod(_WindTex, float4(samplePos, 0, 0));
                
                float heightFactor = input.vertex.y > _HeightCutoff;
                heightFactor = heightFactor * pow(input.vertex.y, _HeightFactor);
                o.pos.z += sin(_WaveSpeed*windSample)*_WaveAmp;
                o.pos.x += cos(_WaveSpeed*windSample)*_WaveAmp;
                o.sp = samplePos;
                return o;
            }

            float4 frag(vertexOutput input) : COLOR
            {
                return float4(_Color);
            }
            ENDCG
        }

        Pass 
		{
			Name "CastShadow"
			Tags { "LightMode" = "ShadowCaster" }
	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"
	
			struct v2f 
			{ 
				V2F_SHADOW_CASTER;
			};
	
			v2f vert( appdata_base v )
			{
				v2f o;
				TRANSFER_SHADOW_CASTER(o)
				return o;
			}
	
			float4 frag( v2f i ) : COLOR
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
}
