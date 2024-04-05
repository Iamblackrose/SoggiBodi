using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootFloorCollisionCheck : MonoBehaviour
{
    [SerializeField] PlayerControls player;

    public void OnCollisionEnter(Collision c){
        if(c.gameObject.tag == "Floor"){
            player.SetGroundedState(true);
        }
    }
    public void OnCollisionExit(Collision c){
        if(c.gameObject.tag == "Floor"){
            player.SetGroundedState(false);
        }
    }
}
