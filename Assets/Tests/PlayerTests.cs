using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTests
{
    // [UnityTest] pozwala nam testować rzeczy, które wymagają czasu (np. czekanie na klatkę)
    [UnityTest]
    public IEnumerator PlayerDies_WhenDieMethodIsCalled_ObjectIsDestroyed()
    {
        // 1. ARRANGE (Przygotowanie sceny)
        // Tworzymy pusty obiekt i udajemy, że to nasz gracz
        GameObject playerObject = new GameObject("TestPlayer");
        PlayerMovement playerMovement = playerObject.AddComponent<PlayerMovement>();

        // 2. ACT (Wykonanie akcji)
        // Wywołujemy funkcję śmierci
        playerMovement.Die();

        // UWAGA: Funkcja Destroy() w Unity nie działa natychmiast. 
        // Obiekt znika dopiero na samym końcu obecnej klatki.
        // Dlatego musimy poczekać jedną klatkę używając yield return null.
        yield return null;

        // 3. ASSERT (Sprawdzenie wyniku)
        // Oczekujemy, że obiekt gracza będzie równy null (czyli został zniszczony)
        Assert.IsTrue(playerObject == null, "Obiekt gracza powinien zostać zniszczony po wywołaniu Die()");
    }
}