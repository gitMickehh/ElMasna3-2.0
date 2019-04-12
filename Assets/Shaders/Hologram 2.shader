Shader "ElMasna3/Hologram"
{
	Properties
	{
		[HDR]_HologramColor("Hologram Color", Color) = (1,1,1,0)
		_Amountofscanlines("Amount of scan lines", Float) = 10
		_ScanLinesOpacity("ScanLines Opacity", Range( 0 , 1)) = 0
		_ScanlineSpeed("Scanline Speed", Float) = 1
		_Lightoffset("Light offset", Range( 0 , 1)) = 1
		_LightRange("Light Range", Range( 0 , 2)) = 1
		_widthfade("width fade", Float) = 0.65
	}
	
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 100
		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend One One , One One
		Cull Off
		ColorMask RGBA
		ZWrite Off
		ZTest LEqual
		Offset 0 , 0
		
		

		Pass
		{
			Name "Unlit"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			uniform float4 _HologramColor;
			uniform float _Amountofscanlines;
			uniform float _ScanlineSpeed;
			uniform float _ScanLinesOpacity;
			uniform float _LightRange;
			uniform float _Lightoffset;
			uniform float _widthfade;
			uniform sampler2D _CameraDepthTexture;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 objectToViewPos = UnityObjectToViewPos(v.vertex.xyz);
				float eyeDepth = -objectToViewPos.z;
				o.ase_texcoord.z = eyeDepth;
				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord1 = screenPos;
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
				
				v.vertex.xyz +=  float3(0,0,0) ;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				fixed4 finalColor;
				float2 uv187 = i.ase_texcoord.xy * float2( 1,1 ) + float2( -0.5,0 );
				float mulTime2 = _Time.y * _ScanlineSpeed;
				float clampResult182 = clamp( (0.0 + (sin( ( _Amountofscanlines * ( uv187.y + mulTime2 ) * 6.28318548202515 ) ) - 0.0) * (_ScanLinesOpacity - 0.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
				float temp_output_107_0 = ( uv187.y * _LightRange );
				float temp_output_189_0 = ( _Lightoffset * -1.0 );
				float clampResult99 = clamp( ( 1.0 - ( temp_output_107_0 + temp_output_189_0 ) ) , 0.0 , 1.0 );
				float clampResult105 = clamp( ( 1.0 - ( ( temp_output_107_0 + temp_output_189_0 ) + 0.5 ) ) , 0.0 , 1.0 );
				float clampResult212 = clamp( ( ( clampResult105 - 0.3 ) * 5.0 ) , 0.0 , 1.0 );
				float clampResult173 = clamp( ( ( ( 1.0 - distance( uv187.x , 0.0 ) ) - _widthfade ) * 10.0 ) , 0.0 , 1.0 );
				float eyeDepth = i.ase_texcoord.z;
				float cameraDepthFade61 = (( eyeDepth -_ProjectionParams.y - 0.0 ) / 0.0);
				float clampResult74 = clamp( pow( cameraDepthFade61 , 20.0 ) , 0.0 , 1.0 );
				float4 screenPos = i.ase_texcoord1;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth63 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( screenPos ))));
				float distanceDepth63 = abs( ( screenDepth63 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 0.1 ) );
				float clampResult69 = clamp( distanceDepth63 , 0.0 , 1.0 );
				float clampResult75 = clamp( ( clampResult74 * clampResult69 ) , 0.0 , 1.0 );
				
				
				finalColor = ( _HologramColor * ( ( ( ( 0.0 + clampResult182 ) * clampResult99 ) + clampResult105 + clampResult212 ) * clampResult173 * clampResult75 ) );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=15800
524;270;1206;720;1909.102;91.28897;1;False;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;187;-1979.681,221.8823;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-0.5,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;22;-1620.499,28.14569;Float;False;Property;_ScanlineSpeed;Scanline Speed;3;0;Create;True;0;0;False;0;1;-0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-1179.225,420.0059;Float;False;Property;_Lightoffset;Light offset;4;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;181;-1673.813,487.61;Float;False;Property;_LightRange;Light Range;5;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;2;-1417.701,26.09033;Float;False;1;0;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;189;-905.8584,426.0332;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;-1336.996,415.7827;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;118;-670.7367,538.7099;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-1173.718,-67.93085;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TauNode;7;-1108.639,50.82407;Float;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1233.059,-200.3714;Float;False;Property;_Amountofscanlines;Amount of scan lines;1;0;Create;True;0;0;False;0;10;100;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-969.4141,-100.2346;Float;False;3;3;0;FLOAT;2;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;180;-507.6133,535.91;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;104;-319.3409,557.9792;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;168;-830.1082,116.8311;Float;False;Property;_ScanLinesOpacity;ScanLines Opacity;2;0;Create;True;0;0;False;0;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;6;-781.2367,-105.3887;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;117;-656.7367,244.7099;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;218;-1672.065,678.446;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;105;-119.2286,505.903;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CameraDepthFade;61;-613.2479,761.9968;Float;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;169;-468.1082,-51.16891;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;219;-1540.065,788.446;Float;False;Property;_widthfade;width fade;6;0;Create;True;0;0;False;0;0.65;0.65;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;220;-1532.065,689.446;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;211;-0.9628906,621.7059;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;63;-621.8945,882.8265;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;182;-175.6133,90.90997;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;73;-368.3433,761.0391;Float;False;2;0;FLOAT;0;False;1;FLOAT;20;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;44;-446.8309,298.8264;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;74;-213.8909,753.965;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;69;-212.7118,875.405;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;210;128.0371,615.7059;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;222;-1344.604,701.2416;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;208;61.66699,38.18408;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;99;-140.7187,302.7502;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;212;254.0371,612.7059;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;101;71.13477,172.498;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;224;-1186.604,735.2416;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-34.67899,834.1389;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;75;122.1316,818.8116;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;173;-1020.498,711.1111;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;43;289.3652,339.0702;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;114;468.9651,239.0162;Float;False;Property;_HologramColor;Hologram Color;0;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0,0.5411765,1.498039,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;450.4158,449.3539;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;188;-1400.858,571.0332;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;708.4594,413.154;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;214;1001.183,425.5547;Float;False;True;2;Float;ASEMaterialInspector;0;1;Hologram;0770190933193b94aaa3065e307002fa;0;0;Unlit;2;True;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;True;0;False;-1;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;0;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;2;0;22;0
WireConnection;189;0;57;0
WireConnection;107;0;187;2
WireConnection;107;1;181;0
WireConnection;118;0;107;0
WireConnection;118;1;189;0
WireConnection;52;0;187;2
WireConnection;52;1;2;0
WireConnection;5;0;8;0
WireConnection;5;1;52;0
WireConnection;5;2;7;0
WireConnection;180;0;118;0
WireConnection;104;0;180;0
WireConnection;6;0;5;0
WireConnection;117;0;107;0
WireConnection;117;1;189;0
WireConnection;218;0;187;1
WireConnection;105;0;104;0
WireConnection;169;0;6;0
WireConnection;169;4;168;0
WireConnection;220;0;218;0
WireConnection;211;0;105;0
WireConnection;182;0;169;0
WireConnection;73;0;61;0
WireConnection;44;0;117;0
WireConnection;74;0;73;0
WireConnection;69;0;63;0
WireConnection;210;0;211;0
WireConnection;222;0;220;0
WireConnection;222;1;219;0
WireConnection;208;1;182;0
WireConnection;99;0;44;0
WireConnection;212;0;210;0
WireConnection;101;0;208;0
WireConnection;101;1;99;0
WireConnection;224;0;222;0
WireConnection;70;0;74;0
WireConnection;70;1;69;0
WireConnection;75;0;70;0
WireConnection;173;0;224;0
WireConnection;43;0;101;0
WireConnection;43;1;105;0
WireConnection;43;2;212;0
WireConnection;79;0;43;0
WireConnection;79;1;173;0
WireConnection;79;2;75;0
WireConnection;188;0;181;0
WireConnection;115;0;114;0
WireConnection;115;1;79;0
WireConnection;214;0;115;0
ASEEND*/
//CHKSM=4C205D57FDAF5E750DFEB190F26D14404D90CCEC