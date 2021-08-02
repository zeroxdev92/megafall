using UnityEngine;

namespace Application.Scripts.Views.Utils
{
    public class CameraFollow : MonoBehaviour
    {
        public bool canDo = false; //Test

        public Transform player;
        public float maxZoom = 17f;
        public float minZoom = 9f;
        public float offsetY = 3.5f;
        public float transitionSpeed = 1.5f;
    
        private Camera thisCamera;
        private Transform followTarget;
        private Vector3 initialCamPos;
        private bool transition = false;

        void Awake()
        {
            thisCamera = GetComponent<Camera>();
        }

        // Use this for initialization
        void Start()
        {

            if (player == null)
            {
                Debug.LogError("Player not assigned in CameraFollow");
                return;
            }

            followTarget = player.GetChild(0).Find("Hips_Bone");
            initialCamPos = transform.position;
        
        }

        float elapsed = 0f;

        // Update is called once per frame
        void Update()
        {
        
            if (player == null)
                return;


            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                canDo = true;
            }
        
            if (canDo)
            {
                elapsed += Time.deltaTime;

                //Vector3 desiredPosition = initialCamPos + new Vector3(0f, 3.5f, 0f);
                //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, elapsed);
                //transform.position = smoothedPosition;
                thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, 17f, elapsed);
            
                if (elapsed >= 1.5f)
                {
                    canDo = false;
                    elapsed = 0f;
                }
            }
        
        }
    }
}
