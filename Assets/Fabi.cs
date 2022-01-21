using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fabi : MonoBehaviour
{
    // Start is called before the first frame update
    int first = 0;
    int fabi;
    int second=1;
    int temp;
    List<int> allFabiNumbers;
    void Start()
    {
        allFabiNumbers = new List<int>();

        while (fabi < 100000)
        {
            fabi = first + second;
            first = second;
            second = fabi;
            allFabiNumbers.Add(fabi);
        }
        print(allFabiNumbers.Count);
        for (int i = 0; i < allFabiNumbers.Count; i++)
        {
            for (int j = 0; j < allFabiNumbers.Count; j++)
            {
                if (ValidatingNumber(allFabiNumbers[i]*allFabiNumbers[j]))
                {
                    print("("+allFabiNumbers[i]+","+ allFabiNumbers[j]+")");
                }
            }

        }
    }
     bool ValidatingNumber(int checkingNumber)
    {
        for (int i = 0; i < allFabiNumbers.Count; i++)
        {
            if (checkingNumber == allFabiNumbers[i])
            {
                return true;
            }
            else
            {
                continue;
            }
        }
        return false;
    }
}
