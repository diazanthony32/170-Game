using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planeScript : MonoBehaviour
{
    //When the mouse hovers over the GameObject, it turns to this color (red)
    Color m_MouseOverColor = Color.yellow;

    //This stores the GameObject’s original color
    Color planeOriginalColor;

    //Get the GameObject’s mesh renderer to access the GameObject’s material and color
    MeshRenderer m_Renderer;

    List<Transform> oldPlanes;

    void Start()
    {
    	oldPlanes = new List<Transform>();
        // //Fetch the mesh renderer component from the GameObject
        // m_Renderer = GetComponent<MeshRenderer>();
        // //Fetch the original color of the GameObject
        // m_OriginalColor = m_Renderer.material.color;
    }

    void Update(){

    	if(transform.parent.gameObject.tag == "PlayerCubePosition")
        {

    		RaycastHit[] hits;
			hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 5.0f);

			if(hits.Length != 0){

			    for (int i = 0; i < hits.Length; i++){
				    RaycastHit hitPlane = hits[i];
				
				    if(hitPlane.transform.gameObject.tag == "unitSquare")
				    {
					    if(oldPlanes.Count < 1){
						    // Change the color of the GameObject to red when the mouse is over GameObject
	        			    planeOriginalColor = hitPlane.transform.GetComponent<MeshRenderer>().material.color;
	        			    hitPlane.transform.GetComponent<MeshRenderer>().material.color = m_MouseOverColor;
	        			    // m_OriginalColor = hitPlane.transform.GetComponent<MeshRenderer>().material.color;
	        			
	        			    oldPlanes.Add(hitPlane.transform);
					    }

        			    break;
				    }
				    else{
					    // Reset the color of the GameObject back to normal
        			    //hitPlane.GetComponent<MeshRenderer>() = m_OriginalColor;
        			    for(int j = 0; j < oldPlanes.Count ; j++){
        				    Transform plane = oldPlanes[j];
        				    plane.GetComponent<MeshRenderer>().material.color = planeOriginalColor;

        				    oldPlanes.RemoveAt(j);
        			    }
        			    //hitPlane.transform.GetComponent<MeshRenderer>().material.color = Color.blue;
				    }

			    }

		    }
		    else{
			    // Reset the color of the GameObject back to normal
			    //hitPlane.GetComponent<MeshRenderer>() = m_OriginalColor;
			    for(int j = 0; j < oldPlanes.Count ; j++){
				    Transform plane = oldPlanes[j];
				    plane.GetComponent<MeshRenderer>().material.color = planeOriginalColor;

				    oldPlanes.RemoveAt(j);
			    }
			    //hitPlane.transform.GetComponent<MeshRenderer>().material.color = Color.blue;
		    }

    	}

    }

    // void OnMouseOver()
    // {
    //     // Change the color of the GameObject to red when the mouse is over GameObject
    //     m_Renderer.material.color = m_MouseOverColor;
    // }

    // void OnMouseExit()
    // {
    //     // Reset the color of the GameObject back to normal
    //     m_Renderer.material.color = m_OriginalColor;
    // }

}
