using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(PlatformController))]
public class PlatformControllerEditor : Editor {

    /* Las variables de este script no son las mismas de aquel
     * al que hacen referencia aunque se llamen igual. Cuando
     * se efectúa algún cambio hay que guardarlo 'de vuelta'.
     */

    PlatformController m_PlatformController;
    
    private void OnEnable()
    {
        m_PlatformController = target as PlatformController;
    }

    /* if (!Application.isPlaying)
     * El OnInspectorGUI y OnSceneGUI usan esta condicion porque hay ciertas
     * cosas del PlatformController que se inicializan en el Start, por lo que
     * si se pretende cambiar algo en el inspector o desde la escena mientras
     * esta en modo juego no va a funcionar bien.
     */

    public override void OnInspectorGUI()
    {
        #region     VELOCIDAD DE LA PLATAFORMA
        EditorGUI.BeginChangeCheck();

        float speed = m_PlatformController.m_speed;
        speed = EditorGUILayout.FloatField("Velocidad", speed);

        if (speed < 0)
            speed = 0;

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "changed speed");
            m_PlatformController.m_speed = speed;
        }
        #endregion

        if (!Application.isPlaying)
        {
            #region     EN QUE NODO EMPIEZA
            EditorGUI.BeginChangeCheck();

            int begin = m_PlatformController.m_button;
            begin = EditorGUILayout.IntField("Empieza en", begin);

            if (begin > (m_PlatformController.localNodes.Length - 1))
                begin = m_PlatformController.localNodes.Length - 1;
            else if (begin < 0)
                begin = 0;

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "changed where it begins");
                m_PlatformController.m_button = begin;
                for (int i = 0; i < m_PlatformController.localNodes.Length; ++i)
                {
                    if (i != begin)
                    {
                        m_PlatformController.localNodes[i] -= m_PlatformController.localNodes[begin];
                    }
                }
                m_PlatformController.transform.position = m_PlatformController.transform.TransformPoint(m_PlatformController.localNodes[begin]);
                m_PlatformController.localNodes[begin] = Vector3.zero;
            }
            #endregion

            #region     NODOS
            if (GUILayout.Button("Add Node"))           // Crea el boton 'Add Node'
            {
                Undo.RecordObject(target, "added node");        // Guarda los cambios que se realicen a continuacion
                Vector3 position = m_PlatformController.localNodes[m_PlatformController.localNodes.Length - 1] + Vector3.right;     // posicion del ultimo nodo
                ArrayUtility.Add(ref m_PlatformController.localNodes, position);        // Anyade un nuevo nodo al array en la posicion 'position'
            }

            EditorGUIUtility.labelWidth = 64;
            int delete = -1;                                          // Muestra y modifica la posicion de los nodos
            for (int i = 0; i < m_PlatformController.localNodes.Length; ++i)
            {
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.BeginHorizontal();          // HORIZONTAL

                int size = 64;
                EditorGUILayout.BeginVertical(GUILayout.Width(size));               // Crea menu desplegable?
                EditorGUILayout.LabelField("Node " + i, GUILayout.Width(size));         // Enumeracion de los nodos
                if ((i != 0) && (GUILayout.Button("Delete", GUILayout.Width(size))))        // Boton para eliminar un nodo (excepto el primero)
                {                                                                              // !BOTON! No implementa aqui la eliminacion
                    delete = i;
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();            // Muestra la posicion (x,y) de cada nuevo nodo
                Vector2 newPosition;                        // (excepto el primero que esta fijo)
                newPosition = EditorGUILayout.Vector2Field("Position",
                    m_PlatformController.transform.TransformPoint(m_PlatformController.localNodes[i]));
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();        // HORIZONTAL: todo esto se escribe en una sola linea

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "changed position");
                    m_PlatformController.localNodes[i] = m_PlatformController.transform.InverseTransformPoint(newPosition);       // Guarda la nueva posicion asignada
                }
            }
            EditorGUIUtility.labelWidth = 0;

            if (delete != -1)
            {
                Undo.RecordObject(target, "Removed node");              // Elimina un nodo
                ArrayUtility.RemoveAt(ref m_PlatformController.localNodes, delete);
            }
            #endregion
        }
    }

    private void OnSceneGUI()
    {
        if (!Application.isPlaying)
        {
            for (int i = 0; i < m_PlatformController.localNodes.Length; ++i)
            {
                Vector3 worldPos;

                worldPos = m_PlatformController.localNodes[i];

                // Dibuja los handles
                Vector3 newWorld = worldPos;
                newWorld = Handles.PositionHandle(m_PlatformController.transform.TransformPoint(worldPos), Quaternion.identity);

                Handles.color = Color.red;

                // Dibuja las lineas que conectan los nodos
                if (i == 0)
                {
                    if (worldPos != newWorld)
                    {
                        Undo.RecordObject(target, "moved node");
                        m_PlatformController.localNodes[i] = m_PlatformController.transform.InverseTransformPoint(newWorld);
                    }
                }
                else
                {
                    // Las lineas de union las dibuja del actual al anterior
                    Handles.DrawDottedLine(m_PlatformController.transform.TransformPoint(m_PlatformController.localNodes[i]),
                        m_PlatformController.transform.TransformPoint(m_PlatformController.localNodes[i - 1]), 10);

                    if (worldPos != newWorld)
                    {
                        Undo.RecordObject(target, "moved node");
                        m_PlatformController.localNodes[i] = m_PlatformController.transform.InverseTransformPoint(newWorld);
                    }
                }
            }
        }
    }
}
