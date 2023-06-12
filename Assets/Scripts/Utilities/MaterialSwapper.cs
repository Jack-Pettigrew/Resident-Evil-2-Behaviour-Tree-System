using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
    public MeshRenderer[] meshRenderers;
    
    public void SwapMaterial(Material material)
    {
        if(meshRenderers.Length == 0) return;

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material = material;
        }
    }

    public void SwapMaterials(Material[] materials)
    {
        if(meshRenderers.Length == 0) return;

        List<Material> materialList = new List<Material>(materials);

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.SetSharedMaterials(materialList);
        }
    }

}
