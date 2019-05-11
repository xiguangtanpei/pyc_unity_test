//The scripts below show how to use a propertyField to change your editor.
//Attach this first script to the GameObject that you would like to control. Add code in this script for any of the actions you require.

using UnityEngine;

public class MyGameObjectScript : MonoBehaviour
{
    public int m_MyInt = 75;
    public Vector3 m_MyVector = new Vector3(20, 1, 0);
    public GameObject m_MyGameObject;
}