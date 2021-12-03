// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX/Matcap"
{
	Properties
	{
		_Main_or_Fresnel("Main_or_Fresnel", Range( 0 , 1)) = 0
		_MetallicTex("MetallicTex", 2D) = "white" {}
		[Toggle]_Metallic("Metallic", Float) = 1
		_MainTex("MainTex", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		[HDR]_Fresnel_Color("Fresnel_Color", Color) = (0,0,0,0)
		_Fresnel_power("Fresnel_power", Float) = 0
		_Fresnel_Range_Step("Fresnel_Range_Step", Float) = 0
		_Highlight_Color("Highlight_Color", Color) = (1,1,1,1)
		_Highlight_Range("Highlight_Range", Float) = 0
		_Shadow("Shadow", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 100

		CGINCLUDE
		#pragma target 2.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			
			CGPROGRAM

#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
		//only defining to not throw compilation error over Unity 5.5
		#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
			};

			//This is a late directive
			
			uniform float _Metallic;
			uniform sampler2D _MetallicTex;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float4 _Color;
			uniform float _Highlight_Range;
			uniform float4 _Highlight_Color;
			uniform float _Shadow;
			uniform float _Fresnel_Range_Step;
			uniform float _Fresnel_power;
			uniform float4 _Fresnel_Color;
			uniform float _Main_or_Fresnel;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord.xyz = ase_worldNormal;
				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord2.xyz = ase_worldPos;
				
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
				o.ase_texcoord1.zw = 0;
				o.ase_texcoord2.w = 0;
				float3 vertexValue =  float3(0,0,0) ;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				float3 ase_worldNormal = i.ase_texcoord.xyz;
				float3 normalizeResult5 = normalize( mul( UNITY_MATRIX_V, float4( ase_worldNormal , 0.0 ) ).xyz );
				float2 uv_MainTex = i.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode54 = tex2D( _MainTex, uv_MainTex );
				float3 ase_worldPos = i.ase_texcoord2.xyz;
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(ase_worldPos);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 worldSpaceLightDir = UnityWorldSpaceLightDir(ase_worldPos);
				float3 normalizeResult4_g13 = normalize( ( ase_worldViewDir + worldSpaceLightDir ) );
				float dotResult82 = dot( normalizeResult4_g13 , ase_worldNormal );
				float temp_output_2_0_g14 = _Highlight_Range;
				float temp_output_87_0 = saturate( ( ( dotResult82 - temp_output_2_0_g14 ) / ( 1.0 - temp_output_2_0_g14 ) ) );
				float temp_output_89_0 = ( temp_output_87_0 * temp_output_87_0 * temp_output_87_0 );
				float4 appendResult90 = (float4(lerp(tex2D( _MetallicTex, (normalizeResult5*0.5 + 0.5).xy ),( ( ( tex2DNode54 * _Color ) + ( temp_output_89_0 * _Highlight_Color ) ) * saturate( ( temp_output_89_0 + _Shadow ) ) ),_Metallic).rgb , tex2DNode54.a));
				float dotResult60 = dot( ase_worldNormal , ase_worldViewDir );
				float temp_output_2_0_g15 = _Fresnel_Range_Step;
				float temp_output_69_0 = saturate( pow( saturate( ( ( ( 1.0 - abs( dotResult60 ) ) - temp_output_2_0_g15 ) / ( 1.0 - temp_output_2_0_g15 ) ) ) , _Fresnel_power ) );
				float4 appendResult72 = (float4(( temp_output_69_0 * _Fresnel_Color ).rgb , ( temp_output_69_0 * _Fresnel_Color.a * tex2DNode54.a )));
				float4 lerpResult57 = lerp( appendResult90 , appendResult72 , _Main_or_Fresnel);
				
				
				finalColor = lerpResult57;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16800
2177;87;1405;843;1269.291;-491.3458;1.6;True;False
Node;AmplifyShaderEditor.WorldNormalVector;59;267.7193,769.2211;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FunctionNode;80;-733.0807,884.8711;Float;True;Blinn-Phong Half Vector;-1;;13;91a149ac9d615be429126c95e20753ce;0;0;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;83;-667.3178,1128.885;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;61;268.1361,1132.716;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;88;-445.6688,1214.398;Float;False;Property;_Highlight_Range;Highlight_Range;9;0;Create;True;0;0;False;0;0;-0.94;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;82;-417.2178,970.3862;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;60;412.4094,943.4537;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;62;595.812,972.3076;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewMatrixNode;4;-1253.53,-254.1411;Float;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.WorldNormalVector;2;-1319.857,-20.14429;Float;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FunctionNode;87;-213.9578,737.6777;Float;False;SmoothStep_new;-1;;14;b1c06ed402eb4b644ba5da4f3236cde6;0;3;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;86;-377.9089,1302.279;Float;False;Property;_Highlight_Color;Highlight_Color;8;0;Create;True;0;0;False;0;1,1,1,1;0.2358491,0.2358491,0.2358491,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-987.8573,-97.14429;Float;True;2;2;0;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;76;754.3928,1463.44;Float;False;Property;_Fresnel_Range_Step;Fresnel_Range_Step;7;0;Create;True;0;0;False;0;0;-1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;94;95.3457,1387.467;Float;False;Property;_Shadow;Shadow;10;0;Create;True;0;0;False;0;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;54;-532.4229,311.6031;Float;True;Property;_MainTex;MainTex;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;-42.74822,943.47;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;56;-493.4674,632.0167;Float;False;Property;_Color;Color;4;0;Create;True;0;0;False;0;1,1,1,1;0.5471698,0.2656901,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;63;848.3652,966.6639;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;93;261.1216,1304.579;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;812.865,1233.895;Float;False;Property;_Fresnel_power;Fresnel_power;6;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-191.3496,493.734;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;196.1423,981.3145;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;75;1012.797,1420.606;Float;False;SmoothStep_new;-1;;15;b1c06ed402eb4b644ba5da4f3236cde6;0;3;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;5;-742.8573,-94.14429;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;84;312.6722,545.7183;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;92;457.9943,1325.329;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;6;-565.8573,-101.1443;Float;True;3;0;FLOAT3;0,0,0;False;1;FLOAT;0.5;False;2;FLOAT;0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;67;1088.91,993.7703;Float;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-263,-111.5;Float;True;Property;_MetallicTex;MetallicTex;1;0;Create;True;0;0;False;0;08cf28f03fe2dc041b367c7ba319f9ec;fa5f7f2397d3fd743afee880761593e7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;71;1412.73,1310.926;Float;False;Property;_Fresnel_Color;Fresnel_Color;5;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,1.176471,1.498039,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;486.1097,621.5759;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;69;1355.562,954.4354;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;53;475.4522,118.6253;Float;False;Property;_Metallic;Metallic;2;0;Create;True;0;0;False;0;1;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;1779.049,1268.74;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;1611.076,945.5491;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;72;1853.596,382.6795;Float;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;90;1060.301,155.8022;Float;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;58;635.7436,533.6203;Float;False;Property;_Main_or_Fresnel;Main_or_Fresnel;0;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;57;1982.613,-2.787873;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;2514.142,-169.0185;Float;False;True;2;Float;ASEMaterialInspector;0;1;FX/Matcap;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;2;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;True;0;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;82;0;80;0
WireConnection;82;1;83;0
WireConnection;60;0;59;0
WireConnection;60;1;61;0
WireConnection;62;0;60;0
WireConnection;87;1;82;0
WireConnection;87;2;88;0
WireConnection;3;0;4;0
WireConnection;3;1;2;0
WireConnection;89;0;87;0
WireConnection;89;1;87;0
WireConnection;89;2;87;0
WireConnection;63;0;62;0
WireConnection;93;0;89;0
WireConnection;93;1;94;0
WireConnection;55;0;54;0
WireConnection;55;1;56;0
WireConnection;85;0;89;0
WireConnection;85;1;86;0
WireConnection;75;1;63;0
WireConnection;75;2;76;0
WireConnection;5;0;3;0
WireConnection;84;0;55;0
WireConnection;84;1;85;0
WireConnection;92;0;93;0
WireConnection;6;0;5;0
WireConnection;67;0;75;0
WireConnection;67;1;68;0
WireConnection;1;1;6;0
WireConnection;95;0;84;0
WireConnection;95;1;92;0
WireConnection;69;0;67;0
WireConnection;53;0;1;0
WireConnection;53;1;95;0
WireConnection;73;0;69;0
WireConnection;73;1;71;4
WireConnection;73;2;54;4
WireConnection;70;0;69;0
WireConnection;70;1;71;0
WireConnection;72;0;70;0
WireConnection;72;3;73;0
WireConnection;90;0;53;0
WireConnection;90;3;54;4
WireConnection;57;0;90;0
WireConnection;57;1;72;0
WireConnection;57;2;58;0
WireConnection;0;0;57;0
ASEEND*/
//CHKSM=6A3E5EC8AE2ADBCA4F39655CD8FE347CC21155C2