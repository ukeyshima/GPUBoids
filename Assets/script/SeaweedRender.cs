using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Seaweed))]
public class SeaweedRender : MonoBehaviour
{    
    #region Script References
    public Seaweed SeaweedScript;
    #endregion

    #region Built-in Resources
    public Mesh InstanceMesh;
    public Material InstanceRenderMaterial;
    #endregion
    #region Private Variables
    uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
    ComputeBuffer argsBuffer;
    #endregion
    void Start()
    { 
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
    }

       void OnDisable()
    {
        if (argsBuffer != null) argsBuffer.Release();
        argsBuffer = null;
    }
    void Update()
    {
        if (InstanceRenderMaterial == null || !SystemInfo.supportsInstancing) return;
        uint numIndices = (InstanceMesh != null) ? (uint)InstanceMesh.GetIndexCount(0) : 0;
        args[0] = numIndices;
        args[1] = (uint)SeaweedScript.GetMaxObjectNum();

        argsBuffer.SetData(args);

        InstanceRenderMaterial.SetBuffer("_SeaweedDataBuffer", SeaweedScript.GetSeaweedDataBuffer());        

        Graphics.DrawMeshInstancedIndirect(InstanceMesh, 0, InstanceRenderMaterial, new Bounds(Vector3.zero, new Vector3(2000.0f, 500.0f, 1000.0f)), argsBuffer);
    }
}
