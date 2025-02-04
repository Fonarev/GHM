using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.WheelOfLuck.Scripts
{
    public class Wheel : MonoBehaviour
    {
        [SerializeField] private Image wheel;
        [SerializeField] private Button clickWhedel;
        [SerializeField] private WheelConfig config;
        [SerializeField] private Button close;
        private bool isRotate;
        private WheelOfLuckService service;
        private float slowdownTime;
        private float randomAngle;
        private int randomSector;

        private void OnEnable()
        {
            //clickWhedel.onClick?.AddListener(Open);
        }
        private void OnDisable()
        {
            service.OnClaimReward -= Service_OnClaimReward;
        }
        public void Init(WheelOfLuckService service, WheelConfig config)
        {
            this.config = config;
            this.service = service;
            clickWhedel.onClick?.AddListener(OnClick);
            close.onClick?.AddListener(() => { gameObject.SetActive(false); });
            service.OnClaimReward += Service_OnClaimReward;
        }

        private void Service_OnClaimReward(bool claim)
        {
            clickWhedel.interactable = claim;
        }

        private void OnClick()
        {
            if (!isRotate)
                StartCoroutine(Rotate());
        }

        private IEnumerator Rotate()
        {
            isRotate = true;
            SetPointSector();
            service.SetNewDateTime();

            yield return StartCoroutine(AccelerationRotate());

            yield return StartCoroutine(TimeMaxSpeedRotate());

            yield return StartCoroutine(SlowdownTimeRotate());

            yield return new WaitForSeconds(1);

            service.OpenPopupWin(randomSector);

            isRotate = false;

        }

        private IEnumerator AccelerationRotate()
        {
            float elapsedTime = 0;
            float rotSpeed = 0;

            while (elapsedTime < config.accelerationTime)
            {
                rotSpeed = Mathf.Lerp(0, config.rotationSpeed, elapsedTime / config.accelerationTime);
                wheel.transform.rotation *= Quaternion.Euler(0, 0, rotSpeed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator TimeMaxSpeedRotate()
        {
            float elapsedTime = 0;

            while (elapsedTime < config.maxSpeedRotateTime)
            {
                wheel.transform.rotation *= Quaternion.Euler(0, 0, config.rotationSpeed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator SlowdownTimeRotate()
        {
            float elapsedTime = 0;
            float rotSpeed;

            float distance = (config.numberSpins * 360) + randomAngle - wheel.transform.rotation.eulerAngles.z; 
            slowdownTime = (2 * distance) / config.rotationSpeed;
            float slowndown = config.rotationSpeed / slowdownTime;

            while (elapsedTime < slowdownTime)
            {
                rotSpeed = Mathf.Lerp(config.rotationSpeed, 0, elapsedTime / slowdownTime);
                wheel.transform.rotation *= Quaternion.Euler(0, 0, rotSpeed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private void SetPointSector()
        {
            randomSector = GetSector();
            Debug.Log(randomSector);
            float maxAngle = 360f / config.cells.Count * (randomSector + 1);
            float minAngle = 360f / config.cells.Count * randomSector;
            randomAngle = Random.Range(minAngle + 5, maxAngle - 5);
        }

        private int GetSector()
        {
            int randomSector = Random.Range(0, config.cells.Count);
            if (config.cells[randomSector].wight <= Random.Range(0f, 1f))
            {
               return GetSector();
            }
            return randomSector;
        }
    }
}