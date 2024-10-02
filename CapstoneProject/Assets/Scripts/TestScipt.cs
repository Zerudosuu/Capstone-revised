using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TestScript : MonoBehaviour
{
    // Reference to the ScriptableObject, type it as LessonsData (your custom ScriptableObject class)
    public GameObject item; 
   private VisualElement ChemicalAreaContainer; 

   void Start() { 
     ChemicalAreaContainer = GameObject.FindObjectOfType<UIDocument>().rootVisualElement.Q<VisualElement>("ChemicalAreaContainer");
   }

}
