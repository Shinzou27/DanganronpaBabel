using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomIndexGenerator
{
    public static List<int> GenerateRandomIntList(List<int> currentAssignment, List<int> visited)
    {
        int listSize = currentAssignment.Count;
        List<int> newList = new(currentAssignment);
        if (!visited.Contains(5)) {
            visited.Add(5);
        }
        // Cria uma lista de índices que não estão em visited
        List<int> toShuffle = new List<int>();

        for (int i = 0; i < listSize; i++)
        {
            if (!visited.Contains(currentAssignment[i]))
            {
                toShuffle.Add(i);
            }
        }

        System.Random random = new();

        // Embaralha os índices usando o algoritmo de Fisher-Yates
        for (int i = toShuffle.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            (toShuffle[j], toShuffle[i]) = (toShuffle[i], toShuffle[j]);
        }

        // Cria uma lista temporária com os valores que não estão em visited
        List<int> valuesToShuffle = new();

        foreach (int index in toShuffle)
        {
            valuesToShuffle.Add(currentAssignment[index]);
        }

        // Embaralha os valores que não estão em visited
        for (int i = valuesToShuffle.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            (valuesToShuffle[j], valuesToShuffle[i]) = (valuesToShuffle[i], valuesToShuffle[j]);
        }

        // Atribui os valores embaralhados de volta aos seus índices originais
        for (int i = 0; i < toShuffle.Count; i++)
        {
            newList[toShuffle[i]] = valuesToShuffle[i];
        }
        return newList;
    }
    public static List<int> GenerateRandomIntList(int listSize)
    {
        List<int> randomInts = new List<int>();

        // Inicializa a lista com todos os valores possíveis
        for (int i = 0; i < listSize; i++)
        {
            randomInts.Add(i);
        }

        System.Random random = new();

        // Embaralha a lista usando o algoritmo de Fisher-Yates
        for (int i = randomInts.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            (randomInts[j], randomInts[i]) = (randomInts[i], randomInts[j]);
        }

        return randomInts;
    }
    private static string PrintedList(List<int> list) {
        string toPrint = "";
        foreach (int i in list) {
            toPrint += i + " ";
        }
        return toPrint;
    }
}
