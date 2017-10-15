﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DollDanceSequence : MonoBehaviour
{

    public enum Move { Idle, Up, Down, Left, Right }

    [Header("Debug fields")]
    [SerializeField]
    bool enableOverrideMoves;
    [SerializeField]
    Move[] overrideMoves;

    [Header("Number of sequential dance moves")]
    [SerializeField]
    int moveCount;

    List<Move> validMoves;
    Stack<Move> sequence;

    void Awake()
    {
        this.sequence = new Stack<Move>();

        this.validMoves = Enum.GetValues(typeof(Move)).Cast<Move>().ToList();
        this.validMoves.Remove(Move.Idle);

        if (enableOverrideMoves)
            this.moveCount = overrideMoves.Length;

        ResetSlots(this.moveCount);
    }

    void ResetSlots(int moveCount)
    {
        // Clear slots
        this.moveCount = moveCount;
        sequence.Clear();

        // Fill all of the sequence slots randomly
        System.Random random = new System.Random();
        Move previousMove = Move.Idle;
        for (int i = 0; i < this.moveCount; i++)
        {
            List<Move> available = new List<Move>(this.validMoves);
            available.Remove(previousMove);

            Move newMove;
            if (enableOverrideMoves)
                newMove = overrideMoves[this.moveCount - 1 - i];
            else
                newMove = available[random.Next(available.Count)];
            sequence.Push(newMove);
            previousMove = newMove;
        }
    }
    
    public List<Move> CopySequence()
    {
        return new List<Move>(this.sequence);
    }

    public Move Process(Move move)
    {
        if (move == sequence.Peek())
            return sequence.Pop();
        else
            return Move.Idle;
    }
    
    public bool IsComplete()
    {
        return sequence.Count == 0;
    }

}
