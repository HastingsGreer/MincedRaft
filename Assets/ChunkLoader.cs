using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
	public Transform r;
    // Start is called before the first frame update
    void Awake()
    { 
    	StartCoroutine(Loopslow());
        
    }
    private IEnumerator Loopslow(){
    	 WaitForSeconds wait = new WaitForSeconds(.3f);
    	 for(int z = -16; z < 16; z++){
    		for(int x = -16; x < 16; x++){
    			Transform chunk = Instantiate(r);
    			chunk.position = Vector3.right * 64 * z + Vector3.forward * 32 * x;
    			yield return wait;
    		}
    	}
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
