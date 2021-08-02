using Application.Scripts.Model;
using Application.Scripts.Views.Managers;
using UnityEngine;

namespace Application.Scripts.Views.Gameplay
{
    public class Coin : MonoBehaviour
    {

        public float movSpeed = 10f;
        public float rotSpeed = 500f;
        public GameObject pickUpEffect;

        public Transform targetToChase;

        private Vector3 startPos;

        void Start()
        {
            startPos = transform.localPosition;
        
        }

        void OnEnable()
        {
            if (startPos == Vector3.zero)
                return;

            targetToChase = null;
            transform.localPosition = startPos;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime, Space.World);

            if (targetToChase != null)
            {
                transform.position = Vector3.Lerp(transform.position, targetToChase.position, Time.deltaTime * movSpeed);//Vector3.MoveTowards(transform.position, targetToChase.position, Time.deltaTime * movSpeed);
                if (Vector3.Distance(transform.position, targetToChase.position) <= 2.8f)
                {
                    OnPickUp();
                } 
            }

        }

        public void OnPickUp()
        {
            ManagerGame.instancia.SumarMoneda();
            gameObject.SetActive(false);
            targetToChase = null;
            AudioManager.instance.PlaySound(Constants.Audio.COIN, false, true, true);
            GameObject effect = Instantiate(pickUpEffect, transform.position, Quaternion.identity, transform.parent);

            Destroy(effect, 3f);
        }

    }
}
