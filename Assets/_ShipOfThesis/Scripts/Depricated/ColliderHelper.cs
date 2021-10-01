using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ColliderHelper
{
    public static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float v321 = p3.x * p2.y * p1.z;
            float v231 = p2.x * p3.y * p1.z;
            float v312 = p3.x * p1.y * p2.z;
            float v132 = p1.x * p3.y * p2.z;
            float v213 = p2.x * p1.y * p3.z;
            float v123 = p1.x * p2.y * p3.z;
     
            return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
        }
        
    public static float VolumeOfMesh(Mesh mesh)
        {
            float volume = 0;
     
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
     
            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 p1 = vertices[triangles[i + 0]];
                Vector3 p2 = vertices[triangles[i + 1]];
                Vector3 p3 = vertices[triangles[i + 2]];
                volume += SignedVolumeOfTriangle(p1, p2, p3);
            }
            return Mathf.Abs(volume);
        }
    
    public static float GetDensity(Collider other)
        {
            if (other.attachedRigidbody != null)
                return other.attachedRigidbody.mass / GetVolume(other);
            return 1; // Assume same density as Water if no rigidbody attached
        }
        
    public static float GetVolume(Collider other)
        {
            float result = 1f;
            switch (other.GetType().ToString())
            {
                case "UnityEngine.BoxCollider":
                    Vector3 v3 = ((BoxCollider) other).size;
                    result = v3.x * v3.y * v3.z;
                    break;
                case "UnityEngine.CapsuleCollider":
                    float r = ((CapsuleCollider) other).radius;
                    float caps = (4f / 3f) * Mathf.PI * Mathf.Pow(r, 3f);
                    float middle = Mathf.PI * Mathf.Pow(r, 2f) * (((CapsuleCollider) other).height - (2 * r));
                    result = caps + (middle > 0f ? middle : 0f);
                    break;
                case "UnityEngine.SphereCollider":
                    result = (4f / 3f) * Mathf.PI * Mathf.Pow(((SphereCollider) other).radius, 3f);
                    break;
                case "UnityEngine.MeshCollider":
                    result = VolumeOfMesh(((MeshCollider) other).sharedMesh);
                    break;
                default:
                    throw new WarningException("Invalid Collider Type - Assuming Density of 1");
            }
            Vector3 scale = other.transform.lossyScale;
            return result * scale.x * scale.y * scale.z;
        }
    
    public static Bounds LocalBounds(Collider other)
    {
        Bounds result = new Bounds(Vector3.zero, Vector3.zero);
        float width;
        switch (other.GetType().ToString())
        {
            case "UnityEngine.BoxCollider":
                result = new Bounds(((BoxCollider) other).center, ((BoxCollider) other).size);
                break;
            case "UnityEngine.CapsuleCollider":
                width = ((CapsuleCollider) other).radius * 2;
                result = new Bounds(((CapsuleCollider) other).center,
                    new Vector3(width, ((CapsuleCollider) other).height, width));
                break;
            case "UnityEngine.SphereCollider":
                width = ((SphereCollider) other).radius * 2;
                result = new Bounds(((SphereCollider) other).center, new Vector3(width,width,width));
                break;
            case "UnityEngine.MeshCollider":
                result = ((MeshCollider) other).sharedMesh.bounds;
                break;
            default:
                throw new WarningException("Invalid Collider Type - Assuming Density of 1");
        }
        return result;
    }
}
