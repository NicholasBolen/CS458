// Adapted from Tvtig's implementation https://github.com/Tvtig/UnityLightsaber

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;

class Slicer
{
    // Slice the object given slicing plane
    public static GameObject[] Slice(Plane plane, GameObject objectToCut)
    {
        objectToCut.transform.parent = null;
        
        //Get mesh
        Mesh mesh = objectToCut.GetComponent<MeshFilter>().mesh;
        Sliceable sliceable = objectToCut.GetComponent<Sliceable>();

        if (sliceable == null)
        {
            throw new NotSupportedException("Missing sliceable script");
        }

        //Create left and right slice of hollow object
        SlicesMetadata slicesMeta = new SlicesMetadata(plane, mesh, sliceable.IsSolid, sliceable.ReverseWireTriangles, sliceable.ShareVertices, sliceable.SmoothVertices);

        GameObject positiveObject = CreateMeshGameObject(objectToCut);
        positiveObject.name = string.Format("{0}_1", objectToCut.name);

        GameObject negativeObject = CreateMeshGameObject(objectToCut);
        negativeObject.name = string.Format("{0}_2", objectToCut.name);

        var positiveSideMeshData = slicesMeta.PositiveSideMesh;
        var negativeSideMeshData = slicesMeta.NegativeSideMesh;

        positiveObject.GetComponent<MeshFilter>().mesh = positiveSideMeshData;
        negativeObject.GetComponent<MeshFilter>().mesh = negativeSideMeshData;

        SetupCollidersAndRigidBodys(ref positiveObject, positiveSideMeshData, sliceable.UseGravity);
        SetupCollidersAndRigidBodys(ref negativeObject, negativeSideMeshData, sliceable.UseGravity);

        return new GameObject[] { positiveObject, negativeObject };
    }

    /// Creates the default mesh game object.
    private static GameObject CreateMeshGameObject(GameObject originalObject)
    {
        var originalMaterial = originalObject.GetComponent<MeshRenderer>().materials;

        GameObject meshGameObject = new GameObject();
        Sliceable originalSliceable = originalObject.GetComponent<Sliceable>();

        meshGameObject.AddComponent<MeshFilter>();
        meshGameObject.AddComponent<MeshRenderer>();
        Sliceable sliceable = meshGameObject.AddComponent<Sliceable>();

        sliceable.IsSolid = originalSliceable.IsSolid;
        sliceable.ReverseWireTriangles = originalSliceable.ReverseWireTriangles;
        sliceable.UseGravity = originalSliceable.UseGravity;

        meshGameObject.GetComponent<MeshRenderer>().materials = originalMaterial;

        meshGameObject.transform.localScale = originalObject.transform.localScale;
        meshGameObject.transform.rotation = originalObject.transform.rotation;
        meshGameObject.transform.position = originalObject.transform.position;

        meshGameObject.tag = originalObject.tag;

        return meshGameObject;
    }

    /// Add mesh collider and rigid body to game object
    private static void SetupCollidersAndRigidBodys(ref GameObject gameObject, Mesh mesh, bool useGravity)
    {
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;

        var rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = useGravity;
    }
}