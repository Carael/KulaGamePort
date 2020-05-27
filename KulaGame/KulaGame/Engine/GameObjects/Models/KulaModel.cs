using System;
using System.Collections.Generic;
using KulaGame.Engine.Inferfaces;
using KulaGame.Engine.Utils;
using KulaGame.ScreenManagement.ScreenManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace KulaGame.Engine.GameObjects.Models
{
    public class KulaModel : BaseModel
    {

        public Matrix RotationMatrix
        {
            get { return _rotationMatrix; }
            private set { _rotationMatrix = value; }
        }

        public bool CheckIsOutsideTheLevel
        {
            get { return _isMoving == false && _isCameraRotating == false && _rotate == false && _isJumping == false && _skyboardJump == false; }
        }

        public bool IsMoving { get { return _isMoving; } }
        public bool FallFromTooHeight { get { return _fallFromTooHeight; } }


        private Vector3 _moveOffset;
        private bool _isMoving = false;
        Matrix _rotationMatrix = Matrix.Identity;

        private bool _isCameraRotating = false;
        private float _cameraRotate = 0;
        private float _toPitch = 0;
        private readonly float _kulaScale;
        private bool _rotate = false;
        private CameraRotationAxis _up = CameraRotationAxis.Y;
        private KulaMoveState _moveState;
        private Matrix _localWorld;
        private bool _isJumping = false;
        private int _jumpHeight = 0;
        private bool _jumpDown = false;
        private bool _isOutsideTheLevel = false;
        private bool _isFalling = false;
        private bool _willFallAfterJump = false;
        private int _totalFallHeight = 0;
        private float _dragOffset;
        private bool _fallFromTooHeight = false;
        private bool _isLongJump = false;
        private readonly float _kulaOverallScale = 0.40f;
        private float _kulaYScaleVariation = 0;
        private int _kulaScaleElapsedTime = 0;
        private bool _kulaScaleDirection = false;
        private bool _willJumpToTheBlockBack = false;
        private Vector3 _willJumpToTheBlockBackValue = new Vector3(0, 0, 0);
        private float _kulaBeforeJumpPositionY = 0;
        private int _totalJumpOffset = 0;
        private bool _rotateFast = false;
        private int _stayPulsateOffset = 0;
        private bool _rotateWhileMove = true;
        private int _upAfterBounce = 0;
        private bool _moveWhileBounce = false;
        private bool _stopBouncingLoop = false;
        private bool _skyboardJump = false;
        private int _skyboardUp = 0;
        private int _skyboardForward = 0;
        private int _totalCameraRotation = 0;

        private List<BrickModel> _brickModels;
        private List<SkyboardModel> _skyboardModels;
        public Vector3 InitialRotation { get; private set; }

        public Pad pad;
        public AccelInput accelerometer;
        public SoundManager soundManager;
        public SpecialEffects specialEffect;

        public KulaModel(Model model, Vector3 position, Vector3 initialRotation, List<BrickModel> brickModels, List<SkyboardModel> skyboardModels)
            : base(model, position, initialRotation)
        {
            _brickModels = brickModels;
            _skyboardModels = skyboardModels;
            _moveOffset = new Vector3(0, 0, 0);
            _kulaScale = _kulaOverallScale;
            _localWorld = Matrix.Identity * Matrix.CreateTranslation(position);
            _dragOffset = 0;
            _world = Matrix.Identity *Matrix.CreateScale(0.80f) * Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(initialRotation.Y), MathHelper.ToRadians(initialRotation.X), MathHelper.ToRadians(initialRotation.Z)) * Matrix.CreateTranslation(position);
            RotationMatrix = Matrix.Identity *
                             Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(initialRotation.Y),
                                                           MathHelper.ToRadians(initialRotation.X),
                                                           MathHelper.ToRadians(initialRotation.Z));
            InitialRotation = initialRotation;
            _bounds = new BoundingBox(Common.RoundVector(Vector3.Transform(_volume.Min, Matrix.CreateScale(new Vector3(_kulaScale, _kulaScale, _kulaScale)) * _localWorld)), Common.RoundVector(Vector3.Transform(_volume.Max, Matrix.CreateScale(new Vector3(_kulaScale, _kulaScale, _kulaScale)) * _localWorld)));

            
        }

        public void HandleInput(InputState input)
        {
            bool handled = false;
            if (accelerometer.IsTurnOn)
            {
                if (accelerometer.direction == AccelInput.Direction.Jump)
                {
                    TryToJump();
                    handled = true;
                }
                else if (accelerometer.direction == AccelInput.Direction.Forward)
                {
                    TryToMove();
                    handled = true;
                }
                if (_isCameraRotating == false && _isMoving == false && _rotate == false && _skyboardJump == false)
                {
                    if (accelerometer.direction == AccelInput.Direction.Back && accelerometer.prevDirection == AccelInput.Direction.None)
                    {
                        _rotateFast = true;
                        _isCameraRotating = true;
                        _cameraRotate = -180;
                        _totalCameraRotation -= 180;
                        _up = Common.CameraRotationAxis(GetUpVector());
                    }

                    if (accelerometer.direction == AccelInput.Direction.Left && accelerometer.prevDirection == AccelInput.Direction.None)
                    {
                        _isCameraRotating = true;
                        _cameraRotate = 90;
                        _totalCameraRotation += 90;
                        _up = Common.CameraRotationAxis(GetUpVector());
                    }

                    if (accelerometer.direction == AccelInput.Direction.Right && accelerometer.prevDirection == AccelInput.Direction.None)
                    {
                        _isCameraRotating = true;
                        _cameraRotate = -90;
                        _totalCameraRotation -= 90;
                        _up = Common.CameraRotationAxis(GetUpVector());
                    }
                }
            }
            if (input.TouchState.Count > 0)
            {
                if (pad.IsVisible)
                {
                    if (input.TouchState[0].Position.X >= pad.JumpRect.X && input.TouchState[0].Position.X <= pad.JumpRect.Width &&
                    input.TouchState[0].Position.Y >= pad.JumpRect.Y &&
                    input.TouchState[0].Position.Y <= pad.JumpRect.Width)
                    {
                        TryToJump();
                        handled = true;
                    }

                    if (input.TouchState[0].Position.X >= pad.UpRect.X && input.TouchState[0].Position.X <= pad.UpRect.Width &&
                        input.TouchState[0].Position.Y >= pad.UpRect.Y && input.TouchState[0].Position.Y <= pad.UpRect.Height)
                    {
                        TryToMove();
                        handled = true;
                    }
                }
            }
            if (handled == false)
            {
                foreach (var gesture in input.Gestures)
                {
                    if (gesture.GestureType == GestureType.Tap)
                    {
                        if (_isCameraRotating == false && _isMoving == false && _rotate == false)
                        {
                            if (pad.IsVisible)
                            {
                                if (gesture.Position.X >= pad.LeftRect.X && gesture.Position.X <= pad.LeftRect.Width &&
                                    gesture.Position.Y >= pad.LeftRect.Y && gesture.Position.Y <= pad.LeftRect.Height)
                                {
                                    _isCameraRotating = true;
                                    _cameraRotate = 90;
                                    _totalCameraRotation += 90;
                                    _up = Common.CameraRotationAxis(GetUpVector());
                                }
                                    //rotate camera right
                                else if (gesture.Position.X >= pad.RightRect.X &&
                                         gesture.Position.X <= pad.RightRect.Width &&
                                         gesture.Position.Y >= pad.RightRect.Y &&
                                         gesture.Position.Y <= pad.RightRect.Height)
                                {
                                    _isCameraRotating = true;
                                    _cameraRotate = -90;
                                    _totalCameraRotation -= 90;
                                    _up = Common.CameraRotationAxis(GetUpVector());
                                }
                                else if (gesture.Position.X >= pad.DownRect.X &&
                                         gesture.Position.X <= pad.DownRect.Width &&
                                         gesture.Position.Y >= pad.DownRect.Y &&
                                         gesture.Position.Y <= pad.DownRect.Height)
                                {
                                    _rotateFast = true;
                                    _isCameraRotating = true;
                                    _cameraRotate = -180;
                                    _totalCameraRotation -= 180;
                                    _up = Common.CameraRotationAxis(GetUpVector());
                                }
                            }
                        }
                    }
                    else if (gesture.GestureType == GestureType.Flick)
                    {
                        if (_isCameraRotating == false && _isMoving == false && _rotate == false)
                        {
                            //from left to right
                            if (gesture.Delta.X > 1000)
                            {
                                _isCameraRotating = true;
                                _cameraRotate = -90;
                                _totalCameraRotation -= 90;
                                _up = Common.CameraRotationAxis(GetUpVector());
                            }
                            //from right to left
                            if (gesture.Delta.X < -1000)
                            {
                                _isCameraRotating = true;
                                _cameraRotate = 90;
                                _totalCameraRotation += 90;
                                _up = Common.CameraRotationAxis(GetUpVector());
                            }
                        }
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_isCameraRotating == false && _isMoving == false && _rotate == false)
            {
                
                //go forward
                if (false)
                {
                    TryToMove();
                }

                //try to jump
                else if (false)
                {
                    TryToJump();
                }

                //rotate camera left
                else if (false)
                {
                    _isCameraRotating = true;
                    _cameraRotate = 90;
                    _totalCameraRotation += 90;
                    _up = Common.CameraRotationAxis(GetUpVector());
                }
                //rotate camera right
                else if (false)
                {
                    _isCameraRotating = true;
                    _cameraRotate = 90;
                    _totalCameraRotation += 90;
                    _up = Common.CameraRotationAxis(GetUpVector());
                }

            }



            //handle accelerometr input




            _stayPulsateOffset += gameTime.ElapsedGameTime.Milliseconds;

            _kulaScaleElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (_kulaScaleElapsedTime > 500)
            {
                _kulaScaleElapsedTime = 0;
                _kulaScaleDirection = !_kulaScaleDirection;
            }

            if (_kulaScaleDirection)
            {
                _kulaYScaleVariation -= 0.002f;
            }
            else
            {
                _kulaYScaleVariation += 0.002f;
            }





            if (_cameraRotate > 0)
            {
                if (_rotateFast)
                {
                    YawAndRotate(30);
                    //calculate withch axis to rotate
                    _cameraRotate -= 30;
                }
                else
                {
                    YawAndRotate(10);
                    //calculate withch axis to rotate
                    _cameraRotate -= 10;
                }

            }
            else if (_cameraRotate < 0)
            {
                if (_rotateFast)
                {
                    YawAndRotate(-30);

                    _cameraRotate += 30;
                }
                else
                {
                    YawAndRotate(-10);

                    _cameraRotate += 10;
                }
            }
            if (_cameraRotate == 0)
            {
                _rotateFast = false;
                _isCameraRotating = false;
            }
            if (_moveOffset.Z == -2)
            {
                _moveOffset.Z += 2;
                MoveObject(new Vector3(0, 0, -2));
            }
            //move in z direction
            else if (_moveOffset.Z < 0)
            {
                int jumpOffset = 0;
                if (_isJumping)
                {
                    if (_jumpDown)
                    {
                        jumpOffset = -2;
                        _jumpHeight -= 2;
                        _totalJumpOffset -= 2;
                    }
                    else
                    {
                        jumpOffset = 2;
                        _jumpHeight += 2;
                        _totalJumpOffset += 2;
                    }
                    if (_isLongJump)
                    {
                        if (_jumpHeight == 20)
                        {
                            _jumpDown = true;
                        }
                        else if (_jumpHeight == 0)
                        {
                            _jumpDown = false;
                        }
                    }
                    else
                    {
                        if (_jumpHeight == 14)
                        {
                            _jumpDown = true;
                        }
                        else if (_jumpHeight == 0)
                        {
                            _jumpDown = false;
                        }
                    }



                }



                if (_moveOffset.Z == -2)
                {
                    _moveOffset.Z += 2;
                    MoveObject(new Vector3(0, 0, -2));
                }
                else if (_moveOffset.Z == -1)
                {
                    _moveOffset.Z += 1;
                    MoveObject(new Vector3(0, 0, -1));
                }
                else
                {
                    _moveOffset.Z += 3;
                    MoveObject(new Vector3(0, 0, -3));
                }

                JumpObject(jumpOffset);
            }

            else if ((_moveOffset.Z == 0 || _moveOffset.Z > 0) && _willJumpToTheBlockBackValue.Z > 0)
            {
                if (_willJumpToTheBlockBack)
                {
                    _totalJumpOffset += _upAfterBounce;

                    if (_totalJumpOffset % 4 != 0)
                    {
                        if (_totalJumpOffset > 0)
                        {
                            JumpObject(-(_totalJumpOffset % 4));
                        }
                        else
                        {
                            JumpObject((_totalJumpOffset % 4));
                        }

                    }
                    _stopBouncingLoop = true;
                    _rotateWhileMove = false;
                    _willJumpToTheBlockBack = false;
                }


                if (_moveWhileBounce)
                {
                    MoveObject(new Vector3(0, 0, 1f));
                }

                _moveWhileBounce = !_moveWhileBounce;


                if (_upAfterBounce > 0)
                {
                    JumpObject(1);
                    _upAfterBounce -= 1;
                }
                else
                {
                    JumpObject(-_totalJumpOffset / 4);
                }


                _willJumpToTheBlockBackValue.Z -= 0.5f;


                //jump to original height

                if (_willJumpToTheBlockBackValue.Z == 0)
                {
                    _isFalling = true;
                    //SoundManager.KulaUpadek();
                }
            }
            else if (_moveOffset.Z == 0 || _moveOffset.Z > 0)
            {

                _totalJumpOffset = 0;
                _jumpDown = false;
                _jumpHeight = 0;
                _upAfterBounce = 4;
                if (_willFallAfterJump == false && _isFalling == false)
                {
                    _isMoving = false;


                    //tutaj nale¿y sprawdziæ czy kula upad³a na kostkê z tarcz¹ - jeœli tak odbija siê do przodu o 60 jednostek
                    //sprawdziæ te¿ czy upda³a na zwyk³a kostkê - jeœli tak odtworzyæ odpowiedni dzwiêk

                    bool isJumpingTemp = _isJumping;
                    bool isLongJumpTemp = _isLongJump;

                    _isJumping = false;
                    _isLongJump = false;

                    if (isJumpingTemp == true || isLongJumpTemp == true)
                    {
                        if (!_stopBouncingLoop)
                        {
                            if (BounceCollision())
                            {
                                TryToLongJump();
                            }
                            else if (CheckCollision())
                            {
                                soundManager.KulaUpadek();
                            }
                        }


                    }


                }


                if (_rotate == true)
                {
                    if (_toPitch > 0)
                    {
                        Pitch(10);
                        _toPitch -= 10;
                    }
                    else if (_toPitch < 0)
                    {
                        Pitch(-10);
                        _toPitch += 10;
                    }
                    if (_toPitch == 0)
                    {
                        _rotate = false;
                        if (_moveState == KulaMoveState.Down)
                        {
                            _moveOffset = new Vector3(0, 0, -15);
                        }
                        else if (_moveState == KulaMoveState.Up)
                        {
                            _moveOffset = new Vector3(0, 0, -5);
                        }

                        _isMoving = true;
                    }
                }


            }

            if (_moveOffset == new Vector3(0, 0, 0))
            {
                if (_willFallAfterJump)
                {
                    _willFallAfterJump = false;
                    _isFalling = true;
                    _isMoving = true;
                    //fall down 100
                }
            }

            if (_isFalling)
            {
                bool check = true;
                //jeœli spadliœmy o 20 sprawdzamy czy jest kolizja z kostk¹
                if (_totalFallHeight % 20 == 0)
                {
                    if (CheckCollision())
                    {
                        _isFalling = false;
                        _isMoving = false;
                        check = false;
                        _totalFallHeight = 0;

                        if (!_stopBouncingLoop)
                        {
                            //tutaj nale¿y sprawdziæ czy kula upad³a na kostkê z tarcz¹ - jeœli tak odbija siê do przodu o 60 jednostek
                            if (BounceCollision())
                            {
                                TryToLongJump();
                            }
                            else if (CheckCollision())
                            {
                                soundManager.KulaUpadek();
                            }
                        }
                    }
                }
                //sprawdzamy czy nies spadliœmy na trampoline - jeœli tak lecimy w górê o 150 (140 + 10 które spadliœmy), 
                //40 do przodu i na koniec spadamy a¿ nie trafimy na blok , kolejn¹ trampolinê lub spadniemy ca³kowicie
                else if (_totalFallHeight % 10 == 0)
                {
                    if (CheckCollisionWithSkyboard() == SkyboardCollision.Front)
                    {

                        specialEffect.StartEffect(specialEffect.effect = SpecialEffects.Effect.Boing,
                                                  new Vector2(400, 350), 0.3f, 0.3f);
                        _isFalling = false;
                        check = false;
                        _totalFallHeight = 0;
                        _skyboardJump = true;
                        _skyboardUp = 150;
                        _skyboardForward = 40;
                        soundManager.Trampolina();

                    }
                    else if (CheckCollisionWithSkyboard() == SkyboardCollision.Back)
                    {
                        _isFalling = false;
                        check = false;
                        _totalFallHeight = 0;
                        _skyboardJump = true;
                        _skyboardUp = 30;
                        _skyboardForward = 20;
                        soundManager.KulaUpadek();
                    }
                }


                if (check)
                { //jeœli upadek ma wielkoœæ powy¿ej 200 zak³adamy ¿e kula siê przebije i koñczymy grê
                    if (_totalFallHeight > 200)
                    {
                        //przegrana
                        _fallFromTooHeight = true;
                    }
                    //jeœli powy¿sze nie jest spe³nione spadamy o 5 jednostek w dó³
                    else
                    {
                        MoveObject(new Vector3(0, -5, 0));
                        _totalFallHeight += 5;
                    }

                }
            }

            if (_skyboardJump)
            {
                if (_skyboardForward == 0 && _skyboardUp == 0)
                {
                    _isFalling = true;
                    _totalFallHeight = 0;
                    _skyboardJump = false;
                    _isMoving = true;
                }
                else
                {
                    if (_skyboardUp != 0)
                    {
                        JumpObject(15);
                        _skyboardUp -= 15;
                    }
                    if (_skyboardForward != 0)
                    {
                        MoveObject(new Vector3(0, 0, -4));
                        _skyboardForward -= 4;
                    }

                }
            }


            _bounds = new BoundingBox(Common.RoundVector(Vector3.Transform(_volume.Min, Matrix.CreateScale(new Vector3(_kulaScale, _kulaScale, _kulaScale)) * _localWorld)), Common.RoundVector(Vector3.Transform(_volume.Max, Matrix.CreateScale(new Vector3(_kulaScale, _kulaScale, _kulaScale)) * _localWorld)));
            //base.Update(gameTime);

            if (_isMoving == true || _isJumping == true || _isFalling == true || _isLongJump == true)
            {
                _stayPulsateOffset = 0;
            }
        }

        private bool BounceCollision()
        {
            bool collision = false;

            foreach (var brick in _brickModels)
            {
                if (brick.BoundingBox.Intersects(BoundingBoxAfterMovement(new Vector3(0, -2, 0))) && brick.BrickType == BrickType.Rubber)
                {
                    specialEffect.StartEffect(specialEffect.effect = SpecialEffects.Effect.Doing, new Vector2(400, 350), 0.3f, 0.3f);
                    collision = true;
                    continue;
                }
            }
            return collision;
        }

        public void MoveObject(Vector3 movement)
        {
            Vector3 position = GetPosition();
            if (_rotateWhileMove)
            {
                _world *= Matrix.CreateTranslation(-position);
                _world *= Matrix.CreateFromAxisAngle(this.RotationMatrix.Left, MathHelper.ToRadians(20));
                _world *= Matrix.CreateTranslation(position);
            }
            _world *= Matrix.CreateTranslation(CalculateVeleocity(movement));
            _localWorld *= Matrix.CreateTranslation(CalculateVeleocity(movement));
        }



        public void JumpObject(int amount)
        {


            _world.Translation += _rotationMatrix.Up * amount;
            _localWorld.Translation += _rotationMatrix.Up * amount;





            //_world *= Matrix.CreateFromAxisAngle(this.RotationMatrix.Left, MathHelper.ToRadians(10));//Matrix.CreateWorld(newPosition, Common.RoundVector(orientation.Forward), Common.RoundVector(orientation.Up));//
            //_world *= Matrix.CreateFromAxisAngle(this.RotationMatrix.Left, MathHelper.ToRadians(10)) * Matrix.CreateTranslation(CalculateVeleocity(movement));// * Matrix.CreateFromAxisAngle(this.RotationMatrix.Left, MathHelper.ToRadians(10));
        }


        public void TryToMove()
        {
            if (_isMoving == false && _isCameraRotating == false && _rotate == false && _isJumping == false && _isFalling == false && _skyboardJump == false)
            {

                _moveState = GetMoveState(new Vector3(0, 0, -20));

                if (_moveState == KulaMoveState.Forward)
                {
                    _moveOffset = new Vector3(0, 0, -20);
                    _isMoving = true;
                    _rotateWhileMove = true;
                }
                else if (_moveState == KulaMoveState.Down)
                {
                    _moveOffset = new Vector3(0, 0, -15);
                    _isMoving = true;
                    _rotate = true;
                    _toPitch = -90;
                    _rotateWhileMove = true;
                }
                else if (_moveState == KulaMoveState.Up)
                {
                    _moveOffset = new Vector3(0, 0, -5);
                    _isMoving = true;
                    _rotate = true;
                    _toPitch = 90;
                    _rotateWhileMove = true;
                }
            }
        }

        public void TryToJump()
        {
            if (_isMoving == false && _isCameraRotating == false && _rotate == false && _isJumping == false && _isFalling == false && _skyboardJump == false)
            {
                _moveState = GetMoveState(new Vector3(0, 0, -40));
                var moveStateNext = GetMoveState(new Vector3(0, 0, -20));

                if (moveStateNext == KulaMoveState.Up)
                {
                    _moveOffset = new Vector3(0, 0, -4);
                    _willJumpToTheBlockBackValue = new Vector3(0, 0, 4);
                    _isMoving = true;
                    _isJumping = true;
                    _isLongJump = false;
                    _kulaBeforeJumpPositionY = GetPosition().Y;
                    _willJumpToTheBlockBack = true;
                }
                else if (_moveState == KulaMoveState.Up)
                {
                    _moveOffset = new Vector3(0, 0, -24);
                    _willJumpToTheBlockBackValue = new Vector3(0, 0, 4);
                    _isMoving = true;
                    _isJumping = true;
                    _isLongJump = false;
                    _kulaBeforeJumpPositionY = GetPosition().Y;
                    _willJumpToTheBlockBack = true;
                }
                else if (_moveState != KulaMoveState.Up && moveStateNext != KulaMoveState.Up)
                {
                    _moveOffset = new Vector3(0, 0, -40);
                    _isMoving = true;
                    _isJumping = true;
                    _isLongJump = false;
                    _kulaBeforeJumpPositionY = GetPosition().Y;
                    if (_moveState == KulaMoveState.Down)
                    {
                        _willFallAfterJump = true;
                    }
                }
                _stopBouncingLoop = false;
                soundManager.KulaOdbicie();
            }
        }


        public void TryToLongJump()
        {
            if (_isMoving == false && _isCameraRotating == false && _rotate == false && _isJumping == false && _isFalling == false && _skyboardJump == false)
            {
                _moveState = GetMoveState(new Vector3(0, 0, -60));
                var moveStateNext = GetMoveState(new Vector3(0, 0, -20));
                var moveStateNextNext = GetMoveState(new Vector3(0, 0, -40));

                if (moveStateNext == KulaMoveState.Up)
                {
                    _moveOffset = new Vector3(0, 0, -4);
                    _willJumpToTheBlockBackValue = new Vector3(0, 0, 4);
                    _isMoving = true;
                    _isJumping = true;
                    _isLongJump = false;
                    _kulaBeforeJumpPositionY = GetPosition().Y;
                    _willJumpToTheBlockBack = true;
                }
                else if (moveStateNextNext == KulaMoveState.Up)
                {
                    _moveOffset = new Vector3(0, 0, -24);
                    _willJumpToTheBlockBackValue = new Vector3(0, 0, 4);
                    _isMoving = true;
                    _isJumping = true;
                    _isLongJump = false;
                    _kulaBeforeJumpPositionY = GetPosition().Y;
                    _willJumpToTheBlockBack = true;
                }
                else if (_moveState == KulaMoveState.Up)
                {
                    _moveOffset = new Vector3(0, 0, -44);
                    _willJumpToTheBlockBackValue = new Vector3(0, 0, 4);
                    _isMoving = true;
                    _isJumping = true;
                    _isLongJump = false;
                    _kulaBeforeJumpPositionY = GetPosition().Y;
                    _willJumpToTheBlockBack = true;
                }

                else if (_moveState != KulaMoveState.Up && moveStateNextNext != KulaMoveState.Up && moveStateNext != KulaMoveState.Up)
                {
                    _moveOffset = new Vector3(0, 0, -60);
                    _isMoving = true;
                    _isJumping = true;
                    _isLongJump = true;
                    if (_moveState == KulaMoveState.Down)
                    {
                        _willFallAfterJump = true;

                    }
                }

                soundManager.OdbicieKostka();
                _stopBouncingLoop = false;
            }
        }



        public void StopMovement()
        {
            _moveOffset = new Vector3(0, 0, 0);
            _isMoving = false;
        }



        public Vector3 GetUpVector()
        {
            Matrix orientation = _rotationMatrix;



            return Common.RoundVector(orientation.Up);
        }

        public void YawAndRotate(float amount)
        {
            this._rotationMatrix *= Matrix.CreateFromAxisAngle(Common.RoundVector(_rotationMatrix.Up), MathHelper.ToRadians(amount));
        }

        public void Pitch(float amount)
        {
            this._rotationMatrix *= Matrix.CreateFromAxisAngle(Common.RoundVector(_rotationMatrix.Right), MathHelper.ToRadians(amount));
        }



        public void Jump()
        {
            if (_isMoving == false && _isCameraRotating == false && _isJumping == false)
            {

                _isMoving = true;
                _isJumping = true;
            }
        }

        public Vector3 CalculateVeleocity(Vector3 movement)
        {

            Vector3 velocity = Vector3.Transform(movement, _rotationMatrix);
            if (_isFalling)
            {
                return Common.RoundVector(velocity, (int)movement.Y);
            }
            else
            {
                return Common.RoundVector(velocity, (int)movement.Z);
            }

        }

        public BoundingBox BoundingBoxAfterMovement(Vector3 movement)
        {
            Vector3 newPosition = GetPosition() + CalculateVeleocity(movement);
            Matrix localWorld = Matrix.Identity * Matrix.CreateTranslation(newPosition);
            var localBounds = new BoundingBox(Common.RoundVector(Vector3.Transform(_volume.Min, Matrix.CreateScale(new Vector3(_kulaScale, _kulaScale, _kulaScale)) * localWorld)), Common.RoundVector(Vector3.Transform(_volume.Max, Matrix.CreateScale(new Vector3(_kulaScale, _kulaScale, _kulaScale)) * localWorld)));
            return localBounds;
        }

        public Vector3 GetPosition()
        {
            return _world.Translation;
        }

        public KulaMoveState GetMoveState(Vector3 movement)
        {
            //initial state - if no changed then don't move
            var moveState = KulaMoveState.Stop;
            var movedBoundingBox = BoundingBoxAfterMovement(movement);

            bool goDown = true;
            //check if go forward - there should be only one collision after move for 20 forward
            //if goDown after iteration is false, than move forward to the edge, rotate -90 degrees in y and go forward to the center
            int colisionCount = 0;
            foreach (var brick in _brickModels)
            {
                if (movedBoundingBox.Intersects(brick.BoundingBox))
                {
                    goDown = false;
                    moveState = KulaMoveState.Forward;
                    colisionCount += 1;
                }
            }

            if (goDown)
            {
                moveState = KulaMoveState.Down;
            }

            if (colisionCount >= 2)
            {
                moveState = KulaMoveState.Up;
            }

            return moveState;
        }

        public override void Draw(GameTime gameTime, ICamera camera)
        {


            Matrix[] transforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(transforms);
            Matrix drawWorld = GetWorldMatrix();
            if (_stayPulsateOffset > 100)
            {
                CameraRotationAxis cameraRotationAxis = Utils.Common.CameraRotationAxis(RotationMatrix.Up);


                Vector3 position = GetPosition();
                drawWorld *= Matrix.CreateTranslation(-position);
                switch (cameraRotationAxis)
                {

                    case CameraRotationAxis.X:
                        drawWorld *= Matrix.CreateScale(1 + _kulaYScaleVariation, 1, 1);
                        break;
                    case CameraRotationAxis.Y:
                        drawWorld *= Matrix.CreateScale(1, 1 + _kulaYScaleVariation, 1);
                        break;
                    case CameraRotationAxis.Z:
                        drawWorld *= Matrix.CreateScale(1, 1, 1 + _kulaYScaleVariation);
                        break;
                }
                drawWorld *= Matrix.CreateTranslation(position);
            }


            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();


                    //effect.Alpha = .5f;
                    //effect.DiffuseColor = new Vector3(0.1f, 0.9f, 0.1f);
                    effect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
                    effect.SpecularPower = 1.0f;
                    effect.AmbientLightColor = new Vector3(0.9f, 0.9f, 0.9f);
                    effect.LightingEnabled = true;
                    //effect.DirectionalLight0.Enabled = true;
                    //effect.DirectionalLight0.DiffuseColor = new Vector3(0.75f, 0.75f, 0.75f);
                    //effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
                    //effect.DirectionalLight0.SpecularColor = new Vector3(0.75f, 0.75f, 0.75f);






                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.World = mesh.ParentBone.Transform * drawWorld;
                }
                mesh.Draw();
            }
        }

        public bool CheckCollision()
        {
            bool collision = false;

            foreach (var brick in _brickModels)
            {
                if (brick.BoundingBox.Intersects(BoundingBoxAfterMovement(new Vector3(0, -2, 0))))
                {
                    collision = true;
                    continue;
                }
            }
            return collision;
        }

        public SkyboardCollision CheckCollisionWithSkyboard()
        {
            SkyboardCollision skyboardCollision = SkyboardCollision.None;

            foreach (var skyboard in _skyboardModels)
            {
                if (skyboard.BoundingBox.Intersects(BoundingBoxAfterMovement(new Vector3(0, -2, 0))))
                {

                    //teraz sprawdzamy czy skoczyliœmy na trampolinê od góry a nie z boku i od do³u

                    if (Utils.Common.CheckUpVector(RotationMatrix.Up, skyboard.GetWorldMatrix().Up))
                    {
                        skyboardCollision = SkyboardCollision.Front;
                    }
                    else
                    {
                        skyboardCollision = SkyboardCollision.Back;
                    }

                    continue;
                }
            }
            return skyboardCollision;
        }

        public void Teleport(Vector3 destinationPosition, Vector3 destinationRotation)
        {
            var oldPosition = _localWorld.Translation;

            _world = Matrix.Identity *Matrix.CreateScale(0.80f) * Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(destinationRotation.Y), MathHelper.ToRadians(destinationRotation.X), MathHelper.ToRadians(destinationRotation.Z)) * Matrix.CreateTranslation(destinationPosition);
            _localWorld = Matrix.Identity * Matrix.CreateTranslation(destinationPosition);
            _rotationMatrix = Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(destinationRotation.Y),
                                                            MathHelper.ToRadians(destinationRotation.X),
                                                            MathHelper.ToRadians(destinationRotation.Z));


            CameraRotationAxis upVector = Common.CameraRotationAxis(_rotationMatrix.Up);

            var positionDifference = _localWorld.Translation - oldPosition;

            switch (upVector)
            {
                case CameraRotationAxis.X:
                    if (positionDifference.X > 5)
                    {
                        _localWorld *= Matrix.CreateTranslation(new Vector3(-5, 0, 0));
                        _world *= Matrix.CreateTranslation(new Vector3(-5, 0, 0));

                        if (CheckCollision() == false)
                        {
                            _localWorld *= Matrix.CreateTranslation(new Vector3(10, 0, 0));
                            _world *= Matrix.CreateTranslation(new Vector3(10, 0, 0));
                        }

                    }
                    else
                    {
                        _localWorld *= Matrix.CreateTranslation(new Vector3(5, 0, 0));
                        _world *= Matrix.CreateTranslation(new Vector3(5, 0, 0));

                        if (CheckCollision() == false)
                        {
                            _localWorld *= Matrix.CreateTranslation(new Vector3(-10, 0, 0));
                            _world *= Matrix.CreateTranslation(new Vector3(-10, 0, 0));
                        }
                    }

                    break;
                case CameraRotationAxis.Y:
                    if (positionDifference.Y > 5)
                    {
                        _localWorld *= Matrix.CreateTranslation(new Vector3(0, 5, 0));
                        _world *= Matrix.CreateTranslation(new Vector3(0, 5, 0));

                        if (CheckCollision()==false)
                        {
                            _localWorld *= Matrix.CreateTranslation(new Vector3(0, -10, 0));
                            _world *= Matrix.CreateTranslation(new Vector3(0, -10, 0));
                        }
                    }
                    else
                    {
                        _localWorld *= Matrix.CreateTranslation(new Vector3(0, -5, 0));
                        _world *= Matrix.CreateTranslation(new Vector3(0, -5, 0));

                        if (CheckCollision() == false)
                        {
                            _localWorld *= Matrix.CreateTranslation(new Vector3(0, 10, 0));
                            _world *= Matrix.CreateTranslation(new Vector3(0, 10, 0));
                        }
                    }

                    break;
                case CameraRotationAxis.Z:

                    if (positionDifference.Z > 5)
                    {
                        _localWorld *= Matrix.CreateTranslation(new Vector3(0, 0, -5));
                        _world *= Matrix.CreateTranslation(new Vector3(0, 0, -5));

                        if (CheckCollision() == false)
                        {
                            _localWorld *= Matrix.CreateTranslation(new Vector3(0, 0, 10));
                            _world *= Matrix.CreateTranslation(new Vector3(0, 0, 10));
                        }
                    }
                    else
                    {
                        _localWorld *= Matrix.CreateTranslation(new Vector3(0, 0, 5));
                        _world *= Matrix.CreateTranslation(new Vector3(0, 0, 5));

                        if (CheckCollision() == false)
                        {
                            _localWorld *= Matrix.CreateTranslation(new Vector3(0, 0, -10));
                            _world *= Matrix.CreateTranslation(new Vector3(0, 0, -10));
                        }
                    }
                    break;
            }

            _bounds = new BoundingBox(Common.RoundVector(Vector3.Transform(_volume.Min, Matrix.CreateScale(new Vector3(_kulaScale, _kulaScale, _kulaScale)) * _localWorld)), Common.RoundVector(Vector3.Transform(_volume.Max, Matrix.CreateScale(new Vector3(_kulaScale, _kulaScale, _kulaScale)) * _localWorld)));


            YawAndRotate(_totalCameraRotation);
        }
    }
}
