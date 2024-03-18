Shader "Frame_Alfa"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 0)
        _ScrollSpeed("ScrollSpeed", Float) = 1.74
        [NonModifiableTextureData][NoScaleOffset]_SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71_BaseMap_2957833126_Texture2D("Texture2D", 2D) = "white" {}
        [NonModifiableTextureData][NoScaleOffset]_SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_Texture_1_Texture2D("Texture2D", 2D) = "white" {}
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Lit"
            "Queue"="Transparent"
            // DisableBatching: <None>
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalSpriteLitSubTarget"
        }
        Pass
        {
            Name "Sprite Lit"
            Tags
            {
                "LightMode" = "Universal2D"
            }
        
        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_0
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_1
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_2
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define VARYINGS_NEED_SCREENPOSITION
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITELIT
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
             float4 screenPosition;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpaceViewDirection;
             float2 NDCPosition;
             float2 PixelPosition;
             float4 uv0;
             float4 VertexColor;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
             float4 screenPosition : INTERP2;
             float3 positionWS : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            output.screenPosition.xyzw = input.screenPosition;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            output.screenPosition = input.screenPosition.xyzw;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_Texture_1_Texture2D_TexelSize;
        float4 _SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71_BaseMap_2957833126_Texture2D_TexelSize;
        float _ScrollSpeed;
        float4 _Color;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_Texture_1_Texture2D);
        SAMPLER(sampler_SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_Texture_1_Texture2D);
        TEXTURE2D(_SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71_BaseMap_2957833126_Texture2D);
        SAMPLER(sampler_SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71_BaseMap_2957833126_Texture2D);
        
        // Graph Includes
        #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/LODDitheringTransition.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
        Out = A * B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Fraction_float(float In, out float Out)
        {
            Out = frac(In);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
        Out = A * B;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Blend_Overlay_float4(float4 Base, float4 Blend, out float4 Out, float Opacity)
        {
            float4 result1 = 1.0 - 2.0 * (1.0 - Base) * (1.0 - Blend);
            float4 result2 = 2.0 * Base * Blend;
            float4 zeroOrOne = step(Base, 0.5);
            Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
            Out = lerp(Base, Out, Opacity);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Maximum_float(float A, float B, out float Out)
        {
            Out = max(A, B);
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Saturate_float4(float4 In, out float4 Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        struct Bindings_SpeedTree8ColorAlpha_1b4ecad27a9bc714e8d3af3ffb8a368c_float
        {
        float3 WorldSpaceViewDirection;
        float4 VertexColor;
        float2 NDCPosition;
        half4 uv0;
        };
        
        void SG_SpeedTree8ColorAlpha_1b4ecad27a9bc714e8d3af3ffb8a368c_float(UnityTexture2D Base_Map, float4 Color_Tint, float Enable_Hue_Variation, float4 Hue_Variation_Color, float Use_Old_Hue_Variation_Behavior, float Is_Billboard, float Crossfade, Bindings_SpeedTree8ColorAlpha_1b4ecad27a9bc714e8d3af3ffb8a368c_float IN, out float3 Modified_Color_1, out float Modified_Alpha_4, out float3 Original_Color_2, out float Original_Alpha_3)
        {
        float _Property_4ec1fadc986743f2b9b3be9ad07b5c23_Out_0_Boolean = Enable_Hue_Variation;
        float _Property_80c510042dc848db99c93f2d10c93a45_Out_0_Boolean = Use_Old_Hue_Variation_Behavior;
        float4 _Property_3447ed3cbe7e4c0ca03d34219340dbda_Out_0_Vector4 = Color_Tint;
        UnityTexture2D _Property_9739be6f931c48a2ab08fb60abd88eac_Out_0_Texture2D = Base_Map;
        float4 _SampleTexture2D_f0368572efa94c5ebedc97b7f89e54d4_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_9739be6f931c48a2ab08fb60abd88eac_Out_0_Texture2D.tex, _Property_9739be6f931c48a2ab08fb60abd88eac_Out_0_Texture2D.samplerstate, _Property_9739be6f931c48a2ab08fb60abd88eac_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
        float _SampleTexture2D_f0368572efa94c5ebedc97b7f89e54d4_R_4_Float = _SampleTexture2D_f0368572efa94c5ebedc97b7f89e54d4_RGBA_0_Vector4.r;
        float _SampleTexture2D_f0368572efa94c5ebedc97b7f89e54d4_G_5_Float = _SampleTexture2D_f0368572efa94c5ebedc97b7f89e54d4_RGBA_0_Vector4.g;
        float _SampleTexture2D_f0368572efa94c5ebedc97b7f89e54d4_B_6_Float = _SampleTexture2D_f0368572efa94c5ebedc97b7f89e54d4_RGBA_0_Vector4.b;
        float _SampleTexture2D_f0368572efa94c5ebedc97b7f89e54d4_A_7_Float = _SampleTexture2D_f0368572efa94c5ebedc97b7f89e54d4_RGBA_0_Vector4.a;
        float4 _Multiply_37087b60e9d043069303dd34abafccdd_Out_2_Vector4;
        Unity_Multiply_float4_float4(_Property_3447ed3cbe7e4c0ca03d34219340dbda_Out_0_Vector4, _SampleTexture2D_f0368572efa94c5ebedc97b7f89e54d4_RGBA_0_Vector4, _Multiply_37087b60e9d043069303dd34abafccdd_Out_2_Vector4);
        float4 _Property_44dfa3cd977d45adb4d44efa3fae33be_Out_0_Vector4 = Hue_Variation_Color;
        float _Split_1e71fee3241d42eea8e7ee1371975d5c_R_1_Float = _Property_44dfa3cd977d45adb4d44efa3fae33be_Out_0_Vector4[0];
        float _Split_1e71fee3241d42eea8e7ee1371975d5c_G_2_Float = _Property_44dfa3cd977d45adb4d44efa3fae33be_Out_0_Vector4[1];
        float _Split_1e71fee3241d42eea8e7ee1371975d5c_B_3_Float = _Property_44dfa3cd977d45adb4d44efa3fae33be_Out_0_Vector4[2];
        float _Split_1e71fee3241d42eea8e7ee1371975d5c_A_4_Float = _Property_44dfa3cd977d45adb4d44efa3fae33be_Out_0_Vector4[3];
        float3 _Transform_16910a20060c43f889cace1d074fd2ca_Out_1_Vector3;
        {
        // Converting Position from Object to AbsoluteWorld via world space
        float3 world;
        world = TransformObjectToWorld(float3 (0, 0, 0).xyz);
        _Transform_16910a20060c43f889cace1d074fd2ca_Out_1_Vector3 = GetAbsolutePositionWS(world);
        }
        float _Split_afb69a2ae3904da0b6b18f0a8fed3415_R_1_Float = _Transform_16910a20060c43f889cace1d074fd2ca_Out_1_Vector3[0];
        float _Split_afb69a2ae3904da0b6b18f0a8fed3415_G_2_Float = _Transform_16910a20060c43f889cace1d074fd2ca_Out_1_Vector3[1];
        float _Split_afb69a2ae3904da0b6b18f0a8fed3415_B_3_Float = _Transform_16910a20060c43f889cace1d074fd2ca_Out_1_Vector3[2];
        float _Split_afb69a2ae3904da0b6b18f0a8fed3415_A_4_Float = 0;
        float _Add_0bc75546351b442e9e40b2e2e104d829_Out_2_Float;
        Unity_Add_float(_Split_afb69a2ae3904da0b6b18f0a8fed3415_R_1_Float, _Split_afb69a2ae3904da0b6b18f0a8fed3415_G_2_Float, _Add_0bc75546351b442e9e40b2e2e104d829_Out_2_Float);
        float _Add_de0b04c2427447f38fb590f8c1b2d313_Out_2_Float;
        Unity_Add_float(_Add_0bc75546351b442e9e40b2e2e104d829_Out_2_Float, _Split_afb69a2ae3904da0b6b18f0a8fed3415_B_3_Float, _Add_de0b04c2427447f38fb590f8c1b2d313_Out_2_Float);
        float _Fraction_740ba08223a24b0d9fcea47fcbe15b39_Out_1_Float;
        Unity_Fraction_float(_Add_de0b04c2427447f38fb590f8c1b2d313_Out_2_Float, _Fraction_740ba08223a24b0d9fcea47fcbe15b39_Out_1_Float);
        float _Multiply_7316ba902de94fdca1789a790def91ff_Out_2_Float;
        Unity_Multiply_float_float(_Split_1e71fee3241d42eea8e7ee1371975d5c_A_4_Float, _Fraction_740ba08223a24b0d9fcea47fcbe15b39_Out_1_Float, _Multiply_7316ba902de94fdca1789a790def91ff_Out_2_Float);
        float _Saturate_2692c4746b544ace91301f7dc416c003_Out_1_Float;
        Unity_Saturate_float(_Multiply_7316ba902de94fdca1789a790def91ff_Out_2_Float, _Saturate_2692c4746b544ace91301f7dc416c003_Out_1_Float);
        float4 _Lerp_3f3e8e2911e4473b9baaded23518dc75_Out_3_Vector4;
        Unity_Lerp_float4(_Multiply_37087b60e9d043069303dd34abafccdd_Out_2_Vector4, _Property_44dfa3cd977d45adb4d44efa3fae33be_Out_0_Vector4, (_Saturate_2692c4746b544ace91301f7dc416c003_Out_1_Float.xxxx), _Lerp_3f3e8e2911e4473b9baaded23518dc75_Out_3_Vector4);
        float4 _Blend_de4f34f2bb7a46769d53aa730fdbebda_Out_2_Vector4;
        Unity_Blend_Overlay_float4(_Multiply_37087b60e9d043069303dd34abafccdd_Out_2_Vector4, _Property_44dfa3cd977d45adb4d44efa3fae33be_Out_0_Vector4, _Blend_de4f34f2bb7a46769d53aa730fdbebda_Out_2_Vector4, _Saturate_2692c4746b544ace91301f7dc416c003_Out_1_Float);
        float4 _Branch_eea3f9e8c5df4bf79dc691911a8e7d45_Out_3_Vector4;
        Unity_Branch_float4(_Property_80c510042dc848db99c93f2d10c93a45_Out_0_Boolean, _Lerp_3f3e8e2911e4473b9baaded23518dc75_Out_3_Vector4, _Blend_de4f34f2bb7a46769d53aa730fdbebda_Out_2_Vector4, _Branch_eea3f9e8c5df4bf79dc691911a8e7d45_Out_3_Vector4);
        float _Split_8b255101be0e4c0686ecfb357b4e08c6_R_1_Float = _Multiply_37087b60e9d043069303dd34abafccdd_Out_2_Vector4[0];
        float _Split_8b255101be0e4c0686ecfb357b4e08c6_G_2_Float = _Multiply_37087b60e9d043069303dd34abafccdd_Out_2_Vector4[1];
        float _Split_8b255101be0e4c0686ecfb357b4e08c6_B_3_Float = _Multiply_37087b60e9d043069303dd34abafccdd_Out_2_Vector4[2];
        float _Split_8b255101be0e4c0686ecfb357b4e08c6_A_4_Float = _Multiply_37087b60e9d043069303dd34abafccdd_Out_2_Vector4[3];
        float _Maximum_172119ade14f4f779d4ae5d9d20db683_Out_2_Float;
        Unity_Maximum_float(_Split_8b255101be0e4c0686ecfb357b4e08c6_R_1_Float, _Split_8b255101be0e4c0686ecfb357b4e08c6_G_2_Float, _Maximum_172119ade14f4f779d4ae5d9d20db683_Out_2_Float);
        float _Maximum_c521d868294841f4b71b21734f9cf047_Out_2_Float;
        Unity_Maximum_float(_Maximum_172119ade14f4f779d4ae5d9d20db683_Out_2_Float, _Split_8b255101be0e4c0686ecfb357b4e08c6_B_3_Float, _Maximum_c521d868294841f4b71b21734f9cf047_Out_2_Float);
        float _Split_d9b6fd95965b407abd03352c64bd95d4_R_1_Float = _Branch_eea3f9e8c5df4bf79dc691911a8e7d45_Out_3_Vector4[0];
        float _Split_d9b6fd95965b407abd03352c64bd95d4_G_2_Float = _Branch_eea3f9e8c5df4bf79dc691911a8e7d45_Out_3_Vector4[1];
        float _Split_d9b6fd95965b407abd03352c64bd95d4_B_3_Float = _Branch_eea3f9e8c5df4bf79dc691911a8e7d45_Out_3_Vector4[2];
        float _Split_d9b6fd95965b407abd03352c64bd95d4_A_4_Float = _Branch_eea3f9e8c5df4bf79dc691911a8e7d45_Out_3_Vector4[3];
        float _Maximum_e25c31be13314b87ae27a198895b64a2_Out_2_Float;
        Unity_Maximum_float(_Split_d9b6fd95965b407abd03352c64bd95d4_R_1_Float, _Split_d9b6fd95965b407abd03352c64bd95d4_G_2_Float, _Maximum_e25c31be13314b87ae27a198895b64a2_Out_2_Float);
        float _Maximum_4315d9c44c34415fbb4d00a6c0e720f6_Out_2_Float;
        Unity_Maximum_float(_Maximum_e25c31be13314b87ae27a198895b64a2_Out_2_Float, _Split_d9b6fd95965b407abd03352c64bd95d4_B_3_Float, _Maximum_4315d9c44c34415fbb4d00a6c0e720f6_Out_2_Float);
        float _Divide_e673cbf0a11048c092ecf1e1fbc09be8_Out_2_Float;
        Unity_Divide_float(_Maximum_c521d868294841f4b71b21734f9cf047_Out_2_Float, _Maximum_4315d9c44c34415fbb4d00a6c0e720f6_Out_2_Float, _Divide_e673cbf0a11048c092ecf1e1fbc09be8_Out_2_Float);
        float _Multiply_8b827c41892549ebb55b1a4907a92bf2_Out_2_Float;
        Unity_Multiply_float_float(_Divide_e673cbf0a11048c092ecf1e1fbc09be8_Out_2_Float, 0.5, _Multiply_8b827c41892549ebb55b1a4907a92bf2_Out_2_Float);
        float _Add_b86fceb0db8d4af9bc21dfe417af2843_Out_2_Float;
        Unity_Add_float(_Multiply_8b827c41892549ebb55b1a4907a92bf2_Out_2_Float, 0.5, _Add_b86fceb0db8d4af9bc21dfe417af2843_Out_2_Float);
        float4 _Multiply_4bb9f6177dff47bbb3c65a7fc5da20af_Out_2_Vector4;
        Unity_Multiply_float4_float4(_Branch_eea3f9e8c5df4bf79dc691911a8e7d45_Out_3_Vector4, (_Add_b86fceb0db8d4af9bc21dfe417af2843_Out_2_Float.xxxx), _Multiply_4bb9f6177dff47bbb3c65a7fc5da20af_Out_2_Vector4);
        float4 _Saturate_4ebc8100309e4882b91c688956b0477c_Out_1_Vector4;
        Unity_Saturate_float4(_Multiply_4bb9f6177dff47bbb3c65a7fc5da20af_Out_2_Vector4, _Saturate_4ebc8100309e4882b91c688956b0477c_Out_1_Vector4);
        float4 _Branch_38536d5bf09446bc9e20abbd05f134e7_Out_3_Vector4;
        Unity_Branch_float4(_Property_4ec1fadc986743f2b9b3be9ad07b5c23_Out_0_Boolean, _Saturate_4ebc8100309e4882b91c688956b0477c_Out_1_Vector4, _Multiply_37087b60e9d043069303dd34abafccdd_Out_2_Vector4, _Branch_38536d5bf09446bc9e20abbd05f134e7_Out_3_Vector4);
        float _Property_c660a337893a4106a47253f4fdaa173d_Out_0_Boolean = Crossfade;
        float _Property_321d0864f8e24c789d8ead6ed475e3c3_Out_0_Boolean = Is_Billboard;
        float _Split_8e113e5414194688aa2c165814b6360b_R_1_Float = _SampleTexture2D_f0368572efa94c5ebedc97b7f89e54d4_RGBA_0_Vector4[0];
        float _Split_8e113e5414194688aa2c165814b6360b_G_2_Float = _SampleTexture2D_f0368572efa94c5ebedc97b7f89e54d4_RGBA_0_Vector4[1];
        float _Split_8e113e5414194688aa2c165814b6360b_B_3_Float = _SampleTexture2D_f0368572efa94c5ebedc97b7f89e54d4_RGBA_0_Vector4[2];
        float _Split_8e113e5414194688aa2c165814b6360b_A_4_Float = _SampleTexture2D_f0368572efa94c5ebedc97b7f89e54d4_RGBA_0_Vector4[3];
        float _Split_d5fe6b04dd9747b6a9e6d27930d2ffcb_R_1_Float = IN.VertexColor[0];
        float _Split_d5fe6b04dd9747b6a9e6d27930d2ffcb_G_2_Float = IN.VertexColor[1];
        float _Split_d5fe6b04dd9747b6a9e6d27930d2ffcb_B_3_Float = IN.VertexColor[2];
        float _Split_d5fe6b04dd9747b6a9e6d27930d2ffcb_A_4_Float = IN.VertexColor[3];
        float _Multiply_e8c48c2857c946f78c6f01261fe2553c_Out_2_Float;
        Unity_Multiply_float_float(_Split_d5fe6b04dd9747b6a9e6d27930d2ffcb_A_4_Float, _Split_8e113e5414194688aa2c165814b6360b_A_4_Float, _Multiply_e8c48c2857c946f78c6f01261fe2553c_Out_2_Float);
        float _Branch_5bd4b0e2a77b4a918e51059017f603e4_Out_3_Float;
        Unity_Branch_float(_Property_321d0864f8e24c789d8ead6ed475e3c3_Out_0_Boolean, _Split_8e113e5414194688aa2c165814b6360b_A_4_Float, _Multiply_e8c48c2857c946f78c6f01261fe2553c_Out_2_Float, _Branch_5bd4b0e2a77b4a918e51059017f603e4_Out_3_Float);
        float4 _ScreenPosition_23cc09dad8e948a6b2c20b60e8ebb8e3_Out_0_Vector4 = float4(IN.NDCPosition.xy, 0, 0);
        float _LODDitheringTransitionSGCustomFunction_6837a7eb54884986933856bdbf84238d_multiplyAlpha_0_Float;
        LODDitheringTransitionSG_float(IN.WorldSpaceViewDirection, _ScreenPosition_23cc09dad8e948a6b2c20b60e8ebb8e3_Out_0_Vector4, _LODDitheringTransitionSGCustomFunction_6837a7eb54884986933856bdbf84238d_multiplyAlpha_0_Float);
        float _Multiply_07757bf1a334469cb891cd448c68d81a_Out_2_Float;
        Unity_Multiply_float_float(_Branch_5bd4b0e2a77b4a918e51059017f603e4_Out_3_Float, _LODDitheringTransitionSGCustomFunction_6837a7eb54884986933856bdbf84238d_multiplyAlpha_0_Float, _Multiply_07757bf1a334469cb891cd448c68d81a_Out_2_Float);
        float _Branch_88ac64f4ac3b4739937bd51278da2174_Out_3_Float;
        Unity_Branch_float(_Property_c660a337893a4106a47253f4fdaa173d_Out_0_Boolean, _Multiply_07757bf1a334469cb891cd448c68d81a_Out_2_Float, _Branch_5bd4b0e2a77b4a918e51059017f603e4_Out_3_Float, _Branch_88ac64f4ac3b4739937bd51278da2174_Out_3_Float);
        Modified_Color_1 = (_Branch_38536d5bf09446bc9e20abbd05f134e7_Out_3_Vector4.xyz);
        Modified_Alpha_4 = _Branch_88ac64f4ac3b4739937bd51278da2174_Out_3_Float;
        Original_Color_2 = (_SampleTexture2D_f0368572efa94c5ebedc97b7f89e54d4_RGBA_0_Vector4.xyz);
        Original_Alpha_3 = _Split_8e113e5414194688aa2c165814b6360b_A_4_Float;
        }
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float4 SpriteMask;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_1bc55e147ddf4a879ea5faee947990be_Out_0_Vector4 = _Color;
            Bindings_SpeedTree8ColorAlpha_1b4ecad27a9bc714e8d3af3ffb8a368c_float _SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71;
            _SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71.WorldSpaceViewDirection = IN.WorldSpaceViewDirection;
            _SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71.VertexColor = IN.VertexColor;
            _SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71.NDCPosition = IN.NDCPosition;
            _SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71.uv0 = IN.uv0;
            float3 _SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71_ModifiedColor_1_Vector3;
            float _SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71_ModifiedAlpha_4_Float;
            float3 _SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71_OriginalColor_2_Vector3;
            float _SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71_OriginalAlpha_3_Float;
            SG_SpeedTree8ColorAlpha_1b4ecad27a9bc714e8d3af3ffb8a368c_float(UnityBuildTexture2DStructNoScale(_SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71_BaseMap_2957833126_Texture2D), float4 (1, 1, 1, 0), 0, float4 (1, 0.5, 0, 0.5), 0, 0, 0, _SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71, _SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71_ModifiedColor_1_Vector3, _SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71_ModifiedAlpha_4_Float, _SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71_OriginalColor_2_Vector3, _SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71_OriginalAlpha_3_Float);
            float _Property_4f3ae18a52a241879ff94106f172daa7_Out_0_Float = _ScrollSpeed;
            float2 _Rotate_0d8c70a6b7044e599b2b0134b878e3f3_Out_3_Vector2;
            Unity_Rotate_Radians_float(IN.uv0.xy, (_Property_4f3ae18a52a241879ff94106f172daa7_Out_0_Float.xx), IN.TimeParameters.x, _Rotate_0d8c70a6b7044e599b2b0134b878e3f3_Out_3_Vector2);
            float2 _TilingAndOffset_27a36f028fe644cb98211b9852a05d84_Out_3_Vector2;
            Unity_TilingAndOffset_float(_Rotate_0d8c70a6b7044e599b2b0134b878e3f3_Out_3_Vector2, float2 (1, 1), float2 (0, 0), _TilingAndOffset_27a36f028fe644cb98211b9852a05d84_Out_3_Vector2);
            float4 _SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(UnityBuildTexture2DStructNoScale(_SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_Texture_1_Texture2D).tex, UnityBuildTexture2DStructNoScale(_SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_Texture_1_Texture2D).samplerstate, UnityBuildTexture2DStructNoScale(_SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_Texture_1_Texture2D).GetTransformedUV(_TilingAndOffset_27a36f028fe644cb98211b9852a05d84_Out_3_Vector2) );
            float _SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_R_4_Float = _SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_RGBA_0_Vector4.r;
            float _SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_G_5_Float = _SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_RGBA_0_Vector4.g;
            float _SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_B_6_Float = _SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_RGBA_0_Vector4.b;
            float _SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_A_7_Float = _SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_RGBA_0_Vector4.a;
            float4 _Blend_83abd13bd1ea46a5b80f0c27c1c104a8_Out_2_Vector4;
            Unity_Blend_Overlay_float4((_SpeedTree8ColorAlpha_fe392d89b95d4f21bdcff72a1113fb71_OriginalAlpha_3_Float.xxxx), _SampleTexture2D_f80e4b6400b04f49aae5057e67ced168_RGBA_0_Vector4, _Blend_83abd13bd1ea46a5b80f0c27c1c104a8_Out_2_Vector4, 1);
            surface.BaseColor = (_Property_1bc55e147ddf4a879ea5faee947990be_Out_0_Vector4.xyz);
            surface.Alpha = (_Blend_83abd13bd1ea46a5b80f0c27c1c104a8_Out_2_Vector4).x;
            surface.SpriteMask = _Property_1bc55e147ddf4a879ea5faee947990be_Out_0_Vector4;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpaceViewDirection = GetWorldSpaceNormalizeViewDir(input.positionWS);
        
            #if UNITY_UV_STARTS_AT_TOP
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #else
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #endif
        
            output.NDCPosition = output.PixelPosition.xy / _ScaledScreenParams.xy;
            output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteLitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}