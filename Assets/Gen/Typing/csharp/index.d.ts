﻿
declare module 'csharp' {
    interface $Ref<T> {
        value: T
    }
    
    namespace System {
        interface Array$1<T> extends System.Array {
            get_Item(index: number):T;
            
            set_Item(index: number, value: T):void;
        }
    }
    
    interface $Task<T> {}
    
    namespace UnityEngine {
        /** Base class for all entities in Unity Scenes. */
        class GameObject extends UnityEngine.Object {
            /** The Transform attached to this GameObject. */
            public get transform(): UnityEngine.Transform;
            /** The layer the game object is in. */
            public get layer(): number;
            public set layer(value: number);
            /** The local active state of this GameObject. (Read Only) */
            public get activeSelf(): boolean;
            /** Defines whether the GameObject is active in the Scene. */
            public get activeInHierarchy(): boolean;
            /** Editor only API that specifies if a game object is static. */
            public get isStatic(): boolean;
            public set isStatic(value: boolean);
            /** The tag of this game object. */
            public get tag(): string;
            public set tag(value: string);
            /** Scene that the GameObject is part of. */
            public get scene(): UnityEngine.SceneManagement.Scene;
            /** Scene culling mask Unity uses to determine which scene to render the GameObject in. */
            public get sceneCullingMask(): bigint;
            
            public get gameObject(): UnityEngine.GameObject;
            
            public constructor($name: string);
            
            public constructor();
            
            public constructor($name: string, ...components: System.Type[]);
            /** Creates a game object with a primitive mesh renderer and appropriate collider. * @param type The type of primitive object to create.
             */
            public static CreatePrimitive($type: UnityEngine.PrimitiveType):UnityEngine.GameObject;
            /** Returns the component of Type type if the game object has one attached, null if it doesn't. * @param type The type of Component to retrieve.
             */
            public GetComponent($type: System.Type):UnityEngine.Component;
            /** Returns the component with name type if the game object has one attached, null if it doesn't. * @param type The type of Component to retrieve.
             */
            public GetComponent($type: string):UnityEngine.Component;
            /** Returns the component of Type type in the GameObject or any of its children using depth first search.
             * @param type The type of Component to retrieve.
             * @returns A component of the matching type, if found. 
             */
            public GetComponentInChildren($type: System.Type, $includeInactive: boolean):UnityEngine.Component;
            /** Returns the component of Type type in the GameObject or any of its children using depth first search.
             * @param type The type of Component to retrieve.
             * @returns A component of the matching type, if found. 
             */
            public GetComponentInChildren($type: System.Type):UnityEngine.Component;
            /** Retrieves the component of Type type in the GameObject or any of its parents.
             * @param type Type of component to find.
             * @returns Returns a component if a component matching the type is found. Returns null otherwise. 
             */
            public GetComponentInParent($type: System.Type, $includeInactive: boolean):UnityEngine.Component;
            /** Retrieves the component of Type type in the GameObject or any of its parents.
             * @param type Type of component to find.
             * @returns Returns a component if a component matching the type is found. Returns null otherwise. 
             */
            public GetComponentInParent($type: System.Type):UnityEngine.Component;
            /** Returns all components of Type type in the GameObject. * @param type The type of component to retrieve.
             */
            public GetComponents($type: System.Type):System.Array$1<UnityEngine.Component>;
            
            public GetComponents($type: System.Type, $results: System.Collections.Generic.List$1<UnityEngine.Component>):void;
            /** Returns all components of Type type in the GameObject or any of its children. * @param type The type of Component to retrieve.
             * @param includeInactive Should Components on inactive GameObjects be included in the found set?
             */
            public GetComponentsInChildren($type: System.Type):System.Array$1<UnityEngine.Component>;
            /** Returns all components of Type type in the GameObject or any of its children. * @param type The type of Component to retrieve.
             * @param includeInactive Should Components on inactive GameObjects be included in the found set?
             */
            public GetComponentsInChildren($type: System.Type, $includeInactive: boolean):System.Array$1<UnityEngine.Component>;
            
            public GetComponentsInParent($type: System.Type):System.Array$1<UnityEngine.Component>;
            /** Returns all components of Type type in the GameObject or any of its parents. * @param type The type of Component to retrieve.
             * @param includeInactive Should inactive Components be included in the found set?
             */
            public GetComponentsInParent($type: System.Type, $includeInactive: boolean):System.Array$1<UnityEngine.Component>;
            /** Gets the component of the specified type, if it exists.
             * @param type The type of component to retrieve.
             * @param component The output argument that will contain the component or null.
             * @returns Returns true if the component is found, false otherwise. 
             */
            public TryGetComponent($type: System.Type, $component: $Ref<UnityEngine.Component>):boolean;
            /** Returns one active GameObject tagged tag. Returns null if no GameObject was found. * @param tag The tag to search for.
             */
            public static FindWithTag($tag: string):UnityEngine.GameObject;
            
            public SendMessageUpwards($methodName: string, $options: UnityEngine.SendMessageOptions):void;
            
            public SendMessage($methodName: string, $options: UnityEngine.SendMessageOptions):void;
            
            public BroadcastMessage($methodName: string, $options: UnityEngine.SendMessageOptions):void;
            /** Adds a component class of type componentType to the game object. C# Users can use a generic version. */
            public AddComponent($componentType: System.Type):UnityEngine.Component;
            /** ActivatesDeactivates the GameObject, depending on the given true or false/ value. * @param value Activate or deactivate the object, where true activates the GameObject and false deactivates the GameObject.
             */
            public SetActive($value: boolean):void;
            /** Is this game object tagged with tag ? * @param tag The tag to compare.
             */
            public CompareTag($tag: string):boolean;
            
            public static FindGameObjectWithTag($tag: string):UnityEngine.GameObject;
            /** Returns an array of active GameObjects tagged tag. Returns empty array if no GameObject was found. * @param tag The name of the tag to search GameObjects for.
             */
            public static FindGameObjectsWithTag($tag: string):System.Array$1<UnityEngine.GameObject>;
            /** Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour. * @param methodName The name of the method to call.
             * @param value An optional parameter value to pass to the called method.
             * @param options Should an error be raised if the method doesn't exist on the target object?
             */
            public SendMessageUpwards($methodName: string, $value: any, $options: UnityEngine.SendMessageOptions):void;
            /** Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour. * @param methodName The name of the method to call.
             * @param value An optional parameter value to pass to the called method.
             * @param options Should an error be raised if the method doesn't exist on the target object?
             */
            public SendMessageUpwards($methodName: string, $value: any):void;
            /** Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour. * @param methodName The name of the method to call.
             * @param value An optional parameter value to pass to the called method.
             * @param options Should an error be raised if the method doesn't exist on the target object?
             */
            public SendMessageUpwards($methodName: string):void;
            /** Calls the method named methodName on every MonoBehaviour in this game object. * @param methodName The name of the method to call.
             * @param value An optional parameter value to pass to the called method.
             * @param options Should an error be raised if the method doesn't exist on the target object?
             */
            public SendMessage($methodName: string, $value: any, $options: UnityEngine.SendMessageOptions):void;
            /** Calls the method named methodName on every MonoBehaviour in this game object. * @param methodName The name of the method to call.
             * @param value An optional parameter value to pass to the called method.
             * @param options Should an error be raised if the method doesn't exist on the target object?
             */
            public SendMessage($methodName: string, $value: any):void;
            /** Calls the method named methodName on every MonoBehaviour in this game object. * @param methodName The name of the method to call.
             * @param value An optional parameter value to pass to the called method.
             * @param options Should an error be raised if the method doesn't exist on the target object?
             */
            public SendMessage($methodName: string):void;
            /** Calls the method named methodName on every MonoBehaviour in this game object or any of its children. */
            public BroadcastMessage($methodName: string, $parameter: any, $options: UnityEngine.SendMessageOptions):void;
            /** Calls the method named methodName on every MonoBehaviour in this game object or any of its children. */
            public BroadcastMessage($methodName: string, $parameter: any):void;
            /** Calls the method named methodName on every MonoBehaviour in this game object or any of its children. */
            public BroadcastMessage($methodName: string):void;
            /** Finds a GameObject by name and returns it. */
            public static Find($name: string):UnityEngine.GameObject;
            
        }
        /** Base class for all objects Unity can reference. */
        class Object extends System.Object {
            
        }
        /** The various primitives that can be created using the GameObject.CreatePrimitive function. */
        enum PrimitiveType { Sphere = 0, Capsule = 1, Cylinder = 2, Cube = 3, Plane = 4, Quad = 5 }
        /** Base class for everything attached to GameObjects. */
        class Component extends UnityEngine.Object {
            
        }
        /** Options for how to send a message. */
        enum SendMessageOptions { RequireReceiver = 0, DontRequireReceiver = 1 }
        /** Position, rotation and scale of an object. */
        class Transform extends UnityEngine.Component {
            
        }
        /** MonoBehaviour is the base class from which every Unity script derives. */
        class MonoBehaviour extends UnityEngine.Behaviour {
            
        }
        /** Behaviours are Components that can be enabled or disabled. */
        class Behaviour extends UnityEngine.Component {
            
        }
        /** Renders a Sprite for 2D graphics. */
        class SpriteRenderer extends UnityEngine.Renderer {
            
        }
        /** General functionality for all renderers. */
        class Renderer extends UnityEngine.Component {
            
        }
        /** Represents a Sprite object for use in 2D gameplay. */
        class Sprite extends UnityEngine.Object {
            
        }
        /** Representation of 3D vectors and points. */
        class Vector3 extends System.ValueType {
            
        }
        
    }
    namespace System {
        
        class Object {
            
        }
        
        class Enum extends System.ValueType {
            
        }
        
        class ValueType extends System.Object {
            
        }
        
        class Type extends System.Reflection.MemberInfo {
            
        }
        
        class String extends System.Object {
            
        }
        
        class Boolean extends System.ValueType {
            
        }
        
        class Array extends System.Object {
            
        }
        
        class Void extends System.ValueType {
            
        }
        
        class Int32 extends System.ValueType {
            
        }
        
        class UInt64 extends System.ValueType {
            
        }
        
        class Single extends System.ValueType {
            
        }
        
        type MulticastDelegate = (...args:any[]) => any;
        var MulticastDelegate: {new (func: (...args:any[]) => any): MulticastDelegate;}
        
        class Delegate extends System.Object {
            
        }
        
    }
    namespace System.Reflection {
        
        class MemberInfo extends System.Object {
            
        }
        
    }
    namespace System.Collections.Generic {
        
        class List$1<T> extends System.Object {
            
        }
        
    }
    namespace UnityEngine.SceneManagement {
        /** Run-time data structure for *.unity file. */
        class Scene extends System.ValueType {
            
        }
        
    }
    
        
        class GameElementImageView extends UnityEngine.MonoBehaviour {
            
            public spriteRenderer: UnityEngine.SpriteRenderer;
            
            public constructor();
            
        }
        
        class GameElementView extends UnityEngine.MonoBehaviour {
            
            public elementType: EGame.Core.GameElementType;
            
            public get x(): number;
            
            public get y(): number;
            
            public get gameController(): EliminateGameController;
            public set gameController(value: EliminateGameController);
            
            public get imageId(): number;
            public set imageId(value: number);
            
            public constructor();
            
            public Init($x: number, $y: number):void;
            
            public MoveElement($targetX: number, $targetY: number, $time: number, $callback: EGame.Core.IElementMoveEndCallback):void;
            
            public CreateImageView($x: number, $y: number):void;
            
            public UpdateImageView($imageId: number, $sprite: UnityEngine.Sprite):void;
            
            public DestroyView():void;
            
            public DestroyImageView():void;
            
        }
        
        class EliminateGameController extends UnityEngine.MonoBehaviour {
            
            public elementContainer: UnityEngine.GameObject;
            
            public gridContainer: UnityEngine.GameObject;
            
            public elementPrefabs: System.Array$1<GameElementView>;
            
            public elementImageViews: System.Array$1<ElementImageView>;
            
            public constructor();
            
            public CreateGameElementView($x: number, $y: number, $elementType: EGame.Core.GameElementType):EGame.Core.IGameElementView;
            
            public PressedElement($elementView: EGame.Core.IGameElementView):void;
            
            public EnterElement($elementView: EGame.Core.IGameElementView):void;
            
            public ReleaseElement($elementView: EGame.Core.IGameElementView):void;
            
            public LogInfo($info: string):void;
            
            public LogWarn($warn: string):void;
            
            public LogErrr($error: string):void;
            
            public CreateGameElementImageView($x: number, $y: number, $elementView: GameElementView):void;
            
            public ConvertElementToUnityPos($x: number, $y: number):UnityEngine.Vector3;
            
        }
        
        class ElementImageView extends System.ValueType {
            
        }
        
    
    namespace EGame.Core {
        
        enum GameElementType { Null = -2, Grid = -1, Empty = 0, Normal = 1, Barrier = 2, Any = 3, Same = 4 }
        
        type IElementMoveEndCallback = (target: EGame.Core.IGameElementView) => void;
        var IElementMoveEndCallback: {new (func: (target: EGame.Core.IGameElementView) => void): IElementMoveEndCallback;}
        
        interface IGameElementView {
            
        }
        
    }
    
}