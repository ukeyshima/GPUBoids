Shader "Custom/SeaweedRender"
{
    Properties
    {     
        _MainTex ("Albedo (RGB)", 2D) = "white" {}        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard vertex:vert addshadow
        #pragma instancing_options procedural:setup                
        struct Input{
            float2 uv_MainTex;
        };

        struct SeaweedData{
            float3 position;
            float3 size;
        };

        #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
        StructuredBuffer<SeaweedData> _SeaweedDataBuffer;
        #endif   

        sampler2D _MainTex;         

        float4x4 eulerAnglesToRotationMatrix(float3 angles) {
        float ch = cos(angles.y); float sh = sin(angles.y); 
        float ca = cos(angles.z); float sa = sin(angles.z); 
        float cb = cos(angles.x); float sb = sin(angles.x); 

        return float4x4( ch * ca + sh * sb * sa, -ch * sa + sh * sb * ca, sh * cb, 0, cb * sa, cb * ca, -sb, 0, -sh * ca + ch * sb * sa, sh * sa + ch * sb * ca, ch * cb, 0, 0, 0, 0, 1);
        }       

        void vert(inout appdata_full v)
        {
        #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED

        SeaweedData seaweedData = _SeaweedDataBuffer[unity_InstanceID];
        float3 pos = seaweedData.position.xyz; 
        float3 scl = seaweedData.size.xyz; 

        float4x4 object2world = (float4x4)0;    

        v.vertex.z+=sin(v.vertex.y*1.8+_Time*30.0)*smoothstep(0.2,0.9,v.vertex.y)*0.2; 

        object2world._11_22_33_44 = float4(scl.xyz, 1.0);

        float4x4 rotMatrix = eulerAnglesToRotationMatrix(float3(0,3.14/2,0));   

        object2world = mul(rotMatrix, object2world);

        object2world._14_24_34 += pos.xyz;

        v.vertex = mul(object2world, v.vertex);

        v.normal = normalize(mul(object2world, v.normal));
        #endif
        }
        void setup(){
        }


        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = float4(0.0,1.0,0.0,1.0);
            o.Albedo = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
