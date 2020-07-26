// GENERATED AUTOMATICALLY FROM 'Assets/Controls/Player.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace PP.Controls
{
    public class @Player : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Player()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player"",
    ""maps"": [
        {
            ""name"": ""Locomotion"",
            ""id"": ""a5b68161-c84a-4bd5-acc4-df320a32654e"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""98289422-cb90-4a57-8392-f197b2009763"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Duck"",
                    ""type"": ""Button"",
                    ""id"": ""f857c668-384c-4e6b-bc96-d2b89774e6dd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Walk"",
                    ""type"": ""Button"",
                    ""id"": ""6eef71e6-e687-4b68-bf7b-999d6f4e0442"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""d7ae8e79-c36b-451b-8d47-70f0f9d2e29f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""83087d5f-20a8-487a-9965-648a63c956c3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""803197a4-0e7c-4956-9a0c-973a9cd2aada"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""014d370f-6b35-4908-b1c6-c674e26c6890"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ddc876c-3964-429b-bae0-74c25a4b9eb3"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Duck"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""774ceffd-1296-49d5-8e62-4667b3a4fbb7"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Duck"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0a50a7b8-6a1f-4830-b651-075598c4195d"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b61f9b85-1f86-4179-87fe-cffbc386d35f"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Move Direction"",
                    ""id"": ""bc479388-1fd0-48aa-9dee-2aa556f123e0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""79bcad12-5d00-4e28-8df7-c19965f40a93"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""854df0ca-04cd-40ac-8e75-70c7f527215e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5cd5e09d-5400-4464-95cd-40cf090ff6b1"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""099db7d1-1206-4eb4-bc8c-0dc1ff9e5166"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ea6ba949-5ba3-4ff4-88f3-635f62ec1bf7"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bd54e738-301f-498f-aadc-d8c083a1de72"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=0.3,y=0.3)"",
                    ""groups"": ""Computer"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aba6a999-7039-4787-9bf1-891ff71b0c71"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=4,y=4)"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Computer"",
            ""bindingGroup"": ""Computer"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Locomotion
            m_Locomotion = asset.FindActionMap("Locomotion", throwIfNotFound: true);
            m_Locomotion_Jump = m_Locomotion.FindAction("Jump", throwIfNotFound: true);
            m_Locomotion_Duck = m_Locomotion.FindAction("Duck", throwIfNotFound: true);
            m_Locomotion_Walk = m_Locomotion.FindAction("Walk", throwIfNotFound: true);
            m_Locomotion_Move = m_Locomotion.FindAction("Move", throwIfNotFound: true);
            m_Locomotion_Look = m_Locomotion.FindAction("Look", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Locomotion
        private readonly InputActionMap m_Locomotion;
        private ILocomotionActions m_LocomotionActionsCallbackInterface;
        private readonly InputAction m_Locomotion_Jump;
        private readonly InputAction m_Locomotion_Duck;
        private readonly InputAction m_Locomotion_Walk;
        private readonly InputAction m_Locomotion_Move;
        private readonly InputAction m_Locomotion_Look;
        public struct LocomotionActions
        {
            private @Player m_Wrapper;
            public LocomotionActions(@Player wrapper) { m_Wrapper = wrapper; }
            public InputAction @Jump => m_Wrapper.m_Locomotion_Jump;
            public InputAction @Duck => m_Wrapper.m_Locomotion_Duck;
            public InputAction @Walk => m_Wrapper.m_Locomotion_Walk;
            public InputAction @Move => m_Wrapper.m_Locomotion_Move;
            public InputAction @Look => m_Wrapper.m_Locomotion_Look;
            public InputActionMap Get() { return m_Wrapper.m_Locomotion; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(LocomotionActions set) { return set.Get(); }
            public void SetCallbacks(ILocomotionActions instance)
            {
                if (m_Wrapper.m_LocomotionActionsCallbackInterface != null)
                {
                    @Jump.started -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnJump;
                    @Jump.performed -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnJump;
                    @Jump.canceled -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnJump;
                    @Duck.started -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnDuck;
                    @Duck.performed -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnDuck;
                    @Duck.canceled -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnDuck;
                    @Walk.started -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnWalk;
                    @Walk.performed -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnWalk;
                    @Walk.canceled -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnWalk;
                    @Move.started -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnMove;
                    @Look.started -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnLook;
                    @Look.performed -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnLook;
                    @Look.canceled -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnLook;
                }
                m_Wrapper.m_LocomotionActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Jump.started += instance.OnJump;
                    @Jump.performed += instance.OnJump;
                    @Jump.canceled += instance.OnJump;
                    @Duck.started += instance.OnDuck;
                    @Duck.performed += instance.OnDuck;
                    @Duck.canceled += instance.OnDuck;
                    @Walk.started += instance.OnWalk;
                    @Walk.performed += instance.OnWalk;
                    @Walk.canceled += instance.OnWalk;
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @Look.started += instance.OnLook;
                    @Look.performed += instance.OnLook;
                    @Look.canceled += instance.OnLook;
                }
            }
        }
        public LocomotionActions @Locomotion => new LocomotionActions(this);
        private int m_ComputerSchemeIndex = -1;
        public InputControlScheme ComputerScheme
        {
            get
            {
                if (m_ComputerSchemeIndex == -1) m_ComputerSchemeIndex = asset.FindControlSchemeIndex("Computer");
                return asset.controlSchemes[m_ComputerSchemeIndex];
            }
        }
        private int m_GamepadSchemeIndex = -1;
        public InputControlScheme GamepadScheme
        {
            get
            {
                if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
                return asset.controlSchemes[m_GamepadSchemeIndex];
            }
        }
        public interface ILocomotionActions
        {
            void OnJump(InputAction.CallbackContext context);
            void OnDuck(InputAction.CallbackContext context);
            void OnWalk(InputAction.CallbackContext context);
            void OnMove(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
        }
    }
}
