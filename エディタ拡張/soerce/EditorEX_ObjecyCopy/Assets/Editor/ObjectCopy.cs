using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectCopy : EditorWindow {
    public Vector3 ObjectSpan;
    public Vector3Int ObjectiNum;
    public GameObject SelectionObject;
    public GameObject Dummy;

    [MenuItem("Ex/ObjectCopy")]

	// Use this for initialization
	static void Open() {
        EditorWindow.GetWindow<ObjectCopy>();
	}

    void OnEnable()
    {
        if (Selection.gameObjects.Length > 0) Dummy = Selection.gameObjects[0];
    }

    private void OnSelectionChange()
    {
        SelectionObject = Selection.gameObjects[0];
        Repaint();
    }

    private void OnGUI()
    {
        SelectionObject = EditorGUILayout.ObjectField("選択オブジェクト",SelectionObject,typeof(GameObject),true) as GameObject;
        ObjectiNum = EditorGUILayout.Vector3IntField("個数の入力",ObjectiNum);
        ObjectSpan = EditorGUILayout.Vector3Field("間隔の入力", ObjectSpan);

        if (GUILayout.Button("Duplicate Start") && SelectionObject != null)
        {
            DuplicateObject(SelectionObject);
        }
    }

    void DuplicateObject(GameObject GO)
    {
        Vector3 GTpos = GO.transform.position;
        bool fflag = true;

        for(int x=0 ; x < ObjectiNum.x; x++)
        {
            Vector3 Lpy = GTpos;
            for (int y = 0 ; y < ObjectiNum.y; y++)
            {
                Vector3 Lpz = GTpos;
                int ofset = 0;
                for (int z=0 ; z < ObjectiNum.z - ofset; z++)
                {
                    if (fflag)
                    {
                        GTpos.z += ObjectSpan.z;
                        ofset=1;
                        fflag = false;
                    }
                    GameObject obj = Instantiate(GO, GTpos,Quaternion.identity);
                    if(SelectionObject) obj.transform.parent = SelectionObject.transform;

                    Undo.RegisterCreatedObjectUndo(obj, "DupulicateObject");
                    GTpos.z += ObjectSpan.z;
                }
                GTpos = Lpz;
                GTpos.y += ObjectSpan.y;
            }
            GTpos = Lpy;
            GTpos.x += ObjectSpan.x;
        }
    }
}
