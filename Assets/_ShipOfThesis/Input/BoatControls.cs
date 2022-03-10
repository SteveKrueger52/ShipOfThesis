// GENERATED AUTOMATICALLY FROM 'Assets/_ShipOfThesis/Input/Boat.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @BoatControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @BoatControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Boat"",
    ""maps"": [
        {
            ""name"": ""Simple"",
            ""id"": ""7b783c66-aa70-4ed1-aa19-5c89d1caf90a"",
            ""actions"": [
                {
                    ""name"": ""ToggleHalyard"",
                    ""type"": ""Button"",
                    ""id"": ""9b54990d-397d-4349-b823-07b237c52f67"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GetSteering"",
                    ""type"": ""PassThrough"",
                    ""id"": ""bc70dece-6b24-44c1-b3aa-4dfd22550817"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""AxisDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GetCameraX"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e391bb19-d2ed-43e0-89c6-137fc7ef45a2"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": ""AxisDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GetCameraY"",
                    ""type"": ""PassThrough"",
                    ""id"": ""645a995f-7960-4c32-af85-3562487bb936"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": ""AxisDeadzone"",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6e2cf649-03b8-4d89-9ead-54e971890b7b"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleHalyard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6645387e-ee97-485a-9808-57732a7eed56"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseAndKey"",
                    ""action"": ""GetCameraX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a0f91f8-02d9-49fe-bf93-64281bbb4ac1"",
                    ""path"": ""<Gamepad>/rightStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""GetCameraX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""443a7b89-ec86-4b29-bbf0-e6461ba958d6"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GetSteering"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e185cb9-c1c0-4030-a281-b06148b8d7fd"",
                    ""path"": ""<Gamepad>/rightStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""GetCameraY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e499ecf7-53d6-4617-a088-bb6656848313"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseAndKey"",
                    ""action"": ""GetCameraY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Complex"",
            ""id"": ""3f448a00-905b-40b1-b3cd-90e39afe8123"",
            ""actions"": [
                {
                    ""name"": ""GetRudder"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2c8ef82b-dd62-4d84-895a-d6df17dcc54e"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": ""AxisDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GetHalyard"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7b7146bc-bd96-41dc-a7dd-851bd789c24f"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": ""AxisDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GetMainsheet"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2a0c6807-9531-4d56-b868-646c9e43d490"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": ""AxisDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GetCameraX"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2b11209d-4d9a-4eff-8d54-f387dc41f44a"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": ""AxisDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GetCameraY"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6d1eb201-3f83-464f-951f-0a6fec1c5389"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": ""AxisDeadzone"",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ece46030-f958-4c87-a26e-2a186e34baef"",
                    ""path"": ""<Gamepad>/dpad/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""GetRudder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b5ea341c-0cf3-4eb5-b014-b6a11f74d339"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""GetRudder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""cd9fb5f8-6f34-4a8b-9d5a-c19cd28ae499"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GetRudder"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""bb4e7d33-1f7f-4c42-ac15-8bca163f1f67"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseAndKey"",
                    ""action"": ""GetRudder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""92a695b7-ab83-4d3a-82cb-10d96ed8c886"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseAndKey"",
                    ""action"": ""GetRudder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""9d6b266f-3f72-42b0-8a75-13918a59bd7f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GetHalyard"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""0c786336-74a4-4db3-a832-e852a1dad009"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""GetHalyard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""2e63542c-234e-4116-aab5-23729e0553df"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""GetHalyard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""4701ae0d-fc37-4caf-bf7e-9f82fdbc7d3a"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GetHalyard"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""324df19b-fd68-4d1c-8a8b-e0d86ca03fd2"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseAndKey"",
                    ""action"": ""GetHalyard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""6241dd62-3470-4a88-8e39-aba135c8b75c"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseAndKey"",
                    ""action"": ""GetHalyard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""8681462e-b400-4123-acd1-bd877ca8c4f3"",
                    ""path"": ""<Keyboard>/leftMeta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseAndKey"",
                    ""action"": ""GetHalyard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""621cb114-ddd0-457c-be6f-526eed7d5e20"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GetMainsheet"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""f68c8fbb-f488-4c28-a4cf-16b42c47f9cd"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseAndKey"",
                    ""action"": ""GetMainsheet"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d77b803b-438d-4ffa-8026-9db1f38c3172"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseAndKey"",
                    ""action"": ""GetMainsheet"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""624bfe74-5a77-48c2-a9dd-0d8b5187ad67"",
                    ""path"": ""<Gamepad>/leftStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""GetMainsheet"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9eadc824-5315-4d50-a8b2-13beb8566bda"",
                    ""path"": ""<Gamepad>/dpad/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""GetMainsheet"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""85db18f2-0776-45cf-a407-8dadb9238b60"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseAndKey"",
                    ""action"": ""GetMainsheet"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""081e39c5-3909-4b8f-bcfc-ed5072d85be4"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseAndKey"",
                    ""action"": ""GetCameraX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7226990c-4876-480c-b65b-7f77a4aa522c"",
                    ""path"": ""<Gamepad>/rightStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""GetCameraX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0863718b-8d52-4cc1-91a4-e5a2b9d26fe5"",
                    ""path"": ""<Gamepad>/rightStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""GetCameraY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0dfe09e2-9c12-4248-8557-985b6b955c18"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseAndKey"",
                    ""action"": ""GetCameraY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""MouseAndKey"",
            ""bindingGroup"": ""MouseAndKey"",
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
        // Simple
        m_Simple = asset.FindActionMap("Simple", throwIfNotFound: true);
        m_Simple_ToggleHalyard = m_Simple.FindAction("ToggleHalyard", throwIfNotFound: true);
        m_Simple_GetSteering = m_Simple.FindAction("GetSteering", throwIfNotFound: true);
        m_Simple_GetCameraX = m_Simple.FindAction("GetCameraX", throwIfNotFound: true);
        m_Simple_GetCameraY = m_Simple.FindAction("GetCameraY", throwIfNotFound: true);
        // Complex
        m_Complex = asset.FindActionMap("Complex", throwIfNotFound: true);
        m_Complex_GetRudder = m_Complex.FindAction("GetRudder", throwIfNotFound: true);
        m_Complex_GetHalyard = m_Complex.FindAction("GetHalyard", throwIfNotFound: true);
        m_Complex_GetMainsheet = m_Complex.FindAction("GetMainsheet", throwIfNotFound: true);
        m_Complex_GetCameraX = m_Complex.FindAction("GetCameraX", throwIfNotFound: true);
        m_Complex_GetCameraY = m_Complex.FindAction("GetCameraY", throwIfNotFound: true);
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

    // Simple
    private readonly InputActionMap m_Simple;
    private ISimpleActions m_SimpleActionsCallbackInterface;
    private readonly InputAction m_Simple_ToggleHalyard;
    private readonly InputAction m_Simple_GetSteering;
    private readonly InputAction m_Simple_GetCameraX;
    private readonly InputAction m_Simple_GetCameraY;
    public struct SimpleActions
    {
        private @BoatControls m_Wrapper;
        public SimpleActions(@BoatControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ToggleHalyard => m_Wrapper.m_Simple_ToggleHalyard;
        public InputAction @GetSteering => m_Wrapper.m_Simple_GetSteering;
        public InputAction @GetCameraX => m_Wrapper.m_Simple_GetCameraX;
        public InputAction @GetCameraY => m_Wrapper.m_Simple_GetCameraY;
        public InputActionMap Get() { return m_Wrapper.m_Simple; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SimpleActions set) { return set.Get(); }
        public void SetCallbacks(ISimpleActions instance)
        {
            if (m_Wrapper.m_SimpleActionsCallbackInterface != null)
            {
                @ToggleHalyard.started -= m_Wrapper.m_SimpleActionsCallbackInterface.OnToggleHalyard;
                @ToggleHalyard.performed -= m_Wrapper.m_SimpleActionsCallbackInterface.OnToggleHalyard;
                @ToggleHalyard.canceled -= m_Wrapper.m_SimpleActionsCallbackInterface.OnToggleHalyard;
                @GetSteering.started -= m_Wrapper.m_SimpleActionsCallbackInterface.OnGetSteering;
                @GetSteering.performed -= m_Wrapper.m_SimpleActionsCallbackInterface.OnGetSteering;
                @GetSteering.canceled -= m_Wrapper.m_SimpleActionsCallbackInterface.OnGetSteering;
                @GetCameraX.started -= m_Wrapper.m_SimpleActionsCallbackInterface.OnGetCameraX;
                @GetCameraX.performed -= m_Wrapper.m_SimpleActionsCallbackInterface.OnGetCameraX;
                @GetCameraX.canceled -= m_Wrapper.m_SimpleActionsCallbackInterface.OnGetCameraX;
                @GetCameraY.started -= m_Wrapper.m_SimpleActionsCallbackInterface.OnGetCameraY;
                @GetCameraY.performed -= m_Wrapper.m_SimpleActionsCallbackInterface.OnGetCameraY;
                @GetCameraY.canceled -= m_Wrapper.m_SimpleActionsCallbackInterface.OnGetCameraY;
            }
            m_Wrapper.m_SimpleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ToggleHalyard.started += instance.OnToggleHalyard;
                @ToggleHalyard.performed += instance.OnToggleHalyard;
                @ToggleHalyard.canceled += instance.OnToggleHalyard;
                @GetSteering.started += instance.OnGetSteering;
                @GetSteering.performed += instance.OnGetSteering;
                @GetSteering.canceled += instance.OnGetSteering;
                @GetCameraX.started += instance.OnGetCameraX;
                @GetCameraX.performed += instance.OnGetCameraX;
                @GetCameraX.canceled += instance.OnGetCameraX;
                @GetCameraY.started += instance.OnGetCameraY;
                @GetCameraY.performed += instance.OnGetCameraY;
                @GetCameraY.canceled += instance.OnGetCameraY;
            }
        }
    }
    public SimpleActions @Simple => new SimpleActions(this);

    // Complex
    private readonly InputActionMap m_Complex;
    private IComplexActions m_ComplexActionsCallbackInterface;
    private readonly InputAction m_Complex_GetRudder;
    private readonly InputAction m_Complex_GetHalyard;
    private readonly InputAction m_Complex_GetMainsheet;
    private readonly InputAction m_Complex_GetCameraX;
    private readonly InputAction m_Complex_GetCameraY;
    public struct ComplexActions
    {
        private @BoatControls m_Wrapper;
        public ComplexActions(@BoatControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @GetRudder => m_Wrapper.m_Complex_GetRudder;
        public InputAction @GetHalyard => m_Wrapper.m_Complex_GetHalyard;
        public InputAction @GetMainsheet => m_Wrapper.m_Complex_GetMainsheet;
        public InputAction @GetCameraX => m_Wrapper.m_Complex_GetCameraX;
        public InputAction @GetCameraY => m_Wrapper.m_Complex_GetCameraY;
        public InputActionMap Get() { return m_Wrapper.m_Complex; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ComplexActions set) { return set.Get(); }
        public void SetCallbacks(IComplexActions instance)
        {
            if (m_Wrapper.m_ComplexActionsCallbackInterface != null)
            {
                @GetRudder.started -= m_Wrapper.m_ComplexActionsCallbackInterface.OnGetRudder;
                @GetRudder.performed -= m_Wrapper.m_ComplexActionsCallbackInterface.OnGetRudder;
                @GetRudder.canceled -= m_Wrapper.m_ComplexActionsCallbackInterface.OnGetRudder;
                @GetHalyard.started -= m_Wrapper.m_ComplexActionsCallbackInterface.OnGetHalyard;
                @GetHalyard.performed -= m_Wrapper.m_ComplexActionsCallbackInterface.OnGetHalyard;
                @GetHalyard.canceled -= m_Wrapper.m_ComplexActionsCallbackInterface.OnGetHalyard;
                @GetMainsheet.started -= m_Wrapper.m_ComplexActionsCallbackInterface.OnGetMainsheet;
                @GetMainsheet.performed -= m_Wrapper.m_ComplexActionsCallbackInterface.OnGetMainsheet;
                @GetMainsheet.canceled -= m_Wrapper.m_ComplexActionsCallbackInterface.OnGetMainsheet;
                @GetCameraX.started -= m_Wrapper.m_ComplexActionsCallbackInterface.OnGetCameraX;
                @GetCameraX.performed -= m_Wrapper.m_ComplexActionsCallbackInterface.OnGetCameraX;
                @GetCameraX.canceled -= m_Wrapper.m_ComplexActionsCallbackInterface.OnGetCameraX;
                @GetCameraY.started -= m_Wrapper.m_ComplexActionsCallbackInterface.OnGetCameraY;
                @GetCameraY.performed -= m_Wrapper.m_ComplexActionsCallbackInterface.OnGetCameraY;
                @GetCameraY.canceled -= m_Wrapper.m_ComplexActionsCallbackInterface.OnGetCameraY;
            }
            m_Wrapper.m_ComplexActionsCallbackInterface = instance;
            if (instance != null)
            {
                @GetRudder.started += instance.OnGetRudder;
                @GetRudder.performed += instance.OnGetRudder;
                @GetRudder.canceled += instance.OnGetRudder;
                @GetHalyard.started += instance.OnGetHalyard;
                @GetHalyard.performed += instance.OnGetHalyard;
                @GetHalyard.canceled += instance.OnGetHalyard;
                @GetMainsheet.started += instance.OnGetMainsheet;
                @GetMainsheet.performed += instance.OnGetMainsheet;
                @GetMainsheet.canceled += instance.OnGetMainsheet;
                @GetCameraX.started += instance.OnGetCameraX;
                @GetCameraX.performed += instance.OnGetCameraX;
                @GetCameraX.canceled += instance.OnGetCameraX;
                @GetCameraY.started += instance.OnGetCameraY;
                @GetCameraY.performed += instance.OnGetCameraY;
                @GetCameraY.canceled += instance.OnGetCameraY;
            }
        }
    }
    public ComplexActions @Complex => new ComplexActions(this);
    private int m_MouseAndKeySchemeIndex = -1;
    public InputControlScheme MouseAndKeyScheme
    {
        get
        {
            if (m_MouseAndKeySchemeIndex == -1) m_MouseAndKeySchemeIndex = asset.FindControlSchemeIndex("MouseAndKey");
            return asset.controlSchemes[m_MouseAndKeySchemeIndex];
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
    public interface ISimpleActions
    {
        void OnToggleHalyard(InputAction.CallbackContext context);
        void OnGetSteering(InputAction.CallbackContext context);
        void OnGetCameraX(InputAction.CallbackContext context);
        void OnGetCameraY(InputAction.CallbackContext context);
    }
    public interface IComplexActions
    {
        void OnGetRudder(InputAction.CallbackContext context);
        void OnGetHalyard(InputAction.CallbackContext context);
        void OnGetMainsheet(InputAction.CallbackContext context);
        void OnGetCameraX(InputAction.CallbackContext context);
        void OnGetCameraY(InputAction.CallbackContext context);
    }
}
