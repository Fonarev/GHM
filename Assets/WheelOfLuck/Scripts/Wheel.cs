using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.WheelOfLuck.Scripts
{
    public class Wheel : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer wheel;
        [SerializeField] private bool isRotate;
        [SerializeField] private int AmountCell;
        [SerializeField] private float speedRotate;
        [SerializeField] private float maxSpeedRotateTime;
        [SerializeField] private float timeRotation;
        [SerializeField] private float accelerationTime;
        [SerializeField] private List<int> cells;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isRotate)
            {
              Rotate();
            }
        }
        private IEnumerator Rotate()
        {
            isRotate = true;
            SetSector();
            float elapsedTime = 0;
            float rotSpeed;
            while (elapsedTime < accelerationTime)
            {
                rotSpeed = Mathf.Lerp(0, speedRotate, elapsedTime / accelerationTime);
                wheel.transform.rotation *= Quaternion.Euler(0, 0, rotSpeed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            rotSpeed = speedRotate;
            elapsedTime = 0;
            while (elapsedTime < maxSpeedRotateTime)
            {
                wheel.transform.rotation *= Quaternion.Euler(0, 0, rotSpeed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0;
            while (elapsedTime < timeRotation) 
            {
            rotSpeed = Mathf.Lerp(speedRotate,0,elapsedTime/timeRotation);
            wheel.transform.rotation*=Quaternion.Euler(0,0,rotSpeed*Time.deltaTime);
            elapsedTime += Time.deltaTime;
                yield return null;
            }
            isRotate = false;
        }
        private int SetSector()
        {
            int randomSector = Random.Range(0, cells.Count);
        }
    }
}