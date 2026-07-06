namespace task01;

public static class StringExtensions
{
    public static bool IsPalindrome(this string input)
    {
        if (string.IsNullOrWhiteSpace(input)){
            return false;
        }

        List<char> list_of_chars = new List<char>(input); 
        for (int i = list_of_chars.Count - 1; i >= 0; i--){
            char c = list_of_chars[i];
    
    
            if (char.IsPunctuation(c) || char.IsWhiteSpace(c))
            {
                list_of_chars.RemoveAt(i);
            }
        }
        input = new string(list_of_chars.ToArray());
        return input.ToLower() == new string(input.ToLower().Reverse().ToArray());
    }
}