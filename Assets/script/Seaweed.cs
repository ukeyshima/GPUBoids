using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Seaweed: MonoBehaviour
{
    [System.Serializable]
    struct SeaweedData
    {
        public Vector3 Position;
        public Vector3 Size;           
    }

    #region Parameters
    [Range(10, 1024)]
    public int MaxObjectNum = 1024;    
    #endregion    

    #region Private Resources
    ComputeBuffer _seaweedDataBuffer;
    #endregion

    #region Accessors
    public ComputeBuffer GetSeaweedDataBuffer()
    {
        return this._seaweedDataBuffer != null ? this._seaweedDataBuffer : null;
    }
    public int GetMaxObjectNum()
    {
        return this.MaxObjectNum;
    }        
    #endregion

    #region MonoBehaviour Functions        
    void Start()
    {
        InitBuffer();        
    }    
    void OnDestroy()
    {
        ReleaseBuffer();
    }    
    #endregion

    #region Private Functions
    void InitBuffer()
    {
        _seaweedDataBuffer = new ComputeBuffer(MaxObjectNum, Marshal.SizeOf(typeof(SeaweedData)));        

        var seaweedDataArr = new SeaweedData[MaxObjectNum];
        for (var i = 0; i < MaxObjectNum; i++)
        {            
            seaweedDataArr[i].Position = new Vector3(Random.Range(-1000.0f, 1000.0f),-250,Random.Range(-500.0f, 500.0f));            
            seaweedDataArr[i].Size = new Vector3(Random.Range(5.0f, 15.0f),Random.Range(2.0f, 15.0f),Random.Range(5.0f, 10.0f));
        }        
        _seaweedDataBuffer.SetData(seaweedDataArr);        
        seaweedDataArr = null;
    }    
    void ReleaseBuffer()
    {
        if (_seaweedDataBuffer != null)
        {
            _seaweedDataBuffer.Release();
            _seaweedDataBuffer = null;
        }        
    }
    #endregion
}
