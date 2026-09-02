using System.Text;

namespace hw1;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите слово для переворачивания:");
        string word = Console.ReadLine();
        Console.WriteLine("Перевернутое слово:");
        Console.WriteLine(ReverseWord(word));
        
        Console.WriteLine("Введите предложение для переворачивания:");
        string sentence = Console.ReadLine();
        Console.WriteLine("Перевернутое предложение:");
        Console.WriteLine(ReverseSentence(sentence));
    }
    
    public static string ReverseWord(string word)
    {
        int length = word.Length;
        char[] reversedWord = new char[length];
        
        for (int i = 0; i < length; i++)
        {
            reversedWord[i] = word[length - 1 - i];
        }

        return new string(reversedWord);
    }

    public static string ReverseSentence(string sentence)
    {
        int start = 0;
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < sentence.Length; i++)
        {
            if (sentence[i] == ' ') 
            {
                result.Append($"{ReverseWord(sentence.Substring(start, i - start))} ");
                start = i + 1;
            }
        }
        
        result.Append(ReverseWord(sentence.Substring(start, sentence.Length - start)));
        
        return result.ToString();
    }
}
