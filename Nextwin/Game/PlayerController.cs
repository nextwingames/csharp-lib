using UnityEngine;

namespace Nextwin.Game
{
    /// <summary>
    /// Use Mouse에 체크되었다면
    /// 왼쪽이 최상위 부모 오른쪽이 최하위 자식일 때
    /// _body - _cameraArm - _camera의 계층 구조를 가져야 하고
    /// 체크되지 않았다면
    /// _camera(_body와 별개) // _cameraArm - _body의 계층 구조를 가져야 함
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        protected bool _onControl = true;

        [Header("Transform(GameObject) Setting")]
        [SerializeField]
        protected Transform _body;
        [SerializeField]
        protected Transform _pivot;
        [SerializeField]
        protected Camera _camera;

        [Header("Mouse Setting")]
        [SerializeField]
        protected bool _useMouse;
        [SerializeField, Range(1, 25), Tooltip("If you do not check Use Mouse, Mouse Sensitivity is not used")]
        protected int _mouseSensitivity = 12;

        [Header("Control Basic Key Setting")]
        [SerializeField]
        protected KeyCode _upKey = KeyCode.UpArrow;
        [SerializeField]
        protected KeyCode _downKey = KeyCode.DownArrow;
        [SerializeField]
        protected KeyCode _leftKey = KeyCode.LeftArrow;
        [SerializeField]
        protected KeyCode _rightKey = KeyCode.RightArrow;
        [SerializeField]
        protected KeyCode _jumpKey = KeyCode.Space;
        [SerializeField]
        protected KeyCode _runKey = KeyCode.LeftShift;

        [Header("Player Setting")]
        [SerializeField]
        protected float _walkSpeed = 4f;
        [SerializeField]
        protected float _runSpeed = 8f;
        protected float _speed;
        [SerializeField, Range(1f, 20f)]
        protected float _rotateSpeed = 10f;
        [SerializeField]
        protected float _jumpPower = 5f;

        [Header("Camera Setting")]
        [SerializeField, Range(0f, 50f)]
        protected float _cameraDistance = 10f;
        [SerializeField, Range(0f, 50f)]
        protected float _cameraHeight = 5f;
        [SerializeField, Range(0f, 90f)]
        protected float _cameraAngle = 20f;
        [SerializeField]
        protected float _cameraSpeed = 2f;

        protected Animator _animator;
        protected Rigidbody _rigidBody;

        protected Vector3 _destPos;
        protected Vector3 _curPos;
        protected Vector3 _lookDir;

        protected bool _isMoving;
        protected bool _isJumping;

        protected virtual void Awake()
        {
            CheckHierarchy();
        }

        protected virtual void Start()
        {
            _animator = _body.GetComponent<Animator>();
            _rigidBody = _body.GetComponent<Rigidbody>();

            _speed = _walkSpeed;
            _destPos = _body.position;
        }

        protected virtual void Update()
        {
            InputKey();
            RotateWithMouse();
        }

        protected virtual void FixedUpdate()
        {
            RotateWithKeyboard();
            Move();
            Jump();
        }

        protected virtual void LateUpdate()
        {
            MoveCamera();
            RotateCamera();
        }

        /// <summary>
        /// 캐릭터 이동
        /// </summary>
        protected virtual void Move()
        {
            if(!_isMoving)
            {
                return;
            }
            OnMoveStart();

            _curPos = _destPos;
            _body.position = _destPos;

            _isMoving = false;
            OnMoveEnd();
        }

        protected virtual void OnMoveStart() { }

        protected virtual void OnMoveEnd() { }

        /// <summary>
        /// 캐릭터 점프
        /// </summary>
        protected virtual void Jump()
        {
            if(!_isJumping)
            {
                return;
            }
            OnJumpStart();

            _rigidBody.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);

            _isJumping = false;
            OnJumpEnd();
        }

        protected virtual void OnJumpStart() { }

        protected virtual void OnJumpEnd() { }

        /// <summary>
        /// 마우스로 회전
        /// </summary>
        protected virtual void RotateWithMouse()
        {
            if(!_onControl || !_useMouse)
            {
                return;
            }

            float sensitivity = _mouseSensitivity / 10f;
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X") * sensitivity, Input.GetAxis("Mouse Y") * sensitivity);
            Vector3 camAngle = _pivot.rotation.eulerAngles;
            float x = camAngle.x - mouseDelta.y;
            x = x < 180f ? Mathf.Clamp(x, -1f, 70f) : Mathf.Clamp(x, 345f, 361f);

            _body.rotation = Quaternion.Euler(0, camAngle.y + mouseDelta.x, camAngle.z);
            _pivot.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
        }

        /// <summary>
        /// 키보드 입력으로만 회전
        /// </summary>
        protected virtual void RotateWithKeyboard()
        {
            if(!_isMoving || _useMouse)
            {
                return;
            }

            float angle = Mathf.Atan2(_lookDir.x, _lookDir.z) * Mathf.Rad2Deg;
            _body.rotation = Quaternion.Slerp(_body.rotation, Quaternion.Euler(0, angle, 0), _rotateSpeed * Time.fixedDeltaTime);

            _lookDir = new Vector3(0, 0, 0);
        }

        /// <summary>
        /// 캐릭터의 body를 따라 카메라를 이동
        /// </summary>
        protected virtual void MoveCamera()
        {
            if(_camera == null)
            {
                return;
            }

            if(_useMouse)
            {
                MoveCameraUsingMouse();
            }
            else
            {
                MoveCameraNotUsingMouse();
            }   
        }

        protected virtual void MoveCameraUsingMouse()
        {
            _camera.transform.localPosition = new Vector3(0, _cameraHeight, -_cameraDistance);
        }

        protected virtual void MoveCameraNotUsingMouse()
        {
            Vector3 backward = -_pivot.forward;
            backward *= _cameraDistance;

            Vector3 camPos = new Vector3(_body.position.x, _body.position.y + _cameraHeight, _body.position.z + backward.z);
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, camPos, Time.deltaTime * _cameraSpeed);
        }

        /// <summary>
        /// 카메라 회전(바라보는 방향 설정)
        /// </summary>
        protected virtual void RotateCamera()
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(_cameraAngle, 0f, 0f));
            
            if(_useMouse)
            {
                RotateCameraUsingMouse(rotation);
            }
            else
            {
                RotateCameraNotUsingMouse(rotation);
            }
        }

        protected virtual void RotateCameraUsingMouse(Quaternion rotation)
        {
            _camera.transform.localRotation = rotation;
        }

        protected virtual void RotateCameraNotUsingMouse(Quaternion rotation)
        {
            _camera.transform.rotation = rotation;
        }

        /// <summary>
        /// 상, 하, 좌, 우 움직임 및 달리기, 점프의 입력을 받음
        /// </summary>
        protected virtual void InputKey()
        {
            if(!_onControl)
            {
                return;
            }

            if(Input.GetKey(_runKey))
            {
                OnInputRunKey();
            }
            if(Input.GetKeyUp(_runKey))
            {
                OnReleaseRunKey();
            }
            if(Input.GetKey(_upKey))
            {
                OnInputUpKey();
            }
            if(Input.GetKey(_downKey))
            {
                OnInputDownKey();
            }
            if(Input.GetKey(_leftKey))
            {
                OnInputLeftKey();
            }
            if(Input.GetKey(_rightKey))
            {
                OnInputRightKey();
            }
            if(Input.GetKeyDown(_jumpKey))
            {
                OnInputJumpKey();
            }
        }

        protected virtual void OnInputUpKey()
        {
            SetDestPos(_pivot.forward);
            if(!_useMouse)
            {
                SetLookDir(0f, 1f);
            }
            _isMoving = true;
        }

        protected virtual void OnInputDownKey()
        {
            SetDestPos(-_pivot.forward);
            if(!_useMouse)
            {
                SetLookDir(0f, -1f);
            }
            _isMoving = true;
        }

        protected virtual void OnInputLeftKey()
        {
            SetDestPos(-_pivot.right);
            if(!_useMouse)
            {
                SetLookDir(-1f, 0f);
            }
            _isMoving = true;
        }

        protected virtual void OnInputRightKey()
        {
            SetDestPos(_pivot.right);
            if(!_useMouse)
            {
                SetLookDir(1f, 0f);
            }
            _isMoving = true;
        }

        /// <summary>
        /// 입력을 받고 이동 목적지 설정
        /// </summary>
        /// <param name="vector">움직이려는 방향</param>
        protected virtual void SetDestPos(Vector3 vector)
        {
            Vector3 dir = new Vector3(vector.x, 0f, vector.z).normalized;
            _destPos += dir * Time.deltaTime * _speed;
            _destPos.y = _body.position.y;
        }

        /// <summary>
        /// 입력을 받고 바라보는 방향 설정
        /// </summary>
        /// <param name="x">좌, 우</param>
        /// <param name="z">상, 하</param>
        protected virtual void SetLookDir(float x, float z)
        {
            if(x == 0)
            {
                _lookDir = new Vector3(_lookDir.x, 0, z);
            }
            else if(z == 0)
            {
                _lookDir = new Vector3(x, 0, _lookDir.z);
            }
        }

        protected virtual void OnInputRunKey()
        {
            _speed = _runSpeed;
        }

        protected virtual void OnReleaseRunKey()
        {
            _speed = _walkSpeed;
        }

        protected virtual void OnInputJumpKey()
        {
            _isJumping = true;
        }

        private void CheckHierarchy()
        {
            if(_useMouse)
            {
                if(!_pivot.IsChildOf(_body))
                {
                    Debug.LogError("Pivot should be child of Body.");
                }
                if(!_camera.transform.IsChildOf(_pivot))
                {
                    Debug.LogError("Camera should be child of Pivot.");
                }
            }
            else
            {
                if(!_body.IsChildOf(_pivot))
                {
                    Debug.LogError("Body should be child of Pivot.");
                }
            }
        }
    }
}
