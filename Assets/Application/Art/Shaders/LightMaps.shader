Shader "BikeTeck/Simple LightMap" { 
Properties {
      _MainTex ("Base (RGB) Self-Illumination (A)", 2D) = "white"
      _PathTex ("Path Texture (RGB) Self-Illumination (A)", 2D) = "White"
   }
   SubShader {
      Pass {
     
      BindChannels
        {         
            Bind "texcoord1", texcoord0
            Bind "texcoord", texcoord1
            }
              
              SetTexture[_PathTex]
			SetTexture[_MainTex]
{
    		ConstantColor ([_Blend],[_Blend],[_Blend])
    		combine previous * texture
            }
              
              
              
      } 
   } 
}
