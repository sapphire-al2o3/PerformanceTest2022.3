using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RemoveUnusedParticleModule : Editor
{
    static bool HasGradient(ParticleSystem.MinMaxGradient v)
    {
        return v.gradient != null || v.gradientMax != null || v.gradientMin != null;
    }

    static bool HasCurve(ParticleSystem.MinMaxCurve v)
    {
        return v.curve != null || v.curveMax != null || v.curveMin != null;
    }

    [MenuItem("Assets/Remove Unused Particle Module")]
    static void Run()
    {
        var go = Selection.activeObject as GameObject;
        if (go == null)
        {
            return;
        }

        var ps = go.GetComponentsInChildren<ParticleSystem>();
        foreach (var p in ps)
        {
            if (HasGradient(p.main.startColor))
            {
                Debug.Log("Start Color");
            }

            if (HasCurve(p.main.startSize))
            {
                Debug.Log("Start Size");
            }

            if (HasCurve(p.main.startSizeX))
            {
                Debug.Log("Start Size X");
            }

            if (!p.emission.enabled)
            {
                var m = p.emission;
                if (HasCurve(m.rateOverDistance) || HasCurve(m.rateOverTime))
                {
                    Debug.Log($"Emission {p.name}");
                    m.rateOverDistance = new ParticleSystem.MinMaxCurve();
                    m.rateOverTime = new ParticleSystem.MinMaxCurve();
                }
                if (m.burstCount > 0)
                {
                    Debug.Log($"Emission {p.name}");
                    m.burstCount = 0;
                }
            }

            if (!p.shape.enabled)
            {
                var m = p.shape;
                if (HasCurve(m.meshSpawnSpeed) || HasCurve(m.arcSpeed) || HasCurve(m.radiusSpeed) || m.texture != null)
                {
                    Debug.Log($"Shape {p.name}");
                    m.meshSpawnSpeed = new ParticleSystem.MinMaxCurve();
                    m.arcSpeed = new ParticleSystem.MinMaxCurve();
                    m.radiusSpeed = new ParticleSystem.MinMaxCurve();
                    m.texture = null;
                }
            }

            if (!p.velocityOverLifetime.enabled)
            {
                var m = p.velocityOverLifetime;
                if (m.x.curve != null || m.y.curve != null || m.z.curve != null)
                {
                    Debug.Log($"Velocity over Lifetime {p.name}");
                    m.x = new ParticleSystem.MinMaxCurve();
                    m.y = new ParticleSystem.MinMaxCurve();
                    m.z = new ParticleSystem.MinMaxCurve();
                }
            }

            if (!p.limitVelocityOverLifetime.enabled)
            {
                var m = p.limitVelocityOverLifetime;
                if (HasCurve(m.limit) || HasCurve(m.limitX) || HasCurve(m.limitY) || HasCurve(m.limitZ) || HasCurve(m.drag))
                {
                    Debug.Log($"Limit Velocity over Lifetime {p.name}");
                    m.limit = new ParticleSystem.MinMaxCurve();
                    m.limitX = new ParticleSystem.MinMaxCurve();
                    m.limitY = new ParticleSystem.MinMaxCurve();
                    m.limitZ = new ParticleSystem.MinMaxCurve();
                    m.drag = new ParticleSystem.MinMaxCurve();
                }
            }

            if (!p.inheritVelocity.enabled)
            {
                var m = p.inheritVelocity;
                if (HasCurve(m.curve))
                {
                    Debug.Log("Inherit Velocity");
                }
            }

            if (!p.sizeOverLifetime.enabled)
            {
                var m = p.sizeOverLifetime;
                if (HasCurve(m.size) || HasCurve(m.x) || HasCurve(m.y) || HasCurve(m.z))
                {
                    Debug.Log($"Size over Lifetime {p.name}");
                    m.size = new ParticleSystem.MinMaxCurve();
                    m.x = new ParticleSystem.MinMaxCurve();
                    m.y = new ParticleSystem.MinMaxCurve();
                    m.z = new ParticleSystem.MinMaxCurve();
                }
            }

            if (!p.sizeBySpeed.enabled)
            {
                var m = p.sizeBySpeed;
                if (HasCurve(m.size) || HasCurve(m.x) || HasCurve(m.y) || HasCurve(m.z))
                {
                    Debug.Log($"Size by Speed {p.name}");
                    m.size = new ParticleSystem.MinMaxCurve();
                    m.x = new ParticleSystem.MinMaxCurve();
                    m.y = new ParticleSystem.MinMaxCurve();
                    m.z = new ParticleSystem.MinMaxCurve();
                }
            }

            if (!p.rotationOverLifetime.enabled)
            {
                var m = p.rotationOverLifetime;
                if (HasCurve(m.x) || HasCurve(m.y) || HasCurve(m.z))
                {
                    Debug.Log($"Rotation over Lifetime {p.name}");
                    m.x = new ParticleSystem.MinMaxCurve();
                    m.y = new ParticleSystem.MinMaxCurve();
                    m.z = new ParticleSystem.MinMaxCurve();
                }
            }

            if (!p.rotationBySpeed.enabled)
            {
                var m = p.rotationBySpeed;
                if (HasCurve(m.x) || HasCurve(m.y) || HasCurve(m.z))
                {
                    Debug.Log($"Rotation by Speed {p.name}");
                    m.x = new ParticleSystem.MinMaxCurve();
                    m.y = new ParticleSystem.MinMaxCurve();
                    m.z = new ParticleSystem.MinMaxCurve();
                }
            }

            if (!p.forceOverLifetime.enabled)
            {
                var m = p.forceOverLifetime;
                if (HasCurve(m.x) || HasCurve(m.y) || HasCurve(m.z))
                {
                    Debug.Log($"Force over Lifetime {p.name}");
                    m.x = new ParticleSystem.MinMaxCurve();
                    m.y = new ParticleSystem.MinMaxCurve();
                    m.z = new ParticleSystem.MinMaxCurve();
                }
            }

            if (!p.colorOverLifetime.enabled)
            {
                var m = p.colorOverLifetime;
                if (HasGradient(m.color))
                {
                    Debug.Log($"Color over Lifetime {p.name}");
                    m.color = new ParticleSystem.MinMaxGradient();
                }
            }

            if (!p.colorBySpeed.enabled)
            {
                var m = p.colorBySpeed;
                if (HasGradient(m.color))
                {
                    Debug.Log($"Color by Speed {p.name}");
                    m.color = new ParticleSystem.MinMaxGradient();
                }
            }

            if (!p.textureSheetAnimation.enabled)
            {
                var m = p.textureSheetAnimation;
                if (HasCurve(m.frameOverTime) || HasCurve(m.startFrame))
                {
                    Debug.Log($"Texture Sheet Animation {p.name}");
                    m.frameOverTime = new ParticleSystem.MinMaxCurve();
                    m.startFrame = new ParticleSystem.MinMaxCurve();
                }
            }

            if (!p.trails.enabled)
            {
                var m = p.trails;
                if (HasGradient(m.colorOverTrail) || HasGradient(m.colorOverLifetime) || HasCurve(m.lifetime) || HasCurve(m.widthOverTrail))
                {
                    Debug.Log($"Trails {p.name}");
                    m.colorOverTrail = new ParticleSystem.MinMaxGradient();
                    m.colorOverLifetime = new ParticleSystem.MinMaxGradient();
                    m.lifetime = new ParticleSystem.MinMaxCurve();
                    m.widthOverTrail = new ParticleSystem.MinMaxCurve();
                }
            }

            if (!p.noise.enabled)
            {
                var m = p.noise;
                if (HasCurve(m.positionAmount)
                    || HasCurve(m.remap) || HasCurve(m.remapX) || HasCurve(m.remapY) || HasCurve(m.remapZ)
                    || HasCurve(m.scrollSpeed)
                    || HasCurve(m.strength) || HasCurve(m.strengthX) || HasCurve(m.strengthY) || HasCurve(m.strengthZ)
                    || HasCurve(m.rotationAmount) || HasCurve(m.sizeAmount))
                {
                    Debug.Log($"Noise {p.name}");
                    m.positionAmount = new ParticleSystem.MinMaxCurve();
                    m.remap = new ParticleSystem.MinMaxCurve();
                    m.remapX = new ParticleSystem.MinMaxCurve();
                    m.remapY = new ParticleSystem.MinMaxCurve();
                    m.remapZ = new ParticleSystem.MinMaxCurve();
                    m.scrollSpeed = new ParticleSystem.MinMaxCurve();
                    m.strength = new ParticleSystem.MinMaxCurve();
                    m.strengthX = new ParticleSystem.MinMaxCurve();
                    m.strengthY = new ParticleSystem.MinMaxCurve();
                    m.strengthZ = new ParticleSystem.MinMaxCurve();
                    m.rotationAmount = new ParticleSystem.MinMaxCurve();
                    m.sizeAmount = new ParticleSystem.MinMaxCurve();
                }
            }

            if (!p.lights.enabled)
            {
                var m = p.lights;
                if (HasCurve(m.range) || HasCurve(m.intensity))
                {
                    Debug.Log($"Lights {p.name}");
                    m.range = new ParticleSystem.MinMaxCurve();
                    m.intensity = new ParticleSystem.MinMaxCurve();
                }
            }

            if (!p.collision.enabled)
            {
                var m = p.collision;
                if (HasCurve(m.bounce) || HasCurve(m.dampen) || HasCurve(m.lifetimeLoss))
                {
                    Debug.Log("Dampen");
                }
            }
        }

        EditorUtility.SetDirty(go);
    }
}
