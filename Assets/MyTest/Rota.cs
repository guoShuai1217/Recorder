/*
 * 		Description: 
 *
 *  	CreatedBy:  国帅
 *
 *  	DataTime: 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rota : MonoBehaviour 
{

    private void Update()
    {
        transform.Rotate(Vector3.right * 60 * Time.deltaTime);
    }



}
