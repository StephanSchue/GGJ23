using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ElectricTrapController))]
public class ElectricSplineEditor : Editor
{
    ElectricTrapController _target;

    private SerializedProperty _propColorBottom;
    private SerializedProperty _propColorMiddle;
    private SerializedProperty _propColorTop;

    private SerializedProperty _propThickness;


    private void OnEnable()
    {
        _target = (ElectricTrapController)target;

        _propColorBottom = serializedObject.FindProperty("ColorBottom");
        _propColorMiddle = serializedObject.FindProperty("ColorMiddle");
        _propColorTop = serializedObject.FindProperty("ColorTop");

        _propThickness = serializedObject.FindProperty("Thickness");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_propColorTop);
        EditorGUILayout.PropertyField(_propColorMiddle);
        EditorGUILayout.PropertyField(_propColorBottom);

        EditorGUILayout.PropertyField(_propThickness);

        if (serializedObject.ApplyModifiedProperties())
        {
            _target.GenerateMesh();
        }
    }

    private void OnSceneGUI()
    {
        Handles.color = Color.green;

        for (int i = 1; i < _target.Points.Length; i++)
        {
            Vector2 p = _target.transform.TransformPoint( _target.Points[i]);
            Vector2 newPosition = Handles.FreeMoveHandle(p, 0.08f, Vector3.zero, Handles.CircleHandleCap);

            if (p != newPosition)
            {
                _target.SetPointPosition(i, _target.transform.InverseTransformPoint(newPosition));
            }
        }

        Handles.color = Color.grey;
        Vector2 p0 = _target.transform.TransformPoint(_target.Points[0]);
        Vector2 p1 = _target.transform.TransformPoint(_target.Points[1]);
        Vector2 p2 = _target.transform.TransformPoint(_target.Points[2]);
        Vector2 p3 = _target.transform.TransformPoint(_target.Points[3]);

        Handles.DrawDottedLine(p0, p1, 5f);
        Handles.DrawAAPolyLine(p2, p3);
        Handles.DrawBezier(p0, p3, p1, p2, Color.grey, null, 3f);        
    }


    [DrawGizmo(GizmoType.Active | GizmoType.Selected)]
    public static void OnDrawGUI(ElectricTrapController target, GizmoType gizmoType)
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < target.Points.Length; i++)
        {
            Vector2 p = target.transform.TransformPoint(target.Points[i]);
            Gizmos.DrawSphere(p, 0.05f);
        }
    }
}
