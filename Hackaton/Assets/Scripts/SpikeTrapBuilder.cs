using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class SpikeTrapBuilder : MonoBehaviour
{
    [Range(0, 50)] public int m_SpikeNumber = 0;

    public BoxCollider2D m_SpikeCollider;
    public GameObject m_SpikeModel;
    [Range(0.1f, 1)] public float m_SpikeYSizeCorrection = 1;
    [Range(0.1f, 1)] public float m_SpikexSizeCorrection = 1;

    public GameObject[] m_Spikes;

    void Start() {
        m_SpikeCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {



        m_SpikeCollider.size = new Vector2(m_SpikeNumber * m_SpikeModel.GetComponent<SpriteRenderer>().size.x * m_SpikexSizeCorrection, m_SpikeModel.GetComponent<SpriteRenderer>().size.y * m_SpikeYSizeCorrection);
        //m_SpikeCollider.offset = new Vector2((m_SpikeNumber * m_SpikeModel.GetComponent<SpriteRenderer>().size.x * m_SpikexSizeCorrection/ 2) - (m_SpikeModel.GetComponent<SpriteRenderer>().size.x * m_SpikexSizeCorrection/2), 0);
        m_SpikeCollider.offset = new Vector2(((float)m_SpikeNumber / 2F - 0.5F) * (m_SpikeModel.GetComponent<SpriteRenderer>().size.x * m_SpikexSizeCorrection), 0);

        if (m_Spikes.Length != m_SpikeNumber)
        {
            for (int i = 0; i < m_Spikes.Length; i++)
            {
                if (m_Spikes[i] != null)
                    DestroyImmediate(m_Spikes[i]);
            }

            if (m_SpikeNumber != 0)
            {

                m_Spikes = new GameObject[m_SpikeNumber];

                for (int i = 0; i < m_SpikeNumber; i++)
                {
                    m_Spikes[i] = GameObject.Instantiate(m_SpikeModel, transform.TransformPoint(m_SpikeModel.GetComponent<SpriteRenderer>().size.x * m_SpikexSizeCorrection * i, 0, 0), transform.rotation, transform);
                }
            }
            else
            {
                m_Spikes = new GameObject[0];
            }

        }


    }

    public void ArrayResize(int size, ref GameObject[] group)
    {
        int oldLength = group.Length;

        GameObject[] temp = new GameObject[size];
        for (int c = 1; c < size; c++)
        {
            temp[c] = group[c];
        }

        for (int c = size; c < oldLength; c++)
        {
            Destroy(group[c]);
        }
        group = temp;
    }
}
