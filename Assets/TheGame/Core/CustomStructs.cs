using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TripleKey
{
    public int KeyA;
    public int KeyB;
    public int KeyC;

    public TripleKey(int keyA, int keyB, int keyC)
    {
        KeyA = keyA;
        KeyB = keyB;
        KeyC = keyC;
    }

    public override bool Equals(object obj)
    {
        return obj is TripleKey code &&
               KeyA == code.KeyA &&
               KeyB == code.KeyB &&
               KeyC == code.KeyC;
    }

    public override int GetHashCode()
    {
        int hashCode = -1640531527;
        hashCode = hashCode * -1521134295 + KeyA.GetHashCode();
        hashCode = hashCode * -1521134295 + KeyB.GetHashCode();
        hashCode = hashCode * -1521134295 + KeyC.GetHashCode();
        return hashCode;
    }
}
