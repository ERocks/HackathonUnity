using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManagerController : MonoBehaviour
{

    public static ClickManagerController instance = null;

    [HideInInspector] public static GameObject[] platforms = null;      // Cuidadin (que soy chiquitin) con el 'static'
    [HideInInspector] public static GameObject[] buttons = null;
    [HideInInspector] public static GameObject[] flyenemies = null;
    [HideInInspector] public static GameObject[] groundenemies = null;

    // Cuanto hay que arrastrar el raton
    public float minForce = 0.5f;
    public float medForce = 2;
    public float maxForce = 3.5f;

    // Cuanto aumenta segun lo que has arrastrado
    public float minForceMagnitude = 1;
    public float medForceMagnitude = 2;
    public float maxForceMagnitude = 3;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)              // SOLO PUEDE SOBREVIVIR UNOOOOOOOO
            Destroy(gameObject);                // MUAHAHAHAHA

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (platforms == null)              // No se puede hacer en el 'Awake' porque no hay garantias de que todas las
            platforms = GameObject.FindGameObjectsWithTag("Platform");      // plataformas se hayan inicializado aun

        if (buttons == null)
            buttons = GameObject.FindGameObjectsWithTag("Sturdy");

        if (flyenemies == null)
            flyenemies = GameObject.FindGameObjectsWithTag("FlyEnemy");

        if (groundenemies == null)
            groundenemies = GameObject.FindGameObjectsWithTag("GroundEnemy");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            StartCoroutine("constantGravity");
        else if (Input.GetMouseButtonUp(1))
            StopCoroutine("constantGravity");

        /* Boton izquierdo comprueba si ha pulsado un boton
        * o bien tiene que activar todas las plataformas movibles
        * */
        if (Input.GetMouseButtonDown(0))
        {
            bool activatedButton = false;
            int i;
            for (i = 0; (i < buttons.Length) && (activatedButton != true); ++i)
            {
                if (buttons[i].GetComponent<PlatformController>().isClicked == true)
                    activatedButton = true;
            }
            --i;    // Si no al salir del bucle aumenta uno extra que no queremos

            if (activatedButton)
            {
                StartCoroutine(buttonGravity(buttons[i]));
            }
            StartCoroutine(variableGravity());

        }

    }

    IEnumerator constantGravity()
    {
        Camera cam = Camera.main;
        Vector3 origin = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = new Vector3(0, 0, 0);
        Vector3 mousePos;

        // Establece en que direccion se movera
        while (direction.magnitude < minForce)
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            direction = mousePos - origin;
            direction.z = 0;

            // Proyecta la direccion sobre los ejes y coge el predominante para pasarlo
            if (Vector3.Project(direction, Vector3.right).magnitude > Vector3.Project(direction, Vector3.up).magnitude)
                direction = Vector3.Project(direction, Vector3.right);
            else
                direction = Vector3.Project(direction, Vector3.up);

            yield return null;
        }

        direction = direction.normalized;

        // Mientras el raton siga apretado avisa a los objetos de que continuen moviendose
        while (!Input.GetMouseButtonUp(1))
        {
            float time = Time.deltaTime;

            // Mueve murcielagos y otros enemigos que les afecta la gravedad constante
            foreach (GameObject flyenemy in flyenemies)
            {
                flyenemy.SendMessage("Move", direction * time);
            }
            yield return null;

            // Plataformas
            foreach (GameObject platform in platforms)
            {
                platform.SendMessage("Move", direction * time);
            }
            yield return null;
        }
    }

    /* 'variable/buttonGravity' no necesitan un 'StopCoroutine'
    * porque terminan de ejecutarse en el momento en que se suelta el boton
    */

    IEnumerator variableGravity()
    {
        Camera cam = Camera.main;
        Vector3 origin = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = new Vector3(0, 0, 0);
        Vector3 mousePos;
        float speed;

        // Establece en que direccion se movera
        while (direction.magnitude < minForce)
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            direction = mousePos - origin;
            direction.z = 0;

            // Proyecta la direccion sobre los ejes y coge el predominante para pasarlo
            if (Vector3.Project(direction, Vector3.right).magnitude > Vector3.Project(direction, Vector3.up).magnitude)
                direction = Vector3.Project(direction, Vector3.right);
            else
                direction = Vector3.Project(direction, Vector3.up);

            yield return null;
        }

        // Determina la magnitud
        while (!Input.GetMouseButtonUp(0))
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            direction = mousePos - origin;
            direction.z = 0;

            // Proyecta la direccion sobre los ejes y coge el predominante para pasarlo
            if (Vector3.Project(direction, Vector3.right).magnitude > Vector3.Project(direction, Vector3.up).magnitude)
                direction = Vector3.Project(direction, Vector3.right);
            else
                direction = Vector3.Project(direction, Vector3.up);

            yield return null;
        }

        // Segun cuanto arrastra el raton le asigna una fuerza
        if (direction.magnitude > maxForce)
        {
            speed = maxForceMagnitude;
            direction = direction.normalized;
            direction *= maxForceMagnitude;
        }
        else if (direction.magnitude > medForce)
        {
            speed = medForceMagnitude;
            direction = direction.normalized;
            direction *= medForceMagnitude;
        }
        else
        {
            speed = minForceMagnitude;
            direction = direction.normalized;
            direction *= minForceMagnitude;
        }
		
		// Enemigos que les afectan los tirones
            foreach (GameObject groundenemy in groundenemies)
            {
                groundenemy.SendMessage("Move", direction);
            }
            yield return null;

        float path = direction.magnitude;

        // MUEVETE SESAMO
        while (path > 0)
        {
            float time = Time.deltaTime;
            Vector3 _direction = direction * time * speed;

            /*// Enemigos que les afectan los tirones
            foreach (GameObject groundenemy in groundenemies)
            {
                groundenemy.SendMessage("Move", _direction);
            }
            yield return null;*/

            // Plataformas
            foreach (GameObject platform in platforms)
            {
                platform.SendMessage("Move", _direction);
            }
            yield return null;

            path -= _direction.magnitude;
        }
    }

    IEnumerator buttonGravity(GameObject target)
    {
        Camera cam = Camera.main;
        Vector3 origin = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = new Vector3(0, 0, 0);
        Vector3 mousePos;
        float speed;

        // Establece en que direccion se movera
        while (direction.magnitude < minForce)
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            direction = mousePos - origin;
            direction.z = 0;

            // Proyecta la direccion sobre los ejes y coge el predominante para pasarlo
            if (Vector3.Project(direction, Vector3.right).magnitude > Vector3.Project(direction, Vector3.up).magnitude)
                direction = Vector3.Project(direction, Vector3.right);
            else
                direction = Vector3.Project(direction, Vector3.up);

            yield return null;
        }

        // Determina la magnitud
        while (!Input.GetMouseButtonUp(0))
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            direction = mousePos - origin;
            direction.z = 0;

            // Proyecta la direccion sobre los ejes y coge el predominante para pasarlo
            if (Vector3.Project(direction, Vector3.right).magnitude > Vector3.Project(direction, Vector3.up).magnitude)
                direction = Vector3.Project(direction, Vector3.right);
            else
                direction = Vector3.Project(direction, Vector3.up);

            yield return null;
        }

        // Segun cuanto arrastra el raton le asigna una fuerza
        if (direction.magnitude > maxForce)
        {
            speed = maxForceMagnitude;
            direction = direction.normalized;
            direction *= maxForceMagnitude;
        }
        else if (direction.magnitude > medForce)
        {
            speed = medForceMagnitude;
            direction = direction.normalized;
            direction *= medForceMagnitude;
        }
        else
        {
            speed = minForceMagnitude;
            direction = direction.normalized;
            direction *= minForceMagnitude;
        }

        float path = direction.magnitude;

        // MUEVETE SESAMO
        while (path > 0)
        {
            float time = Time.deltaTime;
            Vector3 _direction = direction * time * speed;
            target.SendMessage("Move", _direction);
            path -= _direction.magnitude;
            yield return null;
        }
    }
}
