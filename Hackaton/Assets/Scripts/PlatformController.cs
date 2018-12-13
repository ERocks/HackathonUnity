using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(Rigidbody2D))]
public class PlatformController : MonoBehaviour, IMovable {
    
    public Vector3[] localNodes = new Vector3[1];
    private Vector3[] saveNodes;

    private Vector3[] m_WorldNode;
    public Vector3[] worldNode { get { return m_WorldNode; } }
    
    private Rigidbody2D m_Rigidbody2D;

    public bool isClicked = false;      // El ClickManager necesita acceder a este parametro
    public int m_button;                // Para que el PlatformControllerEditor guarde en que nodo se situa la plataforma

    private float i;                    // Numeros enteros o semienteros (esta en un nodo o entre dos)
    private float m_t;                  // [0,1], parametro de interpolacion
    public float m_speed = 1;           // Controla como de rapido varia 'm_t'

    private void Reset()
    {
        localNodes[0] = Vector3.zero;

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Rigidbody2D.isKinematic = true;
    }

    private void OnAwake()
    {
        localNodes[0] = Vector3.zero;
    }

    private void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Rigidbody2D.isKinematic = true;
        
        m_WorldNode = new Vector3[localNodes.Length];
        for (int i = 0; i < m_WorldNode.Length; ++i)
            m_WorldNode[i] = transform.TransformPoint(localNodes[i]);

        i = m_button;
        m_t = 0;

        saveNodes = new Vector3[localNodes.Length];

        for (int i = 0; i < localNodes.Length; ++i)
        {
            saveNodes[i] = transform.TransformPoint(localNodes[i]);
        }
    }
    
    public void Move(Vector3 direction)
    {
        if (Time.timeScale != 0)
        {
            if (localNodes.Length > 1)
            {
                Vector3 next, previous;
                
                // Comprueba en que punto de la trayectoria esta para asi
                // ver si puede avanzar en un determinado sentido o no
                if (i == 0)     // Esta en el primer nodo
                {
                    next = localNodes[(int)i + 1] - localNodes[(int)i];
                    previous = Vector3.zero;
                }
                else if (i == (localNodes.Length - 1))       // Esta en el ultimo
                {
                    next = Vector3.zero;
                    previous = localNodes[(int)i - 1] - localNodes[(int)i];
                }
                else if ((i - (int)i) != 0)       // esta entre dos
                {
                    next = localNodes[(int)i + 1];
                    previous = localNodes[(int)i];
                }
                else
                {
                    next = localNodes[(int)i + 1] - localNodes[(int)i];
                    previous = localNodes[(int)i - 1] - localNodes[(int)i];
                }

                // La direccion que marca el raton esta "permitida".
                // Es decir, conduce hacia uno de los nodos
                if (direction.normalized == next.normalized)
                {
                    m_t += m_speed * direction.magnitude;
                    if (m_t > 1)
                    {
                        m_t = 0;
                        i = (int)i + 1;
                        transform.position = transform.TransformPoint(localNodes[(int)i]);
                    }
                    else
                    {
                        i = ((int)i + ((int)i + 1)) / 2.0f;
                        transform.position = Vector3.Lerp(transform.TransformPoint(localNodes[(int)i]),
                            transform.TransformPoint(localNodes[(int)i + 1]), m_t);
                    }

                    for (int i = 0; i < localNodes.Length; ++i)
                    {
                        localNodes[i] = transform.InverseTransformPoint(saveNodes[i]);
                    }
                }
                else if (direction.normalized == previous.normalized)
                {
                    if (m_t == 0)
                    {
                        m_t = 1 - m_speed * direction.magnitude;
                        i = ((int)i + ((int)i - 1)) / 2.0f;
                        transform.position = Vector3.Lerp(transform.TransformPoint(localNodes[(int)i]),
                            transform.TransformPoint(localNodes[(int)i + 1]), m_t);
                    }
                    /*else if (m_t < 0)
                    {
                        m_t = 0;
                        i = (int)i;
                        transform.position = transform.TransformPoint(localNodes[(int)i]);
                    }*/
                    else
                    {
                        m_t -= m_speed * direction.magnitude;
                        i = ((int)i + ((int)i + 1)) / 2.0f;
                        transform.position = Vector3.Lerp(transform.TransformPoint(localNodes[(int)i]),
                            transform.TransformPoint(localNodes[(int)i + 1]), m_t);
                    }
                    
                    // Actualiza la 'i' para que reconozca que se encuentra en un nodo
                    // y de paso pone 'm_t' a cero
                    if (m_t < 0)
                    {
                        m_t = 0;
                        i = (int)i;
                    }

                        for (int i = 0; i < localNodes.Length; ++i)
                    {
                        localNodes[i] = transform.InverseTransformPoint(saveNodes[i]);
                    }
                }
            }
        }
    }

    // Cuidado porque 'OnMouseDown' funciona solo para el boton izquierdo
    // Aqui hemos tenido suerte
    private void OnMouseDown()
    {
        isClicked = true;
    }

    private void OnMouseUp()
    {
        isClicked = false;
    }
}
