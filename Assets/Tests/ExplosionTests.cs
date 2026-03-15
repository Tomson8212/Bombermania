using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ExplosionTests
{
    [UnityTest]
    public IEnumerator Explosion_DestroysItself_AfterLifetime()
    {
        // 1. ARRANGE (Przygotowanie sceny)
        // Tworzymy pusty obiekt i przypinamy mu skrypt ognia
        GameObject explosionObject = new GameObject("TestExplosion");
        Explosion explosionScript = explosionObject.AddComponent<Explosion>();

        // 2. ACT (Akcja i upływ czasu)
        // Skrypt Explosion w swojej metodzie Start() odpala Destroy z opóźnieniem 0.5 sekundy.
        // Zamiast czekać jedną klatkę (yield return null), każemy testowi poczekać 0.6 sekundy,
        // żeby mieć 100% pewności, że czas minął.
        yield return new WaitForSeconds(0.6f);

        // 3. ASSERT (Sprawdzenie wyniku)
        // Po upływie tego czasu obiekt nie powinien już istnieć
        Assert.IsTrue(explosionObject == null, "Ogień z bomby powinien zniknąć automatycznie po upływie swojego 'lifetime'!");
    }
}