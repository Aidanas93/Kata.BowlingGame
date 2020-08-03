using System;
using System.Collections.Generic;
using System.Text;

namespace Kata.BowlingGame
{
    public enum StrikeType
    {
        Number,
        Spare,
        Strike
    }
    public static class BowlingScoreCalculator
    {
        
        public static ushort CalculateScore(string gamescore)
        {
            gamescore = gamescore.ToLower().Replace('-', '0');

            Node firstNode = null;
            Node prevNode = null;
            foreach (var ch in gamescore)
            {

                ushort numberValue;
                StrikeType type;
                if (UInt16.TryParse(ch.ToString(), out numberValue))
                {
                    type = StrikeType.Number;
                }
                else if (ch == '/')
                {
                    type = StrikeType.Spare;
                    numberValue = (ushort)(10 - prevNode.Value);
                }
                else
                {
                    type = StrikeType.Strike;
                    numberValue = 10;
                }

                Node node = new Node()
                {
                    Value = numberValue,
                    Type = type,
                    PrevNode = prevNode,
                };

                if (prevNode != null)
                {
                    prevNode.NextNode = node;
                }
                else
                {
                    firstNode = node;
                }

                prevNode = node;
            }

            ushort score = 0;
            ushort frameCount = 0;
            Node currentNode = firstNode;
            while (true)
            {
                frameCount++;
                if (currentNode.Type == StrikeType.Number)
                {
                    if (currentNode.NextNode.Type == StrikeType.Spare)
                    {
                        score += (ushort)(currentNode.Value + currentNode.NextNode.Value + currentNode.NextNode.NextNode.Value);

                        if (currentNode.NextNode.NextNode.NextNode == null)
                            break;
                    }
                    else
                    {
                        score += (ushort)(currentNode.Value + currentNode.NextNode.Value);

                        if (currentNode.NextNode.NextNode == null)
                            break;
                    }
                    currentNode = currentNode.NextNode.NextNode;
                }
                else
                {
                    score += (ushort)(currentNode.Value + currentNode.NextNode.Value + currentNode.NextNode.NextNode.Value);

                    if (currentNode.NextNode.NextNode.NextNode == null && frameCount != 9)
                        break;

                    currentNode = currentNode.NextNode;
                }
            }

            return score;
        }
    }
}
