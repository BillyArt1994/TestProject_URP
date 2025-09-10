using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace AmazingAssets.VertexAmbientOcclusionGenerator.Example
{
    public class Runtime : MonoBehaviour
    {
        public VertexAmbientOcclusionGenerator.Enum.Solver solver;

        public float rayLength = 1;
        [Range(0.0f, 1.0f)] public float details = 0.7f;
        public LayerMask layerMask = 0;

        
        public Material material;


        //Here we setup URP camera used in VAOG calculations. 
        void SetupUniversalCameraData(Camera camera)
        {
            //
            UniversalAdditionalCameraData cameraData = camera.gameObject.AddComponent<UniversalAdditionalCameraData>();

            //Use index of the 'Vertex Ambient Occlusion Renderer' from the Renderer List of the SRP asset (For more details check the Manual file).
            cameraData.SetRenderer(1);
        }


        void Start()
        {
            //We calculate AO from 'this.gameobject' to take object's transformaion into acount and make in 'visible' to the other scene mesh objects.
            float[] aoValues = this.gameObject.GenerateVertexAmbientOcclusion(solver, rayLength, details, layerMask, SetupUniversalCameraData);


            if (aoValues != null)
            {
                //Generate vertex color with AO
                Color[] vertexColor = new Color[aoValues.Length];
                for (int i = 0; i < aoValues.Length; i++)
                {
                    vertexColor[i] = Color.Lerp(Color.black, Color.white * aoValues[i], aoValues[i]);
                }


                //Instantiate copy of a 'mesh' and assign new vertex colors
                this.gameObject.GetComponent<MeshFilter>().mesh.colors = vertexColor;

                //Assign material with vertex color support
                this.gameObject.GetComponent<Renderer>().material = material;
            }
        }        
    }
}