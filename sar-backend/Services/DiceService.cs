using System;

public class DiceService
{
    private Random _random = new Random();

    public (int[] Rolls, int Total) RollDice(string diceType, int numberOfDice, int modifier)
    {
        if (numberOfDice < 1)
            throw new ArgumentException("Number of dice must be at least 1.");

        int sides = int.Parse(diceType[1..]);
        int[] rolls = new int[numberOfDice];

        for (int i = 0; i < numberOfDice; i++)
        {
            rolls[i] = _random.Next(1, sides + 1);
        }

        int total = rolls.Sum() + modifier;
        return (rolls, total);
    }
}
