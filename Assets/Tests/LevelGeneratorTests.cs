using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class LevelGeneratorTests
{
    [UnityTest]
    public IEnumerator LevelGenerator_SpawnsCorrectNumberOfCrates()
    {
        // 1. ARRANGE
        SceneManager.LoadScene(0);

        // Czekamy jedną klatkę, aż generator zbuduje planszę
        yield return null;

        // 2. ACT
        LevelGenerator generator = GameObject.FindAnyObjectByType<LevelGenerator>();
        Crate[] spawnedCrates = GameObject.FindObjectsByType<Crate>(FindObjectsSortMode.None);

        // 3. ASSERT
        Assert.IsNotNull(generator, "Nie znaleziono LevelGeneratora na załadowanej scenie!");

        
        Assert.AreEqual(generator.CratesToSpawn, spawnedCrates.Length, "Ilość wygenerowanych skrzynek na mapie nie zgadza się z limitem w LevelGeneratorze!");
    }
}