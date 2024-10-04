using UnityEngine;
using Photon.Pun;

namespace MultiplayerFPS
{
    [RequireComponent(typeof(PlayerMotor))]
    [RequireComponent(typeof(ConfigurableJoint))]
    public class PlayerController : MonoBehaviourPun
    {
        [SerializeField] private float speed = 16f;

        [SerializeField] private float lookSensitivity = 25f;

        [SerializeField] private int thrusterForce = 2500;

        [SerializeField] private float thrusterFuelBurnSpeed = 0.3f;
        [SerializeField] private float thrusterFuelRegenSpeed = 0.3f;
        private float thrusterFuelAmount = 1f;
        public float ThrusterFuelAmount
        {
            get { return thrusterFuelAmount; }
        }

        [Header("Joint Option")]
        [SerializeField] private float jointSpring = 25f;
        [SerializeField] private float jointMaxForce = 50f;

        private PlayerMotor motor;
        private ConfigurableJoint configurableJoint;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            motor = GetComponent<PlayerMotor>();
            configurableJoint = GetComponent<ConfigurableJoint>();

            SetJointSetting(jointSpring);
        }

        private void Update()
        {
            if (!photonView.IsMine)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
                Cursor.lockState = CursorLockMode.None;

            RaycastHit _hit;
            if (Physics.Raycast(transform.position, Vector3.down, out _hit, 100f))
            {
                configurableJoint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
            }
            else
            {
                configurableJoint.targetPosition = new Vector3(0f, 0f, 0f);
            }

            float _xMov = Input.GetAxisRaw("Horizontal");
            float _zMov = Input.GetAxisRaw("Vertical");

            Vector3 _moveHorizontal = transform.right * _xMov;
            Vector3 _moveVertical = transform.forward * _zMov;
            Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * speed;
            motor.Move(_velocity);

            float _yRot = Input.GetAxisRaw("Mouse X");
            Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;
            motor.Rotate(_rotation);

            float _xRot = Input.GetAxisRaw("Mouse Y");
            float _cameraRotation = _xRot * lookSensitivity;
            motor.RotateCamera(_cameraRotation);

            Vector3 _thrusterForce = Vector3.zero;
            if (Input.GetButton("Jump") && thrusterFuelAmount > 0f)
            {
                thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

                if (thrusterFuelAmount >= 0.01f)
                {
                    _thrusterForce = Vector3.up * thrusterForce;
                    SetJointSetting(0f);
                }
            }
            else
            {
                thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;

                SetJointSetting(jointSpring);
            }

            thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

            motor.ApplyThruster(_thrusterForce);
        }

        private void SetJointSetting(float _jointSpring)
        {
            configurableJoint.yDrive = new JointDrive
            {
                positionSpring = _jointSpring,
                maximumForce = jointMaxForce
            };
        }
    }
}